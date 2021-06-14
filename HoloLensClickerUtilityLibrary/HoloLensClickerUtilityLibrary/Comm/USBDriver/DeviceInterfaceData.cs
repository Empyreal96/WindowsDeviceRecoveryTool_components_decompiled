using System;
using System.Runtime.InteropServices;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000032 RID: 50
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct DeviceInterfaceData
	{
		// Token: 0x04000147 RID: 327
		public int Size;

		// Token: 0x04000148 RID: 328
		public Guid InterfaceClassGuid;

		// Token: 0x04000149 RID: 329
		public int Flags;

		// Token: 0x0400014A RID: 330
		public IntPtr Reserved;
	}
}
