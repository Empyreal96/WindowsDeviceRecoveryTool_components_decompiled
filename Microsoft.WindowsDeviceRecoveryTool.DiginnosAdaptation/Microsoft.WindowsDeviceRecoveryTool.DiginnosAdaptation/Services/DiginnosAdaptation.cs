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
using Microsoft.WindowsDeviceRecoveryTool.Localization;
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
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.OemAdaptation.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.DiginnosAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Diginnos)]
	public class DiginnosAdaptation : BaseAdaptation
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002520 File Offset: 0x00000720
		[ImportingConstructor]
		public DiginnosAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x0000255F File Offset: 0x0000075F
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002566 File Offset: 0x00000766
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Diginnos;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000256A File Offset: 0x0000076A
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000256D File Offset: 0x0000076D
		public override string ReportManufacturerName
		{
			get
			{
				return "Diginnos";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002574 File Offset: 0x00000774
		public override string ManufacturerName
		{
			get
			{
				return "Diginnos";
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000257C File Offset: 0x0000077C
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<DiginnosAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<DiginnosAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000262C File Offset: 0x0000082C
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<DiginnosAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<DiginnosAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002653 File Offset: 0x00000853
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002656 File Offset: 0x00000856
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002660 File Offset: 0x00000860
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<DiginnosAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDiginnosProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDiginnosProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<DiginnosAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<DiginnosAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<DiginnosAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027FC File Offset: 0x000009FC
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002803 File Offset: 0x00000A03
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000280C File Offset: 0x00000A0C
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<DiginnosAdaptation>.LogEntry("FlashDevice");
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
			Tracer<DiginnosAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000288C File Offset: 0x00000A8C
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028C1 File Offset: 0x00000AC1
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028D0 File Offset: 0x00000AD0
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
				Tracer<DiginnosAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000295C File Offset: 0x00000B5C
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002994 File Offset: 0x00000B94
		private Phone GetPhone(ModelInfo modelInfo)
		{
			return new Phone
			{
				Type = this.PhoneType,
				SalesName = modelInfo.Name,
				HardwareModel = modelInfo.Name,
				ImageData = modelInfo.Bitmap.ToBytes(),
				ModelIdentificationInstruction = LocalizationManager.GetTranslation("ModelIdentificationUnderBackCover")
			};
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000029F0 File Offset: 0x00000BF0
		protected override void InitializeManuallySupportedModels()
		{
			Phone phone = this.GetPhone(DiginnosModels.DG_W10M);
			phone.QueryParameters = DiginnosModels.DG_W10M.Variants[0].MsrQueryParameters;
			this.manuallySupportedModels.Add(phone);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A2C File Offset: 0x00000C2C
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A44 File Offset: 0x00000C44
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Device Image.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AA8 File Offset: 0x00000CA8
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Diginnos Logo.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AF7 File Offset: 0x00000CF7
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B24 File Offset: 0x00000D24
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<DiginnosAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<DiginnosAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x04000009 RID: 9
		private readonly MsrService msrService;

		// Token: 0x0400000A RID: 10
		private readonly ReportingService reportingService;

		// Token: 0x0400000B RID: 11
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x0400000C RID: 12
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x0400000D RID: 13
		private string progressMessage;
	}
}
