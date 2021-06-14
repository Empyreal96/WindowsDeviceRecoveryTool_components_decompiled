using System;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000CA RID: 202
	public sealed class BlobListingContext : ListingContext
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x0003FF70 File Offset: 0x0003E170
		public BlobListingContext(string prefix, int? maxResults, string delimiter, BlobListingDetails details) : base(prefix, maxResults)
		{
			this.Delimiter = delimiter;
			this.Details = details;
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x0003FF89 File Offset: 0x0003E189
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x0003FF91 File Offset: 0x0003E191
		public string Delimiter { get; set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0003FF9A File Offset: 0x0003E19A
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x0003FFA2 File Offset: 0x0003E1A2
		public BlobListingDetails Details { get; set; }
	}
}
