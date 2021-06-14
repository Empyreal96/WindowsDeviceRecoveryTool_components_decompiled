using System;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000160 RID: 352
	[Flags]
	public enum LoggingOperations
	{
		// Token: 0x0400099D RID: 2461
		None = 0,
		// Token: 0x0400099E RID: 2462
		Read = 1,
		// Token: 0x0400099F RID: 2463
		Write = 2,
		// Token: 0x040009A0 RID: 2464
		Delete = 4,
		// Token: 0x040009A1 RID: 2465
		All = 7
	}
}
