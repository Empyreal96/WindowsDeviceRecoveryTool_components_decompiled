using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Drawing
{
	/// <summary>Provides methods for creating graphics buffers that can be used for double buffering.</summary>
	// Token: 0x02000015 RID: 21
	public sealed class BufferedGraphicsContext : IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.BufferedGraphicsContext" /> class.</summary>
		// Token: 0x060000FE RID: 254 RVA: 0x00006504 File Offset: 0x00004704
		public BufferedGraphicsContext()
		{
			this.maximumBuffer.Width = 225;
			this.maximumBuffer.Height = 96;
			this.bufferSize = Size.Empty;
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060000FF RID: 255 RVA: 0x00006534 File Offset: 0x00004734
		~BufferedGraphicsContext()
		{
			this.Dispose(false);
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006564 File Offset: 0x00004764
		internal static TraceSwitch DoubleBuffering
		{
			get
			{
				if (BufferedGraphicsContext.doubleBuffering == null)
				{
					BufferedGraphicsContext.doubleBuffering = new TraceSwitch("DoubleBuffering", "Output information about double buffering");
				}
				return BufferedGraphicsContext.doubleBuffering;
			}
		}

		/// <summary>Gets or sets the maximum size of the buffer to use.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> indicating the maximum size of the buffer dimensions.</returns>
		/// <exception cref="T:System.ArgumentException">The height or width of the size is less than or equal to zero. </exception>
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006586 File Offset: 0x00004786
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00006590 File Offset: 0x00004790
		public Size MaximumBuffer
		{
			get
			{
				return this.maximumBuffer;
			}
			[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
			set
			{
				if (value.Width <= 0 || value.Height <= 0)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"MaximumBuffer",
						value
					}));
				}
				if (value.Width * value.Height < this.maximumBuffer.Width * this.maximumBuffer.Height)
				{
					this.Invalidate();
				}
				this.maximumBuffer = value;
			}
		}

		/// <summary>Creates a graphics buffer of the specified size using the pixel format of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="targetGraphics">The <see cref="T:System.Drawing.Graphics" /> to match the pixel format for the new buffer to. </param>
		/// <param name="targetRectangle">A <see cref="T:System.Drawing.Rectangle" /> indicating the size of the buffer to create. </param>
		/// <returns>A <see cref="T:System.Drawing.BufferedGraphics" /> that can be used to draw to a buffer of the specified dimensions.</returns>
		// Token: 0x06000103 RID: 259 RVA: 0x0000660D File Offset: 0x0000480D
		public BufferedGraphics Allocate(Graphics targetGraphics, Rectangle targetRectangle)
		{
			if (this.ShouldUseTempManager(targetRectangle))
			{
				return this.AllocBufferInTempManager(targetGraphics, IntPtr.Zero, targetRectangle);
			}
			return this.AllocBuffer(targetGraphics, IntPtr.Zero, targetRectangle);
		}

		/// <summary>Creates a graphics buffer of the specified size using the pixel format of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="targetDC">An <see cref="T:System.IntPtr" /> to a device context to match the pixel format of the new buffer to. </param>
		/// <param name="targetRectangle">A <see cref="T:System.Drawing.Rectangle" /> indicating the size of the buffer to create. </param>
		/// <returns>A <see cref="T:System.Drawing.BufferedGraphics" /> that can be used to draw to a buffer of the specified dimensions.</returns>
		// Token: 0x06000104 RID: 260 RVA: 0x00006633 File Offset: 0x00004833
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BufferedGraphics Allocate(IntPtr targetDC, Rectangle targetRectangle)
		{
			if (this.ShouldUseTempManager(targetRectangle))
			{
				return this.AllocBufferInTempManager(null, targetDC, targetRectangle);
			}
			return this.AllocBuffer(null, targetDC, targetRectangle);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006654 File Offset: 0x00004854
		private BufferedGraphics AllocBuffer(Graphics targetGraphics, IntPtr targetDC, Rectangle targetRectangle)
		{
			int num = Interlocked.CompareExchange(ref this.busy, 1, 0);
			if (num != 0)
			{
				return this.AllocBufferInTempManager(targetGraphics, targetDC, targetRectangle);
			}
			this.targetLoc = new Point(targetRectangle.X, targetRectangle.Y);
			try
			{
				Graphics bufferedGraphicsSurface;
				if (targetGraphics != null)
				{
					IntPtr hdc = targetGraphics.GetHdc();
					try
					{
						bufferedGraphicsSurface = this.CreateBuffer(hdc, -this.targetLoc.X, -this.targetLoc.Y, targetRectangle.Width, targetRectangle.Height);
						goto IL_A4;
					}
					finally
					{
						targetGraphics.ReleaseHdcInternal(hdc);
					}
				}
				bufferedGraphicsSurface = this.CreateBuffer(targetDC, -this.targetLoc.X, -this.targetLoc.Y, targetRectangle.Width, targetRectangle.Height);
				IL_A4:
				this.buffer = new BufferedGraphics(bufferedGraphicsSurface, this, targetGraphics, targetDC, this.targetLoc, this.virtualSize);
			}
			catch
			{
				this.busy = 0;
				throw;
			}
			return this.buffer;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00006750 File Offset: 0x00004950
		private BufferedGraphics AllocBufferInTempManager(Graphics targetGraphics, IntPtr targetDC, Rectangle targetRectangle)
		{
			BufferedGraphicsContext bufferedGraphicsContext = null;
			BufferedGraphics bufferedGraphics = null;
			try
			{
				bufferedGraphicsContext = new BufferedGraphicsContext();
				if (bufferedGraphicsContext != null)
				{
					bufferedGraphics = bufferedGraphicsContext.AllocBuffer(targetGraphics, targetDC, targetRectangle);
					bufferedGraphics.DisposeContext = true;
				}
			}
			finally
			{
				if (bufferedGraphicsContext != null && (bufferedGraphics == null || (bufferedGraphics != null && !bufferedGraphics.DisposeContext)))
				{
					bufferedGraphicsContext.Dispose();
				}
			}
			return bufferedGraphics;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000067A8 File Offset: 0x000049A8
		private bool bFillBitmapInfo(IntPtr hdc, IntPtr hpal, ref NativeMethods.BITMAPINFO_FLAT pbmi)
		{
			IntPtr intPtr = IntPtr.Zero;
			bool result = false;
			try
			{
				intPtr = SafeNativeMethods.CreateCompatibleBitmap(new HandleRef(null, hdc), 1, 1);
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException(SR.GetString("GraphicsBufferQueryFail"));
				}
				pbmi.bmiHeader_biSize = Marshal.SizeOf(typeof(NativeMethods.BITMAPINFOHEADER));
				pbmi.bmiColors = new byte[1024];
				SafeNativeMethods.GetDIBits(new HandleRef(null, hdc), new HandleRef(null, intPtr), 0, 0, IntPtr.Zero, ref pbmi, 0);
				if (pbmi.bmiHeader_biBitCount <= 8)
				{
					result = this.bFillColorTable(hdc, hpal, ref pbmi);
				}
				else
				{
					if (pbmi.bmiHeader_biCompression == 3)
					{
						SafeNativeMethods.GetDIBits(new HandleRef(null, hdc), new HandleRef(null, intPtr), 0, pbmi.bmiHeader_biHeight, IntPtr.Zero, ref pbmi, 0);
					}
					result = true;
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
					intPtr = IntPtr.Zero;
				}
			}
			return result;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000068A4 File Offset: 0x00004AA4
		private unsafe bool bFillColorTable(IntPtr hdc, IntPtr hpal, ref NativeMethods.BITMAPINFO_FLAT pbmi)
		{
			bool result = false;
			byte[] array = new byte[sizeof(NativeMethods.PALETTEENTRY) * 256];
			fixed (byte* bmiColors = pbmi.bmiColors)
			{
				fixed (byte* ptr = array)
				{
					NativeMethods.RGBQUAD* ptr2 = (NativeMethods.RGBQUAD*)bmiColors;
					NativeMethods.PALETTEENTRY* ptr3 = (NativeMethods.PALETTEENTRY*)ptr;
					int num = 1 << (int)pbmi.bmiHeader_biBitCount;
					if (num <= 256)
					{
						IntPtr handle = IntPtr.Zero;
						uint paletteEntries;
						if (hpal == IntPtr.Zero)
						{
							handle = Graphics.GetHalftonePalette();
							paletteEntries = SafeNativeMethods.GetPaletteEntries(new HandleRef(null, handle), 0, num, array);
						}
						else
						{
							paletteEntries = SafeNativeMethods.GetPaletteEntries(new HandleRef(null, hpal), 0, num, array);
						}
						if (paletteEntries != 0U)
						{
							for (int i = 0; i < num; i++)
							{
								ptr2[i].rgbRed = ptr3[i].peRed;
								ptr2[i].rgbGreen = ptr3[i].peGreen;
								ptr2[i].rgbBlue = ptr3[i].peBlue;
								ptr2[i].rgbReserved = 0;
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000069FC File Offset: 0x00004BFC
		private Graphics CreateBuffer(IntPtr src, int offsetX, int offsetY, int width, int height)
		{
			this.busy = 2;
			this.DisposeDC();
			this.busy = 1;
			this.compatDC = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, src));
			if (width > this.bufferSize.Width || height > this.bufferSize.Height)
			{
				int num = Math.Max(width, this.bufferSize.Width);
				int num2 = Math.Max(height, this.bufferSize.Height);
				this.busy = 2;
				this.DisposeBitmap();
				this.busy = 1;
				IntPtr zero = IntPtr.Zero;
				this.dib = this.CreateCompatibleDIB(src, IntPtr.Zero, num, num2, ref zero);
				this.bufferSize = new Size(num, num2);
			}
			this.oldBitmap = SafeNativeMethods.SelectObject(new HandleRef(this, this.compatDC), new HandleRef(this, this.dib));
			this.compatGraphics = Graphics.FromHdcInternal(this.compatDC);
			this.compatGraphics.TranslateTransform((float)(-(float)this.targetLoc.X), (float)(-(float)this.targetLoc.Y));
			this.virtualSize = new Size(width, height);
			return this.compatGraphics;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006B20 File Offset: 0x00004D20
		private IntPtr CreateCompatibleDIB(IntPtr hdc, IntPtr hpal, int ulWidth, int ulHeight, ref IntPtr ppvBits)
		{
			if (hdc == IntPtr.Zero)
			{
				throw new ArgumentNullException("hdc");
			}
			IntPtr intPtr = IntPtr.Zero;
			NativeMethods.BITMAPINFO_FLAT bitmapinfo_FLAT = default(NativeMethods.BITMAPINFO_FLAT);
			int objectType = UnsafeNativeMethods.GetObjectType(new HandleRef(null, hdc));
			if (objectType - 3 > 1 && objectType != 10 && objectType != 12)
			{
				throw new ArgumentException(SR.GetString("DCTypeInvalid"));
			}
			if (this.bFillBitmapInfo(hdc, hpal, ref bitmapinfo_FLAT))
			{
				bitmapinfo_FLAT.bmiHeader_biWidth = ulWidth;
				bitmapinfo_FLAT.bmiHeader_biHeight = ulHeight;
				if (bitmapinfo_FLAT.bmiHeader_biCompression == 0)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
				}
				else if (bitmapinfo_FLAT.bmiHeader_biBitCount == 16)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = ulWidth * ulHeight * 2;
				}
				else if (bitmapinfo_FLAT.bmiHeader_biBitCount == 32)
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = ulWidth * ulHeight * 4;
				}
				else
				{
					bitmapinfo_FLAT.bmiHeader_biSizeImage = 0;
				}
				bitmapinfo_FLAT.bmiHeader_biClrUsed = 0;
				bitmapinfo_FLAT.bmiHeader_biClrImportant = 0;
				intPtr = SafeNativeMethods.CreateDIBSection(new HandleRef(null, hdc), ref bitmapinfo_FLAT, 0, ref ppvBits, IntPtr.Zero, 0);
				Win32Exception ex = null;
				if (intPtr == IntPtr.Zero)
				{
					ex = new Win32Exception(Marshal.GetLastWin32Error());
				}
				if (ex != null)
				{
					throw ex;
				}
			}
			return intPtr;
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Drawing.BufferedGraphicsContext" />.</summary>
		// Token: 0x0600010B RID: 267 RVA: 0x00006C34 File Offset: 0x00004E34
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006C44 File Offset: 0x00004E44
		private void DisposeDC()
		{
			if (this.oldBitmap != IntPtr.Zero && this.compatDC != IntPtr.Zero)
			{
				SafeNativeMethods.SelectObject(new HandleRef(this, this.compatDC), new HandleRef(this, this.oldBitmap));
				this.oldBitmap = IntPtr.Zero;
			}
			if (this.compatDC != IntPtr.Zero)
			{
				UnsafeNativeMethods.DeleteDC(new HandleRef(this, this.compatDC));
				this.compatDC = IntPtr.Zero;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006CCD File Offset: 0x00004ECD
		private void DisposeBitmap()
		{
			if (this.dib != IntPtr.Zero)
			{
				SafeNativeMethods.DeleteObject(new HandleRef(this, this.dib));
				this.dib = IntPtr.Zero;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006D00 File Offset: 0x00004F00
		private void Dispose(bool disposing)
		{
			int num = Interlocked.CompareExchange(ref this.busy, 2, 0);
			if (disposing)
			{
				if (num == 1)
				{
					throw new InvalidOperationException(SR.GetString("GraphicsBufferCurrentlyBusy"));
				}
				if (this.compatGraphics != null)
				{
					this.compatGraphics.Dispose();
					this.compatGraphics = null;
				}
			}
			this.DisposeDC();
			this.DisposeBitmap();
			if (this.buffer != null)
			{
				this.buffer.Dispose();
				this.buffer = null;
			}
			this.bufferSize = Size.Empty;
			this.virtualSize = Size.Empty;
			this.busy = 0;
		}

		/// <summary>Disposes of the current graphics buffer, if a buffer has been allocated and has not yet been disposed.</summary>
		// Token: 0x0600010F RID: 271 RVA: 0x00006D90 File Offset: 0x00004F90
		public void Invalidate()
		{
			if (Interlocked.CompareExchange(ref this.busy, 2, 0) == 0)
			{
				this.Dispose();
				this.busy = 0;
				return;
			}
			this.invalidateWhenFree = true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006DC3 File Offset: 0x00004FC3
		internal void ReleaseBuffer(BufferedGraphics buffer)
		{
			this.buffer = null;
			if (this.invalidateWhenFree)
			{
				this.busy = 2;
				this.Dispose();
			}
			else
			{
				this.busy = 2;
				this.DisposeDC();
			}
			this.busy = 0;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006DF8 File Offset: 0x00004FF8
		private bool ShouldUseTempManager(Rectangle targetBounds)
		{
			return targetBounds.Width * targetBounds.Height > this.MaximumBuffer.Width * this.MaximumBuffer.Height;
		}

		// Token: 0x04000134 RID: 308
		private Size maximumBuffer;

		// Token: 0x04000135 RID: 309
		private Size bufferSize;

		// Token: 0x04000136 RID: 310
		private Size virtualSize;

		// Token: 0x04000137 RID: 311
		private Point targetLoc;

		// Token: 0x04000138 RID: 312
		private IntPtr compatDC;

		// Token: 0x04000139 RID: 313
		private IntPtr dib;

		// Token: 0x0400013A RID: 314
		private IntPtr oldBitmap;

		// Token: 0x0400013B RID: 315
		private Graphics compatGraphics;

		// Token: 0x0400013C RID: 316
		private BufferedGraphics buffer;

		// Token: 0x0400013D RID: 317
		private int busy;

		// Token: 0x0400013E RID: 318
		private bool invalidateWhenFree;

		// Token: 0x0400013F RID: 319
		private const int BUFFER_FREE = 0;

		// Token: 0x04000140 RID: 320
		private const int BUFFER_BUSY_PAINTING = 1;

		// Token: 0x04000141 RID: 321
		private const int BUFFER_BUSY_DISPOSING = 2;

		// Token: 0x04000142 RID: 322
		private static TraceSwitch doubleBuffering;
	}
}
