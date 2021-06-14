using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Drawing.Internal
{
	// Token: 0x020000DA RID: 218
	internal sealed class DeviceContext : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000B82 RID: 2946 RVA: 0x0002A0C0 File Offset: 0x000282C0
		// (remove) Token: 0x06000B83 RID: 2947 RVA: 0x0002A0F8 File Offset: 0x000282F8
		public event EventHandler Disposing;

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0002A12D File Offset: 0x0002832D
		public DeviceContextType DeviceContextType
		{
			get
			{
				return this.dcType;
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0002A135 File Offset: 0x00028335
		public IntPtr Hdc
		{
			get
			{
				if (this.hDC == IntPtr.Zero && this.dcType == DeviceContextType.Display)
				{
					this.hDC = ((IDeviceContext)this).GetHdc();
					this.CacheInitialState();
				}
				return this.hDC;
			}
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002A16C File Offset: 0x0002836C
		private void CacheInitialState()
		{
			this.hCurrentPen = (this.hInitialPen = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 1));
			this.hCurrentBrush = (this.hInitialBrush = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 2));
			this.hCurrentBmp = (this.hInitialBmp = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 7));
			this.hCurrentFont = (this.hInitialFont = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6));
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002A200 File Offset: 0x00028400
		public void DeleteObject(IntPtr handle, GdiObjectType type)
		{
			IntPtr handle2 = IntPtr.Zero;
			if (type != GdiObjectType.Pen)
			{
				if (type != GdiObjectType.Brush)
				{
					if (type == GdiObjectType.Bitmap)
					{
						if (handle == this.hCurrentBmp)
						{
							IntPtr intPtr = IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialBmp));
							this.hCurrentBmp = IntPtr.Zero;
						}
						handle2 = handle;
					}
				}
				else
				{
					if (handle == this.hCurrentBrush)
					{
						IntPtr intPtr2 = IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialBrush));
						this.hCurrentBrush = IntPtr.Zero;
					}
					handle2 = handle;
				}
			}
			else
			{
				if (handle == this.hCurrentPen)
				{
					IntPtr intPtr3 = IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(this, this.hInitialPen));
					this.hCurrentPen = IntPtr.Zero;
				}
				handle2 = handle;
			}
			IntUnsafeNativeMethods.DeleteObject(new HandleRef(this, handle2));
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002A2E0 File Offset: 0x000284E0
		private DeviceContext(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			this.dcType = DeviceContextType.Display;
			DeviceContexts.AddDeviceContext(this);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002A308 File Offset: 0x00028508
		private DeviceContext(IntPtr hDC, DeviceContextType dcType)
		{
			this.hDC = hDC;
			this.dcType = dcType;
			this.CacheInitialState();
			DeviceContexts.AddDeviceContext(this);
			if (dcType == DeviceContextType.Display)
			{
				this.hWnd = IntUnsafeNativeMethods.WindowFromDC(new HandleRef(this, this.hDC));
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002A35C File Offset: 0x0002855C
		public static DeviceContext CreateDC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateDC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.NamedDevice);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002A37C File Offset: 0x0002857C
		public static DeviceContext CreateIC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateIC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.Information);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002A39C File Offset: 0x0002859C
		public static DeviceContext FromCompatibleDC(IntPtr hdc)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, hdc));
			return new DeviceContext(intPtr, DeviceContextType.Memory);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002A3BD File Offset: 0x000285BD
		public static DeviceContext FromHdc(IntPtr hdc)
		{
			return new DeviceContext(hdc, DeviceContextType.Unknown);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002A3C6 File Offset: 0x000285C6
		public static DeviceContext FromHwnd(IntPtr hwnd)
		{
			return new DeviceContext(hwnd);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002A3D0 File Offset: 0x000285D0
		~DeviceContext()
		{
			this.Dispose(false);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002A400 File Offset: 0x00028600
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002A410 File Offset: 0x00028610
		internal void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			if (this.Disposing != null)
			{
				this.Disposing(this, EventArgs.Empty);
			}
			this.disposed = true;
			switch (this.dcType)
			{
			case DeviceContextType.Unknown:
			case DeviceContextType.NCWindow:
				return;
			case DeviceContextType.Display:
				((IDeviceContext)this).ReleaseHdc();
				return;
			case DeviceContextType.NamedDevice:
			case DeviceContextType.Information:
				IntUnsafeNativeMethods.DeleteHDC(new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
				return;
			case DeviceContextType.Memory:
				IntUnsafeNativeMethods.DeleteDC(new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002A4AF File Offset: 0x000286AF
		IntPtr IDeviceContext.GetHdc()
		{
			if (this.hDC == IntPtr.Zero)
			{
				this.hDC = IntUnsafeNativeMethods.GetDC(new HandleRef(this, this.hWnd));
			}
			return this.hDC;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002A4E0 File Offset: 0x000286E0
		void IDeviceContext.ReleaseHdc()
		{
			if (this.hDC != IntPtr.Zero && this.dcType == DeviceContextType.Display)
			{
				IntUnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.hWnd), new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0002A531 File Offset: 0x00028731
		public DeviceContextGraphicsMode GraphicsMode
		{
			get
			{
				return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.GetGraphicsMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002A544 File Offset: 0x00028744
		public DeviceContextGraphicsMode SetGraphicsMode(DeviceContextGraphicsMode newMode)
		{
			return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.SetGraphicsMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002A558 File Offset: 0x00028758
		public void RestoreHdc()
		{
			IntUnsafeNativeMethods.RestoreDC(new HandleRef(this, this.hDC), -1);
			if (this.contextStack != null)
			{
				DeviceContext.GraphicsState graphicsState = (DeviceContext.GraphicsState)this.contextStack.Pop();
				this.hCurrentBmp = graphicsState.hBitmap;
				this.hCurrentBrush = graphicsState.hBrush;
				this.hCurrentPen = graphicsState.hPen;
				this.hCurrentFont = graphicsState.hFont;
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002A5C4 File Offset: 0x000287C4
		public int SaveHdc()
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			int result = IntUnsafeNativeMethods.SaveDC(handleRef);
			if (this.contextStack == null)
			{
				this.contextStack = new Stack();
			}
			DeviceContext.GraphicsState graphicsState = new DeviceContext.GraphicsState();
			graphicsState.hBitmap = this.hCurrentBmp;
			graphicsState.hBrush = this.hCurrentBrush;
			graphicsState.hPen = this.hCurrentPen;
			graphicsState.hFont = this.hCurrentFont;
			this.contextStack.Push(graphicsState);
			return result;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002A63C File Offset: 0x0002883C
		public void SetClip(WindowsRegion region)
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			HandleRef hRgn = new HandleRef(region, region.HRegion);
			IntUnsafeNativeMethods.SelectClipRgn(handleRef, hRgn);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002A670 File Offset: 0x00028870
		public void IntersectClip(WindowsRegion wr)
		{
			if (wr.HRegion == IntPtr.Zero)
			{
				return;
			}
			WindowsRegion windowsRegion = new WindowsRegion(0, 0, 0, 0);
			try
			{
				int clipRgn = IntUnsafeNativeMethods.GetClipRgn(new HandleRef(this, this.Hdc), new HandleRef(windowsRegion, windowsRegion.HRegion));
				if (clipRgn == 1)
				{
					wr.CombineRegion(windowsRegion, wr, RegionCombineMode.AND);
				}
				this.SetClip(wr);
			}
			finally
			{
				windowsRegion.Dispose();
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002A6E8 File Offset: 0x000288E8
		public void TranslateTransform(int dx, int dy)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.OffsetViewportOrgEx(new HandleRef(this, this.Hdc), dx, dy, point);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002A710 File Offset: 0x00028910
		public override bool Equals(object obj)
		{
			DeviceContext deviceContext = obj as DeviceContext;
			return deviceContext == this || (deviceContext != null && deviceContext.Hdc == this.Hdc);
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002A740 File Offset: 0x00028940
		public override int GetHashCode()
		{
			return this.Hdc.GetHashCode();
		}

		// Token: 0x04000A30 RID: 2608
		private IntPtr hDC;

		// Token: 0x04000A31 RID: 2609
		private DeviceContextType dcType;

		// Token: 0x04000A33 RID: 2611
		private bool disposed;

		// Token: 0x04000A34 RID: 2612
		private IntPtr hWnd = (IntPtr)(-1);

		// Token: 0x04000A35 RID: 2613
		private IntPtr hInitialPen;

		// Token: 0x04000A36 RID: 2614
		private IntPtr hInitialBrush;

		// Token: 0x04000A37 RID: 2615
		private IntPtr hInitialBmp;

		// Token: 0x04000A38 RID: 2616
		private IntPtr hInitialFont;

		// Token: 0x04000A39 RID: 2617
		private IntPtr hCurrentPen;

		// Token: 0x04000A3A RID: 2618
		private IntPtr hCurrentBrush;

		// Token: 0x04000A3B RID: 2619
		private IntPtr hCurrentBmp;

		// Token: 0x04000A3C RID: 2620
		private IntPtr hCurrentFont;

		// Token: 0x04000A3D RID: 2621
		private Stack contextStack;

		// Token: 0x02000125 RID: 293
		internal class GraphicsState
		{
			// Token: 0x04000C71 RID: 3185
			internal IntPtr hBrush;

			// Token: 0x04000C72 RID: 3186
			internal IntPtr hFont;

			// Token: 0x04000C73 RID: 3187
			internal IntPtr hPen;

			// Token: 0x04000C74 RID: 3188
			internal IntPtr hBitmap;
		}
	}
}
