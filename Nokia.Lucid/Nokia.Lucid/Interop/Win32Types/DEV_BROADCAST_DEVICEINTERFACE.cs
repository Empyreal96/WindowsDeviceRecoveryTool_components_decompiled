using System;
using System.Runtime.InteropServices;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x02000029 RID: 41
	[BestFitMapping(false, ThrowOnUnmappableChar = true)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct DEV_BROADCAST_DEVICEINTERFACE
	{
		// Token: 0x040000A1 RID: 161
		private const int MAX_PATH = 260;

		// Token: 0x040000A2 RID: 162
		public int dbcc_size;

		// Token: 0x040000A3 RID: 163
		public int dbcc_devicetype;

		// Token: 0x040000A4 RID: 164
		public int dbcc_reserved;

		// Token: 0x040000A5 RID: 165
		public Guid dbcc_classguid;

		// Token: 0x040000A6 RID: 166
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 261)]
		public string dbcc_name;
	}
}
