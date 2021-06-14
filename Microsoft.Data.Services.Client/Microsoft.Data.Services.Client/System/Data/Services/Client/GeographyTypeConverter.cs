using System;
using System.Spatial;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A8 RID: 168
	internal sealed class GeographyTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x00014FD4 File Offset: 0x000131D4
		internal override PrimitiveParserToken TokenizeFromXml(XmlReader reader)
		{
			reader.ReadStartElement();
			return new InstancePrimitiveParserToken<Geography>(GmlFormatter.Create().Read<Geography>(reader));
		}
	}
}
