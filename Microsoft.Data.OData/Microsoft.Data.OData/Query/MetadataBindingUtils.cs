using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200003E RID: 62
	internal static class MetadataBindingUtils
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00006E44 File Offset: 0x00005044
		internal static SingleValueNode ConvertToTypeIfNeeded(SingleValueNode source, IEdmTypeReference targetTypeReference)
		{
			if (targetTypeReference == null)
			{
				return source;
			}
			if (source.TypeReference != null)
			{
				if (source.TypeReference.IsEquivalentTo(targetTypeReference))
				{
					return source;
				}
				if (!TypePromotionUtils.CanConvertTo(source.TypeReference, targetTypeReference))
				{
					throw new ODataException(Strings.MetadataBinder_CannotConvertToType(source.TypeReference.ODataFullName(), targetTypeReference.ODataFullName()));
				}
			}
			return new ConvertNode(source, targetTypeReference);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006EA0 File Offset: 0x000050A0
		internal static IEdmType GetEdmType(this QueryNode segment)
		{
			SingleValueNode singleValueNode = segment as SingleValueNode;
			if (singleValueNode != null)
			{
				IEdmTypeReference typeReference = singleValueNode.TypeReference;
				if (typeReference == null)
				{
					return null;
				}
				return typeReference.Definition;
			}
			else
			{
				CollectionNode collectionNode = segment as CollectionNode;
				if (collectionNode == null)
				{
					return null;
				}
				IEdmTypeReference itemType = collectionNode.ItemType;
				if (itemType == null)
				{
					return null;
				}
				return itemType.Definition;
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006EE8 File Offset: 0x000050E8
		internal static IEdmTypeReference GetEdmTypeReference(this QueryNode segment)
		{
			SingleValueNode singleValueNode = segment as SingleValueNode;
			if (singleValueNode != null)
			{
				return singleValueNode.TypeReference;
			}
			CollectionNode collectionNode = segment as CollectionNode;
			if (collectionNode != null)
			{
				return collectionNode.ItemType;
			}
			return null;
		}
	}
}
