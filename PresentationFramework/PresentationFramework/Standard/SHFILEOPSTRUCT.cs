using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000049 RID: 73
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct SHFILEOPSTRUCT
	{
		// Token: 0x04000423 RID: 1059
		public IntPtr hwnd;

		// Token: 0x04000424 RID: 1060
		[MarshalAs(UnmanagedType.U4)]
		public FO wFunc;

		// Token: 0x04000425 RID: 1061
		public string pFrom;

		// Token: 0x04000426 RID: 1062
		public string pTo;

		// Token: 0x04000427 RID: 1063
		[MarshalAs(UnmanagedType.U2)]
		public FOF fFlags;

		// Token: 0x04000428 RID: 1064
		[MarshalAs(UnmanagedType.Bool)]
		public int fAnyOperationsAborted;

		// Token: 0x04000429 RID: 1065
		public IntPtr hNameMappings;

		// Token: 0x0400042A RID: 1066
		public string lpszProgressTitle;
	}
}
