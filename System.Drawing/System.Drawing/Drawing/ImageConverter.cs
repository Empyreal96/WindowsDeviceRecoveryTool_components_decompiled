using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Drawing
{
	/// <summary>
	///     <see cref="T:System.Drawing.ImageConverter" />  is a class that can be used to convert <see cref="T:System.Drawing.Image" /> objects from one data type to another. Access this class through the <see cref="T:System.ComponentModel.TypeDescriptor" /> object.</summary>
	// Token: 0x02000022 RID: 34
	public class ImageConverter : TypeConverter
	{
		/// <summary>Determines whether this <see cref="T:System.Drawing.ImageConverter" /> can convert an instance of a specified type to an <see cref="T:System.Drawing.Image" />, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that specifies the type you want to convert from. </param>
		/// <returns>This method returns <see langword="true" /> if this <see cref="T:System.Drawing.ImageConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000361 RID: 865 RVA: 0x00010148 File Offset: 0x0000E348
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == this.iconType || sourceType == typeof(byte[]) || (!(sourceType == typeof(InstanceDescriptor)) && base.CanConvertFrom(context, sourceType));
		}

		/// <summary>Determines whether this <see cref="T:System.Drawing.ImageConverter" /> can convert an <see cref="T:System.Drawing.Image" /> to an instance of a specified type, using the specified context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that specifies the type you want to convert to. </param>
		/// <returns>This method returns <see langword="true" /> if this <see cref="T:System.Drawing.ImageConverter" /> can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000362 RID: 866 RVA: 0x00010195 File Offset: 0x0000E395
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(byte[]) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts a specified object to an <see cref="T:System.Drawing.Image" />.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> that holds information about a specific culture. </param>
		/// <param name="value">The <see cref="T:System.Object" /> to be converted. </param>
		/// <returns>If this method succeeds, it returns the <see cref="T:System.Drawing.Image" /> that it created by converting the specified object. Otherwise, it throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		// Token: 0x06000363 RID: 867 RVA: 0x000101B4 File Offset: 0x0000E3B4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is Icon)
			{
				Icon icon = (Icon)value;
				return icon.ToBitmap();
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				Stream stream = this.GetBitmapStream(array);
				if (stream == null)
				{
					stream = new MemoryStream(array);
				}
				return Image.FromStream(stream);
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts an <see cref="T:System.Drawing.Image" /> (or an object that can be cast to an <see cref="T:System.Drawing.Image" />) to the specified type.</summary>
		/// <param name="context">A formatter context. This object can be used to get more information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies formatting conventions used by a particular culture. </param>
		/// <param name="value">The <see cref="T:System.Drawing.Image" /> to convert. </param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <see cref="T:System.Drawing.Image" /> to. </param>
		/// <returns>This method returns the converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
		// Token: 0x06000364 RID: 868 RVA: 0x00010208 File Offset: 0x0000E408
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				if (value != null)
				{
					Image image = (Image)value;
					return image.ToString();
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
				bool flag = false;
				MemoryStream memoryStream = null;
				Image image2 = null;
				try
				{
					memoryStream = new MemoryStream();
					image2 = (Image)value;
					if (image2.RawFormat.Equals(ImageFormat.Icon))
					{
						flag = true;
						image2 = new Bitmap(image2, image2.Width, image2.Height);
					}
					image2.Save(memoryStream);
				}
				finally
				{
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
					if (flag && image2 != null)
					{
						image2.Dispose();
					}
				}
				if (memoryStream != null)
				{
					return memoryStream.ToArray();
				}
				return null;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000102F8 File Offset: 0x0000E4F8
		private unsafe Stream GetBitmapStream(byte[] rawData)
		{
			try
			{
				try
				{
					fixed (byte* ptr = rawData)
					{
						IntPtr intPtr = (IntPtr)((void*)ptr);
						if (intPtr == IntPtr.Zero)
						{
							return null;
						}
						if (rawData.Length <= sizeof(SafeNativeMethods.OBJECTHEADER) || Marshal.ReadInt16(intPtr) != 7189)
						{
							return null;
						}
						SafeNativeMethods.OBJECTHEADER objectheader = (SafeNativeMethods.OBJECTHEADER)Marshal.PtrToStructure(intPtr, typeof(SafeNativeMethods.OBJECTHEADER));
						if (rawData.Length <= (int)(objectheader.headersize + 18))
						{
							return null;
						}
						string @string = Encoding.ASCII.GetString(rawData, (int)(objectheader.headersize + 12), 6);
						if (@string != "PBrush")
						{
							return null;
						}
						byte[] bytes = Encoding.ASCII.GetBytes("BM");
						int num = (int)(objectheader.headersize + 18);
						while (num < (int)(objectheader.headersize + 510) && num + 1 < rawData.Length)
						{
							if (bytes[0] == ptr[num] && bytes[1] == ptr[num + 1])
							{
								return new MemoryStream(rawData, num, rawData.Length - num);
							}
							num++;
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			catch (OutOfMemoryException)
			{
			}
			catch (ArgumentException)
			{
			}
			return null;
		}

		/// <summary>Gets the set of properties for this type.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <param name="value">The value of the object to get the properties for. </param>
		/// <param name="attributes">An array of <see cref="T:System.Attribute" /> objects that describe the properties.</param>
		/// <returns>The set of properties that should be exposed for this data type. If no properties should be exposed, this can return <see langword="null" />. The default implementation always returns <see langword="null" />.</returns>
		// Token: 0x06000366 RID: 870 RVA: 0x00010478 File Offset: 0x0000E678
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(typeof(Image), attributes);
		}

		/// <summary>Indicates whether this object supports properties. By default, this is <see langword="false" />.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if the <see cref="Overload:System.Drawing.ImageConverter.GetProperties" /> method should be called to find the properties of this object.</returns>
		// Token: 0x06000367 RID: 871 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000196 RID: 406
		private Type iconType = typeof(Icon);
	}
}
