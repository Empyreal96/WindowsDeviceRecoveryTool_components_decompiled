using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FFUComponents;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.YEZZAdaptation.Services
{
	// Token: 0x02000003 RID: 3
	[ExportAdaptation(Type = PhoneTypes.YEZZ)]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class YEZZAdaptation : BaseAdaptation
	{
		// Token: 0x0600000A RID: 10 RVA: 0x0000214C File Offset: 0x0000034C
		[ImportingConstructor]
		public YEZZAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000218B File Offset: 0x0000038B
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002192 File Offset: 0x00000392
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.YEZZ;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002196 File Offset: 0x00000396
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002199 File Offset: 0x00000399
		public override string ReportManufacturerName
		{
			get
			{
				return "YEZZ";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000021A0 File Offset: 0x000003A0
		public override string ManufacturerName
		{
			get
			{
				return "YEZZ";
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021A8 File Offset: 0x000003A8
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<YEZZAdaptation>.LogEntry("CheckLatestPackage");
				Task<PackageFileInfo> task = this.msrService.CheckLatestPackage(phone.QueryParameters, cancellationToken);
				task.Wait(cancellationToken);
				result = task.Result;
			}
			catch (Exception ex)
			{
				if (ex.InnerException is PackageNotFoundException)
				{
					throw ex.InnerException;
				}
				if (ex.InnerException != null && ex.InnerException.GetBaseException() is WebException)
				{
					throw new WebException();
				}
				if (ex is OperationCanceledException || ex.InnerException is TaskCanceledException)
				{
					throw;
				}
				throw;
			}
			finally
			{
				Tracer<YEZZAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002258 File Offset: 0x00000458
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<YEZZAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<YEZZAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000227F File Offset: 0x0000047F
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002282 File Offset: 0x00000482
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000228C File Offset: 0x0000048C
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<YEZZAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetYEZZProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetYEZZProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<YEZZAdaptation>.WriteInformation("Download Params: {0}", new object[]
				{
					downloadParameters
				});
				phone.PackageFiles = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken);
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationSucceded(phone, ReportOperationType.DownloadPackage);
			}
			catch (Exception ex)
			{
				bool flag = true;
				UriData resultUriData;
				if (ex is OperationCanceledException || ex.GetBaseException() is TaskCanceledException)
				{
					resultUriData = UriData.DownloadVariantPackageAbortedByUser;
					flag = false;
				}
				else
				{
					resultUriData = UriData.FailedToDownloadVariantPackage;
				}
				Tuple<long, long, bool> downloadPackageInformation2 = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation2.Item1, downloadPackageInformation2.Item2, downloadPackageInformation2.Item3);
				this.reportingService.OperationFailed(phone, ReportOperationType.DownloadPackage, resultUriData, ex);
				Tracer<YEZZAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<YEZZAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002428 File Offset: 0x00000628
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000242F File Offset: 0x0000062F
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002438 File Offset: 0x00000638
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<YEZZAdaptation>.LogEntry("FlashDevice");
			IFFUDevice ffuDevice = this.GetFfuDevice(phone);
			if (ffuDevice != null)
			{
				ffuDevice.ProgressEvent += this.FlashProgressEvent;
				try
				{
					this.progressMessage = "SoftwareInstallation";
					ffuDevice.FlashFFUFile(phone.PackageFiles.First<string>(), true);
				}
				finally
				{
					ffuDevice.ProgressEvent -= this.FlashProgressEvent;
				}
			}
			Tracer<YEZZAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000024B8 File Offset: 0x000006B8
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024ED File Offset: 0x000006ED
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024FC File Offset: 0x000006FC
		public override void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			try
			{
				SimpleIODevice simpleIODevice = new SimpleIODevice(currentPhone.Path);
				if (simpleIODevice.IsConnected())
				{
					PlatformId platformId = new PlatformId();
					platformId.SetPlatformId(simpleIODevice.DeviceFriendlyName);
					currentPhone.PlatformId = platformId;
					currentPhone.SalesName = simpleIODevice.DeviceFriendlyName;
					currentPhone.ConnectionId = simpleIODevice.DeviceUniqueID;
					base.RaiseDeviceInfoRead(currentPhone);
					return;
				}
			}
			catch (Exception ex)
			{
				Tracer<YEZZAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002588 File Offset: 0x00000788
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025C0 File Offset: 0x000007C0
		protected override void InitializeManuallySupportedModels()
		{
			Phone phone = new Phone
			{
				Type = this.PhoneType,
				SalesName = "YEZZ Billy 4.7",
				HardwareModel = "Billy 4.7"
			};
			phone.ImageData = base.GetImageData(phone);
			phone.QueryParameters = new QueryParameters
			{
				ManufacturerName = "YEZZ",
				ManufacturerHardwareModel = "Billy 4.7",
				ManufacturerModelName = "Billy 4.7",
				ManufacturerHardwareVariant = "VAR-GSM"
			};
			this.manuallySupportedModels.Add(phone);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002649 File Offset: 0x00000849
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002660 File Offset: 0x00000860
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("yezz-billy-47.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026C4 File Offset: 0x000008C4
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("yezz-logo.png"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002713 File Offset: 0x00000913
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002740 File Offset: 0x00000940
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<YEZZAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<YEZZAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x04000003 RID: 3
		private readonly MsrService msrService;

		// Token: 0x04000004 RID: 4
		private readonly ReportingService reportingService;

		// Token: 0x04000005 RID: 5
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000006 RID: 6
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000007 RID: 7
		private string progressMessage;
	}
}
