using System;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x0200008E RID: 142
	internal class PrimitiveTypeConverter
	{
		// Token: 0x06000527 RID: 1319 RVA: 0x00014C0E File Offset: 0x00012E0E
		protected PrimitiveTypeConverter()
		{
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014C18 File Offset: 0x00012E18
		internal virtual PrimitiveParserToken TokenizeFromXml(XmlReader reader)
		{
			string text = MaterializeAtom.ReadElementString(reader, true);
			if (text != null)
			{
				return new TextPrimitiveParserToken(text);
			}
			return null;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00014C38 File Offset: 0x00012E38
		internal virtual PrimitiveParserToken TokenizeFromText(string text)
		{
			return new TextPrimitiveParserToken(text);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00014C40 File Offset: 0x00012E40
		internal virtual object Parse(string text)
		{
			return text;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00014C43 File Offset: 0x00012E43
		internal virtual string ToString(object instance)
		{
			throw new NotImplementedException();
		}
	}
}
