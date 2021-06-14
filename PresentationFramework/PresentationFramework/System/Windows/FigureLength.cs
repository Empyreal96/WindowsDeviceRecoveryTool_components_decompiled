using System;
using System.ComponentModel;
using System.Globalization;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Describes the height or width of a <see cref="T:System.Windows.Documents.Figure" />.</summary>
	// Token: 0x020000BC RID: 188
	[TypeConverter(typeof(FigureLengthConverter))]
	public struct FigureLength : IEquatable<FigureLength>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FigureLength" /> class with the specified number of pixels in length.</summary>
		/// <param name="pixels">The number of device-independent pixels (96 pixels-per-inch) that make up the length.</param>
		// Token: 0x060003FD RID: 1021 RVA: 0x0000B652 File Offset: 0x00009852
		public FigureLength(double pixels)
		{
			this = new FigureLength(pixels, FigureUnitType.Pixel);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FigureLength" /> class with the specified <see cref="P:System.Windows.FigureLength.Value" /> and <see cref="P:System.Windows.FigureLength.FigureUnitType" />.</summary>
		/// <param name="value">The <see cref="P:System.Windows.FigureLength.Value" /> of the <see cref="T:System.Windows.FigureLength" /> class.</param>
		/// <param name="type">The <see cref="P:System.Windows.FigureLength.Value" /> of the <see cref="P:System.Windows.FigureLength.FigureUnitType" /> class.</param>
		// Token: 0x060003FE RID: 1022 RVA: 0x0000B65C File Offset: 0x0000985C
		public FigureLength(double value, FigureUnitType type)
		{
			double num = 1000.0;
			double num2 = (double)Math.Min(1000000, 3500000);
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
			if (value < 0.0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("InvalidCtorParameterNoNegative", new object[]
				{
					"value"
				}));
			}
			if (type != FigureUnitType.Auto && type != FigureUnitType.Pixel && type != FigureUnitType.Column && type != FigureUnitType.Content && type != FigureUnitType.Page)
			{
				throw new ArgumentException(SR.Get("InvalidCtorParameterUnknownFigureUnitType", new object[]
				{
					"type"
				}));
			}
			if (value > 1.0 && (type == FigureUnitType.Content || type == FigureUnitType.Page))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > num && type == FigureUnitType.Column)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > num2 && type == FigureUnitType.Pixel)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			this._unitValue = ((type == FigureUnitType.Auto) ? 0.0 : value);
			this._unitType = type;
		}

		/// <summary>Compares two <see cref="T:System.Windows.FigureLength" /> structures for equality.</summary>
		/// <param name="fl1">The first <see cref="T:System.Windows.FigureLength" /> structure to compare.</param>
		/// <param name="fl2">The second <see cref="T:System.Windows.FigureLength" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="fl1" /> and <paramref name="fl2" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003FF RID: 1023 RVA: 0x0000B78C File Offset: 0x0000998C
		public static bool operator ==(FigureLength fl1, FigureLength fl2)
		{
			return fl1.FigureUnitType == fl2.FigureUnitType && fl1.Value == fl2.Value;
		}

		/// <summary>Compares two <see cref="T:System.Windows.FigureLength" /> structures for inequality.</summary>
		/// <param name="fl1">The first <see cref="T:System.Windows.FigureLength" /> structure to compare.</param>
		/// <param name="fl2">The second <see cref="T:System.Windows.FigureLength" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="fl1" /> and <paramref name="fl2" /> are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000400 RID: 1024 RVA: 0x0000B7B0 File Offset: 0x000099B0
		public static bool operator !=(FigureLength fl1, FigureLength fl2)
		{
			return fl1.FigureUnitType != fl2.FigureUnitType || fl1.Value != fl2.Value;
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is a <see cref="T:System.Windows.FigureLength" /> and whether it is identical to this <see cref="T:System.Windows.FigureLength" />.</summary>
		/// <param name="oCompare">The <see cref="T:System.Object" /> to compare to this instance.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="oCompare" /> is a <see cref="T:System.Windows.FigureLength" /> and is identical to this <see cref="T:System.Windows.FigureLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000401 RID: 1025 RVA: 0x0000B7D8 File Offset: 0x000099D8
		public override bool Equals(object oCompare)
		{
			if (oCompare is FigureLength)
			{
				FigureLength fl = (FigureLength)oCompare;
				return this == fl;
			}
			return false;
		}

		/// <summary>Compares two <see cref="T:System.Windows.FigureLength" /> structures for equality.</summary>
		/// <param name="figureLength">The <see cref="T:System.Windows.FigureLength" /> to compare to this instance.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="figureLength" /> is identical to this <see cref="T:System.Windows.FigureLength" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000402 RID: 1026 RVA: 0x0000B802 File Offset: 0x00009A02
		public bool Equals(FigureLength figureLength)
		{
			return this == figureLength;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.FigureLength" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.FigureLength" /> structure.</returns>
		// Token: 0x06000403 RID: 1027 RVA: 0x0000B810 File Offset: 0x00009A10
		public override int GetHashCode()
		{
			return (int)((int)this._unitValue + this._unitType);
		}

		/// <summary>Gets a value that determines whether this <see cref="T:System.Windows.FigureLength" /> holds an absolute value (in pixels).</summary>
		/// <returns>
		///     true if this <see cref="T:System.Windows.FigureLength" /> holds an absolute value (in pixels); otherwise, false. The default value is false.</returns>
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000B820 File Offset: 0x00009A20
		public bool IsAbsolute
		{
			get
			{
				return this._unitType == FigureUnitType.Pixel;
			}
		}

		/// <summary>Gets a value that determines whether this <see cref="T:System.Windows.FigureLength" /> is automatic (not specified).</summary>
		/// <returns>
		///     true if this <see cref="T:System.Windows.FigureLength" /> is automatic (not specified); otherwise, false. The default value is true.</returns>
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0000B82B File Offset: 0x00009A2B
		public bool IsAuto
		{
			get
			{
				return this._unitType == FigureUnitType.Auto;
			}
		}

		/// <summary>Gets a value that determines whether this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Column" />.</summary>
		/// <returns>
		///     true if this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Column" />; otherwise, false. The default value is false.</returns>
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000B836 File Offset: 0x00009A36
		public bool IsColumn
		{
			get
			{
				return this._unitType == FigureUnitType.Column;
			}
		}

		/// <summary>Gets a value that determines whether this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Content" />.</summary>
		/// <returns>Returns true if this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Content" />; otherwise, false. The default value is false.</returns>
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0000B841 File Offset: 0x00009A41
		public bool IsContent
		{
			get
			{
				return this._unitType == FigureUnitType.Content;
			}
		}

		/// <summary>Gets a value that determines whether this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Page" />.</summary>
		/// <returns>
		///     true if this <see cref="T:System.Windows.FigureLength" /> has a <see cref="T:System.Windows.FigureUnitType" /> property value of <see cref="F:System.Windows.FigureUnitType.Page" />; otherwise, false. The default value is false.</returns>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000B84C File Offset: 0x00009A4C
		public bool IsPage
		{
			get
			{
				return this._unitType == FigureUnitType.Page;
			}
		}

		/// <summary>Gets the value of this <see cref="T:System.Windows.FigureLength" />. </summary>
		/// <returns>The value of this <see cref="T:System.Windows.FigureLength" />. The default value is 1.</returns>
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000B857 File Offset: 0x00009A57
		public double Value
		{
			get
			{
				if (this._unitType != FigureUnitType.Auto)
				{
					return this._unitValue;
				}
				return 1.0;
			}
		}

		/// <summary>Gets the unit type of the <see cref="P:System.Windows.FigureLength.Value" />.</summary>
		/// <returns>The unit type of this <see cref="P:System.Windows.FigureLength.Value" />. The default value is <see cref="F:System.Windows.FigureUnitType.Auto" />.</returns>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000B871 File Offset: 0x00009A71
		public FigureUnitType FigureUnitType
		{
			get
			{
				return this._unitType;
			}
		}

		/// <summary>Creates a <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.FigureLength" />.</summary>
		/// <returns>A <see cref="T:System.String" /> representation of this <see cref="T:System.Windows.FigureLength" />.</returns>
		// Token: 0x0600040B RID: 1035 RVA: 0x0000B879 File Offset: 0x00009A79
		public override string ToString()
		{
			return FigureLengthConverter.ToString(this, CultureInfo.InvariantCulture);
		}

		// Token: 0x04000627 RID: 1575
		private double _unitValue;

		// Token: 0x04000628 RID: 1576
		private FigureUnitType _unitType;
	}
}
