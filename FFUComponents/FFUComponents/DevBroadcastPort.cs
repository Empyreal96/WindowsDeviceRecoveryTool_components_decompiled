using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000041 RID: 65
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class DevBroadcastPort
	{
		// Token: 0x040000E3 RID: 227
		public int Size;

		// Token: 0x040000E4 RID: 228
		public int DeviceType;

		// Token: 0x040000E5 RID: 229
		public int Reserverd;

		// Token: 0x040000E6 RID: 230
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
		public string Name;
	}
}
