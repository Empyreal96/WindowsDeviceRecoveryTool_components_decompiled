using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x02000013 RID: 19
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct BLOBHEADER
	{
		// Token: 0x040000DB RID: 219
		internal byte bType;

		// Token: 0x040000DC RID: 220
		internal byte bVersion;

		// Token: 0x040000DD RID: 221
		internal short reserved;

		// Token: 0x040000DE RID: 222
		internal uint aiKeyAlg;
	}
}
