using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E6 RID: 1766
	internal sealed class FloaterParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x0600719F RID: 29087 RVA: 0x00207AED File Offset: 0x00205CED
		internal FloaterParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x060071A0 RID: 29088 RVA: 0x00207AF6 File Offset: 0x00205CF6
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FloaterParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x060071A1 RID: 29089 RVA: 0x00207B38 File Offset: 0x00205D38
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

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x060071A2 RID: 29090 RVA: 0x00207B5A File Offset: 0x00205D5A
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FloaterParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x060071A3 RID: 29091 RVA: 0x00207B94 File Offset: 0x00205D94
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x060071A4 RID: 29092 RVA: 0x00207C0C File Offset: 0x00205E0C
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FloaterParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x04003731 RID: 14129
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04003732 RID: 14130
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
