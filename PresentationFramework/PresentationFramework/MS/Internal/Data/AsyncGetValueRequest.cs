using System;

namespace MS.Internal.Data
{
	// Token: 0x02000704 RID: 1796
	internal class AsyncGetValueRequest : AsyncDataRequest
	{
		// Token: 0x06007360 RID: 29536 RVA: 0x00211330 File Offset: 0x0020F530
		internal AsyncGetValueRequest(object item, string propertyName, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x06007361 RID: 29537 RVA: 0x0021134D File Offset: 0x0020F54D
		public object SourceItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x040037A3 RID: 14243
		private object _item;

		// Token: 0x040037A4 RID: 14244
		private string _propertyName;
	}
}
