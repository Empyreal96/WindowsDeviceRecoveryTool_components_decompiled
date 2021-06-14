using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies the user interface (UI) state of a element within a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001C5 RID: 453
	[Flags]
	[ComVisible(true)]
	public enum DataGridViewElementStates
	{
		/// <summary>Indicates that an element is in its default state.</summary>
		// Token: 0x04000D04 RID: 3332
		None = 0,
		/// <summary>Indicates the an element is currently displayed onscreen.</summary>
		// Token: 0x04000D05 RID: 3333
		Displayed = 1,
		/// <summary>Indicates that an element cannot be scrolled through the UI.</summary>
		// Token: 0x04000D06 RID: 3334
		Frozen = 2,
		/// <summary>Indicates that an element will not accept user input to change its value.</summary>
		// Token: 0x04000D07 RID: 3335
		ReadOnly = 4,
		/// <summary>Indicates that an element can be resized through the UI. This value is ignored except when combined with the <see cref="F:System.Windows.Forms.DataGridViewElementStates.ResizableSet" /> value.</summary>
		// Token: 0x04000D08 RID: 3336
		Resizable = 8,
		/// <summary>Indicates that an element does not inherit the resizable state of its parent.</summary>
		// Token: 0x04000D09 RID: 3337
		ResizableSet = 16,
		/// <summary>Indicates that an element is in a selected (highlighted) UI state.</summary>
		// Token: 0x04000D0A RID: 3338
		Selected = 32,
		/// <summary>Indicates that an element is visible (displayable).</summary>
		// Token: 0x04000D0B RID: 3339
		Visible = 64
	}
}
