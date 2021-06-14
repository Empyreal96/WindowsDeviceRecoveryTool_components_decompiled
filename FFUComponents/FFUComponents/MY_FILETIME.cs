using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000045 RID: 69
	[StructLayout(LayoutKind.Sequential)]
	public class MY_FILETIME
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x00004344 File Offset: 0x00002544
		public DateTime ToLocalDateTime()
		{
			ulong num = (ulong)this.dwHighDateTime << 32;
			ulong num2 = (ulong)this.dwLowDateTime;
			ulong fileTime = num | num2;
			return DateTime.FromFileTime((long)fileTime);
		}

		// Token: 0x040000F1 RID: 241
		public uint dwLowDateTime;

		// Token: 0x040000F2 RID: 242
		public uint dwHighDateTime;
	}
}
