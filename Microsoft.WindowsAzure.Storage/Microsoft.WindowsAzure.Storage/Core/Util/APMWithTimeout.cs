using System;
using System.Threading;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200005D RID: 93
	internal sealed class APMWithTimeout : IDisposable
	{
		// Token: 0x06000D6E RID: 3438 RVA: 0x0003164C File Offset: 0x0002F84C
		public static void RunWithTimeout(Func<AsyncCallback, object, IAsyncResult> beginMethod, AsyncCallback callback, TimerCallback timeoutCallback, object state, TimeSpan timeout)
		{
			CommonUtility.AssertNotNull("beginMethod", beginMethod);
			CommonUtility.AssertNotNull("callback", callback);
			CommonUtility.AssertNotNull("timeoutCallback", timeoutCallback);
			APMWithTimeout apmwithTimeout = new APMWithTimeout(timeoutCallback);
			apmwithTimeout.Begin(beginMethod, callback, state, timeout);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0003168C File Offset: 0x0002F88C
		private APMWithTimeout(TimerCallback timeoutCallback)
		{
			this.timeoutCallback = timeoutCallback;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0003169C File Offset: 0x0002F89C
		private void Begin(Func<AsyncCallback, object, IAsyncResult> beginMethod, AsyncCallback callback, object state, TimeSpan timeout)
		{
			this.asyncResult = beginMethod(callback, state);
			WaitHandle asyncWaitHandle = this.asyncResult.AsyncWaitHandle;
			this.waitHandle = ThreadPool.RegisterWaitForSingleObject(asyncWaitHandle, new WaitOrTimerCallback(this.WaitCallback), state, timeout, true);
			if (this.disposed)
			{
				this.UnregisterWaitHandle();
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000316F0 File Offset: 0x0002F8F0
		private void WaitCallback(object state, bool timedOut)
		{
			try
			{
				if (timedOut && !this.asyncResult.IsCompleted)
				{
					TimerCallback timerCallback = this.timeoutCallback;
					this.timeoutCallback = null;
					if (timerCallback != null)
					{
						timerCallback(state);
					}
				}
			}
			finally
			{
				this.Dispose();
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00031740 File Offset: 0x0002F940
		private void UnregisterWaitHandle()
		{
			RegisteredWaitHandle registeredWaitHandle = Interlocked.Exchange<RegisteredWaitHandle>(ref this.waitHandle, null);
			if (registeredWaitHandle != null)
			{
				registeredWaitHandle.Unregister(null);
			}
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00031765 File Offset: 0x0002F965
		public void Dispose()
		{
			if (!this.disposed)
			{
				this.disposed = true;
				this.UnregisterWaitHandle();
			}
		}

		// Token: 0x040001B8 RID: 440
		private TimerCallback timeoutCallback;

		// Token: 0x040001B9 RID: 441
		private RegisteredWaitHandle waitHandle;

		// Token: 0x040001BA RID: 442
		private IAsyncResult asyncResult;

		// Token: 0x040001BB RID: 443
		private bool disposed;
	}
}
