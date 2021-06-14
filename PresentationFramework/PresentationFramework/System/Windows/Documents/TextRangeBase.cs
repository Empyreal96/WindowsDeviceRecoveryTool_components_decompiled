using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200040C RID: 1036
	internal static class TextRangeBase
	{
		// Token: 0x06003B0B RID: 15115 RVA: 0x0010ABE8 File Offset: 0x00108DE8
		internal static bool Contains(ITextRange thisRange, ITextPointer textPointer)
		{
			TextRangeBase.NormalizeRange(thisRange);
			if (textPointer == null)
			{
				throw new ArgumentNullException("textPointer");
			}
			if (textPointer.TextContainer != thisRange.Start.TextContainer)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedTree"), "textPointer");
			}
			if (textPointer.CompareTo(thisRange.Start) < 0)
			{
				textPointer = textPointer.GetFormatNormalizedPosition(LogicalDirection.Forward);
			}
			else if (textPointer.CompareTo(thisRange.End) > 0)
			{
				textPointer = textPointer.GetFormatNormalizedPosition(LogicalDirection.Backward);
			}
			for (int i = 0; i < thisRange._TextSegments.Count; i++)
			{
				if (thisRange._TextSegments[i].Contains(textPointer))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x0010AC93 File Offset: 0x00108E93
		internal static void Select(ITextRange thisRange, ITextPointer position1, ITextPointer position2)
		{
			TextRangeBase.Select(thisRange, position1, position2, false);
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0010ACA0 File Offset: 0x00108EA0
		internal static void Select(ITextRange thisRange, ITextPointer position1, ITextPointer position2, bool includeCellAtMovingPosition)
		{
			if (thisRange._TextSegments == null)
			{
				TextRangeBase.SelectPrivate(thisRange, position1, position2, includeCellAtMovingPosition, false);
				return;
			}
			ValidationHelper.VerifyPosition(thisRange.Start.TextContainer, position1, "position1");
			ValidationHelper.VerifyPosition(thisRange.Start.TextContainer, position2, "position2");
			TextRangeBase.BeginChange(thisRange);
			try
			{
				TextRangeBase.SelectPrivate(thisRange, position1, position2, includeCellAtMovingPosition, true);
			}
			finally
			{
				TextRangeBase.EndChange(thisRange);
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0010AD18 File Offset: 0x00108F18
		internal static void SelectWord(ITextRange thisRange, ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ITextPointer textPointer = position.CreatePointer();
			textPointer.MoveToInsertionPosition(LogicalDirection.Backward);
			TextSegment wordRange = TextPointerBase.GetWordRange(textPointer);
			TextRangeBase.Select(thisRange, wordRange.Start, wordRange.End);
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0010AD60 File Offset: 0x00108F60
		internal static TextSegment GetAutoWord(ITextRange thisRange)
		{
			TextSegment result = TextSegment.Null;
			if (thisRange.IsEmpty && !TextPointerBase.IsAtWordBoundary(thisRange.Start, LogicalDirection.Forward) && !TextPointerBase.IsAtWordBoundary(thisRange.Start, LogicalDirection.Backward))
			{
				result = TextPointerBase.GetWordRange(thisRange.Start);
				string text = TextRangeBase.GetTextInternal(result.Start, result.End).TrimEnd(new char[]
				{
					' '
				});
				string textInternal = TextRangeBase.GetTextInternal(result.Start, thisRange.Start);
				if (textInternal.Length >= text.Length)
				{
					result = TextSegment.Null;
				}
			}
			return result;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0010ADF0 File Offset: 0x00108FF0
		internal static void SelectParagraph(ITextRange thisRange, ITextPointer position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}
			ITextPointer position2;
			ITextPointer position3;
			TextRangeBase.FindParagraphOrListItemBoundaries(position, out position2, out position3);
			TextRangeBase.Select(thisRange, position2, position3);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0010AE20 File Offset: 0x00109020
		internal static void ApplyInitialTypingHeuristics(ITextRange thisRange)
		{
			if (thisRange.IsTableCellRange)
			{
				TableCell tableCellFromPosition;
				if (thisRange.Start is TextPointer && (tableCellFromPosition = TextRangeEditTables.GetTableCellFromPosition((TextPointer)thisRange.Start)) != null)
				{
					thisRange.Select(tableCellFromPosition.ContentStart, tableCellFromPosition.ContentEnd);
					return;
				}
				thisRange.Select(thisRange.Start, thisRange.Start);
			}
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x0010AE7C File Offset: 0x0010907C
		internal static void ApplyFinalTypingHeuristics(ITextRange thisRange, bool overType)
		{
			if (overType && thisRange.IsEmpty && !TextPointerBase.IsNextToAnyBreak(thisRange.End, LogicalDirection.Forward))
			{
				ITextPointer textPointer = thisRange.End.CreatePointer();
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				if (!TextRangeEditTables.IsTableStructureCrossed(thisRange.Start, textPointer))
				{
					TextRange textRange = new TextRange(thisRange.Start, textPointer);
					Invariant.Assert(!textRange.IsTableCellRange);
					textRange.Text = string.Empty;
				}
			}
			if (!thisRange.IsEmpty && (TextPointerBase.IsNextToAnyBreak(thisRange.End, LogicalDirection.Backward) || TextPointerBase.IsAfterLastParagraph(thisRange.End)))
			{
				ITextPointer nextInsertionPosition = thisRange.End.GetNextInsertionPosition(LogicalDirection.Backward);
				thisRange.Select(thisRange.Start, nextInsertionPosition);
			}
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0010AF28 File Offset: 0x00109128
		internal static void ApplyTypingHeuristics(ITextRange thisRange, bool overType)
		{
			TextRangeBase.BeginChange(thisRange);
			try
			{
				TextRangeBase.ApplyInitialTypingHeuristics(thisRange);
				TextRangeBase.ApplyFinalTypingHeuristics(thisRange, overType);
			}
			finally
			{
				TextRangeBase.EndChange(thisRange);
			}
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0010AF64 File Offset: 0x00109164
		internal static void FindParagraphOrListItemBoundaries(ITextPointer position, out ITextPointer start, out ITextPointer end)
		{
			start = position.CreatePointer();
			end = position.CreatePointer();
			TextRangeBase.SkipParagraphContent(start, LogicalDirection.Backward);
			TextRangeBase.SkipParagraphContent(end, LogicalDirection.Forward);
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x0010AF88 File Offset: 0x00109188
		private static void SkipParagraphContent(ITextPointer navigator, LogicalDirection direction)
		{
			TextPointerContext pointerContext = navigator.GetPointerContext(direction);
			while (pointerContext != TextPointerContext.None && (((pointerContext != TextPointerContext.ElementStart || direction != LogicalDirection.Forward) && (pointerContext != TextPointerContext.ElementEnd || direction != LogicalDirection.Backward)) || typeof(Inline).IsAssignableFrom(navigator.GetElementType(direction))) && (((pointerContext != TextPointerContext.ElementEnd || direction != LogicalDirection.Forward) && (pointerContext != TextPointerContext.ElementStart || direction != LogicalDirection.Backward)) || typeof(Inline).IsAssignableFrom(navigator.ParentType)) && navigator.MoveToNextContextPosition(direction))
			{
				pointerContext = navigator.GetPointerContext(direction);
			}
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0010B000 File Offset: 0x00109200
		internal static object GetPropertyValue(ITextRange thisRange, DependencyProperty formattingProperty)
		{
			if (TextSchema.IsCharacterProperty(formattingProperty))
			{
				return TextRangeBase.GetCharacterPropertyValue(thisRange, formattingProperty);
			}
			Invariant.Assert(TextSchema.IsParagraphProperty(formattingProperty), "The property is expected to be one of either character or paragraph formatting one");
			return TextRangeBase.GetParagraphPropertyValue(thisRange, formattingProperty);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x0010B02C File Offset: 0x0010922C
		private static object GetCharacterPropertyValue(ITextRange thisRange, DependencyProperty formattingProperty)
		{
			object characterValueFromPosition = TextRangeBase.GetCharacterValueFromPosition(thisRange.Start, formattingProperty);
			for (int i = 0; i < thisRange._TextSegments.Count; i++)
			{
				TextSegment textSegment = thisRange._TextSegments[i];
				ITextPointer textPointer = textSegment.Start.CreatePointer();
				bool flag = true;
				while (flag && textPointer.CompareTo(textSegment.End) < 0)
				{
					object characterValueFromPosition2 = TextRangeBase.GetCharacterValueFromPosition(textPointer, formattingProperty);
					if (!TextSchema.ValuesAreEqual(characterValueFromPosition2, characterValueFromPosition))
					{
						return DependencyProperty.UnsetValue;
					}
					if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
					{
						flag = textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					flag = textPointer.MoveToInsertionPosition(LogicalDirection.Forward);
					if (!flag)
					{
						flag = textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
					}
				}
			}
			return characterValueFromPosition;
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x0010B0D8 File Offset: 0x001092D8
		private static object GetCharacterValueFromPosition(ITextPointer pointer, DependencyProperty formattingProperty)
		{
			object obj = null;
			if (formattingProperty != Inline.TextDecorationsProperty)
			{
				obj = pointer.GetValue(formattingProperty);
			}
			else if (pointer is TextPointer)
			{
				DependencyObject dependencyObject = ((TextPointer)pointer).Parent as TextElement;
				while (obj == null && (dependencyObject is Inline || dependencyObject is Paragraph || dependencyObject is TextBlock))
				{
					obj = dependencyObject.GetValue(formattingProperty);
					dependencyObject = ((dependencyObject is TextElement) ? ((TextElement)dependencyObject).Parent : null);
				}
			}
			return obj;
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x0010B150 File Offset: 0x00109350
		private static object GetParagraphPropertyValue(ITextRange thisRange, DependencyProperty formattingProperty)
		{
			object obj = null;
			for (int i = 0; i < thisRange._TextSegments.Count; i++)
			{
				TextSegment textSegment = thisRange._TextSegments[i];
				ITextPointer textPointer = textSegment.Start.CreatePointer();
				while (!typeof(Paragraph).IsAssignableFrom(textPointer.ParentType) && textPointer.MoveToNextContextPosition(LogicalDirection.Backward))
				{
				}
				for (bool flag = true; flag && textPointer.CompareTo(textSegment.End) <= 0; flag = textPointer.MoveToNextContextPosition(LogicalDirection.Forward))
				{
					if (typeof(Paragraph).IsAssignableFrom(textPointer.ParentType))
					{
						object value = textPointer.GetValue(formattingProperty);
						if (obj == null)
						{
							obj = value;
						}
						if (!TextSchema.ValuesAreEqual(value, obj))
						{
							return DependencyProperty.UnsetValue;
						}
						textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
					}
				}
			}
			if (obj == null)
			{
				obj = thisRange.Start.GetValue(formattingProperty);
			}
			return obj;
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x0010B228 File Offset: 0x00109428
		internal static bool IsParagraphBoundaryCrossed(ITextRange thisRange)
		{
			ITextPointer textPointer = thisRange.Start.CreatePointer();
			ITextPointer textPointer2 = thisRange.End.CreatePointer();
			if (TextPointerBase.IsAfterLastParagraph(textPointer2))
			{
				textPointer2.MoveToInsertionPosition(LogicalDirection.Backward);
			}
			while (typeof(Inline).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
			}
			while (typeof(Inline).IsAssignableFrom(textPointer2.ParentType))
			{
				textPointer2.MoveToElementEdge(ElementEdge.AfterEnd);
			}
			return !textPointer.HasEqualScope(textPointer2);
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x0010B2A7 File Offset: 0x001094A7
		internal static void BeginChange(ITextRange thisRange)
		{
			TextRangeBase.BeginChangeWorker(thisRange, string.Empty);
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x0010B2B4 File Offset: 0x001094B4
		internal static void BeginChangeNoUndo(ITextRange thisRange)
		{
			TextRangeBase.BeginChangeWorker(thisRange, null);
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x0010A0E4 File Offset: 0x001082E4
		internal static void EndChange(ITextRange thisRange)
		{
			TextRangeBase.EndChange(thisRange, false, false);
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0010B2C0 File Offset: 0x001094C0
		internal static void EndChange(ITextRange thisRange, bool disableScroll, bool skipEvents)
		{
			Invariant.Assert(thisRange._ChangeBlockLevel > 0, "Unmatched EndChange call!");
			ITextContainer textContainer = thisRange.Start.TextContainer;
			try
			{
				bool isChanged;
				try
				{
					textContainer.EndChange(skipEvents);
				}
				finally
				{
					int changeBlockLevel = thisRange._ChangeBlockLevel;
					thisRange._ChangeBlockLevel = changeBlockLevel - 1;
					isChanged = thisRange._IsChanged;
					if (thisRange._ChangeBlockLevel == 0)
					{
						thisRange._IsChanged = false;
					}
				}
				if (thisRange._ChangeBlockLevel == 0 && isChanged)
				{
					thisRange.NotifyChanged(disableScroll, skipEvents);
				}
			}
			finally
			{
				ChangeBlockUndoRecord changeBlockUndoRecord = thisRange._ChangeBlockUndoRecord;
				if (changeBlockUndoRecord != null && thisRange._ChangeBlockLevel == 0)
				{
					try
					{
						changeBlockUndoRecord.OnEndChange();
					}
					finally
					{
						thisRange._ChangeBlockUndoRecord = null;
					}
				}
			}
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x0010B380 File Offset: 0x00109580
		internal static void NotifyChanged(ITextRange thisRange, bool disableScroll)
		{
			thisRange.FireChanged();
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x0010B388 File Offset: 0x00109588
		internal static string GetTextInternal(ITextPointer startPosition, ITextPointer endPosition)
		{
			char[] array = null;
			return TextRangeBase.GetTextInternal(startPosition, endPosition, ref array);
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0010B3A0 File Offset: 0x001095A0
		internal static string GetTextInternal(ITextPointer startPosition, ITextPointer endPosition, ref char[] charArray)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Stack<int> stack = null;
			ITextPointer textPointer = startPosition.CreatePointer();
			Invariant.Assert(startPosition.CompareTo(endPosition) <= 0, "expecting: startPosition <= endPosition");
			while (textPointer.CompareTo(endPosition) < 0)
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
					TextRangeBase.PlainConvertTextRun(stringBuilder, textPointer, endPosition, ref charArray);
					break;
				case TextPointerContext.EmbeddedElement:
					stringBuilder.Append(' ');
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				case TextPointerContext.ElementStart:
				{
					Type c = textPointer.GetElementType(LogicalDirection.Forward);
					if (typeof(AnchoredBlock).IsAssignableFrom(c))
					{
						stringBuilder.Append(Environment.NewLine);
					}
					else if (typeof(List).IsAssignableFrom(c) && textPointer is TextPointer)
					{
						TextRangeBase.PlainConvertListStart(textPointer, ref stack);
					}
					else if (typeof(ListItem).IsAssignableFrom(c))
					{
						TextRangeBase.PlainConvertListItemStart(stringBuilder, textPointer, ref stack);
					}
					else
					{
						TextRangeBase.PlainConvertAccessKey(stringBuilder, textPointer);
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				}
				case TextPointerContext.ElementEnd:
				{
					Type c = textPointer.ParentType;
					if (typeof(Paragraph).IsAssignableFrom(c) || typeof(BlockUIContainer).IsAssignableFrom(c))
					{
						TextRangeBase.PlainConvertParagraphEnd(stringBuilder, textPointer);
					}
					else if (typeof(LineBreak).IsAssignableFrom(c))
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						stringBuilder.Append(Environment.NewLine);
					}
					else if (typeof(List).IsAssignableFrom(c))
					{
						TextRangeBase.PlainConvertListEnd(textPointer, ref stack);
					}
					else
					{
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					break;
				}
				default:
					Invariant.Assert(false, "Unexpected vlue for TextPointerContext");
					break;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x0010B54C File Offset: 0x0010974C
		private static void PlainConvertTextRun(StringBuilder textBuffer, ITextPointer navigator, ITextPointer endPosition, ref char[] charArray)
		{
			int num = navigator.GetTextRunLength(LogicalDirection.Forward);
			charArray = TextRangeBase.EnsureCharArraySize(charArray, num);
			num = TextPointerBase.GetTextWithLimit(navigator, LogicalDirection.Forward, charArray, 0, num, endPosition);
			textBuffer.Append(charArray, 0, num);
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x0010B58C File Offset: 0x0010978C
		private static void PlainConvertParagraphEnd(StringBuilder textBuffer, ITextPointer navigator)
		{
			navigator.MoveToElementEdge(ElementEdge.BeforeStart);
			bool flag = navigator.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart;
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			navigator.MoveToElementEdge(ElementEdge.AfterEnd);
			TextPointerContext pointerContext = navigator.GetPointerContext(LogicalDirection.Forward);
			if (!flag || pointerContext != TextPointerContext.ElementEnd || !typeof(TableCell).IsAssignableFrom(navigator.ParentType))
			{
				textBuffer.Append(Environment.NewLine);
				return;
			}
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			pointerContext = navigator.GetPointerContext(LogicalDirection.Forward);
			if (pointerContext == TextPointerContext.ElementStart)
			{
				textBuffer.Append('\t');
				return;
			}
			textBuffer.Append(Environment.NewLine);
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x0010B618 File Offset: 0x00109818
		private static void PlainConvertListStart(ITextPointer navigator, ref Stack<int> listItemCounter)
		{
			List list = (List)navigator.GetAdjacentElement(LogicalDirection.Forward);
			if (listItemCounter == null)
			{
				listItemCounter = new Stack<int>(1);
			}
			listItemCounter.Push(0);
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x0010B646 File Offset: 0x00109846
		private static void PlainConvertListEnd(ITextPointer navigator, ref Stack<int> listItemCounter)
		{
			if (listItemCounter != null && listItemCounter.Count > 0)
			{
				listItemCounter.Pop();
			}
			navigator.MoveToNextContextPosition(LogicalDirection.Forward);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x0010B668 File Offset: 0x00109868
		private static void PlainConvertListItemStart(StringBuilder textBuffer, ITextPointer navigator, ref Stack<int> listItemCounter)
		{
			if (navigator is TextPointer)
			{
				List list = (List)((TextPointer)navigator).Parent;
				ListItem listItem = (ListItem)navigator.GetAdjacentElement(LogicalDirection.Forward);
				if (listItemCounter == null)
				{
					listItemCounter = new Stack<int>(1);
				}
				if (listItemCounter.Count == 0)
				{
					listItemCounter.Push(((IList)listItem.SiblingListItems).IndexOf(listItem));
				}
				Invariant.Assert(listItemCounter.Count > 0, "expectinng listItemCounter.Count > 0");
				int num = listItemCounter.Pop();
				int num2 = (list != null) ? list.StartIndex : 0;
				TextMarkerStyle listMarkerStyle = (list != null) ? list.MarkerStyle : TextMarkerStyle.Disc;
				TextRangeBase.WriteListMarker(textBuffer, listMarkerStyle, num + num2);
				num++;
				listItemCounter.Push(num);
			}
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x0010B714 File Offset: 0x00109914
		private static void PlainConvertAccessKey(StringBuilder textBuffer, ITextPointer navigator)
		{
			object adjacentElement = navigator.GetAdjacentElement(LogicalDirection.Forward);
			if (AccessText.HasCustomSerialization(adjacentElement))
			{
				textBuffer.Append(AccessText.AccessKeyMarker);
			}
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x0010B740 File Offset: 0x00109940
		private static char[] EnsureCharArraySize(char[] charArray, int textLength)
		{
			if (charArray == null)
			{
				charArray = new char[textLength + 10];
			}
			else if (charArray.Length < textLength)
			{
				int num = charArray.Length * 2;
				if (num < textLength)
				{
					num = textLength + 10;
				}
				charArray = new char[num];
			}
			return charArray;
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x0010B77C File Offset: 0x0010997C
		private static void WriteListMarker(StringBuilder textBuffer, TextMarkerStyle listMarkerStyle, int listItemNumber)
		{
			string text = null;
			char[] array = null;
			switch (listMarkerStyle)
			{
			case TextMarkerStyle.None:
				text = "";
				break;
			case TextMarkerStyle.Disc:
				text = "•";
				break;
			case TextMarkerStyle.Circle:
				text = "○";
				break;
			case TextMarkerStyle.Square:
				text = "□";
				break;
			case TextMarkerStyle.Box:
				text = "■";
				break;
			case TextMarkerStyle.LowerRoman:
				text = TextRangeBase.ConvertNumberToRomanString(listItemNumber, false);
				break;
			case TextMarkerStyle.UpperRoman:
				text = TextRangeBase.ConvertNumberToRomanString(listItemNumber, true);
				break;
			case TextMarkerStyle.LowerLatin:
				array = TextRangeBase.ConvertNumberToString(listItemNumber, true, "abcdefghijklmnopqrstuvwxyz");
				break;
			case TextMarkerStyle.UpperLatin:
				array = TextRangeBase.ConvertNumberToString(listItemNumber, true, "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
				break;
			case TextMarkerStyle.Decimal:
				array = TextRangeBase.ConvertNumberToString(listItemNumber, false, "0123456789");
				break;
			}
			if (text != null)
			{
				textBuffer.Append(text);
			}
			else if (array != null)
			{
				textBuffer.Append(array, 0, array.Length);
			}
			textBuffer.Append('\t');
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x0010B84C File Offset: 0x00109A4C
		private static char[] ConvertNumberToString(int number, bool oneBased, string numericSymbols)
		{
			if (oneBased)
			{
				number--;
			}
			Invariant.Assert(number >= 0, "expecting: number >= 0");
			int length = numericSymbols.Length;
			char[] array;
			if (number < length)
			{
				array = new char[]
				{
					numericSymbols[number],
					'.'
				};
			}
			else
			{
				int num = oneBased ? 1 : 0;
				int num2 = 1;
				int num3 = length;
				int num4 = length;
				while (number >= num3)
				{
					num4 *= length;
					num3 = num4 + num3 * num;
					num2++;
				}
				array = new char[num2 + 1];
				array[num2] = '.';
				for (int i = num2 - 1; i >= 0; i--)
				{
					array[i] = numericSymbols[number % length];
					number = number / length - num;
				}
			}
			return array;
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x0010B8F4 File Offset: 0x00109AF4
		private static string ConvertNumberToRomanString(int number, bool uppercase)
		{
			if (number > 3999)
			{
				return number.ToString(CultureInfo.InvariantCulture);
			}
			StringBuilder stringBuilder = new StringBuilder();
			TextRangeBase.AddRomanNumeric(stringBuilder, number / 1000, TextRangeBase.RomanNumerics[uppercase ? 1 : 0][0]);
			number %= 1000;
			TextRangeBase.AddRomanNumeric(stringBuilder, number / 100, TextRangeBase.RomanNumerics[uppercase ? 1 : 0][1]);
			number %= 100;
			TextRangeBase.AddRomanNumeric(stringBuilder, number / 10, TextRangeBase.RomanNumerics[uppercase ? 1 : 0][2]);
			number %= 10;
			TextRangeBase.AddRomanNumeric(stringBuilder, number, TextRangeBase.RomanNumerics[uppercase ? 1 : 0][3]);
			stringBuilder.Append('.');
			return stringBuilder.ToString();
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x0010B9A4 File Offset: 0x00109BA4
		private static void AddRomanNumeric(StringBuilder builder, int number, string oneFiveTen)
		{
			Invariant.Assert(number >= 0 && number <= 9, "expecting: number >= 0 && number <= 9");
			if (number >= 1 && number <= 9)
			{
				if (number == 4 || number == 9)
				{
					builder.Append(oneFiveTen[0]);
				}
				if (number == 9)
				{
					builder.Append(oneFiveTen[2]);
					return;
				}
				if (number >= 4)
				{
					builder.Append(oneFiveTen[1]);
				}
				int num = number % 5;
				while (num > 0 && num < 4)
				{
					builder.Append(oneFiveTen[0]);
					num--;
				}
			}
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x0010BA30 File Offset: 0x00109C30
		internal static ITextPointer GetStart(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			Invariant.Assert(thisRange._TextSegments != null && thisRange._TextSegments.Count > 0, "expecting nonempty _TextSegments array for Start position");
			return thisRange._TextSegments[0].Start;
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0010BA7C File Offset: 0x00109C7C
		internal static ITextPointer GetEnd(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			Invariant.Assert(thisRange._TextSegments != null && thisRange._TextSegments.Count > 0, "expecting nonempty _TextSegments array for End position");
			return thisRange._TextSegments[thisRange._TextSegments.Count - 1].End;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x0010BAD4 File Offset: 0x00109CD4
		internal static bool GetIsEmpty(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			Invariant.Assert((thisRange._TextSegments.Count == 1 && thisRange._TextSegments[0].Start == thisRange._TextSegments[0].End) == (thisRange.Start.CompareTo(thisRange.End) == 0), "Range emptiness assumes using one instance of TextPointer for both start and end");
			return thisRange._TextSegments.Count == 1 && thisRange._TextSegments[0].Start == thisRange._TextSegments[0].End;
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0010BB7C File Offset: 0x00109D7C
		internal static List<TextSegment> GetTextSegments(ITextRange thisRange)
		{
			return thisRange._TextSegments;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x0010BB84 File Offset: 0x00109D84
		internal static string GetText(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			if (!thisRange.IsTableCellRange)
			{
				ITextPointer textPointer = thisRange.Start;
				while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && !typeof(AnchoredBlock).IsAssignableFrom(textPointer.ParentType))
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Backward);
				}
				return TextRangeBase.GetTextInternal(textPointer, thisRange.End);
			}
			string text = string.Empty;
			for (int i = 0; i < thisRange._TextSegments.Count; i++)
			{
				TextSegment textSegment = thisRange._TextSegments[i];
				text += TextRangeBase.GetTextInternal(textSegment.Start, textSegment.End);
			}
			return text;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x0010BC24 File Offset: 0x00109E24
		internal static void SetText(ITextRange thisRange, string textData)
		{
			TextRangeBase.NormalizeRange(thisRange);
			if (textData == null)
			{
				throw new ArgumentNullException("textData");
			}
			ITextPointer textPointer = null;
			TextRangeBase.BeginChange(thisRange);
			try
			{
				if (!thisRange.IsEmpty)
				{
					if (thisRange.Start is TextPointer && ((TextPointer)thisRange.Start).Parent == ((TextPointer)thisRange.End).Parent && ((TextPointer)thisRange.Start).Parent is Run && textData.Length > 0)
					{
						if (thisRange.Start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text && thisRange.End.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
						{
							textPointer = thisRange.Start;
						}
						TextContainer textContainer = ((TextPointer)thisRange.Start).TextContainer;
						textContainer.DeleteContentInternal((TextPointer)thisRange.Start, (TextPointer)thisRange.End);
					}
					else
					{
						thisRange.Start.DeleteContentToPosition(thisRange.End);
					}
					if (thisRange.Start is TextPointer)
					{
						TextRangeEdit.MergeFlowDirection((TextPointer)thisRange.Start);
					}
					thisRange.Select(thisRange.Start, thisRange.Start);
				}
				if (textData.Length > 0)
				{
					ITextPointer textPointer2 = (textPointer == null) ? thisRange.Start : textPointer;
					bool flag = textData.EndsWith("\n", StringComparison.Ordinal);
					bool flag2 = textPointer2 is TextPointer && TextSchema.IsValidChild(textPointer2, typeof(Block)) && (textPointer2.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.None || textPointer2.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart) && (textPointer2.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.None || textPointer2.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd);
					if (textPointer2 is TextPointer && textPointer == null)
					{
						TextPointer textPointer3 = TextRangeEditTables.EnsureInsertionPosition((TextPointer)textPointer2);
						thisRange.Select(textPointer3, textPointer3);
						textPointer2 = thisRange.Start;
					}
					Invariant.Assert(TextSchema.IsInTextContent(textPointer2), "range.Start is expected to be in text content");
					ITextPointer frozenPointer = textPointer2.GetFrozenPointer(LogicalDirection.Backward);
					ITextPointer textPointer4 = textPointer2.CreatePointer(LogicalDirection.Forward);
					if (frozenPointer is TextPointer && ((TextPointer)frozenPointer).Paragraph != null)
					{
						TextPointer textPointer5 = (TextPointer)frozenPointer.CreatePointer(LogicalDirection.Forward);
						string[] array = textData.Split(new string[]
						{
							"\r\n",
							"\n"
						}, StringSplitOptions.None);
						int num = array.Length;
						if (flag2 && flag)
						{
							num--;
						}
						for (int i = 0; i < num; i++)
						{
							textPointer5.InsertTextInRun(array[i]);
							if (i < num - 1)
							{
								if (textPointer5.HasNonMergeableInlineAncestor)
								{
									textPointer5.InsertTextInRun(" ");
								}
								else
								{
									textPointer5 = textPointer5.InsertParagraphBreak();
								}
								textPointer4 = textPointer5;
							}
						}
						if (flag2 && flag)
						{
							textPointer4 = textPointer4.GetNextInsertionPosition(LogicalDirection.Forward);
							if (textPointer4 == null)
							{
								textPointer4 = frozenPointer.TextContainer.End;
							}
						}
					}
					else
					{
						frozenPointer.InsertTextInRun(textData);
					}
					TextRangeBase.SelectPrivate(thisRange, frozenPointer, textPointer4, false, true);
				}
			}
			finally
			{
				TextRangeBase.EndChange(thisRange);
			}
		}

		// Token: 0x06003B33 RID: 15155 RVA: 0x0010BEFC File Offset: 0x0010A0FC
		internal static string GetXml(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
			TextRangeSerialization.WriteXaml(xmlWriter, thisRange, false, null);
			return stringWriter.ToString();
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x0010BF30 File Offset: 0x0010A130
		internal static bool CanSave(ITextRange thisRange, string dataFormat)
		{
			TextRangeBase.NormalizeRange(thisRange);
			return dataFormat == DataFormats.Text || dataFormat == DataFormats.Xaml || (SecurityHelper.CheckUnmanagedCodePermission() && (dataFormat == DataFormats.XamlPackage || dataFormat == DataFormats.Rtf));
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x0010BF88 File Offset: 0x0010A188
		internal static bool CanLoad(ITextRange thisRange, string dataFormat)
		{
			TextRangeBase.NormalizeRange(thisRange);
			return dataFormat == DataFormats.Text || dataFormat == DataFormats.Xaml || (SecurityHelper.CheckUnmanagedCodePermission() && (dataFormat == DataFormats.XamlPackage || dataFormat == DataFormats.Rtf));
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x0010BFE0 File Offset: 0x0010A1E0
		internal static void Save(ITextRange thisRange, Stream stream, string dataFormat, bool preserveTextElements)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (dataFormat == null)
			{
				throw new ArgumentNullException("dataFormat");
			}
			TextRangeBase.NormalizeRange(thisRange);
			if (dataFormat == DataFormats.Text)
			{
				string text = thisRange.Text;
				StreamWriter streamWriter = new StreamWriter(stream);
				streamWriter.Write(text);
				streamWriter.Flush();
				return;
			}
			if (dataFormat == DataFormats.Xaml)
			{
				StreamWriter w = new StreamWriter(stream);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
				TextRangeSerialization.WriteXaml(xmlTextWriter, thisRange, false, null, preserveTextElements);
				xmlTextWriter.Flush();
				return;
			}
			if (dataFormat == DataFormats.XamlPackage && SecurityHelper.CheckUnmanagedCodePermission())
			{
				WpfPayload.SaveRange(thisRange, ref stream, false, preserveTextElements);
				return;
			}
			if (dataFormat == DataFormats.Rtf && SecurityHelper.CheckUnmanagedCodePermission())
			{
				Stream wpfContainerMemory = null;
				string xamlContent = WpfPayload.SaveRange(thisRange, ref wpfContainerMemory, false);
				string value = TextEditorCopyPaste.ConvertXamlToRtf(xamlContent, wpfContainerMemory);
				StreamWriter streamWriter2 = new StreamWriter(stream);
				streamWriter2.Write(value);
				streamWriter2.Flush();
				return;
			}
			throw new ArgumentException(SR.Get("TextRange_UnsupportedDataFormat", new object[]
			{
				dataFormat
			}), "dataFormat");
		}

		// Token: 0x06003B37 RID: 15159 RVA: 0x0010C0E8 File Offset: 0x0010A2E8
		internal static void Load(TextRange thisRange, Stream stream, string dataFormat)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (dataFormat == null)
			{
				throw new ArgumentNullException("dataFormat");
			}
			TextRangeBase.NormalizeRange(thisRange);
			if (stream.CanSeek)
			{
				stream.Seek(0L, SeekOrigin.Begin);
			}
			if (dataFormat == DataFormats.Text)
			{
				StreamReader streamReader = new StreamReader(stream);
				string text = streamReader.ReadToEnd();
				thisRange.Text = text;
				return;
			}
			if (dataFormat == DataFormats.Xaml)
			{
				StreamReader streamReader2 = new StreamReader(stream);
				string xml = streamReader2.ReadToEnd();
				thisRange.Xml = xml;
				return;
			}
			if (dataFormat == DataFormats.XamlPackage && SecurityHelper.CheckUnmanagedCodePermission())
			{
				object obj = WpfPayload.LoadElement(stream);
				if (!(obj is Section) && !(obj is Span))
				{
					throw new ArgumentException(SR.Get("TextRange_UnrecognizedStructureInDataFormat", new object[]
					{
						dataFormat
					}), "stream");
				}
				thisRange.SetXmlVirtual((TextElement)obj);
				return;
			}
			else
			{
				if (!(dataFormat == DataFormats.Rtf) || !SecurityHelper.CheckUnmanagedCodePermission())
				{
					throw new ArgumentException(SR.Get("TextRange_UnsupportedDataFormat", new object[]
					{
						dataFormat
					}), "dataFormat");
				}
				StreamReader streamReader3 = new StreamReader(stream);
				string rtfContent = streamReader3.ReadToEnd();
				MemoryStream memoryStream = TextEditorCopyPaste.ConvertRtfToXaml(rtfContent);
				if (memoryStream == null)
				{
					throw new ArgumentException(SR.Get("TextRange_UnrecognizedStructureInDataFormat", new object[]
					{
						dataFormat
					}), "stream");
				}
				TextElement textElement = WpfPayload.LoadElement(memoryStream) as TextElement;
				if (!(textElement is Section) && !(textElement is Span))
				{
					throw new ArgumentException(SR.Get("TextRange_UnrecognizedStructureInDataFormat", new object[]
					{
						dataFormat
					}), "stream");
				}
				thisRange.SetXmlVirtual(textElement);
				return;
			}
		}

		// Token: 0x06003B38 RID: 15160 RVA: 0x0010C287 File Offset: 0x0010A487
		internal static int GetChangeBlockLevel(ITextRange thisRange)
		{
			return thisRange._ChangeBlockLevel;
		}

		// Token: 0x06003B39 RID: 15161 RVA: 0x0010C290 File Offset: 0x0010A490
		internal static UIElement GetUIElementSelected(ITextRange range)
		{
			ITextPointer textPointer = range.Start.CreatePointer();
			TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
			while (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd)
			{
				textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
			}
			if (pointerContext == TextPointerContext.EmbeddedElement)
			{
				ITextPointer textPointer2 = range.End.CreatePointer();
				pointerContext = textPointer2.GetPointerContext(LogicalDirection.Backward);
				while (pointerContext == TextPointerContext.ElementStart || pointerContext == TextPointerContext.ElementEnd)
				{
					textPointer2.MoveToNextContextPosition(LogicalDirection.Backward);
					pointerContext = textPointer2.GetPointerContext(LogicalDirection.Backward);
				}
				if (pointerContext == TextPointerContext.EmbeddedElement && textPointer.GetOffsetToPosition(textPointer2) == 1)
				{
					return textPointer.GetAdjacentElement(LogicalDirection.Forward) as UIElement;
				}
			}
			return null;
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x0010C319 File Offset: 0x0010A519
		internal static bool GetIsTableCellRange(ITextRange thisRange)
		{
			TextRangeBase.NormalizeRange(thisRange);
			return thisRange._IsTableCellRange;
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x0010C328 File Offset: 0x0010A528
		private static void BeginChangeWorker(ITextRange thisRange, string description)
		{
			ITextContainer textContainer = thisRange.Start.TextContainer;
			if (description != null && thisRange._ChangeBlockUndoRecord == null && thisRange._ChangeBlockLevel == 0)
			{
				thisRange._ChangeBlockUndoRecord = new ChangeBlockUndoRecord(textContainer, description);
			}
			Invariant.Assert(thisRange._ChangeBlockLevel > 0 || !thisRange._IsChanged, "_changed must be false on new move sequence");
			int changeBlockLevel = thisRange._ChangeBlockLevel;
			thisRange._ChangeBlockLevel = changeBlockLevel + 1;
			if (description != null)
			{
				textContainer.BeginChange();
				return;
			}
			textContainer.BeginChangeNoUndo();
		}

		// Token: 0x06003B3C RID: 15164 RVA: 0x0010C3A0 File Offset: 0x0010A5A0
		private static void CreateNormalizedTextSegment(ITextRange thisRange, ITextPointer start, ITextPointer end)
		{
			ValidationHelper.VerifyPositionPair(start, end);
			if (start.CompareTo(end) == 0)
			{
				if (!TextRangeBase.IsAtNormalizedPosition(thisRange, start, start.LogicalDirection))
				{
					start = TextRangeBase.GetNormalizedPosition(thisRange, start, start.LogicalDirection);
					end = start;
				}
			}
			else
			{
				start = TextRangeBase.GetNormalizedPosition(thisRange, start, LogicalDirection.Forward);
				if (!TextPointerBase.IsAfterLastParagraph(end))
				{
					end = TextRangeBase.GetNormalizedPosition(thisRange, end, LogicalDirection.Backward);
				}
				if (start.CompareTo(end) >= 0)
				{
					if (start.LogicalDirection == LogicalDirection.Backward)
					{
						start = end.GetFrozenPointer(LogicalDirection.Backward);
					}
					end = start;
				}
				else
				{
					if (start is TextPointer)
					{
						TextPointer textPointer = (TextPointer)start;
						TextPointer textPointer2 = (TextPointer)end;
						TextRangeBase.NormalizeAnchoredBlockBoundaries(ref textPointer, ref textPointer2);
						start = textPointer;
						end = textPointer2;
					}
					Invariant.Assert(start.CompareTo(end) <= 0, "expecting start <= end");
					if (start.CompareTo(end) == 0 && !TextRangeBase.IsAtNormalizedPosition(thisRange, start, start.LogicalDirection))
					{
						start = TextRangeBase.GetNormalizedPosition(thisRange, start, start.LogicalDirection);
						end = start;
					}
				}
			}
			thisRange._TextSegments = new List<TextSegment>(1);
			thisRange._TextSegments.Add(new TextSegment(start, end));
			thisRange._IsTableCellRange = false;
		}

		// Token: 0x06003B3D RID: 15165 RVA: 0x0010C4B0 File Offset: 0x0010A6B0
		private static bool IsAtNormalizedPosition(ITextRange thisRange, ITextPointer position, LogicalDirection direction)
		{
			bool result;
			if (thisRange.IgnoreTextUnitBoundaries)
			{
				result = TextPointerBase.IsAtFormatNormalizedPosition(position, direction);
			}
			else
			{
				result = TextPointerBase.IsAtInsertionPosition(position, direction);
			}
			return result;
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x0010C4D8 File Offset: 0x0010A6D8
		private static ITextPointer GetNormalizedPosition(ITextRange thisRange, ITextPointer position, LogicalDirection direction)
		{
			ITextPointer result;
			if (thisRange.IgnoreTextUnitBoundaries)
			{
				result = position.GetFormatNormalizedPosition(direction);
			}
			else
			{
				result = position.GetInsertionPosition(direction);
			}
			return result;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0010C500 File Offset: 0x0010A700
		internal static void NormalizeAnchoredBlockBoundaries(ref TextPointer start, ref TextPointer end)
		{
			TextElement textElement = start.Parent as TextElement;
			while (textElement != null)
			{
				while (textElement != null && !typeof(AnchoredBlock).IsAssignableFrom(textElement.GetType()))
				{
					textElement = (textElement.Parent as TextElement);
				}
				if (textElement != null)
				{
					AnchoredBlock anchoredBlock = null;
					TextElement textElement2 = end.Parent as TextElement;
					while (textElement2 != null && textElement2 != textElement)
					{
						if (textElement2 is AnchoredBlock)
						{
							anchoredBlock = (AnchoredBlock)textElement2;
						}
						textElement2 = (textElement2.Parent as TextElement);
					}
					if (textElement2 == textElement)
					{
						if (anchoredBlock != null)
						{
							end = anchoredBlock.ElementEnd;
						}
						return;
					}
					start = textElement.ElementStart;
					textElement = (textElement.Parent as TextElement);
				}
			}
			textElement = (end.Parent as TextElement);
			while (textElement != null)
			{
				while (textElement != null && !typeof(AnchoredBlock).IsAssignableFrom(textElement.GetType()))
				{
					textElement = (textElement.Parent as TextElement);
				}
				if (textElement != null)
				{
					AnchoredBlock anchoredBlock2 = null;
					TextElement textElement3 = start.Parent as TextElement;
					while (textElement3 != null && textElement3 != textElement)
					{
						if (textElement3 is AnchoredBlock)
						{
							anchoredBlock2 = (AnchoredBlock)textElement3;
						}
						textElement3 = (textElement3.Parent as TextElement);
					}
					if (textElement3 == textElement)
					{
						if (anchoredBlock2 != null)
						{
							start = anchoredBlock2.ElementStart;
						}
						return;
					}
					end = textElement.ElementEnd;
					textElement = (textElement.Parent as TextElement);
				}
			}
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x0010C640 File Offset: 0x0010A840
		private static void NormalizeRange(ITextRange thisRange)
		{
			if (thisRange._ContentGeneration == thisRange._TextSegments[0].Start.TextContainer.Generation)
			{
				return;
			}
			ITextPointer start = thisRange._TextSegments[0].Start;
			ITextPointer end = thisRange._TextSegments[thisRange._TextSegments.Count - 1].End;
			if (thisRange._IsTableCellRange)
			{
				Invariant.Assert(thisRange._TextSegments[0].Start is TextPointer);
				TextRangeEditTables.IdentifyValidBoundaries(thisRange, out start, out end);
				TextRangeBase.SelectPrivate(thisRange, start, end, false, false);
			}
			else
			{
				bool flag = false;
				if (start == end)
				{
					if (!TextPointerBase.IsAtInsertionPosition(start, start.LogicalDirection))
					{
						flag = true;
					}
				}
				else if (start.CompareTo(end) == 0)
				{
					flag = true;
				}
				else if (!TextPointerBase.IsAtInsertionPosition(start, LogicalDirection.Forward) || !TextPointerBase.IsAtInsertionPosition(end, LogicalDirection.Backward))
				{
					flag = true;
				}
				if (flag)
				{
					TextRangeBase.CreateNormalizedTextSegment(thisRange, start, end);
				}
			}
			thisRange._ContentGeneration = thisRange._TextSegments[0].Start.TextContainer.Generation;
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x0010C754 File Offset: 0x0010A954
		private static void SelectPrivate(ITextRange thisRange, ITextPointer position1, ITextPointer position2, bool includeCellAtMovingPosition, bool markRangeChanged)
		{
			Invariant.Assert(position1 != null, "null check: position1");
			Invariant.Assert(position2 != null, "null check: position2");
			bool isTableCellRange;
			List<TextSegment> list;
			if (position1 is TextPointer)
			{
				list = TextRangeEditTables.BuildTableRange((TextPointer)position1, (TextPointer)position2, includeCellAtMovingPosition, out isTableCellRange);
			}
			else
			{
				Invariant.Assert(!thisRange._IsTableCellRange, "range is not expected to be in IsTableCellRange state - 1");
				list = null;
				isTableCellRange = false;
			}
			if (list != null)
			{
				thisRange._TextSegments = list;
				thisRange._IsTableCellRange = isTableCellRange;
			}
			else
			{
				ITextPointer textPointer = position1;
				ITextPointer textPointer2 = position2;
				if (position1.CompareTo(position2) > 0)
				{
					textPointer = position2;
					textPointer2 = position1;
				}
				TextRangeBase.CreateNormalizedTextSegment(thisRange, textPointer, textPointer2);
				Invariant.Assert(!thisRange._IsTableCellRange, "Expecting that the range is in text segment state now - must be set by CreateNOrmalizedTextSegment");
				if (position1 is TextPointer)
				{
					ITextPointer start = thisRange._TextSegments[0].Start;
					ITextPointer end = thisRange._TextSegments[thisRange._TextSegments.Count - 1].End;
					if (start.CompareTo(textPointer) != 0 || end.CompareTo(textPointer2) != 0)
					{
						list = TextRangeEditTables.BuildTableRange((TextPointer)start, (TextPointer)end, false, out isTableCellRange);
						if (list != null)
						{
							thisRange._TextSegments = list;
							thisRange._IsTableCellRange = isTableCellRange;
						}
					}
				}
			}
			thisRange._ContentGeneration = thisRange._TextSegments[0].Start.TextContainer.Generation;
			if (markRangeChanged)
			{
				TextRangeBase.MarkRangeChanged(thisRange);
			}
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x0010C8A3 File Offset: 0x0010AAA3
		private static void MarkRangeChanged(ITextRange thisRange)
		{
			Invariant.Assert(thisRange._ChangeBlockLevel > 0, "changeBlockLevel > 0 is expected");
			thisRange._IsChanged = true;
		}

		// Token: 0x040025E6 RID: 9702
		private const char NumberSuffix = '.';

		// Token: 0x040025E7 RID: 9703
		private const string DecimalNumerics = "0123456789";

		// Token: 0x040025E8 RID: 9704
		private const string LowerLatinNumerics = "abcdefghijklmnopqrstuvwxyz";

		// Token: 0x040025E9 RID: 9705
		private const string UpperLatinNumerics = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x040025EA RID: 9706
		private static string[][] RomanNumerics = new string[][]
		{
			new string[]
			{
				"m??",
				"cdm",
				"xlc",
				"ivx"
			},
			new string[]
			{
				"M??",
				"CDM",
				"XLC",
				"IVX"
			}
		};
	}
}
