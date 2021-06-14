using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000B5 RID: 181
	public sealed class ContainerResultSegment
	{
		// Token: 0x060010CC RID: 4300 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
		internal ContainerResultSegment(IEnumerable<CloudBlobContainer> containers, BlobContinuationToken continuationToken)
		{
			this.Results = containers;
			this.ContinuationToken = continuationToken;
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x0003EFFF File Offset: 0x0003D1FF
		// (set) Token: 0x060010CE RID: 4302 RVA: 0x0003F007 File Offset: 0x0003D207
		public IEnumerable<CloudBlobContainer> Results { get; private set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0003F010 File Offset: 0x0003D210
		// (set) Token: 0x060010D0 RID: 4304 RVA: 0x0003F018 File Offset: 0x0003D218
		public BlobContinuationToken ContinuationToken { get; private set; }
	}
}
