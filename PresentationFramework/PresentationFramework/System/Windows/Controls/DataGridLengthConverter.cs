using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Converts instances of various types to and from instances of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
	// Token: 0x020004B0 RID: 1200
	public class DataGridLengthConverter : TypeConverter
	{
		/// <summary>Determines whether an instance of the specified type can be converted to an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="sourceType">The type to convert from.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004921 RID: 18721 RVA: 0x0014B93C File Offset: 0x00149B3C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Byte <= 9 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class can be converted to an instance of the specified type.</summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004922 RID: 18722 RVA: 0x0014B95F File Offset: 0x00149B5F
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor);
		}

		/// <summary>Converts the specified object to an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class.</summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="culture">The object to use as the current culture.</param>
		/// <param name="value">The value to convert.</param>
		/// <returns>The converted value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not a valid type that can be converted to type <see cref="T:System.Windows.Controls.DataGridLength" />.</exception>
		// Token: 0x06004923 RID: 18723 RVA: 0x0014B988 File Offset: 0x00149B88
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value != null)
			{
				string text = value as string;
				if (text != null)
				{
					return DataGridLengthConverter.ConvertFromString(text, culture);
				}
				double num = Convert.ToDouble(value, culture);
				DataGridLengthUnitType type;
				if (DoubleUtil.IsNaN(num))
				{
					num = 1.0;
					type = DataGridLengthUnitType.Auto;
				}
				else
				{
					type = DataGridLengthUnitType.Pixel;
				}
				if (!double.IsInfinity(num))
				{
					return new DataGridLength(num, type);
				}
			}
			throw base.GetConvertFromException(value);
		}

		/// <summary>Converts an instance of the <see cref="T:System.Windows.Controls.DataGridLength" /> class to an instance of the specified type.</summary>
		/// <param name="context">An object that provides a format context.</param>
		/// <param name="culture">The object to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Windows.Controls.DataGridLength" /> to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not a <see cref="T:System.Windows.Controls.DataGridLength" /> or <paramref name="destinationType" /> is not a valid conversion type.</exception>
		// Token: 0x06004924 RID: 18724 RVA: 0x0014B9EC File Offset: 0x00149BEC
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is DataGridLength)
			{
				DataGridLength length = (DataGridLength)value;
				if (destinationType == typeof(string))
				{
					return DataGridLengthConverter.ConvertToString(length, culture);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(DataGridLength).GetConstructor(new Type[]
					{
						typeof(double),
						typeof(DataGridLengthUnitType)
					});
					return new InstanceDescriptor(constructor, new object[]
					{
						length.Value,
						length.UnitType
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x0014BAB8 File Offset: 0x00149CB8
		internal static string ConvertToString(DataGridLength length, CultureInfo cultureInfo)
		{
			switch (length.UnitType)
			{
			case DataGridLengthUnitType.Auto:
			case DataGridLengthUnitType.SizeToCells:
			case DataGridLengthUnitType.SizeToHeader:
				return length.UnitType.ToString();
			case DataGridLengthUnitType.Star:
				if (!DoubleUtil.IsOne(length.Value))
				{
					return Convert.ToString(length.Value, cultureInfo) + "*";
				}
				return "*";
			}
			return Convert.ToString(length.Value, cultureInfo);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0014BB38 File Offset: 0x00149D38
		private static DataGridLength ConvertFromString(string s, CultureInfo cultureInfo)
		{
			string text = s.Trim().ToLowerInvariant();
			for (int i = 0; i < 3; i++)
			{
				string b = DataGridLengthConverter._unitStrings[i];
				if (text == b)
				{
					return new DataGridLength(1.0, (DataGridLengthUnitType)i);
				}
			}
			double value = 0.0;
			DataGridLengthUnitType dataGridLengthUnitType = DataGridLengthUnitType.Pixel;
			int length = text.Length;
			int num = 0;
			double num2 = 1.0;
			int num3 = DataGridLengthConverter._unitStrings.Length;
			for (int j = 3; j < num3; j++)
			{
				string text2 = DataGridLengthConverter._unitStrings[j];
				if (text.EndsWith(text2, StringComparison.Ordinal))
				{
					num = text2.Length;
					dataGridLengthUnitType = (DataGridLengthUnitType)j;
					break;
				}
			}
			if (num == 0)
			{
				num3 = DataGridLengthConverter._nonStandardUnitStrings.Length;
				for (int k = 0; k < num3; k++)
				{
					string text3 = DataGridLengthConverter._nonStandardUnitStrings[k];
					if (text.EndsWith(text3, StringComparison.Ordinal))
					{
						num = text3.Length;
						num2 = DataGridLengthConverter._pixelUnitFactors[k];
						break;
					}
				}
			}
			if (length == num)
			{
				if (dataGridLengthUnitType == DataGridLengthUnitType.Star)
				{
					value = 1.0;
				}
			}
			else
			{
				string value2 = text.Substring(0, length - num);
				value = Convert.ToDouble(value2, cultureInfo) * num2;
			}
			return new DataGridLength(value, dataGridLengthUnitType);
		}

		// Token: 0x040029CA RID: 10698
		private static string[] _unitStrings = new string[]
		{
			"auto",
			"px",
			"sizetocells",
			"sizetoheader",
			"*"
		};

		// Token: 0x040029CB RID: 10699
		private const int NumDescriptiveUnits = 3;

		// Token: 0x040029CC RID: 10700
		private static string[] _nonStandardUnitStrings = new string[]
		{
			"in",
			"cm",
			"pt"
		};

		// Token: 0x040029CD RID: 10701
		private static double[] _pixelUnitFactors = new double[]
		{
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
