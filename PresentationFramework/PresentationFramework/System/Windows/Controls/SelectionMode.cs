using System;

namespace System.Windows.Controls
{
	/// <summary>Defines the selection behavior for a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
	// Token: 0x020004FB RID: 1275
	public enum SelectionMode
	{
		/// <summary>The user can select only one item at a time. </summary>
		// Token: 0x04002C5F RID: 11359
		Single,
		/// <summary>The user can select multiple items without holding down a modifier key.</summary>
		// Token: 0x04002C60 RID: 11360
		Multiple,
		/// <summary>The user can select multiple consecutive items while holding down the SHIFT key. </summary>
		// Token: 0x04002C61 RID: 11361
		Extended
	}
}
