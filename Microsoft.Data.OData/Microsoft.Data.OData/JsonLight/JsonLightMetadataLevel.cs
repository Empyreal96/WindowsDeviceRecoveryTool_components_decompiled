using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010A RID: 266
	internal abstract class JsonLightMetadataLevel
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x00018D40 File Offset: 0x00016F40
		internal static JsonLightMetadataLevel Create(MediaType mediaType, Uri metadataDocumentUri, IEdmModel model, bool writingResponse)
		{
			if (writingResponse && mediaType.Parameters != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in mediaType.Parameters)
				{
					if (HttpUtils.CompareMediaTypeParameterNames(keyValuePair.Key, "odata"))
					{
						if (string.Compare(keyValuePair.Value, "minimalmetadata", StringComparison.OrdinalIgnoreCase) == 0)
						{
							return new JsonMinimalMetadataLevel();
						}
						if (string.Compare(keyValuePair.Value, "fullmetadata", StringComparison.OrdinalIgnoreCase) == 0)
						{
							return new JsonFullMetadataLevel(metadataDocumentUri, model);
						}
						if (string.Compare(keyValuePair.Value, "nometadata", StringComparison.OrdinalIgnoreCase) == 0)
						{
							return new JsonNoMetadataLevel();
						}
					}
				}
			}
			return new JsonMinimalMetadataLevel();
		}

		// Token: 0x0600073B RID: 1851
		internal abstract JsonLightTypeNameOracle GetTypeNameOracle(bool autoComputePayloadMetadataInJson);

		// Token: 0x0600073C RID: 1852
		internal abstract bool ShouldWriteODataMetadataUri();

		// Token: 0x0600073D RID: 1853
		internal abstract ODataEntityMetadataBuilder CreateEntityMetadataBuilder(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntityType actualEntityType, SelectedPropertiesNode selectedProperties, bool isResponse, bool? keyAsSegment);

		// Token: 0x0600073E RID: 1854
		internal abstract void InjectMetadataBuilder(ODataEntry entry, ODataEntityMetadataBuilder builder);
	}
}
