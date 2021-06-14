using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200005E RID: 94
	[StructLayout(LayoutKind.Sequential)]
	internal class WINDOWPLACEMENT
	{
		// Token: 0x040004A2 RID: 1186
		public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

		// Token: 0x040004A3 RID: 1187
		public int flags;

		// Token: 0x040004A4 RID: 1188
		public SW showCmd;

		// Token: 0x040004A5 RID: 1189
		public POINT ptMinPosition;

		// Token: 0x040004A6 RID: 1190
		public POINT ptMaxPosition;

		// Token: 0x040004A7 RID: 1191
		public RECT rcNormalPosition;
	}
}
