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

namespace Microsoft.WindowsDeviceRecoveryTool.LgeAdaptation.Services
{
	// Token: 0x02000005 RID: 5
	[ExportAdaptation(Type = PhoneTypes.Lg)]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class LgeAdaptation : BaseAdaptation
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002360 File Offset: 0x00000560
		[ImportingConstructor]
		public LgeAdaptation(FfuFileInfoService ffuFileInfoService, MsrService msrService, ReportingService reportingService)
		{
			this.salesNameProvider = new SalesNameProvider();
			this.reportingService = reportingService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.msrService = msrService;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000023B0 File Offset: 0x000005B0
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Lg;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000023C4 File Offset: 0x000005C4
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000023D8 File Offset: 0x000005D8
		public override string ManufacturerName
		{
			get
			{
				return "LG";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000023F0 File Offset: 0x000005F0
		public override string ReportManufacturerName
		{
			get
			{
				return "LGE";
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002408 File Offset: 0x00000608
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002420 File Offset: 0x00000620
		public override ISalesNameProvider SalesNameProvider()
		{
			return this.salesNameProvider;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002438 File Offset: 0x00000638
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002440 File Offset: 0x00000640
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002448 File Offset: 0x00000648
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<LgeAdaptation>.LogEntry("CheckLatestPackage");
				Task<PackageFileInfo> task = this.msrService.CheckLatestPackage(this.DeviceQueryParameters(phone), cancellationToken);
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
				Tracer<LgeAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002528 File Offset: 0x00000728
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LgeAdaptation>.LogEntry("CheckPackageIntegrity");
			this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
			Tracer<LgeAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002554 File Offset: 0x00000754
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LgeAdaptation>.LogEntry("FlashDevice");
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
			Tracer<LgeAdaptation>.LogExit("FlashDevice");
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000025E8 File Offset: 0x000007E8
		private void FlashProgressEvent(object obj, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002620 File Offset: 0x00000820
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.GetFfuDevice(phone) != null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002640 File Offset: 0x00000840
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LgeAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = this.DeviceQueryParameters(phone),
					DestinationFolder = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetLgeProductsPath(phone.PlatformId.ProductName),
					FilesVersioned = true
				};
				Tracer<LgeAdaptation>.WriteInformation("Download Params: {0}", new object[]
				{
					downloadParameters
				});
				phone.PackageFiles = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken);
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
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
				Tracer<LgeAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<LgeAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027FC File Offset: 0x000009FC
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002804 File Offset: 0x00000A04
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			return SwVersionComparisonResult.UnableToCompare;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002818 File Offset: 0x00000A18
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
				Tracer<LgeAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000028B8 File Offset: 0x00000AB8
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("1004", "627E"));
			this.SupportedFlashModeIds.Add(new DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028F4 File Offset: 0x00000AF4
		private QueryParameters DeviceQueryParameters(Phone phone)
		{
			return new QueryParameters
			{
				ManufacturerName = "LGE",
				ManufacturerHardwareModel = phone.PlatformId.ProductName,
				PackageType = "Firmware"
			};
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002937 File Offset: 0x00000B37
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs progressArgs)
		{
			base.RaiseProgressPercentageChanged(progressArgs.Percentage, progressArgs.Message, progressArgs.DownloadedSize, progressArgs.TotalSize, progressArgs.BytesPerSecond, progressArgs.SecondsLeft);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002968 File Offset: 0x00000B68
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<LgeAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice flashableDevice;
			try
			{
				flashableDevice = FFUManager.GetFlashableDevice(phone.Path, false);
			}
			finally
			{
				FFUManager.Stop();
				Tracer<LgeAdaptation>.LogExit("GetFfuDevice");
			}
			return flashableDevice;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029DC File Offset: 0x00000BDC
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Lancet.png"));
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

		// Token: 0x06000024 RID: 36 RVA: 0x00002A5C File Offset: 0x00000C5C
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("LgLogo.png"));
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

		// Token: 0x04000006 RID: 6
		private readonly MsrService msrService;

		// Token: 0x04000007 RID: 7
		private readonly ReportingService reportingService;

		// Token: 0x04000008 RID: 8
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000009 RID: 9
		private readonly SalesNameProvider salesNameProvider;

		// Token: 0x0400000A RID: 10
		private string progressMessage;
	}
}
