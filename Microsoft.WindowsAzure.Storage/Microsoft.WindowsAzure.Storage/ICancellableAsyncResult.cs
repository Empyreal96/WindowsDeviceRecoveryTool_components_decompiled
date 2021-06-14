using System;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x02000002 RID: 2
	public interface ICancellableAsyncResult : IAsyncResult
	{
		// Token: 0x06000001 RID: 1
		void Cancel();
	}
}
