using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200001E RID: 30
	internal struct IDENTITY_ATTRIBUTE
	{
		// Token: 0x040000FC RID: 252
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Namespace;

		// Token: 0x040000FD RID: 253
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040000FE RID: 254
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;
	}
}
