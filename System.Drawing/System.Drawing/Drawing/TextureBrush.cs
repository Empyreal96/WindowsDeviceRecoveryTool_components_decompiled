using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>Each property of the <see cref="T:System.Drawing.TextureBrush" /> class is a <see cref="T:System.Drawing.Brush" /> object that uses an image to fill the interior of a shape. This class cannot be inherited.</summary>
	// Token: 0x0200004B RID: 75
	public sealed class TextureBrush : Brush
	{
		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image.</summary>
		/// <param name="bitmap">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		// Token: 0x060006D4 RID: 1748 RVA: 0x0001BD2C File Offset: 0x00019F2C
		public TextureBrush(Image bitmap) : this(bitmap, WrapMode.Tile)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image and wrap mode.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="wrapMode">A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that specifies how this <see cref="T:System.Drawing.TextureBrush" /> object is tiled. </param>
		// Token: 0x060006D5 RID: 1749 RVA: 0x0001BD38 File Offset: 0x00019F38
		public TextureBrush(Image image, WrapMode wrapMode)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture(new HandleRef(image, image.nativeImage), (int)wrapMode, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image, wrap mode, and bounding rectangle.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="wrapMode">A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that specifies how this <see cref="T:System.Drawing.TextureBrush" /> object is tiled. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006D6 RID: 1750 RVA: 0x0001BDAC File Offset: 0x00019FAC
		public TextureBrush(Image image, WrapMode wrapMode, RectangleF dstRect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture2(new HandleRef(image, image.nativeImage), (int)wrapMode, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image, wrap mode, and bounding rectangle.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="wrapMode">A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that specifies how this <see cref="T:System.Drawing.TextureBrush" /> object is tiled. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006D7 RID: 1751 RVA: 0x0001BE3C File Offset: 0x0001A03C
		public TextureBrush(Image image, WrapMode wrapMode, Rectangle dstRect)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			if (!ClientUtils.IsEnumValid(wrapMode, (int)wrapMode, 0, 4))
			{
				throw new InvalidEnumArgumentException("wrapMode", (int)wrapMode, typeof(WrapMode));
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTexture2I(new HandleRef(image, image.nativeImage), (int)wrapMode, dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image and bounding rectangle.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006D8 RID: 1752 RVA: 0x0001BECB File Offset: 0x0001A0CB
		public TextureBrush(Image image, RectangleF dstRect) : this(image, dstRect, null)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image, bounding rectangle, and image attributes.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		/// <param name="imageAttr">An <see cref="T:System.Drawing.Imaging.ImageAttributes" /> object that contains additional information about the image used by this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006D9 RID: 1753 RVA: 0x0001BED8 File Offset: 0x0001A0D8
		public TextureBrush(Image image, RectangleF dstRect, ImageAttributes imageAttr)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTextureIA(new HandleRef(image, image.nativeImage), new HandleRef(imageAttr, (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes), dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image and bounding rectangle.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006DA RID: 1754 RVA: 0x0001BF56 File Offset: 0x0001A156
		public TextureBrush(Image image, Rectangle dstRect) : this(image, dstRect, null)
		{
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.TextureBrush" /> object that uses the specified image, bounding rectangle, and image attributes.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> object with which this <see cref="T:System.Drawing.TextureBrush" /> object fills interiors. </param>
		/// <param name="dstRect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the bounding rectangle for this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		/// <param name="imageAttr">An <see cref="T:System.Drawing.Imaging.ImageAttributes" /> object that contains additional information about the image used by this <see cref="T:System.Drawing.TextureBrush" /> object. </param>
		// Token: 0x060006DB RID: 1755 RVA: 0x0001BF64 File Offset: 0x0001A164
		public TextureBrush(Image image, Rectangle dstRect, ImageAttributes imageAttr)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateTextureIAI(new HandleRef(image, image.nativeImage), new HandleRef(imageAttr, (imageAttr == null) ? IntPtr.Zero : imageAttr.nativeImageAttributes), dstRect.X, dstRect.Y, dstRect.Width, dstRect.Height, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001BFE2 File Offset: 0x0001A1E2
		internal TextureBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.TextureBrush" /> object.</summary>
		/// <returns>The <see cref="T:System.Drawing.TextureBrush" /> object this method creates, cast as an <see cref="T:System.Object" /> object.</returns>
		// Token: 0x060006DD RID: 1757 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new TextureBrush(zero);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001C02C File Offset: 0x0001A22C
		private void _SetTransform(Matrix matrix)
		{
			int num = SafeNativeMethods.Gdip.GdipSetTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001C064 File Offset: 0x0001A264
		private Matrix _GetTransform()
		{
			Matrix matrix = new Matrix();
			int num = SafeNativeMethods.Gdip.GdipGetTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return matrix;
		}

		/// <summary>Gets or sets a copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> object that defines a local geometric transformation for the image associated with this <see cref="T:System.Drawing.TextureBrush" /> object.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> object that defines a geometric transformation that applies only to fills drawn by using this <see cref="T:System.Drawing.TextureBrush" /> object.</returns>
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0001C0A8 File Offset: 0x0001A2A8
		public Matrix Transform
		{
			get
			{
				return this._GetTransform();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._SetTransform(value);
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001C0C0 File Offset: 0x0001A2C0
		private void _SetWrapMode(WrapMode wrapMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetTextureWrapMode(new HandleRef(this, base.NativeBrush), (int)wrapMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001C0EC File Offset: 0x0001A2EC
		private WrapMode _GetWrapMode()
		{
			int result = 0;
			int num = SafeNativeMethods.Gdip.GdipGetTextureWrapMode(new HandleRef(this, base.NativeBrush), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return (WrapMode)result;
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that indicates the wrap mode for this <see cref="T:System.Drawing.TextureBrush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that specifies how fills drawn by using this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> object are tiled.</returns>
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001C11A File Offset: 0x0001A31A
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001C122 File Offset: 0x0001A322
		public WrapMode WrapMode
		{
			get
			{
				return this._GetWrapMode();
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(WrapMode));
				}
				this._SetWrapMode(value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Image" /> object associated with this <see cref="T:System.Drawing.TextureBrush" /> object.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> object that represents the image with which this <see cref="T:System.Drawing.TextureBrush" /> object fills shapes.</returns>
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001C154 File Offset: 0x0001A354
		public Image Image
		{
			get
			{
				IntPtr nativeImage;
				int num = SafeNativeMethods.Gdip.GdipGetTextureImage(new HandleRef(this, base.NativeBrush), out nativeImage);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Image.CreateImageObject(nativeImage);
			}
		}

		/// <summary>Resets the <see langword="Transform" /> property of this <see cref="T:System.Drawing.TextureBrush" /> object to identity.</summary>
		// Token: 0x060006E7 RID: 1767 RVA: 0x0001C188 File Offset: 0x0001A388
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetTextureTransform(new HandleRef(this, base.NativeBrush));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> object that represents the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> object by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> object.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> object by which to multiply the geometric transformation. </param>
		// Token: 0x060006E8 RID: 1768 RVA: 0x0001C1B1 File Offset: 0x0001A3B1
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> object that represents the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> object in the specified order.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> object by which to multiply the geometric transformation. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies the order in which to multiply the two matrices. </param>
		// Token: 0x060006E9 RID: 1769 RVA: 0x0001C1BC File Offset: 0x0001A3BC
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyTextureTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Translates the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified dimensions. This method prepends the translation to the transformation.</summary>
		/// <param name="dx">The dimension by which to translate the transformation in the x direction. </param>
		/// <param name="dy">The dimension by which to translate the transformation in the y direction. </param>
		// Token: 0x060006EA RID: 1770 RVA: 0x0001C200 File Offset: 0x0001A400
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		/// <summary>Translates the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified dimensions in the specified order.</summary>
		/// <param name="dx">The dimension by which to translate the transformation in the x direction. </param>
		/// <param name="dy">The dimension by which to translate the transformation in the y direction. </param>
		/// <param name="order">The order (prepend or append) in which to apply the translation. </param>
		// Token: 0x060006EB RID: 1771 RVA: 0x0001C20C File Offset: 0x0001A40C
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateTextureTransform(new HandleRef(this, base.NativeBrush), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Scales the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified amounts. This method prepends the scaling matrix to the transformation.</summary>
		/// <param name="sx">The amount by which to scale the transformation in the x direction. </param>
		/// <param name="sy">The amount by which to scale the transformation in the y direction. </param>
		// Token: 0x060006EC RID: 1772 RVA: 0x0001C238 File Offset: 0x0001A438
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		/// <summary>Scales the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified amounts in the specified order.</summary>
		/// <param name="sx">The amount by which to scale the transformation in the x direction. </param>
		/// <param name="sy">The amount by which to scale the transformation in the y direction. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies whether to append or prepend the scaling matrix. </param>
		// Token: 0x060006ED RID: 1773 RVA: 0x0001C244 File Offset: 0x0001A444
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleTextureTransform(new HandleRef(this, base.NativeBrush), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Rotates the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified amount. This method prepends the rotation to the transformation.</summary>
		/// <param name="angle">The angle of rotation. </param>
		// Token: 0x060006EE RID: 1774 RVA: 0x0001C270 File Offset: 0x0001A470
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		/// <summary>Rotates the local geometric transformation of this <see cref="T:System.Drawing.TextureBrush" /> object by the specified amount in the specified order.</summary>
		/// <param name="angle">The angle of rotation. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> enumeration that specifies whether to append or prepend the rotation matrix. </param>
		// Token: 0x060006EF RID: 1775 RVA: 0x0001C27C File Offset: 0x0001A47C
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateTextureTransform(new HandleRef(this, base.NativeBrush), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}
	}
}
