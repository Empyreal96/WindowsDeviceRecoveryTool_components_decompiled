using System;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000629 RID: 1577
	internal sealed class LineVisual : DrawingVisual
	{
		// Token: 0x0600689E RID: 26782 RVA: 0x001D844E File Offset: 0x001D664E
		internal DrawingContext Open()
		{
			return base.RenderOpen();
		}

		// Token: 0x040033E3 RID: 13283
		internal double WidthIncludingTrailingWhitespace;
	}
}
