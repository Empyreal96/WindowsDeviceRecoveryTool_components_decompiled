using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007D RID: 125
	public class ResultSegment<TElement>
	{
		// Token: 0x06000EE3 RID: 3811 RVA: 0x00038FE0 File Offset: 0x000371E0
		internal ResultSegment(List<TElement> result)
		{
			this.Results = result;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x00038FEF File Offset: 0x000371EF
		// (set) Token: 0x06000EE5 RID: 3813 RVA: 0x00038FF7 File Offset: 0x000371F7
		public List<TElement> Results { get; internal set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00039000 File Offset: 0x00037200
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x00039012 File Offset: 0x00037212
		public IContinuationToken ContinuationToken
		{
			get
			{
				if (this.continuationToken != null)
				{
					return this.continuationToken;
				}
				return null;
			}
			internal set
			{
				this.continuationToken = value;
			}
		}

		// Token: 0x0400026B RID: 619
		private IContinuationToken continuationToken;
	}
}
