using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000230 RID: 560
	internal sealed class ODataVerboseJsonCollectionDeserializer : ODataVerboseJsonPropertyAndValueDeserializer
	{
		// Token: 0x060011D1 RID: 4561 RVA: 0x000420FB File Offset: 0x000402FB
		internal ODataVerboseJsonCollectionDeserializer(ODataVerboseJsonInputContext jsonInputContext) : base(jsonInputContext)
		{
			this.duplicatePropertyNamesChecker = base.CreateDuplicatePropertyNamesChecker();
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00042110 File Offset: 0x00040310
		internal ODataCollectionStart ReadCollectionStart(bool isResultsWrapperExpected)
		{
			if (isResultsWrapperExpected)
			{
				base.JsonReader.ReadStartObject();
				bool flag = false;
				while (base.JsonReader.NodeType == JsonNodeType.Property)
				{
					string strB = base.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal("results", strB) == 0)
					{
						flag = true;
						break;
					}
					base.JsonReader.SkipValue();
				}
				if (!flag)
				{
					throw new ODataException(Strings.ODataJsonCollectionDeserializer_MissingResultsPropertyForCollection);
				}
			}
			if (base.JsonReader.NodeType != JsonNodeType.StartArray)
			{
				throw new ODataException(Strings.ODataJsonCollectionDeserializer_CannotReadCollectionContentStart(base.JsonReader.NodeType));
			}
			return new ODataCollectionStart
			{
				Name = null
			};
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x000421AC File Offset: 0x000403AC
		internal object ReadCollectionItem(IEdmTypeReference expectedItemTypeReference, CollectionWithoutExpectedTypeValidator collectionValidator)
		{
			return base.ReadNonEntityValue(expectedItemTypeReference, this.duplicatePropertyNamesChecker, collectionValidator, true, null);
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x000421CC File Offset: 0x000403CC
		internal void ReadCollectionEnd(bool isResultsWrapperExpected)
		{
			base.JsonReader.ReadEndArray();
			if (!isResultsWrapperExpected)
			{
				return;
			}
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("results", strB) == 0)
				{
					throw new ODataException(Strings.ODataJsonCollectionDeserializer_MultipleResultsPropertiesForCollection);
				}
				base.JsonReader.SkipValue();
			}
			base.JsonReader.ReadEndObject();
		}

		// Token: 0x04000687 RID: 1671
		private readonly DuplicatePropertyNamesChecker duplicatePropertyNamesChecker;
	}
}
