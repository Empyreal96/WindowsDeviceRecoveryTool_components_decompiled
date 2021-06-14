using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200004F RID: 79
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFOLINK
	{
		// Token: 0x04000454 RID: 1108
		private IntPtr psl;

		// Token: 0x04000455 RID: 1109
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
