using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Evaluation;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x0200010E RID: 270
	internal sealed class JsonMinimalMetadataLevel : JsonLightMetadataLevel
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x00019008 File Offset: 0x00017208
		internal override JsonLightTypeNameOracle GetTypeNameOracle(bool autoComputePayloadMetadataInJson)
		{
			return new JsonMinimalMetadataTypeNameOracle();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001900F File Offset: 0x0001720F
		internal override bool ShouldWriteODataMetadataUri()
		{
			return true;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x00019012 File Offset: 0x00017212
		internal override ODataEntityMetadataBuilder CreateEntityMetadataBuilder(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntityType actualEntityType, SelectedPropertiesNode selectedProperties, bool isResponse, bool? keyAsSegment)
		{
			return null;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x00019015 File Offset: 0x00017215
		internal override void InjectMetadataBuilder(ODataEntry entry, ODataEntityMetadataBuilder builder)
		{
		}
	}
}
