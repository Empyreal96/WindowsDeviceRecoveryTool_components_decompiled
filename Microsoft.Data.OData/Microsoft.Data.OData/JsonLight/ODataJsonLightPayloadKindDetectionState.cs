using System;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000140 RID: 320
	internal sealed class ODataJsonLightPayloadKindDetectionState
	{
		// Token: 0x06000890 RID: 2192 RVA: 0x0001BE04 File Offset: 0x0001A004
		internal ODataJsonLightPayloadKindDetectionState(ODataJsonLightMetadataUriParseResult metadataUriParseResult)
		{
			this.metadataUriParseResult = metadataUriParseResult;
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x0001BE13 File Offset: 0x0001A013
		internal ODataJsonLightMetadataUriParseResult MetadataUriParseResult
		{
			get
			{
				return this.metadataUriParseResult;
			}
		}

		// Token: 0x0400034A RID: 842
		private readonly ODataJsonLightMetadataUriParseResult metadataUriParseResult;
	}
}
