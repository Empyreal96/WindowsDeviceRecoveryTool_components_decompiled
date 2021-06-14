using System;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000100 RID: 256
	public sealed class QueueListingContext : ListingContext
	{
		// Token: 0x06001286 RID: 4742 RVA: 0x00044EE5 File Offset: 0x000430E5
		public QueueListingContext(string prefix, int? maxResults, QueueListingDetails include) : base(prefix, maxResults)
		{
			this.Include = include;
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00044EF6 File Offset: 0x000430F6
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00044EFE File Offset: 0x000430FE
		public QueueListingDetails Include { get; set; }
	}
}
