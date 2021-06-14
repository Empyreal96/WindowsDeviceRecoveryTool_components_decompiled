using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Ffu
{
	// Token: 0x02000004 RID: 4
	public sealed class FfuDeviceInformation
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000207B File Offset: 0x0000027B
		public FfuDeviceInformation(string deviceFriendlyName, string usbDevicePath, Guid deviceUniqueId)
		{
			this.DeviceFriendlyName = deviceFriendlyName;
			this.UsbDevicePath = usbDevicePath;
			this.DeviceUniqueId = deviceUniqueId;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020A0 File Offset: 0x000002A0
		public string DeviceFriendlyName { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020A9 File Offset: 0x000002A9
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020B1 File Offset: 0x000002B1
		public string UsbDevicePath { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020BA File Offset: 0x000002BA
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020C2 File Offset: 0x000002C2
		public Guid DeviceUniqueId { get; private set; }
	}
}
