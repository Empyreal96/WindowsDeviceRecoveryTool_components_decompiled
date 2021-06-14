using System;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000D7 RID: 215
	public sealed class FileDirectoryProperties
	{
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00041C69 File Offset: 0x0003FE69
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x00041C71 File Offset: 0x0003FE71
		public string ETag { get; internal set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00041C7A File Offset: 0x0003FE7A
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x00041C82 File Offset: 0x0003FE82
		public DateTimeOffset? LastModified { get; internal set; }
	}
}
