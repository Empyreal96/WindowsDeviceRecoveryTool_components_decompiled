using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x02000044 RID: 68
	[StructLayout(LayoutKind.Sequential)]
	public class SP_DEVICE_INTERFACE_DATA
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00004330 File Offset: 0x00002530
		public SP_DEVICE_INTERFACE_DATA()
		{
			this.cbSize = (uint)Marshal.SizeOf<SP_DEVICE_INTERFACE_DATA>(this);
		}

		// Token: 0x040000ED RID: 237
		public uint cbSize;

		// Token: 0x040000EE RID: 238
		public Guid InterfaceClassGuid;

		// Token: 0x040000EF RID: 239
		public uint Flags;

		// Token: 0x040000F0 RID: 240
		private IntPtr Reserved;
	}
}
