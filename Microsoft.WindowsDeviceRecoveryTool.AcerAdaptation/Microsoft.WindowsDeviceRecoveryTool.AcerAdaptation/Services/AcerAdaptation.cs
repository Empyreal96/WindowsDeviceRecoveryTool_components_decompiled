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

namespace Microsoft.WindowsDeviceRecoveryTool.AcerAdaptation.Services
{
	// Token: 0x02000006 RID: 6
	[ExportAdaptation(Type = PhoneTypes.Acer)]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class AcerAdaptation : BaseAdaptation
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002A40 File Offset: 0x00000C40
		[ImportingConstructor]
		public AcerAdaptation(MsrService msrService, ReportingService reportingService)
		{
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
			this.reportingService = reportingService;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002A8E File Offset: 0x00000C8E
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Acer;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002A91 File Offset: 0x00000C91
		public override string ReportManufacturerName
		{
			get
			{
				return "AcerInc";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002A98 File Offset: 0x00000C98
		public override string ManufacturerName
		{
			get
			{
				return "Acer";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002A9F File Offset: 0x00000C9F
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002AA2 File Offset: 0x00000CA2
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002AA9 File Offset: 0x00000CA9
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<AcerAdaptation>.LogEntry("CheckLatestPackage");
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
				Tracer<AcerAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002B7C File Offset: 0x00000D7C
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B94 File Offset: 0x00000D94
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AcerAdaptation>.LogEntry("FlashDevice");
			IFFUDevice ffuDevice = this.GetFfuDevice(phone);
			if (ffuDevice != null)
			{
				ffuDevice.ProgressEvent += this.FlashProgressEvent;
				try
				{
					this.progressMessage = "SoftwareInstallation";
					ffuDevice.FlashFFUFile((from f in phone.PackageFiles
					where f.EndsWith(".ffu", StringComparison.OrdinalIgnoreCase)
					select f).First<string>(), true);
				}
				finally
				{
					ffuDevice.ProgressEvent -= this.FlashProgressEvent;
				}
			}
			Tracer<AcerAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C38 File Offset: 0x00000E38
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			bool result;
			using (IFFUDevice ffuDevice = this.GetFfuDevice(phone))
			{
				result = (ffuDevice != null);
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C74 File Offset: 0x00000E74
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AcerAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = phone.QueryParameters,
					DestinationFolder = ((!string.IsNullOrEmpty(phone.QueryParameters.ManufacturerHardwareModel)) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetAcerProductsPath(phone.QueryParameters.ManufacturerHardwareModel) : Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetAcerProductsPath(phone.QueryParameters.ManufacturerHardwareVariant)),
					FilesVersioned = true
				};
				Tracer<AcerAdaptation>.WriteInformation("Download Params: {0}", new object[]
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
				Tracer<AcerAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<AcerAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E10 File Offset: 0x00001010
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E17 File Offset: 0x00001017
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002E1A File Offset: 0x0000101A
		public override void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002E24 File Offset: 0x00001024
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "F0CA"));
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("0502", "37A3"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002EF8 File Offset: 0x000010F8
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

		// Token: 0x0600002A RID: 42 RVA: 0x00002F3C File Offset: 0x0000113C
		private Phone GetPhone(ModelInfo modelInfo)
		{
			return new Phone
			{
				Type = this.PhoneType,
				SalesName = modelInfo.Name,
				HardwareModel = modelInfo.Name,
				ImageData = modelInfo.Bitmap.ToBytes()
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030A0 File Offset: 0x000012A0
		protected override void InitializeManuallySupportedModels()
		{
			var array = new <>f__AnonymousType0<ModelInfo, string>[]
			{
				new
				{
					Model = AcerModels.LiquidM220,
					IdentificationInstruction = LocalizationManager.GetTranslation("ModelIdentificationAcerM220")
				},
				new
				{
					Model = AcerModels.JadePrimo,
					IdentificationInstruction = null
				},
				new
				{
					Model = AcerModels.LiquidM330,
					IdentificationInstruction = LocalizationManager.GetTranslation("ModelIdentificationAcerM330")
				}
			};
			var array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				var <>f__AnonymousType = array2[i];
				ModelInfo model = <>f__AnonymousType.Model;
				Phone phone = this.GetPhone(<>f__AnonymousType.Model);
				phone.ModelIdentificationInstruction = <>f__AnonymousType.IdentificationInstruction;
				this.manuallySupportedModels.Add(phone);
				if (model.Variants.Length == 1)
				{
					phone.QueryParameters = model.Variants[0].MsrQueryParameters;
				}
				else
				{
					Phone[] phoneVariants = this.GetPhoneVariants(model);
					this.manuallySupportedVariants.AddRange(phoneVariants);
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003175 File Offset: 0x00001375
		public override List<Phone> ManuallySupportedModels()
		{
			return this.manuallySupportedModels;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000031A0 File Offset: 0x000013A0
		public override List<Phone> ManuallySupportedVariants(Phone phone)
		{
			return (from variant in this.manuallySupportedVariants
			where variant.HardwareModel.Equals(phone.HardwareModel, StringComparison.OrdinalIgnoreCase)
			select variant).ToList<Phone>();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003200 File Offset: 0x00001400
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.SalesName.ToLower().Contains("m220"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("M220.png"));
			}
			else if (phone.SalesName.ToLower().Contains("jade primo"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourcesName) => resourcesName.Contains("JadePrimo.png"));
			}
			else if (phone.SalesName.ToLower().Contains("m330"))
			{
				text = manifestResourceNames.FirstOrDefault((string resourcesName) => resourcesName.Contains("M330.png"));
			}
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return base.GetImageDataStream(phone);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000032FC File Offset: 0x000014FC
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("AcerLogo.png"));
			if (!string.IsNullOrEmpty(text))
			{
				return executingAssembly.GetManifestResourceStream(text);
			}
			return null;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000334B File Offset: 0x0000154B
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003378 File Offset: 0x00001578
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<AcerAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<AcerAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000033C4 File Offset: 0x000015C4
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x04000013 RID: 19
		private readonly MsrService msrService;

		// Token: 0x04000014 RID: 20
		private readonly ReportingService reportingService;

		// Token: 0x04000015 RID: 21
		private readonly List<Phone> manuallySupportedModels = new List<Phone>();

		// Token: 0x04000016 RID: 22
		private readonly List<Phone> manuallySupportedVariants = new List<Phone>();

		// Token: 0x04000017 RID: 23
		private string progressMessage;
	}
}
