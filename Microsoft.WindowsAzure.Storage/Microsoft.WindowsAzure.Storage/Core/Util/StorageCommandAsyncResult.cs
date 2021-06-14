using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000065 RID: 101
	internal class StorageCommandAsyncResult : CancellableOperationBase, ICancellableAsyncResult, IAsyncResult, IDisposable
	{
		// Token: 0x06000DB5 RID: 3509 RVA: 0x00032C54 File Offset: 0x00030E54
		[DebuggerNonUserCode]
		protected StorageCommandAsyncResult()
		{
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00032C63 File Offset: 0x00030E63
		[DebuggerNonUserCode]
		protected StorageCommandAsyncResult(AsyncCallback callback, object state)
		{
			this.userCallback = callback;
			this.userState = state;
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00032C80 File Offset: 0x00030E80
		[DebuggerNonUserCode]
		public object AsyncState
		{
			get
			{
				return this.userState;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00032C88 File Offset: 0x00030E88
		[DebuggerNonUserCode]
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return this.LazyCreateWaitHandle();
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00032C90 File Offset: 0x00030E90
		[DebuggerNonUserCode]
		public bool CompletedSynchronously
		{
			get
			{
				return this.completedSynchronously && this.isCompleted;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00032CA2 File Offset: 0x00030EA2
		[DebuggerNonUserCode]
		public bool IsCompleted
		{
			get
			{
				return this.isCompleted;
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00032CAA File Offset: 0x00030EAA
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00032CB9 File Offset: 0x00030EB9
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.asyncWaitEvent != null)
			{
				this.asyncWaitEvent.Close();
				this.asyncWaitEvent = null;
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00032CD8 File Offset: 0x00030ED8
		private WaitHandle LazyCreateWaitHandle()
		{
			if (this.asyncWaitEvent != null)
			{
				return this.asyncWaitEvent;
			}
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			if (Interlocked.CompareExchange<ManualResetEvent>(ref this.asyncWaitEvent, manualResetEvent, null) != null)
			{
				manualResetEvent.Close();
			}
			if (this.isCompleted)
			{
				this.asyncWaitEvent.Set();
			}
			return this.asyncWaitEvent;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00032D2A File Offset: 0x00030F2A
		[DebuggerNonUserCode]
		internal void OnComplete()
		{
			if (this.isCompleted)
			{
				return;
			}
			this.isCompleted = true;
			Thread.MemoryBarrier();
			if (this.asyncWaitEvent != null)
			{
				this.asyncWaitEvent.Set();
			}
			if (this.userCallback != null)
			{
				this.userCallback(this);
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00032D69 File Offset: 0x00030F69
		[DebuggerNonUserCode]
		internal virtual void End()
		{
			if (!this.isCompleted)
			{
				this.AsyncWaitHandle.WaitOne();
			}
			this.Dispose();
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00032D85 File Offset: 0x00030F85
		[DebuggerNonUserCode]
		internal void UpdateCompletedSynchronously(bool lastOperationCompletedSynchronously)
		{
			this.completedSynchronously = (this.completedSynchronously && lastOperationCompletedSynchronously);
		}

		// Token: 0x040001DD RID: 477
		private AsyncCallback userCallback;

		// Token: 0x040001DE RID: 478
		private object userState;

		// Token: 0x040001DF RID: 479
		private bool isCompleted;

		// Token: 0x040001E0 RID: 480
		private bool completedSynchronously = true;

		// Token: 0x040001E1 RID: 481
		private ManualResetEvent asyncWaitEvent;
	}
}
