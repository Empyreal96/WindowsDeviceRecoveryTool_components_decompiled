using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000039 RID: 57
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct WinUsbSetupPacket
	{
		// Token: 0x040000BF RID: 191
		public byte RequestType;

		// Token: 0x040000C0 RID: 192
		public byte Request;

		// Token: 0x040000C1 RID: 193
		public ushort Value;

		// Token: 0x040000C2 RID: 194
		public ushort Index;

		// Token: 0x040000C3 RID: 195
		public ushort Length;
	}
}
