using System;

namespace MS.Internal.Data
{
	// Token: 0x02000729 RID: 1833
	internal interface IAsyncDataDispatcher
	{
		// Token: 0x06007548 RID: 30024
		void AddRequest(AsyncDataRequest request);

		// Token: 0x06007549 RID: 30025
		void CancelAllRequests();
	}
}
