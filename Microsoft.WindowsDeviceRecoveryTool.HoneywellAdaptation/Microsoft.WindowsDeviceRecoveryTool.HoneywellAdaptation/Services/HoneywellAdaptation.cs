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

namespace Microsoft.WindowsDeviceRecoveryTool.HoneywellAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Honeywell)]
	public class HoneywellAdaptation : BaseAdaptation
	{
		// Token: 0x0600001C RID: 28 RVA: 0x00002914 File Offset: 0x00000B14
		[ImportingConstructor]
		public HoneywellAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002969 File Offset: 0x00000B69
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002970 File Offset: 0x00000B70
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Honeywell;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002974 File Offset: 0x00000B74
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002977 File Offset: 0x00000B77
		public override string ReportManufacturerName
		{
			get
			{
				return "HONEYWELL";
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000297E File Offset: 0x00000B7E
		public override string ManufacturerName
		{
			get
			{
				return "HONEYWELL";
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002988 File Offset: 0x00000B88
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<HoneywellAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<HoneywellAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A38 File Offset: 0x00000C38
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HoneywellAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<HoneywellAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A5F File Offset: 0x00000C5F
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A62 File Offset: 0x00000C62
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A6C File Offset: 0x00000C6C
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HoneywellAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetHoneywellProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetHoneywellProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<HoneywellAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<HoneywellAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<HoneywellAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002C08 File Offset: 0x00000E08
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C0F File Offset: 0x00000E0F
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C18 File Offset: 0x00000E18
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HoneywellAdaptation>.LogEntry("FlashDevice");
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
			Tracer<HoneywellAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C98 File Offset: 0x00000E98
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002CCD File Offset: 0x00000ECD
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002CDC File Offset: 0x00000EDC
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
				Tracer<HoneywellAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D68 File Offset: 0x00000F68
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002DA0 File Offset: 0x00000FA0
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

		// Token: 0x0600002F RID: 47 RVA: 0x00002E74 File Offset: 0x00001074
		private Phone[] GetPhoneVariants(ModelInfo modelInfo)
		{
			return (from v in modelInfo.Variants
			select new Phone
			{
				Type = this.PhoneType,
				SalesName = v.Name,
				HardwareVariant = v.Name,
				HardwareModel = modelInfo.Name,
				ImageData = modelInfo.Bitmap.ToBytes(),
				QueryParameters = v.MsrQueryParameters
			}).ToArray<Phone>();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002EB8 File Offset: 0x000010B8
		protected override void InitializeManuallySupportedModels()
		{
			ModelInfo[] array = new ModelInfo[]
			{
				HoneywellModels.Dolphin_75e_W10M,
				HoneywellModels.Dolphin_CT50_W10M
			};
			foreach (ModelInfo modelInfo in array)
			{
				Phone phone = this.GetPhone(modelInfo);
				this.manuallySupportedModels.Add(phone);
				if (modelInfo.Variants.Length == 1)
				{
					phone.QueryParameters = modelInfo.Variants[0].MsrQueryParameters;
				}
				else
				{
					Phone[] phoneVariants = this.GetPhoneVariants(modelInfo);
					this.manuallySupportedVariants.AddRange(phoneVariants);
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F46 File Offset: 0x00001146
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002F70 File Offset: 0x00001170
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where string.Equals(variant.HardwareModel, phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002FC0 File Offset: 0x000011C0
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.SalesName.ToLower().Contains("dolphin 75e"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("D75e_360Tour.jpg"));
			}
			else if (phone.SalesName.ToLower().Contains("dolphin ct50"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("CT50_360Tour.jpg"));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000307C File Offset: 0x0000127C
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Honeywell_logo.png"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000030CB File Offset: 0x000012CB
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030F8 File Offset: 0x000012F8
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<HoneywellAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<HoneywellAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x04000010 RID: 16
		private readonly MsrService msrService;

		// Token: 0x04000011 RID: 17
		private readonly ReportingService reportingService;

		// Token: 0x04000012 RID: 18
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000013 RID: 19
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000014 RID: 20
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x04000015 RID: 21
		private string progressMessage;
	}
}
