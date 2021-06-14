using System;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>Represents a position within a <see cref="T:System.Windows.Documents.FlowDocument" /> or <see cref="T:System.Windows.Controls.TextBlock" />.</summary>
	// Token: 0x02000408 RID: 1032
	public class TextPointer : ContentPosition, ITextPointer
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x00106834 File Offset: 0x00104A34
		internal TextPointer(TextPointer textPointer)
		{
			if (textPointer == null)
			{
				throw new ArgumentNullException("textPointer");
			}
			this.InitializeOffset(textPointer, 0, textPointer.GetGravityInternal());
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00106858 File Offset: 0x00104A58
		internal TextPointer(TextPointer position, int offset)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			this.InitializeOffset(position, offset, position.GetGravityInternal());
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x0010687C File Offset: 0x00104A7C
		internal TextPointer(TextPointer position, LogicalDirection direction)
		{
			this.InitializeOffset(position, 0, direction);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x0010688D File Offset: 0x00104A8D
		internal TextPointer(TextPointer position, int offset, LogicalDirection direction)
		{
			this.InitializeOffset(position, offset, direction);
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001068A0 File Offset: 0x00104AA0
		internal TextPointer(TextContainer textContainer, int offset, LogicalDirection direction)
		{
			if (offset < 1 || offset > textContainer.InternalSymbolCount - 1)
			{
				throw new ArgumentException(SR.Get("BadDistance"));
			}
			SplayTreeNode splayTreeNode;
			ElementEdge edge;
			textContainer.GetNodeAndEdgeAtOffset(offset, out splayTreeNode, out edge);
			this.Initialize(textContainer, (TextTreeNode)splayTreeNode, edge, direction, textContainer.PositionGeneration, false, false, textContainer.LayoutGeneration);
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x001068FC File Offset: 0x00104AFC
		internal TextPointer(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			this.Initialize(tree, node, edge, LogicalDirection.Forward, tree.PositionGeneration, false, false, tree.LayoutGeneration);
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x00106928 File Offset: 0x00104B28
		internal TextPointer(TextContainer tree, TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			this.Initialize(tree, node, edge, direction, tree.PositionGeneration, false, false, tree.LayoutGeneration);
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x00106954 File Offset: 0x00104B54
		internal TextPointer CreatePointer()
		{
			return new TextPointer(this);
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x0010695C File Offset: 0x00104B5C
		internal TextPointer CreatePointer(LogicalDirection gravity)
		{
			return new TextPointer(this, gravity);
		}

		/// <summary>Indicates whether the specified position is in the same text container as the current position.</summary>
		/// <param name="textPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies a position to compare to the current position.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="textPosition" /> indicates a position that is in the same text container as the current position; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="textPosition" /> is <see langword="null" />.</exception>
		// Token: 0x060039D5 RID: 14805 RVA: 0x00106965 File Offset: 0x00104B65
		public bool IsInSameDocument(TextPointer textPosition)
		{
			if (textPosition == null)
			{
				throw new ArgumentNullException("textPosition");
			}
			this._tree.EmptyDeadPositionList();
			return this.TextContainer == textPosition.TextContainer;
		}

		/// <summary>Performs an ordinal comparison between the positions specified by the current <see cref="T:System.Windows.Documents.TextPointer" /> and a second specified <see cref="T:System.Windows.Documents.TextPointer" />.</summary>
		/// <param name="position">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies a position to compare to the current position.</param>
		/// <returns>–1 if the current <see cref="T:System.Windows.Documents.TextPointer" /> precedes <paramref name="position" />; 0 if the locations are the same; +1 if the current <see cref="T:System.Windows.Documents.TextPointer" /> follows <paramref name="position" />.  </returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="position" /> specifies a position outside of the text container associated with the current position.</exception>
		// Token: 0x060039D6 RID: 14806 RVA: 0x00106990 File Offset: 0x00104B90
		public int CompareTo(TextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			this.SyncToTreeGeneration();
			position.SyncToTreeGeneration();
			int symbolOffset = this.GetSymbolOffset();
			int symbolOffset2 = position.GetSymbolOffset();
			int result;
			if (symbolOffset < symbolOffset2)
			{
				result = -1;
			}
			else if (symbolOffset > symbolOffset2)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		/// <summary>Returns a category indicator for the content adjacent to the current <see cref="T:System.Windows.Documents.TextPointer" /> in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to determine the category for adjacent content.</param>
		/// <returns>One of the <see cref="T:System.Windows.Documents.TextPointerContext" /> values that indicates the category for adjacent content in the specified logical direction.</returns>
		// Token: 0x060039D7 RID: 14807 RVA: 0x001069E4 File Offset: 0x00104BE4
		public TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (direction != LogicalDirection.Forward)
			{
				return TextPointer.GetPointerContextBackward(this._node, this.Edge);
			}
			return TextPointer.GetPointerContextForward(this._node, this.Edge);
		}

		/// <summary>Returns the number of Unicode characters between the current <see cref="T:System.Windows.Documents.TextPointer" /> and the next non-text symbol, in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to count the number of characters.</param>
		/// <returns>The number of Unicode characters between the current <see cref="T:System.Windows.Documents.TextPointer" /> and the next non-text symbol.  This number may be 0 if there is no adjacent text.</returns>
		// Token: 0x060039D8 RID: 14808 RVA: 0x00106A34 File Offset: 0x00104C34
		public int GetTextRunLength(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			int num = 0;
			if (this._tree.PlainTextOnly)
			{
				Invariant.Assert(this.GetScopingNode() is TextTreeRootNode);
				if (direction == LogicalDirection.Forward)
				{
					num = this._tree.InternalSymbolCount - this.GetSymbolOffset() - 1;
				}
				else
				{
					num = this.GetSymbolOffset() - 1;
				}
			}
			else
			{
				for (TextTreeNode textTreeNode = this.GetAdjacentTextNodeSibling(direction); textTreeNode != null; textTreeNode = (((direction == LogicalDirection.Forward) ? textTreeNode.GetNextNode() : textTreeNode.GetPreviousNode()) as TextTreeTextNode))
				{
					num += textTreeNode.SymbolCount;
				}
			}
			return num;
		}

		/// <summary>Returns the count of symbols between the current <see cref="T:System.Windows.Documents.TextPointer" /> and a second specified <see cref="T:System.Windows.Documents.TextPointer" />.</summary>
		/// <param name="position">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies a position to find the distance (in symbols) to.</param>
		/// <returns>The relative number of symbols between the current <see cref="T:System.Windows.Documents.TextPointer" /> and <paramref name="position" />.  A negative value indicates that the current <see cref="T:System.Windows.Documents.TextPointer" /> follows the position specified by <paramref name="position" />, 0 indicates that the positions are equal, and a positive value indicates that the current <see cref="T:System.Windows.Documents.TextPointer" /> precedes the position specified by <paramref name="position" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="position" /> specifies a position outside of the text container associated with the current position.</exception>
		// Token: 0x060039D9 RID: 14809 RVA: 0x00106AD4 File Offset: 0x00104CD4
		public int GetOffsetToPosition(TextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			this.SyncToTreeGeneration();
			position.SyncToTreeGeneration();
			return position.GetSymbolOffset() - this.GetSymbolOffset();
		}

		/// <summary>Returns a string containing any text adjacent to the current <see cref="T:System.Windows.Documents.TextPointer" /> in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to find and return any adjacent text.</param>
		/// <returns>A string containing any adjacent text in the specified logical direction, or <see cref="F:System.String.Empty" /> if no adjacent text can be found.</returns>
		// Token: 0x060039DA RID: 14810 RVA: 0x00106B06 File Offset: 0x00104D06
		public string GetTextInRun(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			return TextPointerBase.GetTextInRun(this, direction);
		}

		/// <summary>Copies the specified maximum number of characters from any adjacent text in the specified direction into a caller-supplied character array.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to find and copy any adjacent text.</param>
		/// <param name="textBuffer">A buffer into which any text is copied.</param>
		/// <param name="startIndex">An index into <paramref name="textBuffer" /> at which to begin writing copied text.</param>
		/// <param name="count">The maximum number of characters to copy.</param>
		/// <returns>The number of characters actually copied into <paramref name="textBuffer" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="startIndex" /> is less than 0 or greater than the <see cref="P:System.Array.Length" /> property of <paramref name="textBuffer" />. -or-
		///         <paramref name="count" /> is less than 0 or greater than the remaining space in <paramref name="textBuffer" /> (<paramref name="textBuffer" />.<see cref="P:System.Array.Length" /> minus <paramref name="startIndex" />).</exception>
		// Token: 0x060039DB RID: 14811 RVA: 0x00106B1C File Offset: 0x00104D1C
		public int GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.SyncToTreeGeneration();
			TextTreeTextNode adjacentTextNodeSibling = this.GetAdjacentTextNodeSibling(direction);
			if (adjacentTextNodeSibling != null)
			{
				return TextPointer.GetTextInRun(this._tree, this.GetSymbolOffset(), adjacentTextNodeSibling, -1, direction, textBuffer, startIndex, count);
			}
			return 0;
		}

		/// <summary>Returns the element, if any, that borders the current <see cref="T:System.Windows.Documents.TextPointer" /> in the specified logical direction. </summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to search for an adjacent element.</param>
		/// <returns>The adjacent element in the specified <paramref name="direction" />, or <see langword="null" /> if no adjacent element exists.</returns>
		// Token: 0x060039DC RID: 14812 RVA: 0x00106B5F File Offset: 0x00104D5F
		public DependencyObject GetAdjacentElement(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointer.GetAdjacentElement(this._node, this.Edge, direction);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the position indicated by the specified offset, in symbols, from the beginning of the current <see cref="T:System.Windows.Documents.TextPointer" />.</summary>
		/// <param name="offset">An offset, in symbols, for which to calculate and return the position.  If the offset is negative, the position is calculated in the logical direction opposite of that indicated by the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> property.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the position indicated by the specified offset, or<see langword=" null " />if no corresponding position can be found.</returns>
		// Token: 0x060039DD RID: 14813 RVA: 0x00106B8F File Offset: 0x00104D8F
		public TextPointer GetPositionAtOffset(int offset)
		{
			return this.GetPositionAtOffset(offset, this.LogicalDirection);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the position indicated by the specified offset, in symbols, from the beginning of the current <see cref="T:System.Windows.Documents.TextPointer" /> and in the specified direction.</summary>
		/// <param name="offset">An offset, in symbols, for which to calculate and return the position.  If the offset is negative, the returned <see cref="T:System.Windows.Documents.TextPointer" /> precedes the current <see cref="T:System.Windows.Documents.TextPointer" />; otherwise, it follows.</param>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction of the returned <see cref="T:System.Windows.Documents.TextPointer" />.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the position indicated by the specified offset, or<see langword=" null " />if the offset extends past the end of the content.</returns>
		// Token: 0x060039DE RID: 14814 RVA: 0x00106BA0 File Offset: 0x00104DA0
		public TextPointer GetPositionAtOffset(int offset, LogicalDirection direction)
		{
			TextPointer textPointer = new TextPointer(this, direction);
			int num = textPointer.MoveByOffset(offset);
			if (num == offset)
			{
				textPointer.Freeze();
				return textPointer;
			}
			return null;
		}

		/// <summary>Returns a pointer to the next symbol in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to search for the next symbol.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the next symbol in the requested direction, or <see langword="null" /> if the current <see cref="T:System.Windows.Documents.TextPointer" /> borders the start or end of content.</returns>
		// Token: 0x060039DF RID: 14815 RVA: 0x00106BCA File Offset: 0x00104DCA
		public TextPointer GetNextContextPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetNextContextPosition(direction);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the closest insertion position in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to search for the closest insertion position.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the closest insertion position in the specified direction.</returns>
		// Token: 0x060039E0 RID: 14816 RVA: 0x00106BD8 File Offset: 0x00104DD8
		public TextPointer GetInsertionPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetInsertionPosition(direction);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x00106BE6 File Offset: 0x00104DE6
		internal TextPointer GetInsertionPosition()
		{
			return this.GetInsertionPosition(LogicalDirection.Forward);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the next insertion position in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to search for the next insertion position.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that identifies the next insertion position in the requested direction, or <see langword="null" /> if no next insertion position can be found.</returns>
		// Token: 0x060039E2 RID: 14818 RVA: 0x00106BEF File Offset: 0x00104DEF
		public TextPointer GetNextInsertionPosition(LogicalDirection direction)
		{
			return (TextPointer)((ITextPointer)this).GetNextInsertionPosition(direction);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the beginning of a line that is specified relative to the current <see cref="T:System.Windows.Documents.TextPointer" />.</summary>
		/// <param name="count">The number of start-of-line markers to skip when determining the line for which to return the starting position. Negative values specify preceding lines, 0 specifies the current line, and positive values specify following lines.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> pointing to the beginning of the specified line (with the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> set to <see cref="F:System.Windows.Documents.LogicalDirection.Forward" />), or <see langword="null" /> if the specified line is out of range or otherwise cannot be located.</returns>
		// Token: 0x060039E3 RID: 14819 RVA: 0x00106C00 File Offset: 0x00104E00
		public TextPointer GetLineStartPosition(int count)
		{
			int num;
			TextPointer lineStartPosition = this.GetLineStartPosition(count, out num);
			if (num == count)
			{
				return lineStartPosition;
			}
			return null;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> to the beginning of a line that is specified relative to the current <see cref="T:System.Windows.Documents.TextPointer" />, and reports how many lines were skipped.</summary>
		/// <param name="count">The number of start-of-line markers to skip when determining the line for which to return the starting position. Negative values specify preceding lines, 0 specifies the current line, and positive values specify following lines.</param>
		/// <param name="actualCount">When this method returns, contains the actual number of start-of-line markers that were skipped when determining the line for which to return the starting position.  This value may be less than <paramref name="count" /> if the beginning or end of content is encountered before the specified number of lines are skipped. This parameter is passed uninitialized.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> pointing to the beginning of the specified line (with the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> set to <see cref="F:System.Windows.Documents.LogicalDirection.Forward" />), or to the beginning of the line closest to the specified line if the specified line is out of range.</returns>
		// Token: 0x060039E4 RID: 14820 RVA: 0x00106C20 File Offset: 0x00104E20
		public TextPointer GetLineStartPosition(int count, out int actualCount)
		{
			this.ValidateLayout();
			TextPointer textPointer = new TextPointer(this);
			if (this.HasValidLayout)
			{
				actualCount = textPointer.MoveToLineBoundary(count);
			}
			else
			{
				actualCount = 0;
			}
			textPointer.SetLogicalDirection(LogicalDirection.Forward);
			textPointer.Freeze();
			return textPointer;
		}

		/// <summary>Returns a bounding box (<see cref="T:System.Windows.Rect" />) for content that borders the current <see cref="T:System.Windows.Documents.TextPointer" /> in the specified logical direction.</summary>
		/// <param name="direction">One of the <see cref="T:System.Windows.Documents.LogicalDirection" /> values that specifies the logical direction in which to find a content bounding box.</param>
		/// <returns>A bounding box for content that borders the current <see cref="T:System.Windows.Documents.TextPointer" /> in the specified direction, or <see cref="P:System.Windows.Rect.Empty" /> if current, valid layout information is unavailable.</returns>
		// Token: 0x060039E5 RID: 14821 RVA: 0x00106C5F File Offset: 0x00104E5F
		public Rect GetCharacterRect(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			this.ValidateLayout();
			if (!this.HasValidLayout)
			{
				return Rect.Empty;
			}
			return TextPointerBase.GetCharacterRect(this, direction);
		}

		/// <summary>Inserts the specified text into the text <see cref="T:System.Windows.Documents.Run" /> at the current position.</summary>
		/// <param name="textData">The text to insert.</param>
		/// <exception cref="T:System.InvalidOperationException">The current position is not within a <see cref="T:System.Windows.Documents.Run" /> element.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="textData" /> is <see langword="null" />.</exception>
		// Token: 0x060039E6 RID: 14822 RVA: 0x00106C9C File Offset: 0x00104E9C
		public void InsertTextInRun(string textData)
		{
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextPointer position;
			if (TextSchema.IsInTextContent(this))
			{
				position = this;
			}
			else
			{
				position = TextRangeEditTables.EnsureInsertionPosition(this);
			}
			this._tree.BeginChange();
			try
			{
				this._tree.InsertTextInternal(position, textData);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		/// <summary>Deletes the specified number of characters from the position indicated by the current <see cref="T:System.Windows.Documents.TextPointer" />.</summary>
		/// <param name="count">The number of characters to delete, starting at the current position. Specify a positive value to delete characters that follow the current position; specify a negative value to delete characters that precede the current position.</param>
		/// <returns>The number of characters actually deleted.</returns>
		/// <exception cref="T:System.InvalidOperationException">The method is called at a position where text is not allowed.</exception>
		// Token: 0x060039E7 RID: 14823 RVA: 0x00106D14 File Offset: 0x00104F14
		public int DeleteTextInRun(int count)
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (!TextSchema.IsInTextContent(this))
			{
				return 0;
			}
			LogicalDirection direction = (count < 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
			int textRunLength = this.GetTextRunLength(direction);
			if (count > 0 && count > textRunLength)
			{
				count = textRunLength;
			}
			else if (count < 0 && count < -textRunLength)
			{
				count = -textRunLength;
			}
			TextPointer textPointer = new TextPointer(this, count);
			this._tree.BeginChange();
			try
			{
				if (count > 0)
				{
					this._tree.DeleteContentInternal(this, textPointer);
				}
				else if (count < 0)
				{
					this._tree.DeleteContentInternal(textPointer, this);
				}
			}
			finally
			{
				this._tree.EndChange();
			}
			return count;
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x00106DC0 File Offset: 0x00104FC0
		internal void InsertTextElement(TextElement textElement)
		{
			Invariant.Assert(textElement != null);
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			ValidationHelper.ValidateChild(this, textElement, "textElement");
			if (textElement.Parent != null)
			{
				throw new InvalidOperationException(SR.Get("TextPointer_CannotInsertTextElementBecauseItBelongsToAnotherTree"));
			}
			textElement.RepositionWithContent(this);
		}

		/// <summary>Inserts a paragraph break at the current position.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> to the beginning (<see cref="P:System.Windows.Documents.TextElement.ContentStart" />) of the new paragraph.</returns>
		/// <exception cref="T:System.InvalidOperationException">This method is called on a position that cannot be split to accommodate a new paragraph, such as in the scope of a <see cref="T:System.Windows.Documents.Hyperlink" /> or <see cref="T:System.Windows.Documents.InlineUIContainer" />. </exception>
		// Token: 0x060039E9 RID: 14825 RVA: 0x00106E14 File Offset: 0x00105014
		public TextPointer InsertParagraphBreak()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (this.TextContainer.Parent != null)
			{
				Type type = this.TextContainer.Parent.GetType();
				if (!TextSchema.IsValidChildOfContainer(type, typeof(Paragraph)))
				{
					throw new InvalidOperationException(SR.Get("TextSchema_IllegalElement", new object[]
					{
						"Paragraph",
						type
					}));
				}
			}
			Inline nonMergeableInlineAncestor = this.GetNonMergeableInlineAncestor();
			if (nonMergeableInlineAncestor != null)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_CannotSplitElement", new object[]
				{
					nonMergeableInlineAncestor.GetType().Name
				}));
			}
			this._tree.BeginChange();
			TextPointer result;
			try
			{
				result = TextRangeEdit.InsertParagraphBreak(this, true);
			}
			finally
			{
				this._tree.EndChange();
			}
			return result;
		}

		/// <summary>Inserts a line break at the current position.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> positioned immediately after the <see cref="T:System.Windows.Documents.LineBreak" /> element inserted by this method.</returns>
		// Token: 0x060039EA RID: 14826 RVA: 0x00106EE4 File Offset: 0x001050E4
		public TextPointer InsertLineBreak()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			this._tree.BeginChange();
			TextPointer result;
			try
			{
				result = TextRangeEdit.InsertLineBreak(this);
			}
			finally
			{
				this._tree.EndChange();
			}
			return result;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The string that represents the object.</returns>
		// Token: 0x060039EB RID: 14827 RVA: 0x000F661D File Offset: 0x000F481D
		public override string ToString()
		{
			return base.ToString();
		}

		/// <summary>Gets a value that indicates whether the text container associated with the current position has a valid (up-to-date) layout.</summary>
		/// <returns>
		///     <see langword="true" /> if the layout is current and valid; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x00106F34 File Offset: 0x00105134
		public bool HasValidLayout
		{
			get
			{
				return this._tree.TextView != null && this._tree.TextView.IsValid && this._tree.TextView.Contains(this);
			}
		}

		/// <summary>Gets the logical direction associated with the current position which is used to disambiguate content associated with the current position.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.LogicalDirection" /> value that is associated with the current position.</returns>
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x00106F6A File Offset: 0x0010516A
		public LogicalDirection LogicalDirection
		{
			get
			{
				return this.GetGravityInternal();
			}
		}

		/// <summary>Gets the logical parent that scopes the current position.</summary>
		/// <returns>The logical parent that scopes the current position.</returns>
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x00106F72 File Offset: 0x00105172
		public DependencyObject Parent
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.GetLogicalTreeNode();
			}
		}

		/// <summary>Gets a value that indicates whether the current position is an insertion position.</summary>
		/// <returns>
		///     <see langword="true" /> if the current position is an insertion position; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x00106F8B File Offset: 0x0010518B
		public bool IsAtInsertionPosition
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return TextPointerBase.IsAtInsertionPosition(this);
			}
		}

		/// <summary>Gets a value that indicates whether the current position is at the beginning of a line.</summary>
		/// <returns>
		///     <see langword="true" /> if the current position is at the beginning of a line; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x00106FA4 File Offset: 0x001051A4
		public bool IsAtLineStartPosition
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				this.ValidateLayout();
				if (!this.HasValidLayout)
				{
					return false;
				}
				TextSegment lineRange = this._tree.TextView.GetLineRange(this);
				if (!lineRange.IsNull)
				{
					TextPointer textPointer = new TextPointer(this);
					TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					while ((pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd) && TextSchema.IsFormattingType(textPointer.GetAdjacentElement(LogicalDirection.Backward).GetType()))
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					}
					if (textPointer.CompareTo((TextPointer)lineRange.Start) <= 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>Gets the paragraph that scopes the current position, if any.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.Paragraph" /> that scopes the current position, or<see langword=" null " />if no such paragraph exists.</returns>
		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x00107042 File Offset: 0x00105242
		public Paragraph Paragraph
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.ParentBlock as Paragraph;
			}
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x00107060 File Offset: 0x00105260
		internal Block ParagraphOrBlockUIContainer
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				Block parentBlock = this.ParentBlock;
				if (!(parentBlock is Paragraph) && !(parentBlock is BlockUIContainer))
				{
					return null;
				}
				return parentBlock;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> at the beginning of content in the text container associated with the current position.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> at the beginning of content in the text container associated with the current position.</returns>
		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x060039F3 RID: 14835 RVA: 0x00107098 File Offset: 0x00105298
		public TextPointer DocumentStart
		{
			get
			{
				return this.TextContainer.Start;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextPointer" /> at the end of content in the text container associated with the current position.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> at the end of content in the text container associated with the current position.</returns>
		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x001070A5 File Offset: 0x001052A5
		public TextPointer DocumentEnd
		{
			get
			{
				return this.TextContainer.End;
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x001070B4 File Offset: 0x001052B4
		internal Inline GetNonMergeableInlineAncestor()
		{
			Inline inline = this.Parent as Inline;
			while (inline != null && TextSchema.IsMergeableInline(inline.GetType()))
			{
				inline = (inline.Parent as Inline);
			}
			return inline;
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x001070EC File Offset: 0x001052EC
		internal ListItem GetListAncestor()
		{
			TextElement textElement = this.Parent as TextElement;
			while (textElement != null && !(textElement is ListItem))
			{
				textElement = (textElement.Parent as TextElement);
			}
			return textElement as ListItem;
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x00107124 File Offset: 0x00105324
		internal static int GetTextInRun(TextContainer textContainer, int symbolOffset, TextTreeTextNode textNode, int nodeOffset, LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			if (textBuffer == null)
			{
				throw new ArgumentNullException("textBuffer");
			}
			if (startIndex < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"startIndex"
				}));
			}
			if (startIndex > textBuffer.Length)
			{
				throw new ArgumentException(SR.Get("StartIndexExceedsBufferSize", new object[]
				{
					startIndex,
					textBuffer.Length
				}));
			}
			if (count < 0)
			{
				throw new ArgumentException(SR.Get("NegativeValue", new object[]
				{
					"count"
				}));
			}
			if (count > textBuffer.Length - startIndex)
			{
				throw new ArgumentException(SR.Get("MaxLengthExceedsBufferSize", new object[]
				{
					count,
					textBuffer.Length,
					startIndex
				}));
			}
			Invariant.Assert(textNode != null, "textNode is expected to be non-null");
			textContainer.EmptyDeadPositionList();
			int num;
			if (nodeOffset < 0)
			{
				num = 0;
			}
			else
			{
				num = ((direction == LogicalDirection.Forward) ? nodeOffset : (textNode.SymbolCount - nodeOffset));
				symbolOffset += nodeOffset;
			}
			int num2 = 0;
			while (textNode != null)
			{
				num2 += Math.Min(count - num2, textNode.SymbolCount - num);
				num = 0;
				if (num2 == count)
				{
					break;
				}
				textNode = (((direction == LogicalDirection.Forward) ? textNode.GetNextNode() : textNode.GetPreviousNode()) as TextTreeTextNode);
			}
			if (direction == LogicalDirection.Backward)
			{
				symbolOffset -= num2;
			}
			if (num2 > 0)
			{
				TextTreeText.ReadText(textContainer.RootTextBlock, symbolOffset, num2, textBuffer, startIndex);
			}
			return num2;
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00107288 File Offset: 0x00105488
		internal static DependencyObject GetAdjacentElement(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			TextTreeNode adjacentNode = TextPointer.GetAdjacentNode(node, edge, direction);
			DependencyObject result;
			if (adjacentNode is TextTreeObjectNode)
			{
				result = ((TextTreeObjectNode)adjacentNode).EmbeddedElement;
			}
			else if (adjacentNode is TextTreeTextElementNode)
			{
				result = ((TextTreeTextElementNode)adjacentNode).TextElement;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x001072D0 File Offset: 0x001054D0
		internal void MoveToPosition(TextPointer textPosition)
		{
			ValidationHelper.VerifyPosition(this._tree, textPosition);
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			textPosition.SyncToTreeGeneration();
			this.MoveToNode(this._tree, textPosition.Node, textPosition.Edge);
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x00107320 File Offset: 0x00105520
		internal int MoveByOffset(int offset)
		{
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			if (offset != 0)
			{
				int symbolOffset = this.GetSymbolOffset();
				int num = symbolOffset + offset;
				if (num < 1)
				{
					if (offset > 0)
					{
						num = this._tree.InternalSymbolCount - 1;
						offset = num - symbolOffset;
					}
					else
					{
						offset += 1 - num;
						num = 1;
					}
				}
				else if (num > this._tree.InternalSymbolCount - 1)
				{
					offset -= num - (this._tree.InternalSymbolCount - 1);
					num = this._tree.InternalSymbolCount - 1;
				}
				SplayTreeNode splayTreeNode;
				ElementEdge edge;
				this._tree.GetNodeAndEdgeAtOffset(num, out splayTreeNode, out edge);
				this.MoveToNode(this._tree, (TextTreeNode)splayTreeNode, edge);
			}
			return offset;
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x001073D4 File Offset: 0x001055D4
		internal bool MoveToNextContextPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeNode newNode;
			ElementEdge elementEdge;
			bool flag;
			if (direction == LogicalDirection.Forward)
			{
				flag = this.GetNextNodeAndEdge(out newNode, out elementEdge);
			}
			else
			{
				flag = this.GetPreviousNodeAndEdge(out newNode, out elementEdge);
			}
			if (flag)
			{
				this.SetNodeAndEdge(this.AdjustRefCounts(newNode, elementEdge, this._node, this.Edge), elementEdge);
				this.DebugAssertGeneration();
			}
			this.AssertState();
			return flag;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x0010744A File Offset: 0x0010564A
		internal bool MoveToInsertionPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToInsertionPosition(this, direction);
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00107475 File Offset: 0x00105675
		internal bool MoveToNextInsertionPosition(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToNextInsertionPosition(this, direction);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x001074A0 File Offset: 0x001056A0
		internal int MoveToLineBoundary(int count)
		{
			this.VerifyNotFrozen();
			this.ValidateLayout();
			if (!this.HasValidLayout)
			{
				return 0;
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return TextPointerBase.MoveToLineBoundary(this, this._tree.TextView, count);
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x001074DC File Offset: 0x001056DC
		internal void InsertUIElement(UIElement uiElement)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			ValidationHelper.ValidateChild(this, uiElement, "uiElement");
			if (!((TextElement)this.Parent).IsEmpty)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_UIElementNotAllowedInThisPosition"));
			}
			this._tree.BeginChange();
			try
			{
				this._tree.InsertEmbeddedObjectInternal(this, uiElement);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x0010756C File Offset: 0x0010576C
		internal TextElement GetAdjacentElementFromOuterPosition(LogicalDirection direction)
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeTextElementNode adjacentTextElementNodeSibling = this.GetAdjacentTextElementNodeSibling(direction);
			if (adjacentTextElementNodeSibling != null)
			{
				return adjacentTextElementNodeSibling.TextElement;
			}
			return null;
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x001075A0 File Offset: 0x001057A0
		internal void SetLogicalDirection(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			if (this.GetGravityInternal() != direction)
			{
				this.SyncToTreeGeneration();
				SplayTreeNode splayTreeNode = this._node;
				ElementEdge edge = this.Edge;
				ElementEdge elementEdge;
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = this._node.GetPreviousNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.AfterEnd;
						goto IL_110;
					}
					splayTreeNode = this._node.GetContainingNode();
					Invariant.Assert(splayTreeNode != null, "Bad tree state: newNode must be non-null (BeforeStart)");
					elementEdge = ElementEdge.AfterStart;
					goto IL_110;
				case ElementEdge.AfterStart:
					splayTreeNode = this._node.GetFirstContainedNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.BeforeStart;
						goto IL_110;
					}
					splayTreeNode = this._node;
					elementEdge = ElementEdge.BeforeEnd;
					goto IL_110;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					splayTreeNode = this._node.GetLastContainedNode();
					if (splayTreeNode != null)
					{
						elementEdge = ElementEdge.AfterEnd;
						goto IL_110;
					}
					splayTreeNode = this._node;
					elementEdge = ElementEdge.AfterStart;
					goto IL_110;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = this._node.GetNextNode();
						if (splayTreeNode != null)
						{
							elementEdge = ElementEdge.BeforeStart;
							goto IL_110;
						}
						splayTreeNode = this._node.GetContainingNode();
						Invariant.Assert(splayTreeNode != null, "Bad tree state: newNode must be non-null (AfterEnd)");
						elementEdge = ElementEdge.BeforeEnd;
						goto IL_110;
					}
					break;
				}
				Invariant.Assert(false, "Bad ElementEdge value");
				elementEdge = this.Edge;
				IL_110:
				this.SetNodeAndEdge(this.AdjustRefCounts((TextTreeNode)splayTreeNode, elementEdge, this._node, this.Edge), elementEdge);
				Invariant.Assert(this.GetGravityInternal() == direction, "Inconsistent position gravity");
			}
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06003A02 RID: 14850 RVA: 0x001076F0 File Offset: 0x001058F0
		internal bool IsFrozen
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				return (this._flags & 16U) == 16U;
			}
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x0010770A File Offset: 0x0010590A
		internal void Freeze()
		{
			this._tree.EmptyDeadPositionList();
			this.SetIsFrozen();
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x0010771D File Offset: 0x0010591D
		internal TextPointer GetFrozenPointer(LogicalDirection logicalDirection)
		{
			ValidationHelper.VerifyDirection(logicalDirection, "logicalDirection");
			this._tree.EmptyDeadPositionList();
			return (TextPointer)TextPointerBase.GetFrozenPointer(this, logicalDirection);
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00107741 File Offset: 0x00105941
		void ITextPointer.SetLogicalDirection(LogicalDirection direction)
		{
			this.SetLogicalDirection(direction);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x0010774A File Offset: 0x0010594A
		int ITextPointer.CompareTo(ITextPointer position)
		{
			return this.CompareTo((TextPointer)position);
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00107758 File Offset: 0x00105958
		int ITextPointer.CompareTo(StaticTextPointer position)
		{
			int num = this.Offset + 1;
			int internalOffset = this.TextContainer.GetInternalOffset(position);
			int result;
			if (num < internalOffset)
			{
				result = -1;
			}
			else if (num > internalOffset)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x0010778E File Offset: 0x0010598E
		int ITextPointer.GetOffsetToPosition(ITextPointer position)
		{
			return this.GetOffsetToPosition((TextPointer)position);
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x0010779C File Offset: 0x0010599C
		TextPointerContext ITextPointer.GetPointerContext(LogicalDirection direction)
		{
			return this.GetPointerContext(direction);
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x001077A5 File Offset: 0x001059A5
		int ITextPointer.GetTextRunLength(LogicalDirection direction)
		{
			return this.GetTextRunLength(direction);
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000C7E1E File Offset: 0x000C601E
		string ITextPointer.GetTextInRun(LogicalDirection direction)
		{
			return TextPointerBase.GetTextInRun(this, direction);
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x001077AE File Offset: 0x001059AE
		int ITextPointer.GetTextInRun(LogicalDirection direction, char[] textBuffer, int startIndex, int count)
		{
			return this.GetTextInRun(direction, textBuffer, startIndex, count);
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x001077BB File Offset: 0x001059BB
		object ITextPointer.GetAdjacentElement(LogicalDirection direction)
		{
			return this.GetAdjacentElement(direction);
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x001077C4 File Offset: 0x001059C4
		Type ITextPointer.GetElementType(LogicalDirection direction)
		{
			ValidationHelper.VerifyDirection(direction, "direction");
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject element = this.GetElement(direction);
			if (element == null)
			{
				return null;
			}
			return element.GetType();
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00107800 File Offset: 0x00105A00
		bool ITextPointer.HasEqualScope(ITextPointer position)
		{
			this._tree.EmptyDeadPositionList();
			ValidationHelper.VerifyPosition(this._tree, position);
			TextPointer textPointer = (TextPointer)position;
			this.SyncToTreeGeneration();
			textPointer.SyncToTreeGeneration();
			TextTreeNode scopingNode = this.GetScopingNode();
			TextTreeNode scopingNode2 = textPointer.GetScopingNode();
			return scopingNode == scopingNode2;
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x0010784C File Offset: 0x00105A4C
		ITextPointer ITextPointer.GetNextContextPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextContextPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00107874 File Offset: 0x00105A74
		ITextPointer ITextPointer.GetInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			textPointer.MoveToInsertionPosition(direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00107898 File Offset: 0x00105A98
		ITextPointer ITextPointer.GetFormatNormalizedPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			TextPointerBase.MoveToFormatNormalizedPosition(textPointer, direction);
			textPointer.Freeze();
			return textPointer;
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x001078BC File Offset: 0x00105ABC
		ITextPointer ITextPointer.GetNextInsertionPosition(LogicalDirection direction)
		{
			ITextPointer textPointer = ((ITextPointer)this).CreatePointer();
			if (textPointer.MoveToNextInsertionPosition(direction))
			{
				textPointer.Freeze();
			}
			else
			{
				textPointer = null;
			}
			return textPointer;
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001078E4 File Offset: 0x00105AE4
		object ITextPointer.GetValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject dependencyParent = this.GetDependencyParent();
			object result;
			if (dependencyParent == null)
			{
				result = DependencyProperty.UnsetValue;
			}
			else
			{
				result = dependencyParent.GetValue(formattingProperty);
			}
			return result;
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x0010792C File Offset: 0x00105B2C
		object ITextPointer.ReadLocalValue(DependencyProperty formattingProperty)
		{
			if (formattingProperty == null)
			{
				throw new ArgumentNullException("formattingProperty");
			}
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextElement textElement = this.Parent as TextElement;
			if (textElement == null)
			{
				throw new InvalidOperationException(SR.Get("NoScopingElement", new object[]
				{
					"This TextPointer"
				}));
			}
			return textElement.ReadLocalValue(formattingProperty);
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0010798C File Offset: 0x00105B8C
		LocalValueEnumerator ITextPointer.GetLocalValueEnumerator()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			DependencyObject dependencyObject = this.Parent as TextElement;
			if (dependencyObject == null)
			{
				return new DependencyObject().GetLocalValueEnumerator();
			}
			return dependencyObject.GetLocalValueEnumerator();
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001079CA File Offset: 0x00105BCA
		ITextPointer ITextPointer.CreatePointer()
		{
			return ((ITextPointer)this).CreatePointer(0, this.LogicalDirection);
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x001079D9 File Offset: 0x00105BD9
		StaticTextPointer ITextPointer.CreateStaticPointer()
		{
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			return new StaticTextPointer(this._tree, this._node, this._node.GetOffsetFromEdge(this.Edge));
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00107A0E File Offset: 0x00105C0E
		ITextPointer ITextPointer.CreatePointer(int offset)
		{
			return ((ITextPointer)this).CreatePointer(offset, this.LogicalDirection);
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x000D32B8 File Offset: 0x000D14B8
		ITextPointer ITextPointer.CreatePointer(LogicalDirection gravity)
		{
			return ((ITextPointer)this).CreatePointer(0, gravity);
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00107A1D File Offset: 0x00105C1D
		ITextPointer ITextPointer.CreatePointer(int offset, LogicalDirection gravity)
		{
			return new TextPointer(this, offset, gravity);
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00107A27 File Offset: 0x00105C27
		void ITextPointer.Freeze()
		{
			this.Freeze();
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00107A2F File Offset: 0x00105C2F
		ITextPointer ITextPointer.GetFrozenPointer(LogicalDirection logicalDirection)
		{
			return this.GetFrozenPointer(logicalDirection);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00107A38 File Offset: 0x00105C38
		bool ITextPointer.MoveToNextContextPosition(LogicalDirection direction)
		{
			return this.MoveToNextContextPosition(direction);
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00107A41 File Offset: 0x00105C41
		int ITextPointer.MoveByOffset(int offset)
		{
			return this.MoveByOffset(offset);
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00107A4A File Offset: 0x00105C4A
		void ITextPointer.MoveToPosition(ITextPointer position)
		{
			this.MoveToPosition((TextPointer)position);
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00107A58 File Offset: 0x00105C58
		void ITextPointer.MoveToElementEdge(ElementEdge edge)
		{
			this.MoveToElementEdge(edge);
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00107A64 File Offset: 0x00105C64
		internal void MoveToElementEdge(ElementEdge edge)
		{
			ValidationHelper.VerifyElementEdge(edge, "edge");
			this.VerifyNotFrozen();
			this._tree.EmptyDeadPositionList();
			this.SyncToTreeGeneration();
			TextTreeNode scopingNode = this.GetScopingNode();
			TextTreeTextElementNode textTreeTextElementNode = scopingNode as TextTreeTextElementNode;
			if (textTreeTextElementNode != null)
			{
				this.MoveToNode(this._tree, textTreeTextElementNode, edge);
				return;
			}
			if (scopingNode is TextTreeRootNode)
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("NoScopingElement", new object[]
			{
				"This TextNavigator"
			}));
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00107AD9 File Offset: 0x00105CD9
		int ITextPointer.MoveToLineBoundary(int count)
		{
			return this.MoveToLineBoundary(count);
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00107AE2 File Offset: 0x00105CE2
		Rect ITextPointer.GetCharacterRect(LogicalDirection direction)
		{
			return this.GetCharacterRect(direction);
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00107AEB File Offset: 0x00105CEB
		bool ITextPointer.MoveToInsertionPosition(LogicalDirection direction)
		{
			return this.MoveToInsertionPosition(direction);
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00107AF4 File Offset: 0x00105CF4
		bool ITextPointer.MoveToNextInsertionPosition(LogicalDirection direction)
		{
			return this.MoveToNextInsertionPosition(direction);
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00107AFD File Offset: 0x00105CFD
		void ITextPointer.InsertTextInRun(string textData)
		{
			this.InsertTextInRun(textData);
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x00107B08 File Offset: 0x00105D08
		void ITextPointer.DeleteContentToPosition(ITextPointer limit)
		{
			this._tree.BeginChange();
			try
			{
				TextRangeEditTables.DeleteContent(this, (TextPointer)limit);
			}
			finally
			{
				this._tree.EndChange();
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00107B4C File Offset: 0x00105D4C
		bool ITextPointer.ValidateLayout()
		{
			return this.ValidateLayout();
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x00107B54 File Offset: 0x00105D54
		internal bool ValidateLayout()
		{
			return TextPointerBase.ValidateLayout(this, this._tree.TextView);
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00107B67 File Offset: 0x00105D67
		internal TextTreeTextNode GetAdjacentTextNodeSibling(LogicalDirection direction)
		{
			return this.GetAdjacentSiblingNode(direction) as TextTreeTextNode;
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x00107B75 File Offset: 0x00105D75
		internal static TextTreeTextNode GetAdjacentTextNodeSibling(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			return TextPointer.GetAdjacentSiblingNode(node, edge, direction) as TextTreeTextNode;
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x00107B84 File Offset: 0x00105D84
		internal TextTreeTextElementNode GetAdjacentTextElementNodeSibling(LogicalDirection direction)
		{
			return this.GetAdjacentSiblingNode(direction) as TextTreeTextElementNode;
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x00107B92 File Offset: 0x00105D92
		internal TextTreeTextElementNode GetAdjacentTextElementNode(LogicalDirection direction)
		{
			return this.GetAdjacentNode(direction) as TextTreeTextElementNode;
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x00107BA0 File Offset: 0x00105DA0
		internal TextTreeNode GetAdjacentSiblingNode(LogicalDirection direction)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetAdjacentSiblingNode(this._node, this.Edge, direction);
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x00107BBC File Offset: 0x00105DBC
		internal static TextTreeNode GetAdjacentSiblingNode(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			SplayTreeNode splayTreeNode;
			if (direction == LogicalDirection.Forward)
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = node;
					goto IL_72;
				case ElementEdge.AfterStart:
					splayTreeNode = node.GetFirstContainedNode();
					goto IL_72;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				case ElementEdge.BeforeEnd:
					break;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = node.GetNextNode();
						goto IL_72;
					}
					break;
				}
				splayTreeNode = null;
			}
			else
			{
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = node.GetPreviousNode();
					goto IL_72;
				case ElementEdge.AfterStart:
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					break;
				case ElementEdge.BeforeEnd:
					splayTreeNode = node.GetLastContainedNode();
					goto IL_72;
				default:
					if (edge == ElementEdge.AfterEnd)
					{
						splayTreeNode = node;
						goto IL_72;
					}
					break;
				}
				splayTreeNode = null;
			}
			IL_72:
			return (TextTreeNode)splayTreeNode;
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x00107C41 File Offset: 0x00105E41
		internal int GetSymbolOffset()
		{
			this.DebugAssertGeneration();
			return TextPointer.GetSymbolOffset(this._tree, this._node, this.Edge);
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x00107C60 File Offset: 0x00105E60
		internal static int GetSymbolOffset(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return node.GetSymbolOffset(tree.Generation);
			case ElementEdge.AfterStart:
				return node.GetSymbolOffset(tree.Generation) + 1;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				return node.GetSymbolOffset(tree.Generation) + node.SymbolCount - 1;
			default:
				if (edge == ElementEdge.AfterEnd)
				{
					return node.GetSymbolOffset(tree.Generation) + node.SymbolCount;
				}
				break;
			}
			Invariant.Assert(false, "Unknown value for position edge");
			return 0;
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x00107CE7 File Offset: 0x00105EE7
		internal DependencyObject GetLogicalTreeNode()
		{
			this.DebugAssertGeneration();
			return this.GetScopingNode().GetLogicalTreeNode();
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x00107CFC File Offset: 0x00105EFC
		internal void SyncToTreeGeneration()
		{
			TextTreeFixupNode textTreeFixupNode = null;
			if (this._generation == this._tree.PositionGeneration)
			{
				return;
			}
			this.IsCaretUnitBoundaryCacheValid = false;
			SplayTreeNode splayTreeNode = this._node;
			ElementEdge elementEdge = this.Edge;
			for (;;)
			{
				SplayTreeNode splayTreeNode2 = splayTreeNode;
				SplayTreeNode splayTreeNode3 = splayTreeNode;
				SplayTreeNode parentNode;
				for (;;)
				{
					parentNode = splayTreeNode2.ParentNode;
					if (parentNode == null)
					{
						break;
					}
					textTreeFixupNode = (parentNode as TextTreeFixupNode);
					if (textTreeFixupNode != null)
					{
						break;
					}
					if (splayTreeNode2.Role == SplayTreeNodeRole.LocalRoot)
					{
						splayTreeNode3.Splay();
						splayTreeNode3 = parentNode;
					}
					splayTreeNode2 = parentNode;
				}
				if (parentNode == null)
				{
					break;
				}
				if (this.GetGravityInternal() == LogicalDirection.Forward)
				{
					if (elementEdge == ElementEdge.BeforeStart && textTreeFixupNode.FirstContainedNode != null)
					{
						splayTreeNode = textTreeFixupNode.FirstContainedNode;
						Invariant.Assert(elementEdge == ElementEdge.BeforeStart, "edge BeforeStart is expected");
					}
					else
					{
						splayTreeNode = textTreeFixupNode.NextNode;
						elementEdge = textTreeFixupNode.NextEdge;
					}
				}
				else if (elementEdge == ElementEdge.AfterEnd && textTreeFixupNode.LastContainedNode != null)
				{
					splayTreeNode = textTreeFixupNode.LastContainedNode;
					Invariant.Assert(elementEdge == ElementEdge.AfterEnd, "edge AfterEnd is expected");
				}
				else
				{
					splayTreeNode = textTreeFixupNode.PreviousNode;
					elementEdge = textTreeFixupNode.PreviousEdge;
				}
			}
			this.SetNodeAndEdge((TextTreeNode)splayTreeNode, elementEdge);
			this._generation = this._tree.PositionGeneration;
			this.AssertState();
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x00107E12 File Offset: 0x00106012
		internal TextTreeNode GetScopingNode()
		{
			return TextPointer.GetScopingNode(this._node, this.Edge);
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x00107E28 File Offset: 0x00106028
		internal static TextTreeNode GetScopingNode(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				break;
			case ElementEdge.AfterStart:
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
			case ElementEdge.BeforeEnd:
				goto IL_2A;
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_2A;
				}
				break;
			}
			return (TextTreeNode)node.GetContainingNode();
			IL_2A:
			return node;
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x00107E62 File Offset: 0x00106062
		internal void DebugAssertGeneration()
		{
			Invariant.Assert(this._generation == this._tree.PositionGeneration, "TextPointer not synchronized to tree generation!");
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x00107E81 File Offset: 0x00106081
		internal bool GetNextNodeAndEdge(out TextTreeNode node, out ElementEdge edge)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetNextNodeAndEdge(this._node, this.Edge, this._tree.PlainTextOnly, out node, out edge);
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00107EA8 File Offset: 0x001060A8
		internal static bool GetNextNodeAndEdge(TextTreeNode sourceNode, ElementEdge sourceEdge, bool plainTextOnly, out TextTreeNode node, out ElementEdge edge)
		{
			node = sourceNode;
			edge = sourceEdge;
			SplayTreeNode splayTreeNode = node;
			SplayTreeNode splayTreeNode2 = node;
			for (;;)
			{
				bool flag = false;
				bool flag2 = false;
				ElementEdge elementEdge = edge;
				switch (elementEdge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = splayTreeNode2.GetFirstContainedNode();
					if (splayTreeNode == null)
					{
						if (!(splayTreeNode2 is TextTreeTextElementNode))
						{
							flag = (splayTreeNode2 is TextTreeTextNode);
							edge = ElementEdge.BeforeEnd;
							goto IL_D6;
						}
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.BeforeEnd;
					}
					break;
				case ElementEdge.AfterStart:
					splayTreeNode = splayTreeNode2.GetFirstContainedNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.AfterStart;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (splayTreeNode.GetNextNode() is TextTreeTextNode);
							edge = ElementEdge.AfterEnd;
						}
					}
					else if (splayTreeNode2 is TextTreeTextElementNode)
					{
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.AfterEnd;
					}
					else
					{
						Invariant.Assert(splayTreeNode2 is TextTreeRootNode, "currentNode is expected to be TextTreeRootNode");
					}
					break;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_144;
				case ElementEdge.BeforeEnd:
					goto IL_D6;
				default:
				{
					if (elementEdge != ElementEdge.AfterEnd)
					{
						goto IL_144;
					}
					SplayTreeNode nextNode = splayTreeNode2.GetNextNode();
					flag = (nextNode is TextTreeTextNode);
					splayTreeNode = nextNode;
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.AfterStart;
						}
						else
						{
							flag2 = (splayTreeNode.GetNextNode() is TextTreeTextNode);
						}
					}
					else
					{
						SplayTreeNode containingNode = splayTreeNode2.GetContainingNode();
						if (!(containingNode is TextTreeRootNode))
						{
							splayTreeNode = containingNode;
						}
					}
					break;
				}
				}
				IL_14F:
				splayTreeNode2 = splayTreeNode;
				if (flag && flag2 && plainTextOnly)
				{
					break;
				}
				if (!flag || !flag2)
				{
					goto IL_1A2;
				}
				continue;
				IL_D6:
				splayTreeNode = splayTreeNode2.GetNextNode();
				if (splayTreeNode != null)
				{
					flag2 = (splayTreeNode is TextTreeTextNode);
					edge = ElementEdge.BeforeStart;
					goto IL_14F;
				}
				splayTreeNode = splayTreeNode2.GetContainingNode();
				goto IL_14F;
				IL_144:
				Invariant.Assert(false, "Unknown ElementEdge value");
				goto IL_14F;
			}
			splayTreeNode = splayTreeNode.GetContainingNode();
			Invariant.Assert(splayTreeNode is TextTreeRootNode);
			if (edge == ElementEdge.BeforeStart)
			{
				edge = ElementEdge.BeforeEnd;
			}
			else
			{
				splayTreeNode = splayTreeNode.GetLastContainedNode();
				Invariant.Assert(splayTreeNode != null);
				Invariant.Assert(edge == ElementEdge.AfterEnd);
			}
			IL_1A2:
			if (splayTreeNode != null)
			{
				node = (TextTreeNode)splayTreeNode;
			}
			return splayTreeNode != null;
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00108066 File Offset: 0x00106266
		internal bool GetPreviousNodeAndEdge(out TextTreeNode node, out ElementEdge edge)
		{
			this.DebugAssertGeneration();
			return TextPointer.GetPreviousNodeAndEdge(this._node, this.Edge, this._tree.PlainTextOnly, out node, out edge);
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x0010808C File Offset: 0x0010628C
		internal static bool GetPreviousNodeAndEdge(TextTreeNode sourceNode, ElementEdge sourceEdge, bool plainTextOnly, out TextTreeNode node, out ElementEdge edge)
		{
			node = sourceNode;
			edge = sourceEdge;
			SplayTreeNode splayTreeNode = node;
			SplayTreeNode splayTreeNode2 = node;
			for (;;)
			{
				bool flag = false;
				bool flag2 = false;
				ElementEdge elementEdge = edge;
				switch (elementEdge)
				{
				case ElementEdge.BeforeStart:
					splayTreeNode = splayTreeNode2.GetPreviousNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.BeforeEnd;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (flag && splayTreeNode.GetPreviousNode() is TextTreeTextNode);
						}
					}
					else
					{
						SplayTreeNode containingNode = splayTreeNode2.GetContainingNode();
						if (!(containingNode is TextTreeRootNode))
						{
							splayTreeNode = containingNode;
						}
					}
					break;
				case ElementEdge.AfterStart:
					goto IL_96;
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_153;
				case ElementEdge.BeforeEnd:
					splayTreeNode = splayTreeNode2.GetLastContainedNode();
					if (splayTreeNode != null)
					{
						if (splayTreeNode is TextTreeTextElementNode)
						{
							edge = ElementEdge.BeforeEnd;
						}
						else
						{
							flag = (splayTreeNode is TextTreeTextNode);
							flag2 = (flag && splayTreeNode.GetPreviousNode() is TextTreeTextNode);
							edge = ElementEdge.BeforeStart;
						}
					}
					else if (splayTreeNode2 is TextTreeTextElementNode)
					{
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.BeforeStart;
					}
					else
					{
						Invariant.Assert(splayTreeNode2 is TextTreeRootNode, "currentNode is expected to be a TextTreeRootNode");
					}
					break;
				default:
					if (elementEdge != ElementEdge.AfterEnd)
					{
						goto IL_153;
					}
					splayTreeNode = splayTreeNode2.GetLastContainedNode();
					if (splayTreeNode == null)
					{
						if (!(splayTreeNode2 is TextTreeTextElementNode))
						{
							flag = (splayTreeNode2 is TextTreeTextNode);
							edge = ElementEdge.AfterStart;
							goto IL_96;
						}
						splayTreeNode = splayTreeNode2;
						edge = ElementEdge.AfterStart;
					}
					break;
				}
				IL_15E:
				splayTreeNode2 = splayTreeNode;
				if (flag && flag2 && plainTextOnly)
				{
					break;
				}
				if (!flag || !flag2)
				{
					goto IL_1AF;
				}
				continue;
				IL_96:
				splayTreeNode = splayTreeNode2.GetPreviousNode();
				if (splayTreeNode != null)
				{
					flag2 = (splayTreeNode is TextTreeTextNode);
					edge = ElementEdge.AfterEnd;
					goto IL_15E;
				}
				splayTreeNode = splayTreeNode2.GetContainingNode();
				goto IL_15E;
				IL_153:
				Invariant.Assert(false, "Unknown ElementEdge value");
				goto IL_15E;
			}
			splayTreeNode = splayTreeNode.GetContainingNode();
			Invariant.Assert(splayTreeNode is TextTreeRootNode);
			if (edge == ElementEdge.AfterEnd)
			{
				edge = ElementEdge.AfterStart;
			}
			else
			{
				splayTreeNode = splayTreeNode.GetFirstContainedNode();
				Invariant.Assert(splayTreeNode != null);
				Invariant.Assert(edge == ElementEdge.BeforeStart);
			}
			IL_1AF:
			if (splayTreeNode != null)
			{
				node = (TextTreeNode)splayTreeNode;
			}
			return splayTreeNode != null;
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x00108258 File Offset: 0x00106458
		internal static TextPointerContext GetPointerContextForward(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
				return node.GetPointerContext(LogicalDirection.Forward);
			case ElementEdge.AfterStart:
				if (node.ContainedNode != null)
				{
					TextTreeNode textTreeNode = (TextTreeNode)node.GetFirstContainedNode();
					return textTreeNode.GetPointerContext(LogicalDirection.Forward);
				}
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				goto IL_B9;
			case ElementEdge.BeforeEnd:
				break;
			default:
			{
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_B9;
				}
				TextTreeNode textTreeNode2 = (TextTreeNode)node.GetNextNode();
				if (textTreeNode2 != null)
				{
					return textTreeNode2.GetPointerContext(LogicalDirection.Forward);
				}
				Invariant.Assert(node.GetContainingNode() != null, "Bad position!");
				return (node.GetContainingNode() is TextTreeRootNode) ? TextPointerContext.None : TextPointerContext.ElementEnd;
			}
			}
			Invariant.Assert(node.ParentNode != null || node is TextTreeRootNode, "Inconsistent node.ParentNode");
			return (node.ParentNode != null) ? TextPointerContext.ElementEnd : TextPointerContext.None;
			IL_B9:
			Invariant.Assert(false, "Unreachable code.");
			return TextPointerContext.Text;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x0010832C File Offset: 0x0010652C
		internal static TextPointerContext GetPointerContextBackward(TextTreeNode node, ElementEdge edge)
		{
			switch (edge)
			{
			case ElementEdge.BeforeStart:
			{
				TextTreeNode textTreeNode = (TextTreeNode)node.GetPreviousNode();
				if (textTreeNode != null)
				{
					return textTreeNode.GetPointerContext(LogicalDirection.Backward);
				}
				Invariant.Assert(node.GetContainingNode() != null, "Bad position!");
				return (node.GetContainingNode() is TextTreeRootNode) ? TextPointerContext.None : TextPointerContext.ElementStart;
			}
			case ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				goto IL_B7;
			case ElementEdge.BeforeEnd:
			{
				TextTreeNode textTreeNode2 = (TextTreeNode)node.GetLastContainedNode();
				if (textTreeNode2 != null)
				{
					return textTreeNode2.GetPointerContext(LogicalDirection.Backward);
				}
				break;
			}
			default:
				if (edge != ElementEdge.AfterEnd)
				{
					goto IL_B7;
				}
				return node.GetPointerContext(LogicalDirection.Backward);
			}
			Invariant.Assert(node.ParentNode != null || node is TextTreeRootNode, "Inconsistent node.ParentNode");
			return (node.ParentNode != null) ? TextPointerContext.ElementStart : TextPointerContext.None;
			IL_B7:
			Invariant.Assert(false, "Unknown ElementEdge value");
			return TextPointerContext.Text;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x00108400 File Offset: 0x00106600
		internal void InsertInline(Inline inline)
		{
			TextPointer textPointer = this;
			if (!TextSchema.ValidateChild(textPointer, inline.GetType(), false, true))
			{
				if (textPointer.Parent == null)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_CannotInsertContentInThisPosition"));
				}
				textPointer = TextRangeEditTables.EnsureInsertionPosition(this);
				Invariant.Assert(textPointer.Parent is Run, "EnsureInsertionPosition() must return a position in text content");
				Run run = (Run)textPointer.Parent;
				if (run.IsEmpty)
				{
					run.RepositionWithContent(null);
				}
				else
				{
					textPointer = TextRangeEdit.SplitFormattingElement(textPointer, false);
				}
				Invariant.Assert(TextSchema.IsValidChild(textPointer, inline.GetType()));
			}
			inline.RepositionWithContent(textPointer);
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00108498 File Offset: 0x00106698
		internal static DependencyObject GetCommonAncestor(TextPointer position1, TextPointer position2)
		{
			TextElement textElement = position1.Parent as TextElement;
			TextElement textElement2 = position2.Parent as TextElement;
			DependencyObject result;
			if (textElement == null)
			{
				result = position1.Parent;
			}
			else if (textElement2 == null)
			{
				result = position2.Parent;
			}
			else
			{
				result = TextElement.GetCommonAncestor(textElement, textElement2);
			}
			return result;
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06003A40 RID: 14912 RVA: 0x001084E0 File Offset: 0x001066E0
		Type ITextPointer.ParentType
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					return null;
				}
				return parent.GetType();
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06003A41 RID: 14913 RVA: 0x00108510 File Offset: 0x00106710
		ITextContainer ITextPointer.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x00108518 File Offset: 0x00106718
		bool ITextPointer.HasValidLayout
		{
			get
			{
				return this.HasValidLayout;
			}
		}

		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06003A43 RID: 14915 RVA: 0x00108520 File Offset: 0x00106720
		bool ITextPointer.IsAtCaretUnitBoundary
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				this.ValidateLayout();
				if (!this.HasValidLayout)
				{
					return false;
				}
				if (this._layoutGeneration != this._tree.LayoutGeneration)
				{
					this.IsCaretUnitBoundaryCacheValid = false;
				}
				if (!this.IsCaretUnitBoundaryCacheValid)
				{
					this.CaretUnitBoundaryCache = this._tree.IsAtCaretUnitBoundary(this);
					this._layoutGeneration = this._tree.LayoutGeneration;
					this.IsCaretUnitBoundaryCacheValid = true;
				}
				return this.CaretUnitBoundaryCache;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x001085A1 File Offset: 0x001067A1
		LogicalDirection ITextPointer.LogicalDirection
		{
			get
			{
				return this.LogicalDirection;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06003A45 RID: 14917 RVA: 0x001085A9 File Offset: 0x001067A9
		bool ITextPointer.IsAtInsertionPosition
		{
			get
			{
				return this.IsAtInsertionPosition;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x06003A46 RID: 14918 RVA: 0x001085B1 File Offset: 0x001067B1
		bool ITextPointer.IsFrozen
		{
			get
			{
				return this.IsFrozen;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x06003A47 RID: 14919 RVA: 0x001085B9 File Offset: 0x001067B9
		int ITextPointer.Offset
		{
			get
			{
				return this.Offset;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x06003A48 RID: 14920 RVA: 0x001085C1 File Offset: 0x001067C1
		internal int Offset
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				return this.GetSymbolOffset() - 1;
			}
		}

		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06003A49 RID: 14921 RVA: 0x001085DC File Offset: 0x001067DC
		int ITextPointer.CharOffset
		{
			get
			{
				return this.CharOffset;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06003A4A RID: 14922 RVA: 0x001085E4 File Offset: 0x001067E4
		internal int CharOffset
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				ElementEdge edge = this.Edge;
				int num;
				switch (edge)
				{
				case ElementEdge.BeforeStart:
					return this._node.GetIMECharOffset();
				case ElementEdge.AfterStart:
				{
					num = this._node.GetIMECharOffset();
					TextTreeTextElementNode textTreeTextElementNode = this._node as TextTreeTextElementNode;
					if (textTreeTextElementNode != null)
					{
						return num + textTreeTextElementNode.IMELeftEdgeCharCount;
					}
					return num;
				}
				case ElementEdge.BeforeStart | ElementEdge.AfterStart:
					goto IL_84;
				case ElementEdge.BeforeEnd:
					break;
				default:
					if (edge != ElementEdge.AfterEnd)
					{
						goto IL_84;
					}
					break;
				}
				return this._node.GetIMECharOffset() + this._node.IMECharCount;
				IL_84:
				Invariant.Assert(false, "Unknown value for position edge");
				num = 0;
				return num;
			}
		}

		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06003A4B RID: 14923 RVA: 0x00108683 File Offset: 0x00106883
		internal TextContainer TextContainer
		{
			get
			{
				return this._tree;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x06003A4C RID: 14924 RVA: 0x0010868B File Offset: 0x0010688B
		internal FrameworkElement ContainingFrameworkElement
		{
			get
			{
				return (FrameworkElement)this._tree.Parent;
			}
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x0010869D File Offset: 0x0010689D
		internal bool IsAtRowEnd
		{
			get
			{
				return TextPointerBase.IsAtRowEnd(this);
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x001086A8 File Offset: 0x001068A8
		internal bool HasNonMergeableInlineAncestor
		{
			get
			{
				Inline nonMergeableInlineAncestor = this.GetNonMergeableInlineAncestor();
				return nonMergeableInlineAncestor != null;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x001086C0 File Offset: 0x001068C0
		internal bool IsAtNonMergeableInlineStart
		{
			get
			{
				return TextPointerBase.IsAtNonMergeableInlineStart(this);
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06003A50 RID: 14928 RVA: 0x001086C8 File Offset: 0x001068C8
		internal TextTreeNode Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x001086D0 File Offset: 0x001068D0
		internal ElementEdge Edge
		{
			get
			{
				return (ElementEdge)(this._flags & 15U);
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x001086DC File Offset: 0x001068DC
		internal Block ParentBlock
		{
			get
			{
				this._tree.EmptyDeadPositionList();
				this.SyncToTreeGeneration();
				DependencyObject parent = this.Parent;
				while (parent is Inline && !(parent is AnchoredBlock))
				{
					parent = ((Inline)parent).Parent;
				}
				return parent as Block;
			}
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00108728 File Offset: 0x00106928
		private void InitializeOffset(TextPointer position, int distance, LogicalDirection direction)
		{
			position.SyncToTreeGeneration();
			SplayTreeNode node;
			ElementEdge edge;
			bool isCaretUnitBoundaryCacheValid;
			if (distance != 0)
			{
				int num = position.GetSymbolOffset() + distance;
				if (num < 1 || num > position.TextContainer.InternalSymbolCount - 1)
				{
					throw new ArgumentException(SR.Get("BadDistance"));
				}
				position.TextContainer.GetNodeAndEdgeAtOffset(num, out node, out edge);
				isCaretUnitBoundaryCacheValid = false;
			}
			else
			{
				node = position.Node;
				edge = position.Edge;
				isCaretUnitBoundaryCacheValid = position.IsCaretUnitBoundaryCacheValid;
			}
			this.Initialize(position.TextContainer, (TextTreeNode)node, edge, direction, position.TextContainer.PositionGeneration, position.CaretUnitBoundaryCache, isCaretUnitBoundaryCacheValid, position._layoutGeneration);
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x001087C0 File Offset: 0x001069C0
		private void Initialize(TextContainer tree, TextTreeNode node, ElementEdge edge, LogicalDirection gravity, uint generation, bool caretUnitBoundaryCache, bool isCaretUnitBoundaryCacheValid, uint layoutGeneration)
		{
			this._tree = tree;
			TextPointer.RepositionForGravity(ref node, ref edge, gravity);
			this.SetNodeAndEdge(node.IncrementReferenceCount(edge), edge);
			this._generation = generation;
			this.CaretUnitBoundaryCache = caretUnitBoundaryCache;
			this.IsCaretUnitBoundaryCacheValid = isCaretUnitBoundaryCacheValid;
			this._layoutGeneration = layoutGeneration;
			this.VerifyFlags();
			tree.AssertTree();
			this.AssertState();
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x0010881F File Offset: 0x00106A1F
		private void VerifyNotFrozen()
		{
			if (this.IsFrozen)
			{
				throw new InvalidOperationException(SR.Get("TextPositionIsFrozen"));
			}
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x0010883C File Offset: 0x00106A3C
		private TextTreeNode AdjustRefCounts(TextTreeNode newNode, ElementEdge newNodeEdge, TextTreeNode oldNode, ElementEdge oldNodeEdge)
		{
			Invariant.Assert(oldNode.ParentNode == null || oldNode.IsChildOfNode(oldNode.ParentNode), "Trying to add ref a dead node!");
			Invariant.Assert(newNode.ParentNode == null || newNode.IsChildOfNode(newNode.ParentNode), "Trying to add ref a dead node!");
			TextTreeNode result = newNode;
			if (newNode != oldNode || newNodeEdge != oldNodeEdge)
			{
				result = newNode.IncrementReferenceCount(newNodeEdge);
				oldNode.DecrementReferenceCount(oldNodeEdge);
			}
			return result;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x001088A8 File Offset: 0x00106AA8
		private static void RepositionForGravity(ref TextTreeNode node, ref ElementEdge edge, LogicalDirection gravity)
		{
			SplayTreeNode splayTreeNode = node;
			ElementEdge elementEdge = edge;
			ElementEdge elementEdge2 = edge;
			switch (elementEdge2)
			{
			case ElementEdge.BeforeStart:
				if (gravity == LogicalDirection.Backward)
				{
					splayTreeNode = node.GetPreviousNode();
					elementEdge = ElementEdge.AfterEnd;
					if (splayTreeNode == null)
					{
						splayTreeNode = node.GetContainingNode();
						elementEdge = ElementEdge.AfterStart;
					}
				}
				break;
			case ElementEdge.AfterStart:
				if (gravity == LogicalDirection.Forward)
				{
					splayTreeNode = node.GetFirstContainedNode();
					elementEdge = ElementEdge.BeforeStart;
					if (splayTreeNode == null)
					{
						splayTreeNode = node;
						elementEdge = ElementEdge.BeforeEnd;
					}
				}
				break;
			case ElementEdge.BeforeStart | ElementEdge.AfterStart:
				break;
			case ElementEdge.BeforeEnd:
				if (gravity == LogicalDirection.Backward)
				{
					splayTreeNode = node.GetLastContainedNode();
					elementEdge = ElementEdge.AfterEnd;
					if (splayTreeNode == null)
					{
						splayTreeNode = node;
						elementEdge = ElementEdge.AfterStart;
					}
				}
				break;
			default:
				if (elementEdge2 == ElementEdge.AfterEnd)
				{
					if (gravity == LogicalDirection.Forward)
					{
						splayTreeNode = node.GetNextNode();
						elementEdge = ElementEdge.BeforeStart;
						if (splayTreeNode == null)
						{
							splayTreeNode = node.GetContainingNode();
							elementEdge = ElementEdge.BeforeEnd;
						}
					}
				}
				break;
			}
			node = (TextTreeNode)splayTreeNode;
			edge = elementEdge;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x0010894D File Offset: 0x00106B4D
		private LogicalDirection GetGravityInternal()
		{
			if (this.Edge != ElementEdge.BeforeStart && this.Edge != ElementEdge.BeforeEnd)
			{
				return LogicalDirection.Backward;
			}
			return LogicalDirection.Forward;
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00108964 File Offset: 0x00106B64
		private DependencyObject GetDependencyParent()
		{
			this.DebugAssertGeneration();
			return this.GetScopingNode().GetDependencyParent();
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00108977 File Offset: 0x00106B77
		internal TextTreeNode GetAdjacentNode(LogicalDirection direction)
		{
			return TextPointer.GetAdjacentNode(this._node, this.Edge, direction);
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x0010898C File Offset: 0x00106B8C
		internal static TextTreeNode GetAdjacentNode(TextTreeNode node, ElementEdge edge, LogicalDirection direction)
		{
			TextTreeNode textTreeNode = TextPointer.GetAdjacentSiblingNode(node, edge, direction);
			if (textTreeNode == null)
			{
				if (edge == ElementEdge.AfterStart || edge == ElementEdge.BeforeEnd)
				{
					textTreeNode = node;
				}
				else
				{
					textTreeNode = (TextTreeNode)node.GetContainingNode();
				}
			}
			return textTreeNode;
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x001089BE File Offset: 0x00106BBE
		private void MoveToNode(TextContainer tree, TextTreeNode node, ElementEdge edge)
		{
			TextPointer.RepositionForGravity(ref node, ref edge, this.GetGravityInternal());
			this._tree = tree;
			this.SetNodeAndEdge(this.AdjustRefCounts(node, edge, this._node, this.Edge), edge);
			this._generation = tree.PositionGeneration;
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00108A00 File Offset: 0x00106C00
		private TextElement GetElement(LogicalDirection direction)
		{
			this.DebugAssertGeneration();
			TextTreeTextElementNode adjacentTextElementNode = this.GetAdjacentTextElementNode(direction);
			if (adjacentTextElementNode != null)
			{
				return adjacentTextElementNode.TextElement;
			}
			return null;
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x00108A28 File Offset: 0x00106C28
		private void AssertState()
		{
			if (Invariant.Strict)
			{
				Invariant.Assert(this._node != null, "Null position node!");
				if (this.GetGravityInternal() == LogicalDirection.Forward)
				{
					Invariant.Assert(this.Edge == ElementEdge.BeforeStart || this.Edge == ElementEdge.BeforeEnd, "Bad position edge/gravity pair! (1)");
				}
				else
				{
					Invariant.Assert(this.Edge == ElementEdge.AfterStart || this.Edge == ElementEdge.AfterEnd, "Bad position edge/gravity pair! (2)");
				}
				if (this._node is TextTreeRootNode)
				{
					Invariant.Assert(this.Edge != ElementEdge.BeforeStart && this.Edge != ElementEdge.AfterEnd, "Position at outer edge of root!");
				}
				else if (this._node is TextTreeTextNode || this._node is TextTreeObjectNode)
				{
					Invariant.Assert(this.Edge != ElementEdge.AfterStart && this.Edge != ElementEdge.BeforeEnd, "Position at inner leaf node edge!");
				}
				else
				{
					Invariant.Assert(this._node is TextTreeTextElementNode, "Unknown node type!");
				}
				Invariant.Assert(this._tree != null, "Position has no tree!");
			}
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00108B35 File Offset: 0x00106D35
		private void SetNodeAndEdge(TextTreeNode node, ElementEdge edge)
		{
			Invariant.Assert(edge == ElementEdge.BeforeStart || edge == ElementEdge.AfterStart || edge == ElementEdge.BeforeEnd || edge == ElementEdge.AfterEnd);
			this._node = node;
			this._flags = ((this._flags & 4294967280U) | (uint)edge);
			this.VerifyFlags();
			this.IsCaretUnitBoundaryCacheValid = false;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00108B74 File Offset: 0x00106D74
		private void SetIsFrozen()
		{
			this._flags |= 16U;
			this.VerifyFlags();
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x00108B8C File Offset: 0x00106D8C
		private void VerifyFlags()
		{
			ElementEdge elementEdge = (ElementEdge)(this._flags & 15U);
			Invariant.Assert(elementEdge == ElementEdge.BeforeStart || elementEdge == ElementEdge.AfterStart || elementEdge == ElementEdge.BeforeEnd || elementEdge == ElementEdge.AfterEnd);
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x00108BBC File Offset: 0x00106DBC
		// (set) Token: 0x06003A63 RID: 14947 RVA: 0x00108BCB File Offset: 0x00106DCB
		private bool IsCaretUnitBoundaryCacheValid
		{
			get
			{
				return (this._flags & 32U) == 32U;
			}
			set
			{
				this._flags = ((this._flags & 4294967263U) | (value ? 32U : 0U));
				this.VerifyFlags();
			}
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x00108BEB File Offset: 0x00106DEB
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x00108BFA File Offset: 0x00106DFA
		private bool CaretUnitBoundaryCache
		{
			get
			{
				return (this._flags & 64U) == 64U;
			}
			set
			{
				this._flags = ((this._flags & 4294967231U) | (value ? 64U : 0U));
				this.VerifyFlags();
			}
		}

		// Token: 0x040025D3 RID: 9683
		private TextContainer _tree;

		// Token: 0x040025D4 RID: 9684
		private TextTreeNode _node;

		// Token: 0x040025D5 RID: 9685
		private uint _generation;

		// Token: 0x040025D6 RID: 9686
		private uint _layoutGeneration;

		// Token: 0x040025D7 RID: 9687
		private uint _flags;

		// Token: 0x02000906 RID: 2310
		[Flags]
		private enum Flags
		{
			// Token: 0x04004307 RID: 17159
			EdgeMask = 15,
			// Token: 0x04004308 RID: 17160
			IsFrozen = 16,
			// Token: 0x04004309 RID: 17161
			IsCaretUnitBoundaryCacheValid = 32,
			// Token: 0x0400430A RID: 17162
			CaretUnitBoundaryCache = 64
		}
	}
}
