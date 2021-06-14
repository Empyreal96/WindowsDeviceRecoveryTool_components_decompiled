using System;
using System.Runtime.InteropServices;

namespace FFUComponents
{
	// Token: 0x0200003D RID: 61
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct ClassInstallHeader
	{
		// Token: 0x040000CE RID: 206
		public int Size;

		// Token: 0x040000CF RID: 207
		public DiFuction InstallFunction;
	}
}
