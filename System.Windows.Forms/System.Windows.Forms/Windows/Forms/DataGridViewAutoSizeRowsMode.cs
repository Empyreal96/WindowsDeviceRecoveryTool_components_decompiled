using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the heights of rows are adjusted. </summary>
	// Token: 0x0200018A RID: 394
	public enum DataGridViewAutoSizeRowsMode
	{
		/// <summary>The row heights adjust to fit the contents of all cells in the rows, including header cells. </summary>
		// Token: 0x04000BAA RID: 2986
		AllCells = 7,
		/// <summary>The row heights adjust to fit the contents of all cells in the rows, excluding header cells. </summary>
		// Token: 0x04000BAB RID: 2987
		AllCellsExceptHeaders = 6,
		/// <summary>The row heights adjust to fit the contents of the row header. </summary>
		// Token: 0x04000BAC RID: 2988
		AllHeaders = 5,
		/// <summary>The row heights adjust to fit the contents of all cells in rows currently displayed onscreen, including header cells. </summary>
		// Token: 0x04000BAD RID: 2989
		DisplayedCells = 11,
		/// <summary>The row heights adjust to fit the contents of all cells in rows currently displayed onscreen, excluding header cells. </summary>
		// Token: 0x04000BAE RID: 2990
		DisplayedCellsExceptHeaders = 10,
		/// <summary>The row heights adjust to fit the contents of the row headers currently displayed onscreen.</summary>
		// Token: 0x04000BAF RID: 2991
		DisplayedHeaders = 9,
		/// <summary>The row heights do not automatically adjust.</summary>
		// Token: 0x04000BB0 RID: 2992
		None = 0
	}
}
