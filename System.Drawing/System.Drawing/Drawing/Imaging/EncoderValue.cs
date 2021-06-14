using System;

namespace System.Drawing.Imaging
{
	/// <summary>Used to specify the parameter value passed to a JPEG or TIFF image encoder when using the <see cref="M:System.Drawing.Image.Save(System.String,System.Drawing.Imaging.ImageCodecInfo,System.Drawing.Imaging.EncoderParameters)" /> or <see cref="M:System.Drawing.Image.SaveAdd(System.Drawing.Imaging.EncoderParameters)" /> methods.</summary>
	// Token: 0x0200009C RID: 156
	public enum EncoderValue
	{
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x0400088D RID: 2189
		ColorTypeCMYK,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x0400088E RID: 2190
		ColorTypeYCCK,
		/// <summary>Specifies the LZW compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the Compression category.</summary>
		// Token: 0x0400088F RID: 2191
		CompressionLZW,
		/// <summary>Specifies the CCITT3 compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
		// Token: 0x04000890 RID: 2192
		CompressionCCITT3,
		/// <summary>Specifies the CCITT4 compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
		// Token: 0x04000891 RID: 2193
		CompressionCCITT4,
		/// <summary>Specifies the RLE compression scheme. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
		// Token: 0x04000892 RID: 2194
		CompressionRle,
		/// <summary>Specifies no compression. Can be passed to the TIFF encoder as a parameter that belongs to the compression category.</summary>
		// Token: 0x04000893 RID: 2195
		CompressionNone,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000894 RID: 2196
		ScanMethodInterlaced,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000895 RID: 2197
		ScanMethodNonInterlaced,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000896 RID: 2198
		VersionGif87,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000897 RID: 2199
		VersionGif89,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000898 RID: 2200
		RenderProgressive,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x04000899 RID: 2201
		RenderNonProgressive,
		/// <summary>Specifies that the image is to be rotated clockwise 90 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
		// Token: 0x0400089A RID: 2202
		TransformRotate90,
		/// <summary>Specifies that the image is to be rotated 180 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
		// Token: 0x0400089B RID: 2203
		TransformRotate180,
		/// <summary>Specifies that the image is to be rotated clockwise 270 degrees about its center. Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
		// Token: 0x0400089C RID: 2204
		TransformRotate270,
		/// <summary>Specifies that the image is to be flipped horizontally (about the vertical axis). Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
		// Token: 0x0400089D RID: 2205
		TransformFlipHorizontal,
		/// <summary>Specifies that the image is to be flipped vertically (about the horizontal axis). Can be passed to the JPEG encoder as a parameter that belongs to the transformation category.</summary>
		// Token: 0x0400089E RID: 2206
		TransformFlipVertical,
		/// <summary>Specifies that the image has more than one frame (page). Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
		// Token: 0x0400089F RID: 2207
		MultiFrame,
		/// <summary>Specifies the last frame in a multiple-frame image. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
		// Token: 0x040008A0 RID: 2208
		LastFrame,
		/// <summary>Specifies that a multiple-frame file or stream should be closed. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
		// Token: 0x040008A1 RID: 2209
		Flush,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x040008A2 RID: 2210
		FrameDimensionTime,
		/// <summary>Not used in GDI+ version 1.0.</summary>
		// Token: 0x040008A3 RID: 2211
		FrameDimensionResolution,
		/// <summary>Specifies that a frame is to be added to the page dimension of an image. Can be passed to the TIFF encoder as a parameter that belongs to the save flag category.</summary>
		// Token: 0x040008A4 RID: 2212
		FrameDimensionPage
	}
}
