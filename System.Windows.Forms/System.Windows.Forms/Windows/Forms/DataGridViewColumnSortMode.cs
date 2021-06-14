using System;

namespace System.Windows.Forms
{
	/// <summary>Defines how a <see cref="T:System.Windows.Forms.DataGridView" /> column can be sorted by the user.</summary>
	// Token: 0x020001B5 RID: 437
	public enum DataGridViewColumnSortMode
	{
		/// <summary>The column can only be sorted programmatically, but it is not intended for sorting, so the column header will not include space for a sorting glyph.</summary>
		// Token: 0x04000CA8 RID: 3240
		NotSortable,
		/// <summary>The user can sort the column by clicking the column header unless the column headers are used for selection. A sorting glyph will be displayed automatically.</summary>
		// Token: 0x04000CA9 RID: 3241
		Automatic,
		/// <summary>The column can only be sorted programmatically, and the column header will include space for a sorting glyph. </summary>
		// Token: 0x04000CAA RID: 3242
		Programmatic
	}
}
