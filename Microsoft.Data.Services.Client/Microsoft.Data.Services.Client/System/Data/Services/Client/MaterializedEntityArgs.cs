using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200004A RID: 74
	public sealed class MaterializedEntityArgs
	{
		// Token: 0x0600025B RID: 603 RVA: 0x0000C7FB File Offset: 0x0000A9FB
		public MaterializedEntityArgs(ODataEntry entry, object entity)
		{
			Util.CheckArgumentNull<ODataEntry>(entry, "entry");
			Util.CheckArgumentNull<object>(entity, "entity");
			this.Entry = entry;
			this.Entity = entity;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000C829 File Offset: 0x0000AA29
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000C831 File Offset: 0x0000AA31
		public ODataEntry Entry { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000C83A File Offset: 0x0000AA3A
		// (set) Token: 0x0600025F RID: 607 RVA: 0x0000C842 File Offset: 0x0000AA42
		public object Entity { get; private set; }
	}
}
