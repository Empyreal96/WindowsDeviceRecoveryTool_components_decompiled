using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000DF RID: 223
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterSetEntry
	{
		// Token: 0x04000386 RID: 902
		public Guid CounterSetGuid;

		// Token: 0x04000387 RID: 903
		public Guid ProviderGuid;

		// Token: 0x04000388 RID: 904
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000389 RID: 905
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x0400038A RID: 906
		public bool InstanceType;
	}
}
