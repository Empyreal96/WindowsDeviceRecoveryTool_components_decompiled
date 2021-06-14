using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x020000ED RID: 237
	internal static class CoreSwitches
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0002BC34 File Offset: 0x00029E34
		public static BooleanSwitch PerfTrack
		{
			get
			{
				if (CoreSwitches.perfTrack == null)
				{
					CoreSwitches.perfTrack = new BooleanSwitch("PERFTRACK", "Debug performance critical sections.");
				}
				return CoreSwitches.perfTrack;
			}
		}

		// Token: 0x04000AD2 RID: 2770
		private static BooleanSwitch perfTrack;
	}
}
