using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200002A RID: 42
	[Flags]
	internal enum ShareModes : uint
	{
		// Token: 0x04000115 RID: 277
		Read = 1U,
		// Token: 0x04000116 RID: 278
		Write = 2U,
		// Token: 0x04000117 RID: 279
		Delete = 4U
	}
}
