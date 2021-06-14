using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000B0 RID: 176
	public sealed class BlobResultSegment
	{
		// Token: 0x060010C7 RID: 4295 RVA: 0x0003EFB1 File Offset: 0x0003D1B1
		internal BlobResultSegment(IEnumerable<IListBlobItem> blobs, BlobContinuationToken continuationToken)
		{
			this.Results = blobs;
			this.ContinuationToken = continuationToken;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0003EFC7 File Offset: 0x0003D1C7
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x0003EFCF File Offset: 0x0003D1CF
		public IEnumerable<IListBlobItem> Results { get; private set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0003EFD8 File Offset: 0x0003D1D8
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
		public BlobContinuationToken ContinuationToken { get; private set; }
	}
}
