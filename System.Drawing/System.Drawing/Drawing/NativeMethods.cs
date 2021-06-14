using System;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000027 RID: 39
	internal class NativeMethods
	{
		// Token: 0x0400025C RID: 604
		internal static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x0400025D RID: 605
		public const byte PC_NOCOLLAPSE = 4;

		// Token: 0x0400025E RID: 606
		public const int MAX_PATH = 260;

		// Token: 0x0400025F RID: 607
		internal const int SM_REMOTESESSION = 4096;

		// Token: 0x04000260 RID: 608
		internal const int OBJ_DC = 3;

		// Token: 0x04000261 RID: 609
		internal const int OBJ_METADC = 4;

		// Token: 0x04000262 RID: 610
		internal const int OBJ_MEMDC = 10;

		// Token: 0x04000263 RID: 611
		internal const int OBJ_ENHMETADC = 12;

		// Token: 0x04000264 RID: 612
		internal const int DIB_RGB_COLORS = 0;

		// Token: 0x04000265 RID: 613
		internal const int BI_BITFIELDS = 3;

		// Token: 0x04000266 RID: 614
		internal const int BI_RGB = 0;

		// Token: 0x04000267 RID: 615
		internal const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04000268 RID: 616
		internal const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x04000269 RID: 617
		internal const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x0400026A RID: 618
		internal const int DEFAULT_GUI_FONT = 17;

		// Token: 0x020000FD RID: 253
		public enum RegionFlags
		{
			// Token: 0x04000AF9 RID: 2809
			ERROR,
			// Token: 0x04000AFA RID: 2810
			NULLREGION,
			// Token: 0x04000AFB RID: 2811
			SIMPLEREGION,
			// Token: 0x04000AFC RID: 2812
			COMPLEXREGION
		}

		// Token: 0x020000FE RID: 254
		internal struct BITMAPINFO_FLAT
		{
			// Token: 0x04000AFD RID: 2813
			public int bmiHeader_biSize;

			// Token: 0x04000AFE RID: 2814
			public int bmiHeader_biWidth;

			// Token: 0x04000AFF RID: 2815
			public int bmiHeader_biHeight;

			// Token: 0x04000B00 RID: 2816
			public short bmiHeader_biPlanes;

			// Token: 0x04000B01 RID: 2817
			public short bmiHeader_biBitCount;

			// Token: 0x04000B02 RID: 2818
			public int bmiHeader_biCompression;

			// Token: 0x04000B03 RID: 2819
			public int bmiHeader_biSizeImage;

			// Token: 0x04000B04 RID: 2820
			public int bmiHeader_biXPelsPerMeter;

			// Token: 0x04000B05 RID: 2821
			public int bmiHeader_biYPelsPerMeter;

			// Token: 0x04000B06 RID: 2822
			public int bmiHeader_biClrUsed;

			// Token: 0x04000B07 RID: 2823
			public int bmiHeader_biClrImportant;

			// Token: 0x04000B08 RID: 2824
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
			public byte[] bmiColors;
		}

		// Token: 0x020000FF RID: 255
		[StructLayout(LayoutKind.Sequential)]
		internal class BITMAPINFOHEADER
		{
			// Token: 0x04000B09 RID: 2825
			public int biSize = 40;

			// Token: 0x04000B0A RID: 2826
			public int biWidth;

			// Token: 0x04000B0B RID: 2827
			public int biHeight;

			// Token: 0x04000B0C RID: 2828
			public short biPlanes;

			// Token: 0x04000B0D RID: 2829
			public short biBitCount;

			// Token: 0x04000B0E RID: 2830
			public int biCompression;

			// Token: 0x04000B0F RID: 2831
			public int biSizeImage;

			// Token: 0x04000B10 RID: 2832
			public int biXPelsPerMeter;

			// Token: 0x04000B11 RID: 2833
			public int biYPelsPerMeter;

			// Token: 0x04000B12 RID: 2834
			public int biClrUsed;

			// Token: 0x04000B13 RID: 2835
			public int biClrImportant;
		}

		// Token: 0x02000100 RID: 256
		internal struct PALETTEENTRY
		{
			// Token: 0x04000B14 RID: 2836
			public byte peRed;

			// Token: 0x04000B15 RID: 2837
			public byte peGreen;

			// Token: 0x04000B16 RID: 2838
			public byte peBlue;

			// Token: 0x04000B17 RID: 2839
			public byte peFlags;
		}

		// Token: 0x02000101 RID: 257
		internal struct RGBQUAD
		{
			// Token: 0x04000B18 RID: 2840
			public byte rgbBlue;

			// Token: 0x04000B19 RID: 2841
			public byte rgbGreen;

			// Token: 0x04000B1A RID: 2842
			public byte rgbRed;

			// Token: 0x04000B1B RID: 2843
			public byte rgbReserved;
		}

		// Token: 0x02000102 RID: 258
		[StructLayout(LayoutKind.Sequential)]
		internal class NONCLIENTMETRICS
		{
			// Token: 0x04000B1C RID: 2844
			public int cbSize = Marshal.SizeOf(typeof(NativeMethods.NONCLIENTMETRICS));

			// Token: 0x04000B1D RID: 2845
			public int iBorderWidth;

			// Token: 0x04000B1E RID: 2846
			public int iScrollWidth;

			// Token: 0x04000B1F RID: 2847
			public int iScrollHeight;

			// Token: 0x04000B20 RID: 2848
			public int iCaptionWidth;

			// Token: 0x04000B21 RID: 2849
			public int iCaptionHeight;

			// Token: 0x04000B22 RID: 2850
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfCaptionFont;

			// Token: 0x04000B23 RID: 2851
			public int iSmCaptionWidth;

			// Token: 0x04000B24 RID: 2852
			public int iSmCaptionHeight;

			// Token: 0x04000B25 RID: 2853
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfSmCaptionFont;

			// Token: 0x04000B26 RID: 2854
			public int iMenuWidth;

			// Token: 0x04000B27 RID: 2855
			public int iMenuHeight;

			// Token: 0x04000B28 RID: 2856
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfMenuFont;

			// Token: 0x04000B29 RID: 2857
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfStatusFont;

			// Token: 0x04000B2A RID: 2858
			[MarshalAs(UnmanagedType.Struct)]
			public SafeNativeMethods.LOGFONT lfMessageFont;
		}
	}
}
