using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Drawing
{
	/// <summary>Represents a Windows icon, which is a small bitmap image that is used to represent an object. Icons can be thought of as transparent bitmaps, although their size is determined by the system.</summary>
	// Token: 0x0200001E RID: 30
	[TypeConverter(typeof(IconConverter))]
	[Editor("System.Drawing.Design.IconEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public sealed class Icon : MarshalByRefObject, ISerializable, ICloneable, IDisposable
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0000D3F3 File Offset: 0x0000B5F3
		private Icon()
		{
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000D418 File Offset: 0x0000B618
		internal Icon(IntPtr handle) : this(handle, false)
		{
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000D424 File Offset: 0x0000B624
		internal Icon(IntPtr handle, bool takeOwnership)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidGDIHandle", new object[]
				{
					typeof(Icon).Name
				}));
			}
			this.handle = handle;
			this.ownHandle = takeOwnership;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class from the specified file name.</summary>
		/// <param name="fileName">The file to load the <see cref="T:System.Drawing.Icon" /> from. </param>
		// Token: 0x060002F3 RID: 755 RVA: 0x0000D497 File Offset: 0x0000B697
		public Icon(string fileName) : this(fileName, 0, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class of the specified size from the specified file.</summary>
		/// <param name="fileName">The name and path to the file that contains the icon data.</param>
		/// <param name="size">The desired size of the icon.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="string" /> is <see langword="null" /> or does not contain image data.</exception>
		// Token: 0x060002F4 RID: 756 RVA: 0x0000D4A2 File Offset: 0x0000B6A2
		public Icon(string fileName, Size size) : this(fileName, size.Width, size.Height)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class with the specified width and height from the specified file.</summary>
		/// <param name="fileName">The name and path to the file that contains the <see cref="T:System.Drawing.Icon" /> data.</param>
		/// <param name="width">The desired width of the <see cref="T:System.Drawing.Icon" />.</param>
		/// <param name="height">The desired height of the <see cref="T:System.Drawing.Icon" />.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="string" /> is <see langword="null" /> or does not contain image data.</exception>
		// Token: 0x060002F5 RID: 757 RVA: 0x0000D4BC File Offset: 0x0000B6BC
		public Icon(string fileName, int width, int height) : this()
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				this.iconData = new byte[(int)fileStream.Length];
				fileStream.Read(this.iconData, 0, this.iconData.Length);
			}
			this.Initialize(width, height);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class and attempts to find a version of the icon that matches the requested size.</summary>
		/// <param name="original">The <see cref="T:System.Drawing.Icon" /> from which to load the newly sized icon. </param>
		/// <param name="size">A <see cref="T:System.Drawing.Size" /> structure that specifies the height and width of the new <see cref="T:System.Drawing.Icon" />. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="original" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060002F6 RID: 758 RVA: 0x0000D528 File Offset: 0x0000B728
		public Icon(Icon original, Size size) : this(original, size.Width, size.Height)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class and attempts to find a version of the icon that matches the requested size.</summary>
		/// <param name="original">The icon to load the different size from. </param>
		/// <param name="width">The width of the new icon. </param>
		/// <param name="height">The height of the new icon. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="original" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060002F7 RID: 759 RVA: 0x0000D540 File Offset: 0x0000B740
		public Icon(Icon original, int width, int height) : this()
		{
			if (original == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"original",
					"null"
				}));
			}
			this.iconData = original.iconData;
			if (this.iconData == null)
			{
				this.iconSize = original.Size;
				this.handle = SafeNativeMethods.CopyImage(new HandleRef(original, original.Handle), 1, this.iconSize.Width, this.iconSize.Height, 0);
				return;
			}
			this.Initialize(width, height);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class from a resource in the specified assembly.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that specifies the assembly in which to look for the resource. </param>
		/// <param name="resource">The resource name to load. </param>
		/// <exception cref="T:System.ArgumentException">An icon specified by <paramref name="resource" /> cannot be found in the assembly that contains the specified <paramref name="type" />.</exception>
		// Token: 0x060002F8 RID: 760 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		public Icon(Type type, string resource) : this()
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
			this.iconData = new byte[(int)manifestResourceStream.Length];
			manifestResourceStream.Read(this.iconData, 0, this.iconData.Length);
			this.Initialize(0, 0);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class from the specified data stream.</summary>
		/// <param name="stream">The data stream from which to load the <see cref="T:System.Drawing.Icon" />. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="stream" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060002F9 RID: 761 RVA: 0x0000D64B File Offset: 0x0000B84B
		public Icon(Stream stream) : this(stream, 0, 0)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class of the specified size from the specified stream.</summary>
		/// <param name="stream">The stream that contains the icon data.</param>
		/// <param name="size">The desired size of the icon.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="stream" /> is <see langword="null" /> or does not contain image data.</exception>
		// Token: 0x060002FA RID: 762 RVA: 0x0000D656 File Offset: 0x0000B856
		public Icon(Stream stream, Size size) : this(stream, size.Width, size.Height)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Icon" /> class from the specified data stream and with the specified width and height.</summary>
		/// <param name="stream">The data stream from which to load the icon. </param>
		/// <param name="width">The width, in pixels, of the icon. </param>
		/// <param name="height">The height, in pixels, of the icon. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="stream" /> parameter is <see langword="null" />.</exception>
		// Token: 0x060002FB RID: 763 RVA: 0x0000D670 File Offset: 0x0000B870
		public Icon(Stream stream, int width, int height) : this()
		{
			if (stream == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"stream",
					"null"
				}));
			}
			this.iconData = new byte[(int)stream.Length];
			stream.Read(this.iconData, 0, this.iconData.Length);
			this.Initialize(width, height);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000D6DC File Offset: 0x0000B8DC
		private Icon(SerializationInfo info, StreamingContext context)
		{
			this.iconData = (byte[])info.GetValue("IconData", typeof(byte[]));
			this.iconSize = (Size)info.GetValue("IconSize", typeof(Size));
			if (this.iconSize.IsEmpty)
			{
				this.Initialize(0, 0);
				return;
			}
			this.Initialize(this.iconSize.Width, this.iconSize.Height);
		}

		/// <summary>Returns an icon representation of an image that is contained in the specified file.</summary>
		/// <param name="filePath">The path to the file that contains an image.</param>
		/// <returns>The <see cref="T:System.Drawing.Icon" /> representation of the image that is contained in the specified file.</returns>
		/// <exception cref="T:System.ArgumentException">The <paramref name="filePath" /> does not indicate a valid file.-or-The <paramref name="filePath" /> indicates a Universal Naming Convention (UNC) path.</exception>
		// Token: 0x060002FD RID: 765 RVA: 0x0000D77E File Offset: 0x0000B97E
		public static Icon ExtractAssociatedIcon(string filePath)
		{
			return Icon.ExtractAssociatedIcon(filePath, 0);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D788 File Offset: 0x0000B988
		private static Icon ExtractAssociatedIcon(string filePath, int index)
		{
			if (filePath == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"filePath",
					"null"
				}));
			}
			Uri uri;
			try
			{
				uri = new Uri(filePath);
			}
			catch (UriFormatException)
			{
				filePath = Path.GetFullPath(filePath);
				uri = new Uri(filePath);
			}
			if (uri.IsUnc)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"filePath",
					filePath
				}));
			}
			if (uri.IsFile)
			{
				if (!File.Exists(filePath))
				{
					IntSecurity.DemandReadFileIO(filePath);
					throw new FileNotFoundException(filePath);
				}
				Icon icon = new Icon();
				StringBuilder stringBuilder = new StringBuilder(260);
				stringBuilder.Append(filePath);
				IntPtr value = SafeNativeMethods.ExtractAssociatedIcon(NativeMethods.NullHandleRef, stringBuilder, ref index);
				if (value != IntPtr.Zero)
				{
					IntSecurity.ObjectFromWin32Handle.Demand();
					return new Icon(value, true);
				}
			}
			return null;
		}

		/// <summary>Gets the Windows handle for this <see cref="T:System.Drawing.Icon" />. This is not a copy of the handle; do not free it.</summary>
		/// <returns>The Windows handle for the icon.</returns>
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060002FF RID: 767 RVA: 0x0000D87C File Offset: 0x0000BA7C
		[Browsable(false)]
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.handle;
			}
		}

		/// <summary>Gets the height of this <see cref="T:System.Drawing.Icon" />.</summary>
		/// <returns>The height of this <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
		[Browsable(false)]
		public int Height
		{
			get
			{
				return this.Size.Height;
			}
		}

		/// <summary>Gets the size of this <see cref="T:System.Drawing.Icon" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> structure that specifies the width and height of this <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000301 RID: 769 RVA: 0x0000D8C4 File Offset: 0x0000BAC4
		public Size Size
		{
			get
			{
				if (this.iconSize.IsEmpty)
				{
					SafeNativeMethods.ICONINFO iconinfo = new SafeNativeMethods.ICONINFO();
					SafeNativeMethods.GetIconInfo(new HandleRef(this, this.Handle), iconinfo);
					SafeNativeMethods.BITMAP bitmap = new SafeNativeMethods.BITMAP();
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap);
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
						this.iconSize = new Size(bitmap.bmWidth, bitmap.bmHeight);
					}
					else if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmMask), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap);
						this.iconSize = new Size(bitmap.bmWidth, bitmap.bmHeight / 2);
					}
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
					}
				}
				return this.iconSize;
			}
		}

		/// <summary>Gets the width of this <see cref="T:System.Drawing.Icon" />.</summary>
		/// <returns>The width of this <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
		[Browsable(false)]
		public int Width
		{
			get
			{
				return this.Size.Width;
			}
		}

		/// <summary>Clones the <see cref="T:System.Drawing.Icon" />, creating a duplicate image.</summary>
		/// <returns>An object that can be cast to an <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x06000303 RID: 771 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		public object Clone()
		{
			return new Icon(this, this.Size.Width, this.Size.Height);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000DA23 File Offset: 0x0000BC23
		internal void DestroyHandle()
		{
			if (this.ownHandle)
			{
				SafeNativeMethods.DestroyIcon(new HandleRef(this, this.handle));
				this.handle = IntPtr.Zero;
			}
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Icon" />.</summary>
		// Token: 0x06000305 RID: 773 RVA: 0x0000DA4A File Offset: 0x0000BC4A
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000DA59 File Offset: 0x0000BC59
		private void Dispose(bool disposing)
		{
			if (this.handle != IntPtr.Zero)
			{
				this.DestroyHandle();
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000DA74 File Offset: 0x0000BC74
		private void DrawIcon(IntPtr dc, Rectangle imageRect, Rectangle targetRect, bool stretch)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			Size size = this.Size;
			int width;
			int height;
			if (!imageRect.IsEmpty)
			{
				num = imageRect.X;
				num2 = imageRect.Y;
				width = imageRect.Width;
				height = imageRect.Height;
			}
			else
			{
				width = size.Width;
				height = size.Height;
			}
			int width2;
			int height2;
			if (!targetRect.IsEmpty)
			{
				num3 = targetRect.X;
				num4 = targetRect.Y;
				width2 = targetRect.Width;
				height2 = targetRect.Height;
			}
			else
			{
				width2 = size.Width;
				height2 = size.Height;
			}
			int width3;
			int height3;
			int num5;
			int num6;
			if (stretch)
			{
				width3 = size.Width * width2 / width;
				height3 = size.Height * height2 / height;
				num5 = width2;
				num6 = height2;
			}
			else
			{
				width3 = size.Width;
				height3 = size.Height;
				num5 = ((width2 < width) ? width2 : width);
				num6 = ((height2 < height) ? height2 : height);
			}
			IntPtr hRgn = SafeNativeMethods.SaveClipRgn(dc);
			try
			{
				SafeNativeMethods.IntersectClipRect(new HandleRef(this, dc), num3, num4, num3 + num5, num4 + num6);
				SafeNativeMethods.DrawIconEx(new HandleRef(null, dc), num3 - num, num4 - num2, new HandleRef(this, this.handle), width3, height3, 0, NativeMethods.NullHandleRef, 3);
			}
			finally
			{
				SafeNativeMethods.RestoreClipRgn(dc, hRgn);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
		internal void Draw(Graphics graphics, int x, int y)
		{
			Size size = this.Size;
			this.Draw(graphics, new Rectangle(x, y, size.Width, size.Height));
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000DC04 File Offset: 0x0000BE04
		internal void Draw(Graphics graphics, Rectangle targetRect)
		{
			Rectangle targetRect2 = targetRect;
			targetRect2.X += (int)graphics.Transform.OffsetX;
			targetRect2.Y += (int)graphics.Transform.OffsetY;
			WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping);
			IntPtr hdc = windowsGraphics.GetHdc();
			try
			{
				this.DrawIcon(hdc, Rectangle.Empty, targetRect2, true);
			}
			finally
			{
				windowsGraphics.Dispose();
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000DC80 File Offset: 0x0000BE80
		internal void DrawUnstretched(Graphics graphics, Rectangle targetRect)
		{
			Rectangle targetRect2 = targetRect;
			targetRect2.X += (int)graphics.Transform.OffsetX;
			targetRect2.Y += (int)graphics.Transform.OffsetY;
			WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(graphics, ApplyGraphicsProperties.Clipping);
			IntPtr hdc = windowsGraphics.GetHdc();
			try
			{
				this.DrawIcon(hdc, Rectangle.Empty, targetRect2, false);
			}
			finally
			{
				windowsGraphics.Dispose();
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x0600030B RID: 779 RVA: 0x0000DCFC File Offset: 0x0000BEFC
		~Icon()
		{
			this.Dispose(false);
		}

		/// <summary>Creates a GDI+ <see cref="T:System.Drawing.Icon" /> from the specified Windows handle to an icon (<see langword="HICON" />).</summary>
		/// <param name="handle">A Windows handle to an icon. </param>
		/// <returns>The <see cref="T:System.Drawing.Icon" /> this method creates.</returns>
		// Token: 0x0600030C RID: 780 RVA: 0x0000DD2C File Offset: 0x0000BF2C
		public static Icon FromHandle(IntPtr handle)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			return new Icon(handle);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000DD40 File Offset: 0x0000BF40
		private unsafe short GetShort(byte* pb)
		{
			int num;
			if ((pb & 1) != 0)
			{
				num = (int)(*pb);
				pb++;
				num |= (int)(*pb) << 8;
			}
			else
			{
				num = (int)(*(short*)pb);
			}
			return (short)num;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		private unsafe int GetInt(byte* pb)
		{
			int num;
			if ((pb & 3) != 0)
			{
				num = (int)(*pb);
				pb++;
				num |= (int)(*pb) << 8;
				pb++;
				num |= (int)(*pb) << 16;
				pb++;
				num |= (int)(*pb) << 24;
			}
			else
			{
				num = *(int*)pb;
			}
			return num;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		private unsafe void Initialize(int width, int height)
		{
			if (this.iconData == null || this.handle != IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("IllegalState", new object[]
				{
					base.GetType().Name
				}));
			}
			int num = Marshal.SizeOf(typeof(SafeNativeMethods.ICONDIR));
			if (this.iconData.Length < num)
			{
				throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
				{
					"picture",
					"Icon"
				}));
			}
			if (width == 0)
			{
				width = UnsafeNativeMethods.GetSystemMetrics(11);
			}
			if (height == 0)
			{
				height = UnsafeNativeMethods.GetSystemMetrics(12);
			}
			if (Icon.bitDepth == 0)
			{
				IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
				Icon.bitDepth = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 12);
				Icon.bitDepth *= UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 14);
				UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
				if (Icon.bitDepth == 8)
				{
					Icon.bitDepth = 4;
				}
			}
			fixed (byte* ptr = this.iconData)
			{
				short @short = this.GetShort(ptr);
				short short2 = this.GetShort(ptr + 2);
				short short3 = this.GetShort(ptr + 4);
				if (@short != 0 || short2 != 1 || short3 == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
					{
						"picture",
						"Icon"
					}));
				}
				byte b = 0;
				byte b2 = 0;
				byte* ptr2 = ptr + 6;
				int num2 = Marshal.SizeOf(typeof(SafeNativeMethods.ICONDIRENTRY));
				if (num2 * (int)(short3 - 1) + num > this.iconData.Length)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
					{
						"picture",
						"Icon"
					}));
				}
				for (int i = 0; i < (int)short3; i++)
				{
					SafeNativeMethods.ICONDIRENTRY icondirentry;
					icondirentry.bWidth = *ptr2;
					icondirentry.bHeight = ptr2[1];
					icondirentry.bColorCount = ptr2[2];
					icondirentry.bReserved = ptr2[3];
					icondirentry.wPlanes = this.GetShort(ptr2 + 4);
					icondirentry.wBitCount = this.GetShort(ptr2 + 6);
					icondirentry.dwBytesInRes = this.GetInt(ptr2 + 8);
					icondirentry.dwImageOffset = this.GetInt(ptr2 + 12);
					bool flag = false;
					int num3;
					if (icondirentry.bColorCount != 0)
					{
						num3 = 4;
						if (icondirentry.bColorCount < 16)
						{
							num3 = 1;
						}
					}
					else
					{
						num3 = (int)icondirentry.wBitCount;
					}
					if (num3 == 0)
					{
						num3 = 8;
					}
					if (this.bestBytesInRes == 0)
					{
						flag = true;
					}
					else
					{
						int num4 = Math.Abs((int)b - width) + Math.Abs((int)b2 - height);
						int num5 = Math.Abs((int)icondirentry.bWidth - width) + Math.Abs((int)icondirentry.bHeight - height);
						if (num5 < num4 || (num5 == num4 && ((num3 <= Icon.bitDepth && num3 > this.bestBitDepth) || (this.bestBitDepth > Icon.bitDepth && num3 < this.bestBitDepth))))
						{
							flag = true;
						}
					}
					if (flag)
					{
						b = icondirentry.bWidth;
						b2 = icondirentry.bHeight;
						this.bestImageOffset = icondirentry.dwImageOffset;
						this.bestBytesInRes = icondirentry.dwBytesInRes;
						this.bestBitDepth = num3;
					}
					ptr2 += num2;
				}
				if (this.bestImageOffset < 0)
				{
					throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
					{
						"picture",
						"Icon"
					}));
				}
				if (this.bestBytesInRes < 0)
				{
					throw new Win32Exception(87);
				}
				checked
				{
					int num6;
					try
					{
						num6 = this.bestImageOffset + this.bestBytesInRes;
					}
					catch (OverflowException)
					{
						throw new Win32Exception(87);
					}
					if (num6 > this.iconData.Length)
					{
						throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
						{
							"picture",
							"Icon"
						}));
					}
					if (this.bestImageOffset % IntPtr.Size != 0)
					{
						byte[] array = new byte[this.bestBytesInRes];
						Array.Copy(this.iconData, this.bestImageOffset, array, 0, this.bestBytesInRes);
						fixed (byte* ptr3 = array)
						{
							this.handle = SafeNativeMethods.CreateIconFromResourceEx(ptr3, this.bestBytesInRes, true, 196608, 0, 0, 0);
						}
					}
					else
					{
						try
						{
							this.handle = SafeNativeMethods.CreateIconFromResourceEx(ptr + this.bestImageOffset, this.bestBytesInRes, true, 196608, 0, 0, 0);
						}
						catch (OverflowException)
						{
							throw new Win32Exception(87);
						}
					}
					if (this.handle == IntPtr.Zero)
					{
						throw new Win32Exception();
					}
				}
			}
		}

		/// <summary>Saves this <see cref="T:System.Drawing.Icon" /> to the specified output <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="outputStream">The <see cref="T:System.IO.Stream" /> to save to. </param>
		// Token: 0x06000310 RID: 784 RVA: 0x0000E258 File Offset: 0x0000C458
		public void Save(Stream outputStream)
		{
			if (this.iconData != null)
			{
				outputStream.Write(this.iconData, 0, this.iconData.Length);
				return;
			}
			SafeNativeMethods.PICTDESC pictdesc = SafeNativeMethods.PICTDESC.CreateIconPICTDESC(this.Handle);
			Guid guid = typeof(SafeNativeMethods.IPicture).GUID;
			SafeNativeMethods.IPicture picture = SafeNativeMethods.OleCreatePictureIndirect(pictdesc, ref guid, false);
			if (picture != null)
			{
				try
				{
					int num;
					picture.SaveAsFile(new UnsafeNativeMethods.ComStreamFromDataStream(outputStream), -1, out num);
				}
				finally
				{
					Marshal.ReleaseComObject(picture);
				}
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		private void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < Math.Min(sourceData.Height, targetData.Height); i++)
			{
				IntPtr intPtr;
				IntPtr intPtr2;
				if (IntPtr.Size == 4)
				{
					intPtr = new IntPtr(sourceData.Scan0.ToInt32() + num);
					intPtr2 = new IntPtr(targetData.Scan0.ToInt32() + num2);
				}
				else
				{
					intPtr = new IntPtr(sourceData.Scan0.ToInt64() + (long)num);
					intPtr2 = new IntPtr(targetData.Scan0.ToInt64() + (long)num2);
				}
				UnsafeNativeMethods.CopyMemory(new HandleRef(this, intPtr2), new HandleRef(this, intPtr), Math.Abs(targetData.Stride));
				num += sourceData.Stride;
				num2 += targetData.Stride;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		private unsafe static bool BitmapHasAlpha(BitmapData bmpData)
		{
			bool result = false;
			for (int i = 0; i < bmpData.Height; i++)
			{
				for (int j = 3; j < Math.Abs(bmpData.Stride); j += 4)
				{
					byte* ptr = (byte*)((byte*)bmpData.Scan0.ToPointer() + i * bmpData.Stride) + j;
					if (*ptr != 0)
					{
						return true;
					}
				}
			}
			return result;
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Icon" /> to a GDI+ <see cref="T:System.Drawing.Bitmap" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Bitmap" /> that represents the converted <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x06000313 RID: 787 RVA: 0x0000E402 File Offset: 0x0000C602
		public Bitmap ToBitmap()
		{
			if (this.HasPngSignature() && !LocalAppContextSwitches.DontSupportPngFramesInIcons)
			{
				return this.PngFrame();
			}
			return this.BmpFrame();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000E420 File Offset: 0x0000C620
		private unsafe Bitmap BmpFrame()
		{
			Bitmap bitmap = null;
			if (this.iconData != null && this.bestBitDepth == 32)
			{
				bitmap = new Bitmap(this.Size.Width, this.Size.Height, PixelFormat.Format32bppArgb);
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, this.Size.Width, this.Size.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				try
				{
					uint* ptr = (uint*)bitmapData.Scan0.ToPointer();
					int num = this.bestImageOffset + Marshal.SizeOf(typeof(SafeNativeMethods.BITMAPINFOHEADER));
					int length = this.Size.Width * 4;
					int width = this.Size.Width;
					for (int i = (this.Size.Height - 1) * 4; i >= 0; i -= 4)
					{
						Marshal.Copy(this.iconData, num + i * width, (IntPtr)((void*)ptr), length);
						ptr += width;
					}
					goto IL_29F;
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
				}
			}
			if (this.bestBitDepth == 0 || this.bestBitDepth == 32)
			{
				SafeNativeMethods.ICONINFO iconinfo = new SafeNativeMethods.ICONINFO();
				SafeNativeMethods.GetIconInfo(new HandleRef(this, this.handle), iconinfo);
				SafeNativeMethods.BITMAP bitmap2 = new SafeNativeMethods.BITMAP();
				try
				{
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(SafeNativeMethods.BITMAP)), bitmap2);
						if (bitmap2.bmBitsPixel == 32)
						{
							Bitmap bitmap3 = null;
							BitmapData bitmapData2 = null;
							BitmapData bitmapData3 = null;
							IntSecurity.ObjectFromWin32Handle.Assert();
							try
							{
								bitmap3 = Image.FromHbitmap(iconinfo.hbmColor);
								bitmapData2 = bitmap3.LockBits(new Rectangle(0, 0, bitmap3.Width, bitmap3.Height), ImageLockMode.ReadOnly, bitmap3.PixelFormat);
								if (Icon.BitmapHasAlpha(bitmapData2))
								{
									bitmap = new Bitmap(bitmapData2.Width, bitmapData2.Height, PixelFormat.Format32bppArgb);
									bitmapData3 = bitmap.LockBits(new Rectangle(0, 0, bitmapData2.Width, bitmapData2.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
									this.CopyBitmapData(bitmapData2, bitmapData3);
								}
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
								if (bitmap3 != null && bitmapData2 != null)
								{
									bitmap3.UnlockBits(bitmapData2);
								}
								if (bitmap != null && bitmapData3 != null)
								{
									bitmap.UnlockBits(bitmapData3);
								}
							}
							bitmap3.Dispose();
						}
					}
				}
				finally
				{
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
					}
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
					}
				}
			}
			IL_29F:
			if (bitmap == null)
			{
				Size size = this.Size;
				bitmap = new Bitmap(size.Width, size.Height);
				Graphics graphics = null;
				try
				{
					graphics = Graphics.FromImage(bitmap);
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						using (Bitmap bitmap4 = Bitmap.FromHicon(this.Handle))
						{
							graphics.DrawImage(bitmap4, new Rectangle(0, 0, size.Width, size.Height));
						}
					}
					catch (ArgumentException)
					{
						this.Draw(graphics, new Rectangle(0, 0, size.Width, size.Height));
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				finally
				{
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
				Color transparentColor = Color.FromArgb(13, 11, 12);
				bitmap.MakeTransparent(transparentColor);
			}
			return bitmap;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000E834 File Offset: 0x0000CA34
		private Bitmap PngFrame()
		{
			Bitmap result = null;
			if (this.iconData != null)
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					memoryStream.Write(this.iconData, this.bestImageOffset, this.bestBytesInRes);
					result = new Bitmap(memoryStream);
				}
			}
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000E890 File Offset: 0x0000CA90
		private bool HasPngSignature()
		{
			if (this.isBestImagePng == null)
			{
				if (this.iconData != null && this.iconData.Length >= this.bestImageOffset + 8)
				{
					int num = BitConverter.ToInt32(this.iconData, this.bestImageOffset);
					int num2 = BitConverter.ToInt32(this.iconData, this.bestImageOffset + 4);
					this.isBestImagePng = new bool?(num == 1196314761 && num2 == 169478669);
				}
				else
				{
					this.isBestImagePng = new bool?(false);
				}
			}
			return this.isBestImagePng.Value;
		}

		/// <summary>Gets a human-readable string that describes the <see cref="T:System.Drawing.Icon" />.</summary>
		/// <returns>A string that describes the <see cref="T:System.Drawing.Icon" />.</returns>
		// Token: 0x06000317 RID: 791 RVA: 0x0000E921 File Offset: 0x0000CB21
		public override string ToString()
		{
			return SR.GetString("toStringIcon");
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data that is required to serialize the target object.</summary>
		/// <param name="si">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
		// Token: 0x06000318 RID: 792 RVA: 0x0000E930 File Offset: 0x0000CB30
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (this.iconData != null)
			{
				si.AddValue("IconData", this.iconData, typeof(byte[]));
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				this.Save(memoryStream);
				si.AddValue("IconData", memoryStream.ToArray(), typeof(byte[]));
			}
			si.AddValue("IconSize", this.iconSize, typeof(Size));
		}

		// Token: 0x04000183 RID: 387
		private static int bitDepth;

		// Token: 0x04000184 RID: 388
		private const int PNGSignature1 = 1196314761;

		// Token: 0x04000185 RID: 389
		private const int PNGSignature2 = 169478669;

		// Token: 0x04000186 RID: 390
		private byte[] iconData;

		// Token: 0x04000187 RID: 391
		private int bestImageOffset;

		// Token: 0x04000188 RID: 392
		private int bestBitDepth;

		// Token: 0x04000189 RID: 393
		private int bestBytesInRes;

		// Token: 0x0400018A RID: 394
		private bool? isBestImagePng;

		// Token: 0x0400018B RID: 395
		private Size iconSize = Size.Empty;

		// Token: 0x0400018C RID: 396
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x0400018D RID: 397
		private bool ownHandle = true;
	}
}
