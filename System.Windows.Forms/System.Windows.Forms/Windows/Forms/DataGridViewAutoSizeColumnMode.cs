using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the width of a column is adjusted. </summary>
	// Token: 0x02000188 RID: 392
	public enum DataGridViewAutoSizeColumnMode
	{
		/// <summary>The sizing behavior of the column is inherited from the <see cref="P:System.Windows.Forms.DataGridView.AutoSizeColumnsMode" /> property.</summary>
		// Token: 0x04000B9B RID: 2971
		NotSet,
		/// <summary>The column width does not automatically adjust.</summary>
		// Token: 0x04000B9C RID: 2972
		None,
		/// <summary>The column width adjusts to fit the contents of all cells in the column, including the header cell. </summary>
		// Token: 0x04000B9D RID: 2973
		AllCells = 6,
		/// <summary>The column width adjusts to fit the contents of all cells in the column, excluding the header cell. </summary>
		// Token: 0x04000B9E RID: 2974
		AllCellsExceptHeader = 4,
		/// <summary>The column width adjusts to fit the contents of all cells in the column that are in rows currently displayed onscreen, including the header cell. </summary>
		// Token: 0x04000B9F RID: 2975
		DisplayedCells = 10,
		/// <summary>The column width adjusts to fit the contents of all cells in the column that are in rows currently displayed onscreen, excluding the header cell. </summary>
		// Token: 0x04000BA0 RID: 2976
		DisplayedCellsExceptHeader = 8,
		/// <summary>The column width adjusts to fit the contents of the column header cell. </summary>
		// Token: 0x04000BA1 RID: 2977
		ColumnHeader = 2,
		/// <summary>The column width adjusts so that the widths of all columns exactly fills the display area of the control, requiring horizontal scrolling only to keep column widths above the <see cref="P:System.Windows.Forms.DataGridViewColumn.MinimumWidth" /> property values. Relative column widths are determined by the relative <see cref="P:System.Windows.Forms.DataGridViewColumn.FillWeight" /> property values.</summary>
		// Token: 0x04000BA2 RID: 2978
		Fill = 16
	}
}
