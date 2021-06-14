using System;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x0200015B RID: 347
	[Flags]
	public enum CorsHttpMethods
	{
		// Token: 0x04000984 RID: 2436
		None = 0,
		// Token: 0x04000985 RID: 2437
		Get = 1,
		// Token: 0x04000986 RID: 2438
		Head = 2,
		// Token: 0x04000987 RID: 2439
		Post = 4,
		// Token: 0x04000988 RID: 2440
		Put = 8,
		// Token: 0x04000989 RID: 2441
		Delete = 16,
		// Token: 0x0400098A RID: 2442
		Trace = 32,
		// Token: 0x0400098B RID: 2443
		Options = 64,
		// Token: 0x0400098C RID: 2444
		Connect = 128,
		// Token: 0x0400098D RID: 2445
		Merge = 256
	}
}
