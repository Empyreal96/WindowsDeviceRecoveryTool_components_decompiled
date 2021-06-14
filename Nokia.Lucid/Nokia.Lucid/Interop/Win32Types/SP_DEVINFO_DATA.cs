using System;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x0200002F RID: 47
	internal struct SP_DEVINFO_DATA
	{
		// Token: 0x040000C1 RID: 193
		public int cbSize;

		// Token: 0x040000C2 RID: 194
		public Guid ClassGuid;

		// Token: 0x040000C3 RID: 195
		public int DevInst;

		// Token: 0x040000C4 RID: 196
		public IntPtr Reserved;
	}
}
