using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Connectivity;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services
{
	// Token: 0x02000007 RID: 7
	[Export]
	public class LumiaAdaptation : BaseAdaptation
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000043AC File Offset: 0x000025AC
		[ImportingConstructor]
		public LumiaAdaptation(DataPackageService dataPackageService, Thor2Service thor2Service, MsrService msrService, ReportingService reportingService)
		{
			this.msrService = msrService;
			this.salesNameProvider = new SalesNameProvider();
			this.dataPackageService = dataPackageService;
			this.thor2Service = thor2Service;
			this.reportingService = reportingService;
			this.dataPackageService.DownloadProgressChanged += this.DataPackageServiceDownloadProgressEvent;
			this.msrService.ProgressChanged += this.MsrDownloadProgressEvent;
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000041 RID: 65 RVA: 0x0000441C File Offset: 0x0000261C
		// (remove) Token: 0x06000042 RID: 66 RVA: 0x00004458 File Offset: 0x00002658
		public event Action<Phone> DeviceConnected;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000043 RID: 67 RVA: 0x00004494 File Offset: 0x00002694
		// (remove) Token: 0x06000044 RID: 68 RVA: 0x000044D0 File Offset: 0x000026D0
		public event Action<Phone> DeviceDisconnected;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000045 RID: 69 RVA: 0x0000450C File Offset: 0x0000270C
		// (remove) Token: 0x06000046 RID: 70 RVA: 0x00004548 File Offset: 0x00002748
		public event Action<Phone> DeviceReadyChanged;

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00004584 File Offset: 0x00002784
		public override string PackageExtension
		{
			get
			{
				return "vpl";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000048 RID: 72 RVA: 0x0000459C File Offset: 0x0000279C
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Lumia;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000045B0 File Offset: 0x000027B0
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000045C4 File Offset: 0x000027C4
		public override string ManufacturerName
		{
			get
			{
				return "Lumia";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000045DC File Offset: 0x000027DC
		public override string ReportManufacturerName
		{
			get
			{
				return "Microsoft";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000045F4 File Offset: 0x000027F4
		public override string ReportManufacturerProductLine
		{
			get
			{
				return "Lumia";
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000460C File Offset: 0x0000280C
		public override ISalesNameProvider SalesNameProvider()
		{
			return this.salesNameProvider;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004624 File Offset: 0x00002824
		public override void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000462C File Offset: 0x0000282C
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.FillNormalModeDeviceIdentifiers();
			this.FillRecoveryModeDeviceIdentifiers();
			this.FillEmergencyModeDeviceIdentifiers();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004644 File Offset: 0x00002844
		private void FillEmergencyModeDeviceIdentifiers()
		{
			this.SupportedEmergencyModeIds.Add(new DeviceIdentifier("05C6", "9008"));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004664 File Offset: 0x00002864
		private void FillRecoveryModeDeviceIdentifiers()
		{
			this.SupportedRecoveryModeIds.Add(new DeviceIdentifier("0421", "066E"));
			this.SupportedRecoveryModeIds.Add(new DeviceIdentifier("0421", "0714"));
			this.SupportedRecoveryModeIds.Add(new DeviceIdentifier("045E", "0A02"));
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000046C4 File Offset: 0x000028C4
		private void FillNormalModeDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("0421", "0661", new int[]
			{
				2,
				3
			}));
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("0421", "06FC"));
			this.SupportedNormalModeIds.Add(new DeviceIdentifier("045E", "0A00"));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004734 File Offset: 0x00002934
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			Tracer<LumiaAdaptation>.LogEntry("FindPackage");
			List<PackageFileInfo> list = new List<PackageFileInfo>();
			try
			{
				List<VariantInfo> list2 = this.dataPackageService.FindLocalVariants();
				Tracer<LumiaAdaptation>.WriteInformation("Selecting variants for Product Type: {0}", new object[]
				{
					currentPhone.HardwareModel
				});
				List<VplPackageFileInfo> list3 = new List<VplPackageFileInfo>();
				foreach (VariantInfo variantInfo in list2)
				{
					if (variantInfo.ProductType == currentPhone.HardwareModel && variantInfo.ProductCode == currentPhone.HardwareVariant)
					{
						VplPackageFileInfo item = new VplPackageFileInfo(variantInfo.Path, variantInfo);
						list3.Add(item);
					}
				}
				list.AddRange(list3);
			}
			catch (Exception error)
			{
				Tracer<LumiaAdaptation>.WriteError(error, "Error while searching for packages", new object[0]);
				throw;
			}
			Tracer<LumiaAdaptation>.LogExit("FindPackage");
			return list;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004858 File Offset: 0x00002A58
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			Tracer<LumiaAdaptation>.LogEntry("FindAllPackages");
			List<PackageFileInfo> list = new List<PackageFileInfo>();
			try
			{
				List<VariantInfo> list2 = new List<VariantInfo>();
				this.dataPackageService.FindLocalVariants(list2, directory);
				List<VplPackageFileInfo> list3 = new List<VplPackageFileInfo>();
				foreach (VariantInfo variantInfo in list2)
				{
					VplPackageFileInfo vplPackageFileInfo = new VplPackageFileInfo(variantInfo.Path, variantInfo);
					if (File.Exists(vplPackageFileInfo.FfuFilePath))
					{
						list3.Add(vplPackageFileInfo);
					}
				}
				list.AddRange(list3);
			}
			catch (Exception error)
			{
				Tracer<LumiaAdaptation>.WriteError(error, "Error while searching for packages", new object[0]);
				throw;
			}
			Tracer<LumiaAdaptation>.LogExit("FindAllPackages");
			return list;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004948 File Offset: 0x00002B48
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LumiaAdaptation>.LogEntry("CheckPackageIntegrity");
			base.RaiseProgressPercentageChanged(0, null);
			this.dataPackageService.IntegrityCheckProgressEvent += this.DataPackageServiceIntegrityCheckProgressEvent;
			try
			{
				this.dataPackageService.CheckVariantIntegrity(phone.PackageFilePath, cancellationToken);
			}
			finally
			{
				this.dataPackageService.IntegrityCheckProgressEvent -= this.DataPackageServiceIntegrityCheckProgressEvent;
			}
			Tracer<LumiaAdaptation>.LogExit("CheckPackageIntegrity");
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000049D0 File Offset: 0x00002BD0
		private void DataPackageServiceIntegrityCheckProgressEvent(int percentage)
		{
			base.RaiseProgressPercentageChanged(percentage, null);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000049DC File Offset: 0x00002BDC
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return this.thor2Service.IsDeviceConnected(phone, cancellationToken);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004A20 File Offset: 0x00002C20
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LumiaAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = this.DeviceQueryParameters(phone),
					DestinationFolder = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetLumiaProductsPath(phone.HardwareModel)
				};
				List<string> source = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken);
				if (source.Any<string>())
				{
					string packageFilePath = source.FirstOrDefault((string file) => file.ToLower().EndsWith(".vpl"));
					phone.PackageFilePath = packageFilePath;
				}
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationSucceded(phone, ReportOperationType.DownloadPackage);
			}
			catch (Exception ex)
			{
				if (ex.InnerException is AggregateException && ex.InnerException.InnerException is IOException && (long)ex.InnerException.InnerException.HResult == -2147024784L)
				{
					Tracer<LumiaAdaptation>.WriteInformation("--100: For some reason the exception wasn't thrown until here.");
					throw new NotEnoughSpaceException();
				}
				UriData uriDataForException = this.GetUriDataForException(ex);
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				this.reportingService.SetDownloadByteInformation(phone, ReportOperationType.DownloadPackage, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
				this.reportingService.OperationFailed(phone, ReportOperationType.DownloadPackage, uriDataForException, ex);
				Tracer<LumiaAdaptation>.WriteError(ex);
				throw;
			}
			finally
			{
				Tracer<LumiaAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004C00 File Offset: 0x00002E00
		private void GetDownloadInformationToReport(Phone phone, ReportOperationType operationType)
		{
			Tuple<long, long, bool> downloadPackageInformation = this.dataPackageService.GetDownloadPackageInformation();
			this.reportingService.SetDownloadByteInformation(phone, operationType, downloadPackageInformation.Item1, downloadPackageInformation.Item2, downloadPackageInformation.Item3);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004C3C File Offset: 0x00002E3C
		public override void DownloadEmergencyPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<LumiaAdaptation>.LogEntry("DownloadEmergencyPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				string destinationPath = string.IsNullOrWhiteSpace(phone.PackageFilePath) ? Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetLumiaProductsPath(phone.HardwareModel) : Path.GetDirectoryName(phone.PackageFilePath);
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadEmergencyPackage);
				this.dataPackageService.DownloadProgressChanged += this.DataPackageServiceDownloadProgressEvent;
				EmergencyPackageInfo emergencyPackageFileInfo = this.dataPackageService.DownloadEmergencyPackage(phone.HardwareModel, destinationPath, cancellationToken);
				this.GetDownloadInformationToReport(phone, ReportOperationType.DownloadEmergencyPackage);
				this.reportingService.OperationSucceded(phone, ReportOperationType.DownloadEmergencyPackage);
				phone.EmergencyPackageFileInfo = emergencyPackageFileInfo;
			}
			catch (Exception ex)
			{
				if (ex.InnerException is AggregateException && ex.InnerException.InnerException is IOException && (long)ex.InnerException.InnerException.HResult == -2147024784L)
				{
					Tracer<LumiaAdaptation>.WriteInformation("--100: For some reason the exception wasn't thrown until here.");
					throw new NotEnoughSpaceException();
				}
				Tracer<LumiaAdaptation>.WriteError(ex);
				if (ex.GetBaseException() is WebException && (long)ex.GetBaseException().HResult == -2146233079L)
				{
					this.reportingService.OperationFailed(phone, ReportOperationType.DownloadEmergencyPackage, UriData.EmergencyFlashFilesNotFoundOnServer, ex);
					throw new EmergencyPackageNotFoundOnServerException();
				}
				this.GetDownloadInformationToReport(phone, ReportOperationType.DownloadEmergencyPackage);
				UriData uriDataForException = this.GetUriDataForException(ex);
				this.reportingService.OperationFailed(phone, ReportOperationType.DownloadEmergencyPackage, uriDataForException, ex);
				throw;
			}
			finally
			{
				this.dataPackageService.DownloadProgressChanged -= this.DataPackageServiceDownloadProgressEvent;
				Tracer<LumiaAdaptation>.LogExit("DownloadEmergencyPackage");
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004E18 File Offset: 0x00003018
		private UriData GetUriDataForException(Exception ex)
		{
			UriData result;
			if (ex is OperationCanceledException || ex.GetBaseException() is TaskCanceledException)
			{
				result = UriData.DownloadVariantPackageAbortedByUser;
			}
			else if (ex is Crc32Exception)
			{
				result = UriData.FailedToDownloadVariantPackageCrc32Failed;
			}
			else if (ex is NotEnoughSpaceException)
			{
				result = UriData.DownloadVariantPackageFilesFailedBecauseOfInsufficientDiskSpace;
			}
			else if (ex is PlannedServiceBreakException)
			{
				result = UriData.FailedToDownloadVariantPackageFireServiceBreak;
			}
			else
			{
				result = UriData.FailedToDownloadVariantPackage;
			}
			return result;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004E9D File Offset: 0x0000309D
		private void DataPackageServiceDownloadProgressEvent(DownloadingProgressChangedEventArgs args)
		{
			base.RaiseProgressPercentageChanged(args.Percentage, args.Message, args.DownloadedSize, args.TotalSize, args.BytesPerSecond, args.SecondsLeft);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004ECC File Offset: 0x000030CC
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			this.thor2Service.ProgressChanged += this.Thor2ServiceOnProgressChanged;
			try
			{
				this.thor2Service.FlashDevice(phone, cancellationToken);
			}
			finally
			{
				this.thor2Service.ProgressChanged -= this.Thor2ServiceOnProgressChanged;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004F30 File Offset: 0x00003130
		public void EmergencyFlashDevice(Phone phone, CancellationToken token)
		{
			this.thor2Service.ProgressChanged += this.Thor2ServiceOnProgressChanged;
			try
			{
				this.thor2Service.EmergencyFlashDevice(phone, token);
			}
			finally
			{
				this.thor2Service.ProgressChanged -= this.Thor2ServiceOnProgressChanged;
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004F94 File Offset: 0x00003194
		private void Thor2ServiceOnProgressChanged(ProgressChangedEventArgs progressChangedEventArgs)
		{
			base.RaiseProgressPercentageChanged(progressChangedEventArgs);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004FA0 File Offset: 0x000031A0
		public void Initialize()
		{
			this.lumiaConnectivity = new LumiaConnectivity();
			this.lumiaConnectivity.DeviceConnected += this.LumiaConnectivityDeviceConnected;
			this.lumiaConnectivity.DeviceDisconnected += this.LumiaConnectivityDeviceDisconnected;
			this.lumiaConnectivity.DeviceReadyChanged += this.LumiaConnectivityDeviceReadyChanged;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005004 File Offset: 0x00003204
		public void StartDetection(DetectionType detectionType)
		{
			this.Initialize();
			switch (detectionType)
			{
			case DetectionType.NormalMode:
				this.lumiaConnectivity.Start(this.SupportedNormalModeIds);
				break;
			case DetectionType.RecoveryMode:
			case DetectionType.RecoveryModeAfterEmergencyFlashing:
			{
				List<DeviceIdentifier> deviceIdentifiers = this.SupportedEmergencyModeIds.Union(this.SupportedRecoveryModeIds).ToList<DeviceIdentifier>();
				this.lumiaConnectivity.Start(deviceIdentifiers);
				break;
			}
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005074 File Offset: 0x00003274
		public void StopDetection()
		{
			if (this.lumiaConnectivity != null)
			{
				this.lumiaConnectivity.Stop();
				this.lumiaConnectivity.DeviceConnected -= this.LumiaConnectivityDeviceConnected;
				this.lumiaConnectivity.DeviceDisconnected -= this.LumiaConnectivityDeviceDisconnected;
				this.lumiaConnectivity.DeviceReadyChanged -= this.LumiaConnectivityDeviceReadyChanged;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000050E8 File Offset: 0x000032E8
		public ReadOnlyCollection<Phone> GetAllPhones()
		{
			return new ReadOnlyCollection<Phone>(this.lumiaConnectivity.GetAllConnectedDevices().Select(new Func<ConnectedDevice, Phone>(this.CreatePhoneObjectForDevice)).ToList<Phone>());
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005120 File Offset: 0x00003320
		private Phone CreatePhoneObjectForDevice(ConnectedDevice device)
		{
			return new Phone(device.PortId, device.Vid, device.Pid, device.DevicePath, device.TypeDesignator, string.Empty, device.SalesName, string.Empty, device.Path, this.PhoneType, device.InstanceId, this.SalesNameProvider(), device.DeviceReady, "", "");
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005190 File Offset: 0x00003390
		public void TryReadMissingInfoWithThor(Phone phone, CancellationToken token)
		{
			this.thor2Service.TryReadMissingInfoWithThor(phone, token, false);
			phone.DeviceReady = true;
			if (string.IsNullOrEmpty(phone.HardwareVariant))
			{
				this.reportingService.OperationFailed(phone, ReportOperationType.ReadDeviceInfoWithThor, UriData.ProductCodeReadFailed, new ReadPhoneInformationException("Could not read product code"));
			}
			else if (string.IsNullOrEmpty(phone.HardwareModel))
			{
				this.reportingService.OperationFailed(phone, ReportOperationType.ReadDeviceInfoWithThor, UriData.CouldNotReadProductType, new ReadPhoneInformationException("Could not read product type"));
			}
			else
			{
				this.reportingService.OperationSucceded(phone, ReportOperationType.ReadDeviceInfoWithThor);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005228 File Offset: 0x00003428
		private void LumiaConnectivityDeviceConnected(object sender, DeviceConnectedEventArgs args)
		{
			Action<Phone> deviceConnected = this.DeviceConnected;
			if (deviceConnected != null)
			{
				deviceConnected(this.CreatePhoneObjectForDevice(args.ConnectedDevice));
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000525C File Offset: 0x0000345C
		private void LumiaConnectivityDeviceDisconnected(object sender, DeviceConnectedEventArgs args)
		{
			Action<Phone> deviceDisconnected = this.DeviceDisconnected;
			if (deviceDisconnected != null)
			{
				deviceDisconnected(this.CreatePhoneObjectForDevice(args.ConnectedDevice));
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005290 File Offset: 0x00003490
		private void LumiaConnectivityDeviceReadyChanged(object sender, DeviceReadyChangedEventArgs args)
		{
			Action<Phone> deviceReadyChanged = this.DeviceReadyChanged;
			if (deviceReadyChanged != null)
			{
				Phone phone = this.CreatePhoneObjectForDevice(args.ConnectedDevice);
				phone.DeviceReady = args.DeviceReady;
				deviceReadyChanged(phone);
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000052D4 File Offset: 0x000034D4
		public void FillLumiaDeviceInfo(Phone phone, CancellationToken token)
		{
			this.lumiaConnectivity.FillLumiaDeviceInfo(phone, token);
			if (!phone.IsProductCodeTypeEmpty())
			{
				phone.ReportManufacturerName = this.ReportManufacturerName;
				phone.ReportManufacturerProductLine = this.ReportManufacturerProductLine;
				this.reportingService.OperationSucceded(phone, ReportOperationType.ReadDeviceInfo);
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005324 File Offset: 0x00003524
		public override int ReadBatteryLevel(Phone phone)
		{
			return this.lumiaConnectivity.ReadBatteryLevel(phone);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00005342 File Offset: 0x00003542
		protected override void ReleaseManagedObjects()
		{
			this.thor2Service.ReleaseManagedObjects();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005354 File Offset: 0x00003554
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			PackageFileInfo result;
			try
			{
				Tracer<LumiaAdaptation>.LogEntry("CheckLatestPackage");
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = this.DeviceQueryParameters(phone),
					FileExtension = "vpl",
					DestinationFolder = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetLumiaProductsPath(phone.HardwareModel)
				};
				List<string> source = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken);
				if (!source.Any<string>())
				{
					throw new PackageNotFoundException();
				}
				string text = source.First<string>();
				result = new VplPackageFileInfo(text, VariantInfo.GetVariantInfo(text));
			}
			catch (Exception ex)
			{
				if (ex.InnerException is PackageNotFoundException)
				{
					this.reportingService.OperationFailed(phone, ReportOperationType.CheckPackage, UriData.NoPackageFound, ex.InnerException);
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
				Tracer<LumiaAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000054C0 File Offset: 0x000036C0
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			string softwareVersion = phone.SoftwareVersion;
			return VersionComparer.CompareSoftwareVersions(softwareVersion, phone.PackageFileInfo.SoftwareVersion, new char[]
			{
				'.'
			});
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005530 File Offset: 0x00003730
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = string.Empty;
			if (phone.Vid.ToUpper() == "0421")
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("NokiaLumia.png"));
			}
			else if (phone.Vid.ToUpper() == "045E")
			{
				text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("MicrosoftLumia.png"));
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

		// Token: 0x0600006F RID: 111 RVA: 0x00005620 File Offset: 0x00003820
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("LumiaLogo.png"));
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

		// Token: 0x06000070 RID: 112 RVA: 0x00005680 File Offset: 0x00003880
		private QueryParameters DeviceQueryParameters(Phone phone)
		{
			return new QueryParameters
			{
				ManufacturerName = "Microsoft",
				ManufacturerProductLine = "Lumia",
				PackageType = "Firmware",
				PackageClass = "Public",
				ManufacturerHardwareModel = phone.HardwareModel,
				ManufacturerHardwareVariant = phone.HardwareVariant
			};
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000056E3 File Offset: 0x000038E3
		private void MsrDownloadProgressEvent(ProgressChangedEventArgs args)
		{
			base.RaiseProgressPercentageChanged(args.Percentage, args.Message, args.DownloadedSize, args.TotalSize, args.BytesPerSecond, args.SecondsLeft);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005714 File Offset: 0x00003914
		public override BatteryStatus ReadBatteryStatus(Phone phone)
		{
			try
			{
				int num = this.ReadBatteryLevel(phone);
				Tracer<LumiaAdaptation>.WriteInformation("Battery level: ", new object[]
				{
					num
				});
				if (num >= 25)
				{
					return BatteryStatus.BatteryOk;
				}
				if (num != -1)
				{
					return BatteryStatus.BatteryNotOkDoNotBlock;
				}
			}
			catch (Exception ex)
			{
				Tracer<LumiaAdaptation>.WriteError("Cannot read battery level!: ", new object[]
				{
					ex
				});
			}
			return BatteryStatus.BatteryUnknown;
		}

		// Token: 0x04000025 RID: 37
		private const long ErrorEmptyDiskSpace = -2147024784L;

		// Token: 0x04000026 RID: 38
		private const long ErrorNotFound = -2146233079L;

		// Token: 0x04000027 RID: 39
		private readonly DataPackageService dataPackageService;

		// Token: 0x04000028 RID: 40
		private readonly Thor2Service thor2Service;

		// Token: 0x04000029 RID: 41
		private readonly ReportingService reportingService;

		// Token: 0x0400002A RID: 42
		private readonly SalesNameProvider salesNameProvider;

		// Token: 0x0400002B RID: 43
		private readonly MsrService msrService;

		// Token: 0x0400002C RID: 44
		private LumiaConnectivity lumiaConnectivity;
	}
}
