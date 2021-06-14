using System;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004F8 RID: 1272
	internal sealed class WindowsFont : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060053B0 RID: 21424 RVA: 0x0015D944 File Offset: 0x0015BB44
		private void CreateFont()
		{
			this.hFont = IntUnsafeNativeMethods.CreateFontIndirect(this.logFont);
			if (this.hFont == IntPtr.Zero)
			{
				this.logFont.lfFaceName = "Microsoft Sans Serif";
				this.logFont.lfOutPrecision = 7;
				this.hFont = IntUnsafeNativeMethods.CreateFontIndirect(this.logFont);
			}
			IntUnsafeNativeMethods.GetObject(new HandleRef(this, this.hFont), this.logFont);
			this.ownHandle = true;
		}

		// Token: 0x060053B1 RID: 21425 RVA: 0x0015D9C0 File Offset: 0x0015BBC0
		public WindowsFont(string faceName) : this(faceName, 8.25f, FontStyle.Regular, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060053B2 RID: 21426 RVA: 0x0015D9D1 File Offset: 0x0015BBD1
		public WindowsFont(string faceName, float size) : this(faceName, size, FontStyle.Regular, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060053B3 RID: 21427 RVA: 0x0015D9DE File Offset: 0x0015BBDE
		public WindowsFont(string faceName, float size, FontStyle style) : this(faceName, size, style, 1, WindowsFontQuality.Default)
		{
		}

		// Token: 0x060053B4 RID: 21428 RVA: 0x0015D9EC File Offset: 0x0015BBEC
		public WindowsFont(string faceName, float size, FontStyle style, byte charSet, WindowsFontQuality fontQuality)
		{
			this.fontSize = -1f;
			base..ctor();
			this.logFont = new IntNativeMethods.LOGFONT();
			int num = (int)Math.Ceiling((double)((float)WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.DpiY * size / 72f));
			this.logFont.lfHeight = -num;
			this.logFont.lfFaceName = ((faceName != null) ? faceName : "Microsoft Sans Serif");
			this.logFont.lfCharSet = charSet;
			this.logFont.lfOutPrecision = 4;
			this.logFont.lfQuality = (byte)fontQuality;
			this.logFont.lfWeight = (((style & FontStyle.Bold) == FontStyle.Bold) ? 700 : 400);
			this.logFont.lfItalic = (((style & FontStyle.Italic) == FontStyle.Italic) ? 1 : 0);
			this.logFont.lfUnderline = (((style & FontStyle.Underline) == FontStyle.Underline) ? 1 : 0);
			this.logFont.lfStrikeOut = (((style & FontStyle.Strikeout) == FontStyle.Strikeout) ? 1 : 0);
			this.style = style;
			this.CreateFont();
		}

		// Token: 0x060053B5 RID: 21429 RVA: 0x0015DAE8 File Offset: 0x0015BCE8
		private WindowsFont(IntNativeMethods.LOGFONT lf, bool createHandle)
		{
			this.fontSize = -1f;
			base..ctor();
			this.logFont = lf;
			if (this.logFont.lfFaceName == null)
			{
				this.logFont.lfFaceName = "Microsoft Sans Serif";
			}
			this.style = FontStyle.Regular;
			if (lf.lfWeight == 700)
			{
				this.style |= FontStyle.Bold;
			}
			if (lf.lfItalic == 1)
			{
				this.style |= FontStyle.Italic;
			}
			if (lf.lfUnderline == 1)
			{
				this.style |= FontStyle.Underline;
			}
			if (lf.lfStrikeOut == 1)
			{
				this.style |= FontStyle.Strikeout;
			}
			if (createHandle)
			{
				this.CreateFont();
			}
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x0015DB9A File Offset: 0x0015BD9A
		public static WindowsFont FromFont(Font font)
		{
			return WindowsFont.FromFont(font, WindowsFontQuality.Default);
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x0015DBA4 File Offset: 0x0015BDA4
		public static WindowsFont FromFont(Font font, WindowsFontQuality fontQuality)
		{
			string text = font.FontFamily.Name;
			if (text != null && text.Length > 1 && text[0] == '@')
			{
				text = text.Substring(1);
			}
			return new WindowsFont(text, font.SizeInPoints, font.Style, font.GdiCharSet, fontQuality);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x0015DBF8 File Offset: 0x0015BDF8
		public static WindowsFont FromHdc(IntPtr hdc)
		{
			IntPtr currentObject = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(null, hdc), 6);
			return WindowsFont.FromHfont(currentObject);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x0015DC19 File Offset: 0x0015BE19
		public static WindowsFont FromHfont(IntPtr hFont)
		{
			return WindowsFont.FromHfont(hFont, false);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x0015DC24 File Offset: 0x0015BE24
		public static WindowsFont FromHfont(IntPtr hFont, bool takeOwnership)
		{
			IntNativeMethods.LOGFONT logfont = new IntNativeMethods.LOGFONT();
			IntUnsafeNativeMethods.GetObject(new HandleRef(null, hFont), logfont);
			return new WindowsFont(logfont, false)
			{
				hFont = hFont,
				ownHandle = takeOwnership
			};
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x0015DC5C File Offset: 0x0015BE5C
		~WindowsFont()
		{
			this.Dispose(false);
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x0015DC8C File Offset: 0x0015BE8C
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x0015DC98 File Offset: 0x0015BE98
		internal void Dispose(bool disposing)
		{
			bool flag = false;
			if (this.ownHandle && (!this.ownedByCacheManager || !disposing) && (this.everOwnedByCacheManager || !disposing || !DeviceContexts.IsFontInUse(this)))
			{
				IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, this.hFont));
				this.hFont = IntPtr.Zero;
				this.ownHandle = false;
				flag = true;
			}
			if (disposing && (flag || !this.ownHandle))
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x0015DD08 File Offset: 0x0015BF08
		public override bool Equals(object font)
		{
			WindowsFont windowsFont = font as WindowsFont;
			return windowsFont != null && (windowsFont == this || (this.Name == windowsFont.Name && this.LogFontHeight == windowsFont.LogFontHeight && this.Style == windowsFont.Style && this.CharSet == windowsFont.CharSet && this.Quality == windowsFont.Quality));
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x0015DD74 File Offset: 0x0015BF74
		public override int GetHashCode()
		{
			return (int)(((int)this.Style << 13 | this.Style >> 19) ^ (FontStyle)((int)this.CharSet << 26 | (int)((uint)this.CharSet >> 6)) ^ (FontStyle)((uint)this.Size << 7 | (uint)this.Size >> 25));
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x0015DDB1 File Offset: 0x0015BFB1
		public object Clone()
		{
			return new WindowsFont(this.logFont, true);
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x0015DDC0 File Offset: 0x0015BFC0
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "[{0}: Name={1}, Size={2} points, Height={3} pixels, Sytle={4}]", new object[]
			{
				base.GetType().Name,
				this.logFont.lfFaceName,
				this.Size,
				this.Height,
				this.Style
			});
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x060053C2 RID: 21442 RVA: 0x0015DE28 File Offset: 0x0015C028
		public IntPtr Hfont
		{
			get
			{
				return this.hFont;
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x060053C3 RID: 21443 RVA: 0x0015DE30 File Offset: 0x0015C030
		public bool Italic
		{
			get
			{
				return this.logFont.lfItalic == 1;
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x060053C4 RID: 21444 RVA: 0x0015DE40 File Offset: 0x0015C040
		// (set) Token: 0x060053C5 RID: 21445 RVA: 0x0015DE48 File Offset: 0x0015C048
		public bool OwnedByCacheManager
		{
			get
			{
				return this.ownedByCacheManager;
			}
			set
			{
				if (value)
				{
					this.everOwnedByCacheManager = true;
				}
				this.ownedByCacheManager = value;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x060053C6 RID: 21446 RVA: 0x0015DE5B File Offset: 0x0015C05B
		public WindowsFontQuality Quality
		{
			get
			{
				return (WindowsFontQuality)this.logFont.lfQuality;
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x060053C7 RID: 21447 RVA: 0x0015DE68 File Offset: 0x0015C068
		public FontStyle Style
		{
			get
			{
				return this.style;
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x060053C8 RID: 21448 RVA: 0x0015DE70 File Offset: 0x0015C070
		public int Height
		{
			get
			{
				if (this.lineSpacing == 0)
				{
					WindowsGraphics measurementGraphics = WindowsGraphicsCacheManager.MeasurementGraphics;
					measurementGraphics.DeviceContext.SelectFont(this);
					IntNativeMethods.TEXTMETRIC textMetrics = measurementGraphics.GetTextMetrics();
					this.lineSpacing = textMetrics.tmHeight;
				}
				return this.lineSpacing;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x060053C9 RID: 21449 RVA: 0x0015DEB1 File Offset: 0x0015C0B1
		public byte CharSet
		{
			get
			{
				return this.logFont.lfCharSet;
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x0015DEBE File Offset: 0x0015C0BE
		public int LogFontHeight
		{
			get
			{
				return this.logFont.lfHeight;
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x060053CB RID: 21451 RVA: 0x0015DECB File Offset: 0x0015C0CB
		public string Name
		{
			get
			{
				return this.logFont.lfFaceName;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x060053CC RID: 21452 RVA: 0x0015DED8 File Offset: 0x0015C0D8
		public float Size
		{
			get
			{
				if (this.fontSize < 0f)
				{
					WindowsGraphics measurementGraphics = WindowsGraphicsCacheManager.MeasurementGraphics;
					measurementGraphics.DeviceContext.SelectFont(this);
					IntNativeMethods.TEXTMETRIC textMetrics = measurementGraphics.GetTextMetrics();
					int num = (this.logFont.lfHeight > 0) ? textMetrics.tmHeight : (textMetrics.tmHeight - textMetrics.tmInternalLeading);
					this.fontSize = (float)num * 72f / (float)measurementGraphics.DeviceContext.DpiY;
				}
				return this.fontSize;
			}
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x0015DF54 File Offset: 0x0015C154
		public static WindowsFontQuality WindowsFontQualityFromTextRenderingHint(Graphics g)
		{
			if (g == null)
			{
				return WindowsFontQuality.Default;
			}
			switch (g.TextRenderingHint)
			{
			case TextRenderingHint.SingleBitPerPixelGridFit:
				return WindowsFontQuality.Proof;
			case TextRenderingHint.SingleBitPerPixel:
				return WindowsFontQuality.Draft;
			case TextRenderingHint.AntiAliasGridFit:
				return WindowsFontQuality.AntiAliased;
			case TextRenderingHint.AntiAlias:
				return WindowsFontQuality.AntiAliased;
			case TextRenderingHint.ClearTypeGridFit:
				if (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
				{
					return WindowsFontQuality.ClearTypeNatural;
				}
				return WindowsFontQuality.ClearType;
			}
			return WindowsFontQuality.Default;
		}

		// Token: 0x040035EE RID: 13806
		private const int LogFontNameOffset = 28;

		// Token: 0x040035EF RID: 13807
		private IntPtr hFont;

		// Token: 0x040035F0 RID: 13808
		private float fontSize;

		// Token: 0x040035F1 RID: 13809
		private int lineSpacing;

		// Token: 0x040035F2 RID: 13810
		private bool ownHandle;

		// Token: 0x040035F3 RID: 13811
		private bool ownedByCacheManager;

		// Token: 0x040035F4 RID: 13812
		private bool everOwnedByCacheManager;

		// Token: 0x040035F5 RID: 13813
		private IntNativeMethods.LOGFONT logFont;

		// Token: 0x040035F6 RID: 13814
		private FontStyle style;

		// Token: 0x040035F7 RID: 13815
		private const string defaultFaceName = "Microsoft Sans Serif";

		// Token: 0x040035F8 RID: 13816
		private const float defaultFontSize = 8.25f;

		// Token: 0x040035F9 RID: 13817
		private const int defaultFontHeight = 13;
	}
}
