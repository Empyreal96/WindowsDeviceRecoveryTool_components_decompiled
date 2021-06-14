using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AC RID: 172
	[StructLayout(LayoutKind.Sequential)]
	internal class WindowClassEntry
	{
		// Token: 0x040002C9 RID: 713
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;

		// Token: 0x040002CA RID: 714
		[MarshalAs(UnmanagedType.LPWStr)]
		public string HostDll;

		// Token: 0x040002CB RID: 715
		public bool fVersioned;
	}
}
