using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x02000236 RID: 566
	internal sealed class ODataVerboseJsonServiceDocumentDeserializer : ODataVerboseJsonDeserializer
	{
		// Token: 0x06001226 RID: 4646 RVA: 0x00044282 File Offset: 0x00042482
		internal ODataVerboseJsonServiceDocumentDeserializer(ODataVerboseJsonInputContext verboseJsonInputContext) : base(verboseJsonInputContext)
		{
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004428C File Offset: 0x0004248C
		internal ODataWorkspace ReadServiceDocument()
		{
			List<ODataResourceCollectionInfo> list = null;
			base.ReadPayloadStart(false);
			base.JsonReader.ReadStartObject();
			while (base.JsonReader.NodeType == JsonNodeType.Property)
			{
				string strB = base.JsonReader.ReadPropertyName();
				if (string.CompareOrdinal("EntitySets", strB) == 0)
				{
					if (list != null)
					{
						throw new ODataException(Strings.ODataJsonServiceDocumentDeserializer_MultipleEntitySetsPropertiesForServiceDocument);
					}
					list = new List<ODataResourceCollectionInfo>();
					base.JsonReader.ReadStartArray();
					while (base.JsonReader.NodeType != JsonNodeType.EndArray)
					{
						string text = base.JsonReader.ReadStringValue();
						ValidationUtils.ValidateResourceCollectionInfoUrl(text);
						ODataResourceCollectionInfo item = new ODataResourceCollectionInfo
						{
							Url = base.ProcessUriFromPayload(text, false)
						};
						list.Add(item);
					}
					base.JsonReader.ReadEndArray();
				}
				else
				{
					base.JsonReader.SkipValue();
				}
			}
			if (list == null)
			{
				throw new ODataException(Strings.ODataJsonServiceDocumentDeserializer_NoEntitySetsPropertyForServiceDocument);
			}
			base.JsonReader.ReadEndObject();
			base.ReadPayloadEnd(false);
			return new ODataWorkspace
			{
				Collections = new ReadOnlyEnumerable<ODataResourceCollectionInfo>(list)
			};
		}
	}
}
