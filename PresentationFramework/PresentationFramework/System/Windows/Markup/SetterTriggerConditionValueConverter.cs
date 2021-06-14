using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Markup
{
	/// <summary>Provides type conversion analogous behavior for <see cref="T:System.Windows.Setter" />, <see cref="T:System.Windows.Trigger" /> and <see cref="T:System.Windows.Condition" /> types that deal with <see cref="T:System.Windows.DependencyProperty" /> values. This converter only supports <see langword="ConvertFrom" />.</summary>
	// Token: 0x0200022C RID: 556
	public sealed class SetterTriggerConditionValueConverter : TypeConverter
	{
		/// <summary>Returns a value that indicates whether the converter can convert from a source object to a side-effect-produced <see cref="T:System.Windows.Setter" />, <see cref="T:System.Windows.Trigger" /> or <see cref="T:System.Windows.Condition" /> . </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">The type to convert from.</param>
		/// <returns>
		///     <see langword="true" /> if the converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600224B RID: 8779 RVA: 0x00098071 File Offset: 0x00096271
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]);
		}

		/// <summary>Returns a value that indicates whether the converter can convert to the specified destination type. Always returns <see langword="false" />.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600224C RID: 8780 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		/// <summary>Converts the converted source value if an underlying type converter can be obtained from context. Otherwise returns an unconverted source.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="source">The object to convert.</param>
		/// <returns>The converter object, or possibly an unconverted source.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="context" /> or <paramref name="source" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">No <see cref="T:System.Xaml.IXamlSchemaContextProvider" /> service available.</exception>
		// Token: 0x0600224D RID: 8781 RVA: 0x000AAB0C File Offset: 0x000A8D0C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			return SetterTriggerConditionValueConverter.ResolveValue(context, null, culture, source);
		}

		/// <summary>Converts the specified object to the specified type. Always throws an exception.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.InvalidOperationException">Thrown in all cases.</exception>
		// Token: 0x0600224E RID: 8782 RVA: 0x0008795A File Offset: 0x00085B5A
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000AAB18 File Offset: 0x000A8D18
		internal static object ResolveValue(ITypeDescriptorContext serviceProvider, DependencyProperty property, CultureInfo culture, object source)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!(source is byte[]) && !(source is string) && !(source is Stream))
			{
				return source;
			}
			IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
			if (xamlSchemaContextProvider == null)
			{
				throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					"Value",
					typeof(object).FullName
				}));
			}
			XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
			if (property == null)
			{
				return source;
			}
			XamlMember xamlMember = schemaContext.GetXamlType(property.OwnerType).GetMember(property.Name);
			if (xamlMember == null)
			{
				xamlMember = schemaContext.GetXamlType(property.OwnerType).GetAttachableMember(property.Name);
			}
			XamlValueConverter<TypeConverter> typeConverter;
			if (xamlMember != null)
			{
				if (xamlMember.Type.UnderlyingType.IsEnum && schemaContext is Baml2006SchemaContext)
				{
					typeConverter = XamlReader.BamlSharedSchemaContext.GetTypeConverter(xamlMember.Type.UnderlyingType);
				}
				else
				{
					typeConverter = xamlMember.TypeConverter;
					if (typeConverter == null)
					{
						typeConverter = xamlMember.Type.TypeConverter;
					}
				}
			}
			else
			{
				typeConverter = schemaContext.GetXamlType(property.PropertyType).TypeConverter;
			}
			if (typeConverter.ConverterType == null)
			{
				return source;
			}
			TypeConverter typeConverter2;
			if (xamlMember != null && xamlMember.Type.UnderlyingType == typeof(bool))
			{
				if (source is string)
				{
					typeConverter2 = new BooleanConverter();
				}
				else
				{
					if (!(source is byte[]))
					{
						throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
						{
							"Value",
							typeof(object).FullName
						}));
					}
					byte[] array = source as byte[];
					if (array != null && array.Length == 1)
					{
						return array[0] > 0;
					}
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Value",
						typeof(object).FullName
					}));
				}
			}
			else
			{
				typeConverter2 = typeConverter.ConverterInstance;
			}
			return typeConverter2.ConvertFrom(serviceProvider, culture, source);
		}
	}
}
