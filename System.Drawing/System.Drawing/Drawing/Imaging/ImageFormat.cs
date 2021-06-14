using System;
using System.ComponentModel;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the file format of the image. Not inheritable.</summary>
	// Token: 0x020000A3 RID: 163
	[TypeConverter(typeof(ImageFormatConverter))]
	public sealed class ImageFormat
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ImageFormat" /> class by using the specified <see cref="T:System.Guid" /> structure.</summary>
		/// <param name="guid">The <see cref="T:System.Guid" /> structure that specifies a particular image format. </param>
		// Token: 0x060009A1 RID: 2465 RVA: 0x00024724 File Offset: 0x00022924
		public ImageFormat(Guid guid)
		{
			this.guid = guid;
		}

		/// <summary>Gets a <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
		/// <returns>A <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00024733 File Offset: 0x00022933
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		/// <summary>Gets the format of a bitmap in memory.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the format of a bitmap in memory.</returns>
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0002473B File Offset: 0x0002293B
		public static ImageFormat MemoryBmp
		{
			get
			{
				return ImageFormat.memoryBMP;
			}
		}

		/// <summary>Gets the bitmap (BMP) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the bitmap image format.</returns>
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x00024742 File Offset: 0x00022942
		public static ImageFormat Bmp
		{
			get
			{
				return ImageFormat.bmp;
			}
		}

		/// <summary>Gets the enhanced metafile (EMF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the enhanced metafile image format.</returns>
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x00024749 File Offset: 0x00022949
		public static ImageFormat Emf
		{
			get
			{
				return ImageFormat.emf;
			}
		}

		/// <summary>Gets the Windows metafile (WMF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows metafile image format.</returns>
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x00024750 File Offset: 0x00022950
		public static ImageFormat Wmf
		{
			get
			{
				return ImageFormat.wmf;
			}
		}

		/// <summary>Gets the Graphics Interchange Format (GIF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the GIF image format.</returns>
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00024757 File Offset: 0x00022957
		public static ImageFormat Gif
		{
			get
			{
				return ImageFormat.gif;
			}
		}

		/// <summary>Gets the Joint Photographic Experts Group (JPEG) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the JPEG image format.</returns>
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0002475E File Offset: 0x0002295E
		public static ImageFormat Jpeg
		{
			get
			{
				return ImageFormat.jpeg;
			}
		}

		/// <summary>Gets the W3C Portable Network Graphics (PNG) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the PNG image format.</returns>
		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x00024765 File Offset: 0x00022965
		public static ImageFormat Png
		{
			get
			{
				return ImageFormat.png;
			}
		}

		/// <summary>Gets the Tagged Image File Format (TIFF) image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the TIFF image format.</returns>
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0002476C File Offset: 0x0002296C
		public static ImageFormat Tiff
		{
			get
			{
				return ImageFormat.tiff;
			}
		}

		/// <summary>Gets the Exchangeable Image File (Exif) format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Exif format.</returns>
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00024773 File Offset: 0x00022973
		public static ImageFormat Exif
		{
			get
			{
				return ImageFormat.exif;
			}
		}

		/// <summary>Gets the Windows icon image format.</summary>
		/// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the Windows icon image format.</returns>
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0002477A File Offset: 0x0002297A
		public static ImageFormat Icon
		{
			get
			{
				return ImageFormat.icon;
			}
		}

		/// <summary>Returns a value that indicates whether the specified object is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
		/// <param name="o">The object to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="o" /> is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060009AD RID: 2477 RVA: 0x00024784 File Offset: 0x00022984
		public override bool Equals(object o)
		{
			ImageFormat imageFormat = o as ImageFormat;
			return imageFormat != null && this.guid == imageFormat.guid;
		}

		/// <summary>Returns a hash code value that represents this object.</summary>
		/// <returns>A hash code that represents this object.</returns>
		// Token: 0x060009AE RID: 2478 RVA: 0x000247AE File Offset: 0x000229AE
		public override int GetHashCode()
		{
			return this.guid.GetHashCode();
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000247C4 File Offset: 0x000229C4
		internal ImageCodecInfo FindEncoder()
		{
			ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
			{
				if (imageCodecInfo.FormatID.Equals(this.guid))
				{
					return imageCodecInfo;
				}
			}
			return null;
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object to a human-readable string.</summary>
		/// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
		// Token: 0x060009B0 RID: 2480 RVA: 0x00024808 File Offset: 0x00022A08
		public override string ToString()
		{
			if (this == ImageFormat.memoryBMP)
			{
				return "MemoryBMP";
			}
			if (this == ImageFormat.bmp)
			{
				return "Bmp";
			}
			if (this == ImageFormat.emf)
			{
				return "Emf";
			}
			if (this == ImageFormat.wmf)
			{
				return "Wmf";
			}
			if (this == ImageFormat.gif)
			{
				return "Gif";
			}
			if (this == ImageFormat.jpeg)
			{
				return "Jpeg";
			}
			if (this == ImageFormat.png)
			{
				return "Png";
			}
			if (this == ImageFormat.tiff)
			{
				return "Tiff";
			}
			if (this == ImageFormat.exif)
			{
				return "Exif";
			}
			if (this == ImageFormat.icon)
			{
				return "Icon";
			}
			return "[ImageFormat: " + this.guid + "]";
		}

		// Token: 0x040008DB RID: 2267
		private static ImageFormat memoryBMP = new ImageFormat(new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008DC RID: 2268
		private static ImageFormat bmp = new ImageFormat(new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008DD RID: 2269
		private static ImageFormat emf = new ImageFormat(new Guid("{b96b3cac-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008DE RID: 2270
		private static ImageFormat wmf = new ImageFormat(new Guid("{b96b3cad-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008DF RID: 2271
		private static ImageFormat jpeg = new ImageFormat(new Guid("{b96b3cae-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E0 RID: 2272
		private static ImageFormat png = new ImageFormat(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E1 RID: 2273
		private static ImageFormat gif = new ImageFormat(new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E2 RID: 2274
		private static ImageFormat tiff = new ImageFormat(new Guid("{b96b3cb1-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E3 RID: 2275
		private static ImageFormat exif = new ImageFormat(new Guid("{b96b3cb2-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E4 RID: 2276
		private static ImageFormat photoCD = new ImageFormat(new Guid("{b96b3cb3-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E5 RID: 2277
		private static ImageFormat flashPIX = new ImageFormat(new Guid("{b96b3cb4-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E6 RID: 2278
		private static ImageFormat icon = new ImageFormat(new Guid("{b96b3cb5-0728-11d3-9d7b-0000f81ef32e}"));

		// Token: 0x040008E7 RID: 2279
		private Guid guid;
	}
}
