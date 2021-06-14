using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Converts instances of other types to and from instances of a <see cref="T:System.Double" /> that represent an object's length.</summary>
	// Token: 0x020000D4 RID: 212
	public class LengthConverter : TypeConverter
	{
		/// <summary>Determines whether conversion is possible from a specified type to a <see cref="T:System.Double" /> that represents an object's length. </summary>
		/// <param name="typeDescriptorContext">Provides contextual information about a component.</param>
		/// <param name="sourceType">Identifies the data type to evaluate for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if conversion is possible; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000754 RID: 1876 RVA: 0x00016D2C File Offset: 0x00014F2C
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			TypeCode typeCode = Type.GetTypeCode(sourceType);
			return typeCode - TypeCode.Int16 <= 8 || typeCode == TypeCode.String;
		}

		/// <summary>Determines whether conversion is possible to a specified type from a <see cref="T:System.Double" /> that represents an object's length. </summary>
		/// <param name="typeDescriptorContext">Provides contextual information about a component.</param>
		/// <param name="destinationType">Identifies the data type to evaluate for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if conversion to the <paramref name="destinationType" /> is possible; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000755 RID: 1877 RVA: 0x00009F0E File Offset: 0x0000810E
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string);
		}

		/// <summary>Converts instances of other data types into instances of <see cref="T:System.Double" /> that represent an object's length. </summary>
		/// <param name="typeDescriptorContext">Provides contextual information about a component.</param>
		/// <param name="cultureInfo">Represents culture-specific information that is maintained during a conversion.</param>
		/// <param name="source">Identifies the object that is being converted to <see cref="T:System.Double" />.</param>
		/// <returns>An instance of <see cref="T:System.Double" /> that is the value of the conversion.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs if the <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs if the <paramref name="source" /> is not <see langword="null" /> and is not a valid type for conversion.</exception>
		// Token: 0x06000756 RID: 1878 RVA: 0x00016D4E File Offset: 0x00014F4E
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw base.GetConvertFromException(source);
			}
			if (source is string)
			{
				return LengthConverter.FromString((string)source, cultureInfo);
			}
			return Convert.ToDouble(source, cultureInfo);
		}

		/// <summary>Converts other types into instances of <see cref="T:System.Double" /> that represent an object's length. </summary>
		/// <param name="typeDescriptorContext">Describes context information of a component, such as its container and <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
		/// <param name="cultureInfo">Identifies culture-specific information, including the writing system and the calendar that is used.</param>
		/// <param name="value">Identifies the <see cref="T:System.Object" /> that is being converted.</param>
		/// <param name="destinationType">The data type that this instance of <see cref="T:System.Double" /> is being converted to.</param>
		/// <returns>A new <see cref="T:System.Object" /> that is the value of the conversion.</returns>
		/// <exception cref="T:System.ArgumentNullException">Occurs if the <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">Occurs if the <paramref name="value" /> is not <see langword="null" /> and is not a <see cref="T:System.Windows.Media.Brush" />, or the <paramref name="destinationType" /> is not valid.</exception>
		// Token: 0x06000757 RID: 1879 RVA: 0x00016D84 File Offset: 0x00014F84
		[SecurityCritical]
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value != null && value is double)
			{
				double num = (double)value;
				if (destinationType == typeof(string))
				{
					if (DoubleUtil.IsNaN(num))
					{
						return "Auto";
					}
					return Convert.ToString(num, cultureInfo);
				}
				else if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(double).GetConstructor(new Type[]
					{
						typeof(double)
					});
					return new InstanceDescriptor(constructor, new object[]
					{
						num
					});
				}
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00016E3C File Offset: 0x0001503C
		internal static double FromString(string s, CultureInfo cultureInfo)
		{
			string text = s.Trim();
			string text2 = text.ToLowerInvariant();
			int length = text2.Length;
			int num = 0;
			double num2 = 1.0;
			if (text2 == "auto")
			{
				return double.NaN;
			}
			for (int i = 0; i < LengthConverter.PixelUnitStrings.Length; i++)
			{
				if (text2.EndsWith(LengthConverter.PixelUnitStrings[i], StringComparison.Ordinal))
				{
					num = LengthConverter.PixelUnitStrings[i].Length;
					num2 = LengthConverter.PixelUnitFactors[i];
					break;
				}
			}
			text = text.Substring(0, length - num);
			double result;
			try
			{
				double num3 = Convert.ToDouble(text, cultureInfo) * num2;
				result = num3;
			}
			catch (FormatException)
			{
				throw new FormatException(SR.Get("LengthFormatError", new object[]
				{
					text
				}));
			}
			return result;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00016F10 File Offset: 0x00015110
		internal static string ToString(double l, CultureInfo cultureInfo)
		{
			if (DoubleUtil.IsNaN(l))
			{
				return "Auto";
			}
			return Convert.ToString(l, cultureInfo);
		}

		// Token: 0x04000735 RID: 1845
		private static string[] PixelUnitStrings = new string[]
		{
			"px",
			"in",
			"cm",
			"pt"
		};

		// Token: 0x04000736 RID: 1846
		private static double[] PixelUnitFactors = new double[]
		{
			1.0,
			96.0,
			37.79527559055118,
			1.3333333333333333
		};
	}
}
