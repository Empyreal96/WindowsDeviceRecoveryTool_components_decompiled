using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
	/// <summary>Represents an area within a <see cref="T:System.Windows.Forms.LinkLabel" /> control that represents a hyperlink within the control.</summary>
	// Token: 0x020002B0 RID: 688
	[TypeConverter(typeof(LinkArea.LinkAreaConverter))]
	[Serializable]
	public struct LinkArea
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkArea" /> class.</summary>
		/// <param name="start">The zero-based starting location of the link area within the text of the <see cref="T:System.Windows.Forms.LinkLabel" />. </param>
		/// <param name="length">The number of characters, after the starting character, to include in the link area. </param>
		// Token: 0x060027D9 RID: 10201 RVA: 0x000B9E49 File Offset: 0x000B8049
		public LinkArea(int start, int length)
		{
			this.start = start;
			this.length = length;
		}

		/// <summary>Gets or sets the starting location of the link area within the text of the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>The location within the text of the <see cref="T:System.Windows.Forms.LinkLabel" /> control where the link starts.</returns>
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060027DA RID: 10202 RVA: 0x000B9E59 File Offset: 0x000B8059
		// (set) Token: 0x060027DB RID: 10203 RVA: 0x000B9E61 File Offset: 0x000B8061
		public int Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		/// <summary>Gets or sets the number of characters in the link area.</summary>
		/// <returns>The number of characters, including spaces, in the link area.</returns>
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x000B9E6A File Offset: 0x000B806A
		// (set) Token: 0x060027DD RID: 10205 RVA: 0x000B9E72 File Offset: 0x000B8072
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.LinkArea" /> is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the specified start and length return an empty link area; otherwise, <see langword="false" />.</returns>
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x000B9E7B File Offset: 0x000B807B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsEmpty
		{
			get
			{
				return this.length == this.start && this.start == 0;
			}
		}

		/// <summary>Determines whether this <see cref="T:System.Windows.Forms.LinkArea" /> is equal to the specified object.</summary>
		/// <param name="o">The object to compare to this <see cref="T:System.Windows.Forms.LinkArea" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to the current <see cref="T:System.Windows.Forms.LinkArea" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027DF RID: 10207 RVA: 0x000B9E98 File Offset: 0x000B8098
		public override bool Equals(object o)
		{
			if (!(o is LinkArea))
			{
				return false;
			}
			LinkArea linkArea = (LinkArea)o;
			return this == linkArea;
		}

		/// <summary>Returns the fully qualified type name of this instance.</summary>
		/// <returns>A <see cref="T:System.String" /> containing a fully qualified type name.</returns>
		// Token: 0x060027E0 RID: 10208 RVA: 0x000B9EC4 File Offset: 0x000B80C4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Start=",
				this.Start.ToString(CultureInfo.CurrentCulture),
				", Length=",
				this.Length.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		/// <summary>Returns a value indicating whether two instances of the <see cref="T:System.Windows.Forms.LinkArea" /> class are equal.</summary>
		/// <param name="linkArea1">A <see cref="T:System.Windows.Forms.LinkArea" /> to compare.</param>
		/// <param name="linkArea2">A <see cref="T:System.Windows.Forms.LinkArea" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if two instances of the <see cref="T:System.Windows.Forms.LinkArea" /> class are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027E1 RID: 10209 RVA: 0x000B9F20 File Offset: 0x000B8120
		public static bool operator ==(LinkArea linkArea1, LinkArea linkArea2)
		{
			return linkArea1.start == linkArea2.start && linkArea1.length == linkArea2.length;
		}

		/// <summary>Returns a value indicating whether two instances of the <see cref="T:System.Windows.Forms.LinkArea" /> class are not equal.</summary>
		/// <param name="linkArea1">A <see cref="T:System.Windows.Forms.LinkArea" /> to compare.</param>
		/// <param name="linkArea2">A <see cref="T:System.Windows.Forms.LinkArea" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if two instances of the <see cref="T:System.Windows.Forms.LinkArea" /> class are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060027E2 RID: 10210 RVA: 0x000B9F40 File Offset: 0x000B8140
		public static bool operator !=(LinkArea linkArea1, LinkArea linkArea2)
		{
			return !(linkArea1 == linkArea2);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		// Token: 0x060027E3 RID: 10211 RVA: 0x000B9F4C File Offset: 0x000B814C
		public override int GetHashCode()
		{
			return this.start << 4 | this.length;
		}

		// Token: 0x04001173 RID: 4467
		private int start;

		// Token: 0x04001174 RID: 4468
		private int length;

		/// <summary>Provides a type converter to convert <see cref="T:System.Windows.Forms.LinkArea.LinkAreaConverter" /> objects to and from various other representations.</summary>
		// Token: 0x020005FE RID: 1534
		public class LinkAreaConverter : TypeConverter
		{
			/// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
			/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be null, so you should always check. Also, properties on the context object may also return null. </param>
			/// <param name="sourceType">The type you wish to convert from. </param>
			/// <returns>True if this object can perform the conversion.</returns>
			// Token: 0x06005BE5 RID: 23525 RVA: 0x000B9F74 File Offset: 0x000B8174
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you wish to convert to. </param>
			/// <returns>
			///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005BE6 RID: 23526 RVA: 0x0001F8F0 File Offset: 0x0001DAF0
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
			{
				return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
			}

			/// <summary>Converts the given object to the converter's native type.</summary>
			/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be null, so you should always check. Also, properties on the context object may also return null. </param>
			/// <param name="culture">An optional culture info. If not supplied, the current culture is assumed. </param>
			/// <param name="value">The object to convert. </param>
			/// <returns>The converted object. This will throw an exception if the conversion could not be performed.</returns>
			// Token: 0x06005BE7 RID: 23527 RVA: 0x0017FDD8 File Offset: 0x0017DFD8
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
					return new LinkArea(array2[0], array2[1]);
				}
				throw new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
				{
					text,
					"start, length"
				}));
			}

			/// <summary>Converts the given object to another type. The most common types to convert are to and from a string object. The default implementation will make a call to <see cref="M:System.Windows.Forms.LinkArea.ToString" /> on the object if the object is valid and if the destination type is string. If this cannot convert to the destination type, this will throw a <see cref="T:System.NotSupportedException" />.</summary>
			/// <param name="context">A formatter context. This object can be used to extract additional information about the environment this converter is being invoked from. This may be null, so you should always check. Also, properties on the context object may also return null. </param>
			/// <param name="culture">An optional culture info. If not supplied the current culture is assumed. </param>
			/// <param name="value">The object to convert. </param>
			/// <param name="destinationType">The type to convert the object to. </param>
			/// <returns>The converted object.</returns>
			// Token: 0x06005BE8 RID: 23528 RVA: 0x0017FEB8 File Offset: 0x0017E0B8
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType == null)
				{
					throw new ArgumentNullException("destinationType");
				}
				if (destinationType == typeof(string) && value is LinkArea)
				{
					LinkArea linkArea = (LinkArea)value;
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					string separator = culture.TextInfo.ListSeparator + " ";
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(int));
					string[] array = new string[2];
					int num = 0;
					array[num++] = converter.ConvertToString(context, culture, linkArea.Start);
					array[num++] = converter.ConvertToString(context, culture, linkArea.Length);
					return string.Join(separator, array);
				}
				if (destinationType == typeof(InstanceDescriptor) && value is LinkArea)
				{
					LinkArea linkArea2 = (LinkArea)value;
					ConstructorInfo constructor = typeof(LinkArea).GetConstructor(new Type[]
					{
						typeof(int),
						typeof(int)
					});
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, new object[]
						{
							linkArea2.Start,
							linkArea2.Length
						});
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}

			/// <summary>Creates an instance of this type, given a set of property values for the object. This is useful for objects that are immutable, but still want to provide changeable properties.</summary>
			/// <param name="context">A type descriptor through which additional context may be provided. </param>
			/// <param name="propertyValues">A dictionary of new property values. The dictionary contains a series of name-value pairs, one for each property returned from <see cref="M:System.Windows.Forms.LinkArea.LinkAreaConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" />. </param>
			/// <returns>The newly created object, or null if the object could not be created. The default implementation returns null.</returns>
			// Token: 0x06005BE9 RID: 23529 RVA: 0x00180013 File Offset: 0x0017E213
			public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
			{
				return new LinkArea((int)propertyValues["Start"], (int)propertyValues["Length"]);
			}

			/// <summary>Determines if changing a value on this object should require a call to <see cref="M:System.Windows.Forms.LinkArea.LinkAreaConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> to create a new value.</summary>
			/// <param name="context">A type descriptor through which additional context may be provided. </param>
			/// <returns>Returns <see langword="true" /> if <see cref="M:System.Windows.Forms.LinkArea.LinkAreaConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> should be called when a change is made to one or more properties of this object.</returns>
			// Token: 0x06005BEA RID: 23530 RVA: 0x0000E214 File Offset: 0x0000C414
			public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			/// <summary>Retrieves the set of properties for this type. </summary>
			/// <param name="context">A type descriptor through which additional context may be provided. </param>
			/// <param name="value">The value of the object to get the properties for. </param>
			/// <param name="attributes">The attributes of the object to get the properties for. </param>
			/// <returns>The set of properties that should be exposed for this data type. If no properties should be exposed, this might return <see langword="null" />. The default implementation always returns <see langword="null" />.</returns>
			// Token: 0x06005BEB RID: 23531 RVA: 0x00180040 File Offset: 0x0017E240
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(LinkArea), attributes);
				return properties.Sort(new string[]
				{
					"Start",
					"Length"
				});
			}

			/// <summary>Determines if this object supports properties. By default, this is <see langword="false" />.</summary>
			/// <param name="context">A type descriptor through which additional context may be provided. </param>
			/// <returns>Returns <see langword="true" /> if <see cref="M:System.Windows.Forms.LinkArea.LinkAreaConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" /> should be called to find the properties of this object.</returns>
			// Token: 0x06005BEC RID: 23532 RVA: 0x0000E214 File Offset: 0x0000C414
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
		}
	}
}
