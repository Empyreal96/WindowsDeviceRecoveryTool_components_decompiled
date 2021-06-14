using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000047 RID: 71
	[StructLayout(LayoutKind.Sequential)]
	internal class USB_NODE_CONNECTION_INFORMATION_EX_V2
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004395 File Offset: 0x00002595
		public USB_NODE_CONNECTION_INFORMATION_EX_V2(uint portNumber)
		{
			this.ConnectionIndex = portNumber;
			this.Length = (uint)Marshal.SizeOf<USB_NODE_CONNECTION_INFORMATION_EX_V2>(this);
			this.SupportedUsbProtocols = 7U;
			this.Flags = 0U;
		}

		// Token: 0x040000F5 RID: 245
		public uint ConnectionIndex;

		// Token: 0x040000F6 RID: 246
		public uint Length;

		// Token: 0x040000F7 RID: 247
		public uint SupportedUsbProtocols;

		// Token: 0x040000F8 RID: 248
		public uint Flags;
	}
}
