using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	/// <summary>Converts rectangles from one data type to another. Access this class through the <see cref="T:System.ComponentModel.TypeDescriptor" />.</summary>
	// Token: 0x0200002D RID: 45
	public class RectangleConverter : TypeConverter
	{
		/// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
		/// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="sourceType">The type you want to convert from. </param>
		/// <returns>This method returns <see langword="true" /> if this object can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000497 RID: 1175 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> object that provides a format context. This can be <see langword="null" />, so you should always check. Also, properties on the context object can also return <see langword="null" />.</param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> object that represents the type you want to convert to. </param>
		/// <returns>This method returns <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000498 RID: 1176 RVA: 0x00007CAA File Offset: 0x00005EAA
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given object to a <see cref="T:System.Drawing.Rectangle" /> object.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">An <see cref="T:System.Globalization.CultureInfo" /> that contains culture specific information, such as the language, calendar, and cultural conventions associated with a specific culture. It is based on the RFC 1766 standard. </param>
		/// <param name="value">The object to convert. </param>
		/// <returns>The converted object. </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		// Token: 0x06000499 RID: 1177 RVA: 0x00015C18 File Offset: 0x00013E18
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text2 = text.Trim();
			if (text2.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char c = culture.TextInfo.ListSeparator[0];
			string[] array = text2.Split(new char[]
			{
				c
			});
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 4)
			{
				return new Rectangle(array2[0], array2[1], array2[2], array2[3]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				"text",
				text2,
				"x, y, width, height"
			}));
		}

		/// <summary>Converts the specified object to the specified type.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">An <see cref="T:System.Globalization.CultureInfo" /> that contains culture specific information, such as the language, calendar, and cultural conventions associated with a specific culture. It is based on the RFC 1766 standard. </param>
		/// <param name="value">The object to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		// Token: 0x0600049A RID: 1178 RVA: 0x00015D0C File Offset: 0x00013F0C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Rectangle)
			{
				if (destinationType == typeof(string))
				{
					Rectangle rectangle = (Rectangle)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, rectangle.X);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Y);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Width);
					array[num++] = converter.ConvertToString(context, culture, rectangle.Height);
					return string.Join(separator, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Rectangle rectangle2 = (Rectangle)value;
					ConstructorInfo constructor = typeof(Rectangle).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int),
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							rectangle2.X,
							rectangle2.Y,
							rectangle2.Width,
							rectangle2.Height
						});
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Creates an instance of this type given a set of property values for the object. This is useful for objects that are immutable but still want to provide changeable properties.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be provided. </param>
		/// <param name="propertyValues">A dictionary of new property values. The dictionary contains a series of name-value pairs, one for each property returned from a call to the <see cref="M:System.Drawing.RectangleConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" /> method. </param>
		/// <returns>The newly created object, or <see langword="null" /> if the object could not be created. The default implementation returns <see langword="null" />.</returns>
		// Token: 0x0600049B RID: 1179 RVA: 0x00015ED4 File Offset: 0x000140D4
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["X"];
			object obj2 = propertyValues["Y"];
			object obj3 = propertyValues["Width"];
			object obj4 = propertyValues["Height"];
			if (obj == null || obj2 == null || obj3 == null || obj4 == null || !(obj is int) || !(obj2 is int) || !(obj3 is int) || !(obj4 is int))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			return new Rectangle((int)obj, (int)obj2, (int)obj3, (int)obj4);
		}

		/// <summary>Determines if changing a value on this object should require a call to <see cref="M:System.Drawing.RectangleConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> to create a new value.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if <see cref="M:System.Drawing.RectangleConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> should be called when a change is made to one or more properties of this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600049C RID: 1180 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Retrieves the set of properties for this type. By default, a type does not return any properties. </summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be provided. </param>
		/// <param name="value">The value of the object to get the properties for. </param>
		/// <param name="attributes">An array of <see cref="T:System.Attribute" /> objects that describe the properties. </param>
		/// <returns>The set of properties that should be exposed for this data type. If no properties should be exposed, this may return <see langword="null" />. The default implementation always returns <see langword="null" />.</returns>
		// Token: 0x0600049D RID: 1181 RVA: 0x00015F80 File Offset: 0x00014180
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Rectangle), attributes);
			return properties.Sort(new string[]
			{
				"X",
				"Y",
				"Width",
				"Height"
			});
		}

		/// <summary>Determines if this object supports properties. By default, this is <see langword="false" />.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if <see cref="M:System.Drawing.RectangleConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" /> should be called to find the properties of this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600049E RID: 1182 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
