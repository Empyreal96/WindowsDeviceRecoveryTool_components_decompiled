using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014E RID: 334
	public sealed class TableResultSegment : IEnumerable<CloudTable>, IEnumerable
	{
		// Token: 0x060014F3 RID: 5363 RVA: 0x0004FCAD File Offset: 0x0004DEAD
		internal TableResultSegment(List<CloudTable> result)
		{
			this.Results = result;
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x0004FCBC File Offset: 0x0004DEBC
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x0004FCC4 File Offset: 0x0004DEC4
		public IList<CloudTable> Results { get; internal set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x0004FCCD File Offset: 0x0004DECD
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x0004FCDF File Offset: 0x0004DEDF
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

		// Token: 0x060014F8 RID: 5368 RVA: 0x0004FCE8 File Offset: 0x0004DEE8
		public IEnumerator<CloudTable> GetEnumerator()
		{
			return this.Results.GetEnumerator();
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x0004FCF5 File Offset: 0x0004DEF5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Results.GetEnumerator();
		}

		// Token: 0x0400082B RID: 2091
		private TableContinuationToken continuationToken;
	}
}
