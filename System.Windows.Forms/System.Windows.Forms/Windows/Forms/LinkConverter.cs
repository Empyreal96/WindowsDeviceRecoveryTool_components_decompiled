using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Provides a type converter for <see cref="T:System.Windows.Forms.LinkLabel.Link" /> objects.</summary>
	// Token: 0x020002B4 RID: 692
	public class LinkConverter : TypeConverter
	{
		/// <summary>Retrieves a value that determines if this <see cref="T:System.Windows.Forms.LinkConverter" /> can convert an object having the specified context and source type to the native type of the <see cref="T:System.Windows.Forms.LinkConverter" />.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> providing contextual information about the object to be converted.</param>
		/// <param name="sourceType">The type of the object to be converted.</param>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Windows.Forms.LinkConverter" /> can convert the specified object; otherwise, <see langword="false" />. </returns>
		// Token: 0x060027EA RID: 10218 RVA: 0x000B9F74 File Offset: 0x000B8174
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Retrieves a value that determines if this <see cref="T:System.Windows.Forms.LinkConverter" /> can convert an object having the specified context to the specified type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> providing contextual information about the object to be converted.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Windows.Forms.LinkConverter" /> can convert the specified object; otherwise, <see langword="false" />. </returns>
		// Token: 0x060027EB RID: 10219 RVA: 0x000B9F92 File Offset: 0x000B8192
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to the native type of the <see cref="T:System.Windows.Forms.LinkConverter" />.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> providing contextual information about the object to be converted.</param>
		/// <param name="culture">Cultural attributes of the object to be converted. If this parameter is <see langword="null" />, the <see cref="P:System.Globalization.CultureInfo.CurrentCulture" /> property value is used.</param>
		/// <param name="value">The object to be converted.</param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.ArgumentException">The text of the object to be converted could not be parsed.</exception>
		// Token: 0x060027EC RID: 10220 RVA: 0x000B9FC4 File Offset: 0x000B81C4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = ((string)value).Trim();
			if (text.Length == 0)
			{
				return null;
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
			int[] array2 = new int[array.Length];
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = (int)converter.ConvertFromString(context, culture, array[i]);
			}
			if (array2.Length == 2)
			{
				return new LinkLabel.Link(array2[0], array2[1]);
			}
			throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				text,
				"Start, Length"
			}));
		}

		/// <summary>Converts the specified object to another type.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> providing contextual information about the object to be converted.</param>
		/// <param name="culture">Cultural attributes of the object to be converted. If this parameter is <see langword="null" />, the <see cref="P:System.Globalization.CultureInfo.CurrentCulture" /> property value is used.</param>
		/// <param name="value">The object to be converted.</param>
		/// <param name="destinationType">The type to convert the object to.</param>
		/// <returns>The converted object.</returns>
		/// <exception cref="T:System.NotSupportedException">The object cannot be converted to the destination type.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> is <see langword="null" />.</exception>
		// Token: 0x060027ED RID: 10221 RVA: 0x000BA0A0 File Offset: 0x000B82A0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is LinkLabel.Link)
			{
				if (destinationType == typeof(string))
				{
					LinkLabel.Link link = (LinkLabel.Link)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[2];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, link.Start);
					array[num++] = converter.ConvertToString(context, culture, link.Length);
					return string.Join(separator, array);
				}
				if (destinationType == typeof(InstanceDescriptor))
				{
					LinkLabel.Link link2 = (LinkLabel.Link)value;
					if (link2.LinkData == null)
					{
						MemberInfo constructor = typeof(LinkLabel.Link).GetConstructor(new Type[]
						{
							typeof(int),
							typeof(int)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								link2.Start,
								link2.Length
							}, true);
						}
					}
					else
					{
						MemberInfo constructor = typeof(LinkLabel.Link).GetConstructor(new Type[]
						{
							typeof(int),
							typeof(int),
							typeof(object)
						});
						if (constructor != null)
						{
							return new InstanceDescriptor(constructor, new object[]
							{
								link2.Start,
								link2.Length,
								link2.LinkData
							}, true);
						}
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
