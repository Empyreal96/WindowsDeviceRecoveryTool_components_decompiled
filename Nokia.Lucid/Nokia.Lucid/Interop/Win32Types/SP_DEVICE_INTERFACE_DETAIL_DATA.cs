using System;
using System.Runtime.InteropServices;

namespace Nokia.Lucid.Interop.Win32Types
{
	// Token: 0x0200002E RID: 46
	[BestFitMapping(false, ThrowOnUnmappableChar = true)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
	internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		// Token: 0x040000BF RID: 191
		public int cbSize;

		// Token: 0x040000C0 RID: 192
		private char DevicePath;
	}
}
