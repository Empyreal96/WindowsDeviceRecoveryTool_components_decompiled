using System;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a 3-by-3 affine matrix that represents a geometric transform. This class cannot be inherited.</summary>
	// Token: 0x020000CA RID: 202
	public sealed class Matrix : MarshalByRefObject, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class as the identity matrix.</summary>
		// Token: 0x06000B0E RID: 2830 RVA: 0x000289E8 File Offset: 0x00026BE8
		public Matrix()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMatrix(out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeMatrix = zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class with the specified elements.</summary>
		/// <param name="m11">The value in the first row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="m12">The value in the first row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="m21">The value in the second row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="m22">The value in the second row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="dx">The value in the third row and first column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="dy">The value in the third row and second column of the new <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		// Token: 0x06000B0F RID: 2831 RVA: 0x00028A1C File Offset: 0x00026C1C
		public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateMatrix2(m11, m12, m21, m22, dx, dy, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.nativeMatrix = zero;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that represents the rectangle to be transformed. </param>
		/// <param name="plgpts">An array of three <see cref="T:System.Drawing.PointF" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners. </param>
		// Token: 0x06000B10 RID: 2832 RVA: 0x00028A58 File Offset: 0x00026C58
		public Matrix(RectangleF rect, PointF[] plgpts)
		{
			if (plgpts == null)
			{
				throw new ArgumentNullException("plgpts");
			}
			if (plgpts.Length != 3)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(plgpts);
			try
			{
				IntPtr zero = IntPtr.Zero;
				GPRECTF gprectf = new GPRECTF(rect);
				int num = SafeNativeMethods.Gdip.GdipCreateMatrix3(ref gprectf, new HandleRef(null, intPtr), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				this.nativeMatrix = zero;
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> class to the geometric transform defined by the specified rectangle and array of points.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that represents the rectangle to be transformed. </param>
		/// <param name="plgpts">An array of three <see cref="T:System.Drawing.Point" /> structures that represents the points of a parallelogram to which the upper-left, upper-right, and lower-left corners of the rectangle is to be transformed. The lower-right corner of the parallelogram is implied by the first three corners. </param>
		// Token: 0x06000B11 RID: 2833 RVA: 0x00028AD8 File Offset: 0x00026CD8
		public Matrix(Rectangle rect, Point[] plgpts)
		{
			if (plgpts == null)
			{
				throw new ArgumentNullException("plgpts");
			}
			if (plgpts.Length != 3)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(plgpts);
			try
			{
				IntPtr zero = IntPtr.Zero;
				GPRECT gprect = new GPRECT(rect);
				int num = SafeNativeMethods.Gdip.GdipCreateMatrix3I(ref gprect, new HandleRef(null, intPtr), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				this.nativeMatrix = zero;
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		// Token: 0x06000B12 RID: 2834 RVA: 0x00028B58 File Offset: 0x00026D58
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00028B67 File Offset: 0x00026D67
		private void Dispose(bool disposing)
		{
			if (this.nativeMatrix != IntPtr.Zero)
			{
				SafeNativeMethods.Gdip.GdipDeleteMatrix(new HandleRef(this, this.nativeMatrix));
				this.nativeMatrix = IntPtr.Zero;
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x06000B14 RID: 2836 RVA: 0x00028B98 File Offset: 0x00026D98
		~Matrix()
		{
			this.Dispose(false);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.Matrix" /> that this method creates.</returns>
		// Token: 0x06000B15 RID: 2837 RVA: 0x00028BC8 File Offset: 0x00026DC8
		public Matrix Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneMatrix(new HandleRef(this, this.nativeMatrix), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Matrix(zero);
		}

		/// <summary>Gets an array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>An array of floating-point values that represents the elements of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00028C00 File Offset: 0x00026E00
		public float[] Elements
		{
			get
			{
				IntPtr intPtr = Marshal.AllocHGlobal(48);
				float[] array;
				try
				{
					int num = SafeNativeMethods.Gdip.GdipGetMatrixElements(new HandleRef(this, this.nativeMatrix), intPtr);
					if (num != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num);
					}
					array = new float[6];
					Marshal.Copy(intPtr, array, 0, 6);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
		}

		/// <summary>Gets the x translation value (the dx value, or the element in the third row and first column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>The x translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00028C5C File Offset: 0x00026E5C
		public float OffsetX
		{
			get
			{
				return this.Elements[4];
			}
		}

		/// <summary>Gets the y translation value (the dy value, or the element in the third row and second column) of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <returns>The y translation value of this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00028C66 File Offset: 0x00026E66
		public float OffsetY
		{
			get
			{
				return this.Elements[5];
			}
		}

		/// <summary>Resets this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to have the elements of the identity matrix.</summary>
		// Token: 0x06000B19 RID: 2841 RVA: 0x00028C70 File Offset: 0x00026E70
		public void Reset()
		{
			int num = SafeNativeMethods.Gdip.GdipSetMatrixElements(new HandleRef(this, this.nativeMatrix), 1f, 0f, 0f, 1f, 0f, 0f);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied. </param>
		// Token: 0x06000B1A RID: 2842 RVA: 0x00028CB7 File Offset: 0x00026EB7
		public void Multiply(Matrix matrix)
		{
			this.Multiply(matrix, MatrixOrder.Prepend);
		}

		/// <summary>Multiplies this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by the matrix specified in the <paramref name="matrix" /> parameter, and in the order specified in the <paramref name="order" /> parameter.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is to be multiplied. </param>
		/// <param name="order">The <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that represents the order of the multiplication. </param>
		// Token: 0x06000B1B RID: 2843 RVA: 0x00028CC4 File Offset: 0x00026EC4
		public void Multiply(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyMatrix(new HandleRef(this, this.nativeMatrix), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified translation vector (<paramref name="offsetX" /> and <paramref name="offsetY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the translation vector.</summary>
		/// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		// Token: 0x06000B1C RID: 2844 RVA: 0x00028D08 File Offset: 0x00026F08
		public void Translate(float offsetX, float offsetY)
		{
			this.Translate(offsetX, offsetY, MatrixOrder.Prepend);
		}

		/// <summary>Applies the specified translation vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="offsetX">The x value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="offsetY">The y value by which to translate this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the translation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		// Token: 0x06000B1D RID: 2845 RVA: 0x00028D14 File Offset: 0x00026F14
		public void Translate(float offsetX, float offsetY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), offsetX, offsetY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified scale vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the scale vector.</summary>
		/// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction. </param>
		/// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction. </param>
		// Token: 0x06000B1E RID: 2846 RVA: 0x00028D40 File Offset: 0x00026F40
		public void Scale(float scaleX, float scaleY)
		{
			this.Scale(scaleX, scaleY, MatrixOrder.Prepend);
		}

		/// <summary>Applies the specified scale vector (<paramref name="scaleX" /> and <paramref name="scaleY" />) to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> using the specified order.</summary>
		/// <param name="scaleX">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the x-axis direction. </param>
		/// <param name="scaleY">The value by which to scale this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the y-axis direction. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the scale vector is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		// Token: 0x06000B1F RID: 2847 RVA: 0x00028D4C File Offset: 0x00026F4C
		public void Scale(float scaleX, float scaleY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleMatrix(new HandleRef(this, this.nativeMatrix), scaleX, scaleY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Prepend to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> a clockwise rotation, around the origin and by the specified angle.</summary>
		/// <param name="angle">The angle of the rotation, in degrees. </param>
		// Token: 0x06000B20 RID: 2848 RVA: 0x00028D78 File Offset: 0x00026F78
		public void Rotate(float angle)
		{
			this.Rotate(angle, MatrixOrder.Prepend);
		}

		/// <summary>Applies a clockwise rotation of an amount specified in the <paramref name="angle" /> parameter, around the origin (zero x and y coordinates) for this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="angle">The angle (extent) of the rotation, in degrees. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied to this <see cref="T:System.Drawing.Drawing2D.Matrix" />. </param>
		// Token: 0x06000B21 RID: 2849 RVA: 0x00028D84 File Offset: 0x00026F84
		public void Rotate(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies a clockwise rotation to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> around the point specified in the <paramref name="point" /> parameter, and by prepending the rotation.</summary>
		/// <param name="angle">The angle (extent) of the rotation, in degrees. </param>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation. </param>
		// Token: 0x06000B22 RID: 2850 RVA: 0x00028DAF File Offset: 0x00026FAF
		public void RotateAt(float angle, PointF point)
		{
			this.RotateAt(angle, point, MatrixOrder.Prepend);
		}

		/// <summary>Applies a clockwise rotation about the specified point to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="angle">The angle of the rotation, in degrees. </param>
		/// <param name="point">A <see cref="T:System.Drawing.PointF" /> that represents the center of the rotation. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the rotation is applied. </param>
		// Token: 0x06000B23 RID: 2851 RVA: 0x00028DBC File Offset: 0x00026FBC
		public void RotateAt(float angle, PointF point, MatrixOrder order)
		{
			int num;
			if (order == MatrixOrder.Prepend)
			{
				num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), point.X, point.Y, order);
				num |= SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
				num |= SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), -point.X, -point.Y, order);
			}
			else
			{
				num = SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), -point.X, -point.Y, order);
				num |= SafeNativeMethods.Gdip.GdipRotateMatrix(new HandleRef(this, this.nativeMatrix), angle, order);
				num |= SafeNativeMethods.Gdip.GdipTranslateMatrix(new HandleRef(this, this.nativeMatrix), point.X, point.Y, order);
			}
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the shear transformation.</summary>
		/// <param name="shearX">The horizontal shear factor. </param>
		/// <param name="shearY">The vertical shear factor. </param>
		// Token: 0x06000B24 RID: 2852 RVA: 0x00028E90 File Offset: 0x00027090
		public void Shear(float shearX, float shearY)
		{
			int num = SafeNativeMethods.Gdip.GdipShearMatrix(new HandleRef(this, this.nativeMatrix), shearX, shearY, MatrixOrder.Prepend);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the specified shear vector to this <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="shearX">The horizontal shear factor. </param>
		/// <param name="shearY">The vertical shear factor. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies the order (append or prepend) in which the shear is applied. </param>
		// Token: 0x06000B25 RID: 2853 RVA: 0x00028EBC File Offset: 0x000270BC
		public void Shear(float shearX, float shearY, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipShearMatrix(new HandleRef(this, this.nativeMatrix), shearX, shearY, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Inverts this <see cref="T:System.Drawing.Drawing2D.Matrix" />, if it is invertible.</summary>
		// Token: 0x06000B26 RID: 2854 RVA: 0x00028EE8 File Offset: 0x000270E8
		public void Invert()
		{
			int num = SafeNativeMethods.Gdip.GdipInvertMatrix(new HandleRef(this, this.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.PointF" /> structures that represents the points to transform. </param>
		// Token: 0x06000B27 RID: 2855 RVA: 0x00028F14 File Offset: 0x00027114
		public void TransformPoints(PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipTransformMatrixPoints(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
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

		/// <summary>Applies the geometric transform represented by this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to a specified array of points.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform. </param>
		// Token: 0x06000B28 RID: 2856 RVA: 0x00028F9C File Offset: 0x0002719C
		public void TransformPoints(Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipTransformMatrixPointsI(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
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

		/// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform. </param>
		// Token: 0x06000B29 RID: 2857 RVA: 0x00029024 File Offset: 0x00027224
		public void TransformVectors(PointF[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipVectorTransformMatrixPoints(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
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

		/// <summary>Multiplies each vector in an array by the matrix. The translation elements of this matrix (third row) are ignored.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform.</param>
		// Token: 0x06000B2A RID: 2858 RVA: 0x000290AC File Offset: 0x000272AC
		public void VectorTransformPoints(Point[] pts)
		{
			this.TransformVectors(pts);
		}

		/// <summary>Applies only the scale and rotate components of this <see cref="T:System.Drawing.Drawing2D.Matrix" /> to the specified array of points.</summary>
		/// <param name="pts">An array of <see cref="T:System.Drawing.Point" /> structures that represents the points to transform. </param>
		// Token: 0x06000B2B RID: 2859 RVA: 0x000290B8 File Offset: 0x000272B8
		public void TransformVectors(Point[] pts)
		{
			if (pts == null)
			{
				throw new ArgumentNullException("pts");
			}
			IntPtr intPtr = SafeNativeMethods.Gdip.ConvertPointToMemory(pts);
			try
			{
				int num = SafeNativeMethods.Gdip.GdipVectorTransformMatrixPointsI(new HandleRef(this, this.nativeMatrix), new HandleRef(null, intPtr), pts.Length);
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

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible.</summary>
		/// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is invertible; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00029140 File Offset: 0x00027340
		public bool IsInvertible
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsMatrixInvertible(new HandleRef(this, this.nativeMatrix), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is the identity matrix.</summary>
		/// <returns>This property is <see langword="true" /> if this <see cref="T:System.Drawing.Drawing2D.Matrix" /> is identity; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x00029170 File Offset: 0x00027370
		public bool IsIdentity
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipIsMatrixIdentity(new HandleRef(this, this.nativeMatrix), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return num2 != 0;
			}
		}

		/// <summary>Tests whether the specified object is a <see cref="T:System.Drawing.Drawing2D.Matrix" /> and is identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="obj">The object to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="obj" /> is the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> identical to this <see cref="T:System.Drawing.Drawing2D.Matrix" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000B2E RID: 2862 RVA: 0x000291A0 File Offset: 0x000273A0
		public override bool Equals(object obj)
		{
			Matrix matrix = obj as Matrix;
			if (matrix == null)
			{
				return false;
			}
			int num2;
			int num = SafeNativeMethods.Gdip.GdipIsMatrixEqual(new HandleRef(this, this.nativeMatrix), new HandleRef(matrix, matrix.nativeMatrix), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2 != 0;
		}

		/// <summary>Returns a hash code.</summary>
		/// <returns>The hash code for this <see cref="T:System.Drawing.Drawing2D.Matrix" />.</returns>
		// Token: 0x06000B2F RID: 2863 RVA: 0x000291E7 File Offset: 0x000273E7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000291EF File Offset: 0x000273EF
		internal Matrix(IntPtr nativeMatrix)
		{
			this.SetNativeMatrix(nativeMatrix);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000291FE File Offset: 0x000273FE
		internal void SetNativeMatrix(IntPtr nativeMatrix)
		{
			this.nativeMatrix = nativeMatrix;
		}

		// Token: 0x040009EC RID: 2540
		internal IntPtr nativeMatrix;
	}
}
