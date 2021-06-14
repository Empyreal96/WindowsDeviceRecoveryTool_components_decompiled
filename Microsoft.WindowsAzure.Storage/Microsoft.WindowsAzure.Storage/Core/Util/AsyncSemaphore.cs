using System;
using System.Collections.Generic;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x0200005F RID: 95
	internal class AsyncSemaphore
	{
		// Token: 0x06000D88 RID: 3464 RVA: 0x00031F4C File Offset: 0x0003014C
		public bool WaitAsync(AsyncSemaphore.AsyncSemaphoreCallback callback)
		{
			CommonUtility.AssertNotNull("callback", callback);
			lock (this.pendingWaits)
			{
				if (this.count > 0)
				{
					this.count--;
				}
				else
				{
					this.pendingWaits.Enqueue(callback);
					callback = null;
				}
			}
			if (callback != null)
			{
				callback(true);
				return true;
			}
			return false;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00031FC8 File Offset: 0x000301C8
		public void Release()
		{
			AsyncSemaphore.AsyncSemaphoreCallback asyncSemaphoreCallback = null;
			lock (this.pendingWaits)
			{
				if (this.pendingWaits.Count > 0)
				{
					asyncSemaphoreCallback = this.pendingWaits.Dequeue();
				}
				else
				{
					this.count++;
				}
			}
			if (asyncSemaphoreCallback != null)
			{
				asyncSemaphoreCallback(false);
			}
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00032038 File Offset: 0x00030238
		public AsyncSemaphore(int initialCount)
		{
			CommonUtility.AssertInBounds<int>("initialCount", initialCount, 0, int.MaxValue);
			this.count = initialCount;
		}

		// Token: 0x040001BC RID: 444
		private readonly Queue<AsyncSemaphore.AsyncSemaphoreCallback> pendingWaits = new Queue<AsyncSemaphore.AsyncSemaphoreCallback>();

		// Token: 0x040001BD RID: 445
		private int count;

		// Token: 0x02000060 RID: 96
		// (Invoke) Token: 0x06000D8C RID: 3468
		public delegate void AsyncSemaphoreCallback(bool calledSynchronously);
	}
}
