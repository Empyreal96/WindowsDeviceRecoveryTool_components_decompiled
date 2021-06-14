using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004E RID: 78
	public sealed class WritingNavigationLinkArgs
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0000C934 File Offset: 0x0000AB34
		public WritingNavigationLinkArgs(ODataNavigationLink link, object source, object target)
		{
			Util.CheckArgumentNull<ODataNavigationLink>(link, "link");
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNull<object>(target, "target");
			this.Link = link;
			this.Source = source;
			this.Target = target;
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000C980 File Offset: 0x0000AB80
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000C988 File Offset: 0x0000AB88
		public ODataNavigationLink Link { get; private set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000C991 File Offset: 0x0000AB91
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000C999 File Offset: 0x0000AB99
		public object Source { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000C9A2 File Offset: 0x0000ABA2
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000C9AA File Offset: 0x0000ABAA
		public object Target { get; private set; }
	}
}
