using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010F RID: 271
	internal sealed class JsonMinimalMetadataTypeNameOracle : JsonLightTypeNameOracle
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x00019020 File Offset: 0x00017220
		internal override string GetEntryTypeNameForWriting(string expectedTypeName, ODataEntry entry)
		{
			SerializationTypeNameAnnotation annotation = entry.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null)
			{
				return annotation.TypeName;
			}
			string typeName = entry.TypeName;
			if (expectedTypeName != typeName)
			{
				return typeName;
			}
			return null;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00019054 File Offset: 0x00017254
		internal override string GetValueTypeNameForWriting(ODataValue value, IEdmTypeReference typeReferenceFromMetadata, IEdmTypeReference typeReferenceFromValue, bool isOpenProperty)
		{
			SerializationTypeNameAnnotation annotation = value.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null)
			{
				return annotation.TypeName;
			}
			if (typeReferenceFromValue != null)
			{
				if (typeReferenceFromMetadata != null && typeReferenceFromMetadata.ODataFullName() != typeReferenceFromValue.ODataFullName())
				{
					return typeReferenceFromValue.ODataFullName();
				}
				if (typeReferenceFromValue.IsPrimitive() && JsonSharedUtils.ValueTypeMatchesJsonType((ODataPrimitiveValue)value, typeReferenceFromValue.AsPrimitive()))
				{
					return null;
				}
			}
			if (!isOpenProperty)
			{
				return null;
			}
			return TypeNameOracle.GetTypeNameFromValue(value);
		}
	}
}
