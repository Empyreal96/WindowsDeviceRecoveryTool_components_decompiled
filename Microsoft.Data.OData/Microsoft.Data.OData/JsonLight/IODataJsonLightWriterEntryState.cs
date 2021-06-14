using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.JsonLight
{
	// Token: 0x02000166 RID: 358
	internal interface IODataJsonLightWriterEntryState
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060009F8 RID: 2552
		ODataEntry Entry { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060009F9 RID: 2553
		IEdmEntityType EntityType { get; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060009FA RID: 2554
		IEdmEntityType EntityTypeFromMetadata { get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060009FB RID: 2555
		ODataFeedAndEntrySerializationInfo SerializationInfo { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060009FC RID: 2556
		// (set) Token: 0x060009FD RID: 2557
		bool EditLinkWritten { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060009FE RID: 2558
		// (set) Token: 0x060009FF RID: 2559
		bool ReadLinkWritten { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A00 RID: 2560
		// (set) Token: 0x06000A01 RID: 2561
		bool MediaEditLinkWritten { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A02 RID: 2562
		// (set) Token: 0x06000A03 RID: 2563
		bool MediaReadLinkWritten { get; set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000A04 RID: 2564
		// (set) Token: 0x06000A05 RID: 2565
		bool MediaContentTypeWritten { get; set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000A06 RID: 2566
		// (set) Token: 0x06000A07 RID: 2567
		bool MediaETagWritten { get; set; }

		// Token: 0x06000A08 RID: 2568
		ODataFeedAndEntryTypeContext GetOrCreateTypeContext(IEdmModel model, bool writingResponse);
	}
}
