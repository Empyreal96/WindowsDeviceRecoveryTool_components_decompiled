using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the button style within a toolbar.</summary>
	// Token: 0x0200039D RID: 925
	public enum ToolBarButtonStyle
	{
		/// <summary>A standard, three-dimensional button.</summary>
		// Token: 0x04002323 RID: 8995
		PushButton = 1,
		/// <summary>A toggle button that appears sunken when clicked and retains the sunken appearance until clicked again.</summary>
		// Token: 0x04002324 RID: 8996
		ToggleButton,
		/// <summary>A space or line between toolbar buttons. The appearance depends on the value of the <see cref="P:System.Windows.Forms.ToolBar.Appearance" /> property.</summary>
		// Token: 0x04002325 RID: 8997
		Separator,
		/// <summary>A drop-down control that displays a menu or other window when clicked.</summary>
		// Token: 0x04002326 RID: 8998
		DropDownButton
	}
}
