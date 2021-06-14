using System;
using System.Collections.Generic;

namespace FFUComponents
{
	// Token: 0x0200000E RID: 14
	internal class DisconnectTimer
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public DisconnectTimer()
		{
			this.devices = new Dictionary<Guid, DisconnectDevice>();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E0C File Offset: 0x0000100C
		public void StopAllTimers()
		{
			DisconnectDevice[] array = new DisconnectDevice[this.devices.Values.Count];
			this.devices.Values.CopyTo(array, 0);
			foreach (DisconnectDevice disconnectDevice in array)
			{
				disconnectDevice.Cancel();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002E5C File Offset: 0x0000105C
		public void StartTimer(IFFUDeviceInternal device)
		{
			lock (this.devices)
			{
				DisconnectDevice disconnectDevice;
				if (this.devices.TryGetValue(device.DeviceUniqueID, out disconnectDevice))
				{
					throw new FFUException(device.DeviceFriendlyName, device.DeviceUniqueID, Resources.ERROR_MULTIPE_DISCONNECT_NOTIFICATIONS);
				}
				this.devices[device.DeviceUniqueID] = new DisconnectDevice(device, this.devices);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002EE0 File Offset: 0x000010E0
		public IFFUDeviceInternal StopTimer(IFFUDeviceInternal device)
		{
			IFFUDeviceInternal result = null;
			lock (this.devices)
			{
				DisconnectDevice disconnectDevice;
				if (this.devices.TryGetValue(device.DeviceUniqueID, out disconnectDevice))
				{
					disconnectDevice.Cancel();
					result = disconnectDevice.FFUDevice;
				}
			}
			return result;
		}

		// Token: 0x04000015 RID: 21
		private Dictionary<Guid, DisconnectDevice> devices;
	}
}
