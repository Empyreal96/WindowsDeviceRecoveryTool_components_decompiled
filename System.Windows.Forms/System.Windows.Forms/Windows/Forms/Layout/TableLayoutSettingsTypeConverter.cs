using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;
using System.Xml;

namespace System.Windows.Forms.Layout
{
	/// <summary>Provides a unified way of converting types of values to other types, as well as for accessing standard values and subproperties.</summary>
	// Token: 0x020004D5 RID: 1237
	public class TableLayoutSettingsTypeConverter : TypeConverter
	{
		/// <summary>Determines whether this converter can convert an object in the given source type to the native type of this converter.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005203 RID: 20995 RVA: 0x000B9F74 File Offset: 0x000B8174
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Returns a value indicating whether this converter can convert an object to the given destination type by using the context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005204 RID: 20996 RVA: 0x000B9F92 File Offset: 0x000B8192
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
		}

		/// <summary>Converts the given object to the type of this converter by using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		// Token: 0x06005205 RID: 20997 RVA: 0x00156BF4 File Offset: 0x00154DF4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(value as string);
				TableLayoutSettings tableLayoutSettings = new TableLayoutSettings();
				this.ParseControls(tableLayoutSettings, xmlDocument.GetElementsByTagName("Control"));
				this.ParseStyles(tableLayoutSettings, xmlDocument.GetElementsByTagName("Columns"), true);
				this.ParseStyles(tableLayoutSettings, xmlDocument.GetElementsByTagName("Rows"), false);
				return tableLayoutSettings;
			}
			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>Converts the given value object to the specified type by using the specified context and culture information.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type" /> to convert the value parameter to.</param>
		/// <returns>An <see cref="T:System.Object" /> that represents the converted value.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="destinationType" /> is <see langword="null" />.</exception>
		// Token: 0x06005206 RID: 20998 RVA: 0x00156C64 File Offset: 0x00154E64
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is TableLayoutSettings && destinationType == typeof(string))
			{
				TableLayoutSettings tableLayoutSettings = value as TableLayoutSettings;
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
				xmlWriter.WriteStartElement("TableLayoutSettings");
				xmlWriter.WriteStartElement("Controls");
				foreach (TableLayoutSettings.ControlInformation controlInformation in tableLayoutSettings.GetControlsInformation())
				{
					xmlWriter.WriteStartElement("Control");
					xmlWriter.WriteAttributeString("Name", controlInformation.Name.ToString());
					XmlWriter xmlWriter2 = xmlWriter;
					string localName = "Row";
					int num = controlInformation.Row;
					xmlWriter2.WriteAttributeString(localName, num.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter3 = xmlWriter;
					string localName2 = "RowSpan";
					num = controlInformation.RowSpan;
					xmlWriter3.WriteAttributeString(localName2, num.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter4 = xmlWriter;
					string localName3 = "Column";
					num = controlInformation.Column;
					xmlWriter4.WriteAttributeString(localName3, num.ToString(CultureInfo.CurrentCulture));
					XmlWriter xmlWriter5 = xmlWriter;
					string localName4 = "ColumnSpan";
					num = controlInformation.ColumnSpan;
					xmlWriter5.WriteAttributeString(localName4, num.ToString(CultureInfo.CurrentCulture));
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Columns");
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (object obj in ((IEnumerable)tableLayoutSettings.ColumnStyles))
				{
					ColumnStyle columnStyle = (ColumnStyle)obj;
					stringBuilder2.AppendFormat("{0},{1},", columnStyle.SizeType, columnStyle.Width);
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
				}
				xmlWriter.WriteAttributeString("Styles", stringBuilder2.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteStartElement("Rows");
				StringBuilder stringBuilder3 = new StringBuilder();
				foreach (object obj2 in ((IEnumerable)tableLayoutSettings.RowStyles))
				{
					RowStyle rowStyle = (RowStyle)obj2;
					stringBuilder3.AppendFormat("{0},{1},", rowStyle.SizeType, rowStyle.Height);
				}
				if (stringBuilder3.Length > 0)
				{
					stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
				}
				xmlWriter.WriteAttributeString("Styles", stringBuilder3.ToString());
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				xmlWriter.Close();
				return stringBuilder.ToString();
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06005207 RID: 20999 RVA: 0x00156F48 File Offset: 0x00155148
		private string GetAttributeValue(XmlNode node, string attribute)
		{
			XmlAttribute xmlAttribute = node.Attributes[attribute];
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			return null;
		}

		// Token: 0x06005208 RID: 21000 RVA: 0x00156F70 File Offset: 0x00155170
		private int GetAttributeValue(XmlNode node, string attribute, int valueIfNotFound)
		{
			string attributeValue = this.GetAttributeValue(node, attribute);
			int result;
			if (!string.IsNullOrEmpty(attributeValue) && int.TryParse(attributeValue, out result))
			{
				return result;
			}
			return valueIfNotFound;
		}

		// Token: 0x06005209 RID: 21001 RVA: 0x00156F9C File Offset: 0x0015519C
		private void ParseControls(TableLayoutSettings settings, XmlNodeList controlXmlFragments)
		{
			foreach (object obj in controlXmlFragments)
			{
				XmlNode node = (XmlNode)obj;
				string attributeValue = this.GetAttributeValue(node, "Name");
				if (!string.IsNullOrEmpty(attributeValue))
				{
					int attributeValue2 = this.GetAttributeValue(node, "Row", -1);
					int attributeValue3 = this.GetAttributeValue(node, "RowSpan", 1);
					int attributeValue4 = this.GetAttributeValue(node, "Column", -1);
					int attributeValue5 = this.GetAttributeValue(node, "ColumnSpan", 1);
					settings.SetRow(attributeValue, attributeValue2);
					settings.SetColumn(attributeValue, attributeValue4);
					settings.SetRowSpan(attributeValue, attributeValue3);
					settings.SetColumnSpan(attributeValue, attributeValue5);
				}
			}
		}

		// Token: 0x0600520A RID: 21002 RVA: 0x00157064 File Offset: 0x00155264
		private void ParseStyles(TableLayoutSettings settings, XmlNodeList controlXmlFragments, bool columns)
		{
			foreach (object obj in controlXmlFragments)
			{
				XmlNode node = (XmlNode)obj;
				string attributeValue = this.GetAttributeValue(node, "Styles");
				Type typeFromHandle = typeof(SizeType);
				if (!string.IsNullOrEmpty(attributeValue))
				{
					int num;
					for (int i = 0; i < attributeValue.Length; i = num)
					{
						num = i;
						while (char.IsLetter(attributeValue[num]))
						{
							num++;
						}
						SizeType sizeType = (SizeType)Enum.Parse(typeFromHandle, attributeValue.Substring(i, num - i), true);
						while (!char.IsDigit(attributeValue[num]))
						{
							num++;
						}
						StringBuilder stringBuilder = new StringBuilder();
						while (num < attributeValue.Length && char.IsDigit(attributeValue[num]))
						{
							stringBuilder.Append(attributeValue[num]);
							num++;
						}
						stringBuilder.Append('.');
						while (num < attributeValue.Length && !char.IsLetter(attributeValue[num]))
						{
							if (char.IsDigit(attributeValue[num]))
							{
								stringBuilder.Append(attributeValue[num]);
							}
							num++;
						}
						string s = stringBuilder.ToString();
						float num2;
						if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out num2))
						{
							num2 = 0f;
						}
						if (columns)
						{
							settings.ColumnStyles.Add(new ColumnStyle(sizeType, num2));
						}
						else
						{
							settings.RowStyles.Add(new RowStyle(sizeType, num2));
						}
					}
				}
			}
		}
	}
}
