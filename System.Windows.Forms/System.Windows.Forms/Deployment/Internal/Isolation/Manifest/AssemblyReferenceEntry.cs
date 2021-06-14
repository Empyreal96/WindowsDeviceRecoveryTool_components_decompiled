using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A9 RID: 169
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceEntry
	{
		// Token: 0x040002C3 RID: 707
		public IReferenceIdentity ReferenceIdentity;

		// Token: 0x040002C4 RID: 708
		public uint Flags;

		// Token: 0x040002C5 RID: 709
		public AssemblyReferenceDependentAssemblyEntry DependentAssembly;
	}
}
