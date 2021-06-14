using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsDeviceRecoveryTool.Core;
using Microsoft.WindowsDeviceRecoveryTool.Core.Cache;
using Nokia.Lucid.DeviceDetection;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x02000018 RID: 24
	internal sealed class DetectionHandler : IDetectionHandler, IDisposable
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00004C74 File Offset: 0x00002E74
		public DetectionHandler(IUsbDeviceMonitor usbDeviceMonitor, IEnumerable<IDeviceSupport> supports, IDeviceInformationCacheManager deviceInformationCacheManager)
		{
			if (usbDeviceMonitor == null)
			{
				throw new ArgumentNullException("usbDeviceMonitor");
			}
			if (supports == null)
			{
				throw new ArgumentNullException("supports");
			}
			if (deviceInformationCacheManager == null)
			{
				throw new ArgumentNullException("deviceInformationCacheManager");
			}
			this.usbDeviceMonitor = usbDeviceMonitor;
			this.supports = supports;
			this.deviceInformationCacheManager = deviceInformationCacheManager;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000563C File Offset: 0x0000383C
		public async Task<DeviceInfoEventArgs> TakeDeviceInfoEventAsync(CancellationToken cancellationToken)
		{
			DetectionHandler.ChangedDevice changedDevice2;
			DetectionHandler.ChangedDevice attachedDevice;
			for (;;)
			{
				Task<DetectionHandler.ChangedDevice> detectionTask = this.CreateDetectionTask(cancellationToken);
				this.ongoingTasks.Add(detectionTask);
				Task<DetectionHandler.ChangedDevice> finishedTask = await Task.WhenAny<DetectionHandler.ChangedDevice>(this.ongoingTasks);
				DetectionHandler.ChangedDevice changedDevice = null;
				try
				{
					changedDevice = await finishedTask;
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception)
				{
					continue;
				}
				finally
				{
					this.ongoingTasks.Remove(finishedTask);
				}
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				if (!changedDevice.IsAttached)
				{
					changedDevice2 = this.attachedDevices.FirstOrDefault((DetectionHandler.ChangedDevice d) => string.Equals(d.Identifier, changedDevice.Identifier, StringComparison.OrdinalIgnoreCase));
					if (changedDevice2 != null)
					{
						goto Block_5;
					}
				}
				else
				{
					attachedDevice = this.attachedDevices.FirstOrDefault((DetectionHandler.ChangedDevice d) => string.Equals(d.Identifier, changedDevice.Identifier, StringComparison.OrdinalIgnoreCase));
					if (attachedDevice != null && attachedDevice.UpdatedDeviceInfo != null)
					{
						goto Block_7;
					}
					VidPidPair vidPid = VidPidPair.Parse(changedDevice.Identifier);
					var detectionInfos = (from s in this.supports.SelectMany((IDeviceSupport s) => from vp in s.GetDeviceDetectionInformation()
					select new
					{
						Support = s,
						DetectionInfo = vp
					})
					where s.DetectionInfo.VidPidPair == vidPid
					select s).ToArray();
					var defferedDetection = (from info in detectionInfos
					where info.DetectionInfo.DetectionDeferred
					select info).ToArray();
					var nonDefferedDetection = (from info in detectionInfos
					where !info.DetectionInfo.DetectionDeferred
					select info).ToArray();
					if (nonDefferedDetection.Length > 0)
					{
						goto Block_13;
					}
					this.attachedDevices.Add(changedDevice);
					IEnumerable<IDeviceSupport> defferedSupports = from d in defferedDetection
					select d.Support;
					Task<DetectionHandler.ChangedDevice> identificationTask = this.UpdateDeviceDetectionDataAsync(defferedSupports, changedDevice, cancellationToken);
					this.ongoingTasks.Add(identificationTask);
				}
			}
			try
			{
				await Task.WhenAll<DetectionHandler.ChangedDevice>(this.ongoingTasks);
			}
			catch (Exception)
			{
			}
			throw new OperationCanceledException(cancellationToken);
			Block_5:
			this.attachedDevices.Remove(changedDevice2);
			return new DeviceInfoEventArgs(new DeviceInfo(changedDevice2.Identifier), DeviceInfoAction.Detached, false);
			Block_7:
			return new DeviceInfoEventArgs(attachedDevice.UpdatedDeviceInfo, DeviceInfoAction.Attached, CS$<>8__locals1.changedDevice.IsEnumerated);
			Block_13:
			this.attachedDevices.Add(CS$<>8__locals1.changedDevice);
			return new DeviceInfoEventArgs(new DeviceInfo(CS$<>8__locals1.changedDevice.Identifier), DeviceInfoAction.Attached, CS$<>8__locals1.changedDevice.IsEnumerated);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000058CC File Offset: 0x00003ACC
		public async Task UpdateDeviceInfoAsync(DeviceInfo deviceInfo, CancellationToken cancellationToken)
		{
			DetectionHandler.ChangedDevice attachedDevice = this.attachedDevices.FirstOrDefault((DetectionHandler.ChangedDevice d) => string.Equals(d.Identifier, deviceInfo.DeviceIdentifier, StringComparison.OrdinalIgnoreCase));
			if (attachedDevice.UpdatedDeviceInfo == null)
			{
				await this.UpdateDeviceDetectionDataAsync(this.supports, attachedDevice, cancellationToken);
			}
			if (attachedDevice.UpdatedDeviceInfo != null)
			{
				deviceInfo.DeviceBitmapBytes = attachedDevice.UpdatedDeviceInfo.DeviceBitmapBytes;
				deviceInfo.DeviceSalesName = attachedDevice.UpdatedDeviceInfo.DeviceSalesName;
				deviceInfo.IsDeviceSupported = attachedDevice.UpdatedDeviceInfo.IsDeviceSupported;
				deviceInfo.SupportId = attachedDevice.UpdatedDeviceInfo.SupportId;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005926 File Offset: 0x00003B26
		public void Dispose()
		{
			this.usbDeviceMonitor.Dispose();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005A70 File Offset: 0x00003C70
		private async Task<DetectionHandler.ChangedDevice> CreateDetectionTask(CancellationToken cancellationToken)
		{
			UsbDeviceChangeEvent result = await this.usbDeviceMonitor.TakeDeviceChangeEventAsync(cancellationToken);
			return new DetectionHandler.ChangedDevice(result.Data.Path, result.Data.Action == DeviceChangeAction.Attach, result.IsEnumerated);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005E0C File Offset: 0x0000400C
		private async Task<DetectionHandler.ChangedDevice> UpdateDeviceDetectionDataAsync(IEnumerable<IDeviceSupport> supportsToCheck, DetectionHandler.ChangedDevice changedDevice, CancellationToken cancellationToken)
		{
			string deviceIdentifier = changedDevice.Identifier;
			DeviceDetectionData deviceDetectionData = new DeviceDetectionData(deviceIdentifier);
			using (this.deviceInformationCacheManager.EnableCacheForDevicePath(deviceIdentifier))
			{
				foreach (IDeviceSupport support in supportsToCheck)
				{
					await support.UpdateDeviceDetectionDataAsync(deviceDetectionData, cancellationToken);
					if (deviceDetectionData.IsDeviceSupported)
					{
						DeviceInfo updatedDeviceInfo = new DeviceInfo(deviceIdentifier)
						{
							DeviceBitmapBytes = deviceDetectionData.DeviceBitmapBytes,
							DeviceSalesName = deviceDetectionData.DeviceSalesName,
							IsDeviceSupported = deviceDetectionData.IsDeviceSupported,
							SupportId = support.Id
						};
						DetectionHandler.ChangedDevice changedDevice2 = this.attachedDevices.FirstOrDefault((DetectionHandler.ChangedDevice at) => string.Equals(at.Identifier, deviceIdentifier, StringComparison.OrdinalIgnoreCase));
						if (changedDevice2 != null)
						{
							changedDevice2.UpdatedDeviceInfo = updatedDeviceInfo;
						}
						break;
					}
				}
			}
			return changedDevice;
		}

		// Token: 0x0400003E RID: 62
		private readonly IUsbDeviceMonitor usbDeviceMonitor;

		// Token: 0x0400003F RID: 63
		private readonly IEnumerable<IDeviceSupport> supports;

		// Token: 0x04000040 RID: 64
		private readonly IDeviceInformationCacheManager deviceInformationCacheManager;

		// Token: 0x04000041 RID: 65
		private readonly List<Task<DetectionHandler.ChangedDevice>> ongoingTasks = new List<Task<DetectionHandler.ChangedDevice>>();

		// Token: 0x04000042 RID: 66
		private readonly List<DetectionHandler.ChangedDevice> attachedDevices = new List<DetectionHandler.ChangedDevice>();

		// Token: 0x02000019 RID: 25
		private sealed class ChangedDevice
		{
			// Token: 0x060000C9 RID: 201 RVA: 0x00005E6E File Offset: 0x0000406E
			public ChangedDevice(string identifier, bool isAttached, bool isEnumerated)
			{
				this.Identifier = identifier;
				this.IsAttached = isAttached;
				this.IsEnumerated = isEnumerated;
			}

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000CA RID: 202 RVA: 0x00005E94 File Offset: 0x00004094
			// (set) Token: 0x060000CB RID: 203 RVA: 0x00005EAB File Offset: 0x000040AB
			public string Identifier { get; private set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000CC RID: 204 RVA: 0x00005EB4 File Offset: 0x000040B4
			// (set) Token: 0x060000CD RID: 205 RVA: 0x00005ECB File Offset: 0x000040CB
			public bool IsAttached { get; private set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x060000CE RID: 206 RVA: 0x00005ED4 File Offset: 0x000040D4
			// (set) Token: 0x060000CF RID: 207 RVA: 0x00005EEB File Offset: 0x000040EB
			public bool IsEnumerated { get; private set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005EF4 File Offset: 0x000040F4
			// (set) Token: 0x060000D1 RID: 209 RVA: 0x00005F0B File Offset: 0x0000410B
			public DeviceInfo UpdatedDeviceInfo { get; set; }
		}
	}
}
