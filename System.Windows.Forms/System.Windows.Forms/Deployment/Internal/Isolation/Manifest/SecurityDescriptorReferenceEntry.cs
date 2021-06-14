using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000DC RID: 220
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityDescriptorReferenceEntry
	{
		// Token: 0x04000381 RID: 897
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000382 RID: 898
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
