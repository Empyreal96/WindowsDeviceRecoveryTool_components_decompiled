using System;
using System.Diagnostics;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000066 RID: 102
	internal class StorageAsyncResult<T> : StorageCommandAsyncResult
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00032D99 File Offset: 0x00030F99
		// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x00032DA1 File Offset: 0x00030FA1
		internal T Result { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00032DAA File Offset: 0x00030FAA
		// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x00032DB2 File Offset: 0x00030FB2
		internal OperationContext OperationContext { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00032DBB File Offset: 0x00030FBB
		// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00032DC3 File Offset: 0x00030FC3
		internal IRequestOptions RequestOptions { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00032DCC File Offset: 0x00030FCC
		// (set) Token: 0x06000DC8 RID: 3528 RVA: 0x00032DD4 File Offset: 0x00030FD4
		internal object OperationState { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00032DDD File Offset: 0x00030FDD
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00032DE5 File Offset: 0x00030FE5
		internal Exception ExceptionRef { get; private set; }

		// Token: 0x06000DCB RID: 3531 RVA: 0x00032DEE File Offset: 0x00030FEE
		internal StorageAsyncResult(AsyncCallback callback, object state) : base(callback, state)
		{
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00032DF8 File Offset: 0x00030FF8
		[DebuggerNonUserCode]
		internal void OnComplete(Exception exception)
		{
			this.ExceptionRef = exception;
			base.OnComplete();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00032E07 File Offset: 0x00031007
		[DebuggerNonUserCode]
		internal override void End()
		{
			base.End();
			if (this.ExceptionRef != null)
			{
				throw this.ExceptionRef;
			}
		}
	}
}
