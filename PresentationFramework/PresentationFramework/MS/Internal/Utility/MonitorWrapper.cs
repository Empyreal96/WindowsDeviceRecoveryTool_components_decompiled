using System;
using System.Threading;

namespace MS.Internal.Utility
{
	// Token: 0x020007ED RID: 2029
	internal class MonitorWrapper
	{
		// Token: 0x06007D38 RID: 32056 RVA: 0x0023329C File Offset: 0x0023149C
		public IDisposable Enter()
		{
			Monitor.Enter(this._syncRoot);
			Interlocked.Increment(ref this._enterCount);
			return new MonitorWrapper.MonitorHelper(this);
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x002332BC File Offset: 0x002314BC
		public void Exit()
		{
			int num = Interlocked.Decrement(ref this._enterCount);
			Invariant.Assert(num >= 0, "unmatched call to MonitorWrapper.Exit");
			Monitor.Exit(this._syncRoot);
		}

		// Token: 0x17001D18 RID: 7448
		// (get) Token: 0x06007D3A RID: 32058 RVA: 0x002332F1 File Offset: 0x002314F1
		public bool Busy
		{
			get
			{
				return this._enterCount > 0;
			}
		}

		// Token: 0x04003AEA RID: 15082
		private int _enterCount;

		// Token: 0x04003AEB RID: 15083
		private object _syncRoot = new object();

		// Token: 0x02000B8A RID: 2954
		private class MonitorHelper : IDisposable
		{
			// Token: 0x06008E7A RID: 36474 RVA: 0x0025C6F7 File Offset: 0x0025A8F7
			public MonitorHelper(MonitorWrapper monitorWrapper)
			{
				this._monitorWrapper = monitorWrapper;
			}

			// Token: 0x06008E7B RID: 36475 RVA: 0x0025C706 File Offset: 0x0025A906
			public void Dispose()
			{
				if (this._monitorWrapper != null)
				{
					this._monitorWrapper.Exit();
					this._monitorWrapper = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004B9B RID: 19355
			private MonitorWrapper _monitorWrapper;
		}
	}
}
