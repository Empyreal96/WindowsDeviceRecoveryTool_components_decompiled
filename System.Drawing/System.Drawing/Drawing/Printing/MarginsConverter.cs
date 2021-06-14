using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Drawing.Printing
{
	/// <summary>Provides a <see cref="T:System.Drawing.Printing.MarginsConverter" /> for <see cref="T:System.Drawing.Printing.Margins" />.</summary>
	// Token: 0x02000056 RID: 86
	public class MarginsConverter : ExpandableObjectConverter
	{
		/// <summary>Returns whether this converter can convert an object of the specified source type to the native type of the converter using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type from which you want to convert. </param>
		/// <returns>
		///     <see langword="true" /> if an object can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000725 RID: 1829 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Returns whether this converter can convert an object to the given destination type using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type to which you want to convert. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000726 RID: 1830 RVA: 0x00007CAA File Offset: 0x00005EAA
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to the converter's native type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that provides the language to convert to. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> does not contain values for all four margins. For example, "100,100,100,100" specifies 1 inch for the left, right, top, and bottom margins. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		// Token: 0x06000727 RID: 1831 RVA: 0x0001CEB4 File Offset: 0x0001B0B4
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
			if (array2.Length != 4)
			{
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
				{
					text2,
					"left, right, top, bottom"
				}));
			}
			return new Margins(array2[0], array2[1], array2[2], array2[3]);
		}

		/// <summary>Converts the given value object to the specified destination type using the specified context and arguments.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that provides the language to convert to. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to which to convert the value. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		// Token: 0x06000728 RID: 1832 RVA: 0x0001CF9C File Offset: 0x0001B19C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is Margins)
			{
				if (destinationType == typeof(string))
				{
					Margins margins = (Margins)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[4];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, margins.Left);
					array[num++] = converter.ConvertToString(context, culture, margins.Right);
					array[num++] = converter.ConvertToString(context, culture, margins.Top);
					array[num++] = converter.ConvertToString(context, culture, margins.Bottom);
					return string.Join(separator, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					Margins margins2 = (Margins)value;
					ConstructorInfo constructor = typeof(Margins).GetConstructor(new Type[]
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
							margins2.Left,
							margins2.Right,
							margins2.Top,
							margins2.Bottom
						});
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Creates an <see cref="T:System.Object" /> given a set of property values for the object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary" /> of new property values. </param>
		/// <returns>An <see cref="T:System.Object" /> representing the specified <see cref="T:System.Collections.IDictionary" />, or <see langword="null" /> if the object cannot be created.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="propertyValues" /> is <see langword="null" />.</exception>
		// Token: 0x06000729 RID: 1833 RVA: 0x0001D160 File Offset: 0x0001B360
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["Left"];
			object obj2 = propertyValues["Right"];
			object obj3 = propertyValues["Top"];
			object obj4 = propertyValues["Bottom"];
			if (obj == null || obj2 == null || obj4 == null || obj3 == null || !(obj is int) || !(obj2 is int) || !(obj4 is int) || !(obj3 is int))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			return new Margins((int)obj, (int)obj2, (int)obj3, (int)obj4);
		}

		/// <summary>Returns whether changing a value on this object requires a call to the <see cref="M:System.Drawing.Printing.MarginsConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> method to create a new value, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <returns>
		///     <see langword="true" /> if changing a property on this object requires a call to <see cref="M:System.Drawing.Printing.MarginsConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> to create a new value; otherwise, <see langword="false" />. This method always returns <see langword="true" />.</returns>
		// Token: 0x0600072A RID: 1834 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
