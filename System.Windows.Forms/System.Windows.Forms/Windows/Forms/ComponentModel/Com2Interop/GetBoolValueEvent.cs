using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B8 RID: 1208
	internal class GetBoolValueEvent : EventArgs
	{
		// Token: 0x06005121 RID: 20769 RVA: 0x001500B6 File Offset: 0x0014E2B6
		public GetBoolValueEvent(bool defValue)
		{
			this.value = defValue;
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06005122 RID: 20770 RVA: 0x001500C5 File Offset: 0x0014E2C5
		// (set) Token: 0x06005123 RID: 20771 RVA: 0x001500CD File Offset: 0x0014E2CD
		public bool Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x0400344E RID: 13390
		private bool value;
	}
}
