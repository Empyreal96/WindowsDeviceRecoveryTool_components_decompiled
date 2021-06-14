using System;

namespace System.Windows.Documents
{
	/// <summary>Provides an inline-level flow content element that causes content to render with an italic font style.</summary>
	// Token: 0x02000380 RID: 896
	public class Italic : Span
	{
		// Token: 0x060030AF RID: 12463 RVA: 0x000DB90C File Offset: 0x000D9B0C
		static Italic()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Italic), new FrameworkPropertyMetadata(typeof(Italic)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Italic" /> class. </summary>
		// Token: 0x060030B0 RID: 12464 RVA: 0x000C4621 File Offset: 0x000C2821
		public Italic()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Italic" /> class with the specified <see cref="T:System.Windows.Documents.Inline" /> object as its initial content.</summary>
		/// <param name="childInline">The initial content of the new <see cref="T:System.Windows.Documents.Italic" />.</param>
		// Token: 0x060030B1 RID: 12465 RVA: 0x000C4629 File Offset: 0x000C2829
		public Italic(Inline childInline) : base(childInline)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Italic" /> class with the specified <see cref="T:System.Windows.Documents.Inline" /> object as its initial content, and a <see cref="T:System.Windows.Documents.TextPointer" /> that specifies an insertion position for the new <see cref="T:System.Windows.Documents.Inline" /> element.</summary>
		/// <param name="childInline">The initial content. This parameter can be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Inline" /> is inserted.</param>
		/// <param name="insertionPosition">The insertion position at which to insert the <see cref="T:System.Windows.Documents.Italic" /> element after it is created.</param>
		// Token: 0x060030B2 RID: 12466 RVA: 0x000C4632 File Offset: 0x000C2832
		public Italic(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Italic" /> class with the specified <see cref="T:System.Windows.Documents.TextPointer" /> objects that indicate the beginning and end of a content selection that the new <see cref="T:System.Windows.Documents.Italic" /> will contain.</summary>
		/// <param name="start">The beginning of a selection of content that the new <see cref="T:System.Windows.Documents.Italic" /> will contain.</param>
		/// <param name="end">The end of a selection of content that the new <see cref="T:System.Windows.Documents.Italic" /> will contain.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="start" /> or <paramref name="end" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="start" /> and <paramref name="end" /> do not resolve to a range of content suitable for enclosure by an <see cref="T:System.Windows.Documents.Italic" /> element; for example, if <paramref name="start" /> and <paramref name="end" /> indicate positions in different paragraphs.</exception>
		// Token: 0x060030B3 RID: 12467 RVA: 0x000C463C File Offset: 0x000C283C
		public Italic(TextPointer start, TextPointer end) : base(start, end)
		{
		}
	}
}
