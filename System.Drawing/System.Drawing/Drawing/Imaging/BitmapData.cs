using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Specifies the attributes of a bitmap image. The <see cref="T:System.Drawing.Imaging.BitmapData" /> class is used by the <see cref="Overload:System.Drawing.Bitmap.LockBits" /> and <see cref="M:System.Drawing.Bitmap.UnlockBits(System.Drawing.Imaging.BitmapData)" /> methods of the <see cref="T:System.Drawing.Bitmap" /> class. Not inheritable. </summary>
	// Token: 0x0200008C RID: 140
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BitmapData
	{
		/// <summary>Gets or sets the pixel width of the <see cref="T:System.Drawing.Bitmap" /> object. This can also be thought of as the number of pixels in one scan line.</summary>
		/// <returns>The pixel width of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x000224AB File Offset: 0x000206AB
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x000224B3 File Offset: 0x000206B3
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		/// <summary>Gets or sets the pixel height of the <see cref="T:System.Drawing.Bitmap" /> object. Also sometimes referred to as the number of scan lines.</summary>
		/// <returns>The pixel height of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x000224BC File Offset: 0x000206BC
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x000224C4 File Offset: 0x000206C4
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>Gets or sets the stride width (also called scan width) of the <see cref="T:System.Drawing.Bitmap" /> object.</summary>
		/// <returns>The stride width, in bytes, of the <see cref="T:System.Drawing.Bitmap" /> object.</returns>
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x000224CD File Offset: 0x000206CD
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x000224D5 File Offset: 0x000206D5
		public int Stride
		{
			get
			{
				return this.stride;
			}
			set
			{
				this.stride = value;
			}
		}

		/// <summary>Gets or sets the format of the pixel information in the <see cref="T:System.Drawing.Bitmap" /> object that returned this <see cref="T:System.Drawing.Imaging.BitmapData" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Imaging.PixelFormat" /> that specifies the format of the pixel information in the associated <see cref="T:System.Drawing.Bitmap" /> object.</returns>
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000224DE File Offset: 0x000206DE
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x000224E8 File Offset: 0x000206E8
		public PixelFormat PixelFormat
		{
			get
			{
				return (PixelFormat)this.pixelFormat;
			}
			set
			{
				if (value <= PixelFormat.Format8bppIndexed)
				{
					if (value <= PixelFormat.Format16bppRgb565)
					{
						if (value <= PixelFormat.Max)
						{
							if (value == PixelFormat.Undefined || value == PixelFormat.Max)
							{
								goto IL_125;
							}
						}
						else if (value == PixelFormat.Indexed || value == PixelFormat.Gdi || value - PixelFormat.Format16bppRgb555 <= 1)
						{
							goto IL_125;
						}
					}
					else if (value <= PixelFormat.Format32bppRgb)
					{
						if (value == PixelFormat.Format24bppRgb || value == PixelFormat.Format32bppRgb)
						{
							goto IL_125;
						}
					}
					else if (value == PixelFormat.Format1bppIndexed || value == PixelFormat.Format4bppIndexed || value == PixelFormat.Format8bppIndexed)
					{
						goto IL_125;
					}
				}
				else if (value <= PixelFormat.Extended)
				{
					if (value <= PixelFormat.Format16bppArgb1555)
					{
						if (value == PixelFormat.Alpha || value == PixelFormat.Format16bppArgb1555)
						{
							goto IL_125;
						}
					}
					else if (value == PixelFormat.PAlpha || value == PixelFormat.Format32bppPArgb || value == PixelFormat.Extended)
					{
						goto IL_125;
					}
				}
				else if (value <= PixelFormat.Format64bppPArgb)
				{
					if (value == PixelFormat.Format16bppGrayScale || value == PixelFormat.Format48bppRgb || value == PixelFormat.Format64bppPArgb)
					{
						goto IL_125;
					}
				}
				else if (value == PixelFormat.Canonical || value == PixelFormat.Format32bppArgb || value == PixelFormat.Format64bppArgb)
				{
					goto IL_125;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(PixelFormat));
				IL_125:
				this.pixelFormat = (int)value;
			}
		}

		/// <summary>Gets or sets the address of the first pixel data in the bitmap. This can also be thought of as the first scan line in the bitmap.</summary>
		/// <returns>The address of the first pixel data in the bitmap.</returns>
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00022621 File Offset: 0x00020821
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x00022629 File Offset: 0x00020829
		public IntPtr Scan0
		{
			get
			{
				return this.scan0;
			}
			set
			{
				this.scan0 = value;
			}
		}

		/// <summary>Reserved. Do not use.</summary>
		/// <returns>Reserved. Do not use.</returns>
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00022632 File Offset: 0x00020832
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0002263A File Offset: 0x0002083A
		public int Reserved
		{
			get
			{
				return this.reserved;
			}
			set
			{
				this.reserved = value;
			}
		}

		// Token: 0x04000733 RID: 1843
		private int width;

		// Token: 0x04000734 RID: 1844
		private int height;

		// Token: 0x04000735 RID: 1845
		private int stride;

		// Token: 0x04000736 RID: 1846
		private int pixelFormat;

		// Token: 0x04000737 RID: 1847
		private IntPtr scan0;

		// Token: 0x04000738 RID: 1848
		private int reserved;
	}
}
