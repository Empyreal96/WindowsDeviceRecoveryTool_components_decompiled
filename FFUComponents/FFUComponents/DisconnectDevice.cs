using System;
using System.Collections.Generic;
using System.Threading;

namespace FFUComponents
{
	// Token: 0x0200000D RID: 13
	internal class DisconnectDevice
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002CBC File Offset: 0x00000EBC
		~DisconnectDevice()
		{
			this.cancelEvent.Set();
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002CF0 File Offset: 0x00000EF0
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public IFFUDeviceInternal FFUDevice { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002D01 File Offset: 0x00000F01
		public Guid DeviceUniqueId
		{
			get
			{
				return this.FFUDevice.DeviceUniqueID;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D10 File Offset: 0x00000F10
		public DisconnectDevice(IFFUDeviceInternal device, Dictionary<Guid, DisconnectDevice> collection)
		{
			this.FFUDevice = device;
			this.DiscCollection = collection;
			this.cancelEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
			this.removalThread = new Thread(new ParameterizedThreadStart(DisconnectDevice.WaitAndRemove));
			this.removalThread.Start(this);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D61 File Offset: 0x00000F61
		public void Cancel()
		{
			this.cancelEvent.Set();
			this.Remove();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D75 File Offset: 0x00000F75
		public bool WaitForReconnect()
		{
			return this.cancelEvent.WaitOne(30000, false);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002D88 File Offset: 0x00000F88
		private static void WaitAndRemove(object obj)
		{
			DisconnectDevice disconnectDevice = obj as DisconnectDevice;
			if (!disconnectDevice.WaitForReconnect())
			{
				disconnectDevice.Remove();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002DAC File Offset: 0x00000FAC
		private void Remove()
		{
			lock (this.DiscCollection)
			{
				this.DiscCollection.Remove(this.DeviceUniqueId);
			}
		}

		// Token: 0x04000011 RID: 17
		private EventWaitHandle cancelEvent;

		// Token: 0x04000012 RID: 18
		private Dictionary<Guid, DisconnectDevice> DiscCollection;

		// Token: 0x04000013 RID: 19
		private Thread removalThread;
	}
}
