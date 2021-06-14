using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000046 RID: 70
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal class SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00004376 File Offset: 0x00002576
		public SP_DEVICE_INTERFACE_DETAIL_DATA()
		{
			if (IntPtr.Size == 4)
			{
				this.cbSize = 6U;
				return;
			}
			this.cbSize = 8U;
		}

		// Token: 0x040000F3 RID: 243
		public uint cbSize;

		// Token: 0x040000F4 RID: 244
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string DevicePath;
	}
}
