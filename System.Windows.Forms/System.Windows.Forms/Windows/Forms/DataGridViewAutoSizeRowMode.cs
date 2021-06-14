using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the height of a row is adjusted. </summary>
	// Token: 0x0200018C RID: 396
	public enum DataGridViewAutoSizeRowMode
	{
		/// <summary>The row height adjusts to fit the contents of all cells in the row, including the header cell. </summary>
		// Token: 0x04000BB5 RID: 2997
		AllCells = 3,
		/// <summary>The row height adjusts to fit the contents of all cells in the row, excluding the header cell. </summary>
		// Token: 0x04000BB6 RID: 2998
		AllCellsExceptHeader = 2,
		/// <summary>The row height adjusts to fit the contents of the row header. </summary>
		// Token: 0x04000BB7 RID: 2999
		RowHeader = 1
	}
}
