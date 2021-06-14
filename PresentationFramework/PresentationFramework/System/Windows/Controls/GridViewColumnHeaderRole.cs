using System;

namespace System.Windows.Controls
{
	/// <summary>Defines the state or role of a <see cref="T:System.Windows.Controls.GridViewColumnHeader" /> control.</summary>
	// Token: 0x020004DA RID: 1242
	public enum GridViewColumnHeaderRole
	{
		/// <summary>The column header displays above its associated column.</summary>
		// Token: 0x04002B56 RID: 11094
		Normal,
		/// <summary>The column header is the object of a drag-and-drop operation to move a column.</summary>
		// Token: 0x04002B57 RID: 11095
		Floating,
		/// <summary>The column header is the last header in the row of column headers and is used for padding.</summary>
		// Token: 0x04002B58 RID: 11096
		Padding
	}
}
