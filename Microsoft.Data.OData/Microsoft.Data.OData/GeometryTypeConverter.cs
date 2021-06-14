using System;
using System.Collections.Generic;
using System.Spatial;
using System.Xml;
using Microsoft.Data.OData.Atom;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData
{
	// Token: 0x020001DE RID: 478
	internal sealed class GeometryTypeConverter : IPrimitiveTypeConverter
	{
		// Token: 0x06000EC6 RID: 3782 RVA: 0x000340A4 File Offset: 0x000322A4
		public object TokenizeFromXml(XmlReader reader)
		{
			reader.ReadStartElement();
			Geometry result = GmlFormatter.Create().Read<Geometry>(reader);
			reader.SkipInsignificantNodes();
			return result;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000340CA File Offset: 0x000322CA
		public void WriteAtom(object instance, XmlWriter writer)
		{
			((Geometry)instance).SendTo(GmlFormatter.Create().CreateWriter(writer));
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00034100 File Offset: 0x00032300
		public void WriteVerboseJson(object instance, IJsonWriter jsonWriter, string typeName, ODataVersion odataVersion)
		{
			IDictionary<string, object> jsonObjectValue = GeoJsonObjectFormatter.Create().Write((ISpatial)instance);
			jsonWriter.WriteJsonObjectValue(jsonObjectValue, delegate(IJsonWriter jw)
			{
				ODataJsonWriterUtils.WriteMetadataWithTypeName(jw, typeName);
			}, odataVersion);
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00034140 File Offset: 0x00032340
		public void WriteJsonLight(object instance, IJsonWriter jsonWriter, ODataVersion odataVersion)
		{
			IDictionary<string, object> jsonObjectValue = GeoJsonObjectFormatter.Create().Write((ISpatial)instance);
			jsonWriter.WriteJsonObjectValue(jsonObjectValue, null, odataVersion);
		}
	}
}
