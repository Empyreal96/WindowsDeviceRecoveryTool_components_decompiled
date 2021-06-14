using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000047 RID: 71
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ATTACH_VIRTUAL_DISK_PARAMETERS
	{
		// Token: 0x0400011D RID: 285
		public ATTACH_VIRTUAL_DISK_VERSION Version;

		// Token: 0x0400011E RID: 286
		public int Reserved;
	}
}
