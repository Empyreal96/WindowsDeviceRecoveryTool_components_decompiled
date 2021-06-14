using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000628 RID: 1576
	internal sealed class LineBreakRecord : UnmanagedHandle
	{
		// Token: 0x0600689A RID: 26778 RVA: 0x001D8403 File Offset: 0x001D6603
		internal LineBreakRecord(PtsContext ptsContext, TextLineBreak textLineBreak) : base(ptsContext)
		{
			this._textLineBreak = textLineBreak;
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x001D8413 File Offset: 0x001D6613
		public override void Dispose()
		{
			if (this._textLineBreak != null)
			{
				this._textLineBreak.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x001D842E File Offset: 0x001D662E
		internal LineBreakRecord Clone()
		{
			return new LineBreakRecord(base.PtsContext, this._textLineBreak.Clone());
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x0600689D RID: 26781 RVA: 0x001D8446 File Offset: 0x001D6646
		internal TextLineBreak TextLineBreak
		{
			get
			{
				return this._textLineBreak;
			}
		}

		// Token: 0x040033E2 RID: 13282
		private TextLineBreak _textLineBreak;
	}
}
