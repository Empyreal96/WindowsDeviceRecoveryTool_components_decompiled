using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004D RID: 77
	public sealed class WritingEntryArgs
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000C8E3 File Offset: 0x0000AAE3
		public WritingEntryArgs(ODataEntry entry, object entity)
		{
			Util.CheckArgumentNull<ODataEntry>(entry, "entry");
			Util.CheckArgumentNull<object>(entity, "entity");
			this.Entry = entry;
			this.Entity = entity;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600027E RID: 638 RVA: 0x0000C911 File Offset: 0x0000AB11
		// (set) Token: 0x0600027F RID: 639 RVA: 0x0000C919 File Offset: 0x0000AB19
		public ODataEntry Entry { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000280 RID: 640 RVA: 0x0000C922 File Offset: 0x0000AB22
		// (set) Token: 0x06000281 RID: 641 RVA: 0x0000C92A File Offset: 0x0000AB2A
		public object Entity { get; private set; }
	}
}
