using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000227 RID: 551
	internal static class ODataAtomReaderUtils
	{
		// Token: 0x06001156 RID: 4438 RVA: 0x00040858 File Offset: 0x0003EA58
		internal static XmlReader CreateXmlReader(Stream stream, Encoding encoding, ODataMessageReaderSettings messageReaderSettings)
		{
			XmlReaderSettings settings = ODataAtomReaderUtils.CreateXmlReaderSettings(messageReaderSettings);
			if (encoding != null)
			{
				return XmlReader.Create(new StreamReader(stream, encoding), settings);
			}
			return XmlReader.Create(stream, settings);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00040884 File Offset: 0x0003EA84
		internal static bool ReadMetadataNullAttributeValue(string attributeValue)
		{
			return XmlConvert.ToBoolean(attributeValue);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0004088C File Offset: 0x0003EA8C
		private static XmlReaderSettings CreateXmlReaderSettings(ODataMessageReaderSettings messageReaderSettings)
		{
			return new XmlReaderSettings
			{
				CheckCharacters = messageReaderSettings.CheckCharacters,
				ConformanceLevel = ConformanceLevel.Document,
				CloseInput = true,
				DtdProcessing = DtdProcessing.Prohibit
			};
		}
	}
}
