using System;
using System.Runtime.InteropServices;

namespace Interop.SirepClient
{
	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SirepEndpointInfo
	{
		// Token: 0x0400001C RID: 28
		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszIPAddress;

		// Token: 0x0400001D RID: 29
		public ushort usSirepPort;

		// Token: 0x0400001E RID: 30
		public ushort usEchoPort;

		// Token: 0x0400001F RID: 31
		public ushort usProtocol2Port;
	}
}
