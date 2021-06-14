using System;
using System.Windows.Threading;

namespace System.Windows.Navigation
{
	// Token: 0x0200031C RID: 796
	internal class NavigateQueueItem
	{
		// Token: 0x06002A03 RID: 10755 RVA: 0x000C1BCC File Offset: 0x000BFDCC
		internal NavigateQueueItem(Uri source, object content, NavigationMode mode, object navState, NavigationService nc)
		{
			this._source = source;
			this._content = content;
			this._navState = navState;
			this._nc = nc;
			this._navigationMode = mode;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000C1BF9 File Offset: 0x000BFDF9
		internal void PostNavigation()
		{
			this._postedOp = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.Dispatch), null);
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000C1C1A File Offset: 0x000BFE1A
		internal void Stop()
		{
			if (this._postedOp != null)
			{
				this._postedOp.Abort();
				this._postedOp = null;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x000C1C37 File Offset: 0x000BFE37
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06002A07 RID: 10759 RVA: 0x000C1C3F File Offset: 0x000BFE3F
		internal object NavState
		{
			get
			{
				return this._navState;
			}
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000C1C48 File Offset: 0x000BFE48
		private object Dispatch(object obj)
		{
			this._postedOp = null;
			if (this._content != null || this._source == null)
			{
				this._nc.DoNavigate(this._content, this._navigationMode, this._navState);
			}
			else
			{
				this._nc.DoNavigate(this._source, this._navigationMode, this._navState);
			}
			return null;
		}

		// Token: 0x04001C25 RID: 7205
		private Uri _source;

		// Token: 0x04001C26 RID: 7206
		private object _content;

		// Token: 0x04001C27 RID: 7207
		private object _navState;

		// Token: 0x04001C28 RID: 7208
		private NavigationService _nc;

		// Token: 0x04001C29 RID: 7209
		private NavigationMode _navigationMode;

		// Token: 0x04001C2A RID: 7210
		private DispatcherOperation _postedOp;
	}
}
