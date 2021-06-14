using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the sides of a rectangle to apply a three-dimensional border to.</summary>
	// Token: 0x0200012E RID: 302
	[ComVisible(true)]
	[Flags]
	public enum Border3DSide
	{
		/// <summary>A three-dimensional border on the left edge of the rectangle.</summary>
		// Token: 0x04000655 RID: 1621
		Left = 1,
		/// <summary>A three-dimensional border on the top edge of the rectangle.</summary>
		// Token: 0x04000656 RID: 1622
		Top = 2,
		/// <summary>A three-dimensional border on the right side of the rectangle.</summary>
		// Token: 0x04000657 RID: 1623
		Right = 4,
		/// <summary>A three-dimensional border on the bottom side of the rectangle.</summary>
		// Token: 0x04000658 RID: 1624
		Bottom = 8,
		/// <summary>The interior of the rectangle is filled with the color defined for three-dimensional controls instead of the background color for the form.</summary>
		// Token: 0x04000659 RID: 1625
		Middle = 2048,
		/// <summary>A three-dimensional border on all four sides of the rectangle. The middle of the rectangle is filled with the color defined for three-dimensional controls.</summary>
		// Token: 0x0400065A RID: 1626
		All = 2063
	}
}
