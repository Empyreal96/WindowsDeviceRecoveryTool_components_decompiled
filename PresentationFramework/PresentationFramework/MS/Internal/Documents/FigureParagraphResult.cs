using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E4 RID: 1764
	internal sealed class FigureParagraphResult : ParagraphResult
	{
		// Token: 0x06007198 RID: 29080 RVA: 0x002074A7 File Offset: 0x002056A7
		internal FigureParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x06007199 RID: 29081 RVA: 0x0020797E File Offset: 0x00205B7E
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FigureParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x0600719A RID: 29082 RVA: 0x002079C0 File Offset: 0x00205BC0
		internal override bool HasTextContent
		{
			get
			{
				if (this._columns == null)
				{
					ReadOnlyCollection<ColumnResult> columns = this.Columns;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x0600719B RID: 29083 RVA: 0x002079E2 File Offset: 0x00205BE2
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FigureParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x0600719C RID: 29084 RVA: 0x00207A1C File Offset: 0x00205C1C
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x0600719D RID: 29085 RVA: 0x00207A94 File Offset: 0x00205C94
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FigureParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x0400372F RID: 14127
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04003730 RID: 14128
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
