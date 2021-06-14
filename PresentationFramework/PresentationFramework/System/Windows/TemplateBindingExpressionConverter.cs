using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>A type converter that is used to construct a markup extension from a <see cref="T:System.Windows.TemplateBindingExpression" /> instance during serialization. </summary>
	// Token: 0x0200011C RID: 284
	public class TemplateBindingExpressionConverter : TypeConverter
	{
		/// <summary>Returns whether this converter can convert the object to the specified type, using the specified context. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> implementation that provides a format context. </param>
		/// <param name="destinationType">The desired type of the conversion's output.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the requested conversion; otherwise, <see langword="false" />. Only a <paramref name="destinationType" /> of <see cref="T:System.Windows.Markup.MarkupExtension" /> returns <see langword="true" />.</returns>
		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002B247 File Offset: 0x00029447
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(MarkupExtension) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given value object to a <see cref="T:System.Windows.Markup.MarkupExtension" /> type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> implementation that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object. If a null reference is passed, the current culture is assumed. </param>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The desired type to convert to.</param>
		/// <returns>The converted value. </returns>
		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002B268 File Offset: 0x00029468
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (!(destinationType == typeof(MarkupExtension)))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			TemplateBindingExpression templateBindingExpression = value as TemplateBindingExpression;
			if (templateBindingExpression == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"TemplateBindingExpression"
				}));
			}
			return templateBindingExpression.TemplateBindingExtension;
		}
	}
}
