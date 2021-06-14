using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000B9 RID: 185
	public enum LeaseAction
	{
		// Token: 0x0400043F RID: 1087
		Acquire,
		// Token: 0x04000440 RID: 1088
		Renew,
		// Token: 0x04000441 RID: 1089
		Release,
		// Token: 0x04000442 RID: 1090
		Break,
		// Token: 0x04000443 RID: 1091
		Change
	}
}
