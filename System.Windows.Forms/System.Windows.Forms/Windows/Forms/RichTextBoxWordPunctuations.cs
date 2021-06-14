using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the types of punctuation tables that can be used with the <see cref="T:System.Windows.Forms.RichTextBox" /> control's word-wrapping and word-breaking features.</summary>
	// Token: 0x0200033D RID: 829
	public enum RichTextBoxWordPunctuations
	{
		/// <summary>Use pre-defined Level 1 punctuation table as default.</summary>
		// Token: 0x04002031 RID: 8241
		Level1 = 128,
		/// <summary>Use pre-defined Level 2 punctuation table as default.</summary>
		// Token: 0x04002032 RID: 8242
		Level2 = 256,
		/// <summary>Use a custom defined punctuation table.</summary>
		// Token: 0x04002033 RID: 8243
		Custom = 512,
		/// <summary>Used as a mask.</summary>
		// Token: 0x04002034 RID: 8244
		All = 896
	}
}
