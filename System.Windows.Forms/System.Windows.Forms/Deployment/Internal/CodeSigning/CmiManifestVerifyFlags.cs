using System;

namespace System.Deployment.Internal.CodeSigning
{
	// Token: 0x0200000E RID: 14
	[Flags]
	internal enum CmiManifestVerifyFlags
	{
		// Token: 0x040000BD RID: 189
		None = 0,
		// Token: 0x040000BE RID: 190
		RevocationNoCheck = 1,
		// Token: 0x040000BF RID: 191
		RevocationCheckEndCertOnly = 2,
		// Token: 0x040000C0 RID: 192
		RevocationCheckEntireChain = 4,
		// Token: 0x040000C1 RID: 193
		UrlCacheOnlyRetrieval = 8,
		// Token: 0x040000C2 RID: 194
		LifetimeSigning = 16,
		// Token: 0x040000C3 RID: 195
		TrustMicrosoftRootOnly = 32,
		// Token: 0x040000C4 RID: 196
		StrongNameOnly = 65536
	}
}
