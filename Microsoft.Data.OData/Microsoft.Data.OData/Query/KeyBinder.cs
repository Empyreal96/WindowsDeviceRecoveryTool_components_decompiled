using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Data.OData.Query.SyntacticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000032 RID: 50
	internal sealed class KeyBinder
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00005F4E File Offset: 0x0000414E
		internal KeyBinder(MetadataBinder.QueryTokenVisitor keyValueBindMethod)
		{
			this.keyValueBindMethod = keyValueBindMethod;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005F60 File Offset: 0x00004160
		internal QueryNode BindKeyValues(EntityCollectionNode collectionNode, IEnumerable<NamedValue> namedValues)
		{
			IEdmEntityTypeReference entityItemType = collectionNode.EntityItemType;
			List<KeyPropertyValue> list = new List<KeyPropertyValue>();
			IEdmEntityType edmEntityType = entityItemType.EntityDefinition();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (NamedValue namedValue in namedValues)
			{
				KeyPropertyValue keyPropertyValue = this.BindKeyPropertyValue(namedValue, edmEntityType);
				if (!hashSet.Add(keyPropertyValue.KeyProperty.Name))
				{
					throw new ODataException(Strings.MetadataBinder_DuplicitKeyPropertyInKeyValues(keyPropertyValue.KeyProperty.Name));
				}
				list.Add(keyPropertyValue);
			}
			if (list.Count == 0)
			{
				return collectionNode;
			}
			if (list.Count != edmEntityType.Key().Count<IEdmStructuralProperty>())
			{
				throw new ODataException(Strings.MetadataBinder_NotAllKeyPropertiesSpecifiedInKeyValues(collectionNode.ItemType.ODataFullName()));
			}
			return new KeyLookupNode(collectionNode, new ReadOnlyCollection<KeyPropertyValue>(list));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006068 File Offset: 0x00004268
		private KeyPropertyValue BindKeyPropertyValue(NamedValue namedValue, IEdmEntityType collectionItemEntityType)
		{
			ExceptionUtils.CheckArgumentNotNull<NamedValue>(namedValue, "namedValue");
			ExceptionUtils.CheckArgumentNotNull<LiteralToken>(namedValue.Value, "namedValue.Value");
			IEdmProperty edmProperty = null;
			if (namedValue.Name == null)
			{
				using (IEnumerator<IEdmStructuralProperty> enumerator = collectionItemEntityType.Key().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IEdmProperty edmProperty2 = enumerator.Current;
						if (edmProperty != null)
						{
							throw new ODataException(Strings.MetadataBinder_UnnamedKeyValueOnTypeWithMultipleKeyProperties(collectionItemEntityType.ODataFullName()));
						}
						edmProperty = edmProperty2;
					}
					goto IL_D5;
				}
			}
			edmProperty = (from k in collectionItemEntityType.Key()
			where string.CompareOrdinal(k.Name, namedValue.Name) == 0
			select k).SingleOrDefault<IEdmStructuralProperty>();
			if (edmProperty == null)
			{
				throw new ODataException(Strings.MetadataBinder_PropertyNotDeclaredOrNotKeyInKeyValue(namedValue.Name, collectionItemEntityType.ODataFullName()));
			}
			IL_D5:
			IEdmTypeReference type = edmProperty.Type;
			SingleValueNode singleValueNode = (SingleValueNode)this.keyValueBindMethod(namedValue.Value);
			singleValueNode = MetadataBindingUtils.ConvertToTypeIfNeeded(singleValueNode, type);
			return new KeyPropertyValue
			{
				KeyProperty = edmProperty,
				KeyValue = singleValueNode
			};
		}

		// Token: 0x04000063 RID: 99
		private readonly MetadataBinder.QueryTokenVisitor keyValueBindMethod;
	}
}
