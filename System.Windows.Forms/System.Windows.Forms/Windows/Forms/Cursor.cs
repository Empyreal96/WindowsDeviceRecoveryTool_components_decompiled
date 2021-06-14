using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the image used to paint the mouse pointer.</summary>
	// Token: 0x02000166 RID: 358
	[TypeConverter(typeof(CursorConverter))]
	[Editor("System.Drawing.Design.CursorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public sealed class Cursor : IDisposable, ISerializable
	{
		// Token: 0x06001085 RID: 4229 RVA: 0x0003A660 File Offset: 0x00038860
		internal Cursor(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "CursorData", StringComparison.OrdinalIgnoreCase))
				{
					this.cursorData = (byte[])enumerator.Value;
					if (this.cursorData != null)
					{
						this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
					}
				}
				else if (string.Compare(enumerator.Name, "CursorResourceId", true, CultureInfo.InvariantCulture) == 0)
				{
					this.LoadFromResourceId((int)enumerator.Value);
				}
			}
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0003A705 File Offset: 0x00038905
		internal Cursor(int nResourceId, int dummy)
		{
			this.LoadFromResourceId(nResourceId);
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0003A728 File Offset: 0x00038928
		internal Cursor(string resource, int dummy)
		{
			Stream manifestResourceStream = typeof(Cursor).Module.Assembly.GetManifestResourceStream(typeof(Cursor), resource);
			this.cursorData = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(this.cursorData, 0, Convert.ToInt32(manifestResourceStream.Length));
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Cursor" /> class from the specified Windows handle.</summary>
		/// <param name="handle">An <see cref="T:System.IntPtr" /> that represents the Windows handle of the cursor to create. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="handle" /> is <see cref="F:System.IntPtr.Zero" />. </exception>
		// Token: 0x06001088 RID: 4232 RVA: 0x0003A7B4 File Offset: 0x000389B4
		public Cursor(IntPtr handle)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidGDIHandle", new object[]
				{
					typeof(Cursor).Name
				}));
			}
			this.handle = handle;
			this.ownHandle = false;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Cursor" /> class from the specified file.</summary>
		/// <param name="fileName">The cursor file to load. </param>
		// Token: 0x06001089 RID: 4233 RVA: 0x0003A828 File Offset: 0x00038A28
		public Cursor(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				this.cursorData = new byte[fileStream.Length];
				fileStream.Read(this.cursorData, 0, Convert.ToInt32(fileStream.Length));
			}
			finally
			{
				fileStream.Close();
			}
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Cursor" /> class from the specified resource with the specified resource type.</summary>
		/// <param name="type">The resource <see cref="T:System.Type" />. </param>
		/// <param name="resource">The name of the resource. </param>
		// Token: 0x0600108A RID: 4234 RVA: 0x0003A8B4 File Offset: 0x00038AB4
		public Cursor(Type type, string resource) : this(type.Module.Assembly.GetManifestResourceStream(type, resource))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Cursor" /> class from the specified data stream.</summary>
		/// <param name="stream">The data stream to load the <see cref="T:System.Windows.Forms.Cursor" /> from. </param>
		// Token: 0x0600108B RID: 4235 RVA: 0x0003A8D0 File Offset: 0x00038AD0
		public Cursor(Stream stream)
		{
			this.cursorData = new byte[stream.Length];
			stream.Read(this.cursorData, 0, Convert.ToInt32(stream.Length));
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		/// <summary>Gets or sets the bounds that represent the clipping rectangle for the cursor.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that represents the clipping rectangle for the <see cref="T:System.Windows.Forms.Cursor" />, in screen coordinates.</returns>
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x0003A936 File Offset: 0x00038B36
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x0003A93D File Offset: 0x00038B3D
		public static Rectangle Clip
		{
			get
			{
				return Cursor.ClipInternal;
			}
			set
			{
				if (!value.IsEmpty)
				{
					IntSecurity.AdjustCursorClip.Demand();
				}
				Cursor.ClipInternal = value;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x0003A958 File Offset: 0x00038B58
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x0003A994 File Offset: 0x00038B94
		internal static Rectangle ClipInternal
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClipCursor(ref rect);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
			set
			{
				if (value.IsEmpty)
				{
					UnsafeNativeMethods.ClipCursor(null);
					return;
				}
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(value.X, value.Y, value.Width, value.Height);
				UnsafeNativeMethods.ClipCursor(ref rect);
			}
		}

		/// <summary>Gets or sets a cursor object that represents the mouse cursor.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the mouse cursor. The default is <see langword="null" /> if the mouse cursor is not visible.</returns>
		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x0003A9DC File Offset: 0x00038BDC
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x0003A9E3 File Offset: 0x00038BE3
		public static Cursor Current
		{
			get
			{
				return Cursor.CurrentInternal;
			}
			set
			{
				IntSecurity.ModifyCursor.Demand();
				Cursor.CurrentInternal = value;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x0003A9F8 File Offset: 0x00038BF8
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x0003AA1C File Offset: 0x00038C1C
		internal static Cursor CurrentInternal
		{
			get
			{
				IntPtr cursor = SafeNativeMethods.GetCursor();
				IntSecurity.UnmanagedCode.Assert();
				return Cursors.KnownCursorFromHCursor(cursor);
			}
			set
			{
				IntPtr intPtr = (value == null) ? IntPtr.Zero : value.handle;
				UnsafeNativeMethods.SetCursor(new HandleRef(value, intPtr));
			}
		}

		/// <summary>Gets the handle of the cursor.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the cursor's handle.</returns>
		/// <exception cref="T:System.Exception">The handle value is <see cref="F:System.IntPtr.Zero" />. </exception>
		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x0003AA4D File Offset: 0x00038C4D
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new ObjectDisposedException(SR.GetString("ObjectDisposed", new object[]
					{
						base.GetType().Name
					}));
				}
				return this.handle;
			}
		}

		/// <summary>Gets the cursor hot spot.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> representing the cursor hot spot.</returns>
		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0003AA8C File Offset: 0x00038C8C
		public Point HotSpot
		{
			get
			{
				Point result = Point.Empty;
				NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
				Icon icon = null;
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					icon = Icon.FromHandle(this.Handle);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				try
				{
					SafeNativeMethods.GetIconInfo(new HandleRef(this, icon.Handle), iconinfo);
					result = new Point(iconinfo.xHotspot, iconinfo.yHotspot);
				}
				finally
				{
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, iconinfo.hbmMask));
						iconinfo.hbmMask = IntPtr.Zero;
					}
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, iconinfo.hbmColor));
						iconinfo.hbmColor = IntPtr.Zero;
					}
					icon.Dispose();
				}
				return result;
			}
		}

		/// <summary>Gets or sets the cursor's position.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the cursor's position in screen coordinates.</returns>
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x0003AB70 File Offset: 0x00038D70
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x0003AB9B File Offset: 0x00038D9B
		public static Point Position
		{
			get
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				return new Point(point.x, point.y);
			}
			set
			{
				IntSecurity.AdjustCursorPosition.Demand();
				UnsafeNativeMethods.SetCursorPos(value.X, value.Y);
			}
		}

		/// <summary>Gets the size of the cursor object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the width and height of the <see cref="T:System.Windows.Forms.Cursor" />.</returns>
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x0003ABBB File Offset: 0x00038DBB
		public Size Size
		{
			get
			{
				if (Cursor.cursorSize.IsEmpty)
				{
					Cursor.cursorSize = new Size(UnsafeNativeMethods.GetSystemMetrics(13), UnsafeNativeMethods.GetSystemMetrics(14));
				}
				return Cursor.cursorSize;
			}
		}

		/// <summary>Gets or sets the object that contains data about the <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.Cursor" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001099 RID: 4249 RVA: 0x0003ABE6 File Offset: 0x00038DE6
		// (set) Token: 0x0600109A RID: 4250 RVA: 0x0003ABEE File Offset: 0x00038DEE
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Copies the handle of this <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the cursor's handle.</returns>
		// Token: 0x0600109B RID: 4251 RVA: 0x0003ABF8 File Offset: 0x00038DF8
		public IntPtr CopyHandle()
		{
			Size size = this.Size;
			return SafeNativeMethods.CopyImage(new HandleRef(this, this.Handle), 2, size.Width, size.Height, 0);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0003AC2D File Offset: 0x00038E2D
		private void DestroyHandle()
		{
			if (this.ownHandle)
			{
				UnsafeNativeMethods.DestroyCursor(new HandleRef(this, this.handle));
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		// Token: 0x0600109D RID: 4253 RVA: 0x0003AC49 File Offset: 0x00038E49
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0003AC58 File Offset: 0x00038E58
		private void Dispose(bool disposing)
		{
			if (this.handle != IntPtr.Zero)
			{
				this.DestroyHandle();
				this.handle = IntPtr.Zero;
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0003AC80 File Offset: 0x00038E80
		private void DrawImageCore(Graphics graphics, Rectangle imageRect, Rectangle targetRect, bool stretch)
		{
			targetRect.X += (int)graphics.Transform.OffsetX;
			targetRect.Y += (int)graphics.Transform.OffsetY;
			int num = 13369376;
			IntPtr hdc = graphics.GetHdc();
			try
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				Size size = this.Size;
				int width;
				int height;
				if (!imageRect.IsEmpty)
				{
					num2 = imageRect.X;
					num3 = imageRect.Y;
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
					num4 = targetRect.X;
					num5 = targetRect.Y;
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
				int num6;
				int num7;
				if (stretch)
				{
					if (width2 == width && height2 == height && num2 == 0 && num3 == 0 && num == 13369376 && width == size.Width && height == size.Height)
					{
						SafeNativeMethods.DrawIcon(new HandleRef(graphics, hdc), num4, num5, new HandleRef(this, this.handle));
						return;
					}
					width3 = size.Width * width2 / width;
					height3 = size.Height * height2 / height;
					num6 = width2;
					num7 = height2;
				}
				else
				{
					if (num2 == 0 && num3 == 0 && num == 13369376 && size.Width <= width2 && size.Height <= height2 && size.Width == width && size.Height == height)
					{
						SafeNativeMethods.DrawIcon(new HandleRef(graphics, hdc), num4, num5, new HandleRef(this, this.handle));
						return;
					}
					width3 = size.Width;
					height3 = size.Height;
					num6 = ((width2 < width) ? width2 : width);
					num7 = ((height2 < height) ? height2 : height);
				}
				if (num == 13369376)
				{
					SafeNativeMethods.IntersectClipRect(new HandleRef(this, this.Handle), num4, num5, num4 + num6, num5 + num7);
					SafeNativeMethods.DrawIconEx(new HandleRef(graphics, hdc), num4 - num2, num5 - num3, new HandleRef(this, this.handle), width3, height3, 0, NativeMethods.NullHandleRef, 3);
				}
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		/// <summary>Draws the cursor on the specified surface, within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw the <see cref="T:System.Windows.Forms.Cursor" />. </param>
		/// <param name="targetRect">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.Cursor" />. </param>
		// Token: 0x060010A0 RID: 4256 RVA: 0x0003AEEC File Offset: 0x000390EC
		public void Draw(Graphics g, Rectangle targetRect)
		{
			this.DrawImageCore(g, Rectangle.Empty, targetRect, false);
		}

		/// <summary>Draws the cursor in a stretched format on the specified surface, within the specified bounds.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> surface on which to draw the <see cref="T:System.Windows.Forms.Cursor" />. </param>
		/// <param name="targetRect">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds of the <see cref="T:System.Windows.Forms.Cursor" />. </param>
		// Token: 0x060010A1 RID: 4257 RVA: 0x0003AEFC File Offset: 0x000390FC
		public void DrawStretched(Graphics g, Rectangle targetRect)
		{
			this.DrawImageCore(g, Rectangle.Empty, targetRect, true);
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060010A2 RID: 4258 RVA: 0x0003AF0C File Offset: 0x0003910C
		~Cursor()
		{
			this.Dispose(false);
		}

		/// <summary>Serializes the object.</summary>
		/// <param name="si">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class.</param>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> class.</param>
		// Token: 0x060010A3 RID: 4259 RVA: 0x0003AF3C File Offset: 0x0003913C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (this.cursorData != null)
			{
				si.AddValue("CursorData", this.cursorData, typeof(byte[]));
				return;
			}
			if (this.resourceId != 0)
			{
				si.AddValue("CursorResourceId", this.resourceId, typeof(int));
				return;
			}
			throw new SerializationException(SR.GetString("CursorNonSerializableHandle"));
		}

		/// <summary>Hides the cursor.</summary>
		// Token: 0x060010A4 RID: 4260 RVA: 0x0003AFA5 File Offset: 0x000391A5
		public static void Hide()
		{
			IntSecurity.AdjustCursorClip.Demand();
			UnsafeNativeMethods.ShowCursor(false);
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0003AFB8 File Offset: 0x000391B8
		private void LoadFromResourceId(int nResourceId)
		{
			this.ownHandle = false;
			try
			{
				this.resourceId = nResourceId;
				this.handle = SafeNativeMethods.LoadCursor(NativeMethods.NullHandleRef, nResourceId);
			}
			catch (Exception ex)
			{
				this.handle = IntPtr.Zero;
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0003B004 File Offset: 0x00039204
		private Size GetIconSize(IntPtr iconHandle)
		{
			Size size = this.Size;
			NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
			SafeNativeMethods.GetIconInfo(new HandleRef(this, iconHandle), iconinfo);
			NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
			if (iconinfo.hbmColor != IntPtr.Zero)
			{
				UnsafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap);
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
				size = new Size(bitmap.bmWidth, bitmap.bmHeight);
			}
			else if (iconinfo.hbmMask != IntPtr.Zero)
			{
				UnsafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmMask), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap);
				size = new Size(bitmap.bmWidth, bitmap.bmHeight / 2);
			}
			if (iconinfo.hbmMask != IntPtr.Zero)
			{
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
			}
			return size;
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0003B0FC File Offset: 0x000392FC
		private void LoadPicture(UnsafeNativeMethods.IStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			try
			{
				Guid guid = typeof(UnsafeNativeMethods.IPicture).GUID;
				UnsafeNativeMethods.IPicture picture = null;
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					picture = UnsafeNativeMethods.OleCreateIPictureIndirect(null, ref guid, true);
					UnsafeNativeMethods.IPersistStream persistStream = (UnsafeNativeMethods.IPersistStream)picture;
					persistStream.Load(stream);
					if (picture == null || picture.GetPictureType() != 3)
					{
						throw new ArgumentException(SR.GetString("InvalidPictureType", new object[]
						{
							"picture",
							"Cursor"
						}), "picture");
					}
					IntPtr iconHandle = picture.GetHandle();
					Size logicalSize = this.GetIconSize(iconHandle);
					if (DpiHelper.IsScalingRequired)
					{
						logicalSize = DpiHelper.LogicalToDeviceUnits(logicalSize, 0);
					}
					this.handle = SafeNativeMethods.CopyImageAsCursor(new HandleRef(this, iconHandle), 2, logicalSize.Width, logicalSize.Height, 0);
					this.ownHandle = true;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					if (picture != null)
					{
						Marshal.ReleaseComObject(picture);
					}
				}
			}
			catch (COMException innerException)
			{
				throw new ArgumentException(SR.GetString("InvalidPictureFormat"), "stream", innerException);
			}
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0003B21C File Offset: 0x0003941C
		internal void SavePicture(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.resourceId != 0)
			{
				throw new FormatException(SR.GetString("CursorCannotCovertToBytes"));
			}
			try
			{
				stream.Write(this.cursorData, 0, this.cursorData.Length);
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(SR.GetString("InvalidPictureFormat"));
			}
		}

		/// <summary>Displays the cursor.</summary>
		// Token: 0x060010A9 RID: 4265 RVA: 0x0003B298 File Offset: 0x00039498
		public static void Show()
		{
			UnsafeNativeMethods.ShowCursor(true);
		}

		/// <summary>Retrieves a human readable string representing this <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents this <see cref="T:System.Windows.Forms.Cursor" />.</returns>
		// Token: 0x060010AA RID: 4266 RVA: 0x0003B2A4 File Offset: 0x000394A4
		public override string ToString()
		{
			string str;
			if (!this.ownHandle)
			{
				str = TypeDescriptor.GetConverter(typeof(Cursor)).ConvertToString(this);
			}
			else
			{
				str = base.ToString();
			}
			return "[Cursor: " + str + "]";
		}

		/// <summary>Returns a value indicating whether two instances of the <see cref="T:System.Windows.Forms.Cursor" /> class are equal.</summary>
		/// <param name="left">A <see cref="T:System.Windows.Forms.Cursor" /> to compare. </param>
		/// <param name="right">A <see cref="T:System.Windows.Forms.Cursor" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if two instances of the <see cref="T:System.Windows.Forms.Cursor" /> class are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060010AB RID: 4267 RVA: 0x0003B2EA File Offset: 0x000394EA
		public static bool operator ==(Cursor left, Cursor right)
		{
			return left == null == (right == null) && (left == null || left.handle == right.handle);
		}

		/// <summary>Returns a value indicating whether two instances of the <see cref="T:System.Windows.Forms.Cursor" /> class are not equal.</summary>
		/// <param name="left">A <see cref="T:System.Windows.Forms.Cursor" /> to compare. </param>
		/// <param name="right">A <see cref="T:System.Windows.Forms.Cursor" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if two instances of the <see cref="T:System.Windows.Forms.Cursor" /> class are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060010AC RID: 4268 RVA: 0x0003B30E File Offset: 0x0003950E
		public static bool operator !=(Cursor left, Cursor right)
		{
			return !(left == right);
		}

		/// <summary>Retrieves the hash code for the current <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.Forms.Cursor" />.</returns>
		// Token: 0x060010AD RID: 4269 RVA: 0x0003B31A File Offset: 0x0003951A
		public override int GetHashCode()
		{
			return (int)this.handle;
		}

		/// <summary>Returns a value indicating whether this cursor is equal to the specified <see cref="T:System.Windows.Forms.Cursor" />.</summary>
		/// <param name="obj">The <see cref="T:System.Windows.Forms.Cursor" /> to compare. </param>
		/// <returns>
		///     <see langword="true" /> if this cursor is equal to the specified <see cref="T:System.Windows.Forms.Cursor" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060010AE RID: 4270 RVA: 0x0003B327 File Offset: 0x00039527
		public override bool Equals(object obj)
		{
			return obj is Cursor && this == (Cursor)obj;
		}

		// Token: 0x04000899 RID: 2201
		private static Size cursorSize = Size.Empty;

		// Token: 0x0400089A RID: 2202
		private byte[] cursorData;

		// Token: 0x0400089B RID: 2203
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x0400089C RID: 2204
		private bool ownHandle = true;

		// Token: 0x0400089D RID: 2205
		private int resourceId;

		// Token: 0x0400089E RID: 2206
		private object userData;
	}
}
