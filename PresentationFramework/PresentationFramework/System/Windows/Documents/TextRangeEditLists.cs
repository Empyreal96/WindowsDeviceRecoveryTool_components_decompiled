using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200040E RID: 1038
	internal static class TextRangeEditLists
	{
		// Token: 0x06003B7F RID: 15231 RVA: 0x0010ECA4 File Offset: 0x0010CEA4
		internal static bool MergeParagraphs(Block firstParagraphOrBlockUIContainer, Block secondParagraphOrBlockUIContainer)
		{
			if (!TextRangeEditLists.ParagraphsAreMergeable(firstParagraphOrBlockUIContainer, secondParagraphOrBlockUIContainer))
			{
				return false;
			}
			ListItem listItem = (secondParagraphOrBlockUIContainer.PreviousBlock == null) ? (secondParagraphOrBlockUIContainer.Parent as ListItem) : null;
			if (listItem != null && listItem.PreviousListItem == null && secondParagraphOrBlockUIContainer.NextBlock is List)
			{
				List list = (List)secondParagraphOrBlockUIContainer.NextBlock;
				if (list.ElementEnd.CompareTo(listItem.ContentEnd) == 0)
				{
					listItem.Reposition(null, null);
				}
				else
				{
					listItem.Reposition(list.ElementEnd, listItem.ContentEnd);
				}
				list.Reposition(null, null);
			}
			while (secondParagraphOrBlockUIContainer.ElementStart.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				TextElement textElement = (TextElement)secondParagraphOrBlockUIContainer.Parent;
				Invariant.Assert(textElement != null);
				Invariant.Assert(TextSchema.AllowsParagraphMerging(textElement.GetType()));
				if (secondParagraphOrBlockUIContainer.ElementEnd.CompareTo(textElement.ContentEnd) == 0)
				{
					textElement.Reposition(null, null);
				}
				else
				{
					textElement.Reposition(secondParagraphOrBlockUIContainer.ElementEnd, textElement.ContentEnd);
				}
			}
			TextPointer frozenPointer = secondParagraphOrBlockUIContainer.ElementEnd.GetFrozenPointer(LogicalDirection.Forward);
			for (;;)
			{
				TextElement textElement2 = secondParagraphOrBlockUIContainer.ElementStart.GetAdjacentElement(LogicalDirection.Backward) as TextElement;
				Invariant.Assert(textElement2 != null);
				if (textElement2 is Paragraph || textElement2 is BlockUIContainer)
				{
					break;
				}
				Invariant.Assert(TextSchema.AllowsParagraphMerging(textElement2.GetType()));
				textElement2.Reposition(textElement2.ContentStart, secondParagraphOrBlockUIContainer.ElementEnd);
			}
			if (secondParagraphOrBlockUIContainer.TextRange.IsEmpty)
			{
				secondParagraphOrBlockUIContainer.RepositionWithContent(null);
			}
			else if (firstParagraphOrBlockUIContainer.TextRange.IsEmpty)
			{
				firstParagraphOrBlockUIContainer.RepositionWithContent(null);
			}
			else if (firstParagraphOrBlockUIContainer is Paragraph && secondParagraphOrBlockUIContainer is Paragraph)
			{
				Invariant.Assert(firstParagraphOrBlockUIContainer.ElementEnd.CompareTo(secondParagraphOrBlockUIContainer.ElementStart) == 0);
				firstParagraphOrBlockUIContainer.Reposition(firstParagraphOrBlockUIContainer.ContentStart, secondParagraphOrBlockUIContainer.ElementEnd);
				TextPointer elementStart = secondParagraphOrBlockUIContainer.ElementStart;
				secondParagraphOrBlockUIContainer.Reposition(null, null);
				TextRangeEdit.MergeFormattingInlines(elementStart);
			}
			ListItem listItem2 = (frozenPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart) ? (frozenPointer.GetAdjacentElement(LogicalDirection.Forward) as ListItem) : null;
			if (listItem2 != null && listItem2 == listItem)
			{
				ListItem listItem3 = frozenPointer.GetAdjacentElement(LogicalDirection.Backward) as ListItem;
				if (listItem3 != null)
				{
					Invariant.Assert(frozenPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart);
					Invariant.Assert(frozenPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd);
					listItem3.Reposition(listItem3.ContentStart, listItem2.ElementEnd);
					listItem2.Reposition(null, null);
				}
			}
			TextRangeEditLists.MergeLists(frozenPointer);
			return true;
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0010EEFC File Offset: 0x0010D0FC
		internal static bool MergeListsAroundNormalizedPosition(TextPointer mergePosition)
		{
			TextPointer textPointer = mergePosition.CreatePointer();
			while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			bool flag = TextRangeEditLists.MergeLists(textPointer);
			if (!flag)
			{
				textPointer.MoveToPosition(mergePosition);
				while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
				}
				flag = TextRangeEditLists.MergeLists(textPointer);
			}
			return flag;
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0010EF54 File Offset: 0x0010D154
		internal static bool MergeLists(TextPointer mergePosition)
		{
			if (mergePosition.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementEnd || mergePosition.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementStart)
			{
				return false;
			}
			List list = mergePosition.GetAdjacentElement(LogicalDirection.Backward) as List;
			List list2 = mergePosition.GetAdjacentElement(LogicalDirection.Forward) as List;
			if (list == null || list2 == null)
			{
				return false;
			}
			list.Reposition(list.ContentStart, list2.ElementEnd);
			list2.Reposition(null, null);
			TextRangeEdit.SetParagraphProperty(list.ElementStart, list.ElementEnd, Block.FlowDirectionProperty, list.GetValue(Block.FlowDirectionProperty));
			return true;
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0010EFD8 File Offset: 0x0010D1D8
		internal static bool IsListOperationApplicable(TextRange range)
		{
			if (TextRangeEditLists.IsRangeWithinSingleList(range))
			{
				return true;
			}
			TextPointer textPointer = (TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End);
			Block paragraphOrBlockUIContainer = range.Start.ParagraphOrBlockUIContainer;
			Block paragraphOrBlockUIContainer2 = textPointer.ParagraphOrBlockUIContainer;
			return (paragraphOrBlockUIContainer != null && paragraphOrBlockUIContainer2 != null && paragraphOrBlockUIContainer.Parent == paragraphOrBlockUIContainer2.Parent) || (range.IsEmpty && TextPointerBase.IsAtPotentialParagraphPosition(range.Start));
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x0010F048 File Offset: 0x0010D248
		internal static bool ConvertParagraphsToListItems(TextRange range, TextMarkerStyle markerStyle)
		{
			if (range.IsEmpty && TextPointerBase.IsAtPotentialParagraphPosition(range.Start))
			{
				TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition(range.Start);
				((ITextRange)range).Select(textPointer, textPointer);
			}
			Block paragraphOrBlockUIContainer = range.Start.ParagraphOrBlockUIContainer;
			TextPointer textPointer2 = (TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End);
			Block paragraphOrBlockUIContainer2 = textPointer2.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer == null || paragraphOrBlockUIContainer2 == null || paragraphOrBlockUIContainer.Parent != paragraphOrBlockUIContainer2.Parent || (paragraphOrBlockUIContainer.Parent is ListItem && paragraphOrBlockUIContainer.PreviousBlock == null))
			{
				return false;
			}
			Block block = paragraphOrBlockUIContainer;
			while (block != paragraphOrBlockUIContainer2 && block != null)
			{
				if (block is Table || block is Section)
				{
					return false;
				}
				block = block.NextBlock;
			}
			ListItem listItem = paragraphOrBlockUIContainer.Parent as ListItem;
			if (listItem != null)
			{
				Block block3;
				for (Block block2 = paragraphOrBlockUIContainer; block2 != null; block2 = block3)
				{
					block3 = ((block2 == paragraphOrBlockUIContainer2) ? null : (block2.ElementEnd.GetAdjacentElement(LogicalDirection.Forward) as Block));
					Invariant.Assert(block2.Parent is ListItem);
					TextRangeEdit.SplitElement(block2.ElementStart);
				}
			}
			else
			{
				new List
				{
					MarkerStyle = markerStyle
				}.Apply(paragraphOrBlockUIContainer, paragraphOrBlockUIContainer2);
			}
			return true;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x0010F178 File Offset: 0x0010D378
		internal static void ConvertListItemsToParagraphs(TextRange range)
		{
			ListItem listItem = TextPointerBase.GetListItem(range.Start);
			ListItem listItem2 = TextPointerBase.GetListItem((TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End));
			if (listItem == null || listItem2 == null || listItem.Parent != listItem2.Parent || !(listItem.Parent is List))
			{
				return;
			}
			List list = null;
			ListItem previousListItem = listItem.PreviousListItem;
			if (previousListItem != null)
			{
				previousListItem.Reposition(previousListItem.ContentStart, listItem2.ElementEnd);
			}
			else
			{
				if (listItem2.NextListItem != null)
				{
					TextRangeEdit.SplitElement(listItem2.ElementEnd);
				}
				list = listItem.List;
			}
			ListItem listItem4;
			for (ListItem listItem3 = listItem; listItem3 != null; listItem3 = ((listItem3 == listItem2) ? null : listItem4))
			{
				listItem4 = (listItem3.ElementEnd.GetAdjacentElement(LogicalDirection.Forward) as ListItem);
				if (listItem3.ContentStart.CompareTo(listItem3.ContentEnd) == 0)
				{
					TextRangeEditTables.EnsureInsertionPosition(listItem3.ContentStart);
				}
				listItem3.Reposition(null, null);
			}
			if (list != null)
			{
				FlowDirection flowDirection = (FlowDirection)list.GetValue(Block.FlowDirectionProperty);
				list.Reposition(null, null);
				TextRangeEdit.SetParagraphProperty(range.Start, range.End, Block.FlowDirectionProperty, flowDirection);
			}
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x0010F298 File Offset: 0x0010D498
		internal static void IndentListItems(TextRange range)
		{
			ListItem immediateListItem = TextPointerBase.GetImmediateListItem(range.Start);
			ListItem immediateListItem2 = TextPointerBase.GetImmediateListItem((TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End));
			if (immediateListItem == null || immediateListItem2 == null || immediateListItem.Parent != immediateListItem2.Parent || !(immediateListItem.Parent is List))
			{
				return;
			}
			ListItem previousListItem = immediateListItem.PreviousListItem;
			if (previousListItem == null)
			{
				return;
			}
			List element = (List)immediateListItem.Parent;
			List list = (List)TextRangeEdit.InsertElementClone(immediateListItem.ElementStart, immediateListItem2.ElementEnd, element);
			previousListItem.Reposition(previousListItem.ContentStart, list.ElementEnd);
			Paragraph paragraph = immediateListItem2.Blocks.FirstBlock as Paragraph;
			if (paragraph != null)
			{
				List list2 = paragraph.NextBlock as List;
				if (list2 != null && list2.NextBlock == null)
				{
					immediateListItem2.Reposition(immediateListItem2.ContentStart, list2.ElementStart);
					list2.Reposition(null, null);
				}
			}
			TextRangeEditLists.MergeLists(list.ElementStart);
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0010F38C File Offset: 0x0010D58C
		internal static bool UnindentListItems(TextRange range)
		{
			if (!TextRangeEditLists.IsRangeWithinSingleList(range))
			{
				return false;
			}
			ListItem listItem = TextPointerBase.GetListItem(range.Start);
			ListItem listItem2 = TextPointerBase.GetListItem((TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End));
			for (TextElement textElement = (TextElement)listItem2.Parent; textElement != listItem.Parent; textElement = (TextElement)textElement.Parent)
			{
				listItem2 = (textElement as ListItem);
			}
			if (listItem2 == null)
			{
				return false;
			}
			if (listItem.PreviousListItem != null)
			{
				TextRangeEdit.SplitElement(listItem.ElementStart);
			}
			if (listItem2.NextListItem != null)
			{
				TextRangeEdit.SplitElement(listItem2.ElementEnd);
			}
			List list = (List)listItem.Parent;
			ListItem listItem3 = list.Parent as ListItem;
			if (listItem3 != null)
			{
				list.Reposition(null, null);
				TextPointer contentEnd = listItem3.ContentEnd;
				if (listItem3.ContentStart.CompareTo(listItem.ElementStart) == 0)
				{
					listItem3.Reposition(null, null);
				}
				else
				{
					listItem3.Reposition(listItem3.ContentStart, listItem.ElementStart);
				}
				if (contentEnd.CompareTo(listItem2.ElementEnd) != 0)
				{
					TextPointer contentEnd2 = listItem2.ContentEnd;
					listItem2.Reposition(listItem2.ContentStart, contentEnd);
					TextRangeEditLists.MergeLists(contentEnd2);
				}
			}
			else
			{
				TextPointer elementStart = list.ElementStart;
				TextPointer elementEnd = list.ElementEnd;
				object value = list.GetValue(Block.FlowDirectionProperty);
				list.Reposition(null, null);
				ListItem listItem5;
				for (ListItem listItem4 = listItem; listItem4 != null; listItem4 = ((listItem4 == listItem2) ? null : listItem5))
				{
					listItem5 = (listItem4.ElementEnd.GetAdjacentElement(LogicalDirection.Forward) as ListItem);
					if (listItem4.ContentStart.CompareTo(listItem4.ContentEnd) == 0)
					{
						TextRangeEditTables.EnsureInsertionPosition(listItem4.ContentStart);
					}
					listItem4.Reposition(null, null);
				}
				TextRangeEdit.SetParagraphProperty(elementStart, elementEnd, Block.FlowDirectionProperty, value);
				TextRangeEditLists.MergeLists(elementStart);
				TextRangeEditLists.MergeLists(elementEnd);
			}
			return true;
		}

		// Token: 0x06003B87 RID: 15239 RVA: 0x0010F554 File Offset: 0x0010D754
		private static bool IsRangeWithinSingleList(TextRange range)
		{
			ListItem listItem = TextPointerBase.GetListItem(range.Start);
			TextPointer textPointer = (TextPointer)TextRangeEdit.GetAdjustedRangeEnd(range.Start, range.End);
			ListItem listItem2 = TextPointerBase.GetListItem(textPointer);
			if (listItem != null && listItem2 != null && listItem.Parent == listItem2.Parent)
			{
				return true;
			}
			if (listItem != null && listItem2 != null)
			{
				while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
				{
					if (textPointer.Parent == listItem.Parent)
					{
						return true;
					}
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				}
			}
			return false;
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0010F5CC File Offset: 0x0010D7CC
		internal static bool ParagraphsAreMergeable(Block firstParagraphOrBlockUIContainer, Block secondParagraphOrBlockUIContainer)
		{
			if (firstParagraphOrBlockUIContainer == null || secondParagraphOrBlockUIContainer == null || firstParagraphOrBlockUIContainer == secondParagraphOrBlockUIContainer)
			{
				return false;
			}
			TextPointer textPointer = firstParagraphOrBlockUIContainer.ElementEnd;
			TextPointer elementStart = secondParagraphOrBlockUIContainer.ElementStart;
			while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				if (!TextSchema.AllowsParagraphMerging(textPointer.Parent.GetType()))
				{
					return false;
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
			{
				if (textPointer.CompareTo(elementStart) == 0)
				{
					return true;
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				if (!TextSchema.AllowsParagraphMerging(textPointer.Parent.GetType()))
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06003B89 RID: 15241 RVA: 0x0010F650 File Offset: 0x0010D850
		internal static bool SplitListsForFlowDirectionChange(TextPointer start, TextPointer end, object newFlowDirectionValue)
		{
			ListItem listAncestor = start.GetListAncestor();
			if (listAncestor != null && listAncestor.List != null && !TextSchema.ValuesAreEqual(newFlowDirectionValue, listAncestor.List.GetValue(Block.FlowDirectionProperty)))
			{
				while (listAncestor != null && listAncestor.List != null && listAncestor.List.Parent is ListItem)
				{
					if (!TextRangeEditLists.UnindentListItems(new TextRange(start, TextRangeEditLists.GetPositionAfterList(listAncestor.List))))
					{
						return false;
					}
					listAncestor = start.GetListAncestor();
				}
			}
			ListItem listItem = end.GetListAncestor();
			if (listItem != null && listItem.List != null && !TextSchema.ValuesAreEqual(newFlowDirectionValue, listItem.List.GetValue(Block.FlowDirectionProperty)))
			{
				if (listAncestor == null || listAncestor.List == null || listItem.List.ElementEnd.CompareTo(listAncestor.List.ElementEnd) >= 0)
				{
					while (listItem != null && listItem.List != null && listItem.List.Parent is ListItem)
					{
						if (!TextRangeEditLists.UnindentListItems(new TextRange(listItem.List.ContentStart, TextRangeEditLists.GetPositionAfterList(listItem.List))))
						{
							return false;
						}
						listItem = end.GetListAncestor();
					}
				}
			}
			if ((listAncestor = start.GetListAncestor()) != null && listAncestor.PreviousListItem != null && listAncestor.List != null && !TextSchema.ValuesAreEqual(newFlowDirectionValue, listAncestor.List.GetValue(Block.FlowDirectionProperty)))
			{
				Invariant.Assert(!(listAncestor.List.Parent is ListItem), "startListItem's list must not be nested!");
				TextRangeEdit.SplitElement(listAncestor.ElementStart);
			}
			if ((listItem = end.GetListAncestor()) != null && listItem.List != null && !TextSchema.ValuesAreEqual(newFlowDirectionValue, listItem.List.GetValue(Block.FlowDirectionProperty)))
			{
				if (listItem.List.Parent is ListItem)
				{
					while (listItem.List != null && listItem.List.Parent is ListItem)
					{
						listItem = (ListItem)listItem.List.Parent;
					}
				}
				if (listItem.List != null && listItem.NextListItem != null)
				{
					Invariant.Assert(!(listItem.List.Parent is ListItem), "endListItem's list must not be nested!");
					TextRangeEdit.SplitElement(listItem.ElementEnd);
				}
			}
			return true;
		}

		// Token: 0x06003B8A RID: 15242 RVA: 0x0010F874 File Offset: 0x0010DA74
		private static TextPointer GetPositionAfterList(List list)
		{
			Invariant.Assert(list != null, "list cannot be null");
			TextPointer textPointer = list.ElementEnd.GetInsertionPosition(LogicalDirection.Backward);
			if (textPointer != null)
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			if (textPointer == null)
			{
				textPointer = list.ElementEnd.TextContainer.End;
			}
			if (TextRangeEditTables.IsTableStructureCrossed(list.ElementEnd, textPointer))
			{
				textPointer = list.ContentEnd;
			}
			return textPointer;
		}
	}
}
