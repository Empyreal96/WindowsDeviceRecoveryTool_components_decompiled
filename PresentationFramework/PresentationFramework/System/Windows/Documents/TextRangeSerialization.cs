using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000410 RID: 1040
	internal static class TextRangeSerialization
	{
		// Token: 0x06003BBC RID: 15292 RVA: 0x00111F67 File Offset: 0x00110167
		internal static void WriteXaml(XmlWriter xmlWriter, ITextRange range, bool useFlowDocumentAsRoot, WpfPayload wpfPayload)
		{
			TextRangeSerialization.WriteXaml(xmlWriter, range, useFlowDocumentAsRoot, wpfPayload, false);
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x00111F74 File Offset: 0x00110174
		internal static void WriteXaml(XmlWriter xmlWriter, ITextRange range, bool useFlowDocumentAsRoot, WpfPayload wpfPayload, bool preserveTextElements)
		{
			Formatting formatting = Formatting.None;
			if (xmlWriter is XmlTextWriter)
			{
				formatting = ((XmlTextWriter)xmlWriter).Formatting;
				((XmlTextWriter)xmlWriter).Formatting = Formatting.None;
			}
			XamlTypeMapper defaultMapper = XmlParserDefaults.DefaultMapper;
			ITextPointer textPointer = TextRangeSerialization.FindSerializationCommonAncestor(range);
			bool lastParagraphMustBeMerged = !TextPointerBase.IsAfterLastParagraph(range.End) && range.End.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart;
			TextRangeSerialization.WriteRootFlowDocument(range, textPointer, xmlWriter, defaultMapper, lastParagraphMustBeMerged, useFlowDocumentAsRoot);
			List<int> ignoreList = new List<int>();
			bool ignoreWriteHyperlinkEnd;
			int num = 1 + TextRangeSerialization.WriteOpeningTags(range, range.Start, textPointer, xmlWriter, defaultMapper, wpfPayload == null, out ignoreWriteHyperlinkEnd, ref ignoreList, preserveTextElements);
			if (range.IsTableCellRange)
			{
				TextRangeSerialization.WriteXamlTableCellRange(xmlWriter, range, defaultMapper, ref num, wpfPayload, preserveTextElements);
			}
			else
			{
				TextRangeSerialization.WriteXamlTextSegment(xmlWriter, range.Start, range.End, defaultMapper, ref num, wpfPayload, ignoreWriteHyperlinkEnd, ignoreList, preserveTextElements);
			}
			Invariant.Assert(num >= 0, "elementLevel cannot be negative");
			while (num-- > 0)
			{
				xmlWriter.WriteFullEndElement();
			}
			if (xmlWriter is XmlTextWriter)
			{
				((XmlTextWriter)xmlWriter).Formatting = formatting;
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x00112070 File Offset: 0x00110270
		internal static void PasteXml(TextRange range, TextElement fragment)
		{
			Invariant.Assert(fragment != null);
			if (TextRangeSerialization.PasteSingleEmbeddedElement(range, fragment))
			{
				return;
			}
			TextRangeSerialization.AdjustFragmentForTargetRange(fragment, range);
			if (!range.IsEmpty)
			{
				range.Text = string.Empty;
			}
			Invariant.Assert(range.IsEmpty, "range must be empty in the beginning of pasting");
			if (((ITextPointer)fragment.ContentStart).CompareTo(fragment.ContentEnd) == 0)
			{
				return;
			}
			TextRangeSerialization.PasteTextFragment(fragment, range);
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x001120D8 File Offset: 0x001102D8
		private static void WriteXamlTextSegment(XmlWriter xmlWriter, ITextPointer rangeStart, ITextPointer rangeEnd, XamlTypeMapper xamlTypeMapper, ref int elementLevel, WpfPayload wpfPayload, bool ignoreWriteHyperlinkEnd, List<int> ignoreList, bool preserveTextElements)
		{
			if (elementLevel == 1 && typeof(Run).IsAssignableFrom(rangeStart.ParentType))
			{
				elementLevel++;
				xmlWriter.WriteStartElement(typeof(Run).Name);
			}
			ITextPointer textPointer = rangeStart.CreatePointer();
			while (rangeEnd.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				rangeEnd = rangeEnd.GetNextContextPosition(LogicalDirection.Backward);
			}
			while (textPointer.CompareTo(rangeEnd) < 0)
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.Text:
				{
					int num = textPointer.GetTextRunLength(LogicalDirection.Forward);
					char[] array = new char[num];
					num = TextPointerBase.GetTextWithLimit(textPointer, LogicalDirection.Forward, array, 0, num, rangeEnd);
					num = TextRangeSerialization.StripInvalidSurrogateChars(array, num);
					xmlWriter.WriteChars(array, 0, num);
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				}
				case TextPointerContext.EmbeddedElement:
				{
					object adjacentElement = textPointer.GetAdjacentElement(LogicalDirection.Forward);
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					TextRangeSerialization.WriteEmbeddedObject(adjacentElement, xmlWriter, wpfPayload);
					break;
				}
				case TextPointerContext.ElementStart:
				{
					TextElement textElement = (TextElement)textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if (textElement is Hyperlink)
					{
						if (TextRangeSerialization.IsHyperlinkInvalid(textPointer, rangeEnd))
						{
							ignoreWriteHyperlinkEnd = true;
							textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
							break;
						}
					}
					else if (textElement != null)
					{
						TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(textElement.GetType(), typeof(TextElementEditingBehaviorAttribute));
						if (textElementEditingBehaviorAttribute != null && !textElementEditingBehaviorAttribute.IsTypographicOnly && TextRangeSerialization.IsPartialNonTypographic(textPointer, rangeEnd))
						{
							ITextPointer textPointer2 = textPointer.CreatePointer();
							textPointer2.MoveToElementEdge(ElementEdge.BeforeEnd);
							ignoreList.Add(textPointer2.Offset);
							textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
							break;
						}
					}
					elementLevel++;
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					TextRangeSerialization.WriteStartXamlElement(null, textPointer, xmlWriter, xamlTypeMapper, wpfPayload == null, preserveTextElements);
					break;
				}
				case TextPointerContext.ElementEnd:
					if (ignoreWriteHyperlinkEnd && textPointer.GetAdjacentElement(LogicalDirection.Forward) is Hyperlink)
					{
						ignoreWriteHyperlinkEnd = false;
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					else
					{
						if (ignoreList.Count > 0)
						{
							ITextPointer textPointer3 = textPointer.CreatePointer();
							textPointer3.MoveToElementEdge(ElementEdge.BeforeEnd);
							if (ignoreList.Contains(textPointer3.Offset))
							{
								ignoreList.Remove(textPointer3.Offset);
								textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
								break;
							}
						}
						elementLevel--;
						if (TextSchema.IsBreak(textPointer.ParentType))
						{
							xmlWriter.WriteEndElement();
						}
						else
						{
							xmlWriter.WriteFullEndElement();
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					break;
				default:
					Invariant.Assert(false, "unexpected value of runType");
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				}
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00112324 File Offset: 0x00110524
		private static void WriteXamlTableCellRange(XmlWriter xmlWriter, ITextRange range, XamlTypeMapper xamlTypeMapper, ref int elementLevel, WpfPayload wpfPayload, bool preserveTextElements)
		{
			Invariant.Assert(range.IsTableCellRange, "range is expected to be in IsTableCellRange state");
			List<TextSegment> textSegments = range.TextSegments;
			int num = -1;
			bool ignoreWriteHyperlinkEnd = false;
			List<int> ignoreList = new List<int>();
			for (int i = 0; i < textSegments.Count; i++)
			{
				TextSegment textSegment = textSegments[i];
				if (i > 0)
				{
					ITextPointer textPointer = textSegment.Start.CreatePointer();
					while (!typeof(TableRow).IsAssignableFrom(textPointer.ParentType))
					{
						Invariant.Assert(typeof(TextElement).IsAssignableFrom(textPointer.ParentType), "pointer must be still in a scope of TextElement");
						textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
					}
					Invariant.Assert(typeof(TableRow).IsAssignableFrom(textPointer.ParentType), "pointer must be in a scope of TableRow");
					textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
					ITextRange range2 = new TextRange(textSegment.Start, textSegment.End);
					elementLevel += TextRangeSerialization.WriteOpeningTags(range2, textSegment.Start, textPointer, xmlWriter, xamlTypeMapper, wpfPayload == null, out ignoreWriteHyperlinkEnd, ref ignoreList, preserveTextElements);
				}
				TextRangeSerialization.WriteXamlTextSegment(xmlWriter, textSegment.Start, textSegment.End, xamlTypeMapper, ref elementLevel, wpfPayload, ignoreWriteHyperlinkEnd, ignoreList, preserveTextElements);
				Invariant.Assert(elementLevel >= 4, "At the minimun we expected to stay within four elements: Section(wrapper),Table,TableRowGroup,TableRow");
				if (num < 0)
				{
					num = elementLevel;
				}
				Invariant.Assert(num == elementLevel, "elementLevel is supposed to be unchanged between segments of table cell range");
				elementLevel--;
				xmlWriter.WriteFullEndElement();
			}
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x00112480 File Offset: 0x00110680
		private static int WriteOpeningTags(ITextRange range, ITextPointer thisElement, ITextPointer scope, XmlWriter xmlWriter, XamlTypeMapper xamlTypeMapper, bool reduceElement, out bool ignoreWriteHyperlinkEnd, ref List<int> ignoreList, bool preserveTextElements)
		{
			ignoreWriteHyperlinkEnd = false;
			if (thisElement.HasEqualScope(scope))
			{
				return 0;
			}
			Invariant.Assert(typeof(TextElement).IsAssignableFrom(thisElement.ParentType), "thisElement is expected to be a TextElement");
			ITextPointer textPointer = thisElement.CreatePointer();
			textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			int num = TextRangeSerialization.WriteOpeningTags(range, textPointer, scope, xmlWriter, xamlTypeMapper, reduceElement, out ignoreWriteHyperlinkEnd, ref ignoreList, preserveTextElements);
			bool flag = false;
			bool flag2 = false;
			if (thisElement.ParentType == typeof(Hyperlink))
			{
				if (TextPointerBase.IsAtNonMergeableInlineStart(range.Start))
				{
					ITextPointer textPointer2 = thisElement.CreatePointer();
					textPointer2.MoveToElementEdge(ElementEdge.BeforeStart);
					flag = TextRangeSerialization.IsHyperlinkInvalid(textPointer2, range.End);
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(thisElement.ParentType, typeof(TextElementEditingBehaviorAttribute));
				if (textElementEditingBehaviorAttribute != null && !textElementEditingBehaviorAttribute.IsTypographicOnly)
				{
					if (TextPointerBase.IsAtNonMergeableInlineStart(range.Start))
					{
						ITextPointer textPointer3 = thisElement.CreatePointer();
						textPointer3.MoveToElementEdge(ElementEdge.BeforeStart);
						flag2 = TextRangeSerialization.IsPartialNonTypographic(textPointer3, range.End);
					}
					else
					{
						flag2 = true;
					}
				}
			}
			int result;
			if (flag)
			{
				ignoreWriteHyperlinkEnd = true;
				result = num;
			}
			else if (flag2)
			{
				ITextPointer textPointer4 = thisElement.CreatePointer();
				textPointer4.MoveToElementEdge(ElementEdge.BeforeEnd);
				ignoreList.Add(textPointer4.Offset);
				result = num;
			}
			else
			{
				TextRangeSerialization.WriteStartXamlElement(range, thisElement, xmlWriter, xamlTypeMapper, reduceElement, preserveTextElements);
				result = num + 1;
			}
			return result;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001125CC File Offset: 0x001107CC
		private static void WriteStartXamlElement(ITextRange range, ITextPointer textReader, XmlWriter xmlWriter, XamlTypeMapper xamlTypeMapper, bool reduceElement, bool preserveTextElements)
		{
			Type parentType = textReader.ParentType;
			Type type = TextSchema.GetStandardElementType(parentType, reduceElement);
			if (type == typeof(InlineUIContainer) || type == typeof(BlockUIContainer))
			{
				Invariant.Assert(!reduceElement);
				InlineUIContainer inlineUIContainer = textReader.GetAdjacentElement(LogicalDirection.Backward) as InlineUIContainer;
				BlockUIContainer blockUIContainer = textReader.GetAdjacentElement(LogicalDirection.Backward) as BlockUIContainer;
				if ((inlineUIContainer == null || !(inlineUIContainer.Child is Image)) && (blockUIContainer == null || !(blockUIContainer.Child is Image)))
				{
					type = TextSchema.GetStandardElementType(parentType, true);
				}
			}
			else if (preserveTextElements)
			{
				type = parentType;
			}
			bool flag = preserveTextElements && !TextSchema.IsKnownType(parentType);
			if (flag)
			{
				int num = type.Module.Name.LastIndexOf('.');
				string str = (num == -1) ? type.Module.Name : type.Module.Name.Substring(0, num);
				string ns = "clr-namespace:" + type.Namespace + ";assembly=" + str;
				string @namespace = type.Namespace;
				xmlWriter.WriteStartElement(@namespace, type.Name, ns);
			}
			else
			{
				xmlWriter.WriteStartElement(type.Name);
			}
			DependencyObject complexProperties = new DependencyObject();
			TextRangeSerialization.WriteInheritableProperties(type, textReader, xmlWriter, true, complexProperties);
			TextRangeSerialization.WriteNoninheritableProperties(type, textReader, xmlWriter, true, complexProperties);
			if (flag)
			{
				TextRangeSerialization.WriteLocallySetProperties(type, textReader, xmlWriter, complexProperties);
			}
			TextRangeSerialization.WriteComplexProperties(xmlWriter, complexProperties, type);
			if (type == typeof(Table) && textReader is TextPointer)
			{
				TextRangeSerialization.WriteTableColumnsInformation(range, (Table)((TextPointer)textReader).Parent, xmlWriter, xamlTypeMapper);
			}
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x00112758 File Offset: 0x00110958
		private static void WriteTableColumnsInformation(ITextRange range, Table table, XmlWriter xmlWriter, XamlTypeMapper xamlTypeMapper)
		{
			TableColumnCollection columns = table.Columns;
			int num;
			int num2;
			if (!TextRangeEditTables.GetColumnRange(range, table, out num, out num2))
			{
				num = 0;
				num2 = columns.Count - 1;
			}
			Invariant.Assert(num >= 0, "startColumn index is supposed to be non-negative");
			if (columns.Count > 0)
			{
				string localName = table.GetType().Name + ".Columns";
				xmlWriter.WriteStartElement(localName);
				int num3 = num;
				while (num3 <= num2 && num3 < columns.Count)
				{
					TextRangeSerialization.WriteXamlAtomicElement(columns[num3], xmlWriter, false);
					num3++;
				}
				xmlWriter.WriteEndElement();
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001127EC File Offset: 0x001109EC
		private static void WriteRootFlowDocument(ITextRange range, ITextPointer context, XmlWriter xmlWriter, XamlTypeMapper xamlTypeMapper, bool lastParagraphMustBeMerged, bool useFlowDocumentAsRoot)
		{
			Type typeFromHandle;
			if (useFlowDocumentAsRoot)
			{
				typeFromHandle = typeof(FlowDocument);
			}
			else
			{
				Type parentType = context.ParentType;
				if (parentType == null || typeof(Paragraph).IsAssignableFrom(parentType) || (typeof(Inline).IsAssignableFrom(parentType) && !typeof(AnchoredBlock).IsAssignableFrom(parentType)))
				{
					typeFromHandle = typeof(Span);
				}
				else
				{
					typeFromHandle = typeof(Section);
				}
			}
			xmlWriter.WriteStartElement(typeFromHandle.Name, "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			xmlWriter.WriteAttributeString("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
			xmlWriter.WriteAttributeString("xml:space", "preserve");
			DependencyObject complexProperties = new DependencyObject();
			if (useFlowDocumentAsRoot)
			{
				TextRangeSerialization.WriteInheritablePropertiesForFlowDocument(((TextPointer)context).Parent, xmlWriter, complexProperties);
			}
			else
			{
				TextRangeSerialization.WriteInheritableProperties(typeFromHandle, context, xmlWriter, false, complexProperties);
			}
			if (typeFromHandle == typeof(Span))
			{
				TextRangeSerialization.WriteNoninheritableProperties(typeof(Span), context, xmlWriter, false, complexProperties);
			}
			if (typeFromHandle == typeof(Section) && lastParagraphMustBeMerged)
			{
				xmlWriter.WriteAttributeString("HasTrailingParagraphBreakOnPaste", "False");
			}
			TextRangeSerialization.WriteComplexProperties(xmlWriter, complexProperties, typeFromHandle);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00112914 File Offset: 0x00110B14
		private static void WriteInheritablePropertiesForFlowDocument(DependencyObject context, XmlWriter xmlWriter, DependencyObject complexProperties)
		{
			foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(typeof(FlowDocument)))
			{
				object obj = context.ReadLocalValue(dependencyProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					string text = DPTypeDescriptorContext.GetStringValue(dependencyProperty, obj);
					if (text != null)
					{
						text = TextRangeSerialization.FilterNaNStringValueForDoublePropertyType(text, dependencyProperty.PropertyType);
						string localName;
						if (dependencyProperty == FrameworkContentElement.LanguageProperty)
						{
							localName = "xml:lang";
						}
						else
						{
							localName = ((dependencyProperty.OwnerType == typeof(Typography)) ? ("Typography." + dependencyProperty.Name) : dependencyProperty.Name);
						}
						xmlWriter.WriteAttributeString(localName, text);
					}
					else
					{
						complexProperties.SetValue(dependencyProperty, obj);
					}
				}
			}
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001129CC File Offset: 0x00110BCC
		private static void WriteInheritableProperties(Type elementTypeStandardized, ITextPointer context, XmlWriter xmlWriter, bool onlyAffected, DependencyObject complexProperties)
		{
			ITextPointer textPointer = null;
			if (onlyAffected)
			{
				textPointer = context.CreatePointer();
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(elementTypeStandardized))
			{
				object value = context.GetValue(dependencyProperty);
				if (value != null)
				{
					object value2 = null;
					if (onlyAffected)
					{
						value2 = textPointer.GetValue(dependencyProperty);
					}
					if (!onlyAffected || !TextSchema.ValuesAreEqual(value, value2))
					{
						string text = DPTypeDescriptorContext.GetStringValue(dependencyProperty, value);
						if (text != null)
						{
							text = TextRangeSerialization.FilterNaNStringValueForDoublePropertyType(text, dependencyProperty.PropertyType);
							string localName;
							if (dependencyProperty == FrameworkContentElement.LanguageProperty)
							{
								localName = "xml:lang";
							}
							else
							{
								localName = TextRangeSerialization.GetPropertyNameForElement(dependencyProperty, elementTypeStandardized, false);
							}
							xmlWriter.WriteAttributeString(localName, text);
						}
						else
						{
							complexProperties.SetValue(dependencyProperty, value);
						}
					}
				}
			}
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x00112A84 File Offset: 0x00110C84
		private static void WriteNoninheritableProperties(Type elementTypeStandardized, ITextPointer context, XmlWriter xmlWriter, bool onlyAffected, DependencyObject complexProperties)
		{
			DependencyProperty[] noninheritableProperties = TextSchema.GetNoninheritableProperties(elementTypeStandardized);
			ITextPointer textPointer = onlyAffected ? null : context.CreatePointer();
			int i = 0;
			while (i < noninheritableProperties.Length)
			{
				DependencyProperty dependencyProperty = noninheritableProperties[i];
				Type parentType = context.ParentType;
				object value;
				if (onlyAffected)
				{
					value = context.GetValue(dependencyProperty);
					goto IL_D7;
				}
				Invariant.Assert(elementTypeStandardized == typeof(Span), "Request for contextual properties is expected for Span wrapper only");
				value = context.GetValue(dependencyProperty);
				if (value != null && !TextDecorationCollection.Empty.ValueEquals(value as TextDecorationCollection))
				{
					goto IL_D7;
				}
				if (dependencyProperty != Inline.BaselineAlignmentProperty && dependencyProperty != TextElement.TextEffectsProperty)
				{
					textPointer.MoveToPosition(context);
					while ((value == null || TextDecorationCollection.Empty.ValueEquals(value as TextDecorationCollection)) && typeof(Inline).IsAssignableFrom(textPointer.ParentType))
					{
						textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
						value = textPointer.GetValue(dependencyProperty);
						parentType = textPointer.ParentType;
					}
					goto IL_D7;
				}
				IL_147:
				i++;
				continue;
				IL_D7:
				if ((dependencyProperty == Block.MarginProperty && (typeof(Paragraph).IsAssignableFrom(parentType) || typeof(List).IsAssignableFrom(parentType))) || (dependencyProperty == Block.PaddingProperty && typeof(List).IsAssignableFrom(parentType)))
				{
					Thickness margin = (Thickness)value;
					if (Paragraph.IsMarginAuto(margin))
					{
						goto IL_147;
					}
				}
				TextRangeSerialization.WriteNoninheritableProperty(xmlWriter, dependencyProperty, value, parentType, onlyAffected, complexProperties, context.ReadLocalValue(dependencyProperty));
				goto IL_147;
			}
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x00112BE8 File Offset: 0x00110DE8
		private static void WriteNoninheritableProperty(XmlWriter xmlWriter, DependencyProperty property, object propertyValue, Type propertyOwnerType, bool onlyAffected, DependencyObject complexProperties, object localValue)
		{
			bool flag = false;
			if (propertyValue != null && propertyValue != DependencyProperty.UnsetValue)
			{
				if (!onlyAffected)
				{
					flag = true;
				}
				else
				{
					PropertyMetadata metadata = property.GetMetadata(propertyOwnerType);
					flag = (metadata == null || !TextSchema.ValuesAreEqual(propertyValue, metadata.DefaultValue) || localValue != DependencyProperty.UnsetValue);
				}
			}
			if (flag)
			{
				string text = DPTypeDescriptorContext.GetStringValue(property, propertyValue);
				if (text != null)
				{
					text = TextRangeSerialization.FilterNaNStringValueForDoublePropertyType(text, property.PropertyType);
					xmlWriter.WriteAttributeString(property.Name, text);
					return;
				}
				complexProperties.SetValue(property, propertyValue);
			}
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00112C68 File Offset: 0x00110E68
		private static void WriteLocallySetProperties(Type elementTypeStandardized, ITextPointer context, XmlWriter xmlWriter, DependencyObject complexProperties)
		{
			if (!(context is TextPointer))
			{
				return;
			}
			LocalValueEnumerator localValueEnumerator = context.GetLocalValueEnumerator();
			DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(elementTypeStandardized);
			DependencyProperty[] noninheritableProperties = TextSchema.GetNoninheritableProperties(elementTypeStandardized);
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (!property.ReadOnly && !TextRangeSerialization.IsPropertyKnown(property, inheritableProperties, noninheritableProperties) && !TextSchema.IsKnownType(property.OwnerType))
				{
					object obj = context.ReadLocalValue(property);
					string text = DPTypeDescriptorContext.GetStringValue(property, obj);
					if (text != null)
					{
						text = TextRangeSerialization.FilterNaNStringValueForDoublePropertyType(text, property.PropertyType);
						string propertyNameForElement = TextRangeSerialization.GetPropertyNameForElement(property, elementTypeStandardized, false);
						xmlWriter.WriteAttributeString(propertyNameForElement, text);
					}
					else
					{
						complexProperties.SetValue(property, obj);
					}
				}
			}
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00112D24 File Offset: 0x00110F24
		private static bool IsPropertyKnown(DependencyProperty propertyToTest, DependencyProperty[] inheritableProperties, DependencyProperty[] nonInheritableProperties)
		{
			foreach (DependencyProperty dependencyProperty in inheritableProperties)
			{
				if (dependencyProperty == propertyToTest)
				{
					return true;
				}
			}
			foreach (DependencyProperty dependencyProperty2 in nonInheritableProperties)
			{
				if (dependencyProperty2 == propertyToTest)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x00112D64 File Offset: 0x00110F64
		private static void WriteComplexProperties(XmlWriter xmlWriter, DependencyObject complexProperties, Type elementType)
		{
			if (!SecurityHelper.CheckUnmanagedCodePermission())
			{
				return;
			}
			LocalValueEnumerator localValueEnumerator = complexProperties.GetLocalValueEnumerator();
			localValueEnumerator.Reset();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				string propertyNameForElement = TextRangeSerialization.GetPropertyNameForElement(localValueEntry.Property, elementType, true);
				xmlWriter.WriteStartElement(propertyNameForElement);
				string data = XamlWriter.Save(localValueEntry.Value);
				xmlWriter.WriteRaw(data);
				xmlWriter.WriteEndElement();
			}
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00112DCC File Offset: 0x00110FCC
		private static string GetPropertyNameForElement(DependencyProperty property, Type elementType, bool forceComplexName)
		{
			string result;
			if (DependencyProperty.FromName(property.Name, elementType) == property)
			{
				if (forceComplexName)
				{
					result = elementType.Name + "." + property.Name;
				}
				else
				{
					result = property.Name;
				}
			}
			else
			{
				result = property.OwnerType.Name + "." + property.Name;
			}
			return result;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00112E2C File Offset: 0x0011102C
		private static void WriteXamlAtomicElement(DependencyObject element, XmlWriter xmlWriter, bool reduceElement)
		{
			Type standardElementType = TextSchema.GetStandardElementType(element.GetType(), reduceElement);
			DependencyProperty[] noninheritableProperties = TextSchema.GetNoninheritableProperties(standardElementType);
			xmlWriter.WriteStartElement(standardElementType.Name);
			foreach (DependencyProperty dependencyProperty in noninheritableProperties)
			{
				object obj = element.ReadLocalValue(dependencyProperty);
				if (obj != null && obj != DependencyProperty.UnsetValue)
				{
					TypeConverter converter = TypeDescriptor.GetConverter(dependencyProperty.PropertyType);
					Invariant.Assert(converter != null, "typeConverter==null: is not expected for atomic elements");
					Invariant.Assert(converter.CanConvertTo(typeof(string)), "type is expected to be convertable into string type");
					string text = (string)converter.ConvertTo(null, CultureInfo.InvariantCulture, obj, typeof(string));
					Invariant.Assert(text != null, "expecting non-null stringValue");
					xmlWriter.WriteAttributeString(dependencyProperty.Name, text);
				}
			}
			xmlWriter.WriteEndElement();
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00112F04 File Offset: 0x00111104
		private static void WriteEmbeddedObject(object embeddedObject, XmlWriter xmlWriter, WpfPayload wpfPayload)
		{
			if (wpfPayload != null && embeddedObject is Image)
			{
				Image image = (Image)embeddedObject;
				if (image.Source != null && !string.IsNullOrEmpty(image.Source.ToString()))
				{
					string text = wpfPayload.AddImage(image);
					if (text != null)
					{
						Type typeFromHandle = typeof(Image);
						xmlWriter.WriteStartElement(typeFromHandle.Name);
						DependencyProperty[] imageProperties = TextSchema.ImageProperties;
						DependencyObject complexProperties = new DependencyObject();
						foreach (DependencyProperty dependencyProperty in imageProperties)
						{
							if (dependencyProperty != Image.SourceProperty)
							{
								object value = image.GetValue(dependencyProperty);
								TextRangeSerialization.WriteNoninheritableProperty(xmlWriter, dependencyProperty, value, typeFromHandle, true, complexProperties, image.ReadLocalValue(dependencyProperty));
							}
						}
						xmlWriter.WriteStartElement(typeof(Image).Name + "." + Image.SourceProperty.Name);
						xmlWriter.WriteStartElement(typeof(BitmapImage).Name);
						xmlWriter.WriteAttributeString(BitmapImage.UriSourceProperty.Name, text);
						xmlWriter.WriteAttributeString(BitmapImage.CacheOptionProperty.Name, "OnLoad");
						xmlWriter.WriteEndElement();
						xmlWriter.WriteEndElement();
						TextRangeSerialization.WriteComplexProperties(xmlWriter, complexProperties, typeFromHandle);
						xmlWriter.WriteEndElement();
						return;
					}
				}
			}
			else
			{
				xmlWriter.WriteString(" ");
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00113048 File Offset: 0x00111248
		private static bool PasteSingleEmbeddedElement(TextRange range, TextElement fragment)
		{
			if (fragment.ContentStart.GetOffsetToPosition(fragment.ContentEnd) == 3)
			{
				TextElement textElement = fragment.ContentStart.GetAdjacentElement(LogicalDirection.Forward) as TextElement;
				FrameworkElement frameworkElement = null;
				if (textElement is BlockUIContainer)
				{
					frameworkElement = (((BlockUIContainer)textElement).Child as FrameworkElement);
					if (frameworkElement != null)
					{
						((BlockUIContainer)textElement).Child = null;
					}
				}
				else if (textElement is InlineUIContainer)
				{
					frameworkElement = (((InlineUIContainer)textElement).Child as FrameworkElement);
					if (frameworkElement != null)
					{
						((InlineUIContainer)textElement).Child = null;
					}
				}
				if (frameworkElement != null)
				{
					range.InsertEmbeddedUIElement(frameworkElement);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x001130DC File Offset: 0x001112DC
		private static void PasteTextFragment(TextElement fragment, TextRange range)
		{
			Invariant.Assert(range.IsEmpty, "range must be empty at this point - emptied by a caller");
			Invariant.Assert(fragment is Section || fragment is Span, "The wrapper element must be a Section or Span");
			TextPointer textPointer = TextRangeEditTables.EnsureInsertionPosition(range.End);
			if (textPointer.HasNonMergeableInlineAncestor)
			{
				TextRangeSerialization.PasteNonMergeableTextFragment(fragment, range);
				return;
			}
			TextRangeSerialization.PasteMergeableTextFragment(fragment, range, textPointer);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0011313C File Offset: 0x0011133C
		private static void PasteNonMergeableTextFragment(TextElement fragment, TextRange range)
		{
			string textInternal = TextRangeBase.GetTextInternal(fragment.ElementStart, fragment.ElementEnd);
			range.Text = textInternal;
			range.Select(range.Start, range.End);
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00113174 File Offset: 0x00111374
		private static void PasteMergeableTextFragment(TextElement fragment, TextRange range, TextPointer insertionPosition)
		{
			TextPointer elementStart;
			TextPointer textPointer;
			if (fragment is Span)
			{
				insertionPosition = TextRangeEdit.SplitFormattingElements(insertionPosition, false);
				Invariant.Assert(insertionPosition.Parent is Paragraph, "insertionPosition must be in a scope of a Paragraph after splitting formatting elements");
				fragment.RepositionWithContent(insertionPosition);
				elementStart = fragment.ElementStart;
				textPointer = fragment.ElementEnd;
				fragment.Reposition(null, null);
				TextRangeSerialization.ValidateMergingPositions(typeof(Inline), elementStart, textPointer);
				TextRangeSerialization.ApplyContextualProperties(elementStart, textPointer, fragment);
			}
			else
			{
				TextRangeSerialization.CorrectLeadingNestedLists((Section)fragment);
				bool flag = TextRangeSerialization.SplitParagraphForPasting(ref insertionPosition);
				fragment.RepositionWithContent(insertionPosition);
				elementStart = fragment.ElementStart;
				textPointer = fragment.ElementEnd.GetPositionAtOffset(0, LogicalDirection.Forward);
				fragment.Reposition(null, null);
				TextRangeSerialization.ValidateMergingPositions(typeof(Block), elementStart, textPointer);
				TextRangeSerialization.ApplyContextualProperties(elementStart, textPointer, fragment);
				if (flag)
				{
					TextRangeSerialization.MergeParagraphsAtPosition(elementStart, true);
				}
				if (!((Section)fragment).HasTrailingParagraphBreakOnPaste)
				{
					TextRangeSerialization.MergeParagraphsAtPosition(textPointer, false);
				}
			}
			if (fragment is Section && ((Section)fragment).HasTrailingParagraphBreakOnPaste)
			{
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			}
			range.Select(elementStart, textPointer);
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00113278 File Offset: 0x00111478
		private static void CorrectLeadingNestedLists(Section fragment)
		{
			List list2;
			for (List list = fragment.Blocks.FirstBlock as List; list != null; list = list2)
			{
				ListItem firstListItem = list.ListItems.FirstListItem;
				if (firstListItem == null)
				{
					return;
				}
				if (firstListItem.NextListItem != null)
				{
					return;
				}
				list2 = (firstListItem.Blocks.FirstBlock as List);
				if (list2 == null)
				{
					return;
				}
				firstListItem.Reposition(null, null);
				list.Reposition(null, null);
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x001132DC File Offset: 0x001114DC
		private static bool SplitParagraphForPasting(ref TextPointer insertionPosition)
		{
			bool flag = true;
			TextPointer textPointer = insertionPosition;
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				if (!TextSchema.IsFormattingType(textPointer.Parent.GetType()))
				{
					break;
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Backward);
			}
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && TextSchema.AllowsParagraphMerging(textPointer.Parent.GetType()))
			{
				flag = false;
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Backward);
			}
			if (!flag)
			{
				insertionPosition = textPointer;
			}
			else
			{
				insertionPosition = TextRangeEdit.InsertParagraphBreak(insertionPosition, false);
			}
			if (insertionPosition.Parent is List)
			{
				insertionPosition = TextRangeEdit.SplitElement(insertionPosition);
			}
			return flag;
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00113368 File Offset: 0x00111568
		private static void MergeParagraphsAtPosition(TextPointer position, bool mergingOnFragmentStart)
		{
			TextPointer textPointer = position;
			while (textPointer != null && !(textPointer.Parent is Paragraph))
			{
				if (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementEnd)
				{
					textPointer = textPointer.GetNextContextPosition(LogicalDirection.Backward);
				}
				else
				{
					textPointer = null;
				}
			}
			if (textPointer != null)
			{
				Invariant.Assert(textPointer.Parent is Paragraph, "We suppose have a first paragraph found");
				Paragraph paragraph = (Paragraph)textPointer.Parent;
				textPointer = position;
				while (textPointer != null && !(textPointer.Parent is Paragraph))
				{
					if (textPointer.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
					{
						textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
					}
					else
					{
						textPointer = null;
					}
				}
				if (textPointer != null)
				{
					Invariant.Assert(textPointer.Parent is Paragraph, "We suppose a second paragraph found");
					Paragraph paragraph2 = (Paragraph)textPointer.Parent;
					if (TextRangeEditLists.ParagraphsAreMergeable(paragraph, paragraph2))
					{
						TextRangeEditLists.MergeParagraphs(paragraph, paragraph2);
						return;
					}
					if (mergingOnFragmentStart && paragraph.TextRange.IsEmpty)
					{
						paragraph.RepositionWithContent(null);
						return;
					}
					if (!mergingOnFragmentStart && paragraph2.TextRange.IsEmpty)
					{
						paragraph2.RepositionWithContent(null);
					}
				}
			}
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0011345C File Offset: 0x0011165C
		private static void ValidateMergingPositions(Type itemType, TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = start.GetPointerContext(LogicalDirection.Forward);
				TextPointerContext pointerContext2 = end.GetPointerContext(LogicalDirection.Backward);
				Invariant.Assert(pointerContext == TextPointerContext.ElementStart, "Expecting first opening tag of pasted fragment");
				Invariant.Assert(pointerContext2 == TextPointerContext.ElementEnd, "Expecting last closing tag of pasted fragment");
				Invariant.Assert(itemType.IsAssignableFrom(start.GetAdjacentElement(LogicalDirection.Forward).GetType()), "The first pasted fragment item is expected to be a " + itemType.Name);
				Invariant.Assert(itemType.IsAssignableFrom(end.GetAdjacentElement(LogicalDirection.Backward).GetType()), "The last pasted fragment item is expected to be a " + itemType.Name);
				TextPointerContext pointerContext3 = start.GetPointerContext(LogicalDirection.Backward);
				TextPointerContext pointerContext4 = end.GetPointerContext(LogicalDirection.Forward);
				Invariant.Assert(pointerContext3 == TextPointerContext.ElementStart || pointerContext3 == TextPointerContext.ElementEnd || pointerContext3 == TextPointerContext.None, "Bad context preceding a pasted fragment");
				Invariant.Assert(pointerContext3 != TextPointerContext.ElementEnd || itemType.IsAssignableFrom(start.GetAdjacentElement(LogicalDirection.Backward).GetType()), "An element preceding a pasted fragment is expected to be a " + itemType.Name);
				Invariant.Assert(pointerContext4 == TextPointerContext.ElementStart || pointerContext4 == TextPointerContext.ElementEnd || pointerContext4 == TextPointerContext.None, "Bad context following a pasted fragment");
				Invariant.Assert(pointerContext4 != TextPointerContext.ElementStart || itemType.IsAssignableFrom(end.GetAdjacentElement(LogicalDirection.Forward).GetType()), "An element following a pasted fragment is expected to be a " + itemType.Name);
			}
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0011358E File Offset: 0x0011178E
		private static void AdjustFragmentForTargetRange(TextElement fragment, TextRange range)
		{
			if (fragment is Section && ((Section)fragment).HasTrailingParagraphBreakOnPaste)
			{
				((Section)fragment).HasTrailingParagraphBreakOnPaste = (range.End.GetPointerContext(LogicalDirection.Forward) > TextPointerContext.None);
			}
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x001135C0 File Offset: 0x001117C0
		private static void ApplyContextualProperties(TextPointer start, TextPointer end, TextElement propertyBag)
		{
			Invariant.Assert(propertyBag.IsEmpty && propertyBag.Parent == null, "propertyBag is supposed to be an empty element outside any tree");
			LocalValueEnumerator localValueEnumerator = propertyBag.GetLocalValueEnumerator();
			while (start.CompareTo(end) < 0 && localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				DependencyProperty property = localValueEntry.Property;
				if (TextSchema.IsCharacterProperty(property) && TextSchema.IsParagraphProperty(property))
				{
					if (TextSchema.IsBlock(propertyBag.GetType()))
					{
						TextRangeSerialization.ApplyContextualProperty(typeof(Block), start, end, property, localValueEntry.Value);
					}
					else
					{
						TextRangeSerialization.ApplyContextualProperty(typeof(Inline), start, end, property, localValueEntry.Value);
					}
				}
				else if (TextSchema.IsCharacterProperty(property))
				{
					TextRangeSerialization.ApplyContextualProperty(typeof(Inline), start, end, property, localValueEntry.Value);
				}
				else if (TextSchema.IsParagraphProperty(property))
				{
					TextRangeSerialization.ApplyContextualProperty(typeof(Block), start, end, property, localValueEntry.Value);
				}
			}
			TextRangeEdit.MergeFormattingInlines(start);
			TextRangeEdit.MergeFormattingInlines(end);
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x001136C4 File Offset: 0x001118C4
		private static void ApplyContextualProperty(Type targetType, TextPointer start, TextPointer end, DependencyProperty property, object value)
		{
			if (TextSchema.ValuesAreEqual(start.Parent.GetValue(property), value))
			{
				return;
			}
			start = start.GetNextContextPosition(LogicalDirection.Forward);
			while (start != null && start.CompareTo(end) < 0)
			{
				TextPointerContext pointerContext = start.GetPointerContext(LogicalDirection.Backward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement textElement = (TextElement)start.Parent;
					if (textElement.ReadLocalValue(property) != DependencyProperty.UnsetValue || !TextSchema.ValuesAreEqual(textElement.GetValue(property), textElement.Parent.GetValue(property)))
					{
						start = textElement.ElementEnd;
					}
					else if (targetType.IsAssignableFrom(textElement.GetType()))
					{
						start = textElement.ElementEnd;
						if (targetType == typeof(Block) && start.CompareTo(end) > 0)
						{
							break;
						}
						if (!TextSchema.ValuesAreEqual(value, textElement.GetValue(property)))
						{
							textElement.ClearValue(property);
							if (!TextSchema.ValuesAreEqual(value, textElement.GetValue(property)))
							{
								textElement.SetValue(property, value);
							}
							TextRangeEdit.MergeFormattingInlines(textElement.ElementStart);
						}
					}
					else
					{
						start = start.GetNextContextPosition(LogicalDirection.Forward);
					}
				}
				else
				{
					Invariant.Assert(pointerContext > TextPointerContext.None, "TextPointerContext.None is not expected");
					start = start.GetNextContextPosition(LogicalDirection.Forward);
				}
			}
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x001137E8 File Offset: 0x001119E8
		private static ITextPointer FindSerializationCommonAncestor(ITextRange range)
		{
			ITextPointer textPointer = range.Start.CreatePointer();
			ITextPointer textPointer2 = range.End.CreatePointer();
			while (!textPointer.HasEqualScope(textPointer2))
			{
				textPointer2.MoveToPosition(range.End);
				while (typeof(TextElement).IsAssignableFrom(textPointer2.ParentType) && !textPointer2.HasEqualScope(textPointer))
				{
					textPointer2.MoveToElementEdge(ElementEdge.AfterEnd);
				}
				if (textPointer2.HasEqualScope(textPointer))
				{
					IL_71:
					while (!TextRangeSerialization.IsAcceptableAncestor(textPointer, range))
					{
						textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
					}
					if (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
					{
						textPointer.MoveToElementEdge(ElementEdge.AfterStart);
						ITextPointer hyperlinkStart = TextRangeSerialization.GetHyperlinkStart(range);
						if (hyperlinkStart != null)
						{
							textPointer = hyperlinkStart;
						}
					}
					else
					{
						textPointer.MoveToPosition(textPointer.TextContainer.Start);
					}
					return textPointer;
				}
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			goto IL_71;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x001138B0 File Offset: 0x00111AB0
		private static bool IsAcceptableAncestor(ITextPointer commonAncestor, ITextRange range)
		{
			if (typeof(TableRow).IsAssignableFrom(commonAncestor.ParentType) || typeof(TableRowGroup).IsAssignableFrom(commonAncestor.ParentType) || typeof(Table).IsAssignableFrom(commonAncestor.ParentType) || typeof(BlockUIContainer).IsAssignableFrom(commonAncestor.ParentType) || typeof(List).IsAssignableFrom(commonAncestor.ParentType) || (typeof(Inline).IsAssignableFrom(commonAncestor.ParentType) && TextSchema.HasTextDecorations(commonAncestor.GetValue(Inline.TextDecorationsProperty))))
			{
				return false;
			}
			ITextPointer textPointer = commonAncestor.CreatePointer();
			while (typeof(TextElement).IsAssignableFrom(textPointer.ParentType))
			{
				TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(textPointer.ParentType, typeof(TextElementEditingBehaviorAttribute));
				if (textElementEditingBehaviorAttribute != null && !textElementEditingBehaviorAttribute.IsTypographicOnly)
				{
					return false;
				}
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			return true;
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x001139B0 File Offset: 0x00111BB0
		private static int StripInvalidSurrogateChars(char[] text, int length)
		{
			Invariant.Assert(text.Length >= length, "Asserting that text.Length >= length");
			int i;
			for (i = 0; i < length; i++)
			{
				char c = text[i];
				if (char.IsHighSurrogate(c) || char.IsLowSurrogate(c) || TextRangeSerialization.IsBadCode(c))
				{
					break;
				}
			}
			int num;
			if (i == length)
			{
				num = length;
			}
			else
			{
				num = i;
				while (i < length)
				{
					if (char.IsHighSurrogate(text[i]))
					{
						if (i + 1 < length && char.IsLowSurrogate(text[i + 1]))
						{
							text[num] = text[i];
							text[num + 1] = text[i + 1];
							num += 2;
							i++;
						}
					}
					else if (!char.IsLowSurrogate(text[i]) && !TextRangeSerialization.IsBadCode(text[i]))
					{
						text[num] = text[i];
						num++;
					}
					i++;
				}
			}
			return num;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00113A61 File Offset: 0x00111C61
		private static bool IsBadCode(char code)
		{
			return code < ' ' && code != '\t' && code != '\n' && code != '\r';
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x00113A7C File Offset: 0x00111C7C
		private static bool IsPartialNonTypographic(ITextPointer textReader, ITextPointer rangeEnd)
		{
			bool result = false;
			Invariant.Assert(textReader.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart);
			ITextPointer textPointer = textReader.CreatePointer();
			ITextPointer textPointer2 = textReader.CreatePointer();
			textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
			textPointer2.MoveToElementEdge(ElementEdge.AfterEnd);
			if (textPointer2.CompareTo(rangeEnd) > 0)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00113AC4 File Offset: 0x00111CC4
		private static bool IsHyperlinkInvalid(ITextPointer textReader, ITextPointer rangeEnd)
		{
			Invariant.Assert(textReader.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart);
			Invariant.Assert(typeof(Hyperlink).IsAssignableFrom(textReader.GetElementType(LogicalDirection.Forward)));
			bool result = false;
			Hyperlink hyperlink = (Hyperlink)textReader.GetAdjacentElement(LogicalDirection.Forward);
			ITextPointer textPointer = textReader.CreatePointer();
			ITextPointer textPointer2 = textReader.CreatePointer();
			textPointer2.MoveToNextContextPosition(LogicalDirection.Forward);
			textPointer2.MoveToElementEdge(ElementEdge.AfterEnd);
			if (textPointer2.CompareTo(rangeEnd) > 0)
			{
				result = true;
			}
			else
			{
				while (textPointer.CompareTo(textPointer2) < 0)
				{
					InlineUIContainer inlineUIContainer = textPointer.GetAdjacentElement(LogicalDirection.Forward) as InlineUIContainer;
					if (inlineUIContainer != null && !(inlineUIContainer.Child is Image))
					{
						result = true;
						break;
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			return result;
		}

		// Token: 0x06003BE0 RID: 15328 RVA: 0x00113B6C File Offset: 0x00111D6C
		private static ITextPointer GetHyperlinkStart(ITextRange range)
		{
			ITextPointer textPointer = null;
			if (TextPointerBase.IsAtNonMergeableInlineStart(range.Start) && TextPointerBase.IsAtNonMergeableInlineEnd(range.End))
			{
				textPointer = range.Start.CreatePointer(LogicalDirection.Forward);
				while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && !typeof(Hyperlink).IsAssignableFrom(textPointer.ParentType))
				{
					textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				}
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
				textPointer.Freeze();
			}
			return textPointer;
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x00113BDA File Offset: 0x00111DDA
		private static string FilterNaNStringValueForDoublePropertyType(string stringValue, Type propertyType)
		{
			if (propertyType == typeof(double) && string.Compare(stringValue, "NaN", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return "Auto";
			}
			return stringValue;
		}

		// Token: 0x040025EB RID: 9707
		private const int EmptyDocumentDepth = 1;
	}
}
