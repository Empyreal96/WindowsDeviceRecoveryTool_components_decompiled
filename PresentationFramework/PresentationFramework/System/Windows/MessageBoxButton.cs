using System;

namespace System.Windows
{
	/// <summary>Specifies the buttons that are displayed on a message box. Used as an argument of the <see cref="Overload:System.Windows.MessageBox.Show" /> method.</summary>
	// Token: 0x020000DC RID: 220
	public enum MessageBoxButton
	{
		/// <summary>The message box displays an OK button.</summary>
		// Token: 0x0400075C RID: 1884
		OK,
		/// <summary>The message box displays OK and Cancel buttons.</summary>
		// Token: 0x0400075D RID: 1885
		OKCancel,
		/// <summary>The message box displays Yes, No, and Cancel buttons.</summary>
		// Token: 0x0400075E RID: 1886
		YesNoCancel = 3,
		/// <summary>The message box displays Yes and No buttons.</summary>
		// Token: 0x0400075F RID: 1887
		YesNo
	}
}
