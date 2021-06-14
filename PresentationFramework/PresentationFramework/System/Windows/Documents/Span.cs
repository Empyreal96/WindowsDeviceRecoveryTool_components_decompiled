using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>Groups other <see cref="T:System.Windows.Documents.Inline" /> flow content elements.</summary>
	// Token: 0x020003D9 RID: 985
	[ContentProperty("Inlines")]
	public class Span : Inline
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.Span" /> class.</summary>
		// Token: 0x06003541 RID: 13633 RVA: 0x000DB589 File Offset: 0x000D9789
		public Span()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Span" /> class with the specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents.</summary>
		/// <param name="childInline">The initial contents of the new <see cref="T:System.Windows.Documents.Span" />.</param>
		// Token: 0x06003542 RID: 13634 RVA: 0x000F12D1 File Offset: 0x000EF4D1
		public Span(Inline childInline) : this(childInline, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Span" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Span" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> that specifies an insertion position for the new <see cref="T:System.Windows.Documents.Inline" /> element.</summary>
		/// <param name="childInline">An <see cref="T:System.Windows.Documents.Inline" /> object that specifies the initial contents of the new <see cref="T:System.Windows.Documents.Span" />. This parameter may be null, in which case no <see cref="T:System.Windows.Documents.Inline" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies the position at which to insert the <see cref="T:System.Windows.Documents.Span" /> element after it is created, or null for no automatic insertion.</param>
		// Token: 0x06003543 RID: 13635 RVA: 0x000F12DC File Offset: 0x000EF4DC
		public Span(Inline childInline, TextPointer insertionPosition)
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
				if (childInline != null)
				{
					this.Inlines.Add(childInline);
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

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Span" /> class, taking two <see cref="T:System.Windows.Documents.TextPointer" /> objects that indicate the beginning and end of a selection of content that the new <see cref="T:System.Windows.Documents.Span" /> will contain.</summary>
		/// <param name="start">A <see cref="T:System.Windows.Documents.TextPointer" /> that indicates the beginning of a selection of content that the new <see cref="T:System.Windows.Documents.Span" /> will contain.</param>
		/// <param name="end">A <see cref="T:System.Windows.Documents.TextPointer" /> that indicates the end of a selection of content that the new <see cref="T:System.Windows.Documents.Span" /> will contain.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when <paramref name="start" /> or <paramref name="end" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">Raised when <paramref name="start" /> and <paramref name="end" /> do not resolve to a range of content suitable for enclosure by a <see cref="T:System.Windows.Documents.Span" /> element; for example, if <paramref name="start" /> and <paramref name="end" /> indicate positions in different paragraphs.</exception>
		// Token: 0x06003544 RID: 13636 RVA: 0x000F1338 File Offset: 0x000EF538
		public Span(TextPointer start, TextPointer end)
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			if (end == null)
			{
				throw new ArgumentNullException("start");
			}
			if (start.TextContainer != end.TextContainer)
			{
				throw new ArgumentException(SR.Get("InDifferentTextContainers", new object[]
				{
					"start",
					"end"
				}));
			}
			if (start.CompareTo(end) > 0)
			{
				throw new ArgumentException(SR.Get("BadTextPositionOrder", new object[]
				{
					"start",
					"end"
				}));
			}
			start.TextContainer.BeginChange();
			try
			{
				start = TextRangeEditTables.EnsureInsertionPosition(start);
				Invariant.Assert(start.Parent is Run);
				end = TextRangeEditTables.EnsureInsertionPosition(end);
				Invariant.Assert(end.Parent is Run);
				if (start.Paragraph != end.Paragraph)
				{
					throw new ArgumentException(SR.Get("InDifferentParagraphs", new object[]
					{
						"start",
						"end"
					}));
				}
				Inline nonMergeableInlineAncestor;
				if ((nonMergeableInlineAncestor = start.GetNonMergeableInlineAncestor()) != null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
					{
						nonMergeableInlineAncestor.GetType().Name
					}));
				}
				if ((nonMergeableInlineAncestor = end.GetNonMergeableInlineAncestor()) != null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
					{
						nonMergeableInlineAncestor.GetType().Name
					}));
				}
				TextElement commonAncestor = TextElement.GetCommonAncestor((TextElement)start.Parent, (TextElement)end.Parent);
				while (start.Parent != commonAncestor)
				{
					start = this.SplitElement(start);
				}
				while (end.Parent != commonAncestor)
				{
					end = this.SplitElement(end);
				}
				if (start.Parent is Run)
				{
					start = this.SplitElement(start);
				}
				if (end.Parent is Run)
				{
					end = this.SplitElement(end);
				}
				Invariant.Assert(start.Parent == end.Parent);
				Invariant.Assert(TextSchema.IsValidChild(start, typeof(Span)));
				base.Reposition(start, end);
			}
			finally
			{
				start.TextContainer.EndChange();
			}
		}

		/// <summary>Gets an <see cref="T:System.Windows.Documents.InlineCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Span" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.InlineCollection" /> containing the <see cref="T:System.Windows.Documents.Inline" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Span" />.This property has no default value.</returns>
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x000DCFC8 File Offset: 0x000DB1C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InlineCollection Inlines
		{
			get
			{
				return new InlineCollection(this, true);
			}
		}

		/// <summary>Returns a value that indicates whether the content of a <see cref="T:System.Windows.Documents.Span" /> element should be serialized during serialization of a <see cref="T:System.Windows.Documents.Run" /> object.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the content should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">
		///         <paramref name="manager" /> is null.</exception>
		// Token: 0x06003546 RID: 13638 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeInlines(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x000F1568 File Offset: 0x000EF768
		private TextPointer SplitElement(TextPointer position)
		{
			if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				position = position.GetNextContextPosition(LogicalDirection.Backward);
			}
			else if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				position = position.GetNextContextPosition(LogicalDirection.Forward);
			}
			else
			{
				position = TextRangeEdit.SplitElement(position);
			}
			return position;
		}
	}
}
