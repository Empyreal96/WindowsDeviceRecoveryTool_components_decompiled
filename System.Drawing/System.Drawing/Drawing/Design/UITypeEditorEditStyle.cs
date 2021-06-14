using System;

namespace System.Drawing.Design
{
	/// <summary>Specifies identifiers that indicate the value editing style of a <see cref="T:System.Drawing.Design.UITypeEditor" />.</summary>
	// Token: 0x02000083 RID: 131
	public enum UITypeEditorEditStyle
	{
		/// <summary>Provides no interactive user interface (UI) component.</summary>
		// Token: 0x04000718 RID: 1816
		None = 1,
		/// <summary>Displays an ellipsis (...) button to start a modal dialog box, which requires user input before continuing a program, or a modeless dialog box, which stays on the screen and is available for use at any time but permits other user activities.</summary>
		// Token: 0x04000719 RID: 1817
		Modal,
		/// <summary>Displays a drop-down arrow button and hosts the user interface (UI) in a drop-down dialog box.</summary>
		// Token: 0x0400071A RID: 1818
		DropDown
	}
}
