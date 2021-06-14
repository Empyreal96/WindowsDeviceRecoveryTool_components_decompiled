using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.States.BaseStates;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000BA RID: 186
	[Export]
	public class CheckLatestPackageViewModel : BaseViewModel, ICanHandle<FoundSoftwareVersionMessage>, ICanHandle<FirmwareVersionsCompareMessage>, ICanHandle<FfuFilePlatformIdMessage>, ICanHandle<PackageDirectoryMessage>, ICanHandle<DeviceDisconnectedMessage>, ICanHandle<DeviceConnectionStatusReadMessage>, ICanHandle
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x0001C0D7 File Offset: 0x0001A2D7
		[ImportingConstructor]
		public CheckLatestPackageViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
			this.StartSoftwareInstallCommand = new DelegateCommand<object>(new Action<object>(this.StartSoftwareInstallCommandOnExecuted));
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001C104 File Offset: 0x0001A304
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0001C11B File Offset: 0x0001A31B
		public ICommand StartSoftwareInstallCommand { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0001C124 File Offset: 0x0001A324
		public override string PreviousStateName
		{
			get
			{
				string result;
				if (this.conditions.IsHtcConnected())
				{
					base.EventAggregator.Publish<FlashResultMessage>(new FlashResultMessage(false));
					result = "RebootHtcState";
				}
				else
				{
					result = "AutomaticManufacturerSelectionState";
				}
				return result;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0001C168 File Offset: 0x0001A368
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0001C180 File Offset: 0x0001A380
		public Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext AppContext
		{
			get
			{
				return this.appContext;
			}
			set
			{
				base.SetValue<Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext>(() => this.AppContext, ref this.appContext, value);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001C1D0 File Offset: 0x0001A3D0
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0001C1E8 File Offset: 0x0001A3E8
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				base.SetValue<string>(() => this.Description, ref this.description, value);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0001C238 File Offset: 0x0001A438
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0001C250 File Offset: 0x0001A450
		public bool IsBusy
		{
			get
			{
				return this.isBusy;
			}
			set
			{
				base.SetValue<bool>(() => this.IsBusy, ref this.isBusy, value);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001C2A0 File Offset: 0x0001A4A0
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x0001C2B8 File Offset: 0x0001A4B8
		public bool IsAkVersionVisible
		{
			get
			{
				return this.isAkVersionVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.IsAkVersionVisible, ref this.isAkVersionVisible, value);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001C308 File Offset: 0x0001A508
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x0001C320 File Offset: 0x0001A520
		public bool IsFirmwareVersionVisible
		{
			get
			{
				return this.isFirmwareVersionVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.IsFirmwareVersionVisible, ref this.isFirmwareVersionVisible, value);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0001C370 File Offset: 0x0001A570
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x0001C388 File Offset: 0x0001A588
		public bool IsBuildVersionVisible
		{
			get
			{
				return this.isBuildVersionVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.IsBuildVersionVisible, ref this.isBuildVersionVisible, value);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
		public bool IsPlatformIdVisible
		{
			get
			{
				return this.isPlatformIdVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.IsPlatformIdVisible, ref this.isPlatformIdVisible, value);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001C440 File Offset: 0x0001A640
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x0001C460 File Offset: 0x0001A660
		public bool UseSignatureCheck
		{
			get
			{
				return this.flowConditions.UseSignatureCheck;
			}
			set
			{
				this.flowConditions.UseSignatureCheck = value;
				base.RaisePropertyChanged<bool>(() => this.UseSignatureCheck);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001C4B8 File Offset: 0x0001A6B8
		public bool IsUseSignatureCheckChoiceAvailable
		{
			get
			{
				return this.flowConditions.IsSignatureCheckChoiceAvailable;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001C4D8 File Offset: 0x0001A6D8
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x0001C504 File Offset: 0x0001A704
		public bool IsManualSelectionEnabled
		{
			get
			{
				return this.flowConditions.IsManualSelectionAvailable && this.isManualSelectionEnabled;
			}
			private set
			{
				base.SetValue<bool>(() => this.IsManualSelectionEnabled, ref this.isManualSelectionEnabled, value);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001C554 File Offset: 0x0001A754
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x0001C56C File Offset: 0x0001A76C
		public bool IsNextEnabled
		{
			get
			{
				return this.isNextEnabled;
			}
			set
			{
				base.SetValue<bool>(() => this.IsNextEnabled, ref this.isNextEnabled, value);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001C5BC File Offset: 0x0001A7BC
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
		public bool IsPackageFound
		{
			get
			{
				return this.isPackageFound;
			}
			set
			{
				base.SetValue<bool>(() => this.IsPackageFound, ref this.isPackageFound, value);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001C624 File Offset: 0x0001A824
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x0001C63C File Offset: 0x0001A83C
		public SwVersionComparisonResult SoftwareComparisonStatus
		{
			get
			{
				return this.softwareComparisonStatus;
			}
			set
			{
				base.SetValue<SwVersionComparisonResult>(() => this.SoftwareComparisonStatus, ref this.softwareComparisonStatus, value);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001C68C File Offset: 0x0001A88C
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x0001C6A4 File Offset: 0x0001A8A4
		public string ContinueButtonText
		{
			get
			{
				return this.continueButtonText;
			}
			set
			{
				base.SetValue<string>(() => this.ContinueButtonText, ref this.continueButtonText, value);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x0001C70C File Offset: 0x0001A90C
		public string SoftwareInfoHeader
		{
			get
			{
				return this.softwareInfoHeader;
			}
			set
			{
				base.SetValue<string>(() => this.SoftwareInfoHeader, ref this.softwareInfoHeader, value);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001C75C File Offset: 0x0001A95C
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		public string FfuFilePath
		{
			get
			{
				string result;
				if (this.appContext.CurrentPhone != null)
				{
					result = this.appContext.CurrentPhone.PackageFilePath;
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			set
			{
				if (this.appContext.CurrentPhone != null)
				{
					base.SetValue<string>(() => this.FfuFilePath, delegate()
					{
						this.appContext.CurrentPhone.PackageFilePath = value;
					});
				}
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001C850 File Offset: 0x0001AA50
		public override void OnStarted()
		{
			if (this.AppContext.CurrentPhone == null)
			{
				Tracer<CheckLatestPackageViewModel>.WriteInformation("Current phone is empty. Unable to check latest package.");
				throw new DeviceNotFoundException();
			}
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("CheckLatestPackageHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			this.ContinueButtonText = LocalizationManager.GetTranslation("ButtonInstallSoftware");
			this.IsNextEnabled = false;
			this.IsManualSelectionEnabled = false;
			this.IsAkVersionVisible = (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.Analog);
			this.IsFirmwareVersionVisible = (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type != PhoneTypes.Analog);
			this.IsBuildVersionVisible = false;
			this.IsPackageFound = true;
			this.Description = string.Empty;
			this.IsPlatformIdVisible = false;
			this.SoftwareInfoHeader = LocalizationManager.GetTranslation("SoftwareOnServer");
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.All, PhoneModes.Normal);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
			if (this.AppContext.CurrentPhone.Type == PhoneTypes.Lumia || this.AppContext.CurrentPhone.Type == PhoneTypes.Htc || this.AppContext.CurrentPhone.Type == PhoneTypes.Lg || this.AppContext.CurrentPhone.Type == PhoneTypes.Mcj || this.AppContext.CurrentPhone.Type == PhoneTypes.Blu || this.AppContext.CurrentPhone.Type == PhoneTypes.Alcatel || this.AppContext.CurrentPhone.Type == PhoneTypes.Analog || this.AppContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory || this.AppContext.CurrentPhone.Type == PhoneTypes.Acer || this.AppContext.CurrentPhone.Type == PhoneTypes.Trinity || this.AppContext.CurrentPhone.Type == PhoneTypes.Unistrong || this.AppContext.CurrentPhone.Type == PhoneTypes.YEZZ || this.AppContext.CurrentPhone.Type == PhoneTypes.Acer || this.AppContext.CurrentPhone.Type == PhoneTypes.VAIO || this.AppContext.CurrentPhone.Type == PhoneTypes.Diginnos || this.AppContext.CurrentPhone.Type == PhoneTypes.VAIO || this.AppContext.CurrentPhone.Type == PhoneTypes.Inversenet || this.AppContext.CurrentPhone.Type == PhoneTypes.Freetel || this.AppContext.CurrentPhone.Type == PhoneTypes.Funker || this.AppContext.CurrentPhone.Type == PhoneTypes.Micromax || this.AppContext.CurrentPhone.Type == PhoneTypes.XOLO || this.AppContext.CurrentPhone.Type == PhoneTypes.KM || this.AppContext.CurrentPhone.Type == PhoneTypes.Jenesis || this.AppContext.CurrentPhone.Type == PhoneTypes.Gomobile || this.AppContext.CurrentPhone.Type == PhoneTypes.HP || this.AppContext.CurrentPhone.Type == PhoneTypes.Lenovo || this.AppContext.CurrentPhone.Type == PhoneTypes.Zebra || this.AppContext.CurrentPhone.Type == PhoneTypes.Honeywell || this.AppContext.CurrentPhone.Type == PhoneTypes.Panasonic || this.AppContext.CurrentPhone.Type == PhoneTypes.TrekStor || this.AppContext.CurrentPhone.Type == PhoneTypes.Wileyfox)
			{
				this.IsBusy = true;
				base.Commands.Run((FlowController c) => c.CheckLatestPackage(null, CancellationToken.None));
			}
			else if (this.AppContext.CurrentPhone.Type == PhoneTypes.Generic)
			{
				this.IsManualSelectionEnabled = true;
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001CD50 File Offset: 0x0001AF50
		public override void OnStopped()
		{
			base.OnStopped();
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
			base.Commands.Run((FlowController c) => c.CancelCheckLatestPackage());
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001CE00 File Offset: 0x0001B000
		public void Handle(FoundSoftwareVersionMessage message)
		{
			this.IsBusy = false;
			this.IsManualSelectionEnabled = (this.AppContext.CurrentPhone != null && this.AppContext.CurrentPhone.Type == PhoneTypes.Analog);
			if (message.Status)
			{
				base.Commands.Run((FlowController c) => c.CompareFirmwareVersions());
				this.IsAkVersionVisible = (message.PackageFileInfo != null && !string.IsNullOrWhiteSpace(message.PackageFileInfo.AkVersion) && message.PackageFileInfo.AkVersion != "0000.0000");
			}
			else
			{
				this.Handle(new FirmwareVersionsCompareMessage(SwVersionComparisonResult.PackageNotFound));
				if (this.AppContext.CurrentPhone != null)
				{
					this.IsAkVersionVisible = (this.AppContext.CurrentPhone.Type == PhoneTypes.Analog);
					this.IsManualSelectionEnabled = (this.AppContext.CurrentPhone.Type == PhoneTypes.Analog);
				}
			}
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001CF34 File Offset: 0x0001B134
		public void Handle(FirmwareVersionsCompareMessage message)
		{
			this.IsPackageFound = true;
			this.SoftwareComparisonStatus = message.Status;
			Tracer<CheckLatestPackageViewModel>.WriteInformation("Software comparison result: {0}", new object[]
			{
				message.Status
			});
			this.ContinueButtonText = LocalizationManager.GetTranslation("ButtonInstallSoftware");
			switch (message.Status)
			{
			case SwVersionComparisonResult.UnableToCompare:
				this.IsNextEnabled = true;
				break;
			case SwVersionComparisonResult.FirstIsGreater:
				this.IsNextEnabled = true;
				this.Description = LocalizationManager.GetTranslation("AvailablePackageIsOlder");
				break;
			case SwVersionComparisonResult.SecondIsGreater:
				this.IsNextEnabled = true;
				this.Description = LocalizationManager.GetTranslation("UpdateAvailable");
				break;
			case SwVersionComparisonResult.NumbersAreEqual:
				this.ContinueButtonText = LocalizationManager.GetTranslation("ReinstallSoftware");
				this.IsNextEnabled = true;
				this.Description = LocalizationManager.GetTranslation("PhoneIsUpToDate");
				break;
			case SwVersionComparisonResult.PackageNotFound:
				this.IsNextEnabled = false;
				this.IsPackageFound = false;
				this.Description = LocalizationManager.GetTranslation("PackageNotFound");
				if (this.appContext.SelectedManufacturer == PhoneTypes.Htc)
				{
					base.Commands.Run((AppController c) => c.SwitchToState("RebootHtcState"));
				}
				break;
			}
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001D0D0 File Offset: 0x0001B2D0
		public void Handle(PackageDirectoryMessage message)
		{
			if (base.IsStarted)
			{
				Tracer<CheckLatestPackageViewModel>.LogEntry("Handle");
				Tracer<CheckLatestPackageViewModel>.WriteInformation("Selected package manually: {0}", new object[]
				{
					message.Directory
				});
				this.FfuFilePath = message.Directory;
				if (!string.IsNullOrEmpty(this.FfuFilePath) && this.appContext.CurrentPhone.Type != PhoneTypes.Lumia)
				{
					base.Commands.Run((FfuController c) => c.ReadFfuFilePlatformId(this.FfuFilePath, CancellationToken.None));
				}
				else
				{
					this.CheckCompatibility(null);
				}
				Tracer<CheckLatestPackageViewModel>.LogExit("Handle");
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001D200 File Offset: 0x0001B400
		private void CheckCompatibility(FfuFilePlatformIdMessage platformIdMessage)
		{
			Tracer<CheckLatestPackageViewModel>.LogEntry("CheckCompatibility");
			PlatformId platformId = CheckLatestPackageViewModel.FindCompatiblePlatformId(platformIdMessage, this.appContext.CurrentPhone);
			if (this.appContext.CurrentPhone.Type == PhoneTypes.Analog)
			{
				this.SoftwareInfoHeader = LocalizationManager.GetTranslation("LocalPackage");
				if (platformIdMessage != null)
				{
					this.AppContext.CurrentPhone.PackageFileInfo = new FfuPackageFileInfo(this.FfuFilePath, platformId ?? platformIdMessage.PlatformId, platformIdMessage.Version, platformIdMessage.Version)
					{
						OfflinePackage = true
					};
				}
				else
				{
					this.AppContext.CurrentPhone.PackageFileInfo = new FfuPackageFileInfo(this.FfuFilePath, null, null);
				}
				this.AppContext.CurrentPhone.PackageFilePath = this.FfuFilePath;
				base.Commands.Run((FlowController c) => c.CompareFirmwareVersions());
				Tracer<CheckLatestPackageViewModel>.WriteInformation("Set local package: {0}", new object[]
				{
					this.FfuFilePath
				});
				SwVersionComparisonResult swVersionComparisonResult = VersionComparer.CompareSoftwareVersions(this.AppContext.CurrentPhone.PackageFileInfo.SoftwareVersion, AnalogAdaptation.OldestRollbackOsVersion, new char[]
				{
					'.'
				});
				if (platformId != null && (swVersionComparisonResult == SwVersionComparisonResult.FirstIsGreater || swVersionComparisonResult == SwVersionComparisonResult.NumbersAreEqual))
				{
					this.IsNextEnabled = true;
					this.Description = string.Empty;
				}
				else
				{
					this.IsNextEnabled = false;
					this.Description = LocalizationManager.GetTranslation("PackageNotCompatible");
				}
			}
			Tracer<CheckLatestPackageViewModel>.LogExit("CheckCompatibility");
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001D3D8 File Offset: 0x0001B5D8
		public void Handle(FfuFilePlatformIdMessage message)
		{
			if (base.IsStarted)
			{
				if (this.appContext.CurrentPhone != null)
				{
					this.CheckCompatibility(message);
				}
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001D444 File Offset: 0x0001B644
		private static PlatformId FindCompatiblePlatformId(FfuFilePlatformIdMessage message, Phone phone)
		{
			PlatformId result;
			if (message == null || phone == null || phone.PlatformId == null)
			{
				result = null;
			}
			else if (message.AllPlatformIds == null || !message.AllPlatformIds.Any<PlatformId>())
			{
				if (message.PlatformId.IsCompatibleWithDevicePlatformId(phone.PlatformId))
				{
					result = message.PlatformId;
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = message.AllPlatformIds.FirstOrDefault((PlatformId id) => id.IsCompatibleWithDevicePlatformId(phone.PlatformId));
			}
			return result;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001D4FC File Offset: 0x0001B6FC
		private void StartSoftwareInstallCommandOnExecuted(object obj)
		{
			if (this.AppContext.CurrentPhone != null && (this.AppContext.CurrentPhone.Type == PhoneTypes.Analog || this.AppContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory))
			{
				string state = (this.appContext.CurrentPhone.PackageFileInfo != null && this.appContext.CurrentPhone.PackageFileInfo.OfflinePackage) ? "PackageIntegrityCheckState" : "DownloadPackageState";
				base.Commands.Run((AppController c) => c.StartSoftwareInstallStatus(new Tuple<SwVersionComparisonResult, string>(this.SoftwareComparisonStatus, state)));
			}
			else
			{
				SwVersionComparisonResult softwareStatus = (SwVersionComparisonResult)obj;
				base.Commands.Run((AppController c) => c.StartSoftwareInstall(softwareStatus));
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
		private bool NeedToCheckIfDeviceWasDisconnected()
		{
			return this.appContext != null && this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001D724 File Offset: 0x0001B924
		public void Handle(DeviceDisconnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (this.NeedToCheckIfDeviceWasDisconnected())
				{
					base.Commands.Run((FlowController fc) => fc.CheckIfDeviceStillConnected(this.appContext.CurrentPhone, CancellationToken.None));
				}
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001D7F8 File Offset: 0x0001B9F8
		public void Handle(DeviceConnectionStatusReadMessage message)
		{
			if (base.IsStarted)
			{
				if (!message.Status)
				{
					throw new DeviceDisconnectedException();
				}
			}
		}

		// Token: 0x0400025B RID: 603
		[Import]
		private Conditions conditions;

		// Token: 0x0400025C RID: 604
		[Import]
		private FlowConditionService flowConditions;

		// Token: 0x0400025D RID: 605
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400025E RID: 606
		private bool isBusy;

		// Token: 0x0400025F RID: 607
		private bool isManualSelectionEnabled;

		// Token: 0x04000260 RID: 608
		private bool isNextEnabled;

		// Token: 0x04000261 RID: 609
		private bool isPackageFound;

		// Token: 0x04000262 RID: 610
		private string continueButtonText;

		// Token: 0x04000263 RID: 611
		private bool isAkVersionVisible;

		// Token: 0x04000264 RID: 612
		private bool isPlatformIdVisible;

		// Token: 0x04000265 RID: 613
		private bool isFirmwareVersionVisible;

		// Token: 0x04000266 RID: 614
		private bool isBuildVersionVisible;

		// Token: 0x04000267 RID: 615
		private string description;

		// Token: 0x04000268 RID: 616
		private string softwareInfoHeader;

		// Token: 0x04000269 RID: 617
		private SwVersionComparisonResult softwareComparisonStatus;
	}
}
