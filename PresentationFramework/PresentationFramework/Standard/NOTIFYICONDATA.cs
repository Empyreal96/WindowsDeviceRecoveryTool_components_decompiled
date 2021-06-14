using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200004C RID: 76
	[StructLayout(LayoutKind.Sequential)]
	internal class NOTIFYICONDATA
	{
		// Token: 0x04000441 RID: 1089
		public int cbSize;

		// Token: 0x04000442 RID: 1090
		public IntPtr hWnd;

		// Token: 0x04000443 RID: 1091
		public int uID;

		// Token: 0x04000444 RID: 1092
		public NIF uFlags;

		// Token: 0x04000445 RID: 1093
		public int uCallbackMessage;

		// Token: 0x04000446 RID: 1094
		public IntPtr hIcon;

		// Token: 0x04000447 RID: 1095
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] szTip = new char[128];

		// Token: 0x04000448 RID: 1096
		public uint dwState;

		// Token: 0x04000449 RID: 1097
		public uint dwStateMask;

		// Token: 0x0400044A RID: 1098
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		public char[] szInfo = new char[256];

		// Token: 0x0400044B RID: 1099
		public uint uVersion;

		// Token: 0x0400044C RID: 1100
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		public char[] szInfoTitle = new char[64];

		// Token: 0x0400044D RID: 1101
		public uint dwInfoFlags;

		// Token: 0x0400044E RID: 1102
		public Guid guidItem;

		// Token: 0x0400044F RID: 1103
		private IntPtr hBalloonIcon;
	}
}
