using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003FD RID: 1021
	internal static class TextEditorTyping
	{
		// Token: 0x060038F1 RID: 14577 RVA: 0x001014A4 File Offset: 0x000FF6A4
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Keyboard.PreviewKeyDownEvent, new KeyEventHandler(TextEditorTyping.OnPreviewKeyDown));
				EventManager.RegisterClassHandler(controlType, Keyboard.KeyDownEvent, new KeyEventHandler(TextEditorTyping.OnKeyDown));
				EventManager.RegisterClassHandler(controlType, Keyboard.KeyUpEvent, new KeyEventHandler(TextEditorTyping.OnKeyUp));
				EventManager.RegisterClassHandler(controlType, TextCompositionManager.TextInputEvent, new TextCompositionEventHandler(TextEditorTyping.OnTextInput));
			}
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorTyping.OnEnterBreak);
			ExecutedRoutedEventHandler executedRoutedEventHandler2 = new ExecutedRoutedEventHandler(TextEditorTyping.OnSpace);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusNYI);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler2 = new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusEnterBreak);
			EventManager.RegisterClassHandler(controlType, Mouse.MouseMoveEvent, new MouseEventHandler(TextEditorTyping.OnMouseMove), true);
			EventManager.RegisterClassHandler(controlType, Mouse.MouseLeaveEvent, new MouseEventHandler(TextEditorTyping.OnMouseLeave), true);
			CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.CorrectionList, new ExecutedRoutedEventHandler(TextEditorTyping.OnCorrectionList), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusCorrectionList), "KeyCorrectionList", "KeyCorrectionListDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleInsert, new ExecutedRoutedEventHandler(TextEditorTyping.OnToggleInsert), canExecuteRoutedEventHandler, "KeyToggleInsert", "KeyToggleInsertDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Delete, new ExecutedRoutedEventHandler(TextEditorTyping.OnDelete), canExecuteRoutedEventHandler, "KeyDelete", "KeyDeleteDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteNextWord, new ExecutedRoutedEventHandler(TextEditorTyping.OnDeleteNextWord), canExecuteRoutedEventHandler, "KeyDeleteNextWord", "KeyDeleteNextWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeletePreviousWord, new ExecutedRoutedEventHandler(TextEditorTyping.OnDeletePreviousWord), canExecuteRoutedEventHandler, "KeyDeletePreviousWord", "KeyDeletePreviousWordDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.EnterParagraphBreak, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeyEnterParagraphBreak", "KeyEnterParagraphBreakDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.EnterLineBreak, executedRoutedEventHandler, canExecuteRoutedEventHandler2, "KeyEnterLineBreak", "KeyEnterLineBreakDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.TabForward, new ExecutedRoutedEventHandler(TextEditorTyping.OnTabForward), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusTabForward), "KeyTabForward", "KeyTabForwardDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.TabBackward, new ExecutedRoutedEventHandler(TextEditorTyping.OnTabBackward), new CanExecuteRoutedEventHandler(TextEditorTyping.OnQueryStatusTabBackward), "KeyTabBackward", "KeyTabBackwardDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Space, executedRoutedEventHandler2, canExecuteRoutedEventHandler, "KeySpace", "KeySpaceDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ShiftSpace, executedRoutedEventHandler2, canExecuteRoutedEventHandler, "KeyShiftSpace", "KeyShiftSpaceDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.Backspace, new ExecutedRoutedEventHandler(TextEditorTyping.OnBackspace), canExecuteRoutedEventHandler, KeyGesture.CreateFromResourceStrings(SR.Get("KeyBackspace"), SR.Get("KeyBackspaceDisplayString")), KeyGesture.CreateFromResourceStrings(SR.Get("KeyShiftBackspace"), SR.Get("KeyShiftBackspaceDisplayString")));
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0010172C File Offset: 0x000FF92C
		internal static void _AddInputLanguageChangedEventHandler(TextEditor This)
		{
			Invariant.Assert(This._dispatcher == null);
			This._dispatcher = Dispatcher.CurrentDispatcher;
			Invariant.Assert(This._dispatcher != null);
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.InputLanguageChangeEventHandlerCount == 0)
			{
				InputLanguageManager.Current.InputLanguageChanged += TextEditorTyping.OnInputLanguageChanged;
				Dispatcher.CurrentDispatcher.ShutdownFinished += TextEditorTyping.OnDispatcherShutdownFinished;
			}
			TextEditorThreadLocalStore textEditorThreadLocalStore = threadLocalStore;
			int inputLanguageChangeEventHandlerCount = textEditorThreadLocalStore.InputLanguageChangeEventHandlerCount;
			textEditorThreadLocalStore.InputLanguageChangeEventHandlerCount = inputLanguageChangeEventHandlerCount + 1;
		}

		// Token: 0x060038F3 RID: 14579 RVA: 0x001017AC File Offset: 0x000FF9AC
		internal static void _RemoveInputLanguageChangedEventHandler(TextEditor This)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			TextEditorThreadLocalStore textEditorThreadLocalStore = threadLocalStore;
			int inputLanguageChangeEventHandlerCount = textEditorThreadLocalStore.InputLanguageChangeEventHandlerCount;
			textEditorThreadLocalStore.InputLanguageChangeEventHandlerCount = inputLanguageChangeEventHandlerCount - 1;
			if (threadLocalStore.InputLanguageChangeEventHandlerCount == 0)
			{
				InputLanguageManager.Current.InputLanguageChanged -= TextEditorTyping.OnInputLanguageChanged;
				Dispatcher.CurrentDispatcher.ShutdownFinished -= TextEditorTyping.OnDispatcherShutdownFinished;
			}
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x00101803 File Offset: 0x000FFA03
		internal static void _BreakTypingSequence(TextEditor This)
		{
			This._typingUndoUnit = null;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0010180C File Offset: 0x000FFA0C
		internal static void _FlushPendingInputItems(TextEditor This)
		{
			if (This.TextView != null)
			{
				This.TextView.ThrottleBackgroundTasksForUserInput();
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.PendingInputItems != null)
			{
				try
				{
					for (int i = 0; i < threadLocalStore.PendingInputItems.Count; i++)
					{
						((TextEditorTyping.InputItem)threadLocalStore.PendingInputItems[i]).Do();
						threadLocalStore.PureControlShift = false;
					}
				}
				finally
				{
					threadLocalStore.PendingInputItems.Clear();
				}
			}
			threadLocalStore.PureControlShift = false;
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x00101894 File Offset: 0x000FFA94
		internal static void _ShowCursor()
		{
			if (TextEditor._ThreadLocalStore.HideCursor)
			{
				TextEditor._ThreadLocalStore.HideCursor = false;
				SafeNativeMethods.ShowCursor(true);
			}
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x001018B4 File Offset: 0x000FFAB4
		internal static void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.ImeProcessed)
			{
				return;
			}
			RichTextBox richTextBox = sender as RichTextBox;
			if (richTextBox == null)
			{
				return;
			}
			TextEditor textEditor = richTextBox.TextEditor;
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (e.IsRepeat)
			{
				return;
			}
			if (textEditor.TextStore == null || textEditor.TextStore.IsComposing)
			{
				return;
			}
			if (richTextBox.Selection.IsEmpty)
			{
				return;
			}
			textEditor.SetText(textEditor.Selection, string.Empty, InputLanguageManager.Current.CurrentInputLanguage);
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0010194C File Offset: 0x000FFB4C
		internal static void OnKeyDown(object sender, KeyEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (e.IsRepeat)
			{
				return;
			}
			textEditor.CloseToolTip();
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			threadLocalStore.PureControlShift = false;
			if (textEditor.TextView != null && !textEditor.UiScope.IsMouseCaptured)
			{
				if ((e.Key == Key.RightShift || e.Key == Key.LeftShift) && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					threadLocalStore.PureControlShift = true;
					return;
				}
				if ((e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl) && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					threadLocalStore.PureControlShift = true;
					return;
				}
				if (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
				{
					TextEditorTyping.UpdateHyperlinkCursor(textEditor);
				}
			}
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x00101A50 File Offset: 0x000FFC50
		internal static void OnKeyUp(object sender, KeyEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || (textEditor.IsReadOnly && !textEditor.IsReadOnlyCaretVisible) || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			Key key = e.Key;
			if (key - Key.LeftShift > 1)
			{
				if (key - Key.LeftCtrl > 1)
				{
					return;
				}
				TextEditorTyping.UpdateHyperlinkCursor(textEditor);
			}
			else if (TextEditor._ThreadLocalStore.PureControlShift && (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
			{
				TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.KeyUpInputItem(textEditor, e.Key, e.KeyboardDevice.Modifiers));
				return;
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x00101AE4 File Offset: 0x000FFCE4
		internal static void OnTextInput(object sender, TextCompositionEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			FrameworkTextComposition frameworkTextComposition = e.TextComposition as FrameworkTextComposition;
			if (frameworkTextComposition == null && (e.Text == null || e.Text.Length == 0))
			{
				return;
			}
			e.Handled = true;
			if (textEditor.TextView != null)
			{
				textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			}
			if (frameworkTextComposition != null)
			{
				if (frameworkTextComposition.Owner == textEditor.TextStore)
				{
					textEditor.TextStore.UpdateCompositionText(frameworkTextComposition);
					return;
				}
				if (frameworkTextComposition.Owner == textEditor.ImmComposition)
				{
					textEditor.ImmComposition.UpdateCompositionText(frameworkTextComposition);
					return;
				}
			}
			else
			{
				KeyboardDevice keyboardDevice = e.Device as KeyboardDevice;
				TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.TextInputItem(textEditor, e.Text, keyboardDevice != null && keyboardDevice.IsKeyToggled(Key.Insert)));
			}
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x00101BC0 File Offset: 0x000FFDC0
		private static void OnQueryStatusCorrectionList(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextStore != null)
			{
				args.CanExecute = textEditor.TextStore.QueryRangeOrReconvertSelection(false);
				return;
			}
			args.CanExecute = false;
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x00101BFC File Offset: 0x000FFDFC
		private static void OnCorrectionList(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextStore != null)
			{
				textEditor.TextStore.QueryRangeOrReconvertSelection(true);
			}
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x00101C2C File Offset: 0x000FFE2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnToggleInsert(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			textEditor._OvertypeMode = !textEditor._OvertypeMode;
			if (TextServicesLoader.ServicesInstalled && textEditor.TextStore != null)
			{
				TextServicesHost textServicesHost = TextServicesHost.Current;
				if (textServicesHost != null)
				{
					if (textEditor._OvertypeMode)
					{
						IInputElement inputElement = target as IInputElement;
						if (inputElement != null)
						{
							PresentationSource.AddSourceChangedHandler(inputElement, new SourceChangedEventHandler(TextEditorTyping.OnSourceChanged));
						}
						TextServicesHost.StartTransitoryExtension(textEditor.TextStore);
						return;
					}
					IInputElement inputElement2 = target as IInputElement;
					if (inputElement2 != null)
					{
						PresentationSource.RemoveSourceChangedHandler(inputElement2, new SourceChangedEventHandler(TextEditorTyping.OnSourceChanged));
					}
					TextServicesHost.StopTransitoryExtension(textEditor.TextStore);
				}
			}
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x00101CD2 File Offset: 0x000FFED2
		private static void OnSourceChanged(object sender, SourceChangedEventArgs args)
		{
			TextEditorTyping.OnToggleInsert(sender, null);
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x00101CDC File Offset: 0x000FFEDC
		private static void OnDelete(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
			TextEditorSelection._ClearSuggestedX(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer end = textEditor.Selection.End;
				if (textEditor.Selection.IsEmpty)
				{
					ITextPointer nextInsertionPosition = end.GetNextInsertionPosition(LogicalDirection.Forward);
					if (nextInsertionPosition == null)
					{
						return;
					}
					if (TextPointerBase.IsAtRowEnd(nextInsertionPosition))
					{
						return;
					}
					if (end is TextPointer && !TextEditorTyping.IsAtListItemStart(nextInsertionPosition) && TextEditorTyping.HandleDeleteWhenStructuralBoundaryIsCrossed(textEditor, (TextPointer)end, (TextPointer)nextInsertionPosition))
					{
						return;
					}
					textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Forward);
				}
				textEditor.Selection.Text = string.Empty;
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x00101DCC File Offset: 0x000FFFCC
		private static void OnBackspace(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(args.Source))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorSelection._ClearSuggestedX(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer = textEditor.Selection.Start;
				ITextPointer textPointer2 = null;
				if (textEditor.Selection.IsEmpty)
				{
					if (textEditor.AcceptsRichContent && TextEditorTyping.IsAtListItemStart(textPointer))
					{
						TextRangeEditLists.ConvertListItemsToParagraphs((TextRange)textEditor.Selection);
					}
					else if (textEditor.AcceptsRichContent && (TextEditorTyping.IsAtListItemChildStart(textPointer, false) || TextEditorTyping.IsAtIndentedParagraphOrBlockUIContainerStart(textEditor.Selection.Start)))
					{
						TextEditorLists.DecreaseIndentation(textEditor);
					}
					else
					{
						ITextPointer nextInsertionPosition = textPointer.GetNextInsertionPosition(LogicalDirection.Backward);
						if (nextInsertionPosition == null)
						{
							((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
							return;
						}
						if (TextPointerBase.IsAtRowEnd(nextInsertionPosition))
						{
							((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
							return;
						}
						if (textPointer is TextPointer && TextEditorTyping.HandleDeleteWhenStructuralBoundaryIsCrossed(textEditor, (TextPointer)textPointer, (TextPointer)nextInsertionPosition))
						{
							return;
						}
						textPointer = textPointer.GetFrozenPointer(LogicalDirection.Backward);
						if (textEditor.TextView != null && textPointer.HasValidLayout && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text)
						{
							textPointer2 = textEditor.TextView.GetBackspaceCaretUnitPosition(textPointer);
							Invariant.Assert(textPointer2 != null);
							if (textPointer2.CompareTo(textPointer) == 0)
							{
								textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Backward);
								textPointer2 = null;
							}
							else if (textPointer2.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text)
							{
								textEditor.Selection.Select(textEditor.Selection.End, textPointer2);
								textPointer2 = null;
							}
						}
						else
						{
							textEditor.Selection.ExtendToNextInsertionPosition(LogicalDirection.Backward);
						}
					}
				}
				if (textEditor.AcceptsRichContent)
				{
					((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
					((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
				}
				if (textPointer2 != null)
				{
					Invariant.Assert(textPointer2.CompareTo(textPointer) < 0);
					textPointer2.DeleteContentToPosition(textPointer);
				}
				else
				{
					textEditor.Selection.Text = string.Empty;
					textPointer = textEditor.Selection.Start;
				}
				textEditor.Selection.SetCaretToPosition(textPointer, LogicalDirection.Backward, false, true);
			}
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x00102008 File Offset: 0x00100208
		private static bool HandleDeleteWhenStructuralBoundaryIsCrossed(TextEditor This, TextPointer position, TextPointer deletePosition)
		{
			if (!TextRangeEditTables.IsTableStructureCrossed(position, deletePosition) && !TextEditorTyping.IsBlockUIContainerBoundaryCrossed(position, deletePosition) && !TextPointerBase.IsAtRowEnd(position))
			{
				return false;
			}
			LogicalDirection logicalDirection = (position.CompareTo(deletePosition) < 0) ? LogicalDirection.Forward : LogicalDirection.Backward;
			Block paragraphOrBlockUIContainer = position.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer != null)
			{
				if (logicalDirection == LogicalDirection.Forward)
				{
					if ((paragraphOrBlockUIContainer.NextBlock != null && paragraphOrBlockUIContainer is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer)) || (paragraphOrBlockUIContainer is BlockUIContainer && paragraphOrBlockUIContainer.IsEmpty))
					{
						paragraphOrBlockUIContainer.RepositionWithContent(null);
					}
				}
				else if ((paragraphOrBlockUIContainer.PreviousBlock != null && paragraphOrBlockUIContainer is Paragraph && Paragraph.HasNoTextContent((Paragraph)paragraphOrBlockUIContainer)) || (paragraphOrBlockUIContainer is BlockUIContainer && paragraphOrBlockUIContainer.IsEmpty))
				{
					paragraphOrBlockUIContainer.RepositionWithContent(null);
				}
			}
			This.Selection.SetCaretToPosition(deletePosition, logicalDirection, false, true);
			if (logicalDirection == LogicalDirection.Backward)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
			return true;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x001020DC File Offset: 0x001002DC
		private static bool IsAtIndentedParagraphOrBlockUIContainerStart(ITextPointer position)
		{
			if (position is TextPointer && TextPointerBase.IsAtParagraphOrBlockUIContainerStart(position))
			{
				Block paragraphOrBlockUIContainer = ((TextPointer)position).ParagraphOrBlockUIContainer;
				if (paragraphOrBlockUIContainer != null)
				{
					FlowDirection flowDirection = paragraphOrBlockUIContainer.FlowDirection;
					Thickness margin = paragraphOrBlockUIContainer.Margin;
					return (flowDirection == FlowDirection.LeftToRight && margin.Left > 0.0) || (flowDirection == FlowDirection.RightToLeft && margin.Right > 0.0) || (paragraphOrBlockUIContainer is Paragraph && ((Paragraph)paragraphOrBlockUIContainer).TextIndent > 0.0);
				}
			}
			return false;
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x00102168 File Offset: 0x00100368
		private static bool IsAtListItemStart(ITextPointer position)
		{
			if (typeof(ListItem).IsAssignableFrom(position.ParentType) && position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
			{
				return true;
			}
			while (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
			{
				Type parentType = position.ParentType;
				if (TextSchema.IsBlock(parentType))
				{
					if (TextSchema.IsParagraphOrBlockUIContainer(parentType))
					{
						position = position.GetNextContextPosition(LogicalDirection.Backward);
						if (position.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && typeof(ListItem).IsAssignableFrom(position.ParentType))
						{
							return true;
						}
					}
					return false;
				}
				position = position.GetNextContextPosition(LogicalDirection.Backward);
			}
			return false;
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x001021FC File Offset: 0x001003FC
		private static bool IsAtListItemChildStart(ITextPointer position, bool emptyChildOnly)
		{
			if (position.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart)
			{
				return false;
			}
			if (emptyChildOnly && position.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.ElementEnd)
			{
				return false;
			}
			ITextPointer textPointer = position.CreatePointer();
			while (textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && typeof(Inline).IsAssignableFrom(textPointer.ParentType))
			{
				textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			}
			if (textPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.ElementStart || !TextSchema.IsParagraphOrBlockUIContainer(textPointer.ParentType))
			{
				return false;
			}
			textPointer.MoveToElementEdge(ElementEdge.BeforeStart);
			return typeof(ListItem).IsAssignableFrom(textPointer.ParentType);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0010228A File Offset: 0x0010048A
		private static bool IsBlockUIContainerBoundaryCrossed(TextPointer position1, TextPointer position2)
		{
			return (position1.Parent is BlockUIContainer || position2.Parent is BlockUIContainer) && position1.Parent != position2.Parent;
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x001022BC File Offset: 0x001004BC
		private static void OnDeleteNextWord(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer = textEditor.Selection.End.CreatePointer();
			if (textEditor.Selection.IsEmpty)
			{
				TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Forward);
			}
			if (TextRangeEditTables.IsTableStructureCrossed(textEditor.Selection.Start, textPointer))
			{
				return;
			}
			ITextRange textRange = new TextRange(textEditor.Selection.Start, textPointer);
			if (textRange.IsTableCellRange)
			{
				return;
			}
			if (!textRange.IsEmpty)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
					}
					textEditor.Selection.Select(textRange.Start, textRange.End);
					textEditor.Selection.Text = string.Empty;
				}
			}
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x001023BC File Offset: 0x001005BC
		private static void OnDeletePreviousWord(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			ITextPointer textPointer = textEditor.Selection.Start.CreatePointer();
			if (textEditor.Selection.IsEmpty)
			{
				TextPointerBase.MoveToNextWordBoundary(textPointer, LogicalDirection.Backward);
			}
			if (TextRangeEditTables.IsTableStructureCrossed(textPointer, textEditor.Selection.Start))
			{
				return;
			}
			ITextRange textRange = new TextRange(textPointer, textEditor.Selection.End);
			if (textRange.IsTableCellRange)
			{
				return;
			}
			if (!textRange.IsEmpty)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).ClearSpringloadFormatting();
						textEditor.Selection.Select(textRange.Start, textRange.End);
						((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
					}
					else
					{
						textEditor.Selection.Select(textRange.Start, textRange.End);
					}
					textEditor.Selection.Text = string.Empty;
				}
			}
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x001024E8 File Offset: 0x001006E8
		private static void OnQueryStatusEnterBreak(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				args.ContinueRouting = true;
				return;
			}
			if (textEditor.Selection.IsTableCellRange || !textEditor.AcceptsReturn)
			{
				args.ContinueRouting = true;
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0010253C File Offset: 0x0010073C
		private static void OnEnterBreak(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly)
			{
				return;
			}
			if (textEditor.Selection.IsTableCellRange || !textEditor.AcceptsReturn || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			using (textEditor.Selection.DeclareChangeBlock())
			{
				bool flag;
				if (textEditor.AcceptsRichContent && textEditor.Selection.Start is TextPointer)
				{
					flag = TextEditorTyping.HandleEnterBreakForRichText(textEditor, args.Command);
				}
				else
				{
					flag = TextEditorTyping.HandleEnterBreakForPlainText(textEditor);
				}
				if (flag)
				{
					textEditor.Selection.SetCaretToPosition(textEditor.Selection.End, LogicalDirection.Forward, false, false);
					TextEditorSelection._ClearSuggestedX(textEditor);
				}
			}
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x00102608 File Offset: 0x00100808
		private static bool HandleEnterBreakForRichText(TextEditor This, ICommand command)
		{
			bool flag = true;
			((TextSelection)This.Selection).SpringloadCurrentFormatting();
			if (!This.Selection.IsEmpty)
			{
				This.Selection.Text = string.Empty;
			}
			if (!TextEditorTyping.HandleEnterBreakWhenStructuralBoundaryIsCrossed(This, command))
			{
				TextPointer textPointer = ((TextSelection)This.Selection).End;
				if (command == EditingCommands.EnterParagraphBreak)
				{
					if (textPointer.HasNonMergeableInlineAncestor && !TextPointerBase.IsPositionAtNonMergeableInlineBoundary(textPointer))
					{
						flag = false;
					}
					else
					{
						textPointer = TextRangeEdit.InsertParagraphBreak(textPointer, true);
					}
				}
				else if (command == EditingCommands.EnterLineBreak)
				{
					textPointer = textPointer.InsertLineBreak();
				}
				if (flag)
				{
					This.Selection.Select(textPointer, textPointer);
				}
			}
			return flag;
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x001026A4 File Offset: 0x001008A4
		private static bool HandleEnterBreakForPlainText(TextEditor This)
		{
			bool result = true;
			string a = This._FilterText(Environment.NewLine, This.Selection);
			if (a != string.Empty)
			{
				This.Selection.Text = Environment.NewLine;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x001026E8 File Offset: 0x001008E8
		private static bool HandleEnterBreakWhenStructuralBoundaryIsCrossed(TextEditor This, ICommand command)
		{
			Invariant.Assert(This.Selection.Start is TextPointer);
			TextPointer textPointer = (TextPointer)This.Selection.Start;
			bool result = true;
			if (TextPointerBase.IsAtRowEnd(textPointer))
			{
				TextRange textRange = ((TextSelection)This.Selection).InsertRows(1);
				This.Selection.SetCaretToPosition(textRange.Start, LogicalDirection.Forward, false, false);
			}
			else if (This.Selection.IsEmpty && (TextPointerBase.IsInEmptyListItem(textPointer) || TextEditorTyping.IsAtListItemChildStart(textPointer, true)) && command == EditingCommands.EnterParagraphBreak)
			{
				TextEditorLists.DecreaseIndentation(This);
			}
			else if (TextPointerBase.IsBeforeFirstTable(textPointer) || TextPointerBase.IsAtBlockUIContainerStart(textPointer))
			{
				TextRangeEditTables.EnsureInsertionPosition(textPointer);
			}
			else if (TextPointerBase.IsAtBlockUIContainerEnd(textPointer))
			{
				TextPointer textPointer2 = TextRangeEditTables.EnsureInsertionPosition(textPointer);
				This.Selection.Select(textPointer2, textPointer2);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x001027B8 File Offset: 0x001009B8
		private static void OnFlowDirectionCommand(TextEditor This, Key key)
		{
			using (This.Selection.DeclareChangeBlock())
			{
				if (key == Key.LeftShift)
				{
					if (This.AcceptsRichContent && This.Selection is TextSelection)
					{
						((TextSelection)This.Selection).ApplyPropertyValue(FlowDocument.FlowDirectionProperty, FlowDirection.LeftToRight, true);
					}
					else
					{
						Invariant.Assert(This.UiScope != null);
						UIElementPropertyUndoUnit.Add(This.TextContainer, This.UiScope, FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
						This.UiScope.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
					}
				}
				else
				{
					Invariant.Assert(key == Key.RightShift);
					if (This.AcceptsRichContent && This.Selection is TextSelection)
					{
						((TextSelection)This.Selection).ApplyPropertyValue(FlowDocument.FlowDirectionProperty, FlowDirection.RightToLeft, true);
					}
					else
					{
						Invariant.Assert(This.UiScope != null);
						UIElementPropertyUndoUnit.Add(This.TextContainer, This.UiScope, FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
						This.UiScope.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft);
					}
				}
				((TextSelection)This.Selection).UpdateCaretState(CaretScrollMethod.Simple);
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x001028FC File Offset: 0x00100AFC
		private static void OnSpace(object sender, ExecutedRoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor._IsSourceInScope(e.OriginalSource))
			{
				return;
			}
			if (textEditor.TextStore != null && textEditor.TextStore.IsComposing)
			{
				return;
			}
			if (textEditor.ImmComposition != null && textEditor.ImmComposition.IsComposition)
			{
				return;
			}
			e.Handled = true;
			if (textEditor.TextView != null)
			{
				textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			}
			TextEditorTyping.ScheduleInput(textEditor, new TextEditorTyping.TextInputItem(textEditor, " ", !textEditor._OvertypeMode));
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x00102994 File Offset: 0x00100B94
		private static void OnQueryStatusTabForward(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
				return;
			}
			args.ContinueRouting = true;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x001029C4 File Offset: 0x00100BC4
		private static void OnQueryStatusTabBackward(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
				return;
			}
			args.ContinueRouting = true;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x001029F4 File Offset: 0x00100BF4
		private static void OnTabForward(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (TextEditorTyping.HandleTabInTables(textEditor, LogicalDirection.Forward))
			{
				return;
			}
			if (textEditor.AcceptsRichContent && (!textEditor.Selection.IsEmpty || TextPointerBase.IsAtParagraphOrBlockUIContainerStart(textEditor.Selection.Start)) && EditingCommands.IncreaseIndentation.CanExecute(null, (IInputElement)sender))
			{
				EditingCommands.IncreaseIndentation.Execute(null, (IInputElement)sender);
				return;
			}
			TextEditorTyping.DoTextInput(textEditor, "\t", !textEditor._OvertypeMode, true);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x00102A9C File Offset: 0x00100C9C
		private static void OnTabBackward(object sender, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.UiScope.IsKeyboardFocused)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (TextEditorTyping.HandleTabInTables(textEditor, LogicalDirection.Backward))
			{
				return;
			}
			if (textEditor.AcceptsRichContent && (!textEditor.Selection.IsEmpty || TextPointerBase.IsAtParagraphOrBlockUIContainerStart(textEditor.Selection.Start)) && EditingCommands.DecreaseIndentation.CanExecute(null, (IInputElement)sender))
			{
				EditingCommands.DecreaseIndentation.Execute(null, (IInputElement)sender);
				return;
			}
			TextEditorTyping.DoTextInput(textEditor, "\t", !textEditor._OvertypeMode, true);
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x00102B44 File Offset: 0x00100D44
		private static bool HandleTabInTables(TextEditor This, LogicalDirection direction)
		{
			if (!This.AcceptsRichContent)
			{
				return false;
			}
			if (This.Selection.IsTableCellRange)
			{
				This.Selection.SetCaretToPosition(This.Selection.Start, LogicalDirection.Backward, false, false);
				return true;
			}
			if (This.Selection.IsEmpty && TextPointerBase.IsAtRowEnd(This.Selection.End))
			{
				TableCell tableCell = null;
				TableRow tableRow = ((TextPointer)This.Selection.End).Parent as TableRow;
				Invariant.Assert(tableRow != null);
				TableRowGroup rowGroup = tableRow.RowGroup;
				int num = rowGroup.Rows.IndexOf(tableRow);
				if (direction == LogicalDirection.Forward)
				{
					if (num + 1 < rowGroup.Rows.Count)
					{
						tableCell = rowGroup.Rows[num + 1].Cells[0];
					}
				}
				else if (num > 0)
				{
					tableCell = rowGroup.Rows[num - 1].Cells[rowGroup.Rows[num - 1].Cells.Count - 1];
				}
				if (tableCell != null)
				{
					This.Selection.Select(tableCell.ContentStart, tableCell.ContentEnd);
				}
				return true;
			}
			TextElement textElement = ((TextPointer)This.Selection.Start).Parent as TextElement;
			while (textElement != null && !(textElement is TableCell))
			{
				textElement = (textElement.Parent as TextElement);
			}
			if (textElement is TableCell)
			{
				TableCell tableCell2 = (TableCell)textElement;
				TableRow row = tableCell2.Row;
				TableRowGroup rowGroup2 = row.RowGroup;
				int num2 = row.Cells.IndexOf(tableCell2);
				int num3 = rowGroup2.Rows.IndexOf(row);
				if (direction == LogicalDirection.Forward)
				{
					if (num2 + 1 < row.Cells.Count)
					{
						tableCell2 = row.Cells[num2 + 1];
					}
					else if (num3 + 1 < rowGroup2.Rows.Count)
					{
						tableCell2 = rowGroup2.Rows[num3 + 1].Cells[0];
					}
				}
				else if (num2 > 0)
				{
					tableCell2 = row.Cells[num2 - 1];
				}
				else if (num3 > 0)
				{
					tableCell2 = rowGroup2.Rows[num3 - 1].Cells[rowGroup2.Rows[num3 - 1].Cells.Count - 1];
				}
				Invariant.Assert(tableCell2 != null);
				This.Selection.Select(tableCell2.ContentStart, tableCell2.ContentEnd);
				return true;
			}
			return false;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x00102DC0 File Offset: 0x00100FC0
		private static void DoTextInput(TextEditor This, string textData, bool isInsertKeyToggled, bool acceptControlCharacters)
		{
			TextEditorTyping.HideCursor(This);
			if (!acceptControlCharacters)
			{
				for (int i = 0; i < textData.Length; i++)
				{
					if (char.IsControl(textData[i]))
					{
						textData = textData.Remove(i--, 1);
					}
				}
			}
			string text = This._FilterText(textData, This.Selection);
			if (text.Length == 0)
			{
				return;
			}
			TextEditorTyping.OpenTypingUndoUnit(This);
			UndoCloseAction closeAction = UndoCloseAction.Rollback;
			try
			{
				using (This.Selection.DeclareChangeBlock())
				{
					This.Selection.ApplyTypingHeuristics(This.AllowOvertype && This._OvertypeMode && text != "\t");
					This.SetSelectedText(text, InputLanguageManager.Current.CurrentInputLanguage);
					ITextPointer caretPosition = This.Selection.End.CreatePointer(LogicalDirection.Backward);
					This.Selection.SetCaretToPosition(caretPosition, LogicalDirection.Backward, true, true);
					closeAction = UndoCloseAction.Commit;
				}
			}
			finally
			{
				TextEditorTyping.CloseTypingUndoUnit(This, closeAction);
			}
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x00102EC0 File Offset: 0x001010C0
		private static void ScheduleInput(TextEditor This, TextEditorTyping.InputItem item)
		{
			if (!This.AcceptsRichContent || TextEditorTyping.IsMouseInputPending(This))
			{
				TextEditorTyping._FlushPendingInputItems(This);
				item.Do();
				return;
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (threadLocalStore.PendingInputItems == null)
			{
				threadLocalStore.PendingInputItems = new ArrayList(1);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(TextEditorTyping.BackgroundInputCallback), This);
			}
			threadLocalStore.PendingInputItems.Add(item);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x00102F2C File Offset: 0x0010112C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool IsMouseInputPending(TextEditor This)
		{
			bool result = false;
			IWin32Window win32Window = PresentationSource.CriticalFromVisual(This.UiScope) as IWin32Window;
			if (win32Window != null)
			{
				IntPtr intPtr = IntPtr.Zero;
				new UIPermission(UIPermissionWindow.AllWindows).Assert();
				try
				{
					intPtr = win32Window.Handle;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (intPtr != (IntPtr)0)
				{
					MSG msg = default(MSG);
					result = UnsafeNativeMethods.PeekMessage(ref msg, new HandleRef(null, intPtr), WindowMessage.WM_MOUSEMOVE, WindowMessage.WM_MOUSEHWHEEL, 0);
				}
			}
			return result;
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x00102FB0 File Offset: 0x001011B0
		private static object BackgroundInputCallback(object This)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			Invariant.Assert(This is TextEditor);
			Invariant.Assert(threadLocalStore.PendingInputItems != null);
			try
			{
				TextEditorTyping._FlushPendingInputItems((TextEditor)This);
			}
			finally
			{
				threadLocalStore.PendingInputItems = null;
			}
			return null;
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x00103008 File Offset: 0x00101208
		private static void OnDispatcherShutdownFinished(object sender, EventArgs args)
		{
			Dispatcher.CurrentDispatcher.ShutdownFinished -= TextEditorTyping.OnDispatcherShutdownFinished;
			InputLanguageManager.Current.InputLanguageChanged -= TextEditorTyping.OnInputLanguageChanged;
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			threadLocalStore.InputLanguageChangeEventHandlerCount = 0;
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x0010304E File Offset: 0x0010124E
		private static void OnInputLanguageChanged(object sender, InputLanguageEventArgs e)
		{
			TextSelection.OnInputLanguageChanged(e.NewLanguage);
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0010305C File Offset: 0x0010125C
		private static void OpenTypingUndoUnit(TextEditor This)
		{
			UndoManager undoManager = This._GetUndoManager();
			if (undoManager != null && undoManager.IsEnabled)
			{
				if (This._typingUndoUnit != null && undoManager.LastUnit == This._typingUndoUnit && !This._typingUndoUnit.Locked)
				{
					undoManager.Reopen(This._typingUndoUnit);
					return;
				}
				This._typingUndoUnit = new TextParentUndoUnit(This.Selection);
				undoManager.Open(This._typingUndoUnit);
			}
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x001030C8 File Offset: 0x001012C8
		private static void CloseTypingUndoUnit(TextEditor This, UndoCloseAction closeAction)
		{
			UndoManager undoManager = This._GetUndoManager();
			if (undoManager != null && undoManager.IsEnabled)
			{
				if (This._typingUndoUnit != null && undoManager.LastUnit == This._typingUndoUnit && !This._typingUndoUnit.Locked)
				{
					if (This._typingUndoUnit is TextParentUndoUnit)
					{
						((TextParentUndoUnit)This._typingUndoUnit).RecordRedoSelectionState();
					}
					undoManager.Close(This._typingUndoUnit, closeAction);
					return;
				}
			}
			else
			{
				This._typingUndoUnit = null;
			}
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0010313C File Offset: 0x0010133C
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x0010315B File Offset: 0x0010135B
		private static void OnMouseMove(object sender, MouseEventArgs e)
		{
			TextEditorTyping._ShowCursor();
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x0010315B File Offset: 0x0010135B
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			TextEditorTyping._ShowCursor();
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x00103162 File Offset: 0x00101362
		private static void HideCursor(TextEditor This)
		{
			if (!TextEditor._ThreadLocalStore.HideCursor && SystemParameters.MouseVanish && This.UiScope.IsMouseOver)
			{
				TextEditor._ThreadLocalStore.HideCursor = true;
				SafeNativeMethods.ShowCursor(false);
			}
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x00103198 File Offset: 0x00101398
		private static void UpdateHyperlinkCursor(TextEditor This)
		{
			if (This.UiScope is RichTextBox && This.TextView != null && This.TextView.IsValid)
			{
				TextPointer textPointer = (TextPointer)This.TextView.GetTextPositionFromPoint(Mouse.GetPosition(This.TextView.RenderScope), false);
				if (textPointer != null && textPointer.Parent is TextElement && TextSchema.HasHyperlinkAncestor((TextElement)textPointer.Parent))
				{
					Mouse.UpdateCursor();
				}
			}
		}

		// Token: 0x02000901 RID: 2305
		private abstract class InputItem
		{
			// Token: 0x060085C9 RID: 34249 RVA: 0x0024B111 File Offset: 0x00249311
			internal InputItem(TextEditor textEditor)
			{
				this._textEditor = textEditor;
			}

			// Token: 0x060085CA RID: 34250
			internal abstract void Do();

			// Token: 0x17001E33 RID: 7731
			// (get) Token: 0x060085CB RID: 34251 RVA: 0x0024B120 File Offset: 0x00249320
			protected TextEditor TextEditor
			{
				get
				{
					return this._textEditor;
				}
			}

			// Token: 0x040042FD RID: 17149
			private TextEditor _textEditor;
		}

		// Token: 0x02000902 RID: 2306
		private class TextInputItem : TextEditorTyping.InputItem
		{
			// Token: 0x060085CC RID: 34252 RVA: 0x0024B128 File Offset: 0x00249328
			internal TextInputItem(TextEditor textEditor, string text, bool isInsertKeyToggled) : base(textEditor)
			{
				this._text = text;
				this._isInsertKeyToggled = isInsertKeyToggled;
			}

			// Token: 0x060085CD RID: 34253 RVA: 0x0024B13F File Offset: 0x0024933F
			internal override void Do()
			{
				if (base.TextEditor.UiScope == null)
				{
					return;
				}
				TextEditorTyping.DoTextInput(base.TextEditor, this._text, this._isInsertKeyToggled, false);
			}

			// Token: 0x040042FE RID: 17150
			private readonly string _text;

			// Token: 0x040042FF RID: 17151
			private readonly bool _isInsertKeyToggled;
		}

		// Token: 0x02000903 RID: 2307
		private class KeyUpInputItem : TextEditorTyping.InputItem
		{
			// Token: 0x060085CE RID: 34254 RVA: 0x0024B167 File Offset: 0x00249367
			internal KeyUpInputItem(TextEditor textEditor, Key key, ModifierKeys modifiers) : base(textEditor)
			{
				this._key = key;
				this._modifiers = modifiers;
			}

			// Token: 0x060085CF RID: 34255 RVA: 0x0024B180 File Offset: 0x00249380
			internal override void Do()
			{
				if (base.TextEditor.UiScope == null)
				{
					return;
				}
				Key key = this._key;
				if (key != Key.LeftShift)
				{
					if (key == Key.RightShift)
					{
						if (TextSelection.IsBidiInputLanguageInstalled())
						{
							TextEditorTyping.OnFlowDirectionCommand(base.TextEditor, this._key);
							return;
						}
					}
					else
					{
						Invariant.Assert(false, "Unexpected key value!");
					}
					return;
				}
				TextEditorTyping.OnFlowDirectionCommand(base.TextEditor, this._key);
			}

			// Token: 0x04004300 RID: 17152
			private readonly Key _key;

			// Token: 0x04004301 RID: 17153
			private readonly ModifierKeys _modifiers;
		}
	}
}
