using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x02000050 RID: 80
	public sealed class ReadingNavigationLinkArgs
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000CD7C File Offset: 0x0000AF7C
		public ReadingNavigationLinkArgs(ODataNavigationLink link)
		{
			Util.CheckArgumentNull<ODataNavigationLink>(link, "link");
			this.Link = link;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000CD97 File Offset: 0x0000AF97
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000CD9F File Offset: 0x0000AF9F
		public ODataNavigationLink Link { get; private set; }
	}
}
