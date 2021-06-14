using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E2 RID: 1762
	internal sealed class RowParagraphResult : ParagraphResult
	{
		// Token: 0x06007190 RID: 29072 RVA: 0x002077DE File Offset: 0x002059DE
		internal RowParagraphResult(BaseParaClient paraClient, int index, Rect rowRect, RowParagraph rowParagraph) : base(paraClient, rowRect, rowParagraph.Element)
		{
			this._index = index;
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x06007191 RID: 29073 RVA: 0x002077F8 File Offset: 0x002059F8
		internal ReadOnlyCollection<ParagraphResult> CellParagraphs
		{
			get
			{
				if (this._cells == null)
				{
					this._cells = ((TableParaClient)this._paraClient).GetChildrenParagraphResultsForRow(this._index, out this._hasTextContent);
				}
				Invariant.Assert(this._cells != null, "Paragraph collection is empty");
				return this._cells;
			}
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x06007192 RID: 29074 RVA: 0x00207848 File Offset: 0x00205A48
		internal override bool HasTextContent
		{
			get
			{
				if (this._cells == null)
				{
					ReadOnlyCollection<ParagraphResult> cellParagraphs = this.CellParagraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x0400372B RID: 14123
		private ReadOnlyCollection<ParagraphResult> _cells;

		// Token: 0x0400372C RID: 14124
		private int _index;
	}
}
