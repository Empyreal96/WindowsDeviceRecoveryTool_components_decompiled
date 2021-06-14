using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000E4 RID: 228
	public sealed class ShareResultSegment
	{
		// Token: 0x060011EB RID: 4587 RVA: 0x00042773 File Offset: 0x00040973
		internal ShareResultSegment(IEnumerable<CloudFileShare> shares, FileContinuationToken continuationToken)
		{
			this.Results = shares;
			this.ContinuationToken = continuationToken;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00042789 File Offset: 0x00040989
		// (set) Token: 0x060011ED RID: 4589 RVA: 0x00042791 File Offset: 0x00040991
		public IEnumerable<CloudFileShare> Results { get; private set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x0004279A File Offset: 0x0004099A
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x000427A2 File Offset: 0x000409A2
		public FileContinuationToken ContinuationToken { get; private set; }
	}
}
