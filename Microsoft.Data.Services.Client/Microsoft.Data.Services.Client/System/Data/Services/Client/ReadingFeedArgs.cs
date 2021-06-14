using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000051 RID: 81
	public sealed class ReadingFeedArgs
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
		public ReadingFeedArgs(ODataFeed feed)
		{
			Util.CheckArgumentNull<ODataFeed>(feed, "feed");
			this.Feed = feed;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000CDC3 File Offset: 0x0000AFC3
		// (set) Token: 0x060002A0 RID: 672 RVA: 0x0000CDCB File Offset: 0x0000AFCB
		public ODataFeed Feed { get; private set; }
	}
}
