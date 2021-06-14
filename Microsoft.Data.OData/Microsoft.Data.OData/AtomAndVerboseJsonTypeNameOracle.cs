using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData
{
	// Token: 0x020000E5 RID: 229
	internal sealed class AtomAndVerboseJsonTypeNameOracle : TypeNameOracle
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x00013C10 File Offset: 0x00011E10
		internal string GetEntryTypeNameForWriting(ODataEntry entry)
		{
			SerializationTypeNameAnnotation annotation = entry.GetAnnotation<SerializationTypeNameAnnotation>();
			if (annotation != null)
			{
				return annotation.TypeName;
			}
			return entry.TypeName;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00013C34 File Offset: 0x00011E34
		internal string GetValueTypeNameForWriting(object value, IEdmTypeReference typeReferenceFromValue, SerializationTypeNameAnnotation typeNameAnnotation, CollectionWithoutExpectedTypeValidator collectionValidator, out string collectionItemTypeName)
		{
			collectionItemTypeName = null;
			string text = TypeNameOracle.GetTypeNameFromValue(value);
			if (text == null && typeReferenceFromValue != null)
			{
				text = typeReferenceFromValue.ODataFullName();
			}
			if (text != null)
			{
				if (collectionValidator != null && string.CompareOrdinal(collectionValidator.ItemTypeNameFromCollection, text) == 0)
				{
					text = null;
				}
				if (text != null && value is ODataCollectionValue)
				{
					collectionItemTypeName = ValidationUtils.ValidateCollectionTypeName(text);
				}
			}
			if (typeNameAnnotation != null)
			{
				text = typeNameAnnotation.TypeName;
			}
			return text;
		}
	}
}
