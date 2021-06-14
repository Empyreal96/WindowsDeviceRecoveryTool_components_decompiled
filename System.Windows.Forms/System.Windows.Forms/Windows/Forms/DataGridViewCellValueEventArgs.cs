using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGridView.CellValueNeeded" /> and <see cref="E:System.Windows.Forms.DataGridView.CellValuePushed" /> events of the <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001AA RID: 426
	public class DataGridViewCellValueEventArgs : EventArgs
	{
		// Token: 0x06001B69 RID: 7017 RVA: 0x00088790 File Offset: 0x00086990
		internal DataGridViewCellValueEventArgs()
		{
			this.columnIndex = (this.rowIndex = -1);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewCellValueEventArgs" /> class. </summary>
		/// <param name="columnIndex">The index of the column containing the cell that the event occurs for.</param>
		/// <param name="rowIndex">The index of the row containing the cell that the event occurs for.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="columnIndex" /> is less than 0.-or-
		///         <paramref name="rowIndex" /> is less than 0.</exception>
		// Token: 0x06001B6A RID: 7018 RVA: 0x000887B3 File Offset: 0x000869B3
		public DataGridViewCellValueEventArgs(int columnIndex, int rowIndex)
		{
			if (columnIndex < 0)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			if (rowIndex < 0)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
		}

		/// <summary>Gets a value indicating the column index of the cell that the event occurs for.</summary>
		/// <returns>The index of the column containing the cell that the event occurs for.</returns>
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x000887E7 File Offset: 0x000869E7
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		/// <summary>Gets a value indicating the row index of the cell that the event occurs for.</summary>
		/// <returns>The index of the row containing the cell that the event occurs for.</returns>
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x000887EF File Offset: 0x000869EF
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		/// <summary>Gets or sets the value of the cell that the event occurs for.</summary>
		/// <returns>An <see cref="T:System.Object" /> representing the cell's value.</returns>
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x000887F7 File Offset: 0x000869F7
		// (set) Token: 0x06001B6E RID: 7022 RVA: 0x000887FF File Offset: 0x000869FF
		public object Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x00088808 File Offset: 0x00086A08
		internal void SetProperties(int columnIndex, int rowIndex, object value)
		{
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
			this.val = value;
		}

		// Token: 0x04000C53 RID: 3155
		private int rowIndex;

		// Token: 0x04000C54 RID: 3156
		private int columnIndex;

		// Token: 0x04000C55 RID: 3157
		private object val;
	}
}
