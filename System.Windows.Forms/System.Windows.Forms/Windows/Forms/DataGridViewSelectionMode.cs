using System;

namespace System.Windows.Forms
{
	/// <summary>Describes how cells of a DataGridView control can be selected.</summary>
	// Token: 0x0200020A RID: 522
	public enum DataGridViewSelectionMode
	{
		/// <summary>One or more individual cells can be selected.</summary>
		// Token: 0x04000DB3 RID: 3507
		CellSelect,
		/// <summary>The entire row will be selected by clicking its row's header or a cell contained in that row.</summary>
		// Token: 0x04000DB4 RID: 3508
		FullRowSelect,
		/// <summary>The entire column will be selected by clicking the column's header or a cell contained in that column.</summary>
		// Token: 0x04000DB5 RID: 3509
		FullColumnSelect,
		/// <summary>The row will be selected by clicking in the row's header cell. An individual cell can be selected by clicking that cell.</summary>
		// Token: 0x04000DB6 RID: 3510
		RowHeaderSelect,
		/// <summary>The column will be selected by clicking in the column's header cell. An individual cell can be selected by clicking that cell.</summary>
		// Token: 0x04000DB7 RID: 3511
		ColumnHeaderSelect
	}
}
