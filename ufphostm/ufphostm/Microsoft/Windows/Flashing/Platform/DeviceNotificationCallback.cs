using System;
using System.Runtime.InteropServices;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x0200003E RID: 62
	public abstract class DeviceNotificationCallback
	{
		// Token: 0x06000114 RID: 276
		public abstract void Connected([In] string DevicePath);

		// Token: 0x06000115 RID: 277
		public abstract void Disconnected([In] string DevicePath);

		// Token: 0x06000116 RID: 278 RVA: 0x000124A8 File Offset: 0x000118A8
		public DeviceNotificationCallback()
		{
		}
	}
}
