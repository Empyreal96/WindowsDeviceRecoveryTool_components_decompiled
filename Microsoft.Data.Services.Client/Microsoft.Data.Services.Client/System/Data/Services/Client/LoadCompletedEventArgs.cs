using System;
using System.ComponentModel;

namespace System.Data.Services.Client
{
	// Token: 0x020000F5 RID: 245
	public sealed class LoadCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000829 RID: 2089 RVA: 0x00022E5C File Offset: 0x0002105C
		internal LoadCompletedEventArgs(QueryOperationResponse queryOperationResponse, Exception error) : this(queryOperationResponse, error, false)
		{
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00022E67 File Offset: 0x00021067
		internal LoadCompletedEventArgs(QueryOperationResponse queryOperationResponse, Exception error, bool cancelled) : base(error, cancelled, null)
		{
			this.queryOperationResponse = queryOperationResponse;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00022E79 File Offset: 0x00021079
		public QueryOperationResponse QueryOperationResponse
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.queryOperationResponse;
			}
		}

		// Token: 0x040004DF RID: 1247
		private QueryOperationResponse queryOperationResponse;
	}
}
