using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E1 RID: 1761
	internal sealed class TableParagraphResult : ParagraphResult
	{
		// Token: 0x06007185 RID: 29061 RVA: 0x002074A7 File Offset: 0x002056A7
		internal TableParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007186 RID: 29062 RVA: 0x002076DB File Offset: 0x002058DB
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPoint(Point point, bool snapToText)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPoint(point, snapToText);
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x002076EF File Offset: 0x002058EF
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPosition(position);
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x00207702 File Offset: 0x00205902
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((TableParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x00207717 File Offset: 0x00205917
		internal CellParaClient GetCellParaClientFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetCellParaClientFromPosition(position);
		}

		// Token: 0x0600718A RID: 29066 RVA: 0x0020772A File Offset: 0x0020592A
		internal CellParaClient GetCellAbove(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellAbove(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x0020773F File Offset: 0x0020593F
		internal CellParaClient GetCellBelow(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellBelow(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x0600718C RID: 29068 RVA: 0x00207754 File Offset: 0x00205954
		internal CellInfo GetCellInfoFromPoint(Point point)
		{
			return ((TableParaClient)this._paraClient).GetCellInfoFromPoint(point);
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x00207767 File Offset: 0x00205967
		internal Rect GetRectangleFromRowEndPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetRectangleFromRowEndPosition(position);
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x0600718E RID: 29070 RVA: 0x0020777A File Offset: 0x0020597A
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((TableParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x0600718F RID: 29071 RVA: 0x002077BC File Offset: 0x002059BC
		internal override bool HasTextContent
		{
			get
			{
				if (this._paragraphs == null)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs = this.Paragraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x0400372A RID: 14122
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
