using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services
{
	// Token: 0x02000003 RID: 3
	[Export(typeof(IManufacturerDataProvider))]
	[Export(typeof(AdaptationManager))]
	public class AdaptationManager : IDisposable, IManufacturerDataProvider
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000024AF File Offset: 0x000006AF
		[ImportingConstructor]
		public AdaptationManager(ManufacturerAutodetectionService manufacturerAutodetectionService, ReportingService reportingService)
		{
			this.manufacturerAutodetectionService = manufacturerAutodetectionService;
			this.reportingService = reportingService;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000013 RID: 19 RVA: 0x000024D4 File Offset: 0x000006D4
		// (remove) Token: 0x06000014 RID: 20 RVA: 0x00002510 File Offset: 0x00000710
		public event Action<ProgressChangedEventArgs> ProgressChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000015 RID: 21 RVA: 0x0000254C File Offset: 0x0000074C
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x00002588 File Offset: 0x00000788
		public event Action<Phone> DeviceConnected;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000017 RID: 23 RVA: 0x000025C4 File Offset: 0x000007C4
		// (remove) Token: 0x06000018 RID: 24 RVA: 0x00002600 File Offset: 0x00000800
		public event Action<Phone> DeviceDisconnected;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000019 RID: 25 RVA: 0x0000263C File Offset: 0x0000083C
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x00002678 File Offset: 0x00000878
		public event Action<Phone> DeviceEndpointConnected;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600001B RID: 27 RVA: 0x000026B4 File Offset: 0x000008B4
		// (remove) Token: 0x0600001C RID: 28 RVA: 0x000026F0 File Offset: 0x000008F0
		public event Action<Phone> DeviceInfoRead;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600001D RID: 29 RVA: 0x0000272C File Offset: 0x0000092C
		// (remove) Token: 0x0600001E RID: 30 RVA: 0x00002768 File Offset: 0x00000968
		public event Action<Phone> DeviceBatteryLevelRead;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600001F RID: 31 RVA: 0x000027A4 File Offset: 0x000009A4
		// (remove) Token: 0x06000020 RID: 32 RVA: 0x000027E0 File Offset: 0x000009E0
		public event Action<BatteryStatus> DeviceBatteryStatusRead;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000021 RID: 33 RVA: 0x0000281C File Offset: 0x00000A1C
		// (remove) Token: 0x06000022 RID: 34 RVA: 0x00002858 File Offset: 0x00000A58
		public event Action<bool> DeviceConnectionStatusRead;

		// Token: 0x06000023 RID: 35 RVA: 0x00002894 File Offset: 0x00000A94
		internal void AddAdaptation(BaseAdaptation adaptationService)
		{
			this.adaptationServices.Add(adaptationService);
			adaptationService.ProgressChanged += this.RaiseProgressChanged;
			adaptationService.DeviceInfoRead += this.RaiseDeviceInfoRead;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028CC File Offset: 0x00000ACC
		private void RaiseDeviceInfoRead(Phone phone)
		{
			Action<Phone> deviceInfoRead = this.DeviceInfoRead;
			if (deviceInfoRead != null)
			{
				deviceInfoRead(phone);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000291C File Offset: 0x00000B1C
		public BaseAdaptation GetAdaptation(PhoneTypes selectedManufacturer)
		{
			return this.adaptationServices.FirstOrDefault((BaseAdaptation adaptationService) => adaptationService.PhoneType == selectedManufacturer);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000297C File Offset: 0x00000B7C
		public ReadOnlyCollection<BaseAdaptation> GetAdaptations(Predicate<BaseAdaptation> match)
		{
			return Array.AsReadOnly<BaseAdaptation>((from a in this.adaptationServices
			where match(a)
			select a).ToArray<BaseAdaptation>());
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029BC File Offset: 0x00000BBC
		public string GetAdaptationExtension(PhoneTypes phoneType)
		{
			return this.GetAdaptation(phoneType).PackageExtension;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A30 File Offset: 0x00000C30
		public List<ManufacturerInfo> GetAdaptationsData()
		{
			return (from a in this.adaptationServices
			orderby a.PhoneType
			select a into adaptationService
			select new ManufacturerInfo(adaptationService.PhoneType, adaptationService.RecoverySupport, adaptationService.ManufacturerName, adaptationService.GetManufacturerImageData(), adaptationService.ReportManufacturerName, adaptationService.ReportManufacturerProductLine)).ToList<ManufacturerInfo>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A98 File Offset: 0x00000C98
		public List<Phone> GetSupportedAdaptationModels(PhoneTypes phoneType)
		{
			BaseAdaptation adaptation = this.GetAdaptation(phoneType);
			List<Phone> result;
			if (adaptation != null)
			{
				result = adaptation.ManuallySupportedModels();
			}
			else
			{
				result = new List<Phone>();
			}
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002ACC File Offset: 0x00000CCC
		public List<PackageFileInfo> FindCorrectPackage(string directory, Phone phone, CancellationToken cancellationToken)
		{
			return this.GetAdaptation(phone.Type).FindPackage(directory, phone, cancellationToken);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public void CheckPackageIntegrity(Phone phone, CancellationToken token)
		{
			BaseAdaptation adaptation = this.GetAdaptation(phone.Type);
			adaptation.CheckPackageIntegrity(phone, token);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B18 File Offset: 0x00000D18
		public PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			BaseAdaptation adaptation = this.GetAdaptation(phone.Type);
			phone.ReportManufacturerName = adaptation.ReportManufacturerName;
			phone.ReportManufacturerProductLine = adaptation.ReportManufacturerProductLine;
			return adaptation.CheckLatestPackage(phone, cancellationToken);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B5C File Offset: 0x00000D5C
		private void RaiseProgressChanged(ProgressChangedEventArgs progressChangedEventArgs)
		{
			Action<ProgressChangedEventArgs> progressChanged = this.ProgressChanged;
			if (progressChanged != null)
			{
				progressChanged(progressChangedEventArgs);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B84 File Offset: 0x00000D84
		public void FlashDevice(Phone phone, DetectionType detectionType, CancellationToken token)
		{
			ReportOperationType reportOperationType;
			UriData resultUriData;
			this.GetRequiredValuesForDetectionType(detectionType, out reportOperationType, out resultUriData);
			try
			{
				this.reportingService.OperationStarted(phone, reportOperationType);
				this.GetAdaptation(phone.Type).FlashDevice(phone, token);
				this.reportingService.OperationSucceded(phone, reportOperationType);
			}
			catch (Exception ex)
			{
				if (ex is SoftwareIsNotCorrectlySignedException)
				{
					resultUriData = UriData.SoftwareNotCorrectlySigned;
				}
				else if (ex is CheckResetProtectionException)
				{
					resultUriData = UriData.ResetProtectionStatusIsIncorrect;
				}
				else if (ex is FlashModeChangeException)
				{
					resultUriData = UriData.ChangeToFlashModeFailed;
				}
				this.reportingService.OperationFailed(phone, reportOperationType, resultUriData, ex);
				throw;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C44 File Offset: 0x00000E44
		public void EmergencyFlashDevice(Phone phone, CancellationToken token)
		{
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.EmergencyFlashing);
				LumiaAdaptation lumiaAdaptation = this.GetAdaptation(PhoneTypes.Lumia) as LumiaAdaptation;
				lumiaAdaptation.EmergencyFlashDevice(phone, token);
				this.reportingService.PartialOperationSucceded(phone, ReportOperationType.EmergencyFlashing, UriData.EmergencyFlashingSuccesfullyFinished);
			}
			catch (Exception ex)
			{
				this.reportingService.OperationFailed(phone, ReportOperationType.EmergencyFlashing, UriData.EmergencyFlashingFailed, ex);
				throw;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CB8 File Offset: 0x00000EB8
		private void GetRequiredValuesForDetectionType(DetectionType detectionType, out ReportOperationType operationType, out UriData failUriData)
		{
			switch (detectionType)
			{
			case DetectionType.NormalMode:
				operationType = ReportOperationType.Flashing;
				failUriData = UriData.ProgrammingPhoneFailed;
				break;
			case DetectionType.RecoveryMode:
				operationType = ReportOperationType.Recovery;
				failUriData = UriData.DeadPhoneRecoveryFailed;
				break;
			case DetectionType.RecoveryModeAfterEmergencyFlashing:
				operationType = ReportOperationType.EmergencyFlashing;
				failUriData = UriData.RecoveryAfterEmergencyFlashingFailed;
				break;
			default:
				throw new InvalidOperationException("Detection type not supported. Currently there is only Normal mode and Recovery mode detection allowed.");
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002D0C File Offset: 0x00000F0C
		public bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken token)
		{
			return this.GetAdaptation(phone.Type).IsDeviceInFlashModeConnected(phone, token);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002D34 File Offset: 0x00000F34
		public void DownloadPackage(Phone phone, CancellationToken token)
		{
			try
			{
				this.GetAdaptation(phone.Type).DownloadPackage(phone, token);
			}
			catch (DownloadPackageException ex)
			{
				if (ex.InnerException is WebException)
				{
					throw ex.InnerException;
				}
				AggregateException ex2 = ex.InnerException as AggregateException;
				if (ex2 != null)
				{
					throw ex2.Flatten().InnerException;
				}
				throw;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002DB0 File Offset: 0x00000FB0
		public SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			SwVersionComparisonResult result;
			if (phone != null)
			{
				BaseAdaptation adaptation = this.GetAdaptation(phone.Type);
				result = adaptation.CompareFirmwareVersions(phone);
			}
			else
			{
				Tracer<AdaptationManager>.WriteError("Current phone is NULL", new object[0]);
				result = SwVersionComparisonResult.UnableToCompare;
			}
			return result;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002DF4 File Offset: 0x00000FF4
		public void StartDevicesAutodetection(DetectionParameters detectionParams)
		{
			this.detectionParameters = detectionParams;
			this.manufacturerAutodetectionService.DeviceConnected += this.AutodetectionServiceDeviceConnected;
			this.manufacturerAutodetectionService.DeviceDisconnected += this.AutodetectionServiceDeviceDisconnected;
			this.manufacturerAutodetectionService.DeviceEndpointConnected += this.AutodetectionServiceDeviceEndpointConnected;
			switch (detectionParams.PhoneModes)
			{
			case PhoneModes.Normal:
				this.manufacturerAutodetectionService.Start(this.GetNormalModeSupportedIdentifiers());
				break;
			case PhoneModes.Flash:
				this.manufacturerAutodetectionService.Start(this.GetFlashModeSupportedIdentifiers());
				break;
			case PhoneModes.All:
			{
				Collection<DeviceIdentifier> normalModeSupportedIdentifiers = this.GetNormalModeSupportedIdentifiers();
				Collection<DeviceIdentifier> flashModeSupportedIdentifiers = this.GetFlashModeSupportedIdentifiers();
				IEnumerable<DeviceIdentifier> enumerable = normalModeSupportedIdentifiers.Concat(flashModeSupportedIdentifiers);
				this.manufacturerAutodetectionService.Start((Collection<DeviceIdentifier>)enumerable);
				break;
			}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002EC0 File Offset: 0x000010C0
		public void StopDevicesAutodetection()
		{
			this.manufacturerAutodetectionService.Stop();
			this.manufacturerAutodetectionService.DeviceConnected -= this.AutodetectionServiceDeviceConnected;
			this.manufacturerAutodetectionService.DeviceDisconnected -= this.AutodetectionServiceDeviceDisconnected;
			this.manufacturerAutodetectionService.DeviceEndpointConnected -= this.AutodetectionServiceDeviceEndpointConnected;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F24 File Offset: 0x00001124
		public List<Phone> GetConnectedPhones(DetectionParameters detectionParams)
		{
			List<Phone> list = new List<Phone>();
			ReadOnlyCollection<UsbDevice> connectedDevices = this.manufacturerAutodetectionService.GetConnectedDevices();
			foreach (UsbDevice usbDevice in connectedDevices)
			{
				IEnumerable<BaseAdaptation> source = this.MatchAdaptation(usbDevice, detectionParams);
				Phone phone = this.PhoneFromUsbDevice(usbDevice, source.ToList<BaseAdaptation>());
				if (phone != null)
				{
					list.Add(phone);
				}
			}
			return list;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002FDC File Offset: 0x000011DC
		private Phone PhoneFromUsbDevice(UsbDevice usbDevice, IList<BaseAdaptation> adaptations)
		{
			if (adaptations.Count<BaseAdaptation>() == 1)
			{
				BaseAdaptation baseAdaptation = adaptations.FirstOrDefault<BaseAdaptation>();
				if (baseAdaptation != null)
				{
					Phone phone = new Phone(usbDevice, baseAdaptation.PhoneType, baseAdaptation.SalesNameProvider(), false, "", "");
					phone.ImageData = baseAdaptation.GetImageData(phone);
					phone.ReportManufacturerName = baseAdaptation.ReportManufacturerName;
					phone.ReportManufacturerProductLine = baseAdaptation.ReportManufacturerProductLine;
					return phone;
				}
			}
			else if (adaptations.Count<BaseAdaptation>() > 1)
			{
				Phone phone2 = new Phone(usbDevice, PhoneTypes.UnknownWp, null, false, "", "");
				phone2.SalesName = "Unknown Windows Phone";
				phone2.MatchedAdaptationTypes = new List<PhoneTypes>(from a in adaptations
				select a.PhoneType);
				return phone2;
			}
			return null;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000031D8 File Offset: 0x000013D8
		private IEnumerable<BaseAdaptation> MatchAdaptation(UsbDevice usbDevice, DetectionParameters detectionParams)
		{
			List<BaseAdaptation> list = this.adaptationServices;
			if (detectionParams.PhoneTypes != PhoneTypes.All)
			{
				list = list.FindAll((BaseAdaptation a) => a.PhoneType == detectionParams.PhoneTypes);
			}
			IEnumerable<BaseAdaptation> result;
			switch (detectionParams.PhoneModes)
			{
			case PhoneModes.Normal:
				result = from a in list
				orderby a.PhoneType
				where a.IsSupportedInNormalMode(usbDevice)
				select a;
				break;
			case PhoneModes.Flash:
				result = from a in list
				orderby a.PhoneType
				where a.IsSupportedInFlashMode(usbDevice)
				select a;
				break;
			case PhoneModes.All:
			{
				IEnumerable<BaseAdaptation> enumerable = from a in list
				orderby a.PhoneType
				where a.IsSupportedInNormalMode(usbDevice)
				select a;
				IList<BaseAdaptation> list2 = (enumerable as IList<BaseAdaptation>) ?? enumerable.ToList<BaseAdaptation>();
				if (list2.Any<BaseAdaptation>())
				{
					result = list2;
				}
				else
				{
					result = from a in list
					orderby a.PhoneType
					where a.IsSupportedInFlashMode(usbDevice)
					select a;
				}
				break;
			}
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000033B0 File Offset: 0x000015B0
		private void AutodetectionServiceDeviceConnected(UsbDeviceEventArgs args)
		{
			Action<Phone> deviceConnected = this.DeviceConnected;
			if (deviceConnected != null)
			{
				IEnumerable<BaseAdaptation> source = this.MatchAdaptation(args.UsbDevice, this.detectionParameters);
				Phone obj = this.PhoneFromUsbDevice(args.UsbDevice, source.ToList<BaseAdaptation>()) ?? new Phone(args.UsbDevice, PhoneTypes.None, null, false, "", "");
				deviceConnected(obj);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000341C File Offset: 0x0000161C
		private void AutodetectionServiceDeviceDisconnected(UsbDeviceEventArgs args)
		{
			Action<Phone> deviceDisconnected = this.DeviceDisconnected;
			if (deviceDisconnected != null)
			{
				deviceDisconnected(new Phone(args.UsbDevice, PhoneTypes.None, null, false, "", ""));
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000345C File Offset: 0x0000165C
		private void AutodetectionServiceDeviceEndpointConnected(UsbDeviceEventArgs args)
		{
			Action<Phone> deviceEndpointConnected = this.DeviceEndpointConnected;
			if (deviceEndpointConnected != null)
			{
				IEnumerable<BaseAdaptation> source = this.MatchAdaptation(args.UsbDevice, this.detectionParameters);
				Phone phone = this.PhoneFromUsbDevice(args.UsbDevice, source.ToList<BaseAdaptation>());
				if (phone != null)
				{
					deviceEndpointConnected(phone);
				}
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000034F0 File Offset: 0x000016F0
		private Collection<DeviceIdentifier> GetNormalModeSupportedIdentifiers()
		{
			Collection<DeviceIdentifier> result;
			if (this.detectionParameters.PhoneTypes == PhoneTypes.All)
			{
				result = new Collection<DeviceIdentifier>(this.adaptationServices.SelectMany((BaseAdaptation adaptation) => adaptation.SupportedNormalModeIds).ToList<DeviceIdentifier>());
			}
			else
			{
				BaseAdaptation baseAdaptation = this.adaptationServices.Find((BaseAdaptation adaptation) => adaptation.PhoneType == this.detectionParameters.PhoneTypes);
				result = new Collection<DeviceIdentifier>(baseAdaptation.SupportedNormalModeIds);
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000035B0 File Offset: 0x000017B0
		private Collection<DeviceIdentifier> GetFlashModeSupportedIdentifiers()
		{
			Collection<DeviceIdentifier> result;
			if (this.detectionParameters.PhoneTypes == PhoneTypes.All)
			{
				result = new Collection<DeviceIdentifier>(this.adaptationServices.SelectMany((BaseAdaptation adaptation) => adaptation.SupportedFlashModeIds).ToList<DeviceIdentifier>());
			}
			else
			{
				BaseAdaptation baseAdaptation = this.adaptationServices.Find((BaseAdaptation adaptation) => adaptation.PhoneType == this.detectionParameters.PhoneTypes);
				result = new Collection<DeviceIdentifier>(baseAdaptation.SupportedFlashModeIds);
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003632 File Offset: 0x00001832
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003644 File Offset: 0x00001844
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.ReleaseManagedObjects();
				}
				this.ReleaseUnmanagedObjects();
				this.disposed = true;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003684 File Offset: 0x00001884
		protected virtual void ReleaseManagedObjects()
		{
			foreach (BaseAdaptation baseAdaptation in this.adaptationServices)
			{
				baseAdaptation.Dispose();
			}
			this.StopDevicesAutodetection();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000036E8 File Offset: 0x000018E8
		protected virtual void ReleaseUnmanagedObjects()
		{
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000036EC File Offset: 0x000018EC
		public List<PackageFileInfo> FindAllPackages(string directory, PhoneTypes phoneType, CancellationToken cancellationToken)
		{
			return this.GetAdaptation(phoneType).FindAllPackages(directory, cancellationToken);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000370C File Offset: 0x0000190C
		public void DownloadEmeregencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			try
			{
				this.GetAdaptation(currentPhone.Type).DownloadEmergencyPackage(currentPhone, cancellationToken);
			}
			catch (DownloadPackageException ex)
			{
				if (ex.InnerException is WebException)
				{
					throw ex.InnerException;
				}
				AggregateException ex2 = ex.InnerException as AggregateException;
				if (ex2 != null)
				{
					throw ex2.Flatten().InnerException;
				}
				throw;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003788 File Offset: 0x00001988
		public void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			this.GetAdaptation(currentPhone.Type).ReadDeviceInfo(currentPhone, cancellationToken);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000037A0 File Offset: 0x000019A0
		public void ReadDeviceBatteryLevel(Phone currentPhone, CancellationToken cancellationToken)
		{
			currentPhone.BatteryLevel = this.GetAdaptation(currentPhone.Type).ReadBatteryLevel(currentPhone);
			Action<Phone> deviceBatteryLevelRead = this.DeviceBatteryLevelRead;
			if (deviceBatteryLevelRead != null)
			{
				deviceBatteryLevelRead(currentPhone);
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000037E0 File Offset: 0x000019E0
		public void ReadDeviceBatteryStatus(Phone phone, CancellationToken cancellationToken)
		{
			BatteryStatus obj = this.GetAdaptation(phone.Type).ReadBatteryStatus(phone);
			Action<BatteryStatus> deviceBatteryStatusRead = this.DeviceBatteryStatusRead;
			if (deviceBatteryStatusRead != null)
			{
				deviceBatteryStatusRead(obj);
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000381C File Offset: 0x00001A1C
		public void CheckIfDeviceStillConnected(Phone phone, CancellationToken cancellationToken)
		{
			bool obj = this.GetAdaptation(phone.Type).CheckIfDeviceStillConnected(phone);
			Action<bool> deviceConnectionStatusRead = this.DeviceConnectionStatusRead;
			if (deviceConnectionStatusRead != null)
			{
				deviceConnectionStatusRead(obj);
			}
		}

		// Token: 0x0400000B RID: 11
		private readonly List<BaseAdaptation> adaptationServices = new List<BaseAdaptation>();

		// Token: 0x0400000C RID: 12
		private readonly ManufacturerAutodetectionService manufacturerAutodetectionService;

		// Token: 0x0400000D RID: 13
		private readonly ReportingService reportingService;

		// Token: 0x0400000E RID: 14
		private bool disposed;

		// Token: 0x0400000F RID: 15
		private DetectionParameters detectionParameters;
	}
}
