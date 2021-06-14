using System;

namespace System.Windows.Forms
{
	// Token: 0x020003BB RID: 955
	internal class ToolStripItemImageIndexer : ImageList.Indexer
	{
		// Token: 0x06004018 RID: 16408 RVA: 0x0011575F File Offset: 0x0011395F
		public ToolStripItemImageIndexer(ToolStripItem item)
		{
			this.item = item;
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06004019 RID: 16409 RVA: 0x0011576E File Offset: 0x0011396E
		// (set) Token: 0x0600401A RID: 16410 RVA: 0x0000701A File Offset: 0x0000521A
		public override ImageList ImageList
		{
			get
			{
				if (this.item != null && this.item.Owner != null)
				{
					return this.item.Owner.ImageList;
				}
				return null;
			}
			set
			{
			}
		}

		// Token: 0x04002488 RID: 9352
		private ToolStripItem item;
	}
}
