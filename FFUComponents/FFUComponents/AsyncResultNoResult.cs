using System;
using System.Threading;

namespace FFUComponents
{
	// Token: 0x02000002 RID: 2
	internal class AsyncResultNoResult : IAsyncResult
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000002D0
		public AsyncCallback AsyncCallback
		{
			get
			{
				return this.asyncCallback;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D8 File Offset: 0x000002D8
		public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
		{
			this.asyncCallback = asyncCallback;
			this.asyncState = state;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000002F0
		public void SetAsCompleted(Exception exception, bool completedSynchronously)
		{
			this.exception = exception;
			int num = Interlocked.Exchange(ref this.completedState, completedSynchronously ? 1 : 2);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.ERROR_RESULT_ALREADY_SET);
			}
			if (this.asyncWaitHandle != null)
			{
				this.asyncWaitHandle.Set();
			}
			if (this.asyncCallback != null)
			{
				this.asyncCallback(this);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002150 File Offset: 0x00000350
		public void EndInvoke()
		{
			if (!this.IsCompleted)
			{
				TimeSpan timeout = TimeSpan.FromMinutes(2.0);
				try
				{
					if (!this.AsyncWaitHandle.WaitOne(timeout, false))
					{
						throw new TimeoutException();
					}
				}
				finally
				{
					this.AsyncWaitHandle.Close();
					this.asyncWaitHandle = null;
				}
			}
			if (this.exception != null)
			{
				throw this.exception;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000021C0 File Offset: 0x000003C0
		public object AsyncState
		{
			get
			{
				return this.asyncState;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000021C8 File Offset: 0x000003C8
		public bool CompletedSynchronously
		{
			get
			{
				return Thread.VolatileRead(ref this.completedState) == 1;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000021D8 File Offset: 0x000003D8
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				if (this.asyncWaitHandle == null)
				{
					bool isCompleted = this.IsCompleted;
					ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted);
					if (Interlocked.CompareExchange<ManualResetEvent>(ref this.asyncWaitHandle, manualResetEvent, null) != null)
					{
						manualResetEvent.Close();
					}
					else if (!isCompleted && this.IsCompleted)
					{
						this.asyncWaitHandle.Set();
					}
				}
				return this.asyncWaitHandle;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000222F File Offset: 0x0000042F
		public bool IsCompleted
		{
			get
			{
				return Thread.VolatileRead(ref this.completedState) != 0;
			}
		}

		// Token: 0x04000001 RID: 1
		private const int statePending = 0;

		// Token: 0x04000002 RID: 2
		private const int stateCompletedSynchronously = 1;

		// Token: 0x04000003 RID: 3
		private const int stateCompletedAsynchronously = 2;

		// Token: 0x04000004 RID: 4
		private readonly AsyncCallback asyncCallback;

		// Token: 0x04000005 RID: 5
		private readonly object asyncState;

		// Token: 0x04000006 RID: 6
		private int completedState;

		// Token: 0x04000007 RID: 7
		private ManualResetEvent asyncWaitHandle;

		// Token: 0x04000008 RID: 8
		private Exception exception;
	}
}
