using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A3 RID: 163
	[StructLayout(LayoutKind.Sequential)]
	internal class CLRSurrogateEntry
	{
		// Token: 0x040002A6 RID: 678
		public Guid Clsid;

		// Token: 0x040002A7 RID: 679
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x040002A8 RID: 680
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;
	}
}
