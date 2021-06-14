using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000020 RID: 32
	internal struct STORE_ASSEMBLY
	{
		// Token: 0x04000103 RID: 259
		public uint Status;

		// Token: 0x04000104 RID: 260
		public IDefinitionIdentity DefinitionIdentity;

		// Token: 0x04000105 RID: 261
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x04000106 RID: 262
		public ulong AssemblySize;

		// Token: 0x04000107 RID: 263
		public ulong ChangeId;
	}
}
