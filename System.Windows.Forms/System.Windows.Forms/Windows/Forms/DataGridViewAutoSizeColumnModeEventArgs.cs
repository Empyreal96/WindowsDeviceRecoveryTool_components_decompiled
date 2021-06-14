using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.AutoSizeColumnModeChanged" /> event. </summary>
	// Token: 0x02000182 RID: 386
	public class DataGridViewAutoSizeColumnModeEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnModeEventArgs" /> class. </summary>
		/// <param name="dataGridViewColumn">The column with the <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property that changed.</param>
		/// <param name="previousMode">The previous <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value of the column's <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property. </param>
		// Token: 0x06001966 RID: 6502 RVA: 0x0007E77C File Offset: 0x0007C97C
		public DataGridViewAutoSizeColumnModeEventArgs(DataGridViewColumn dataGridViewColumn, DataGridViewAutoSizeColumnMode previousMode)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.previousMode = previousMode;
		}

		/// <summary>Gets the column with the <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property that changed.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.DataGridViewColumn" /> with the <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property that changed.</returns>
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001967 RID: 6503 RVA: 0x0007E792 File Offset: 0x0007C992
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		/// <summary>Gets the previous value of the <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property of the column.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.DataGridViewAutoSizeColumnMode" /> value representing the previous value of the <see cref="P:System.Windows.Forms.DataGridViewColumn.AutoSizeMode" /> property of the <see cref="P:System.Windows.Forms.DataGridViewAutoSizeColumnModeEventArgs.Column" />.</returns>
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x0007E79A File Offset: 0x0007C99A
		public DataGridViewAutoSizeColumnMode PreviousMode
		{
			get
			{
				return this.previousMode;
			}
		}

		// Token: 0x04000B7E RID: 2942
		private DataGridViewAutoSizeColumnMode previousMode;

		// Token: 0x04000B7F RID: 2943
		private DataGridViewColumn dataGridViewColumn;
	}
}
