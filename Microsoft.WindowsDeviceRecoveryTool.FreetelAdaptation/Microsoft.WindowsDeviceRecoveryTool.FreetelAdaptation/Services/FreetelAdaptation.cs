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

namespace Microsoft.WindowsDeviceRecoveryTool.FreetelAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Freetel)]
	public class FreetelAdaptation : BaseAdaptation
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002620 File Offset: 0x00000820
		[ImportingConstructor]
		public FreetelAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002675 File Offset: 0x00000875
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000016 RID: 22 RVA: 0x0000267C File Offset: 0x0000087C
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Freetel;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002680 File Offset: 0x00000880
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002683 File Offset: 0x00000883
		public override string ReportManufacturerName
		{
			get
			{
				return "Freetel";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000019 RID: 25 RVA: 0x0000268A File Offset: 0x0000088A
		public override string ManufacturerName
		{
			get
			{
				return "Freetel";
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002694 File Offset: 0x00000894
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<FreetelAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<FreetelAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002744 File Offset: 0x00000944
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<FreetelAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<FreetelAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000276B File Offset: 0x0000096B
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000276E File Offset: 0x0000096E
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002778 File Offset: 0x00000978
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<FreetelAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetFreetelProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetFreetelProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<FreetelAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<FreetelAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<FreetelAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002914 File Offset: 0x00000B14
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000291B File Offset: 0x00000B1B
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002924 File Offset: 0x00000B24
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<FreetelAdaptation>.LogEntry("FlashDevice");
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
			Tracer<FreetelAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029A4 File Offset: 0x00000BA4
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029D9 File Offset: 0x00000BD9
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000029E8 File Offset: 0x00000BE8
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
				Tracer<FreetelAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A74 File Offset: 0x00000C74
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B24 File Offset: 0x00000D24
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

		// Token: 0x06000027 RID: 39 RVA: 0x00002B68 File Offset: 0x00000D68
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

		// Token: 0x06000028 RID: 40 RVA: 0x00002BC4 File Offset: 0x00000DC4
		protected override void InitializeManuallySupportedModels()
		{
			ModelInfo[] array = new ModelInfo[]
			{
				FreetelModels.Katana01Model,
				FreetelModels.Katana02Model
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

		// Token: 0x06000029 RID: 41 RVA: 0x00002C52 File Offset: 0x00000E52
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C7C File Offset: 0x00000E7C
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where string.Equals(variant.HardwareModel, phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002CCC File Offset: 0x00000ECC
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.SalesName.ToLower().Contains("katana 01"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Katana01.jpg"));
			}
			else if (phone.SalesName.ToLower().Contains("katana 02"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Katana02.jpg"));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D88 File Offset: 0x00000F88
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Freetel Logo.jpg"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DD7 File Offset: 0x00000FD7
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002E04 File Offset: 0x00001004
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<FreetelAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<FreetelAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x0400000B RID: 11
		private readonly MsrService msrService;

		// Token: 0x0400000C RID: 12
		private readonly ReportingService reportingService;

		// Token: 0x0400000D RID: 13
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x0400000E RID: 14
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x0400000F RID: 15
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x04000010 RID: 16
		private string progressMessage;
	}
}
