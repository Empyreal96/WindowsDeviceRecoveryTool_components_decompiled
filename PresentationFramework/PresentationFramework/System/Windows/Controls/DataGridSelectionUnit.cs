using System;

namespace System.Windows.Controls
{
	/// <summary>Defines constants that specify whether cells, rows, or both, are used for selection in a <see cref="T:System.Windows.Controls.DataGrid" /> control.</summary>
	// Token: 0x020004BB RID: 1211
	public enum DataGridSelectionUnit
	{
		/// <summary>Only cells are selectable. Clicking a cell selects the cell. Clicking a row or column header does nothing.</summary>
		// Token: 0x04002A21 RID: 10785
		Cell,
		/// <summary>Only full rows are selectable. Clicking a cell or a row header selects the full row.</summary>
		// Token: 0x04002A22 RID: 10786
		FullRow,
		/// <summary>Cells and rows are selectable. Clicking a cell selects only the cell. Clicking a row header selects the full row.</summary>
		// Token: 0x04002A23 RID: 10787
		CellOrRowHeader
	}
}
