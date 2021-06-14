using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the type of action used to raise the <see cref="E:System.Windows.Forms.ScrollBar.Scroll" /> event.</summary>
	// Token: 0x0200034B RID: 843
	[ComVisible(true)]
	public enum ScrollEventType
	{
		/// <summary>The scroll box was moved a small distance. The user clicked the left(horizontal) or top(vertical) scroll arrow, or pressed the UP ARROW key.</summary>
		// Token: 0x0400207B RID: 8315
		SmallDecrement,
		/// <summary>The scroll box was moved a small distance. The user clicked the right(horizontal) or bottom(vertical) scroll arrow, or pressed the DOWN ARROW key.</summary>
		// Token: 0x0400207C RID: 8316
		SmallIncrement,
		/// <summary>The scroll box moved a large distance. The user clicked the scroll bar to the left(horizontal) or above(vertical) the scroll box, or pressed the PAGE UP key.</summary>
		// Token: 0x0400207D RID: 8317
		LargeDecrement,
		/// <summary>The scroll box moved a large distance. The user clicked the scroll bar to the right(horizontal) or below(vertical) the scroll box, or pressed the PAGE DOWN key.</summary>
		// Token: 0x0400207E RID: 8318
		LargeIncrement,
		/// <summary>The scroll box was moved.</summary>
		// Token: 0x0400207F RID: 8319
		ThumbPosition,
		/// <summary>The scroll box is currently being moved.</summary>
		// Token: 0x04002080 RID: 8320
		ThumbTrack,
		/// <summary>The scroll box was moved to the <see cref="P:System.Windows.Forms.ScrollBar.Minimum" /> position.</summary>
		// Token: 0x04002081 RID: 8321
		First,
		/// <summary>The scroll box was moved to the <see cref="P:System.Windows.Forms.ScrollBar.Maximum" /> position.</summary>
		// Token: 0x04002082 RID: 8322
		Last,
		/// <summary>The scroll box has stopped moving.</summary>
		// Token: 0x04002083 RID: 8323
		EndScroll
	}
}
