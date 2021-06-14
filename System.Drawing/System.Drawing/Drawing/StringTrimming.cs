using System;

namespace System.Drawing
{
	/// <summary>Specifies how to trim characters from a string that does not completely fit into a layout shape.</summary>
	// Token: 0x02000049 RID: 73
	public enum StringTrimming
	{
		/// <summary>Specifies no trimming.</summary>
		// Token: 0x0400058E RID: 1422
		None,
		/// <summary>Specifies that the text is trimmed to the nearest character.</summary>
		// Token: 0x0400058F RID: 1423
		Character,
		/// <summary>Specifies that text is trimmed to the nearest word.</summary>
		// Token: 0x04000590 RID: 1424
		Word,
		/// <summary>Specifies that the text is trimmed to the nearest character, and an ellipsis is inserted at the end of a trimmed line.</summary>
		// Token: 0x04000591 RID: 1425
		EllipsisCharacter,
		/// <summary>Specifies that text is trimmed to the nearest word, and an ellipsis is inserted at the end of a trimmed line.</summary>
		// Token: 0x04000592 RID: 1426
		EllipsisWord,
		/// <summary>The center is removed from trimmed lines and replaced by an ellipsis. The algorithm keeps as much of the last slash-delimited segment of the line as possible.</summary>
		// Token: 0x04000593 RID: 1427
		EllipsisPath
	}
}
