using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006B9 RID: 1721
	internal sealed class ColumnResult
	{
		// Token: 0x06006ED0 RID: 28368 RVA: 0x001FDCF4 File Offset: 0x001FBEF4
		[SecurityCritical]
		internal ColumnResult(FlowDocumentPage page, ref PTS.FSTRACKDESCRIPTION trackDesc, Vector contentOffset)
		{
			this._page = page;
			this._columnHandle = trackDesc.pfstrack;
			this._layoutBox = new Rect(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v), TextDpi.FromTextDpi(trackDesc.fsrc.du), TextDpi.FromTextDpi(trackDesc.fsrc.dv));
			this._layoutBox.X = this._layoutBox.X + contentOffset.X;
			this._layoutBox.Y = this._layoutBox.Y + contentOffset.Y;
			this._columnOffset = new Vector(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v));
			this._hasTextContent = false;
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x001FDDCC File Offset: 0x001FBFCC
		[SecurityCritical]
		internal ColumnResult(BaseParaClient subpage, ref PTS.FSTRACKDESCRIPTION trackDesc, Vector contentOffset)
		{
			Invariant.Assert(subpage is SubpageParaClient || subpage is FigureParaClient || subpage is FloaterParaClient);
			this._subpage = subpage;
			this._columnHandle = trackDesc.pfstrack;
			this._layoutBox = new Rect(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v), TextDpi.FromTextDpi(trackDesc.fsrc.du), TextDpi.FromTextDpi(trackDesc.fsrc.dv));
			this._layoutBox.X = this._layoutBox.X + contentOffset.X;
			this._layoutBox.Y = this._layoutBox.Y + contentOffset.Y;
			this._columnOffset = new Vector(TextDpi.FromTextDpi(trackDesc.fsrc.u), TextDpi.FromTextDpi(trackDesc.fsrc.v));
		}

		// Token: 0x06006ED2 RID: 28370 RVA: 0x001FDEBB File Offset: 0x001FC0BB
		internal bool Contains(ITextPointer position, bool strict)
		{
			this.EnsureTextContentRange();
			return this._contentRange.Contains(position, strict);
		}

		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x001FDED0 File Offset: 0x001FC0D0
		internal ITextPointer StartPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.StartPosition;
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x001FDEE3 File Offset: 0x001FC0E3
		internal ITextPointer EndPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.EndPosition;
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x001FDEF6 File Offset: 0x001FC0F6
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x17001A4C RID: 6732
		// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x001FDF00 File Offset: 0x001FC100
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (this._paragraphs == null)
				{
					this._hasTextContent = false;
					if (this._page != null)
					{
						this._paragraphs = this._page.GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is FigureParaClient)
					{
						this._paragraphs = ((FigureParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is FloaterParaClient)
					{
						this._paragraphs = ((FloaterParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else if (this._subpage is SubpageParaClient)
					{
						this._paragraphs = ((SubpageParaClient)this._subpage).GetParagraphResultsFromColumn(this._columnHandle, this._columnOffset, out this._hasTextContent);
					}
					else
					{
						Invariant.Assert(false, "Expecting Subpage, Figure or Floater ParaClient");
					}
				}
				return this._paragraphs;
			}
		}

		// Token: 0x17001A4D RID: 6733
		// (get) Token: 0x06006ED7 RID: 28375 RVA: 0x001FE008 File Offset: 0x001FC208
		internal bool HasTextContent
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

		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x06006ED8 RID: 28376 RVA: 0x001FE02A File Offset: 0x001FC22A
		internal TextContentRange TextContentRange
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange;
			}
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x001FE038 File Offset: 0x001FC238
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EnsureTextContentRange()
		{
			if (this._contentRange == null)
			{
				if (this._page != null)
				{
					this._contentRange = this._page.GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is FigureParaClient)
				{
					this._contentRange = ((FigureParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is FloaterParaClient)
				{
					this._contentRange = ((FloaterParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else if (this._subpage is SubpageParaClient)
				{
					this._contentRange = ((SubpageParaClient)this._subpage).GetTextContentRangeFromColumn(this._columnHandle);
				}
				else
				{
					Invariant.Assert(false, "Expecting Subpage, Figure or Floater ParaClient");
				}
				Invariant.Assert(this._contentRange != null);
			}
		}

		// Token: 0x04003687 RID: 13959
		private readonly FlowDocumentPage _page;

		// Token: 0x04003688 RID: 13960
		private readonly BaseParaClient _subpage;

		// Token: 0x04003689 RID: 13961
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private readonly IntPtr _columnHandle;

		// Token: 0x0400368A RID: 13962
		private readonly Rect _layoutBox;

		// Token: 0x0400368B RID: 13963
		private readonly Vector _columnOffset;

		// Token: 0x0400368C RID: 13964
		private TextContentRange _contentRange;

		// Token: 0x0400368D RID: 13965
		private ReadOnlyCollection<ParagraphResult> _paragraphs;

		// Token: 0x0400368E RID: 13966
		private bool _hasTextContent;
	}
}
