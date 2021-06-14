using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006DE RID: 1758
	internal abstract class ParagraphResult
	{
		// Token: 0x0600716B RID: 29035 RVA: 0x002073D8 File Offset: 0x002055D8
		internal ParagraphResult(BaseParaClient paraClient)
		{
			this._paraClient = paraClient;
			this._layoutBox = this._paraClient.Rect.FromTextDpi();
			this._element = paraClient.Paragraph.Element;
		}

		// Token: 0x0600716C RID: 29036 RVA: 0x0020741C File Offset: 0x0020561C
		internal ParagraphResult(BaseParaClient paraClient, Rect layoutBox, DependencyObject element) : this(paraClient)
		{
			this._layoutBox = layoutBox;
			this._element = element;
		}

		// Token: 0x0600716D RID: 29037 RVA: 0x00207433 File Offset: 0x00205633
		internal virtual bool Contains(ITextPointer position, bool strict)
		{
			this.EnsureTextContentRange();
			return this._contentRange.Contains(position, strict);
		}

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x0600716E RID: 29038 RVA: 0x00207448 File Offset: 0x00205648
		internal ITextPointer StartPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.StartPosition;
			}
		}

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600716F RID: 29039 RVA: 0x0020745B File Offset: 0x0020565B
		internal ITextPointer EndPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.EndPosition;
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x06007170 RID: 29040 RVA: 0x0020746E File Offset: 0x0020566E
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x06007171 RID: 29041 RVA: 0x00207476 File Offset: 0x00205676
		internal DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x06007172 RID: 29042 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasTextContent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007173 RID: 29043 RVA: 0x0020747E File Offset: 0x0020567E
		private void EnsureTextContentRange()
		{
			if (this._contentRange == null)
			{
				this._contentRange = this._paraClient.GetTextContentRange();
				Invariant.Assert(this._contentRange != null);
			}
		}

		// Token: 0x04003721 RID: 14113
		protected readonly BaseParaClient _paraClient;

		// Token: 0x04003722 RID: 14114
		protected readonly Rect _layoutBox;

		// Token: 0x04003723 RID: 14115
		protected readonly DependencyObject _element;

		// Token: 0x04003724 RID: 14116
		private TextContentRange _contentRange;

		// Token: 0x04003725 RID: 14117
		protected bool _hasTextContent;
	}
}
