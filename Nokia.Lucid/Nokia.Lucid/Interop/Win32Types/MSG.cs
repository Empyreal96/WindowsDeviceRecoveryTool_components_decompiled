using System;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x0200002A RID: 42
	internal struct MSG
	{
		// Token: 0x040000A7 RID: 167
		public IntPtr hwnd;

		// Token: 0x040000A8 RID: 168
		public int message;

		// Token: 0x040000A9 RID: 169
		public IntPtr wParam;

		// Token: 0x040000AA RID: 170
		public IntPtr lParam;

		// Token: 0x040000AB RID: 171
		public int time;

		// Token: 0x040000AC RID: 172
		public POINT pt;
	}
}
