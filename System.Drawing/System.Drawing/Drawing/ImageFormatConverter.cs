using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Imaging;
using System.Globalization;
using System.Reflection;

namespace System.Drawing
{
	/// <summary>
	///     <see cref="T:System.Drawing.ImageFormatConverter" /> is a class that can be used to convert <see cref="T:System.Drawing.Imaging.ImageFormat" /> objects from one data type to another. Access this class through the <see cref="T:System.ComponentModel.TypeDescriptor" /> object.</summary>
	// Token: 0x0200003F RID: 63
	public class ImageFormatConverter : TypeConverter
	{
		/// <summary>Indicates whether this converter can convert an object in the specified source type to the native type of the converter.</summary>
		/// <param name="context">A formatter context. This object can be used to get more information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="sourceType">The type you want to convert from. </param>
		/// <returns>This method returns <see langword="true" /> if this object can perform the conversion.</returns>
		// Token: 0x0600064E RID: 1614 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the specified destination type using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that specifies the context for this type conversion. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> that represents the type to which you want to convert this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object. </param>
		/// <returns>This method returns <see langword="true" /> if this object can perform the conversion.</returns>
		// Token: 0x0600064F RID: 1615 RVA: 0x00007CAA File Offset: 0x00005EAA
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
		/// <param name="context">A formatter context. This object can be used to get more information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies formatting conventions for a particular culture. </param>
		/// <param name="value">The object to convert. </param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		// Token: 0x06000650 RID: 1616 RVA: 0x0001A820 File Offset: 0x00018A20
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				string b = text.Trim();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					if (string.Equals(propertyInfo.Name, b, StringComparison.OrdinalIgnoreCase))
					{
						object[] index = null;
						return propertyInfo.GetValue(null, index);
					}
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the specified object to the specified type.</summary>
		/// <param name="context">A formatter context. This object can be used to get more information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies formatting conventions for a particular culture. </param>
		/// <param name="value">The object to convert. </param>
		/// <param name="destinationType">The type to convert the object to. </param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null." /></exception>
		// Token: 0x06000651 RID: 1617 RVA: 0x0001A880 File Offset: 0x00018A80
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ImageFormat)
			{
				PropertyInfo propertyInfo = null;
				PropertyInfo[] properties = this.GetProperties();
				foreach (PropertyInfo propertyInfo2 in properties)
				{
					if (propertyInfo2.GetValue(null, null).Equals(value))
					{
						propertyInfo = propertyInfo2;
						break;
					}
				}
				if (propertyInfo != null)
				{
					if (destinationType == typeof(string))
					{
						return propertyInfo.Name;
					}
					if (destinationType == typeof(InstanceDescriptor))
					{
						return new InstanceDescriptor(propertyInfo, null);
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001A927 File Offset: 0x00018B27
		private PropertyInfo[] GetProperties()
		{
			return typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public);
		}

		/// <summary>Gets a collection that contains a set of standard values for the data type this validator is designed for. Returns <see langword="null" /> if the data type does not support a standard set of values.</summary>
		/// <param name="context">A formatter context. This object can be used to get more information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <returns>A collection that contains a standard set of valid values, or <see langword="null" />. The default implementation always returns <see langword="null" />.</returns>
		// Token: 0x06000653 RID: 1619 RVA: 0x0001A93C File Offset: 0x00018B3C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				ArrayList arrayList = new ArrayList();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					object[] index = null;
					arrayList.Add(propertyInfo.GetValue(null, index));
				}
				this.values = new TypeConverter.StandardValuesCollection(arrayList.ToArray());
			}
			return this.values;
		}

		/// <summary>Indicates whether this object supports a standard set of values that can be picked from a list.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if the <see cref="Overload:System.Drawing.ImageFormatConverter.GetStandardValues" /> method should be called to find a common set of values the object supports.</returns>
		// Token: 0x06000654 RID: 1620 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0400056B RID: 1387
		private TypeConverter.StandardValuesCollection values;
	}
}
