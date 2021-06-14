using System;
using System.Windows.Input;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003F8 RID: 1016
	internal static class TextEditorParagraphs
	{
		// Token: 0x0600388C RID: 14476 RVA: 0x000FDE2C File Offset: 0x000FC02C
		internal static void _RegisterClassHandlers(Type controlType, bool acceptsRichContent, bool registerEventListeners)
		{
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorParagraphs.OnQueryStatusNYI);
			if (acceptsRichContent)
			{
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignLeft, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignLeft), canExecuteRoutedEventHandler, "KeyAlignLeft", "KeyAlignLeftDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignCenter, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignCenter), canExecuteRoutedEventHandler, "KeyAlignCenter", "KeyAlignCenterDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignRight, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignRight), canExecuteRoutedEventHandler, "KeyAlignRight", "KeyAlignRightDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.AlignJustify, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnAlignJustify), canExecuteRoutedEventHandler, "KeyAlignJustify", "KeyAlignJustifyDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplySingleSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplySingleSpace), canExecuteRoutedEventHandler, "KeyApplySingleSpace", "KeyApplySingleSpaceDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyOneAndAHalfSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyOneAndAHalfSpace), canExecuteRoutedEventHandler, "KeyApplyOneAndAHalfSpace", "KeyApplyOneAndAHalfSpaceDisplayString");
				CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyDoubleSpace, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyDoubleSpace), canExecuteRoutedEventHandler, "KeyApplyDoubleSpace", "KeyApplyDoubleSpaceDisplayString");
			}
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyParagraphFlowDirectionLTR, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyParagraphFlowDirectionLTR), canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ApplyParagraphFlowDirectionRTL, new ExecutedRoutedEventHandler(TextEditorParagraphs.OnApplyParagraphFlowDirectionRTL), canExecuteRoutedEventHandler);
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x000FDF6C File Offset: 0x000FC16C
		private static void OnAlignLeft(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Left, true);
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x000FDF98 File Offset: 0x000FC198
		private static void OnAlignCenter(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Center, true);
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x000FDFC4 File Offset: 0x000FC1C4
		private static void OnAlignRight(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Right, true);
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x000FDFF0 File Offset: 0x000FC1F0
		private static void OnAlignJustify(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorCharacters._OnApplyProperty(textEditor, Block.TextAlignmentProperty, TextAlignment.Justify, true);
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnApplySingleSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnApplyOneAndAHalfSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnApplyDoubleSpace(object sender, ExecutedRoutedEventArgs e)
		{
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x000FE01C File Offset: 0x000FC21C
		private static void OnApplyParagraphFlowDirectionLTR(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor @this = TextEditor._GetTextEditor(sender);
			TextEditorCharacters._OnApplyProperty(@this, FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight, true);
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x000FE044 File Offset: 0x000FC244
		private static void OnApplyParagraphFlowDirectionRTL(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor @this = TextEditor._GetTextEditor(sender);
			TextEditorCharacters._OnApplyProperty(@this, FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft, true);
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x000FE06C File Offset: 0x000FC26C
		private static void OnQueryStatusNYI(object sender, CanExecuteRoutedEventArgs e)
		{
			if (TextEditor._GetTextEditor(sender) == null)
			{
				return;
			}
			e.CanExecute = true;
		}
	}
}
