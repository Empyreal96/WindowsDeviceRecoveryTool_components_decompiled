using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	/// <summary>Defines a rectangular brush with a hatch style, a foreground color, and a background color. This class cannot be inherited.</summary>
	// Token: 0x020000C3 RID: 195
	public sealed class HatchBrush : Brush
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> class with the specified <see cref="T:System.Drawing.Drawing2D.HatchStyle" /> enumeration and foreground color.</summary>
		/// <param name="hatchstyle">One of the <see cref="T:System.Drawing.Drawing2D.HatchStyle" /> values that represents the pattern drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />. </param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> structure that represents the color of lines drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />. </param>
		// Token: 0x06000AD8 RID: 2776 RVA: 0x00027BFF File Offset: 0x00025DFF
		public HatchBrush(HatchStyle hatchstyle, Color foreColor) : this(hatchstyle, foreColor, Color.FromArgb(-16777216))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> class with the specified <see cref="T:System.Drawing.Drawing2D.HatchStyle" /> enumeration, foreground color, and background color.</summary>
		/// <param name="hatchstyle">One of the <see cref="T:System.Drawing.Drawing2D.HatchStyle" /> values that represents the pattern drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />. </param>
		/// <param name="foreColor">The <see cref="T:System.Drawing.Color" /> structure that represents the color of lines drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />. </param>
		/// <param name="backColor">The <see cref="T:System.Drawing.Color" /> structure that represents the color of spaces between the lines drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />. </param>
		// Token: 0x06000AD9 RID: 2777 RVA: 0x00027C14 File Offset: 0x00025E14
		public HatchBrush(HatchStyle hatchstyle, Color foreColor, Color backColor)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHatchBrush((int)hatchstyle, foreColor.ToArgb(), backColor.ToArgb(), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0001BFE2 File Offset: 0x0001A1E2
		internal HatchBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		/// <summary>Creates an exact copy of this <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> object.</summary>
		/// <returns>The <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> this method creates, cast as an object.</returns>
		// Token: 0x06000ADB RID: 2779 RVA: 0x00027C58 File Offset: 0x00025E58
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new HatchBrush(zero);
		}

		/// <summary>Gets the hatch style of this <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> object.</summary>
		/// <returns>One of the <see cref="T:System.Drawing.Drawing2D.HatchStyle" /> values that represents the pattern of this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />.</returns>
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00027C90 File Offset: 0x00025E90
		public HatchStyle HatchStyle
		{
			get
			{
				int result = 0;
				int num = SafeNativeMethods.Gdip.GdipGetHatchStyle(new HandleRef(this, base.NativeBrush), out result);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return (HatchStyle)result;
			}
		}

		/// <summary>Gets the color of hatch lines drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure that represents the foreground color for this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />.</returns>
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00027CC0 File Offset: 0x00025EC0
		public Color ForegroundColor
		{
			get
			{
				int argb;
				int num = SafeNativeMethods.Gdip.GdipGetHatchForegroundColor(new HandleRef(this, base.NativeBrush), out argb);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Color.FromArgb(argb);
			}
		}

		/// <summary>Gets the color of spaces between the hatch lines drawn by this <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure that represents the background color for this <see cref="T:System.Drawing.Drawing2D.HatchBrush" />.</returns>
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00027CF4 File Offset: 0x00025EF4
		public Color BackgroundColor
		{
			get
			{
				int argb;
				int num = SafeNativeMethods.Gdip.GdipGetHatchBackgroundColor(new HandleRef(this, base.NativeBrush), out argb);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Color.FromArgb(argb);
			}
		}
	}
}
