using System;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x0200002D RID: 45
	internal struct SP_DEVICE_INTERFACE_DATA
	{
		// Token: 0x040000BB RID: 187
		public int cbSize;

		// Token: 0x040000BC RID: 188
		public Guid InterfaceClassGuid;

		// Token: 0x040000BD RID: 189
		public int Flags;

		// Token: 0x040000BE RID: 190
		private IntPtr Reserved;
	}
}
