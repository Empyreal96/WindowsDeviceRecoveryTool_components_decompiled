using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000C7 RID: 199
	public sealed class BlobContainerEntry
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x0003FD2F File Offset: 0x0003DF2F
		internal BlobContainerEntry()
		{
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x0003FD37 File Offset: 0x0003DF37
		// (set) Token: 0x06001123 RID: 4387 RVA: 0x0003FD3F File Offset: 0x0003DF3F
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x0003FD48 File Offset: 0x0003DF48
		// (set) Token: 0x06001125 RID: 4389 RVA: 0x0003FD50 File Offset: 0x0003DF50
		public BlobContainerProperties Properties { get; internal set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x0003FD59 File Offset: 0x0003DF59
		// (set) Token: 0x06001127 RID: 4391 RVA: 0x0003FD61 File Offset: 0x0003DF61
		public string Name { get; internal set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001128 RID: 4392 RVA: 0x0003FD6A File Offset: 0x0003DF6A
		// (set) Token: 0x06001129 RID: 4393 RVA: 0x0003FD72 File Offset: 0x0003DF72
		public Uri Uri { get; internal set; }
	}
}
