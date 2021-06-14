using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Markup;
using MS.Internal.Data;

namespace System.Windows
{
	/// <summary>Provides a type converter for <see cref="T:System.Windows.PropertyPath" /> objects. </summary>
	// Token: 0x020000E3 RID: 227
	public sealed class PropertyPathConverter : TypeConverter
	{
		/// <summary>Returns whether this converter can convert an object of one type to the <see cref="T:System.Windows.PropertyPath" /> type.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="sourceType" /> is type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007C7 RID: 1991 RVA: 0x00018B21 File Offset: 0x00016D21
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Returns whether this converter can convert the object to the <see cref="T:System.Windows.PropertyPath" /> type.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="destinationType" /> is type <see cref="T:System.String" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007C8 RID: 1992 RVA: 0x00018B21 File Offset: 0x00016D21
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		/// <summary>Converts the specified value to the <see cref="T:System.Windows.PropertyPath" /> type.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="source">The object to convert to a <see cref="T:System.Windows.PropertyPath" />. This is expected to be a string.</param>
		/// <returns>The converted <see cref="T:System.Windows.PropertyPath" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> was provided as <see langword="null." /></exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="source" /> was not <see langword="null" />, but was not of the expected <see cref="T:System.String" /> type.</exception>
		// Token: 0x060007C9 RID: 1993 RVA: 0x00018B38 File Offset: 0x00016D38
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (source is string)
			{
				return new PropertyPath((string)source, typeDescriptorContext);
			}
			throw new ArgumentException(SR.Get("CannotConvertType", new object[]
			{
				source.GetType().FullName,
				typeof(PropertyPath)
			}));
		}

		/// <summary>Converts the specified value object to the <see cref="T:System.Windows.PropertyPath" /> type.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Windows.PropertyPath" /> to convert.</param>
		/// <param name="destinationType">The destination type. This is expected to be the <see cref="T:System.String" /> type.</param>
		/// <returns>The converted destination <see cref="T:System.String" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> was provided as <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> was not <see langword="null" />, but was not of the expected <see cref="T:System.Windows.PropertyPath" /> type.- or -The <paramref name="destinationType" /> was not the <see cref="T:System.String" /> type.</exception>
		// Token: 0x060007CA RID: 1994 RVA: 0x00018B98 File Offset: 0x00016D98
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (null == destinationType)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				throw new ArgumentException(SR.Get("CannotConvertType", new object[]
				{
					typeof(PropertyPath),
					destinationType.FullName
				}));
			}
			PropertyPath propertyPath = value as PropertyPath;
			if (propertyPath == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(PropertyPath)
				}), "value");
			}
			if (propertyPath.PathParameters.Count == 0)
			{
				return propertyPath.Path;
			}
			string path = propertyPath.Path;
			Collection<object> pathParameters = propertyPath.PathParameters;
			XamlDesignerSerializationManager xamlDesignerSerializationManager = (typeDescriptorContext == null) ? null : (typeDescriptorContext.GetService(typeof(XamlDesignerSerializationManager)) as XamlDesignerSerializationManager);
			ValueSerializer valueSerializer = null;
			IValueSerializerContext valueSerializerContext = null;
			if (xamlDesignerSerializationManager == null)
			{
				valueSerializerContext = (typeDescriptorContext as IValueSerializerContext);
				if (valueSerializerContext != null)
				{
					valueSerializer = ValueSerializer.GetSerializerFor(typeof(Type), valueSerializerContext);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i] == '(')
				{
					int num2 = i + 1;
					while (num2 < path.Length && path[num2] != ')')
					{
						num2++;
					}
					int index;
					if (int.TryParse(path.Substring(i + 1, num2 - i - 1), NumberStyles.Integer, TypeConverterHelper.InvariantEnglishUS.NumberFormat, out index))
					{
						stringBuilder.Append(path.Substring(num, i - num + 1));
						object obj = pathParameters[index];
						DependencyProperty dependencyProperty;
						PropertyInfo propertyInfo;
						PropertyDescriptor propertyDescriptor;
						DynamicObjectAccessor dynamicObjectAccessor;
						PropertyPath.DowncastAccessor(obj, out dependencyProperty, out propertyInfo, out propertyDescriptor, out dynamicObjectAccessor);
						Type type;
						string text;
						if (dependencyProperty != null)
						{
							type = dependencyProperty.OwnerType;
							text = dependencyProperty.Name;
						}
						else if (propertyInfo != null)
						{
							type = propertyInfo.DeclaringType;
							text = propertyInfo.Name;
						}
						else if (propertyDescriptor != null)
						{
							type = propertyDescriptor.ComponentType;
							text = propertyDescriptor.Name;
						}
						else if (dynamicObjectAccessor != null)
						{
							type = dynamicObjectAccessor.OwnerType;
							text = dynamicObjectAccessor.PropertyName;
						}
						else
						{
							type = obj.GetType();
							text = null;
						}
						if (valueSerializer != null)
						{
							stringBuilder.Append(valueSerializer.ConvertToString(type, valueSerializerContext));
						}
						else
						{
							string text2 = null;
							if (text2 != null && text2 != string.Empty)
							{
								stringBuilder.Append(text2);
								stringBuilder.Append(':');
							}
							stringBuilder.Append(type.Name);
						}
						if (text != null)
						{
							stringBuilder.Append('.');
							stringBuilder.Append(text);
							stringBuilder.Append(')');
						}
						else
						{
							stringBuilder.Append(')');
							text = (obj as string);
							if (text == null)
							{
								TypeConverter converter = TypeDescriptor.GetConverter(type);
								if (converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
								{
									try
									{
										text = converter.ConvertToString(obj);
									}
									catch (NotSupportedException)
									{
									}
								}
							}
							stringBuilder.Append(text);
						}
						i = num2;
						num = num2 + 1;
					}
				}
			}
			if (num < path.Length)
			{
				stringBuilder.Append(path.Substring(num));
			}
			return stringBuilder.ToString();
		}
	}
}
