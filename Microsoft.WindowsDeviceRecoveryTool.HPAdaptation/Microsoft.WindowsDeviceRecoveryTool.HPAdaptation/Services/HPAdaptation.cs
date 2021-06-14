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

namespace Microsoft.WindowsDeviceRecoveryTool.HPAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.HP)]
	public class HPAdaptation : BaseAdaptation
	{
		// Token: 0x06000015 RID: 21 RVA: 0x000027F8 File Offset: 0x000009F8
		[ImportingConstructor]
		public HPAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000284D File Offset: 0x00000A4D
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002854 File Offset: 0x00000A54
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.HP;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002858 File Offset: 0x00000A58
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000285B File Offset: 0x00000A5B
		public override string ReportManufacturerName
		{
			get
			{
				return "HP";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002862 File Offset: 0x00000A62
		public override string ManufacturerName
		{
			get
			{
				return "HP";
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000286C File Offset: 0x00000A6C
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<HPAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<HPAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000291C File Offset: 0x00000B1C
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HPAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<HPAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002943 File Offset: 0x00000B43
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002946 File Offset: 0x00000B46
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002950 File Offset: 0x00000B50
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HPAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetHPProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetHPProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<HPAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<HPAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<HPAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002AEC File Offset: 0x00000CEC
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002AF3 File Offset: 0x00000CF3
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002AFC File Offset: 0x00000CFC
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<HPAdaptation>.LogEntry("FlashDevice");
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
			Tracer<HPAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002B7C File Offset: 0x00000D7C
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002BB1 File Offset: 0x00000DB1
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002BC0 File Offset: 0x00000DC0
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
				Tracer<HPAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002C4C File Offset: 0x00000E4C
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("03F0", "0155"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002CA8 File Offset: 0x00000EA8
		private Phone GetPhone(ModelInfo modelInfo)
		{
			return new Phone
			{
				Type = this.PhoneType,
				SalesName = modelInfo.Name,
				HardwareModel = modelInfo.Name,
				ImageData = modelInfo.Bitmap.ToBytes(),
				ModelIdentificationInstruction = LocalizationManager.GetTranslation("ModelIndentificationHPElite3")
			};
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002D7C File Offset: 0x00000F7C
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

		// Token: 0x06000029 RID: 41 RVA: 0x00002DC0 File Offset: 0x00000FC0
		protected override void InitializeManuallySupportedModels()
		{
			ModelInfo[] array = new ModelInfo[]
			{
				HPModels.Elitex3,
				HPModels.Elitex3_Telstra,
				HPModels.Elitex3_Verizon
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

		// Token: 0x0600002A RID: 42 RVA: 0x00002E57 File Offset: 0x00001057
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E80 File Offset: 0x00001080
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where string.Equals(variant.HardwareModel, phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002EC4 File Offset: 0x000010C4
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("EliteX3_Gallery_Zoom1.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002F28 File Offset: 0x00001128
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("HP logo.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002F77 File Offset: 0x00001177
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002FA4 File Offset: 0x000011A4
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<HPAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<HPAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x0400000D RID: 13
		private readonly MsrService msrService;

		// Token: 0x0400000E RID: 14
		private readonly ReportingService reportingService;

		// Token: 0x0400000F RID: 15
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000010 RID: 16
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000011 RID: 17
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x04000012 RID: 18
		private string progressMessage;
	}
}
