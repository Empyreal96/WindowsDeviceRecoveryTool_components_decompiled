using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000060 RID: 96
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct WNDCLASSEX
	{
		// Token: 0x040004AF RID: 1199
		public int cbSize;

		// Token: 0x040004B0 RID: 1200
		public CS style;

		// Token: 0x040004B1 RID: 1201
		public WndProc lpfnWndProc;

		// Token: 0x040004B2 RID: 1202
		public int cbClsExtra;

		// Token: 0x040004B3 RID: 1203
		public int cbWndExtra;

		// Token: 0x040004B4 RID: 1204
		public IntPtr hInstance;

		// Token: 0x040004B5 RID: 1205
		public IntPtr hIcon;

		// Token: 0x040004B6 RID: 1206
		public IntPtr hCursor;

		// Token: 0x040004B7 RID: 1207
		public IntPtr hbrBackground;

		// Token: 0x040004B8 RID: 1208
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszMenuName;

		// Token: 0x040004B9 RID: 1209
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszClassName;

		// Token: 0x040004BA RID: 1210
		public IntPtr hIconSm;
	}
}
