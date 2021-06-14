using System;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the attributes of the pixel data contained in an <see cref="T:System.Drawing.Image" /> object. The <see cref="P:System.Drawing.Image.Flags" /> property returns a member of this enumeration.</summary>
	// Token: 0x020000A2 RID: 162
	[Flags]
	public enum ImageFlags
	{
		/// <summary>There is no format information.</summary>
		// Token: 0x040008CD RID: 2253
		None = 0,
		/// <summary>The pixel data is scalable.</summary>
		// Token: 0x040008CE RID: 2254
		Scalable = 1,
		/// <summary>The pixel data contains alpha information.</summary>
		// Token: 0x040008CF RID: 2255
		HasAlpha = 2,
		/// <summary>Specifies that the pixel data has alpha values other than 0 (transparent) and 255 (opaque).</summary>
		// Token: 0x040008D0 RID: 2256
		HasTranslucent = 4,
		/// <summary>The pixel data is partially scalable, but there are some limitations.</summary>
		// Token: 0x040008D1 RID: 2257
		PartiallyScalable = 8,
		/// <summary>The pixel data uses an RGB color space.</summary>
		// Token: 0x040008D2 RID: 2258
		ColorSpaceRgb = 16,
		/// <summary>The pixel data uses a CMYK color space.</summary>
		// Token: 0x040008D3 RID: 2259
		ColorSpaceCmyk = 32,
		/// <summary>The pixel data is grayscale.</summary>
		// Token: 0x040008D4 RID: 2260
		ColorSpaceGray = 64,
		/// <summary>Specifies that the image is stored using a YCBCR color space.</summary>
		// Token: 0x040008D5 RID: 2261
		ColorSpaceYcbcr = 128,
		/// <summary>Specifies that the image is stored using a YCCK color space.</summary>
		// Token: 0x040008D6 RID: 2262
		ColorSpaceYcck = 256,
		/// <summary>Specifies that dots per inch information is stored in the image.</summary>
		// Token: 0x040008D7 RID: 2263
		HasRealDpi = 4096,
		/// <summary>Specifies that the pixel size is stored in the image.</summary>
		// Token: 0x040008D8 RID: 2264
		HasRealPixelSize = 8192,
		/// <summary>The pixel data is read-only.</summary>
		// Token: 0x040008D9 RID: 2265
		ReadOnly = 65536,
		/// <summary>The pixel data can be cached for faster access.</summary>
		// Token: 0x040008DA RID: 2266
		Caching = 131072
	}
}
