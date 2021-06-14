using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200000C RID: 12
	public class EmergencyPackageInfo
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000083 RID: 131 RVA: 0x000030A4 File Offset: 0x000012A4
		// (set) Token: 0x06000084 RID: 132 RVA: 0x000030BB File Offset: 0x000012BB
		public string ConfigFilePath { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000030C4 File Offset: 0x000012C4
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000030DB File Offset: 0x000012DB
		public string HexFlasherFilePath { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000030E4 File Offset: 0x000012E4
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000030FB File Offset: 0x000012FB
		public string EdpImageFilePath { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003104 File Offset: 0x00001304
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000311B File Offset: 0x0000131B
		public string MbnImageFilePath { get; set; }

		// Token: 0x0600008B RID: 139 RVA: 0x00003124 File Offset: 0x00001324
		public bool IsAlphaCollins()
		{
			return !string.IsNullOrWhiteSpace(this.MbnImageFilePath) && !string.IsNullOrWhiteSpace(this.HexFlasherFilePath);
		}
	}
}
