using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200011D RID: 285
	[Flags]
	public enum SaveChangesOptions
	{
		// Token: 0x0400057E RID: 1406
		None = 0,
		// Token: 0x0400057F RID: 1407
		Batch = 1,
		// Token: 0x04000580 RID: 1408
		ContinueOnError = 2,
		// Token: 0x04000581 RID: 1409
		ReplaceOnUpdate = 4,
		// Token: 0x04000582 RID: 1410
		PatchOnUpdate = 8,
		// Token: 0x04000583 RID: 1411
		BatchWithIndependentOperations = 16
	}
}
