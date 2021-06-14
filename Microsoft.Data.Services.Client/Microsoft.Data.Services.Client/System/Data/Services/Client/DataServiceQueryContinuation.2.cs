using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200005B RID: 91
	public sealed class DataServiceQueryContinuation<T> : DataServiceQueryContinuation
	{
		// Token: 0x0600030D RID: 781 RVA: 0x0000DEE8 File Offset: 0x0000C0E8
		internal DataServiceQueryContinuation(Uri nextLinkUri, ProjectionPlan plan) : base(nextLinkUri, plan)
		{
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0000DEF2 File Offset: 0x0000C0F2
		internal override Type ElementType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
