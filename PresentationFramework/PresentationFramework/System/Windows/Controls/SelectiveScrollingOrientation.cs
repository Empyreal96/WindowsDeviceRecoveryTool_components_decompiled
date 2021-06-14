using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the direction that <see cref="T:System.Windows.Controls.Primitives.SelectiveScrollingGrid" /> panels can scroll.</summary>
	// Token: 0x02000530 RID: 1328
	public enum SelectiveScrollingOrientation
	{
		/// <summary>The panel does not scroll.</summary>
		// Token: 0x04002E1E RID: 11806
		None,
		/// <summary>The panel scrolls in the horizontal direction only.</summary>
		// Token: 0x04002E1F RID: 11807
		Horizontal,
		/// <summary>The panel scrolls in the vertical direction only.</summary>
		// Token: 0x04002E20 RID: 11808
		Vertical,
		/// <summary>The panel scrolls in both the horizontal and vertical direction.</summary>
		// Token: 0x04002E21 RID: 11809
		Both
	}
}
