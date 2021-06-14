using System;
using System.IO;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000028 RID: 40
	public abstract class CloudFileStream : Stream
	{
		// Token: 0x060008FB RID: 2299
		public abstract void Commit();

		// Token: 0x060008FC RID: 2300
		public abstract ICancellableAsyncResult BeginCommit(AsyncCallback callback, object state);

		// Token: 0x060008FD RID: 2301
		public abstract void EndCommit(IAsyncResult asyncResult);

		// Token: 0x060008FE RID: 2302
		public abstract ICancellableAsyncResult BeginFlush(AsyncCallback callback, object state);

		// Token: 0x060008FF RID: 2303
		public abstract void EndFlush(IAsyncResult asyncResult);
	}
}
