using System;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	/// <summary>An inline flow content element that causes a line break to occur in flow content.</summary>
	// Token: 0x02000391 RID: 913
	[TrimSurroundingWhitespace]
	public class LineBreak : Inline
	{
		/// <summary>Initializes a new, default instance of the <see cref="T:System.Windows.Documents.LineBreak" /> class.</summary>
		// Token: 0x060031A2 RID: 12706 RVA: 0x000DB589 File Offset: 0x000D9789
		public LineBreak()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.LineBreak" /> class, and inserts the new <see cref="T:System.Windows.Documents.LineBreak" /> at a specified position.</summary>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the <see cref="T:System.Windows.Documents.LineBreak" /> element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x060031A3 RID: 12707 RVA: 0x000DBA40 File Offset: 0x000D9C40
		public LineBreak(TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}
	}
}
