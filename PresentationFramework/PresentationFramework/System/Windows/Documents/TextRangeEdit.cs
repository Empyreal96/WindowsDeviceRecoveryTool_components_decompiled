using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200040D RID: 1037
	internal static class TextRangeEdit
	{
		// Token: 0x06003B44 RID: 15172 RVA: 0x0010C92C File Offset: 0x0010AB2C
		internal static TextElement InsertElementClone(TextPointer start, TextPointer end, TextElement element)
		{
			TextElement textElement = (TextElement)Activator.CreateInstance(element.GetType());
			textElement.TextContainer.SetValues(textElement.ContentStart, element.GetLocalValueEnumerator());
			textElement.Reposition(start, end);
			return textElement;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x0010C96A File Offset: 0x0010AB6A
		internal static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting)
		{
			return TextRangeEdit.SplitFormattingElements(splitPosition, keepEmptyFormatting, null);
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x0010C974 File Offset: 0x0010AB74
		internal static TextPointer SplitFormattingElement(TextPointer splitPosition, bool keepEmptyFormatting)
		{
			Invariant.Assert(splitPosition.Parent != null && TextSchema.IsMergeableInline(splitPosition.Parent.GetType()));
			Inline inline = (Inline)splitPosition.Parent;
			if (splitPosition.IsFrozen)
			{
				splitPosition = new TextPointer(splitPosition);
			}
			if (!keepEmptyFormatting && splitPosition.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				splitPosition.MoveToPosition(inline.ElementStart);
			}
			else if (!keepEmptyFormatting && splitPosition.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				splitPosition.MoveToPosition(inline.ElementEnd);
			}
			else
			{
				splitPosition = TextRangeEdit.SplitElement(splitPosition);
			}
			return splitPosition;
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x0010C9FC File Offset: 0x0010ABFC
		private static bool InheritablePropertiesAreEqual(Inline firstInline, Inline secondInline)
		{
			Invariant.Assert(firstInline != null, "null check: firstInline");
			Invariant.Assert(secondInline != null, "null check: secondInline");
			foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(typeof(Inline)))
			{
				if (TextSchema.IsStructuralCharacterProperty(dependencyProperty))
				{
					if (firstInline.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue || secondInline.ReadLocalValue(dependencyProperty) != DependencyProperty.UnsetValue)
					{
						return false;
					}
				}
				else if (!TextSchema.ValuesAreEqual(firstInline.GetValue(dependencyProperty), secondInline.GetValue(dependencyProperty)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x0010CA88 File Offset: 0x0010AC88
		private static bool CharacterPropertiesAreEqual(Inline firstElement, Inline secondElement)
		{
			Invariant.Assert(firstElement != null, "null check: firstElement");
			if (secondElement == null)
			{
				return false;
			}
			foreach (DependencyProperty dp in TextSchema.GetNoninheritableProperties(typeof(Span)))
			{
				if (!TextSchema.ValuesAreEqual(firstElement.GetValue(dp), secondElement.GetValue(dp)))
				{
					return false;
				}
			}
			return TextRangeEdit.InheritablePropertiesAreEqual(firstElement, secondElement);
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x0010CAF0 File Offset: 0x0010ACF0
		private static bool ExtractEmptyFormattingElements(TextPointer position)
		{
			bool result = false;
			Inline inline = position.Parent as Inline;
			if (inline != null && inline.IsEmpty)
			{
				while (inline != null && inline.IsEmpty)
				{
					if (TextSchema.IsFormattingType(inline.GetType()))
					{
						break;
					}
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
				while (inline != null && inline.IsEmpty && (inline.GetType() == typeof(Run) || inline.GetType() == typeof(Span)))
				{
					if (TextRangeEdit.HasWriteableLocalPropertyValues(inline))
					{
						break;
					}
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
				while (inline != null && inline.IsEmpty && ((inline.NextInline != null && TextSchema.IsFormattingType(inline.NextInline.GetType())) || (inline.PreviousInline != null && TextSchema.IsFormattingType(inline.PreviousInline.GetType()))))
				{
					inline.Reposition(null, null);
					result = true;
					inline = (position.Parent as Inline);
				}
			}
			return result;
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x0010CC00 File Offset: 0x0010AE00
		internal static void SetInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value, PropertyValueAction propertyValueAction)
		{
			if (start.CompareTo(end) >= 0 || (propertyValueAction == PropertyValueAction.SetValue && start.Parent is Run && start.Parent == end.Parent && TextSchema.ValuesAreEqual(start.Parent.GetValue(formattingProperty), value)))
			{
				return;
			}
			TextRangeEdit.RemoveUnnecessarySpans(start);
			TextRangeEdit.RemoveUnnecessarySpans(end);
			if (TextSchema.IsStructuralCharacterProperty(formattingProperty))
			{
				TextRangeEdit.SetStructuralInlineProperty(start, end, formattingProperty, value);
				return;
			}
			TextRangeEdit.SetNonStructuralInlineProperty(start, end, formattingProperty, value, propertyValueAction);
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x0010CC74 File Offset: 0x0010AE74
		internal static bool MergeFormattingInlines(TextPointer position)
		{
			TextRangeEdit.RemoveUnnecessarySpans(position);
			TextRangeEdit.ExtractEmptyFormattingElements(position);
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (!TextSchema.IsMergeableInline(position.Parent.GetType()))
				{
					break;
				}
				position = ((Inline)position.Parent).ElementStart;
			}
			while (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(position.Parent.GetType()))
			{
				position = ((Inline)position.Parent).ElementEnd;
			}
			bool flag = false;
			Inline inline;
			Inline inline2;
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart && (inline = (position.GetAdjacentElement(LogicalDirection.Backward) as Inline)) != null && (inline2 = (position.GetAdjacentElement(LogicalDirection.Forward) as Inline)) != null)
			{
				if (TextSchema.IsFormattingType(inline.GetType()) && inline.TextRange.IsEmpty)
				{
					inline.RepositionWithContent(null);
					flag = true;
				}
				else if (TextSchema.IsFormattingType(inline2.GetType()) && inline2.TextRange.IsEmpty)
				{
					inline2.RepositionWithContent(null);
					flag = true;
				}
				else
				{
					if (!TextSchema.IsKnownType(inline.GetType()) || !TextSchema.IsKnownType(inline2.GetType()) || ((!(inline is Run) || !(inline2 is Run)) && (!(inline is Span) || !(inline2 is Span))) || !TextSchema.IsMergeableInline(inline.GetType()) || !TextSchema.IsMergeableInline(inline2.GetType()) || !TextRangeEdit.CharacterPropertiesAreEqual(inline, inline2))
					{
						break;
					}
					inline.Reposition(inline.ElementStart, inline2.ElementEnd);
					inline2.Reposition(null, null);
					flag = true;
				}
			}
			if (flag)
			{
				TextRangeEdit.RemoveUnnecessarySpans(position);
			}
			return flag;
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x0010CE04 File Offset: 0x0010B004
		private static void RemoveUnnecessarySpans(TextPointer position)
		{
			Inline inline = position.Parent as Inline;
			while (inline != null)
			{
				if (inline.Parent != null && TextSchema.IsMergeableInline(inline.Parent.GetType()) && TextSchema.IsKnownType(inline.Parent.GetType()) && inline.ElementStart.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && inline.ElementEnd.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
				{
					Span span = (Span)inline.Parent;
					if (span.Parent == null)
					{
						break;
					}
					foreach (DependencyProperty dp in TextSchema.GetInheritableProperties(typeof(Span)))
					{
						object value = inline.GetValue(dp);
						object value2 = span.GetValue(dp);
						if (TextSchema.ValuesAreEqual(value, value2))
						{
							object value3 = span.Parent.GetValue(dp);
							if (!TextSchema.ValuesAreEqual(value, value3))
							{
								inline.SetValue(dp, value2);
							}
						}
					}
					foreach (DependencyProperty dp2 in TextSchema.GetNoninheritableProperties(typeof(Span)))
					{
						bool flag2;
						bool flag = span.GetValueSource(dp2, null, out flag2) == BaseValueSourceInternal.Default && !flag2;
						bool flag3 = inline.GetValueSource(dp2, null, out flag2) == BaseValueSourceInternal.Default && !flag2;
						if (flag3 && !flag)
						{
							inline.SetValue(dp2, span.GetValue(dp2));
						}
					}
					span.Reposition(null, null);
				}
				else
				{
					inline = (inline.Parent as Inline);
				}
			}
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x0010CF8C File Offset: 0x0010B18C
		internal static void CharacterResetFormatting(TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) < 0)
			{
				start = TextRangeEdit.SplitFormattingElements(start, false, true, null);
				end = TextRangeEdit.SplitFormattingElements(end, false, true, null);
				while (start.CompareTo(end) < 0)
				{
					if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
					{
						TextElement textElement = (TextElement)start.Parent;
						if (!(textElement is Span) || textElement.ContentEnd.CompareTo(end) <= 0)
						{
							if (textElement is Span && TextSchema.IsKnownType(textElement.GetType()))
							{
								TextPointer elementStart = textElement.ElementStart;
								Span span = TextRangeEdit.TransferNonFormattingInlineProperties((Span)textElement);
								if (span != null)
								{
									span.Reposition(textElement.ElementStart, textElement.ElementEnd);
									elementStart = span.ElementStart;
								}
								textElement.Reposition(null, null);
								TextRangeEdit.MergeFormattingInlines(elementStart);
							}
							else if (textElement is Inline)
							{
								TextRangeEdit.ClearFormattingInlineProperties((Inline)textElement);
								TextRangeEdit.MergeFormattingInlines(textElement.ElementStart);
							}
						}
					}
					start = start.GetNextContextPosition(LogicalDirection.Forward);
				}
				TextRangeEdit.MergeFormattingInlines(end);
			}
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x0010D084 File Offset: 0x0010B284
		private static void ClearFormattingInlineProperties(Inline inline)
		{
			LocalValueEnumerator localValueEnumerator = inline.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (!property.ReadOnly && !TextSchema.IsNonFormattingCharacterProperty(property))
				{
					localValueEntry = localValueEnumerator.Current;
					inline.ClearValue(localValueEntry.Property);
				}
			}
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x0010D0D8 File Offset: 0x0010B2D8
		private static Span TransferNonFormattingInlineProperties(Span source)
		{
			Span span = null;
			DependencyProperty[] nonFormattingCharacterProperties = TextSchema.GetNonFormattingCharacterProperties();
			for (int i = 0; i < nonFormattingCharacterProperties.Length; i++)
			{
				object value = source.GetValue(nonFormattingCharacterProperties[i]);
				object value2 = ((ITextPointer)source.ElementStart).GetValue(nonFormattingCharacterProperties[i]);
				if (!TextSchema.ValuesAreEqual(value, value2))
				{
					if (span == null)
					{
						span = new Span();
					}
					span.SetValue(nonFormattingCharacterProperties[i], value);
				}
			}
			return span;
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0010D134 File Offset: 0x0010B334
		internal static TextPointer SplitElement(TextPointer position)
		{
			TextElement textElement = (TextElement)position.Parent;
			if (position.IsFrozen)
			{
				position = new TextPointer(position);
			}
			if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(textElement.ElementEnd, textElement.ElementEnd, textElement);
				position.MoveToPosition(textElement.ElementEnd);
			}
			else if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(textElement.ElementStart, textElement.ElementStart, textElement);
				position.MoveToPosition(textElement.ElementStart);
			}
			else
			{
				TextElement textElement2 = TextRangeEdit.InsertElementClone(position, textElement.ContentEnd, textElement);
				textElement.Reposition(textElement.ContentStart, textElement2.ElementStart);
				position.MoveToPosition(textElement.ElementEnd);
			}
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd, "position must be after ElementEnd");
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "position must be before ElementStart");
			return position;
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x0010D208 File Offset: 0x0010B408
		internal static TextPointer InsertParagraphBreak(TextPointer position, bool moveIntoSecondParagraph)
		{
			Invariant.Assert(position.TextContainer.Parent == null || TextSchema.IsValidChildOfContainer(position.TextContainer.Parent.GetType(), typeof(Paragraph)));
			bool flag = TextPointerBase.IsAtRowEnd(position) || TextPointerBase.IsBeforeFirstTable(position) || TextPointerBase.IsInBlockUIContainer(position);
			if (position.Paragraph == null)
			{
				position = TextRangeEditTables.EnsureInsertionPosition(position);
			}
			Inline nonMergeableInlineAncestor = position.GetNonMergeableInlineAncestor();
			if (nonMergeableInlineAncestor != null)
			{
				Invariant.Assert(TextPointerBase.IsPositionAtNonMergeableInlineBoundary(position), "Position must be at hyperlink boundary!");
				position = (position.IsAtNonMergeableInlineStart ? nonMergeableInlineAncestor.ElementStart : nonMergeableInlineAncestor.ElementEnd);
			}
			Paragraph paragraph = position.Paragraph;
			if (paragraph == null)
			{
				Invariant.Assert(position.TextContainer.Parent == null);
				paragraph = new Paragraph();
				paragraph.Reposition(position.DocumentStart, position.DocumentEnd);
			}
			if (flag)
			{
				return position;
			}
			TextPointer textPointer = position;
			textPointer = TextRangeEdit.SplitFormattingElements(textPointer, true);
			Invariant.Assert(textPointer.Parent == paragraph, "breakPosition must be in paragraph scope after splitting formatting elements");
			bool flag2 = TextPointerBase.GetImmediateListItem(paragraph.ContentStart) != null;
			textPointer = TextRangeEdit.SplitElement(textPointer);
			if (flag2)
			{
				Invariant.Assert(textPointer.Parent is ListItem, "breakPosition must be in ListItem scope");
				textPointer = TextRangeEdit.SplitElement(textPointer);
			}
			if (moveIntoSecondParagraph)
			{
				while (!(textPointer.Parent is Paragraph) && textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				}
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			}
			return textPointer;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0010D364 File Offset: 0x0010B564
		internal static TextPointer InsertLineBreak(TextPointer position)
		{
			if (!TextSchema.IsValidChild(position, typeof(LineBreak)))
			{
				position = TextRangeEditTables.EnsureInsertionPosition(position);
			}
			if (TextSchema.IsInTextContent(position))
			{
				position = TextRangeEdit.SplitElement(position);
			}
			Invariant.Assert(TextSchema.IsValidChild(position, typeof(LineBreak)), "position must be in valid scope now to insert a LineBreak element");
			LineBreak lineBreak = new LineBreak();
			position.InsertTextElement(lineBreak);
			return lineBreak.ElementEnd.GetInsertionPosition(LogicalDirection.Forward);
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0010D3CE File Offset: 0x0010B5CE
		internal static void SetParagraphProperty(TextPointer start, TextPointer end, DependencyProperty property, object value)
		{
			TextRangeEdit.SetParagraphProperty(start, end, property, value, PropertyValueAction.SetValue);
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x0010D3DC File Offset: 0x0010B5DC
		internal static void SetParagraphProperty(TextPointer start, TextPointer end, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			Invariant.Assert(start.CompareTo(end) <= 0, "expecting: start <= end");
			Invariant.Assert(property != null, "null check: property");
			end = (TextPointer)TextRangeEdit.GetAdjustedRangeEnd(start, end);
			Block paragraphOrBlockUIContainer = start.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				start = paragraphOrBlockUIContainer.ContentStart;
			}
			if (property == Block.FlowDirectionProperty)
			{
				if (!TextRangeEditLists.SplitListsForFlowDirectionChange(start, end, value))
				{
					return;
				}
				ListItem listAncestor = start.GetListAncestor();
				if (listAncestor != null && listAncestor.List != null)
				{
					start = listAncestor.List.ElementStart;
				}
			}
			TextRangeEdit.SetParagraphPropertyWorker(start, end, property, value, propertyValueAction);
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x0010D488 File Offset: 0x0010B688
		private static void SetParagraphPropertyWorker(TextPointer start, TextPointer end, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			for (Block nextBlock = TextRangeEdit.GetNextBlock(start, end); nextBlock != null; nextBlock = TextRangeEdit.GetNextBlock(start, end))
			{
				if (TextSchema.IsParagraphOrBlockUIContainer(nextBlock.GetType()))
				{
					DependencyObject parent = start.TextContainer.Parent;
					TextRangeEdit.SetPropertyOnParagraphOrBlockUIContainer(parent, nextBlock, property, value, propertyValueAction);
					start = nextBlock.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				}
				else if (nextBlock is List)
				{
					TextPointer textPointer = nextBlock.ContentStart.GetPositionAtOffset(0, LogicalDirection.Forward);
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
					TextPointer contentEnd = nextBlock.ContentEnd;
					TextRangeEdit.SetParagraphPropertyWorker(textPointer, contentEnd, property, value, propertyValueAction);
					if (property == Block.FlowDirectionProperty)
					{
						object value2 = nextBlock.GetValue(property);
						TextRangeEdit.SetPropertyValue(nextBlock, property, value2, value);
						if (!object.Equals(value2, value))
						{
							TextRangeEdit.SwapBlockLeftAndRightMargins(nextBlock);
						}
					}
					start = nextBlock.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				}
			}
		}

		// Token: 0x06003B56 RID: 15190 RVA: 0x0010D550 File Offset: 0x0010B750
		private static void SetPropertyOnParagraphOrBlockUIContainer(DependencyObject parent, Block block, DependencyProperty property, object value, PropertyValueAction propertyValueAction)
		{
			FlowDirection parentFlowDirection;
			if (parent != null)
			{
				parentFlowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
			}
			else
			{
				parentFlowDirection = (FlowDirection)FrameworkElement.FlowDirectionProperty.GetDefaultValue(typeof(FrameworkElement));
			}
			FlowDirection flowDirection = (FlowDirection)block.GetValue(Block.FlowDirectionProperty);
			object value2 = block.GetValue(property);
			object obj = value;
			TextRangeEdit.PreserveBlockContentStructuralProperty(block, property, value2, value);
			if (property.PropertyType == typeof(Thickness))
			{
				Invariant.Assert(value2 is Thickness, "Expecting the currentValue to be of Thinkness type");
				Invariant.Assert(obj is Thickness, "Expecting the newValue to be of Thinkness type");
				obj = TextRangeEdit.ComputeNewThicknessValue((Thickness)value2, (Thickness)obj, parentFlowDirection, flowDirection, propertyValueAction);
			}
			else if (property == Block.TextAlignmentProperty)
			{
				Invariant.Assert(value is TextAlignment, "Expecting TextAlignment as a value of a Paragraph.TextAlignmentProperty");
				obj = TextRangeEdit.ComputeNewTextAlignmentValue((TextAlignment)value, flowDirection);
				if (block is BlockUIContainer)
				{
					UIElement child = ((BlockUIContainer)block).Child;
					if (child != null)
					{
						HorizontalAlignment horizontalAlignmentFromTextAlignment = TextRangeEdit.GetHorizontalAlignmentFromTextAlignment((TextAlignment)obj);
						UIElementPropertyUndoUnit.Add(block.TextContainer, child, FrameworkElement.HorizontalAlignmentProperty, horizontalAlignmentFromTextAlignment);
						child.SetValue(FrameworkElement.HorizontalAlignmentProperty, horizontalAlignmentFromTextAlignment);
					}
				}
			}
			else if (value2 is double)
			{
				obj = TextRangeEdit.GetNewDoubleValue(property, (double)value2, (double)obj, propertyValueAction);
			}
			TextRangeEdit.SetPropertyValue(block, property, value2, obj);
			if (property == Block.FlowDirectionProperty && !object.Equals(value2, obj))
			{
				TextRangeEdit.SwapBlockLeftAndRightMargins(block);
			}
		}

		// Token: 0x06003B57 RID: 15191 RVA: 0x0010D6CC File Offset: 0x0010B8CC
		private static void PreserveBlockContentStructuralProperty(Block block, DependencyProperty property, object currentValue, object newValue)
		{
			Paragraph paragraph = block as Paragraph;
			if (paragraph != null && TextSchema.IsStructuralCharacterProperty(property) && !TextSchema.ValuesAreEqual(currentValue, newValue))
			{
				Inline inline = paragraph.Inlines.FirstInline;
				Inline inline2 = paragraph.Inlines.LastInline;
				while (inline != null && inline == inline2 && inline is Span && !TextRangeEdit.HasLocalPropertyValue(inline, property))
				{
					inline = ((Span)inline).Inlines.FirstInline;
					inline2 = ((Span)inline2).Inlines.LastInline;
				}
				if (inline != inline2)
				{
					do
					{
						object value = inline.GetValue(property);
						inline2 = inline;
						Inline inline3;
						for (;;)
						{
							inline3 = (Inline)inline2.NextElement;
							if (inline3 == null || !TextSchema.ValuesAreEqual(inline3.GetValue(property), value))
							{
								break;
							}
							inline2 = inline3;
						}
						if (TextSchema.ValuesAreEqual(value, currentValue))
						{
							if (inline != inline2)
							{
								TextPointer frozenPointer = inline.ElementStart.GetFrozenPointer(LogicalDirection.Backward);
								TextPointer frozenPointer2 = inline2.ElementEnd.GetFrozenPointer(LogicalDirection.Forward);
								TextRangeEdit.SetStructuralInlineProperty(frozenPointer, frozenPointer2, property, currentValue);
								inline = (Inline)frozenPointer.GetAdjacentElement(LogicalDirection.Forward);
								inline2 = (Inline)frozenPointer2.GetAdjacentElement(LogicalDirection.Backward);
								if (inline != inline2)
								{
									Span span = inline.Parent as Span;
									if (span == null || span.Inlines.FirstInline != inline || span.Inlines.LastInline != inline2)
									{
										span = new Span(inline.ElementStart, inline2.ElementEnd);
									}
									span.SetValue(property, currentValue);
								}
							}
							if (inline == inline2)
							{
								TextRangeEdit.SetStructuralPropertyOnInline(inline, property, currentValue);
							}
						}
						inline = inline3;
					}
					while (inline != null);
					return;
				}
				TextRangeEdit.SetStructuralPropertyOnInline(inline, property, currentValue);
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x0010D849 File Offset: 0x0010BA49
		private static void SetStructuralPropertyOnInline(Inline inline, DependencyProperty property, object value)
		{
			if (inline is Run && !inline.IsEmpty && !TextRangeEdit.HasLocalPropertyValue(inline, property))
			{
				inline.SetValue(property, value);
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x0010D86C File Offset: 0x0010BA6C
		private static Block GetNextBlock(TextPointer pointer, TextPointer limit)
		{
			Block block = null;
			while (pointer != null && pointer.CompareTo(limit) <= 0)
			{
				if (pointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
				{
					block = (pointer.Parent as Block);
					if (block is Paragraph || block is BlockUIContainer || block is List)
					{
						break;
					}
				}
				if (TextPointerBase.IsAtPotentialParagraphPosition(pointer))
				{
					pointer = TextRangeEditTables.EnsureInsertionPosition(pointer);
					block = pointer.Paragraph;
					Invariant.Assert(block != null);
					break;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return block;
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0010D8E4 File Offset: 0x0010BAE4
		private static Thickness ComputeNewThicknessValue(Thickness currentThickness, Thickness newThickness, FlowDirection parentFlowDirection, FlowDirection flowDirection, PropertyValueAction propertyValueAction)
		{
			double top = (newThickness.Top < 0.0) ? currentThickness.Top : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Top, newThickness.Top, propertyValueAction);
			double bottom = (newThickness.Bottom < 0.0) ? currentThickness.Bottom : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Bottom, newThickness.Bottom, propertyValueAction);
			double left;
			double right;
			if (parentFlowDirection != flowDirection)
			{
				left = ((newThickness.Right < 0.0) ? currentThickness.Left : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Left, newThickness.Right, propertyValueAction));
				right = ((newThickness.Left < 0.0) ? currentThickness.Right : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Right, newThickness.Left, propertyValueAction));
			}
			else
			{
				left = ((newThickness.Left < 0.0) ? currentThickness.Left : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Left, newThickness.Left, propertyValueAction));
				right = ((newThickness.Right < 0.0) ? currentThickness.Right : TextRangeEdit.GetNewDoubleValue(null, currentThickness.Right, newThickness.Right, propertyValueAction));
			}
			return new Thickness(left, top, right, bottom);
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x0010DA2C File Offset: 0x0010BC2C
		private static TextAlignment ComputeNewTextAlignmentValue(TextAlignment textAlignment, FlowDirection flowDirection)
		{
			if (textAlignment == TextAlignment.Left)
			{
				textAlignment = ((flowDirection == FlowDirection.LeftToRight) ? TextAlignment.Left : TextAlignment.Right);
			}
			else if (textAlignment == TextAlignment.Right)
			{
				textAlignment = ((flowDirection == FlowDirection.LeftToRight) ? TextAlignment.Right : TextAlignment.Left);
			}
			return textAlignment;
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0010DA4C File Offset: 0x0010BC4C
		private static double GetNewDoubleValue(DependencyProperty property, double currentValue, double newValue, PropertyValueAction propertyValueAction)
		{
			double value = TextRangeEdit.NewValue(currentValue, newValue, propertyValueAction);
			return TextRangeEdit.DoublePropertyBounds.GetClosestValidValue(property, value);
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x0010DA6C File Offset: 0x0010BC6C
		private static double NewValue(double currentValue, double newValue, PropertyValueAction propertyValueAction)
		{
			if (double.IsNaN(newValue))
			{
				return newValue;
			}
			if (double.IsNaN(currentValue))
			{
				currentValue = 0.0;
			}
			newValue = ((propertyValueAction == PropertyValueAction.IncreaseByAbsoluteValue) ? (currentValue + newValue) : ((propertyValueAction == PropertyValueAction.DecreaseByAbsoluteValue) ? (currentValue - newValue) : ((propertyValueAction == PropertyValueAction.IncreaseByPercentageValue) ? (currentValue * (1.0 + newValue / 100.0)) : ((propertyValueAction == PropertyValueAction.DecreaseByPercentageValue) ? (currentValue * (1.0 - newValue / 100.0)) : newValue))));
			return newValue;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0010DAE8 File Offset: 0x0010BCE8
		internal static HorizontalAlignment GetHorizontalAlignmentFromTextAlignment(TextAlignment textAlignment)
		{
			HorizontalAlignment result;
			switch (textAlignment)
			{
			default:
				result = HorizontalAlignment.Left;
				break;
			case TextAlignment.Right:
				result = HorizontalAlignment.Right;
				break;
			case TextAlignment.Center:
				result = HorizontalAlignment.Center;
				break;
			case TextAlignment.Justify:
				result = HorizontalAlignment.Stretch;
				break;
			}
			return result;
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x0010DB1C File Offset: 0x0010BD1C
		internal static TextAlignment GetTextAlignmentFromHorizontalAlignment(HorizontalAlignment horizontalAlignment)
		{
			switch (horizontalAlignment)
			{
			case HorizontalAlignment.Left:
				return TextAlignment.Left;
			case HorizontalAlignment.Center:
				return TextAlignment.Center;
			case HorizontalAlignment.Right:
				return TextAlignment.Right;
			}
			return TextAlignment.Justify;
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x0010DB50 File Offset: 0x0010BD50
		private static void SetPropertyValue(TextElement element, DependencyProperty property, object currentValue, object newValue)
		{
			if (!TextSchema.ValuesAreEqual(newValue, currentValue))
			{
				element.ClearValue(property);
				if (!TextSchema.ValuesAreEqual(newValue, element.GetValue(property)))
				{
					element.SetValue(property, newValue);
				}
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x0010DB7C File Offset: 0x0010BD7C
		private static void SwapBlockLeftAndRightMargins(Block block)
		{
			object value = block.GetValue(Block.MarginProperty);
			if (value is Thickness && !Paragraph.IsMarginAuto((Thickness)value))
			{
				object newValue = new Thickness(((Thickness)value).Right, ((Thickness)value).Top, ((Thickness)value).Left, ((Thickness)value).Bottom);
				TextRangeEdit.SetPropertyValue(block, Block.MarginProperty, value, newValue);
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x0010DBFA File Offset: 0x0010BDFA
		internal static ITextPointer GetAdjustedRangeEnd(ITextPointer rangeStart, ITextPointer rangeEnd)
		{
			if (rangeStart.CompareTo(rangeEnd) < 0 && rangeEnd.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				rangeEnd = rangeEnd.GetNextInsertionPosition(LogicalDirection.Backward);
				if (rangeEnd == null)
				{
					rangeEnd = rangeStart;
				}
			}
			else if (TextPointerBase.IsAfterLastParagraph(rangeEnd))
			{
				rangeEnd = rangeEnd.GetInsertionPosition(LogicalDirection.Backward);
			}
			return rangeEnd;
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0010DC34 File Offset: 0x0010BE34
		internal static void MergeFlowDirection(TextPointer position)
		{
			TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Backward);
			TextPointerContext pointerContext2 = position.GetPointerContext(LogicalDirection.Forward);
			if (pointerContext != TextPointerContext.ElementStart && pointerContext != TextPointerContext.ElementEnd && pointerContext2 != TextPointerContext.ElementStart && pointerContext2 != TextPointerContext.ElementEnd)
			{
				return;
			}
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (!TextSchema.IsMergeableInline(position.Parent.GetType()))
				{
					break;
				}
				position = ((Inline)position.Parent).ElementStart;
			}
			while (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(position.Parent.GetType()))
			{
				position = ((Inline)position.Parent).ElementEnd;
			}
			TextElement textElement = position.Parent as TextElement;
			if (!(textElement is Span) && !(textElement is Paragraph))
			{
				return;
			}
			TextPointer textPointer = position.CreatePointer();
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd && TextSchema.IsMergeableInline(textPointer.GetAdjacentElement(LogicalDirection.Backward).GetType()))
			{
				textPointer = ((Inline)textPointer.GetAdjacentElement(LogicalDirection.Backward)).ContentEnd;
			}
			Run run = textPointer.Parent as Run;
			TextPointer textPointer2 = position.CreatePointer();
			while (textPointer2.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart && TextSchema.IsMergeableInline(textPointer2.GetAdjacentElement(LogicalDirection.Forward).GetType()))
			{
				textPointer2 = ((Inline)textPointer2.GetAdjacentElement(LogicalDirection.Forward)).ContentStart;
			}
			Run run2 = textPointer2.Parent as Run;
			if (run == null || run.IsEmpty || run2 == null || run2.IsEmpty)
			{
				return;
			}
			FlowDirection flowDirection = (FlowDirection)textElement.GetValue(FrameworkElement.FlowDirectionProperty);
			FlowDirection flowDirection2 = (FlowDirection)run.GetValue(FrameworkElement.FlowDirectionProperty);
			FlowDirection flowDirection3 = (FlowDirection)run2.GetValue(FrameworkElement.FlowDirectionProperty);
			if (flowDirection2 == flowDirection3 && flowDirection2 != flowDirection)
			{
				Inline scopingFlowDirectionInline = TextRangeEdit.GetScopingFlowDirectionInline(run);
				Inline scopingFlowDirectionInline2 = TextRangeEdit.GetScopingFlowDirectionInline(run2);
				TextRangeEdit.SetStructuralInlineProperty(scopingFlowDirectionInline.ElementStart, scopingFlowDirectionInline2.ElementEnd, FrameworkElement.FlowDirectionProperty, flowDirection2);
			}
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x0010DDFD File Offset: 0x0010BFFD
		internal static bool CanApplyStructuralInlineProperty(TextPointer start, TextPointer end)
		{
			return TextRangeEdit.ValidateApplyStructuralInlineProperty(start, end, TextPointer.GetCommonAncestor(start, end), null);
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x0010DE10 File Offset: 0x0010C010
		internal static void IncrementParagraphLeadingMargin(TextRange range, double increment, PropertyValueAction propertyValueAction)
		{
			Invariant.Assert(increment >= 0.0);
			Invariant.Assert(propertyValueAction > PropertyValueAction.SetValue);
			if (increment == 0.0)
			{
				return;
			}
			Thickness thickness = new Thickness(increment, -1.0, -1.0, -1.0);
			TextRangeEdit.SetParagraphProperty(range.Start, range.End, Block.MarginProperty, thickness, propertyValueAction);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0010DE87 File Offset: 0x0010C087
		internal static void DeleteInlineContent(ITextPointer start, ITextPointer end)
		{
			TextRangeEdit.DeleteParagraphContent(start, end);
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0010DE90 File Offset: 0x0010C090
		internal static void DeleteParagraphContent(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			Invariant.Assert(start.CompareTo(end) <= 0, "expecting: start <= end");
			if (!(start is TextPointer))
			{
				start.DeleteContentToPosition(end);
				return;
			}
			TextPointer textPointer = (TextPointer)start;
			TextPointer textPointer2 = (TextPointer)end;
			TextRangeEdit.DeleteEquiScopedContent(textPointer, textPointer2);
			TextRangeEdit.DeleteEquiScopedContent(textPointer2, textPointer);
			if (textPointer.CompareTo(textPointer2) < 0)
			{
				if (TextPointerBase.IsAfterLastParagraph(textPointer2))
				{
					while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
					{
						if (textPointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementEnd)
						{
							break;
						}
						TextElement textElement = (TextElement)textPointer.Parent;
						if (!(textElement is Inline) && !TextSchema.AllowsParagraphMerging(textElement.GetType()))
						{
							break;
						}
						textElement.RepositionWithContent(null);
					}
				}
				else
				{
					Block block = textPointer.ParagraphOrBlockUIContainer;
					Block block2 = textPointer2.ParagraphOrBlockUIContainer;
					if (block == null && TextPointerBase.IsInEmptyListItem(textPointer))
					{
						textPointer = TextRangeEditTables.EnsureInsertionPosition(textPointer);
						block = textPointer.Paragraph;
						Invariant.Assert(block != null, "EnsureInsertionPosition must create a paragraph inside list item - 1");
					}
					if (block2 == null && TextPointerBase.IsInEmptyListItem(textPointer2))
					{
						textPointer2 = TextRangeEditTables.EnsureInsertionPosition(textPointer2);
						block2 = textPointer2.Paragraph;
						Invariant.Assert(block2 != null, "EnsureInsertionPosition must create a paragraph inside list item - 2");
					}
					if (block != null && block2 != null)
					{
						TextRangeEditLists.MergeParagraphs(block, block2);
					}
					else
					{
						TextRangeEdit.MergeEmptyParagraphsAndBlockUIContainers(textPointer, textPointer2);
					}
				}
			}
			TextRangeEdit.MergeFormattingInlines(textPointer);
			TextRangeEdit.MergeFormattingInlines(textPointer2);
			if (textPointer.Parent is BlockUIContainer && ((BlockUIContainer)textPointer.Parent).IsEmpty)
			{
				((BlockUIContainer)textPointer.Parent).Reposition(null, null);
				return;
			}
			if (textPointer.Parent is Hyperlink && ((Hyperlink)textPointer.Parent).IsEmpty)
			{
				((Hyperlink)textPointer.Parent).Reposition(null, null);
				TextRangeEdit.MergeFormattingInlines(textPointer);
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0010E048 File Offset: 0x0010C248
		private static void MergeEmptyParagraphsAndBlockUIContainers(TextPointer startPosition, TextPointer endPosition)
		{
			Block paragraphOrBlockUIContainer = startPosition.ParagraphOrBlockUIContainer;
			Block paragraphOrBlockUIContainer2 = endPosition.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer is BlockUIContainer)
			{
				if (paragraphOrBlockUIContainer.IsEmpty)
				{
					paragraphOrBlockUIContainer.Reposition(null, null);
					return;
				}
				if (paragraphOrBlockUIContainer2 is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer2))
				{
					paragraphOrBlockUIContainer2.RepositionWithContent(null);
					return;
				}
			}
			if (paragraphOrBlockUIContainer2 is BlockUIContainer)
			{
				if (paragraphOrBlockUIContainer2.IsEmpty)
				{
					paragraphOrBlockUIContainer2.Reposition(null, null);
					return;
				}
				if (paragraphOrBlockUIContainer2 is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer))
				{
					paragraphOrBlockUIContainer.RepositionWithContent(null);
					return;
				}
			}
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0010E0D0 File Offset: 0x0010C2D0
		private static void DeleteEquiScopedContent(TextPointer start, TextPointer end)
		{
			Invariant.Assert(start != null, "null check: start");
			Invariant.Assert(end != null, "null check: end");
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			if (start.Parent == end.Parent)
			{
				TextRangeEdit.DeleteContentBetweenPositions(start, end);
				return;
			}
			LogicalDirection logicalDirection;
			LogicalDirection direction;
			TextPointerContext textPointerContext;
			TextPointerContext textPointerContext2;
			ElementEdge edge;
			ElementEdge edge2;
			if (start.CompareTo(end) < 0)
			{
				logicalDirection = LogicalDirection.Forward;
				direction = LogicalDirection.Backward;
				textPointerContext = TextPointerContext.ElementStart;
				textPointerContext2 = TextPointerContext.ElementEnd;
				edge = ElementEdge.BeforeStart;
				edge2 = ElementEdge.AfterEnd;
			}
			else
			{
				logicalDirection = LogicalDirection.Backward;
				direction = LogicalDirection.Forward;
				textPointerContext = TextPointerContext.ElementEnd;
				textPointerContext2 = TextPointerContext.ElementStart;
				edge = ElementEdge.AfterEnd;
				edge2 = ElementEdge.BeforeStart;
			}
			TextPointer textPointer = new TextPointer(start);
			TextPointer textPointer2 = new TextPointer(start);
			while (textPointer2.CompareTo(end) != 0)
			{
				Invariant.Assert((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) < 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) > 0), "Inappropriate position ordering");
				Invariant.Assert(textPointer.Parent == textPointer2.Parent, "inconsistent position Parents: previous and next");
				TextPointerContext pointerContext = textPointer2.GetPointerContext(logicalDirection);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement)
				{
					textPointer2.MoveToNextContextPosition(logicalDirection);
					if ((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) > 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) < 0))
					{
						Invariant.Assert(textPointer2.Parent == end.Parent, "inconsistent poaition Parents: next and end");
						textPointer2.MoveToPosition(end);
						break;
					}
				}
				else if (pointerContext == textPointerContext)
				{
					textPointer2.MoveToNextContextPosition(logicalDirection);
					((ITextPointer)textPointer2).MoveToElementEdge(edge2);
					if ((logicalDirection == LogicalDirection.Forward && textPointer2.CompareTo(end) >= 0) || (logicalDirection == LogicalDirection.Backward && textPointer2.CompareTo(end) <= 0))
					{
						textPointer2.MoveToNextContextPosition(direction);
						((ITextPointer)textPointer2).MoveToElementEdge(edge);
						break;
					}
				}
				else
				{
					if (pointerContext != textPointerContext2)
					{
						Invariant.Assert(false, "Not expecting None context here");
						Invariant.Assert(pointerContext == TextPointerContext.None, "Unknown pointer context");
						break;
					}
					TextRangeEdit.DeleteContentBetweenPositions(textPointer, textPointer2);
					if (!TextRangeEdit.ExtractEmptyFormattingElements(textPointer))
					{
						Invariant.Assert(textPointer2.GetPointerContext(logicalDirection) == textPointerContext2, "Unexpected context of nextPosition");
						textPointer2.MoveToNextContextPosition(logicalDirection);
					}
					textPointer.MoveToPosition(textPointer2);
				}
			}
			Invariant.Assert(textPointer.Parent == textPointer2.Parent, "inconsistent Parents: previousPosition, nextPosition");
			TextRangeEdit.DeleteContentBetweenPositions(textPointer, textPointer2);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0010E2DC File Offset: 0x0010C4DC
		private static bool DeleteContentBetweenPositions(TextPointer one, TextPointer two)
		{
			Invariant.Assert(one.Parent == two.Parent, "inconsistent Parents: one and two");
			if (one.CompareTo(two) < 0)
			{
				one.TextContainer.DeleteContentInternal(one, two);
			}
			else if (one.CompareTo(two) > 0)
			{
				two.TextContainer.DeleteContentInternal(two, one);
			}
			Invariant.Assert(one.CompareTo(two) == 0, "Positions one and two must be equal now");
			return false;
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0010E346 File Offset: 0x0010C546
		private static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting, TextElement limitingAncestor)
		{
			return TextRangeEdit.SplitFormattingElements(splitPosition, keepEmptyFormatting, false, limitingAncestor);
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0010E354 File Offset: 0x0010C554
		private static TextPointer SplitFormattingElements(TextPointer splitPosition, bool keepEmptyFormatting, bool preserveStructuralFormatting, TextElement limitingAncestor)
		{
			if (preserveStructuralFormatting)
			{
				Run run = splitPosition.Parent as Run;
				if (run != null && run != limitingAncestor && ((run.Parent != null && TextRangeEdit.HasLocalInheritableStructuralPropertyValue(run)) || (run.Parent == null && TextRangeEdit.HasLocalStructuralPropertyValue(run))))
				{
					Span destination = new Span(run.ElementStart, run.ElementEnd);
					TextRangeEdit.TransferStructuralProperties(run, destination);
				}
			}
			while (splitPosition.Parent != null && TextSchema.IsMergeableInline(splitPosition.Parent.GetType()) && splitPosition.Parent != limitingAncestor && (!preserveStructuralFormatting || (((Inline)splitPosition.Parent).Parent != null && !TextRangeEdit.HasLocalInheritableStructuralPropertyValue((Inline)splitPosition.Parent)) || (((Inline)splitPosition.Parent).Parent == null && !TextRangeEdit.HasLocalStructuralPropertyValue((Inline)splitPosition.Parent))))
			{
				splitPosition = TextRangeEdit.SplitFormattingElement(splitPosition, keepEmptyFormatting);
			}
			return splitPosition;
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0010E42C File Offset: 0x0010C62C
		private static void TransferStructuralProperties(Inline source, Inline destination)
		{
			bool flag = source.Parent == destination;
			for (int i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty dp = TextSchema.StructuralCharacterProperties[i];
				if ((flag && TextRangeEdit.HasLocalInheritableStructuralPropertyValue(source)) || (!flag && TextRangeEdit.HasLocalStructuralPropertyValue(source)))
				{
					object value = source.GetValue(dp);
					source.ClearValue(dp);
					destination.SetValue(dp, value);
				}
			}
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0010E48C File Offset: 0x0010C68C
		private static bool HasWriteableLocalPropertyValues(Inline inline)
		{
			LocalValueEnumerator localValueEnumerator = inline.GetLocalValueEnumerator();
			bool flag = false;
			while (!flag && localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				flag = !localValueEntry.Property.ReadOnly;
			}
			return flag;
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0010E4CC File Offset: 0x0010C6CC
		private static bool HasLocalInheritableStructuralPropertyValue(Inline inline)
		{
			int i;
			for (i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty dp = TextSchema.StructuralCharacterProperties[i];
				if (!TextSchema.ValuesAreEqual(inline.GetValue(dp), inline.Parent.GetValue(dp)))
				{
					break;
				}
			}
			return i < TextSchema.StructuralCharacterProperties.Length;
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x0010E518 File Offset: 0x0010C718
		private static bool HasLocalStructuralPropertyValue(Inline inline)
		{
			int i;
			for (i = 0; i < TextSchema.StructuralCharacterProperties.Length; i++)
			{
				DependencyProperty property = TextSchema.StructuralCharacterProperties[i];
				if (TextRangeEdit.HasLocalPropertyValue(inline, property))
				{
					break;
				}
			}
			return i < TextSchema.StructuralCharacterProperties.Length;
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0010E554 File Offset: 0x0010C754
		private static bool HasLocalPropertyValue(Inline inline, DependencyProperty property)
		{
			bool flag;
			BaseValueSourceInternal valueSource = inline.GetValueSource(property, null, out flag);
			return valueSource != BaseValueSourceInternal.Unknown && valueSource != BaseValueSourceInternal.Default && valueSource != BaseValueSourceInternal.Inherited;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0010E57C File Offset: 0x0010C77C
		private static Inline GetScopingFlowDirectionInline(Run run)
		{
			FlowDirection flowDirection = run.FlowDirection;
			Inline inline = run;
			while ((FlowDirection)inline.Parent.GetValue(FrameworkElement.FlowDirectionProperty) == flowDirection)
			{
				inline = (Span)inline.Parent;
			}
			return inline;
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0010E5BC File Offset: 0x0010C7BC
		private static void SetNonStructuralInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value, PropertyValueAction propertyValueAction)
		{
			start = TextRangeEdit.SplitFormattingElements(start, false, true, null);
			end = TextRangeEdit.SplitFormattingElements(end, false, true, null);
			TextPointer textPointer;
			for (Run nextRun = TextRangeEdit.GetNextRun(start, end); nextRun != null; nextRun = TextRangeEdit.GetNextRun(textPointer, end))
			{
				object value2 = nextRun.GetValue(formattingProperty);
				object newValue = value;
				if (propertyValueAction != PropertyValueAction.SetValue)
				{
					Invariant.Assert(formattingProperty == TextElement.FontSizeProperty, "Only FontSize can be incremented/decremented among character properties");
					newValue = TextRangeEdit.GetNewFontSizeValue((double)value2, (double)value, propertyValueAction);
				}
				TextRangeEdit.SetPropertyValue(nextRun, formattingProperty, value2, newValue);
				textPointer = nextRun.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				if (TextPointerBase.IsAtPotentialRunPosition(nextRun))
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
				}
				TextRangeEdit.MergeFormattingInlines(nextRun.ContentStart);
			}
			TextRangeEdit.MergeFormattingInlines(end);
		}

		// Token: 0x06003B74 RID: 15220 RVA: 0x0010E668 File Offset: 0x0010C868
		private static double GetNewFontSizeValue(double currentValue, double value, PropertyValueAction propertyValueAction)
		{
			double num = value;
			if (propertyValueAction == PropertyValueAction.IncreaseByAbsoluteValue)
			{
				num = currentValue + value;
			}
			else if (propertyValueAction == PropertyValueAction.DecreaseByAbsoluteValue)
			{
				num = currentValue - value;
			}
			if (num < 0.75)
			{
				num = 0.75;
			}
			else if (num > 1638.0)
			{
				num = 1638.0;
			}
			return num;
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0010E6B8 File Offset: 0x0010C8B8
		private static void SetStructuralInlineProperty(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value)
		{
			DependencyObject commonAncestor = TextPointer.GetCommonAncestor(start, end);
			TextRangeEdit.ValidateApplyStructuralInlineProperty(start, end, commonAncestor, formattingProperty);
			if (commonAncestor is Run)
			{
				TextRangeEdit.ApplyStructuralInlinePropertyAcrossRun(start, end, (Run)commonAncestor, formattingProperty, value);
				return;
			}
			if ((commonAncestor is Inline && !(commonAncestor is AnchoredBlock)) || commonAncestor is Paragraph)
			{
				Invariant.Assert(!(commonAncestor is InlineUIContainer));
				TextRangeEdit.ApplyStructuralInlinePropertyAcrossInline(start, end, (TextElement)commonAncestor, formattingProperty, value);
				return;
			}
			TextRangeEdit.ApplyStructuralInlinePropertyAcrossParagraphs(start, end, formattingProperty, value);
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0010E734 File Offset: 0x0010C934
		private static void FixupStructuralPropertyEnvironment(Inline inline, DependencyProperty property)
		{
			TextRangeEdit.ClearParentStructuralPropertyValue(inline, property);
			for (Inline inline2 = inline; inline2 != null; inline2 = (inline2.Parent as Span))
			{
				Inline inline3 = (Inline)inline2.PreviousElement;
				if (inline3 != null)
				{
					TextRangeEdit.FlattenStructuralProperties(inline3);
					break;
				}
			}
			for (Inline inline4 = inline; inline4 != null; inline4 = (inline4.Parent as Span))
			{
				Inline inline5 = (Inline)inline4.NextElement;
				if (inline5 != null)
				{
					TextRangeEdit.FlattenStructuralProperties(inline5);
					return;
				}
			}
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0010E79C File Offset: 0x0010C99C
		private static void FlattenStructuralProperties(Inline inline)
		{
			Span span = inline as Span;
			for (Span span2 = inline.Parent as Span; span2 != null; span2 = (span2.Parent as Span))
			{
				if (span2.Inlines.FirstInline != span2.Inlines.LastInline)
				{
					break;
				}
				span = span2;
			}
			while (span != null && span.Inlines.FirstInline == span.Inlines.LastInline)
			{
				Inline firstInline = span.Inlines.FirstInline;
				TextRangeEdit.TransferStructuralProperties(span, firstInline);
				if (TextSchema.IsMergeableInline(span.GetType()) && TextSchema.IsKnownType(span.GetType()) && !TextRangeEdit.HasWriteableLocalPropertyValues(span))
				{
					span.Reposition(null, null);
				}
				span = (firstInline as Span);
			}
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0010E848 File Offset: 0x0010CA48
		private static void ClearParentStructuralPropertyValue(Inline child, DependencyProperty property)
		{
			Span span = null;
			Span span2 = child.Parent as Span;
			while (span2 != null && TextSchema.IsMergeableInline(span2.GetType()))
			{
				if (TextRangeEdit.HasLocalPropertyValue(span2, property))
				{
					span = span2;
				}
				span2 = (span2.Parent as Span);
			}
			if (span != null)
			{
				TextElement limitingAncestor = (TextElement)span.Parent;
				TextRangeEdit.SplitFormattingElements(child.ElementStart, false, limitingAncestor);
				TextPointer textPointer = TextRangeEdit.SplitFormattingElements(child.ElementEnd, false, limitingAncestor);
				Span span3 = (Span)textPointer.GetAdjacentElement(LogicalDirection.Backward);
				while (span3 != null && span3 != child)
				{
					span3.ClearValue(property);
					Span span4 = span3.Inlines.FirstInline as Span;
					if (!TextRangeEdit.HasWriteableLocalPropertyValues(span3))
					{
						span3.Reposition(null, null);
					}
					span3 = span4;
				}
			}
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0010E904 File Offset: 0x0010CB04
		private static Run GetNextRun(TextPointer pointer, TextPointer limit)
		{
			Run result = null;
			while (pointer != null && pointer.CompareTo(limit) < 0 && (pointer.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementStart || (result = (pointer.GetAdjacentElement(LogicalDirection.Forward) as Run)) == null))
			{
				if (TextPointerBase.IsAtPotentialRunPosition(pointer))
				{
					pointer = TextRangeEditTables.EnsureInsertionPosition(pointer);
					Invariant.Assert(pointer.Parent is Run);
					result = (pointer.Parent as Run);
					break;
				}
				pointer = pointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return result;
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0010E978 File Offset: 0x0010CB78
		private static void ClearPropertyValueFromSpansAndRuns(TextPointer start, TextPointer end, DependencyProperty formattingProperty)
		{
			start = start.GetPositionAtOffset(0, LogicalDirection.Forward);
			start = start.GetNextContextPosition(LogicalDirection.Forward);
			while (start != null && start.CompareTo(end) < 0)
			{
				if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && TextSchema.IsFormattingType(start.Parent.GetType()))
				{
					start.Parent.ClearValue(formattingProperty);
					TextRangeEdit.MergeFormattingInlines(start);
				}
				start = start.GetNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0010E9E0 File Offset: 0x0010CBE0
		private static void ApplyStructuralInlinePropertyAcrossRun(TextPointer start, TextPointer end, Run run, DependencyProperty formattingProperty, object value)
		{
			if (start.CompareTo(end) == 0)
			{
				if (run.IsEmpty)
				{
					run.SetValue(formattingProperty, value);
				}
			}
			else
			{
				start = TextRangeEdit.SplitFormattingElements(start, false, run.Parent as TextElement);
				end = TextRangeEdit.SplitFormattingElements(end, false, run.Parent as TextElement);
				run = (Run)start.GetAdjacentElement(LogicalDirection.Forward);
				run.SetValue(formattingProperty, value);
			}
			TextRangeEdit.FixupStructuralPropertyEnvironment(run, formattingProperty);
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0010EA50 File Offset: 0x0010CC50
		private static void ApplyStructuralInlinePropertyAcrossInline(TextPointer start, TextPointer end, TextElement commonAncestor, DependencyProperty formattingProperty, object value)
		{
			start = TextRangeEdit.SplitFormattingElements(start, false, commonAncestor);
			end = TextRangeEdit.SplitFormattingElements(end, false, commonAncestor);
			DependencyObject adjacentElement = start.GetAdjacentElement(LogicalDirection.Forward);
			DependencyObject adjacentElement2 = end.GetAdjacentElement(LogicalDirection.Backward);
			if (adjacentElement == adjacentElement2 && (adjacentElement is Run || adjacentElement is Span))
			{
				Inline inline = (Inline)start.GetAdjacentElement(LogicalDirection.Forward);
				inline.SetValue(formattingProperty, value);
				TextRangeEdit.FixupStructuralPropertyEnvironment(inline, formattingProperty);
				if (adjacentElement is Span)
				{
					TextRangeEdit.ClearPropertyValueFromSpansAndRuns(inline.ContentStart, inline.ContentEnd, formattingProperty);
					return;
				}
			}
			else
			{
				Span span;
				if (commonAncestor is Span && start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && end.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd && start.GetAdjacentElement(LogicalDirection.Backward) == commonAncestor)
				{
					span = (Span)commonAncestor;
				}
				else
				{
					span = new Span();
					span.Reposition(start, end);
				}
				span.SetValue(formattingProperty, value);
				TextRangeEdit.FixupStructuralPropertyEnvironment(span, formattingProperty);
				TextRangeEdit.ClearPropertyValueFromSpansAndRuns(span.ContentStart, span.ContentEnd, formattingProperty);
			}
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0010EB2C File Offset: 0x0010CD2C
		private static void ApplyStructuralInlinePropertyAcrossParagraphs(TextPointer start, TextPointer end, DependencyProperty formattingProperty, object value)
		{
			Invariant.Assert(start.Paragraph != null);
			Invariant.Assert(start.Paragraph.ContentEnd.CompareTo(end) < 0);
			TextRangeEdit.SetStructuralInlineProperty(start, start.Paragraph.ContentEnd, formattingProperty, value);
			start = start.Paragraph.ElementEnd;
			if (end.Paragraph != null)
			{
				TextRangeEdit.SetStructuralInlineProperty(end.Paragraph.ContentStart, end, formattingProperty, value);
				end = end.Paragraph.ElementStart;
			}
			while (start != null && start.CompareTo(end) < 0)
			{
				if (start.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && start.Parent is Paragraph)
				{
					Paragraph paragraph = (Paragraph)start.Parent;
					TextRangeEdit.SetStructuralInlineProperty(paragraph.ContentStart, paragraph.ContentEnd, formattingProperty, value);
					start = paragraph.ElementEnd;
				}
				start = start.GetNextContextPosition(LogicalDirection.Forward);
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0010EC00 File Offset: 0x0010CE00
		private static bool ValidateApplyStructuralInlineProperty(TextPointer start, TextPointer end, DependencyObject commonAncestor, DependencyProperty property)
		{
			if (!(commonAncestor is Inline))
			{
				return true;
			}
			Inline inline = null;
			Inline inline2;
			for (inline2 = (Inline)start.Parent; inline2 != commonAncestor; inline2 = (Inline)inline2.Parent)
			{
				if (!TextSchema.IsMergeableInline(inline2.GetType()))
				{
					inline = inline2;
					commonAncestor = inline2;
					break;
				}
			}
			for (inline2 = (Inline)end.Parent; inline2 != commonAncestor; inline2 = (Inline)inline2.Parent)
			{
				if (!TextSchema.IsMergeableInline(inline2.GetType()))
				{
					inline = inline2;
					break;
				}
			}
			if (property != null && inline2 != commonAncestor)
			{
				throw new InvalidOperationException(SR.Get("TextRangeEdit_InvalidStructuralPropertyApply", new object[]
				{
					property,
					inline
				}));
			}
			return inline2 == commonAncestor;
		}

		// Token: 0x0200090A RID: 2314
		internal static class DoublePropertyBounds
		{
			// Token: 0x060085D9 RID: 34265 RVA: 0x0024B29C File Offset: 0x0024949C
			internal static double GetClosestValidValue(DependencyProperty property, double value)
			{
				return TextRangeEdit.DoublePropertyBounds.GetValueRange(property).GetClosestValue(value);
			}

			// Token: 0x060085DA RID: 34266 RVA: 0x0024B2B8 File Offset: 0x002494B8
			private static TextRangeEdit.DoublePropertyBounds.DoublePropertyRange GetValueRange(DependencyProperty property)
			{
				for (int i = 0; i < TextRangeEdit.DoublePropertyBounds._ranges.Length; i++)
				{
					if (property == TextRangeEdit.DoublePropertyBounds._ranges[i].Property)
					{
						return TextRangeEdit.DoublePropertyBounds._ranges[i];
					}
				}
				return TextRangeEdit.DoublePropertyBounds.DefaultRange;
			}

			// Token: 0x17001E38 RID: 7736
			// (get) Token: 0x060085DB RID: 34267 RVA: 0x0024B2FB File Offset: 0x002494FB
			private static TextRangeEdit.DoublePropertyBounds.DoublePropertyRange DefaultRange
			{
				get
				{
					return TextRangeEdit.DoublePropertyBounds._ranges[0];
				}
			}

			// Token: 0x04004315 RID: 17173
			private static readonly TextRangeEdit.DoublePropertyBounds.DoublePropertyRange[] _ranges = new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange[]
			{
				new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange(null, 0.0, double.MaxValue),
				new TextRangeEdit.DoublePropertyBounds.DoublePropertyRange(Paragraph.TextIndentProperty, (double)(-(double)Math.Min(1000000, 3500000)), (double)Math.Min(1000000, 3500000))
			};

			// Token: 0x02000BAB RID: 2987
			private struct DoublePropertyRange
			{
				// Token: 0x060091D6 RID: 37334 RVA: 0x0025F449 File Offset: 0x0025D649
				internal DoublePropertyRange(DependencyProperty property, double lowerBound, double upperBound)
				{
					Invariant.Assert(lowerBound < upperBound);
					this._lowerBound = lowerBound;
					this._upperBound = upperBound;
					this._property = property;
				}

				// Token: 0x060091D7 RID: 37335 RVA: 0x0025F46C File Offset: 0x0025D66C
				internal double GetClosestValue(double value)
				{
					double val = Math.Max(this._lowerBound, value);
					return Math.Min(val, this._upperBound);
				}

				// Token: 0x17001FD3 RID: 8147
				// (get) Token: 0x060091D8 RID: 37336 RVA: 0x0025F494 File Offset: 0x0025D694
				internal DependencyProperty Property
				{
					get
					{
						return this._property;
					}
				}

				// Token: 0x04004ED9 RID: 20185
				private DependencyProperty _property;

				// Token: 0x04004EDA RID: 20186
				private double _lowerBound;

				// Token: 0x04004EDB RID: 20187
				private double _upperBound;
			}
		}
	}
}
