using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Internal
{
	// Token: 0x020004E7 RID: 1255
	internal sealed class DeviceContext : MarshalByRefObject, IDeviceContext, IDisposable
	{
		// Token: 0x14000414 RID: 1044
		// (add) Token: 0x060052ED RID: 21229 RVA: 0x0015C200 File Offset: 0x0015A400
		// (remove) Token: 0x060052EE RID: 21230 RVA: 0x0015C238 File Offset: 0x0015A438
		public event EventHandler Disposing;

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x060052EF RID: 21231 RVA: 0x0015C26D File Offset: 0x0015A46D
		public DeviceContextType DeviceContextType
		{
			get
			{
				return this.dcType;
			}
		}

		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x060052F0 RID: 21232 RVA: 0x0015C275 File Offset: 0x0015A475
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

		// Token: 0x060052F1 RID: 21233 RVA: 0x0015C2AC File Offset: 0x0015A4AC
		private void CacheInitialState()
		{
			this.hCurrentPen = (this.hInitialPen = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 1));
			this.hCurrentBrush = (this.hInitialBrush = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 2));
			this.hCurrentBmp = (this.hInitialBmp = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 7));
			this.hCurrentFont = (this.hInitialFont = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6));
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x0015C340 File Offset: 0x0015A540
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

		// Token: 0x060052F3 RID: 21235 RVA: 0x0015C420 File Offset: 0x0015A620
		private DeviceContext(IntPtr hWnd)
		{
			this.hWnd = hWnd;
			this.dcType = DeviceContextType.Display;
			DeviceContexts.AddDeviceContext(this);
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x0015C448 File Offset: 0x0015A648
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

		// Token: 0x060052F5 RID: 21237 RVA: 0x0015C49C File Offset: 0x0015A69C
		public static DeviceContext CreateDC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateDC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.NamedDevice);
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x0015C4BC File Offset: 0x0015A6BC
		public static DeviceContext CreateIC(string driverName, string deviceName, string fileName, HandleRef devMode)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateIC(driverName, deviceName, fileName, devMode);
			return new DeviceContext(intPtr, DeviceContextType.Information);
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0015C4DC File Offset: 0x0015A6DC
		public static DeviceContext FromCompatibleDC(IntPtr hdc)
		{
			IntPtr intPtr = IntUnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, hdc));
			return new DeviceContext(intPtr, DeviceContextType.Memory);
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0015C4FD File Offset: 0x0015A6FD
		public static DeviceContext FromHdc(IntPtr hdc)
		{
			return new DeviceContext(hdc, DeviceContextType.Unknown);
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x0015C506 File Offset: 0x0015A706
		public static DeviceContext FromHwnd(IntPtr hwnd)
		{
			return new DeviceContext(hwnd);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x0015C510 File Offset: 0x0015A710
		~DeviceContext()
		{
			this.Dispose(false);
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0015C540 File Offset: 0x0015A740
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0015C550 File Offset: 0x0015A750
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
			this.DisposeFont(disposing);
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

		// Token: 0x060052FD RID: 21245 RVA: 0x0015C5F6 File Offset: 0x0015A7F6
		IntPtr IDeviceContext.GetHdc()
		{
			if (this.hDC == IntPtr.Zero)
			{
				this.hDC = IntUnsafeNativeMethods.GetDC(new HandleRef(this, this.hWnd));
			}
			return this.hDC;
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0015C628 File Offset: 0x0015A828
		void IDeviceContext.ReleaseHdc()
		{
			if (this.hDC != IntPtr.Zero && this.dcType == DeviceContextType.Display)
			{
				IntUnsafeNativeMethods.ReleaseDC(new HandleRef(this, this.hWnd), new HandleRef(this, this.hDC));
				this.hDC = IntPtr.Zero;
			}
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x060052FF RID: 21247 RVA: 0x0015C679 File Offset: 0x0015A879
		public DeviceContextGraphicsMode GraphicsMode
		{
			get
			{
				return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.GetGraphicsMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x0015C68C File Offset: 0x0015A88C
		public DeviceContextGraphicsMode SetGraphicsMode(DeviceContextGraphicsMode newMode)
		{
			return (DeviceContextGraphicsMode)IntUnsafeNativeMethods.SetGraphicsMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x06005301 RID: 21249 RVA: 0x0015C6A0 File Offset: 0x0015A8A0
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
				if (graphicsState.font != null && graphicsState.font.IsAlive)
				{
					this.selectedFont = (graphicsState.font.Target as WindowsFont);
				}
				else
				{
					WindowsFont windowsFont = this.selectedFont;
					this.selectedFont = null;
					if (windowsFont != null && MeasurementDCInfo.IsMeasurementDC(this))
					{
						windowsFont.Dispose();
					}
				}
			}
			MeasurementDCInfo.ResetIfIsMeasurementDC(this.hDC);
		}

		// Token: 0x06005302 RID: 21250 RVA: 0x0015C764 File Offset: 0x0015A964
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
			graphicsState.font = new WeakReference(this.selectedFont);
			this.contextStack.Push(graphicsState);
			return result;
		}

		// Token: 0x06005303 RID: 21251 RVA: 0x0015C7F0 File Offset: 0x0015A9F0
		public void SetClip(WindowsRegion region)
		{
			HandleRef handleRef = new HandleRef(this, this.Hdc);
			HandleRef hRgn = new HandleRef(region, region.HRegion);
			IntUnsafeNativeMethods.SelectClipRgn(handleRef, hRgn);
		}

		// Token: 0x06005304 RID: 21252 RVA: 0x0015C824 File Offset: 0x0015AA24
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

		// Token: 0x06005305 RID: 21253 RVA: 0x0015C89C File Offset: 0x0015AA9C
		public void TranslateTransform(int dx, int dy)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.OffsetViewportOrgEx(new HandleRef(this, this.Hdc), dx, dy, point);
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x0015C8C4 File Offset: 0x0015AAC4
		public override bool Equals(object obj)
		{
			DeviceContext deviceContext = obj as DeviceContext;
			return deviceContext == this || (deviceContext != null && deviceContext.Hdc == this.Hdc);
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x0015C8F4 File Offset: 0x0015AAF4
		public override int GetHashCode()
		{
			return this.Hdc.GetHashCode();
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06005308 RID: 21256 RVA: 0x0015C90F File Offset: 0x0015AB0F
		public WindowsFont ActiveFont
		{
			get
			{
				return this.selectedFont;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06005309 RID: 21257 RVA: 0x0015C917 File Offset: 0x0015AB17
		public Color BackgroundColor
		{
			get
			{
				return ColorTranslator.FromWin32(IntUnsafeNativeMethods.GetBkColor(new HandleRef(this, this.Hdc)));
			}
		}

		// Token: 0x0600530A RID: 21258 RVA: 0x0015C92F File Offset: 0x0015AB2F
		public Color SetBackgroundColor(Color newColor)
		{
			return ColorTranslator.FromWin32(IntUnsafeNativeMethods.SetBkColor(new HandleRef(this, this.Hdc), ColorTranslator.ToWin32(newColor)));
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x0600530B RID: 21259 RVA: 0x0015C94D File Offset: 0x0015AB4D
		public DeviceContextBackgroundMode BackgroundMode
		{
			get
			{
				return (DeviceContextBackgroundMode)IntUnsafeNativeMethods.GetBkMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x0015C960 File Offset: 0x0015AB60
		public DeviceContextBackgroundMode SetBackgroundMode(DeviceContextBackgroundMode newMode)
		{
			return (DeviceContextBackgroundMode)IntUnsafeNativeMethods.SetBkMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x0600530D RID: 21261 RVA: 0x0015C974 File Offset: 0x0015AB74
		public DeviceContextBinaryRasterOperationFlags BinaryRasterOperation
		{
			get
			{
				return (DeviceContextBinaryRasterOperationFlags)IntUnsafeNativeMethods.GetROP2(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x0015C987 File Offset: 0x0015AB87
		public DeviceContextBinaryRasterOperationFlags SetRasterOperation(DeviceContextBinaryRasterOperationFlags rasterOperation)
		{
			return (DeviceContextBinaryRasterOperationFlags)IntUnsafeNativeMethods.SetROP2(new HandleRef(this, this.Hdc), (int)rasterOperation);
		}

		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x0600530F RID: 21263 RVA: 0x0015C99B File Offset: 0x0015AB9B
		public Size Dpi
		{
			get
			{
				return new Size(this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsX), this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsY));
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06005310 RID: 21264 RVA: 0x0015C9B2 File Offset: 0x0015ABB2
		public int DpiX
		{
			get
			{
				return this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsX);
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x06005311 RID: 21265 RVA: 0x0015C9BC File Offset: 0x0015ABBC
		public int DpiY
		{
			get
			{
				return this.GetDeviceCapabilities(DeviceCapabilities.LogicalPixelsY);
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x06005312 RID: 21266 RVA: 0x0015C9C8 File Offset: 0x0015ABC8
		public WindowsFont Font
		{
			get
			{
				if (MeasurementDCInfo.IsMeasurementDC(this))
				{
					WindowsFont lastUsedFont = MeasurementDCInfo.LastUsedFont;
					if (lastUsedFont != null && lastUsedFont.Hfont != IntPtr.Zero)
					{
						return lastUsedFont;
					}
				}
				return WindowsFont.FromHdc(this.Hdc);
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06005313 RID: 21267 RVA: 0x0015CA05 File Offset: 0x0015AC05
		public static DeviceContext ScreenDC
		{
			get
			{
				return DeviceContext.FromHwnd(IntPtr.Zero);
			}
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x0015CA14 File Offset: 0x0015AC14
		internal void DisposeFont(bool disposing)
		{
			if (disposing)
			{
				DeviceContexts.RemoveDeviceContext(this);
			}
			if (this.selectedFont != null && this.selectedFont.Hfont != IntPtr.Zero)
			{
				IntPtr currentObject = IntUnsafeNativeMethods.GetCurrentObject(new HandleRef(this, this.hDC), 6);
				if (currentObject == this.selectedFont.Hfont)
				{
					IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, this.hInitialFont));
					currentObject = this.hInitialFont;
				}
				this.selectedFont.Dispose(disposing);
				this.selectedFont = null;
			}
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x0015CAA8 File Offset: 0x0015ACA8
		public IntPtr SelectFont(WindowsFont font)
		{
			if (font.Equals(this.Font))
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr = this.SelectObject(font.Hfont, GdiObjectType.Font);
			WindowsFont windowsFont = this.selectedFont;
			this.selectedFont = font;
			this.hCurrentFont = font.Hfont;
			if (windowsFont != null && MeasurementDCInfo.IsMeasurementDC(this))
			{
				windowsFont.Dispose();
			}
			if (MeasurementDCInfo.IsMeasurementDC(this))
			{
				if (intPtr != IntPtr.Zero)
				{
					MeasurementDCInfo.LastUsedFont = font;
				}
				else
				{
					MeasurementDCInfo.Reset();
				}
			}
			return intPtr;
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x0015CB25 File Offset: 0x0015AD25
		public void ResetFont()
		{
			MeasurementDCInfo.ResetIfIsMeasurementDC(this.Hdc);
			IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, this.hInitialFont));
			this.selectedFont = null;
			this.hCurrentFont = this.hInitialFont;
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x0015CB63 File Offset: 0x0015AD63
		public int GetDeviceCapabilities(DeviceCapabilities capabilityIndex)
		{
			return IntUnsafeNativeMethods.GetDeviceCaps(new HandleRef(this, this.Hdc), (int)capabilityIndex);
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06005318 RID: 21272 RVA: 0x0015CB77 File Offset: 0x0015AD77
		public DeviceContextMapMode MapMode
		{
			get
			{
				return (DeviceContextMapMode)IntUnsafeNativeMethods.GetMapMode(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x0015CB8C File Offset: 0x0015AD8C
		public bool IsFontOnContextStack(WindowsFont wf)
		{
			if (this.contextStack == null)
			{
				return false;
			}
			foreach (object obj in this.contextStack)
			{
				DeviceContext.GraphicsState graphicsState = (DeviceContext.GraphicsState)obj;
				if (graphicsState.hFont == wf.Hfont)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x0015CC04 File Offset: 0x0015AE04
		public DeviceContextMapMode SetMapMode(DeviceContextMapMode newMode)
		{
			return (DeviceContextMapMode)IntUnsafeNativeMethods.SetMapMode(new HandleRef(this, this.Hdc), (int)newMode);
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x0015CC18 File Offset: 0x0015AE18
		public IntPtr SelectObject(IntPtr hObj, GdiObjectType type)
		{
			if (type != GdiObjectType.Pen)
			{
				if (type != GdiObjectType.Brush)
				{
					if (type == GdiObjectType.Bitmap)
					{
						this.hCurrentBmp = hObj;
					}
				}
				else
				{
					this.hCurrentBrush = hObj;
				}
			}
			else
			{
				this.hCurrentPen = hObj;
			}
			return IntUnsafeNativeMethods.SelectObject(new HandleRef(this, this.Hdc), new HandleRef(null, hObj));
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x0600531C RID: 21276 RVA: 0x0015CC64 File Offset: 0x0015AE64
		public DeviceContextTextAlignment TextAlignment
		{
			get
			{
				return (DeviceContextTextAlignment)IntUnsafeNativeMethods.GetTextAlign(new HandleRef(this, this.Hdc));
			}
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x0015CC77 File Offset: 0x0015AE77
		public DeviceContextTextAlignment SetTextAlignment(DeviceContextTextAlignment newAligment)
		{
			return (DeviceContextTextAlignment)IntUnsafeNativeMethods.SetTextAlign(new HandleRef(this, this.Hdc), (int)newAligment);
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x0600531E RID: 21278 RVA: 0x0015CC8B File Offset: 0x0015AE8B
		public Color TextColor
		{
			get
			{
				return ColorTranslator.FromWin32(IntUnsafeNativeMethods.GetTextColor(new HandleRef(this, this.Hdc)));
			}
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x0015CCA3 File Offset: 0x0015AEA3
		public Color SetTextColor(Color newColor)
		{
			return ColorTranslator.FromWin32(IntUnsafeNativeMethods.SetTextColor(new HandleRef(this, this.Hdc), ColorTranslator.ToWin32(newColor)));
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06005320 RID: 21280 RVA: 0x0015CCC4 File Offset: 0x0015AEC4
		// (set) Token: 0x06005321 RID: 21281 RVA: 0x0015CCF0 File Offset: 0x0015AEF0
		public Size ViewportExtent
		{
			get
			{
				IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
				IntUnsafeNativeMethods.GetViewportExtEx(new HandleRef(this, this.Hdc), size);
				return size.ToSize();
			}
			set
			{
				this.SetViewportExtent(value);
			}
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x0015CCFC File Offset: 0x0015AEFC
		public Size SetViewportExtent(Size newExtent)
		{
			IntNativeMethods.SIZE size = new IntNativeMethods.SIZE();
			IntUnsafeNativeMethods.SetViewportExtEx(new HandleRef(this, this.Hdc), newExtent.Width, newExtent.Height, size);
			return size.ToSize();
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06005323 RID: 21283 RVA: 0x0015CD38 File Offset: 0x0015AF38
		// (set) Token: 0x06005324 RID: 21284 RVA: 0x0015CD64 File Offset: 0x0015AF64
		public Point ViewportOrigin
		{
			get
			{
				IntNativeMethods.POINT point = new IntNativeMethods.POINT();
				IntUnsafeNativeMethods.GetViewportOrgEx(new HandleRef(this, this.Hdc), point);
				return point.ToPoint();
			}
			set
			{
				this.SetViewportOrigin(value);
			}
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x0015CD70 File Offset: 0x0015AF70
		public Point SetViewportOrigin(Point newOrigin)
		{
			IntNativeMethods.POINT point = new IntNativeMethods.POINT();
			IntUnsafeNativeMethods.SetViewportOrgEx(new HandleRef(this, this.Hdc), newOrigin.X, newOrigin.Y, point);
			return point.ToPoint();
		}

		// Token: 0x0400351D RID: 13597
		private IntPtr hDC;

		// Token: 0x0400351E RID: 13598
		private DeviceContextType dcType;

		// Token: 0x04003520 RID: 13600
		private bool disposed;

		// Token: 0x04003521 RID: 13601
		private IntPtr hWnd = (IntPtr)(-1);

		// Token: 0x04003522 RID: 13602
		private IntPtr hInitialPen;

		// Token: 0x04003523 RID: 13603
		private IntPtr hInitialBrush;

		// Token: 0x04003524 RID: 13604
		private IntPtr hInitialBmp;

		// Token: 0x04003525 RID: 13605
		private IntPtr hInitialFont;

		// Token: 0x04003526 RID: 13606
		private IntPtr hCurrentPen;

		// Token: 0x04003527 RID: 13607
		private IntPtr hCurrentBrush;

		// Token: 0x04003528 RID: 13608
		private IntPtr hCurrentBmp;

		// Token: 0x04003529 RID: 13609
		private IntPtr hCurrentFont;

		// Token: 0x0400352A RID: 13610
		private Stack contextStack;

		// Token: 0x0400352B RID: 13611
		private WindowsFont selectedFont;

		// Token: 0x02000862 RID: 2146
		internal class GraphicsState
		{
			// Token: 0x04004360 RID: 17248
			internal IntPtr hBrush;

			// Token: 0x04004361 RID: 17249
			internal IntPtr hFont;

			// Token: 0x04004362 RID: 17250
			internal IntPtr hPen;

			// Token: 0x04004363 RID: 17251
			internal IntPtr hBitmap;

			// Token: 0x04004364 RID: 17252
			internal WeakReference font;
		}
	}
}
