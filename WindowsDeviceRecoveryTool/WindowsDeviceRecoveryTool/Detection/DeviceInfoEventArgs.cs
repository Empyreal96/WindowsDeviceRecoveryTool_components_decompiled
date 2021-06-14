using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x02000013 RID: 19
	internal sealed class DeviceInfoEventArgs
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00003966 File Offset: 0x00001B66
		public DeviceInfoEventArgs(DeviceInfo deviceInfo, DeviceInfoAction deviceInfoAction, bool isEnumerated = false)
		{
			this.DeviceInfo = deviceInfo;
			this.DeviceInfoAction = deviceInfoAction;
			this.IsEnumerated = isEnumerated;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000398C File Offset: 0x00001B8C
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000039A3 File Offset: 0x00001BA3
		public DeviceInfo DeviceInfo { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000039AC File Offset: 0x00001BAC
		// (set) Token: 0x0600007B RID: 123 RVA: 0x000039C3 File Offset: 0x00001BC3
		public DeviceInfoAction DeviceInfoAction { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000039CC File Offset: 0x00001BCC
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000039E3 File Offset: 0x00001BE3
		public bool IsEnumerated { get; private set; }
	}
}
