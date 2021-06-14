using System;
using Microsoft.Windows.Flashing.Platform;

namespace FFUComponents
{
	// Token: 0x0200001A RID: 26
	public class NotificationCallback : DeviceNotificationCallback
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00003268 File Offset: 0x00001468
		public override void Connected(string devicePath)
		{
			FFUManager.OnThorConnect(devicePath);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003270 File Offset: 0x00001470
		public override void Disconnected(string devicePath)
		{
			FFUManager.OnThorDisconnect(devicePath);
		}
	}
}
