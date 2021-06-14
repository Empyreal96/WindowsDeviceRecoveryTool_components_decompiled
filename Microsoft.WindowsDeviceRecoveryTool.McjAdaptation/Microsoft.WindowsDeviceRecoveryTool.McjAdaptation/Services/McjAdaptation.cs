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

namespace Microsoft.WindowsDeviceRecoveryTool.McjAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Mcj)]
	public class McjAdaptation : BaseAdaptation
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000027D8 File Offset: 0x000009D8
		[ImportingConstructor]
		public McjAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.salesNameProvider = new SalesNameProvider();
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x0000283C File Offset: 0x00000A3C
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Mcj;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002850 File Offset: 0x00000A50
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002864 File Offset: 0x00000A64
		public override string ReportManufacturerName
		{
			get
			{
				return "MCJ";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001A RID: 26 RVA: 0x0000287C File Offset: 0x00000A7C
		public override string ManufacturerName
		{
			get
			{
				return "MCJ";
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002894 File Offset: 0x00000A94
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000028AC File Offset: 0x00000AAC
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028F4 File Offset: 0x00000AF4
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where string.Equals(variant.HardwareModel, phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002930 File Offset: 0x00000B30
		public override ISalesNameProvider SalesNameProvider()
		{
			return this.salesNameProvider;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002948 File Offset: 0x00000B48
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002950 File Offset: 0x00000B50
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002958 File Offset: 0x00000B58
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<McjAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<McjAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002AD8 File Offset: 0x00000CD8
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

		// Token: 0x06000023 RID: 35 RVA: 0x00002B20 File Offset: 0x00000D20
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

		// Token: 0x06000024 RID: 36 RVA: 0x00002B84 File Offset: 0x00000D84
		protected override void InitializeManuallySupportedModels()
		{
			ModelInfo[] array = new ModelInfo[]
			{
				McjModels.MadosmaQ501,
				McjModels.MadosmaQ601
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

		// Token: 0x06000025 RID: 37 RVA: 0x00002C2C File Offset: 0x00000E2C
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<McjAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<McjAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C58 File Offset: 0x00000E58
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<McjAdaptation>.LogEntry("FlashDevice");
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
			Tracer<McjAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CEC File Offset: 0x00000EEC
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D0C File Offset: 0x00000F0C
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<McjAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetMcjProductsPath(phone.QueryParameters.ManufacturerHardwareModel),
					FilesVersioned = true
				};
				Tracer<McjAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<McjAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<McjAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002EB4 File Offset: 0x000010B4
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002EBC File Offset: 0x000010BC
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002ED0 File Offset: 0x000010D0
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
				Tracer<McjAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F70 File Offset: 0x00001170
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002FAC File Offset: 0x000011AC
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<McjAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<McjAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003004 File Offset: 0x00001204
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000303B File Offset: 0x0000123B
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000030A4 File Offset: 0x000012A4
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.SalesName.ToLower().Contains("Q501"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Madosma.png"));
			}
			else if (phone.SalesName.ToLower().Contains("Q601"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Madosma_Q601.jpg"));
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

		// Token: 0x06000031 RID: 49 RVA: 0x00003194 File Offset: 0x00001394
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Mouse_logo_new.gif"));
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

		// Token: 0x0400000B RID: 11
		private readonly MsrService msrService;

		// Token: 0x0400000C RID: 12
		private readonly ReportingService reportingService;

		// Token: 0x0400000D RID: 13
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x0400000E RID: 14
		private readonly SalesNameProvider salesNameProvider;

		// Token: 0x0400000F RID: 15
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000010 RID: 16
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x04000011 RID: 17
		private string progressMessage;
	}
}
