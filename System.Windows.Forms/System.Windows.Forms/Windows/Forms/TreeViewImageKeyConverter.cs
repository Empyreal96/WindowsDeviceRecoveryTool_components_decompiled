using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert data for an image key to and from another data type.</summary>
	// Token: 0x02000412 RID: 1042
	public class TreeViewImageKeyConverter : ImageKeyConverter
	{
		/// <summary>Converts the specified object to an object of the specified type using the specified culture information and context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that provides locale information.</param>
		/// <param name="value">The object to convert, typically an image key.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The specified <paramref name="value" /> cannot be converted to the specified <paramref name="destinationType" />.</exception>
		// Token: 0x0600475B RID: 18267 RVA: 0x00130B60 File Offset: 0x0012ED60
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value == null)
			{
				return SR.GetString("toStringDefault");
			}
			string text = value as string;
			if (text != null && text.Length == 0)
			{
				return SR.GetString("toStringDefault");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
