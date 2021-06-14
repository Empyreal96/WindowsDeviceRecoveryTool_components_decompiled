using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Xaml;

namespace System.Windows.Markup
{
	/// <summary>Converts from a string to a <see cref="T:System.Windows.DependencyProperty" /> object.</summary>
	// Token: 0x02000213 RID: 531
	public sealed class DependencyPropertyConverter : TypeConverter
	{
		/// <summary>Determines whether an object of the specified type can be converted to an instance of <see cref="T:System.Windows.DependencyProperty" />.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="sourceType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the operation; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002109 RID: 8457 RVA: 0x00098071 File Offset: 0x00096271
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]);
		}

		/// <summary>Determines whether an instance of <see cref="T:System.Windows.DependencyProperty" /> can be converted to the specified type.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="destinationType">The type being evaluated for conversion.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the operation; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600210A RID: 8458 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		/// <summary>Attempts to convert the specified object to a <see cref="T:System.Windows.DependencyProperty" />, using the specified context.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="source">The object to convert.</param>
		/// <returns>The converted object. If the conversion is successful, this is a <see cref="T:System.Windows.DependencyProperty" />.</returns>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="source" /> cannot be converted.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="context" /> or <paramref name="source" /> are <see langword="null" />.</exception>
		// Token: 0x0600210B RID: 8459 RVA: 0x0009809C File Offset: 0x0009629C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			DependencyProperty dependencyProperty = DependencyPropertyConverter.ResolveProperty(context, null, source);
			if (dependencyProperty != null)
			{
				return dependencyProperty;
			}
			throw base.GetConvertFromException(source);
		}

		/// <summary>Attempts to convert a <see cref="T:System.Windows.DependencyProperty" /> to the specified type, using the specified context. Always throws an exception.</summary>
		/// <param name="context">A format context that provides information about the environment from which this converter is being invoked.</param>
		/// <param name="culture">Culture specific information.</param>
		/// <param name="value">The object to convert.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		// Token: 0x0600210C RID: 8460 RVA: 0x0008795A File Offset: 0x00085B5A
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000980DC File Offset: 0x000962DC
		internal static DependencyProperty ResolveProperty(IServiceProvider serviceProvider, string targetName, object source)
		{
			Type type = null;
			string text = null;
			DependencyProperty dependencyProperty = source as DependencyProperty;
			if (dependencyProperty != null)
			{
				return dependencyProperty;
			}
			byte[] array;
			if ((array = (source as byte[])) != null)
			{
				Baml2006SchemaContext baml2006SchemaContext = (serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider).SchemaContext as Baml2006SchemaContext;
				if (array.Length == 2)
				{
					short propertyId = (short)((int)array[0] | (int)array[1] << 8);
					return baml2006SchemaContext.GetDependencyProperty(propertyId);
				}
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
				{
					type = baml2006SchemaContext.GetXamlType(binaryReader.ReadInt16()).UnderlyingType;
					text = binaryReader.ReadString();
					goto IL_142;
				}
			}
			string text2;
			if ((text2 = (source as string)) == null)
			{
				throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
				{
					"Property",
					typeof(DependencyProperty).FullName
				}));
			}
			text2 = text2.Trim();
			if (text2.Contains("."))
			{
				int num = text2.LastIndexOf('.');
				string qualifiedTypeName = text2.Substring(0, num);
				text = text2.Substring(num + 1);
				IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
				type = xamlTypeResolver.Resolve(qualifiedTypeName);
			}
			else
			{
				int num2 = text2.LastIndexOf(':');
				text = text2.Substring(num2 + 1);
			}
			IL_142:
			if (type == null && targetName != null)
			{
				IAmbientProvider ambientProvider = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
				XamlSchemaContext schemaContext = (serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider).SchemaContext;
				type = DependencyPropertyConverter.GetTypeFromName(schemaContext, ambientProvider, targetName);
			}
			if (type == null)
			{
				IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
				if (xamlSchemaContextProvider == null)
				{
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Property",
						typeof(DependencyProperty).FullName
					}));
				}
				XamlSchemaContext schemaContext2 = xamlSchemaContextProvider.SchemaContext;
				XamlType xamlType = schemaContext2.GetXamlType(typeof(Style));
				XamlType xamlType2 = schemaContext2.GetXamlType(typeof(FrameworkTemplate));
				XamlType xamlType3 = schemaContext2.GetXamlType(typeof(DataTemplate));
				XamlType xamlType4 = schemaContext2.GetXamlType(typeof(ControlTemplate));
				List<XamlType> list = new List<XamlType>();
				list.Add(xamlType);
				list.Add(xamlType2);
				list.Add(xamlType3);
				list.Add(xamlType4);
				XamlMember member = xamlType.GetMember("TargetType");
				XamlMember member2 = xamlType2.GetMember("Template");
				XamlMember member3 = xamlType4.GetMember("TargetType");
				IAmbientProvider ambientProvider2 = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
				if (ambientProvider2 == null)
				{
					throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
					{
						"Property",
						typeof(DependencyProperty).FullName
					}));
				}
				AmbientPropertyValue firstAmbientValue = ambientProvider2.GetFirstAmbientValue(list, new XamlMember[]
				{
					member,
					member2,
					member3
				});
				if (firstAmbientValue != null)
				{
					if (firstAmbientValue.Value is Type)
					{
						type = (Type)firstAmbientValue.Value;
					}
					else
					{
						if (!(firstAmbientValue.Value is TemplateContent))
						{
							throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
							{
								"Property",
								typeof(DependencyProperty).FullName
							}));
						}
						TemplateContent templateContent = firstAmbientValue.Value as TemplateContent;
						type = templateContent.OwnerTemplate.TargetTypeInternal;
					}
				}
			}
			if (type != null && text != null)
			{
				return DependencyProperty.FromName(text, type);
			}
			throw new NotSupportedException(SR.Get("ParserCannotConvertPropertyValue", new object[]
			{
				"Property",
				typeof(DependencyProperty).FullName
			}));
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000984B0 File Offset: 0x000966B0
		private static Type GetTypeFromName(XamlSchemaContext schemaContext, IAmbientProvider ambientProvider, string target)
		{
			XamlType xamlType = schemaContext.GetXamlType(typeof(FrameworkTemplate));
			XamlMember member = xamlType.GetMember("Template");
			AmbientPropertyValue firstAmbientValue = ambientProvider.GetFirstAmbientValue(new XamlType[]
			{
				xamlType
			}, new XamlMember[]
			{
				member
			});
			TemplateContent templateContent = firstAmbientValue.Value as TemplateContent;
			if (templateContent != null)
			{
				return templateContent.GetTypeForName(target).UnderlyingType;
			}
			return null;
		}
	}
}
