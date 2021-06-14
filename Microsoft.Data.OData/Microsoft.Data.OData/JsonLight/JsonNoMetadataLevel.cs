using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000110 RID: 272
	internal sealed class JsonNoMetadataLevel : JsonLightMetadataLevel
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x000190C3 File Offset: 0x000172C3
		internal override JsonLightTypeNameOracle GetTypeNameOracle(bool autoComputePayloadMetadataInJson)
		{
			if (autoComputePayloadMetadataInJson)
			{
				return new JsonNoMetadataTypeNameOracle();
			}
			return new JsonMinimalMetadataTypeNameOracle();
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000190D3 File Offset: 0x000172D3
		internal override bool ShouldWriteODataMetadataUri()
		{
			return false;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000190D6 File Offset: 0x000172D6
		internal override ODataEntityMetadataBuilder CreateEntityMetadataBuilder(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntityType actualEntityType, SelectedPropertiesNode selectedProperties, bool isResponse, bool? keyAsSegment)
		{
			return ODataEntityMetadataBuilder.Null;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000190DD File Offset: 0x000172DD
		internal override void InjectMetadataBuilder(ODataEntry entry, ODataEntityMetadataBuilder builder)
		{
			entry.MetadataBuilder = builder;
		}
	}
}
