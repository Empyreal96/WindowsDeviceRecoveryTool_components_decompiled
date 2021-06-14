using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200011C RID: 284
	[Flags]
	public enum EntityStates
	{
		// Token: 0x04000578 RID: 1400
		Detached = 1,
		// Token: 0x04000579 RID: 1401
		Unchanged = 2,
		// Token: 0x0400057A RID: 1402
		Added = 4,
		// Token: 0x0400057B RID: 1403
		Deleted = 8,
		// Token: 0x0400057C RID: 1404
		Modified = 16
	}
}
