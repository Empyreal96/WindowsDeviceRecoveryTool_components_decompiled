using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x0200033B RID: 827
	internal class DocumentSequenceHighlightLayer : HighlightLayer
	{
		// Token: 0x06002B9D RID: 11165 RVA: 0x000C7348 File Offset: 0x000C5548
		internal DocumentSequenceHighlightLayer(DocumentSequenceTextContainer docSeqContainer)
		{
			this._docSeqContainer = docSeqContainer;
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override object GetHighlightValue(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000C7357 File Offset: 0x000C5557
		internal override bool IsContentHighlighted(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return this._docSeqContainer.Highlights.IsContentHighlighted(staticTextPointer, direction);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000C736B File Offset: 0x000C556B
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer staticTextPointer, LogicalDirection direction)
		{
			return this._docSeqContainer.Highlights.GetNextHighlightChangePosition(staticTextPointer, direction);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000C7380 File Offset: 0x000C5580
		internal void RaiseHighlightChangedEvent(IList ranges)
		{
			if (this.Changed != null)
			{
				DocumentSequenceHighlightLayer.DocumentSequenceHighlightChangedEventArgs args = new DocumentSequenceHighlightLayer.DocumentSequenceHighlightChangedEventArgs(ranges);
				this.Changed(this, args);
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002BA2 RID: 11170 RVA: 0x000C73A9 File Offset: 0x000C55A9
		internal override Type OwnerType
		{
			get
			{
				return typeof(TextSelection);
			}
		}

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002BA3 RID: 11171 RVA: 0x000C73B8 File Offset: 0x000C55B8
		// (remove) Token: 0x06002BA4 RID: 11172 RVA: 0x000C73F0 File Offset: 0x000C55F0
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x04001CB5 RID: 7349
		private readonly DocumentSequenceTextContainer _docSeqContainer;

		// Token: 0x020008CB RID: 2251
		private class DocumentSequenceHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008478 RID: 33912 RVA: 0x002484BD File Offset: 0x002466BD
			internal DocumentSequenceHighlightChangedEventArgs(IList ranges)
			{
				this._ranges = ranges;
			}

			// Token: 0x17001E01 RID: 7681
			// (get) Token: 0x06008479 RID: 33913 RVA: 0x002484CC File Offset: 0x002466CC
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001E02 RID: 7682
			// (get) Token: 0x0600847A RID: 33914 RVA: 0x000C73A9 File Offset: 0x000C55A9
			internal override Type OwnerType
			{
				get
				{
					return typeof(TextSelection);
				}
			}

			// Token: 0x04004233 RID: 16947
			private readonly IList _ranges;
		}
	}
}
