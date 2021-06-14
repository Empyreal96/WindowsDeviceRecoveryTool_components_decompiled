using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	/// <summary>Converts <see cref="T:System.Drawing.SizeF" /> objects from one type to another.</summary>
	// Token: 0x02000043 RID: 67
	public class SizeFConverter : TypeConverter
	{
		/// <summary>Returns a value indicating whether the converter can convert from the type specified to the <see cref="T:System.Drawing.SizeF" /> type, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be supplied.</param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> the represents the type you wish to convert from.</param>
		/// <returns>
		///     <see langword="true" /> to indicate the conversion can be performed; otherwise, <see langword="false" />. </returns>
		// Token: 0x060006A6 RID: 1702 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Returns a value indicating whether the <see cref="T:System.Drawing.SizeFConverter" /> can convert a <see cref="T:System.Drawing.SizeF" /> to the specified type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be supplied.</param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion otherwise, <see langword="false" />.</returns>
		// Token: 0x060006A7 RID: 1703 RVA: 0x00007CAA File Offset: 0x00005EAA
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x060006A8 RID: 1704 RVA: 0x0001B414 File Offset: 0x00019614
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
			float[] array2 = new float[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (float)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 2)
			{
				return new SizeF(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				text2,
				"Width,Height"
			}));
		}

		/// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x060006A9 RID: 1705 RVA: 0x0001B4F8 File Offset: 0x000196F8
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is SizeF)
			{
				SizeF sizeF = (SizeF)value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string separator = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
				string[] array = new string[2];
				int num = 0;
				array[num++] = converter.ConvertToString(context, culture, sizeF.Width);
				array[num++] = converter.ConvertToString(context, culture, sizeF.Height);
				return string.Join(separator, array);
			}
			if (destinationType == typeof(InstanceDescriptor) && value is SizeF)
			{
				SizeF sizeF2 = (SizeF)value;
				ConstructorInfo constructor = typeof(SizeF).GetConstructor(new Type[]
				{
					typeof(float),
					typeof(float)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						sizeF2.Width,
						sizeF2.Height
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Creates an instance of a <see cref="T:System.Drawing.SizeF" /> with the specified property values using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be supplied.</param>
		/// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary" /> containing property names and values.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the new <see cref="T:System.Drawing.SizeF" />, or <see langword="null" /> if the object cannot be created.</returns>
		// Token: 0x060006AA RID: 1706 RVA: 0x0001B653 File Offset: 0x00019853
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			return new SizeF((float)propertyValues["Width"], (float)propertyValues["Height"]);
		}

		/// <summary>Returns a value indicating whether changing a value on this object requires a call to the <see cref="Overload:System.Drawing.SizeFConverter.CreateInstance" /> method to create a new value.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. This may be <see langword="null" />.</param>
		/// <returns>Always returns <see langword="true" />.</returns>
		// Token: 0x060006AB RID: 1707 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Retrieves a set of properties for the <see cref="T:System.Drawing.SizeF" /> type using the specified context and attributes.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be supplied.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to return properties for.</param>
		/// <param name="attributes">An array of <see cref="T:System.Attribute" /> objects that describe the properties.</param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> containing the properties.</returns>
		// Token: 0x060006AC RID: 1708 RVA: 0x0001B680 File Offset: 0x00019880
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SizeF), attributes);
			return properties.Sort(new string[]
			{
				"Width",
				"Height"
			});
		}

		/// <summary>Returns whether the <see cref="T:System.Drawing.SizeF" /> type supports properties.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> through which additional context can be supplied.</param>
		/// <returns>Always returns <see langword="true" />.</returns>
		// Token: 0x060006AD RID: 1709 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
