using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000641 RID: 1601
	internal sealed class FloatingRun : TextHidden
	{
		// Token: 0x06006A3C RID: 27196 RVA: 0x001E3F26 File Offset: 0x001E2126
		internal FloatingRun(int length, bool figure) : base(length)
		{
			this._figure = figure;
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x06006A3D RID: 27197 RVA: 0x001E3F36 File Offset: 0x001E2136
		internal bool Figure
		{
			get
			{
				return this._figure;
			}
		}

		// Token: 0x04003431 RID: 13361
		private readonly bool _figure;
	}
}
