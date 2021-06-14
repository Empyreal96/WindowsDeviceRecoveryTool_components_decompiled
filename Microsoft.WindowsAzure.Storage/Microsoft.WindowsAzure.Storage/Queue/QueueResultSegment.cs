using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000F7 RID: 247
	public sealed class QueueResultSegment
	{
		// Token: 0x0600124E RID: 4686 RVA: 0x0004411B File Offset: 0x0004231B
		internal QueueResultSegment(IEnumerable<CloudQueue> queues, QueueContinuationToken continuationToken)
		{
			this.Results = queues;
			this.ContinuationToken = continuationToken;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600124F RID: 4687 RVA: 0x00044131 File Offset: 0x00042331
		// (set) Token: 0x06001250 RID: 4688 RVA: 0x00044139 File Offset: 0x00042339
		public IEnumerable<CloudQueue> Results { get; private set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x00044142 File Offset: 0x00042342
		// (set) Token: 0x06001252 RID: 4690 RVA: 0x0004414A File Offset: 0x0004234A
		public QueueContinuationToken ContinuationToken { get; private set; }
	}
}
