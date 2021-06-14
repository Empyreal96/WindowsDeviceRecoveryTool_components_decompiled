using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the starting position that the system uses to arrange minimized windows.</summary>
	// Token: 0x02000116 RID: 278
	[Flags]
	public enum ArrangeStartingPosition
	{
		/// <summary>Starts at the lower-left corner of the screen, which is the default position.</summary>
		// Token: 0x0400054E RID: 1358
		BottomLeft = 0,
		/// <summary>Starts at the lower-right corner of the screen.</summary>
		// Token: 0x0400054F RID: 1359
		BottomRight = 1,
		/// <summary>Hides minimized windows by moving them off the visible area of the screen.</summary>
		// Token: 0x04000550 RID: 1360
		Hide = 8,
		/// <summary>Starts at the upper-left corner of the screen.</summary>
		// Token: 0x04000551 RID: 1361
		TopLeft = 2,
		/// <summary>Starts at the upper-right corner of the screen.</summary>
		// Token: 0x04000552 RID: 1362
		TopRight = 3
	}
}
