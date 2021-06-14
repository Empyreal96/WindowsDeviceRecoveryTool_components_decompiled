using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Represents the length of elements that explicitly support <see cref="F:System.Windows.GridUnitType.Star" /> unit types. </summary>
	// Token: 0x020000CD RID: 205
	[TypeConverter(typeof(GridLengthConverter))]
	public struct GridLength : IEquatable<GridLength>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.GridLength" /> structure using the specified absolute value in pixels. </summary>
		/// <param name="pixels">The number of device-independent pixels (96 pixels-per-inch).</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="Pixels" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NaN" />.</exception>
		// Token: 0x06000717 RID: 1815 RVA: 0x00016839 File Offset: 0x00014A39
		public GridLength(double pixels)
		{
			this = new GridLength(pixels, GridUnitType.Pixel);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.GridLength" /> structure and specifies what kind of value it holds. </summary>
		/// <param name="value">The initial value of this instance of <see cref="T:System.Windows.GridLength" />.</param>
		/// <param name="type">The <see cref="T:System.Windows.GridUnitType" /> held by this instance of <see cref="T:System.Windows.GridLength" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NaN" />.</exception>
		// Token: 0x06000718 RID: 1816 RVA: 0x00016844 File Offset: 0x00014A44
		public GridLength(double value, GridUnitType type)
		{
			if (DoubleUtil.IsNaN(value))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoNaN", new object[]
				{
					"value"
				}));
			}
			if (double.IsInfinity(value))
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterNoInfinity", new object[]
				{
					"value"
				}));
			}
			if (type != GridUnitType.Auto && type != GridUnitType.Pixel && type != GridUnitType.Star)
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterUnknownGridUnitType", new object[]
				{
					"type"
				}));
			}
			this._unitValue = ((type == GridUnitType.Auto) ? 0.0 : value);
			this._unitType = type;
		}

		/// <summary>Compares two <see cref="T:System.Windows.GridLength" /> structures for equality.</summary>
		/// <param name="gl1">The first instance of <see cref="T:System.Windows.GridLength" /> to compare.</param>
		/// <param name="gl2">The second instance of <see cref="T:System.Windows.GridLength" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances of <see cref="T:System.Windows.GridLength" /> have the same value and <see cref="T:System.Windows.GridUnitType" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000719 RID: 1817 RVA: 0x000168E2 File Offset: 0x00014AE2
		public static bool operator ==(GridLength gl1, GridLength gl2)
		{
			return gl1.GridUnitType == gl2.GridUnitType && gl1.Value == gl2.Value;
		}

		/// <summary>Compares two <see cref="T:System.Windows.GridLength" /> structures to determine if they are not equal.</summary>
		/// <param name="gl1">The first instance of <see cref="T:System.Windows.GridLength" /> to compare.</param>
		/// <param name="gl2">The second instance of <see cref="T:System.Windows.GridLength" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances of <see cref="T:System.Windows.GridLength" /> do not have the same value and <see cref="T:System.Windows.GridUnitType" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600071A RID: 1818 RVA: 0x00016906 File Offset: 0x00014B06
		public static bool operator !=(GridLength gl1, GridLength gl2)
		{
			return gl1.GridUnitType != gl2.GridUnitType || gl1.Value != gl2.Value;
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.GridLength" /> instance. </summary>
		/// <param name="oCompare">The object to compare with the current instance.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object has the same value and <see cref="T:System.Windows.GridUnitType" /> as the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600071B RID: 1819 RVA: 0x00016930 File Offset: 0x00014B30
		public override bool Equals(object oCompare)
		{
			if (oCompare is GridLength)
			{
				GridLength gl = (GridLength)oCompare;
				return this == gl;
			}
			return false;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Windows.GridLength" /> is equal to the current <see cref="T:System.Windows.GridLength" />.</summary>
		/// <param name="gridLength">The <see cref="T:System.Windows.GridLength" /> structure to compare with the current instance.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.GridLength" /> has the same value and <see cref="P:System.Windows.GridLength.GridUnitType" /> as the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600071C RID: 1820 RVA: 0x0001695A File Offset: 0x00014B5A
		public bool Equals(GridLength gridLength)
		{
			return this == gridLength;
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Windows.GridLength" />. </summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.GridLength" /> structure.</returns>
		// Token: 0x0600071D RID: 1821 RVA: 0x00016968 File Offset: 0x00014B68
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.GridLength" /> holds a value that is expressed in pixels. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.GridLength.GridUnitType" /> property is <see cref="F:System.Windows.GridUnitType.Pixel" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00016978 File Offset: 0x00014B78
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == GridUnitType.Pixel;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.GridLength" /> holds a value whose size is determined by the size properties of the content object. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.GridLength.GridUnitType" /> property is <see cref="F:System.Windows.GridUnitType.Auto" />; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00016983 File Offset: 0x00014B83
		public bool IsAuto
		{
			get
			{
				return this._unitType == GridUnitType.Auto;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.GridLength" /> holds a value that is expressed as a weighted proportion of available space. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.GridLength.GridUnitType" /> property is <see cref="F:System.Windows.GridUnitType.Star" />; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001698E File Offset: 0x00014B8E
		public bool IsStar
		{
			get
			{
				return this._unitType == GridUnitType.Star;
			}
		}

		/// <summary>Gets a <see cref="T:System.Double" /> that represents the value of the <see cref="T:System.Windows.GridLength" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the value of the current instance. </returns>
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00016999 File Offset: 0x00014B99
		public double Value
		{
			get
			{
				if (this._unitType != GridUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		/// <summary>Gets the associated <see cref="T:System.Windows.GridUnitType" /> for the <see cref="T:System.Windows.GridLength" />. </summary>
		/// <returns>One of the <see cref="T:System.Windows.GridUnitType" /> values. The default is <see cref="F:System.Windows.GridUnitType.Auto" />.</returns>
		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x000169B3 File Offset: 0x00014BB3
		public GridUnitType GridUnitType
		{
			get
			{
				return this._unitType;
			}
		}

		/// <summary>Returns a <see cref="T:System.String" /> representation of the <see cref="T:System.Windows.GridLength" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of the current <see cref="T:System.Windows.GridLength" /> structure.</returns>
		// Token: 0x06000723 RID: 1827 RVA: 0x000169BB File Offset: 0x00014BBB
		public override string ToString()
		{
			return GridLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		/// <summary>Gets an instance of <see cref="T:System.Windows.GridLength" /> that holds a value whose size is determined by the size properties of the content object.</summary>
		/// <returns>A instance of <see cref="T:System.Windows.GridLength" /> whose <see cref="P:System.Windows.GridLength.GridUnitType" /> property is set to <see cref="F:System.Windows.GridUnitType.Auto" />. </returns>
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x000169CD File Offset: 0x00014BCD
		public static GridLength Auto
		{
			get
			{
				return GridLength.s_auto;
			}
		}

		// Token: 0x04000717 RID: 1815
		private double _unitValue;

		// Token: 0x04000718 RID: 1816
		private GridUnitType _unitType;

		// Token: 0x04000719 RID: 1817
		private static readonly GridLength s_auto = new GridLength(1.0, GridUnitType.Auto);
	}
}
