using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000D3 RID: 211
	[StructLayout(LayoutKind.Sequential)]
	internal class RegistryValueEntry
	{
		// Token: 0x04000358 RID: 856
		public uint Flags;

		// Token: 0x04000359 RID: 857
		public uint OperationHint;

		// Token: 0x0400035A RID: 858
		public uint Type;

		// Token: 0x0400035B RID: 859
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Value;

		// Token: 0x0400035C RID: 860
		[MarshalAs(UnmanagedType.LPWStr)]
		public string BuildFilter;
	}
}
