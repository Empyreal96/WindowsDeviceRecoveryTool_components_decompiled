using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Describes the thickness of a frame around a rectangle. Four <see cref="T:System.Double" /> values describe the <see cref="P:System.Windows.Thickness.Left" />, <see cref="P:System.Windows.Thickness.Top" />, <see cref="P:System.Windows.Thickness.Right" />, and <see cref="P:System.Windows.Thickness.Bottom" /> sides of the rectangle, respectively. </summary>
	// Token: 0x0200012D RID: 301
	[TypeConverter(typeof(ThicknessConverter))]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public struct Thickness : IEquatable<Thickness>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Thickness" /> structure that has the specified uniform length on each side. </summary>
		/// <param name="uniformLength">The uniform length applied to all four sides of the bounding rectangle.</param>
		// Token: 0x06000C3A RID: 3130 RVA: 0x0002D8AC File Offset: 0x0002BAAC
		public Thickness(double uniformLength)
		{
			this._Bottom = uniformLength;
			this._Right = uniformLength;
			this._Top = uniformLength;
			this._Left = uniformLength;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Thickness" /> structure that has specific lengths (supplied as a <see cref="T:System.Double" />) applied to each side of the rectangle. </summary>
		/// <param name="left">The thickness for the left side of the rectangle.</param>
		/// <param name="top">The thickness for the upper side of the rectangle.</param>
		/// <param name="right">The thickness for the right side of the rectangle</param>
		/// <param name="bottom">The thickness for the lower side of the rectangle.</param>
		// Token: 0x06000C3B RID: 3131 RVA: 0x0002D8DB File Offset: 0x0002BADB
		public Thickness(double left, double top, double right, double bottom)
		{
			this._Left = left;
			this._Top = top;
			this._Right = right;
			this._Bottom = bottom;
		}

		/// <summary>Compares this <see cref="T:System.Windows.Thickness" /> structure to another <see cref="T:System.Object" /> for equality.</summary>
		/// <param name="obj">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C3C RID: 3132 RVA: 0x0002D8FC File Offset: 0x0002BAFC
		public override bool Equals(object obj)
		{
			if (obj is Thickness)
			{
				Thickness t = (Thickness)obj;
				return this == t;
			}
			return false;
		}

		/// <summary>Compares this <see cref="T:System.Windows.Thickness" /> structure to another <see cref="T:System.Windows.Thickness" /> structure for equality.</summary>
		/// <param name="thickness">An instance of <see cref="T:System.Windows.Thickness" /> to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances of <see cref="T:System.Windows.Thickness" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C3D RID: 3133 RVA: 0x0002D926 File Offset: 0x0002BB26
		public bool Equals(Thickness thickness)
		{
			return this == thickness;
		}

		/// <summary>Returns the hash code of the structure.</summary>
		/// <returns>A hash code for this instance of <see cref="T:System.Windows.Thickness" />.</returns>
		// Token: 0x06000C3E RID: 3134 RVA: 0x0002D934 File Offset: 0x0002BB34
		public override int GetHashCode()
		{
			return this._Left.GetHashCode() ^ this._Top.GetHashCode() ^ this._Right.GetHashCode() ^ this._Bottom.GetHashCode();
		}

		/// <summary>Returns the string representation of the <see cref="T:System.Windows.Thickness" /> structure.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the <see cref="T:System.Windows.Thickness" /> value.</returns>
		// Token: 0x06000C3F RID: 3135 RVA: 0x0002D965 File Offset: 0x0002BB65
		public override string ToString()
		{
			return ThicknessConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0002D977 File Offset: 0x0002BB77
		internal string ToString(CultureInfo cultureInfo)
		{
			return ThicknessConverter.ToString(this, cultureInfo);
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x0002D985 File Offset: 0x0002BB85
		internal bool IsZero
		{
			get
			{
				return DoubleUtil.IsZero(this.Left) && DoubleUtil.IsZero(this.Top) && DoubleUtil.IsZero(this.Right) && DoubleUtil.IsZero(this.Bottom);
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0002D9BB File Offset: 0x0002BBBB
		internal bool IsUniform
		{
			get
			{
				return DoubleUtil.AreClose(this.Left, this.Top) && DoubleUtil.AreClose(this.Left, this.Right) && DoubleUtil.AreClose(this.Left, this.Bottom);
			}
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0002D9F8 File Offset: 0x0002BBF8
		internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			return (allowNegative || (this.Left >= 0.0 && this.Right >= 0.0 && this.Top >= 0.0 && this.Bottom >= 0.0)) && (allowNaN || (!DoubleUtil.IsNaN(this.Left) && !DoubleUtil.IsNaN(this.Right) && !DoubleUtil.IsNaN(this.Top) && !DoubleUtil.IsNaN(this.Bottom))) && (allowPositiveInfinity || (!double.IsPositiveInfinity(this.Left) && !double.IsPositiveInfinity(this.Right) && !double.IsPositiveInfinity(this.Top) && !double.IsPositiveInfinity(this.Bottom))) && (allowNegativeInfinity || (!double.IsNegativeInfinity(this.Left) && !double.IsNegativeInfinity(this.Right) && !double.IsNegativeInfinity(this.Top) && !double.IsNegativeInfinity(this.Bottom)));
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0002DAFC File Offset: 0x0002BCFC
		internal bool IsClose(Thickness thickness)
		{
			return DoubleUtil.AreClose(this.Left, thickness.Left) && DoubleUtil.AreClose(this.Top, thickness.Top) && DoubleUtil.AreClose(this.Right, thickness.Right) && DoubleUtil.AreClose(this.Bottom, thickness.Bottom);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0002DB59 File Offset: 0x0002BD59
		internal static bool AreClose(Thickness thickness0, Thickness thickness1)
		{
			return thickness0.IsClose(thickness1);
		}

		/// <summary>Compares the value of two <see cref="T:System.Windows.Thickness" /> structures for equality.</summary>
		/// <param name="t1">The first structure to compare.</param>
		/// <param name="t2">The other structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances of <see cref="T:System.Windows.Thickness" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C46 RID: 3142 RVA: 0x0002DB64 File Offset: 0x0002BD64
		public static bool operator ==(Thickness t1, Thickness t2)
		{
			return (t1._Left == t2._Left || (DoubleUtil.IsNaN(t1._Left) && DoubleUtil.IsNaN(t2._Left))) && (t1._Top == t2._Top || (DoubleUtil.IsNaN(t1._Top) && DoubleUtil.IsNaN(t2._Top))) && (t1._Right == t2._Right || (DoubleUtil.IsNaN(t1._Right) && DoubleUtil.IsNaN(t2._Right))) && (t1._Bottom == t2._Bottom || (DoubleUtil.IsNaN(t1._Bottom) && DoubleUtil.IsNaN(t2._Bottom)));
		}

		/// <summary>Compares two <see cref="T:System.Windows.Thickness" /> structures for inequality. </summary>
		/// <param name="t1">The first structure to compare.</param>
		/// <param name="t2">The other structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances of <see cref="T:System.Windows.Thickness" /> are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C47 RID: 3143 RVA: 0x0002DC18 File Offset: 0x0002BE18
		public static bool operator !=(Thickness t1, Thickness t2)
		{
			return !(t1 == t2);
		}

		/// <summary>Gets or sets the width, in pixels, of the left side of the bounding rectangle. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the width, in pixels, of the left side of the bounding rectangle for this instance of <see cref="T:System.Windows.Thickness" />. a pixel is equal to 1/96 on an inch. The default is 0.</returns>
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0002DC24 File Offset: 0x0002BE24
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0002DC2C File Offset: 0x0002BE2C
		public double Left
		{
			get
			{
				return this._Left;
			}
			set
			{
				this._Left = value;
			}
		}

		/// <summary>Gets or sets the width, in pixels, of the upper side of the bounding rectangle.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the width, in pixels, of the upper side of the bounding rectangle for this instance of <see cref="T:System.Windows.Thickness" />. A pixel is equal to 1/96 of an inch. The default is 0.</returns>
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0002DC35 File Offset: 0x0002BE35
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0002DC3D File Offset: 0x0002BE3D
		public double Top
		{
			get
			{
				return this._Top;
			}
			set
			{
				this._Top = value;
			}
		}

		/// <summary>Gets or sets the width, in pixels, of the right side of the bounding rectangle. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the width, in pixels, of the right side of the bounding rectangle for this instance of <see cref="T:System.Windows.Thickness" />. A pixel is equal to 1/96 of an inch. The default is 0.</returns>
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0002DC46 File Offset: 0x0002BE46
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0002DC4E File Offset: 0x0002BE4E
		public double Right
		{
			get
			{
				return this._Right;
			}
			set
			{
				this._Right = value;
			}
		}

		/// <summary>Gets or sets the width, in pixels, of the lower side of the bounding rectangle.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the width, in pixels, of the lower side of the bounding rectangle for this instance of <see cref="T:System.Windows.Thickness" />. A pixel is equal to 1/96 of an inch. The default is 0.</returns>
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0002DC57 File Offset: 0x0002BE57
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0002DC5F File Offset: 0x0002BE5F
		public double Bottom
		{
			get
			{
				return this._Bottom;
			}
			set
			{
				this._Bottom = value;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0002DC68 File Offset: 0x0002BE68
		internal Size Size
		{
			get
			{
				return new Size(this._Left + this._Right, this._Top + this._Bottom);
			}
		}

		// Token: 0x04000AFD RID: 2813
		private double _Left;

		// Token: 0x04000AFE RID: 2814
		private double _Top;

		// Token: 0x04000AFF RID: 2815
		private double _Right;

		// Token: 0x04000B00 RID: 2816
		private double _Bottom;
	}
}
