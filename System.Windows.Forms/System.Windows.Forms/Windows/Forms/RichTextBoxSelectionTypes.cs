using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the type of selection in a <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
	// Token: 0x0200033B RID: 827
	[Flags]
	public enum RichTextBoxSelectionTypes
	{
		/// <summary>No text is selected in the current selection.</summary>
		// Token: 0x04002025 RID: 8229
		Empty = 0,
		/// <summary>The current selection contains only text.</summary>
		// Token: 0x04002026 RID: 8230
		Text = 1,
		/// <summary>At least one Object Linking and Embedding (OLE) object is selected.</summary>
		// Token: 0x04002027 RID: 8231
		Object = 2,
		/// <summary>More than one character is selected.</summary>
		// Token: 0x04002028 RID: 8232
		MultiChar = 4,
		/// <summary>More than one Object Linking and Embedding (OLE) object is selected.</summary>
		// Token: 0x04002029 RID: 8233
		MultiObject = 8
	}
}
