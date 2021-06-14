using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B8 RID: 184
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyRequestEntry
	{
		// Token: 0x040002E1 RID: 737
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040002E2 RID: 738
		[MarshalAs(UnmanagedType.LPWStr)]
		public string permissionSetID;
	}
}
