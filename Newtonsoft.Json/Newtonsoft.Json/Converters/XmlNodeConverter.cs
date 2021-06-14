using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x0200003B RID: 59
	public class XmlNodeConverter : JsonConverter
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000236 RID: 566 RVA: 0x000088D5 File Offset: 0x00006AD5
		// (set) Token: 0x06000237 RID: 567 RVA: 0x000088DD File Offset: 0x00006ADD
		public string DeserializeRootElementName { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000238 RID: 568 RVA: 0x000088E6 File Offset: 0x00006AE6
		// (set) Token: 0x06000239 RID: 569 RVA: 0x000088EE File Offset: 0x00006AEE
		public bool WriteArrayAttribute { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600023A RID: 570 RVA: 0x000088F7 File Offset: 0x00006AF7
		// (set) Token: 0x0600023B RID: 571 RVA: 0x000088FF File Offset: 0x00006AFF
		public bool OmitRootObject { get; set; }

		// Token: 0x0600023C RID: 572 RVA: 0x00008908 File Offset: 0x00006B08
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			IXmlNode node = this.WrapXml(value);
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			this.PushParentNamespaces(node, manager);
			if (!this.OmitRootObject)
			{
				writer.WriteStartObject();
			}
			this.SerializeNode(writer, node, manager, !this.OmitRootObject);
			if (!this.OmitRootObject)
			{
				writer.WriteEndObject();
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000895E File Offset: 0x00006B5E
		private IXmlNode WrapXml(object value)
		{
			if (value is XObject)
			{
				return XContainerWrapper.WrapNode((XObject)value);
			}
			if (value is XmlNode)
			{
				return XmlNodeWrapper.WrapNode((XmlNode)value);
			}
			throw new ArgumentException("Value must be an XML object.", "value");
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008998 File Offset: 0x00006B98
		private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
		{
			List<IXmlNode> list = null;
			IXmlNode xmlNode = node;
			while ((xmlNode = xmlNode.ParentNode) != null)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (list == null)
					{
						list = new List<IXmlNode>();
					}
					list.Add(xmlNode);
				}
			}
			if (list != null)
			{
				list.Reverse();
				foreach (IXmlNode xmlNode2 in list)
				{
					manager.PushScope();
					foreach (IXmlNode xmlNode3 in xmlNode2.Attributes)
					{
						if (xmlNode3.NamespaceUri == "http://www.w3.org/2000/xmlns/" && xmlNode3.LocalName != "xmlns")
						{
							manager.AddNamespace(xmlNode3.LocalName, xmlNode3.Value);
						}
					}
				}
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00008A8C File Offset: 0x00006C8C
		private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
		{
			string text = (node.NamespaceUri == null || (node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/")) ? null : manager.LookupPrefix(node.NamespaceUri);
			if (!string.IsNullOrEmpty(text))
			{
				return text + ":" + node.LocalName;
			}
			return node.LocalName;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008AF8 File Offset: 0x00006CF8
		private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Element:
				return this.ResolveFullName(node, manager);
			case XmlNodeType.Attribute:
				if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
				{
					return "$" + node.LocalName;
				}
				return "@" + this.ResolveFullName(node, manager);
			case XmlNodeType.Text:
				return "#text";
			case XmlNodeType.CDATA:
				return "#cdata-section";
			case XmlNodeType.ProcessingInstruction:
				return "?" + this.ResolveFullName(node, manager);
			case XmlNodeType.Comment:
				return "#comment";
			case XmlNodeType.DocumentType:
				return "!" + this.ResolveFullName(node, manager);
			case XmlNodeType.Whitespace:
				return "#whitespace";
			case XmlNodeType.SignificantWhitespace:
				return "#significant-whitespace";
			case XmlNodeType.XmlDeclaration:
				return "?xml";
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008C28 File Offset: 0x00006E28
		private bool IsArray(IXmlNode node)
		{
			IXmlNode xmlNode;
			if (node.Attributes == null)
			{
				xmlNode = null;
			}
			else
			{
				xmlNode = node.Attributes.SingleOrDefault((IXmlNode a) => a.LocalName == "Array" && a.NamespaceUri == "http://james.newtonking.com/projects/json");
			}
			IXmlNode xmlNode2 = xmlNode;
			return xmlNode2 != null && XmlConvert.ToBoolean(xmlNode2.Value);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008C7C File Offset: 0x00006E7C
		private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			Dictionary<string, List<IXmlNode>> dictionary = new Dictionary<string, List<IXmlNode>>();
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				IXmlNode xmlNode = node.ChildNodes[i];
				string propertyName = this.GetPropertyName(xmlNode, manager);
				List<IXmlNode> list;
				if (!dictionary.TryGetValue(propertyName, out list))
				{
					list = new List<IXmlNode>();
					dictionary.Add(propertyName, list);
				}
				list.Add(xmlNode);
			}
			foreach (KeyValuePair<string, List<IXmlNode>> keyValuePair in dictionary)
			{
				List<IXmlNode> value = keyValuePair.Value;
				if (value.Count == 1 && !this.IsArray(value[0]))
				{
					this.SerializeNode(writer, value[0], manager, writePropertyName);
				}
				else
				{
					string key = keyValuePair.Key;
					if (writePropertyName)
					{
						writer.WritePropertyName(key);
					}
					writer.WriteStartArray();
					for (int j = 0; j < value.Count; j++)
					{
						this.SerializeNode(writer, value[j], manager, false);
					}
					writer.WriteEndArray();
				}
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00008DCC File Offset: 0x00006FCC
		private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Element:
				if (this.IsArray(node))
				{
					if (node.ChildNodes.All((IXmlNode n) => n.LocalName == node.LocalName) && node.ChildNodes.Count > 0)
					{
						this.SerializeGroupedNodes(writer, node, manager, false);
						return;
					}
				}
				manager.PushScope();
				foreach (IXmlNode xmlNode in node.Attributes)
				{
					if (xmlNode.NamespaceUri == "http://www.w3.org/2000/xmlns/")
					{
						string prefix = (xmlNode.LocalName != "xmlns") ? xmlNode.LocalName : string.Empty;
						string value = xmlNode.Value;
						manager.AddNamespace(prefix, value);
					}
				}
				if (writePropertyName)
				{
					writer.WritePropertyName(this.GetPropertyName(node, manager));
				}
				if (!this.ValueAttributes(node.Attributes).Any<IXmlNode>() && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
				{
					writer.WriteValue(node.ChildNodes[0].Value);
				}
				else if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty<IXmlNode>(node.Attributes))
				{
					IXmlElement xmlElement = (IXmlElement)node;
					if (xmlElement.IsEmpty)
					{
						writer.WriteNull();
					}
					else
					{
						writer.WriteValue(string.Empty);
					}
				}
				else
				{
					writer.WriteStartObject();
					for (int i = 0; i < node.Attributes.Count; i++)
					{
						this.SerializeNode(writer, node.Attributes[i], manager, true);
					}
					this.SerializeGroupedNodes(writer, node, manager, true);
					writer.WriteEndObject();
				}
				manager.PopScope();
				return;
			case XmlNodeType.Attribute:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if (node.NamespaceUri == "http://www.w3.org/2000/xmlns/" && node.Value == "http://james.newtonking.com/projects/json")
				{
					return;
				}
				if (node.NamespaceUri == "http://james.newtonking.com/projects/json" && node.LocalName == "Array")
				{
					return;
				}
				if (writePropertyName)
				{
					writer.WritePropertyName(this.GetPropertyName(node, manager));
				}
				writer.WriteValue(node.Value);
				return;
			case XmlNodeType.Comment:
				if (writePropertyName)
				{
					writer.WriteComment(node.Value);
					return;
				}
				return;
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				this.SerializeGroupedNodes(writer, node, manager, writePropertyName);
				return;
			case XmlNodeType.DocumentType:
			{
				IXmlDocumentType xmlDocumentType = (IXmlDocumentType)node;
				writer.WritePropertyName(this.GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDocumentType.Name))
				{
					writer.WritePropertyName("@name");
					writer.WriteValue(xmlDocumentType.Name);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.Public))
				{
					writer.WritePropertyName("@public");
					writer.WriteValue(xmlDocumentType.Public);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.System))
				{
					writer.WritePropertyName("@system");
					writer.WriteValue(xmlDocumentType.System);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.InternalSubset))
				{
					writer.WritePropertyName("@internalSubset");
					writer.WriteValue(xmlDocumentType.InternalSubset);
				}
				writer.WriteEndObject();
				return;
			}
			case XmlNodeType.XmlDeclaration:
			{
				IXmlDeclaration xmlDeclaration = (IXmlDeclaration)node;
				writer.WritePropertyName(this.GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDeclaration.Version))
				{
					writer.WritePropertyName("@version");
					writer.WriteValue(xmlDeclaration.Version);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
				{
					writer.WritePropertyName("@encoding");
					writer.WriteValue(xmlDeclaration.Encoding);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
				{
					writer.WritePropertyName("@standalone");
					writer.WriteValue(xmlDeclaration.Standalone);
				}
				writer.WriteEndObject();
				return;
			}
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000092A8 File Offset: 0x000074A8
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			IXmlDocument xmlDocument = null;
			IXmlNode xmlNode = null;
			if (typeof(XObject).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XDocument) && objectType != typeof(XElement))
				{
					throw new JsonSerializationException("XmlNodeConverter only supports deserializing XDocument or XElement.");
				}
				XDocument document = new XDocument();
				xmlDocument = new XDocumentWrapper(document);
				xmlNode = xmlDocument;
			}
			if (typeof(XmlNode).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XmlDocument))
				{
					throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");
				}
				xmlDocument = new XmlDocumentWrapper(new XmlDocument
				{
					XmlResolver = null
				});
				xmlNode = xmlDocument;
			}
			if (xmlDocument == null || xmlNode == null)
			{
				throw new JsonSerializationException("Unexpected type when converting XML: " + objectType);
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
			}
			if (!string.IsNullOrEmpty(this.DeserializeRootElementName))
			{
				this.ReadElement(reader, xmlDocument, xmlNode, this.DeserializeRootElementName, manager);
			}
			else
			{
				reader.Read();
				this.DeserializeNode(reader, xmlDocument, manager, xmlNode);
			}
			if (objectType == typeof(XElement))
			{
				XElement xelement = (XElement)xmlDocument.DocumentElement.WrappedNode;
				xelement.Remove();
				return xelement;
			}
			return xmlDocument.WrappedNode;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000093F8 File Offset: 0x000075F8
		private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
		{
			if (propertyName != null)
			{
				if (propertyName == "#text")
				{
					currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#cdata-section")
				{
					currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#whitespace")
				{
					currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#significant-whitespace")
				{
					currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
					return;
				}
			}
			if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
			{
				this.CreateInstruction(reader, document, currentNode, propertyName);
				return;
			}
			if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
			{
				this.CreateDocumentType(reader, document, currentNode);
				return;
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				this.ReadArrayElements(reader, document, propertyName, currentNode, manager);
				return;
			}
			this.ReadElement(reader, document, currentNode, propertyName, manager);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000950C File Offset: 0x0000770C
		private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new JsonSerializationException("XmlNodeConverter cannot convert JSON with an empty property name to XML.");
			}
			Dictionary<string, string> dictionary = this.ReadAttributeElements(reader, manager);
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			if (propertyName.StartsWith('@'))
			{
				string text = propertyName.Substring(1);
				string value = reader.Value.ToString();
				string prefix2 = MiscellaneousUtils.GetPrefix(text);
				IXmlNode attributeNode = (!string.IsNullOrEmpty(prefix2)) ? document.CreateAttribute(text, manager.LookupNamespace(prefix2), value) : document.CreateAttribute(text, value);
				((IXmlElement)currentNode).SetAttributeNode(attributeNode);
				return;
			}
			IXmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string prefix3 = MiscellaneousUtils.GetPrefix(keyValuePair.Key);
				IXmlNode attributeNode2 = (!string.IsNullOrEmpty(prefix3)) ? document.CreateAttribute(keyValuePair.Key, manager.LookupNamespace(prefix3), keyValuePair.Value) : document.CreateAttribute(keyValuePair.Key, keyValuePair.Value);
				xmlElement.SetAttributeNode(attributeNode2);
			}
			if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Boolean || reader.TokenType == JsonToken.Date)
			{
				xmlElement.AppendChild(document.CreateTextNode(this.ConvertTokenToXmlValue(reader)));
				return;
			}
			if (reader.TokenType == JsonToken.Null)
			{
				return;
			}
			if (reader.TokenType != JsonToken.EndObject)
			{
				manager.PushScope();
				this.DeserializeNode(reader, document, manager, xmlElement);
				manager.PopScope();
			}
			manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x000096CC File Offset: 0x000078CC
		private string ConvertTokenToXmlValue(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.String)
			{
				return reader.Value.ToString();
			}
			if (reader.TokenType == JsonToken.Integer)
			{
				return XmlConvert.ToString(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
			}
			if (reader.TokenType == JsonToken.Float)
			{
				if (reader.Value is decimal)
				{
					return XmlConvert.ToString((decimal)reader.Value);
				}
				if (reader.Value is float)
				{
					return XmlConvert.ToString((float)reader.Value);
				}
				return XmlConvert.ToString(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
			}
			else
			{
				if (reader.TokenType == JsonToken.Boolean)
				{
					return XmlConvert.ToString(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
				}
				if (reader.TokenType == JsonToken.Date)
				{
					if (reader.Value is DateTimeOffset)
					{
						return XmlConvert.ToString((DateTimeOffset)reader.Value);
					}
					DateTime value = Convert.ToDateTime(reader.Value, CultureInfo.InvariantCulture);
					return XmlConvert.ToString(value, DateTimeUtils.ToSerializationMode(value.Kind));
				}
				else
				{
					if (reader.TokenType == JsonToken.Null)
					{
						return null;
					}
					throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000981C File Offset: 0x00007A1C
		private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
		{
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			int num = 0;
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				this.DeserializeValue(reader, document, manager, propertyName, xmlElement);
				num++;
			}
			if (this.WriteArrayAttribute)
			{
				this.AddJsonArrayAttribute(xmlElement, document);
			}
			if (num == 1 && this.WriteArrayAttribute)
			{
				IXmlElement element = xmlElement.ChildNodes.OfType<IXmlElement>().Single((IXmlElement n) => n.LocalName == propertyName);
				this.AddJsonArrayAttribute(element, document);
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000098DC File Offset: 0x00007ADC
		private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
		{
			element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
			if (element is XElementWrapper && element.GetPrefixOfNamespace("http://james.newtonking.com/projects/json") == null)
			{
				element.SetAttributeNode(document.CreateAttribute("xmlns:json", "http://www.w3.org/2000/xmlns/", "http://james.newtonking.com/projects/json"));
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009934 File Offset: 0x00007B34
		private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = false;
			bool flag2 = false;
			if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor)
			{
				while (!flag && !flag2 && reader.Read())
				{
					JsonToken tokenType = reader.TokenType;
					switch (tokenType)
					{
					case JsonToken.PropertyName:
					{
						string text = reader.Value.ToString();
						if (!string.IsNullOrEmpty(text))
						{
							char c = text[0];
							char c2 = c;
							if (c2 != '$')
							{
								if (c2 == '@')
								{
									text = text.Substring(1);
									reader.Read();
									string text2 = this.ConvertTokenToXmlValue(reader);
									dictionary.Add(text, text2);
									string prefix;
									if (this.IsNamespaceAttribute(text, out prefix))
									{
										manager.AddNamespace(prefix, text2);
									}
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								text = text.Substring(1);
								reader.Read();
								string text2 = reader.Value.ToString();
								string text3 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
								if (text3 == null)
								{
									int? num = null;
									while (manager.LookupNamespace("json" + num) != null)
									{
										num = new int?(num.GetValueOrDefault() + 1);
									}
									text3 = "json" + num;
									dictionary.Add("xmlns:" + text3, "http://james.newtonking.com/projects/json");
									manager.AddNamespace(text3, "http://james.newtonking.com/projects/json");
								}
								dictionary.Add(text3 + ":" + text, text2);
							}
						}
						else
						{
							flag = true;
						}
						break;
					}
					case JsonToken.Comment:
						flag2 = true;
						break;
					default:
						if (tokenType != JsonToken.EndObject)
						{
							throw new JsonSerializationException("Unexpected JsonToken: " + reader.TokenType);
						}
						flag2 = true;
						break;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009B2C File Offset: 0x00007D2C
		private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
		{
			if (propertyName == "?xml")
			{
				string version = null;
				string encoding = null;
				string standalone = null;
				while (reader.Read() && reader.TokenType != JsonToken.EndObject)
				{
					string a;
					if ((a = reader.Value.ToString()) != null)
					{
						if (a == "@version")
						{
							reader.Read();
							version = reader.Value.ToString();
							continue;
						}
						if (a == "@encoding")
						{
							reader.Read();
							encoding = reader.Value.ToString();
							continue;
						}
						if (a == "@standalone")
						{
							reader.Read();
							standalone = reader.Value.ToString();
							continue;
						}
					}
					throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
				}
				IXmlNode newChild = document.CreateXmlDeclaration(version, encoding, standalone);
				currentNode.AppendChild(newChild);
				return;
			}
			IXmlNode newChild2 = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
			currentNode.AppendChild(newChild2);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00009C34 File Offset: 0x00007E34
		private void CreateDocumentType(JsonReader reader, IXmlDocument document, IXmlNode currentNode)
		{
			string name = null;
			string publicId = null;
			string systemId = null;
			string internalSubset = null;
			while (reader.Read() && reader.TokenType != JsonToken.EndObject)
			{
				string a;
				if ((a = reader.Value.ToString()) != null)
				{
					if (a == "@name")
					{
						reader.Read();
						name = reader.Value.ToString();
						continue;
					}
					if (a == "@public")
					{
						reader.Read();
						publicId = reader.Value.ToString();
						continue;
					}
					if (a == "@system")
					{
						reader.Read();
						systemId = reader.Value.ToString();
						continue;
					}
					if (a == "@internalSubset")
					{
						reader.Read();
						internalSubset = reader.Value.ToString();
						continue;
					}
				}
				throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
			}
			IXmlNode newChild = document.CreateXmlDocumentType(name, publicId, systemId, internalSubset);
			currentNode.AppendChild(newChild);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00009D30 File Offset: 0x00007F30
		private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			string text = string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix);
			return (!string.IsNullOrEmpty(text)) ? document.CreateElement(elementName, text) : document.CreateElement(elementName);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009D90 File Offset: 0x00007F90
		private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
		{
			JsonToken tokenType;
			for (;;)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.StartConstructor:
				{
					string propertyName2 = reader.Value.ToString();
					while (reader.Read())
					{
						if (reader.TokenType == JsonToken.EndConstructor)
						{
							break;
						}
						this.DeserializeValue(reader, document, manager, propertyName2, currentNode);
					}
					goto IL_162;
				}
				case JsonToken.PropertyName:
				{
					if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
					{
						goto Block_3;
					}
					string propertyName = reader.Value.ToString();
					reader.Read();
					if (reader.TokenType != JsonToken.StartArray)
					{
						this.DeserializeValue(reader, document, manager, propertyName, currentNode);
						goto IL_162;
					}
					int num = 0;
					while (reader.Read() && reader.TokenType != JsonToken.EndArray)
					{
						this.DeserializeValue(reader, document, manager, propertyName, currentNode);
						num++;
					}
					if (num == 1 && this.WriteArrayAttribute)
					{
						IXmlElement element = currentNode.ChildNodes.OfType<IXmlElement>().Single((IXmlElement n) => n.LocalName == propertyName);
						this.AddJsonArrayAttribute(element, document);
						goto IL_162;
					}
					goto IL_162;
				}
				case JsonToken.Comment:
					currentNode.AppendChild(document.CreateComment((string)reader.Value));
					goto IL_162;
				}
				break;
				IL_162:
				if (reader.TokenType != JsonToken.PropertyName && !reader.Read())
				{
					return;
				}
			}
			switch (tokenType)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
				return;
			default:
				throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + reader.TokenType);
			}
			Block_3:
			throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00009F18 File Offset: 0x00008118
		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
			{
				if (attributeName.Length == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName[5] == ':')
				{
					prefix = attributeName.Substring(6, attributeName.Length - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009F79 File Offset: 0x00008179
		private IEnumerable<IXmlNode> ValueAttributes(IEnumerable<IXmlNode> c)
		{
			return from a in c
			where a.NamespaceUri != "http://james.newtonking.com/projects/json"
			select a;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00009F9E File Offset: 0x0000819E
		public override bool CanConvert(Type valueType)
		{
			return typeof(XObject).IsAssignableFrom(valueType) || typeof(XmlNode).IsAssignableFrom(valueType);
		}

		// Token: 0x040000A9 RID: 169
		private const string TextName = "#text";

		// Token: 0x040000AA RID: 170
		private const string CommentName = "#comment";

		// Token: 0x040000AB RID: 171
		private const string CDataName = "#cdata-section";

		// Token: 0x040000AC RID: 172
		private const string WhitespaceName = "#whitespace";

		// Token: 0x040000AD RID: 173
		private const string SignificantWhitespaceName = "#significant-whitespace";

		// Token: 0x040000AE RID: 174
		private const string DeclarationName = "?xml";

		// Token: 0x040000AF RID: 175
		private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";
	}
}
