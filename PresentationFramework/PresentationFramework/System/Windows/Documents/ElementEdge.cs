using System;

namespace System.Windows.Documents
{
	// Token: 0x02000341 RID: 833
	[Flags]
	internal enum ElementEdge : byte
	{
		// Token: 0x04001D26 RID: 7462
		BeforeStart = 1,
		// Token: 0x04001D27 RID: 7463
		AfterStart = 2,
		// Token: 0x04001D28 RID: 7464
		BeforeEnd = 4,
		// Token: 0x04001D29 RID: 7465
		AfterEnd = 8
	}
}
