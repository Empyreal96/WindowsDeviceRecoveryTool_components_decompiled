using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000029 RID: 41
	[Flags]
	internal enum AccessRights : uint
	{
		// Token: 0x04000110 RID: 272
		Read = 2147483648U,
		// Token: 0x04000111 RID: 273
		Write = 1073741824U,
		// Token: 0x04000112 RID: 274
		Execute = 536870912U,
		// Token: 0x04000113 RID: 275
		All = 268435456U
	}
}
