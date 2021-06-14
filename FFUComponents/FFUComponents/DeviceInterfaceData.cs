using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200003A RID: 58
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct DeviceInterfaceData
	{
		// Token: 0x040000C4 RID: 196
		public int Size;

		// Token: 0x040000C5 RID: 197
		public Guid InterfaceClassGuid;

		// Token: 0x040000C6 RID: 198
		public int Flags;

		// Token: 0x040000C7 RID: 199
		public IntPtr Reserved;
	}
}
