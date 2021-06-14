using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004C RID: 76
	public sealed class WritingEntityReferenceLinkArgs
	{
		// Token: 0x06000276 RID: 630 RVA: 0x0000C864 File Offset: 0x0000AA64
		public WritingEntityReferenceLinkArgs(ODataEntityReferenceLink entityReferenceLink, object source, object target)
		{
			Util.CheckArgumentNull<ODataEntityReferenceLink>(entityReferenceLink, "entityReferenceLink");
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNull<object>(target, "target");
			this.EntityReferenceLink = entityReferenceLink;
			this.Source = source;
			this.Target = target;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		// (set) Token: 0x06000278 RID: 632 RVA: 0x0000C8B8 File Offset: 0x0000AAB8
		public ODataEntityReferenceLink EntityReferenceLink { get; private set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000C8C1 File Offset: 0x0000AAC1
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000C8C9 File Offset: 0x0000AAC9
		public object Source { get; private set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000C8D2 File Offset: 0x0000AAD2
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000C8DA File Offset: 0x0000AADA
		public object Target { get; private set; }
	}
}
