using System;

namespace MS.Internal.Data
{
	// Token: 0x02000705 RID: 1797
	internal class AsyncSetValueRequest : AsyncDataRequest
	{
		// Token: 0x06007362 RID: 29538 RVA: 0x00211355 File Offset: 0x0020F555
		internal AsyncSetValueRequest(object item, string propertyName, object value, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
			this._value = value;
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x06007363 RID: 29539 RVA: 0x0021137A File Offset: 0x0020F57A
		public object TargetItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x06007364 RID: 29540 RVA: 0x00211382 File Offset: 0x0020F582
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040037A5 RID: 14245
		private object _item;

		// Token: 0x040037A6 RID: 14246
		private string _propertyName;

		// Token: 0x040037A7 RID: 14247
		private object _value;
	}
}
