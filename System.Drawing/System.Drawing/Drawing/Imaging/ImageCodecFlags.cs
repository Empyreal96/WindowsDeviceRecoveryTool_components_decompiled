using System;

namespace System.Drawing.Imaging
{
	/// <summary>Provides attributes of an image encoder/decoder (codec).</summary>
	// Token: 0x0200009F RID: 159
	[Flags]
	public enum ImageCodecFlags
	{
		/// <summary>The codec supports encoding (saving).</summary>
		// Token: 0x040008AB RID: 2219
		Encoder = 1,
		/// <summary>The codec supports decoding (reading).</summary>
		// Token: 0x040008AC RID: 2220
		Decoder = 2,
		/// <summary>The codec supports raster images (bitmaps).</summary>
		// Token: 0x040008AD RID: 2221
		SupportBitmap = 4,
		/// <summary>The codec supports vector images (metafiles).</summary>
		// Token: 0x040008AE RID: 2222
		SupportVector = 8,
		/// <summary>The encoder requires a seekable output stream.</summary>
		// Token: 0x040008AF RID: 2223
		SeekableEncode = 16,
		/// <summary>The decoder has blocking behavior during the decoding process.</summary>
		// Token: 0x040008B0 RID: 2224
		BlockingDecode = 32,
		/// <summary>The codec is built into GDI+.</summary>
		// Token: 0x040008B1 RID: 2225
		Builtin = 65536,
		/// <summary>Not used.</summary>
		// Token: 0x040008B2 RID: 2226
		System = 131072,
		/// <summary>Not used.</summary>
		// Token: 0x040008B3 RID: 2227
		User = 262144
	}
}
