using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Forms
{
	/// <summary>Provides methods to manage a collection of <see cref="T:System.Drawing.Image" /> objects. This class cannot be inherited.</summary>
	// Token: 0x02000284 RID: 644
	[Designer("System.Windows.Forms.Design.ImageListDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[DefaultProperty("Images")]
	[TypeConverter(typeof(ImageListConverter))]
	[DesignerSerializer("System.Windows.Forms.Design.ImageListCodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionImageList")]
	public sealed class ImageList : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ImageList" /> class with default values for <see cref="P:System.Windows.Forms.ImageList.ColorDepth" />, <see cref="P:System.Windows.Forms.ImageList.ImageSize" />, and <see cref="P:System.Windows.Forms.ImageList.TransparentColor" />.</summary>
		// Token: 0x06002676 RID: 9846 RVA: 0x000B5DB4 File Offset: 0x000B3FB4
		public ImageList()
		{
			if (!ImageList.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					ImageList.maxImageWidth = DpiHelper.LogicalToDeviceUnitsX(256);
					ImageList.maxImageHeight = DpiHelper.LogicalToDeviceUnitsY(256);
				}
				ImageList.isScalingInitialized = true;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ImageList" /> class, associating it with a container.</summary>
		/// <param name="container">An object implementing <see cref="T:System.ComponentModel.IContainer" /> to associate with this instance of <see cref="T:System.Windows.Forms.ImageList" />. </param>
		// Token: 0x06002677 RID: 9847 RVA: 0x000B5E21 File Offset: 0x000B4021
		public ImageList(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Gets the color depth of the image list.</summary>
		/// <returns>The number of available colors for the image. In the .NET Framework version 1.0, the default is <see cref="F:System.Windows.Forms.ColorDepth.Depth4Bit" />. In the .NET Framework version 1.1 or later, the default is <see cref="F:System.Windows.Forms.ColorDepth.Depth8Bit" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The color depth is not a valid <see cref="T:System.Windows.Forms.ColorDepth" /> enumeration value. </exception>
		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x000B5E3E File Offset: 0x000B403E
		// (set) Token: 0x06002679 RID: 9849 RVA: 0x000B5E48 File Offset: 0x000B4048
		[SRCategory("CatAppearance")]
		[SRDescription("ImageListColorDepthDescr")]
		public ColorDepth ColorDepth
		{
			get
			{
				return this.colorDepth;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					4,
					8,
					16,
					24,
					32
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ColorDepth));
				}
				if (this.colorDepth != value)
				{
					this.colorDepth = value;
					this.PerformRecreateHandle("ColorDepth");
				}
			}
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000B5EA5 File Offset: 0x000B40A5
		private bool ShouldSerializeColorDepth()
		{
			return this.Images.Count == 0;
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000B5EB5 File Offset: 0x000B40B5
		private void ResetColorDepth()
		{
			this.ColorDepth = ColorDepth.Depth8Bit;
		}

		/// <summary>Gets the handle of the image list object.</summary>
		/// <returns>The handle for the image list. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">Creating the handle for the <see cref="T:System.Windows.Forms.ImageList" /> failed.</exception>
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x000B5EBE File Offset: 0x000B40BE
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ImageListHandleDescr")]
		public IntPtr Handle
		{
			get
			{
				if (this.nativeImageList == null)
				{
					this.CreateHandle();
				}
				return this.nativeImageList.Handle;
			}
		}

		/// <summary>Gets a value indicating whether the underlying Win32 handle has been created.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.ImageList.Handle" /> has been created; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x000B5ED9 File Offset: 0x000B40D9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ImageListHandleCreatedDescr")]
		public bool HandleCreated
		{
			get
			{
				return this.nativeImageList != null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" /> for this image list.</summary>
		/// <returns>The collection of images.</returns>
		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x000B5EE4 File Offset: 0x000B40E4
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ImageListImagesDescr")]
		[MergableProperty(false)]
		public ImageList.ImageCollection Images
		{
			get
			{
				if (this.imageCollection == null)
				{
					this.imageCollection = new ImageList.ImageCollection(this);
				}
				return this.imageCollection;
			}
		}

		/// <summary>Gets or sets the size of the images in the image list.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> that defines the height and width, in pixels, of the images in the list. The default size is 16 by 16. The maximum size is 256 by 256.</returns>
		/// <exception cref="T:System.ArgumentException">The value assigned is equal to <see cref="P:System.Drawing.Size.IsEmpty" />.-or- The value of the height or width is less than or equal to 0.-or- The value of the height or width is greater than 256. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The new size has a dimension less than 0 or greater than 256.</exception>
		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x000B5F00 File Offset: 0x000B4100
		// (set) Token: 0x06002680 RID: 9856 RVA: 0x000B5F08 File Offset: 0x000B4108
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("ImageListSizeDescr")]
		public Size ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				if (value.IsEmpty)
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"ImageSize",
						"Size.Empty"
					}));
				}
				if (value.Width <= 0 || value.Width > ImageList.maxImageWidth)
				{
					throw new ArgumentOutOfRangeException("ImageSize", SR.GetString("InvalidBoundArgument", new object[]
					{
						"ImageSize.Width",
						value.Width.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						ImageList.maxImageWidth.ToString()
					}));
				}
				if (value.Height <= 0 || value.Height > ImageList.maxImageHeight)
				{
					throw new ArgumentOutOfRangeException("ImageSize", SR.GetString("InvalidBoundArgument", new object[]
					{
						"ImageSize.Height",
						value.Height.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture),
						ImageList.maxImageHeight.ToString()
					}));
				}
				if (this.imageSize.Width != value.Width || this.imageSize.Height != value.Height)
				{
					this.imageSize = new Size(value.Width, value.Height);
					this.PerformRecreateHandle("ImageSize");
				}
			}
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000B5EA5 File Offset: 0x000B40A5
		private bool ShouldSerializeImageSize()
		{
			return this.Images.Count == 0;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ImageListStreamer" /> associated with this image list.</summary>
		/// <returns>
		///     <see langword="null" /> if the image list is empty; otherwise, a <see cref="T:System.Windows.Forms.ImageListStreamer" /> for this <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x000B6070 File Offset: 0x000B4270
		// (set) Token: 0x06002683 RID: 9859 RVA: 0x000B6088 File Offset: 0x000B4288
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DefaultValue(null)]
		[SRDescription("ImageListImageStreamDescr")]
		public ImageListStreamer ImageStream
		{
			get
			{
				if (this.Images.Empty)
				{
					return null;
				}
				return new ImageListStreamer(this);
			}
			set
			{
				if (value != null)
				{
					ImageList.NativeImageList nativeImageList = value.GetNativeImageList();
					if (nativeImageList != null && nativeImageList != this.nativeImageList)
					{
						bool handleCreated = this.HandleCreated;
						this.DestroyHandle();
						this.originals = null;
						this.nativeImageList = new ImageList.NativeImageList(SafeNativeMethods.ImageList_Duplicate(new HandleRef(nativeImageList, nativeImageList.Handle)));
						int width;
						int height;
						if (SafeNativeMethods.ImageList_GetIconSize(new HandleRef(this, this.nativeImageList.Handle), out width, out height))
						{
							this.imageSize = new Size(width, height);
						}
						NativeMethods.IMAGEINFO imageinfo = new NativeMethods.IMAGEINFO();
						if (SafeNativeMethods.ImageList_GetImageInfo(new HandleRef(this, this.nativeImageList.Handle), 0, imageinfo))
						{
							NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
							UnsafeNativeMethods.GetObject(new HandleRef(null, imageinfo.hbmImage), Marshal.SizeOf(bitmap), bitmap);
							short bmBitsPixel = bitmap.bmBitsPixel;
							if (bmBitsPixel <= 8)
							{
								if (bmBitsPixel != 4)
								{
									if (bmBitsPixel == 8)
									{
										this.colorDepth = ColorDepth.Depth8Bit;
									}
								}
								else
								{
									this.colorDepth = ColorDepth.Depth4Bit;
								}
							}
							else if (bmBitsPixel != 16)
							{
								if (bmBitsPixel != 24)
								{
									if (bmBitsPixel == 32)
									{
										this.colorDepth = ColorDepth.Depth32Bit;
									}
								}
								else
								{
									this.colorDepth = ColorDepth.Depth24Bit;
								}
							}
							else
							{
								this.colorDepth = ColorDepth.Depth16Bit;
							}
						}
						this.Images.ResetKeys();
						if (handleCreated)
						{
							this.OnRecreateHandle(new EventArgs());
							return;
						}
					}
				}
				else
				{
					this.DestroyHandle();
					this.Images.Clear();
				}
			}
		}

		/// <summary>Gets or sets an object that contains additional data about the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains additional data about the <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x000B61D9 File Offset: 0x000B43D9
		// (set) Token: 0x06002685 RID: 9861 RVA: 0x000B61E1 File Offset: 0x000B43E1
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

		/// <summary>Gets or sets the color to treat as transparent.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Color" /> values. The default is <see langword="Transparent" />.</returns>
		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x000B61EA File Offset: 0x000B43EA
		// (set) Token: 0x06002687 RID: 9863 RVA: 0x000B61F2 File Offset: 0x000B43F2
		[SRCategory("CatBehavior")]
		[SRDescription("ImageListTransparentColorDescr")]
		public Color TransparentColor
		{
			get
			{
				return this.transparentColor;
			}
			set
			{
				this.transparentColor = value;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002688 RID: 9864 RVA: 0x000B61FC File Offset: 0x000B43FC
		private bool UseTransparentColor
		{
			get
			{
				return this.TransparentColor.A > 0;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ImageList.Handle" /> is recreated.</summary>
		// Token: 0x140001CE RID: 462
		// (add) Token: 0x06002689 RID: 9865 RVA: 0x000B621A File Offset: 0x000B441A
		// (remove) Token: 0x0600268A RID: 9866 RVA: 0x000B6233 File Offset: 0x000B4433
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SRDescription("ImageListOnRecreateHandleDescr")]
		public event EventHandler RecreateHandle
		{
			add
			{
				this.recreateHandler = (EventHandler)Delegate.Combine(this.recreateHandler, value);
			}
			remove
			{
				this.recreateHandler = (EventHandler)Delegate.Remove(this.recreateHandler, value);
			}
		}

		// Token: 0x140001CF RID: 463
		// (add) Token: 0x0600268B RID: 9867 RVA: 0x000B624C File Offset: 0x000B444C
		// (remove) Token: 0x0600268C RID: 9868 RVA: 0x000B6265 File Offset: 0x000B4465
		internal event EventHandler ChangeHandle
		{
			add
			{
				this.changeHandler = (EventHandler)Delegate.Combine(this.changeHandler, value);
			}
			remove
			{
				this.changeHandler = (EventHandler)Delegate.Remove(this.changeHandler, value);
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000B6280 File Offset: 0x000B4480
		private Bitmap CreateBitmap(ImageList.Original original, out bool ownsBitmap)
		{
			Color customTransparentColor = this.transparentColor;
			ownsBitmap = false;
			if ((original.options & ImageList.OriginalOptions.CustomTransparentColor) != ImageList.OriginalOptions.Default)
			{
				customTransparentColor = original.customTransparentColor;
			}
			Bitmap bitmap;
			if (original.image is Bitmap)
			{
				bitmap = (Bitmap)original.image;
			}
			else if (original.image is Icon)
			{
				bitmap = ((Icon)original.image).ToBitmap();
				ownsBitmap = true;
			}
			else
			{
				bitmap = new Bitmap((Image)original.image);
				ownsBitmap = true;
			}
			if (customTransparentColor.A > 0)
			{
				Bitmap bitmap2 = bitmap;
				bitmap = (Bitmap)bitmap.Clone();
				bitmap.MakeTransparent(customTransparentColor);
				if (ownsBitmap)
				{
					bitmap2.Dispose();
				}
				ownsBitmap = true;
			}
			Size size = bitmap.Size;
			if ((original.options & ImageList.OriginalOptions.ImageStrip) != ImageList.OriginalOptions.Default)
			{
				if (size.Width == 0 || size.Width % this.imageSize.Width != 0)
				{
					throw new ArgumentException(SR.GetString("ImageListStripBadWidth"), "original");
				}
				if (size.Height != this.imageSize.Height)
				{
					throw new ArgumentException(SR.GetString("ImageListImageTooShort"), "original");
				}
			}
			else if (!size.Equals(this.ImageSize))
			{
				Bitmap bitmap3 = bitmap;
				bitmap = new Bitmap(bitmap3, this.ImageSize);
				if (ownsBitmap)
				{
					bitmap3.Dispose();
				}
				ownsBitmap = true;
			}
			return bitmap;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000B63D0 File Offset: 0x000B45D0
		private int AddIconToHandle(ImageList.Original original, Icon icon)
		{
			int result;
			try
			{
				int num = SafeNativeMethods.ImageList_ReplaceIcon(new HandleRef(this, this.Handle), -1, new HandleRef(icon, icon.Handle));
				if (num == -1)
				{
					throw new InvalidOperationException(SR.GetString("ImageListAddFailed"));
				}
				result = num;
			}
			finally
			{
				if ((original.options & ImageList.OriginalOptions.OwnsImage) != ImageList.OriginalOptions.Default)
				{
					icon.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000B6438 File Offset: 0x000B4638
		private int AddToHandle(ImageList.Original original, Bitmap bitmap)
		{
			IntPtr intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
			IntPtr handle = ControlPaint.CreateHBitmapColorMask(bitmap, intPtr);
			int num = SafeNativeMethods.ImageList_Add(new HandleRef(this, this.Handle), new HandleRef(null, handle), new HandleRef(null, intPtr));
			SafeNativeMethods.DeleteObject(new HandleRef(null, handle));
			SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
			if (num == -1)
			{
				throw new InvalidOperationException(SR.GetString("ImageListAddFailed"));
			}
			return num;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000B64A4 File Offset: 0x000B46A4
		private void CreateHandle()
		{
			int num = 1;
			ColorDepth colorDepth = this.colorDepth;
			if (colorDepth <= ColorDepth.Depth8Bit)
			{
				if (colorDepth != ColorDepth.Depth4Bit)
				{
					if (colorDepth == ColorDepth.Depth8Bit)
					{
						num |= 8;
					}
				}
				else
				{
					num |= 4;
				}
			}
			else if (colorDepth != ColorDepth.Depth16Bit)
			{
				if (colorDepth != ColorDepth.Depth24Bit)
				{
					if (colorDepth == ColorDepth.Depth32Bit)
					{
						num |= 32;
					}
				}
				else
				{
					num |= 24;
				}
			}
			else
			{
				num |= 16;
			}
			IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
			try
			{
				SafeNativeMethods.InitCommonControls();
				this.nativeImageList = new ImageList.NativeImageList(SafeNativeMethods.ImageList_Create(this.imageSize.Width, this.imageSize.Height, num, 4, 4));
			}
			finally
			{
				UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
			}
			if (this.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("ImageListCreateFailed"));
			}
			SafeNativeMethods.ImageList_SetBkColor(new HandleRef(this, this.Handle), -1);
			for (int i = 0; i < this.originals.Count; i++)
			{
				ImageList.Original original = (ImageList.Original)this.originals[i];
				if (original.image is Icon)
				{
					this.AddIconToHandle(original, (Icon)original.image);
				}
				else
				{
					bool flag = false;
					Bitmap bitmap = this.CreateBitmap(original, out flag);
					this.AddToHandle(original, bitmap);
					if (flag)
					{
						bitmap.Dispose();
					}
				}
			}
			this.originals = null;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000B65F8 File Offset: 0x000B47F8
		private void DestroyHandle()
		{
			if (this.HandleCreated)
			{
				this.nativeImageList.Dispose();
				this.nativeImageList = null;
				this.originals = new ArrayList();
			}
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000B6620 File Offset: 0x000B4820
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.originals != null)
				{
					foreach (object obj in this.originals)
					{
						ImageList.Original original = (ImageList.Original)obj;
						if ((original.options & ImageList.OriginalOptions.OwnsImage) != ImageList.OriginalOptions.Default)
						{
							((IDisposable)original.image).Dispose();
						}
					}
				}
				this.DestroyHandle();
			}
			base.Dispose(disposing);
		}

		/// <summary>Draws the image indicated by the specified index on the specified <see cref="T:System.Drawing.Graphics" /> at the given location.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw on. </param>
		/// <param name="pt">The location defined by a <see cref="T:System.Drawing.Point" /> at which to draw the image. </param>
		/// <param name="index">The index of the image in the <see cref="T:System.Windows.Forms.ImageList" /> to draw. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than 0.-or- The index is greater than or equal to the count of images in the image list. </exception>
		// Token: 0x06002693 RID: 9875 RVA: 0x000B66A4 File Offset: 0x000B48A4
		public void Draw(Graphics g, Point pt, int index)
		{
			this.Draw(g, pt.X, pt.Y, index);
		}

		/// <summary>Draws the image indicated by the given index on the specified <see cref="T:System.Drawing.Graphics" /> at the specified location.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw on. </param>
		/// <param name="x">The horizontal position at which to draw the image. </param>
		/// <param name="y">The vertical position at which to draw the image. </param>
		/// <param name="index">The index of the image in the <see cref="T:System.Windows.Forms.ImageList" /> to draw. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than 0.-or- The index is greater than or equal to the count of images in the image list. </exception>
		// Token: 0x06002694 RID: 9876 RVA: 0x000B66BC File Offset: 0x000B48BC
		public void Draw(Graphics g, int x, int y, int index)
		{
			this.Draw(g, x, y, this.imageSize.Width, this.imageSize.Height, index);
		}

		/// <summary>Draws the image indicated by the given index on the specified <see cref="T:System.Drawing.Graphics" /> using the specified location and size.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> to draw on. </param>
		/// <param name="x">The horizontal position at which to draw the image. </param>
		/// <param name="y">The vertical position at which to draw the image. </param>
		/// <param name="width">The width, in pixels, of the destination image. </param>
		/// <param name="height">The height, in pixels, of the destination image. </param>
		/// <param name="index">The index of the image in the <see cref="T:System.Windows.Forms.ImageList" /> to draw. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than 0.-or- The index is greater than or equal to the count of images in the image list. </exception>
		// Token: 0x06002695 RID: 9877 RVA: 0x000B66E0 File Offset: 0x000B48E0
		public void Draw(Graphics g, int x, int y, int width, int height, int index)
		{
			if (index < 0 || index >= this.Images.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			IntPtr hdc = g.GetHdc();
			try
			{
				SafeNativeMethods.ImageList_DrawEx(new HandleRef(this, this.Handle), index, new HandleRef(g, hdc), x, y, width, height, -1, -1, 1);
			}
			finally
			{
				g.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000B6778 File Offset: 0x000B4978
		private void CopyBitmapData(BitmapData sourceData, BitmapData targetData)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < targetData.Height; i++)
			{
				IntPtr handle;
				IntPtr handle2;
				if (IntPtr.Size == 4)
				{
					handle = new IntPtr(sourceData.Scan0.ToInt32() + num);
					handle2 = new IntPtr(targetData.Scan0.ToInt32() + num2);
				}
				else
				{
					handle = new IntPtr(sourceData.Scan0.ToInt64() + (long)num);
					handle2 = new IntPtr(targetData.Scan0.ToInt64() + (long)num2);
				}
				UnsafeNativeMethods.CopyMemory(new HandleRef(this, handle2), new HandleRef(this, handle), Math.Abs(targetData.Stride));
				num += sourceData.Stride;
				num2 += targetData.Stride;
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000B6840 File Offset: 0x000B4A40
		private unsafe static bool BitmapHasAlpha(BitmapData bmpData)
		{
			if (bmpData.PixelFormat != PixelFormat.Format32bppArgb && bmpData.PixelFormat != PixelFormat.Format32bppRgb)
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < bmpData.Height; i++)
			{
				int num = i * bmpData.Stride;
				for (int j = 3; j < bmpData.Width * 4; j += 4)
				{
					byte* ptr = (byte*)((byte*)bmpData.Scan0.ToPointer() + num) + j;
					if (*ptr != 0)
					{
						return true;
					}
				}
			}
			return result;
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000B68B8 File Offset: 0x000B4AB8
		private Bitmap GetBitmap(int index)
		{
			if (index < 0 || index >= this.Images.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
				{
					"index",
					index.ToString(CultureInfo.CurrentCulture)
				}));
			}
			Bitmap bitmap = null;
			if (this.ColorDepth == ColorDepth.Depth32Bit)
			{
				NativeMethods.IMAGEINFO imageinfo = new NativeMethods.IMAGEINFO();
				if (SafeNativeMethods.ImageList_GetImageInfo(new HandleRef(this, this.Handle), index, imageinfo))
				{
					Bitmap bitmap2 = null;
					BitmapData bitmapData = null;
					BitmapData bitmapData2 = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						bitmap2 = Image.FromHbitmap(imageinfo.hbmImage);
						bitmapData = bitmap2.LockBits(new Rectangle(imageinfo.rcImage_left, imageinfo.rcImage_top, imageinfo.rcImage_right - imageinfo.rcImage_left, imageinfo.rcImage_bottom - imageinfo.rcImage_top), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
						int num = bitmapData.Stride * this.imageSize.Height * index;
						if (ImageList.BitmapHasAlpha(bitmapData))
						{
							bitmap = new Bitmap(this.imageSize.Width, this.imageSize.Height, PixelFormat.Format32bppArgb);
							bitmapData2 = bitmap.LockBits(new Rectangle(0, 0, this.imageSize.Width, this.imageSize.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
							this.CopyBitmapData(bitmapData, bitmapData2);
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
						if (bitmap2 != null)
						{
							if (bitmapData != null)
							{
								bitmap2.UnlockBits(bitmapData);
							}
							bitmap2.Dispose();
						}
						if (bitmap != null && bitmapData2 != null)
						{
							bitmap.UnlockBits(bitmapData2);
						}
					}
				}
			}
			if (bitmap == null)
			{
				bitmap = new Bitmap(this.imageSize.Width, this.imageSize.Height);
				Graphics graphics = Graphics.FromImage(bitmap);
				try
				{
					IntPtr hdc = graphics.GetHdc();
					try
					{
						SafeNativeMethods.ImageList_DrawEx(new HandleRef(this, this.Handle), index, new HandleRef(graphics, hdc), 0, 0, this.imageSize.Width, this.imageSize.Height, -1, -1, 1);
					}
					finally
					{
						graphics.ReleaseHdcInternal(hdc);
					}
				}
				finally
				{
					graphics.Dispose();
				}
			}
			bitmap.MakeTransparent(ImageList.fakeTransparencyColor);
			return bitmap;
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x000B6AD8 File Offset: 0x000B4CD8
		private void OnRecreateHandle(EventArgs eventargs)
		{
			if (this.recreateHandler != null)
			{
				this.recreateHandler(this, eventargs);
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x000B6AEF File Offset: 0x000B4CEF
		private void OnChangeHandle(EventArgs eventargs)
		{
			if (this.changeHandler != null)
			{
				this.changeHandler(this, eventargs);
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000B6B08 File Offset: 0x000B4D08
		private void PerformRecreateHandle(string reason)
		{
			if (!this.HandleCreated)
			{
				return;
			}
			if (this.originals == null || this.Images.Empty)
			{
				this.originals = new ArrayList();
			}
			if (this.originals == null)
			{
				throw new InvalidOperationException(SR.GetString("ImageListCantRecreate", new object[]
				{
					reason
				}));
			}
			this.DestroyHandle();
			this.CreateHandle();
			this.OnRecreateHandle(new EventArgs());
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000B6B77 File Offset: 0x000B4D77
		private void ResetImageSize()
		{
			this.ImageSize = ImageList.DefaultImageSize;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000B6B84 File Offset: 0x000B4D84
		private void ResetTransparentColor()
		{
			this.TransparentColor = Color.LightGray;
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000B6B94 File Offset: 0x000B4D94
		private bool ShouldSerializeTransparentColor()
		{
			return !this.TransparentColor.Equals(Color.LightGray);
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.ImageList" />.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.ImageList" />.</returns>
		// Token: 0x0600269F RID: 9887 RVA: 0x000B6BC4 File Offset: 0x000B4DC4
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Images != null)
			{
				return string.Concat(new string[]
				{
					text,
					" Images.Count: ",
					this.Images.Count.ToString(CultureInfo.CurrentCulture),
					", ImageSize: ",
					this.ImageSize.ToString()
				});
			}
			return text;
		}

		// Token: 0x0400103B RID: 4155
		private static Color fakeTransparencyColor = Color.FromArgb(13, 11, 12);

		// Token: 0x0400103C RID: 4156
		private static Size DefaultImageSize = new Size(16, 16);

		// Token: 0x0400103D RID: 4157
		private const int INITIAL_CAPACITY = 4;

		// Token: 0x0400103E RID: 4158
		private const int GROWBY = 4;

		// Token: 0x0400103F RID: 4159
		private const int MAX_DIMENSION = 256;

		// Token: 0x04001040 RID: 4160
		private static int maxImageWidth = 256;

		// Token: 0x04001041 RID: 4161
		private static int maxImageHeight = 256;

		// Token: 0x04001042 RID: 4162
		private static bool isScalingInitialized;

		// Token: 0x04001043 RID: 4163
		private ImageList.NativeImageList nativeImageList;

		// Token: 0x04001044 RID: 4164
		private ColorDepth colorDepth = ColorDepth.Depth8Bit;

		// Token: 0x04001045 RID: 4165
		private Color transparentColor = Color.Transparent;

		// Token: 0x04001046 RID: 4166
		private Size imageSize = ImageList.DefaultImageSize;

		// Token: 0x04001047 RID: 4167
		private ImageList.ImageCollection imageCollection;

		// Token: 0x04001048 RID: 4168
		private object userData;

		// Token: 0x04001049 RID: 4169
		private IList originals = new ArrayList();

		// Token: 0x0400104A RID: 4170
		private EventHandler recreateHandler;

		// Token: 0x0400104B RID: 4171
		private EventHandler changeHandler;

		// Token: 0x0400104C RID: 4172
		private bool inAddRange;

		// Token: 0x020005F0 RID: 1520
		internal class Indexer
		{
			// Token: 0x170015EF RID: 5615
			// (get) Token: 0x06005B98 RID: 23448 RVA: 0x0017EFE7 File Offset: 0x0017D1E7
			// (set) Token: 0x06005B99 RID: 23449 RVA: 0x0017EFEF File Offset: 0x0017D1EF
			public virtual ImageList ImageList
			{
				get
				{
					return this.imageList;
				}
				set
				{
					this.imageList = value;
				}
			}

			// Token: 0x170015F0 RID: 5616
			// (get) Token: 0x06005B9A RID: 23450 RVA: 0x0017EFF8 File Offset: 0x0017D1F8
			// (set) Token: 0x06005B9B RID: 23451 RVA: 0x0017F000 File Offset: 0x0017D200
			public virtual string Key
			{
				get
				{
					return this.key;
				}
				set
				{
					this.index = -1;
					this.key = ((value == null) ? string.Empty : value);
					this.useIntegerIndex = false;
				}
			}

			// Token: 0x170015F1 RID: 5617
			// (get) Token: 0x06005B9C RID: 23452 RVA: 0x0017F021 File Offset: 0x0017D221
			// (set) Token: 0x06005B9D RID: 23453 RVA: 0x0017F029 File Offset: 0x0017D229
			public virtual int Index
			{
				get
				{
					return this.index;
				}
				set
				{
					this.key = string.Empty;
					this.index = value;
					this.useIntegerIndex = true;
				}
			}

			// Token: 0x170015F2 RID: 5618
			// (get) Token: 0x06005B9E RID: 23454 RVA: 0x0017F044 File Offset: 0x0017D244
			public virtual int ActualIndex
			{
				get
				{
					if (this.useIntegerIndex)
					{
						return this.Index;
					}
					if (this.ImageList != null)
					{
						return this.ImageList.Images.IndexOfKey(this.Key);
					}
					return -1;
				}
			}

			// Token: 0x040039B9 RID: 14777
			private string key = string.Empty;

			// Token: 0x040039BA RID: 14778
			private int index = -1;

			// Token: 0x040039BB RID: 14779
			private bool useIntegerIndex = true;

			// Token: 0x040039BC RID: 14780
			private ImageList imageList;
		}

		// Token: 0x020005F1 RID: 1521
		internal class NativeImageList : IDisposable
		{
			// Token: 0x06005BA0 RID: 23456 RVA: 0x0017F096 File Offset: 0x0017D296
			internal NativeImageList(IntPtr himl)
			{
				this.himl = himl;
			}

			// Token: 0x170015F3 RID: 5619
			// (get) Token: 0x06005BA1 RID: 23457 RVA: 0x0017F0A5 File Offset: 0x0017D2A5
			internal IntPtr Handle
			{
				get
				{
					return this.himl;
				}
			}

			// Token: 0x06005BA2 RID: 23458 RVA: 0x0017F0AD File Offset: 0x0017D2AD
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06005BA3 RID: 23459 RVA: 0x0017F0BC File Offset: 0x0017D2BC
			public void Dispose(bool disposing)
			{
				if (this.himl != IntPtr.Zero)
				{
					SafeNativeMethods.ImageList_Destroy(new HandleRef(null, this.himl));
					this.himl = IntPtr.Zero;
				}
			}

			// Token: 0x06005BA4 RID: 23460 RVA: 0x0017F0F0 File Offset: 0x0017D2F0
			~NativeImageList()
			{
				this.Dispose(false);
			}

			// Token: 0x040039BD RID: 14781
			private IntPtr himl;
		}

		// Token: 0x020005F2 RID: 1522
		private class Original
		{
			// Token: 0x06005BA5 RID: 23461 RVA: 0x0017F120 File Offset: 0x0017D320
			internal Original(object image, ImageList.OriginalOptions options) : this(image, options, Color.Transparent)
			{
			}

			// Token: 0x06005BA6 RID: 23462 RVA: 0x0017F12F File Offset: 0x0017D32F
			internal Original(object image, ImageList.OriginalOptions options, int nImages) : this(image, options, Color.Transparent)
			{
				this.nImages = nImages;
			}

			// Token: 0x06005BA7 RID: 23463 RVA: 0x0017F148 File Offset: 0x0017D348
			internal Original(object image, ImageList.OriginalOptions options, Color customTransparentColor)
			{
				if (!(image is Icon) && !(image is Image))
				{
					throw new InvalidOperationException(SR.GetString("ImageListEntryType"));
				}
				this.image = image;
				this.options = options;
				this.customTransparentColor = customTransparentColor;
				ImageList.OriginalOptions originalOptions = options & ImageList.OriginalOptions.CustomTransparentColor;
			}

			// Token: 0x040039BE RID: 14782
			internal object image;

			// Token: 0x040039BF RID: 14783
			internal ImageList.OriginalOptions options;

			// Token: 0x040039C0 RID: 14784
			internal Color customTransparentColor = Color.Transparent;

			// Token: 0x040039C1 RID: 14785
			internal int nImages = 1;
		}

		// Token: 0x020005F3 RID: 1523
		[Flags]
		private enum OriginalOptions
		{
			// Token: 0x040039C3 RID: 14787
			Default = 0,
			// Token: 0x040039C4 RID: 14788
			ImageStrip = 1,
			// Token: 0x040039C5 RID: 14789
			CustomTransparentColor = 2,
			// Token: 0x040039C6 RID: 14790
			OwnsImage = 4
		}

		/// <summary>Encapsulates the collection of <see cref="T:System.Drawing.Image" /> objects in an <see cref="T:System.Windows.Forms.ImageList" />.</summary>
		// Token: 0x020005F4 RID: 1524
		[Editor("System.Windows.Forms.Design.ImageCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public sealed class ImageCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Gets the collection of keys associated with the images in the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</summary>
			/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> containing the names of the images in the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</returns>
			// Token: 0x170015F4 RID: 5620
			// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x0017F1A8 File Offset: 0x0017D3A8
			public StringCollection Keys
			{
				get
				{
					StringCollection stringCollection = new StringCollection();
					for (int i = 0; i < this.imageInfoCollection.Count; i++)
					{
						ImageList.ImageCollection.ImageInfo imageInfo = this.imageInfoCollection[i] as ImageList.ImageCollection.ImageInfo;
						if (imageInfo != null && imageInfo.Name != null && imageInfo.Name.Length != 0)
						{
							stringCollection.Add(imageInfo.Name);
						}
						else
						{
							stringCollection.Add(string.Empty);
						}
					}
					return stringCollection;
				}
			}

			// Token: 0x06005BA9 RID: 23465 RVA: 0x0017F217 File Offset: 0x0017D417
			internal ImageCollection(ImageList owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005BAA RID: 23466 RVA: 0x0017F238 File Offset: 0x0017D438
			internal void ResetKeys()
			{
				if (this.imageInfoCollection != null)
				{
					this.imageInfoCollection.Clear();
				}
				for (int i = 0; i < this.Count; i++)
				{
					this.imageInfoCollection.Add(new ImageList.ImageCollection.ImageInfo());
				}
			}

			// Token: 0x06005BAB RID: 23467 RVA: 0x0000701A File Offset: 0x0000521A
			[Conditional("DEBUG")]
			private void AssertInvariant()
			{
			}

			/// <summary>Gets the number of images currently in the list.</summary>
			/// <returns>The number of images in the list. The default is 0.</returns>
			// Token: 0x170015F5 RID: 5621
			// (get) Token: 0x06005BAC RID: 23468 RVA: 0x0017F27C File Offset: 0x0017D47C
			[Browsable(false)]
			public int Count
			{
				get
				{
					if (this.owner.HandleCreated)
					{
						return SafeNativeMethods.ImageList_GetImageCount(new HandleRef(this.owner, this.owner.Handle));
					}
					int num = 0;
					foreach (object obj in this.owner.originals)
					{
						ImageList.Original original = (ImageList.Original)obj;
						if (original != null)
						{
							num += original.nImages;
						}
					}
					return num;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>The object used to synchronize the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</returns>
			// Token: 0x170015F6 RID: 5622
			// (get) Token: 0x06005BAD RID: 23469 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x170015F7 RID: 5623
			// (get) Token: 0x06005BAE RID: 23470 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" /> has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x170015F8 RID: 5624
			// (get) Token: 0x06005BAF RID: 23471 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the list is read-only.</summary>
			/// <returns>Always <see langword="false" />.</returns>
			// Token: 0x170015F9 RID: 5625
			// (get) Token: 0x06005BB0 RID: 23472 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ImageList" /> has any images.</summary>
			/// <returns>
			///     <see langword="true" /> if there are no images in the list; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
			// Token: 0x170015FA RID: 5626
			// (get) Token: 0x06005BB1 RID: 23473 RVA: 0x0017F30C File Offset: 0x0017D50C
			public bool Empty
			{
				get
				{
					return this.Count == 0;
				}
			}

			/// <summary>Gets or sets an <see cref="T:System.Drawing.Image" /> at the specified index within the collection.</summary>
			/// <param name="index">The index of the image to get or set. </param>
			/// <returns>The image in the list specified by <paramref name="index" />. </returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than 0 or greater than or equal to <see cref="P:System.Windows.Forms.ImageList.ImageCollection.Count" />. </exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="image" /> is not a <see cref="T:System.Drawing.Bitmap" />.</exception>
			/// <exception cref="T:System.ArgumentNullException">The image to be assigned is <see langword="null" /> or not a <see cref="T:System.Drawing.Bitmap" />. </exception>
			/// <exception cref="T:System.InvalidOperationException">The image cannot be added to the list.</exception>
			// Token: 0x170015FB RID: 5627
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public Image this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return this.owner.GetBitmap(index);
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}
					if (!(value is Bitmap))
					{
						throw new ArgumentException(SR.GetString("ImageListBitmap"));
					}
					Bitmap bitmap = (Bitmap)value;
					bool flag = false;
					if (this.owner.UseTransparentColor)
					{
						bitmap = (Bitmap)bitmap.Clone();
						bitmap.MakeTransparent(this.owner.transparentColor);
						flag = true;
					}
					try
					{
						IntPtr intPtr = ControlPaint.CreateHBitmapTransparencyMask(bitmap);
						IntPtr handle = ControlPaint.CreateHBitmapColorMask(bitmap, intPtr);
						bool flag2 = SafeNativeMethods.ImageList_Replace(new HandleRef(this.owner, this.owner.Handle), index, new HandleRef(null, handle), new HandleRef(null, intPtr));
						SafeNativeMethods.DeleteObject(new HandleRef(null, handle));
						SafeNativeMethods.DeleteObject(new HandleRef(null, intPtr));
						if (!flag2)
						{
							throw new InvalidOperationException(SR.GetString("ImageListReplaceFailed"));
						}
					}
					finally
					{
						if (flag)
						{
							bitmap.Dispose();
						}
					}
				}
			}

			/// <summary>Gets or sets an image in an existing <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</summary>
			/// <param name="index">The zero-based index of the image to get or set. </param>
			/// <returns>The image in the list specified by the index.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index is less than 0 or greater than or equal to <see cref="P:System.Windows.Forms.ImageList.ImageCollection.Count" />.</exception>
			/// <exception cref="T:System.Exception">The attempt to replace the image failed.</exception>
			/// <exception cref="T:System.ArgumentNullException">The image to be assigned is <see langword="null" /> or not a bitmap.</exception>
			// Token: 0x170015FC RID: 5628
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is Image)
					{
						this[index] = (Image)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ImageListBadImage"), "value");
				}
			}

			/// <summary>Gets an <see cref="T:System.Drawing.Image" /> with the specified key from the collection.</summary>
			/// <param name="key">The name of the image to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Drawing.Image" /> with the specified key.</returns>
			// Token: 0x170015FD RID: 5629
			public Image this[string key]
			{
				get
				{
					if (key == null || key.Length == 0)
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Adds an image with the specified key to the end of the collection.</summary>
			/// <param name="key">The name of the image.</param>
			/// <param name="image">The <see cref="T:System.Drawing.Image" /> to add to the collection.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="image" /> is <see langword="null" />. </exception>
			// Token: 0x06005BB7 RID: 23479 RVA: 0x0017F510 File Offset: 0x0017D710
			public void Add(string key, Image image)
			{
				ImageList.ImageCollection.ImageInfo imageInfo = new ImageList.ImageCollection.ImageInfo();
				imageInfo.Name = key;
				ImageList.Original original = new ImageList.Original(image, ImageList.OriginalOptions.Default);
				this.Add(original, imageInfo);
			}

			/// <summary>Adds an icon with the specified key to the end of the collection. </summary>
			/// <param name="key">The name of the icon.</param>
			/// <param name="icon">The <see cref="T:System.Drawing.Icon" /> to add to the collection.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="icon" /> is <see langword="null" />. </exception>
			// Token: 0x06005BB8 RID: 23480 RVA: 0x0017F53C File Offset: 0x0017D73C
			public void Add(string key, Icon icon)
			{
				ImageList.ImageCollection.ImageInfo imageInfo = new ImageList.ImageCollection.ImageInfo();
				imageInfo.Name = key;
				ImageList.Original original = new ImageList.Original(icon, ImageList.OriginalOptions.Default);
				this.Add(original, imageInfo);
			}

			/// <summary>Adds the specified image to the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
			/// <param name="value">The image to add to the list.</param>
			/// <returns>The index of the newly added image, or -1 if the image could not be added.</returns>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Drawing.Bitmap" />.</exception>
			// Token: 0x06005BB9 RID: 23481 RVA: 0x0017F567 File Offset: 0x0017D767
			int IList.Add(object value)
			{
				if (value is Image)
				{
					this.Add((Image)value);
					return this.Count - 1;
				}
				throw new ArgumentException(SR.GetString("ImageListBadImage"), "value");
			}

			/// <summary>Adds the specified icon to the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
			/// <param name="value">An <see cref="T:System.Drawing.Icon" /> to add to the list. </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="value" /> is <see langword="null" />-or-value is not an <see cref="T:System.Drawing.Icon" />. </exception>
			// Token: 0x06005BBA RID: 23482 RVA: 0x0017F59A File Offset: 0x0017D79A
			public void Add(Icon value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.Add(new ImageList.Original(value.Clone(), ImageList.OriginalOptions.OwnsImage), null);
			}

			/// <summary>Adds the specified image to the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
			/// <param name="value">A <see cref="T:System.Drawing.Bitmap" /> of the image to add to the list. </param>
			/// <exception cref="T:System.ArgumentNullException">The image being added is <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentException">The image being added is not a <see cref="T:System.Drawing.Bitmap" />. </exception>
			// Token: 0x06005BBB RID: 23483 RVA: 0x0017F5C0 File Offset: 0x0017D7C0
			public void Add(Image value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.Default);
				this.Add(original, null);
			}

			/// <summary>Adds the specified image to the <see cref="T:System.Windows.Forms.ImageList" />, using the specified color to generate the mask.</summary>
			/// <param name="value">A <see cref="T:System.Drawing.Bitmap" /> of the image to add to the list. </param>
			/// <param name="transparentColor">The <see cref="T:System.Drawing.Color" /> to mask this image. </param>
			/// <returns>The index of the newly added image, or -1 if the image cannot be added.</returns>
			/// <exception cref="T:System.ArgumentNullException">The image being added is <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentException">The image being added is not a <see cref="T:System.Drawing.Bitmap" />. </exception>
			// Token: 0x06005BBC RID: 23484 RVA: 0x0017F5EC File Offset: 0x0017D7EC
			public int Add(Image value, Color transparentColor)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.CustomTransparentColor, transparentColor);
				return this.Add(original, null);
			}

			// Token: 0x06005BBD RID: 23485 RVA: 0x0017F618 File Offset: 0x0017D818
			private int Add(ImageList.Original original, ImageList.ImageCollection.ImageInfo imageInfo)
			{
				if (original == null || original.image == null)
				{
					throw new ArgumentNullException("original");
				}
				int result = -1;
				if (original.image is Bitmap)
				{
					if (this.owner.originals != null)
					{
						result = this.owner.originals.Add(original);
					}
					if (this.owner.HandleCreated)
					{
						bool flag = false;
						Bitmap bitmap = this.owner.CreateBitmap(original, out flag);
						result = this.owner.AddToHandle(original, bitmap);
						if (flag)
						{
							bitmap.Dispose();
						}
					}
				}
				else
				{
					if (!(original.image is Icon))
					{
						throw new ArgumentException(SR.GetString("ImageListBitmap"));
					}
					if (this.owner.originals != null)
					{
						result = this.owner.originals.Add(original);
					}
					if (this.owner.HandleCreated)
					{
						result = this.owner.AddIconToHandle(original, (Icon)original.image);
					}
				}
				if ((original.options & ImageList.OriginalOptions.ImageStrip) != ImageList.OriginalOptions.Default)
				{
					for (int i = 0; i < original.nImages; i++)
					{
						this.imageInfoCollection.Add(new ImageList.ImageCollection.ImageInfo());
					}
				}
				else
				{
					if (imageInfo == null)
					{
						imageInfo = new ImageList.ImageCollection.ImageInfo();
					}
					this.imageInfoCollection.Add(imageInfo);
				}
				if (!this.owner.inAddRange)
				{
					this.owner.OnChangeHandle(new EventArgs());
				}
				return result;
			}

			/// <summary>Adds an array of images to the collection.</summary>
			/// <param name="images">The array of <see cref="T:System.Drawing.Image" /> objects to add to the collection.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="images" /> is <see langword="null" />.</exception>
			// Token: 0x06005BBE RID: 23486 RVA: 0x0017F76C File Offset: 0x0017D96C
			public void AddRange(Image[] images)
			{
				if (images == null)
				{
					throw new ArgumentNullException("images");
				}
				this.owner.inAddRange = true;
				foreach (Image value in images)
				{
					this.Add(value);
				}
				this.owner.inAddRange = false;
				this.owner.OnChangeHandle(new EventArgs());
			}

			/// <summary>Adds an image strip for the specified image to the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
			/// <param name="value">A <see cref="T:System.Drawing.Bitmap" /> with the images to add. </param>
			/// <returns>The index of the newly added image, or -1 if the image cannot be added.</returns>
			/// <exception cref="T:System.ArgumentException">The image being added is <see langword="null" />.-or- The image being added is not a <see cref="T:System.Drawing.Bitmap" />. </exception>
			/// <exception cref="T:System.InvalidOperationException">The image cannot be added.-or- The width of image strip being added is 0, or the width is not equal to the existing image width.-or- The image strip height is not equal to existing image height. </exception>
			// Token: 0x06005BBF RID: 23487 RVA: 0x0017F7CC File Offset: 0x0017D9CC
			public int AddStrip(Image value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Width == 0 || value.Width % this.owner.ImageSize.Width != 0)
				{
					throw new ArgumentException(SR.GetString("ImageListStripBadWidth"), "value");
				}
				if (value.Height != this.owner.ImageSize.Height)
				{
					throw new ArgumentException(SR.GetString("ImageListImageTooShort"), "value");
				}
				int nImages = value.Width / this.owner.ImageSize.Width;
				ImageList.Original original = new ImageList.Original(value, ImageList.OriginalOptions.ImageStrip, nImages);
				return this.Add(original, null);
			}

			/// <summary>Removes all the images and masks from the <see cref="T:System.Windows.Forms.ImageList" />.</summary>
			// Token: 0x06005BC0 RID: 23488 RVA: 0x0017F87C File Offset: 0x0017DA7C
			public void Clear()
			{
				if (this.owner.originals != null)
				{
					this.owner.originals.Clear();
				}
				this.imageInfoCollection.Clear();
				if (this.owner.HandleCreated)
				{
					SafeNativeMethods.ImageList_Remove(new HandleRef(this.owner, this.owner.Handle), -1);
				}
				this.owner.OnChangeHandle(new EventArgs());
			}

			/// <summary>Not supported. The <see cref="M:System.Collections.IList.Contains(System.Object)" /> method indicates whether a specified object is contained in the list.</summary>
			/// <param name="image">The <see cref="T:System.Drawing.Image" /> to find in the list. </param>
			/// <returns>
			///     <see langword="true" /> if the image is found in the list; otherwise, <see langword="false" />.</returns>
			/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
			// Token: 0x06005BC1 RID: 23489 RVA: 0x0000A2AB File Offset: 0x000084AB
			[EditorBrowsable(EditorBrowsableState.Never)]
			public bool Contains(Image image)
			{
				throw new NotSupportedException();
			}

			/// <summary>Implements the <see cref="M:System.Collections.IList.Contains(System.Object)" /> method. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
			/// <param name="image">The image to locate in the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06005BC2 RID: 23490 RVA: 0x0017F8EB File Offset: 0x0017DAEB
			bool IList.Contains(object image)
			{
				return image is Image && this.Contains((Image)image);
			}

			/// <summary>Determines if the collection contains an image with the specified key.</summary>
			/// <param name="key">The key of the image to search for.</param>
			/// <returns>
			///     <see langword="true" /> to indicate an image with the specified key is contained in the collection; otherwise, <see langword="false" />. </returns>
			// Token: 0x06005BC3 RID: 23491 RVA: 0x0017F903 File Offset: 0x0017DB03
			public bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Not supported. The <see cref="M:System.Collections.IList.IndexOf(System.Object)" /> method returns the index of a specified object in the list.</summary>
			/// <param name="image">The <see cref="T:System.Drawing.Image" /> to find in the list. </param>
			/// <returns>The index of the image in the list.</returns>
			/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
			// Token: 0x06005BC4 RID: 23492 RVA: 0x0000A2AB File Offset: 0x000084AB
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int IndexOf(Image image)
			{
				throw new NotSupportedException();
			}

			/// <summary>Implements the <see cref="M:System.Collections.IList.IndexOf(System.Object)" /> method. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
			/// <param name="image">The image to find in the list.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06005BC5 RID: 23493 RVA: 0x0017F912 File Offset: 0x0017DB12
			int IList.IndexOf(object image)
			{
				if (image is Image)
				{
					return this.IndexOf((Image)image);
				}
				return -1;
			}

			/// <summary>Determines the index of the first occurrence of an image with the specified key in the collection.</summary>
			/// <param name="key">The key of the image to retrieve the index for.</param>
			/// <returns>The zero-based index of the first occurrence of an image with the specified key in the collection, if found; otherwise, -1.</returns>
			// Token: 0x06005BC6 RID: 23494 RVA: 0x0017F92C File Offset: 0x0017DB2C
			public int IndexOfKey(string key)
			{
				if (key == null || key.Length == 0)
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && this.imageInfoCollection[this.lastAccessedIndex] != null && WindowsFormsUtils.SafeCompareStrings(((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[this.lastAccessedIndex]).Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (this.imageInfoCollection[i] != null && WindowsFormsUtils.SafeCompareStrings(((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[i]).Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			/// <summary>Implements the <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" /> method. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
			/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
			/// <param name="value">The object to insert into the collection.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06005BC7 RID: 23495 RVA: 0x0000A2AB File Offset: 0x000084AB
			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005BC8 RID: 23496 RVA: 0x0017F9E1 File Offset: 0x0017DBE1
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Copies the items in this collection to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
			/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the collection. The array must have zero-based indexing.  </param>
			/// <param name="index">The zero-based index in the <see cref="T:System.Array" /> at which copying begins.  </param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="dest" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than 0.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="dest" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination array.</exception>
			/// <exception cref="T:System.InvalidCastException">The type of the <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection" /> cannot be cast automatically to the type of the destination array.</exception>
			// Token: 0x06005BC9 RID: 23497 RVA: 0x0017F9F4 File Offset: 0x0017DBF4
			void ICollection.CopyTo(Array dest, int index)
			{
				for (int i = 0; i < this.Count; i++)
				{
					dest.SetValue(this.owner.GetBitmap(i), index++);
				}
			}

			/// <summary>Returns an enumerator that can be used to iterate through the item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			// Token: 0x06005BCA RID: 23498 RVA: 0x0017FA2C File Offset: 0x0017DC2C
			public IEnumerator GetEnumerator()
			{
				Image[] array = new Image[this.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.owner.GetBitmap(i);
				}
				return array.GetEnumerator();
			}

			/// <summary>Not supported. The <see cref="M:System.Collections.IList.Remove(System.Object)" /> method removes a specified object from the list.</summary>
			/// <param name="image">The <see cref="T:System.Drawing.Image" /> to remove from the list. </param>
			/// <exception cref="T:System.NotSupportedException">This method is not supported. </exception>
			// Token: 0x06005BCB RID: 23499 RVA: 0x0000A2AB File Offset: 0x000084AB
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Remove(Image image)
			{
				throw new NotSupportedException();
			}

			/// <summary>Implements the <see cref="M:System.Collections.IList.Remove(System.Object)" />. Throws a <see cref="T:System.NotSupportedException" /> in all cases.</summary>
			/// <param name="image">The object to add to the <see cref="T:System.Windows.Forms.ImageList.ImageCollection" />.</param>
			/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
			// Token: 0x06005BCC RID: 23500 RVA: 0x0017FA68 File Offset: 0x0017DC68
			void IList.Remove(object image)
			{
				if (image is Image)
				{
					this.Remove((Image)image);
					this.owner.OnChangeHandle(new EventArgs());
				}
			}

			/// <summary>Removes an image from the list.</summary>
			/// <param name="index">The index of the image to remove. </param>
			/// <exception cref="T:System.InvalidOperationException">The image cannot be removed. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index value was less than 0.-or- The index value is greater than or equal to the <see cref="P:System.Windows.Forms.ImageList.ImageCollection.Count" /> of images. </exception>
			// Token: 0x06005BCD RID: 23501 RVA: 0x0017FA90 File Offset: 0x0017DC90
			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (!SafeNativeMethods.ImageList_Remove(new HandleRef(this.owner, this.owner.Handle), index))
				{
					throw new InvalidOperationException(SR.GetString("ImageListRemoveFailed"));
				}
				if (this.imageInfoCollection != null && index >= 0 && index < this.imageInfoCollection.Count)
				{
					this.imageInfoCollection.RemoveAt(index);
					this.owner.OnChangeHandle(new EventArgs());
				}
			}

			/// <summary>Removes the image with the specified key from the collection.</summary>
			/// <param name="key">The key of the image to remove from the collection.</param>
			// Token: 0x06005BCE RID: 23502 RVA: 0x0017FB44 File Offset: 0x0017DD44
			public void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Sets the key for an image in the collection.</summary>
			/// <param name="index">The zero-based index of an image in the collection.</param>
			/// <param name="name">The name of the image to be set as the image key.</param>
			/// <exception cref="T:System.IndexOutOfRangeException">The specified index is less than 0 or greater than or equal to <see cref="P:System.Windows.Forms.ImageList.ImageCollection.Count" />.</exception>
			// Token: 0x06005BCF RID: 23503 RVA: 0x0017FB6C File Offset: 0x0017DD6C
			public void SetKeyName(int index, string name)
			{
				if (!this.IsValidIndex(index))
				{
					throw new IndexOutOfRangeException();
				}
				if (this.imageInfoCollection[index] == null)
				{
					this.imageInfoCollection[index] = new ImageList.ImageCollection.ImageInfo();
				}
				((ImageList.ImageCollection.ImageInfo)this.imageInfoCollection[index]).Name = name;
			}

			// Token: 0x040039C7 RID: 14791
			private ImageList owner;

			// Token: 0x040039C8 RID: 14792
			private ArrayList imageInfoCollection = new ArrayList();

			// Token: 0x040039C9 RID: 14793
			private int lastAccessedIndex = -1;

			// Token: 0x02000894 RID: 2196
			internal class ImageInfo
			{
				// Token: 0x1700187A RID: 6266
				// (get) Token: 0x060070AE RID: 28846 RVA: 0x0019BECB File Offset: 0x0019A0CB
				// (set) Token: 0x060070AF RID: 28847 RVA: 0x0019BED3 File Offset: 0x0019A0D3
				public string Name
				{
					get
					{
						return this.name;
					}
					set
					{
						this.name = value;
					}
				}

				// Token: 0x040043F0 RID: 17392
				private string name;
			}
		}
	}
}
