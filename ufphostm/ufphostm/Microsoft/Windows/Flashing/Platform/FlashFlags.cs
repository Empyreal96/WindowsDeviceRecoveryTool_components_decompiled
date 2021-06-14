using System;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x0200004A RID: 74
	[Flags]
	public enum FlashFlags : uint
	{
		// Token: 0x0400012A RID: 298
		Normal = 0U,
		// Token: 0x0400012B RID: 299
		SkipPlatformIDCheck = 1U,
		// Token: 0x0400012C RID: 300
		SkipSignatureCheck = 2U,
		// Token: 0x0400012D RID: 301
		SkipRootKeyHashCheck = 4U,
		// Token: 0x0400012E RID: 302
		SkipHash = 8U,
		// Token: 0x0400012F RID: 303
		VerifyWrite = 16U
	}
}
