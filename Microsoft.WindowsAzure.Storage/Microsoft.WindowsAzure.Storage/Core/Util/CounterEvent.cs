using System;
using System.Threading;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000095 RID: 149
	internal sealed class CounterEvent : IDisposable
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0003CA19 File Offset: 0x0003AC19
		public WaitHandle WaitHandle
		{
			get
			{
				return this.internalEvent;
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0003CA24 File Offset: 0x0003AC24
		public void Increment()
		{
			lock (this.counterLock)
			{
				this.counter++;
				this.internalEvent.Reset();
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0003CA78 File Offset: 0x0003AC78
		public void Decrement()
		{
			lock (this.counterLock)
			{
				if (--this.counter == 0)
				{
					this.internalEvent.Set();
				}
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0003CAD4 File Offset: 0x0003ACD4
		public void Wait()
		{
			this.internalEvent.WaitOne();
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0003CAE2 File Offset: 0x0003ACE2
		public bool Wait(int millisecondsTimeout)
		{
			return this.internalEvent.WaitOne(millisecondsTimeout);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0003CAF0 File Offset: 0x0003ACF0
		public void Dispose()
		{
			if (this.internalEvent != null)
			{
				this.internalEvent.Dispose();
				this.internalEvent = null;
			}
		}

		// Token: 0x040003B4 RID: 948
		private ManualResetEvent internalEvent = new ManualResetEvent(true);

		// Token: 0x040003B5 RID: 949
		private object counterLock = new object();

		// Token: 0x040003B6 RID: 950
		private int counter;
	}
}
