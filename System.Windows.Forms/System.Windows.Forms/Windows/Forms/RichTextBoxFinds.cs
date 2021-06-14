using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies how a text search is carried out in a <see cref="T:System.Windows.Forms.RichTextBox" /> control.</summary>
	// Token: 0x02000337 RID: 823
	[Flags]
	public enum RichTextBoxFinds
	{
		/// <summary>Locate all instances of the search text, whether the instances found in the search are whole words or not.</summary>
		// Token: 0x0400200B RID: 8203
		None = 0,
		/// <summary>Locate only instances of the search text that are whole words.</summary>
		// Token: 0x0400200C RID: 8204
		WholeWord = 2,
		/// <summary>Locate only instances of the search text that have the exact casing.</summary>
		// Token: 0x0400200D RID: 8205
		MatchCase = 4,
		/// <summary>The search text, if found, should not be highlighted.</summary>
		// Token: 0x0400200E RID: 8206
		NoHighlight = 8,
		/// <summary>The search starts at the end of the control's document and searches to the beginning of the document.</summary>
		// Token: 0x0400200F RID: 8207
		Reverse = 16
	}
}
