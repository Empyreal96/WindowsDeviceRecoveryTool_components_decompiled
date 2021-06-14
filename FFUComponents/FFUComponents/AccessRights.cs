using System;

namespace FFUComponents
{
	// Token: 0x02000027 RID: 39
	[Flags]
	internal enum AccessRights : uint
	{
		// Token: 0x04000066 RID: 102
		Read = 2147483648U,
		// Token: 0x04000067 RID: 103
		Write = 1073741824U,
		// Token: 0x04000068 RID: 104
		Execute = 536870912U,
		// Token: 0x04000069 RID: 105
		All = 268435456U
	}
}
