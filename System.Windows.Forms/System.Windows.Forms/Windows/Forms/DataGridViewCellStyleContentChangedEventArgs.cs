using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellStyleContentChanged" /> event. </summary>
	// Token: 0x020001A4 RID: 420
	public class DataGridViewCellStyleContentChangedEventArgs : EventArgs
	{
		// Token: 0x06001B5B RID: 7003 RVA: 0x0008869C File Offset: 0x0008689C
		internal DataGridViewCellStyleContentChangedEventArgs(DataGridViewCellStyle dataGridViewCellStyle, bool changeAffectsPreferredSize)
		{
			this.dataGridViewCellStyle = dataGridViewCellStyle;
			this.changeAffectsPreferredSize = changeAffectsPreferredSize;
		}

		/// <summary>Gets the changed <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</summary>
		/// <returns>The changed <see cref="T:System.Windows.Forms.DataGridViewCellStyle" />.</returns>
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000886B2 File Offset: 0x000868B2
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.dataGridViewCellStyle;
			}
		}

		/// <summary>Gets the scope that is affected by the changed cell style.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridViewCellStyleScopes" /> that indicates which <see cref="T:System.Windows.Forms.DataGridView" /> entity owns the cell style that changed.</returns>
		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000886BA File Offset: 0x000868BA
		public DataGridViewCellStyleScopes CellStyleScope
		{
			get
			{
				return this.dataGridViewCellStyle.Scope;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000886C7 File Offset: 0x000868C7
		internal bool ChangeAffectsPreferredSize
		{
			get
			{
				return this.changeAffectsPreferredSize;
			}
		}

		// Token: 0x04000C3F RID: 3135
		private DataGridViewCellStyle dataGridViewCellStyle;

		// Token: 0x04000C40 RID: 3136
		private bool changeAffectsPreferredSize;
	}
}
