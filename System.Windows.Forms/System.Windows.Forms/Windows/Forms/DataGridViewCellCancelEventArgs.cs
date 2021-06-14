using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides data for <see cref="E:System.Windows.Forms.DataGridView.CellBeginEdit" /> and <see cref="E:System.Windows.Forms.DataGridView.RowValidating" /> events.</summary>
	// Token: 0x02000194 RID: 404
	public class DataGridViewCellCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06001AB2 RID: 6834 RVA: 0x000864DE File Offset: 0x000846DE
		internal DataGridViewCellCancelEventArgs(DataGridViewCell dataGridViewCell) : this(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellCancelEventArgs" /> class. </summary>
		/// <param name="columnIndex">The index of the column containing the cell that the event occurs for.</param>
		/// <param name="rowIndex">The index of the row containing the cell that the event occurs for.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than -1.-or-
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001AB3 RID: 6835 RVA: 0x000864F2 File Offset: 0x000846F2
		public DataGridViewCellCancelEventArgs(int columnIndex, int rowIndex)
		{
			if (columnIndex < -1)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
		}

		/// <summary>Gets the column index of the cell that the event occurs for.</summary>
		/// <returns>The column index of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that the event occurs for.</returns>
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x00086526 File Offset: 0x00084726
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets the row index of the cell that the event occurs for.</summary>
		/// <returns>The row index of the <see cref="T:System.Windows.Forms.DataGridViewCell" /> that the event occurs for.</returns>
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0008652E File Offset: 0x0008472E
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000C02 RID: 3074
		private int columnIndex;

		// Token: 0x04000C03 RID: 3075
		private int rowIndex;
	}
}
