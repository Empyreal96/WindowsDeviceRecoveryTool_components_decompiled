using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004FB RID: 1275
	internal sealed class WindowsGraphics : MarshalByRefObject, IDisposable, IDeviceContext
	{
		// Token: 0x060053D5 RID: 21461 RVA: 0x0015E170 File Offset: 0x0015C370
		public WindowsGraphics(DeviceContext dc)
		{
			this.dc = dc;
			this.dc.SaveHdc();
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x0015E18C File Offset: 0x0015C38C
		public static WindowsGraphics CreateMeasurementWindowsGraphics()
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(IntPtr.Zero);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x0015E1B4 File Offset: 0x0015C3B4
		public static WindowsGraphics CreateMeasurementWindowsGraphics(IntPtr screenDC)
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(screenDC);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x0015E1D8 File Offset: 0x0015C3D8
		public static WindowsGraphics FromHwnd(IntPtr hWnd)
		{
			DeviceContext deviceContext = DeviceContext.FromHwnd(hWnd);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x0015E1FC File Offset: 0x0015C3FC
		public static WindowsGraphics FromHdc(IntPtr hDc)
		{
			DeviceContext deviceContext = DeviceContext.FromHdc(hDc);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x0015E220 File Offset: 0x0015C420
		public static WindowsGraphics FromGraphics(Graphics g)
		{
			ApplyGraphicsProperties properties = ApplyGraphicsProperties.All;
			return WindowsGraphics.FromGraphics(g, properties);
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x0015E238 File Offset: 0x0015C438
		public static WindowsGraphics FromGraphics(Graphics g, ApplyGraphicsProperties properties)
		{
			WindowsRegion windowsRegion = null;
			float[] array = null;
			Region region = null;
			Matrix matrix = null;
			if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None || (properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None)
			{
				object[] array2 = g.GetContextInfo() as object[];
				if (array2 != null && array2.Length == 2)
				{
					region = (array2[0] as Region);
					matrix = (array2[1] as Matrix);
				}
				if (matrix != null)
				{
					if ((properties & ApplyGraphicsProperties.TranslateTransform) != ApplyGraphicsProperties.None)
					{
						array = matrix.Elements;
					}
					matrix.Dispose();
				}
				if (region != null)
				{
					if ((properties & ApplyGraphicsProperties.Clipping) != ApplyGraphicsProperties.None && !region.IsInfinite(g))
					{
						windowsRegion = WindowsRegion.FromRegion(region, g);
					}
					region.Dispose();
				}
			}
			WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(g.GetHdc());
			windowsGraphics.graphics = g;
			if (windowsRegion != null)
			{
				using (windowsRegion)
				{
					windowsGraphics.DeviceContext.IntersectClip(windowsRegion);
				}
			}
			if (array != null)
			{
				windowsGraphics.DeviceContext.TranslateTransform((int)array[4], (int)array[5]);
			}
			return windowsGraphics;
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x0015E318 File Offset: 0x0015C518
		~WindowsGraphics()
		{
			this.Dispose(false);
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x060053DD RID: 21469 RVA: 0x0015E348 File Offset: 0x0015C548
		public DeviceContext DeviceContext
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x0015E350 File Offset: 0x0015C550
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x0015E360 File Offset: 0x0015C560
		internal void Dispose(bool disposing)
		{
			if (this.dc != null)
			{
				try
				{
					this.dc.RestoreHdc();
					if (this.disposeDc)
					{
						this.dc.Dispose(disposing);
					}
					if (this.graphics != null)
					{
						this.graphics.ReleaseHdcInternal(this.dc.Hdc);
						this.graphics = null;
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.dc = null;
				}
			}
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x0015E3EC File Offset: 0x0015C5EC
		public IntPtr GetHdc()
		{
			return this.dc.Hdc;
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x0015E3F9 File Offset: 0x0015C5F9
		public void ReleaseHdc()
		{
			this.dc.Dispose();
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x060053E2 RID: 21474 RVA: 0x0015E406 File Offset: 0x0015C606
		// (set) Token: 0x060053E3 RID: 21475 RVA: 0x0015E40E File Offset: 0x0015C60E
		public TextPaddingOptions TextPadding
		{
			get
			{
				return this.paddingFlags;
			}
			set
			{
				if (this.paddingFlags != value)
				{
					this.paddingFlags = value;
				}
			}
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x0015E420 File Offset: 0x0015C620
		public void DrawPie(WindowsPen pen, Rectangle bounds, float startAngle, float sweepAngle)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(pen, pen.HPen));
			}
			int num = Math.Min(bounds.Width, bounds.Height);
			Point point = new Point(bounds.X + num / 2, bounds.Y + num / 2);
			int radius = num / 2;
			IntUnsafeNativeMethods.BeginPath(handleRef);
			IntUnsafeNativeMethods.MoveToEx(handleRef, point.X, point.Y, null);
			IntUnsafeNativeMethods.AngleArc(handleRef, point.X, point.Y, radius, startAngle, sweepAngle);
			IntUnsafeNativeMethods.LineTo(handleRef, point.X, point.Y);
			IntUnsafeNativeMethods.EndPath(handleRef);
			IntUnsafeNativeMethods.StrokePath(handleRef);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x0015E4EC File Offset: 0x0015C6EC
		private void DrawEllipse(WindowsPen pen, WindowsBrush brush, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
		{
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(pen, pen.HPen));
			}
			if (brush != null)
			{
				IntUnsafeNativeMethods.SelectObject(handleRef, new HandleRef(brush, brush.HBrush));
			}
			IntUnsafeNativeMethods.Ellipse(handleRef, nLeftRect, nTopRect, nRightRect, nBottomRect);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x0015E54B File Offset: 0x0015C74B
		public void DrawAndFillEllipse(WindowsPen pen, WindowsBrush brush, Rectangle bounds)
		{
			this.DrawEllipse(pen, brush, bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x0015E571 File Offset: 0x0015C771
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor)
		{
			this.DrawText(text, font, pt, foreColor, Color.Empty, IntTextFormatFlags.Default);
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x0015E584 File Offset: 0x0015C784
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, Color backColor)
		{
			this.DrawText(text, font, pt, foreColor, backColor, IntTextFormatFlags.Default);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x0015E594 File Offset: 0x0015C794
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, IntTextFormatFlags flags)
		{
			this.DrawText(text, font, pt, foreColor, Color.Empty, flags);
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x0015E5A8 File Offset: 0x0015C7A8
		public void DrawText(string text, WindowsFont font, Point pt, Color foreColor, Color backColor, IntTextFormatFlags flags)
		{
			Rectangle bounds = new Rectangle(pt.X, pt.Y, int.MaxValue, int.MaxValue);
			this.DrawText(text, font, bounds, foreColor, backColor, flags);
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x0015E5E3 File Offset: 0x0015C7E3
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor)
		{
			this.DrawText(text, font, bounds, foreColor, Color.Empty);
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x0015E5F5 File Offset: 0x0015C7F5
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor, Color backColor)
		{
			this.DrawText(text, font, bounds, foreColor, backColor, IntTextFormatFlags.HorizontalCenter | IntTextFormatFlags.VerticalCenter);
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x0015E605 File Offset: 0x0015C805
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color color, IntTextFormatFlags flags)
		{
			this.DrawText(text, font, bounds, color, Color.Empty, flags);
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x0015E61C File Offset: 0x0015C81C
		public void DrawText(string text, WindowsFont font, Rectangle bounds, Color foreColor, Color backColor, IntTextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text) || foreColor == Color.Transparent)
			{
				return;
			}
			HandleRef handleRef = new HandleRef(this.dc, this.dc.Hdc);
			if (this.dc.TextAlignment != DeviceContextTextAlignment.Top)
			{
				this.dc.SetTextAlignment(DeviceContextTextAlignment.Top);
			}
			if (!foreColor.IsEmpty && foreColor != this.dc.TextColor)
			{
				this.dc.SetTextColor(foreColor);
			}
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			DeviceContextBackgroundMode deviceContextBackgroundMode = (backColor.IsEmpty || backColor == Color.Transparent) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque;
			if (this.dc.BackgroundMode != deviceContextBackgroundMode)
			{
				this.dc.SetBackgroundMode(deviceContextBackgroundMode);
			}
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent && backColor != this.dc.BackgroundColor)
			{
				this.dc.SetBackgroundColor(backColor);
			}
			IntNativeMethods.DRAWTEXTPARAMS textMargins = this.GetTextMargins(font);
			bounds = WindowsGraphics.AdjustForVerticalAlignment(handleRef, text, bounds, flags, textMargins);
			if (bounds.Width == WindowsGraphics.MaxSize.Width)
			{
				bounds.Width -= bounds.X;
			}
			if (bounds.Height == WindowsGraphics.MaxSize.Height)
			{
				bounds.Height -= bounds.Y;
			}
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(bounds);
			IntUnsafeNativeMethods.DrawTextEx(handleRef, text, ref rect, (int)flags, textMargins);
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x0015E790 File Offset: 0x0015C990
		public Color GetNearestColor(Color color)
		{
			HandleRef hDC = new HandleRef(null, this.dc.Hdc);
			int nearestColor = IntUnsafeNativeMethods.GetNearestColor(hDC, ColorTranslator.ToWin32(color));
			return ColorTranslator.FromWin32(nearestColor);
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x0015E7C4 File Offset: 0x0015C9C4
		public float GetOverhangPadding(WindowsFont font)
		{
			WindowsFont windowsFont = font;
			if (windowsFont == null)
			{
				windowsFont = this.dc.Font;
			}
			float result = (float)windowsFont.Height / 6f;
			if (windowsFont != font)
			{
				windowsFont.Dispose();
			}
			return result;
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0015E7FC File Offset: 0x0015C9FC
		public IntNativeMethods.DRAWTEXTPARAMS GetTextMargins(WindowsFont font)
		{
			int leftMargin = 0;
			int rightMargin = 0;
			switch (this.TextPadding)
			{
			case TextPaddingOptions.GlyphOverhangPadding:
			{
				float overhangPadding = this.GetOverhangPadding(font);
				leftMargin = (int)Math.Ceiling((double)overhangPadding);
				rightMargin = (int)Math.Ceiling((double)(overhangPadding * 1.5f));
				break;
			}
			case TextPaddingOptions.LeftAndRightPadding:
			{
				float overhangPadding = this.GetOverhangPadding(font);
				leftMargin = (int)Math.Ceiling((double)(2f * overhangPadding));
				rightMargin = (int)Math.Ceiling((double)(overhangPadding * 2.5f));
				break;
			}
			}
			return new IntNativeMethods.DRAWTEXTPARAMS(leftMargin, rightMargin);
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x0015E880 File Offset: 0x0015CA80
		public Size GetTextExtent(string text, WindowsFont font)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
			HandleRef hDC = new HandleRef(null, this.dc.Hdc);
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			IntUnsafeNativeMethods.GetTextExtentPoint32(hDC, text, size);
			if (font != null && !MeasurementDCInfo.IsMeasurementDC(this.dc))
			{
				this.dc.ResetFont();
			}
			return new Size(size.cx, size.cy);
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x0015E8F9 File Offset: 0x0015CAF9
		public Size MeasureText(string text, WindowsFont font)
		{
			return this.MeasureText(text, font, WindowsGraphics.MaxSize, IntTextFormatFlags.Default);
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x0015E909 File Offset: 0x0015CB09
		public Size MeasureText(string text, WindowsFont font, Size proposedSize)
		{
			return this.MeasureText(text, font, proposedSize, IntTextFormatFlags.Default);
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x0015E918 File Offset: 0x0015CB18
		public Size MeasureText(string text, WindowsFont font, Size proposedSize, IntTextFormatFlags flags)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Size.Empty;
			}
			IntNativeMethods.DRAWTEXTPARAMS drawtextparams = null;
			if (MeasurementDCInfo.IsMeasurementDC(this.DeviceContext))
			{
				drawtextparams = MeasurementDCInfo.GetTextMargins(this, font);
			}
			if (drawtextparams == null)
			{
				drawtextparams = this.GetTextMargins(font);
			}
			int num = 1 + drawtextparams.iLeftMargin + drawtextparams.iRightMargin;
			if (proposedSize.Width <= num)
			{
				proposedSize.Width = num;
			}
			if (proposedSize.Height <= 0)
			{
				proposedSize.Height = 1;
			}
			IntNativeMethods.RECT rect = IntNativeMethods.RECT.FromXYWH(0, 0, proposedSize.Width, proposedSize.Height);
			HandleRef hDC = new HandleRef(null, this.dc.Hdc);
			if (font != null)
			{
				this.dc.SelectFont(font);
			}
			if (proposedSize.Height >= WindowsGraphics.MaxSize.Height && (flags & IntTextFormatFlags.SingleLine) != IntTextFormatFlags.Default)
			{
				flags &= ~(IntTextFormatFlags.Bottom | IntTextFormatFlags.VerticalCenter);
			}
			if (proposedSize.Width == WindowsGraphics.MaxSize.Width)
			{
				flags &= ~IntTextFormatFlags.WordBreak;
			}
			flags |= IntTextFormatFlags.CalculateRectangle;
			IntUnsafeNativeMethods.DrawTextEx(hDC, text, ref rect, (int)flags, drawtextparams);
			return rect.Size;
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x0015EA24 File Offset: 0x0015CC24
		public static Rectangle AdjustForVerticalAlignment(HandleRef hdc, string text, Rectangle bounds, IntTextFormatFlags flags, IntNativeMethods.DRAWTEXTPARAMS dtparams)
		{
			if (((flags & IntTextFormatFlags.Bottom) == IntTextFormatFlags.Default && (flags & IntTextFormatFlags.VerticalCenter) == IntTextFormatFlags.Default) || (flags & IntTextFormatFlags.SingleLine) != IntTextFormatFlags.Default || (flags & IntTextFormatFlags.CalculateRectangle) != IntTextFormatFlags.Default)
			{
				return bounds;
			}
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(bounds);
			flags |= IntTextFormatFlags.CalculateRectangle;
			int num = IntUnsafeNativeMethods.DrawTextEx(hdc, text, ref rect, (int)flags, dtparams);
			if (num > bounds.Height)
			{
				return bounds;
			}
			Rectangle result = bounds;
			if ((flags & IntTextFormatFlags.VerticalCenter) != IntTextFormatFlags.Default)
			{
				result.Y = result.Top + result.Height / 2 - num / 2;
			}
			else
			{
				result.Y = result.Bottom - num;
			}
			return result;
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x0015EAB4 File Offset: 0x0015CCB4
		public void DrawRectangle(WindowsPen pen, Rectangle rect)
		{
			this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x0015EADC File Offset: 0x0015CCDC
		public void DrawRectangle(WindowsPen pen, int x, int y, int width, int height)
		{
			HandleRef hdc = new HandleRef(this.dc, this.dc.Hdc);
			if (pen != null)
			{
				this.dc.SelectObject(pen.HPen, GdiObjectType.Pen);
			}
			DeviceContextBinaryRasterOperationFlags deviceContextBinaryRasterOperationFlags = this.dc.BinaryRasterOperation;
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				deviceContextBinaryRasterOperationFlags = this.dc.SetRasterOperation(DeviceContextBinaryRasterOperationFlags.CopyPen);
			}
			IntUnsafeNativeMethods.SelectObject(hdc, new HandleRef(null, IntUnsafeNativeMethods.GetStockObject(5)));
			IntUnsafeNativeMethods.Rectangle(hdc, x, y, x + width, y + height);
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				this.dc.SetRasterOperation(deviceContextBinaryRasterOperationFlags);
			}
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x0015EB6C File Offset: 0x0015CD6C
		public void FillRectangle(WindowsBrush brush, Rectangle rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x0015EB94 File Offset: 0x0015CD94
		public void FillRectangle(WindowsBrush brush, int x, int y, int width, int height)
		{
			HandleRef hDC = new HandleRef(this.dc, this.dc.Hdc);
			IntPtr hbrush = brush.HBrush;
			IntNativeMethods.RECT rect = new IntNativeMethods.RECT(x, y, x + width, y + height);
			IntUnsafeNativeMethods.FillRect(hDC, ref rect, new HandleRef(brush, hbrush));
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x0015EBE1 File Offset: 0x0015CDE1
		public void DrawLine(WindowsPen pen, Point p1, Point p2)
		{
			this.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x0015EC08 File Offset: 0x0015CE08
		public void DrawLine(WindowsPen pen, int x1, int y1, int x2, int y2)
		{
			HandleRef hdc = new HandleRef(this.dc, this.dc.Hdc);
			DeviceContextBinaryRasterOperationFlags deviceContextBinaryRasterOperationFlags = this.dc.BinaryRasterOperation;
			DeviceContextBackgroundMode deviceContextBackgroundMode = this.dc.BackgroundMode;
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				deviceContextBinaryRasterOperationFlags = this.dc.SetRasterOperation(DeviceContextBinaryRasterOperationFlags.CopyPen);
			}
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
			{
				deviceContextBackgroundMode = this.dc.SetBackgroundMode(DeviceContextBackgroundMode.Transparent);
			}
			if (pen != null)
			{
				this.dc.SelectObject(pen.HPen, GdiObjectType.Pen);
			}
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.MoveToEx(hdc, x1, y1, point);
			IntUnsafeNativeMethods.LineTo(hdc, x2, y2);
			if (deviceContextBackgroundMode != DeviceContextBackgroundMode.Transparent)
			{
				this.dc.SetBackgroundMode(deviceContextBackgroundMode);
			}
			if (deviceContextBinaryRasterOperationFlags != DeviceContextBinaryRasterOperationFlags.CopyPen)
			{
				this.dc.SetRasterOperation(deviceContextBinaryRasterOperationFlags);
			}
			IntUnsafeNativeMethods.MoveToEx(hdc, point.x, point.y, null);
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x0015ECD4 File Offset: 0x0015CED4
		public IntNativeMethods.TEXTMETRIC GetTextMetrics()
		{
			IntNativeMethods.TEXTMETRIC result = default(IntNativeMethods.TEXTMETRIC);
			HandleRef hDC = new HandleRef(this.dc, this.dc.Hdc);
			DeviceContextMapMode deviceContextMapMode = this.dc.MapMode;
			bool flag = deviceContextMapMode != DeviceContextMapMode.Text;
			if (flag)
			{
				this.dc.SaveHdc();
			}
			try
			{
				if (flag)
				{
					deviceContextMapMode = this.dc.SetMapMode(DeviceContextMapMode.Text);
				}
				IntUnsafeNativeMethods.GetTextMetrics(hDC, ref result);
			}
			finally
			{
				if (flag)
				{
					this.dc.RestoreHdc();
				}
			}
			return result;
		}

		// Token: 0x04003606 RID: 13830
		private DeviceContext dc;

		// Token: 0x04003607 RID: 13831
		private bool disposeDc;

		// Token: 0x04003608 RID: 13832
		private Graphics graphics;

		// Token: 0x04003609 RID: 13833
		public const int GdiUnsupportedFlagMask = -16777216;

		// Token: 0x0400360A RID: 13834
		public static readonly Size MaxSize = new Size(int.MaxValue, int.MaxValue);

		// Token: 0x0400360B RID: 13835
		private const float ItalicPaddingFactor = 0.5f;

		// Token: 0x0400360C RID: 13836
		private TextPaddingOptions paddingFlags;
	}
}
