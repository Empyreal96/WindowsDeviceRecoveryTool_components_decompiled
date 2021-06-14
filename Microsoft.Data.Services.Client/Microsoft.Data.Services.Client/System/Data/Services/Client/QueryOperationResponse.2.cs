using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Services.Client
{
	// Token: 0x0200010E RID: 270
	public sealed class QueryOperationResponse<T> : QueryOperationResponse, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x060008C9 RID: 2249 RVA: 0x000246E0 File Offset: 0x000228E0
		internal QueryOperationResponse(HeaderCollection headers, DataServiceRequest query, MaterializeAtom results) : base(headers, query, results)
		{
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x000246EB File Offset: 0x000228EB
		public override long TotalCount
		{
			get
			{
				if (base.Results != null && base.Results.IsCountable)
				{
					return base.Results.CountValue();
				}
				throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
			}
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00024718 File Offset: 0x00022918
		public new DataServiceQueryContinuation<T> GetContinuation()
		{
			return (DataServiceQueryContinuation<T>)base.GetContinuation();
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00024737 File Offset: 0x00022937
		public new IEnumerator<T> GetEnumerator()
		{
			return base.GetEnumeratorHelper<IEnumerator<T>>(() => base.Results.Cast<T>().GetEnumerator());
		}
	}
}
