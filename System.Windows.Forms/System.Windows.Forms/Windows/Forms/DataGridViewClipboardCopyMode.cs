using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that indicate whether content is copied from a <see cref="T:System.Windows.Forms.DataGridView" /> control to the Clipboard.</summary>
	// Token: 0x020001AD RID: 429
	public enum DataGridViewClipboardCopyMode
	{
		/// <summary>Copying to the Clipboard is disabled.</summary>
		// Token: 0x04000C6D RID: 3181
		Disable,
		/// <summary>The text values of selected cells can be copied to the Clipboard. Row or column header text is included for rows or columns that contain selected cells only when the <see cref="P:System.Windows.Forms.DataGridView.SelectionMode" /> property is set to <see cref="F:System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect" /> or <see cref="F:System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect" /> and at least one header is selected. </summary>
		// Token: 0x04000C6E RID: 3182
		EnableWithAutoHeaderText,
		/// <summary>The text values of selected cells can be copied to the Clipboard. Header text is not included.</summary>
		// Token: 0x04000C6F RID: 3183
		EnableWithoutHeaderText,
		/// <summary>The text values of selected cells can be copied to the Clipboard. Header text is included for rows and columns that contain selected cells.  </summary>
		// Token: 0x04000C70 RID: 3184
		EnableAlwaysIncludeHeaderText
	}
}
