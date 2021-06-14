using System;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace System.Windows.Documents
{
	// Token: 0x0200039E RID: 926
	internal sealed class PageContentAsyncResult : IAsyncResult
	{
		// Token: 0x0600325B RID: 12891 RVA: 0x000DCBC8 File Offset: 0x000DADC8
		internal PageContentAsyncResult(AsyncCallback callback, object state, Dispatcher dispatcher, Uri baseUri, Uri source, FixedPage child)
		{
			this._dispatcher = dispatcher;
			this._isCompleted = false;
			this._completedSynchronously = false;
			this._callback = callback;
			this._asyncState = state;
			this._getpageStatus = PageContentAsyncResult.GetPageStatus.Loading;
			this._child = child;
			this._baseUri = baseUri;
			this._source = source;
		}

		// Token: 0x17000CAD RID: 3245
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x000DCC1D File Offset: 0x000DAE1D
		public object AsyncState
		{
			get
			{
				return this._asyncState;
			}
		}

		// Token: 0x17000CAE RID: 3246
		// (get) Token: 0x0600325D RID: 12893 RVA: 0x0000C238 File Offset: 0x0000A438
		public WaitHandle AsyncWaitHandle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x000DCC25 File Offset: 0x000DAE25
		public bool CompletedSynchronously
		{
			get
			{
				return this._completedSynchronously;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x0600325F RID: 12895 RVA: 0x000DCC2D File Offset: 0x000DAE2D
		public bool IsCompleted
		{
			get
			{
				return this._isCompleted;
			}
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x000DCC38 File Offset: 0x000DAE38
		internal object Dispatch(object arg)
		{
			if (this._exception != null)
			{
				this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
			}
			switch (this._getpageStatus)
			{
			case PageContentAsyncResult.GetPageStatus.Loading:
				try
				{
					if (this._child != null)
					{
						this._completedSynchronously = true;
						this._result = this._child;
						this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
					}
					else
					{
						Stream stream;
						PageContent._LoadPageImpl(this._baseUri, this._source, out this._result, out stream);
						if (this._result == null || this._result.IsInitialized)
						{
							stream.Close();
						}
						else
						{
							this._pendingStream = stream;
							this._result.Initialized += this._OnPaserFinished;
						}
						this._getpageStatus = PageContentAsyncResult.GetPageStatus.Finished;
					}
				}
				catch (ApplicationException exception)
				{
					this._exception = exception;
				}
				break;
			case PageContentAsyncResult.GetPageStatus.Cancelled:
			case PageContentAsyncResult.GetPageStatus.Finished:
				break;
			default:
				goto IL_D4;
			}
			this._isCompleted = true;
			if (this._callback != null)
			{
				this._callback(this);
			}
			IL_D4:
			return null;
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x000DCD2C File Offset: 0x000DAF2C
		internal void Cancel()
		{
			this._getpageStatus = PageContentAsyncResult.GetPageStatus.Cancelled;
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x000DCD35 File Offset: 0x000DAF35
		internal void Wait()
		{
			this._dispatcherOperation.Wait();
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06003263 RID: 12899 RVA: 0x000DCD43 File Offset: 0x000DAF43
		internal Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06003264 RID: 12900 RVA: 0x000DCD4B File Offset: 0x000DAF4B
		internal bool IsCancelled
		{
			get
			{
				return this._getpageStatus == PageContentAsyncResult.GetPageStatus.Cancelled;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (set) Token: 0x06003265 RID: 12901 RVA: 0x000DCD56 File Offset: 0x000DAF56
		internal DispatcherOperation DispatcherOperation
		{
			set
			{
				this._dispatcherOperation = value;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06003266 RID: 12902 RVA: 0x000DCD5F File Offset: 0x000DAF5F
		internal FixedPage Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x000DCD67 File Offset: 0x000DAF67
		private void _OnPaserFinished(object sender, EventArgs args)
		{
			if (this._pendingStream != null)
			{
				this._pendingStream.Close();
				this._pendingStream = null;
			}
		}

		// Token: 0x04001EB5 RID: 7861
		private object _asyncState;

		// Token: 0x04001EB6 RID: 7862
		private bool _isCompleted;

		// Token: 0x04001EB7 RID: 7863
		private bool _completedSynchronously;

		// Token: 0x04001EB8 RID: 7864
		private AsyncCallback _callback;

		// Token: 0x04001EB9 RID: 7865
		private Exception _exception;

		// Token: 0x04001EBA RID: 7866
		private PageContentAsyncResult.GetPageStatus _getpageStatus;

		// Token: 0x04001EBB RID: 7867
		private Uri _baseUri;

		// Token: 0x04001EBC RID: 7868
		private Uri _source;

		// Token: 0x04001EBD RID: 7869
		private FixedPage _child;

		// Token: 0x04001EBE RID: 7870
		private Dispatcher _dispatcher;

		// Token: 0x04001EBF RID: 7871
		private FixedPage _result;

		// Token: 0x04001EC0 RID: 7872
		private Stream _pendingStream;

		// Token: 0x04001EC1 RID: 7873
		private DispatcherOperation _dispatcherOperation;

		// Token: 0x020008DA RID: 2266
		internal enum GetPageStatus
		{
			// Token: 0x0400428C RID: 17036
			Loading,
			// Token: 0x0400428D RID: 17037
			Cancelled,
			// Token: 0x0400428E RID: 17038
			Finished
		}
	}
}
