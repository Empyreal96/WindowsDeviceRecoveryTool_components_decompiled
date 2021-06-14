using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003FA RID: 1018
	internal static class TextEditorSpelling
	{
		// Token: 0x060038D4 RID: 14548 RVA: 0x00100CD0 File Offset: 0x000FEED0
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.CorrectSpellingError, new ExecutedRoutedEventHandler(TextEditorSpelling.OnCorrectSpellingError), new CanExecuteRoutedEventHandler(TextEditorSpelling.OnQueryStatusSpellingError));
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IgnoreSpellingError, new ExecutedRoutedEventHandler(TextEditorSpelling.OnIgnoreSpellingError), new CanExecuteRoutedEventHandler(TextEditorSpelling.OnQueryStatusSpellingError));
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00100D23 File Offset: 0x000FEF23
		internal static SpellingError GetSpellingErrorAtPosition(TextEditor This, ITextPointer position, LogicalDirection direction)
		{
			if (This.Speller != null)
			{
				return This.Speller.GetError(position, direction, true);
			}
			return null;
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x00100D40 File Offset: 0x000FEF40
		internal static SpellingError GetSpellingErrorAtSelection(TextEditor This)
		{
			if (This.Speller == null)
			{
				return null;
			}
			if (TextEditorSpelling.IsSelectionIgnoringErrors(This.Selection))
			{
				return null;
			}
			LogicalDirection logicalDirection = This.Selection.IsEmpty ? This.Selection.Start.LogicalDirection : LogicalDirection.Forward;
			char c;
			ITextPointer textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
			if (textPointer == null)
			{
				logicalDirection = ((logicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
				textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
			}
			else if (char.IsWhiteSpace(c))
			{
				if (This.Selection.IsEmpty)
				{
					logicalDirection = ((logicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
					textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
				}
				else
				{
					logicalDirection = LogicalDirection.Forward;
					textPointer = TextEditorSpelling.GetNextNonWhiteSpacePosition(This.Selection.Start, This.Selection.End);
					if (textPointer == null)
					{
						logicalDirection = LogicalDirection.Backward;
						textPointer = TextEditorSpelling.GetNextTextPosition(This.Selection.Start, null, logicalDirection, out c);
					}
				}
			}
			if (textPointer != null)
			{
				return This.Speller.GetError(textPointer, logicalDirection, false);
			}
			return null;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x00100E3F File Offset: 0x000FF03F
		internal static ITextPointer GetNextSpellingErrorPosition(TextEditor This, ITextPointer position, LogicalDirection direction)
		{
			if (This.Speller != null)
			{
				return This.Speller.GetNextSpellingErrorPosition(position, direction);
			}
			return null;
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x00100E58 File Offset: 0x000FF058
		private static void OnCorrectSpellingError(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			string text = args.Parameter as string;
			if (text == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			if (spellingErrorAtSelection == null)
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				ITextPointer textPointer;
				ITextPointer position;
				bool flag = TextEditorSpelling.IsErrorAtNonMergeableInlineEdge(spellingErrorAtSelection, out textPointer, out position);
				ITextPointer textPointer2;
				if (flag && textPointer is TextPointer)
				{
					((TextPointer)textPointer).DeleteTextInRun(textPointer.GetOffsetToPosition(position));
					textPointer.InsertTextInRun(text);
					textPointer2 = textPointer.CreatePointer(text.Length, LogicalDirection.Forward);
				}
				else
				{
					textEditor.Selection.Select(spellingErrorAtSelection.Start, spellingErrorAtSelection.End);
					if (textEditor.AcceptsRichContent)
					{
						((TextSelection)textEditor.Selection).SpringloadCurrentFormatting();
					}
					XmlLanguage xmlLanguage = (XmlLanguage)spellingErrorAtSelection.Start.GetValue(FrameworkElement.LanguageProperty);
					textEditor.SetSelectedText(text, xmlLanguage.GetSpecificCulture());
					textPointer2 = textEditor.Selection.End;
				}
				textEditor.Selection.Select(textPointer2, textPointer2);
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x00100F6C File Offset: 0x000FF16C
		private static bool IsErrorAtNonMergeableInlineEdge(SpellingError spellingError, out ITextPointer textStart, out ITextPointer textEnd)
		{
			bool result = false;
			textStart = spellingError.Start.CreatePointer(LogicalDirection.Backward);
			while (textStart.CompareTo(spellingError.End) < 0 && textStart.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text)
			{
				textStart.MoveToNextContextPosition(LogicalDirection.Forward);
			}
			textEnd = spellingError.End.CreatePointer();
			while (textEnd.CompareTo(spellingError.Start) > 0 && textEnd.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text)
			{
				textEnd.MoveToNextContextPosition(LogicalDirection.Backward);
			}
			if (textStart.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text || textStart.CompareTo(spellingError.End) == 0)
			{
				return false;
			}
			Invariant.Assert(textEnd.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.Text && textEnd.CompareTo(spellingError.Start) != 0);
			if ((TextPointerBase.IsAtNonMergeableInlineStart(textStart) || TextPointerBase.IsAtNonMergeableInlineEnd(textEnd)) && typeof(Run).IsAssignableFrom(textStart.ParentType) && textStart.HasEqualScope(textEnd))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0010105C File Offset: 0x000FF25C
		private static void OnIgnoreSpellingError(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			if (spellingErrorAtSelection == null)
			{
				return;
			}
			spellingErrorAtSelection.IgnoreAll();
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x00101088 File Offset: 0x000FF288
		private static void OnQueryStatusSpellingError(object target, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null)
			{
				return;
			}
			SpellingError spellingErrorAtSelection = TextEditorSpelling.GetSpellingErrorAtSelection(textEditor);
			args.CanExecute = (spellingErrorAtSelection != null);
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x001010B4 File Offset: 0x000FF2B4
		private static ITextPointer GetNextTextPosition(ITextPointer position, ITextPointer limit, LogicalDirection direction, out char character)
		{
			bool flag = false;
			character = '\0';
			while (position != null && !flag && (limit == null || position.CompareTo(limit) < 0))
			{
				switch (position.GetPointerContext(direction))
				{
				case TextPointerContext.Text:
				{
					char[] array = new char[1];
					position.GetTextInRun(direction, array, 0, 1);
					character = array[0];
					flag = true;
					continue;
				}
				case TextPointerContext.ElementStart:
				case TextPointerContext.ElementEnd:
					if (TextSchema.IsFormattingType(position.GetElementType(direction)))
					{
						position = position.CreatePointer(1);
						continue;
					}
					position = null;
					continue;
				}
				position = null;
			}
			return position;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0010113C File Offset: 0x000FF33C
		private static ITextPointer GetNextNonWhiteSpacePosition(ITextPointer position, ITextPointer limit)
		{
			Invariant.Assert(limit != null);
			while (position.CompareTo(limit) != 0)
			{
				char c;
				position = TextEditorSpelling.GetNextTextPosition(position, limit, LogicalDirection.Forward, out c);
				if (position == null || !char.IsWhiteSpace(c))
				{
					return position;
				}
				position = position.CreatePointer(1);
			}
			return null;
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x00101184 File Offset: 0x000FF384
		private static bool IsSelectionIgnoringErrors(ITextSelection selection)
		{
			bool flag = false;
			if (selection.Start is TextPointer)
			{
				flag = (((TextPointer)selection.Start).ParentBlock != ((TextPointer)selection.End).ParentBlock);
			}
			if (!flag)
			{
				flag = (selection.Start.GetOffsetToPosition(selection.End) >= 256);
			}
			if (!flag)
			{
				string text = selection.Text;
				int num = 0;
				while (num < text.Length && !flag)
				{
					flag = TextPointerBase.IsCharUnicodeNewLine(text[num]);
					num++;
				}
			}
			return flag;
		}
	}
}
