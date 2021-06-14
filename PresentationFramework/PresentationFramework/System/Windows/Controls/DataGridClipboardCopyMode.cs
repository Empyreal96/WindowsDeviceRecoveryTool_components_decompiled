using System;

namespace System.Windows.Controls
{
	/// <summary>Defines constants that specify whether users can copy data from a <see cref="T:System.Windows.Controls.DataGrid" /> control to the Clipboard and whether column header values are included.</summary>
	// Token: 0x0200049D RID: 1181
	public enum DataGridClipboardCopyMode
	{
		/// <summary>Clipboard support is disabled.</summary>
		// Token: 0x04002963 RID: 10595
		None,
		/// <summary>Users can copy the text values of selected cells to the Clipboard, and column header values are not included. </summary>
		// Token: 0x04002964 RID: 10596
		ExcludeHeader,
		/// <summary>Users can copy the text values of selected cells to the Clipboard, and column header values are included. </summary>
		// Token: 0x04002965 RID: 10597
		IncludeHeader
	}
}
