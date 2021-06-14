using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Drawing
{
	/// <summary>Encapsulates a GDI+ bitmap, which consists of the pixel data for a graphics image and its attributes. A <see cref="T:System.Drawing.Bitmap" /> is an object used to work with images defined by pixel data.</summary>
	// Token: 0x0200000E RID: 14
	[Editor("System.Drawing.Design.BitmapEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ComVisible(true)]
	[Serializable]
	public sealed class Bitmap : Image
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified file.</summary>
		/// <param name="filename">The bitmap file name and path. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified file is not found.</exception>
		// Token: 0x0600002D RID: 45 RVA: 0x00002A98 File Offset: 0x00000C98
		public Bitmap(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFile(filename, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, filename, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified file.</summary>
		/// <param name="filename">The name of the bitmap file. </param>
		/// <param name="useIcm">
		///       <see langword="true" /> to use color correction for this <see cref="T:System.Drawing.Bitmap" />; otherwise, <see langword="false" />. </param>
		// Token: 0x0600002E RID: 46 RVA: 0x00002B08 File Offset: 0x00000D08
		public Bitmap(string filename, bool useIcm)
		{
			IntSecurity.DemandReadFileIO(filename);
			filename = Path.GetFullPath(filename);
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useIcm)
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFileICM(filename, out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromFile(filename, out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, filename, null);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from a specified resource.</summary>
		/// <param name="type">The class used to extract the resource. </param>
		/// <param name="resource">The name of the resource. </param>
		// Token: 0x0600002F RID: 47 RVA: 0x00002B84 File Offset: 0x00000D84
		public Bitmap(Type type, string resource)
		{
			Stream manifestResourceStream = type.Module.Assembly.GetManifestResourceStream(type, resource);
			if (manifestResourceStream == null)
			{
				throw new ArgumentException(SR.GetString("ResourceNotFound", new object[]
				{
					type,
					resource
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(manifestResourceStream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, manifestResourceStream);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified data stream.</summary>
		/// <param name="stream">The data stream used to load the image. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="stream" /> does not contain image data or is <see langword="null" />.-or-
		///         <paramref name="stream" /> contains a PNG image file with a single dimension greater than 65,535 pixels.</exception>
		// Token: 0x06000030 RID: 48 RVA: 0x00002C1C File Offset: 0x00000E1C
		public Bitmap(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"stream",
					"null"
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(stream), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, stream);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified data stream.</summary>
		/// <param name="stream">The data stream used to load the image. </param>
		/// <param name="useIcm">
		///       <see langword="true" /> to use color correction for this <see cref="T:System.Drawing.Bitmap" />; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="stream" /> does not contain image data or is <see langword="null" />.-or-
		///         <paramref name="stream" /> contains a PNG image file with a single dimension greater than 65,535 pixels.</exception>
		// Token: 0x06000031 RID: 49 RVA: 0x00002CAC File Offset: 0x00000EAC
		public Bitmap(Stream stream, bool useIcm)
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"stream",
					"null"
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num;
			if (useIcm)
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStreamICM(new GPStream(stream), out zero);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipCreateBitmapFromStream(new GPStream(stream), out zero);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			num = SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef(null, zero));
			if (num != 0)
			{
				SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef(null, zero));
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
			Image.EnsureSave(this, null, stream);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class with the specified size, pixel format, and pixel data.</summary>
		/// <param name="width">The width, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="height">The height, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="stride">Integer that specifies the byte offset between the beginning of one scan line and the next. This is usually (but not necessarily) the number of bytes in the pixel format (for example, 2 for 16 bits per pixel) multiplied by the width of the bitmap. The value passed to this parameter must be a multiple of four.. </param>
		/// <param name="format">The pixel format for the new <see cref="T:System.Drawing.Bitmap" />. This must specify a value that begins with <paramref name="Format" />.</param>
		/// <param name="scan0">Pointer to an array of bytes that contains the pixel data.</param>
		/// <exception cref="T:System.ArgumentException">A <see cref="T:System.Drawing.Imaging.PixelFormat" /> value is specified whose name does not start with Format. For example, specifying <see cref="F:System.Drawing.Imaging.PixelFormat.Gdi" /> will cause an <see cref="T:System.ArgumentException" />, but <see cref="F:System.Drawing.Imaging.PixelFormat.Format48bppRgb" /> will not.</exception>
		// Token: 0x06000032 RID: 50 RVA: 0x00002D4C File Offset: 0x00000F4C
		public Bitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromScan0(width, height, stride, (int)format, new HandleRef(null, scan0), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class with the specified size and format.</summary>
		/// <param name="width">The width, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="height">The height, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="format">The pixel format for the new <see cref="T:System.Drawing.Bitmap" />. This must specify a value that begins with <paramref name="Format" />.</param>
		/// <exception cref="T:System.ArgumentException">A <see cref="T:System.Drawing.Imaging.PixelFormat" /> value is specified whose name does not start with Format. For example, specifying <see cref="F:System.Drawing.Imaging.PixelFormat.Gdi" /> will cause an <see cref="T:System.ArgumentException" />, but <see cref="F:System.Drawing.Imaging.PixelFormat.Format48bppRgb" /> will not.</exception>
		// Token: 0x06000033 RID: 51 RVA: 0x00002D98 File Offset: 0x00000F98
		public Bitmap(int width, int height, PixelFormat format)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromScan0(width, height, 0, (int)format, NativeMethods.NullHandleRef, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class with the specified size.</summary>
		/// <param name="width">The width, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="height">The height, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000034 RID: 52 RVA: 0x00002DD3 File Offset: 0x00000FD3
		public Bitmap(int width, int height) : this(width, height, PixelFormat.Format32bppArgb)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class with the specified size and with the resolution of the specified <see cref="T:System.Drawing.Graphics" /> object.</summary>
		/// <param name="width">The width, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="height">The height, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> object that specifies the resolution for the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="g" /> is <see langword="null" />.</exception>
		// Token: 0x06000035 RID: 53 RVA: 0x00002DE4 File Offset: 0x00000FE4
		public Bitmap(int width, int height, Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException(SR.GetString("InvalidArgument", new object[]
				{
					"g",
					"null"
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromGraphics(width, height, new HandleRef(g, g.NativeGraphics), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeImage(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified existing image.</summary>
		/// <param name="original">The <see cref="T:System.Drawing.Image" /> from which to create the new <see cref="T:System.Drawing.Bitmap" />. </param>
		// Token: 0x06000036 RID: 54 RVA: 0x00002E4D File Offset: 0x0000104D
		public Bitmap(Image original) : this(original, original.Width, original.Height)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified existing image, scaled to the specified size.</summary>
		/// <param name="original">The <see cref="T:System.Drawing.Image" /> from which to create the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="width">The width, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="height">The height, in pixels, of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000037 RID: 55 RVA: 0x00002E64 File Offset: 0x00001064
		public Bitmap(Image original, int width, int height) : this(width, height)
		{
			Graphics graphics = null;
			try
			{
				graphics = Graphics.FromImage(this);
				graphics.Clear(Color.Transparent);
				graphics.DrawImage(original, 0, 0, width, height);
			}
			finally
			{
				if (graphics != null)
				{
					graphics.Dispose();
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002EB4 File Offset: 0x000010B4
		private Bitmap(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from a Windows handle to an icon.</summary>
		/// <param name="hicon">A handle to an icon. </param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> that this method creates.</returns>
		// Token: 0x06000039 RID: 57 RVA: 0x00002EC0 File Offset: 0x000010C0
		public static Bitmap FromHicon(IntPtr hicon)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromHICON(new HandleRef(null, hicon), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Bitmap" /> from the specified Windows resource.</summary>
		/// <param name="hinstance">A handle to an instance of the executable file that contains the resource. </param>
		/// <param name="bitmapName">A string that contains the name of the resource bitmap. </param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> that this method creates.</returns>
		// Token: 0x0600003A RID: 58 RVA: 0x00002EFC File Offset: 0x000010FC
		public static Bitmap FromResource(IntPtr hinstance, string bitmapName)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			IntPtr intPtr = Marshal.StringToHGlobalUni(bitmapName);
			IntPtr handle;
			int num = SafeNativeMethods.Gdip.GdipCreateBitmapFromResource(new HandleRef(null, hinstance), new HandleRef(null, intPtr), out handle);
			Marshal.FreeHGlobal(intPtr);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(handle);
		}

		/// <summary>Creates a GDI bitmap object from this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <returns>A handle to the GDI bitmap object that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The height or width of the bitmap is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x0600003B RID: 59 RVA: 0x00002F46 File Offset: 0x00001146
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHbitmap()
		{
			return this.GetHbitmap(Color.LightGray);
		}

		/// <summary>Creates a GDI bitmap object from this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <param name="background">A <see cref="T:System.Drawing.Color" /> structure that specifies the background color. This parameter is ignored if the bitmap is totally opaque. </param>
		/// <returns>A handle to the GDI bitmap object that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">The height or width of the bitmap is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x0600003C RID: 60 RVA: 0x00002F54 File Offset: 0x00001154
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHbitmap(Color background)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHBITMAPFromBitmap(new HandleRef(this, this.nativeImage), out zero, ColorTranslator.ToWin32(background));
			if (num == 2 && (base.Width >= 32767 || base.Height >= 32767))
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidSize"));
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		/// <summary>Returns the handle to an icon.</summary>
		/// <returns>A Windows handle to an icon with the same image as the <see cref="T:System.Drawing.Bitmap" />.</returns>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x0600003D RID: 61 RVA: 0x00002FBC File Offset: 0x000011BC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr GetHicon()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHICONFromBitmap(new HandleRef(this, this.nativeImage), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Bitmap" /> class from the specified existing image, scaled to the specified size.</summary>
		/// <param name="original">The <see cref="T:System.Drawing.Image" /> from which to create the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="newSize">The <see cref="T:System.Drawing.Size" /> structure that represent the size of the new <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x0600003E RID: 62 RVA: 0x00002FEE File Offset: 0x000011EE
		public Bitmap(Image original, Size newSize) : this(original, (newSize != null) ? newSize.Width : 0, (newSize != null) ? newSize.Height : 0)
		{
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000301B File Offset: 0x0000121B
		private Bitmap()
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003024 File Offset: 0x00001224
		internal static Bitmap FromGDIplus(IntPtr handle)
		{
			Bitmap bitmap = new Bitmap();
			bitmap.SetNativeImage(handle);
			return bitmap;
		}

		/// <summary>Creates a copy of the section of this <see cref="T:System.Drawing.Bitmap" /> defined by <see cref="T:System.Drawing.Rectangle" /> structure and with a specified <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration.</summary>
		/// <param name="rect">Defines the portion of this <see cref="T:System.Drawing.Bitmap" /> to copy. Coordinates are relative to this <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="format">The pixel format for the new <see cref="T:System.Drawing.Bitmap" />. This must specify a value that begins with <paramref name="Format" />.</param>
		/// <returns>The new <see cref="T:System.Drawing.Bitmap" /> that this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">
		///         <paramref name="rect" /> is outside of the source bitmap bounds.</exception>
		/// <exception cref="T:System.ArgumentException">The height or width of <paramref name="rect" /> is 0. -or-A <see cref="T:System.Drawing.Imaging.PixelFormat" /> value is specified whose name does not start with Format. For example, specifying <see cref="F:System.Drawing.Imaging.PixelFormat.Gdi" /> will cause an <see cref="T:System.ArgumentException" />, but <see cref="F:System.Drawing.Imaging.PixelFormat.Format48bppRgb" /> will not.</exception>
		// Token: 0x06000041 RID: 65 RVA: 0x00003040 File Offset: 0x00001240
		public Bitmap Clone(Rectangle rect, PixelFormat format)
		{
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBitmapAreaI(rect.X, rect.Y, rect.Width, rect.Height, (int)format, new HandleRef(this, this.nativeImage), out zero);
			if (num != 0 || zero == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		/// <summary>Creates a copy of the section of this <see cref="T:System.Drawing.Bitmap" /> defined with a specified <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration.</summary>
		/// <param name="rect">Defines the portion of this <see cref="T:System.Drawing.Bitmap" /> to copy. </param>
		/// <param name="format">Specifies the <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration for the destination <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Bitmap" /> that this method creates.</returns>
		/// <exception cref="T:System.OutOfMemoryException">
		///         <paramref name="rect" /> is outside of the source bitmap bounds.</exception>
		/// <exception cref="T:System.ArgumentException">The height or width of <paramref name="rect" /> is 0. </exception>
		// Token: 0x06000042 RID: 66 RVA: 0x000030DC File Offset: 0x000012DC
		public Bitmap Clone(RectangleF rect, PixelFormat format)
		{
			if (rect.Width == 0f || rect.Height == 0f)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBitmapArea(rect.X, rect.Y, rect.Width, rect.Height, (int)format, new HandleRef(this, this.nativeImage), out zero);
			if (num != 0 || zero == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Bitmap.FromGDIplus(zero);
		}

		/// <summary>Makes the default transparent color transparent for this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">The image format of the <see cref="T:System.Drawing.Bitmap" /> is an icon format.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000043 RID: 67 RVA: 0x00003180 File Offset: 0x00001380
		public void MakeTransparent()
		{
			Color pixel = Bitmap.defaultTransparentColor;
			if (base.Height > 0 && base.Width > 0)
			{
				pixel = this.GetPixel(0, base.Size.Height - 1);
			}
			if (pixel.A < 255)
			{
				return;
			}
			this.MakeTransparent(pixel);
		}

		/// <summary>Makes the specified color transparent for this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <param name="transparentColor">The <see cref="T:System.Drawing.Color" /> structure that represents the color to make transparent. </param>
		/// <exception cref="T:System.InvalidOperationException">The image format of the <see cref="T:System.Drawing.Bitmap" /> is an icon format.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000044 RID: 68 RVA: 0x000031D4 File Offset: 0x000013D4
		public void MakeTransparent(Color transparentColor)
		{
			if (base.RawFormat.Guid == ImageFormat.Icon.Guid)
			{
				throw new InvalidOperationException(SR.GetString("CantMakeIconTransparent"));
			}
			Size size = base.Size;
			Bitmap bitmap = null;
			Graphics graphics = null;
			try
			{
				bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
				try
				{
					graphics = Graphics.FromImage(bitmap);
					graphics.Clear(Color.Transparent);
					Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);
					ImageAttributes imageAttributes = null;
					try
					{
						imageAttributes = new ImageAttributes();
						imageAttributes.SetColorKey(transparentColor, transparentColor);
						graphics.DrawImage(this, destRect, 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttributes, null, IntPtr.Zero);
					}
					finally
					{
						if (imageAttributes != null)
						{
							imageAttributes.Dispose();
						}
					}
				}
				finally
				{
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
				IntPtr nativeImage = this.nativeImage;
				this.nativeImage = bitmap.nativeImage;
				bitmap.nativeImage = nativeImage;
			}
			finally
			{
				if (bitmap != null)
				{
					bitmap.Dispose();
				}
			}
		}

		/// <summary>Locks a <see cref="T:System.Drawing.Bitmap" /> into system memory.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the portion of the <see cref="T:System.Drawing.Bitmap" /> to lock. </param>
		/// <param name="flags">An <see cref="T:System.Drawing.Imaging.ImageLockMode" /> enumeration that specifies the access level (read/write) for the <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="format">A <see cref="T:System.Drawing.Imaging.PixelFormat" /> enumeration that specifies the data format of this <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <returns>A <see cref="T:System.Drawing.Imaging.BitmapData" /> that contains information about this lock operation.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="T:System.Drawing.Imaging.PixelFormat" /> is not a specific bits-per-pixel value.-or-The incorrect <see cref="T:System.Drawing.Imaging.PixelFormat" /> is passed in for a bitmap.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000045 RID: 69 RVA: 0x000032F8 File Offset: 0x000014F8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format)
		{
			BitmapData bitmapData = new BitmapData();
			return this.LockBits(rect, flags, format, bitmapData);
		}

		/// <summary>Locks a <see cref="T:System.Drawing.Bitmap" /> into system memory </summary>
		/// <param name="rect">A rectangle structure that specifies the portion of the <see cref="T:System.Drawing.Bitmap" /> to lock.</param>
		/// <param name="flags">One of the <see cref="T:System.Drawing.Imaging.ImageLockMode" /> values that specifies the access level (read/write) for the <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="format">One of the <see cref="T:System.Drawing.Imaging.PixelFormat" /> values that specifies the data format of the <see cref="T:System.Drawing.Bitmap" />.</param>
		/// <param name="bitmapData">A <see cref="T:System.Drawing.Imaging.BitmapData" /> that contains information about the lock operation.</param>
		/// <returns>A <see cref="T:System.Drawing.Imaging.BitmapData" /> that contains information about the lock operation.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="T:System.Drawing.Imaging.PixelFormat" /> value is not a specific bits-per-pixel value.-or-The incorrect <see cref="T:System.Drawing.Imaging.PixelFormat" /> is passed in for a bitmap.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000046 RID: 70 RVA: 0x00003318 File Offset: 0x00001518
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public BitmapData LockBits(Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData)
		{
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipBitmapLockBits(new HandleRef(this, this.nativeImage), ref gprect, flags, format, bitmapData);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return bitmapData;
		}

		/// <summary>Unlocks this <see cref="T:System.Drawing.Bitmap" /> from system memory.</summary>
		/// <param name="bitmapdata">A <see cref="T:System.Drawing.Imaging.BitmapData" /> that specifies information about the lock operation. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000047 RID: 71 RVA: 0x00003354 File Offset: 0x00001554
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void UnlockBits(BitmapData bitmapdata)
		{
			int num = SafeNativeMethods.Gdip.GdipBitmapUnlockBits(new HandleRef(this, this.nativeImage), bitmapdata);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Gets the color of the specified pixel in this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <param name="x">The x-coordinate of the pixel to retrieve. </param>
		/// <param name="y">The y-coordinate of the pixel to retrieve. </param>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure that represents the color of the specified pixel.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="x" /> is less than 0, or greater than or equal to <see cref="P:System.Drawing.Image.Width" />. -or-
		///         <paramref name="y" /> is less than 0, or greater than or equal to <see cref="P:System.Drawing.Image.Height" />.</exception>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000048 RID: 72 RVA: 0x00003380 File Offset: 0x00001580
		public Color GetPixel(int x, int y)
		{
			int argb = 0;
			if (x < 0 || x >= base.Width)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("ValidRangeX"));
			}
			if (y < 0 || y >= base.Height)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("ValidRangeY"));
			}
			int num = SafeNativeMethods.Gdip.GdipBitmapGetPixel(new HandleRef(this, this.nativeImage), x, y, out argb);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return Color.FromArgb(argb);
		}

		/// <summary>Sets the color of the specified pixel in this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <param name="x">The x-coordinate of the pixel to set. </param>
		/// <param name="y">The y-coordinate of the pixel to set. </param>
		/// <param name="color">A <see cref="T:System.Drawing.Color" /> structure that represents the color to assign to the specified pixel. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x06000049 RID: 73 RVA: 0x000033FC File Offset: 0x000015FC
		public void SetPixel(int x, int y, Color color)
		{
			if ((base.PixelFormat & PixelFormat.Indexed) != PixelFormat.Undefined)
			{
				throw new InvalidOperationException(SR.GetString("GdiplusCannotSetPixelFromIndexedPixelFormat"));
			}
			if (x < 0 || x >= base.Width)
			{
				throw new ArgumentOutOfRangeException("x", SR.GetString("ValidRangeX"));
			}
			if (y < 0 || y >= base.Height)
			{
				throw new ArgumentOutOfRangeException("y", SR.GetString("ValidRangeY"));
			}
			int num = SafeNativeMethods.Gdip.GdipBitmapSetPixel(new HandleRef(this, this.nativeImage), x, y, color.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Sets the resolution for this <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <param name="xDpi">The horizontal resolution, in dots per inch, of the <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <param name="yDpi">The vertical resolution, in dots per inch, of the <see cref="T:System.Drawing.Bitmap" />. </param>
		/// <exception cref="T:System.Exception">The operation failed.</exception>
		// Token: 0x0600004A RID: 74 RVA: 0x00003490 File Offset: 0x00001690
		public void SetResolution(float xDpi, float yDpi)
		{
			int num = SafeNativeMethods.Gdip.GdipBitmapSetResolution(new HandleRef(this, this.nativeImage), xDpi, yDpi);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0400009C RID: 156
		private static Color defaultTransparentColor = Color.LightGray;
	}
}
