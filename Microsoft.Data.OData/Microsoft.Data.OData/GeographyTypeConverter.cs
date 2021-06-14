using System;
using System.Collections.Generic;
using System.Spatial;
using System.Xml;
using Microsoft.Data.OData.Atom;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData
{
	// Token: 0x0200021D RID: 541
	internal sealed class GeographyTypeConverter : IPrimitiveTypeConverter
	{
		// Token: 0x060010D9 RID: 4313 RVA: 0x0003F14C File Offset: 0x0003D34C
		public object TokenizeFromXml(XmlReader reader)
		{
			reader.ReadStartElement();
			Geography result = GmlFormatter.Create().Read<Geography>(reader);
			reader.SkipInsignificantNodes();
			return result;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0003F172 File Offset: 0x0003D372
		public void WriteAtom(object instance, XmlWriter writer)
		{
			((Geography)instance).SendTo(GmlFormatter.Create().CreateWriter(writer));
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0003F1A8 File Offset: 0x0003D3A8
		public void WriteVerboseJson(object instance, IJsonWriter jsonWriter, string typeName, ODataVersion odataVersion)
		{
			IDictionary<string, object> jsonObjectValue = GeoJsonObjectFormatter.Create().Write((ISpatial)instance);
			jsonWriter.WriteJsonObjectValue(jsonObjectValue, delegate(IJsonWriter jw)
			{
				ODataJsonWriterUtils.WriteMetadataWithTypeName(jw, typeName);
			}, odataVersion);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
		public void WriteJsonLight(object instance, IJsonWriter jsonWriter, ODataVersion odataVersion)
		{
			IDictionary<string, object> jsonObjectValue = GeoJsonObjectFormatter.Create().Write((ISpatial)instance);
			jsonWriter.WriteJsonObjectValue(jsonObjectValue, null, odataVersion);
		}
	}
}
