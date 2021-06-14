using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000297 RID: 663
	internal static class ODataAtomWriterUtils
	{
		// Token: 0x06001664 RID: 5732 RVA: 0x00051234 File Offset: 0x0004F434
		internal static XmlWriter CreateXmlWriter(Stream stream, ODataMessageWriterSettings messageWriterSettings, Encoding encoding)
		{
			XmlWriterSettings settings = ODataAtomWriterUtils.CreateXmlWriterSettings(messageWriterSettings, encoding);
			XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
			if (messageWriterSettings.AlwaysUseDefaultXmlNamespaceForRootElement)
			{
				xmlWriter = new DefaultNamespaceCompensatingXmlWriter(xmlWriter);
			}
			return xmlWriter;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00051261 File Offset: 0x0004F461
		internal static void WriteError(XmlWriter writer, ODataError error, bool includeDebugInformation, int maxInnerErrorDepth)
		{
			ErrorUtils.WriteXmlError(writer, error, includeDebugInformation, maxInnerErrorDepth);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0005126C File Offset: 0x0004F46C
		internal static void WriteETag(XmlWriter writer, string etag)
		{
			writer.WriteAttributeString("m", "etag", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", etag);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00051284 File Offset: 0x0004F484
		internal static void WriteNullAttribute(XmlWriter writer)
		{
			writer.WriteAttributeString("m", "null", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", "true");
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000512A0 File Offset: 0x0004F4A0
		internal static void WriteRaw(XmlWriter writer, string value)
		{
			ODataAtomWriterUtils.WritePreserveSpaceAttributeIfNeeded(writer, value);
			writer.WriteRaw(value);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000512B0 File Offset: 0x0004F4B0
		internal static void WriteString(XmlWriter writer, string value)
		{
			ODataAtomWriterUtils.WritePreserveSpaceAttributeIfNeeded(writer, value);
			writer.WriteString(value);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000512C0 File Offset: 0x0004F4C0
		private static XmlWriterSettings CreateXmlWriterSettings(ODataMessageWriterSettings messageWriterSettings, Encoding encoding)
		{
			return new XmlWriterSettings
			{
				CheckCharacters = messageWriterSettings.CheckCharacters,
				ConformanceLevel = ConformanceLevel.Document,
				OmitXmlDeclaration = false,
				Encoding = (encoding ?? MediaTypeUtils.EncodingUtf8NoPreamble),
				NewLineHandling = NewLineHandling.Entitize,
				Indent = messageWriterSettings.Indent,
				CloseOutput = false
			};
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x00051318 File Offset: 0x0004F518
		private static void WritePreserveSpaceAttributeIfNeeded(XmlWriter writer, string value)
		{
			if (value == null)
			{
				return;
			}
			int length = value.Length;
			if (length > 0 && (char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[length - 1])))
			{
				writer.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
			}
		}
	}
}
