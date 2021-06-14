using System;
using System.Xml;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData
{
	// Token: 0x020001DD RID: 477
	internal interface IPrimitiveTypeConverter
	{
		// Token: 0x06000EC2 RID: 3778
		object TokenizeFromXml(XmlReader reader);

		// Token: 0x06000EC3 RID: 3779
		void WriteAtom(object instance, XmlWriter writer);

		// Token: 0x06000EC4 RID: 3780
		void WriteVerboseJson(object instance, IJsonWriter jsonWriter, string typeName, ODataVersion odataVersion);

		// Token: 0x06000EC5 RID: 3781
		void WriteJsonLight(object instance, IJsonWriter jsonWriter, ODataVersion odataVersion);
	}
}
