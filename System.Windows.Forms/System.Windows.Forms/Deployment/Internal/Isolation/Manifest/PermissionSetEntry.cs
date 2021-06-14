using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B5 RID: 181
	[StructLayout(LayoutKind.Sequential)]
	internal class PermissionSetEntry
	{
		// Token: 0x040002DD RID: 733
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Id;

		// Token: 0x040002DE RID: 734
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XmlSegment;
	}
}
