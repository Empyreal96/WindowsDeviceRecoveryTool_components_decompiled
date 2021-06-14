using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert opacity values to and from a string.</summary>
	// Token: 0x02000300 RID: 768
	public class OpacityConverter : TypeConverter
	{
		/// <summary>Returns a value indicating whether this converter can convert an object of the specified source type to the native type of the converter that uses the specified context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides information about the context of a type converter. </param>
		/// <param name="sourceType">The <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002EA1 RID: 11937 RVA: 0x000B9F74 File Offset: 0x000B8174
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Converts the specified object to the converter's native type.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides information about the context of a type converter. </param>
		/// <param name="culture">The locale information for the conversion. </param>
		/// <param name="value">The object to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.Exception">The object was not a supported type for the conversion.</exception>
		/// <exception cref="T:System.FormatException">
		///         <paramref name="value" /> could not be properly converted to type <see cref="T:System.Double" />. -or- The resulting converted <paramref name="value" /> was less than zero percent or greater than one hundred percent.</exception>
		// Token: 0x06002EA2 RID: 11938 RVA: 0x000D8B5C File Offset: 0x000D6D5C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = ((string)value).Replace('%', ' ').Trim();
			double num = double.Parse(text, CultureInfo.CurrentCulture);
			int num2 = ((string)value).IndexOf("%");
			if (num2 > 0 && num >= 0.0 && num <= 1.0)
			{
				text = (num / 100.0).ToString(CultureInfo.CurrentCulture);
			}
			double num3 = 1.0;
			try
			{
				num3 = (double)TypeDescriptor.GetConverter(typeof(double)).ConvertFrom(context, culture, text);
				if (num3 > 1.0)
				{
					num3 /= 100.0;
				}
			}
			catch (FormatException innerException)
			{
				throw new FormatException(SR.GetString("InvalidBoundArgument", new object[]
				{
					"Opacity",
					text,
					"0%",
					"100%"
				}), innerException);
			}
			if (num3 < 0.0 || num3 > 1.0)
			{
				throw new FormatException(SR.GetString("InvalidBoundArgument", new object[]
				{
					"Opacity",
					text,
					"0%",
					"100%"
				}));
			}
			return num3;
		}

		/// <summary>Converts from the converter's native type to a value of the destination type.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides information about the context of a type converter. </param>
		/// <param name="culture">The locale information for the conversion. </param>
		/// <param name="value">The value to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> cannot be converted to the <paramref name="destinationType" />.</exception>
		// Token: 0x06002EA3 RID: 11939 RVA: 0x000D8CBC File Offset: 0x000D6EBC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				double num = (double)value;
				return ((int)(num * 100.0)).ToString(CultureInfo.CurrentCulture) + "%";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
