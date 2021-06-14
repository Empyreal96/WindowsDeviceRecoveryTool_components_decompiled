using System;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004FC RID: 1276
	internal sealed class WindowsPen : MarshalByRefObject, ICloneable, IDisposable
	{
		// Token: 0x060053FF RID: 21503 RVA: 0x0015ED76 File Offset: 0x0015CF76
		public WindowsPen(DeviceContext dc) : this(dc, WindowsPenStyle.Solid, 1, Color.Black)
		{
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x0015ED86 File Offset: 0x0015CF86
		public WindowsPen(DeviceContext dc, Color color) : this(dc, WindowsPenStyle.Solid, 1, color)
		{
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x0015ED92 File Offset: 0x0015CF92
		public WindowsPen(DeviceContext dc, WindowsBrush windowsBrush) : this(dc, WindowsPenStyle.Solid, 1, windowsBrush)
		{
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x0015ED9E File Offset: 0x0015CF9E
		public WindowsPen(DeviceContext dc, WindowsPenStyle style, int width, Color color)
		{
			this.style = style;
			this.width = width;
			this.color = color;
			this.dc = dc;
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x0015EDC3 File Offset: 0x0015CFC3
		public WindowsPen(DeviceContext dc, WindowsPenStyle style, int width, WindowsBrush windowsBrush)
		{
			this.style = style;
			this.wndBrush = (WindowsBrush)windowsBrush.Clone();
			this.width = width;
			this.color = windowsBrush.Color;
			this.dc = dc;
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x0015EE00 File Offset: 0x0015D000
		private void CreatePen()
		{
			if (this.width > 1)
			{
				this.style |= WindowsPenStyle.Geometric;
			}
			if (this.wndBrush == null)
			{
				this.nativeHandle = IntSafeNativeMethods.CreatePen((int)this.style, this.width, ColorTranslator.ToWin32(this.color));
				return;
			}
			IntNativeMethods.LOGBRUSH logbrush = new IntNativeMethods.LOGBRUSH();
			logbrush.lbColor = ColorTranslator.ToWin32(this.wndBrush.Color);
			logbrush.lbStyle = 0;
			logbrush.lbHatch = 0;
			this.nativeHandle = IntSafeNativeMethods.ExtCreatePen((int)this.style, this.width, logbrush, 0, null);
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x0015EE98 File Offset: 0x0015D098
		public object Clone()
		{
			if (this.wndBrush == null)
			{
				return new WindowsPen(this.dc, this.style, this.width, this.color);
			}
			return new WindowsPen(this.dc, this.style, this.width, (WindowsBrush)this.wndBrush.Clone());
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x0015EEF4 File Offset: 0x0015D0F4
		~WindowsPen()
		{
			this.Dispose(false);
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x0015EF24 File Offset: 0x0015D124
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x0015EF30 File Offset: 0x0015D130
		private void Dispose(bool disposing)
		{
			if (this.nativeHandle != IntPtr.Zero && this.dc != null)
			{
				this.dc.DeleteObject(this.nativeHandle, GdiObjectType.Pen);
				this.nativeHandle = IntPtr.Zero;
			}
			if (this.wndBrush != null)
			{
				this.wndBrush.Dispose();
				this.wndBrush = null;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06005409 RID: 21513 RVA: 0x0015EF97 File Offset: 0x0015D197
		public IntPtr HPen
		{
			get
			{
				if (this.nativeHandle == IntPtr.Zero)
				{
					this.CreatePen();
				}
				return this.nativeHandle;
			}
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x0015EFB8 File Offset: 0x0015D1B8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: Style={1}, Color={2}, Width={3}, Brush={4}", new object[]
			{
				base.GetType().Name,
				this.style,
				this.color,
				this.width,
				(this.wndBrush != null) ? this.wndBrush.ToString() : "null"
			});
		}

		// Token: 0x0400360D RID: 13837
		private IntPtr nativeHandle;

		// Token: 0x0400360E RID: 13838
		private const int dashStyleMask = 15;

		// Token: 0x0400360F RID: 13839
		private const int endCapMask = 3840;

		// Token: 0x04003610 RID: 13840
		private const int joinMask = 61440;

		// Token: 0x04003611 RID: 13841
		private DeviceContext dc;

		// Token: 0x04003612 RID: 13842
		private WindowsBrush wndBrush;

		// Token: 0x04003613 RID: 13843
		private WindowsPenStyle style;

		// Token: 0x04003614 RID: 13844
		private Color color;

		// Token: 0x04003615 RID: 13845
		private int width;

		// Token: 0x04003616 RID: 13846
		private const int cosmeticPenWidth = 1;
	}
}
