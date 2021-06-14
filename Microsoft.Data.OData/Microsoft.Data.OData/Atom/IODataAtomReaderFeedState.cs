using System;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x02000217 RID: 535
	internal interface IODataAtomReaderFeedState
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001090 RID: 4240
		ODataFeed Feed { get; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001091 RID: 4241
		// (set) Token: 0x06001092 RID: 4242
		bool FeedElementEmpty { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001093 RID: 4243
		AtomFeedMetadata AtomFeedMetadata { get; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001094 RID: 4244
		// (set) Token: 0x06001095 RID: 4245
		bool HasCount { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001096 RID: 4246
		// (set) Token: 0x06001097 RID: 4247
		bool HasNextPageLink { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001098 RID: 4248
		// (set) Token: 0x06001099 RID: 4249
		bool HasReadLink { get; set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600109A RID: 4250
		// (set) Token: 0x0600109B RID: 4251
		bool HasDeltaLink { get; set; }
	}
}
