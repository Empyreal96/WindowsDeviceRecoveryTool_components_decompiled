using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter to convert data for an image index to and from one data type to another for use by the <see cref="T:System.Windows.Forms.TreeView" /> control.</summary>
	// Token: 0x02000411 RID: 1041
	public class TreeViewImageIndexConverter : ImageIndexConverter
	{
		/// <summary>Gets a value indicating <see langword="null" /> is valid in the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> collection.</summary>
		/// <returns>
		///     <see langword="true" /> if <see langword="null" /> is valid in the standard values collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x06004756 RID: 18262 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		protected override bool IncludeNoneAsStandardValue
		{
			get
			{
				return false;
			}
		}

		/// <summary>Converts the specified value object to a 32-bit signed integer object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> to provide locale information. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x06004757 RID: 18263 RVA: 0x00130944 File Offset: 0x0012EB44
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				if (string.Compare(text, SR.GetString("toStringDefault"), true, culture) == 0)
				{
					return -1;
				}
				if (string.Compare(text, SR.GetString("toStringNone"), true, culture) == 0)
				{
					return -2;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the specified object to the specified destination type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that provides locale information.  </param>
		/// <param name="value">The object to convert, typically an index represented as an <see cref="T:System.Int32" />.</param>
		/// <param name="destinationType">The type to convert the object to, often a <see cref="T:System.String" />.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x06004758 RID: 18264 RVA: 0x0013099C File Offset: 0x0012EB9C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is int)
			{
				int num = (int)value;
				if (num == -1)
				{
					return SR.GetString("toStringDefault");
				}
				if (num == -2)
				{
					return SR.GetString("toStringNone");
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		/// <summary>Returns a collection of standard index values for the image list associated with the specified format context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context, which can be used to extract additional information about the environment this type converter is being invoked from. This parameter or properties of this parameter can be <see langword="null" />.</param>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid index values. If no image list is found, this collection will contain a single object with a value of -1.</returns>
		// Token: 0x06004759 RID: 18265 RVA: 0x00130A0C File Offset: 0x0012EC0C
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
						PropertyDescriptor propertyDescriptor3 = properties[base.ParentImageListProperty];
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
						int num = imageList.Images.Count + 2;
						object[] array = new object[num];
						array[num - 2] = -1;
						array[num - 1] = -2;
						for (int i = 0; i < num - 2; i++)
						{
							array[i] = i;
						}
						return new TypeConverter.StandardValuesCollection(array);
					}
				}
			}
			return new TypeConverter.StandardValuesCollection(new object[]
			{
				-1,
				-2
			});
		}
	}
}
