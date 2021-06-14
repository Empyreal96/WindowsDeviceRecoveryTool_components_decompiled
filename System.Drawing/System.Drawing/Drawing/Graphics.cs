using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.Drawing.Text;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing
{
	/// <summary>Encapsulates a GDI+ drawing surface. This class cannot be inherited.</summary>
	// Token: 0x0200001D RID: 29
	public sealed class Graphics : MarshalByRefObject, IDisposable, IDeviceContext
	{
		// Token: 0x060001EB RID: 491 RVA: 0x00008E5A File Offset: 0x0000705A
		private Graphics(IntPtr gdipNativeGraphics)
		{
			if (gdipNativeGraphics == IntPtr.Zero)
			{
				throw new ArgumentNullException("gdipNativeGraphics");
			}
			this.nativeGraphics = gdipNativeGraphics;
		}

		/// <summary>Creates a new <see cref="T:System.Drawing.Graphics" /> from the specified handle to a device context.</summary>
		/// <param name="hdc">Handle to a device context. </param>
		/// <returns>This method returns a new <see cref="T:System.Drawing.Graphics" /> for the specified device context.</returns>
		// Token: 0x060001EC RID: 492 RVA: 0x00008E81 File Offset: 0x00007081
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			if (hdc == IntPtr.Zero)
			{
				throw new ArgumentNullException("hdc");
			}
			return Graphics.FromHdcInternal(hdc);
		}

		/// <summary>Returns a <see cref="T:System.Drawing.Graphics" /> for the specified device context.</summary>
		/// <param name="hdc">Handle to a device context. </param>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> for the specified device context.</returns>
		// Token: 0x060001ED RID: 493 RVA: 0x00008EAC File Offset: 0x000070AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Graphics FromHdcInternal(IntPtr hdc)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHDC(new HandleRef(null, hdc), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		/// <summary>Creates a new <see cref="T:System.Drawing.Graphics" /> from the specified handle to a device context and handle to a device.</summary>
		/// <param name="hdc">Handle to a device context. </param>
		/// <param name="hdevice">Handle to a device. </param>
		/// <returns>This method returns a new <see cref="T:System.Drawing.Graphics" /> for the specified device context and device.</returns>
		// Token: 0x060001EE RID: 494 RVA: 0x00008EE0 File Offset: 0x000070E0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHdc(IntPtr hdc, IntPtr hdevice)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHDC2(new HandleRef(null, hdc), new HandleRef(null, hdevice), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		/// <summary>Creates a new <see cref="T:System.Drawing.Graphics" /> from the specified handle to a window.</summary>
		/// <param name="hwnd">Handle to a window. </param>
		/// <returns>This method returns a new <see cref="T:System.Drawing.Graphics" /> for the specified window handle.</returns>
		// Token: 0x060001EF RID: 495 RVA: 0x00008F23 File Offset: 0x00007123
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static Graphics FromHwnd(IntPtr hwnd)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return Graphics.FromHwndInternal(hwnd);
		}

		/// <summary>Creates a new <see cref="T:System.Drawing.Graphics" /> for the specified windows handle.</summary>
		/// <param name="hwnd">Handle to a window. </param>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> for the specified window handle.</returns>
		// Token: 0x060001F0 RID: 496 RVA: 0x00008F38 File Offset: 0x00007138
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static Graphics FromHwndInternal(IntPtr hwnd)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateFromHWND(new HandleRef(null, hwnd), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero);
		}

		/// <summary>Creates a new <see cref="T:System.Drawing.Graphics" /> from the specified <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> from which to create the new <see cref="T:System.Drawing.Graphics" />. </param>
		/// <returns>This method returns a new <see cref="T:System.Drawing.Graphics" /> for the specified <see cref="T:System.Drawing.Image" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.Exception">
		///         <paramref name="image" /> has an indexed pixel format or its format is undefined.</exception>
		// Token: 0x060001F1 RID: 497 RVA: 0x00008F6C File Offset: 0x0000716C
		public static Graphics FromImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if ((image.PixelFormat & PixelFormat.Indexed) != PixelFormat.Undefined)
			{
				throw new Exception(SR.GetString("GdiplusCannotCreateGraphicsFromIndexedPixelFormat"));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetImageGraphicsContext(new HandleRef(image, image.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Graphics(zero)
			{
				backingImage = image
			};
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00008FD8 File Offset: 0x000071D8
		internal IntPtr NativeGraphics
		{
			get
			{
				return this.nativeGraphics;
			}
		}

		/// <summary>Gets the handle to the device context associated with this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>Handle to the device context associated with this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x060001F3 RID: 499 RVA: 0x00008FE0 File Offset: 0x000071E0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHdc()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetDC(new HandleRef(this, this.NativeGraphics), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeHdc = zero;
			return this.nativeHdc;
		}

		/// <summary>Releases a device context handle obtained by a previous call to the <see cref="M:System.Drawing.Graphics.GetHdc" /> method of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="hdc">Handle to a device context obtained by a previous call to the <see cref="M:System.Drawing.Graphics.GetHdc" /> method of this <see cref="T:System.Drawing.Graphics" />. </param>
		// Token: 0x060001F4 RID: 500 RVA: 0x0000901E File Offset: 0x0000721E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void ReleaseHdc(IntPtr hdc)
		{
			IntSecurity.Win32HandleManipulation.Demand();
			this.ReleaseHdcInternal(hdc);
		}

		/// <summary>Releases a device context handle obtained by a previous call to the <see cref="M:System.Drawing.Graphics.GetHdc" /> method of this <see cref="T:System.Drawing.Graphics" />.</summary>
		// Token: 0x060001F5 RID: 501 RVA: 0x00009031 File Offset: 0x00007231
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void ReleaseHdc()
		{
			this.ReleaseHdcInternal(this.nativeHdc);
		}

		/// <summary>Releases a handle to a device context.</summary>
		/// <param name="hdc">Handle to a device context. </param>
		// Token: 0x060001F6 RID: 502 RVA: 0x00009040 File Offset: 0x00007240
		[EditorBrowsable(EditorBrowsableState.Never)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void ReleaseHdcInternal(IntPtr hdc)
		{
			int num = SafeNativeMethods.Gdip.GdipReleaseDC(new HandleRef(this, this.NativeGraphics), new HandleRef(null, hdc));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeHdc = IntPtr.Zero;
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Graphics" />.</summary>
		// Token: 0x060001F7 RID: 503 RVA: 0x0000907B File Offset: 0x0000727B
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000908C File Offset: 0x0000728C
		private void Dispose(bool disposing)
		{
			while (this.previousContext != null)
			{
				GraphicsContext previous = this.previousContext.Previous;
				this.previousContext.Dispose();
				this.previousContext = previous;
			}
			if (this.nativeGraphics != IntPtr.Zero)
			{
				try
				{
					if (this.nativeHdc != IntPtr.Zero)
					{
						this.ReleaseHdc();
					}
					if (this.PrintingHelper != null)
					{
						DeviceContext deviceContext = this.PrintingHelper as DeviceContext;
						if (deviceContext != null)
						{
							deviceContext.Dispose();
							this.printingHelper = null;
						}
					}
					SafeNativeMethods.Gdip.GdipDeleteGraphics(new HandleRef(this, this.nativeGraphics));
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
					this.nativeGraphics = IntPtr.Zero;
				}
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060001F9 RID: 505 RVA: 0x00009158 File Offset: 0x00007358
		~Graphics()
		{
			this.Dispose(false);
		}

		/// <summary>Forces execution of all pending graphics operations and returns immediately without waiting for the operations to finish.</summary>
		// Token: 0x060001FA RID: 506 RVA: 0x00009188 File Offset: 0x00007388
		public void Flush()
		{
			this.Flush(FlushIntention.Flush);
		}

		/// <summary>Forces execution of all pending graphics operations with the method waiting or not waiting, as specified, to return before the operations finish.</summary>
		/// <param name="intention">Member of the <see cref="T:System.Drawing.Drawing2D.FlushIntention" /> enumeration that specifies whether the method returns immediately or waits for any existing operations to finish. </param>
		// Token: 0x060001FB RID: 507 RVA: 0x00009194 File Offset: 0x00007394
		public void Flush(FlushIntention intention)
		{
			int num = SafeNativeMethods.Gdip.GdipFlush(new HandleRef(this, this.NativeGraphics), intention);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets a value that specifies how composited images are drawn to this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>This property specifies a member of the <see cref="T:System.Drawing.Drawing2D.CompositingMode" /> enumeration. The default is <see cref="F:System.Drawing.Drawing2D.CompositingMode.SourceOver" />.</returns>
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060001FC RID: 508 RVA: 0x000091C0 File Offset: 0x000073C0
		// (set) Token: 0x060001FD RID: 509 RVA: 0x000091F0 File Offset: 0x000073F0
		public CompositingMode CompositingMode
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetCompositingMode(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return (CompositingMode)result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CompositingMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetCompositingMode(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the rendering origin of this <see cref="T:System.Drawing.Graphics" /> for dithering and for hatch brushes.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> structure that represents the dither origin for 8-bits-per-pixel and 16-bits-per-pixel dithering and is also used to set the origin for hatch brushes.</returns>
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00009240 File Offset: 0x00007440
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00009274 File Offset: 0x00007474
		public Point RenderingOrigin
		{
			get
			{
				int x;
				int y;
				int num = SafeNativeMethods.Gdip.GdipGetRenderingOrigin(new HandleRef(this, this.NativeGraphics), out x, out y);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return new Point(x, y);
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetRenderingOrigin(new HandleRef(this, this.NativeGraphics), value.X, value.Y);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the rendering quality of composited images drawn to this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>This property specifies a member of the <see cref="T:System.Drawing.Drawing2D.CompositingQuality" /> enumeration. The default is <see cref="F:System.Drawing.Drawing2D.CompositingQuality.Default" />.</returns>
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000200 RID: 512 RVA: 0x000092AC File Offset: 0x000074AC
		// (set) Token: 0x06000201 RID: 513 RVA: 0x000092D8 File Offset: 0x000074D8
		public CompositingQuality CompositingQuality
		{
			get
			{
				CompositingQuality result;
				int num = SafeNativeMethods.Gdip.GdipGetCompositingQuality(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(CompositingQuality));
				}
				int num = SafeNativeMethods.Gdip.GdipSetCompositingQuality(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the rendering mode for text associated with this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Text.TextRenderingHint" /> values.</returns>
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00009328 File Offset: 0x00007528
		// (set) Token: 0x06000203 RID: 515 RVA: 0x00009358 File Offset: 0x00007558
		public TextRenderingHint TextRenderingHint
		{
			get
			{
				TextRenderingHint result = TextRenderingHint.SystemDefault;
				int num = SafeNativeMethods.Gdip.GdipGetTextRenderingHint(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(TextRenderingHint));
				}
				int num = SafeNativeMethods.Gdip.GdipSetTextRenderingHint(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the gamma correction value for rendering text.</summary>
		/// <returns>The gamma correction value used for rendering antialiased and ClearType text.</returns>
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000093A8 File Offset: 0x000075A8
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000093D8 File Offset: 0x000075D8
		public int TextContrast
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetTextContrast(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetTextContrast(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the rendering quality for this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.SmoothingMode" /> values.</returns>
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00009404 File Offset: 0x00007604
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00009434 File Offset: 0x00007634
		public SmoothingMode SmoothingMode
		{
			get
			{
				SmoothingMode result = SmoothingMode.Default;
				int num = SafeNativeMethods.Gdip.GdipGetSmoothingMode(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SmoothingMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetSmoothingMode(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or set a value specifying how pixels are offset during rendering of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>This property specifies a member of the <see cref="T:System.Drawing.Drawing2D.PixelOffsetMode" /> enumeration </returns>
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000208 RID: 520 RVA: 0x00009484 File Offset: 0x00007684
		// (set) Token: 0x06000209 RID: 521 RVA: 0x000094B4 File Offset: 0x000076B4
		public PixelOffsetMode PixelOffsetMode
		{
			get
			{
				PixelOffsetMode result = PixelOffsetMode.Default;
				int num = SafeNativeMethods.Gdip.GdipGetPixelOffsetMode(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PixelOffsetMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPixelOffsetMode(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00009504 File Offset: 0x00007704
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000950C File Offset: 0x0000770C
		internal object PrintingHelper
		{
			get
			{
				return this.printingHelper;
			}
			set
			{
				this.printingHelper = value;
			}
		}

		/// <summary>Gets or sets the interpolation mode associated with this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.InterpolationMode" /> values.</returns>
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00009518 File Offset: 0x00007718
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00009548 File Offset: 0x00007748
		public InterpolationMode InterpolationMode
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetInterpolationMode(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return (InterpolationMode)result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -1, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InterpolationMode));
				}
				int num = SafeNativeMethods.Gdip.GdipSetInterpolationMode(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets a copy of the geometric world transformation for this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the geometric world transformation for this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00009598 File Offset: 0x00007798
		// (set) Token: 0x0600020F RID: 527 RVA: 0x000095D4 File Offset: 0x000077D4
		public Matrix Transform
		{
			get
			{
				Matrix matrix = new Matrix();
				int num = SafeNativeMethods.Gdip.GdipGetWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(matrix, matrix.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return matrix;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(value, value.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the unit of measure used for page coordinates in this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.GraphicsUnit" /> values other than <see cref="F:System.Drawing.GraphicsUnit.World" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <see cref="P:System.Drawing.Graphics.PageUnit" /> is set to <see cref="F:System.Drawing.GraphicsUnit.World" />, which is not a physical unit. </exception>
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000960C File Offset: 0x0000780C
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000963C File Offset: 0x0000783C
		public GraphicsUnit PageUnit
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetPageUnit(new HandleRef(this, this.NativeGraphics), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return (GraphicsUnit)result;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 6))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(GraphicsUnit));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPageUnit(new HandleRef(this, this.NativeGraphics), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets or sets the scaling between world units and page units for this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>This property specifies a value for the scaling between world units and page units for this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000968C File Offset: 0x0000788C
		// (set) Token: 0x06000213 RID: 531 RVA: 0x000096C0 File Offset: 0x000078C0
		public float PageScale
		{
			get
			{
				float[] array = new float[1];
				int num = SafeNativeMethods.Gdip.GdipGetPageScale(new HandleRef(this, this.NativeGraphics), array);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array[0];
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetPageScale(new HandleRef(this, this.NativeGraphics), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		/// <summary>Gets the horizontal resolution of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>The value, in dots per inch, for the horizontal resolution supported by this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000096EC File Offset: 0x000078EC
		public float DpiX
		{
			get
			{
				float[] array = new float[1];
				int num = SafeNativeMethods.Gdip.GdipGetDpiX(new HandleRef(this, this.NativeGraphics), array);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array[0];
			}
		}

		/// <summary>Gets the vertical resolution of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>The value, in dots per inch, for the vertical resolution supported by this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00009720 File Offset: 0x00007920
		public float DpiY
		{
			get
			{
				float[] array = new float[1];
				int num = SafeNativeMethods.Gdip.GdipGetDpiY(new HandleRef(this, this.NativeGraphics), array);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array[0];
			}
		}

		/// <summary>Performs a bit-block transfer of color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="upperLeftSource">The point at the upper-left corner of the source rectangle.</param>
		/// <param name="upperLeftDestination">The point at the upper-left corner of the destination rectangle.</param>
		/// <param name="blockRegionSize">The size of the area to be transferred.</param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The operation failed.</exception>
		// Token: 0x06000216 RID: 534 RVA: 0x00009754 File Offset: 0x00007954
		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
		{
			this.CopyFromScreen(upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y, blockRegionSize);
		}

		/// <summary>Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
		/// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle.</param>
		/// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
		/// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
		/// <param name="blockRegionSize">The size of the area to be transferred.</param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The operation failed.</exception>
		// Token: 0x06000217 RID: 535 RVA: 0x00009779 File Offset: 0x00007979
		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
			this.CopyFromScreen(sourceX, sourceY, destinationX, destinationY, blockRegionSize, CopyPixelOperation.SourceCopy);
		}

		/// <summary>Performs a bit-block transfer of color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="upperLeftSource">The point at the upper-left corner of the source rectangle.</param>
		/// <param name="upperLeftDestination">The point at the upper-left corner of the destination rectangle.</param>
		/// <param name="blockRegionSize">The size of the area to be transferred.</param>
		/// <param name="copyPixelOperation">One of the <see cref="T:System.Drawing.CopyPixelOperation" /> values.</param>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="copyPixelOperation" /> is not a member of <see cref="T:System.Drawing.CopyPixelOperation" />.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The operation failed.</exception>
		// Token: 0x06000218 RID: 536 RVA: 0x0000978D File Offset: 0x0000798D
		public void CopyFromScreen(Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			this.CopyFromScreen(upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y, blockRegionSize, copyPixelOperation);
		}

		/// <summary>Performs a bit-block transfer of the color data, corresponding to a rectangle of pixels, from the screen to the drawing surface of the <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="sourceX">The x-coordinate of the point at the upper-left corner of the source rectangle.</param>
		/// <param name="sourceY">The y-coordinate of the point at the upper-left corner of the source rectangle</param>
		/// <param name="destinationX">The x-coordinate of the point at the upper-left corner of the destination rectangle.</param>
		/// <param name="destinationY">The y-coordinate of the point at the upper-left corner of the destination rectangle.</param>
		/// <param name="blockRegionSize">The size of the area to be transferred.</param>
		/// <param name="copyPixelOperation">One of the <see cref="T:System.Drawing.CopyPixelOperation" /> values.</param>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="copyPixelOperation" /> is not a member of <see cref="T:System.Drawing.CopyPixelOperation" />.</exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The operation failed.</exception>
		// Token: 0x06000219 RID: 537 RVA: 0x000097B4 File Offset: 0x000079B4
		public void CopyFromScreen(int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			if (copyPixelOperation <= CopyPixelOperation.SourceInvert)
			{
				if (copyPixelOperation <= CopyPixelOperation.NotSourceCopy)
				{
					if (copyPixelOperation <= CopyPixelOperation.Blackness)
					{
						if (copyPixelOperation == CopyPixelOperation.NoMirrorBitmap || copyPixelOperation == CopyPixelOperation.Blackness)
						{
							goto IL_11B;
						}
					}
					else if (copyPixelOperation == CopyPixelOperation.NotSourceErase || copyPixelOperation == CopyPixelOperation.NotSourceCopy)
					{
						goto IL_11B;
					}
				}
				else if (copyPixelOperation <= CopyPixelOperation.DestinationInvert)
				{
					if (copyPixelOperation == CopyPixelOperation.SourceErase || copyPixelOperation == CopyPixelOperation.DestinationInvert)
					{
						goto IL_11B;
					}
				}
				else if (copyPixelOperation == CopyPixelOperation.PatInvert || copyPixelOperation == CopyPixelOperation.SourceInvert)
				{
					goto IL_11B;
				}
			}
			else if (copyPixelOperation <= CopyPixelOperation.SourceCopy)
			{
				if (copyPixelOperation <= CopyPixelOperation.MergePaint)
				{
					if (copyPixelOperation == CopyPixelOperation.SourceAnd || copyPixelOperation == CopyPixelOperation.MergePaint)
					{
						goto IL_11B;
					}
				}
				else if (copyPixelOperation == CopyPixelOperation.MergeCopy || copyPixelOperation == CopyPixelOperation.SourceCopy)
				{
					goto IL_11B;
				}
			}
			else if (copyPixelOperation <= CopyPixelOperation.PatCopy)
			{
				if (copyPixelOperation == CopyPixelOperation.SourcePaint || copyPixelOperation == CopyPixelOperation.PatCopy)
				{
					goto IL_11B;
				}
			}
			else if (copyPixelOperation == CopyPixelOperation.PatPaint || copyPixelOperation == CopyPixelOperation.Whiteness || copyPixelOperation == CopyPixelOperation.CaptureBlt)
			{
				goto IL_11B;
			}
			throw new InvalidEnumArgumentException("value", (int)copyPixelOperation, typeof(CopyPixelOperation));
			IL_11B:
			new UIPermission(UIPermissionWindow.AllWindows).Demand();
			int width = blockRegionSize.Width;
			int height = blockRegionSize.Height;
			using (DeviceContext deviceContext = DeviceContext.FromHwnd(IntPtr.Zero))
			{
				HandleRef hSrcDC = new HandleRef(null, deviceContext.Hdc);
				HandleRef hDC = new HandleRef(null, this.GetHdc());
				try
				{
					if (SafeNativeMethods.BitBlt(hDC, destinationX, destinationY, width, height, hSrcDC, sourceX, sourceY, (int)copyPixelOperation) == 0)
					{
						throw new Win32Exception();
					}
				}
				finally
				{
					this.ReleaseHdc();
				}
			}
		}

		/// <summary>Resets the world transformation matrix of this <see cref="T:System.Drawing.Graphics" /> to the identity matrix.</summary>
		// Token: 0x0600021A RID: 538 RVA: 0x0000996C File Offset: 0x00007B6C
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetWorldTransform(new HandleRef(this, this.NativeGraphics));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Multiplies the world transformation of this <see cref="T:System.Drawing.Graphics" /> and specified the <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">4x4 <see cref="T:System.Drawing.Drawing2D.Matrix" /> that multiplies the world transformation. </param>
		// Token: 0x0600021B RID: 539 RVA: 0x00009995 File Offset: 0x00007B95
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		/// <summary>Multiplies the world transformation of this <see cref="T:System.Drawing.Graphics" /> and specified the <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="matrix">4x4 <see cref="T:System.Drawing.Drawing2D.Matrix" /> that multiplies the world transformation. </param>
		/// <param name="order">Member of the <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that determines the order of the multiplication. </param>
		// Token: 0x0600021C RID: 540 RVA: 0x000099A0 File Offset: 0x00007BA0
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyWorldTransform(new HandleRef(this, this.NativeGraphics), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Changes the origin of the coordinate system by prepending the specified translation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="dx">The x-coordinate of the translation. </param>
		/// <param name="dy">The y-coordinate of the translation. </param>
		// Token: 0x0600021D RID: 541 RVA: 0x000099E4 File Offset: 0x00007BE4
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		/// <summary>Changes the origin of the coordinate system by applying the specified translation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" /> in the specified order.</summary>
		/// <param name="dx">The x-coordinate of the translation. </param>
		/// <param name="dy">The y-coordinate of the translation. </param>
		/// <param name="order">Member of the <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies whether the translation is prepended or appended to the transformation matrix. </param>
		// Token: 0x0600021E RID: 542 RVA: 0x000099F0 File Offset: 0x00007BF0
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateWorldTransform(new HandleRef(this, this.NativeGraphics), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified scaling operation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" /> by prepending it to the object's transformation matrix.</summary>
		/// <param name="sx">Scale factor in the x direction. </param>
		/// <param name="sy">Scale factor in the y direction. </param>
		// Token: 0x0600021F RID: 543 RVA: 0x00009A1C File Offset: 0x00007C1C
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		/// <summary>Applies the specified scaling operation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" /> in the specified order.</summary>
		/// <param name="sx">Scale factor in the x direction. </param>
		/// <param name="sy">Scale factor in the y direction. </param>
		/// <param name="order">Member of the <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies whether the scaling operation is prepended or appended to the transformation matrix. </param>
		// Token: 0x06000220 RID: 544 RVA: 0x00009A28 File Offset: 0x00007C28
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleWorldTransform(new HandleRef(this, this.NativeGraphics), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified rotation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="angle">Angle of rotation in degrees. </param>
		// Token: 0x06000221 RID: 545 RVA: 0x00009A54 File Offset: 0x00007C54
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		/// <summary>Applies the specified rotation to the transformation matrix of this <see cref="T:System.Drawing.Graphics" /> in the specified order.</summary>
		/// <param name="angle">Angle of rotation in degrees. </param>
		/// <param name="order">Member of the <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies whether the rotation is appended or prepended to the matrix transformation. </param>
		// Token: 0x06000222 RID: 546 RVA: 0x00009A60 File Offset: 0x00007C60
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateWorldTransform(new HandleRef(this, this.NativeGraphics), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Transforms an array of points from one coordinate space to another using the current world and page transformations of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="destSpace">Member of the <see cref="T:System.Drawing.Drawing2D.CoordinateSpace" /> enumeration that specifies the destination coordinate space. </param>
		/// <param name="srcSpace">Member of the <see cref="T:System.Drawing.Drawing2D.CoordinateSpace" /> enumeration that specifies the source coordinate space. </param>
		/// <param name="pts">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the points to transform. </param>
		// Token: 0x06000223 RID: 547 RVA: 0x00009A8C File Offset: 0x00007C8C
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			int num = SafeNativeMethods.Gdip.GdipTransformPoints(new HandleRef(this, this.NativeGraphics), (int)destSpace, (int)srcSpace, intPtr, pts.Length);
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				PointF[] array = SafeNativeMethods.Gdip.ConvertGPPOINTFArrayF(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Transforms an array of points from one coordinate space to another using the current world and page transformations of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="destSpace">Member of the <see cref="T:System.Drawing.Drawing2D.CoordinateSpace" /> enumeration that specifies the destination coordinate space. </param>
		/// <param name="srcSpace">Member of the <see cref="T:System.Drawing.Drawing2D.CoordinateSpace" /> enumeration that specifies the source coordinate space. </param>
		/// <param name="pts">Array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transformation. </param>
		// Token: 0x06000224 RID: 548 RVA: 0x00009B10 File Offset: 0x00007D10
		public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			int num = SafeNativeMethods.Gdip.GdipTransformPointsI(new HandleRef(this, this.NativeGraphics), (int)destSpace, (int)srcSpace, intPtr, pts.Length);
			try
			{
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				Point[] array = SafeNativeMethods.Gdip.ConvertGPPOINTArray(intPtr, pts.Length);
				for (int i = 0; i < pts.Length; i++)
				{
					pts[i] = array[i];
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Gets the nearest color to the specified <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="color">
		///       <see cref="T:System.Drawing.Color" /> structure for which to find a match. </param>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure that represents the nearest color to the one specified with the <paramref name="color" /> parameter.</returns>
		// Token: 0x06000225 RID: 549 RVA: 0x00009B94 File Offset: 0x00007D94
		public Color GetNearestColor(Color color)
		{
			int argb = color.ToArgb();
			int num = SafeNativeMethods.Gdip.GdipGetNearestColor(new HandleRef(this, this.NativeGraphics), ref argb);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Color.FromArgb(argb);
		}

		/// <summary>Draws a line connecting the two points specified by the coordinate pairs.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line. </param>
		/// <param name="x1">The x-coordinate of the first point. </param>
		/// <param name="y1">The y-coordinate of the first point. </param>
		/// <param name="x2">The x-coordinate of the second point. </param>
		/// <param name="y2">The y-coordinate of the second point. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000226 RID: 550 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawLine(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a line connecting two <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line. </param>
		/// <param name="pt1">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the first point to connect. </param>
		/// <param name="pt2">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the second point to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000227 RID: 551 RVA: 0x00009C16 File Offset: 0x00007E16
		public void DrawLine(Pen pen, PointF pt1, PointF pt2)
		{
			this.DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>Draws a series of line segments that connect an array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line segments. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the points to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000228 RID: 552 RVA: 0x00009C3C File Offset: 0x00007E3C
		public void DrawLines(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawLines(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a line connecting the two points specified by the coordinate pairs.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line. </param>
		/// <param name="x1">The x-coordinate of the first point. </param>
		/// <param name="y1">The y-coordinate of the first point. </param>
		/// <param name="x2">The x-coordinate of the second point. </param>
		/// <param name="y2">The y-coordinate of the second point. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000229 RID: 553 RVA: 0x00009CB4 File Offset: 0x00007EB4
		public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawLineI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a line connecting two <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line. </param>
		/// <param name="pt1">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the first point to connect. </param>
		/// <param name="pt2">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the second point to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600022A RID: 554 RVA: 0x00009CFA File Offset: 0x00007EFA
		public void DrawLine(Pen pen, Point pt1, Point pt2)
		{
			this.DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>Draws a series of line segments that connect an array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the line segments. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that represent the points to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600022B RID: 555 RVA: 0x00009D20 File Offset: 0x00007F20
		public void DrawLines(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawLinesI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws an arc representing a portion of an ellipse specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the arc. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the rectangle that defines the ellipse. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to ending point of the arc. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600022C RID: 556 RVA: 0x00009D98 File Offset: 0x00007F98
		public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawArc(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws an arc representing a portion of an ellipse specified by a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the arc. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that defines the boundaries of the ellipse. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to ending point of the arc. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" /></exception>
		// Token: 0x0600022D RID: 557 RVA: 0x00009DE2 File Offset: 0x00007FE2
		public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			this.DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws an arc representing a portion of an ellipse specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the arc. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the rectangle that defines the ellipse. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to ending point of the arc. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="rects" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="rects" /> is a zero-length array.</exception>
		// Token: 0x0600022E RID: 558 RVA: 0x00009E0C File Offset: 0x0000800C
		public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawArcI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws an arc representing a portion of an ellipse specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the arc. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that defines the boundaries of the ellipse. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to ending point of the arc. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600022F RID: 559 RVA: 0x00009E58 File Offset: 0x00008058
		public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.DrawArc(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws a Bézier spline defined by four ordered pairs of coordinates that represent points.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="x1">The x-coordinate of the starting point of the curve. </param>
		/// <param name="y1">The y-coordinate of the starting point of the curve. </param>
		/// <param name="x2">The x-coordinate of the first control point of the curve. </param>
		/// <param name="y2">The y-coordinate of the first control point of the curve. </param>
		/// <param name="x3">The x-coordinate of the second control point of the curve. </param>
		/// <param name="y3">The y-coordinate of the second control point of the curve. </param>
		/// <param name="x4">The x-coordinate of the ending point of the curve. </param>
		/// <param name="y4">The y-coordinate of the ending point of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000230 RID: 560 RVA: 0x00009E84 File Offset: 0x00008084
		public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawBezier(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x1, y1, x2, y2, x3, y3, x4, y4);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a Bézier spline defined by four <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="pt1">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the starting point of the curve. </param>
		/// <param name="pt2">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the first control point for the curve. </param>
		/// <param name="pt3">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the second control point for the curve. </param>
		/// <param name="pt4">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the ending point of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000231 RID: 561 RVA: 0x00009ED4 File Offset: 0x000080D4
		public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
		{
			this.DrawBezier(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
		}

		/// <summary>Draws a series of Bézier splines from an array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the points that determine the curve. The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000232 RID: 562 RVA: 0x00009F20 File Offset: 0x00008120
		public void DrawBeziers(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawBeziers(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a Bézier spline defined by four <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> structure that determines the color, width, and style of the curve. </param>
		/// <param name="pt1">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the starting point of the curve. </param>
		/// <param name="pt2">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the first control point for the curve. </param>
		/// <param name="pt3">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the second control point for the curve. </param>
		/// <param name="pt4">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the ending point of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000233 RID: 563 RVA: 0x00009F98 File Offset: 0x00008198
		public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
		{
			this.DrawBezier(pen, (float)pt1.X, (float)pt1.Y, (float)pt2.X, (float)pt2.Y, (float)pt3.X, (float)pt3.Y, (float)pt4.X, (float)pt4.Y);
		}

		/// <summary>Draws a series of Bézier splines from an array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that represent the points that determine the curve. The number of points in the array should be a multiple of 3 plus 1, such as 4, 7, or 10.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000234 RID: 564 RVA: 0x00009FEC File Offset: 0x000081EC
		public void DrawBeziers(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawBeziersI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the rectangle. </param>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000235 RID: 565 RVA: 0x0000A064 File Offset: 0x00008264
		public void DrawRectangle(Pen pen, Rectangle rect)
		{
			this.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Draws a rectangle specified by a coordinate pair, a width, and a height.</summary>
		/// <param name="pen">A <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the rectangle. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw. </param>
		/// <param name="width">The width of the rectangle to draw. </param>
		/// <param name="height">The height of the rectangle to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000236 RID: 566 RVA: 0x0000A08C File Offset: 0x0000828C
		public void DrawRectangle(Pen pen, float x, float y, float width, float height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawRectangle(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a rectangle specified by a coordinate pair, a width, and a height.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the rectangle. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw. </param>
		/// <param name="width">Width of the rectangle to draw. </param>
		/// <param name="height">Height of the rectangle to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000237 RID: 567 RVA: 0x0000A0D4 File Offset: 0x000082D4
		public void DrawRectangle(Pen pen, int x, int y, int width, int height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawRectangleI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a series of rectangles specified by <see cref="T:System.Drawing.RectangleF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the outlines of the rectangles. </param>
		/// <param name="rects">Array of <see cref="T:System.Drawing.RectangleF" /> structures that represent the rectangles to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="rects" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="rects" /> is a zero-length array.</exception>
		// Token: 0x06000238 RID: 568 RVA: 0x0000A11C File Offset: 0x0000831C
		public void DrawRectangles(Pen pen, RectangleF[] rects)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawRectangles(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), rects.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a series of rectangles specified by <see cref="T:System.Drawing.Rectangle" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the outlines of the rectangles. </param>
		/// <param name="rects">Array of <see cref="T:System.Drawing.Rectangle" /> structures that represent the rectangles to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="rects" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="rects" /> is a zero-length array.</exception>
		// Token: 0x06000239 RID: 569 RVA: 0x0000A194 File Offset: 0x00008394
		public void DrawRectangles(Pen pen, Rectangle[] rects)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawRectanglesI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), rects.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws an ellipse defined by a bounding <see cref="T:System.Drawing.RectangleF" />.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the ellipse. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that defines the boundaries of the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023A RID: 570 RVA: 0x0000A20C File Offset: 0x0000840C
		public void DrawEllipse(Pen pen, RectangleF rect)
		{
			this.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a height, and a width.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the ellipse. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023B RID: 571 RVA: 0x0000A234 File Offset: 0x00008434
		public void DrawEllipse(Pen pen, float x, float y, float width, float height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawEllipse(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws an ellipse specified by a bounding <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the ellipse. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that defines the boundaries of the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023C RID: 572 RVA: 0x0000A27A File Offset: 0x0000847A
		public void DrawEllipse(Pen pen, Rectangle rect)
		{
			this.DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Draws an ellipse defined by a bounding rectangle specified by coordinates for the upper-left corner of the rectangle, a height, and a width.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the ellipse. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023D RID: 573 RVA: 0x0000A2A0 File Offset: 0x000084A0
		public void DrawEllipse(Pen pen, int x, int y, int width, int height)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawEllipseI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a pie shape defined by an ellipse specified by a <see cref="T:System.Drawing.RectangleF" /> structure and two radial lines.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the pie shape. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that represents the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape. </param>
		/// <param name="sweepAngle">Angle measured in degrees clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie shape. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023E RID: 574 RVA: 0x0000A2E6 File Offset: 0x000084E6
		public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
		{
			this.DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws a pie shape defined by an ellipse specified by a coordinate pair, a width, a height, and two radial lines.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the pie shape. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape. </param>
		/// <param name="sweepAngle">Angle measured in degrees clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie shape. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x0600023F RID: 575 RVA: 0x0000A310 File Offset: 0x00008510
		public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawPie(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a pie shape defined by an ellipse specified by a <see cref="T:System.Drawing.Rectangle" /> structure and two radial lines.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the pie shape. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape. </param>
		/// <param name="sweepAngle">Angle measured in degrees clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie shape. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000240 RID: 576 RVA: 0x0000A35A File Offset: 0x0000855A
		public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.DrawPie(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Draws a pie shape defined by an ellipse specified by a coordinate pair, a width, a height, and two radial lines.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the pie shape. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie shape comes. </param>
		/// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape. </param>
		/// <param name="sweepAngle">Angle measured in degrees clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie shape. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000241 RID: 577 RVA: 0x0000A388 File Offset: 0x00008588
		public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawPieI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a polygon defined by an array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the polygon. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the vertices of the polygon. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000242 RID: 578 RVA: 0x0000A3D4 File Offset: 0x000085D4
		public void DrawPolygon(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawPolygon(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a polygon defined by an array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the polygon. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that represent the vertices of the polygon. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.</exception>
		// Token: 0x06000243 RID: 579 RVA: 0x0000A44C File Offset: 0x0000864C
		public void DrawPolygon(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawPolygonI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the path. </param>
		/// <param name="path">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to draw. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="path" /> is <see langword="null" />.</exception>
		// Token: 0x06000244 RID: 580 RVA: 0x0000A4C4 File Offset: 0x000086C4
		public void DrawPath(Pen pen, GraphicsPath path)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawPath(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(path, path.nativePath));
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000245 RID: 581 RVA: 0x0000A520 File Offset: 0x00008720
		public void DrawCurve(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.PointF" /> structures using a specified tension.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the points that define the curve. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000246 RID: 582 RVA: 0x0000A598 File Offset: 0x00008798
		public void DrawCurve(Pen pen, PointF[] points, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.PointF" /> structures. The drawing begins offset from the beginning of the array.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <param name="offset">Offset from the first element in the array of the <paramref name="points" /> parameter to the starting point in the curve. </param>
		/// <param name="numberOfSegments">Number of segments after the starting point to include in the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000247 RID: 583 RVA: 0x0000A614 File Offset: 0x00008814
		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
		{
			this.DrawCurve(pen, points, offset, numberOfSegments, 0.5f);
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.PointF" /> structures using a specified tension. The drawing begins offset from the beginning of the array.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <param name="offset">Offset from the first element in the array of the <paramref name="points" /> parameter to the starting point in the curve. </param>
		/// <param name="numberOfSegments">Number of segments after the starting point to include in the curve. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000248 RID: 584 RVA: 0x0000A628 File Offset: 0x00008828
		public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurve3(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, offset, numberOfSegments, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and height of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000249 RID: 585 RVA: 0x0000A6A8 File Offset: 0x000088A8
		public void DrawCurve(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.Point" /> structures using a specified tension.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024A RID: 586 RVA: 0x0000A720 File Offset: 0x00008920
		public void DrawCurve(Pen pen, Point[] points, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a cardinal spline through a specified array of <see cref="T:System.Drawing.Point" /> structures using a specified tension.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and style of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <param name="offset">Offset from the first element in the array of the <paramref name="points" /> parameter to the starting point in the curve. </param>
		/// <param name="numberOfSegments">Number of segments after the starting point to include in the curve. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024B RID: 587 RVA: 0x0000A79C File Offset: 0x0000899C
		public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawCurve3I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, offset, numberOfSegments, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a closed cardinal spline defined by an array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and height of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024C RID: 588 RVA: 0x0000A81C File Offset: 0x00008A1C
		public void DrawClosedCurve(Pen pen, PointF[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawClosedCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a closed cardinal spline defined by an array of <see cref="T:System.Drawing.PointF" /> structures using a specified tension.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and height of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled. This parameter is required but is ignored. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024D RID: 589 RVA: 0x0000A894 File Offset: 0x00008A94
		public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawClosedCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a closed cardinal spline defined by an array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and height of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024E RID: 590 RVA: 0x0000A910 File Offset: 0x00008B10
		public void DrawClosedCurve(Pen pen, Point[] points)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawClosedCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a closed cardinal spline defined by an array of <see cref="T:System.Drawing.Point" /> structures using a specified tension.</summary>
		/// <param name="pen">
		///       <see cref="T:System.Drawing.Pen" /> that determines the color, width, and height of the curve. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled. This parameter is required but ignored. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="pen" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600024F RID: 591 RVA: 0x0000A988 File Offset: 0x00008B88
		public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
		{
			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawClosedCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(pen, pen.NativePen), new HandleRef(this, intPtr), points.Length, tension);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Clears the entire drawing surface and fills it with the specified background color.</summary>
		/// <param name="color">
		///       <see cref="T:System.Drawing.Color" /> structure that represents the background color of the drawing surface. </param>
		// Token: 0x06000250 RID: 592 RVA: 0x0000AA04 File Offset: 0x00008C04
		public void Clear(Color color)
		{
			int num = SafeNativeMethods.Gdip.GdipGraphicsClear(new HandleRef(this, this.NativeGraphics), color.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Fills the interior of a rectangle specified by a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000251 RID: 593 RVA: 0x0000AA34 File Offset: 0x00008C34
		public void FillRectangle(Brush brush, RectangleF rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill. </param>
		/// <param name="width">Width of the rectangle to fill. </param>
		/// <param name="height">Height of the rectangle to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000252 RID: 594 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public void FillRectangle(Brush brush, float x, float y, float width, float height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillRectangle(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of a rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000253 RID: 595 RVA: 0x0000AAA2 File Offset: 0x00008CA2
		public void FillRectangle(Brush brush, Rectangle rect)
		{
			this.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill. </param>
		/// <param name="width">Width of the rectangle to fill. </param>
		/// <param name="height">Height of the rectangle to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000254 RID: 596 RVA: 0x0000AAC8 File Offset: 0x00008CC8
		public void FillRectangle(Brush brush, int x, int y, int width, int height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillRectangleI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interiors of a series of rectangles specified by <see cref="T:System.Drawing.RectangleF" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rects">Array of <see cref="T:System.Drawing.RectangleF" /> structures that represent the rectangles to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="rects" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="Rects" /> is a zero-length array.</exception>
		// Token: 0x06000255 RID: 597 RVA: 0x0000AB10 File Offset: 0x00008D10
		public void FillRectangles(Brush brush, RectangleF[] rects)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillRectangles(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), rects.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interiors of a series of rectangles specified by <see cref="T:System.Drawing.Rectangle" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rects">Array of <see cref="T:System.Drawing.Rectangle" /> structures that represent the rectangles to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="rects" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="rects" /> is a zero-length array.</exception>
		// Token: 0x06000256 RID: 598 RVA: 0x0000AB88 File Offset: 0x00008D88
		public void FillRectangles(Brush brush, Rectangle[] rects)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (rects == null)
			{
				throw new ArgumentNullException("rects");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertRectangleToMemory(rects);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillRectanglesI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), rects.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the vertices of the polygon to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000257 RID: 599 RVA: 0x0000AC00 File Offset: 0x00008E00
		public void FillPolygon(Brush brush, PointF[] points)
		{
			this.FillPolygon(brush, points, FillMode.Alternate);
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by <see cref="T:System.Drawing.PointF" /> structures using the specified fill mode.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that represent the vertices of the polygon to fill. </param>
		/// <param name="fillMode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines the style of the fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000258 RID: 600 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillPolygon(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, (int)fillMode);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that represent the vertices of the polygon to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000259 RID: 601 RVA: 0x0000AC88 File Offset: 0x00008E88
		public void FillPolygon(Brush brush, Point[] points)
		{
			this.FillPolygon(brush, points, FillMode.Alternate);
		}

		/// <summary>Fills the interior of a polygon defined by an array of points specified by <see cref="T:System.Drawing.Point" /> structures using the specified fill mode.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that represent the vertices of the polygon to fill. </param>
		/// <param name="fillMode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines the style of the fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x0600025A RID: 602 RVA: 0x0000AC94 File Offset: 0x00008E94
		public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillPolygonI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, (int)fillMode);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle specified by a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that represents the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x0600025B RID: 603 RVA: 0x0000AD10 File Offset: 0x00008F10
		public void FillEllipse(Brush brush, RectangleF rect)
		{
			this.FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x0600025C RID: 604 RVA: 0x0000AD38 File Offset: 0x00008F38
		public void FillEllipse(Brush brush, float x, float y, float width, float height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillEllipse(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x0600025D RID: 605 RVA: 0x0000AD7E File Offset: 0x00008F7E
		public void FillEllipse(Brush brush, Rectangle rect)
		{
			this.FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x0600025E RID: 606 RVA: 0x0000ADA4 File Offset: 0x00008FA4
		public void FillEllipse(Brush brush, int x, int y, int width, int height)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillEllipseI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a <see cref="T:System.Drawing.RectangleF" /> structure and two radial lines.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie section. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x0600025F RID: 607 RVA: 0x0000ADEA File Offset: 0x00008FEA
		public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
		{
			this.FillPie(brush, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height, startAngle, sweepAngle);
		}

		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a pair of coordinates, a width, a height, and two radial lines.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie section. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000260 RID: 608 RVA: 0x0000AE18 File Offset: 0x00009018
		public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillPie(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height, startAngle, sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of a pie section defined by an ellipse specified by a pair of coordinates, a width, a height, and two radial lines.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie section comes. </param>
		/// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section. </param>
		/// <param name="sweepAngle">Angle in degrees measured clockwise from the <paramref name="startAngle" /> parameter to the second side of the pie section. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.</exception>
		// Token: 0x06000261 RID: 609 RVA: 0x0000AE64 File Offset: 0x00009064
		public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int status = SafeNativeMethods.Gdip.GdipFillPieI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), x, y, width, height, (float)startAngle, (float)sweepAngle);
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of a <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="path">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> that represents the path to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="path" /> is <see langword="null" />.</exception>
		// Token: 0x06000262 RID: 610 RVA: 0x0000AEB0 File Offset: 0x000090B0
		public void FillPath(Brush brush, GraphicsPath path)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int status = SafeNativeMethods.Gdip.GdipFillPath(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(path, path.nativePath));
			this.CheckErrorStatus(status);
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.PointF" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000263 RID: 611 RVA: 0x0000AF0C File Offset: 0x0000910C
		public void FillClosedCurve(Brush brush, PointF[] points)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillClosedCurve(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.PointF" /> structures using the specified fill mode.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000264 RID: 612 RVA: 0x0000AF84 File Offset: 0x00009184
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
		{
			this.FillClosedCurve(brush, points, fillmode, 0.5f);
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.PointF" /> structures using the specified fill mode and tension.</summary>
		/// <param name="brush">A <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.PointF" /> structures that define the spline. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000265 RID: 613 RVA: 0x0000AF94 File Offset: 0x00009194
		public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillClosedCurve2(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, tension, (int)fillmode);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.Point" /> structures.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000266 RID: 614 RVA: 0x0000B010 File Offset: 0x00009210
		public void FillClosedCurve(Brush brush, Point[] points)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillClosedCurveI(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.Point" /> structures using the specified fill mode.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000267 RID: 615 RVA: 0x0000B088 File Offset: 0x00009288
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
		{
			this.FillClosedCurve(brush, points, fillmode, 0.5f);
		}

		/// <summary>Fills the interior of a closed cardinal spline curve defined by an array of <see cref="T:System.Drawing.Point" /> structures using the specified fill mode and tension.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="points">Array of <see cref="T:System.Drawing.Point" /> structures that define the spline. </param>
		/// <param name="fillmode">Member of the <see cref="T:System.Drawing.Drawing2D.FillMode" /> enumeration that determines how the curve is filled. </param>
		/// <param name="tension">Value greater than or equal to 0.0F that specifies the tension of the curve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="points" /> is <see langword="null" />.</exception>
		// Token: 0x06000268 RID: 616 RVA: 0x0000B098 File Offset: 0x00009298
		public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(points);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipFillClosedCurve2I(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(this, intPtr), points.Length, tension, (int)fillmode);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Fills the interior of a <see cref="T:System.Drawing.Region" />.</summary>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the characteristics of the fill. </param>
		/// <param name="region">
		///       <see cref="T:System.Drawing.Region" /> that represents the area to fill. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="region" /> is <see langword="null" />.</exception>
		// Token: 0x06000269 RID: 617 RVA: 0x0000B114 File Offset: 0x00009314
		public void FillRegion(Brush brush, Region region)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int status = SafeNativeMethods.Gdip.GdipFillRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(brush, brush.NativeBrush), new HandleRef(region, region.nativeRegion));
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified text string at the specified location with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026A RID: 618 RVA: 0x0000B16E File Offset: 0x0000936E
		public void DrawString(string s, Font font, Brush brush, float x, float y)
		{
			this.DrawString(s, font, brush, new RectangleF(x, y, 0f, 0f), null);
		}

		/// <summary>Draws the specified text string at the specified location with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="point">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the upper-left corner of the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026B RID: 619 RVA: 0x0000B18D File Offset: 0x0000938D
		public void DrawString(string s, Font font, Brush brush, PointF point)
		{
			this.DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0f, 0f), null);
		}

		/// <summary>Draws the specified text string at the specified location with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects using the formatting attributes of the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn text. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn text. </param>
		/// <param name="format">
		///       <see cref="T:System.Drawing.StringFormat" /> that specifies formatting attributes, such as line spacing and alignment, that are applied to the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026C RID: 620 RVA: 0x0000B1B6 File Offset: 0x000093B6
		public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
		{
			this.DrawString(s, font, brush, new RectangleF(x, y, 0f, 0f), format);
		}

		/// <summary>Draws the specified text string at the specified location with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects using the formatting attributes of the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="point">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the upper-left corner of the drawn text. </param>
		/// <param name="format">
		///       <see cref="T:System.Drawing.StringFormat" /> that specifies formatting attributes, such as line spacing and alignment, that are applied to the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026D RID: 621 RVA: 0x0000B1D6 File Offset: 0x000093D6
		public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
		{
			this.DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0f, 0f), format);
		}

		/// <summary>Draws the specified text string in the specified rectangle with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="layoutRectangle">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location of the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026E RID: 622 RVA: 0x0000B200 File Offset: 0x00009400
		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
		{
			this.DrawString(s, font, brush, layoutRectangle, null);
		}

		/// <summary>Draws the specified text string in the specified rectangle with the specified <see cref="T:System.Drawing.Brush" /> and <see cref="T:System.Drawing.Font" /> objects using the formatting attributes of the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="s">String to draw. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="brush">
		///       <see cref="T:System.Drawing.Brush" /> that determines the color and texture of the drawn text. </param>
		/// <param name="layoutRectangle">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location of the drawn text. </param>
		/// <param name="format">
		///       <see cref="T:System.Drawing.StringFormat" /> that specifies formatting attributes, such as line spacing and alignment, that are applied to the drawn text. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="brush" /> is <see langword="null" />.-or-
		///         <paramref name="s" /> is <see langword="null" />.</exception>
		// Token: 0x0600026F RID: 623 RVA: 0x0000B210 File Offset: 0x00009410
		public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
		{
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			if (s == null || s.Length == 0)
			{
				return;
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(layoutRectangle);
			IntPtr handle = (format == null) ? IntPtr.Zero : format.nativeFormat;
			int status = SafeNativeMethods.Gdip.GdipDrawString(new HandleRef(this, this.NativeGraphics), s, s.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(format, handle), new HandleRef(brush, brush.NativeBrush));
			this.CheckErrorStatus(status);
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" /> and formatted with the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="layoutArea">
		///       <see cref="T:System.Drawing.SizeF" /> structure that specifies the maximum layout area for the text. </param>
		/// <param name="stringFormat">
		///       <see cref="T:System.Drawing.StringFormat" /> that represents formatting information, such as line spacing, for the string. </param>
		/// <param name="charactersFitted">Number of characters in the string. </param>
		/// <param name="linesFilled">Number of text lines in the string. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size of the string, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter and the <paramref name="stringFormat" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000270 RID: 624 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat, out int charactersFitted, out int linesFilled)
		{
			if (text == null || text.Length == 0)
			{
				charactersFitted = 0;
				linesFilled = 0;
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(0f, 0f, layoutArea.Width, layoutArea.Height);
			GPRECTF gprectf2 = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out charactersFitted, out linesFilled);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" /> and formatted with the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> defines the text format of the string. </param>
		/// <param name="origin">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the upper-left corner of the string. </param>
		/// <param name="stringFormat">
		///       <see cref="T:System.Drawing.StringFormat" /> that represents formatting information, such as line spacing, for the string. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified by the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter and the <paramref name="stringFormat" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000271 RID: 625 RVA: 0x0000B360 File Offset: 0x00009560
		public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = default(GPRECTF);
			GPRECTF gprectf2 = default(GPRECTF);
			gprectf.X = origin.X;
			gprectf.Y = origin.Y;
			gprectf.Width = 0f;
			gprectf.Height = 0f;
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" /> within the specified layout area.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> defines the text format of the string. </param>
		/// <param name="layoutArea">
		///       <see cref="T:System.Drawing.SizeF" /> structure that specifies the maximum layout area for the text. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified by the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000272 RID: 626 RVA: 0x0000B434 File Offset: 0x00009634
		public SizeF MeasureString(string text, Font font, SizeF layoutArea)
		{
			return this.MeasureString(text, font, layoutArea, null);
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" /> and formatted with the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> defines the text format of the string. </param>
		/// <param name="layoutArea">
		///       <see cref="T:System.Drawing.SizeF" /> structure that specifies the maximum layout area for the text. </param>
		/// <param name="stringFormat">
		///       <see cref="T:System.Drawing.StringFormat" /> that represents formatting information, such as line spacing, for the string. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified in the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter and the <paramref name="stringFormat" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000273 RID: 627 RVA: 0x0000B440 File Offset: 0x00009640
		public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new SizeF(0f, 0f);
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			GPRECTF gprectf = new GPRECTF(0f, 0f, layoutArea.Width, layoutArea.Height);
			GPRECTF gprectf2 = default(GPRECTF);
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipMeasureString(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), ref gprectf2, out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf2.SizeF;
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified by the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000274 RID: 628 RVA: 0x0000B4F7 File Offset: 0x000096F7
		public SizeF MeasureString(string text, Font font)
		{
			return this.MeasureString(text, font, new SizeF(0f, 0f));
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the format of the string. </param>
		/// <param name="width">Maximum width of the string in pixels. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified in the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000275 RID: 629 RVA: 0x0000B510 File Offset: 0x00009710
		public SizeF MeasureString(string text, Font font, int width)
		{
			return this.MeasureString(text, font, new SizeF((float)width, 999999f));
		}

		/// <summary>Measures the specified string when drawn with the specified <see cref="T:System.Drawing.Font" /> and formatted with the specified <see cref="T:System.Drawing.StringFormat" />.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="width">Maximum width of the string. </param>
		/// <param name="format">
		///       <see cref="T:System.Drawing.StringFormat" /> that represents formatting information, such as line spacing, for the string. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.SizeF" /> structure that represents the size, in the units specified by the <see cref="P:System.Drawing.Graphics.PageUnit" /> property, of the string specified in the <paramref name="text" /> parameter as drawn with the <paramref name="font" /> parameter and the <paramref name="stringFormat" /> parameter.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="font" /> is <see langword="null" />.</exception>
		// Token: 0x06000276 RID: 630 RVA: 0x0000B526 File Offset: 0x00009726
		public SizeF MeasureString(string text, Font font, int width, StringFormat format)
		{
			return this.MeasureString(text, font, new SizeF((float)width, 999999f), format);
		}

		/// <summary>Gets an array of <see cref="T:System.Drawing.Region" /> objects, each of which bounds a range of character positions within the specified string.</summary>
		/// <param name="text">String to measure. </param>
		/// <param name="font">
		///       <see cref="T:System.Drawing.Font" /> that defines the text format of the string. </param>
		/// <param name="layoutRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the layout rectangle for the string. </param>
		/// <param name="stringFormat">
		///       <see cref="T:System.Drawing.StringFormat" /> that represents formatting information, such as line spacing, for the string. </param>
		/// <returns>This method returns an array of <see cref="T:System.Drawing.Region" /> objects, each of which bounds a range of character positions within the specified string.</returns>
		// Token: 0x06000277 RID: 631 RVA: 0x0000B540 File Offset: 0x00009740
		public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
		{
			if (text == null || text.Length == 0)
			{
				return new Region[0];
			}
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipGetStringFormatMeasurableCharacterRangeCount(new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr[] array = new IntPtr[num2];
			GPRECTF gprectf = new GPRECTF(layoutRect);
			Region[] array2 = new Region[num2];
			for (int i = 0; i < num2; i++)
			{
				array2[i] = new Region();
				array[i] = array2[i].nativeRegion;
			}
			num = SafeNativeMethods.Gdip.GdipMeasureCharacterRanges(new HandleRef(this, this.NativeGraphics), text, text.Length, new HandleRef(font, font.NativeFont), ref gprectf, new HandleRef(stringFormat, (stringFormat == null) ? IntPtr.Zero : stringFormat.nativeFormat), num2, array);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return array2;
		}

		/// <summary>Draws the image represented by the specified <see cref="T:System.Drawing.Icon" /> at the specified coordinates.</summary>
		/// <param name="icon">
		///       <see cref="T:System.Drawing.Icon" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="icon" /> is <see langword="null" />.</exception>
		// Token: 0x06000278 RID: 632 RVA: 0x0000B625 File Offset: 0x00009825
		public void DrawIcon(Icon icon, int x, int y)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImage(icon.ToBitmap(), x, y);
				return;
			}
			icon.Draw(this, x, y);
		}

		/// <summary>Draws the image represented by the specified <see cref="T:System.Drawing.Icon" /> within the area specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="icon">
		///       <see cref="T:System.Drawing.Icon" /> to draw. </param>
		/// <param name="targetRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the resulting image on the display surface. The image contained in the <paramref name="icon" /> parameter is scaled to the dimensions of this rectangular area. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="icon" /> is <see langword="null" />.</exception>
		// Token: 0x06000279 RID: 633 RVA: 0x0000B655 File Offset: 0x00009855
		public void DrawIcon(Icon icon, Rectangle targetRect)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImage(icon.ToBitmap(), targetRect);
				return;
			}
			icon.Draw(this, targetRect);
		}

		/// <summary>Draws the image represented by the specified <see cref="T:System.Drawing.Icon" /> without scaling the image.</summary>
		/// <param name="icon">
		///       <see cref="T:System.Drawing.Icon" /> to draw. </param>
		/// <param name="targetRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the resulting image. The image is not scaled to fit this rectangle, but retains its original size. If the image is larger than the rectangle, it is clipped to fit inside it. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="icon" /> is <see langword="null" />.</exception>
		// Token: 0x0600027A RID: 634 RVA: 0x0000B683 File Offset: 0x00009883
		public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
		{
			if (icon == null)
			{
				throw new ArgumentNullException("icon");
			}
			if (this.backingImage != null)
			{
				this.DrawImageUnscaled(icon.ToBitmap(), targetRect);
				return;
			}
			icon.DrawUnstretched(this, targetRect);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" />, using its original physical size, at the specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="point">
		///       <see cref="T:System.Drawing.PointF" /> structure that represents the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600027B RID: 635 RVA: 0x0000B6B1 File Offset: 0x000098B1
		public void DrawImage(Image image, PointF point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" />, using its original physical size, at the specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600027C RID: 636 RVA: 0x0000B6C8 File Offset: 0x000098C8
		public void DrawImage(Image image, float x, float y)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImage(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600027D RID: 637 RVA: 0x0000B713 File Offset: 0x00009913
		public void DrawImage(Image image, RectangleF rect)
		{
			this.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="width">Width of the drawn image. </param>
		/// <param name="height">Height of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600027E RID: 638 RVA: 0x0000B738 File Offset: 0x00009938
		public void DrawImage(Image image, float x, float y, float width, float height)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, width, height);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" />, using its original physical size, at the specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="point">
		///       <see cref="T:System.Drawing.Point" /> structure that represents the location of the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600027F RID: 639 RVA: 0x0000B787 File Offset: 0x00009987
		public void DrawImage(Image image, Point point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		/// <summary>Draws the specified image, using its original physical size, at the location specified by a coordinate pair.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000280 RID: 640 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public void DrawImage(Image image, int x, int y)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000281 RID: 641 RVA: 0x0000B7EB File Offset: 0x000099EB
		public void DrawImage(Image image, Rectangle rect)
		{
			this.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="width">Width of the drawn image. </param>
		/// <param name="height">Height of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000282 RID: 642 RVA: 0x0000B810 File Offset: 0x00009A10
		public void DrawImage(Image image, int x, int y, int width, int height)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, width, height);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a specified image using its original physical size at a specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="point">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000283 RID: 643 RVA: 0x0000B787 File Offset: 0x00009987
		public void DrawImageUnscaled(Image image, Point point)
		{
			this.DrawImage(image, point.X, point.Y);
		}

		/// <summary>Draws the specified image using its original physical size at the location specified by a coordinate pair.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000284 RID: 644 RVA: 0x0000B85F File Offset: 0x00009A5F
		public void DrawImageUnscaled(Image image, int x, int y)
		{
			this.DrawImage(image, x, y);
		}

		/// <summary>Draws a specified image using its original physical size at a specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> that specifies the upper-left corner of the drawn image. The X and Y properties of the rectangle specify the upper-left corner. The Width and Height properties are ignored. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000285 RID: 645 RVA: 0x0000B86A File Offset: 0x00009A6A
		public void DrawImageUnscaled(Image image, Rectangle rect)
		{
			this.DrawImage(image, rect.X, rect.Y);
		}

		/// <summary>Draws a specified image using its original physical size at a specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="width">Not used. </param>
		/// <param name="height">Not used. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000286 RID: 646 RVA: 0x0000B85F File Offset: 0x00009A5F
		public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
		{
			this.DrawImage(image, x, y);
		}

		/// <summary>Draws the specified image without scaling and clips it, if necessary, to fit in the specified rectangle.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to draw.</param>
		/// <param name="rect">The <see cref="T:System.Drawing.Rectangle" /> in which to draw the image.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000287 RID: 647 RVA: 0x0000B884 File Offset: 0x00009A84
		public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int srcWidth = Math.Min(rect.Width, image.Width);
			int srcHeight = Math.Min(rect.Height, image.Height);
			this.DrawImage(image, rect, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified shape and size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000288 RID: 648 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public void DrawImage(Image image, PointF[] destPoints)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawImagePoints(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), num);
				this.IgnoreMetafileErrors(image, ref status);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified shape and size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000289 RID: 649 RVA: 0x0000B970 File Offset: 0x00009B70
		public void DrawImage(Image image, Point[] destPoints)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawImagePointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), num);
				this.IgnoreMetafileErrors(image, ref status);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws a portion of an image at a specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028A RID: 650 RVA: 0x0000BA0C File Offset: 0x00009C0C
		public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImagePointRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws a portion of an image at a specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="x">The x-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the drawn image. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028B RID: 651 RVA: 0x0000BA78 File Offset: 0x00009C78
		public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImagePointRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), x, y, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028C RID: 652 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRectRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028D RID: 653 RVA: 0x0000BB74 File Offset: 0x00009D74
		public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRectRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028E RID: 654 RVA: 0x0000BC04 File Offset: 0x00009E04
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawImagePointsRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, NativeMethods.NullHandleRef, null, NativeMethods.NullHandleRef);
				this.IgnoreMetafileErrors(image, ref status);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600028F RID: 655 RVA: 0x0000BCCC File Offset: 0x00009ECC
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)" />  method according to application-determined criteria. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000290 RID: 656 RVA: 0x0000BCDD File Offset: 0x00009EDD
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)" /> method according to application-determined criteria. </param>
		/// <param name="callbackData">Value specifying additional data for the <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate to use when checking whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.PointF[],System.Drawing.RectangleF,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)" /> method. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000291 RID: 657 RVA: 0x0000BCF0 File Offset: 0x00009EF0
		public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawImagePointsRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, new HandleRef(imageAttr, (imageAttr != null) ? imageAttr.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, (IntPtr)callbackData));
				this.IgnoreMetafileErrors(image, ref status);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000292 RID: 658 RVA: 0x0000BDD4 File Offset: 0x00009FD4
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, null, null, 0);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000293 RID: 659 RVA: 0x0000BDE4 File Offset: 0x00009FE4
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, null, 0);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)" /> method according to application-determined criteria. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000294 RID: 660 RVA: 0x0000BDF5 File Offset: 0x00009FF5
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destPoints, srcRect, srcUnit, imageAttr, callback, 0);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <paramref name="image" /> object to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used by the <paramref name="srcRect" /> parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)" /> method according to application-determined criteria. </param>
		/// <param name="callbackData">Value specifying additional data for the <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate to use when checking whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Point[],System.Drawing.Rectangle,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.Int32)" /> method. </param>
		// Token: 0x06000295 RID: 661 RVA: 0x0000BE08 File Offset: 0x0000A008
		public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback, int callbackData)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int num = destPoints.Length;
			if (num != 3 && num != 4)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidLength"));
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			try
			{
				int status = SafeNativeMethods.Gdip.GdipDrawImagePointsRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), new HandleRef(this, intPtr), destPoints.Length, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, (int)srcUnit, new HandleRef(imageAttr, (imageAttr != null) ? imageAttr.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, (IntPtr)callbackData));
				this.IgnoreMetafileErrors(image, ref status);
				this.CheckErrorStatus(status);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000296 RID: 662 RVA: 0x0000BEEC File Offset: 0x0000A0EC
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttrs">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000297 RID: 663 RVA: 0x0000BF0C File Offset: 0x0000A10C
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, null);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttrs">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Single,System.Single,System.Single,System.Single,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)" /> method according to application-determined criteria. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000298 RID: 664 RVA: 0x0000BF30 File Offset: 0x0000A130
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttrs, callback, IntPtr.Zero);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttrs">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Single,System.Single,System.Single,System.Single,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.IntPtr)" /> method according to application-determined criteria. </param>
		/// <param name="callbackData">Value specifying additional data for the <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate to use when checking whether to stop execution of the <see langword="DrawImage" /> method. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000299 RID: 665 RVA: 0x0000BF58 File Offset: 0x0000A158
		public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRectRect(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), (float)destRect.X, (float)destRect.Y, (float)destRect.Width, (float)destRect.Height, srcX, srcY, srcWidth, srcHeight, (int)srcUnit, new HandleRef(imageAttrs, (imageAttrs != null) ? imageAttrs.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, callbackData));
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600029A RID: 666 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, null);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600029B RID: 667 RVA: 0x0000C010 File Offset: 0x0000A210
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, null);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for <paramref name="image" />. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Int32,System.Int32,System.Int32,System.Int32,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort)" /> method according to application-determined criteria. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600029C RID: 668 RVA: 0x0000C034 File Offset: 0x0000A234
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, Graphics.DrawImageAbort callback)
		{
			this.DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit, imageAttr, callback, IntPtr.Zero);
		}

		/// <summary>Draws the specified portion of the specified <see cref="T:System.Drawing.Image" /> at the specified location and with the specified size.</summary>
		/// <param name="image">
		///       <see cref="T:System.Drawing.Image" /> to draw. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn image. The image is scaled to fit the rectangle. </param>
		/// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw. </param>
		/// <param name="srcWidth">Width of the portion of the source image to draw. </param>
		/// <param name="srcHeight">Height of the portion of the source image to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the units of measure used to determine the source rectangle. </param>
		/// <param name="imageAttrs">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies recoloring and gamma information for the <paramref name="image" /> object. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate that specifies a method to call during the drawing of the image. This method is called frequently to check whether to stop execution of the <see cref="M:System.Drawing.Graphics.DrawImage(System.Drawing.Image,System.Drawing.Rectangle,System.Int32,System.Int32,System.Int32,System.Int32,System.Drawing.GraphicsUnit,System.Drawing.Imaging.ImageAttributes,System.Drawing.Graphics.DrawImageAbort,System.IntPtr)" /> method according to application-determined criteria. </param>
		/// <param name="callbackData">Value specifying additional data for the <see cref="T:System.Drawing.Graphics.DrawImageAbort" /> delegate to use when checking whether to stop execution of the <see langword="DrawImage" /> method. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x0600029D RID: 669 RVA: 0x0000C05C File Offset: 0x0000A25C
		public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, Graphics.DrawImageAbort callback, IntPtr callbackData)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			int status = SafeNativeMethods.Gdip.GdipDrawImageRectRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(image, image.nativeImage), destRect.X, destRect.Y, destRect.Width, destRect.Height, srcX, srcY, srcWidth, srcHeight, (int)srcUnit, new HandleRef(imageAttrs, (imageAttrs != null) ? imageAttrs.nativeImageAttributes : IntPtr.Zero), callback, new HandleRef(null, callbackData));
			this.IgnoreMetafileErrors(image, ref status);
			this.CheckErrorStatus(status);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x0600029E RID: 670 RVA: 0x0000C0ED File Offset: 0x0000A2ED
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x0600029F RID: 671 RVA: 0x0000C0FD File Offset: 0x0000A2FD
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, callbackData, null);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002A0 RID: 672 RVA: 0x0000C10C File Offset: 0x0000A30C
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPoint(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), new GPPOINTF(destPoint), callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002A1 RID: 673 RVA: 0x0000C177 File Offset: 0x0000A377
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002A2 RID: 674 RVA: 0x0000C187 File Offset: 0x0000A387
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, callback, callbackData, null);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002A3 RID: 675 RVA: 0x0000C198 File Offset: 0x0000A398
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPointI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), new GPPOINT(destPoint), callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002A4 RID: 676 RVA: 0x0000C203 File Offset: 0x0000A403
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002A5 RID: 677 RVA: 0x0000C213 File Offset: 0x0000A413
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, callback, callbackData, null);
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002A6 RID: 678 RVA: 0x0000C224 File Offset: 0x0000A424
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPRECTF gprectf = new GPRECTF(destRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestRect(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), ref gprectf, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002A7 RID: 679 RVA: 0x0000C293 File Offset: 0x0000A493
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002A8 RID: 680 RVA: 0x0000C2A3 File Offset: 0x0000A4A3
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, callback, callbackData, null);
		}

		/// <summary>Sends the records of the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002A9 RID: 681 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPRECT gprect = new GPRECT(destRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), ref gprect, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002AA RID: 682 RVA: 0x0000C323 File Offset: 0x0000A523
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002AB RID: 683 RVA: 0x0000C333 File Offset: 0x0000A533
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero, null);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002AC RID: 684 RVA: 0x0000C344 File Offset: 0x0000A544
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPoints(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), intPtr, destPoints.Length, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002AD RID: 685 RVA: 0x0000C3DE File Offset: 0x0000A5DE
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002AE RID: 686 RVA: 0x0000C3EE File Offset: 0x0000A5EE
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, callback, callbackData, null);
		}

		/// <summary>Sends the records in the specified <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002AF RID: 687 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileDestPointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), intPtr, destPoints.Length, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002B0 RID: 688 RVA: 0x0000C496 File Offset: 0x0000A696
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002B1 RID: 689 RVA: 0x0000C4AA File Offset: 0x0000A6AA
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.PointF" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002B2 RID: 690 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPRECTF gprectf = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPoint(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), new GPPOINTF(destPoint), ref gprectf, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002B3 RID: 691 RVA: 0x0000C534 File Offset: 0x0000A734
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002B4 RID: 692 RVA: 0x0000C548 File Offset: 0x0000A748
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoint, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display at a specified point using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoint">
		///       <see cref="T:System.Drawing.Point" /> structure that specifies the location of the upper-left corner of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002B5 RID: 693 RVA: 0x0000C55C File Offset: 0x0000A75C
		public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPPOINT destPoint2 = new GPPOINT(destPoint);
			GPRECT gprect = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPointI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), destPoint2, ref gprect, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002B6 RID: 694 RVA: 0x0000C5D9 File Offset: 0x0000A7D9
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002B7 RID: 695 RVA: 0x0000C5ED File Offset: 0x0000A7ED
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002B8 RID: 696 RVA: 0x0000C600 File Offset: 0x0000A800
		public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPRECTF gprectf = new GPRECTF(destRect);
			GPRECTF gprectf2 = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestRect(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), ref gprectf, ref gprectf2, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002B9 RID: 697 RVA: 0x0000C67F File Offset: 0x0000A87F
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002BA RID: 698 RVA: 0x0000C693 File Offset: 0x0000A893
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destRect, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records of a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified rectangle using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the location and size of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002BB RID: 699 RVA: 0x0000C6A8 File Offset: 0x0000A8A8
		public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			GPRECT gprect = new GPRECT(destRect);
			GPRECT gprect2 = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestRectI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), ref gprect, ref gprect2, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structures that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002BC RID: 700 RVA: 0x0000C727 File Offset: 0x0000A927
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002BD RID: 701 RVA: 0x0000C73B File Offset: 0x0000A93B
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.PointF" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002BE RID: 702 RVA: 0x0000C750 File Offset: 0x0000A950
		public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			GPRECTF gprectf = new GPRECTF(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPoints(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), intPtr, destPoints.Length, ref gprectf, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		// Token: 0x060002BF RID: 703 RVA: 0x0000C7FA File Offset: 0x0000A9FA
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, IntPtr.Zero);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="srcUnit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		// Token: 0x060002C0 RID: 704 RVA: 0x0000C80E File Offset: 0x0000AA0E
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData)
		{
			this.EnumerateMetafile(metafile, destPoints, srcRect, srcUnit, callback, callbackData, null);
		}

		/// <summary>Sends the records in a selected rectangle from a <see cref="T:System.Drawing.Imaging.Metafile" />, one at a time, to a callback method for display in a specified parallelogram using specified image attributes.</summary>
		/// <param name="metafile">
		///       <see cref="T:System.Drawing.Imaging.Metafile" /> to enumerate. </param>
		/// <param name="destPoints">Array of three <see cref="T:System.Drawing.Point" /> structures that define a parallelogram that determines the size and location of the drawn metafile. </param>
		/// <param name="srcRect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the metafile, relative to its upper-left corner, to draw. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure used to determine the portion of the metafile that the rectangle specified by the <paramref name="srcRect" /> parameter contains. </param>
		/// <param name="callback">
		///       <see cref="T:System.Drawing.Graphics.EnumerateMetafileProc" /> delegate that specifies the method to which the metafile records are sent. </param>
		/// <param name="callbackData">Internal pointer that is required, but ignored. You can pass <see cref="F:System.IntPtr.Zero" /> for this parameter. </param>
		/// <param name="imageAttr">
		///       <see cref="T:System.Drawing.Imaging.ImageAttributes" /> that specifies image attribute information for the drawn image. </param>
		// Token: 0x060002C1 RID: 705 RVA: 0x0000C820 File Offset: 0x0000AA20
		public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, Graphics.EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
		{
			if (destPoints == null)
			{
				throw new ArgumentNullException("destPoints");
			}
			if (destPoints.Length != 3)
			{
				throw new ArgumentException(SR.GetString("GdiplusDestPointsInvalidParallelogram"));
			}
			IntPtr handle = (metafile == null) ? IntPtr.Zero : metafile.nativeImage;
			IntPtr handle2 = (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes;
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(destPoints);
			GPRECT gprect = new GPRECT(srcRect);
			int num = SafeNativeMethods.Gdip.GdipEnumerateMetafileSrcRectDestPointsI(new HandleRef(this, this.NativeGraphics), new HandleRef(metafile, handle), intPtr, destPoints.Length, ref gprect, (int)unit, callback, new HandleRef(null, callbackData), new HandleRef(imageAttr, handle2));
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the <see langword="Clip" /> property of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="g">
		///       <see cref="T:System.Drawing.Graphics" /> from which to take the new clip region. </param>
		// Token: 0x060002C2 RID: 706 RVA: 0x0000C8CA File Offset: 0x0000AACA
		public void SetClip(Graphics g)
		{
			this.SetClip(g, CombineMode.Replace);
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the result of the specified combining operation of the current clip region and the <see cref="P:System.Drawing.Graphics.Clip" /> property of the specified <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="g">
		///       <see cref="T:System.Drawing.Graphics" /> that specifies the clip region to combine. </param>
		/// <param name="combineMode">Member of the <see cref="T:System.Drawing.Drawing2D.CombineMode" /> enumeration that specifies the combining operation to use. </param>
		// Token: 0x060002C3 RID: 707 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		public void SetClip(Graphics g, CombineMode combineMode)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipGraphics(new HandleRef(this, this.NativeGraphics), new HandleRef(g, g.NativeGraphics), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that represents the new clip region. </param>
		// Token: 0x060002C4 RID: 708 RVA: 0x0000C918 File Offset: 0x0000AB18
		public void SetClip(Rectangle rect)
		{
			this.SetClip(rect, CombineMode.Replace);
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the result of the specified operation combining the current clip region and the rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure to combine. </param>
		/// <param name="combineMode">Member of the <see cref="T:System.Drawing.Drawing2D.CombineMode" /> enumeration that specifies the combining operation to use. </param>
		// Token: 0x060002C5 RID: 709 RVA: 0x0000C924 File Offset: 0x0000AB24
		public void SetClip(Rectangle rect, CombineMode combineMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the rectangle specified by a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that represents the new clip region. </param>
		// Token: 0x060002C6 RID: 710 RVA: 0x0000C96A File Offset: 0x0000AB6A
		public void SetClip(RectangleF rect)
		{
			this.SetClip(rect, CombineMode.Replace);
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the result of the specified operation combining the current clip region and the rectangle specified by a <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure to combine. </param>
		/// <param name="combineMode">Member of the <see cref="T:System.Drawing.Drawing2D.CombineMode" /> enumeration that specifies the combining operation to use. </param>
		// Token: 0x060002C7 RID: 711 RVA: 0x0000C974 File Offset: 0x0000AB74
		public void SetClip(RectangleF rect, CombineMode combineMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the specified <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="path">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> that represents the new clip region. </param>
		// Token: 0x060002C8 RID: 712 RVA: 0x0000C9BA File Offset: 0x0000ABBA
		public void SetClip(GraphicsPath path)
		{
			this.SetClip(path, CombineMode.Replace);
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the result of the specified operation combining the current clip region and the specified <see cref="T:System.Drawing.Drawing2D.GraphicsPath" />.</summary>
		/// <param name="path">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsPath" /> to combine. </param>
		/// <param name="combineMode">Member of the <see cref="T:System.Drawing.Drawing2D.CombineMode" /> enumeration that specifies the combining operation to use. </param>
		// Token: 0x060002C9 RID: 713 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
		public void SetClip(GraphicsPath path, CombineMode combineMode)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipPath(new HandleRef(this, this.NativeGraphics), new HandleRef(path, path.nativePath), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the clipping region of this <see cref="T:System.Drawing.Graphics" /> to the result of the specified operation combining the current clip region and the specified <see cref="T:System.Drawing.Region" />.</summary>
		/// <param name="region">
		///       <see cref="T:System.Drawing.Region" /> to combine. </param>
		/// <param name="combineMode">Member from the <see cref="T:System.Drawing.Drawing2D.CombineMode" /> enumeration that specifies the combining operation to use. </param>
		// Token: 0x060002CA RID: 714 RVA: 0x0000CA08 File Offset: 0x0000AC08
		public void SetClip(Region region, CombineMode combineMode)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), combineMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Updates the clip region of this <see cref="T:System.Drawing.Graphics" /> to the intersection of the current clip region and the specified <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure to intersect with the current clip region. </param>
		// Token: 0x060002CB RID: 715 RVA: 0x0000CA4C File Offset: 0x0000AC4C
		public void IntersectClip(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Updates the clip region of this <see cref="T:System.Drawing.Graphics" /> to the intersection of the current clip region and the specified <see cref="T:System.Drawing.RectangleF" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure to intersect with the current clip region. </param>
		// Token: 0x060002CC RID: 716 RVA: 0x0000CA94 File Offset: 0x0000AC94
		public void IntersectClip(RectangleF rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Updates the clip region of this <see cref="T:System.Drawing.Graphics" /> to the intersection of the current clip region and the specified <see cref="T:System.Drawing.Region" />.</summary>
		/// <param name="region">
		///       <see cref="T:System.Drawing.Region" /> to intersect with the current region. </param>
		// Token: 0x060002CD RID: 717 RVA: 0x0000CADC File Offset: 0x0000ACDC
		public void IntersectClip(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), CombineMode.Intersect);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Updates the clip region of this <see cref="T:System.Drawing.Graphics" /> to exclude the area specified by a <see cref="T:System.Drawing.Rectangle" /> structure.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that specifies the rectangle to exclude from the clip region. </param>
		// Token: 0x060002CE RID: 718 RVA: 0x0000CB20 File Offset: 0x0000AD20
		public void ExcludeClip(Rectangle rect)
		{
			int num = SafeNativeMethods.Gdip.GdipSetClipRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Updates the clip region of this <see cref="T:System.Drawing.Graphics" /> to exclude the area specified by a <see cref="T:System.Drawing.Region" />.</summary>
		/// <param name="region">
		///       <see cref="T:System.Drawing.Region" /> that specifies the region to exclude from the clip region. </param>
		// Token: 0x060002CF RID: 719 RVA: 0x0000CB68 File Offset: 0x0000AD68
		public void ExcludeClip(Region region)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			int num = SafeNativeMethods.Gdip.GdipSetClipRegion(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion), CombineMode.Exclude);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Resets the clip region of this <see cref="T:System.Drawing.Graphics" /> to an infinite region.</summary>
		// Token: 0x060002D0 RID: 720 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public void ResetClip()
		{
			int num = SafeNativeMethods.Gdip.GdipResetClip(new HandleRef(this, this.NativeGraphics));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Translates the clipping region of this <see cref="T:System.Drawing.Graphics" /> by specified amounts in the horizontal and vertical directions.</summary>
		/// <param name="dx">The x-coordinate of the translation. </param>
		/// <param name="dy">The y-coordinate of the translation. </param>
		// Token: 0x060002D1 RID: 721 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		public void TranslateClip(float dx, float dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateClip(new HandleRef(this, this.NativeGraphics), dx, dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Translates the clipping region of this <see cref="T:System.Drawing.Graphics" /> by specified amounts in the horizontal and vertical directions.</summary>
		/// <param name="dx">The x-coordinate of the translation. </param>
		/// <param name="dy">The y-coordinate of the translation. </param>
		// Token: 0x060002D2 RID: 722 RVA: 0x0000CC04 File Offset: 0x0000AE04
		public void TranslateClip(int dx, int dy)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateClip(new HandleRef(this, this.NativeGraphics), (float)dx, (float)dy);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the cumulative graphics context.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the cumulative graphics context.</returns>
		// Token: 0x060002D3 RID: 723 RVA: 0x0000CC34 File Offset: 0x0000AE34
		[EditorBrowsable(EditorBrowsableState.Never)]
		[StrongNameIdentityPermission(SecurityAction.LinkDemand, Name = "System.Windows.Forms", PublicKey = "0x00000000000000000400000000000000")]
		public object GetContextInfo()
		{
			Region clip = this.Clip;
			Matrix transform = this.Transform;
			PointF pointF = PointF.Empty;
			PointF empty = PointF.Empty;
			if (!transform.IsIdentity)
			{
				float[] elements = transform.Elements;
				pointF.X = elements[4];
				pointF.Y = elements[5];
			}
			GraphicsContext previous = this.previousContext;
			while (previous != null)
			{
				if (!previous.TransformOffset.IsEmpty)
				{
					transform.Translate(previous.TransformOffset.X, previous.TransformOffset.Y);
				}
				if (!pointF.IsEmpty)
				{
					clip.Translate(pointF.X, pointF.Y);
					empty.X += pointF.X;
					empty.Y += pointF.Y;
				}
				if (previous.Clip != null)
				{
					clip.Intersect(previous.Clip);
				}
				pointF = previous.TransformOffset;
				do
				{
					previous = previous.Previous;
				}
				while (previous != null && previous.Next.IsCumulative && previous.IsCumulative);
			}
			if (!empty.IsEmpty)
			{
				clip.Translate(-empty.X, -empty.Y);
			}
			return new object[]
			{
				clip,
				transform
			};
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Region" /> that limits the drawing region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Region" /> that limits the portion of this <see cref="T:System.Drawing.Graphics" /> that is currently available for drawing.</returns>
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000CD84 File Offset: 0x0000AF84
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000CDC0 File Offset: 0x0000AFC0
		public Region Clip
		{
			get
			{
				Region region = new Region();
				int num = SafeNativeMethods.Gdip.GdipGetClip(new HandleRef(this, this.NativeGraphics), new HandleRef(region, region.nativeRegion));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return region;
			}
			set
			{
				this.SetClip(value, CombineMode.Replace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.RectangleF" /> structure that bounds the clipping region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> structure that represents a bounding rectangle for the clipping region of this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000CDCC File Offset: 0x0000AFCC
		public RectangleF ClipBounds
		{
			get
			{
				GPRECTF gprectf = default(GPRECTF);
				int num = SafeNativeMethods.Gdip.GdipGetClipBounds(new HandleRef(this, this.NativeGraphics), ref gprectf);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return gprectf.ToRectangleF();
			}
		}

		/// <summary>Gets a value indicating whether the clipping region of this <see cref="T:System.Drawing.Graphics" /> is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the clipping region of this <see cref="T:System.Drawing.Graphics" /> is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000CE08 File Offset: 0x0000B008
		public bool IsClipEmpty
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsClipEmpty(new HandleRef(this, this.NativeGraphics), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		/// <summary>Gets the bounding rectangle of the visible clipping region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> structure that represents a bounding rectangle for the visible clipping region of this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0000CE38 File Offset: 0x0000B038
		public RectangleF VisibleClipBounds
		{
			get
			{
				if (this.PrintingHelper != null)
				{
					PrintPreviewGraphics printPreviewGraphics = this.PrintingHelper as PrintPreviewGraphics;
					if (printPreviewGraphics != null)
					{
						return printPreviewGraphics.VisibleClipBounds;
					}
				}
				GPRECTF gprectf = default(GPRECTF);
				int num = SafeNativeMethods.Gdip.GdipGetVisibleClipBounds(new HandleRef(this, this.NativeGraphics), ref gprectf);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return gprectf.ToRectangleF();
			}
		}

		/// <summary>Gets a value indicating whether the visible clipping region of this <see cref="T:System.Drawing.Graphics" /> is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the visible portion of the clipping region of this <see cref="T:System.Drawing.Graphics" /> is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x0000CE90 File Offset: 0x0000B090
		public bool IsVisibleClipEmpty
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsVisibleClipEmpty(new HandleRef(this, this.NativeGraphics), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		/// <summary>Indicates whether the point specified by a pair of coordinates is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test for visibility. </param>
		/// <param name="y">The y-coordinate of the point to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the point defined by the <paramref name="x" /> and <paramref name="y" /> parameters is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DA RID: 730 RVA: 0x0000CEBF File Offset: 0x0000B0BF
		public bool IsVisible(int x, int y)
		{
			return this.IsVisible(new Point(x, y));
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Drawing.Point" /> structure is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="point">
		///       <see cref="T:System.Drawing.Point" /> structure to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the point specified by the <paramref name="point" /> parameter is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DB RID: 731 RVA: 0x0000CED0 File Offset: 0x0000B0D0
		public bool IsVisible(Point point)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePointI(new HandleRef(this, this.NativeGraphics), point.X, point.Y, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the point specified by a pair of coordinates is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the point to test for visibility. </param>
		/// <param name="y">The y-coordinate of the point to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the point defined by the <paramref name="x" /> and <paramref name="y" /> parameters is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DC RID: 732 RVA: 0x0000CF0D File Offset: 0x0000B10D
		public bool IsVisible(float x, float y)
		{
			return this.IsVisible(new PointF(x, y));
		}

		/// <summary>Indicates whether the specified <see cref="T:System.Drawing.PointF" /> structure is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="point">
		///       <see cref="T:System.Drawing.PointF" /> structure to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the point specified by the <paramref name="point" /> parameter is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DD RID: 733 RVA: 0x0000CF1C File Offset: 0x0000B11C
		public bool IsVisible(PointF point)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisiblePoint(new HandleRef(this, this.NativeGraphics), point.X, point.Y, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the rectangle specified by a pair of coordinates, a width, and a height is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to test for visibility. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to test for visibility.</param>
		/// <param name="width">Width of the rectangle to test for visibility. </param>
		/// <param name="height">Height of the rectangle to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the rectangle defined by the <paramref name="x" />, <paramref name="y" />, <paramref name="width" />, and <paramref name="height" /> parameters is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DE RID: 734 RVA: 0x0000CF59 File Offset: 0x0000B159
		public bool IsVisible(int x, int y, int width, int height)
		{
			return this.IsVisible(new Rectangle(x, y, width, height));
		}

		/// <summary>Indicates whether the rectangle specified by a <see cref="T:System.Drawing.Rectangle" /> structure is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the rectangle specified by the <paramref name="rect" /> parameter is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002DF RID: 735 RVA: 0x0000CF6C File Offset: 0x0000B16C
		public bool IsVisible(Rectangle rect)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisibleRectI(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Indicates whether the rectangle specified by a pair of coordinates, a width, and a height is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to test for visibility. </param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to test for visibility. </param>
		/// <param name="width">Width of the rectangle to test for visibility. </param>
		/// <param name="height">Height of the rectangle to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the rectangle defined by the <paramref name="x" />, <paramref name="y" />, <paramref name="width" />, and <paramref name="height" /> parameters is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002E0 RID: 736 RVA: 0x0000CFB7 File Offset: 0x0000B1B7
		public bool IsVisible(float x, float y, float width, float height)
		{
			return this.IsVisible(new RectangleF(x, y, width, height));
		}

		/// <summary>Indicates whether the rectangle specified by a <see cref="T:System.Drawing.RectangleF" /> structure is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />.</summary>
		/// <param name="rect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure to test for visibility. </param>
		/// <returns>
		///     <see langword="true" /> if the rectangle specified by the <paramref name="rect" /> parameter is contained within the visible clip region of this <see cref="T:System.Drawing.Graphics" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060002E1 RID: 737 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		public bool IsVisible(RectangleF rect)
		{
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsVisibleRect(new HandleRef(this, this.NativeGraphics), rect.X, rect.Y, rect.Width, rect.Height, out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000D017 File Offset: 0x0000B217
		private void PushContext(GraphicsContext context)
		{
			if (this.previousContext != null)
			{
				context.Previous = this.previousContext;
				this.previousContext.Next = context;
			}
			this.previousContext = context;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000D040 File Offset: 0x0000B240
		private void PopContext(int currentContextState)
		{
			for (GraphicsContext previous = this.previousContext; previous != null; previous = previous.Previous)
			{
				if (previous.State == currentContextState)
				{
					this.previousContext = previous.Previous;
					previous.Dispose();
					return;
				}
			}
		}

		/// <summary>Saves the current state of this <see cref="T:System.Drawing.Graphics" /> and identifies the saved state with a <see cref="T:System.Drawing.Drawing2D.GraphicsState" />.</summary>
		/// <returns>This method returns a <see cref="T:System.Drawing.Drawing2D.GraphicsState" /> that represents the saved state of this <see cref="T:System.Drawing.Graphics" />.</returns>
		// Token: 0x060002E4 RID: 740 RVA: 0x0000D07C File Offset: 0x0000B27C
		public GraphicsState Save()
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipSaveGraphics(new HandleRef(this, this.NativeGraphics), out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			graphicsContext.IsCumulative = true;
			this.PushContext(graphicsContext);
			return new GraphicsState(num);
		}

		/// <summary>Restores the state of this <see cref="T:System.Drawing.Graphics" /> to the state represented by a <see cref="T:System.Drawing.Drawing2D.GraphicsState" />.</summary>
		/// <param name="gstate">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsState" /> that represents the state to which to restore this <see cref="T:System.Drawing.Graphics" />. </param>
		// Token: 0x060002E5 RID: 741 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		public void Restore(GraphicsState gstate)
		{
			int num = SafeNativeMethods.Gdip.GdipRestoreGraphics(new HandleRef(this, this.NativeGraphics), gstate.nativeState);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.PopContext(gstate.nativeState);
		}

		/// <summary>Saves a graphics container with the current state of this <see cref="T:System.Drawing.Graphics" /> and opens and uses a new graphics container with the specified scale transformation.</summary>
		/// <param name="dstrect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that, together with the <paramref name="srcrect" /> parameter, specifies a scale transformation for the new graphics container. </param>
		/// <param name="srcrect">
		///       <see cref="T:System.Drawing.RectangleF" /> structure that, together with the <paramref name="dstrect" /> parameter, specifies a scale transformation for the new graphics container. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure for the container. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.Drawing2D.GraphicsContainer" /> that represents the state of this <see cref="T:System.Drawing.Graphics" /> at the time of the method call.</returns>
		// Token: 0x060002E6 RID: 742 RVA: 0x0000D110 File Offset: 0x0000B310
		public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			GPRECTF gprectf = dstrect.ToGPRECTF();
			GPRECTF gprectf2 = srcrect.ToGPRECTF();
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainer(new HandleRef(this, this.NativeGraphics), ref gprectf, ref gprectf2, (int)unit, out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		/// <summary>Saves a graphics container with the current state of this <see cref="T:System.Drawing.Graphics" /> and opens and uses a new graphics container.</summary>
		/// <returns>This method returns a <see cref="T:System.Drawing.Drawing2D.GraphicsContainer" /> that represents the state of this <see cref="T:System.Drawing.Graphics" /> at the time of the method call.</returns>
		// Token: 0x060002E7 RID: 743 RVA: 0x0000D178 File Offset: 0x0000B378
		public GraphicsContainer BeginContainer()
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainer2(new HandleRef(this, this.NativeGraphics), out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		/// <summary>Closes the current graphics container and restores the state of this <see cref="T:System.Drawing.Graphics" /> to the state saved by a call to the <see cref="M:System.Drawing.Graphics.BeginContainer" /> method.</summary>
		/// <param name="container">
		///       <see cref="T:System.Drawing.Drawing2D.GraphicsContainer" /> that represents the container this method restores. </param>
		// Token: 0x060002E8 RID: 744 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		public void EndContainer(GraphicsContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			int num = SafeNativeMethods.Gdip.GdipEndContainer(new HandleRef(this, this.NativeGraphics), container.nativeGraphicsContainer);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.PopContext(container.nativeGraphicsContainer);
		}

		/// <summary>Saves a graphics container with the current state of this <see cref="T:System.Drawing.Graphics" /> and opens and uses a new graphics container with the specified scale transformation.</summary>
		/// <param name="dstrect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that, together with the <paramref name="srcrect" /> parameter, specifies a scale transformation for the container. </param>
		/// <param name="srcrect">
		///       <see cref="T:System.Drawing.Rectangle" /> structure that, together with the <paramref name="dstrect" /> parameter, specifies a scale transformation for the container. </param>
		/// <param name="unit">Member of the <see cref="T:System.Drawing.GraphicsUnit" /> enumeration that specifies the unit of measure for the container. </param>
		/// <returns>This method returns a <see cref="T:System.Drawing.Drawing2D.GraphicsContainer" /> that represents the state of this <see cref="T:System.Drawing.Graphics" /> at the time of the method call.</returns>
		// Token: 0x060002E9 RID: 745 RVA: 0x0000D214 File Offset: 0x0000B414
		public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
		{
			GraphicsContext graphicsContext = new GraphicsContext(this);
			int num = 0;
			GPRECT gprect = new GPRECT(dstrect);
			GPRECT gprect2 = new GPRECT(srcrect);
			int num2 = SafeNativeMethods.Gdip.GdipBeginContainerI(new HandleRef(this, this.NativeGraphics), ref gprect, ref gprect2, (int)unit, out num);
			if (num2 != 0)
			{
				graphicsContext.Dispose();
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			graphicsContext.State = num;
			this.PushContext(graphicsContext);
			return new GraphicsContainer(num);
		}

		/// <summary>Adds a comment to the current <see cref="T:System.Drawing.Imaging.Metafile" />.</summary>
		/// <param name="data">Array of bytes that contains the comment. </param>
		// Token: 0x060002EA RID: 746 RVA: 0x0000D27C File Offset: 0x0000B47C
		public void AddMetafileComment(byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			int num = SafeNativeMethods.Gdip.GdipComment(new HandleRef(this, this.NativeGraphics), data.Length, data);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets a handle to the current Windows halftone palette.</summary>
		/// <returns>Internal pointer that specifies the handle to the palette.</returns>
		// Token: 0x060002EB RID: 747 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
		public static IntPtr GetHalftonePalette()
		{
			if (Graphics.halftonePalette == IntPtr.Zero)
			{
				object obj = Graphics.syncObject;
				lock (obj)
				{
					if (Graphics.halftonePalette == IntPtr.Zero)
					{
						if (Environment.OSVersion.Platform != PlatformID.Win32Windows)
						{
							AppDomain.CurrentDomain.DomainUnload += Graphics.OnDomainUnload;
						}
						AppDomain.CurrentDomain.ProcessExit += Graphics.OnDomainUnload;
						Graphics.halftonePalette = SafeNativeMethods.Gdip.GdipCreateHalftonePalette();
					}
				}
			}
			return Graphics.halftonePalette;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D35C File Offset: 0x0000B55C
		[PrePrepareMethod]
		private static void OnDomainUnload(object sender, EventArgs e)
		{
			if (Graphics.halftonePalette != IntPtr.Zero)
			{
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, Graphics.halftonePalette));
				Graphics.halftonePalette = IntPtr.Zero;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D38C File Offset: 0x0000B58C
		private void CheckErrorStatus(int status)
		{
			if (status != 0)
			{
				if (status == 1 || status == 7)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					if (lastWin32Error == 5 || lastWin32Error == 127 || ((UnsafeNativeMethods.GetSystemMetrics(4096) & 1) != 0 && lastWin32Error == 0))
					{
						return;
					}
				}
				throw SafeNativeMethods.Gdip.StatusException(status);
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D3CC File Offset: 0x0000B5CC
		private void IgnoreMetafileErrors(Image image, ref int errorStatus)
		{
			if (errorStatus != 0 && image.RawFormat.Equals(ImageFormat.Emf))
			{
				errorStatus = 0;
			}
		}

		// Token: 0x0400017C RID: 380
		private GraphicsContext previousContext;

		// Token: 0x0400017D RID: 381
		private static readonly object syncObject = new object();

		// Token: 0x0400017E RID: 382
		private IntPtr nativeGraphics;

		// Token: 0x0400017F RID: 383
		private IntPtr nativeHdc;

		// Token: 0x04000180 RID: 384
		private object printingHelper;

		// Token: 0x04000181 RID: 385
		private static IntPtr halftonePalette;

		// Token: 0x04000182 RID: 386
		private Image backingImage;

		/// <summary>Provides a callback method for deciding when the <see cref="Overload:System.Drawing.Graphics.DrawImage" /> method should prematurely cancel execution and stop drawing an image.</summary>
		/// <param name="callbackdata">Internal pointer that specifies data for the callback method. This parameter is not passed by all <see cref="Overload:System.Drawing.Graphics.DrawImage" /> overloads. You can test for its absence by checking for the value <see cref="F:System.IntPtr.Zero" />. </param>
		/// <returns>This method returns <see langword="true" /> if it decides that the <see cref="Overload:System.Drawing.Graphics.DrawImage" /> method should prematurely stop execution. Otherwise it returns <see langword="false" /> to indicate that the <see cref="Overload:System.Drawing.Graphics.DrawImage" /> method should continue execution.</returns>
		// Token: 0x020000F8 RID: 248
		// (Invoke) Token: 0x06000CA0 RID: 3232
		public delegate bool DrawImageAbort(IntPtr callbackdata);

		/// <summary>Provides a callback method for the <see cref="Overload:System.Drawing.Graphics.EnumerateMetafile" /> method.</summary>
		/// <param name="recordType">Member of the <see cref="T:System.Drawing.Imaging.EmfPlusRecordType" /> enumeration that specifies the type of metafile record. </param>
		/// <param name="flags">Set of flags that specify attributes of the record. </param>
		/// <param name="dataSize">Number of bytes in the record data. </param>
		/// <param name="data">Pointer to a buffer that contains the record data. </param>
		/// <param name="callbackData">Not used. </param>
		/// <returns>Return <see langword="true" /> if you want to continue enumerating records; otherwise, <see langword="false" />.</returns>
		// Token: 0x020000F9 RID: 249
		// (Invoke) Token: 0x06000CA4 RID: 3236
		public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType, int flags, int dataSize, IntPtr data, PlayRecordCallback callbackData);
	}
}
