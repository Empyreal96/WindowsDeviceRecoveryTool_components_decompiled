using System;

namespace System.Drawing
{
	/// <summary>Specifies the alignment of a text string relative to its layout rectangle.</summary>
	// Token: 0x02000044 RID: 68
	public enum StringAlignment
	{
		/// <summary>Specifies the text be aligned near the layout. In a left-to-right layout, the near position is left. In a right-to-left layout, the near position is right.</summary>
		// Token: 0x04000578 RID: 1400
		Near,
		/// <summary>Specifies that text is aligned in the center of the layout rectangle.</summary>
		// Token: 0x04000579 RID: 1401
		Center,
		/// <summary>Specifies that text is aligned far from the origin position of the layout rectangle. In a left-to-right layout, the far position is right. In a right-to-left layout, the far position is left.</summary>
		// Token: 0x0400057A RID: 1402
		Far
	}
}
