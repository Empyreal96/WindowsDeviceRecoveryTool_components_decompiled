using System;

namespace Microsoft.WindowsDeviceRecoveryTool.FawkesAdaptation
{
	// Token: 0x02000004 RID: 4
	internal sealed class FawkesDeviceInfo
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002208 File Offset: 0x00000408
		public FawkesDeviceInfo(string firmwareVersion, string hardwareId, string deviceFriendlyName)
		{
			if (firmwareVersion == null)
			{
				throw new ArgumentNullException("firmwareVersion");
			}
			this.FirmwareVersion = firmwareVersion;
			this.HardwareId = hardwareId;
			this.DeviceFriendlyName = deviceFriendlyName;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002233 File Offset: 0x00000433
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000223B File Offset: 0x0000043B
		public string FirmwareVersion { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002244 File Offset: 0x00000444
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000224C File Offset: 0x0000044C
		public string HardwareId { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002255 File Offset: 0x00000455
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000225D File Offset: 0x0000045D
		public string DeviceFriendlyName { get; set; }
	}
}
