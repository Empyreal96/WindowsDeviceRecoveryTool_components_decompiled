using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for <see cref="T:System.Windows.Forms.DataGridView" /> events related to cell and row operations.</summary>
	// Token: 0x02000199 RID: 409
	public class DataGridViewCellEventArgs : EventArgs
	{
		// Token: 0x06001AE5 RID: 6885 RVA: 0x00086D29 File Offset: 0x00084F29
		internal DataGridViewCellEventArgs(DataGridViewCell dataGridViewCell) : this(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> class. </summary>
		/// <param name="columnIndex">The index of the column containing the cell that the event occurs for.</param>
		/// <param name="rowIndex">The index of the row containing the cell that the event occurs for.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than -1.-or-
		///         <paramref name="rowIndex" /> is less than -1.</exception>
		// Token: 0x06001AE6 RID: 6886 RVA: 0x00086D3D File Offset: 0x00084F3D
		public DataGridViewCellEventArgs(int columnIndex, int rowIndex)
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

		/// <summary>Gets a value indicating the column index of the cell that the event occurs for.</summary>
		/// <returns>The index of the column containing the cell that the event occurs for.</returns>
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x00086D71 File Offset: 0x00084F71
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets a value indicating the row index of the cell that the event occurs for.</summary>
		/// <returns>The index of the row containing the cell that the event occurs for.</returns>
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x00086D79 File Offset: 0x00084F79
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04000C09 RID: 3081
		private int columnIndex;

		// Token: 0x04000C0A RID: 3082
		private int rowIndex;
	}
}
