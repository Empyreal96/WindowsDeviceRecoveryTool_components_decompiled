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
using Microsoft.Tools.Connectivity;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Reporting.Enums;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Nokia.Lucid;
using Nokia.Lucid.DeviceDetection;
using Nokia.Lucid.Primitives;

namespace Microsoft.WindowsDeviceRecoveryTool.AnalogAdaptation.Services
{
	// Token: 0x02000005 RID: 5
	[PartCreationPolicy(CreationPolicy.Shared)]
	[ExportAdaptation(Type = PhoneTypes.Analog)]
	public class AnalogAdaptation : BaseAdaptation
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002616 File Offset: 0x00000816
		[ImportingConstructor]
		public AnalogAdaptation(MsrService msrService, FfuFileInfoService ffuFileInfoService, ReportingService reportingService, FlowConditionService flowCondition)
		{
			this.msrService = msrService;
			this.ffuFileInfoService = ffuFileInfoService;
			this.reportingService = reportingService;
			this.flowCondition = flowCondition;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002640 File Offset: 0x00000840
		public override PhoneTypes PhoneType
		{
			get
			{
				return PhoneTypes.Analog;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002654 File Offset: 0x00000854
		public override string ReportManufacturerName
		{
			get
			{
				return "Microsoft";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000266C File Offset: 0x0000086C
		public override string ReportManufacturerProductLine
		{
			get
			{
				return "HoloLens";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002684 File Offset: 0x00000884
		public override string PackageExtension
		{
			get
			{
				return "ffu";
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000269C File Offset: 0x0000089C
		public override bool RecoverySupport
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000026B0 File Offset: 0x000008B0
		public override string ManufacturerName
		{
			get
			{
				return "Microsoft HoloLens";
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026C8 File Offset: 0x000008C8
		public override List<PackageFileInfo> FindPackage(string directory, Phone currentPhone, CancellationToken cancellationToken)
		{
			Tracer<AnalogAdaptation>.LogEntry("FindPackage");
			List<PackageFileInfo> list = new List<PackageFileInfo>();
			try
			{
				Tracer<AnalogAdaptation>.WriteVerbose("Device platform id: {0}", new object[]
				{
					currentPhone.PlatformId
				});
				if (currentPhone.PlatformId == null)
				{
					return list;
				}
				string[] files = Directory.GetFiles(directory, string.Format("*.{0}", this.PackageExtension), SearchOption.AllDirectories);
				foreach (string text in files)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return null;
					}
					PlatformId platformId = this.ffuFileInfoService.ReadFfuFilePlatformId(text);
					string softwareVersion;
					this.ffuFileInfoService.TryReadFfuSoftwareVersion(text, out softwareVersion);
					Tracer<AnalogAdaptation>.WriteVerbose("Package platform id: {0}", new object[]
					{
						platformId
					});
					if (platformId.IsCompatibleWithDevicePlatformId(currentPhone.PlatformId))
					{
						Tracer<AnalogAdaptation>.WriteVerbose("PlatformIds compatible", new object[0]);
						list.Add(new FfuPackageFileInfo(text, platformId, softwareVersion));
					}
					else
					{
						Tracer<AnalogAdaptation>.WriteVerbose("PlatformIds NOT compatible - package skipped", new object[0]);
					}
				}
				if (cancellationToken.IsCancellationRequested)
				{
					return null;
				}
			}
			catch (DirectoryNotFoundException error)
			{
				Tracer<AnalogAdaptation>.WriteError(error);
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("FindPackage");
			}
			return list;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002878 File Offset: 0x00000A78
		public override List<PackageFileInfo> FindAllPackages(string directory, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002880 File Offset: 0x00000A80
		public override PackageFileInfo CheckLatestPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AnalogAdaptation>.LogEntry("CheckLatestPackage");
			PackageFileInfo result;
			try
			{
				Task<PackageFileInfo> task = this.msrService.CheckLatestPackage(this.DeviceQueryParameters(phone), cancellationToken);
				task.Wait(cancellationToken);
				PackageFileInfo packageFileInfo = task.Result;
				MsrPackageInfo msrPackageInfo = packageFileInfo as MsrPackageInfo;
				if (msrPackageInfo != null && msrPackageInfo.PackageFileData != null)
				{
					packageFileInfo = new MsrPackageInfo(msrPackageInfo.PackageId, msrPackageInfo.Name, msrPackageInfo.SoftwareVersion, msrPackageInfo.SoftwareVersion)
					{
						PackageFileData = msrPackageInfo.PackageFileData
					};
				}
				result = packageFileInfo;
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
				Tracer<AnalogAdaptation>.LogExit("CheckLatestPackage");
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029C0 File Offset: 0x00000BC0
		public override void CheckPackageIntegrity(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AnalogAdaptation>.LogEntry("CheckPackageIntegrity");
			try
			{
				if (phone.PackageFileInfo.OfflinePackage)
				{
					Tracer<AnalogAdaptation>.WriteInformation("Check Offline ffu package");
					this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
				}
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("CheckPackageIntegrity");
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A30 File Offset: 0x00000C30
		public override void FlashDevice(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AnalogAdaptation>.LogEntry("FlashDevice");
			try
			{
				IFFUDevice device;
				if (this.TryTakeFfuDevice(phone, out device))
				{
					this.RelockDeviceUnlockId(device);
					this.FlashFfu(device, phone.PackageFilePath);
				}
				this.WaitForReboot(phone);
			}
			catch (FFUFlashException ex)
			{
				if (ex.Error == FFUFlashException.ErrorCode.SignatureCheckFailed)
				{
					throw new SoftwareIsNotCorrectlySignedException("The device rejected the cryptographic signature in the software package.", ex);
				}
				throw;
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("FlashDevice");
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002AD0 File Offset: 0x00000CD0
		private void WaitForReboot(Phone phone)
		{
			this.progressMessage = "WaitForDeviceToBoot";
			this.RaiseIndeterminateProgressChanged(this.progressMessage);
			IpDeviceCommunicator ipDeviceCommunicator = this.WaitNormalModeDevice(phone);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B00 File Offset: 0x00000D00
		private bool TryTakeFfuDevice(Phone phone, out IFFUDevice device)
		{
			this.ffuDeviceEvent = new AutoResetEvent(false);
			this.normalModeDeviceEvent = new AutoResetEvent(false);
			this.progressMessage = "PuttingDeviceInRecoveryMode";
			this.RaiseIndeterminateProgressChanged(this.progressMessage);
			device = (this.GetConnectedFfuDevice(phone) ?? this.WaitConnectedFfuDevice(phone));
			bool result;
			if (device == null)
			{
				result = false;
			}
			else
			{
				Tracer<AnalogAdaptation>.WriteInformation("Device in FFU mode detected");
				Tracer<AnalogAdaptation>.WriteInformation("Friendly name : {0}", new object[]
				{
					device.DeviceFriendlyName
				});
				Tracer<AnalogAdaptation>.WriteInformation("Unique ID : {0}", new object[]
				{
					device.DeviceUniqueID
				});
				result = true;
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public override bool IsDeviceInFlashModeConnected(Phone phone, CancellationToken cancellationToken)
		{
			return true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public override void DownloadPackage(Phone phone, CancellationToken cancellationToken)
		{
			Tracer<AnalogAdaptation>.LogEntry("DownloadPackage");
			base.RaiseProgressPercentageChanged(0, null);
			try
			{
				this.reportingService.OperationStarted(phone, ReportOperationType.DownloadPackage);
				DownloadParameters downloadParameters = new DownloadParameters
				{
					DiscoveryParameters = this.DeviceQueryParameters(phone),
					DestinationFolder = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetLumiaProductsPath((phone.PlatformId != null) ? phone.PlatformId.ProductName : "holo-lens"),
					FilesVersioned = true
				};
				Tracer<AnalogAdaptation>.WriteInformation("Download Params: {0}", new object[]
				{
					downloadParameters
				});
				phone.PackageFilePath = this.msrService.DownloadLatestPackage(downloadParameters, cancellationToken).FirstOrDefault<string>();
				Tuple<long, long, bool> downloadPackageInformation = this.msrService.GetDownloadPackageInformation();
				Tracer<AnalogAdaptation>.WriteInformation("Downloaded package file path set: {0}", new object[]
				{
					phone.PackageFilePath
				});
				Tracer<AnalogAdaptation>.WriteInformation("Check downloaded ffu package");
				this.ffuFileInfoService.ReadFfuFile(phone.PackageFilePath);
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
				Tracer<AnalogAdaptation>.WriteError(ex);
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("DownloadPackage");
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public override void DownloadEmergencyPackage(Phone currentPhone, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public override SwVersionComparisonResult CompareFirmwareVersions(Phone phone)
		{
			PackageFileInfo packageFileInfo = phone.PackageFileInfo;
			string akVersion = phone.AkVersion;
			SwVersionComparisonResult result;
			if (packageFileInfo == null || packageFileInfo.AkVersion == null || akVersion == null)
			{
				result = SwVersionComparisonResult.UnableToCompare;
			}
			else
			{
				result = VersionComparer.CompareSoftwareVersions(akVersion, packageFileInfo.AkVersion, new char[]
				{
					'.'
				});
			}
			return result;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002E24 File Offset: 0x00001024
		protected override void FillSupportedDeviceIdentifiers()
		{
			this.SupportedNormalModeIds.Add(new Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier("045E", "FFE5"));
			this.SupportedNormalModeIds.Add(new Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier("045E", "0653"));
			this.SupportedNormalModeIds.Add(new Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier("045E", "0652"));
			this.SupportedNormalModeIds.Add(new Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier("045E", "062A"));
			this.SupportedFlashModeIds.Add(new Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier("045E", "062A"));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002EBC File Offset: 0x000010BC
		public override int ReadBatteryLevel(Phone phone)
		{
			DiscoveredDeviceInfo discoveredDeviceInfo = this.MatchDeviceInNormalMode(phone);
			int result;
			if (discoveredDeviceInfo == null)
			{
				Tracer<AnalogAdaptation>.WriteWarning("Device not found in NORMAL mode! - skipping battery check.", new object[0]);
				result = -1;
			}
			else
			{
				IpDeviceCommunicator ipDeviceCommunicator = new IpDeviceCommunicator(discoveredDeviceInfo.UniqueId);
				int num = -1;
				try
				{
					ipDeviceCommunicator.Connect();
				}
				catch (Exception error)
				{
					Tracer<AnalogAdaptation>.WriteError(error);
					throw;
				}
				try
				{
					num = ipDeviceCommunicator.ReadBatteryLevel();
				}
				catch (Exception error)
				{
					Tracer<AnalogAdaptation>.WriteError(error);
				}
				finally
				{
					ipDeviceCommunicator.Disconnect();
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002F70 File Offset: 0x00001170
		public override void ReadDeviceInfo(Phone currentPhone, CancellationToken cancellationToken)
		{
			try
			{
				Phone phone = this.ReadDeviceInfoInNormalMode(currentPhone) ?? this.ReadDeviceInfoInFfuMode(currentPhone);
				if (phone != null)
				{
					base.RaiseDeviceInfoRead(phone);
					return;
				}
			}
			catch (Exception ex)
			{
				Tracer<AnalogAdaptation>.WriteError(ex);
				throw new ReadPhoneInformationException(ex.Message, ex);
			}
			throw new ReadPhoneInformationException("Cannot find selected device!");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002FDC File Offset: 0x000011DC
		private Phone ReadDeviceInfoInFfuMode(Phone currentPhone)
		{
			IFFUDevice connectedFfuDevice = this.GetConnectedFfuDevice(currentPhone);
			Phone result;
			if (connectedFfuDevice != null)
			{
				PlatformId platformId = new PlatformId();
				platformId.SetPlatformId(connectedFfuDevice.DeviceFriendlyName);
				currentPhone.PlatformId = platformId;
				currentPhone.SalesName = connectedFfuDevice.DeviceFriendlyName;
				currentPhone.ConnectionId = connectedFfuDevice.DeviceUniqueID;
				result = currentPhone;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000303C File Offset: 0x0000123C
		private IFFUDevice GetConnectedFfuDevice(Phone phone)
		{
			Tracer<AnalogAdaptation>.LogEntry("GetConnectedFfuDevice");
			IpDeviceCommunicator normalModeDevice = this.GetNormalModeDevice(phone);
			IFFUDevice result;
			if (normalModeDevice != null)
			{
				Tracer<AnalogAdaptation>.WriteInformation("Rebooting Device to FFU (UEFI) mode");
				normalModeDevice.Connect();
				normalModeDevice.ExecuteCommand(IpDeviceCommunicator.DeviceUpdateCommandRebootToUefi, null);
				normalModeDevice.Disconnect();
				result = this.WaitFfuDevice(phone);
			}
			else
			{
				result = this.GetFfuDevice(phone);
			}
			Tracer<AnalogAdaptation>.LogExit("GetConnectedFfuDevice");
			return result;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000030B4 File Offset: 0x000012B4
		private IpDeviceCommunicator GetNormalModeDevice(Phone currentPhone)
		{
			Tracer<AnalogAdaptation>.LogEntry("GetNormalModeDevice");
			try
			{
				DiscoveredDeviceInfo discoveredDeviceInfo = this.MatchDeviceInNormalMode(currentPhone);
				if (discoveredDeviceInfo == null)
				{
					Tracer<AnalogAdaptation>.WriteError("No devices found and was looking for {0}!", new object[]
					{
						currentPhone
					});
					return null;
				}
				if (discoveredDeviceInfo.Connection == DiscoveredDeviceInfo.ConnectionType.IpOverUsb)
				{
					IpDeviceCommunicator ipDeviceCommunicator = new IpDeviceCommunicator(discoveredDeviceInfo.UniqueId);
					ipDeviceCommunicator.Connect();
					try
					{
						IpDeviceCommunicator.DeviceProperties deviceProperties = default(IpDeviceCommunicator.DeviceProperties);
						if (ipDeviceCommunicator.GetDeviceProperties(ref deviceProperties))
						{
							Tracer<AnalogAdaptation>.WriteInformation("Device in OS mode detected");
							Tracer<AnalogAdaptation>.WriteInformation("Name : {0}", new object[]
							{
								deviceProperties.Name
							});
							Tracer<AnalogAdaptation>.WriteInformation("Firmware version : {0}", new object[]
							{
								deviceProperties.FirmwareVersion
							});
							Tracer<AnalogAdaptation>.WriteInformation("UEFI name : {0}", new object[]
							{
								deviceProperties.UefiName
							});
							Tracer<AnalogAdaptation>.WriteInformation("Battery level : {0}", new object[]
							{
								deviceProperties.BatteryLevel
							});
							return ipDeviceCommunicator;
						}
					}
					finally
					{
						ipDeviceCommunicator.Disconnect();
					}
				}
				else
				{
					Tracer<AnalogAdaptation>.WriteWarning("Found device is not IpOverUsb device!", new object[0]);
				}
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("GetNormalModeDevice");
			}
			return null;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003250 File Offset: 0x00001450
		private IFFUDevice WaitFfuDevice(Phone phone)
		{
			this.selectedPhone = phone;
			FFUManager.DeviceConnectEvent += this.OnFfuDeviceConnected;
			FFUManager.Start();
			Tracer<AnalogAdaptation>.WriteInformation("Waiting for device to boot into FFU (UEFI) mode... ({0})", new object[]
			{
				phone.ConnectionId
			});
			bool flag = this.ffuDeviceEvent.WaitOne(AnalogAdaptation.FfuModeTimeout);
			FFUManager.DeviceConnectEvent -= this.OnFfuDeviceConnected;
			FFUManager.Stop();
			if (flag)
			{
				return this.ffuDevice;
			}
			throw new DeviceNotFoundException("Could not find device booted into FFU (UEFI) mode");
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032E8 File Offset: 0x000014E8
		private void OnFfuDeviceConnected(object sender, ConnectEventArgs e)
		{
			if (this.selectedPhone != null)
			{
				if (e.Device.DeviceUniqueID == this.selectedPhone.ConnectionId)
				{
					Tracer<AnalogAdaptation>.WriteVerbose("Found ffu device with UniqueId: {0}", new object[]
					{
						e.Device.DeviceUniqueID
					});
					this.ffuDevice = e.Device;
					this.ffuDeviceEvent.Set();
				}
			}
			else
			{
				Tracer<AnalogAdaptation>.WriteInformation("Taking first one, selected phone is NULL");
				this.ffuDevice = e.Device;
				this.ffuDeviceEvent.Set();
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000033C0 File Offset: 0x000015C0
		private IFFUDevice GetFfuDevice(Phone phone)
		{
			Tracer<AnalogAdaptation>.LogEntry("GetFfuDevice");
			FFUManager.Start();
			IFFUDevice result;
			try
			{
				ICollection<IFFUDevice> collection = new List<IFFUDevice>();
				FFUManager.GetFlashableDevices(ref collection);
				if (collection.Count == 0)
				{
					Tracer<AnalogAdaptation>.WriteWarning("No FFU device found!", new object[0]);
				}
				IFFUDevice iffudevice = collection.FirstOrDefault((IFFUDevice dev) => dev.DeviceUniqueID == phone.ConnectionId);
				if (iffudevice == null)
				{
					Tracer<AnalogAdaptation>.WriteWarning("Device not found by connectionId, trying to find with Path", new object[0]);
					iffudevice = FFUManager.GetFlashableDevice(phone.Path, false);
				}
				if (iffudevice == null && collection.Any<IFFUDevice>())
				{
					Tracer<AnalogAdaptation>.WriteError("Taking first one, not found device: {0}", new object[]
					{
						phone.ConnectionId
					});
					result = collection.First<IFFUDevice>();
				}
				else
				{
					result = iffudevice;
				}
			}
			finally
			{
				FFUManager.Stop();
				Tracer<AnalogAdaptation>.LogExit("GetFfuDevice");
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000034E4 File Offset: 0x000016E4
		private IFFUDevice WaitConnectedFfuDevice(Phone phone)
		{
			this.selectedPhone = phone;
			DeviceWatcher deviceWatcher = this.InitializeWatcher();
			deviceWatcher.DeviceChanged += this.OnDeviceChanged;
			FFUManager.DeviceConnectEvent += this.OnFfuDeviceConnected;
			deviceWatcher.Start();
			FFUManager.Start();
			WaitHandle[] waitHandles = new WaitHandle[]
			{
				this.normalModeDeviceEvent,
				this.ffuDeviceEvent
			};
			Tracer<AnalogAdaptation>.WriteInformation("Please connect device in OS or FFU (UEFI) mode");
			int num = WaitHandle.WaitAny(waitHandles, AnalogAdaptation.DeviceConnectionTimeout);
			deviceWatcher.DeviceChanged -= this.OnDeviceChanged;
			FFUManager.DeviceConnectEvent -= this.OnFfuDeviceConnected;
			FFUManager.Stop();
			IFFUDevice connectedFfuDevice;
			if (num == 0)
			{
				connectedFfuDevice = this.GetConnectedFfuDevice(phone);
			}
			else
			{
				if (num != 1)
				{
					throw new DeviceNotFoundException("Could not find device in OS or FFU (UEFI) mode");
				}
				connectedFfuDevice = this.ffuDevice;
			}
			return connectedFfuDevice;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000035D0 File Offset: 0x000017D0
		private DeviceWatcher InitializeWatcher()
		{
			return new DeviceWatcher
			{
				DeviceTypeMap = new DeviceTypeMap(new Dictionary<Guid, DeviceType>
				{
					{
						AnalogAdaptation.IpOverUsbDeviceInterfaceGuid,
						DeviceType.Interface
					}
				}),
				Filter = ((Nokia.Lucid.Primitives.DeviceIdentifier ss) => (ss.Vid("045E") && ss.Pid("FFE5")) || ((ss.Vid("045E") && ss.Pid("0653")) || (ss.Vid("045E") && ss.Pid("0652"))))
			};
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000379C File Offset: 0x0000199C
		private void OnDeviceChanged(object sender, DeviceChangedEventArgs e)
		{
			if (e.Action == DeviceChangeAction.Attach)
			{
				this.normalModeDeviceEvent.Set();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000037C8 File Offset: 0x000019C8
		private void FlashFfu(IFFUDevice device, string ffuPath)
		{
			Tracer<AnalogAdaptation>.WriteInformation("Flashing {0}", new object[]
			{
				ffuPath
			});
			this.progressMessage = "SoftwareInstallation";
			base.RaiseProgressPercentageChanged(0, this.progressMessage);
			device.ProgressEvent += this.FlashProgressEvent;
			try
			{
				device.EndTransfer();
				device.FlashFFUFile(ffuPath, true);
			}
			finally
			{
				device.ProgressEvent -= this.FlashProgressEvent;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003854 File Offset: 0x00001A54
		private void RelockDeviceUnlockId(IFFUDevice device)
		{
			string deviceFriendlyName = device.DeviceFriendlyName;
			if (AnalogAdaptation.RelockDeviceUnlockIdNotSupportedPlatformIds.Contains(deviceFriendlyName, StringComparer.OrdinalIgnoreCase))
			{
				Tracer<AnalogAdaptation>.WriteInformation("Skipping device unlock ID relock. '{0}' is on the list of not supported platform IDs", new object[]
				{
					deviceFriendlyName
				});
			}
			else
			{
				this.progressMessage = "PreparingForSoftwareInstallation";
				this.RaiseIndeterminateProgressChanged(this.progressMessage);
				Tracer<AnalogAdaptation>.WriteInformation("Executing device unlock ID relock");
				try
				{
					device.RelockDeviceUnlockId();
				}
				catch (FFUDeviceCommandNotAvailableException)
				{
					Tracer<AnalogAdaptation>.WriteWarning("Device unlock ID relock not supported on this device", new object[0]);
				}
				catch (Exception error)
				{
					Tracer<AnalogAdaptation>.WriteError(error, "Error executing device unlock ID relock", new object[0]);
					throw;
				}
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003914 File Offset: 0x00001B14
		private void FlashProgressEvent(object sender, ProgressEventArgs progress)
		{
			double num = (double)progress.Position / (double)progress.Length * 100.0;
			base.RaiseProgressPercentageChanged((int)num, this.progressMessage);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000394C File Offset: 0x00001B4C
		private IpDeviceCommunicator WaitNormalModeDevice(Phone phone)
		{
			DeviceWatcher deviceWatcher = this.InitializeWatcher();
			deviceWatcher.DeviceChanged += this.OnDeviceChanged;
			deviceWatcher.Start();
			Tracer<AnalogAdaptation>.WriteInformation("Waiting for device to boot into OS...");
			bool flag = this.normalModeDeviceEvent.WaitOne(AnalogAdaptation.OsModeTimeout);
			deviceWatcher.DeviceChanged -= this.OnDeviceChanged;
			if (flag)
			{
				return this.GetNormalModeDevice(phone);
			}
			throw new DeviceNotFoundException("Could not find device booted into OS");
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000039E8 File Offset: 0x00001BE8
		protected override Stream GetImageDataStream(Phone phone)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Analog.png"));
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

		// Token: 0x06000031 RID: 49 RVA: 0x00003A4C File Offset: 0x00001C4C
		private Phone ReadDeviceInfoInNormalMode(Phone currentPhone)
		{
			Phone result;
			try
			{
				Tracer<AnalogAdaptation>.LogEntry("ReadDeviceInfoInNormalMode");
				DiscoveredDeviceInfo discoveredDeviceInfo = this.MatchDeviceInNormalMode(currentPhone);
				if (discoveredDeviceInfo == null)
				{
					Tracer<AnalogAdaptation>.WriteError("No devices found and was looking for {0}!", new object[]
					{
						currentPhone
					});
					result = null;
				}
				else
				{
					Tracer<AnalogAdaptation>.WriteVerbose("Current phone: {0}", new object[]
					{
						currentPhone
					});
					Tracer<AnalogAdaptation>.WriteVerbose("Found device data: Address - {0}, Connection - {1}, Location - {2}, Name - {3}, UniqueId - {4}", new object[]
					{
						discoveredDeviceInfo.Address,
						discoveredDeviceInfo.Connection,
						discoveredDeviceInfo.Location,
						discoveredDeviceInfo.Name,
						discoveredDeviceInfo.UniqueId
					});
					if (discoveredDeviceInfo.Connection == DiscoveredDeviceInfo.ConnectionType.IpOverUsb)
					{
						IpDeviceCommunicator ipDeviceCommunicator = new IpDeviceCommunicator(discoveredDeviceInfo.UniqueId);
						try
						{
							ipDeviceCommunicator.Connect();
						}
						catch (Exception error)
						{
							Tracer<AnalogAdaptation>.WriteError(error);
							throw;
						}
						try
						{
							IpDeviceCommunicator.DeviceProperties deviceProperties = default(IpDeviceCommunicator.DeviceProperties);
							if (ipDeviceCommunicator.GetDeviceProperties(ref deviceProperties))
							{
								Tracer<AnalogAdaptation>.WriteInformation("Device in OS mode detected");
								currentPhone.SalesName = deviceProperties.Name;
								currentPhone.SoftwareVersion = deviceProperties.FirmwareVersion;
								PlatformId platformId = new PlatformId();
								platformId.SetPlatformId(deviceProperties.UefiName);
								currentPhone.PlatformId = platformId;
								currentPhone.BatteryLevel = deviceProperties.BatteryLevel;
								currentPhone.ConnectionId = discoveredDeviceInfo.UniqueId;
								currentPhone.AkVersion = ipDeviceCommunicator.GetOSVersion();
								Tracer<AnalogAdaptation>.WriteInformation("Name: {0}, Firmware version: {1}, Uefi Name: {2}, Battery Level: {3}, OS Version: {4}", new object[]
								{
									deviceProperties.Name,
									deviceProperties.FirmwareVersion,
									deviceProperties.UefiName,
									deviceProperties.BatteryLevel,
									currentPhone.AkVersion
								});
								return currentPhone;
							}
							Tracer<AnalogAdaptation>.WriteError("GetDeviceProperties failed!", new object[0]);
						}
						catch (Exception error)
						{
							Tracer<AnalogAdaptation>.WriteError(error);
							throw;
						}
						finally
						{
							ipDeviceCommunicator.Disconnect();
						}
					}
					else
					{
						Tracer<AnalogAdaptation>.WriteWarning("Found device is not IpOverUsb device!", new object[0]);
					}
					result = null;
				}
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("ReadDeviceInfoInNormalMode");
			}
			return result;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003D7C File Offset: 0x00001F7C
		public override bool IsSupportedInNormalMode(UsbDevice usbDevice)
		{
			bool result;
			if (this.SupportedNormalModeIds.Any((Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid.Equals(usbDevice.Vid, StringComparison.CurrentCultureIgnoreCase) && deviceIdentifier.Pid.Equals(usbDevice.Pid, StringComparison.CurrentCultureIgnoreCase)))
			{
				result = (!this.SupportedFlashModeIds.Any((Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid.Equals(usbDevice.Vid, StringComparison.CurrentCultureIgnoreCase) && deviceIdentifier.Pid.Equals(usbDevice.Pid, StringComparison.CurrentCultureIgnoreCase)) || this.IsSupportedInFlashMode(usbDevice));
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003E48 File Offset: 0x00002048
		public override bool IsSupportedInFlashMode(UsbDevice usbDevice)
		{
			if (this.SupportedFlashModeIds.Any((Microsoft.WindowsDeviceRecoveryTool.Model.DeviceIdentifier deviceIdentifier) => deviceIdentifier.Vid.Equals(usbDevice.Vid, StringComparison.CurrentCultureIgnoreCase) && deviceIdentifier.Pid.Equals(usbDevice.Pid, StringComparison.CurrentCultureIgnoreCase)))
			{
				FFUManager.Start();
				Tracer<AnalogAdaptation>.WriteInformation("FFU Manager started.");
				try
				{
					IFFUDevice flashableDevice = FFUManager.GetFlashableDevice(usbDevice.Path, false);
					if (flashableDevice != null)
					{
						if (flashableDevice.DeviceFriendlyName.ToLower().Contains("sakura") || flashableDevice.DeviceFriendlyName.ToLower().Contains("hololens"))
						{
							Tracer<AnalogAdaptation>.WriteInformation("Analog device in ffu mode.");
							return true;
						}
						Tracer<AnalogAdaptation>.WriteWarning("Analog device friendly name not expected: {0}", new object[]
						{
							flashableDevice.DeviceFriendlyName
						});
					}
					return false;
				}
				finally
				{
					FFUManager.Stop();
					Tracer<AnalogAdaptation>.WriteInformation("FFU Manager stopped.");
				}
			}
			return false;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004040 File Offset: 0x00002240
		private DiscoveredDeviceInfo MatchDeviceInNormalMode(Phone currentPhone)
		{
			DiscoveredDeviceInfo result;
			try
			{
				Tracer<AnalogAdaptation>.LogEntry("MatchDeviceInNormalMode");
				if (currentPhone == null)
				{
					Tracer<AnalogAdaptation>.WriteError("Phone is NULL!", new object[0]);
					result = null;
				}
				else
				{
					DeviceDiscoveryService deviceDiscoveryService = new DeviceDiscoveryService();
					deviceDiscoveryService.Start(default(Guid));
					List<DiscoveredDeviceInfo> list = deviceDiscoveryService.DevicesDiscovered();
					deviceDiscoveryService.Stop();
					if (list.Count == 0)
					{
						Tracer<AnalogAdaptation>.WriteWarning("No devices in OS mode detected!", new object[0]);
						result = null;
					}
					else
					{
						Tracer<AnalogAdaptation>.WriteVerbose(string.Join(Environment.NewLine, from d in list
						where d != null
						select string.Format("Device discovered: Name = {0}, loc = {1}, add = {2}, arch = {3}, con = {4}, os = {5}, uid = {6} ", new object[]
						{
							d.Name,
							d.Location,
							d.Address,
							d.Architecture,
							d.Connection,
							d.OSVersion,
							d.UniqueId
						})), new object[0]);
						DiscoveredDeviceInfo discoveredDeviceInfo = list.FirstOrDefault((DiscoveredDeviceInfo device) => string.Compare(device.Location, currentPhone.InstanceId, StringComparison.InvariantCultureIgnoreCase) == 0);
						if (discoveredDeviceInfo == null)
						{
							Tracer<AnalogAdaptation>.WriteWarning("Device not found via location. Check dev UID.", new object[0]);
							string uid = this.ExtractDeviceUid(currentPhone);
							discoveredDeviceInfo = list.FirstOrDefault((DiscoveredDeviceInfo device) => string.Compare(uid, string.Format("{0}", device.UniqueId), StringComparison.InvariantCultureIgnoreCase) == 0);
						}
						if (discoveredDeviceInfo == null)
						{
							Tracer<AnalogAdaptation>.WriteError("No match found for {0}", new object[]
							{
								currentPhone
							});
							result = null;
						}
						else
						{
							result = discoveredDeviceInfo;
						}
					}
				}
			}
			finally
			{
				Tracer<AnalogAdaptation>.LogExit("MatchDeviceInNormalMode");
			}
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000423C File Offset: 0x0000243C
		protected override Stream GetManufacturerImageDataStream()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames.FirstOrDefault((string resourceName) => resourceName.Contains("Analog.png"));
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

		// Token: 0x06000036 RID: 54 RVA: 0x0000429C File Offset: 0x0000249C
		public override BatteryStatus ReadBatteryStatus(Phone phone)
		{
			try
			{
				int num = this.ReadBatteryLevel(phone);
				Tracer<AnalogAdaptation>.WriteInformation("Battery level: ", new object[]
				{
					num
				});
				if (num >= 25)
				{
					return BatteryStatus.BatteryOk;
				}
				if (num != -1)
				{
					return BatteryStatus.BatteryNotOkBlock;
				}
			}
			catch (Exception error)
			{
				Tracer<AnalogAdaptation>.WriteError(error, "Error reading battery status", new object[0]);
			}
			return BatteryStatus.BatteryUnknown;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004320 File Offset: 0x00002520
		private QueryParameters DeviceQueryParameters(Phone phone)
		{
			return new QueryParameters
			{
				ManufacturerName = "Microsoft",
				ManufacturerProductLine = "HoloLens",
				ManufacturerHardwareModel = "HoloLens",
				PackageType = "Firmware"
			};
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000436C File Offset: 0x0000256C
		private string ExtractDeviceUid(Phone currentPhone)
		{
			string result;
			if (string.IsNullOrEmpty(currentPhone.InstanceId))
			{
				Tracer<AnalogAdaptation>.WriteWarning("Current phone has no instance ID", new object[0]);
				result = null;
			}
			else
			{
				string text = currentPhone.InstanceId.Substring(currentPhone.InstanceId.LastIndexOf('\\') + 1);
				if (string.IsNullOrEmpty(text) || text.Length != 32)
				{
					Tracer<AnalogAdaptation>.WriteWarning("Base UID not proper: {0}", new object[]
					{
						text
					});
					result = null;
				}
				else
				{
					string text2 = new string(text.Substring(0, 8).Reverse<char>().ToArray<char>());
					string text3 = new string(text.Substring(8, 4).Reverse<char>().ToArray<char>());
					string text4 = new string(text.Substring(12, 4).Reverse<char>().ToArray<char>());
					string text5 = new string(text.Substring(16, 2).Reverse<char>().ToArray<char>());
					string text6 = new string(text.Substring(18, 2).Reverse<char>().ToArray<char>());
					string text7 = new string(text.Substring(20, 2).Reverse<char>().ToArray<char>());
					string text8 = new string(text.Substring(22, 2).Reverse<char>().ToArray<char>());
					string text9 = new string(text.Substring(24, 2).Reverse<char>().ToArray<char>());
					string text10 = new string(text.Substring(26, 2).Reverse<char>().ToArray<char>());
					string text11 = new string(text.Substring(28, 2).Reverse<char>().ToArray<char>());
					string text12 = new string(text.Substring(30, 2).Reverse<char>().ToArray<char>());
					string format = "{0}-{1}-{2}-{3}{4}-{5}{6}{7}{8}{9}{10}";
					string text13 = string.Format(format, new object[]
					{
						text2,
						text3,
						text4,
						text5,
						text6,
						text7,
						text8,
						text9,
						text10,
						text11,
						text12
					});
					Tracer<AnalogAdaptation>.WriteWarning("Extracted UID: {0}", new object[]
					{
						text13
					});
					result = text13;
				}
			}
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000459C File Offset: 0x0000279C
		public override bool CheckIfDeviceStillConnected(Phone phone)
		{
			return this.MatchDeviceInNormalMode(phone) != null || this.GetFfuDevice(phone) != null;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000045C8 File Offset: 0x000027C8
		private void RaiseIndeterminateProgressChanged(string message)
		{
			base.RaiseProgressPercentageChanged(101, message);
		}

		// Token: 0x04000008 RID: 8
		private static readonly string[] RelockDeviceUnlockIdNotSupportedPlatformIds = new string[]
		{
			"Microsoft Corporation.HoloLens.HoloLens.R06",
			"Microsoft Corporation.HoloLens.HoloLens.1.0"
		};

		// Token: 0x04000009 RID: 9
		private static readonly TimeSpan OsModeTimeout = new TimeSpan(0, 8, 0);

		// Token: 0x0400000A RID: 10
		private static readonly TimeSpan FfuModeTimeout = new TimeSpan(0, 5, 0);

		// Token: 0x0400000B RID: 11
		private static readonly TimeSpan DeviceConnectionTimeout = new TimeSpan(0, 2, 0);

		// Token: 0x0400000C RID: 12
		private static readonly Guid IpOverUsbDeviceInterfaceGuid = new Guid(654236750, 27331, 16961, 158, 77, 227, 212, 178, 197, 197, 52);

		// Token: 0x0400000D RID: 13
		public static readonly string OldestRollbackOsVersion = "10.0.14393.0";

		// Token: 0x0400000E RID: 14
		private readonly MsrService msrService;

		// Token: 0x0400000F RID: 15
		private readonly ReportingService reportingService;

		// Token: 0x04000010 RID: 16
		private readonly FlowConditionService flowCondition;

		// Token: 0x04000011 RID: 17
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000012 RID: 18
		private AutoResetEvent ffuDeviceEvent;

		// Token: 0x04000013 RID: 19
		private AutoResetEvent normalModeDeviceEvent;

		// Token: 0x04000014 RID: 20
		private IFFUDevice ffuDevice;

		// Token: 0x04000015 RID: 21
		private string progressMessage;

		// Token: 0x04000016 RID: 22
		private Phone selectedPhone;
	}
}
