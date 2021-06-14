using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Core
{
	// Token: 0x0200000B RID: 11
	public class DeviceDetectionData
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000024A2 File Offset: 0x000006A2
		public DeviceDetectionData(string usbDeviceInterfaceDevicePath)
		{
			this.UsbDeviceInterfaceDevicePath = usbDeviceInterfaceDevicePath;
			this.VidPidPair = VidPidPair.Parse(usbDeviceInterfaceDevicePath);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000024BD File Offset: 0x000006BD
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000024C5 File Offset: 0x000006C5
		public VidPidPair VidPidPair { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000024CE File Offset: 0x000006CE
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000024D6 File Offset: 0x000006D6
		public string UsbDeviceInterfaceDevicePath { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024DF File Offset: 0x000006DF
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000024E7 File Offset: 0x000006E7
		public bool IsDeviceSupported { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024F0 File Offset: 0x000006F0
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000024F8 File Offset: 0x000006F8
		public string DeviceSalesName { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002501 File Offset: 0x00000701
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002509 File Offset: 0x00000709
		public byte[] DeviceBitmapBytes { get; set; }
	}
}
