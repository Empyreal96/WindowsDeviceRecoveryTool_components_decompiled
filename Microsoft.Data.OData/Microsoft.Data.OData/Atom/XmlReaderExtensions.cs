using System;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200022B RID: 555
	internal static class XmlReaderExtensions
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x00041636 File Offset: 0x0003F836
		[Conditional("DEBUG")]
		internal static void AssertNotBuffering(this BufferingXmlReader bufferedXmlReader)
		{
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00041638 File Offset: 0x0003F838
		[Conditional("DEBUG")]
		internal static void AssertBuffering(this BufferingXmlReader bufferedXmlReader)
		{
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004163C File Offset: 0x0003F83C
		internal static string ReadElementValue(this XmlReader reader)
		{
			string result = reader.ReadElementContentValue();
			reader.Read();
			return result;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00041658 File Offset: 0x0003F858
		internal static string ReadFirstTextNodeValue(this XmlReader reader)
		{
			reader.MoveToElement();
			string text = null;
			if (!reader.IsEmptyElement)
			{
				bool flag = false;
				while (!flag && reader.Read())
				{
					XmlNodeType nodeType = reader.NodeType;
					switch (nodeType)
					{
					case XmlNodeType.Element:
						reader.SkipElementContent();
						continue;
					case XmlNodeType.Attribute:
						continue;
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
						break;
					default:
						switch (nodeType)
						{
						case XmlNodeType.SignificantWhitespace:
							break;
						case XmlNodeType.EndElement:
							flag = true;
							continue;
						default:
							continue;
						}
						break;
					}
					if (text == null)
					{
						text = reader.Value;
					}
				}
			}
			reader.Read();
			return text ?? string.Empty;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x000416E0 File Offset: 0x0003F8E0
		internal static string ReadElementContentValue(this XmlReader reader)
		{
			reader.MoveToElement();
			string text = null;
			if (reader.IsEmptyElement)
			{
				text = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = null;
				bool flag = false;
				while (!flag && reader.Read())
				{
					switch (reader.NodeType)
					{
					case XmlNodeType.Text:
					case XmlNodeType.CDATA:
					case XmlNodeType.SignificantWhitespace:
						if (text == null)
						{
							text = reader.Value;
							continue;
						}
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder();
							stringBuilder.Append(text);
							stringBuilder.Append(reader.Value);
							continue;
						}
						stringBuilder.Append(reader.Value);
						continue;
					case XmlNodeType.ProcessingInstruction:
					case XmlNodeType.Comment:
					case XmlNodeType.Whitespace:
						continue;
					case XmlNodeType.EndElement:
						flag = true;
						continue;
					}
					throw new ODataException(Strings.XmlReaderExtension_InvalidNodeInStringValue(reader.NodeType));
				}
				if (stringBuilder != null)
				{
					text = stringBuilder.ToString();
				}
				else if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000417D0 File Offset: 0x0003F9D0
		internal static void SkipInsignificantNodes(this XmlReader reader)
		{
			for (;;)
			{
				XmlNodeType nodeType = reader.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.None:
				case XmlNodeType.ProcessingInstruction:
				case XmlNodeType.Comment:
					break;
				case XmlNodeType.Element:
					return;
				case XmlNodeType.Attribute:
				case XmlNodeType.CDATA:
				case XmlNodeType.EntityReference:
				case XmlNodeType.Entity:
					return;
				case XmlNodeType.Text:
					if (!XmlReaderExtensions.IsNullOrWhitespace(reader.Value))
					{
						return;
					}
					break;
				default:
					if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.XmlDeclaration)
					{
						return;
					}
					break;
				}
				if (!reader.Read())
				{
					return;
				}
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004182F File Offset: 0x0003FA2F
		internal static void SkipElementContent(this XmlReader reader)
		{
			reader.MoveToElement();
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (reader.NodeType != XmlNodeType.EndElement)
				{
					reader.Skip();
				}
			}
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00041859 File Offset: 0x0003FA59
		internal static void ReadPayloadStart(this XmlReader reader)
		{
			reader.SkipInsignificantNodes();
			if (reader.NodeType != XmlNodeType.Element)
			{
				throw new ODataException(Strings.XmlReaderExtension_InvalidRootNode(reader.NodeType));
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00041880 File Offset: 0x0003FA80
		internal static void ReadPayloadEnd(this XmlReader reader)
		{
			reader.SkipInsignificantNodes();
			if (reader.NodeType != XmlNodeType.None && !reader.EOF)
			{
				throw new ODataException(Strings.XmlReaderExtension_InvalidRootNode(reader.NodeType));
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000418AE File Offset: 0x0003FAAE
		internal static bool NamespaceEquals(this XmlReader reader, string namespaceUri)
		{
			return object.ReferenceEquals(reader.NamespaceURI, namespaceUri);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x000418BC File Offset: 0x0003FABC
		internal static bool LocalNameEquals(this XmlReader reader, string localName)
		{
			return object.ReferenceEquals(reader.LocalName, localName);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x000418CA File Offset: 0x0003FACA
		internal static bool TryReadEmptyElement(this XmlReader reader)
		{
			reader.MoveToElement();
			return reader.IsEmptyElement || (reader.Read() && reader.NodeType == XmlNodeType.EndElement);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x000418F2 File Offset: 0x0003FAF2
		internal static bool TryReadToNextElement(this XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004190A File Offset: 0x0003FB0A
		private static bool IsNullOrWhitespace(string text)
		{
			return string.IsNullOrWhiteSpace(text);
		}
	}
}
