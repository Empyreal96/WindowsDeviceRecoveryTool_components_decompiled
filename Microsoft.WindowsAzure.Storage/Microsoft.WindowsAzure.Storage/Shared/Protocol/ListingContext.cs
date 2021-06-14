using System;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x020000C9 RID: 201
	public class ListingContext
	{
		// Token: 0x0600112B RID: 4395 RVA: 0x0003FF05 File Offset: 0x0003E105
		public ListingContext(string prefix, int? maxResults)
		{
			this.Prefix = prefix;
			this.MaxResults = maxResults;
			this.Marker = null;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x0003FF22 File Offset: 0x0003E122
		// (set) Token: 0x0600112D RID: 4397 RVA: 0x0003FF2A File Offset: 0x0003E12A
		public string Prefix { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x0003FF33 File Offset: 0x0003E133
		// (set) Token: 0x0600112F RID: 4399 RVA: 0x0003FF3B File Offset: 0x0003E13B
		public int? MaxResults
		{
			get
			{
				return this.maxResults;
			}
			set
			{
				if (value != null)
				{
					CommonUtility.AssertInBounds<int>("maxResults", value.Value, 1);
				}
				this.maxResults = value;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0003FF5F File Offset: 0x0003E15F
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x0003FF67 File Offset: 0x0003E167
		public string Marker { get; set; }

		// Token: 0x0400049A RID: 1178
		private int? maxResults;
	}
}
