using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000022 RID: 34
	internal struct STORE_ASSEMBLY_FILE
	{
		// Token: 0x0400010A RID: 266
		public uint Size;

		// Token: 0x0400010B RID: 267
		public uint Flags;

		// Token: 0x0400010C RID: 268
		[MarshalAs(UnmanagedType.LPWStr)]
		public string FileName;

		// Token: 0x0400010D RID: 269
		public uint FileStatusFlags;
	}
}
