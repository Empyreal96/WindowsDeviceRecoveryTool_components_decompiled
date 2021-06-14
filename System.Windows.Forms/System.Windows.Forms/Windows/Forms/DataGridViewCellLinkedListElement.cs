using System;

namespace System.Windows.Forms
{
	// Token: 0x0200019D RID: 413
	internal class DataGridViewCellLinkedListElement
	{
		// Token: 0x06001AFE RID: 6910 RVA: 0x000870B7 File Offset: 0x000852B7
		public DataGridViewCellLinkedListElement(DataGridViewCell dataGridViewCell)
		{
			this.dataGridViewCell = dataGridViewCell;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x000870C6 File Offset: 0x000852C6
		public DataGridViewCell DataGridViewCell
		{
			get
			{
				return this.dataGridViewCell;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x000870CE File Offset: 0x000852CE
		// (set) Token: 0x06001B01 RID: 6913 RVA: 0x000870D6 File Offset: 0x000852D6
		public DataGridViewCellLinkedListElement Next
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

		// Token: 0x04000C16 RID: 3094
		private DataGridViewCell dataGridViewCell;

		// Token: 0x04000C17 RID: 3095
		private DataGridViewCellLinkedListElement next;
	}
}
