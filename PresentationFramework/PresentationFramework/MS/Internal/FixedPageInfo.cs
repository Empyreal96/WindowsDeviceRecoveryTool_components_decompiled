using System;

namespace MS.Internal
{
	// Token: 0x020005DA RID: 1498
	internal abstract class FixedPageInfo
	{
		// Token: 0x0600639E RID: 25502
		internal abstract GlyphRunInfo GlyphRunAtPosition(int position);

		// Token: 0x170017E0 RID: 6112
		// (get) Token: 0x0600639F RID: 25503
		internal abstract int GlyphRunCount { get; }
	}
}
