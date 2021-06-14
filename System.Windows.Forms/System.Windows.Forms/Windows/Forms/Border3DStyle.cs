using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the style of a three-dimensional border.</summary>
	// Token: 0x0200012F RID: 303
	[ComVisible(true)]
	public enum Border3DStyle
	{
		/// <summary>The border is drawn outside the specified rectangle, preserving the dimensions of the rectangle for drawing.</summary>
		// Token: 0x0400065C RID: 1628
		Adjust = 8192,
		/// <summary>The inner and outer edges of the border have a raised appearance.</summary>
		// Token: 0x0400065D RID: 1629
		Bump = 9,
		/// <summary>The inner and outer edges of the border have an etched appearance.</summary>
		// Token: 0x0400065E RID: 1630
		Etched = 6,
		/// <summary>The border has no three-dimensional effects.</summary>
		// Token: 0x0400065F RID: 1631
		Flat = 16394,
		/// <summary>The border has raised inner and outer edges.</summary>
		// Token: 0x04000660 RID: 1632
		Raised = 5,
		/// <summary>The border has a raised inner edge and no outer edge.</summary>
		// Token: 0x04000661 RID: 1633
		RaisedInner = 4,
		/// <summary>The border has a raised outer edge and no inner edge.</summary>
		// Token: 0x04000662 RID: 1634
		RaisedOuter = 1,
		/// <summary>The border has sunken inner and outer edges.</summary>
		// Token: 0x04000663 RID: 1635
		Sunken = 10,
		/// <summary>The border has a sunken inner edge and no outer edge.</summary>
		// Token: 0x04000664 RID: 1636
		SunkenInner = 8,
		/// <summary>The border has a sunken outer edge and no inner edge.</summary>
		// Token: 0x04000665 RID: 1637
		SunkenOuter = 2
	}
}
