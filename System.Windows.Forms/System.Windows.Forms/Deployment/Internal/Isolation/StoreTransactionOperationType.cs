using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200005A RID: 90
	internal enum StoreTransactionOperationType
	{
		// Token: 0x0400017F RID: 383
		Invalid,
		// Token: 0x04000180 RID: 384
		SetCanonicalizationContext = 14,
		// Token: 0x04000181 RID: 385
		StageComponent = 20,
		// Token: 0x04000182 RID: 386
		PinDeployment,
		// Token: 0x04000183 RID: 387
		UnpinDeployment,
		// Token: 0x04000184 RID: 388
		StageComponentFile,
		// Token: 0x04000185 RID: 389
		InstallDeployment,
		// Token: 0x04000186 RID: 390
		UninstallDeployment,
		// Token: 0x04000187 RID: 391
		SetDeploymentMetadata,
		// Token: 0x04000188 RID: 392
		Scavenge
	}
}
