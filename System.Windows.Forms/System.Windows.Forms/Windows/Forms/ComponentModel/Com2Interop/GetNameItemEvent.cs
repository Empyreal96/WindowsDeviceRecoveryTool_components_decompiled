using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B6 RID: 1206
	internal class GetNameItemEvent : EventArgs
	{
		// Token: 0x06005119 RID: 20761 RVA: 0x0015007B File Offset: 0x0014E27B
		public GetNameItemEvent(object defName)
		{
			this.nameItem = defName;
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x0600511A RID: 20762 RVA: 0x0015008A File Offset: 0x0014E28A
		// (set) Token: 0x0600511B RID: 20763 RVA: 0x00150092 File Offset: 0x0014E292
		public object Name
		{
			get
			{
				return this.nameItem;
			}
			set
			{
				this.nameItem = value;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x0600511C RID: 20764 RVA: 0x0015009B File Offset: 0x0014E29B
		public string NameString
		{
			get
			{
				if (this.nameItem != null)
				{
					return this.nameItem.ToString();
				}
				return "";
			}
		}

		// Token: 0x0400344D RID: 13389
		private object nameItem;
	}
}
