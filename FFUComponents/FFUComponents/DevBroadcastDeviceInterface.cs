using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000040 RID: 64
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class DevBroadcastDeviceInterface
	{
		// Token: 0x040000DE RID: 222
		public int Size;

		// Token: 0x040000DF RID: 223
		public int DeviceType;

		// Token: 0x040000E0 RID: 224
		public int Reserverd;

		// Token: 0x040000E1 RID: 225
		public Guid ClassGuid;

		// Token: 0x040000E2 RID: 226
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
		public string Name;
	}
}
