using System;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x0200002B RID: 43
	internal enum CreateFileDisposition : uint
	{
		// Token: 0x04000119 RID: 281
		CreateNew = 1U,
		// Token: 0x0400011A RID: 282
		CreateAlways,
		// Token: 0x0400011B RID: 283
		CreateExisting,
		// Token: 0x0400011C RID: 284
		OpenAlways,
		// Token: 0x0400011D RID: 285
		TruncateExisting
	}
}
