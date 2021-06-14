using System;
using System.Collections.Generic;
using System.Windows.Documents.Internal;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.PtsHost;

namespace System.Windows.Documents
{
	// Token: 0x0200040F RID: 1039
	internal static class TextRangeEditTables
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x0010F8D4 File Offset: 0x0010DAD4
		internal static bool GetColumnRange(ITextRange range, Table table, out int firstColumnIndex, out int lastColumnIndex)
		{
			firstColumnIndex = -1;
			lastColumnIndex = -1;
			if (range == null || !range.IsTableCellRange)
			{
				return false;
			}
			if (!(range.Start is TextPointer))
			{
				return false;
			}
			if (table != TextRangeEditTables.GetTableFromPosition((TextPointer)range.TextSegments[0].Start))
			{
				return false;
			}
			TableCell tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition((TextPointer)range.TextSegments[0].Start);
			if (tableCellFromPosition == null)
			{
				return false;
			}
			TextPointer textPointer = (TextPointer)range.TextSegments[0].End.GetNextInsertionPosition(LogicalDirection.Backward);
			Invariant.Assert(textPointer != null, "lastCellPointer cannot be null here. Even empty table cells have a potential insertion position.");
			TableCell tableCellFromPosition2 = TextRangeEditTables.GetTableCellFromPosition(textPointer);
			if (tableCellFromPosition2 == null)
			{
				return false;
			}
			if (tableCellFromPosition.ColumnIndex < 0 || tableCellFromPosition2.ColumnIndex < 0)
			{
				return false;
			}
			firstColumnIndex = tableCellFromPosition.ColumnIndex;
			lastColumnIndex = tableCellFromPosition2.ColumnIndex + tableCellFromPosition2.ColumnSpan - 1;
			Invariant.Assert(firstColumnIndex <= lastColumnIndex, string.Concat(new object[]
			{
				"expecting: firstColumnIndex <= lastColumnIndex. Actual values: ",
				firstColumnIndex,
				" and ",
				lastColumnIndex
			}));
			return true;
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0010F9F0 File Offset: 0x0010DBF0
		internal static Table GetTableFromPosition(TextPointer position)
		{
			TextElement textElement = position.Parent as TextElement;
			while (textElement != null && !(textElement is Table))
			{
				textElement = (textElement.Parent as TextElement);
			}
			return textElement as Table;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0010FA28 File Offset: 0x0010DC28
		private static TableRow GetTableRowFromPosition(TextPointer position)
		{
			TextElement textElement = position.Parent as TextElement;
			while (textElement != null && !(textElement is TableRow))
			{
				textElement = (textElement.Parent as TextElement);
			}
			return textElement as TableRow;
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0010FA60 File Offset: 0x0010DC60
		internal static TableCell GetTableCellFromPosition(TextPointer position)
		{
			TextElement textElement = position.Parent as TextElement;
			while (textElement != null && !(textElement is TableCell))
			{
				textElement = (textElement.Parent as TextElement);
			}
			return textElement as TableCell;
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0010FA98 File Offset: 0x0010DC98
		internal static bool IsTableStructureCrossed(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			TableCell tableCell;
			TableCell tableCell2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			return anchorPosition is TextPointer && movingPosition is TextPointer && TextRangeEditTables.IdentifyTableElements((TextPointer)anchorPosition, (TextPointer)movingPosition, true, out tableCell, out tableCell2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2);
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0010FADC File Offset: 0x0010DCDC
		internal static bool IsTableCellRange(TextPointer anchorPosition, TextPointer movingPosition, bool includeCellAtMovingPosition, out TableCell anchorCell, out TableCell movingCell)
		{
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			return TextRangeEditTables.IdentifyTableElements(anchorPosition, movingPosition, includeCellAtMovingPosition, out anchorCell, out movingCell, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2) && anchorCell != null && movingCell != null;
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0010FB10 File Offset: 0x0010DD10
		internal static List<TextSegment> BuildTableRange(TextPointer anchorPosition, TextPointer movingPosition, bool includeCellAtMovingPosition, out bool isTableCellRange)
		{
			TableCell tableCell;
			TableCell tableCell2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			if (!TextRangeEditTables.IdentifyTableElements(anchorPosition, movingPosition, includeCellAtMovingPosition, out tableCell, out tableCell2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2))
			{
				isTableCellRange = false;
				return null;
			}
			if (tableCell != null && tableCell2 != null)
			{
				isTableCellRange = true;
				return TextRangeEditTables.BuildCellSelection(tableCell, tableCell2);
			}
			if (tableRow != null || tableRow2 != null || tableRowGroup != null || tableRowGroup2 != null || table != null || table2 != null)
			{
				isTableCellRange = false;
				return TextRangeEditTables.BuildCrossTableSelection(anchorPosition, movingPosition, tableRow, tableRow2);
			}
			isTableCellRange = false;
			return null;
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0010FB74 File Offset: 0x0010DD74
		private static List<TextSegment> BuildCellSelection(TableCell anchorCell, TableCell movingCell)
		{
			TableRowGroup rowGroup = anchorCell.Row.RowGroup;
			int num = Math.Min(anchorCell.Row.Index, movingCell.Row.Index);
			int num2 = Math.Max(anchorCell.Row.Index + anchorCell.RowSpan - 1, movingCell.Row.Index + movingCell.RowSpan - 1);
			int num3 = Math.Min(anchorCell.ColumnIndex, movingCell.ColumnIndex);
			int num4 = Math.Max(anchorCell.ColumnIndex + anchorCell.ColumnSpan - 1, movingCell.ColumnIndex + movingCell.ColumnSpan - 1);
			List<TextSegment> list = new List<TextSegment>(num2 - num + 1);
			int num5 = num;
			while (num5 <= num2 && num5 < rowGroup.Rows.Count)
			{
				TableCellCollection cells = rowGroup.Rows[num5].Cells;
				TableCell tableCell = null;
				TableCell tableCell2 = null;
				for (int i = 0; i < cells.Count; i++)
				{
					TableCell tableCell3 = cells[i];
					if (num3 <= tableCell3.ColumnIndex && tableCell3.ColumnIndex + tableCell3.ColumnSpan - 1 <= num4)
					{
						if (tableCell == null)
						{
							tableCell = tableCell3;
						}
						tableCell2 = tableCell3;
					}
				}
				if (tableCell != null && tableCell2 != null)
				{
					Invariant.Assert(tableCell.Row == tableCell2.Row, "Inconsistent Rows for segmentStartCell and segmentEndCell");
					Invariant.Assert(tableCell.Index <= tableCell2.Index, "Index of segmentStartCell must be <= index of segentEndCell");
					list.Add(TextRangeEditTables.NewNormalizedCellSegment(tableCell, tableCell2));
				}
				num5++;
			}
			return list;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0010FCF8 File Offset: 0x0010DEF8
		private static List<TextSegment> BuildCrossTableSelection(TextPointer anchorPosition, TextPointer movingPosition, TableRow anchorRow, TableRow movingRow)
		{
			List<TextSegment> list = new List<TextSegment>(1);
			if (anchorPosition.CompareTo(movingPosition) < 0)
			{
				list.Add(TextRangeEditTables.NewNormalizedTextSegment((anchorRow != null) ? anchorRow.ContentStart : anchorPosition, (movingRow != null) ? movingRow.ContentEnd : movingPosition));
			}
			else
			{
				list.Add(TextRangeEditTables.NewNormalizedTextSegment((movingRow != null) ? movingRow.ContentStart : movingPosition, (anchorRow != null) ? anchorRow.ContentEnd : anchorPosition));
			}
			return list;
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0010FD60 File Offset: 0x0010DF60
		internal static void IdentifyValidBoundaries(ITextRange range, out ITextPointer start, out ITextPointer end)
		{
			Invariant.Assert(range._IsTableCellRange, "Range must be in table cell range state");
			List<TextSegment> textSegments = range._TextSegments;
			start = null;
			end = null;
			for (int i = 0; i < textSegments.Count; i++)
			{
				TextSegment textSegment = textSegments[i];
				if (textSegment.Start.CompareTo(textSegment.End) != 0)
				{
					if (start == null)
					{
						start = textSegment.Start;
					}
					end = textSegment.End;
				}
			}
			if (start == null)
			{
				start = textSegments[0].Start;
				end = textSegments[textSegments.Count - 1].End;
			}
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0010FDFC File Offset: 0x0010DFFC
		internal static TextPointer GetNextTableCellRangeInsertionPosition(TextSelection selection, LogicalDirection direction)
		{
			Invariant.Assert(selection.IsTableCellRange, "TextSelection call this method only if selection is in TableCellRange state");
			TextPointer textPointer = selection.MovingPosition;
			TableCell tableCell;
			TableCell tableCell2;
			if (TextRangeEditTables.IsTableCellRange(selection.AnchorPosition, textPointer, false, out tableCell, out tableCell2))
			{
				Invariant.Assert(tableCell != null && tableCell2 != null, "anchorCell != null && movingCell != null");
				Invariant.Assert(tableCell.Row.RowGroup == tableCell2.Row.RowGroup, "anchorCell.Row.RowGroup == movingCell.Row.RowGroup");
				if (direction == LogicalDirection.Backward && tableCell2 == tableCell)
				{
					textPointer = tableCell.ContentEnd.GetInsertionPosition();
				}
				else if (direction == LogicalDirection.Forward && ((tableCell2.Row == tableCell.Row && tableCell2.Index + 1 == tableCell.Index) || (tableCell.Index == 0 && tableCell2.Index == tableCell2.Row.Cells.Count - 1 && tableCell2.Row.Index + 1 == tableCell.Row.Index)))
				{
					textPointer = tableCell.ContentStart.GetInsertionPosition();
				}
				else
				{
					TableRow row = tableCell2.Row;
					TableCellCollection cells = row.Cells;
					TableRowCollection rows = row.RowGroup.Rows;
					if (direction == LogicalDirection.Forward)
					{
						if (tableCell2.Index + 1 < cells.Count)
						{
							tableCell2 = cells[tableCell2.Index + 1];
						}
						else
						{
							int num = row.Index + 1;
							while (num < rows.Count && rows[num].Cells.Count == 0)
							{
								num++;
							}
							if (num < rows.Count)
							{
								tableCell2 = rows[num].Cells[0];
							}
							else
							{
								tableCell2 = null;
							}
						}
					}
					else if (tableCell2.Index > 0)
					{
						tableCell2 = cells[tableCell2.Index - 1];
					}
					else
					{
						int num2 = row.Index - 1;
						while (num2 >= 0 && rows[num2].Cells.Count == 0)
						{
							num2--;
						}
						if (num2 >= 0)
						{
							tableCell2 = rows[num2].Cells[rows[num2].Cells.Count - 1];
						}
						else
						{
							tableCell2 = null;
						}
					}
					if (tableCell2 != null)
					{
						if (tableCell2.ColumnIndex >= tableCell.ColumnIndex)
						{
							textPointer = tableCell2.ContentEnd.GetInsertionPosition().GetNextInsertionPosition(LogicalDirection.Forward);
						}
						else
						{
							textPointer = tableCell2.ContentStart.GetInsertionPosition();
						}
					}
					else
					{
						if (direction == LogicalDirection.Forward)
						{
							textPointer = tableCell.Table.ContentEnd;
						}
						else
						{
							textPointer = tableCell.Table.ContentStart;
						}
						textPointer = textPointer.GetNextInsertionPosition(direction);
					}
				}
			}
			return textPointer;
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x00110064 File Offset: 0x0010E264
		internal static TextPointer GetNextRowEndMovingPosition(TextSelection selection, LogicalDirection direction)
		{
			Invariant.Assert(!selection.IsTableCellRange);
			Invariant.Assert(TextPointerBase.IsAtRowEnd(selection.MovingPosition));
			TableRow tableRow = (TableRow)selection.MovingPosition.Parent;
			if (direction != LogicalDirection.Forward)
			{
				return tableRow.ContentStart.GetNextInsertionPosition(LogicalDirection.Backward);
			}
			return tableRow.ContentEnd.GetNextInsertionPosition(LogicalDirection.Forward);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x001100C0 File Offset: 0x0010E2C0
		internal static bool MovingPositionCrossesCellBoundary(TextSelection selection)
		{
			Invariant.Assert(((ITextRange)selection).Start is TextPointer);
			TableCell tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition(selection.MovingPosition);
			return tableCellFromPosition != null && !tableCellFromPosition.Contains(selection.AnchorPosition);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x00110100 File Offset: 0x0010E300
		internal static TextPointer GetNextRowStartMovingPosition(TextSelection selection, LogicalDirection direction)
		{
			Invariant.Assert(((ITextRange)selection).Start is TextPointer);
			Invariant.Assert(!selection.IsTableCellRange);
			TableCell tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition(selection.MovingPosition);
			Invariant.Assert(tableCellFromPosition != null);
			TableRow row = tableCellFromPosition.Row;
			if (direction != LogicalDirection.Forward)
			{
				return row.ContentStart.GetNextInsertionPosition(LogicalDirection.Backward);
			}
			return row.ContentEnd.GetNextInsertionPosition(LogicalDirection.Forward);
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x00110168 File Offset: 0x0010E368
		internal static Table InsertTable(TextPointer insertionPosition, int rowCount, int columnCount)
		{
			for (TextElement textElement = insertionPosition.Parent as TextElement; textElement != null; textElement = (textElement.Parent as TextElement))
			{
				if (textElement is List || (textElement is Inline && !TextSchema.IsMergeableInline(textElement.GetType())))
				{
					return null;
				}
			}
			insertionPosition = TextRangeEditTables.EnsureInsertionPosition(insertionPosition);
			if (insertionPosition.Paragraph == null)
			{
				return null;
			}
			insertionPosition = insertionPosition.InsertParagraphBreak();
			Paragraph paragraph = insertionPosition.Paragraph;
			Invariant.Assert(paragraph != null, "Expecting non-null paragraph at insertionPosition");
			Table table = new Table();
			table.CellSpacing = 0.0;
			TableRowGroup tableRowGroup = new TableRowGroup();
			for (int i = 0; i < rowCount; i++)
			{
				TableRow tableRow = new TableRow();
				for (int j = 0; j < columnCount; j++)
				{
					TableCell tableCell = new TableCell(new Paragraph());
					tableCell.BorderThickness = TextRangeEditTables.GetCellBorder(1.0, i, j, 1, 1, rowCount, columnCount);
					tableCell.BorderBrush = Brushes.Black;
					tableRow.Cells.Add(tableCell);
				}
				tableRowGroup.Rows.Add(tableRow);
			}
			table.RowGroups.Add(tableRowGroup);
			paragraph.SiblingBlocks.InsertBefore(paragraph, table);
			return table;
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x0011028E File Offset: 0x0010E48E
		private static Thickness GetCellBorder(double thickness, int rowIndex, int columnIndex, int rowSpan, int columnSpan, int rowCount, int columnCount)
		{
			return new Thickness(thickness, thickness, (columnIndex + columnSpan < columnCount) ? 0.0 : thickness, (rowIndex + rowSpan < rowCount) ? 0.0 : thickness);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x001102C0 File Offset: 0x0010E4C0
		internal static TextPointer EnsureInsertionPosition(TextPointer position)
		{
			Invariant.Assert(position != null, "null check: position");
			position = position.GetInsertionPosition(position.LogicalDirection);
			if (!TextPointerBase.IsAtInsertionPosition(position))
			{
				position = TextRangeEditTables.CreateInsertionPositionInIncompleteContent(position);
			}
			else
			{
				if (position.IsAtRowEnd)
				{
					Table tableFromPosition = TextRangeEditTables.GetTableFromPosition(position);
					position = TextRangeEditTables.GetAdjustedRowEndPosition(tableFromPosition, position);
					if (position.CompareTo(tableFromPosition.ElementEnd) == 0)
					{
						position = TextRangeEditTables.CreateImplicitParagraph(tableFromPosition.ElementEnd);
					}
				}
				Invariant.Assert(!position.IsAtRowEnd, "position is not expected to be at RowEnd anymore");
				if (TextPointerBase.IsInBlockUIContainer(position))
				{
					BlockUIContainer blockUIContainer = (BlockUIContainer)position.Parent;
					position = ((position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart) ? TextRangeEditTables.CreateImplicitParagraph(blockUIContainer.ElementStart) : TextRangeEditTables.CreateImplicitParagraph(blockUIContainer.ElementEnd));
					if (blockUIContainer.IsEmpty)
					{
						blockUIContainer.RepositionWithContent(null);
					}
				}
				else if (TextPointerBase.IsBeforeFirstTable(position) || TextPointerBase.IsAtPotentialParagraphPosition(position))
				{
					position = TextRangeEditTables.CreateImplicitParagraph(position);
				}
				else if (TextPointerBase.IsAtPotentialRunPosition(position))
				{
					position = TextRangeEditTables.CreateImplicitRun(position);
				}
			}
			Invariant.Assert(TextSchema.IsInTextContent(position), "position must be in text content now");
			return position;
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x001103CC File Offset: 0x0010E5CC
		internal static TextPointer GetAdjustedRowEndPosition(Table currentTable, TextPointer rowEndPosition)
		{
			TextPointer textPointer = rowEndPosition;
			while (textPointer != null && textPointer.IsAtRowEnd && currentTable == TextRangeEditTables.GetTableFromPosition(textPointer))
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
			}
			TextPointer result;
			if (textPointer != null && currentTable == TextRangeEditTables.GetTableFromPosition(textPointer))
			{
				result = textPointer;
			}
			else
			{
				result = currentTable.ElementEnd;
			}
			return result;
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x00110414 File Offset: 0x0010E614
		private static TextPointer CreateInsertionPositionInIncompleteContent(TextPointer position)
		{
			while (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
			{
				position = position.GetNextContextPosition(LogicalDirection.Forward);
			}
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd)
			{
				position = position.GetNextContextPosition(LogicalDirection.Backward);
			}
			DependencyObject parent = position.Parent;
			if (parent != null)
			{
				if (parent is Table)
				{
					TableRowGroup tableRowGroup = new TableRowGroup();
					tableRowGroup.Reposition(position, position);
					position = tableRowGroup.ContentStart;
					parent = position.Parent;
				}
				if (parent is TableRowGroup)
				{
					TableRow tableRow = new TableRow();
					tableRow.Reposition(position, position);
					position = tableRow.ContentStart;
					parent = position.Parent;
				}
				if (parent is TableRow)
				{
					TableCell tableCell = new TableCell();
					tableCell.Reposition(position, position);
					position = tableCell.ContentStart;
					parent = position.Parent;
				}
				if (parent is List)
				{
					ListItem listItem = new ListItem();
					listItem.Reposition(position, position);
					position = listItem.ContentStart;
					parent = position.Parent;
				}
				if (parent is LineBreak || parent is InlineUIContainer)
				{
					position = ((Inline)parent).ElementStart;
					parent = position.Parent;
				}
			}
			if (parent == null)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_CannotInsertContentInThisPosition"));
			}
			TextPointer result;
			if (TextSchema.IsValidChild(position, typeof(Inline)))
			{
				result = TextRangeEditTables.CreateImplicitRun(position);
			}
			else
			{
				Invariant.Assert(TextSchema.IsValidChild(position, typeof(Block)), "Expecting valid parent-child relationship");
				result = TextRangeEditTables.CreateImplicitParagraph(position);
			}
			return result;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x00110568 File Offset: 0x0010E768
		private static TextPointer CreateImplicitRun(TextPointer position)
		{
			TextPointer textPointer;
			if (position.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward) is Run)
			{
				textPointer = position.CreatePointer();
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				textPointer.Freeze();
			}
			else if (position.GetAdjacentElementFromOuterPosition(LogicalDirection.Backward) is Run)
			{
				textPointer = position.CreatePointer();
				textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
				textPointer.Freeze();
			}
			else
			{
				Run run = Inline.CreateImplicitRun(position.Parent);
				run.Reposition(position, position);
				textPointer = run.ContentStart.GetFrozenPointer(position.LogicalDirection);
			}
			return textPointer;
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x001105E8 File Offset: 0x0010E7E8
		private static TextPointer CreateImplicitParagraph(TextPointer position)
		{
			Paragraph paragraph = new Paragraph();
			paragraph.Reposition(position, position);
			Run run = Inline.CreateImplicitRun(paragraph);
			paragraph.Inlines.Add(run);
			return run.ContentStart.GetFrozenPointer(position.LogicalDirection);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0011062C File Offset: 0x0010E82C
		internal static void DeleteContent(TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) > 0)
			{
				TextPointer textPointer = end;
				end = start;
				start = textPointer;
			}
			TableCell tableCell;
			TableCell tableCell2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			while (start.CompareTo(end) < 0 && TextRangeEditTables.IdentifyTableElements(start, end, false, out tableCell, out tableCell2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2))
			{
				if ((table == null && table2 == null) || table == table2)
				{
					bool flag;
					List<TextSegment> list = TextRangeEditTables.BuildTableRange(start, end, false, out flag);
					if (flag && list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							TextRangeEditTables.ClearTableCells(list[i]);
						}
						end = start;
					}
					else
					{
						if (tableCell != null)
						{
							tableRow = tableCell.Row;
						}
						else if (tableRow == null && tableRowGroup != null)
						{
							tableRow = tableRowGroup.Rows[0];
						}
						if (tableCell2 != null)
						{
							tableRow2 = tableCell.Row;
						}
						else if (tableRow2 == null && tableRowGroup2 != null)
						{
							tableRow2 = tableRowGroup2.Rows[tableRowGroup2.Rows.Count - 1];
						}
						Invariant.Assert(tableRow != null && tableRow2 != null, "startRow and endRow cannot be null, since our range is within one table");
						TextRange textRange = new TextRange(tableRow.ContentStart, tableRow2.ContentEnd);
						TextRangeEditTables.DeleteRows(textRange);
					}
				}
				else
				{
					if (tableRow != null)
					{
						start = tableRow.Table.ElementEnd;
						TextRange textRange2 = new TextRange(tableRow.ContentStart, tableRow.Table.ContentEnd);
						TextRangeEditTables.DeleteRows(textRange2);
					}
					if (tableRow2 != null)
					{
						end = tableRow2.Table.ElementStart;
						TextRange textRange3 = new TextRange(tableRow2.Table.ContentStart, tableRow2.ContentEnd);
						TextRangeEditTables.DeleteRows(textRange3);
					}
				}
			}
			if (start.CompareTo(end) < 0)
			{
				TextRangeEdit.DeleteParagraphContent(start, end);
			}
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001107C0 File Offset: 0x0010E9C0
		private static void ClearTableCells(TextSegment textSegment)
		{
			TableCell tableCell = TextRangeEditTables.GetTableCellFromPosition((TextPointer)textSegment.Start);
			TextPointer nextInsertionPosition = ((TextPointer)textSegment.End).GetNextInsertionPosition(LogicalDirection.Backward);
			while (tableCell != null)
			{
				tableCell.Blocks.Clear();
				tableCell.Blocks.Add(new Paragraph());
				TextPointer elementEnd = tableCell.ElementEnd;
				if (elementEnd.CompareTo(nextInsertionPosition) < 0 && elementEnd.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					tableCell = (TableCell)elementEnd.GetAdjacentElement(LogicalDirection.Forward);
				}
				else
				{
					tableCell = null;
				}
			}
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x00110840 File Offset: 0x0010EA40
		internal static TextRange InsertRows(TextRange textRange, int rowCount)
		{
			Invariant.Assert(textRange != null, "null check: textRange");
			TextPointer position = (rowCount > 0) ? textRange.End : textRange.Start;
			TableRow tableRowFromPosition = TextRangeEditTables.GetTableRowFromPosition(position);
			if (tableRowFromPosition == null || rowCount == 0)
			{
				return new TextRange(textRange.Start, textRange.Start);
			}
			TableCell[] spannedCells = tableRowFromPosition.SpannedCells;
			if (spannedCells != null)
			{
				foreach (TableCell tableCell in spannedCells)
				{
					tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.RowSpanProperty, tableCell.RowSpan + ((rowCount > 0) ? rowCount : (-rowCount)));
				}
			}
			TableRowGroup rowGroup = tableRowFromPosition.RowGroup;
			int num = rowGroup.Rows.IndexOf(tableRowFromPosition);
			if (rowCount > 0)
			{
				num++;
			}
			TableRow tableRow = null;
			TableRow tableRow2 = null;
			while (rowCount != 0)
			{
				TableRow tableRow3 = TextRangeEditTables.CopyRow(tableRowFromPosition);
				if (tableRow == null)
				{
					tableRow = tableRow3;
				}
				tableRow2 = tableRow3;
				TableCellCollection cells = tableRowFromPosition.Cells;
				for (int j = 0; j < cells.Count; j++)
				{
					TableCell tableCell2 = cells[j];
					if (rowCount < 0 || tableCell2.RowSpan == 1)
					{
						TextRangeEditTables.AddCellCopy(tableRow3, tableCell2, -1, false, true);
					}
				}
				rowGroup.Rows.Insert(num, tableRow3);
				if (rowCount > 0)
				{
					num++;
				}
				rowCount -= ((rowCount > 0) ? 1 : -1);
			}
			TextRangeEditTables.CorrectBorders(rowGroup.Rows);
			if (rowCount <= 0)
			{
				return new TextRange(tableRow2.ContentStart, tableRow.ContentEnd);
			}
			return new TextRange(tableRow.ContentStart, tableRow2.ContentEnd);
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001109C0 File Offset: 0x0010EBC0
		internal static bool DeleteRows(TextRange textRange)
		{
			Invariant.Assert(textRange != null, "null check: textRange");
			TableRow tableRowFromPosition = TextRangeEditTables.GetTableRowFromPosition(textRange.Start);
			TableRow tableRowFromPosition2 = TextRangeEditTables.GetTableRowFromPosition(textRange.End);
			if (tableRowFromPosition == null || tableRowFromPosition2 == null || tableRowFromPosition.RowGroup != tableRowFromPosition2.RowGroup)
			{
				return false;
			}
			TableRowCollection rows = tableRowFromPosition.RowGroup.Rows;
			int num = tableRowFromPosition2.Index - tableRowFromPosition.Index + 1;
			if (num == rows.Count)
			{
				Table table = tableRowFromPosition.Table;
				table.RepositionWithContent(null);
			}
			else
			{
				if (tableRowFromPosition2.Index != rows.Count - 1)
				{
					TextRangeEditTables.CorrectRowSpansOnDeleteRows(rows[tableRowFromPosition2.Index + 1], num);
				}
				rows.RemoveRange(tableRowFromPosition.Index, tableRowFromPosition2.Index - tableRowFromPosition.Index + 1);
				Invariant.Assert(rows.Count > 0);
				TextRangeEditTables.CorrectBorders(rows);
			}
			textRange.Select(textRange.Start, textRange.Start);
			return true;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x00110AAC File Offset: 0x0010ECAC
		private static void CorrectBorders(TableRowCollection rows)
		{
			Table table = rows[0].Table;
			if (table.CellSpacing > 0.0)
			{
				return;
			}
			int columnCount = table.ColumnCount;
			int count = rows.Count;
			for (int i = 0; i < count; i++)
			{
				TableRow tableRow = rows[i];
				TableCellCollection cells = tableRow.Cells;
				for (int j = 0; j < cells.Count; j++)
				{
					TableCell tableCell = cells[j];
					Thickness borderThickness = tableCell.BorderThickness;
					double num = (tableCell.ColumnIndex + tableCell.ColumnSpan < columnCount) ? 0.0 : borderThickness.Left;
					double num2 = (i + tableCell.RowSpan < count) ? 0.0 : borderThickness.Top;
					if (borderThickness.Right != num || borderThickness.Bottom != num2)
					{
						borderThickness.Right = num;
						borderThickness.Bottom = num2;
						tableCell.BorderThickness = borderThickness;
					}
				}
			}
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x00110BB4 File Offset: 0x0010EDB4
		private static void CorrectRowSpansOnDeleteRows(TableRow nextRow, int deletedRowsCount)
		{
			Invariant.Assert(nextRow != null, "null check: nextRow");
			Invariant.Assert(nextRow.Index >= deletedRowsCount, "nextRow.Index is expected to be >= deletedRowsCount");
			TableCellCollection cells = nextRow.Cells;
			TableCell[] spannedCells = nextRow.SpannedCells;
			if (spannedCells != null)
			{
				int num = 0;
				foreach (TableCell tableCell in spannedCells)
				{
					int index = tableCell.Row.Index;
					if (index < nextRow.Index)
					{
						if (index < nextRow.Index - deletedRowsCount)
						{
							Invariant.Assert(tableCell.RowSpan > deletedRowsCount, "spannedCell.RowSpan is expected to be > deletedRowsCount");
							tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.RowSpanProperty, tableCell.RowSpan - deletedRowsCount);
						}
						else
						{
							int columnIndex = tableCell.ColumnIndex;
							while (num < cells.Count && cells[num].ColumnIndex < columnIndex)
							{
								num++;
							}
							TableCell tableCell2 = TextRangeEditTables.AddCellCopy(nextRow, tableCell, num, false, true);
							Invariant.Assert(tableCell.RowSpan - (nextRow.Index - tableCell.Row.Index) > 0, "expecting: spannedCell.RowSpan - (nextRow.Index - spannedCell.Row.Index) > 0");
							tableCell2.ContentStart.TextContainer.SetValue(tableCell2.ContentStart, TableCell.RowSpanProperty, tableCell.RowSpan - (nextRow.Index - tableCell.Row.Index));
							num++;
						}
					}
				}
			}
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x00110D20 File Offset: 0x0010EF20
		private static void InsertColumn(int colIndex, Table table)
		{
			for (int i = 0; i < table.RowGroups.Count; i++)
			{
				TableRowGroup tableRowGroup = table.RowGroups[i];
				for (int j = 0; j < tableRowGroup.Rows.Count; j++)
				{
					TableRow tableRow = tableRowGroup.Rows[j];
					if (colIndex == -1)
					{
						if (tableRow.Cells[0].ColumnIndex == 0)
						{
							TextRangeEditTables.AddCellCopy(tableRow, tableRow.Cells[0], 0, true, false);
						}
					}
					else
					{
						TableCell tableCell = null;
						int k = 0;
						while (k < tableRow.Cells.Count)
						{
							TableCell tableCell2 = tableRow.Cells[k];
							if (tableCell2.ColumnIndex + tableCell2.ColumnSpan > colIndex)
							{
								if (tableCell2.ColumnIndex <= colIndex)
								{
									tableCell = tableCell2;
									break;
								}
								break;
							}
							else
							{
								k++;
							}
						}
						if (tableCell != null)
						{
							if (tableCell.ColumnSpan == 1)
							{
								TextRangeEditTables.AddCellCopy(tableRow, tableCell, tableRow.Cells.IndexOf(tableCell) + 1, true, true);
							}
							else
							{
								tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.ColumnSpanProperty, tableCell.ColumnSpan + 1);
							}
						}
					}
				}
				TextRangeEditTables.CorrectBorders(tableRowGroup.Rows);
			}
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x00110E64 File Offset: 0x0010F064
		internal static TextRange InsertColumns(TextRange textRange, int columnCount)
		{
			int num = Math.Abs(columnCount);
			Invariant.Assert(textRange != null, "null check: textRange");
			TableCell tableCellFromPosition;
			TableCell tableCellFromPosition2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			if (!TextRangeEditTables.IdentifyTableElements(textRange.Start, textRange.End, false, out tableCellFromPosition, out tableCellFromPosition2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2))
			{
				if (textRange.IsTableCellRange)
				{
					return null;
				}
				tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition(textRange.Start);
				tableCellFromPosition2 = TextRangeEditTables.GetTableCellFromPosition(textRange.End);
				if (tableCellFromPosition == null || tableCellFromPosition != tableCellFromPosition2)
				{
					return null;
				}
			}
			int num2 = tableCellFromPosition2.ColumnIndex + tableCellFromPosition2.ColumnSpan - 1;
			for (int i = 0; i < num; i++)
			{
				if (columnCount < 0)
				{
					TextRangeEditTables.InsertColumn(num2 - 1, tableCellFromPosition2.Table);
				}
				else
				{
					TextRangeEditTables.InsertColumn(num2, tableCellFromPosition2.Table);
				}
			}
			return null;
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x00110F1C File Offset: 0x0010F11C
		internal static void DeleteColumn(int colIndex, Table table)
		{
			for (int i = 0; i < table.RowGroups.Count; i++)
			{
				TableRowGroup tableRowGroup = table.RowGroups[i];
				for (int j = 0; j < tableRowGroup.Rows.Count; j++)
				{
					TableRow tableRow = tableRowGroup.Rows[j];
					TableCell tableCell = null;
					int k = 0;
					while (k < tableRow.Cells.Count)
					{
						TableCell tableCell2 = tableRow.Cells[k];
						if (tableCell2.ColumnIndex + tableCell2.ColumnSpan > colIndex)
						{
							if (tableCell2.ColumnIndex <= colIndex)
							{
								tableCell = tableCell2;
								break;
							}
							break;
						}
						else
						{
							k++;
						}
					}
					if (tableCell != null)
					{
						if (tableCell.ColumnSpan == 1)
						{
							tableRow.Cells.Remove(tableCell);
						}
						else
						{
							tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.ColumnSpanProperty, tableCell.ColumnSpan - 1);
						}
					}
				}
				TextRangeEditTables.CorrectBorders(tableRowGroup.Rows);
			}
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x00111020 File Offset: 0x0010F220
		internal static bool DeleteColumns(TextRange textRange)
		{
			Invariant.Assert(textRange != null, "null check: textRange");
			TableCell tableCell;
			TableCell tableCell2;
			if (!TextRangeEditTables.IsTableCellRange(textRange.Start, textRange.End, false, out tableCell, out tableCell2))
			{
				return false;
			}
			int columnIndex = tableCell.ColumnIndex;
			int num = tableCell2.ColumnIndex - tableCell.ColumnIndex + 1;
			if (num == 0 || num == tableCell.Table.ColumnCount)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				TextRangeEditTables.DeleteColumn(columnIndex, tableCell2.Table);
			}
			return true;
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x0011109C File Offset: 0x0010F29C
		internal static bool TableBorderHitTest(ITextView textView, Point pt)
		{
			Table table;
			int num;
			Rect rect;
			double num2;
			double[] array;
			return TextRangeEditTables.TableBorderHitTest(textView, pt, out table, out num, out rect, out num2, out array);
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x001110BC File Offset: 0x0010F2BC
		private static bool TableBorderHitTest(ITextView textView, Point point, out Table table, out int columnIndex, out Rect columnRect, out double tableAutofitWidth, out double[] columnWidths)
		{
			table = null;
			columnIndex = -1;
			columnRect = Rect.Empty;
			tableAutofitWidth = 0.0;
			columnWidths = null;
			if (!(textView is TextDocumentView))
			{
				return false;
			}
			TextDocumentView textDocumentView = (TextDocumentView)textView;
			CellInfo cellInfoFromPoint = textDocumentView.GetCellInfoFromPoint(point, null);
			if (cellInfoFromPoint == null)
			{
				return false;
			}
			if (point.Y < cellInfoFromPoint.TableArea.Top || point.Y > cellInfoFromPoint.TableArea.Bottom)
			{
				return false;
			}
			double num = 1.0;
			TableCell cell = cellInfoFromPoint.Cell;
			if (cell.ColumnIndex != 0 && point.X < cellInfoFromPoint.CellArea.Left + num)
			{
				columnIndex = cellInfoFromPoint.Cell.ColumnIndex - 1;
				columnRect = new Rect(cellInfoFromPoint.CellArea.Left, cellInfoFromPoint.TableArea.Top, 1.0, cellInfoFromPoint.TableArea.Height);
			}
			if (cell.ColumnIndex + cell.ColumnSpan <= cell.Table.ColumnCount && point.X > cellInfoFromPoint.CellArea.Right - num && (!TextRangeEditTables.IsLastCellInRow(cell) || point.X < cellInfoFromPoint.CellArea.Right + num))
			{
				columnIndex = cell.ColumnIndex + cell.ColumnSpan - 1;
				columnRect = new Rect(cellInfoFromPoint.CellArea.Right, cellInfoFromPoint.TableArea.Top, 1.0, cellInfoFromPoint.TableArea.Height);
			}
			if (columnIndex == -1)
			{
				return false;
			}
			table = cell.Table;
			tableAutofitWidth = cellInfoFromPoint.TableAutofitWidth;
			columnWidths = cellInfoFromPoint.TableColumnWidths;
			return true;
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x00111290 File Offset: 0x0010F490
		internal static TextRangeEditTables.TableColumnResizeInfo StartColumnResize(ITextView textView, Point pt)
		{
			Table table;
			int columnIndex;
			Rect columnRect;
			double tableAutofitWidth;
			double[] columnWidths;
			if (TextRangeEditTables.TableBorderHitTest(textView, pt, out table, out columnIndex, out columnRect, out tableAutofitWidth, out columnWidths))
			{
				return new TextRangeEditTables.TableColumnResizeInfo(textView, table, columnIndex, columnRect, tableAutofitWidth, columnWidths);
			}
			return null;
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x001112C0 File Offset: 0x0010F4C0
		internal static void EnsureTableColumnsAreFixedSize(Table table, double[] columnWidths)
		{
			while (table.Columns.Count < columnWidths.Length)
			{
				table.Columns.Add(new TableColumn());
			}
			for (int i = 0; i < table.ColumnCount; i++)
			{
				table.Columns[i].Width = new GridLength(columnWidths[i]);
			}
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x0011131C File Offset: 0x0010F51C
		internal static TextRange MergeCells(TextRange textRange)
		{
			Invariant.Assert(textRange != null, "null check: textRange");
			TableCell tableCell;
			TableCell tableCell2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			if (!TextRangeEditTables.IdentifyTableElements(textRange.Start, textRange.End, false, out tableCell, out tableCell2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2))
			{
				return null;
			}
			if (tableCell == null || tableCell2 == null)
			{
				return null;
			}
			Invariant.Assert(tableCell.Row.RowGroup == tableCell2.Row.RowGroup, "startCell and endCell must belong to the same RowGroup");
			Invariant.Assert(tableCell.Row.Index <= tableCell2.Row.Index, "startCell.Row.Index must be <= endCell.Row.Index");
			Invariant.Assert(tableCell.ColumnIndex <= tableCell2.ColumnIndex + tableCell2.ColumnSpan - 1, "startCell.ColumnIndex must be <= an index+span of an endCell");
			TextRange textRange2 = TextRangeEditTables.MergeCellRange(tableCell.Row.RowGroup, tableCell.Row.Index, tableCell2.Row.Index + tableCell2.RowSpan - 1, tableCell.ColumnIndex, tableCell2.ColumnIndex + tableCell2.ColumnSpan - 1);
			if (textRange2 != null)
			{
				textRange.Select(textRange.Start, textRange.End);
			}
			return textRange2;
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00111430 File Offset: 0x0010F630
		internal static TextRange SplitCell(TextRange textRange, int splitCountHorizontal, int splitCountVertical)
		{
			Invariant.Assert(textRange != null, "null check: textRange");
			TableCell tableCell;
			TableCell tableCell2;
			TableRow tableRow;
			TableRow tableRow2;
			TableRowGroup tableRowGroup;
			TableRowGroup tableRowGroup2;
			Table table;
			Table table2;
			if (!TextRangeEditTables.IdentifyTableElements(textRange.Start, textRange.End, false, out tableCell, out tableCell2, out tableRow, out tableRow2, out tableRowGroup, out tableRowGroup2, out table, out table2))
			{
				return null;
			}
			if (tableCell == null || tableCell != tableCell2)
			{
				return null;
			}
			if (tableCell.ColumnSpan == 1 && tableCell.RowSpan == 1)
			{
				return null;
			}
			TableRowGroup rowGroup = tableCell.Row.RowGroup;
			TableCellCollection cells = tableCell.Row.Cells;
			int index = tableCell.Index;
			if (splitCountHorizontal > tableCell.ColumnSpan - 1)
			{
				splitCountHorizontal = tableCell.ColumnSpan - 1;
			}
			Invariant.Assert(splitCountHorizontal >= 0, "expecting: splitCountHorizontal >= 0");
			if (splitCountVertical > tableCell.RowSpan - 1)
			{
				splitCountVertical = tableCell.RowSpan - 1;
			}
			Invariant.Assert(splitCountVertical >= 0, "expecting; splitCoutVertical >= 0");
			while (splitCountHorizontal > 0)
			{
				TextRangeEditTables.AddCellCopy(tableCell.Row, tableCell, index + 1, true, false);
				tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.ColumnSpanProperty, tableCell.ColumnSpan - 1);
				if (tableCell.ColumnSpan == 1)
				{
					tableCell.ClearValue(TableCell.ColumnSpanProperty);
				}
				splitCountHorizontal--;
			}
			TextRangeEditTables.CorrectBorders(rowGroup.Rows);
			return new TextRange(tableCell.ContentStart, tableCell.ContentStart);
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00111573 File Offset: 0x0010F773
		private static TextSegment NewNormalizedTextSegment(TextPointer startPosition, TextPointer endPosition)
		{
			startPosition = startPosition.GetInsertionPosition(LogicalDirection.Forward);
			if (!TextPointerBase.IsAfterLastParagraph(endPosition))
			{
				endPosition = endPosition.GetInsertionPosition(LogicalDirection.Backward);
			}
			if (startPosition.CompareTo(endPosition) < 0)
			{
				return new TextSegment(startPosition, endPosition);
			}
			return new TextSegment(startPosition, startPosition);
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x001115A8 File Offset: 0x0010F7A8
		private static TextSegment NewNormalizedCellSegment(TableCell startCell, TableCell endCell)
		{
			Invariant.Assert(startCell.Row == endCell.Row, "startCell and endCell must be in the same Row");
			Invariant.Assert(startCell.Index <= endCell.Index, "insed of a startCell mustbe <= an index of an endCell");
			TextPointer insertionPosition = startCell.ContentStart.GetInsertionPosition(LogicalDirection.Forward);
			TextPointer nextInsertionPosition = endCell.ContentEnd.GetNextInsertionPosition(LogicalDirection.Forward);
			Invariant.Assert(TextRangeEditTables.GetTableRowFromPosition(nextInsertionPosition) == TextRangeEditTables.GetTableRowFromPosition(endCell.ContentEnd), "Inconsistent Rows on end");
			Invariant.Assert(insertionPosition.CompareTo(nextInsertionPosition) < 0, "The end must be in the beginning of the next cell (or at row end).");
			Invariant.Assert(TextRangeEditTables.GetTableRowFromPosition(insertionPosition) == TextRangeEditTables.GetTableRowFromPosition(nextInsertionPosition), "Inconsistent Rows for start and end");
			return new TextSegment(insertionPosition, nextInsertionPosition);
		}

		// Token: 0x06003BB2 RID: 15282 RVA: 0x00111654 File Offset: 0x0010F854
		private static bool IdentifyTableElements(TextPointer anchorPosition, TextPointer movingPosition, bool includeCellAtMovingPosition, out TableCell anchorCell, out TableCell movingCell, out TableRow anchorRow, out TableRow movingRow, out TableRowGroup anchorRowGroup, out TableRowGroup movingRowGroup, out Table anchorTable, out Table movingTable)
		{
			anchorPosition = anchorPosition.GetInsertionPosition(LogicalDirection.Forward);
			if (!TextPointerBase.IsAfterLastParagraph(movingPosition))
			{
				movingPosition = movingPosition.GetInsertionPosition(LogicalDirection.Backward);
			}
			if (!TextRangeEditTables.FindTableElements(anchorPosition, movingPosition, out anchorCell, out movingCell, out anchorRow, out movingRow, out anchorRowGroup, out movingRowGroup, out anchorTable, out movingTable))
			{
				return false;
			}
			if (anchorTable != null || movingTable != null)
			{
				anchorCell = null;
				movingCell = null;
			}
			else if (anchorCell != null && movingCell != null)
			{
				if (!includeCellAtMovingPosition && movingPosition.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && movingCell.ColumnIndex > anchorCell.ColumnIndex + anchorCell.ColumnSpan - 1 && movingCell.Index > 0)
				{
					movingCell = movingCell.Row.Cells[movingCell.Index - 1];
				}
			}
			else if (anchorCell != null && movingCell == null && movingPosition.IsAtRowEnd)
			{
				TableRow tableRow = movingPosition.Parent as TableRow;
				movingCell = tableRow.Cells[tableRow.Cells.Count - 1];
			}
			else
			{
				anchorCell = null;
				movingCell = null;
			}
			return anchorCell != null || movingCell != null || anchorRow != null || movingRow != null || anchorRowGroup != null || movingRowGroup != null || anchorTable != null || movingTable != null;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x0011177C File Offset: 0x0010F97C
		private static bool FindTableElements(TextPointer anchorPosition, TextPointer movingPosition, out TableCell anchorCell, out TableCell movingCell, out TableRow anchorRow, out TableRow movingRow, out TableRowGroup anchorRowGroup, out TableRowGroup movingRowGroup, out Table anchorTable, out Table movingTable)
		{
			if (anchorPosition.Parent == movingPosition.Parent)
			{
				anchorCell = null;
				movingCell = null;
				anchorRow = null;
				movingRow = null;
				anchorRowGroup = null;
				movingRowGroup = null;
				anchorTable = null;
				movingTable = null;
				return false;
			}
			TextElement textElement = anchorPosition.Parent as TextElement;
			while (textElement != null && !textElement.Contains(movingPosition))
			{
				textElement = (textElement.Parent as TextElement);
			}
			TextRangeEditTables.FindTableElements(textElement, anchorPosition, out anchorCell, out anchorRow, out anchorRowGroup, out anchorTable);
			TextRangeEditTables.FindTableElements(textElement, movingPosition, out movingCell, out movingRow, out movingRowGroup, out movingTable);
			return anchorCell != null || movingCell != null || anchorRow != null || movingRow != null || anchorRowGroup != null || movingRowGroup != null || anchorTable != null || movingTable != null;
		}

		// Token: 0x06003BB4 RID: 15284 RVA: 0x00111824 File Offset: 0x0010FA24
		private static void FindTableElements(TextElement commonAncestor, TextPointer position, out TableCell cell, out TableRow row, out TableRowGroup rowGroup, out Table table)
		{
			cell = null;
			row = null;
			rowGroup = null;
			table = null;
			for (TextElement textElement = position.Parent as TextElement; textElement != commonAncestor; textElement = (textElement.Parent as TextElement))
			{
				Invariant.Assert(textElement != null, "Not expecting null for element: otherwise it must hit commonAncestor which must be null in this case...");
				if (textElement is TableCell)
				{
					cell = (TableCell)textElement;
					row = null;
					rowGroup = null;
					table = null;
				}
				else if (textElement is TableRow)
				{
					row = (TableRow)textElement;
					rowGroup = null;
					table = null;
				}
				else if (textElement is TableRowGroup)
				{
					rowGroup = (TableRowGroup)textElement;
					table = null;
				}
				else if (textElement is Table)
				{
					table = (Table)textElement;
				}
			}
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001118CC File Offset: 0x0010FACC
		private static TableRow CopyRow(TableRow currentRow)
		{
			Invariant.Assert(currentRow != null, "null check: currentRow");
			TableRow tableRow = new TableRow();
			LocalValueEnumerator localValueEnumerator = currentRow.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if (!localValueEntry.Property.ReadOnly)
				{
					tableRow.SetValue(localValueEntry.Property, localValueEntry.Value);
				}
			}
			return tableRow;
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x0011192C File Offset: 0x0010FB2C
		private static TableCell AddCellCopy(TableRow newRow, TableCell currentCell, int cellInsertionIndex, bool copyRowSpan, bool copyColumnSpan)
		{
			Invariant.Assert(currentCell != null, "null check: currentCell");
			TableCell tableCell = new TableCell();
			if (cellInsertionIndex < 0)
			{
				newRow.Cells.Add(tableCell);
			}
			else
			{
				newRow.Cells.Insert(cellInsertionIndex, tableCell);
			}
			LocalValueEnumerator localValueEnumerator = currentCell.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if ((localValueEntry.Property != TableCell.RowSpanProperty || copyRowSpan) && (localValueEntry.Property != TableCell.ColumnSpanProperty || copyColumnSpan) && !localValueEntry.Property.ReadOnly)
				{
					tableCell.SetValue(localValueEntry.Property, localValueEntry.Value);
				}
			}
			if (currentCell.Blocks.FirstBlock != null)
			{
				Paragraph paragraph = new Paragraph();
				Paragraph paragraph2 = currentCell.Blocks.FirstBlock as Paragraph;
				if (paragraph2 != null)
				{
					DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(typeof(Paragraph));
					foreach (DependencyProperty dp in TextSchema.GetNoninheritableProperties(typeof(Paragraph)))
					{
						object obj = paragraph2.ReadLocalValue(dp);
						if (obj != DependencyProperty.UnsetValue)
						{
							paragraph.SetValue(dp, obj);
						}
					}
					foreach (DependencyProperty dp2 in inheritableProperties)
					{
						object obj2 = paragraph2.ReadLocalValue(dp2);
						if (obj2 != DependencyProperty.UnsetValue)
						{
							paragraph.SetValue(dp2, obj2);
						}
					}
				}
				tableCell.Blocks.Add(paragraph);
			}
			return tableCell;
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x00111A9C File Offset: 0x0010FC9C
		private static TextRange MergeCellRange(TableRowGroup rowGroup, int topRow, int bottomRow, int leftColumn, int rightColumn)
		{
			Invariant.Assert(rowGroup != null, "null check: rowGroup");
			Invariant.Assert(topRow >= 0, "topRow must be >= 0");
			Invariant.Assert(bottomRow >= 0, "bottomRow must be >= 0");
			Invariant.Assert(leftColumn >= 0, "leftColumn must be >= 0");
			Invariant.Assert(rightColumn >= 0, "rightColumn must be >= 0");
			Invariant.Assert(topRow <= bottomRow, "topRow must be <= bottomRow");
			Invariant.Assert(leftColumn <= rightColumn, "leftColumn must be <= rightColumn");
			if (!TextRangeEditTables.CanMergeCellRange(rowGroup, topRow, bottomRow, leftColumn, rightColumn))
			{
				return null;
			}
			return TextRangeEditTables.DoMergeCellRange(rowGroup, topRow, bottomRow, leftColumn, rightColumn);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x00111B3C File Offset: 0x0010FD3C
		private static bool CanMergeCellRange(TableRowGroup rowGroup, int topRow, int bottomRow, int leftColumn, int rightColumn)
		{
			bool result = false;
			if (topRow >= rowGroup.Rows.Count || bottomRow >= rowGroup.Rows.Count)
			{
				return result;
			}
			if (rowGroup.Rows[topRow].ColumnCount != rowGroup.Rows[bottomRow].ColumnCount)
			{
				return result;
			}
			if (leftColumn >= rowGroup.Rows[topRow].ColumnCount || rightColumn >= rowGroup.Rows[bottomRow].ColumnCount)
			{
				return result;
			}
			TableCell[] spannedCells = rowGroup.Rows[topRow].SpannedCells;
			for (int i = 0; i < spannedCells.Length; i++)
			{
				if (spannedCells[i].Row.Index < topRow)
				{
					int columnIndex = spannedCells[i].ColumnIndex;
					int num = columnIndex + spannedCells[i].ColumnSpan - 1;
					if (columnIndex <= rightColumn && num >= leftColumn)
					{
						return result;
					}
				}
			}
			for (int j = topRow; j <= bottomRow; j++)
			{
				TableCell tableCell;
				TableCell tableCell2;
				if (!TextRangeEditTables.GetBoundaryCells(rowGroup.Rows[j], bottomRow, leftColumn, rightColumn, out tableCell, out tableCell2))
				{
					return result;
				}
				if (j == topRow && (tableCell == null || tableCell.ColumnIndex != leftColumn))
				{
					return result;
				}
			}
			return true;
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00111C54 File Offset: 0x0010FE54
		private static TextRange DoMergeCellRange(TableRowGroup rowGroup, int topRow, int bottomRow, int leftColumn, int rightColumn)
		{
			TextRange result = null;
			for (int i = bottomRow; i >= topRow; i--)
			{
				TableRow tableRow = rowGroup.Rows[i];
				TableCell tableCell;
				TableCell tableCell2;
				TextRangeEditTables.GetBoundaryCells(tableRow, bottomRow, leftColumn, rightColumn, out tableCell, out tableCell2);
				if (i == topRow)
				{
					Invariant.Assert(tableCell != null, "firstCell is not expected to be null");
					Invariant.Assert(tableCell2 != null, "lastCell is not expected to be null");
					Invariant.Assert(tableCell.ColumnIndex == leftColumn, "expecting: firstCell.ColumnIndex == leftColumn");
					int num = bottomRow - topRow + 1;
					int num2 = rightColumn - leftColumn + 1;
					if (num == 1)
					{
						tableCell.ClearValue(TableCell.RowSpanProperty);
					}
					else
					{
						tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.RowSpanProperty, num);
					}
					tableCell.ContentStart.TextContainer.SetValue(tableCell.ContentStart, TableCell.ColumnSpanProperty, num2);
					result = new TextRange(tableCell.ContentStart, tableCell.ContentStart);
					if (tableCell != tableCell2)
					{
						tableRow.Cells.RemoveRange(tableCell.Index + 1, tableCell2.Index - tableCell.Index + 1 - 1);
					}
				}
				else if (tableCell != null)
				{
					Invariant.Assert(tableCell2 != null, "lastCell is not expected to be null");
					if (tableCell.Index == 0 && tableCell2.Index == tableCell2.Row.Cells.Count - 1)
					{
						foreach (TableCell tableCell3 in tableRow.SpannedCells)
						{
							if (tableCell3.ColumnIndex < tableCell.ColumnIndex || tableCell3.ColumnIndex > tableCell2.ColumnIndex)
							{
								int num3 = tableCell3.RowSpan - 1;
								if (num3 == 1)
								{
									tableCell3.ClearValue(TableCell.RowSpanProperty);
								}
								else
								{
									tableCell3.ContentStart.TextContainer.SetValue(tableCell3.ContentStart, TableCell.RowSpanProperty, num3);
								}
							}
						}
						tableRow.RowGroup.Rows.Remove(tableRow);
						bottomRow--;
					}
					else
					{
						tableRow.Cells.RemoveRange(tableCell.Index, tableCell2.Index - tableCell.Index + 1);
					}
				}
			}
			TextRangeEditTables.CorrectBorders(rowGroup.Rows);
			return result;
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00111E7C File Offset: 0x0011007C
		private static bool GetBoundaryCells(TableRow row, int bottomRow, int leftColumn, int rightColumn, out TableCell firstCell, out TableCell lastCell)
		{
			firstCell = null;
			lastCell = null;
			bool flag = false;
			for (int i = 0; i < row.Cells.Count; i++)
			{
				TableCell tableCell = row.Cells[i];
				int columnIndex = tableCell.ColumnIndex;
				int num = columnIndex + tableCell.ColumnSpan - 1;
				if (columnIndex <= rightColumn && num >= leftColumn)
				{
					if (row.Index + tableCell.RowSpan - 1 > bottomRow)
					{
						flag = true;
					}
					if (firstCell == null)
					{
						firstCell = tableCell;
					}
					lastCell = tableCell;
				}
			}
			return !flag && (firstCell == null || (firstCell.ColumnIndex >= leftColumn && firstCell.ColumnIndex + firstCell.ColumnSpan - 1 <= rightColumn)) && (lastCell == null || (lastCell.ColumnIndex >= leftColumn && lastCell.ColumnIndex + lastCell.ColumnSpan - 1 <= rightColumn));
		}

		// Token: 0x06003BBB RID: 15291 RVA: 0x00111F4B File Offset: 0x0011014B
		private static bool IsLastCellInRow(TableCell cell)
		{
			return cell.ColumnIndex + cell.ColumnSpan == cell.Table.ColumnCount;
		}

		// Token: 0x0200090B RID: 2315
		internal class TableColumnResizeInfo
		{
			// Token: 0x060085DD RID: 34269 RVA: 0x0024B374 File Offset: 0x00249574
			internal TableColumnResizeInfo(ITextView textView, Table table, int columnIndex, Rect columnRect, double tableAutofitWidth, double[] columnWidths)
			{
				Invariant.Assert(table != null, "null check: table");
				Invariant.Assert(columnIndex >= 0 && columnIndex < table.ColumnCount, "ColumnIndex validity check");
				this._table = table;
				this._columnIndex = columnIndex;
				this._columnRect = columnRect;
				this._columnWidths = columnWidths;
				this._tableAutofitWidth = tableAutofitWidth;
				this._dxl = this._columnWidths[columnIndex];
				if (columnIndex == table.ColumnCount - 1)
				{
					this._dxr = this._tableAutofitWidth;
					for (int i = 0; i < table.ColumnCount; i++)
					{
						this._dxr -= this._columnWidths[i] + table.InternalCellSpacing;
					}
					this._dxr = Math.Max(this._dxr, 0.0);
				}
				else
				{
					this._dxr = this._columnWidths[columnIndex + 1];
				}
				this._tableColResizeAdorner = new ColumnResizeAdorner(textView.RenderScope);
				this._tableColResizeAdorner.Initialize(textView.RenderScope, this._columnRect.Left + this._columnRect.Width / 2.0, this._columnRect.Top, this._columnRect.Height);
			}

			// Token: 0x060085DE RID: 34270 RVA: 0x0024B4AC File Offset: 0x002496AC
			internal void UpdateAdorner(Point mouseMovePoint)
			{
				if (this._tableColResizeAdorner != null)
				{
					double num = mouseMovePoint.X;
					num = Math.Max(num, this._columnRect.Left - this.LeftDragMax);
					num = Math.Min(num, this._columnRect.Right + this.RightDragMax);
					this._tableColResizeAdorner.Update(num);
				}
			}

			// Token: 0x060085DF RID: 34271 RVA: 0x0024B508 File Offset: 0x00249708
			internal void ResizeColumn(Point mousePoint)
			{
				double num = mousePoint.X - (this._columnRect.X + this._columnRect.Width / 2.0);
				num = Math.Max(num, -this.LeftDragMax);
				num = Math.Min(num, this.RightDragMax);
				int columnIndex = this._columnIndex;
				Table table = this.Table;
				Invariant.Assert(table != null, "Table is not expected to be null");
				Invariant.Assert(table.ColumnCount > 0, "ColumnCount is expected to be > 0");
				this._columnWidths[columnIndex] += num;
				if (columnIndex < table.ColumnCount - 1)
				{
					this._columnWidths[columnIndex + 1] -= num;
				}
				TextRangeEditTables.EnsureTableColumnsAreFixedSize(table, this._columnWidths);
				UndoManager undoManager = table.TextContainer.UndoManager;
				if (undoManager != null && undoManager.IsEnabled)
				{
					IParentUndoUnit unit = new ColumnResizeUndoUnit(table.ContentStart, columnIndex, this._columnWidths, num);
					undoManager.Open(unit);
					undoManager.Close(unit, UndoCloseAction.Commit);
				}
				this.DisposeAdorner();
			}

			// Token: 0x060085E0 RID: 34272 RVA: 0x0024B607 File Offset: 0x00249807
			internal void DisposeAdorner()
			{
				if (this._tableColResizeAdorner != null)
				{
					this._tableColResizeAdorner.Uninitialize();
					this._tableColResizeAdorner = null;
				}
			}

			// Token: 0x17001E39 RID: 7737
			// (get) Token: 0x060085E1 RID: 34273 RVA: 0x0024B623 File Offset: 0x00249823
			internal double LeftDragMax
			{
				get
				{
					return this._dxl;
				}
			}

			// Token: 0x17001E3A RID: 7738
			// (get) Token: 0x060085E2 RID: 34274 RVA: 0x0024B62B File Offset: 0x0024982B
			internal double RightDragMax
			{
				get
				{
					return this._dxr;
				}
			}

			// Token: 0x17001E3B RID: 7739
			// (get) Token: 0x060085E3 RID: 34275 RVA: 0x0024B633 File Offset: 0x00249833
			internal Table Table
			{
				get
				{
					return this._table;
				}
			}

			// Token: 0x04004316 RID: 17174
			private Rect _columnRect;

			// Token: 0x04004317 RID: 17175
			private double _tableAutofitWidth;

			// Token: 0x04004318 RID: 17176
			private double[] _columnWidths;

			// Token: 0x04004319 RID: 17177
			private Table _table;

			// Token: 0x0400431A RID: 17178
			private int _columnIndex;

			// Token: 0x0400431B RID: 17179
			private double _dxl;

			// Token: 0x0400431C RID: 17180
			private double _dxr;

			// Token: 0x0400431D RID: 17181
			private ColumnResizeAdorner _tableColResizeAdorner;
		}
	}
}
