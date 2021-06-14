using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004BA RID: 1210
	internal class GetRefreshStateEvent : GetBoolValueEvent
	{
		// Token: 0x06005128 RID: 20776 RVA: 0x001500D6 File Offset: 0x0014E2D6
		public GetRefreshStateEvent(Com2ShouldRefreshTypes item, bool defValue) : base(defValue)
		{
			this.item = item;
		}

		// Token: 0x0400344F RID: 13391
		private Com2ShouldRefreshTypes item;
	}
}
