using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000048 RID: 72
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CREATESTRUCT
	{
		// Token: 0x04000417 RID: 1047
		public IntPtr lpCreateParams;

		// Token: 0x04000418 RID: 1048
		public IntPtr hInstance;

		// Token: 0x04000419 RID: 1049
		public IntPtr hMenu;

		// Token: 0x0400041A RID: 1050
		public IntPtr hwndParent;

		// Token: 0x0400041B RID: 1051
		public int cy;

		// Token: 0x0400041C RID: 1052
		public int cx;

		// Token: 0x0400041D RID: 1053
		public int y;

		// Token: 0x0400041E RID: 1054
		public int x;

		// Token: 0x0400041F RID: 1055
		public WS style;

		// Token: 0x04000420 RID: 1056
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszName;

		// Token: 0x04000421 RID: 1057
		[MarshalAs(UnmanagedType.LPWStr)]
		public string lpszClass;

		// Token: 0x04000422 RID: 1058
		public WS_EX dwExStyle;
	}
}
