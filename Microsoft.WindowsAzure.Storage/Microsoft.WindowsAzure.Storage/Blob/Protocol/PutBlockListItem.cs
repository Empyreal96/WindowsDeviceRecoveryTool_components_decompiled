using System;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000D4 RID: 212
	public sealed class PutBlockListItem
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x00041949 File Offset: 0x0003FB49
		public PutBlockListItem(string id, BlockSearchMode searchMode)
		{
			this.Id = id;
			this.SearchMode = searchMode;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0004195F File Offset: 0x0003FB5F
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x00041967 File Offset: 0x0003FB67
		public string Id { get; private set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00041970 File Offset: 0x0003FB70
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x00041978 File Offset: 0x0003FB78
		public BlockSearchMode SearchMode { get; private set; }
	}
}
