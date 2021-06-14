using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000E5 RID: 229
	[StructLayout(LayoutKind.Sequential)]
	internal class CompatibleFrameworkEntry
	{
		// Token: 0x040003A0 RID: 928
		public uint index;

		// Token: 0x040003A1 RID: 929
		[MarshalAs(UnmanagedType.LPWStr)]
		public string TargetVersion;

		// Token: 0x040003A2 RID: 930
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Profile;

		// Token: 0x040003A3 RID: 931
		[MarshalAs(UnmanagedType.LPWStr)]
		public string SupportedRuntime;
	}
}
