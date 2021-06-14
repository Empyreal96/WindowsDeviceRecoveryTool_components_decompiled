using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000042 RID: 66
	[StructLayout(LayoutKind.Sequential)]
	public class SP_DEVINFO_DATA
	{
		// Token: 0x060000C1 RID: 193 RVA: 0x00004306 File Offset: 0x00002506
		public SP_DEVINFO_DATA()
		{
			this.cbSize = (uint)Marshal.SizeOf<SP_DEVINFO_DATA>(this);
		}

		// Token: 0x040000E7 RID: 231
		public uint cbSize;

		// Token: 0x040000E8 RID: 232
		public Guid ClassGuid;

		// Token: 0x040000E9 RID: 233
		public uint DevInst;

		// Token: 0x040000EA RID: 234
		public IntPtr Reserved;
	}
}
