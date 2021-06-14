using System;

namespace Microsoft.WindowsAzure.Storage.RetryPolicies
{
	// Token: 0x020000A2 RID: 162
	public enum LocationMode
	{
		// Token: 0x040003D1 RID: 977
		PrimaryOnly,
		// Token: 0x040003D2 RID: 978
		PrimaryThenSecondary,
		// Token: 0x040003D3 RID: 979
		SecondaryOnly,
		// Token: 0x040003D4 RID: 980
		SecondaryThenPrimary
	}
}
