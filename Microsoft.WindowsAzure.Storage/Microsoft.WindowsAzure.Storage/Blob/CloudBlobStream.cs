using System;
using System.IO;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200000E RID: 14
	public abstract class CloudBlobStream : Stream
	{
		// Token: 0x0600011D RID: 285
		public abstract void Commit();

		// Token: 0x0600011E RID: 286
		public abstract ICancellableAsyncResult BeginCommit(AsyncCallback callback, object state);

		// Token: 0x0600011F RID: 287
		public abstract void EndCommit(IAsyncResult asyncResult);

		// Token: 0x06000120 RID: 288
		public abstract ICancellableAsyncResult BeginFlush(AsyncCallback callback, object state);

		// Token: 0x06000121 RID: 289
		public abstract void EndFlush(IAsyncResult asyncResult);
	}
}
