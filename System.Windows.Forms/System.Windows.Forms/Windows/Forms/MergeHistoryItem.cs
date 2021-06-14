using System;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020003D4 RID: 980
	internal class MergeHistoryItem
	{
		// Token: 0x060040ED RID: 16621 RVA: 0x0011826F File Offset: 0x0011646F
		public MergeHistoryItem(MergeAction mergeAction)
		{
			this.mergeAction = mergeAction;
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x060040EE RID: 16622 RVA: 0x0011828C File Offset: 0x0011648C
		public MergeAction MergeAction
		{
			get
			{
				return this.mergeAction;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x060040EF RID: 16623 RVA: 0x00118294 File Offset: 0x00116494
		// (set) Token: 0x060040F0 RID: 16624 RVA: 0x0011829C File Offset: 0x0011649C
		public ToolStripItem TargetItem
		{
			get
			{
				return this.targetItem;
			}
			set
			{
				this.targetItem = value;
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x060040F1 RID: 16625 RVA: 0x001182A5 File Offset: 0x001164A5
		// (set) Token: 0x060040F2 RID: 16626 RVA: 0x001182AD File Offset: 0x001164AD
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x060040F3 RID: 16627 RVA: 0x001182B6 File Offset: 0x001164B6
		// (set) Token: 0x060040F4 RID: 16628 RVA: 0x001182BE File Offset: 0x001164BE
		public int PreviousIndex
		{
			get
			{
				return this.previousIndex;
			}
			set
			{
				this.previousIndex = value;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x060040F5 RID: 16629 RVA: 0x001182C7 File Offset: 0x001164C7
		// (set) Token: 0x060040F6 RID: 16630 RVA: 0x001182CF File Offset: 0x001164CF
		public ToolStripItemCollection PreviousIndexCollection
		{
			get
			{
				return this.previousIndexCollection;
			}
			set
			{
				this.previousIndexCollection = value;
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x060040F7 RID: 16631 RVA: 0x001182D8 File Offset: 0x001164D8
		// (set) Token: 0x060040F8 RID: 16632 RVA: 0x001182E0 File Offset: 0x001164E0
		public ToolStripItemCollection IndexCollection
		{
			get
			{
				return this.indexCollection;
			}
			set
			{
				this.indexCollection = value;
			}
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x001182EC File Offset: 0x001164EC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MergeAction: ",
				this.mergeAction.ToString(),
				" | TargetItem: ",
				(this.TargetItem == null) ? "null" : this.TargetItem.Text,
				" Index: ",
				this.index.ToString(CultureInfo.CurrentCulture)
			});
		}

		// Token: 0x040024EB RID: 9451
		private MergeAction mergeAction;

		// Token: 0x040024EC RID: 9452
		private ToolStripItem targetItem;

		// Token: 0x040024ED RID: 9453
		private int index = -1;

		// Token: 0x040024EE RID: 9454
		private int previousIndex = -1;

		// Token: 0x040024EF RID: 9455
		private ToolStripItemCollection previousIndexCollection;

		// Token: 0x040024F0 RID: 9456
		private ToolStripItemCollection indexCollection;
	}
}
