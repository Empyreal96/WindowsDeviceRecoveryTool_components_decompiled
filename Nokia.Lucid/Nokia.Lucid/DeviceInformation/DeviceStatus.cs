using System;

namespace Nokia.Lucid.DeviceInformation
{
	// Token: 0x0200001A RID: 26
	[Flags]
	public enum DeviceStatus
	{
		// Token: 0x0400006B RID: 107
		None = 0,
		// Token: 0x0400006C RID: 108
		Present = 1,
		// Token: 0x0400006D RID: 109
		Default = 2,
		// Token: 0x0400006E RID: 110
		Removed = 4
	}
}
