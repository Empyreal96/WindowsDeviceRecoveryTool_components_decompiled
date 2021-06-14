using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using Microsoft.Win32;

namespace System.Drawing
{
	/// <summary>Converts <see cref="T:System.Drawing.Font" /> objects from one data type to another. </summary>
	// Token: 0x0200001B RID: 27
	public class FontConverter : TypeConverter
	{
		/// <summary>Allows the <see cref="T:System.Drawing.FontConverter" /> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Drawing.FontConverter" /> is reclaimed by garbage collection.</summary>
		// Token: 0x060001CF RID: 463 RVA: 0x000084B4 File Offset: 0x000066B4
		~FontConverter()
		{
			if (this.fontNameConverter != null)
			{
				((IDisposable)this.fontNameConverter).Dispose();
			}
		}

		/// <summary>Determines whether this converter can convert an object in the specified source type to the native type of the converter.</summary>
		/// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="sourceType">The type you want to convert from. </param>
		/// <returns>This method returns <see langword="true" /> if this object can perform the conversion.</returns>
		// Token: 0x060001D0 RID: 464 RVA: 0x00007C8C File Offset: 0x00005E8C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Gets a value indicating whether this converter can convert an object to the given destination type using the context.</summary>
		/// <param name="context">An <see langword="ITypeDescriptorContext" /> object that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> object that represents the type you want to convert to. </param>
		/// <returns>This method returns <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001D1 RID: 465 RVA: 0x00007CAA File Offset: 0x00005EAA
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the specified object to the native type of the converter.</summary>
		/// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">A <see langword="CultureInfo" /> object that specifies the culture used to represent the font. </param>
		/// <param name="value">The object to convert. </param>
		/// <returns>The converted object. </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed. </exception>
		// Token: 0x060001D2 RID: 466 RVA: 0x000084F0 File Offset: 0x000066F0
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
			string text3 = text2;
			string text4 = null;
			float emSize = 8.25f;
			FontStyle fontStyle = FontStyle.Regular;
			GraphicsUnit unit = GraphicsUnit.Point;
			int num = text2.IndexOf(c);
			if (num > 0)
			{
				text3 = text2.Substring(0, num);
				if (num < text2.Length - 1)
				{
					int num2 = text2.IndexOf("style=");
					string text5;
					if (num2 != -1)
					{
						text4 = text2.Substring(num2, text2.Length - num2);
						if (!text4.StartsWith("style="))
						{
							throw this.GetFormatException(text2, c);
						}
						text5 = text2.Substring(num + 1, num2 - num - 1);
					}
					else
					{
						text5 = text2.Substring(num + 1, text2.Length - num - 1);
					}
					string[] array = this.ParseSizeTokens(text5, c);
					if (array[0] != null)
					{
						try
						{
							emSize = (float)TypeDescriptor.GetConverter(typeof(float)).ConvertFromString(context, culture, array[0]);
						}
						catch
						{
							throw this.GetFormatException(text2, c);
						}
					}
					if (array[1] != null)
					{
						unit = this.ParseGraphicsUnits(array[1]);
					}
					if (text4 != null)
					{
						int num3 = text4.IndexOf("=");
						text4 = text4.Substring(num3 + 1, text4.Length - "style=".Length);
						foreach (string text6 in text4.Split(new char[]
						{
							c
						}))
						{
							text6 = text6.Trim();
							try
							{
								fontStyle |= (FontStyle)Enum.Parse(typeof(FontStyle), text6, true);
							}
							catch (Exception ex)
							{
								if (ex is InvalidEnumArgumentException)
								{
									throw;
								}
								throw this.GetFormatException(text2, c);
							}
							FontStyle fontStyle2 = FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout;
							if ((fontStyle | fontStyle2) != fontStyle2)
							{
								throw new InvalidEnumArgumentException("style", (int)fontStyle, typeof(FontStyle));
							}
						}
					}
				}
			}
			if (this.fontNameConverter == null)
			{
				this.fontNameConverter = new FontConverter.FontNameConverter();
			}
			text3 = (string)this.fontNameConverter.ConvertFrom(context, culture, text3);
			return new Font(text3, emSize, fontStyle, unit);
		}

		/// <summary>Converts the specified object to another type. </summary>
		/// <param name="context">A formatter context. This object can be used to get additional information about the environment this converter is being called from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may also return <see langword="null" />. </param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> object that specifies the culture used to represent the object. </param>
		/// <param name="value">The object to convert. </param>
		/// <param name="destinationType">The data type to convert the object to. </param>
		/// <returns>The converted object. </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion was not successful.</exception>
		// Token: 0x060001D3 RID: 467 RVA: 0x0000874C File Offset: 0x0000694C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (!(destinationType == typeof(string)))
			{
				if (destinationType == typeof(InstanceDescriptor) && value is Font)
				{
					Font font = (Font)value;
					int num = 2;
					if (font.GdiVerticalFont)
					{
						num = 6;
					}
					else if (font.GdiCharSet != 1)
					{
						num = 5;
					}
					else if (font.Unit != GraphicsUnit.Point)
					{
						num = 4;
					}
					else if (font.Style != FontStyle.Regular)
					{
						num++;
					}
					object[] array = new object[num];
					Type[] array2 = new Type[num];
					array[0] = font.Name;
					array2[0] = typeof(string);
					array[1] = font.Size;
					array2[1] = typeof(float);
					if (num > 2)
					{
						array[2] = font.Style;
						array2[2] = typeof(FontStyle);
					}
					if (num > 3)
					{
						array[3] = font.Unit;
						array2[3] = typeof(GraphicsUnit);
					}
					if (num > 4)
					{
						array[4] = font.GdiCharSet;
						array2[4] = typeof(byte);
					}
					if (num > 5)
					{
						array[5] = font.GdiVerticalFont;
						array2[5] = typeof(bool);
					}
					MemberInfo constructor = typeof(Font).GetConstructor(array2);
					if (constructor != null)
					{
						return new InstanceDescriptor(constructor, array);
					}
				}
				return base.ConvertTo(context, culture, value, destinationType);
			}
			Font font2 = value as Font;
			if (font2 == null)
			{
				return SR.GetString("toStringNone");
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			string separator = culture.TextInfo.ListSeparator + " ";
			int num2 = 2;
			if (font2.Style != FontStyle.Regular)
			{
				num2++;
			}
			string[] array3 = new string[num2];
			int num3 = 0;
			array3[num3++] = font2.Name;
			array3[num3++] = TypeDescriptor.GetConverter(font2.Size).ConvertToString(context, culture, font2.Size) + this.GetGraphicsUnitText(font2.Unit);
			if (font2.Style != FontStyle.Regular)
			{
				array3[num3++] = "style=" + font2.Style.ToString("G");
			}
			return string.Join(separator, array3);
		}

		/// <summary>Creates an object of this type by using a specified set of property values for the object. </summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <param name="propertyValues">A dictionary of new property values. The dictionary contains a series of name-value pairs, one for each property returned from the <see cref="Overload:System.Drawing.FontConverter.GetProperties" /> method. </param>
		/// <returns>The newly created object, or <see langword="null" /> if the object could not be created. The default implementation returns <see langword="null" />.
		///     <see cref="M:System.Drawing.FontConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> useful for creating non-changeable objects that have changeable properties.</returns>
		// Token: 0x060001D4 RID: 468 RVA: 0x000089C4 File Offset: 0x00006BC4
		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			if (propertyValues == null)
			{
				throw new ArgumentNullException("propertyValues");
			}
			object obj = propertyValues["Name"];
			object obj2 = propertyValues["Size"];
			object obj3 = propertyValues["Unit"];
			object obj4 = propertyValues["Bold"];
			object obj5 = propertyValues["Italic"];
			object obj6 = propertyValues["Strikeout"];
			object obj7 = propertyValues["Underline"];
			object obj8 = propertyValues["GdiCharSet"];
			object obj9 = propertyValues["GdiVerticalFont"];
			if (obj == null)
			{
				obj = "Tahoma";
			}
			if (obj2 == null)
			{
				obj2 = 8f;
			}
			if (obj3 == null)
			{
				obj3 = GraphicsUnit.Point;
			}
			if (obj4 == null)
			{
				obj4 = false;
			}
			if (obj5 == null)
			{
				obj5 = false;
			}
			if (obj6 == null)
			{
				obj6 = false;
			}
			if (obj7 == null)
			{
				obj7 = false;
			}
			if (obj8 == null)
			{
				obj8 = 0;
			}
			if (obj9 == null)
			{
				obj9 = false;
			}
			if (!(obj is string) || !(obj2 is float) || !(obj8 is byte) || !(obj3 is GraphicsUnit) || !(obj4 is bool) || !(obj5 is bool) || !(obj6 is bool) || !(obj7 is bool) || !(obj9 is bool))
			{
				throw new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
			}
			FontStyle fontStyle = FontStyle.Regular;
			if (obj4 != null && (bool)obj4)
			{
				fontStyle |= FontStyle.Bold;
			}
			if (obj5 != null && (bool)obj5)
			{
				fontStyle |= FontStyle.Italic;
			}
			if (obj6 != null && (bool)obj6)
			{
				fontStyle |= FontStyle.Strikeout;
			}
			if (obj7 != null && (bool)obj7)
			{
				fontStyle |= FontStyle.Underline;
			}
			return new Font((string)obj, (float)obj2, fontStyle, (GraphicsUnit)obj3, (byte)obj8, (bool)obj9);
		}

		/// <summary>Determines whether changing a value on this object should require a call to the <see cref="Overload:System.Drawing.FontConverter.CreateInstance" /> method to create a new value.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if the <see langword="CreateInstance" /> object should be called when a change is made to one or more properties of this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001D5 RID: 469 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008B88 File Offset: 0x00006D88
		private ArgumentException GetFormatException(string text, char separator)
		{
			string text2 = string.Format(CultureInfo.CurrentCulture, "name{0} size[units[{0} style=style1[{0} style2{0} ...]]]", new object[]
			{
				separator
			});
			return new ArgumentException(SR.GetString("TextParseFailedFormat", new object[]
			{
				text,
				text2
			}));
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00008BD4 File Offset: 0x00006DD4
		private string GetGraphicsUnitText(GraphicsUnit units)
		{
			string result = "";
			for (int i = 0; i < FontConverter.UnitName.names.Length; i++)
			{
				if (FontConverter.UnitName.names[i].unit == units)
				{
					result = FontConverter.UnitName.names[i].name;
					break;
				}
			}
			return result;
		}

		/// <summary>Retrieves the set of properties for this type. By default, a type does not have any properties to return. </summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <param name="value">The value of the object to get the properties for. </param>
		/// <param name="attributes">An array of <see cref="T:System.Attribute" /> objects that describe the properties.</param>
		/// <returns>The set of properties that should be exposed for this data type. If no properties should be exposed, this may return <see langword="null" />. The default implementation always returns <see langword="null" />.An easy implementation of this method can call the <see cref="Overload:System.ComponentModel.TypeConverter.GetProperties" /> method for the correct data type.</returns>
		// Token: 0x060001D8 RID: 472 RVA: 0x00008C18 File Offset: 0x00006E18
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Font), attributes);
			return properties.Sort(new string[]
			{
				"Name",
				"Size",
				"Unit",
				"Weight"
			});
		}

		/// <summary>Determines whether this object supports properties. The default is <see langword="false" />.</summary>
		/// <param name="context">A type descriptor through which additional context can be provided. </param>
		/// <returns>This method returns <see langword="true" /> if the <see cref="M:System.Drawing.FontConverter.GetPropertiesSupported(System.ComponentModel.ITypeDescriptorContext)" /> method should be called to find the properties of this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001D9 RID: 473 RVA: 0x00008490 File Offset: 0x00006690
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008C64 File Offset: 0x00006E64
		private string[] ParseSizeTokens(string text, char separator)
		{
			string text2 = null;
			string text3 = null;
			text = text.Trim();
			int length = text.Length;
			if (length > 0)
			{
				int num = 0;
				while (num < length && !char.IsLetter(text[num]))
				{
					num++;
				}
				char[] trimChars = new char[]
				{
					separator,
					' '
				};
				if (num > 0)
				{
					text2 = text.Substring(0, num);
					text2 = text2.Trim(trimChars);
				}
				if (num < length)
				{
					text3 = text.Substring(num);
					text3 = text3.TrimEnd(trimChars);
				}
			}
			return new string[]
			{
				text2,
				text3
			};
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00008CEC File Offset: 0x00006EEC
		private GraphicsUnit ParseGraphicsUnits(string units)
		{
			FontConverter.UnitName unitName = null;
			for (int i = 0; i < FontConverter.UnitName.names.Length; i++)
			{
				if (string.Equals(FontConverter.UnitName.names[i].name, units, StringComparison.OrdinalIgnoreCase))
				{
					unitName = FontConverter.UnitName.names[i];
					break;
				}
			}
			if (unitName == null)
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					"units",
					units
				}));
			}
			return unitName.unit;
		}

		// Token: 0x04000174 RID: 372
		private FontConverter.FontNameConverter fontNameConverter;

		// Token: 0x04000175 RID: 373
		private const string styleHdr = "style=";

		// Token: 0x020000F5 RID: 245
		internal class UnitName
		{
			// Token: 0x06000C92 RID: 3218 RVA: 0x0002C4A4 File Offset: 0x0002A6A4
			internal UnitName(string name, GraphicsUnit unit)
			{
				this.name = name;
				this.unit = unit;
			}

			// Token: 0x04000AE8 RID: 2792
			internal string name;

			// Token: 0x04000AE9 RID: 2793
			internal GraphicsUnit unit;

			// Token: 0x04000AEA RID: 2794
			internal static readonly FontConverter.UnitName[] names = new FontConverter.UnitName[]
			{
				new FontConverter.UnitName("world", GraphicsUnit.World),
				new FontConverter.UnitName("display", GraphicsUnit.Display),
				new FontConverter.UnitName("px", GraphicsUnit.Pixel),
				new FontConverter.UnitName("pt", GraphicsUnit.Point),
				new FontConverter.UnitName("in", GraphicsUnit.Inch),
				new FontConverter.UnitName("doc", GraphicsUnit.Document),
				new FontConverter.UnitName("mm", GraphicsUnit.Millimeter)
			};
		}

		/// <summary>
		///     <see cref="T:System.Drawing.FontConverter.FontNameConverter" /> is a type converter that is used to convert a font name to and from various other representations.</summary>
		// Token: 0x020000F6 RID: 246
		public sealed class FontNameConverter : TypeConverter, IDisposable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.FontConverter.FontNameConverter" /> class. </summary>
			// Token: 0x06000C94 RID: 3220 RVA: 0x0002C536 File Offset: 0x0002A736
			public FontNameConverter()
			{
				SystemEvents.InstalledFontsChanged += this.OnInstalledFontsChanged;
			}

			/// <summary>Determines if this converter can convert an object in the given source type to the native type of the converter.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may return <see langword="null" />.</param>
			/// <param name="sourceType">The type you wish to convert from. </param>
			/// <returns>
			///     <see langword="true" /> if the converter can perform the conversion; otherwise, <see langword="false" />. </returns>
			// Token: 0x06000C95 RID: 3221 RVA: 0x00007C8C File Offset: 0x00005E8C
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			/// <summary>Converts the given object to the converter's native type.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may return <see langword="null" />. </param>
			/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" /> to use to perform the conversion </param>
			/// <param name="value">The object to convert. </param>
			/// <returns>The converted object. </returns>
			/// <exception cref="T:System.NotSupportedException">The conversion cannot be completed.</exception>
			// Token: 0x06000C96 RID: 3222 RVA: 0x0002C54F File Offset: 0x0002A74F
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return this.MatchFontName((string)value, context);
				}
				return base.ConvertFrom(context, culture, value);
			}

			/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
			// Token: 0x06000C97 RID: 3223 RVA: 0x0002C570 File Offset: 0x0002A770
			void IDisposable.Dispose()
			{
				SystemEvents.InstalledFontsChanged -= this.OnInstalledFontsChanged;
			}

			/// <summary>Retrieves a collection containing a set of standard values for the data type this converter is designed for. </summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may return <see langword="null" />.</param>
			/// <returns>A collection containing a standard set of valid values, or <see langword="null" />. The default is <see langword="null" />.</returns>
			// Token: 0x06000C98 RID: 3224 RVA: 0x0002C584 File Offset: 0x0002A784
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (this.values == null)
				{
					FontFamily[] families = FontFamily.Families;
					Hashtable hashtable = new Hashtable();
					for (int i = 0; i < families.Length; i++)
					{
						string name = families[i].Name;
						hashtable[name.ToLower(CultureInfo.InvariantCulture)] = name;
					}
					object[] array = new object[hashtable.Values.Count];
					hashtable.Values.CopyTo(array, 0);
					Array.Sort(array, Comparer.Default);
					this.values = new TypeConverter.StandardValuesCollection(array);
				}
				return this.values;
			}

			/// <summary>Determines if the list of standard values returned from the <see cref="Overload:System.Drawing.FontConverter.FontNameConverter.GetStandardValues" /> method is an exclusive list. </summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may return <see langword="null" />.</param>
			/// <returns>
			///     <see langword="true" /> if the collection returned from <see cref="Overload:System.Drawing.FontConverter.FontNameConverter.GetStandardValues" />is an exclusive list of possible values; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
			// Token: 0x06000C99 RID: 3225 RVA: 0x0001E374 File Offset: 0x0001C574
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			/// <summary>Determines if this object supports a standard set of values that can be picked from a list.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to extract additional information about the environment this converter is being invoked from. This may be <see langword="null" />, so you should always check. Also, properties on the context object may return <see langword="null" />. </param>
			/// <returns>
			///     <see langword="true" /> if <see cref="Overload:System.Drawing.FontConverter.FontNameConverter.GetStandardValues" /> should be called to find a common set of values the object supports; otherwise, <see langword="false" />.</returns>
			// Token: 0x06000C9A RID: 3226 RVA: 0x00008490 File Offset: 0x00006690
			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				return true;
			}

			// Token: 0x06000C9B RID: 3227 RVA: 0x0002C60C File Offset: 0x0002A80C
			private string MatchFontName(string name, ITypeDescriptorContext context)
			{
				string text = null;
				name = name.ToLower(CultureInfo.InvariantCulture);
				foreach (object obj in this.GetStandardValues(context))
				{
					string text2 = obj.ToString().ToLower(CultureInfo.InvariantCulture);
					IEnumerator enumerator;
					if (text2.Equals(name))
					{
						return enumerator.Current.ToString();
					}
					if (text2.StartsWith(name) && (text == null || text2.Length <= text.Length))
					{
						text = enumerator.Current.ToString();
					}
				}
				if (text == null)
				{
					text = name;
				}
				return text;
			}

			// Token: 0x06000C9C RID: 3228 RVA: 0x0002C696 File Offset: 0x0002A896
			private void OnInstalledFontsChanged(object sender, EventArgs e)
			{
				this.values = null;
			}

			// Token: 0x04000AEB RID: 2795
			private TypeConverter.StandardValuesCollection values;
		}

		/// <summary>Converts font units to and from other unit types.</summary>
		// Token: 0x020000F7 RID: 247
		public class FontUnitConverter : EnumConverter
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.FontConverter.FontUnitConverter" /> class.</summary>
			// Token: 0x06000C9D RID: 3229 RVA: 0x0002C69F File Offset: 0x0002A89F
			public FontUnitConverter() : base(typeof(GraphicsUnit))
			{
			}

			/// <summary>Returns a collection of standard values valid for the <see cref="T:System.Drawing.Font" /> type.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
			// Token: 0x06000C9E RID: 3230 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (base.Values == null)
				{
					base.GetStandardValues(context);
					ArrayList arrayList = new ArrayList(base.Values);
					arrayList.Remove(GraphicsUnit.Display);
					base.Values = new TypeConverter.StandardValuesCollection(arrayList);
				}
				return base.Values;
			}
		}
	}
}
