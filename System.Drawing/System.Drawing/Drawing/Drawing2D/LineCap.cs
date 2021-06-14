using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the available cap styles with which a <see cref="T:System.Drawing.Pen" /> object can end a line.</summary>
	// Token: 0x020000C8 RID: 200
	public enum LineCap
	{
		/// <summary>Specifies a flat line cap.</summary>
		// Token: 0x040009DC RID: 2524
		Flat,
		/// <summary>Specifies a square line cap.</summary>
		// Token: 0x040009DD RID: 2525
		Square,
		/// <summary>Specifies a round line cap.</summary>
		// Token: 0x040009DE RID: 2526
		Round,
		/// <summary>Specifies a triangular line cap.</summary>
		// Token: 0x040009DF RID: 2527
		Triangle,
		/// <summary>Specifies no anchor.</summary>
		// Token: 0x040009E0 RID: 2528
		NoAnchor = 16,
		/// <summary>Specifies a square anchor line cap.</summary>
		// Token: 0x040009E1 RID: 2529
		SquareAnchor,
		/// <summary>Specifies a round anchor cap.</summary>
		// Token: 0x040009E2 RID: 2530
		RoundAnchor,
		/// <summary>Specifies a diamond anchor cap.</summary>
		// Token: 0x040009E3 RID: 2531
		DiamondAnchor,
		/// <summary>Specifies an arrow-shaped anchor cap.</summary>
		// Token: 0x040009E4 RID: 2532
		ArrowAnchor,
		/// <summary>Specifies a custom line cap.</summary>
		// Token: 0x040009E5 RID: 2533
		Custom = 255,
		/// <summary>Specifies a mask used to check whether a line cap is an anchor cap.</summary>
		// Token: 0x040009E6 RID: 2534
		AnchorMask = 240
	}
}
