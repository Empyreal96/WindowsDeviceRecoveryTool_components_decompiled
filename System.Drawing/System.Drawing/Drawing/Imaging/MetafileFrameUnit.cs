using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the unit of measurement for the rectangle used to size and position a metafile. This is specified during the creation of the <see cref="T:System.Drawing.Imaging.Metafile" /> object.</summary>
	// Token: 0x020000A6 RID: 166
	public enum MetafileFrameUnit
	{
		/// <summary>The unit of measurement is 1 pixel.</summary>
		// Token: 0x040008EE RID: 2286
		Pixel = 2,
		/// <summary>The unit of measurement is 1 printer's point.</summary>
		// Token: 0x040008EF RID: 2287
		Point,
		/// <summary>The unit of measurement is 1 inch.</summary>
		// Token: 0x040008F0 RID: 2288
		Inch,
		/// <summary>The unit of measurement is 1/300 of an inch.</summary>
		// Token: 0x040008F1 RID: 2289
		Document,
		/// <summary>The unit of measurement is 1 millimeter.</summary>
		// Token: 0x040008F2 RID: 2290
		Millimeter,
		/// <summary>The unit of measurement is 0.01 millimeter. Provided for compatibility with GDI.</summary>
		// Token: 0x040008F3 RID: 2291
		GdiCompatible
	}
}
