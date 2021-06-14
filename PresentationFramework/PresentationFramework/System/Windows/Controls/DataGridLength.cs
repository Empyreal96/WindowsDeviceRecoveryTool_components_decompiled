using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents the lengths of elements within the <see cref="T:System.Windows.Controls.DataGrid" /> control. </summary>
	// Token: 0x020004AF RID: 1199
	[TypeConverter(typeof(DataGridLengthConverter))]
	public struct DataGridLength : IEquatable<DataGridLength>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with an absolute value in pixels.</summary>
		/// <param name="pixels">The absolute pixel value (96 pixels-per-inch) to initialize the length to.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="pixels" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.</exception>
		// Token: 0x0600490A RID: 18698 RVA: 0x0014B5BF File Offset: 0x001497BF
		public DataGridLength(double pixels)
		{
			this = new DataGridLength(pixels, DataGridLengthUnitType.Pixel);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with a specified value and unit.</summary>
		/// <param name="value">The requested size of the element.</param>
		/// <param name="type">The type that is used to determine how the size of the element is calculated.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.-or-
		///         <paramref name="type" /> is not <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />, or <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />.</exception>
		// Token: 0x0600490B RID: 18699 RVA: 0x0014B5C9 File Offset: 0x001497C9
		public DataGridLength(double value, DataGridLengthUnitType type)
		{
			this = new DataGridLength(value, type, (type == DataGridLengthUnitType.Pixel) ? value : double.NaN, (type == DataGridLengthUnitType.Pixel) ? value : double.NaN);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class with the specified value, unit, desired value, and display value.</summary>
		/// <param name="value">The requested size of the element.</param>
		/// <param name="type">The type that is used to determine how the size of the element is calculated.</param>
		/// <param name="desiredValue">The calculated size needed for the element.</param>
		/// <param name="displayValue">The allocated size for the element.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />.-or-
		///         <paramref name="type" /> is not <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />, <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />, or <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />.-or-
		///         <paramref name="desiredValue" /> is <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />.-or-
		///         <paramref name="displayValue" /> is <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />.</exception>
		// Token: 0x0600490C RID: 18700 RVA: 0x0014B5F4 File Offset: 0x001497F4
		public DataGridLength(double value, DataGridLengthUnitType type, double desiredValue, double displayValue)
		{
			if (DoubleUtil.IsNaN(value) || double.IsInfinity(value))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "value");
			}
			if (type != DataGridLengthUnitType.Auto && type != DataGridLengthUnitType.Pixel && type != DataGridLengthUnitType.Star && type != DataGridLengthUnitType.SizeToCells && type != DataGridLengthUnitType.SizeToHeader)
			{
				throw new ArgumentException(SR.Get("DataGridLength_InvalidType"), "type");
			}
			if (double.IsInfinity(desiredValue))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "desiredValue");
			}
			if (double.IsInfinity(displayValue))
			{
				throw new ArgumentException(SR.Get("DataGridLength_Infinity"), "displayValue");
			}
			this._unitValue = ((type == DataGridLengthUnitType.Auto) ? 1.0 : value);
			this._unitType = type;
			this._desiredValue = desiredValue;
			this._displayValue = displayValue;
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.DataGridLength" /> structures for equality.</summary>
		/// <param name="gl1">The first <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
		/// <param name="gl2">The second <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Controls.DataGridLength" /> instances have the same value or sizing mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600490D RID: 18701 RVA: 0x0014B6B4 File Offset: 0x001498B4
		public static bool operator ==(DataGridLength gl1, DataGridLength gl2)
		{
			return gl1.UnitType == gl2.UnitType && gl1.Value == gl2.Value && (gl1.DesiredValue == gl2.DesiredValue || (DoubleUtil.IsNaN(gl1.DesiredValue) && DoubleUtil.IsNaN(gl2.DesiredValue))) && (gl1.DisplayValue == gl2.DisplayValue || (DoubleUtil.IsNaN(gl1.DisplayValue) && DoubleUtil.IsNaN(gl2.DisplayValue)));
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.DataGridLength" /> structures to determine whether they are not equal.</summary>
		/// <param name="gl1">The first <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
		/// <param name="gl2">The second <see cref="T:System.Windows.Controls.DataGridLength" /> instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.Controls.DataGridLength" /> instances do not have the same value or sizing mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600490E RID: 18702 RVA: 0x0014B740 File Offset: 0x00149940
		public static bool operator !=(DataGridLength gl1, DataGridLength gl2)
		{
			return gl1.UnitType != gl2.UnitType || gl1.Value != gl2.Value || (gl1.DesiredValue != gl2.DesiredValue && (!DoubleUtil.IsNaN(gl1.DesiredValue) || !DoubleUtil.IsNaN(gl2.DesiredValue))) || (gl1.DisplayValue != gl2.DisplayValue && (!DoubleUtil.IsNaN(gl1.DisplayValue) || !DoubleUtil.IsNaN(gl2.DisplayValue)));
		}

		/// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
		/// <param name="obj">The object to compare to the current instance.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Controls.DataGridLength" /> with the same value or sizing mode as the current <see cref="T:System.Windows.Controls.DataGridLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600490F RID: 18703 RVA: 0x0014B7CC File Offset: 0x001499CC
		public override bool Equals(object obj)
		{
			if (obj is DataGridLength)
			{
				DataGridLength gl = (DataGridLength)obj;
				return this == gl;
			}
			return false;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Windows.Controls.DataGridLength" /> is equal to the current <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
		/// <param name="other">The <see cref="T:System.Windows.Controls.DataGridLength" /> to compare to the current instance.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is a <see cref="T:System.Windows.Controls.DataGridLength" /> with the same value or sizing mode as the current <see cref="T:System.Windows.Controls.DataGridLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004910 RID: 18704 RVA: 0x0014B7F6 File Offset: 0x001499F6
		public bool Equals(DataGridLength other)
		{
			return this == other;
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Windows.Controls.DataGridLength" />.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Windows.Controls.DataGridLength" />.</returns>
		// Token: 0x06004911 RID: 18705 RVA: 0x0014B804 File Offset: 0x00149A04
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType + (int)this._desiredValue + (int)this._displayValue);
		}

		/// <summary>Gets a value that indicates whether this instance sizes elements based on a fixed pixel value.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Pixel" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06004912 RID: 18706 RVA: 0x0014B824 File Offset: 0x00149A24
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Pixel;
			}
		}

		/// <summary>Gets a value that indicates whether this instance automatically sizes elements based on both the content of cells and the column headers.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06004913 RID: 18707 RVA: 0x0014B82F File Offset: 0x00149A2F
		public bool IsAuto
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Auto;
			}
		}

		/// <summary>Gets a value that indicates whether this instance automatically sizes elements based on a weighted proportion of available space.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Star" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06004914 RID: 18708 RVA: 0x0014B83A File Offset: 0x00149A3A
		public bool IsStar
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.Star;
			}
		}

		/// <summary>Gets a value that indicates whether this instance automatically sizes elements based on the content of the cells.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToCells" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x0014B845 File Offset: 0x00149A45
		public bool IsSizeToCells
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.SizeToCells;
			}
		}

		/// <summary>Gets a value that indicates whether this instance automatically sizes elements based on the header.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.SizeToHeader" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06004916 RID: 18710 RVA: 0x0014B850 File Offset: 0x00149A50
		public bool IsSizeToHeader
		{
			get
			{
				return this._unitType == DataGridLengthUnitType.SizeToHeader;
			}
		}

		/// <summary>Gets the absolute value of the <see cref="T:System.Windows.Controls.DataGridLength" /> in pixels.</summary>
		/// <returns>The absolute value of the <see cref="T:System.Windows.Controls.DataGridLength" /> in pixels, or 1.0 if the <see cref="P:System.Windows.Controls.DataGridLength.UnitType" /> property is set to <see cref="F:System.Windows.Controls.DataGridLengthUnitType.Auto" />.</returns>
		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x0014B85B File Offset: 0x00149A5B
		public double Value
		{
			get
			{
				if (this._unitType != DataGridLengthUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		/// <summary>Gets the type that is used to determine how the size of the element is calculated.</summary>
		/// <returns>A type that represents how size is determined.</returns>
		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x06004918 RID: 18712 RVA: 0x0014B875 File Offset: 0x00149A75
		public DataGridLengthUnitType UnitType
		{
			get
			{
				return this._unitType;
			}
		}

		/// <summary>Gets the calculated pixel value needed for the element.</summary>
		/// <returns>The number of pixels calculated for the size of the element.</returns>
		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x06004919 RID: 18713 RVA: 0x0014B87D File Offset: 0x00149A7D
		public double DesiredValue
		{
			get
			{
				return this._desiredValue;
			}
		}

		/// <summary>Gets the pixel value allocated for the size of the element.</summary>
		/// <returns>The number of pixels allocated for the element.</returns>
		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x0600491A RID: 18714 RVA: 0x0014B885 File Offset: 0x00149A85
		public double DisplayValue
		{
			get
			{
				return this._displayValue;
			}
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represent the current object.</returns>
		// Token: 0x0600491B RID: 18715 RVA: 0x0014B88D File Offset: 0x00149A8D
		public override string ToString()
		{
			return DataGridLengthConverter.ConvertToString(this, CultureInfo.InvariantCulture);
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the standard automatic sizing mode.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the standard automatic sizing mode.</returns>
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x0600491C RID: 18716 RVA: 0x0014B89F File Offset: 0x00149A9F
		public static DataGridLength Auto
		{
			get
			{
				return DataGridLength._auto;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the cell-based automatic sizing mode.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the cell-based automatic sizing mode.</returns>
		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600491D RID: 18717 RVA: 0x0014B8A6 File Offset: 0x00149AA6
		public static DataGridLength SizeToCells
		{
			get
			{
				return DataGridLength._sizeToCells;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the header-based automatic sizing mode.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.DataGridLength" /> structure that represents the header-based automatic sizing mode.</returns>
		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x0600491E RID: 18718 RVA: 0x0014B8AD File Offset: 0x00149AAD
		public static DataGridLength SizeToHeader
		{
			get
			{
				return DataGridLength._sizeToHeader;
			}
		}

		/// <summary>Converts a <see cref="T:System.Double" /> to an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
		/// <param name="value">The absolute pixel value (96 pixels-per-inch) to initialize the length to.</param>
		/// <returns>An object that represents the specified length.</returns>
		// Token: 0x0600491F RID: 18719 RVA: 0x0014B8B4 File Offset: 0x00149AB4
		public static implicit operator DataGridLength(double value)
		{
			return new DataGridLength(value);
		}

		// Token: 0x040029C2 RID: 10690
		private double _unitValue;

		// Token: 0x040029C3 RID: 10691
		private DataGridLengthUnitType _unitType;

		// Token: 0x040029C4 RID: 10692
		private double _desiredValue;

		// Token: 0x040029C5 RID: 10693
		private double _displayValue;

		// Token: 0x040029C6 RID: 10694
		private const double AutoValue = 1.0;

		// Token: 0x040029C7 RID: 10695
		private static readonly DataGridLength _auto = new DataGridLength(1.0, DataGridLengthUnitType.Auto, 0.0, 0.0);

		// Token: 0x040029C8 RID: 10696
		private static readonly DataGridLength _sizeToCells = new DataGridLength(1.0, DataGridLengthUnitType.SizeToCells, 0.0, 0.0);

		// Token: 0x040029C9 RID: 10697
		private static readonly DataGridLength _sizeToHeader = new DataGridLength(1.0, DataGridLengthUnitType.SizeToHeader, 0.0, 0.0);
	}
}
