using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace System.Windows
{
	/// <summary>Converts font size values to and from other type representations.</summary>
	// Token: 0x020000BE RID: 190
	public class FontSizeConverter : TypeConverter
	{
		/// <summary>Determines if conversion from a specified type to a <see cref="T:System.Double" /> value is possible.</summary>
		/// <param name="context">Describes context information of a component such as its container and <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
		/// <param name="sourceType">Identifies the data type to evaluate for purposes of conversion.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="sourceType" /> can be converted to <see cref="T:System.Double" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000413 RID: 1043 RVA: 0x0000BA58 File Offset: 0x00009C58
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(int) || sourceType == typeof(float) || sourceType == typeof(double);
		}

		/// <summary>Determines if conversion of a font size value to a specified type is possible.</summary>
		/// <param name="context">Context information of a component such as its container and <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
		/// <param name="destinationType">The data type to evaluate for purposes of conversion.</param>
		/// <returns>
		///     <see langword="true" /> if this type can be converted; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000414 RID: 1044 RVA: 0x0000B0C1 File Offset: 0x000092C1
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts a specified type to a <see cref="T:System.Double" />.</summary>
		/// <param name="context">Context information of a component such as its container and <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
		/// <param name="culture">Cultural specific information, including the writing system and calendar used.</param>
		/// <param name="value">The value which is being converted to a font size value.</param>
		/// <returns>A <see cref="T:System.Double" /> value that represents the converted font size value.</returns>
		// Token: 0x06000415 RID: 1045 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				throw base.GetConvertFromException(value);
			}
			string text = value as string;
			if (text != null)
			{
				double num;
				FontSizeConverter.FromString(text, culture, out num);
				return num;
			}
			if (value is int || value is float || value is double)
			{
				return (double)value;
			}
			return null;
		}

		/// <summary>Converts a <see cref="T:System.Double" /> value to a specified type.</summary>
		/// <param name="context">Context information of a component such as its container and <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
		/// <param name="culture">Cultural specific information, including writing system and calendar used.</param>
		/// <param name="value">The <see cref="T:System.Object" /> being converted.</param>
		/// <param name="destinationType">The data type this font size value is being converted to.</param>
		/// <returns>A new <see cref="T:System.Object" /> that is the value of the conversion.</returns>
		// Token: 0x06000416 RID: 1046 RVA: 0x0000BB08 File Offset: 0x00009D08
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			double num = (double)value;
			if (destinationType == typeof(string))
			{
				return num.ToString(culture);
			}
			if (destinationType == typeof(int))
			{
				return (int)num;
			}
			if (destinationType == typeof(float))
			{
				return (float)num;
			}
			if (destinationType == typeof(double))
			{
				return num;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		internal static void FromString(string text, CultureInfo culture, out double amount)
		{
			amount = LengthConverter.FromString(text, culture);
		}
	}
}
