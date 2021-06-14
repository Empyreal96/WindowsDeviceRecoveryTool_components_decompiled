using System;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x020001AF RID: 431
	internal static class ODataJsonLightWriterUtils
	{
		// Token: 0x06000D5F RID: 3423 RVA: 0x0002DE24 File Offset: 0x0002C024
		internal static void WriteODataTypeInstanceAnnotation(IJsonWriter jsonWriter, string typeName)
		{
			jsonWriter.WriteName("odata.type");
			jsonWriter.WriteValue(typeName);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0002DE38 File Offset: 0x0002C038
		internal static void WriteODataTypePropertyAnnotation(IJsonWriter jsonWriter, string propertyName, string typeName)
		{
			jsonWriter.WritePropertyAnnotationName(propertyName, "odata.type");
			jsonWriter.WriteValue(typeName);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0002DE4D File Offset: 0x0002C04D
		internal static void WriteValuePropertyName(this IJsonWriter jsonWriter)
		{
			jsonWriter.WriteName("value");
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0002DE5A File Offset: 0x0002C05A
		internal static void WritePropertyAnnotationName(this IJsonWriter jsonWriter, string propertyName, string annotationName)
		{
			jsonWriter.WriteName(propertyName + '@' + annotationName);
		}
	}
}
