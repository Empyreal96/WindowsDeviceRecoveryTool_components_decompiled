using System;

namespace System.Windows.Controls
{
	/// <summary>Identifies the various parts of a selection adorner on an <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
	// Token: 0x020004EC RID: 1260
	public enum InkCanvasSelectionHitResult
	{
		/// <summary>No part of the selection adorner.</summary>
		// Token: 0x04002BFD RID: 11261
		None,
		/// <summary>The upper left handle of the selection adorner.</summary>
		// Token: 0x04002BFE RID: 11262
		TopLeft,
		/// <summary>The upper middle handle of the selection adorner.</summary>
		// Token: 0x04002BFF RID: 11263
		Top,
		/// <summary>The upper right handle of the selection adorner.</summary>
		// Token: 0x04002C00 RID: 11264
		TopRight,
		/// <summary>The middle handle on the right edge of the selection adorner.</summary>
		// Token: 0x04002C01 RID: 11265
		Right,
		/// <summary>The lower right handle of the selection adorner.</summary>
		// Token: 0x04002C02 RID: 11266
		BottomRight,
		/// <summary>The lower middle handle of the selection adorner.</summary>
		// Token: 0x04002C03 RID: 11267
		Bottom,
		/// <summary>The lower left handle of the selection adorner.</summary>
		// Token: 0x04002C04 RID: 11268
		BottomLeft,
		/// <summary>The middle handle on the left edge of the selection adorner.</summary>
		// Token: 0x04002C05 RID: 11269
		Left,
		/// <summary>The area within the bounds of the selection adorner.</summary>
		// Token: 0x04002C06 RID: 11270
		Selection
	}
}
