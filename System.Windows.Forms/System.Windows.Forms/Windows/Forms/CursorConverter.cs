using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.Cursor" /> objects to and from various other representations. </summary>
	// Token: 0x02000167 RID: 359
	public class CursorConverter : TypeConverter
	{
		/// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
		/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="sourceType">The type you wish to convert from. </param>
		/// <returns>
		///     <see langword="true" /> if this object can perform the conversion.</returns>
		// Token: 0x060010B0 RID: 4272 RVA: 0x0003B34B File Offset: 0x0003954B
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x060010B1 RID: 4273 RVA: 0x0003B37B File Offset: 0x0003957B
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given object to the type of this converter, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x060010B2 RID: 4274 RVA: 0x0003B3AC File Offset: 0x000395AC
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string b = ((string)value).Trim();
				foreach (PropertyInfo propertyInfo in this.GetProperties())
				{
					if (string.Equals(propertyInfo.Name, b, StringComparison.OrdinalIgnoreCase))
					{
						object[] index = null;
						return propertyInfo.GetValue(null, index);
					}
				}
			}
			if (value is byte[])
			{
				MemoryStream stream = new MemoryStream((byte[])value);
				return new Cursor(stream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the given value object to the specified type, using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x060010B3 RID: 4275 RVA: 0x0003B428 File Offset: 0x00039628
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null)
			{
				PropertyInfo[] properties = this.GetProperties();
				int num = -1;
				for (int i = 0; i < properties.Length; i++)
				{
					PropertyInfo propertyInfo = properties[i];
					object[] index = null;
					Cursor cursor = (Cursor)propertyInfo.GetValue(null, index);
					if (cursor == (Cursor)value)
					{
						if (cursor == value)
						{
							return propertyInfo.Name;
						}
						num = i;
					}
				}
				if (num != -1)
				{
					return properties[num].Name;
				}
				throw new FormatException(SR.GetString("CursorCannotCovertToString"));
			}
			else
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Cursor)
				{
					PropertyInfo[] properties2 = this.GetProperties();
					foreach (PropertyInfo propertyInfo2 in properties2)
					{
						if (propertyInfo2.GetValue(null, null) == value)
						{
							return new InstanceDescriptor(propertyInfo2, null);
						}
					}
				}
				if (!(destinationType == typeof(byte[])))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value != null)
				{
					MemoryStream memoryStream = new MemoryStream();
					Cursor cursor2 = (Cursor)value;
					cursor2.SavePicture(memoryStream);
					memoryStream.Close();
					return memoryStream.ToArray();
				}
				return new byte[0];
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0003B56D File Offset: 0x0003976D
		private PropertyInfo[] GetProperties()
		{
			return typeof(Cursors).GetProperties(BindingFlags.Static | BindingFlags.Public);
		}

		/// <summary>Retrieves a collection containing a set of standard values for the data type this validator is designed for. This will return <see langword="null" /> if the data type does not support a standard set of values.</summary>
		/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <returns>A collection containing a standard set of valid values, or <see langword="null" />. The default implementation always returns <see langword="null" />.</returns>
		// Token: 0x060010B5 RID: 4277 RVA: 0x0003B580 File Offset: 0x00039780
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

		/// <summary>Determines if this object supports a standard set of values that can be picked from a list.</summary>
		/// <param name="context">A type descriptor through which additional context may be provided. </param>
		/// <returns>Returns <see langword="true" /> if <see langword="GetStandardValues" /> should be called to find a common set of values the object supports.</returns>
		// Token: 0x060010B6 RID: 4278 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0400089F RID: 2207
		private TypeConverter.StandardValuesCollection values;
	}
}
