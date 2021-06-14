using System;

namespace System.Windows.Documents
{
	/// <summary>Determines the category of content that is adjacent to a <see cref="T:System.Windows.Documents.TextPointer" /> in a specified <see cref="T:System.Windows.Documents.LogicalDirection" />.</summary>
	// Token: 0x0200040A RID: 1034
	public enum TextPointerContext
	{
		/// <summary>The <see cref="T:System.Windows.Documents.TextPointer" /> is adjacent to the beginning or end of content.</summary>
		// Token: 0x040025DA RID: 9690
		None,
		/// <summary>The <see cref="T:System.Windows.Documents.TextPointer" /> is adjacent to text.</summary>
		// Token: 0x040025DB RID: 9691
		Text,
		/// <summary>The <see cref="T:System.Windows.Documents.TextPointer" /> is adjacent to an embedded <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" />.</summary>
		// Token: 0x040025DC RID: 9692
		EmbeddedElement,
		/// <summary>The <see cref="T:System.Windows.Documents.TextPointer" /> is adjacent to the opening tag of a <see cref="T:System.Windows.Documents.TextElement" />.</summary>
		// Token: 0x040025DD RID: 9693
		ElementStart,
		/// <summary>The <see cref="T:System.Windows.Documents.TextPointer" /> is adjacent to the closing tag of a <see cref="T:System.Windows.Documents.TextElement" />.</summary>
		// Token: 0x040025DE RID: 9694
		ElementEnd
	}
}
