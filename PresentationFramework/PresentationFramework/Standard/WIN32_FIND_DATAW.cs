using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Standard
{
	// Token: 0x0200005D RID: 93
	[BestFitMapping(false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class WIN32_FIND_DATAW
	{
		// Token: 0x04000498 RID: 1176
		public FileAttributes dwFileAttributes;

		// Token: 0x04000499 RID: 1177
		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		// Token: 0x0400049A RID: 1178
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		// Token: 0x0400049B RID: 1179
		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		// Token: 0x0400049C RID: 1180
		public int nFileSizeHigh;

		// Token: 0x0400049D RID: 1181
		public int nFileSizeLow;

		// Token: 0x0400049E RID: 1182
		public int dwReserved0;

		// Token: 0x0400049F RID: 1183
		public int dwReserved1;

		// Token: 0x040004A0 RID: 1184
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string cFileName;

		// Token: 0x040004A1 RID: 1185
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}
}
