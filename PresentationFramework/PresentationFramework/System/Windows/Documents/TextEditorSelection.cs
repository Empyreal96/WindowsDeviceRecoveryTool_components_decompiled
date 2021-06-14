using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003F9 RID: 1017
	internal static class TextEditorSelection
	{
		// Token: 0x06003897 RID: 14487 RVA: 0x000FE08C File Offset: 0x000FC28C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorSelection.OnNYICommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorSelection.OnQueryStatusCaretNavigation);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler2 = new CanExecuteRoutedEventHandler(TextEditorSelection.OnQueryStatusKeyboardSelection);
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.SelectAll, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectAll), canExecuteRoutedEventHandler2, "KeySelectAll", "KeySelectAllDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveRightByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveRightByCharacter), canExecuteRoutedEventHandler, "KeyMoveRightByCharacter", "KeyMoveRightByCharacterDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveLeftByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveLeftByCharacter), canExecuteRoutedEventHandler, "KeyMoveLeftByCharacter", "KeyMoveLeftByCharacterDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveRightByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveRightByWord), canExecuteRoutedEventHandler, "KeyMoveRightByWord", "KeyMoveRightByWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveLeftByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveLeftByWord), canExecuteRoutedEventHandler, "KeyMoveLeftByWord", "KeyMoveLeftByWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByLine), canExecuteRoutedEventHandler, "KeyMoveDownByLine", "KeyMoveDownByLineDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByLine), canExecuteRoutedEventHandler, "KeyMoveUpByLine", "KeyMoveUpByLineDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByParagraph), canExecuteRoutedEventHandler, "KeyMoveDownByParagraph", "KeyMoveDownByParagraphDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByParagraph), canExecuteRoutedEventHandler, "KeyMoveUpByParagraph", "KeyMoveUpByParagraphDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveDownByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveDownByPage), canExecuteRoutedEventHandler, "KeyMoveDownByPage", "KeyMoveDownByPageDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveUpByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveUpByPage), canExecuteRoutedEventHandler, "KeyMoveUpByPage", "KeyMoveUpByPageDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToLineStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToLineStart), canExecuteRoutedEventHandler, "KeyMoveToLineStart", "KeyMoveToLineStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToLineEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToLineEnd), canExecuteRoutedEventHandler, "KeyMoveToLineEnd", "KeyMoveToLineEndDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToColumnStart, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyMoveToColumnStart", "KeyMoveToColumnStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToColumnEnd, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyMoveToColumnEnd", "KeyMoveToColumnEndDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToWindowTop, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyMoveToWindowTop", "KeyMoveToWindowTopDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToWindowBottom, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyMoveToWindowBottom", "KeyMoveToWindowBottomDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToDocumentStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToDocumentStart), canExecuteRoutedEventHandler, "KeyMoveToDocumentStart", "KeyMoveToDocumentStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MoveToDocumentEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnMoveToDocumentEnd), canExecuteRoutedEventHandler, "KeyMoveToDocumentEnd", "KeyMoveToDocumentEndDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectRightByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectRightByCharacter), canExecuteRoutedEventHandler2, "KeySelectRightByCharacter", "KeySelectRightByCharacterDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectLeftByCharacter, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectLeftByCharacter), canExecuteRoutedEventHandler2, "KeySelectLeftByCharacter", "KeySelectLeftByCharacterDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectRightByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectRightByWord), canExecuteRoutedEventHandler2, "KeySelectRightByWord", "KeySelectRightByWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectLeftByWord, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectLeftByWord), canExecuteRoutedEventHandler2, "KeySelectLeftByWord", "KeySelectLeftByWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByLine), canExecuteRoutedEventHandler2, "KeySelectDownByLine", "KeySelectDownByLineDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByLine, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByLine), canExecuteRoutedEventHandler2, "KeySelectUpByLine", "KeySelectUpByLineDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByParagraph), canExecuteRoutedEventHandler2, "KeySelectDownByParagraph", "KeySelectDownByParagraphDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByParagraph, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByParagraph), canExecuteRoutedEventHandler2, "KeySelectUpByParagraph", "KeySelectUpByParagraphDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectDownByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectDownByPage), canExecuteRoutedEventHandler2, "KeySelectDownByPage", "KeySelectDownByPageDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectUpByPage, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectUpByPage), canExecuteRoutedEventHandler2, "KeySelectUpByPage", "KeySelectUpByPageDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToLineStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToLineStart), canExecuteRoutedEventHandler2, "KeySelectToLineStart", "KeySelectToLineStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToLineEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToLineEnd), canExecuteRoutedEventHandler2, "KeySelectToLineEnd", "KeySelectToLineEndDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToColumnStart, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeySelectToColumnStart", "KeySelectToColumnStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToColumnEnd, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeySelectToColumnEnd", "KeySelectToColumnEndDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToWindowTop, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeySelectToWindowTop", "KeySelectToWindowTopDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToWindowBottom, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeySelectToWindowBottom", "KeySelectToWindowBottomDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToDocumentStart, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToDocumentStart), canExecuteRoutedEventHandler2, "KeySelectToDocumentStart", "KeySelectToDocumentStartDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SelectToDocumentEnd, new ExecutedRoutedEventHandler(TextEditorSelection.OnSelectToDocumentEnd), canExecuteRoutedEventHandler2, "KeySelectToDocumentEnd", "KeySelectToDocumentEndDisplayString");
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x000FE552 File Offset: 0x000FC752
		internal static void _ClearSuggestedX(TextEditor This)
		{
			This._suggestedX = double.NaN;
			This._NextLineAdvanceMovingPosition = null;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x000FE56C File Offset: 0x000FC76C
		internal static TextSegment GetNormalizedLineRange(ITextView textView, ITextPointer position)
		{
			TextSegment lineRange = textView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				ITextRange textRange = new TextRange(lineRange.Start, lineRange.End);
				return new TextSegment(textRange.Start, textRange.End);
			}
			if (!typeof(BlockUIContainer).IsAssignableFrom(position.ParentType))
			{
				return lineRange;
			}
			ITextPointer textPointer = position.CreatePointer(LogicalDirection.Forward);
			textPointer.MoveToElementEdge(ElementEdge.AfterStart);
			ITextPointer textPointer2 = position.CreatePointer(LogicalDirection.Backward);
			textPointer2.MoveToElementEdge(ElementEdge.BeforeEnd);
			lineRange = new TextSegment(textPointer, textPointer2);
			return lineRange;
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x000FE5F1 File Offset: 0x000FC7F1
		internal static bool IsPaginated(ITextView textview)
		{
			return !(textview is TextBoxView);
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x000FE600 File Offset: 0x000FC800
		private static void OnSelectAll(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock(true))
			{
				textEditor.Selection.Select(textEditor.TextContainer.Start, textEditor.TextContainer.End);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x000FE690 File Offset: 0x000FC890
		private static void OnMoveRightByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, false);
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000FE6D4 File Offset: 0x000FC8D4
		private static void OnMoveLeftByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, false);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x000FE718 File Offset: 0x000FC918
		private static void OnMoveRightByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.NavigateWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x000FE75C File Offset: 0x000FC95C
		private static void OnMoveLeftByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.NavigateWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x000FE7A0 File Offset: 0x000FC9A0
		private static void OnMoveDownByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.End.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, true, true);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
				Invariant.Assert(textEditor.Selection.IsEmpty);
				TextEditorSelection.AdjustCaretAtTableRowEnd(textEditor);
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num;
					int num2;
					ITextPointer positionAtNextLine = textEditor.TextView.GetPositionAtNextLine(textEditor.Selection.MovingPosition, suggestedX, 1, out num, out num2);
					Invariant.Assert(positionAtNextLine != null);
					if (num2 != 0)
					{
						TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
						textEditor.Selection.SetCaretToPosition(positionAtNextLine, positionAtNextLine.LogicalDirection, true, true);
					}
					else if (TextPointerBase.IsInAnchoredBlock(textPointer))
					{
						ITextPointer positionAtLineEnd = TextEditorSelection.GetPositionAtLineEnd(textPointer);
						ITextPointer nextInsertionPosition = positionAtLineEnd.GetNextInsertionPosition(LogicalDirection.Forward);
						textEditor.Selection.SetCaretToPosition((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineEnd, textPointer.LogicalDirection, true, true);
					}
					else if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleMoveByLineCompleted;
						textEditor.TextView.BringLineIntoViewAsync(positionAtNextLine, num, 1, textEditor);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x000FE93C File Offset: 0x000FCB3C
		private static void OnMoveUpByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.Start.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, true, true);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
				Invariant.Assert(textEditor.Selection.IsEmpty);
				TextEditorSelection.AdjustCaretAtTableRowEnd(textEditor);
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num;
					int num2;
					ITextPointer positionAtNextLine = textEditor.TextView.GetPositionAtNextLine(textEditor.Selection.MovingPosition, suggestedX, -1, out num, out num2);
					Invariant.Assert(positionAtNextLine != null);
					if (num2 != 0)
					{
						TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
						textEditor.Selection.SetCaretToPosition(positionAtNextLine, positionAtNextLine.LogicalDirection, true, true);
					}
					else if (TextPointerBase.IsInAnchoredBlock(textPointer))
					{
						ITextPointer positionAtLineStart = TextEditorSelection.GetPositionAtLineStart(textPointer);
						ITextPointer nextInsertionPosition = positionAtLineStart.GetNextInsertionPosition(LogicalDirection.Backward);
						textEditor.Selection.SetCaretToPosition((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineStart, textPointer.LogicalDirection, true, true);
					}
					else if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleMoveByLineCompleted;
						textEditor.TextView.BringLineIntoViewAsync(positionAtNextLine, num, -1, textEditor);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x000FEAD8 File Offset: 0x000FCCD8
		private static void OnMoveDownByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, false, false);
				}
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				textPointer.MoveToPosition(textRange.End);
				if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
				{
					textRange.SelectParagraph(textPointer);
					textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
				}
				else
				{
					textEditor.Selection.SetCaretToPosition(textRange.End, LogicalDirection.Backward, false, false);
				}
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x000FEBD8 File Offset: 0x000FCDD8
		private static void OnMoveUpByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, false, false);
				}
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				if (textEditor.Selection.Start.CompareTo(textRange.Start) > 0)
				{
					textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
				}
				else
				{
					textPointer.MoveToPosition(textRange.Start);
					if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward))
					{
						textRange.SelectParagraph(textPointer);
						textEditor.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Backward, false, false);
					}
				}
			}
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x000FECF0 File Offset: 0x000FCEF0
		private static void OnMoveDownByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.End.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
					textEditor.Selection.SetCaretToPosition(endInner, endInner.LogicalDirection, true, true);
				}
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
					if (num == 0.0)
					{
						if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
							Point suggestedOffset;
							int num2;
							ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), 1, out suggestedOffset, out num2);
							double x = suggestedOffset.X;
							Invariant.Assert(textPointer2 != null);
							if (num2 != 0)
							{
								TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
								textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
							}
							else if (TextEditorSelection.IsPaginated(textEditor.TextView))
							{
								textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleMoveByPageCompleted;
								textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, 1, textEditor);
							}
						}
					}
					else
					{
						Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
						Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Top + num);
						ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
						if (textPointer2 == null)
						{
							return;
						}
						if (textPointer2.CompareTo(textPointer) <= 0)
						{
							textPointer2 = textEditor.TextContainer.End;
							TextEditorSelection._ClearSuggestedX(textEditor);
						}
						ScrollBar.PageDownCommand.Execute(null, textEditor.TextView.RenderScope);
						textEditor.TextView.RenderScope.UpdateLayout();
						textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x000FEF2C File Offset: 0x000FD12C
		private static void OnMoveUpByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.Selection.Start.ValidateLayout())
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.IsEmpty)
				{
					ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
					textEditor.Selection.SetCaretToPosition(startInner, startInner.LogicalDirection, true, true);
				}
				ITextPointer textPointer;
				double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
				if (textPointer != null)
				{
					double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
					if (num == 0.0)
					{
						if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
							Point suggestedOffset;
							int num2;
							ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), -1, out suggestedOffset, out num2);
							double x = suggestedOffset.X;
							Invariant.Assert(textPointer2 != null);
							if (num2 != 0)
							{
								TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
								textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
							}
							else if (TextEditorSelection.IsPaginated(textEditor.TextView))
							{
								textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleMoveByPageCompleted;
								textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, -1, textEditor);
							}
						}
					}
					else
					{
						Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
						Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Bottom - num);
						ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
						if (textPointer2 == null)
						{
							return;
						}
						if (textPointer2.CompareTo(textPointer) >= 0)
						{
							textPointer2 = textEditor.TextContainer.Start;
							TextEditorSelection._ClearSuggestedX(textEditor);
						}
						ScrollBar.PageUpCommand.Execute(null, textEditor.TextView.RenderScope);
						textEditor.TextView.RenderScope.UpdateLayout();
						textEditor.Selection.SetCaretToPosition(textPointer2, textPointer2.LogicalDirection, true, false);
					}
					TextEditorTyping._BreakTypingSequence(textEditor);
					TextEditorSelection.ClearSpringloadFormatting(textEditor);
				}
			}
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x000FF168 File Offset: 0x000FD368
		private static void OnMoveToLineStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer startInner = TextEditorSelection.GetStartInner(textEditor);
			if (!startInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, startInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer frozenPointer = normalizedLineRange.Start.GetFrozenPointer(LogicalDirection.Forward);
				textEditor.Selection.SetCaretToPosition(frozenPointer, LogicalDirection.Forward, true, true);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x000FF21C File Offset: 0x000FD41C
		private static void OnMoveToLineEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer endInner = TextEditorSelection.GetEndInner(textEditor);
			if (!endInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, endInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				LogicalDirection logicalDirection = TextPointerBase.IsNextToPlainLineBreak(normalizedLineRange.End, LogicalDirection.Backward) ? LogicalDirection.Forward : LogicalDirection.Backward;
				ITextPointer frozenPointer = normalizedLineRange.End.GetFrozenPointer(logicalDirection);
				textEditor.Selection.SetCaretToPosition(frozenPointer, logicalDirection, true, true);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x000FF2E8 File Offset: 0x000FD4E8
		private static void OnMoveToDocumentStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor.Selection.SetCaretToPosition(textEditor.TextContainer.Start, LogicalDirection.Forward, false, false);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x000FF370 File Offset: 0x000FD570
		private static void OnMoveToDocumentEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor.Selection.SetCaretToPosition(textEditor.TextContainer.End, LogicalDirection.Backward, false, false);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x000FF3F8 File Offset: 0x000FD5F8
		private static void OnSelectRightByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, true);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x000FF43C File Offset: 0x000FD63C
		private static void OnSelectLeftByCharacter(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.MoveToCharacterLogicalDirection(textEditor, direction, true);
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000FF480 File Offset: 0x000FD680
		private static void OnSelectRightByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Backward : LogicalDirection.Forward;
			TextEditorSelection.ExtendWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000FF4C4 File Offset: 0x000FD6C4
		private static void OnSelectLeftByWord(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			LogicalDirection direction = TextEditorSelection.IsFlowDirectionRightToLeftThenTopToBottom(textEditor) ? LogicalDirection.Forward : LogicalDirection.Backward;
			TextEditorSelection.ExtendWordLogicalDirection(textEditor, direction);
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000FF508 File Offset: 0x000FD708
		private static void OnSelectDownByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.ExtendToNextTableRow(LogicalDirection.Forward))
				{
					ITextPointer textPointer;
					double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
					if (textPointer == null)
					{
						return;
					}
					if (textEditor._NextLineAdvanceMovingPosition != null && textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor._NextLineAdvanceMovingPosition, textEditor);
						textEditor._NextLineAdvanceMovingPosition = null;
					}
					else
					{
						ITextPointer textPointer2 = TextEditorSelection.AdjustPositionAtTableRowEnd(textPointer);
						double num;
						int num2;
						textPointer2 = textEditor.TextView.GetPositionAtNextLine(textPointer2, suggestedX, 1, out num, out num2);
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
							TextEditorSelection.AdjustMovingPositionForSelectDownByLine(textEditor, textPointer2, textPointer, num);
						}
						else if (TextPointerBase.IsInAnchoredBlock(textPointer))
						{
							ITextPointer positionAtLineEnd = TextEditorSelection.GetPositionAtLineEnd(textPointer);
							ITextPointer nextInsertionPosition = positionAtLineEnd.GetNextInsertionPosition(LogicalDirection.Forward);
							TextEditorSelection.ExtendSelectionAndBringIntoView((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineEnd, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleSelectByLineCompleted;
							textEditor.TextView.BringLineIntoViewAsync(textPointer2, num, 1, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = textPointer;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = false;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineEnd(textPointer2), textEditor);
						}
					}
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000FF690 File Offset: 0x000FD890
		private static void AdjustMovingPositionForSelectDownByLine(TextEditor This, ITextPointer newMovingPosition, ITextPointer originalMovingPosition, double suggestedX)
		{
			int num = newMovingPosition.CompareTo(originalMovingPosition);
			if (num > 0 || (num == 0 && newMovingPosition.LogicalDirection != originalMovingPosition.LogicalDirection))
			{
				if (TextPointerBase.IsNextToAnyBreak(newMovingPosition, LogicalDirection.Forward) || newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward) == null)
				{
					double absoluteXOffset = TextEditorSelection.GetAbsoluteXOffset(This.TextView, newMovingPosition);
					FlowDirection scopingParagraphFlowDirection = TextEditorSelection.GetScopingParagraphFlowDirection(newMovingPosition);
					FlowDirection flowDirection = This.UiScope.FlowDirection;
					if ((scopingParagraphFlowDirection == flowDirection && absoluteXOffset < suggestedX) || (scopingParagraphFlowDirection != flowDirection && absoluteXOffset > suggestedX))
					{
						newMovingPosition = newMovingPosition.GetInsertionPosition(LogicalDirection.Forward);
						newMovingPosition = newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward);
						if (newMovingPosition == null)
						{
							newMovingPosition = originalMovingPosition.TextContainer.End;
						}
						newMovingPosition = newMovingPosition.GetFrozenPointer(LogicalDirection.Backward);
					}
				}
				TextEditorSelection.ExtendSelectionAndBringIntoView(newMovingPosition, This);
				return;
			}
			if (This._NextLineAdvanceMovingPosition == null)
			{
				This._NextLineAdvanceMovingPosition = originalMovingPosition;
				This._IsNextLineAdvanceMovingPositionAtDocumentHead = false;
			}
			newMovingPosition = TextEditorSelection.GetPositionAtLineEnd(originalMovingPosition);
			if (newMovingPosition.GetNextInsertionPosition(LogicalDirection.Forward) == null)
			{
				newMovingPosition = newMovingPosition.TextContainer.End;
			}
			TextEditorSelection.ExtendSelectionAndBringIntoView(newMovingPosition, This);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000FF770 File Offset: 0x000FD970
		private static void OnSelectUpByLine(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (!textEditor.Selection.ExtendToNextTableRow(LogicalDirection.Backward))
				{
					ITextPointer textPointer;
					double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
					if (textPointer == null)
					{
						return;
					}
					if (textEditor._NextLineAdvanceMovingPosition != null && !textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor._NextLineAdvanceMovingPosition, textEditor);
						textEditor._NextLineAdvanceMovingPosition = null;
					}
					else
					{
						ITextPointer textPointer2 = TextEditorSelection.AdjustPositionAtTableRowEnd(textPointer);
						double num;
						int num2;
						textPointer2 = textEditor.TextView.GetPositionAtNextLine(textPointer2, suggestedX, -1, out num, out num2);
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, num);
							int num3 = textPointer2.CompareTo(textPointer);
							if (num3 < 0 || (num3 == 0 && textPointer2.LogicalDirection != textPointer.LogicalDirection))
							{
								TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
							}
							else
							{
								TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(textPointer), textEditor);
							}
						}
						else if (TextPointerBase.IsInAnchoredBlock(textPointer))
						{
							ITextPointer positionAtLineStart = TextEditorSelection.GetPositionAtLineStart(textPointer);
							ITextPointer nextInsertionPosition = positionAtLineStart.GetNextInsertionPosition(LogicalDirection.Backward);
							TextEditorSelection.ExtendSelectionAndBringIntoView((nextInsertionPosition != null) ? nextInsertionPosition : positionAtLineStart, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringLineIntoViewCompleted += TextEditorSelection.HandleSelectByLineCompleted;
							textEditor.TextView.BringLineIntoViewAsync(textPointer2, num, -1, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = textPointer;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = true;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(textPointer2), textEditor);
						}
					}
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000FF928 File Offset: 0x000FDB28
		private static void OnSelectDownByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				textPointer.MoveToPosition(textRange.End);
				if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
				{
					textRange.SelectParagraph(textPointer);
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
				}
				else
				{
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.End, textEditor);
				}
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000FF9EC File Offset: 0x000FDBEC
		private static void OnSelectUpByParagraph(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				ITextRange textRange = new TextRange(textPointer, textPointer);
				textRange.SelectParagraph(textPointer);
				if (textPointer.CompareTo(textRange.Start) > 0)
				{
					TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
				}
				else
				{
					textPointer.MoveToPosition(textRange.Start);
					if (textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward))
					{
						textRange.SelectParagraph(textPointer);
						TextEditorSelection.ExtendSelectionAndBringIntoView(textRange.Start, textEditor);
					}
				}
			}
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000FFAC0 File Offset: 0x000FDCC0
		private static void OnSelectDownByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer;
			double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
			if (textPointer == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
				if (num == 0.0)
				{
					if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
						Point suggestedOffset;
						int num2;
						ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), 1, out suggestedOffset, out num2);
						double x = suggestedOffset.X;
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleSelectByPageCompleted;
							textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, 1, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2.TextContainer.End, textEditor);
						}
					}
				}
				else
				{
					Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
					Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Top + num);
					ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
					if (textPointer2 == null)
					{
						return;
					}
					if (textPointer2.CompareTo(textPointer) <= 0)
					{
						textPointer2 = textEditor.TextContainer.End;
					}
					TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
					ScrollBar.PageDownCommand.Execute(null, textEditor.TextView.RenderScope);
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000FFC98 File Offset: 0x000FDE98
		private static void OnSelectUpByPage(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer;
			double suggestedX = TextEditorSelection.GetSuggestedX(textEditor, out textPointer);
			if (textPointer == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
				if (num == 0.0)
				{
					if (TextEditorSelection.IsPaginated(textEditor.TextView))
					{
						double suggestedYFromPosition = TextEditorSelection.GetSuggestedYFromPosition(textEditor, textPointer);
						Point suggestedOffset;
						int num2;
						ITextPointer textPointer2 = textEditor.TextView.GetPositionAtNextPage(textPointer, new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), suggestedYFromPosition), -1, out suggestedOffset, out num2);
						double x = suggestedOffset.X;
						Invariant.Assert(textPointer2 != null);
						if (num2 != 0)
						{
							TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, x);
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
						}
						else if (TextEditorSelection.IsPaginated(textEditor.TextView))
						{
							textEditor.TextView.BringPageIntoViewCompleted += TextEditorSelection.HandleSelectByPageCompleted;
							textEditor.TextView.BringPageIntoViewAsync(textPointer2, suggestedOffset, -1, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2.TextContainer.Start, textEditor);
						}
					}
				}
				else
				{
					Rect rectangleFromTextPosition = textEditor.TextView.GetRectangleFromTextPosition(textPointer);
					Point point = new Point(TextEditorSelection.GetViewportXOffset(textEditor.TextView, suggestedX), rectangleFromTextPosition.Bottom - num);
					ITextPointer textPointer2 = textEditor.TextView.GetTextPositionFromPoint(point, true);
					if (textPointer2 == null)
					{
						return;
					}
					if (textPointer2.CompareTo(textPointer) >= 0)
					{
						textPointer2 = textEditor.TextContainer.Start;
					}
					TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer2, textEditor);
					ScrollBar.PageUpCommand.Execute(null, textEditor.TextView.RenderScope);
				}
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000FFE70 File Offset: 0x000FE070
		private static void OnSelectToLineStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer movingPositionInner = TextEditorSelection.GetMovingPositionInner(textEditor);
			if (!movingPositionInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, movingPositionInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(normalizedLineRange.Start.CreatePointer(LogicalDirection.Forward), textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000FFF18 File Offset: 0x000FE118
		private static void OnSelectToLineEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer movingPositionInner = TextEditorSelection.GetMovingPositionInner(textEditor);
			if (!movingPositionInner.ValidateLayout())
			{
				return;
			}
			TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, movingPositionInner);
			if (normalizedLineRange.IsNull)
			{
				return;
			}
			if (normalizedLineRange.End.CompareTo(textEditor.Selection.End) < 0)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer = normalizedLineRange.End;
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Forward) || TextPointerBase.IsNextToRichLineBreak(textPointer, LogicalDirection.Forward))
				{
					if (textEditor.Selection.AnchorPosition.ValidateLayout())
					{
						TextSegment normalizedLineRange2 = TextEditorSelection.GetNormalizedLineRange(textEditor.TextView, textEditor.Selection.AnchorPosition);
						if (!normalizedLineRange.IsNull && normalizedLineRange2.Start.CompareTo(textEditor.Selection.AnchorPosition) == 0)
						{
							textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
						}
					}
				}
				else if (TextPointerBase.IsNextToParagraphBreak(textPointer, LogicalDirection.Forward) && TextPointerBase.IsNextToParagraphBreak(textEditor.Selection.AnchorPosition, LogicalDirection.Backward))
				{
					ITextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Forward);
					if (nextInsertionPosition == null)
					{
						textPointer = textPointer.TextContainer.End;
					}
					else
					{
						textPointer = nextInsertionPosition;
					}
				}
				textPointer = textPointer.GetFrozenPointer(LogicalDirection.Backward);
				TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x00100094 File Offset: 0x000FE294
		private static void OnSelectToDocumentStart(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor.TextContainer.Start, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x00100114 File Offset: 0x000FE314
		private static void OnSelectToDocumentEnd(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection.ExtendSelectionAndBringIntoView(textEditor.TextContainer.End, textEditor);
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x00100194 File Offset: 0x000FE394
		private static void HandleMoveByLineCompleted(object sender, BringLineIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringLineIntoViewCompleted -= TextEditorSelection.HandleMoveByLineCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedX);
					textEditor.Selection.SetCaretToPosition(e.NewPosition, e.NewPosition.LogicalDirection, true, true);
				}
			}
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0010024C File Offset: 0x000FE44C
		private static void HandleMoveByPageCompleted(object sender, BringPageIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPageIntoViewCompleted -= TextEditorSelection.HandleMoveByPageCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedOffset.X);
					textEditor.Selection.SetCaretToPosition(e.NewPosition, e.NewPosition.LogicalDirection, true, true);
				}
			}
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0010030C File Offset: 0x000FE50C
		private static void HandleSelectByLineCompleted(object sender, BringLineIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringLineIntoViewCompleted -= TextEditorSelection.HandleSelectByLineCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedX);
					int num = e.NewPosition.CompareTo(e.Position);
					if (e.Count < 0)
					{
						if (num < 0 || (num == 0 && e.NewPosition.LogicalDirection != e.Position.LogicalDirection))
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
						}
						else
						{
							if (textEditor._NextLineAdvanceMovingPosition == null)
							{
								textEditor._NextLineAdvanceMovingPosition = e.Position;
								textEditor._IsNextLineAdvanceMovingPositionAtDocumentHead = true;
							}
							TextEditorSelection.ExtendSelectionAndBringIntoView(TextEditorSelection.GetPositionAtLineStart(e.NewPosition), textEditor);
						}
					}
					else
					{
						TextEditorSelection.AdjustMovingPositionForSelectDownByLine(textEditor, e.NewPosition, e.Position, e.NewSuggestedX);
					}
				}
			}
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0010043C File Offset: 0x000FE63C
		private static void HandleSelectByPageCompleted(object sender, BringPageIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPageIntoViewCompleted -= TextEditorSelection.HandleSelectByPageCompleted;
			if (e != null && !e.Cancelled && e.Error == null)
			{
				TextEditor textEditor = e.UserState as TextEditor;
				if (textEditor == null || !textEditor._IsEnabled)
				{
					return;
				}
				TextEditorTyping._FlushPendingInputItems(textEditor);
				using (textEditor.Selection.DeclareChangeBlock())
				{
					TextEditorSelection.UpdateSuggestedXOnColumnOrPageBoundary(textEditor, e.NewSuggestedOffset.X);
					int num = e.NewPosition.CompareTo(e.Position);
					if (e.Count < 0)
					{
						if (num < 0)
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
						}
						else
						{
							TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition.TextContainer.Start, textEditor);
						}
					}
					else if (num > 0)
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition, textEditor);
					}
					else
					{
						TextEditorSelection.ExtendSelectionAndBringIntoView(e.NewPosition.TextContainer.End, textEditor);
					}
				}
			}
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x00100554 File Offset: 0x000FE754
		private static void OnQueryStatusKeyboardSelection(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0010057C File Offset: 0x000FE77C
		private static void OnQueryStatusCaretNavigation(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnNYICommand(object source, ExecutedRoutedEventArgs args)
		{
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x001005B4 File Offset: 0x000FE7B4
		private static void ClearSpringloadFormatting(TextEditor This)
		{
			if (This.Selection is TextSelection)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x001005D4 File Offset: 0x000FE7D4
		private static bool IsFlowDirectionRightToLeftThenTopToBottom(TextEditor textEditor)
		{
			Invariant.Assert(textEditor != null);
			ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
			while (TextSchema.IsFormattingType(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
			}
			FlowDirection flowDirection = (FlowDirection)textPointer.GetValue(FlowDocument.FlowDirectionProperty);
			return flowDirection == FlowDirection.RightToLeft;
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x00100628 File Offset: 0x000FE828
		private static void MoveToCharacterLogicalDirection(TextEditor textEditor, LogicalDirection direction, bool extend)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				if (extend)
				{
					textEditor.Selection.ExtendToNextInsertionPosition(direction);
					TextEditorSelection.BringIntoView(textEditor.Selection.MovingPosition, textEditor);
				}
				else
				{
					ITextPointer textPointer = (direction == LogicalDirection.Forward) ? textEditor.Selection.End : textEditor.Selection.Start;
					if (textEditor.Selection.IsEmpty)
					{
						textPointer = textPointer.GetNextInsertionPosition(direction);
					}
					if (textPointer != null)
					{
						LogicalDirection direction2 = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
						textPointer = textPointer.GetInsertionPosition(direction2);
						textEditor.Selection.SetCaretToPosition(textPointer, direction2, false, false);
					}
				}
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x00100704 File Offset: 0x000FE904
		private static void NavigateWordLogicalDirection(TextEditor textEditor, LogicalDirection direction)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				if (direction == LogicalDirection.Forward)
				{
					if (!textEditor.Selection.IsEmpty && TextPointerBase.IsAtWordBoundary(textEditor.Selection.End, LogicalDirection.Forward))
					{
						textEditor.Selection.SetCaretToPosition(textEditor.Selection.End, LogicalDirection.Backward, false, false);
					}
					else
					{
						ITextPointer textPointer = textEditor.Selection.End.CreatePointer();
						TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Forward);
						textEditor.Selection.SetCaretToPosition(textPointer, LogicalDirection.Backward, false, false);
					}
				}
				else if (!textEditor.Selection.IsEmpty && TextPointerBase.IsAtWordBoundary(textEditor.Selection.Start, LogicalDirection.Forward))
				{
					textEditor.Selection.SetCaretToPosition(textEditor.Selection.Start, LogicalDirection.Forward, false, false);
				}
				else
				{
					ITextPointer textPointer2 = textEditor.Selection.Start.CreatePointer();
					TextPointerBase.MoveToNextWordBoundary(textPointer2, LogicalDirection.Backward);
					textEditor.Selection.SetCaretToPosition(textPointer2, LogicalDirection.Forward, false, false);
				}
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x00100854 File Offset: 0x000FEA54
		private static void ExtendWordLogicalDirection(TextEditor textEditor, LogicalDirection direction)
		{
			Invariant.Assert(textEditor != null);
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
				ITextPointer textPointer = textEditor.Selection.MovingPosition.CreatePointer();
				TextPointerBase.MoveToNextWordBoundary(textPointer, direction);
				textPointer.SetLogicalDirection((direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
				TextEditorSelection.ExtendSelectionAndBringIntoView(textPointer, textEditor);
				textEditor.Selection.OnCaretNavigation();
				TextEditorSelection._ClearSuggestedX(textEditor);
				TextEditorTyping._BreakTypingSequence(textEditor);
				TextEditorSelection.ClearSpringloadFormatting(textEditor);
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x001008F8 File Offset: 0x000FEAF8
		private static double GetSuggestedX(TextEditor This, out ITextPointer innerMovingPosition)
		{
			innerMovingPosition = TextEditorSelection.GetMovingPositionInner(This);
			if (!innerMovingPosition.ValidateLayout())
			{
				innerMovingPosition = null;
				return double.NaN;
			}
			if (double.IsNaN(This._suggestedX))
			{
				This._suggestedX = TextEditorSelection.GetAbsoluteXOffset(This.TextView, innerMovingPosition);
				if (This.Selection.MovingPosition.CompareTo(innerMovingPosition) > 0)
				{
					double num = (double)innerMovingPosition.GetValue(TextElement.FontSizeProperty) * 0.5;
					FlowDirection scopingParagraphFlowDirection = TextEditorSelection.GetScopingParagraphFlowDirection(innerMovingPosition);
					FlowDirection flowDirection = This.UiScope.FlowDirection;
					if (scopingParagraphFlowDirection != flowDirection)
					{
						num = -num;
					}
					This._suggestedX += num;
				}
			}
			return This._suggestedX;
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x001009A4 File Offset: 0x000FEBA4
		private static double GetSuggestedYFromPosition(TextEditor This, ITextPointer position)
		{
			double result = double.NaN;
			if (position != null)
			{
				result = This.TextView.GetRectangleFromTextPosition(position).Y;
			}
			return result;
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x001009D4 File Offset: 0x000FEBD4
		private static void UpdateSuggestedXOnColumnOrPageBoundary(TextEditor This, double newSuggestedX)
		{
			if (This._suggestedX != newSuggestedX)
			{
				This._suggestedX = newSuggestedX;
			}
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x001009E8 File Offset: 0x000FEBE8
		private static ITextPointer GetMovingPositionInner(TextEditor This)
		{
			ITextPointer textPointer = This.Selection.MovingPosition;
			if (!(textPointer is DocumentSequenceTextPointer) && !(textPointer is FixedTextPointer) && textPointer.LogicalDirection == LogicalDirection.Backward && This.Selection.Start.CompareTo(textPointer) < 0 && TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward))
			{
				textPointer = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
				if (TextPointerBase.IsNextToPlainLineBreak(textPointer, LogicalDirection.Backward))
				{
					textPointer = textPointer.GetFrozenPointer(LogicalDirection.Forward);
				}
			}
			else if (TextPointerBase.IsAfterLastParagraph(textPointer))
			{
				textPointer = textPointer.GetInsertionPosition(textPointer.LogicalDirection);
			}
			return textPointer;
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x00100A67 File Offset: 0x000FEC67
		private static ITextPointer GetStartInner(TextEditor This)
		{
			if (!This.Selection.IsEmpty)
			{
				return This.Selection.Start.GetFrozenPointer(LogicalDirection.Forward);
			}
			return This.Selection.Start;
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x00100A94 File Offset: 0x000FEC94
		private static ITextPointer GetEndInner(TextEditor This)
		{
			ITextPointer textPointer = This.Selection.End;
			if (textPointer.CompareTo(This.Selection.MovingPosition) == 0)
			{
				textPointer = TextEditorSelection.GetMovingPositionInner(This);
			}
			return textPointer;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x00100AC8 File Offset: 0x000FECC8
		private static ITextPointer GetPositionAtLineStart(ITextPointer position)
		{
			TextSegment lineRange = position.TextContainer.TextView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				return lineRange.Start;
			}
			return position;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x00100AFC File Offset: 0x000FECFC
		private static ITextPointer GetPositionAtLineEnd(ITextPointer position)
		{
			TextSegment lineRange = position.TextContainer.TextView.GetLineRange(position);
			if (!lineRange.IsNull)
			{
				return lineRange.End;
			}
			return position;
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x00100B2D File Offset: 0x000FED2D
		private static void ExtendSelectionAndBringIntoView(ITextPointer position, TextEditor textEditor)
		{
			textEditor.Selection.ExtendToPosition(position);
			TextEditorSelection.BringIntoView(position, textEditor);
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x00100B44 File Offset: 0x000FED44
		private static void BringIntoView(ITextPointer position, TextEditor textEditor)
		{
			double num = (double)textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
			if (num == 0.0 && textEditor.TextView != null && textEditor.TextView.IsValid && !textEditor.TextView.Contains(position) && TextEditorSelection.IsPaginated(textEditor.TextView))
			{
				textEditor.TextView.BringPositionIntoViewAsync(position, textEditor);
			}
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x00100BB0 File Offset: 0x000FEDB0
		private static void AdjustCaretAtTableRowEnd(TextEditor This)
		{
			if (This.Selection.IsEmpty && TextPointerBase.IsAtRowEnd(This.Selection.Start))
			{
				ITextPointer nextInsertionPosition = This.Selection.Start.GetNextInsertionPosition(LogicalDirection.Backward);
				if (nextInsertionPosition != null)
				{
					This.Selection.SetCaretToPosition(nextInsertionPosition, LogicalDirection.Forward, false, false);
				}
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x00100C00 File Offset: 0x000FEE00
		private static ITextPointer AdjustPositionAtTableRowEnd(ITextPointer position)
		{
			if (TextPointerBase.IsAtRowEnd(position))
			{
				ITextPointer nextInsertionPosition = position.GetNextInsertionPosition(LogicalDirection.Backward);
				if (nextInsertionPosition != null)
				{
					position = nextInsertionPosition;
				}
			}
			return position;
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x00100C24 File Offset: 0x000FEE24
		private static FlowDirection GetScopingParagraphFlowDirection(ITextPointer position)
		{
			ITextPointer textPointer = position.CreatePointer();
			while (typeof(Inline).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			return (FlowDirection)textPointer.GetValue(FrameworkElement.FlowDirectionProperty);
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x00100C68 File Offset: 0x000FEE68
		private static double GetAbsoluteXOffset(ITextView textview, ITextPointer position)
		{
			double num = textview.GetRectangleFromTextPosition(position).X;
			if (textview is TextBoxView)
			{
				IScrollInfo scrollInfo = textview as IScrollInfo;
				if (scrollInfo != null)
				{
					num += scrollInfo.HorizontalOffset;
				}
			}
			return num;
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x00100CA4 File Offset: 0x000FEEA4
		private static double GetViewportXOffset(ITextView textview, double suggestedX)
		{
			if (textview is TextBoxView)
			{
				IScrollInfo scrollInfo = textview as IScrollInfo;
				if (scrollInfo != null)
				{
					suggestedX -= scrollInfo.HorizontalOffset;
				}
			}
			return suggestedX;
		}
	}
}
