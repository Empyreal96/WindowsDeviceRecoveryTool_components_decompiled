using System;
using System.Spatial;
using System.Xml;

namespace System.Data.Services.Client
{
	// Token: 0x020000A9 RID: 169
	internal sealed class GeometryTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x00015004 File Offset: 0x00013204
		internal override PrimitiveParserToken TokenizeFromXml(XmlReader reader)
		{
			reader.ReadStartElement();
			return new InstancePrimitiveParserToken<Geometry>(GmlFormatter.Create().Read<Geometry>(reader));
		}
	}
}
