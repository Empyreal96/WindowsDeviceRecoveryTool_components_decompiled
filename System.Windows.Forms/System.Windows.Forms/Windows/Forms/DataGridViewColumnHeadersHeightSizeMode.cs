using System;

namespace System.Windows.Forms
{
	/// <summary>Defines values for specifying how the height of the column headers is adjusted. </summary>
	// Token: 0x02000184 RID: 388
	public enum DataGridViewColumnHeadersHeightSizeMode
	{
		/// <summary>Users can adjust the column header height with the mouse.</summary>
		// Token: 0x04000B82 RID: 2946
		EnableResizing,
		/// <summary>Users cannot adjust the column header height with the mouse.</summary>
		// Token: 0x04000B83 RID: 2947
		DisableResizing,
		/// <summary>The column header height adjusts to fit the contents of all the column header cells. </summary>
		// Token: 0x04000B84 RID: 2948
		AutoSize
	}
}
