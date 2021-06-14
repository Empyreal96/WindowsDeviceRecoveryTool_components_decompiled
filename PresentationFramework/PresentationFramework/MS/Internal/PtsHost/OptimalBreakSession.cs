using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000631 RID: 1585
	internal sealed class OptimalBreakSession : UnmanagedHandle
	{
		// Token: 0x060068E0 RID: 26848 RVA: 0x001D9799 File Offset: 0x001D7999
		internal OptimalBreakSession(TextParagraph textParagraph, TextParaClient textParaClient, TextParagraphCache TextParagraphCache, OptimalTextSource optimalTextSource) : base(textParagraph.PtsContext)
		{
			this._textParagraph = textParagraph;
			this._textParaClient = textParaClient;
			this._textParagraphCache = TextParagraphCache;
			this._optimalTextSource = optimalTextSource;
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x001D97C4 File Offset: 0x001D79C4
		public override void Dispose()
		{
			try
			{
				if (this._textParagraphCache != null)
				{
					this._textParagraphCache.Dispose();
				}
				if (this._optimalTextSource != null)
				{
					this._optimalTextSource.Dispose();
				}
			}
			finally
			{
				this._textParagraphCache = null;
				this._optimalTextSource = null;
			}
			base.Dispose();
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x060068E2 RID: 26850 RVA: 0x001D9820 File Offset: 0x001D7A20
		internal TextParagraphCache TextParagraphCache
		{
			get
			{
				return this._textParagraphCache;
			}
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x001D9828 File Offset: 0x001D7A28
		internal TextParagraph TextParagraph
		{
			get
			{
				return this._textParagraph;
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x060068E4 RID: 26852 RVA: 0x001D9830 File Offset: 0x001D7A30
		internal TextParaClient TextParaClient
		{
			get
			{
				return this._textParaClient;
			}
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x060068E5 RID: 26853 RVA: 0x001D9838 File Offset: 0x001D7A38
		internal OptimalTextSource OptimalTextSource
		{
			get
			{
				return this._optimalTextSource;
			}
		}

		// Token: 0x040033F4 RID: 13300
		private TextParagraphCache _textParagraphCache;

		// Token: 0x040033F5 RID: 13301
		private TextParagraph _textParagraph;

		// Token: 0x040033F6 RID: 13302
		private TextParaClient _textParaClient;

		// Token: 0x040033F7 RID: 13303
		private OptimalTextSource _optimalTextSource;
	}
}
