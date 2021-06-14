using System;

namespace System.Windows.Forms
{
	// Token: 0x020001F3 RID: 499
	internal class DataGridViewIntLinkedListElement
	{
		// Token: 0x06001E2A RID: 7722 RVA: 0x00095CF5 File Offset: 0x00093EF5
		public DataGridViewIntLinkedListElement(int integer)
		{
			this.integer = integer;
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x00095D04 File Offset: 0x00093F04
		// (set) Token: 0x06001E2C RID: 7724 RVA: 0x00095D0C File Offset: 0x00093F0C
		public int Int
		{
			get
			{
				return this.integer;
			}
			set
			{
				this.integer = value;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x00095D15 File Offset: 0x00093F15
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x00095D1D File Offset: 0x00093F1D
		public DataGridViewIntLinkedListElement Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x04000D46 RID: 3398
		private int integer;

		// Token: 0x04000D47 RID: 3399
		private DataGridViewIntLinkedListElement next;
	}
}
