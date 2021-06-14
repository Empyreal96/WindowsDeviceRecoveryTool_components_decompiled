using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Represents the radii of a rectangle's corners. </summary>
	// Token: 0x020000A7 RID: 167
	[TypeConverter(typeof(CornerRadiusConverter))]
	public struct CornerRadius : IEquatable<CornerRadius>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.CornerRadius" /> class with a specified uniform radius value for every corner or the rectangle. </summary>
		/// <param name="uniformRadius">The radius value applied to every corner of the rectangle.</param>
		// Token: 0x0600036A RID: 874 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public CornerRadius(double uniformRadius)
		{
			this._bottomRight = uniformRadius;
			this._bottomLeft = uniformRadius;
			this._topRight = uniformRadius;
			this._topLeft = uniformRadius;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.CornerRadius" /> class with the specified radius values for each corner of the rectangle. </summary>
		/// <param name="topLeft">The radius of the top-left corner.</param>
		/// <param name="topRight">The radius of the top-right corner.</param>
		/// <param name="bottomRight">The radius of the bottom-right corner.</param>
		/// <param name="bottomLeft">The radius of the bottom-left corner.</param>
		// Token: 0x0600036B RID: 875 RVA: 0x00009C0F File Offset: 0x00007E0F
		public CornerRadius(double topLeft, double topRight, double bottomRight, double bottomLeft)
		{
			this._topLeft = topLeft;
			this._topRight = topRight;
			this._bottomRight = bottomRight;
			this._bottomLeft = bottomLeft;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.CornerRadius" /> and whether it contains the same corner radius values as this <see cref="T:System.Windows.CornerRadius" />. </summary>
		/// <param name="obj">The <see cref="T:System.Object" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Windows.CornerRadius" /> and contains the same corner radius values as this <see cref="T:System.Windows.CornerRadius" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600036C RID: 876 RVA: 0x00009C30 File Offset: 0x00007E30
		public override bool Equals(object obj)
		{
			if (obj is CornerRadius)
			{
				CornerRadius cr = (CornerRadius)obj;
				return this == cr;
			}
			return false;
		}

		/// <summary>Compares two <see cref="T:System.Windows.CornerRadius" /> structures for equality.</summary>
		/// <param name="cornerRadius">The <see cref="T:System.Windows.CornerRadius" /> to compare to this <see cref="T:System.Windows.CornerRadius" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="cornerRadius" /> contains the same corner radius values as this <see cref="T:System.Windows.CornerRadius" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600036D RID: 877 RVA: 0x00009C5A File Offset: 0x00007E5A
		public bool Equals(CornerRadius cornerRadius)
		{
			return this == cornerRadius;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.CornerRadius" />. </summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.CornerRadius" /> structure.</returns>
		// Token: 0x0600036E RID: 878 RVA: 0x00009C68 File Offset: 0x00007E68
		public override int GetHashCode()
		{
			return this._topLeft.GetHashCode() ^ this._topRight.GetHashCode() ^ this._bottomLeft.GetHashCode() ^ this._bottomRight.GetHashCode();
		}

		/// <summary>Returns the string representation of the <see cref="T:System.Windows.CornerRadius" />. </summary>
		/// <returns>A string representation of the <see cref="T:System.Windows.CornerRadius" />.</returns>
		// Token: 0x0600036F RID: 879 RVA: 0x00009C99 File Offset: 0x00007E99
		public override string ToString()
		{
			return CornerRadiusConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		/// <summary>Compares two <see cref="T:System.Windows.CornerRadius" /> structures for equality.</summary>
		/// <param name="cr1">The first <see cref="T:System.Windows.CornerRadius" /> to compare.</param>
		/// <param name="cr2">The second <see cref="T:System.Windows.CornerRadius" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="cr1" /> and <paramref name="cr2" /> have equal values for all corners (same values for <see cref="P:System.Windows.CornerRadius.TopLeft" />, <see cref="P:System.Windows.CornerRadius.TopRight" />, <see cref="P:System.Windows.CornerRadius.BottomLeft" />, <see cref="P:System.Windows.CornerRadius.BottomRight" />); <see langword="false" /> if <paramref name="cr1" /> and <paramref name="cr2" /> have different values for one or more corners.</returns>
		// Token: 0x06000370 RID: 880 RVA: 0x00009CAC File Offset: 0x00007EAC
		public static bool operator ==(CornerRadius cr1, CornerRadius cr2)
		{
			return (cr1._topLeft == cr2._topLeft || (DoubleUtil.IsNaN(cr1._topLeft) && DoubleUtil.IsNaN(cr2._topLeft))) && (cr1._topRight == cr2._topRight || (DoubleUtil.IsNaN(cr1._topRight) && DoubleUtil.IsNaN(cr2._topRight))) && (cr1._bottomRight == cr2._bottomRight || (DoubleUtil.IsNaN(cr1._bottomRight) && DoubleUtil.IsNaN(cr2._bottomRight))) && (cr1._bottomLeft == cr2._bottomLeft || (DoubleUtil.IsNaN(cr1._bottomLeft) && DoubleUtil.IsNaN(cr2._bottomLeft)));
		}

		/// <summary>Compares two <see cref="T:System.Windows.CornerRadius" /> structures for inequality. </summary>
		/// <param name="cr1">The first <see cref="T:System.Windows.CornerRadius" /> to compare.</param>
		/// <param name="cr2">The second <see cref="T:System.Windows.CornerRadius" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="cr1" /> and <paramref name="cr2" /> have different values for one or more corners (different values for <see cref="P:System.Windows.CornerRadius.TopLeft" />, <see cref="P:System.Windows.CornerRadius.TopRight" />, <see cref="P:System.Windows.CornerRadius.BottomLeft" />, <see cref="P:System.Windows.CornerRadius.BottomRight" />); <see langword="false" /> if <paramref name="cr1" /> and <paramref name="cr2" /> have identical corners.</returns>
		// Token: 0x06000371 RID: 881 RVA: 0x00009D60 File Offset: 0x00007F60
		public static bool operator !=(CornerRadius cr1, CornerRadius cr2)
		{
			return !(cr1 == cr2);
		}

		/// <summary>Gets or sets the radius of the top-left corner. </summary>
		/// <returns>The radius of the top-left corner. The default is 0.</returns>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00009D6C File Offset: 0x00007F6C
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00009D74 File Offset: 0x00007F74
		public double TopLeft
		{
			get
			{
				return this._topLeft;
			}
			set
			{
				this._topLeft = value;
			}
		}

		/// <summary>Gets or sets the radius of the top-right corner. </summary>
		/// <returns>The radius of the top-right corner. The default is 0.</returns>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00009D7D File Offset: 0x00007F7D
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00009D85 File Offset: 0x00007F85
		public double TopRight
		{
			get
			{
				return this._topRight;
			}
			set
			{
				this._topRight = value;
			}
		}

		/// <summary>Gets or sets the radius of the bottom-right corner. </summary>
		/// <returns>The radius of the bottom-right corner. The default is 0.</returns>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00009D8E File Offset: 0x00007F8E
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00009D96 File Offset: 0x00007F96
		public double BottomRight
		{
			get
			{
				return this._bottomRight;
			}
			set
			{
				this._bottomRight = value;
			}
		}

		/// <summary>Gets or sets the radius of the bottom-left corner. </summary>
		/// <returns>The radius of the bottom-left corner. The default is 0.</returns>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00009D9F File Offset: 0x00007F9F
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00009DA7 File Offset: 0x00007FA7
		public double BottomLeft
		{
			get
			{
				return this._bottomLeft;
			}
			set
			{
				this._bottomLeft = value;
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00009DB0 File Offset: 0x00007FB0
		internal bool IsValid(bool allowNegative, bool allowNaN, bool allowPositiveInfinity, bool allowNegativeInfinity)
		{
			return (allowNegative || (this._topLeft >= 0.0 && this._topRight >= 0.0 && this._bottomLeft >= 0.0 && this._bottomRight >= 0.0)) && (allowNaN || (!DoubleUtil.IsNaN(this._topLeft) && !DoubleUtil.IsNaN(this._topRight) && !DoubleUtil.IsNaN(this._bottomLeft) && !DoubleUtil.IsNaN(this._bottomRight))) && (allowPositiveInfinity || (!double.IsPositiveInfinity(this._topLeft) && !double.IsPositiveInfinity(this._topRight) && !double.IsPositiveInfinity(this._bottomLeft) && !double.IsPositiveInfinity(this._bottomRight))) && (allowNegativeInfinity || (!double.IsNegativeInfinity(this._topLeft) && !double.IsNegativeInfinity(this._topRight) && !double.IsNegativeInfinity(this._bottomLeft) && !double.IsNegativeInfinity(this._bottomRight)));
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00009EB3 File Offset: 0x000080B3
		internal bool IsZero
		{
			get
			{
				return DoubleUtil.IsZero(this._topLeft) && DoubleUtil.IsZero(this._topRight) && DoubleUtil.IsZero(this._bottomRight) && DoubleUtil.IsZero(this._bottomLeft);
			}
		}

		// Token: 0x040005EA RID: 1514
		private double _topLeft;

		// Token: 0x040005EB RID: 1515
		private double _topRight;

		// Token: 0x040005EC RID: 1516
		private double _bottomLeft;

		// Token: 0x040005ED RID: 1517
		private double _bottomRight;
	}
}
