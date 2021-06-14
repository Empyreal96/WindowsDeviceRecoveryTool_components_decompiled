using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that indicate how a <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> is displayed.</summary>
	// Token: 0x020001B9 RID: 441
	public enum DataGridViewComboBoxDisplayStyle
	{
		/// <summary>When it is not in edit mode, the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> mimics the appearance of a <see cref="T:System.Windows.Forms.ComboBox" /> control.</summary>
		// Token: 0x04000CD7 RID: 3287
		ComboBox,
		/// <summary>When it is not in edit mode, the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> is displayed with a drop-down button but does not otherwise mimic the appearance of a <see cref="T:System.Windows.Forms.ComboBox" /> control.</summary>
		// Token: 0x04000CD8 RID: 3288
		DropDownButton,
		/// <summary>When it is not in edit mode, the <see cref="T:System.Windows.Forms.DataGridViewComboBoxCell" /> is displayed without a drop-down button.</summary>
		// Token: 0x04000CD9 RID: 3289
		Nothing
	}
}
