using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000248 RID: 584
	public enum ODataReaderState
	{
		// Token: 0x040006B0 RID: 1712
		Start,
		// Token: 0x040006B1 RID: 1713
		FeedStart,
		// Token: 0x040006B2 RID: 1714
		FeedEnd,
		// Token: 0x040006B3 RID: 1715
		EntryStart,
		// Token: 0x040006B4 RID: 1716
		EntryEnd,
		// Token: 0x040006B5 RID: 1717
		NavigationLinkStart,
		// Token: 0x040006B6 RID: 1718
		NavigationLinkEnd,
		// Token: 0x040006B7 RID: 1719
		EntityReferenceLink,
		// Token: 0x040006B8 RID: 1720
		Exception,
		// Token: 0x040006B9 RID: 1721
		Completed
	}
}
