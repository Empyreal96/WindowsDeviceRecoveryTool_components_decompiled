using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace FFUComponents
{
	// Token: 0x0200003F RID: 63
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct FindFileData
	{
		// Token: 0x040000D4 RID: 212
		public uint dwFileAttributes;

		// Token: 0x040000D5 RID: 213
		public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;

		// Token: 0x040000D6 RID: 214
		public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

		// Token: 0x040000D7 RID: 215
		public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;

		// Token: 0x040000D8 RID: 216
		public uint FileSizeHigh;

		// Token: 0x040000D9 RID: 217
		public uint FileSizeLow;

		// Token: 0x040000DA RID: 218
		public uint Reserved0;

		// Token: 0x040000DB RID: 219
		public uint Reserved1;

		// Token: 0x040000DC RID: 220
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string FileName;

		// Token: 0x040000DD RID: 221
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string Alternate;
	}
}
