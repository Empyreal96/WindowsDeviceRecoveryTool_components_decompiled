using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the row header width is adjusted. </summary>
	// Token: 0x02000185 RID: 389
	public enum DataGridViewRowHeadersWidthSizeMode
	{
		/// <summary>Users can adjust the column header width with the mouse.</summary>
		// Token: 0x04000B86 RID: 2950
		EnableResizing,
		/// <summary>Users cannot adjust the column header width with the mouse.</summary>
		// Token: 0x04000B87 RID: 2951
		DisableResizing,
		/// <summary>The row header width adjusts to fit the contents of all the row header cells. </summary>
		// Token: 0x04000B88 RID: 2952
		AutoSizeToAllHeaders,
		/// <summary>The row header width adjusts to fit the contents of all the row headers in the currently displayed rows. </summary>
		// Token: 0x04000B89 RID: 2953
		AutoSizeToDisplayedHeaders,
		/// <summary>The row header width adjusts to fit the contents of the first row header.</summary>
		// Token: 0x04000B8A RID: 2954
		AutoSizeToFirstHeader
	}
}
