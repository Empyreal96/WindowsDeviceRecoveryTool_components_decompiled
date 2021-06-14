using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Detection
{
	// Token: 0x0200001A RID: 26
	internal sealed class DeviceInfo
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x00005F14 File Offset: 0x00004114
		public DeviceInfo(string deviceIdentifier)
		{
			this.DeviceIdentifier = deviceIdentifier;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005F28 File Offset: 0x00004128
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00005F3F File Offset: 0x0000413F
		public string DeviceIdentifier { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005F48 File Offset: 0x00004148
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005F5F File Offset: 0x0000415F
		public bool IsDeviceSupported { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005F68 File Offset: 0x00004168
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00005F7F File Offset: 0x0000417F
		public Guid SupportId { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005F88 File Offset: 0x00004188
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00005F9F File Offset: 0x0000419F
		public string DeviceSalesName { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005FA8 File Offset: 0x000041A8
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00005FBF File Offset: 0x000041BF
		public byte[] DeviceBitmapBytes { get; set; }
	}
}
