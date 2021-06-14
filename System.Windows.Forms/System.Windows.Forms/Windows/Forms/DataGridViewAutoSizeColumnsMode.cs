using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the widths of columns are adjusted. </summary>
	// Token: 0x02000187 RID: 391
	public enum DataGridViewAutoSizeColumnsMode
	{
		/// <summary>The column widths adjust to fit the contents of all cells in the columns, including header cells. </summary>
		// Token: 0x04000B93 RID: 2963
		AllCells = 6,
		/// <summary>The column widths adjust to fit the contents of all cells in the columns, excluding header cells. </summary>
		// Token: 0x04000B94 RID: 2964
		AllCellsExceptHeader = 4,
		/// <summary>The column widths adjust to fit the contents of all cells in the columns that are in rows currently displayed onscreen, including header cells. </summary>
		// Token: 0x04000B95 RID: 2965
		DisplayedCells = 10,
		/// <summary>The column widths adjust to fit the contents of all cells in the columns that are in rows currently displayed onscreen, excluding header cells. </summary>
		// Token: 0x04000B96 RID: 2966
		DisplayedCellsExceptHeader = 8,
		/// <summary>The column widths do not automatically adjust. </summary>
		// Token: 0x04000B97 RID: 2967
		None = 1,
		/// <summary>The column widths adjust to fit the contents of the column header cells. </summary>
		// Token: 0x04000B98 RID: 2968
		ColumnHeader,
		/// <summary>The column widths adjust so that the widths of all columns exactly fill the display area of the control, requiring horizontal scrolling only to keep column widths above the <see cref="P:System.Windows.Forms.DataGridViewColumn.MinimumWidth" /> property values. Relative column widths are determined by the relative <see cref="P:System.Windows.Forms.DataGridViewColumn.FillWeight" /> property values.</summary>
		// Token: 0x04000B99 RID: 2969
		Fill = 16
	}
}
