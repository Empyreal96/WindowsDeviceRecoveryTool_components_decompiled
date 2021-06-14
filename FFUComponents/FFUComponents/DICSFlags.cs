using System;

namespace FFUComponents
{
	// Token: 0x0200002F RID: 47
	[Flags]
	internal enum DICSFlags : uint
	{
		// Token: 0x04000095 RID: 149
		Global = 1U,
		// Token: 0x04000096 RID: 150
		ConfigSpecific = 2U,
		// Token: 0x04000097 RID: 151
		ConfigGeneral = 4U
	}
}
