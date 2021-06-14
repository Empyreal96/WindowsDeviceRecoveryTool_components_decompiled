using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that indicate the alignment of content within a <see cref="T:System.Windows.Forms.DataGridView" /> cell.</summary>
	// Token: 0x020001BC RID: 444
	public enum DataGridViewContentAlignment
	{
		/// <summary>The alignment is not set.</summary>
		// Token: 0x04000CE0 RID: 3296
		NotSet,
		/// <summary>The content is aligned vertically at the top and horizontally at the left of a cell.</summary>
		// Token: 0x04000CE1 RID: 3297
		TopLeft,
		/// <summary>The content is aligned vertically at the top and horizontally at the center of a cell.</summary>
		// Token: 0x04000CE2 RID: 3298
		TopCenter,
		/// <summary>The content is aligned vertically at the top and horizontally at the right of a cell.</summary>
		// Token: 0x04000CE3 RID: 3299
		TopRight = 4,
		/// <summary>The content is aligned vertically at the middle and horizontally at the left of a cell.</summary>
		// Token: 0x04000CE4 RID: 3300
		MiddleLeft = 16,
		/// <summary>The content is aligned at the vertical and horizontal center of a cell.</summary>
		// Token: 0x04000CE5 RID: 3301
		MiddleCenter = 32,
		/// <summary>The content is aligned vertically at the middle and horizontally at the right of a cell.</summary>
		// Token: 0x04000CE6 RID: 3302
		MiddleRight = 64,
		/// <summary>The content is aligned vertically at the bottom and horizontally at the left of a cell.</summary>
		// Token: 0x04000CE7 RID: 3303
		BottomLeft = 256,
		/// <summary>The content is aligned vertically at the bottom and horizontally at the center of a cell.</summary>
		// Token: 0x04000CE8 RID: 3304
		BottomCenter = 512,
		/// <summary>The content is aligned vertically at the bottom and horizontally at the right of a cell.</summary>
		// Token: 0x04000CE9 RID: 3305
		BottomRight = 1024
	}
}
