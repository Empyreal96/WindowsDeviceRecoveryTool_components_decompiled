using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E3 RID: 1763
	internal sealed class SubpageParagraphResult : ParagraphResult
	{
		// Token: 0x06007193 RID: 29075 RVA: 0x002074A7 File Offset: 0x002056A7
		internal SubpageParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x06007194 RID: 29076 RVA: 0x0020786A File Offset: 0x00205A6A
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((SubpageParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06007195 RID: 29077 RVA: 0x002078AC File Offset: 0x00205AAC
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

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x06007196 RID: 29078 RVA: 0x002078CE File Offset: 0x00205ACE
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((SubpageParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x06007197 RID: 29079 RVA: 0x00207908 File Offset: 0x00205B08
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x0400372D RID: 14125
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x0400372E RID: 14126
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
