using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies how to join consecutive line or curve segments in a figure (subpath) contained in a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> object.</summary>
	// Token: 0x020000C9 RID: 201
	public enum LineJoin
	{
		/// <summary>Specifies a mitered join. This produces a sharp corner or a clipped corner, depending on whether the length of the miter exceeds the miter limit.</summary>
		// Token: 0x040009E8 RID: 2536
		Miter,
		/// <summary>Specifies a beveled join. This produces a diagonal corner.</summary>
		// Token: 0x040009E9 RID: 2537
		Bevel,
		/// <summary>Specifies a circular join. This produces a smooth, circular arc between the lines.</summary>
		// Token: 0x040009EA RID: 2538
		Round,
		/// <summary>Specifies a mitered join. This produces a sharp corner or a beveled corner, depending on whether the length of the miter exceeds the miter limit.</summary>
		// Token: 0x040009EB RID: 2539
		MiterClipped
	}
}
