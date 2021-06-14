using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Specifies which edges of a visual style element to draw.</summary>
	// Token: 0x0200046F RID: 1135
	[Flags]
	public enum Edges
	{
		/// <summary>The left edge of the element.</summary>
		// Token: 0x04003283 RID: 12931
		Left = 1,
		/// <summary>The top edge of the element.</summary>
		// Token: 0x04003284 RID: 12932
		Top = 2,
		/// <summary>The right edge of the element.</summary>
		// Token: 0x04003285 RID: 12933
		Right = 4,
		/// <summary>The bottom edge of the element.</summary>
		// Token: 0x04003286 RID: 12934
		Bottom = 8,
		/// <summary>A diagonal border.</summary>
		// Token: 0x04003287 RID: 12935
		Diagonal = 16
	}
}
