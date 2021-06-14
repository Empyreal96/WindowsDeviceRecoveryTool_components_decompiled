using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.Controllers
{
	// Token: 0x02000033 RID: 51
	[Export("Microsoft.WindowsDeviceRecoveryTool.Controllers.FlowController", typeof(IController))]
	public class FlowController : BaseController
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000B50C File Offset: 0x0000970C
		[ImportingConstructor]
		public FlowController(ICommandRepository commandRepository, Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext, LogicContext logics, EventAggregator eventAggregator, ReportingService reportingService) : base(commandRepository, eventAggregator)
		{
			this.appContext = appContext;
			this.logics = logics;
			this.reportingService = reportingService;
			this.logics.AdaptationManager.DeviceInfoRead += this.AdaptationManagerDeviceInfoRead;
			this.logics.AdaptationManager.ProgressChanged += this.AdaptationManagerProgressChanged;
			this.logics.AdaptationManager.DeviceBatteryLevelRead += this.AdaptationManagerDeviceBatteryLevelRead;
			this.logics.AdaptationManager.DeviceBatteryStatusRead += this.AdaptationManagerDeviceBatteryStatusRead;
			this.logics.AdaptationManager.DeviceConnectionStatusRead += this.AdaptationManagerDeviceConnectionStatusRead;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B5E4 File Offset: 0x000097E4
		[CustomCommand]
		public void ChangePackageDirectoryCommand()
		{
			string result = DialogManager.Instance.OpenDirectoryDialog("c:\\");
			if (!string.IsNullOrEmpty(result))
			{
				base.EventAggregator.Publish<PackageDirectoryMessage>(new PackageDirectoryMessage(result));
				base.Commands.Run((FlowController c) => c.FindCorrectPackage(result, CancellationToken.None));
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B6BC File Offset: 0x000098BC
		[CustomCommand]
		public void ChangePackagePathCommand()
		{
			string adaptationExtension = this.logics.AdaptationManager.GetAdaptationExtension(this.appContext.CurrentPhone.Type);
			string text = DialogManager.Instance.OpenFileDialog(adaptationExtension, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultFfuPath);
			if (!string.IsNullOrEmpty(text))
			{
				base.EventAggregator.Publish<PackageDirectoryMessage>(new PackageDirectoryMessage(text));
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B71C File Offset: 0x0000991C
		[CustomCommand]
		public void CancelSearchingPackageAndSwitchToManual()
		{
			((IAsyncDelegateCommand)base.Commands["FindCorrectPackage"]).Cancel();
			base.Commands.Run((AppController c) => c.SwitchToState("ManualPackageSelectionState"));
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B7AC File Offset: 0x000099AC
		[CustomCommand(IsAsynchronous = true)]
		public void FindCorrectPackage(string directory, CancellationToken token)
		{
			List<PackageFileInfo> packages = this.logics.AdaptationManager.FindCorrectPackage(directory, this.appContext.CurrentPhone, token);
			base.EventAggregator.Publish<CompatibleFfuFilesMessage>(new CompatibleFfuFilesMessage(packages));
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B7EC File Offset: 0x000099EC
		[CustomCommand(IsAsynchronous = true)]
		public void FindAllLumiaPackages(string directory, CancellationToken cancellationToken)
		{
			List<PackageFileInfo> list = this.logics.AdaptationManager.FindAllPackages(directory, PhoneTypes.Lumia, cancellationToken);
			Tracer<FlowController>.WriteInformation("Found packages: {0}", new object[]
			{
				list.Count
			});
			base.EventAggregator.Publish<CompatibleFfuFilesMessage>(new CompatibleFfuFilesMessage(list));
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B841 File Offset: 0x00009A41
		[CustomCommand]
		public void StartAwaitRecoveryState()
		{
			base.EventAggregator.Publish<DetectionTypeMessage>(new DetectionTypeMessage(DetectionType.RecoveryMode));
			base.EventAggregator.Publish<SwitchStateMessage>(new SwitchStateMessage("AwaitRecoveryDeviceState"));
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B86C File Offset: 0x00009A6C
		[CustomCommand(IsAsynchronous = true)]
		public void CancelAwaitRecoveryAfterEmergency(bool cancelled, CancellationToken token)
		{
			this.reportingService.OperationFailed(new Phone(), ReportOperationType.EmergencyFlashing, UriData.AwaitAfterEmergencyFlashingCanceled, new OperationCanceledException("User canceled waiting for device after succesfull emergency flashing operation"));
			List<string> extendedMessage = new List<string>
			{
				"Error_OperationCanceledException"
			};
			base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, extendedMessage, null));
			base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B92C File Offset: 0x00009B2C
		[CustomCommand(IsAsynchronous = true)]
		public void FinishAwaitRecoveryAfterEmergency(bool cancelled, CancellationToken token)
		{
			Phone currentPhone = this.appContext.CurrentPhone;
			if (currentPhone.IsDeviceInEmergencyMode())
			{
				this.reportingService.OperationFailed(currentPhone, ReportOperationType.EmergencyFlashing, UriData.EmergencyModeAfterEmergencyFlashing, new Exception("Emergency mode appeared after succesfull emergency flashing operation"));
				List<string> extendedMessage = new List<string>
				{
					"Error_DeviceNotFoundException"
				};
				base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, extendedMessage, null));
				base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
			}
			else
			{
				try
				{
					this.logics.LumiaAdaptation.StopDetection();
					this.logics.Thor2Service.TryReadMissingInfoWithThor(currentPhone, CancellationToken.None, true);
					this.reportingService.PartialOperationSucceded(currentPhone, ReportOperationType.EmergencyFlashing, UriData.UefiModeAfterEmergencyFlashing);
				}
				catch (Exception ex)
				{
					this.reportingService.OperationFailed(currentPhone, ReportOperationType.ReadInfoAfterEmergencyFlashing, UriData.ReadingDeviceInfoAfterEmergencyFlashingFailed, ex);
				}
				base.EventAggregator.Publish<DetectionTypeMessage>(new DetectionTypeMessage(DetectionType.RecoveryModeAfterEmergencyFlashing));
				base.EventAggregator.Publish<SwitchStateMessage>(new SwitchStateMessage("AwaitRecoveryDeviceState"));
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		[CustomCommand(IsAsynchronous = true)]
		public void EmergencyFlashDevice(object parameter, CancellationToken token)
		{
			bool flag = false;
			List<string> extendedMessage = new List<string>();
			string argument = string.Empty;
			try
			{
				this.logics.AdaptationManager.EmergencyFlashDevice(this.appContext.CurrentPhone, token);
				flag = true;
			}
			catch (NoDeviceException)
			{
				extendedMessage = new List<string>
				{
					"Error_DeviceNotFoundException"
				};
			}
			catch (FileNotFoundException ex)
			{
				extendedMessage = new List<string>
				{
					"Error_FileNotFoundException"
				};
				argument = ex.Message;
			}
			catch (SoftwareIsNotCorrectlySignedException ex2)
			{
				extendedMessage = new List<string>
				{
					"Error_SoftwareIsNotCorrectlySignedException"
				};
				argument = ex2.Message;
			}
			catch (Exception arg)
			{
				extendedMessage = new List<string>
				{
					"Error_SoftwareInstallationFailed",
					"ButtonMyPhoneWasNotDetected"
				};
				Tracer<FlowController>.WriteInformation("Flashing failed:\n" + arg);
			}
			finally
			{
				if (flag)
				{
					base.Commands.Run((AppController c) => c.SwitchToState("AwaitRecoveryModeAfterEmergencyFlashingState"));
				}
				else
				{
					base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, extendedMessage, argument));
					base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
				}
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000BD18 File Offset: 0x00009F18
		[CustomCommand(IsAsynchronous = true)]
		public void FlashDevice(DetectionType detectionType, CancellationToken token)
		{
			bool result = false;
			List<string> extendedMessage = new List<string>();
			string argument = string.Empty;
			if (this.appContext.CurrentPhone == null || !this.logics.AdaptationManager.IsDeviceInFlashModeConnected(this.appContext.CurrentPhone, token))
			{
				throw new DeviceNotFoundException();
			}
			try
			{
				this.logics.AdaptationManager.FlashDevice(this.appContext.CurrentPhone, detectionType, token);
				result = true;
			}
			catch (NoDeviceException)
			{
				extendedMessage = new List<string>
				{
					"Error_DeviceNotFoundException"
				};
			}
			catch (DeviceDisconnectedException)
			{
				extendedMessage = new List<string>
				{
					"Error_DeviceDisconnectedException"
				};
			}
			catch (FileNotFoundException ex)
			{
				extendedMessage = new List<string>
				{
					"Error_FileNotFoundException"
				};
				argument = ex.Message;
			}
			catch (SoftwareIsNotCorrectlySignedException ex2)
			{
				extendedMessage = new List<string>
				{
					"Error_SoftwareIsNotCorrectlySignedException"
				};
				argument = ex2.Message;
			}
			catch (Exception error)
			{
				extendedMessage = new List<string>
				{
					(this.appContext != null && this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory) ? "Error_SoftwareInstallationFailed_ReconnectUSB" : "Error_SoftwareInstallationFailed",
					"ButtonMyPhoneWasNotDetected"
				};
				Tracer<FlowController>.WriteInformation("Flashing failed!");
				Tracer<FlowController>.WriteError(error);
			}
			finally
			{
				base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(result, extendedMessage, argument));
				base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000BFAC File Offset: 0x0000A1AC
		[CustomCommand(IsAsynchronous = true)]
		public void CheckPackageIntegrity(object parameter, CancellationToken token)
		{
			if (this.appContext.CurrentPhone == null)
			{
				throw new DeviceNotFoundException();
			}
			bool result = false;
			try
			{
				this.logics.AdaptationManager.CheckPackageIntegrity(this.appContext.CurrentPhone, token);
				result = true;
			}
			finally
			{
				base.EventAggregator.Publish<FfuIntegrityCheckMessage>(new FfuIntegrityCheckMessage(result));
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C024 File Offset: 0x0000A224
		private void AdaptationManagerProgressChanged(ProgressChangedEventArgs progressChangedEventArgs)
		{
			base.EventAggregator.Publish<ProgressMessage>(new ProgressMessage(progressChangedEventArgs.Percentage, progressChangedEventArgs.Message, progressChangedEventArgs.DownloadedSize, progressChangedEventArgs.TotalSize, progressChangedEventArgs.BytesPerSecond, progressChangedEventArgs.SecondsLeft));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C05C File Offset: 0x0000A25C
		[CustomCommand(IsAsynchronous = true)]
		public void CheckLatestPackage(object parameter, CancellationToken cancellationToken)
		{
			bool status = false;
			PackageFileInfo packageFileInfo = null;
			if (this.appContext.CurrentPhone == null)
			{
				throw new DeviceNotFoundException();
			}
			if (Settings.Default.CustomPackagesPathEnabled && !Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckDirectoryWritePermission(Settings.Default.PackagesPath))
			{
				throw new CannotAccessDirectoryException(Settings.Default.PackagesPath);
			}
			try
			{
				packageFileInfo = this.logics.AdaptationManager.CheckLatestPackage(this.appContext.CurrentPhone, cancellationToken);
				status = (packageFileInfo != null);
				if (packageFileInfo == null)
				{
					packageFileInfo = new MsrPackageInfo(this.notFoundText, this.notFoundText, this.notFoundText);
				}
			}
			catch (PackageNotFoundException error)
			{
				packageFileInfo = new MsrPackageInfo(this.notFoundText, this.notFoundText, this.notFoundText);
				status = false;
				Tracer<FlowController>.WriteError(error);
			}
			catch (WebException)
			{
				if (!NetworkInterface.GetIsNetworkAvailable())
				{
					throw new NoInternetConnectionException();
				}
				throw;
			}
			catch (Exception ex)
			{
				Tracer<FlowController>.WriteError(ex);
				if (!(ex is OperationCanceledException) && !(ex.InnerException is PackageNotFoundException))
				{
					throw;
				}
				packageFileInfo = new MsrPackageInfo(this.notFoundText, this.notFoundText, this.notFoundText);
			}
			finally
			{
				if (this.appContext.CurrentPhone != null)
				{
					this.appContext.CurrentPhone.PackageFileInfo = packageFileInfo;
					if (packageFileInfo != null && !string.IsNullOrEmpty(packageFileInfo.ManufacturerModelName) && (this.appContext.CurrentPhone.Type == PhoneTypes.Htc || this.appContext.CurrentPhone.Type == PhoneTypes.Lg || this.appContext.CurrentPhone.Type == PhoneTypes.Mcj || this.appContext.CurrentPhone.Type == PhoneTypes.Alcatel))
					{
						this.appContext.CurrentPhone.SalesName = packageFileInfo.ManufacturerModelName;
					}
					base.EventAggregator.Publish<FoundSoftwareVersionMessage>(new FoundSoftwareVersionMessage(status, packageFileInfo));
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
		[CustomCommand(IsAsynchronous = true)]
		public void DownloadEmergencyPackage(object parameter, CancellationToken cancellationToken)
		{
			if (this.appContext.CurrentPhone == null)
			{
				throw new DeviceNotFoundException();
			}
			if (!Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckPermission(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath))
			{
				throw new CannotAccessDirectoryException(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath);
			}
			try
			{
				this.logics.AdaptationManager.DownloadEmeregencyPackage(this.appContext.CurrentPhone, cancellationToken);
				if (cancellationToken.IsCancellationRequested)
				{
					base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, new List<string>
					{
						"DownloadCancelled"
					}));
					base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
					Tracer<FlowController>.WriteInformation("Download package canceled.");
				}
				else
				{
					base.Commands.Run((AppController c) => c.SwitchToState("FlashingState"));
				}
			}
			catch (TaskCanceledException)
			{
				base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, new List<string>
				{
					"DownloadCancelled"
				}));
				base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
				Tracer<FlowController>.WriteInformation("Download package canceled.");
			}
			catch (WebException arg)
			{
				Tracer<FlowController>.WriteInformation("Download package failed:\n" + arg);
				if (!NetworkInterface.GetIsNetworkAvailable())
				{
					throw new NoInternetConnectionException();
				}
				throw;
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C52C File Offset: 0x0000A72C
		[CustomCommand]
		public void CancelDownloadEmergencyPackage()
		{
			if (new DialogMessageManager().ShowQuestionDialog(LocalizationManager.GetTranslation("DownloadingCancelMessage"), null, true) == true)
			{
				((IAsyncDelegateCommand)base.Commands["DownloadEmergencyPackage"]).Cancel();
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000C594 File Offset: 0x0000A794
		[CustomCommand(IsAsynchronous = true)]
		public void DownloadPackage(object parameter, CancellationToken token)
		{
			if (this.appContext.CurrentPhone == null)
			{
				throw new DeviceNotFoundException();
			}
			if (!Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckPermission(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath))
			{
				throw new CannotAccessDirectoryException(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath);
			}
			try
			{
				this.logics.AdaptationManager.DownloadPackage(this.appContext.CurrentPhone, token);
				if (token.IsCancellationRequested)
				{
					base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, new List<string>
					{
						"DownloadCancelled"
					}));
					base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
					Tracer<FlowController>.WriteInformation("Download package canceled.");
				}
				else
				{
					string nextState = (this.appContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory) ? "FlashingState" : "BatteryCheckingState";
					base.Commands.Run((AppController c) => c.SwitchToState(nextState));
				}
			}
			catch (OperationCanceledException)
			{
				base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, new List<string>
				{
					"DownloadCancelled"
				}));
				base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
				Tracer<FlowController>.WriteInformation("Download package canceled.");
			}
			catch (WebException arg)
			{
				Tracer<FlowController>.WriteInformation("Download package failed:\n" + arg);
				if (!NetworkInterface.GetIsNetworkAvailable())
				{
					throw new NoInternetConnectionException();
				}
				throw;
			}
			catch (NotEnoughSpaceException)
			{
				base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false));
				throw;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C888 File Offset: 0x0000AA88
		[CustomCommand]
		public void CancelDownloadPackage()
		{
			if (new DialogMessageManager().ShowQuestionDialog(LocalizationManager.GetTranslation("DownloadingCancelMessage"), null, true) == true)
			{
				((IAsyncDelegateCommand)base.Commands["DownloadPackage"]).Cancel();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
		[CustomCommand]
		public void CompareFirmwareVersions()
		{
			SwVersionComparisonResult status = this.logics.AdaptationManager.CompareFirmwareVersions(this.appContext.CurrentPhone);
			base.EventAggregator.Publish<FirmwareVersionsCompareMessage>(new FirmwareVersionsCompareMessage(status));
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000C924 File Offset: 0x0000AB24
		[CustomCommand]
		public void StartDeviceDetection(DetectionParameters detectionParams)
		{
			this.logics.AdaptationManager.DeviceConnected += this.AdaptationManagerDeviceConnected;
			this.logics.AdaptationManager.DeviceDisconnected += this.AdaptationManagerDeviceDisconnected;
			this.logics.AdaptationManager.StartDevicesAutodetection(detectionParams);
			List<Phone> connectedPhones = this.logics.AdaptationManager.GetConnectedPhones(detectionParams);
			foreach (Phone phone in connectedPhones)
			{
				this.AdaptationManagerDeviceConnected(phone);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		[CustomCommand]
		public void StopDeviceDetection()
		{
			this.logics.AdaptationManager.StopDevicesAutodetection();
			this.logics.AdaptationManager.DeviceConnected -= this.AdaptationManagerDeviceConnected;
			this.logics.AdaptationManager.DeviceDisconnected -= this.AdaptationManagerDeviceDisconnected;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000CA35 File Offset: 0x0000AC35
		[CustomCommand]
		public void GetConnectedPhones(DetectionParameters detectionParams)
		{
			base.EventAggregator.Publish<ConnectedPhonesMessage>(new ConnectedPhonesMessage(this.logics.AdaptationManager.GetConnectedPhones(detectionParams)));
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000CA5A File Offset: 0x0000AC5A
		[CustomCommand]
		public void GetSupportedManufacturers()
		{
			base.EventAggregator.Publish<SupportedManufacturersMessage>(new SupportedManufacturersMessage(this.logics.AdaptationManager.GetAdaptationsData()));
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000CA7E File Offset: 0x0000AC7E
		[CustomCommand]
		public void GetSupportedAdaptationModels(PhoneTypes phoneType)
		{
			base.EventAggregator.Publish<SupportedAdaptationModelsMessage>(new SupportedAdaptationModelsMessage(this.logics.AdaptationManager.GetSupportedAdaptationModels(phoneType)));
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000CAA4 File Offset: 0x0000ACA4
		[CustomCommand]
		public void Finish(bool isPassed)
		{
			if (!isPassed && this.appContext.SelectedManufacturer == PhoneTypes.Htc)
			{
				base.Commands.Run((AppController c) => c.SwitchToState("RebootHtcState"));
			}
			else
			{
				base.Commands.Run((AppController c) => c.SwitchToState("AutomaticManufacturerSelectionState"));
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		[CustomCommand]
		public void CancelBatteryChecking()
		{
			base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false, new List<string>
			{
				"BatteryCheckingCancelled"
			}));
			base.Commands.Run((AppController c) => c.SwitchToState("SummaryState"));
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000CC39 File Offset: 0x0000AE39
		private void AdaptationManagerDeviceDisconnected(Phone phone)
		{
			base.EventAggregator.Publish<DeviceDisconnectedMessage>(new DeviceDisconnectedMessage(phone));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000CC4E File Offset: 0x0000AE4E
		private void AdaptationManagerDeviceConnected(Phone phone)
		{
			base.EventAggregator.Publish<DeviceConnectedMessage>(new DeviceConnectedMessage(phone));
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000CC63 File Offset: 0x0000AE63
		[CustomCommand]
		public void CancelCheckLatestPackage()
		{
			((IAsyncDelegateCommand)base.Commands["CheckLatestPackage"]).Cancel();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000CC81 File Offset: 0x0000AE81
		[CustomCommand(IsAsynchronous = true)]
		public void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			this.logics.AdaptationManager.ReadDeviceInfo(currentPhone, cancellationToken);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000CC97 File Offset: 0x0000AE97
		[CustomCommand(IsAsynchronous = true)]
		public void ReadDeviceBatteryLevel(Phone currentPhone, CancellationToken cancellationToken)
		{
			this.logics.AdaptationManager.ReadDeviceBatteryLevel(currentPhone, cancellationToken);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000CCAD File Offset: 0x0000AEAD
		[CustomCommand(IsAsynchronous = true)]
		public void ReadDeviceBatteryStatus(Phone phone, CancellationToken cancellationToken)
		{
			this.logics.AdaptationManager.ReadDeviceBatteryStatus(phone, cancellationToken);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000CCC3 File Offset: 0x0000AEC3
		[CustomCommand(IsAsynchronous = true)]
		public void CheckIfDeviceStillConnected(Phone phone, CancellationToken cancellationToken)
		{
			this.logics.AdaptationManager.CheckIfDeviceStillConnected(phone, cancellationToken);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000CCD9 File Offset: 0x0000AED9
		[CustomCommand]
		public void CancelReadDeviceInfo()
		{
			((IAsyncDelegateCommand)base.Commands["ReadDeviceInfo"]).Cancel();
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000CCF7 File Offset: 0x0000AEF7
		[CustomCommand(IsAsynchronous = true)]
		public void SurveyCompleted(SurveyReport survey, CancellationToken cancellationToken)
		{
			this.reportingService.SurveySucceded(survey);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000CD07 File Offset: 0x0000AF07
		[CustomCommand(IsAsynchronous = true)]
		public void StartSessionFlow(string sessionParameter, CancellationToken cancellationToken)
		{
			this.reportingService.StartFlowSession();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000CD18 File Offset: 0x0000AF18
		private void AdaptationManagerDeviceInfoRead(Phone phone)
		{
			if (phone != null)
			{
				this.appContext.CurrentPhone = phone;
			}
			base.EventAggregator.Publish<DeviceInfoReadMessage>(new DeviceInfoReadMessage(phone != null));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000CD58 File Offset: 0x0000AF58
		private void AdaptationManagerDeviceBatteryLevelRead(Phone phone)
		{
			if (phone != null)
			{
				this.appContext.CurrentPhone = phone;
			}
			base.EventAggregator.Publish<DeviceInfoReadMessage>(new DeviceInfoReadMessage(phone != null));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000CD95 File Offset: 0x0000AF95
		private void AdaptationManagerDeviceBatteryStatusRead(BatteryStatus batteryStatus)
		{
			base.EventAggregator.Publish<DeviceBatteryStatusReadMessage>(new DeviceBatteryStatusReadMessage(batteryStatus));
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000CDAA File Offset: 0x0000AFAA
		private void AdaptationManagerDeviceConnectionStatusRead(bool deviceConnectionStatus)
		{
			base.EventAggregator.Publish<DeviceConnectionStatusReadMessage>(new DeviceConnectionStatusReadMessage(deviceConnectionStatus));
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000CDC0 File Offset: 0x0000AFC0
		private bool CheckCustomDirectoryExistenceAndPermissions(string path)
		{
			string path2 = Path.Combine(path, "Products");
			if (!Directory.Exists(path2))
			{
				try
				{
					Directory.CreateDirectory(path2);
				}
				catch
				{
					return false;
				}
			}
			return Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CheckDirectoryWritePermission(path2);
		}

		// Token: 0x040000CC RID: 204
		private readonly string notFoundText = LocalizationManager.GetTranslation("NotFound");

		// Token: 0x040000CD RID: 205
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040000CE RID: 206
		private readonly LogicContext logics;

		// Token: 0x040000CF RID: 207
		private readonly ReportingService reportingService;
	}
}
