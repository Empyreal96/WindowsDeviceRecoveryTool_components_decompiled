using System;

namespace System.Windows.Shell
{
	/// <summary>Specifies constants that indicate which edges of the window frame are not owned by the client.</summary>
	// Token: 0x02000150 RID: 336
	[Flags]
	public enum NonClientFrameEdges
	{
		/// <summary>
		///     All edges are owned by the client (value = 0).</summary>
		// Token: 0x0400115C RID: 4444
		None = 0,
		/// <summary>The left edge is not owned by the client (value = 1).</summary>
		// Token: 0x0400115D RID: 4445
		Left = 1,
		/// <summary>The top edge is not owned by the client (value = 2).</summary>
		// Token: 0x0400115E RID: 4446
		Top = 2,
		/// <summary>The right edge is not owned by the client (value = 4).</summary>
		// Token: 0x0400115F RID: 4447
		Right = 4,
		/// <summary>The bottom edge is not owned by the client (value = 8).</summary>
		// Token: 0x04001160 RID: 4448
		Bottom = 8
	}
}
