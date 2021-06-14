using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000055 RID: 85
	[StructLayout(LayoutKind.Sequential)]
	internal class MONITORINFO
	{
		// Token: 0x04000480 RID: 1152
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		// Token: 0x04000481 RID: 1153
		public RECT rcMonitor;

		// Token: 0x04000482 RID: 1154
		public RECT rcWork;

		// Token: 0x04000483 RID: 1155
		public int dwFlags;
	}
}
