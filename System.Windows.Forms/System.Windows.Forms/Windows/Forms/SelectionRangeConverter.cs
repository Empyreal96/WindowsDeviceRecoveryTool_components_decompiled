using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.SelectionRange" /> objects to and from various other types.</summary>
	// Token: 0x02000356 RID: 854
	public class SelectionRangeConverter : TypeConverter
	{
		/// <summary>Determines if this converter can convert an object of the specified source type to the native type of the converter by querying the supplied type descriptor context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">The source <see cref="T:System.Type" /> to be converted. </param>
		/// <returns>
		///     <see langword="true" /> if the converter can perform the specified conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003527 RID: 13607 RVA: 0x000F22AE File Offset: 0x000F04AE
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(DateTime) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the specified destination type by using the specified context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">The destination <see cref="T:System.Type" /> to convert into. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the specified conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003528 RID: 13608 RVA: 0x000F22DE File Offset: 0x000F04DE
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(DateTime) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified value to the converter's native type by using the specified locale.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The locale information for the conversion. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />. </returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is of type <see cref="T:System.String" /> but could not be parsed into two strings representing dates.</exception>
		/// <exception cref="T:System.InvalidCastException">
		///         <paramref name="value" /> is of type <see cref="T:System.String" /> that was parsed into two strings, but the conversion of one or both into the <see cref="T:System.DateTime" /> type did not succeed.</exception>
		// Token: 0x06003529 RID: 13609 RVA: 0x000F2310 File Offset: 0x000F0510
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (text.Length == 0)
				{
					return new SelectionRange(DateTime.Now.Date, DateTime.Now.Date);
				}
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				char c = culture.TextInfo.ListSeparator[0];
				string[] array = text.Split(new char[]
				{
					c
				});
				if (array.Length == 2)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(DateTime));
					DateTime lower = (DateTime)converter.ConvertFromString(context, culture, array[0]);
					DateTime upper = (DateTime)converter.ConvertFromString(context, culture, array[1]);
					return new SelectionRange(lower, upper);
				}
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
				{
					text,
					"Start" + c.ToString() + " End"
				}));
			}
			else
			{
				if (value is DateTime)
				{
					DateTime dateTime = (DateTime)value;
					return new SelectionRange(dateTime, dateTime);
				}
				return base.ConvertFrom(context, culture, value);
			}
		}

		/// <summary>Converts the specified <see cref="T:System.Windows.Forms.SelectionRangeConverter" /> object to another type by using the specified culture.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The locale information for the conversion. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The destination <see cref="T:System.Type" /> to convert into. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///         <paramref name="value" /> cannot be converted to the <paramref name="destinationType" />.</exception>
		// Token: 0x0600352A RID: 13610 RVA: 0x000F2428 File Offset: 0x000F0628
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			SelectionRange selectionRange = value as SelectionRange;
			if (selectionRange != null)
			{
				if (destinationType == typeof(string))
				{
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					PropertyDescriptorCollection properties = base.GetProperties(value);
					string[] array = new string[properties.Count];
					for (int i = 0; i < properties.Count; i++)
					{
						object value2 = properties[i].GetValue(value);
						array[i] = TypeDescriptor.GetConverter(value2).ConvertToString(context, culture, value2);
					}
					return string.Join(separator, array);
				}
				if (destinationType == typeof(DateTime))
				{
					return selectionRange.Start;
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					ConstructorInfo constructor = typeof(SelectionRange).GetConstructor(new Type[]
					{
						typeof(DateTime),
						typeof(DateTime)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							selectionRange.Start,
							selectionRange.End
						});
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Creates a <see cref="T:System.Windows.Forms.SelectionRange" /> object by using the specified type descriptor and set of property values for that object.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary" /> that contains the new property values. </param>
		/// <returns>If successful, the newly created <see cref="T:System.Windows.Forms.SelectionRange" />; otherwise, this method throws an exception.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="propertyValues" /> is <see langword="null" /> or its Start and End elements could not be converted into a <see cref="T:System.Windows.Forms.SelectionRange" />.</exception>
		// Token: 0x0600352B RID: 13611 RVA: 0x000F2584 File Offset: 0x000F0784
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			object result;
			try
			{
				result = new SelectionRange((DateTime)propertyValues["Start"], (DateTime)propertyValues["End"]);
			}
			catch (InvalidCastException innerException)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException);
			}
			catch (NullReferenceException innerException2)
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"), innerException2);
			}
			return result;
		}

		/// <summary>Determines if changing a value on an instance should require a call to <see cref="Overload:System.Windows.Forms.SelectionRangeConverter.CreateInstance" /> to create a new value.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <returns>
		///     <see langword="true" /> if <see cref="Overload:System.Windows.Forms.SelectionRangeConverter.CreateInstance" /> must be called to make a change to one or more properties; otherwise <see langword="false" />.</returns>
		// Token: 0x0600352C RID: 13612 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Returns the set of filtered properties for the <see cref="T:System.Windows.Forms.SelectionRange" /> type </summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="value">An <see cref="T:System.Object" /> that specifies the type of array for which to get properties.</param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
		/// <returns>If successful, the set of properties that should be exposed for the <see cref="T:System.Windows.Forms.SelectionRange" /> type; otherwise, <see langword="null" />.</returns>
		// Token: 0x0600352D RID: 13613 RVA: 0x000F25FC File Offset: 0x000F07FC
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(SelectionRange), attributes);
			return properties.Sort(new string[]
			{
				"Start",
				"End"
			});
		}

		/// <summary>Determines whether the current object supports properties that use the specified type descriptor context.</summary>
		/// <param name="context">A <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <returns>
		///     <see langword="true" /> if <see cref="Overload:System.Windows.Forms.SelectionRangeConverter.GetProperties" /> can be called to find the properties of a <see cref="T:System.Windows.Forms.SelectionRange" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600352E RID: 13614 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
