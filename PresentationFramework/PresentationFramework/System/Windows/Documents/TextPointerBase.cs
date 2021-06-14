using System;
using System.Globalization;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000409 RID: 1033
	internal static class TextPointerBase
	{
		// Token: 0x06003A66 RID: 14950 RVA: 0x00108C1A File Offset: 0x00106E1A
		internal static ITextPointer Min(ITextPointer position1, ITextPointer position2)
		{
			if (position1.CompareTo(position2) > 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x00108C29 File Offset: 0x00106E29
		internal static ITextPointer Max(ITextPointer position1, ITextPointer position2)
		{
			if (position1.CompareTo(position2) < 0)
			{
				return position2;
			}
			return position1;
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x00108C38 File Offset: 0x00106E38
		internal static string GetTextInRun(ITextPointer position, LogicalDirection direction)
		{
			int textRunLength = position.GetTextRunLength(direction);
			char[] array = new char[textRunLength];
			int textInRun = position.GetTextInRun(direction, array, 0, textRunLength);
			Invariant.Assert(textInRun == textRunLength, "textLengths returned from GetTextRunLength and GetTextInRun are innconsistent");
			return new string(array);
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x00108C74 File Offset: 0x00106E74
		internal static int GetTextWithLimit(ITextPointer thisPointer, LogicalDirection direction, char[] textBuffer, int startIndex, int count, ITextPointer limit)
		{
			int result;
			if (limit == null)
			{
				result = thisPointer.GetTextInRun(direction, textBuffer, startIndex, count);
			}
			else if (direction == LogicalDirection.Forward && limit.CompareTo(thisPointer) <= 0)
			{
				result = 0;
			}
			else if (direction == LogicalDirection.Backward && limit.CompareTo(thisPointer) >= 0)
			{
				result = 0;
			}
			else
			{
				int num;
				if (direction == LogicalDirection.Forward)
				{
					num = Math.Min(count, thisPointer.GetOffsetToPosition(limit));
				}
				else
				{
					num = Math.Min(count, limit.GetOffsetToPosition(thisPointer));
				}
				num = Math.Min(count, num);
				result = thisPointer.GetTextInRun(direction, textBuffer, startIndex, num);
			}
			return result;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x00108CF3 File Offset: 0x00106EF3
		internal static bool IsAtInsertionPosition(ITextPointer position)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, true);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00108CFC File Offset: 0x00106EFC
		internal static bool IsAtPotentialRunPosition(ITextPointer position)
		{
			bool flag = TextPointerBase.IsAtPotentialRunPosition(position, position);
			if (!flag)
			{
				flag = TextPointerBase.IsAtPotentialParagraphPosition(position);
			}
			return flag;
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x00108D1C File Offset: 0x00106F1C
		internal static bool IsAtPotentialRunPosition(TextElement run)
		{
			return run is Run && run.IsEmpty && TextPointerBase.IsAtPotentialRunPosition(run.ElementStart, run.ElementEnd);
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00108D44 File Offset: 0x00106F44
		private static bool IsAtPotentialRunPosition(ITextPointer backwardPosition, ITextPointer forwardPosition)
		{
			Invariant.Assert(backwardPosition.HasEqualScope(forwardPosition));
			if (TextSchema.IsValidChild(backwardPosition, typeof(Run)))
			{
				Type elementType = forwardPosition.GetElementType(LogicalDirection.Forward);
				Type elementType2 = backwardPosition.GetElementType(LogicalDirection.Backward);
				if (elementType != null && elementType2 != null)
				{
					TextPointerContext pointerContext = forwardPosition.GetPointerContext(LogicalDirection.Forward);
					TextPointerContext pointerContext2 = backwardPosition.GetPointerContext(LogicalDirection.Backward);
					if ((pointerContext2 == TextPointerContext.ElementStart && pointerContext == TextPointerContext.ElementEnd) || (pointerContext2 == TextPointerContext.ElementStart && TextSchema.IsNonFormattingInline(elementType) && !TextPointerBase.IsAtNonMergeableInlineStart(backwardPosition)) || (pointerContext == TextPointerContext.ElementEnd && TextSchema.IsNonFormattingInline(elementType2) && !TextPointerBase.IsAtNonMergeableInlineEnd(forwardPosition)) || (pointerContext2 == TextPointerContext.ElementEnd && pointerContext == TextPointerContext.ElementStart && TextSchema.IsNonFormattingInline(elementType2) && TextSchema.IsNonFormattingInline(elementType)) || (pointerContext2 == TextPointerContext.ElementEnd && typeof(Inline).IsAssignableFrom(elementType2) && !TextSchema.IsMergeableInline(elementType2) && !typeof(Run).IsAssignableFrom(elementType) && (pointerContext != TextPointerContext.ElementEnd || !TextPointerBase.IsAtNonMergeableInlineEnd(forwardPosition))) || (pointerContext == TextPointerContext.ElementStart && typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsMergeableInline(elementType) && !typeof(Run).IsAssignableFrom(elementType2) && (pointerContext2 != TextPointerContext.ElementStart || !TextPointerBase.IsAtNonMergeableInlineStart(backwardPosition))))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00108E78 File Offset: 0x00107078
		internal static bool IsAtPotentialParagraphPosition(ITextPointer position)
		{
			Type parentType = position.ParentType;
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Backward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Forward);
			if (pointerContext == TextPointerContext.ElementStart && pointerContext2 == TextPointerContext.ElementEnd)
			{
				return typeof(ListItem).IsAssignableFrom(parentType) || typeof(TableCell).IsAssignableFrom(parentType);
			}
			return pointerContext == TextPointerContext.None && pointerContext2 == TextPointerContext.None && (typeof(FlowDocumentView).IsAssignableFrom(parentType) || typeof(FlowDocument).IsAssignableFrom(parentType));
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00108EF8 File Offset: 0x001070F8
		internal static bool IsBeforeFirstTable(ITextPointer position)
		{
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Forward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Backward);
			return pointerContext == TextPointerContext.ElementStart && (pointerContext2 == TextPointerContext.ElementStart || pointerContext2 == TextPointerContext.None) && typeof(Table).IsAssignableFrom(position.GetElementType(LogicalDirection.Forward));
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x00108F38 File Offset: 0x00107138
		internal static bool IsInBlockUIContainer(ITextPointer position)
		{
			return typeof(BlockUIContainer).IsAssignableFrom(position.ParentType);
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00108F4F File Offset: 0x0010714F
		internal static bool IsAtBlockUIContainerStart(ITextPointer position)
		{
			return TextPointerBase.IsInBlockUIContainer(position) && position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart;
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00108F65 File Offset: 0x00107165
		internal static bool IsAtBlockUIContainerEnd(ITextPointer position)
		{
			return TextPointerBase.IsInBlockUIContainer(position) && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd;
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00108F7C File Offset: 0x0010717C
		private static bool IsInAncestorScope(ITextPointer position, Type allowedParentType, Type limitingType)
		{
			ITextPointer textPointer = position.CreatePointer();
			Type parentType = textPointer.ParentType;
			while (parentType != null && allowedParentType.IsAssignableFrom(parentType))
			{
				if (limitingType.IsAssignableFrom(parentType))
				{
					return true;
				}
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				parentType = textPointer.ParentType;
			}
			return false;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00108FC5 File Offset: 0x001071C5
		internal static bool IsInAnchoredBlock(ITextPointer position)
		{
			return TextPointerBase.IsInAncestorScope(position, typeof(TextElement), typeof(AnchoredBlock));
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00108FE1 File Offset: 0x001071E1
		internal static bool IsInHyperlinkScope(ITextPointer position)
		{
			return TextPointerBase.IsInAncestorScope(position, typeof(Inline), typeof(Hyperlink));
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00109000 File Offset: 0x00107200
		internal static ITextPointer GetFollowingNonMergeableInlineContentStart(ITextPointer position)
		{
			ITextPointer textPointer = position.CreatePointer();
			bool flag = false;
			Type elementType;
			for (;;)
			{
				if (TextPointerBase.GetBorderingElementCategory(textPointer, LogicalDirection.Forward) == TextPointerBase.BorderingElementCategory.MergeableScopingInline)
				{
					do
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					while (TextPointerBase.GetBorderingElementCategory(textPointer, LogicalDirection.Forward) == TextPointerBase.BorderingElementCategory.MergeableScopingInline);
					flag = true;
				}
				elementType = textPointer.GetElementType(LogicalDirection.Forward);
				if (elementType == typeof(InlineUIContainer) || elementType == typeof(BlockUIContainer))
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				}
				else
				{
					if (!(textPointer.ParentType == typeof(InlineUIContainer)) && !(textPointer.ParentType == typeof(BlockUIContainer)))
					{
						break;
					}
					textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
				}
				elementType = textPointer.GetElementType(LogicalDirection.Forward);
				if (!(elementType == typeof(InlineUIContainer)) && !(elementType == typeof(BlockUIContainer)))
				{
					textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				}
				flag = true;
			}
			if (typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsMergeableInline(elementType))
			{
				do
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
				while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart);
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return textPointer;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00109115 File Offset: 0x00107315
		internal static bool IsAtNonMergeableInlineStart(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineEdge(position, LogicalDirection.Backward);
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x0010911E File Offset: 0x0010731E
		internal static bool IsAtNonMergeableInlineEnd(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineEdge(position, LogicalDirection.Forward);
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x00109127 File Offset: 0x00107327
		internal static bool IsPositionAtNonMergeableInlineBoundary(ITextPointer position)
		{
			return TextPointerBase.IsAtNonMergeableInlineStart(position) || TextPointerBase.IsAtNonMergeableInlineEnd(position);
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00109139 File Offset: 0x00107339
		internal static bool IsAtFormatNormalizedPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, direction, false);
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00109143 File Offset: 0x00107343
		internal static bool IsAtInsertionPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextPointerBase.IsAtNormalizedPosition(position, direction, true);
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00109150 File Offset: 0x00107350
		internal static bool IsAtNormalizedPosition(ITextPointer position, LogicalDirection direction, bool respectCaretUnitBoundaries)
		{
			if (!TextPointerBase.IsAtNormalizedPosition(position, respectCaretUnitBoundaries))
			{
				return false;
			}
			if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				return true;
			}
			if (TextSchema.IsFormattingType(position.GetElementType(direction)))
			{
				position = position.CreatePointer();
				while (TextSchema.IsFormattingType(position.GetElementType(direction)))
				{
					position.MoveToNextContextPosition(direction);
				}
				if (TextPointerBase.IsAtNormalizedPosition(position, respectCaretUnitBoundaries))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x001091B8 File Offset: 0x001073B8
		internal static int GetOffset(ITextPointer thisPosition)
		{
			return thisPosition.TextContainer.Start.GetOffsetToPosition(thisPosition);
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x001091CC File Offset: 0x001073CC
		internal static bool IsAtWordBoundary(ITextPointer thisPosition, LogicalDirection insideWordDirection)
		{
			ITextPointer textPointer = thisPosition.CreatePointer();
			if (textPointer.GetPointerContext(insideWordDirection) != TextPointerContext.Text)
			{
				textPointer.MoveToInsertionPosition(insideWordDirection);
			}
			bool result;
			if (textPointer.GetPointerContext(insideWordDirection) == TextPointerContext.Text)
			{
				char[] text;
				int position;
				TextPointerBase.GetWordBreakerText(thisPosition, out text, out position);
				result = SelectionWordBreaker.IsAtWordBoundary(text, position, insideWordDirection);
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00109214 File Offset: 0x00107414
		internal static TextSegment GetWordRange(ITextPointer thisPosition)
		{
			return TextPointerBase.GetWordRange(thisPosition, LogicalDirection.Forward);
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00109220 File Offset: 0x00107420
		internal static TextSegment GetWordRange(ITextPointer thisPosition, LogicalDirection direction)
		{
			if (!thisPosition.IsAtInsertionPosition)
			{
				thisPosition = thisPosition.GetInsertionPosition(direction);
			}
			if (!thisPosition.IsAtInsertionPosition)
			{
				return new TextSegment(thisPosition, thisPosition);
			}
			ITextPointer textPointer = thisPosition.CreatePointer();
			bool flag = TextPointerBase.MoveToNextWordBoundary(textPointer, direction);
			ITextPointer textPointer2 = textPointer;
			ITextPointer textPointer3;
			if (flag && TextPointerBase.IsAtWordBoundary(thisPosition, LogicalDirection.Forward))
			{
				textPointer3 = thisPosition;
			}
			else
			{
				textPointer = thisPosition.CreatePointer();
				TextPointerBase.MoveToNextWordBoundary(textPointer, (direction == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
				textPointer3 = textPointer;
			}
			if (direction == LogicalDirection.Backward)
			{
				textPointer = textPointer3;
				textPointer3 = textPointer2;
				textPointer2 = textPointer;
			}
			textPointer3 = TextPointerBase.RestrictWithinBlock(thisPosition, textPointer3, LogicalDirection.Backward);
			textPointer2 = TextPointerBase.RestrictWithinBlock(thisPosition, textPointer2, LogicalDirection.Forward);
			if (textPointer3.CompareTo(textPointer2) < 0)
			{
				textPointer3 = textPointer3.GetFrozenPointer(LogicalDirection.Backward);
				textPointer2 = textPointer2.GetFrozenPointer(LogicalDirection.Forward);
			}
			else
			{
				textPointer3 = textPointer2.GetFrozenPointer(LogicalDirection.Backward);
				textPointer2 = textPointer3;
			}
			Invariant.Assert(textPointer3.CompareTo(textPointer2) <= 0, "expecting wordStart <= wordEnd");
			return new TextSegment(textPointer3, textPointer2);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x001092E8 File Offset: 0x001074E8
		private static ITextPointer RestrictWithinBlock(ITextPointer position, ITextPointer limit, LogicalDirection direction)
		{
			Invariant.Assert(direction != LogicalDirection.Backward || position.CompareTo(limit) >= 0, "for backward direction position must be >= than limit");
			Invariant.Assert(direction != LogicalDirection.Forward || position.CompareTo(limit) <= 0, "for forward direcion position must be <= than linit");
			while ((direction == LogicalDirection.Backward) ? (position.CompareTo(limit) > 0) : (position.CompareTo(limit) < 0))
			{
				TextPointerContext pointerContext = position.GetPointerContext(direction);
				if (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd)
				{
					Type elementType = position.GetElementType(direction);
					if (!typeof(Inline).IsAssignableFrom(elementType))
					{
						limit = position;
						break;
					}
				}
				else if (pointerContext == TextPointerContext.EmbeddedElement)
				{
					limit = position;
					break;
				}
				position = position.GetNextContextPosition(direction);
			}
			return limit.GetInsertionPosition((direction == LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward);
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x0010939C File Offset: 0x0010759C
		internal static bool IsNextToPlainLineBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			char[] array = new char[2];
			int textInRun = thisPosition.GetTextInRun(direction, array, 0, 2);
			return (textInRun == 1 && TextPointerBase.IsCharUnicodeNewLine(array[0])) || (textInRun == 2 && ((direction == LogicalDirection.Backward && TextPointerBase.IsCharUnicodeNewLine(array[1])) || (direction == LogicalDirection.Forward && TextPointerBase.IsCharUnicodeNewLine(array[0]))));
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x001093EE File Offset: 0x001075EE
		internal static bool IsCharUnicodeNewLine(char ch)
		{
			return Array.IndexOf<char>(TextPointerBase.NextLineCharacters, ch) > -1;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x001093FE File Offset: 0x001075FE
		internal static bool IsNextToRichLineBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			return TextPointerBase.IsNextToRichBreak(thisPosition, direction, typeof(LineBreak));
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x00109411 File Offset: 0x00107611
		internal static bool IsNextToParagraphBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			return TextPointerBase.IsNextToRichBreak(thisPosition, direction, typeof(Paragraph));
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00109424 File Offset: 0x00107624
		internal static bool IsNextToAnyBreak(ITextPointer thisPosition, LogicalDirection direction)
		{
			if (!thisPosition.IsAtInsertionPosition)
			{
				thisPosition = thisPosition.GetInsertionPosition(direction);
			}
			return TextPointerBase.IsNextToPlainLineBreak(thisPosition, direction) || TextPointerBase.IsNextToRichBreak(thisPosition, direction, null);
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x0010944C File Offset: 0x0010764C
		internal static bool IsAtLineWrappingPosition(ITextPointer position, ITextView textView)
		{
			Invariant.Assert(position != null, "null check: position");
			if (!position.HasValidLayout)
			{
				return false;
			}
			Invariant.Assert(textView != null, "textView cannot be null because the position has valid layout");
			TextSegment lineRange = textView.GetLineRange(position);
			return !lineRange.IsNull && ((position.LogicalDirection == LogicalDirection.Forward) ? (position.CompareTo(lineRange.Start) == 0) : (position.CompareTo(lineRange.End) == 0));
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001094BF File Offset: 0x001076BF
		internal static bool IsAtRowEnd(ITextPointer thisPosition)
		{
			return typeof(TableRow).IsAssignableFrom(thisPosition.ParentType) && thisPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && thisPosition.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart;
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001094F1 File Offset: 0x001076F1
		internal static bool IsAfterLastParagraph(ITextPointer thisPosition)
		{
			return thisPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.None && thisPosition.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && !typeof(Inline).IsAssignableFrom(thisPosition.GetElementType(LogicalDirection.Backward));
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00109521 File Offset: 0x00107721
		internal static bool IsAtParagraphOrBlockUIContainerStart(ITextPointer pointer)
		{
			if (TextPointerBase.IsAtPotentialParagraphPosition(pointer))
			{
				return true;
			}
			while (pointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (TextSchema.IsParagraphOrBlockUIContainer(pointer.ParentType))
				{
					return true;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Backward);
			}
			return false;
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x00109550 File Offset: 0x00107750
		internal static ListItem GetListItem(TextPointer pointer)
		{
			if (pointer.Parent is ListItem)
			{
				return (ListItem)pointer.Parent;
			}
			Block paragraphOrBlockUIContainer = pointer.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				return paragraphOrBlockUIContainer.Parent as ListItem;
			}
			return null;
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x00109590 File Offset: 0x00107790
		internal static ListItem GetImmediateListItem(TextPointer position)
		{
			if (position.Parent is ListItem)
			{
				return (ListItem)position.Parent;
			}
			Block paragraphOrBlockUIContainer = position.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null && paragraphOrBlockUIContainer.Parent is ListItem && paragraphOrBlockUIContainer.ElementStart.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				return (ListItem)paragraphOrBlockUIContainer.Parent;
			}
			return null;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x001095EC File Offset: 0x001077EC
		internal static bool IsInEmptyListItem(TextPointer position)
		{
			ListItem listItem = position.Parent as ListItem;
			return listItem != null && listItem.IsEmpty;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x00109610 File Offset: 0x00107810
		internal static int MoveToLineBoundary(ITextPointer thisPointer, ITextView textView, int count)
		{
			return TextPointerBase.MoveToLineBoundary(thisPointer, textView, count, false);
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x0010961C File Offset: 0x0010781C
		internal static int MoveToLineBoundary(ITextPointer thisPointer, ITextView textView, int count, bool respectNonMeargeableInlineStart)
		{
			Invariant.Assert(!thisPointer.IsFrozen, "Can't reposition a frozen pointer!");
			Invariant.Assert(textView != null, "Null TextView!");
			double num;
			ITextPointer positionAtNextLine = textView.GetPositionAtNextLine(thisPointer, double.NaN, count, out num, out count);
			if (!positionAtNextLine.IsAtInsertionPosition && (!respectNonMeargeableInlineStart || (!TextPointerBase.IsAtNonMergeableInlineStart(positionAtNextLine) && !TextPointerBase.IsAtNonMergeableInlineEnd(positionAtNextLine))))
			{
				positionAtNextLine.MoveToInsertionPosition(positionAtNextLine.LogicalDirection);
			}
			if (TextPointerBase.IsAtRowEnd(positionAtNextLine))
			{
				thisPointer.MoveToPosition(positionAtNextLine);
				thisPointer.SetLogicalDirection(positionAtNextLine.LogicalDirection);
			}
			else
			{
				TextSegment lineRange = textView.GetLineRange(positionAtNextLine);
				if (!lineRange.IsNull)
				{
					thisPointer.MoveToPosition(lineRange.Start);
					thisPointer.SetLogicalDirection(lineRange.Start.LogicalDirection);
				}
				else if (count > 0)
				{
					thisPointer.MoveToPosition(positionAtNextLine);
					thisPointer.SetLogicalDirection(positionAtNextLine.LogicalDirection);
				}
			}
			return count;
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001096EF File Offset: 0x001078EF
		internal static Rect GetCharacterRect(ITextPointer thisPointer, LogicalDirection direction)
		{
			return TextPointerBase.GetCharacterRect(thisPointer, direction, true);
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x001096FC File Offset: 0x001078FC
		internal static Rect GetCharacterRect(ITextPointer thisPointer, LogicalDirection direction, bool transformToUiScope)
		{
			ITextView textView = thisPointer.TextContainer.TextView;
			Invariant.Assert(textView != null, "Null TextView!");
			Invariant.Assert(textView.RenderScope != null, "Null RenderScope");
			Invariant.Assert(thisPointer.TextContainer != null, "Null TextContainer");
			Invariant.Assert(thisPointer.TextContainer.Parent != null, "Null parent of TextContainer");
			if (!thisPointer.IsAtInsertionPosition)
			{
				ITextPointer insertionPosition = thisPointer.GetInsertionPosition(direction);
				if (insertionPosition != null)
				{
					thisPointer = insertionPosition;
				}
			}
			Rect rect = textView.GetRectangleFromTextPosition(thisPointer.CreatePointer(direction));
			if (transformToUiScope)
			{
				Visual visual;
				if (thisPointer.TextContainer.Parent is FlowDocument && textView.RenderScope is FlowDocumentView)
				{
					visual = (((FlowDocumentView)textView.RenderScope).TemplatedParent as Visual);
					if (visual == null && ((FlowDocumentView)textView.RenderScope).Parent is FrameworkElement)
					{
						visual = (((FrameworkElement)((FlowDocumentView)textView.RenderScope).Parent).TemplatedParent as Visual);
					}
				}
				else if (thisPointer.TextContainer.Parent is Visual)
				{
					Invariant.Assert(textView.RenderScope == thisPointer.TextContainer.Parent || ((Visual)thisPointer.TextContainer.Parent).IsAncestorOf(textView.RenderScope), "Unexpected location of RenderScope within visual tree");
					visual = (Visual)thisPointer.TextContainer.Parent;
				}
				else
				{
					visual = null;
				}
				if (visual != null && visual.IsAncestorOf(textView.RenderScope))
				{
					GeneralTransform generalTransform = textView.RenderScope.TransformToAncestor(visual);
					rect = generalTransform.TransformBounds(rect);
				}
			}
			return rect;
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x0010988D File Offset: 0x00107A8D
		internal static bool MoveToFormatNormalizedPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			return TextPointerBase.NormalizePosition(thisNavigator, direction, false);
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x00109897 File Offset: 0x00107A97
		internal static bool MoveToInsertionPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			return TextPointerBase.NormalizePosition(thisNavigator, direction, true);
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x001098A4 File Offset: 0x00107AA4
		internal static bool MoveToNextInsertionPosition(ITextPointer thisNavigator, LogicalDirection direction)
		{
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			bool flag = true;
			int num = (direction == LogicalDirection.Forward) ? 1 : -1;
			ITextPointer textPointer = thisNavigator.CreatePointer();
			if (!TextPointerBase.IsAtInsertionPosition(thisNavigator))
			{
				if (!TextPointerBase.MoveToInsertionPosition(thisNavigator, direction))
				{
					flag = false;
					goto IL_E9;
				}
				if (direction == LogicalDirection.Forward && textPointer.CompareTo(thisNavigator) < 0)
				{
					goto IL_E9;
				}
				if (direction == LogicalDirection.Backward && thisNavigator.CompareTo(textPointer) < 0)
				{
					goto IL_E9;
				}
			}
			while (TextSchema.IsFormattingType(thisNavigator.GetElementType(direction)))
			{
				thisNavigator.MoveByOffset(num);
			}
			while (thisNavigator.GetPointerContext(direction) != TextPointerContext.None)
			{
				thisNavigator.MoveByOffset(num);
				if (TextPointerBase.IsAtInsertionPosition(thisNavigator))
				{
					if (direction != LogicalDirection.Backward)
					{
						goto IL_E9;
					}
					while (TextSchema.IsFormattingType(thisNavigator.GetElementType(direction)))
					{
						thisNavigator.MoveByOffset(num);
					}
					TextPointerContext pointerContext = thisNavigator.GetPointerContext(direction);
					if (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.None)
					{
						num = -num;
						while (TextSchema.IsFormattingType(thisNavigator.GetElementType(LogicalDirection.Forward)) && !TextPointerBase.IsAtInsertionPosition(thisNavigator))
						{
							thisNavigator.MoveByOffset(num);
						}
						goto IL_E9;
					}
					goto IL_E9;
				}
			}
			thisNavigator.MoveToPosition(textPointer);
			flag = false;
			IL_E9:
			if (flag)
			{
				if (direction == LogicalDirection.Forward)
				{
					Invariant.Assert(thisNavigator.CompareTo(textPointer) > 0, "thisNavigator is expected to be moved from initialPosition - 1");
				}
				else
				{
					Invariant.Assert(thisNavigator.CompareTo(textPointer) < 0, "thisNavigator is expected to be moved from initialPosition - 2");
				}
			}
			else
			{
				Invariant.Assert(thisNavigator.CompareTo(textPointer) == 0, "thisNavigator must stay at initial position");
			}
			return flag;
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x001099E4 File Offset: 0x00107BE4
		internal static bool MoveToNextWordBoundary(ITextPointer thisNavigator, LogicalDirection movingDirection)
		{
			int num = 0;
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			ITextPointer position = thisNavigator.CreatePointer();
			while (thisNavigator.MoveToNextInsertionPosition(movingDirection))
			{
				num++;
				if (num > 64)
				{
					thisNavigator.MoveToPosition(position);
					thisNavigator.MoveToNextContextPosition(movingDirection);
					break;
				}
				if (TextPointerBase.IsAtWordBoundary(thisNavigator, LogicalDirection.Forward))
				{
					break;
				}
			}
			return num > 0;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00109A40 File Offset: 0x00107C40
		internal static ITextPointer GetFrozenPointer(ITextPointer thisPointer, LogicalDirection logicalDirection)
		{
			ITextPointer textPointer;
			if (thisPointer.IsFrozen && thisPointer.LogicalDirection == logicalDirection)
			{
				textPointer = thisPointer;
			}
			else
			{
				textPointer = thisPointer.CreatePointer(logicalDirection);
				textPointer.Freeze();
			}
			return textPointer;
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x00109A71 File Offset: 0x00107C71
		internal static bool ValidateLayout(ITextPointer thisPointer, ITextView textView)
		{
			return textView != null && textView.Validate(thisPointer);
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x00109A80 File Offset: 0x00107C80
		private static bool NormalizePosition(ITextPointer thisNavigator, LogicalDirection direction, bool respectCaretUnitBoundaries)
		{
			Invariant.Assert(!thisNavigator.IsFrozen, "Can't reposition a frozen pointer!");
			int num = 0;
			int num2;
			LogicalDirection direction2;
			TextPointerContext textPointerContext;
			TextPointerContext textPointerContext2;
			if (direction == LogicalDirection.Forward)
			{
				num2 = 1;
				direction2 = LogicalDirection.Backward;
				textPointerContext = TextPointerContext.ElementStart;
				textPointerContext2 = TextPointerContext.ElementEnd;
			}
			else
			{
				num2 = -1;
				direction2 = LogicalDirection.Forward;
				textPointerContext = TextPointerContext.ElementEnd;
				textPointerContext2 = TextPointerContext.ElementStart;
			}
			if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
			{
				while (thisNavigator.GetPointerContext(direction) == textPointerContext && !typeof(Inline).IsAssignableFrom(thisNavigator.GetElementType(direction)))
				{
					if (TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
					{
						break;
					}
					thisNavigator.MoveToNextContextPosition(direction);
					num += num2;
				}
				while (thisNavigator.GetPointerContext(direction2) == textPointerContext2 && !typeof(Inline).IsAssignableFrom(thisNavigator.GetElementType(direction2)) && !TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					thisNavigator.MoveToNextContextPosition(direction2);
					num -= num2;
				}
			}
			num = TextPointerBase.LeaveNonMergeableInlineBoundary(thisNavigator, direction, num);
			if (respectCaretUnitBoundaries)
			{
				while (!TextPointerBase.IsAtCaretUnitBoundary(thisNavigator))
				{
					num += num2;
					thisNavigator.MoveByOffset(num2);
				}
			}
			while (TextSchema.IsMergeableInline(thisNavigator.GetElementType(direction)))
			{
				thisNavigator.MoveToNextContextPosition(direction);
				num += num2;
			}
			if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
			{
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					if (!TextSchema.IsMergeableInline(thisNavigator.GetElementType(direction2)))
					{
						break;
					}
					thisNavigator.MoveToNextContextPosition(direction2);
					num -= num2;
				}
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					if (!thisNavigator.MoveToNextContextPosition(direction))
					{
						break;
					}
					num += num2;
				}
				while (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries) && thisNavigator.MoveToNextContextPosition(direction2))
				{
					num -= num2;
				}
				if (!TextPointerBase.IsAtNormalizedPosition(thisNavigator, respectCaretUnitBoundaries))
				{
					thisNavigator.MoveByOffset(-num);
				}
			}
			return num != 0;
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x00109BE8 File Offset: 0x00107DE8
		private static int LeaveNonMergeableInlineBoundary(ITextPointer thisNavigator, LogicalDirection direction, int symbolCount)
		{
			if (TextPointerBase.IsAtNonMergeableInlineStart(thisNavigator))
			{
				if (direction == LogicalDirection.Forward && TextPointerBase.IsAtNonMergeableInlineEnd(thisNavigator))
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Forward);
				}
				else
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Backward);
				}
			}
			else if (TextPointerBase.IsAtNonMergeableInlineEnd(thisNavigator))
			{
				if (direction == LogicalDirection.Backward && TextPointerBase.IsAtNonMergeableInlineStart(thisNavigator))
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Backward);
				}
				else
				{
					symbolCount += TextPointerBase.LeaveNonMergeableAncestor(thisNavigator, LogicalDirection.Forward);
				}
			}
			return symbolCount;
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00109C50 File Offset: 0x00107E50
		private static int LeaveNonMergeableAncestor(ITextPointer thisNavigator, LogicalDirection direction)
		{
			int num = 0;
			int num2 = (direction == LogicalDirection.Forward) ? 1 : -1;
			while (TextSchema.IsMergeableInline(thisNavigator.ParentType))
			{
				thisNavigator.MoveToNextContextPosition(direction);
				num += num2;
			}
			thisNavigator.MoveToNextContextPosition(direction);
			return num + num2;
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00109C90 File Offset: 0x00107E90
		private static bool IsAtNormalizedPosition(ITextPointer position, bool respectCaretUnitBoundaries)
		{
			if (TextPointerBase.IsPositionAtNonMergeableInlineBoundary(position))
			{
				return false;
			}
			if (TextSchema.IsValidChild(position, typeof(string)))
			{
				return !respectCaretUnitBoundaries || TextPointerBase.IsAtCaretUnitBoundary(position);
			}
			return TextPointerBase.IsAtRowEnd(position) || TextPointerBase.IsAtPotentialRunPosition(position) || TextPointerBase.IsBeforeFirstTable(position) || TextPointerBase.IsInBlockUIContainer(position);
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00109CE8 File Offset: 0x00107EE8
		private static bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Forward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Backward);
			bool result;
			if (pointerContext2 == TextPointerContext.Text && pointerContext == TextPointerContext.Text)
			{
				if (position.HasValidLayout)
				{
					result = position.IsAtCaretUnitBoundary;
				}
				else
				{
					result = !TextPointerBase.IsInsideCompoundSequence(position);
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x00109D30 File Offset: 0x00107F30
		private static bool IsInsideCompoundSequence(ITextPointer position)
		{
			char[] array = new char[2];
			if (position.GetTextInRun(LogicalDirection.Backward, array, 0, 1) == 1 && position.GetTextInRun(LogicalDirection.Forward, array, 1, 1) == 1)
			{
				if (char.IsSurrogatePair(array[0], array[1]) || (array[0] == '\r' && array[1] == '\n'))
				{
					return true;
				}
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(array[1]);
				if (unicodeCategory == UnicodeCategory.SpacingCombiningMark || unicodeCategory == UnicodeCategory.NonSpacingMark || unicodeCategory == UnicodeCategory.EnclosingMark)
				{
					UnicodeCategory unicodeCategory2 = char.GetUnicodeCategory(array[0]);
					if (unicodeCategory2 != UnicodeCategory.Control && unicodeCategory2 != UnicodeCategory.Format && unicodeCategory2 != UnicodeCategory.OtherNotAssigned)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x00109DAC File Offset: 0x00107FAC
		private static void GetWordBreakerText(ITextPointer pointer, out char[] text, out int position)
		{
			char[] array = new char[SelectionWordBreaker.MinContextLength];
			char[] array2 = new char[SelectionWordBreaker.MinContextLength];
			int num = 0;
			int num2 = 0;
			ITextPointer textPointer = pointer.CreatePointer();
			do
			{
				int num3 = Math.Min(textPointer.GetTextRunLength(LogicalDirection.Backward), SelectionWordBreaker.MinContextLength - num);
				num += num3;
				textPointer.MoveByOffset(-num3);
				textPointer.GetTextInRun(LogicalDirection.Forward, array, SelectionWordBreaker.MinContextLength - num, num3);
				if (num == SelectionWordBreaker.MinContextLength)
				{
					break;
				}
				textPointer.MoveToInsertionPosition(LogicalDirection.Backward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text);
			textPointer.MoveToPosition(pointer);
			do
			{
				int num3 = Math.Min(textPointer.GetTextRunLength(LogicalDirection.Forward), SelectionWordBreaker.MinContextLength - num2);
				textPointer.GetTextInRun(LogicalDirection.Forward, array2, num2, num3);
				num2 += num3;
				if (num2 == SelectionWordBreaker.MinContextLength)
				{
					break;
				}
				textPointer.MoveByOffset(num3);
				textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text);
			text = new char[num + num2];
			Array.Copy(array, SelectionWordBreaker.MinContextLength - num, text, 0, num);
			Array.Copy(array2, 0, text, num, num2);
			position = num;
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00109EB0 File Offset: 0x001080B0
		private static bool IsAtNonMergeableInlineEdge(ITextPointer position, LogicalDirection direction)
		{
			TextPointerBase.BorderingElementCategory borderingElementCategory = TextPointerBase.GetBorderingElementCategory(position, direction);
			if (borderingElementCategory == TextPointerBase.BorderingElementCategory.MergeableScopingInline)
			{
				ITextPointer textPointer = position.CreatePointer();
				do
				{
					textPointer.MoveToNextContextPosition(direction);
				}
				while ((borderingElementCategory = TextPointerBase.GetBorderingElementCategory(textPointer, direction)) == TextPointerBase.BorderingElementCategory.MergeableScopingInline);
			}
			return borderingElementCategory == TextPointerBase.BorderingElementCategory.NonMergeableScopingInline;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00109EE8 File Offset: 0x001080E8
		private static TextPointerBase.BorderingElementCategory GetBorderingElementCategory(ITextPointer position, LogicalDirection direction)
		{
			TextPointerContext textPointerContext = (direction == LogicalDirection.Forward) ? TextPointerContext.ElementEnd : TextPointerContext.ElementStart;
			TextPointerBase.BorderingElementCategory result;
			if (position.GetPointerContext(direction) != textPointerContext || !typeof(Inline).IsAssignableFrom(position.ParentType))
			{
				result = TextPointerBase.BorderingElementCategory.NotScopingInline;
			}
			else if (TextSchema.IsMergeableInline(position.ParentType))
			{
				result = TextPointerBase.BorderingElementCategory.MergeableScopingInline;
			}
			else
			{
				result = TextPointerBase.BorderingElementCategory.NonMergeableScopingInline;
			}
			return result;
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00109F38 File Offset: 0x00108138
		private static bool IsNextToRichBreak(ITextPointer thisPosition, LogicalDirection direction, Type lineBreakType)
		{
			Invariant.Assert(lineBreakType == null || lineBreakType == typeof(LineBreak) || lineBreakType == typeof(Paragraph));
			bool result = false;
			for (;;)
			{
				Type elementType = thisPosition.GetElementType(direction);
				if (lineBreakType == null)
				{
					if (typeof(LineBreak).IsAssignableFrom(elementType) || typeof(Paragraph).IsAssignableFrom(elementType))
					{
						break;
					}
				}
				else if (lineBreakType.IsAssignableFrom(elementType))
				{
					goto Block_5;
				}
				if (!TextSchema.IsFormattingType(elementType))
				{
					return result;
				}
				thisPosition = thisPosition.GetNextContextPosition(direction);
			}
			return true;
			Block_5:
			result = true;
			return result;
		}

		// Token: 0x040025D8 RID: 9688
		internal static char[] NextLineCharacters = new char[]
		{
			'\n',
			'\r',
			'\v',
			'\f',
			'\u0085',
			'\u2028',
			'\u2029'
		};

		// Token: 0x02000907 RID: 2311
		private enum BorderingElementCategory
		{
			// Token: 0x0400430C RID: 17164
			MergeableScopingInline,
			// Token: 0x0400430D RID: 17165
			NonMergeableScopingInline,
			// Token: 0x0400430E RID: 17166
			NotScopingInline
		}
	}
}
