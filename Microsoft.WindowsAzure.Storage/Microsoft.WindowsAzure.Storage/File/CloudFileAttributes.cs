using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000D5 RID: 213
	internal sealed class CloudFileAttributes
	{
		// Token: 0x06001168 RID: 4456 RVA: 0x00041981 File Offset: 0x0003FB81
		internal CloudFileAttributes()
		{
			this.Properties = new FileProperties();
			this.Metadata = new Dictionary<string, string>();
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x0004199F File Offset: 0x0003FB9F
		// (set) Token: 0x0600116A RID: 4458 RVA: 0x000419A7 File Offset: 0x0003FBA7
		public FileProperties Properties { get; internal set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600116B RID: 4459 RVA: 0x000419B0 File Offset: 0x0003FBB0
		// (set) Token: 0x0600116C RID: 4460 RVA: 0x000419B8 File Offset: 0x0003FBB8
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x000419C1 File Offset: 0x0003FBC1
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x000419CE File Offset: 0x0003FBCE
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x000419D6 File Offset: 0x0003FBD6
		public StorageUri StorageUri { get; internal set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x000419DF File Offset: 0x0003FBDF
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x000419E7 File Offset: 0x0003FBE7
		public CopyState CopyState { get; internal set; }
	}
}
