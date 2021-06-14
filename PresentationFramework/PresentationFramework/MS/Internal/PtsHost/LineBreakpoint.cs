using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000632 RID: 1586
	internal sealed class LineBreakpoint : UnmanagedHandle
	{
		// Token: 0x060068E6 RID: 26854 RVA: 0x001D9840 File Offset: 0x001D7A40
		internal LineBreakpoint(OptimalBreakSession optimalBreakSession, TextBreakpoint textBreakpoint) : base(optimalBreakSession.PtsContext)
		{
			this._textBreakpoint = textBreakpoint;
			this._optimalBreakSession = optimalBreakSession;
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x001D985C File Offset: 0x001D7A5C
		public override void Dispose()
		{
			if (this._textBreakpoint != null)
			{
				this._textBreakpoint.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x060068E8 RID: 26856 RVA: 0x001D9877 File Offset: 0x001D7A77
		internal OptimalBreakSession OptimalBreakSession
		{
			get
			{
				return this._optimalBreakSession;
			}
		}

		// Token: 0x040033F8 RID: 13304
		private TextBreakpoint _textBreakpoint;

		// Token: 0x040033F9 RID: 13305
		private OptimalBreakSession _optimalBreakSession;
	}
}
