using System;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000F8 RID: 248
	[Flags]
	public enum SharedAccessQueuePermissions
	{
		// Token: 0x04000538 RID: 1336
		None = 0,
		// Token: 0x04000539 RID: 1337
		Read = 1,
		// Token: 0x0400053A RID: 1338
		Add = 2,
		// Token: 0x0400053B RID: 1339
		Update = 4,
		// Token: 0x0400053C RID: 1340
		ProcessMessages = 8
	}
}
