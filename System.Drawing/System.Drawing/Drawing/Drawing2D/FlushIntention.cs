using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies whether commands in the graphics stack are terminated (flushed) immediately or executed as soon as possible.</summary>
	// Token: 0x020000BE RID: 190
	public enum FlushIntention
	{
		/// <summary>Specifies that the stack of all graphics operations is flushed immediately.</summary>
		// Token: 0x0400098C RID: 2444
		Flush,
		/// <summary>Specifies that all graphics operations on the stack are executed as soon as possible. This synchronizes the graphics state.</summary>
		// Token: 0x0400098D RID: 2445
		Sync
	}
}
