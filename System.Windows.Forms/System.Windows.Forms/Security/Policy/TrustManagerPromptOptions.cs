using System;

namespace System.Security.Policy
{
	// Token: 0x02000507 RID: 1287
	[Flags]
	internal enum TrustManagerPromptOptions
	{
		// Token: 0x04003651 RID: 13905
		None = 0,
		// Token: 0x04003652 RID: 13906
		StopApp = 1,
		// Token: 0x04003653 RID: 13907
		RequiresPermissions = 2,
		// Token: 0x04003654 RID: 13908
		WillHaveFullTrust = 4,
		// Token: 0x04003655 RID: 13909
		AddsShortcut = 8,
		// Token: 0x04003656 RID: 13910
		LocalNetworkSource = 16,
		// Token: 0x04003657 RID: 13911
		LocalComputerSource = 32,
		// Token: 0x04003658 RID: 13912
		InternetSource = 64,
		// Token: 0x04003659 RID: 13913
		TrustedSitesSource = 128,
		// Token: 0x0400365A RID: 13914
		UntrustedSitesSource = 256
	}
}
