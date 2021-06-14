using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200003C RID: 60
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct DeviceInterfaceDetailData
	{
		// Token: 0x040000CC RID: 204
		public int Size;

		// Token: 0x040000CD RID: 205
		public char DevicePath;
	}
}
