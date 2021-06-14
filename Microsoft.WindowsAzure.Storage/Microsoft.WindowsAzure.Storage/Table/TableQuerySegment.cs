using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014B RID: 331
	public class TableQuerySegment<TElement> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x060014C8 RID: 5320 RVA: 0x0004F716 File Offset: 0x0004D916
		internal TableQuerySegment(List<TElement> result)
		{
			this.Results = result;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0004F725 File Offset: 0x0004D925
		internal TableQuerySegment(ResultSegment<TElement> resSeg) : this(resSeg.Results)
		{
			this.continuationToken = (TableContinuationToken)resSeg.ContinuationToken;
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060014CA RID: 5322 RVA: 0x0004F744 File Offset: 0x0004D944
		// (set) Token: 0x060014CB RID: 5323 RVA: 0x0004F74C File Offset: 0x0004D94C
		public List<TElement> Results { get; internal set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x0004F755 File Offset: 0x0004D955
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x0004F767 File Offset: 0x0004D967
		public TableContinuationToken ContinuationToken
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

		// Token: 0x060014CE RID: 5326 RVA: 0x0004F770 File Offset: 0x0004D970
		public IEnumerator<TElement> GetEnumerator()
		{
			return this.Results.GetEnumerator();
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0004F782 File Offset: 0x0004D982
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Results.GetEnumerator();
		}

		// Token: 0x0400081C RID: 2076
		private TableContinuationToken continuationToken;
	}
}
