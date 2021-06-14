using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000039 RID: 57
	public class UsbDevice
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000081E0 File Offset: 0x000063E0
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000081E8 File Offset: 0x000063E8
		public string DeviceInstanceId { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000081F1 File Offset: 0x000063F1
		// (set) Token: 0x06000156 RID: 342 RVA: 0x000081F9 File Offset: 0x000063F9
		public string FriendlyName { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00008202 File Offset: 0x00006402
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000820A File Offset: 0x0000640A
		public string DeviceInterfaceSymbolicLinkName { get; private set; }

		// Token: 0x06000159 RID: 345 RVA: 0x00008213 File Offset: 0x00006413
		public UsbDevice(string DeviceInstanceId, string FriendlyName, string DeviceInterfaceSymbolicLinkName)
		{
			this.DeviceInstanceId = DeviceInstanceId;
			this.FriendlyName = FriendlyName;
			this.DeviceInterfaceSymbolicLinkName = DeviceInterfaceSymbolicLinkName;
		}
	}
}
