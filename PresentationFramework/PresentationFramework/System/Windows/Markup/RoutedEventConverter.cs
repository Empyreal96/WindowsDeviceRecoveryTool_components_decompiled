using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xaml;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup
{
	/// <summary>Converts a <see cref="T:System.Windows.RoutedEvent" /> object from a string.</summary>
	// Token: 0x0200022A RID: 554
	public sealed class RoutedEventConverter : TypeConverter
	{
		/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.RoutedEvent" />.</summary>
		/// <param name="typeDescriptorContext">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="sourceType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the operation; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600223E RID: 8766 RVA: 0x00018B21 File Offset: 0x00016D21
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.RoutedEvent" /> can be converted to the specified type.</summary>
		/// <param name="typeDescriptorContext">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="destinationType">The type being evaluated for conversion.</param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x0600223F RID: 8767 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return false;
		}

		/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.RoutedEvent" /> object, using the specified context.</summary>
		/// <param name="typeDescriptorContext">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="cultureInfo">Culture specific information.</param>
		/// <param name="source">The object to convert.</param>
		/// <returns>The conversion result.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="source" /> is not a string or cannot be converted.</exception>
		// Token: 0x06002240 RID: 8768 RVA: 0x000AA7F4 File Offset: 0x000A89F4
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			string text = source as string;
			RoutedEvent routedEvent = null;
			if (text != null)
			{
				text = text.Trim();
				if (typeDescriptorContext != null)
				{
					IXamlTypeResolver xamlTypeResolver = typeDescriptorContext.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
					Type type = null;
					if (xamlTypeResolver != null)
					{
						int num = text.IndexOf('.');
						if (num != -1)
						{
							string qualifiedTypeName = text.Substring(0, num);
							text = text.Substring(num + 1);
							type = xamlTypeResolver.Resolve(qualifiedTypeName);
						}
					}
					if (type == null)
					{
						IXamlSchemaContextProvider xamlSchemaContextProvider = typeDescriptorContext.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
						IAmbientProvider ambientProvider = typeDescriptorContext.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
						if (xamlSchemaContextProvider != null && ambientProvider != null)
						{
							XamlSchemaContext schemaContext = xamlSchemaContextProvider.SchemaContext;
							XamlType xamlType = schemaContext.GetXamlType(typeof(Style));
							List<XamlType> list = new List<XamlType>();
							list.Add(xamlType);
							XamlMember member = xamlType.GetMember("TargetType");
							AmbientPropertyValue firstAmbientValue = ambientProvider.GetFirstAmbientValue(list, new XamlMember[]
							{
								member
							});
							if (firstAmbientValue != null)
							{
								type = (firstAmbientValue.Value as Type);
							}
							if (type == null)
							{
								type = typeof(FrameworkElement);
							}
						}
					}
					if (type != null)
					{
						Type type2 = type;
						while (null != type2)
						{
							SecurityHelper.RunClassConstructor(type2);
							type2 = type2.BaseType;
						}
						routedEvent = EventManager.GetRoutedEventFromName(text, type);
					}
				}
			}
			if (routedEvent == null)
			{
				throw base.GetConvertFromException(source);
			}
			return routedEvent;
		}

		/// <summary>Attempts to convert a <see cref="T:System.Windows.RoutedEvent" /> to the specified type. Throws an exception in all cases.</summary>
		/// <param name="typeDescriptorContext">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="cultureInfo">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> cannot be converted. This is not a functioning converter for a save path..</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="value" /> or <paramref name="destinationType" /> are <see langword="null" />.</exception>
		// Token: 0x06002241 RID: 8769 RVA: 0x000AA964 File Offset: 0x000A8B64
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x000AA994 File Offset: 0x000A8B94
		private string ExtractNamespaceString(ref string nameString, ParserContext parserContext)
		{
			int num = nameString.IndexOf(':');
			string text = string.Empty;
			if (num != -1)
			{
				text = nameString.Substring(0, num);
				nameString = nameString.Substring(num + 1);
			}
			string text2 = parserContext.XmlnsDictionary[text];
			if (text2 == null)
			{
				throw new ArgumentException(SR.Get("ParserPrefixNSProperty", new object[]
				{
					text,
					nameString
				}));
			}
			return text2;
		}
	}
}
