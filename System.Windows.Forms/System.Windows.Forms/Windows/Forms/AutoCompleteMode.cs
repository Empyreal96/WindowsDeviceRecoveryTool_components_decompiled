using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the mode for the automatic completion feature used in the <see cref="T:System.Windows.Forms.ComboBox" /> and <see cref="T:System.Windows.Forms.TextBox" /> controls.</summary>
	// Token: 0x02000117 RID: 279
	public enum AutoCompleteMode
	{
		/// <summary>Disables the automatic completion feature for the <see cref="T:System.Windows.Forms.ComboBox" /> and <see cref="T:System.Windows.Forms.TextBox" /> controls.</summary>
		// Token: 0x04000554 RID: 1364
		None,
		/// <summary>Displays the auxiliary drop-down list associated with the edit control. This drop-down is populated with one or more suggested completion strings.</summary>
		// Token: 0x04000555 RID: 1365
		Suggest,
		/// <summary>Appends the remainder of the most likely candidate string to the existing characters, highlighting the appended characters.</summary>
		// Token: 0x04000556 RID: 1366
		Append,
		/// <summary>Applies both <see langword="Suggest" /> and <see langword="Append" /> options.</summary>
		// Token: 0x04000557 RID: 1367
		SuggestAppend
	}
}
