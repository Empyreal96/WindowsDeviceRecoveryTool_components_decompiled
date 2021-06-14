using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F1 RID: 1265
	internal class IntNativeMethods
	{
		// Token: 0x04003578 RID: 13688
		public const int MaxTextLengthInWin9x = 8192;

		// Token: 0x04003579 RID: 13689
		public const int DT_TOP = 0;

		// Token: 0x0400357A RID: 13690
		public const int DT_LEFT = 0;

		// Token: 0x0400357B RID: 13691
		public const int DT_CENTER = 1;

		// Token: 0x0400357C RID: 13692
		public const int DT_RIGHT = 2;

		// Token: 0x0400357D RID: 13693
		public const int DT_VCENTER = 4;

		// Token: 0x0400357E RID: 13694
		public const int DT_BOTTOM = 8;

		// Token: 0x0400357F RID: 13695
		public const int DT_WORDBREAK = 16;

		// Token: 0x04003580 RID: 13696
		public const int DT_SINGLELINE = 32;

		// Token: 0x04003581 RID: 13697
		public const int DT_EXPANDTABS = 64;

		// Token: 0x04003582 RID: 13698
		public const int DT_TABSTOP = 128;

		// Token: 0x04003583 RID: 13699
		public const int DT_NOCLIP = 256;

		// Token: 0x04003584 RID: 13700
		public const int DT_EXTERNALLEADING = 512;

		// Token: 0x04003585 RID: 13701
		public const int DT_CALCRECT = 1024;

		// Token: 0x04003586 RID: 13702
		public const int DT_NOPREFIX = 2048;

		// Token: 0x04003587 RID: 13703
		public const int DT_INTERNAL = 4096;

		// Token: 0x04003588 RID: 13704
		public const int DT_EDITCONTROL = 8192;

		// Token: 0x04003589 RID: 13705
		public const int DT_PATH_ELLIPSIS = 16384;

		// Token: 0x0400358A RID: 13706
		public const int DT_END_ELLIPSIS = 32768;

		// Token: 0x0400358B RID: 13707
		public const int DT_MODIFYSTRING = 65536;

		// Token: 0x0400358C RID: 13708
		public const int DT_RTLREADING = 131072;

		// Token: 0x0400358D RID: 13709
		public const int DT_WORD_ELLIPSIS = 262144;

		// Token: 0x0400358E RID: 13710
		public const int DT_NOFULLWIDTHCHARBREAK = 524288;

		// Token: 0x0400358F RID: 13711
		public const int DT_HIDEPREFIX = 1048576;

		// Token: 0x04003590 RID: 13712
		public const int DT_PREFIXONLY = 2097152;

		// Token: 0x04003591 RID: 13713
		public const int DIB_RGB_COLORS = 0;

		// Token: 0x04003592 RID: 13714
		public const int BI_BITFIELDS = 3;

		// Token: 0x04003593 RID: 13715
		public const int BI_RGB = 0;

		// Token: 0x04003594 RID: 13716
		public const int BITMAPINFO_MAX_COLORSIZE = 256;

		// Token: 0x04003595 RID: 13717
		public const int SPI_GETICONTITLELOGFONT = 31;

		// Token: 0x04003596 RID: 13718
		public const int SPI_GETNONCLIENTMETRICS = 41;

		// Token: 0x04003597 RID: 13719
		public const int DEFAULT_GUI_FONT = 17;

		// Token: 0x04003598 RID: 13720
		public const int HOLLOW_BRUSH = 5;

		// Token: 0x04003599 RID: 13721
		public const int BITSPIXEL = 12;

		// Token: 0x0400359A RID: 13722
		public const int ALTERNATE = 1;

		// Token: 0x0400359B RID: 13723
		public const int WINDING = 2;

		// Token: 0x0400359C RID: 13724
		public const int SRCCOPY = 13369376;

		// Token: 0x0400359D RID: 13725
		public const int SRCPAINT = 15597702;

		// Token: 0x0400359E RID: 13726
		public const int SRCAND = 8913094;

		// Token: 0x0400359F RID: 13727
		public const int SRCINVERT = 6684742;

		// Token: 0x040035A0 RID: 13728
		public const int SRCERASE = 4457256;

		// Token: 0x040035A1 RID: 13729
		public const int NOTSRCCOPY = 3342344;

		// Token: 0x040035A2 RID: 13730
		public const int NOTSRCERASE = 1114278;

		// Token: 0x040035A3 RID: 13731
		public const int MERGECOPY = 12583114;

		// Token: 0x040035A4 RID: 13732
		public const int MERGEPAINT = 12255782;

		// Token: 0x040035A5 RID: 13733
		public const int PATCOPY = 15728673;

		// Token: 0x040035A6 RID: 13734
		public const int PATPAINT = 16452105;

		// Token: 0x040035A7 RID: 13735
		public const int PATINVERT = 5898313;

		// Token: 0x040035A8 RID: 13736
		public const int DSTINVERT = 5570569;

		// Token: 0x040035A9 RID: 13737
		public const int BLACKNESS = 66;

		// Token: 0x040035AA RID: 13738
		public const int WHITENESS = 16711778;

		// Token: 0x040035AB RID: 13739
		public const int CAPTUREBLT = 1073741824;

		// Token: 0x040035AC RID: 13740
		public const int FW_DONTCARE = 0;

		// Token: 0x040035AD RID: 13741
		public const int FW_NORMAL = 400;

		// Token: 0x040035AE RID: 13742
		public const int FW_BOLD = 700;

		// Token: 0x040035AF RID: 13743
		public const int ANSI_CHARSET = 0;

		// Token: 0x040035B0 RID: 13744
		public const int DEFAULT_CHARSET = 1;

		// Token: 0x040035B1 RID: 13745
		public const int OUT_DEFAULT_PRECIS = 0;

		// Token: 0x040035B2 RID: 13746
		public const int OUT_TT_PRECIS = 4;

		// Token: 0x040035B3 RID: 13747
		public const int OUT_TT_ONLY_PRECIS = 7;

		// Token: 0x040035B4 RID: 13748
		public const int CLIP_DEFAULT_PRECIS = 0;

		// Token: 0x040035B5 RID: 13749
		public const int DEFAULT_QUALITY = 0;

		// Token: 0x040035B6 RID: 13750
		public const int DRAFT_QUALITY = 1;

		// Token: 0x040035B7 RID: 13751
		public const int PROOF_QUALITY = 2;

		// Token: 0x040035B8 RID: 13752
		public const int NONANTIALIASED_QUALITY = 3;

		// Token: 0x040035B9 RID: 13753
		public const int ANTIALIASED_QUALITY = 4;

		// Token: 0x040035BA RID: 13754
		public const int CLEARTYPE_QUALITY = 5;

		// Token: 0x040035BB RID: 13755
		public const int CLEARTYPE_NATURAL_QUALITY = 6;

		// Token: 0x040035BC RID: 13756
		public const int OBJ_PEN = 1;

		// Token: 0x040035BD RID: 13757
		public const int OBJ_BRUSH = 2;

		// Token: 0x040035BE RID: 13758
		public const int OBJ_DC = 3;

		// Token: 0x040035BF RID: 13759
		public const int OBJ_METADC = 4;

		// Token: 0x040035C0 RID: 13760
		public const int OBJ_FONT = 6;

		// Token: 0x040035C1 RID: 13761
		public const int OBJ_BITMAP = 7;

		// Token: 0x040035C2 RID: 13762
		public const int OBJ_MEMDC = 10;

		// Token: 0x040035C3 RID: 13763
		public const int OBJ_EXTPEN = 11;

		// Token: 0x040035C4 RID: 13764
		public const int OBJ_ENHMETADC = 12;

		// Token: 0x040035C5 RID: 13765
		public const int BS_SOLID = 0;

		// Token: 0x040035C6 RID: 13766
		public const int BS_HATCHED = 2;

		// Token: 0x040035C7 RID: 13767
		public const int CP_ACP = 0;

		// Token: 0x040035C8 RID: 13768
		public const int FORMAT_MESSAGE_ALLOCATE_BUFFER = 256;

		// Token: 0x040035C9 RID: 13769
		public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;

		// Token: 0x040035CA RID: 13770
		public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;

		// Token: 0x040035CB RID: 13771
		public const int FORMAT_MESSAGE_DEFAULT = 4608;

		// Token: 0x02000863 RID: 2147
		public enum RegionFlags
		{
			// Token: 0x04004366 RID: 17254
			ERROR,
			// Token: 0x04004367 RID: 17255
			NULLREGION,
			// Token: 0x04004368 RID: 17256
			SIMPLEREGION,
			// Token: 0x04004369 RID: 17257
			COMPLEXREGION
		}

		// Token: 0x02000864 RID: 2148
		public struct RECT
		{
			// Token: 0x06007033 RID: 28723 RVA: 0x0019B086 File Offset: 0x00199286
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x06007034 RID: 28724 RVA: 0x0019B0A5 File Offset: 0x001992A5
			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			// Token: 0x06007035 RID: 28725 RVA: 0x0019B0DB File Offset: 0x001992DB
			public static IntNativeMethods.RECT FromXYWH(int x, int y, int width, int height)
			{
				return new IntNativeMethods.RECT(x, y, x + width, y + height);
			}

			// Token: 0x17001865 RID: 6245
			// (get) Token: 0x06007036 RID: 28726 RVA: 0x0019B0EA File Offset: 0x001992EA
			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			// Token: 0x06007037 RID: 28727 RVA: 0x0019B10B File Offset: 0x0019930B
			public Rectangle ToRectangle()
			{
				return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
			}

			// Token: 0x0400436A RID: 17258
			public int left;

			// Token: 0x0400436B RID: 17259
			public int top;

			// Token: 0x0400436C RID: 17260
			public int right;

			// Token: 0x0400436D RID: 17261
			public int bottom;
		}

		// Token: 0x02000865 RID: 2149
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			// Token: 0x06007038 RID: 28728 RVA: 0x000027DB File Offset: 0x000009DB
			public POINT()
			{
			}

			// Token: 0x06007039 RID: 28729 RVA: 0x0019B138 File Offset: 0x00199338
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x0600703A RID: 28730 RVA: 0x0019B14E File Offset: 0x0019934E
			public Point ToPoint()
			{
				return new Point(this.x, this.y);
			}

			// Token: 0x0400436E RID: 17262
			public int x;

			// Token: 0x0400436F RID: 17263
			public int y;
		}

		// Token: 0x02000866 RID: 2150
		[StructLayout(LayoutKind.Sequential)]
		public class DRAWTEXTPARAMS
		{
			// Token: 0x0600703B RID: 28731 RVA: 0x0019B161 File Offset: 0x00199361
			public DRAWTEXTPARAMS()
			{
			}

			// Token: 0x0600703C RID: 28732 RVA: 0x0019B180 File Offset: 0x00199380
			public DRAWTEXTPARAMS(IntNativeMethods.DRAWTEXTPARAMS original)
			{
				this.iLeftMargin = original.iLeftMargin;
				this.iRightMargin = original.iRightMargin;
				this.iTabLength = original.iTabLength;
			}

			// Token: 0x0600703D RID: 28733 RVA: 0x0019B1CC File Offset: 0x001993CC
			public DRAWTEXTPARAMS(int leftMargin, int rightMargin)
			{
				this.iLeftMargin = leftMargin;
				this.iRightMargin = rightMargin;
			}

			// Token: 0x04004370 RID: 17264
			private int cbSize = Marshal.SizeOf(typeof(IntNativeMethods.DRAWTEXTPARAMS));

			// Token: 0x04004371 RID: 17265
			public int iTabLength;

			// Token: 0x04004372 RID: 17266
			public int iLeftMargin;

			// Token: 0x04004373 RID: 17267
			public int iRightMargin;

			// Token: 0x04004374 RID: 17268
			public int uiLengthDrawn;
		}

		// Token: 0x02000867 RID: 2151
		[StructLayout(LayoutKind.Sequential)]
		public class LOGBRUSH
		{
			// Token: 0x04004375 RID: 17269
			public int lbStyle;

			// Token: 0x04004376 RID: 17270
			public int lbColor;

			// Token: 0x04004377 RID: 17271
			public int lbHatch;
		}

		// Token: 0x02000868 RID: 2152
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			// Token: 0x0600703F RID: 28735 RVA: 0x000027DB File Offset: 0x000009DB
			public LOGFONT()
			{
			}

			// Token: 0x06007040 RID: 28736 RVA: 0x0019B1F8 File Offset: 0x001993F8
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

			// Token: 0x04004378 RID: 17272
			public int lfHeight;

			// Token: 0x04004379 RID: 17273
			public int lfWidth;

			// Token: 0x0400437A RID: 17274
			public int lfEscapement;

			// Token: 0x0400437B RID: 17275
			public int lfOrientation;

			// Token: 0x0400437C RID: 17276
			public int lfWeight;

			// Token: 0x0400437D RID: 17277
			public byte lfItalic;

			// Token: 0x0400437E RID: 17278
			public byte lfUnderline;

			// Token: 0x0400437F RID: 17279
			public byte lfStrikeOut;

			// Token: 0x04004380 RID: 17280
			public byte lfCharSet;

			// Token: 0x04004381 RID: 17281
			public byte lfOutPrecision;

			// Token: 0x04004382 RID: 17282
			public byte lfClipPrecision;

			// Token: 0x04004383 RID: 17283
			public byte lfQuality;

			// Token: 0x04004384 RID: 17284
			public byte lfPitchAndFamily;

			// Token: 0x04004385 RID: 17285
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}

		// Token: 0x02000869 RID: 2153
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TEXTMETRIC
		{
			// Token: 0x04004386 RID: 17286
			public int tmHeight;

			// Token: 0x04004387 RID: 17287
			public int tmAscent;

			// Token: 0x04004388 RID: 17288
			public int tmDescent;

			// Token: 0x04004389 RID: 17289
			public int tmInternalLeading;

			// Token: 0x0400438A RID: 17290
			public int tmExternalLeading;

			// Token: 0x0400438B RID: 17291
			public int tmAveCharWidth;

			// Token: 0x0400438C RID: 17292
			public int tmMaxCharWidth;

			// Token: 0x0400438D RID: 17293
			public int tmWeight;

			// Token: 0x0400438E RID: 17294
			public int tmOverhang;

			// Token: 0x0400438F RID: 17295
			public int tmDigitizedAspectX;

			// Token: 0x04004390 RID: 17296
			public int tmDigitizedAspectY;

			// Token: 0x04004391 RID: 17297
			public char tmFirstChar;

			// Token: 0x04004392 RID: 17298
			public char tmLastChar;

			// Token: 0x04004393 RID: 17299
			public char tmDefaultChar;

			// Token: 0x04004394 RID: 17300
			public char tmBreakChar;

			// Token: 0x04004395 RID: 17301
			public byte tmItalic;

			// Token: 0x04004396 RID: 17302
			public byte tmUnderlined;

			// Token: 0x04004397 RID: 17303
			public byte tmStruckOut;

			// Token: 0x04004398 RID: 17304
			public byte tmPitchAndFamily;

			// Token: 0x04004399 RID: 17305
			public byte tmCharSet;
		}

		// Token: 0x0200086A RID: 2154
		public struct TEXTMETRICA
		{
			// Token: 0x0400439A RID: 17306
			public int tmHeight;

			// Token: 0x0400439B RID: 17307
			public int tmAscent;

			// Token: 0x0400439C RID: 17308
			public int tmDescent;

			// Token: 0x0400439D RID: 17309
			public int tmInternalLeading;

			// Token: 0x0400439E RID: 17310
			public int tmExternalLeading;

			// Token: 0x0400439F RID: 17311
			public int tmAveCharWidth;

			// Token: 0x040043A0 RID: 17312
			public int tmMaxCharWidth;

			// Token: 0x040043A1 RID: 17313
			public int tmWeight;

			// Token: 0x040043A2 RID: 17314
			public int tmOverhang;

			// Token: 0x040043A3 RID: 17315
			public int tmDigitizedAspectX;

			// Token: 0x040043A4 RID: 17316
			public int tmDigitizedAspectY;

			// Token: 0x040043A5 RID: 17317
			public byte tmFirstChar;

			// Token: 0x040043A6 RID: 17318
			public byte tmLastChar;

			// Token: 0x040043A7 RID: 17319
			public byte tmDefaultChar;

			// Token: 0x040043A8 RID: 17320
			public byte tmBreakChar;

			// Token: 0x040043A9 RID: 17321
			public byte tmItalic;

			// Token: 0x040043AA RID: 17322
			public byte tmUnderlined;

			// Token: 0x040043AB RID: 17323
			public byte tmStruckOut;

			// Token: 0x040043AC RID: 17324
			public byte tmPitchAndFamily;

			// Token: 0x040043AD RID: 17325
			public byte tmCharSet;
		}

		// Token: 0x0200086B RID: 2155
		[StructLayout(LayoutKind.Sequential)]
		public class SIZE
		{
			// Token: 0x06007041 RID: 28737 RVA: 0x000027DB File Offset: 0x000009DB
			public SIZE()
			{
			}

			// Token: 0x06007042 RID: 28738 RVA: 0x0019B2B3 File Offset: 0x001994B3
			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			// Token: 0x06007043 RID: 28739 RVA: 0x0019B2C9 File Offset: 0x001994C9
			public Size ToSize()
			{
				return new Size(this.cx, this.cy);
			}

			// Token: 0x040043AE RID: 17326
			public int cx;

			// Token: 0x040043AF RID: 17327
			public int cy;
		}
	}
}
