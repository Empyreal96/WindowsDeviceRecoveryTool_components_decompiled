using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x0200004E RID: 78
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal class SHARDAPPIDINFOIDLIST
	{
		// Token: 0x04000452 RID: 1106
		private IntPtr pidl;

		// Token: 0x04000453 RID: 1107
		[MarshalAs(UnmanagedType.LPWStr)]
		private string pszAppID;
	}
}
