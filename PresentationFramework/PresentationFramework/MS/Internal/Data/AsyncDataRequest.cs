using System;

namespace MS.Internal.Data
{
	// Token: 0x02000703 RID: 1795
	internal class AsyncDataRequest
	{
		// Token: 0x06007355 RID: 29525 RVA: 0x002111C2 File Offset: 0x0020F3C2
		internal AsyncDataRequest(object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args)
		{
			this._bindingState = bindingState;
			this._workCallback = workCallback;
			this._completedCallback = completedCallback;
			this._args = args;
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x06007356 RID: 29526 RVA: 0x002111F2 File Offset: 0x0020F3F2
		public object Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x06007357 RID: 29527 RVA: 0x002111FA File Offset: 0x0020F3FA
		public AsyncRequestStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x06007358 RID: 29528 RVA: 0x00211202 File Offset: 0x0020F402
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x06007359 RID: 29529 RVA: 0x0021120A File Offset: 0x0020F40A
		public object DoWork()
		{
			if (this.DoBeginWork() && this._workCallback != null)
			{
				return this._workCallback(this);
			}
			return null;
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x0021122A File Offset: 0x0020F42A
		public bool DoBeginWork()
		{
			return this.ChangeStatus(AsyncRequestStatus.Working);
		}

		// Token: 0x0600735B RID: 29531 RVA: 0x00211233 File Offset: 0x0020F433
		public void Complete(object result)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Completed))
			{
				this._result = result;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x0600735C RID: 29532 RVA: 0x0021125A File Offset: 0x0020F45A
		public void Cancel()
		{
			this.ChangeStatus(AsyncRequestStatus.Cancelled);
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x00211264 File Offset: 0x0020F464
		public void Fail(Exception exception)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Failed))
			{
				this._exception = exception;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x0600735E RID: 29534 RVA: 0x0021128B File Offset: 0x0020F48B
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x00211294 File Offset: 0x0020F494
		private bool ChangeStatus(AsyncRequestStatus newStatus)
		{
			bool flag = false;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				switch (newStatus)
				{
				case AsyncRequestStatus.Working:
					flag = (this._status == AsyncRequestStatus.Waiting);
					break;
				case AsyncRequestStatus.Completed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Cancelled:
					flag = (this._status == AsyncRequestStatus.Waiting || this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Failed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				}
				if (flag)
				{
					this._status = newStatus;
				}
			}
			return flag;
		}

		// Token: 0x0400379B RID: 14235
		private AsyncRequestStatus _status;

		// Token: 0x0400379C RID: 14236
		private object _result;

		// Token: 0x0400379D RID: 14237
		private object _bindingState;

		// Token: 0x0400379E RID: 14238
		private object[] _args;

		// Token: 0x0400379F RID: 14239
		private Exception _exception;

		// Token: 0x040037A0 RID: 14240
		private AsyncRequestCallback _workCallback;

		// Token: 0x040037A1 RID: 14241
		private AsyncRequestCallback _completedCallback;

		// Token: 0x040037A2 RID: 14242
		private object SyncRoot = new object();
	}
}
