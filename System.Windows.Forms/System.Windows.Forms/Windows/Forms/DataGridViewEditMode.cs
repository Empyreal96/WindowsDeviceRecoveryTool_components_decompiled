using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a user starts cell editing in the <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001C3 RID: 451
	public enum DataGridViewEditMode
	{
		/// <summary>Editing begins when the cell receives focus. This mode is useful when pressing the TAB key to enter values across a row, or when pressing the ENTER key to enter values down a column.</summary>
		// Token: 0x04000CFC RID: 3324
		EditOnEnter,
		/// <summary>Editing begins when any alphanumeric key is pressed while the cell has focus.</summary>
		// Token: 0x04000CFD RID: 3325
		EditOnKeystroke,
		/// <summary>Editing begins when any alphanumeric key or F2 is pressed while the cell has focus.</summary>
		// Token: 0x04000CFE RID: 3326
		EditOnKeystrokeOrF2,
		/// <summary>Editing begins when F2 is pressed while the cell has focus. This mode places the selection point at the end of the cell contents.</summary>
		// Token: 0x04000CFF RID: 3327
		EditOnF2,
		/// <summary>Editing begins only when the <see cref="M:System.Windows.Forms.DataGridView.BeginEdit(System.Boolean)" /> method is called. </summary>
		// Token: 0x04000D00 RID: 3328
		EditProgrammatically
	}
}
