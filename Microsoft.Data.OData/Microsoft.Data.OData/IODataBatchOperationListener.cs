using System;
using System.Threading.Tasks;

namespace Microsoft.Data.OData
{
	// Token: 0x02000249 RID: 585
	internal interface IODataBatchOperationListener
	{
		// Token: 0x060012BF RID: 4799
		void BatchOperationContentStreamRequested();

		// Token: 0x060012C0 RID: 4800
		Task BatchOperationContentStreamRequestedAsync();

		// Token: 0x060012C1 RID: 4801
		void BatchOperationContentStreamDisposed();
	}
}
