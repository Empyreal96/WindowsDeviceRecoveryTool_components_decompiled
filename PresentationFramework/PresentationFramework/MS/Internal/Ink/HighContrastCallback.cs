using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000688 RID: 1672
	internal abstract class HighContrastCallback
	{
		// Token: 0x06006D72 RID: 28018
		internal abstract void TurnHighContrastOn(Color highContrastColor);

		// Token: 0x06006D73 RID: 28019
		internal abstract void TurnHighContrastOff();

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x06006D74 RID: 28020
		internal abstract Dispatcher Dispatcher { get; }
	}
}
