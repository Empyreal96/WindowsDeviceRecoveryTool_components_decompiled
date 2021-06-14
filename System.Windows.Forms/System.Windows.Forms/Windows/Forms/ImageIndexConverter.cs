using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert data for an image index to and from a string.</summary>
	// Token: 0x02000281 RID: 641
	public class ImageIndexConverter : Int32Converter
	{
		/// <summary>Gets or sets a value indicating whether a <see langword="none" /> or <see langword="null" /> value is valid in the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> collection.</summary>
		/// <returns>
		///     <see langword="true" /> if a <see langword="none" /> or <see langword="null" /> value is valid in the standard values collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x0000E214 File Offset: 0x0000C414
		protected virtual bool IncludeNoneAsStandardValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x000B5917 File Offset: 0x000B3B17
		// (set) Token: 0x06002665 RID: 9829 RVA: 0x000B591F File Offset: 0x000B3B1F
		internal string ParentImageListProperty
		{
			get
			{
				return this.parentImageListProperty;
			}
			set
			{
				this.parentImageListProperty = value;
			}
		}

		/// <summary>Converts the specified value object to a 32-bit signed integer object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> to provide locale information. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.Exception">The conversion could not be performed. </exception>
		// Token: 0x06002666 RID: 9830 RVA: 0x000B5928 File Offset: 0x000B3B28
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null && string.Compare(text, SR.GetString("toStringNone"), true, culture) == 0)
			{
				return -1;
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the specified object to the specified destination type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that provides locale information. </param>
		/// <param name="value">The object to convert, typically an index represented as an <see cref="T:System.Int32" />.</param>
		/// <param name="destinationType">The type to convert the object to, often a <see cref="T:System.String" />.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted <paramref name="value" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.NotSupportedException">The specified <paramref name="value" /> could not be converted to the specified <paramref name="destinationType" />.</exception>
		// Token: 0x06002667 RID: 9831 RVA: 0x000B5964 File Offset: 0x000B3B64
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is int && (int)value == -1)
			{
				return SR.GetString("toStringNone");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Returns a collection of standard index values for the image list associated with the specified format context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />. </param>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid index values. If no image list is found, this collection will contain a single object with a value of -1.</returns>
		// Token: 0x06002668 RID: 9832 RVA: 0x000B59C0 File Offset: 0x000B3BC0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance != null)
			{
				object obj = context.Instance;
				PropertyDescriptor propertyDescriptor = ImageListUtils.GetImageListProperty(context.PropertyDescriptor, ref obj);
				while (obj != null && propertyDescriptor == null)
				{
					PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
					foreach (object obj2 in properties)
					{
						PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)obj2;
						if (typeof(ImageList).IsAssignableFrom(propertyDescriptor2.PropertyType))
						{
							propertyDescriptor = propertyDescriptor2;
							break;
						}
					}
					if (propertyDescriptor == null)
					{
						PropertyDescriptor propertyDescriptor3 = properties[this.ParentImageListProperty];
						if (propertyDescriptor3 != null)
						{
							obj = propertyDescriptor3.GetValue(obj);
						}
						else
						{
							obj = null;
						}
					}
				}
				if (propertyDescriptor != null)
				{
					ImageList imageList = (ImageList)propertyDescriptor.GetValue(obj);
					if (imageList != null)
					{
						int count = imageList.Images.Count;
						object[] array;
						if (this.IncludeNoneAsStandardValue)
						{
							array = new object[count + 1];
							array[count] = -1;
						}
						else
						{
							array = new object[count];
						}
						for (int i = 0; i < count; i++)
						{
							array[i] = i;
						}
						return new TypeConverter.StandardValuesCollection(array);
					}
				}
			}
			if (this.IncludeNoneAsStandardValue)
			{
				return new TypeConverter.StandardValuesCollection(new object[]
				{
					-1
				});
			}
			return new TypeConverter.StandardValuesCollection(new object[0]);
		}

		/// <summary>Determines if the list of standard values returned from the <see cref="Overload:System.Windows.Forms.ImageIndexConverter.GetStandardValues" /> method is an exclusive list. </summary>
		/// <param name="context">A formatter context. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="Overload:System.Windows.Forms.ImageIndexConverter.GetStandardValues" /> method returns an exclusive list of valid values; otherwise, <see langword="false" />. This implementation always returns <see langword="false" />.</returns>
		// Token: 0x06002669 RID: 9833 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		/// <summary>Determines if the type converter supports a standard set of values that can be picked from a list.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="Overload:System.Windows.Forms.ImageIndexConverter.GetStandardValues" /> method returns a standard set of values; otherwise, <see langword="false" />. Always returns <see langword="true" />.</returns>
		// Token: 0x0600266A RID: 9834 RVA: 0x0000E214 File Offset: 0x0000C414
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04001033 RID: 4147
		private string parentImageListProperty = "Parent";
	}
}
