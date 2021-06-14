using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010D RID: 269
	internal sealed class JsonFullMetadataTypeNameOracle : JsonLightTypeNameOracle
	{
		// Token: 0x06000749 RID: 1865 RVA: 0x00018F98 File Offset: 0x00017198
		internal override string GetEntryTypeNameForWriting(string expectedTypeName, ODataEntry entry)
		{
			SerializationTypeNameAnnotation annotation = entry.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null)
			{
				return annotation.TypeName;
			}
			return entry.TypeName;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x00018FBC File Offset: 0x000171BC
		internal override string GetValueTypeNameForWriting(ODataValue value, IEdmTypeReference typeReferenceFromMetadata, IEdmTypeReference typeReferenceFromValue, bool isOpenProperty)
		{
			SerializationTypeNameAnnotation annotation = value.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null)
			{
				return annotation.TypeName;
			}
			if (typeReferenceFromValue != null && typeReferenceFromValue.IsPrimitive() && JsonSharedUtils.ValueTypeMatchesJsonType((ODataPrimitiveValue)value, typeReferenceFromValue.AsPrimitive()))
			{
				return null;
			}
			return TypeNameOracle.GetTypeNameFromValue(value);
		}
	}
}
