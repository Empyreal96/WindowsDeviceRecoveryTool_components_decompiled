using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the type of point in a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object.</summary>
	// Token: 0x020000CE RID: 206
	public enum PathPointType
	{
		/// <summary>The starting point of a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object.</summary>
		// Token: 0x040009F3 RID: 2547
		Start,
		/// <summary>A line segment.</summary>
		// Token: 0x040009F4 RID: 2548
		Line,
		/// <summary>A default Bézier curve.</summary>
		// Token: 0x040009F5 RID: 2549
		Bezier = 3,
		/// <summary>A mask point.</summary>
		// Token: 0x040009F6 RID: 2550
		PathTypeMask = 7,
		/// <summary>The corresponding segment is dashed.</summary>
		// Token: 0x040009F7 RID: 2551
		DashMode = 16,
		/// <summary>A path marker.</summary>
		// Token: 0x040009F8 RID: 2552
		PathMarker = 32,
		/// <summary>The endpoint of a subpath.</summary>
		// Token: 0x040009F9 RID: 2553
		CloseSubpath = 128,
		/// <summary>A cubic Bézier curve.</summary>
		// Token: 0x040009FA RID: 2554
		Bezier3 = 3
	}
}
