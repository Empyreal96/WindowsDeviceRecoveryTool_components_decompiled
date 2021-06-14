using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A0 RID: 160
	[StructLayout(LayoutKind.Sequential)]
	internal class ProgIdRedirectionEntry
	{
		// Token: 0x040002A2 RID: 674
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ProgId;

		// Token: 0x040002A3 RID: 675
		public Guid RedirectedGuid;
	}
}
