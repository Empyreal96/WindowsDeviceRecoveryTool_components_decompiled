using System;
using System.Runtime.InteropServices;

namespace ClickerUtilityLibrary.Comm.USBDriver
{
	// Token: 0x02000034 RID: 52
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct DeviceInterfaceDetailData
	{
		// Token: 0x0400014F RID: 335
		public int Size;

		// Token: 0x04000150 RID: 336
		public char DevicePath;
	}
}
