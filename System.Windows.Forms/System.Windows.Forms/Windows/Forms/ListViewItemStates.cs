using System;

namespace System.Windows.Forms
{
	/// <summary>Defines constants that represent the possible states of a <see cref="T:System.Windows.Forms.ListViewItem" />.</summary>
	// Token: 0x020002D1 RID: 721
	[Flags]
	public enum ListViewItemStates
	{
		/// <summary>The item is checked.</summary>
		// Token: 0x04001288 RID: 4744
		Checked = 8,
		/// <summary>The item is in its default state.</summary>
		// Token: 0x04001289 RID: 4745
		Default = 32,
		/// <summary>The item has focus.</summary>
		// Token: 0x0400128A RID: 4746
		Focused = 16,
		/// <summary>The item is disabled.</summary>
		// Token: 0x0400128B RID: 4747
		Grayed = 2,
		/// <summary>The item is currently under the mouse pointer.</summary>
		// Token: 0x0400128C RID: 4748
		Hot = 64,
		/// <summary>The item is in an indeterminate state.</summary>
		// Token: 0x0400128D RID: 4749
		Indeterminate = 256,
		/// <summary>The item is marked.</summary>
		// Token: 0x0400128E RID: 4750
		Marked = 128,
		/// <summary>The item is selected.</summary>
		// Token: 0x0400128F RID: 4751
		Selected = 1,
		/// <summary>The item should indicate a keyboard shortcut.</summary>
		// Token: 0x04001290 RID: 4752
		ShowKeyboardCues = 512
	}
}
