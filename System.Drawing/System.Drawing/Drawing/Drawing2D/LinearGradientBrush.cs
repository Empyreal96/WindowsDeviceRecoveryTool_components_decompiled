using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Encapsulates a <see cref="T:System.Drawing.Brush" /> with a linear gradient. This class cannot be inherited.</summary>
	// Token: 0x020000C6 RID: 198
	public sealed class LinearGradientBrush : Brush
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class with the specified points and colors.</summary>
		/// <param name="point1">A <see cref="T:System.Drawing.PointF" /> structure that represents the starting point of the linear gradient. </param>
		/// <param name="point2">A <see cref="T:System.Drawing.PointF" /> structure that represents the endpoint of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color of the linear gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color of the linear gradient. </param>
		// Token: 0x06000ADF RID: 2783 RVA: 0x00027D28 File Offset: 0x00025F28
		public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrush(new GPPOINTF(point1), new GPPOINTF(point2), color1.ToArgb(), color2.ToArgb(), 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class with the specified points and colors.</summary>
		/// <param name="point1">A <see cref="T:System.Drawing.Point" /> structure that represents the starting point of the linear gradient. </param>
		/// <param name="point2">A <see cref="T:System.Drawing.Point" /> structure that represents the endpoint of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color of the linear gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color of the linear gradient. </param>
		// Token: 0x06000AE0 RID: 2784 RVA: 0x00027D78 File Offset: 0x00025F78
		public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushI(new GPPOINT(point1), new GPPOINT(point2), color1.ToArgb(), color2.ToArgb(), 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> based on a rectangle, starting and ending colors, and an orientation mode.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="linearGradientMode">A <see cref="T:System.Drawing.Drawing2D.LinearGradientMode" /> enumeration element that specifies the orientation of the gradient. The orientation determines the starting and ending points of the gradient. For example, <see langword="LinearGradientMode.ForwardDiagonal" /> specifies that the starting point is the upper-left corner of the rectangle and the ending point is the lower-right corner of the rectangle. </param>
		// Token: 0x06000AE1 RID: 2785 RVA: 0x00027DC8 File Offset: 0x00025FC8
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
		{
			if (!ClientUtils.IsEnumValid(linearGradientMode, (int)linearGradientMode, 0, 3))
			{
				throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(LinearGradientMode));
			}
			if ((double)rect.Width == 0.0 || (double)rect.Height == 0.0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			IntPtr zero = IntPtr.Zero;
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRect(ref gprectf, color1.ToArgb(), color2.ToArgb(), (int)linearGradientMode, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and orientation.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="linearGradientMode">A <see cref="T:System.Drawing.Drawing2D.LinearGradientMode" /> enumeration element that specifies the orientation of the gradient. The orientation determines the starting and ending points of the gradient. For example, <see langword="LinearGradientMode.ForwardDiagonal" /> specifies that the starting point is the upper-left corner of the rectangle and the ending point is the lower-right corner of the rectangle. </param>
		// Token: 0x06000AE2 RID: 2786 RVA: 0x00027E8C File Offset: 0x0002608C
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
		{
			if (!ClientUtils.IsEnumValid(linearGradientMode, (int)linearGradientMode, 0, 3))
			{
				throw new InvalidEnumArgumentException("linearGradientMode", (int)linearGradientMode, typeof(LinearGradientMode));
			}
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			IntPtr zero = IntPtr.Zero;
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectI(ref gprect, color1.ToArgb(), color2.ToArgb(), (int)linearGradientMode, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. </param>
		// Token: 0x06000AE3 RID: 2787 RVA: 0x00027F3A File Offset: 0x0002613A
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle) : this(rect, color1, color2, angle, false)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. </param>
		/// <param name="isAngleScaleable">Set to <see langword="true" /> to specify that the angle is affected by the transform associated with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />. </param>
		// Token: 0x06000AE4 RID: 2788 RVA: 0x00027F48 File Offset: 0x00026148
		public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable)
		{
			IntPtr zero = IntPtr.Zero;
			if ((double)rect.Width == 0.0 || (double)rect.Height == 0.0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			GPRECTF gprectf = new GPRECTF(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectWithAngle(ref gprectf, color1.ToArgb(), color2.ToArgb(), angle, isAngleScaleable, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. </param>
		// Token: 0x06000AE5 RID: 2789 RVA: 0x00027FE3 File Offset: 0x000261E3
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle) : this(rect, color1, color2, angle, false)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> class based on a rectangle, starting and ending colors, and an orientation angle.</summary>
		/// <param name="rect">A <see cref="T:System.Drawing.Rectangle" /> structure that specifies the bounds of the linear gradient. </param>
		/// <param name="color1">A <see cref="T:System.Drawing.Color" /> structure that represents the starting color for the gradient. </param>
		/// <param name="color2">A <see cref="T:System.Drawing.Color" /> structure that represents the ending color for the gradient. </param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. </param>
		/// <param name="isAngleScaleable">Set to <see langword="true" /> to specify that the angle is affected by the transform associated with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />. </param>
		// Token: 0x06000AE6 RID: 2790 RVA: 0x00027FF4 File Offset: 0x000261F4
		public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable)
		{
			IntPtr zero = IntPtr.Zero;
			if (rect.Width == 0 || rect.Height == 0)
			{
				throw new ArgumentException(SR.GetString("GdiplusInvalidRectangle", new object[]
				{
					rect.ToString()
				}));
			}
			GPRECT gprect = new GPRECT(rect);
			int num = SafeNativeMethods.Gdip.GdipCreateLineBrushFromRectWithAngleI(ref gprect, color1.ToArgb(), color2.ToArgb(), angle, isAngleScaleable, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0001BFE2 File Offset: 0x0001A1E2
		internal LinearGradientBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> this method creates, cast as an object.</returns>
		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002807C File Offset: 0x0002627C
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new LinearGradientBrush(zero);
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x000280B4 File Offset: 0x000262B4
		private void _SetLinearColors(Color color1, Color color2)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineColors(new HandleRef(this, base.NativeBrush), color1.ToArgb(), color2.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000280EC File Offset: 0x000262EC
		private Color[] _GetLinearColors()
		{
			int[] array = new int[2];
			int num = SafeNativeMethods.Gdip.GdipGetLineColors(new HandleRef(this, base.NativeBrush), array);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Color[]
			{
				Color.FromArgb(array[0]),
				Color.FromArgb(array[1])
			};
		}

		/// <summary>Gets or sets the starting and ending colors of the gradient.</summary>
		/// <returns>An array of two <see cref="T:System.Drawing.Color" /> structures that represents the starting and ending colors of the gradient.</returns>
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00028143 File Offset: 0x00026343
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x0002814B File Offset: 0x0002634B
		public Color[] LinearColors
		{
			get
			{
				return this._GetLinearColors();
			}
			set
			{
				this._SetLinearColors(value[0], value[1]);
			}
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00028164 File Offset: 0x00026364
		private RectangleF _GetRectangle()
		{
			GPRECTF gprectf = default(GPRECTF);
			int num = SafeNativeMethods.Gdip.GdipGetLineRect(new HandleRef(this, base.NativeBrush), ref gprectf);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return gprectf.ToRectangleF();
		}

		/// <summary>Gets a rectangular region that defines the starting and ending points of the gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.RectangleF" /> structure that specifies the starting and ending points of the gradient.</returns>
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x0002819E File Offset: 0x0002639E
		public RectangleF Rectangle
		{
			get
			{
				return this._GetRectangle();
			}
		}

		/// <summary>Gets or sets a value indicating whether gamma correction is enabled for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>The value is <see langword="true" /> if gamma correction is enabled for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000281A8 File Offset: 0x000263A8
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x000281D4 File Offset: 0x000263D4
		public bool GammaCorrection
		{
			get
			{
				bool result;
				int num = SafeNativeMethods.Gdip.GdipGetLineGammaCorrection(new HandleRef(this, base.NativeBrush), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return result;
			}
			set
			{
				int num = SafeNativeMethods.Gdip.GdipSetLineGammaCorrection(new HandleRef(this, base.NativeBrush), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00028200 File Offset: 0x00026400
		private Blend _GetBlend()
		{
			if (this.interpolationColorsWasSet)
			{
				return null;
			}
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLineBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			if (num <= 0)
			{
				return null;
			}
			int num3 = num;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			Blend blend;
			try
			{
				int cb = checked(4 * num3);
				intPtr = Marshal.AllocHGlobal(cb);
				intPtr2 = Marshal.AllocHGlobal(cb);
				num2 = SafeNativeMethods.Gdip.GdipGetLineBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				blend = new Blend(num3);
				Marshal.Copy(intPtr, blend.Factors, 0, num3);
				Marshal.Copy(intPtr2, blend.Positions, 0, num3);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return blend;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000282E8 File Offset: 0x000264E8
		private void _SetBlend(Blend blend)
		{
			int num = blend.Factors.Length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int cb = checked(4 * num);
				intPtr = Marshal.AllocHGlobal(cb);
				intPtr2 = Marshal.AllocHGlobal(cb);
				Marshal.Copy(blend.Factors, 0, intPtr, num);
				Marshal.Copy(blend.Positions, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipSetLineBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.Blend" /> that specifies positions and factors that define a custom falloff for the gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.Blend" /> that represents a custom falloff for the gradient.</returns>
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000283A0 File Offset: 0x000265A0
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x000283A8 File Offset: 0x000265A8
		public Blend Blend
		{
			get
			{
				return this._GetBlend();
			}
			set
			{
				this._SetBlend(value);
			}
		}

		/// <summary>Creates a gradient falloff based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the starting color and ending color are blended equally). </param>
		// Token: 0x06000AF5 RID: 2805 RVA: 0x000283B1 File Offset: 0x000265B1
		public void SetSigmaBellShape(float focus)
		{
			this.SetSigmaBellShape(focus, 1f);
		}

		/// <summary>Creates a gradient falloff based on a bell-shaped curve.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color). </param>
		/// <param name="scale">A value from 0 through 1 that specifies how fast the colors falloff from the <paramref name="focus" />. </param>
		// Token: 0x06000AF6 RID: 2806 RVA: 0x000283C0 File Offset: 0x000265C0
		public void SetSigmaBellShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineSigmaBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Creates a linear gradient with a center color and a linear falloff to a single color on both ends.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color). </param>
		// Token: 0x06000AF7 RID: 2807 RVA: 0x000283EB File Offset: 0x000265EB
		public void SetBlendTriangularShape(float focus)
		{
			this.SetBlendTriangularShape(focus, 1f);
		}

		/// <summary>Creates a linear gradient with a center color and a linear falloff to a single color on both ends.</summary>
		/// <param name="focus">A value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color). </param>
		/// <param name="scale">A value from 0 through1 that specifies how fast the colors falloff from the starting color to <paramref name="focus" /> (ending color) </param>
		// Token: 0x06000AF8 RID: 2808 RVA: 0x000283FC File Offset: 0x000265FC
		public void SetBlendTriangularShape(float focus, float scale)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineLinearBlend(new HandleRef(this, base.NativeBrush), focus, scale);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00028428 File Offset: 0x00026628
		private ColorBlend _GetInterpolationColors()
		{
			if (!this.interpolationColorsWasSet)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsColorBlendNotSet"),
					""
				}));
			}
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetLinePresetBlendCount(new HandleRef(this, base.NativeBrush), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			int num3 = num;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			ColorBlend colorBlend;
			try
			{
				int cb = checked(4 * num3);
				intPtr = Marshal.AllocHGlobal(cb);
				intPtr2 = Marshal.AllocHGlobal(cb);
				num2 = SafeNativeMethods.Gdip.GdipGetLinePresetBlend(new HandleRef(this, base.NativeBrush), intPtr, intPtr2, num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				colorBlend = new ColorBlend(num3);
				int[] array = new int[num3];
				Marshal.Copy(intPtr, array, 0, num3);
				Marshal.Copy(intPtr2, colorBlend.Positions, 0, num3);
				colorBlend.Colors = new Color[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					colorBlend.Colors[i] = Color.FromArgb(array[i]);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
			return colorBlend;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00028570 File Offset: 0x00026770
		private void _SetInterpolationColors(ColorBlend blend)
		{
			this.interpolationColorsWasSet = true;
			if (blend == null)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					""
				}));
			}
			if (blend.Colors.Length < 2)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsLength")
				}));
			}
			if (blend.Colors.Length != blend.Positions.Length)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsLengthsDiffer")
				}));
			}
			if (blend.Positions[0] != 0f)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsInvalidStartPosition")
				}));
			}
			if (blend.Positions[blend.Positions.Length - 1] != 1f)
			{
				throw new ArgumentException(SR.GetString("InterpolationColorsCommon", new object[]
				{
					SR.GetString("InterpolationColorsInvalidColorBlendObject"),
					SR.GetString("InterpolationColorsInvalidEndPosition")
				}));
			}
			int num = blend.Colors.Length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				int cb = checked(4 * num);
				intPtr = Marshal.AllocHGlobal(cb);
				intPtr2 = Marshal.AllocHGlobal(cb);
				int[] array = new int[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = blend.Colors[i].ToArgb();
				}
				Marshal.Copy(array, 0, intPtr, num);
				Marshal.Copy(blend.Positions, 0, intPtr2, num);
				int num2 = SafeNativeMethods.Gdip.GdipSetLinePresetBlend(new HandleRef(this, base.NativeBrush), new HandleRef(null, intPtr), new HandleRef(null, intPtr2), num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> that defines a multicolor linear gradient.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.ColorBlend" /> that defines a multicolor linear gradient.</returns>
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x0002878C File Offset: 0x0002698C
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00028794 File Offset: 0x00026994
		public ColorBlend InterpolationColors
		{
			get
			{
				return this._GetInterpolationColors();
			}
			set
			{
				this._SetInterpolationColors(value);
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x000287A0 File Offset: 0x000269A0
		private void _SetWrapMode(WrapMode wrapMode)
		{
			int num = SafeNativeMethods.Gdip.GdipSetLineWrapMode(new HandleRef(this, base.NativeBrush), (int)wrapMode);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000287CC File Offset: 0x000269CC
		private WrapMode _GetWrapMode()
		{
			int result = 0;
			int num = SafeNativeMethods.Gdip.GdipGetLineWrapMode(new HandleRef(this, base.NativeBrush), out result);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return (WrapMode)result;
		}

		/// <summary>Gets or sets a <see cref="T:System.Drawing.Drawing2D.WrapMode" /> enumeration that indicates the wrap mode for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Drawing2D.WrapMode" /> that specifies how fills drawn with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> are tiled.</returns>
		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x000287FA File Offset: 0x000269FA
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00028802 File Offset: 0x00026A02
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

		// Token: 0x06000B01 RID: 2817 RVA: 0x00028834 File Offset: 0x00026A34
		private void _SetTransform(Matrix matrix)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipSetLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00028878 File Offset: 0x00026A78
		private Matrix _GetTransform()
		{
			Matrix matrix = new Matrix();
			int num = SafeNativeMethods.Gdip.GdipGetLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return matrix;
		}

		/// <summary>Gets or sets a copy <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a local geometric transform for this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</summary>
		/// <returns>A copy of the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that defines a geometric transform that applies only to fills drawn with this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" />.</returns>
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x000288B4 File Offset: 0x00026AB4
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x000288BC File Offset: 0x00026ABC
		public Matrix Transform
		{
			get
			{
				return this._GetTransform();
			}
			set
			{
				this._SetTransform(value);
			}
		}

		/// <summary>Resets the <see cref="P:System.Drawing.Drawing2D.LinearGradientBrush.Transform" /> property to identity.</summary>
		// Token: 0x06000B05 RID: 2821 RVA: 0x000288C8 File Offset: 0x00026AC8
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetLineTransform(new HandleRef(this, base.NativeBrush));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the local geometric transform of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> by prepending the specified <see cref="T:System.Drawing.Drawing2D.Matrix" />.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to multiply the geometric transform. </param>
		// Token: 0x06000B06 RID: 2822 RVA: 0x000288F1 File Offset: 0x00026AF1
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		/// <summary>Multiplies the <see cref="T:System.Drawing.Drawing2D.Matrix" /> that represents the local geometric transform of this <see cref="T:System.Drawing.Drawing2D.LinearGradientBrush" /> by the specified <see cref="T:System.Drawing.Drawing2D.Matrix" /> in the specified order.</summary>
		/// <param name="matrix">The <see cref="T:System.Drawing.Drawing2D.Matrix" /> by which to multiply the geometric transform. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies in which order to multiply the two matrices. </param>
		// Token: 0x06000B07 RID: 2823 RVA: 0x000288FC File Offset: 0x00026AFC
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			if (matrix == null)
			{
				throw new ArgumentNullException("matrix");
			}
			int num = SafeNativeMethods.Gdip.GdipMultiplyLineTransform(new HandleRef(this, base.NativeBrush), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Translates the local geometric transform by the specified dimensions. This method prepends the translation to the transform.</summary>
		/// <param name="dx">The value of the translation in x. </param>
		/// <param name="dy">The value of the translation in y. </param>
		// Token: 0x06000B08 RID: 2824 RVA: 0x00028940 File Offset: 0x00026B40
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		/// <summary>Translates the local geometric transform by the specified dimensions in the specified order.</summary>
		/// <param name="dx">The value of the translation in x. </param>
		/// <param name="dy">The value of the translation in y. </param>
		/// <param name="order">The order (prepend or append) in which to apply the translation. </param>
		// Token: 0x06000B09 RID: 2825 RVA: 0x0002894C File Offset: 0x00026B4C
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslateLineTransform(new HandleRef(this, base.NativeBrush), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Scales the local geometric transform by the specified amounts. This method prepends the scaling matrix to the transform.</summary>
		/// <param name="sx">The amount by which to scale the transform in the x-axis direction. </param>
		/// <param name="sy">The amount by which to scale the transform in the y-axis direction. </param>
		// Token: 0x06000B0A RID: 2826 RVA: 0x00028978 File Offset: 0x00026B78
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		/// <summary>Scales the local geometric transform by the specified amounts in the specified order.</summary>
		/// <param name="sx">The amount by which to scale the transform in the x-axis direction. </param>
		/// <param name="sy">The amount by which to scale the transform in the y-axis direction. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the scaling matrix. </param>
		// Token: 0x06000B0B RID: 2827 RVA: 0x00028984 File Offset: 0x00026B84
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScaleLineTransform(new HandleRef(this, base.NativeBrush), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		/// <summary>Rotates the local geometric transform by the specified amount. This method prepends the rotation to the transform.</summary>
		/// <param name="angle">The angle of rotation. </param>
		// Token: 0x06000B0C RID: 2828 RVA: 0x000289B0 File Offset: 0x00026BB0
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		/// <summary>Rotates the local geometric transform by the specified amount in the specified order.</summary>
		/// <param name="angle">The angle of rotation. </param>
		/// <param name="order">A <see cref="T:System.Drawing.Drawing2D.MatrixOrder" /> that specifies whether to append or prepend the rotation matrix. </param>
		// Token: 0x06000B0D RID: 2829 RVA: 0x000289BC File Offset: 0x00026BBC
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotateLineTransform(new HandleRef(this, base.NativeBrush), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x040009D5 RID: 2517
		private bool interpolationColorsWasSet;
	}
}
