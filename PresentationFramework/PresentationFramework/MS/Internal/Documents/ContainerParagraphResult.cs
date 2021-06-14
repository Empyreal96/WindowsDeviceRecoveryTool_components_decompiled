using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006DF RID: 1759
	internal sealed class ContainerParagraphResult : ParagraphResult
	{
		// Token: 0x06007174 RID: 29044 RVA: 0x002074A7 File Offset: 0x002056A7
		internal ContainerParagraphResult(ContainerParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x002074B0 File Offset: 0x002056B0
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((ContainerParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x06007176 RID: 29046 RVA: 0x002074C5 File Offset: 0x002056C5
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((ContainerParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x06007177 RID: 29047 RVA: 0x00207504 File Offset: 0x00205704
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

		// Token: 0x04003726 RID: 14118
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
