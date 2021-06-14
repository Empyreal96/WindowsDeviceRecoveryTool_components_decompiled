using System;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000411 RID: 1041
	internal static class TextSchema
	{
		// Token: 0x06003BE2 RID: 15330 RVA: 0x00113C04 File Offset: 0x00111E04
		static TextSchema()
		{
			DependencyProperty[] array = new DependencyProperty[]
			{
				FrameworkElement.LanguageProperty,
				FrameworkElement.FlowDirectionProperty,
				NumberSubstitution.CultureSourceProperty,
				NumberSubstitution.SubstitutionProperty,
				NumberSubstitution.CultureOverrideProperty,
				TextElement.FontFamilyProperty,
				TextElement.FontStyleProperty,
				TextElement.FontWeightProperty,
				TextElement.FontStretchProperty,
				TextElement.FontSizeProperty,
				TextElement.ForegroundProperty
			};
			TextSchema._inheritableTextElementProperties = new DependencyProperty[array.Length + Typography.TypographyPropertiesList.Length];
			Array.Copy(array, 0, TextSchema._inheritableTextElementProperties, 0, array.Length);
			Array.Copy(Typography.TypographyPropertiesList, 0, TextSchema._inheritableTextElementProperties, array.Length, Typography.TypographyPropertiesList.Length);
			DependencyProperty[] array2 = new DependencyProperty[]
			{
				Block.TextAlignmentProperty,
				Block.LineHeightProperty,
				Block.IsHyphenationEnabledProperty
			};
			TextSchema._inheritableBlockProperties = new DependencyProperty[array2.Length + TextSchema._inheritableTextElementProperties.Length];
			Array.Copy(array2, 0, TextSchema._inheritableBlockProperties, 0, array2.Length);
			Array.Copy(TextSchema._inheritableTextElementProperties, 0, TextSchema._inheritableBlockProperties, array2.Length, TextSchema._inheritableTextElementProperties.Length);
			DependencyProperty[] array3 = new DependencyProperty[]
			{
				Block.TextAlignmentProperty
			};
			TextSchema._inheritableTableCellProperties = new DependencyProperty[array3.Length + TextSchema._inheritableTextElementProperties.Length];
			Array.Copy(array3, TextSchema._inheritableTableCellProperties, array3.Length);
			Array.Copy(TextSchema._inheritableTextElementProperties, 0, TextSchema._inheritableTableCellProperties, array3.Length, TextSchema._inheritableTextElementProperties.Length);
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001141E3 File Offset: 0x001123E3
		internal static bool IsInTextContent(ITextPointer position)
		{
			return TextSchema.IsValidChild(position, typeof(string));
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x001141F8 File Offset: 0x001123F8
		internal static bool ValidateChild(TextElement parent, TextElement child, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			if (TextSchema.HasHyperlinkAncestor(parent) && TextSchema.HasIllegalHyperlinkDescendant(child, throwIfIllegalHyperlinkDescendent))
			{
				return false;
			}
			bool flag = TextSchema.IsValidChild(parent.GetType(), child.GetType());
			if (!flag && throwIfIllegalChild)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					parent.GetType().Name,
					child.GetType().Name
				}));
			}
			return flag;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00114265 File Offset: 0x00112465
		internal static bool IsValidChild(TextElement parent, Type childType)
		{
			return TextSchema.ValidateChild(parent, childType, false, false);
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00114270 File Offset: 0x00112470
		internal static bool ValidateChild(TextElement parent, Type childType, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			if (TextSchema.HasHyperlinkAncestor(parent) && (typeof(Hyperlink).IsAssignableFrom(childType) || typeof(AnchoredBlock).IsAssignableFrom(childType)))
			{
				if (throwIfIllegalHyperlinkDescendent)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_IllegalHyperlinkChild", new object[]
					{
						childType
					}));
				}
				return false;
			}
			else
			{
				bool flag = TextSchema.IsValidChild(parent.GetType(), childType);
				if (!flag && throwIfIllegalChild)
				{
					throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
					{
						parent.GetType().Name,
						childType.Name
					}));
				}
				return flag;
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0011430B File Offset: 0x0011250B
		internal static bool IsValidChild(TextPointer position, Type childType)
		{
			return TextSchema.ValidateChild(position, childType, false, false);
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x00114318 File Offset: 0x00112518
		internal static bool ValidateChild(TextPointer position, Type childType, bool throwIfIllegalChild, bool throwIfIllegalHyperlinkDescendent)
		{
			DependencyObject parent = position.Parent;
			if (parent == null)
			{
				TextElement adjacentElementFromOuterPosition = position.GetAdjacentElementFromOuterPosition(LogicalDirection.Backward);
				TextElement adjacentElementFromOuterPosition2 = position.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				return (adjacentElementFromOuterPosition == null || TextSchema.IsValidSibling(adjacentElementFromOuterPosition.GetType(), childType)) && (adjacentElementFromOuterPosition2 == null || TextSchema.IsValidSibling(adjacentElementFromOuterPosition2.GetType(), childType));
			}
			if (parent is TextElement)
			{
				return TextSchema.ValidateChild((TextElement)parent, childType, throwIfIllegalChild, throwIfIllegalHyperlinkDescendent);
			}
			bool flag = TextSchema.IsValidChild(parent.GetType(), childType);
			if (!flag && throwIfIllegalChild)
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					parent.GetType().Name,
					childType.Name
				}));
			}
			return flag;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x001143C0 File Offset: 0x001125C0
		internal static bool IsValidSibling(Type siblingType, Type newType)
		{
			if (typeof(Inline).IsAssignableFrom(newType))
			{
				return typeof(Inline).IsAssignableFrom(siblingType);
			}
			if (typeof(Block).IsAssignableFrom(newType))
			{
				return typeof(Block).IsAssignableFrom(siblingType);
			}
			if (typeof(TableRowGroup).IsAssignableFrom(newType))
			{
				return typeof(TableRowGroup).IsAssignableFrom(siblingType);
			}
			if (typeof(TableRow).IsAssignableFrom(newType))
			{
				return typeof(TableRow).IsAssignableFrom(siblingType);
			}
			if (typeof(TableCell).IsAssignableFrom(newType))
			{
				return typeof(TableCell).IsAssignableFrom(siblingType);
			}
			if (typeof(ListItem).IsAssignableFrom(newType))
			{
				return typeof(ListItem).IsAssignableFrom(siblingType);
			}
			Invariant.Assert(false, "unexpected value for newType");
			return false;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x001144AC File Offset: 0x001126AC
		internal static bool IsValidChild(ITextPointer position, Type childType)
		{
			return (!typeof(TextElement).IsAssignableFrom(position.ParentType) || !TextPointerBase.IsInHyperlinkScope(position) || (!typeof(Hyperlink).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType))) && TextSchema.IsValidChild(position.ParentType, childType);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0011450A File Offset: 0x0011270A
		internal static bool IsValidChildOfContainer(Type parentType, Type childType)
		{
			Invariant.Assert(!typeof(TextElement).IsAssignableFrom(parentType));
			return TextSchema.IsValidChild(parentType, childType);
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0011452C File Offset: 0x0011272C
		internal static bool HasHyperlinkAncestor(TextElement element)
		{
			Inline inline = element as Inline;
			while (inline != null && !(inline is Hyperlink))
			{
				inline = (inline.Parent as Inline);
			}
			return inline != null;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x0011455D File Offset: 0x0011275D
		internal static bool IsFormattingType(Type elementType)
		{
			return typeof(Run).IsAssignableFrom(elementType) || typeof(Span).IsAssignableFrom(elementType);
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00114583 File Offset: 0x00112783
		internal static bool IsKnownType(Type elementType)
		{
			return elementType.Module == typeof(TextElement).Module || elementType.Module == typeof(UIElement).Module;
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x001145BD File Offset: 0x001127BD
		internal static bool IsNonFormattingInline(Type elementType)
		{
			return typeof(Inline).IsAssignableFrom(elementType) && !TextSchema.IsFormattingType(elementType);
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x001145DC File Offset: 0x001127DC
		internal static bool IsMergeableInline(Type elementType)
		{
			return TextSchema.IsFormattingType(elementType) && !TextSchema.IsNonMergeableInline(elementType);
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x001145F4 File Offset: 0x001127F4
		internal static bool IsNonMergeableInline(Type elementType)
		{
			TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(elementType, typeof(TextElementEditingBehaviorAttribute));
			return textElementEditingBehaviorAttribute != null && !textElementEditingBehaviorAttribute.IsMergeable;
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00114628 File Offset: 0x00112828
		internal static bool AllowsParagraphMerging(Type elementType)
		{
			return typeof(Paragraph).IsAssignableFrom(elementType) || typeof(ListItem).IsAssignableFrom(elementType) || typeof(List).IsAssignableFrom(elementType) || typeof(Section).IsAssignableFrom(elementType);
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0011467D File Offset: 0x0011287D
		internal static bool IsParagraphOrBlockUIContainer(Type elementType)
		{
			return typeof(Paragraph).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType);
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x001146A3 File Offset: 0x001128A3
		internal static bool IsBlock(Type type)
		{
			return typeof(Block).IsAssignableFrom(type);
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x001146B5 File Offset: 0x001128B5
		internal static bool IsBreak(Type type)
		{
			return typeof(LineBreak).IsAssignableFrom(type);
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x001146C7 File Offset: 0x001128C7
		internal static bool HasTextDecorations(object value)
		{
			return value is TextDecorationCollection && ((TextDecorationCollection)value).Count > 0;
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x001146E4 File Offset: 0x001128E4
		internal static Type GetStandardElementType(Type type, bool reduceElement)
		{
			if (typeof(Run).IsAssignableFrom(type))
			{
				return typeof(Run);
			}
			if (typeof(Hyperlink).IsAssignableFrom(type))
			{
				return typeof(Hyperlink);
			}
			if (typeof(Span).IsAssignableFrom(type))
			{
				return typeof(Span);
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(type))
			{
				if (!reduceElement)
				{
					return typeof(InlineUIContainer);
				}
				return typeof(Run);
			}
			else
			{
				if (typeof(LineBreak).IsAssignableFrom(type))
				{
					return typeof(LineBreak);
				}
				if (typeof(Floater).IsAssignableFrom(type))
				{
					return typeof(Floater);
				}
				if (typeof(Figure).IsAssignableFrom(type))
				{
					return typeof(Figure);
				}
				if (typeof(Paragraph).IsAssignableFrom(type))
				{
					return typeof(Paragraph);
				}
				if (typeof(Section).IsAssignableFrom(type))
				{
					return typeof(Section);
				}
				if (typeof(List).IsAssignableFrom(type))
				{
					return typeof(List);
				}
				if (typeof(Table).IsAssignableFrom(type))
				{
					return typeof(Table);
				}
				if (typeof(BlockUIContainer).IsAssignableFrom(type))
				{
					if (!reduceElement)
					{
						return typeof(BlockUIContainer);
					}
					return typeof(Paragraph);
				}
				else
				{
					if (typeof(ListItem).IsAssignableFrom(type))
					{
						return typeof(ListItem);
					}
					if (typeof(TableColumn).IsAssignableFrom(type))
					{
						return typeof(TableColumn);
					}
					if (typeof(TableRowGroup).IsAssignableFrom(type))
					{
						return typeof(TableRowGroup);
					}
					if (typeof(TableRow).IsAssignableFrom(type))
					{
						return typeof(TableRow);
					}
					if (typeof(TableCell).IsAssignableFrom(type))
					{
						return typeof(TableCell);
					}
					Invariant.Assert(false, "We do not expect any unknown elements derived directly from TextElement, Block or Inline. Schema must have been checking for that");
					return null;
				}
			}
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00114908 File Offset: 0x00112B08
		internal static DependencyProperty[] GetInheritableProperties(Type type)
		{
			if (typeof(TableCell).IsAssignableFrom(type))
			{
				return TextSchema._inheritableTableCellProperties;
			}
			if (typeof(Block).IsAssignableFrom(type) || typeof(FlowDocument).IsAssignableFrom(type))
			{
				return TextSchema._inheritableBlockProperties;
			}
			Invariant.Assert(typeof(TextElement).IsAssignableFrom(type) || typeof(TableColumn).IsAssignableFrom(type), "type must be one of TextElement, FlowDocument or TableColumn");
			return TextSchema._inheritableTextElementProperties;
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0011498C File Offset: 0x00112B8C
		internal static DependencyProperty[] GetNoninheritableProperties(Type type)
		{
			if (typeof(Run).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(Hyperlink).IsAssignableFrom(type))
			{
				return TextSchema._hyperlinkProperties;
			}
			if (typeof(Span).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(type))
			{
				return TextSchema._inlineProperties;
			}
			if (typeof(LineBreak).IsAssignableFrom(type))
			{
				return TextSchema._emptyPropertyList;
			}
			if (typeof(Floater).IsAssignableFrom(type))
			{
				return TextSchema._floaterProperties;
			}
			if (typeof(Figure).IsAssignableFrom(type))
			{
				return TextSchema._figureProperties;
			}
			if (typeof(Paragraph).IsAssignableFrom(type))
			{
				return TextSchema._paragraphProperties;
			}
			if (typeof(Section).IsAssignableFrom(type))
			{
				return TextSchema._blockProperties;
			}
			if (typeof(List).IsAssignableFrom(type))
			{
				return TextSchema._listProperties;
			}
			if (typeof(Table).IsAssignableFrom(type))
			{
				return TextSchema._tableProperties;
			}
			if (typeof(BlockUIContainer).IsAssignableFrom(type))
			{
				return TextSchema._blockProperties;
			}
			if (typeof(ListItem).IsAssignableFrom(type))
			{
				return TextSchema._listItemProperties;
			}
			if (typeof(TableColumn).IsAssignableFrom(type))
			{
				return TextSchema._tableColumnProperties;
			}
			if (typeof(TableRowGroup).IsAssignableFrom(type))
			{
				return TextSchema._tableRowGroupProperties;
			}
			if (typeof(TableRow).IsAssignableFrom(type))
			{
				return TextSchema._tableRowProperties;
			}
			if (typeof(TableCell).IsAssignableFrom(type))
			{
				return TextSchema._tableCellProperties;
			}
			Invariant.Assert(false, "We do not expect any unknown elements derived directly from TextElement. Schema must have been checking for that");
			return TextSchema._emptyPropertyList;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00114B44 File Offset: 0x00112D44
		internal static bool ValuesAreEqual(object value1, object value2)
		{
			if (value1 == value2)
			{
				return true;
			}
			if (value1 == null)
			{
				if (value2 is TextDecorationCollection)
				{
					TextDecorationCollection textDecorationCollection = (TextDecorationCollection)value2;
					return textDecorationCollection.Count == 0;
				}
				if (value2 is TextEffectCollection)
				{
					TextEffectCollection textEffectCollection = (TextEffectCollection)value2;
					return textEffectCollection.Count == 0;
				}
				return false;
			}
			else if (value2 == null)
			{
				if (value1 is TextDecorationCollection)
				{
					TextDecorationCollection textDecorationCollection2 = (TextDecorationCollection)value1;
					return textDecorationCollection2.Count == 0;
				}
				if (value1 is TextEffectCollection)
				{
					TextEffectCollection textEffectCollection2 = (TextEffectCollection)value1;
					return textEffectCollection2.Count == 0;
				}
				return false;
			}
			else
			{
				if (value1.GetType() != value2.GetType())
				{
					return false;
				}
				if (value1 is TextDecorationCollection)
				{
					TextDecorationCollection textDecorationCollection3 = (TextDecorationCollection)value1;
					TextDecorationCollection textDecorations = (TextDecorationCollection)value2;
					return textDecorationCollection3.ValueEquals(textDecorations);
				}
				if (value1 is FontFamily)
				{
					FontFamily fontFamily = (FontFamily)value1;
					FontFamily obj = (FontFamily)value2;
					return fontFamily.Equals(obj);
				}
				if (value1 is Brush)
				{
					return TextSchema.AreBrushesEqual((Brush)value1, (Brush)value2);
				}
				string a = value1.ToString();
				string b = value2.ToString();
				return a == b;
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x00114C54 File Offset: 0x00112E54
		internal static bool IsParagraphProperty(DependencyProperty formattingProperty)
		{
			for (int i = 0; i < TextSchema._inheritableBlockProperties.Length; i++)
			{
				if (formattingProperty == TextSchema._inheritableBlockProperties[i])
				{
					return true;
				}
			}
			for (int j = 0; j < TextSchema._paragraphProperties.Length; j++)
			{
				if (formattingProperty == TextSchema._paragraphProperties[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00114CA0 File Offset: 0x00112EA0
		internal static bool IsCharacterProperty(DependencyProperty formattingProperty)
		{
			for (int i = 0; i < TextSchema._inheritableTextElementProperties.Length; i++)
			{
				if (formattingProperty == TextSchema._inheritableTextElementProperties[i])
				{
					return true;
				}
			}
			for (int j = 0; j < TextSchema._inlineProperties.Length; j++)
			{
				if (formattingProperty == TextSchema._inlineProperties[j])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00114CEC File Offset: 0x00112EEC
		internal static bool IsNonFormattingCharacterProperty(DependencyProperty property)
		{
			for (int i = 0; i < TextSchema._nonFormattingCharacterProperties.Length; i++)
			{
				if (property == TextSchema._nonFormattingCharacterProperties[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00114D18 File Offset: 0x00112F18
		internal static DependencyProperty[] GetNonFormattingCharacterProperties()
		{
			return TextSchema._nonFormattingCharacterProperties;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x00114D20 File Offset: 0x00112F20
		internal static bool IsStructuralCharacterProperty(DependencyProperty formattingProperty)
		{
			int num = 0;
			while (num < TextSchema._structuralCharacterProperties.Length && formattingProperty != TextSchema._structuralCharacterProperties[num])
			{
				num++;
			}
			return num < TextSchema._structuralCharacterProperties.Length;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00114D54 File Offset: 0x00112F54
		internal static bool IsPropertyIncremental(DependencyProperty property)
		{
			if (property == null)
			{
				return false;
			}
			Type propertyType = property.PropertyType;
			return typeof(double).IsAssignableFrom(propertyType) || typeof(long).IsAssignableFrom(propertyType) || typeof(int).IsAssignableFrom(propertyType) || typeof(Thickness).IsAssignableFrom(propertyType);
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x00114DB5 File Offset: 0x00112FB5
		internal static DependencyProperty[] BehavioralProperties
		{
			get
			{
				return TextSchema._behavioralPropertyList;
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06003C02 RID: 15362 RVA: 0x00114DBC File Offset: 0x00112FBC
		internal static DependencyProperty[] ImageProperties
		{
			get
			{
				return TextSchema._imagePropertyList;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06003C03 RID: 15363 RVA: 0x00114DC3 File Offset: 0x00112FC3
		internal static DependencyProperty[] StructuralCharacterProperties
		{
			get
			{
				return TextSchema._structuralCharacterProperties;
			}
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00114DCC File Offset: 0x00112FCC
		private static bool IsValidChild(Type parentType, Type childType)
		{
			if (parentType == null || typeof(Run).IsAssignableFrom(parentType) || typeof(TextBox).IsAssignableFrom(parentType) || typeof(PasswordBox).IsAssignableFrom(parentType))
			{
				return childType == typeof(string);
			}
			if (typeof(TextBlock).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType);
			}
			if (typeof(Hyperlink).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType) && !typeof(Hyperlink).IsAssignableFrom(childType) && !typeof(AnchoredBlock).IsAssignableFrom(childType);
			}
			if (typeof(Span).IsAssignableFrom(parentType) || typeof(Paragraph).IsAssignableFrom(parentType) || typeof(AccessText).IsAssignableFrom(parentType))
			{
				return typeof(Inline).IsAssignableFrom(childType);
			}
			if (typeof(InlineUIContainer).IsAssignableFrom(parentType))
			{
				return typeof(UIElement).IsAssignableFrom(childType);
			}
			if (typeof(List).IsAssignableFrom(parentType))
			{
				return typeof(ListItem).IsAssignableFrom(childType);
			}
			if (typeof(Table).IsAssignableFrom(parentType))
			{
				return typeof(TableRowGroup).IsAssignableFrom(childType);
			}
			if (typeof(TableRowGroup).IsAssignableFrom(parentType))
			{
				return typeof(TableRow).IsAssignableFrom(childType);
			}
			if (typeof(TableRow).IsAssignableFrom(parentType))
			{
				return typeof(TableCell).IsAssignableFrom(childType);
			}
			if (typeof(Section).IsAssignableFrom(parentType) || typeof(ListItem).IsAssignableFrom(parentType) || typeof(TableCell).IsAssignableFrom(parentType) || typeof(Floater).IsAssignableFrom(parentType) || typeof(Figure).IsAssignableFrom(parentType) || typeof(FlowDocument).IsAssignableFrom(parentType))
			{
				return typeof(Block).IsAssignableFrom(childType);
			}
			return typeof(BlockUIContainer).IsAssignableFrom(parentType) && typeof(UIElement).IsAssignableFrom(childType);
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00115048 File Offset: 0x00113248
		private static bool HasIllegalHyperlinkDescendant(TextElement element, bool throwIfIllegalDescendent)
		{
			TextPointer textPointer = element.ElementStart;
			TextPointer elementEnd = element.ElementEnd;
			while (textPointer.CompareTo(elementEnd) < 0)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
				if (pointerContext == TextPointerContext.ElementStart)
				{
					TextElement textElement = (TextElement)textPointer.GetAdjacentElement(LogicalDirection.Forward);
					if (textElement is Hyperlink || textElement is AnchoredBlock)
					{
						if (throwIfIllegalDescendent)
						{
							throw new InvalidOperationException(SR.Get("TextSchema_IllegalHyperlinkChild", new object[]
							{
								textElement.GetType()
							}));
						}
						return true;
					}
				}
				textPointer = textPointer.GetNextContextPosition(LogicalDirection.Forward);
			}
			return false;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001150C8 File Offset: 0x001132C8
		private static bool AreBrushesEqual(Brush brush1, Brush brush2)
		{
			SolidColorBrush solidColorBrush = brush1 as SolidColorBrush;
			if (solidColorBrush != null)
			{
				return solidColorBrush.Color.Equals(((SolidColorBrush)brush2).Color);
			}
			string stringValue = DPTypeDescriptorContext.GetStringValue(TextElement.BackgroundProperty, brush1);
			string stringValue2 = DPTypeDescriptorContext.GetStringValue(TextElement.BackgroundProperty, brush2);
			return stringValue != null && stringValue2 != null && stringValue == stringValue2;
		}

		// Token: 0x040025EC RID: 9708
		private static readonly DependencyProperty[] _inheritableTextElementProperties;

		// Token: 0x040025ED RID: 9709
		private static readonly DependencyProperty[] _inheritableBlockProperties;

		// Token: 0x040025EE RID: 9710
		private static readonly DependencyProperty[] _inheritableTableCellProperties;

		// Token: 0x040025EF RID: 9711
		private static readonly DependencyProperty[] _hyperlinkProperties = new DependencyProperty[]
		{
			Hyperlink.NavigateUriProperty,
			Hyperlink.TargetNameProperty,
			Hyperlink.CommandProperty,
			Hyperlink.CommandParameterProperty,
			Hyperlink.CommandTargetProperty,
			Inline.BaselineAlignmentProperty,
			Inline.TextDecorationsProperty,
			TextElement.BackgroundProperty,
			FrameworkContentElement.ToolTipProperty
		};

		// Token: 0x040025F0 RID: 9712
		private static readonly DependencyProperty[] _inlineProperties = new DependencyProperty[]
		{
			Inline.BaselineAlignmentProperty,
			Inline.TextDecorationsProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F1 RID: 9713
		private static readonly DependencyProperty[] _paragraphProperties = new DependencyProperty[]
		{
			Paragraph.MinWidowLinesProperty,
			Paragraph.MinOrphanLinesProperty,
			Paragraph.TextIndentProperty,
			Paragraph.KeepWithNextProperty,
			Paragraph.KeepTogetherProperty,
			Paragraph.TextDecorationsProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F2 RID: 9714
		private static readonly DependencyProperty[] _listProperties = new DependencyProperty[]
		{
			List.MarkerStyleProperty,
			List.MarkerOffsetProperty,
			List.StartIndexProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F3 RID: 9715
		private static readonly DependencyProperty[] _listItemProperties = new DependencyProperty[]
		{
			ListItem.MarginProperty,
			ListItem.PaddingProperty,
			ListItem.BorderThicknessProperty,
			ListItem.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F4 RID: 9716
		private static readonly DependencyProperty[] _tableProperties = new DependencyProperty[]
		{
			Table.CellSpacingProperty,
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F5 RID: 9717
		private static readonly DependencyProperty[] _tableColumnProperties = new DependencyProperty[]
		{
			TableColumn.WidthProperty,
			TableColumn.BackgroundProperty
		};

		// Token: 0x040025F6 RID: 9718
		private static readonly DependencyProperty[] _tableRowGroupProperties = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F7 RID: 9719
		private static readonly DependencyProperty[] _tableRowProperties = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F8 RID: 9720
		private static readonly DependencyProperty[] _tableCellProperties = new DependencyProperty[]
		{
			TableCell.ColumnSpanProperty,
			TableCell.RowSpanProperty,
			TableCell.PaddingProperty,
			TableCell.BorderThicknessProperty,
			TableCell.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025F9 RID: 9721
		private static readonly DependencyProperty[] _floaterProperties = new DependencyProperty[]
		{
			Floater.HorizontalAlignmentProperty,
			Floater.WidthProperty,
			AnchoredBlock.MarginProperty,
			AnchoredBlock.PaddingProperty,
			AnchoredBlock.BorderThicknessProperty,
			AnchoredBlock.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025FA RID: 9722
		private static readonly DependencyProperty[] _figureProperties = new DependencyProperty[]
		{
			Figure.HorizontalAnchorProperty,
			Figure.VerticalAnchorProperty,
			Figure.HorizontalOffsetProperty,
			Figure.VerticalOffsetProperty,
			Figure.CanDelayPlacementProperty,
			Figure.WrapDirectionProperty,
			Figure.WidthProperty,
			Figure.HeightProperty,
			AnchoredBlock.MarginProperty,
			AnchoredBlock.PaddingProperty,
			AnchoredBlock.BorderThicknessProperty,
			AnchoredBlock.BorderBrushProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025FB RID: 9723
		private static readonly DependencyProperty[] _blockProperties = new DependencyProperty[]
		{
			Block.MarginProperty,
			Block.PaddingProperty,
			Block.BorderThicknessProperty,
			Block.BorderBrushProperty,
			Block.BreakPageBeforeProperty,
			Block.BreakColumnBeforeProperty,
			Block.ClearFloatersProperty,
			Block.IsHyphenationEnabledProperty,
			TextElement.BackgroundProperty
		};

		// Token: 0x040025FC RID: 9724
		private static readonly DependencyProperty[] _textElementPropertyList = new DependencyProperty[]
		{
			TextElement.BackgroundProperty
		};

		// Token: 0x040025FD RID: 9725
		private static readonly DependencyProperty[] _imagePropertyList = new DependencyProperty[]
		{
			Image.SourceProperty,
			Image.StretchProperty,
			Image.StretchDirectionProperty,
			FrameworkElement.LanguageProperty,
			FrameworkElement.LayoutTransformProperty,
			FrameworkElement.WidthProperty,
			FrameworkElement.MinWidthProperty,
			FrameworkElement.MaxWidthProperty,
			FrameworkElement.HeightProperty,
			FrameworkElement.MinHeightProperty,
			FrameworkElement.MaxHeightProperty,
			FrameworkElement.MarginProperty,
			FrameworkElement.HorizontalAlignmentProperty,
			FrameworkElement.VerticalAlignmentProperty,
			FrameworkElement.CursorProperty,
			FrameworkElement.ForceCursorProperty,
			FrameworkElement.ToolTipProperty,
			UIElement.RenderTransformProperty,
			UIElement.RenderTransformOriginProperty,
			UIElement.OpacityProperty,
			UIElement.OpacityMaskProperty,
			UIElement.BitmapEffectProperty,
			UIElement.BitmapEffectInputProperty,
			UIElement.VisibilityProperty,
			UIElement.ClipToBoundsProperty,
			UIElement.ClipProperty,
			UIElement.SnapsToDevicePixelsProperty,
			TextBlock.BaselineOffsetProperty
		};

		// Token: 0x040025FE RID: 9726
		private static readonly DependencyProperty[] _behavioralPropertyList = new DependencyProperty[]
		{
			UIElement.AllowDropProperty
		};

		// Token: 0x040025FF RID: 9727
		private static readonly DependencyProperty[] _emptyPropertyList = new DependencyProperty[0];

		// Token: 0x04002600 RID: 9728
		private static readonly DependencyProperty[] _structuralCharacterProperties = new DependencyProperty[]
		{
			Inline.FlowDirectionProperty
		};

		// Token: 0x04002601 RID: 9729
		private static readonly DependencyProperty[] _nonFormattingCharacterProperties = new DependencyProperty[]
		{
			FrameworkElement.FlowDirectionProperty,
			FrameworkElement.LanguageProperty,
			Run.TextProperty
		};
	}
}
