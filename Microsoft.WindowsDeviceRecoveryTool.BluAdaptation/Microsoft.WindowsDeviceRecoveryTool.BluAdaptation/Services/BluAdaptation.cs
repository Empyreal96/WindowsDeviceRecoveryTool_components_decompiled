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

namespace Microsoft.WindowsDeviceRecoveryTool.BluAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[ExportAdaptation(Type = PhoneTypes.Blu)]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class BluAdaptation : BaseAdaptation
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002D78 File Offset: 0x00000F78
		[ImportingConstructor]
		public BluAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Blu;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002DFC File Offset: 0x00000FFC
		public override string ReportManufacturerName
		{
			get
			{
				return "BLU";
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002E14 File Offset: 0x00001014
		public override string ManufacturerName
		{
			get
			{
				return "BLU";
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x00002E2C File Offset: 0x0000102C
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002E43 File Offset: 0x00001043
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002E4C File Offset: 0x0000104C
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("05C6", "9093"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002EAB File Offset: 0x000010AB
		public override void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002EB0 File Offset: 0x000010B0
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002EC3 File Offset: 0x000010C3
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002ECC File Offset: 0x000010CC
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<BluAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetBluProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetBluProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<BluAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationFailed(phone, ReportOperationType.DownloadPackage, resultUriData, ex);
				Tracer<BluAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<BluAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003098 File Offset: 0x00001298
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000030B8 File Offset: 0x000012B8
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<BluAdaptation>.LogEntry("FlashDevice");
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
			Tracer<BluAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000314C File Offset: 0x0000134C
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<BluAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<BluAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003178 File Offset: 0x00001378
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<BluAdaptation>.LogEntry("CheckLatestPackage");
				if (phone.QueryParameters == null)
				{
					throw new ArgumentException("Package query parameter not set.");
				}
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
				Tracer<BluAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003274 File Offset: 0x00001474
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003300 File Offset: 0x00001500
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

		// Token: 0x0600002C RID: 44 RVA: 0x00003348 File Offset: 0x00001548
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

		// Token: 0x0600002D RID: 45 RVA: 0x000033AC File Offset: 0x000015AC
		protected override void InitializeManuallySupportedModels()
		{
			ModelInfo[] array = new ModelInfo[]
			{
				BluModels.WinJrLte,
				BluModels.WinHdLte,
				BluModels.WinJR410,
				BluModels.WinHd510
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

		// Token: 0x0600002E RID: 46 RVA: 0x00003468 File Offset: 0x00001668
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000034B0 File Offset: 0x000016B0
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where string.Equals(variant.HardwareModel, phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000355C File Offset: 0x0000175C
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.SalesName.ToLower().Contains("win hd lte"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("winhdlte.png"));
			}
			else if (phone.SalesName.ToLower().Contains("win jr w410a"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("winjr.png"));
			}
			else if (phone.SalesName.ToLower().Contains("win hd w510u"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("winhd.png"));
			}
			else if (phone.SalesName.ToLower().Contains("win jr lte"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourcesName) => resourcesName.Contains("winjrlte.png"));
			}
			Stream result;
			if (!string.IsNullOrEmpty(text))
			{
				result = executingAssembly.GetManifestResourceStream(text);
			}
			else
			{
				result = base.GetImageDataStream(phone);
			}
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000036E0 File Offset: 0x000018E0
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("blulogo.png"));
			Stream result;
			if (!string.IsNullOrEmpty(text))
			{
				result = executingAssembly.GetManifestResourceStream(text);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000373D File Offset: 0x0000193D
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000376C File Offset: 0x0000196C
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<BluAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<BluAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000037C4 File Offset: 0x000019C4
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x04000015 RID: 21
		private readonly MsrService msrService;

		// Token: 0x04000016 RID: 22
		private readonly ReportingService reportingService;

		// Token: 0x04000017 RID: 23
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000018 RID: 24
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000019 RID: 25
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x0400001A RID: 26
		private string progressMessage;
	}
}
