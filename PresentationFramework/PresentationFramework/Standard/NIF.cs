using System;

namespace Standard
{
	// Token: 0x02000038 RID: 56
	[Flags]
	internal enum NIF : uint
	{
		// Token: 0x040003D6 RID: 982
		MESSAGE = 1U,
		// Token: 0x040003D7 RID: 983
		ICON = 2U,
		// Token: 0x040003D8 RID: 984
		TIP = 4U,
		// Token: 0x040003D9 RID: 985
		STATE = 8U,
		// Token: 0x040003DA RID: 986
		INFO = 16U,
		// Token: 0x040003DB RID: 987
		GUID = 32U,
		// Token: 0x040003DC RID: 988
		REALTIME = 64U,
		// Token: 0x040003DD RID: 989
		SHOWTIP = 128U,
		// Token: 0x040003DE RID: 990
		XP_MASK = 59U,
		// Token: 0x040003DF RID: 991
		VISTA_MASK = 251U
	}
}
