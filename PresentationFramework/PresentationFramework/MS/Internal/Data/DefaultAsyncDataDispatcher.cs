using System;
using System.Collections;
using System.Threading;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000718 RID: 1816
	internal class DefaultAsyncDataDispatcher : IAsyncDataDispatcher
	{
		// Token: 0x060074D2 RID: 29906 RVA: 0x00216AF4 File Offset: 0x00214CF4
		void IAsyncDataDispatcher.AddRequest(AsyncDataRequest request)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Add(request);
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessRequest), request);
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x00216B54 File Offset: 0x00214D54
		void IAsyncDataDispatcher.CancelAllRequests()
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					AsyncDataRequest asyncDataRequest = (AsyncDataRequest)this._list[i];
					asyncDataRequest.Cancel();
				}
				this._list.Clear();
			}
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x00216BCC File Offset: 0x00214DCC
		private void ProcessRequest(object o)
		{
			AsyncDataRequest asyncDataRequest = (AsyncDataRequest)o;
			try
			{
				asyncDataRequest.Complete(asyncDataRequest.DoWork());
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				asyncDataRequest.Fail(ex);
			}
			catch
			{
				asyncDataRequest.Fail(new InvalidOperationException(SR.Get("NonCLSException", new object[]
				{
					"processing an async data request"
				})));
			}
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(asyncDataRequest);
			}
		}

		// Token: 0x040037FD RID: 14333
		private ArrayList _list = new ArrayList();
	}
}
