using System;
using System.Windows.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000616 RID: 1558
	internal struct DirtyTextRange
	{
		// Token: 0x060067AC RID: 26540 RVA: 0x001D1037 File Offset: 0x001CF237
		internal DirtyTextRange(int startIndex, int positionsAdded, int positionsRemoved, bool fromHighlightLayer = false)
		{
			this.StartIndex = startIndex;
			this.PositionsAdded = positionsAdded;
			this.PositionsRemoved = positionsRemoved;
			this.FromHighlightLayer = fromHighlightLayer;
		}

		// Token: 0x060067AD RID: 26541 RVA: 0x001D1058 File Offset: 0x001CF258
		internal DirtyTextRange(TextContainerChangeEventArgs change)
		{
			this.StartIndex = change.ITextPosition.Offset;
			this.PositionsAdded = 0;
			this.PositionsRemoved = 0;
			this.FromHighlightLayer = false;
			switch (change.TextChange)
			{
			case TextChangeType.ContentAdded:
				this.PositionsAdded = change.Count;
				return;
			case TextChangeType.ContentRemoved:
				this.PositionsRemoved = change.Count;
				return;
			case TextChangeType.PropertyModified:
				this.PositionsAdded = change.Count;
				this.PositionsRemoved = change.Count;
				return;
			default:
				return;
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x060067AE RID: 26542 RVA: 0x001D10D7 File Offset: 0x001CF2D7
		// (set) Token: 0x060067AF RID: 26543 RVA: 0x001D10DF File Offset: 0x001CF2DF
		internal int StartIndex { get; set; }

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x060067B0 RID: 26544 RVA: 0x001D10E8 File Offset: 0x001CF2E8
		// (set) Token: 0x060067B1 RID: 26545 RVA: 0x001D10F0 File Offset: 0x001CF2F0
		internal int PositionsAdded { get; set; }

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x060067B2 RID: 26546 RVA: 0x001D10F9 File Offset: 0x001CF2F9
		// (set) Token: 0x060067B3 RID: 26547 RVA: 0x001D1101 File Offset: 0x001CF301
		internal int PositionsRemoved { get; set; }

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x060067B4 RID: 26548 RVA: 0x001D110A File Offset: 0x001CF30A
		// (set) Token: 0x060067B5 RID: 26549 RVA: 0x001D1112 File Offset: 0x001CF312
		internal bool FromHighlightLayer { get; set; }
	}
}
