using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x020000F7 RID: 247
	internal static class CoreSwitches
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000C283 File Offset: 0x0000A483
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

		// Token: 0x04000421 RID: 1057
		private static BooleanSwitch perfTrack;
	}
}
