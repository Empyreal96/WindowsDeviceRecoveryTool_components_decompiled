using System;

namespace System.Windows.Documents
{
	/// <summary>An inline-level flow content element which causes content to render with an underlined text decoration.</summary>
	// Token: 0x0200042C RID: 1068
	public class Underline : Span
	{
		// Token: 0x06003EE8 RID: 16104 RVA: 0x0011F3B8 File Offset: 0x0011D5B8
		static Underline()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Underline), new FrameworkPropertyMetadata(typeof(Underline)));
		}

		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.Underline" /> class. </summary>
		// Token: 0x06003EE9 RID: 16105 RVA: 0x000C4621 File Offset: 0x000C2821
		public Underline()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Underline" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Underline" />.</summary>
		/// <param name="childInline">An <see cref="T:System.Windows.Documents.Inline" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Underline" />.</param>
		// Token: 0x06003EEA RID: 16106 RVA: 0x000C4629 File Offset: 0x000C2829
		public Underline(Inline childInline) : base(childInline)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Underline" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Underline" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the new <see cref="T:System.Windows.Documents.Inline" /> element.</summary>
		/// <param name="childInline">An <see cref="T:System.Windows.Documents.Inline" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Underline" />.  This parameter may be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Inline" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the <see cref="T:System.Windows.Documents.Underline" /> element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x06003EEB RID: 16107 RVA: 0x000C4632 File Offset: 0x000C2832
		public Underline(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Underline" /> class, taking two <see cref="T:System.Windows.Documents.TextPointer" /> objects that indicate the beginning and end of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Underline" />.</summary>
		/// <param name="start">A <see cref="T:System.Windows.Documents.TextPointer" /> indicating the beginning of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Underline" />.</param>
		/// <param name="end">A <see cref="T:System.Windows.Documents.TextPointer" /> indicating the end of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Underline" />.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="start" /> or <paramref name="end" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="start" /> and <paramref name="end" /> do not resolve to a range of content suitable for enclosure by a <see cref="T:System.Windows.Documents.Underline" /> element, for example, if <paramref name="start" /> and <paramref name="end" /> indicate positions in different paragraphs.</exception>
		// Token: 0x06003EEC RID: 16108 RVA: 0x000C463C File Offset: 0x000C283C
		public Underline(TextPointer start, TextPointer end) : base(start, end)
		{
		}
	}
}
