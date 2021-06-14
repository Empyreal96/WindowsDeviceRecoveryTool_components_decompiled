using System;
using System.Drawing;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	/// <summary>Provides methods used to measure and render text. This class cannot be inherited. </summary>
	// Token: 0x02000441 RID: 1089
	public sealed class TextRenderer
	{
		// Token: 0x06004C8D RID: 19597 RVA: 0x000027DB File Offset: 0x000009DB
		private TextRenderer()
		{
		}

		/// <summary>Draws the specified text at the specified location using the specified device context, font, and color.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the drawn text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C8E RID: 19598 RVA: 0x0013A11C File Offset: 0x0013831C
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, pt, foreColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		/// <summary>Draws the specified text at the specified location, using the specified device context, font, color, and back color.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the drawn text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> to apply to the background area of the drawn text.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C8F RID: 19599 RVA: 0x0013A1AC File Offset: 0x001383AC
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, Color backColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, pt, foreColor, backColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		/// <summary>Draws the specified text at the specified location using the specified device context, font, color, and formatting instructions. </summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the drawn text. </param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C90 RID: 19600 RVA: 0x0013A23C File Offset: 0x0013843C
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, pt, foreColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		/// <summary>Draws the specified text at the specified location using the specified device context, font, color, back color, and formatting instructions </summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="pt">The <see cref="T:System.Drawing.Point" /> that represents the upper-left corner of the drawn text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the text.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> to apply to the background area of the drawn text.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C91 RID: 19601 RVA: 0x0013A2C0 File Offset: 0x001384C0
		public static void DrawText(IDeviceContext dc, string text, Font font, Point pt, Color foreColor, Color backColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, pt, foreColor, backColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		/// <summary>Draws the specified text within the specified bounds, using the specified device context, font, and color.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C92 RID: 19602 RVA: 0x0013A344 File Offset: 0x00138544
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, bounds, foreColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		/// <summary>Draws the specified text within the specified bounds using the specified device context, font, color, and back color.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> to apply to the area represented by <paramref name="bounds" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C93 RID: 19603 RVA: 0x0013A3D4 File Offset: 0x001385D4
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, Color backColor)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						windowsGraphics.DrawText(text, windowsFont, bounds, foreColor, backColor);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
		}

		/// <summary>Draws the specified text within the specified bounds using the specified device context, font, color, and formatting instructions.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the drawn text.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C94 RID: 19604 RVA: 0x0013A464 File Offset: 0x00138664
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, bounds, foreColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		/// <summary>Draws the specified text within the specified bounds using the specified device context, font, color, back color, and formatting instructions.</summary>
		/// <param name="dc">The device context in which to draw the text.</param>
		/// <param name="text">The text to draw.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the drawn text.</param>
		/// <param name="bounds">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the text.</param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> to apply to the text.</param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> to apply to the area represented by <paramref name="bounds" />.</param>
		/// <param name="flags">A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C95 RID: 19605 RVA: 0x0013A4E8 File Offset: 0x001386E8
		public static void DrawText(IDeviceContext dc, string text, Font font, Rectangle bounds, Color foreColor, Color backColor, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
				{
					windowsGraphicsWrapper.WindowsGraphics.DrawText(text, windowsFont, bounds, foreColor, backColor, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0013A56C File Offset: 0x0013876C
		private static IntTextFormatFlags GetIntTextFormatFlags(TextFormatFlags flags)
		{
			if (((ulong)flags & 18446744073692774400UL) == 0UL)
			{
				return (IntTextFormatFlags)flags;
			}
			return (IntTextFormatFlags)(flags & (TextFormatFlags)16777215);
		}

		/// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font.</summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn on a single line with the specified <paramref name="font" />. You can manipulate how the text is drawn by using one of the <see cref="M:System.Windows.Forms.TextRenderer.DrawText(System.Drawing.IDeviceContext,System.String,System.Drawing.Font,System.Drawing.Rectangle,System.Drawing.Color,System.Windows.Forms.TextFormatFlags)" /> overloads that takes a <see cref="T:System.Windows.Forms.TextFormatFlags" /> parameter. For example, the default behavior of the <see cref="T:System.Windows.Forms.TextRenderer" /> is to add padding to the bounding rectangle of the drawn text to accommodate overhanging glyphs. If you need to draw a line of text without these extra spaces you should use the versions of <see cref="M:System.Windows.Forms.TextRenderer.DrawText(System.Drawing.IDeviceContext,System.String,System.Drawing.Font,System.Drawing.Point,System.Drawing.Color)" /> and <see cref="M:System.Windows.Forms.TextRenderer.MeasureText(System.Drawing.IDeviceContext,System.String,System.Drawing.Font)" /> that take a <see cref="T:System.Drawing.Size" /> and <see cref="T:System.Windows.Forms.TextFormatFlags" /> parameter. For an example, see <see cref="M:System.Windows.Forms.TextRenderer.MeasureText(System.Drawing.IDeviceContext,System.String,System.Drawing.Font,System.Drawing.Size,System.Windows.Forms.TextFormatFlags)" />.</returns>
		// Token: 0x06004C97 RID: 19607 RVA: 0x0013A590 File Offset: 0x00138790
		public static Size MeasureText(string text, Font font)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size result;
			using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				result = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, windowsFont);
			}
			return result;
		}

		/// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font, using the specified size to create an initial bounding rectangle.</summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <param name="proposedSize">The <see cref="T:System.Drawing.Size" /> of the initial bounding rectangle.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn with the specified <paramref name="font" />.</returns>
		// Token: 0x06004C98 RID: 19608 RVA: 0x0013A5DC File Offset: 0x001387DC
		public static Size MeasureText(string text, Font font, Size proposedSize)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size result;
			using (WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				result = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, WindowsGraphicsCacheManager.GetWindowsFont(font), proposedSize);
			}
			return result;
		}

		/// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font and formatting instructions, using the specified size to create the initial bounding rectangle for the text.</summary>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <param name="proposedSize">The <see cref="T:System.Drawing.Size" /> of the initial bounding rectangle.</param>
		/// <param name="flags">The formatting instructions to apply to the measured text.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn with the specified <paramref name="font" /> and format.</returns>
		// Token: 0x06004C99 RID: 19609 RVA: 0x0013A630 File Offset: 0x00138830
		public static Size MeasureText(string text, Font font, Size proposedSize, TextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			Size result;
			using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font))
			{
				result = WindowsGraphicsCacheManager.MeasurementGraphics.MeasureText(text, windowsFont, proposedSize, TextRenderer.GetIntTextFormatFlags(flags));
			}
			return result;
		}

		/// <summary>Provides the size, in pixels, of the specified text drawn with the specified font in the specified device context.</summary>
		/// <param name="dc">The device context in which to measure the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn in a single line with the specified <paramref name="font" /> in the specified device context.</returns>
		// Token: 0x06004C9A RID: 19610 RVA: 0x0013A684 File Offset: 0x00138884
		public static Size MeasureText(IDeviceContext dc, string text, Font font)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			Size result;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						result = windowsGraphics.MeasureText(text, windowsFont);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
			return result;
		}

		/// <summary>Provides the size, in pixels, of the specified text when drawn with the specified font in the specified device context, using the specified size to create an initial bounding rectangle for the text.</summary>
		/// <param name="dc">The device context in which to measure the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <param name="proposedSize">The <see cref="T:System.Drawing.Size" /> of the initial bounding rectangle.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn with the specified <paramref name="font" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C9B RID: 19611 RVA: 0x0013A720 File Offset: 0x00138920
		public static Size MeasureText(IDeviceContext dc, string text, Font font, Size proposedSize)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			IntPtr hdc = dc.GetHdc();
			Size result;
			try
			{
				using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
				{
					using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
					{
						result = windowsGraphics.MeasureText(text, windowsFont, proposedSize);
					}
				}
			}
			finally
			{
				dc.ReleaseHdc();
			}
			return result;
		}

		/// <summary>Provides the size, in pixels, of the specified text when drawn with the specified device context, font, and formatting instructions, using the specified size to create the initial bounding rectangle for the text.</summary>
		/// <param name="dc">The device context in which to measure the text.</param>
		/// <param name="text">The text to measure.</param>
		/// <param name="font">The <see cref="T:System.Drawing.Font" /> to apply to the measured text.</param>
		/// <param name="proposedSize">The <see cref="T:System.Drawing.Size" /> of the initial bounding rectangle.</param>
		/// <param name="flags">The formatting instructions to apply to the measured text.</param>
		/// <returns>The <see cref="T:System.Drawing.Size" />, in pixels, of <paramref name="text" /> drawn with the specified <paramref name="font" /> and format.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="dc" /> is <see langword="null" />.</exception>
		// Token: 0x06004C9C RID: 19612 RVA: 0x0013A7C0 File Offset: 0x001389C0
		public static Size MeasureText(IDeviceContext dc, string text, Font font, Size proposedSize, TextFormatFlags flags)
		{
			if (dc == null)
			{
				throw new ArgumentNullException("dc");
			}
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			WindowsFontQuality fontQuality = WindowsFont.WindowsFontQualityFromTextRenderingHint(dc as Graphics);
			Size result;
			using (WindowsGraphicsWrapper windowsGraphicsWrapper = new WindowsGraphicsWrapper(dc, flags))
			{
				using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(font, fontQuality))
				{
					result = windowsGraphicsWrapper.WindowsGraphics.MeasureText(text, windowsFont, proposedSize, TextRenderer.GetIntTextFormatFlags(flags));
				}
			}
			return result;
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0013A850 File Offset: 0x00138A50
		internal static Color DisabledTextColor(Color backColor)
		{
			if (SystemInformation.HighContrast && AccessibilityImprovements.Level1)
			{
				return SystemColors.GrayText;
			}
			Color result = SystemColors.ControlDark;
			if (ControlPaint.IsDarker(backColor, SystemColors.Control))
			{
				result = ControlPaint.Dark(backColor);
			}
			return result;
		}
	}
}
