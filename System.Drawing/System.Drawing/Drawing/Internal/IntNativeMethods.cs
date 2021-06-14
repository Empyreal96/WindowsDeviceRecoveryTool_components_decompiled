using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x020000E2 RID: 226
	internal class IntNativeMethods
	{
		// Token: 0x04000A67 RID: 2663
		public const int MaxTextLengthInWin9x = 8192;

		// Token: 0x04000A68 RID: 2664
		public const int DT_TOP = 0;

		// Token: 0x04000A69 RID: 2665
		public const int DT_LEFT = 0;

		// Token: 0x04000A6A RID: 2666
		public const int DT_CENTER = 1;

		// Token: 0x04000A6B RID: 2667
		public const int DT_RIGHT = 2;

		// Token: 0x04000A6C RID: 2668
		public const int DT_VCENTER = 4;

		// Token: 0x04000A6D RID: 2669
		public const int DT_BOTTOM = 8;

		// Token: 0x04000A6E RID: 2670
		public const int DT_WORDBREAK = 16;

		// Token: 0x04000A6F RID: 2671
		public const int DT_SINGLELINE = 32;

		// Token: 0x04000A70 RID: 2672
		public const int DT_EXPANDTABS = 64;

		// Token: 0x04000A71 RID: 2673
		public const int DT_TABSTOP = 128;

		// Token: 0x04000A72 RID: 2674
		public const int DT_NOCLIP = 256;

		// Token: 0x04000A73 RID: 2675
		public const int DT_EXTERNALLEADING = 512;

		// Token: 0x04000A74 RID: 2676
		public const int DT_CALCRECT = 1024;

		// Token: 0x04000A75 RID: 2677
		public const int DT_NOPREFIX = 2048;

		// Token: 0x04000A76 RID: 2678
		public const int DT_INTERNAL = 4096;

		// Token: 0x04000A77 RID: 2679
		public const int DT_EDITCONTROL = 8192;

		// Token: 0x04000A78 RID: 2680
		public const int DT_PATH_ELLIPSIS = 16384;

		// Token: 0x04000A79 RID: 2681
		public const int DT_END_ELLIPSIS = 32768;

		// Token: 0x04000A7A RID: 2682
		public const int DT_MODIFYSTRING = 65536;

		// Token: 0x04000A7B RID: 2683
		public const int DT_RTLREADING = 131072;

		// Token: 0x04000A7C RID: 2684
		public const int DT_WORD_ELLIPSIS = 262144;

		// Token: 0x04000A7D RID: 2685
		public const int DT_NOFULLWIDTHCHARBREAK = 524288;

		// Token: 0x04000A7E RID: 2686
		public const int DT_HIDEPREFIX = 1048576;

		// Token: 0x04000A7F RID: 2687
		public const int DT_PREFIXONLY = 2097152;

		// Token: 0x04000A80 RID: 2688
		public const int DIB_RGB_COLORS = 0;

		// Token: 0x04000A81 RID: 2689
		public const int BI_BITFIELDS = 3;

		// Token: 0x04000A82 RID: 2690
		public const int BI_RGB = 0;

		// Token: 0x04000A83 RID: 2691
		public const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04000A84 RID: 2692
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x04000A85 RID: 2693
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x04000A86 RID: 2694
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x04000A87 RID: 2695
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x04000A88 RID: 2696
		public const int BITSPIXEL = 12;

		// Token: 0x04000A89 RID: 2697
		public const int ALTERNATE = 1;

		// Token: 0x04000A8A RID: 2698
		public const int WINDING = 2;

		// Token: 0x04000A8B RID: 2699
		public const int SRCCOPY = 13369376;

		// Token: 0x04000A8C RID: 2700
		public const int SRCPAINT = 15597702;

		// Token: 0x04000A8D RID: 2701
		public const int SRCAND = 8913094;

		// Token: 0x04000A8E RID: 2702
		public const int SRCINVERT = 6684742;

		// Token: 0x04000A8F RID: 2703
		public const int SRCERASE = 4457256;

		// Token: 0x04000A90 RID: 2704
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x04000A91 RID: 2705
		public const int NOTSRCERASE = 1114278;

		// Token: 0x04000A92 RID: 2706
		public const int MERGECOPY = 12583114;

		// Token: 0x04000A93 RID: 2707
		public const int MERGEPAINT = 12255782;

		// Token: 0x04000A94 RID: 2708
		public const int PATCOPY = 15728673;

		// Token: 0x04000A95 RID: 2709
		public const int PATPAINT = 16452105;

		// Token: 0x04000A96 RID: 2710
		public const int PATINVERT = 5898313;

		// Token: 0x04000A97 RID: 2711
		public const int DSTINVERT = 5570569;

		// Token: 0x04000A98 RID: 2712
		public const int BLACKNESS = 66;

		// Token: 0x04000A99 RID: 2713
		public const int WHITENESS = 16711778;

		// Token: 0x04000A9A RID: 2714
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x04000A9B RID: 2715
		public const int FW_DONTCARE = 0;

		// Token: 0x04000A9C RID: 2716
		public const int FW_NORMAL = 400;

		// Token: 0x04000A9D RID: 2717
		public const int FW_BOLD = 700;

		// Token: 0x04000A9E RID: 2718
		public const int ANSI_CHARSET = 0;

		// Token: 0x04000A9F RID: 2719
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x04000AA0 RID: 2720
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x04000AA1 RID: 2721
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x04000AA2 RID: 2722
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x04000AA3 RID: 2723
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x04000AA4 RID: 2724
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x04000AA5 RID: 2725
		public const int DRAFT_QUALITY = 1;

		// Token: 0x04000AA6 RID: 2726
		public const int PROOF_QUALITY = 2;

		// Token: 0x04000AA7 RID: 2727
		public const int NONANTIALIASED_QUALITY = 3;

		// Token: 0x04000AA8 RID: 2728
		public const int ANTIALIASED_QUALITY = 4;

		// Token: 0x04000AA9 RID: 2729
		public const int CLEARTYPE_QUALITY = 5;

		// Token: 0x04000AAA RID: 2730
		public const int CLEARTYPE_NATURAL_QUALITY = 6;

		// Token: 0x04000AAB RID: 2731
		public const int OBJ_PEN = 1;

		// Token: 0x04000AAC RID: 2732
		public const int OBJ_BRUSH = 2;

		// Token: 0x04000AAD RID: 2733
		public const int OBJ_DC = 3;

		// Token: 0x04000AAE RID: 2734
		public const int OBJ_METADC = 4;

		// Token: 0x04000AAF RID: 2735
		public const int OBJ_FONT = 6;

		// Token: 0x04000AB0 RID: 2736
		public const int OBJ_BITMAP = 7;

		// Token: 0x04000AB1 RID: 2737
		public const int OBJ_MEMDC = 10;

		// Token: 0x04000AB2 RID: 2738
		public const int OBJ_EXTPEN = 11;

		// Token: 0x04000AB3 RID: 2739
		public const int OBJ_ENHMETADC = 12;

		// Token: 0x04000AB4 RID: 2740
		public const int BS_SOLID = 0;

		// Token: 0x04000AB5 RID: 2741
		public const int BS_HATCHED = 2;

		// Token: 0x04000AB6 RID: 2742
		public const int CP_ACP = 0;

		// Token: 0x04000AB7 RID: 2743
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x04000AB8 RID: 2744
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x04000AB9 RID: 2745
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x04000ABA RID: 2746
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x02000126 RID: 294
		public enum RegionFlags
		{
			// Token: 0x04000C76 RID: 3190
			ERROR,
			// Token: 0x04000C77 RID: 3191
			NULLREGION,
			// Token: 0x04000C78 RID: 3192
			SIMPLEREGION,
			// Token: 0x04000C79 RID: 3193
			COMPLEXREGION
		}

		// Token: 0x02000127 RID: 295
		public struct RECT
		{
			// Token: 0x06000F90 RID: 3984 RVA: 0x0002DEC3 File Offset: 0x0002C0C3
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x06000F91 RID: 3985 RVA: 0x0002DEE2 File Offset: 0x0002C0E2
			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x06000F92 RID: 3986 RVA: 0x0002DF18 File Offset: 0x0002C118
			public static IntNativeMethods.RECT FromXYWH(int x, int y, int width, int height)
			{
				return new IntNativeMethods.RECT(x, y, x + width, y + height);
			}

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x06000F93 RID: 3987 RVA: 0x0002DF27 File Offset: 0x0002C127
			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			// Token: 0x06000F94 RID: 3988 RVA: 0x0002DF48 File Offset: 0x0002C148
			public Rectangle ToRectangle()
			{
				return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
			}

			// Token: 0x04000C7A RID: 3194
			public int left;

			// Token: 0x04000C7B RID: 3195
			public int top;

			// Token: 0x04000C7C RID: 3196
			public int right;

			// Token: 0x04000C7D RID: 3197
			public int bottom;
		}

		// Token: 0x02000128 RID: 296
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x06000F95 RID: 3989 RVA: 0x00003800 File Offset: 0x00001A00
			public POINT()
			{
			}

			// Token: 0x06000F96 RID: 3990 RVA: 0x0002DF75 File Offset: 0x0002C175
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x06000F97 RID: 3991 RVA: 0x0002DF8B File Offset: 0x0002C18B
			public Point ToPoint()
			{
				return new Point(this.x, this.y);
			}

			// Token: 0x04000C7E RID: 3198
			public int x;

			// Token: 0x04000C7F RID: 3199
			public int y;
		}

		// Token: 0x02000129 RID: 297
		[StructLayout(LayoutKind.Sequential)]
		public class DRAWTEXTPARAMS
		{
			// Token: 0x06000F98 RID: 3992 RVA: 0x0002DF9E File Offset: 0x0002C19E
			public DRAWTEXTPARAMS()
			{
			}

			// Token: 0x06000F99 RID: 3993 RVA: 0x0002DFBC File Offset: 0x0002C1BC
			public DRAWTEXTPARAMS(IntNativeMethods.DRAWTEXTPARAMS original)
			{
				this.iLeftMargin = original.iLeftMargin;
				this.iRightMargin = original.iRightMargin;
				this.iTabLength = original.iTabLength;
			}

			// Token: 0x06000F9A RID: 3994 RVA: 0x0002E008 File Offset: 0x0002C208
			public DRAWTEXTPARAMS(int leftMargin, int rightMargin)
			{
				this.iLeftMargin = leftMargin;
				this.iRightMargin = rightMargin;
			}

			// Token: 0x04000C80 RID: 3200
			private int cbSize = Marshal.SizeOf(typeof(IntNativeMethods.DRAWTEXTPARAMS));

			// Token: 0x04000C81 RID: 3201
			public int iTabLength;

			// Token: 0x04000C82 RID: 3202
			public int iLeftMargin;

			// Token: 0x04000C83 RID: 3203
			public int iRightMargin;

			// Token: 0x04000C84 RID: 3204
			public int uiLengthDrawn;
		}

		// Token: 0x0200012A RID: 298
		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			// Token: 0x04000C85 RID: 3205
			public int lbStyle;

			// Token: 0x04000C86 RID: 3206
			public int lbColor;

			// Token: 0x04000C87 RID: 3207
			public int lbHatch;
		}

		// Token: 0x0200012B RID: 299
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x06000F9C RID: 3996 RVA: 0x00003800 File Offset: 0x00001A00
			public LOGFONT()
			{
			}

			// Token: 0x06000F9D RID: 3997 RVA: 0x0002E034 File Offset: 0x0002C234
			public LOGFONT(IntNativeMethods.LOGFONT lf)
			{
				this.lfHeight = lf.lfHeight;
				this.lfWidth = lf.lfWidth;
				this.lfEscapement = lf.lfEscapement;
				this.lfOrientation = lf.lfOrientation;
				this.lfWeight = lf.lfWeight;
				this.lfItalic = lf.lfItalic;
				this.lfUnderline = lf.lfUnderline;
				this.lfStrikeOut = lf.lfStrikeOut;
				this.lfCharSet = lf.lfCharSet;
				this.lfOutPrecision = lf.lfOutPrecision;
				this.lfClipPrecision = lf.lfClipPrecision;
				this.lfQuality = lf.lfQuality;
				this.lfPitchAndFamily = lf.lfPitchAndFamily;
				this.lfFaceName = lf.lfFaceName;
			}

			// Token: 0x04000C88 RID: 3208
			public int lfHeight;

			// Token: 0x04000C89 RID: 3209
			public int lfWidth;

			// Token: 0x04000C8A RID: 3210
			public int lfEscapement;

			// Token: 0x04000C8B RID: 3211
			public int lfOrientation;

			// Token: 0x04000C8C RID: 3212
			public int lfWeight;

			// Token: 0x04000C8D RID: 3213
			public byte lfItalic;

			// Token: 0x04000C8E RID: 3214
			public byte lfUnderline;

			// Token: 0x04000C8F RID: 3215
			public byte lfStrikeOut;

			// Token: 0x04000C90 RID: 3216
			public byte lfCharSet;

			// Token: 0x04000C91 RID: 3217
			public byte lfOutPrecision;

			// Token: 0x04000C92 RID: 3218
			public byte lfClipPrecision;

			// Token: 0x04000C93 RID: 3219
			public byte lfQuality;

			// Token: 0x04000C94 RID: 3220
			public byte lfPitchAndFamily;

			// Token: 0x04000C95 RID: 3221
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x0200012C RID: 300
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04000C96 RID: 3222
			public int tmHeight;

			// Token: 0x04000C97 RID: 3223
			public int tmAscent;

			// Token: 0x04000C98 RID: 3224
			public int tmDescent;

			// Token: 0x04000C99 RID: 3225
			public int tmInternalLeading;

			// Token: 0x04000C9A RID: 3226
			public int tmExternalLeading;

			// Token: 0x04000C9B RID: 3227
			public int tmAveCharWidth;

			// Token: 0x04000C9C RID: 3228
			public int tmMaxCharWidth;

			// Token: 0x04000C9D RID: 3229
			public int tmWeight;

			// Token: 0x04000C9E RID: 3230
			public int tmOverhang;

			// Token: 0x04000C9F RID: 3231
			public int tmDigitizedAspectX;

			// Token: 0x04000CA0 RID: 3232
			public int tmDigitizedAspectY;

			// Token: 0x04000CA1 RID: 3233
			public char tmFirstChar;

			// Token: 0x04000CA2 RID: 3234
			public char tmLastChar;

			// Token: 0x04000CA3 RID: 3235
			public char tmDefaultChar;

			// Token: 0x04000CA4 RID: 3236
			public char tmBreakChar;

			// Token: 0x04000CA5 RID: 3237
			public byte tmItalic;

			// Token: 0x04000CA6 RID: 3238
			public byte tmUnderlined;

			// Token: 0x04000CA7 RID: 3239
			public byte tmStruckOut;

			// Token: 0x04000CA8 RID: 3240
			public byte tmPitchAndFamily;

			// Token: 0x04000CA9 RID: 3241
			public byte tmCharSet;
		}

		// Token: 0x0200012D RID: 301
		public struct TEXTMETRICA
		{
			// Token: 0x04000CAA RID: 3242
			public int tmHeight;

			// Token: 0x04000CAB RID: 3243
			public int tmAscent;

			// Token: 0x04000CAC RID: 3244
			public int tmDescent;

			// Token: 0x04000CAD RID: 3245
			public int tmInternalLeading;

			// Token: 0x04000CAE RID: 3246
			public int tmExternalLeading;

			// Token: 0x04000CAF RID: 3247
			public int tmAveCharWidth;

			// Token: 0x04000CB0 RID: 3248
			public int tmMaxCharWidth;

			// Token: 0x04000CB1 RID: 3249
			public int tmWeight;

			// Token: 0x04000CB2 RID: 3250
			public int tmOverhang;

			// Token: 0x04000CB3 RID: 3251
			public int tmDigitizedAspectX;

			// Token: 0x04000CB4 RID: 3252
			public int tmDigitizedAspectY;

			// Token: 0x04000CB5 RID: 3253
			public byte tmFirstChar;

			// Token: 0x04000CB6 RID: 3254
			public byte tmLastChar;

			// Token: 0x04000CB7 RID: 3255
			public byte tmDefaultChar;

			// Token: 0x04000CB8 RID: 3256
			public byte tmBreakChar;

			// Token: 0x04000CB9 RID: 3257
			public byte tmItalic;

			// Token: 0x04000CBA RID: 3258
			public byte tmUnderlined;

			// Token: 0x04000CBB RID: 3259
			public byte tmStruckOut;

			// Token: 0x04000CBC RID: 3260
			public byte tmPitchAndFamily;

			// Token: 0x04000CBD RID: 3261
			public byte tmCharSet;
		}

		// Token: 0x0200012E RID: 302
		[StructLayout(LayoutKind.Sequential)]
		public class SIZE
		{
			// Token: 0x06000F9E RID: 3998 RVA: 0x00003800 File Offset: 0x00001A00
			public SIZE()
			{
			}

			// Token: 0x06000F9F RID: 3999 RVA: 0x0002E0EF File Offset: 0x0002C2EF
			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			// Token: 0x06000FA0 RID: 4000 RVA: 0x0002E105 File Offset: 0x0002C305
			public Size ToSize()
			{
				return new Size(this.cx, this.cy);
			}

			// Token: 0x04000CBE RID: 3262
			public int cx;

			// Token: 0x04000CBF RID: 3263
			public int cy;
		}
	}
}
