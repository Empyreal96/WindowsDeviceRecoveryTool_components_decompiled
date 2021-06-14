using System;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003F2 RID: 1010
	internal static class TextEditorCharacters
	{
		// Token: 0x06003833 RID: 14387 RVA: 0x000FA86C File Offset: 0x000F8A6C
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ResetFormat, new ExecutedRoutedEventHandler(TextEditorCharacters.OnResetFormat), canExecuteRoutedEventHandler, "KeyResetFormat", "KeyResetFormatDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleBold, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleBold), canExecuteRoutedEventHandler, "KeyToggleBold", "KeyToggleBoldDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleItalic, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleItalic), canExecuteRoutedEventHandler, "KeyToggleItalic", "KeyToggleItalicDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleUnderline, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleUnderline), canExecuteRoutedEventHandler, "KeyToggleUnderline", "KeyToggleUnderlineDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSubscript, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSubscript), canExecuteRoutedEventHandler, "KeyToggleSubscript", "KeyToggleSubscriptDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSuperscript, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSuperscript), canExecuteRoutedEventHandler, "KeyToggleSuperscript", "KeyToggleSuperscriptDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IncreaseFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnIncreaseFontSize), canExecuteRoutedEventHandler, "KeyIncreaseFontSize", "KeyIncreaseFontSizeDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DecreaseFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnDecreaseFontSize), canExecuteRoutedEventHandler, "KeyDecreaseFontSize", "KeyDecreaseFontSizeDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyFontSize, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyFontSize), canExecuteRoutedEventHandler, "KeyApplyFontSize", "KeyApplyFontSizeDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyFontFamily, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyFontFamily), canExecuteRoutedEventHandler, "KeyApplyFontFamily", "KeyApplyFontFamilyDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyForeground, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyForeground), canExecuteRoutedEventHandler, "KeyApplyForeground", "KeyApplyForegroundDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyBackground, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyBackground), canExecuteRoutedEventHandler, "KeyApplyBackground", "KeyApplyBackgroundDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleSpellCheck, new ExecutedRoutedEventHandler(TextEditorCharacters.OnToggleSpellCheck), canExecuteRoutedEventHandler, "KeyToggleSpellCheck", "KeyToggleSpellCheckDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyInlineFlowDirectionRTL, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyInlineFlowDirectionRTL), new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyInlineFlowDirectionLTR, new ExecutedRoutedEventHandler(TextEditorCharacters.OnApplyInlineFlowDirectionLTR), new CanExecuteRoutedEventHandler(TextEditorCharacters.OnQueryStatusNYI));
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x000FAA86 File Offset: 0x000F8C86
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue)
		{
			TextEditorCharacters._OnApplyProperty(This, formattingProperty, propertyValue, false, PropertyValueAction.SetValue);
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x000FAA92 File Offset: 0x000F8C92
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue, bool applyToParagraphs)
		{
			TextEditorCharacters._OnApplyProperty(This, formattingProperty, propertyValue, applyToParagraphs, PropertyValueAction.SetValue);
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x000FAAA0 File Offset: 0x000F8CA0
		internal static void _OnApplyProperty(TextEditor This, DependencyProperty formattingProperty, object propertyValue, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			if (This == null || !This._IsEnabled || This.IsReadOnly || !This.AcceptsRichContent || !(This.Selection is TextSelection))
			{
				return;
			}
			if (!TextSchema.IsParagraphProperty(formattingProperty) && !TextSchema.IsCharacterProperty(formattingProperty))
			{
				Invariant.Assert(false, "The property '" + formattingProperty.Name + "' is unknown to TextEditor");
				return;
			}
			TextSelection textSelection = (TextSelection)This.Selection;
			if (TextSchema.IsStructuralCharacterProperty(formattingProperty) && !TextRangeEdit.CanApplyStructuralInlineProperty(textSelection.Start, textSelection.End))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(This);
			TextEditorSelection._ClearSuggestedX(This);
			TextEditorTyping._BreakTypingSequence(This);
			textSelection.ApplyPropertyValue(formattingProperty, propertyValue, applyToParagraphs, propertyValueAction);
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x000FAB48 File Offset: 0x000F8D48
		private static void OnResetFormat(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection.Start is TextPointer))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextPointer start = (TextPointer)textEditor.Selection.Start;
				TextPointer end = (TextPointer)textEditor.Selection.End;
				if (textEditor.Selection.IsEmpty)
				{
					TextSegment autoWord = TextRangeBase.GetAutoWord(textEditor.Selection);
					if (autoWord.IsNull)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
						return;
					}
					start = (TextPointer)autoWord.Start;
					end = (TextPointer)autoWord.End;
				}
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextRangeEdit.CharacterResetFormatting(start, end);
			}
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x000FAC38 File Offset: 0x000F8E38
		private static void OnToggleBold(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontWeightProperty);
			FontWeight fontWeight = (currentValue != DependencyProperty.UnsetValue && (FontWeight)currentValue == FontWeights.Bold) ? FontWeights.Normal : FontWeights.Bold;
			TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontWeightProperty, fontWeight);
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000FACCC File Offset: 0x000F8ECC
		private static void OnToggleItalic(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontStyleProperty);
			FontStyle fontStyle = (currentValue != DependencyProperty.UnsetValue && (FontStyle)currentValue == FontStyles.Italic) ? FontStyles.Normal : FontStyles.Italic;
			TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontStyleProperty, fontStyle);
			textEditor.Selection.RefreshCaret();
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x000FAD68 File Offset: 0x000F8F68
		private static void OnToggleUnderline(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			object currentValue = ((TextSelection)textEditor.Selection).GetCurrentValue(Inline.TextDecorationsProperty);
			TextDecorationCollection textDecorationCollection = (currentValue != DependencyProperty.UnsetValue) ? ((TextDecorationCollection)currentValue) : null;
			TextDecorationCollection underline;
			if (!TextSchema.HasTextDecorations(textDecorationCollection))
			{
				underline = TextDecorations.Underline;
			}
			else if (!textDecorationCollection.TryRemove(TextDecorations.Underline, out underline))
			{
				underline.Add(TextDecorations.Underline);
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Inline.TextDecorationsProperty, underline);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x000FAE0C File Offset: 0x000F900C
		private static void OnToggleSubscript(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			FontVariants fontVariants = (FontVariants)((TextSelection)textEditor.Selection).GetCurrentValue(Typography.VariantsProperty);
			fontVariants = ((fontVariants == FontVariants.Subscript) ? FontVariants.Normal : FontVariants.Subscript);
			TextEditorCharacters._OnApplyProperty(textEditor, Typography.VariantsProperty, fontVariants);
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x000FAE84 File Offset: 0x000F9084
		private static void OnToggleSuperscript(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			FontVariants fontVariants = (FontVariants)((TextSelection)textEditor.Selection).GetCurrentValue(Typography.VariantsProperty);
			fontVariants = ((fontVariants == FontVariants.Superscript) ? FontVariants.Normal : FontVariants.Superscript);
			TextEditorCharacters._OnApplyProperty(textEditor, Typography.VariantsProperty, fontVariants);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000FAEFC File Offset: 0x000F90FC
		private static void OnIncreaseFontSize(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (textEditor.Selection.IsEmpty)
			{
				double num = (double)((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontSizeProperty);
				if (num == 0.0)
				{
					return;
				}
				if (num < 1638.0)
				{
					num += 0.75;
					if (num > 1638.0)
					{
						num = 1638.0;
					}
					TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, num);
					return;
				}
			}
			else
			{
				TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, 0.75, false, PropertyValueAction.IncreaseByAbsoluteValue);
			}
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000FAFD0 File Offset: 0x000F91D0
		private static void OnDecreaseFontSize(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (textEditor.Selection.IsEmpty)
			{
				double num = (double)((TextSelection)textEditor.Selection).GetCurrentValue(TextElement.FontSizeProperty);
				if (num == 0.0)
				{
					return;
				}
				if (num > 0.75)
				{
					num -= 0.75;
					if (num < 0.75)
					{
						num = 0.75;
					}
					TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, num);
					return;
				}
			}
			else
			{
				TextEditorCharacters._OnApplyProperty(textEditor, TextElement.FontSizeProperty, 0.75, false, PropertyValueAction.DecreaseByAbsoluteValue);
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000FB0A4 File Offset: 0x000F92A4
		private static void OnApplyFontSize(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, TextElement.FontSizeProperty, args.Parameter);
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x000FB0D4 File Offset: 0x000F92D4
		private static void OnApplyFontFamily(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, TextElement.FontFamilyProperty, args.Parameter);
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x000FB104 File Offset: 0x000F9304
		private static void OnApplyForeground(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, TextElement.ForegroundProperty, args.Parameter);
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x000FB134 File Offset: 0x000F9334
		private static void OnApplyBackground(object target, ExecutedRoutedEventArgs args)
		{
			if (args.Parameter == null)
			{
				return;
			}
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, TextElement.BackgroundProperty, args.Parameter);
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x000FB164 File Offset: 0x000F9364
		private static void OnToggleSpellCheck(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.IsSpellCheckEnabled = !textEditor.IsSpellCheckEnabled;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x000FB19C File Offset: 0x000F939C
		private static void OnApplyInlineFlowDirectionRTL(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, Inline.FlowDirectionProperty, FlowDirection.RightToLeft);
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x000FB1C4 File Offset: 0x000F93C4
		private static void OnApplyInlineFlowDirectionLTR(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor @this = TextEditor._GetTextEditor(target);
			TextEditorCharacters._OnApplyProperty(@this, Inline.FlowDirectionProperty, FlowDirection.LeftToRight);
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000FB1EC File Offset: 0x000F93EC
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x04002597 RID: 9623
		internal const double OneFontPoint = 0.75;

		// Token: 0x04002598 RID: 9624
		internal const double MaxFontPoint = 1638.0;
	}
}
