using System;
using System.Drawing.Drawing2D;

namespace System.Drawing.Internal
{
	// Token: 0x020000E1 RID: 225
	internal sealed class WindowsGraphics : MarshalByRefObject, IDisposable, IDeviceContext
	{
		// Token: 0x06000BAE RID: 2990 RVA: 0x0002AA10 File Offset: 0x00028C10
		public WindowsGraphics(DeviceContext dc)
		{
			this.dc = dc;
			this.dc.SaveHdc();
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002AA2C File Offset: 0x00028C2C
		public static WindowsGraphics CreateMeasurementWindowsGraphics()
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(IntPtr.Zero);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0002AA54 File Offset: 0x00028C54
		public static WindowsGraphics CreateMeasurementWindowsGraphics(IntPtr screenDC)
		{
			DeviceContext deviceContext = DeviceContext.FromCompatibleDC(screenDC);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002AA78 File Offset: 0x00028C78
		public static WindowsGraphics FromHwnd(IntPtr hWnd)
		{
			DeviceContext deviceContext = DeviceContext.FromHwnd(hWnd);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002AA9C File Offset: 0x00028C9C
		public static WindowsGraphics FromHdc(IntPtr hDc)
		{
			DeviceContext deviceContext = DeviceContext.FromHdc(hDc);
			return new WindowsGraphics(deviceContext)
			{
				disposeDc = true
			};
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002AAC0 File Offset: 0x00028CC0
		public static WindowsGraphics FromGraphics(Graphics g)
		{
			ApplyGraphicsProperties properties = ApplyGraphicsProperties.All;
			return WindowsGraphics.FromGraphics(g, properties);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002AAD8 File Offset: 0x00028CD8
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

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002ABB8 File Offset: 0x00028DB8
		~WindowsGraphics()
		{
			this.Dispose(false);
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0002ABE8 File Offset: 0x00028DE8
		public DeviceContext DeviceContext
		{
			get
			{
				return this.dc;
			}
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002ABF0 File Offset: 0x00028DF0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002AC00 File Offset: 0x00028E00
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

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002AC8C File Offset: 0x00028E8C
		public IntPtr GetHdc()
		{
			return this.dc.Hdc;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002AC99 File Offset: 0x00028E99
		public void ReleaseHdc()
		{
			this.dc.Dispose();
		}

		// Token: 0x04000A64 RID: 2660
		private DeviceContext dc;

		// Token: 0x04000A65 RID: 2661
		private bool disposeDc;

		// Token: 0x04000A66 RID: 2662
		private Graphics graphics;
	}
}
