using System;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Microsoft.WindowsAzure.Storage.Core.Executor
{
	// Token: 0x0200006B RID: 107
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	internal class TableCommand<T, INTERMEDIATE_TYPE> : StorageCommandBase<T>
	{
		// Token: 0x040001F0 RID: 496
		public Func<INTERMEDIATE_TYPE> ExecuteFunc;

		// Token: 0x040001F1 RID: 497
		public Func<AsyncCallback, object, IAsyncResult> Begin;

		// Token: 0x040001F2 RID: 498
		public Func<IAsyncResult, INTERMEDIATE_TYPE> End;

		// Token: 0x040001F3 RID: 499
		public Func<INTERMEDIATE_TYPE, RequestResult, TableCommand<T, INTERMEDIATE_TYPE>, T> ParseResponse;

		// Token: 0x040001F4 RID: 500
		public TableServiceContext Context;
	}
}
