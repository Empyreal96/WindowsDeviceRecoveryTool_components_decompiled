using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Markup
{
	/// <summary>Converts instances of <see langword="ResourceReferenceExpression" /> to and from other types. </summary>
	// Token: 0x020001C3 RID: 451
	public class ResourceReferenceExpressionConverter : ExpressionConverter
	{
		/// <summary>Returns a value that indicates whether the converter can convert from a source object to a <see langword="ResourceReferenceExpression" /> object. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">The type to convert from.</param>
		/// <returns>
		///     <see langword="true" /> if the converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001D07 RID: 7431 RVA: 0x00087965 File Offset: 0x00085B65
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Returns a value that indicates whether the converter can convert a <see langword="ResourceReferenceExpression" /> object to the specified destination type. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>
		///     <see langword="true" /> if the converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		// Token: 0x06001D08 RID: 7432 RVA: 0x0008796F File Offset: 0x00085B6F
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			return destinationType == typeof(MarkupExtension) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified value to the <see langword="ResourceReferenceExpression" /> type. </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The object to convert.</param>
		/// <returns>The converted value.</returns>
		// Token: 0x06001D09 RID: 7433 RVA: 0x000877EA File Offset: 0x000859EA
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the specified <see langword="ResourceReferenceExpression" /> object to the specified type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>The converted value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> must be of type <see langword="ResourceReferenceExpression" />.</exception>
		// Token: 0x06001D0A RID: 7434 RVA: 0x000879A4 File Offset: 0x00085BA4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			ResourceReferenceExpression resourceReferenceExpression = value as ResourceReferenceExpression;
			if (resourceReferenceExpression == null)
			{
				throw new ArgumentException(SR.Get("MustBeOfType", new object[]
				{
					"value",
					"ResourceReferenceExpression"
				}));
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(MarkupExtension))
			{
				return new DynamicResourceExtension(resourceReferenceExpression.ResourceKey);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
