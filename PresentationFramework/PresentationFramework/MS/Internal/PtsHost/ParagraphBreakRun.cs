using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000642 RID: 1602
	internal sealed class ParagraphBreakRun : TextEndOfParagraph
	{
		// Token: 0x06006A3E RID: 27198 RVA: 0x001E3F3E File Offset: 0x001E213E
		internal ParagraphBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x04003432 RID: 13362
		internal readonly PTS.FSFLRES BreakReason;
	}
}
