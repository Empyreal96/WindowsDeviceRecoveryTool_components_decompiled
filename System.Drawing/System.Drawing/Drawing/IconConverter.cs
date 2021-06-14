using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;

namespace System.Drawing
{
	/// <summary>Converts an <see cref="T:System.Drawing.Icon" /> object from one data type to another. Access this class through the <see cref="T:System.ComponentModel.TypeDescriptor" /> object.</summary>
	// Token: 0x0200001F RID: 31
	public class IconConverter : ExpandableObjectConverter
	{
		/// <summary>Determines whether this <see cref="T:System.Drawing.IconConverter" /> can convert an instance of a specified type to an <see cref="T:System.Drawing.Icon" />, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that specifies the type you want to convert from. </param>
		/// <returns>This method returns <see langword="true" /> if this <see cref="T:System.Drawing.IconConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000319 RID: 793 RVA: 0x0000E9AA File Offset: 0x0000CBAA
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(byte[]) || (!(sourceType == typeof(InstanceDescriptor)) && base.CanConvertFrom(context, sourceType));
		}

		/// <summary>Determines whether this <see cref="T:System.Drawing.IconConverter" /> can convert an <see cref="T:System.Drawing.Icon" /> to an instance of a specified type, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that specifies the type you want to convert to. </param>
		/// <returns>This method returns <see langword="true" /> if this <see cref="T:System.Drawing.IconConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600031A RID: 794 RVA: 0x0000E9DC File Offset: 0x0000CBDC
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(Image) || destinationType == typeof(Bitmap) || destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts a specified object to an <see cref="T:System.Drawing.Icon" />.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that holds information about a specific culture. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to be converted. </param>
		/// <returns>If this method succeeds, it returns the <see cref="T:System.Drawing.Icon" /> that it created by converting the specified object. Otherwise, it throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed.</exception>
		// Token: 0x0600031B RID: 795 RVA: 0x0000EA2C File Offset: 0x0000CC2C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is byte[])
			{
				MemoryStream stream = new MemoryStream((byte[])value);
				return new Icon(stream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts an <see cref="T:System.Drawing.Icon" /> (or an object that can be cast to an <see cref="T:System.Drawing.Icon" />) to a specified type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies formatting conventions used by a particular culture. </param>
		/// <param name="value">The object to convert. This object should be of type icon or some type that can be cast to <see cref="T:System.Drawing.Icon" />. </param>
		/// <param name="destinationType">The type to convert the icon to. </param>
		/// <returns>This method returns the converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed.</exception>
		// Token: 0x0600031C RID: 796 RVA: 0x0000EA60 File Offset: 0x0000CC60
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(Image) || destinationType == typeof(Bitmap))
			{
				Icon icon = value as Icon;
				if (icon != null)
				{
					return icon.ToBitmap();
				}
			}
			if (destinationType == typeof(string))
			{
				if (value != null)
				{
					return value.ToString();
				}
				return SR.GetString("toStringNone");
			}
			else
			{
				if (!(destinationType == typeof(byte[])))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				if (value == null)
				{
					return new byte[0];
				}
				MemoryStream memoryStream = null;
				try
				{
					memoryStream = new MemoryStream();
					Icon icon2 = value as Icon;
					if (icon2 != null)
					{
						icon2.Save(memoryStream);
					}
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
				}
				if (memoryStream != null)
				{
					return memoryStream.ToArray();
				}
				return null;
			}
		}
	}
}
