using System;
using System.Collections.Generic;

namespace System.Windows.Forms
{
	// Token: 0x020003D3 RID: 979
	internal class MergeHistory
	{
		// Token: 0x060040EA RID: 16618 RVA: 0x0011823D File Offset: 0x0011643D
		public MergeHistory(ToolStrip mergedToolStrip)
		{
			this.mergedToolStrip = mergedToolStrip;
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x060040EB RID: 16619 RVA: 0x0011824C File Offset: 0x0011644C
		public Stack<MergeHistoryItem> MergeHistoryItemsStack
		{
			get
			{
				if (this.mergeHistoryItemsStack == null)
				{
					this.mergeHistoryItemsStack = new Stack<MergeHistoryItem>();
				}
				return this.mergeHistoryItemsStack;
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x060040EC RID: 16620 RVA: 0x00118267 File Offset: 0x00116467
		public ToolStrip MergedToolStrip
		{
			get
			{
				return this.mergedToolStrip;
			}
		}

		// Token: 0x040024E9 RID: 9449
		private Stack<MergeHistoryItem> mergeHistoryItemsStack;

		// Token: 0x040024EA RID: 9450
		private ToolStrip mergedToolStrip;
	}
}
