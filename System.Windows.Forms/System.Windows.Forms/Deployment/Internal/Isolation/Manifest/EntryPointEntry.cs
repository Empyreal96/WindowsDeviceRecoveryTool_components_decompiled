using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B2 RID: 178
	[StructLayout(LayoutKind.Sequential)]
	internal class EntryPointEntry
	{
		// Token: 0x040002D3 RID: 723
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040002D4 RID: 724
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_File;

		// Token: 0x040002D5 RID: 725
		[MarshalAs(UnmanagedType.LPWStr)]
		public string CommandLine_Parameters;

		// Token: 0x040002D6 RID: 726
		public IReferenceIdentity Identity;

		// Token: 0x040002D7 RID: 727
		public uint Flags;
	}
}
