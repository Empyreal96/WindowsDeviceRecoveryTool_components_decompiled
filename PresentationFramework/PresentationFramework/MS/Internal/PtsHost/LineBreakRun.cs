using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000643 RID: 1603
	internal sealed class LineBreakRun : TextEndOfLine
	{
		// Token: 0x06006A3F RID: 27199 RVA: 0x001E3F4E File Offset: 0x001E214E
		internal LineBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x04003433 RID: 13363
		internal readonly PTS.FSFLRES BreakReason;
	}
}
