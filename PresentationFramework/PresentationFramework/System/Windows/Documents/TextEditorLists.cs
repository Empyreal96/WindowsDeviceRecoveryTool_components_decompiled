using System;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003F6 RID: 1014
	internal static class TextEditorLists
	{
		// Token: 0x06003870 RID: 14448 RVA: 0x000FCC30 File Offset: 0x000FAE30
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.RemoveListMarkers, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), "KeyRemoveListMarkers", "KeyRemoveListMarkersDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleBullets, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), "KeyToggleBullets", "KeyToggleBulletsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.ToggleNumbering, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusNYI), "KeyToggleNumbering", "KeyToggleNumberingDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.IncreaseIndentation, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusTab), "KeyIncreaseIndentation", "KeyIncreaseIndentationDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DecreaseIndentation, new ExecutedRoutedEventHandler(TextEditorLists.OnListCommand), new CanExecuteRoutedEventHandler(TextEditorLists.OnQueryStatusTab), "KeyDecreaseIndentation", "KeyDecreaseIndentationDisplayString");
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x000FCD20 File Offset: 0x000FAF20
		internal static void DecreaseIndentation(TextEditor This)
		{
			TextSelection textSelection = (TextSelection)This.Selection;
			ListItem listItem = TextPointerBase.GetListItem(textSelection.Start);
			ListItem immediateListItem = TextPointerBase.GetImmediateListItem(textSelection.Start);
			TextEditorLists.DecreaseIndentation(textSelection, listItem, immediateListItem);
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x000FCD5C File Offset: 0x000FAF5C
		private static TextEditor IsEnabledNotReadOnlyIsTextSegment(object sender)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor != null && textEditor._IsEnabled && !textEditor.IsReadOnly && !textEditor.Selection.IsTableCellRange)
			{
				return textEditor;
			}
			return null;
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x000FCD94 File Offset: 0x000FAF94
		private static void OnQueryStatusTab(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditorLists.IsEnabledNotReadOnlyIsTextSegment(sender);
			if (textEditor != null && textEditor.AcceptsTab)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000FCDBC File Offset: 0x000FAFBC
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x000FCDDC File Offset: 0x000FAFDC
		private static void OnListCommand(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!TextRangeEditLists.IsListOperationApplicable((TextSelection)textEditor.Selection))
			{
				return;
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				TextSelection textSelection = (TextSelection)textEditor.Selection;
				ListItem listItem = TextPointerBase.GetListItem(textSelection.Start);
				ListItem immediateListItem = TextPointerBase.GetImmediateListItem(textSelection.Start);
				List list = (listItem == null) ? null : ((List)listItem.Parent);
				TextEditorSelection._ClearSuggestedX(textEditor);
				if (args.Command == EditingCommands.ToggleBullets)
				{
					TextEditorLists.ToggleBullets(textSelection, listItem, immediateListItem, list);
				}
				else if (args.Command == EditingCommands.ToggleNumbering)
				{
					TextEditorLists.ToggleNumbering(textSelection, listItem, immediateListItem, list);
				}
				else if (args.Command == EditingCommands.RemoveListMarkers)
				{
					TextRangeEditLists.ConvertListItemsToParagraphs(textSelection);
				}
				else if (args.Command == EditingCommands.IncreaseIndentation)
				{
					TextEditorLists.IncreaseIndentation(textSelection, listItem, immediateListItem);
				}
				else if (args.Command == EditingCommands.DecreaseIndentation)
				{
					TextEditorLists.DecreaseIndentation(textSelection, listItem, immediateListItem);
				}
				else
				{
					Invariant.Assert(false);
				}
			}
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x000FCF18 File Offset: 0x000FB118
		private static void ToggleBullets(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem, List list)
		{
			if (immediateListItem != null && TextEditorLists.HasBulletMarker(list))
			{
				if (list.Parent is ListItem)
				{
					TextRangeEditLists.UnindentListItems(thisSelection);
					TextRangeEditLists.ConvertListItemsToParagraphs(thisSelection);
					return;
				}
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			else
			{
				if (immediateListItem != null)
				{
					list.MarkerStyle = TextMarkerStyle.Disc;
					return;
				}
				if (parentListItem != null)
				{
					TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
					TextRangeEditLists.IndentListItems(thisSelection);
					return;
				}
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
				return;
			}
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x000FCF78 File Offset: 0x000FB178
		private static void ToggleNumbering(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem, List list)
		{
			if (immediateListItem != null && TextEditorLists.HasNumericMarker(list))
			{
				if (list.Parent is ListItem)
				{
					TextRangeEditLists.UnindentListItems(thisSelection);
					TextRangeEditLists.ConvertListItemsToParagraphs(thisSelection);
					return;
				}
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			else
			{
				if (immediateListItem != null)
				{
					list.MarkerStyle = TextMarkerStyle.Decimal;
					return;
				}
				if (parentListItem != null)
				{
					TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
					TextRangeEditLists.IndentListItems(thisSelection);
					return;
				}
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
				return;
			}
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x000FCFDC File Offset: 0x000FB1DC
		private static void IncreaseIndentation(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem)
		{
			if (immediateListItem != null)
			{
				TextRangeEditLists.IndentListItems(thisSelection);
				return;
			}
			if (parentListItem != null)
			{
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Decimal);
				TextRangeEditLists.IndentListItems(thisSelection);
				return;
			}
			if (!thisSelection.IsEmpty)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
				return;
			}
			Block paragraphOrBlockUIContainer = thisSelection.Start.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer is BlockUIContainer)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
				return;
			}
			TextEditorLists.CreateImplicitParagraphIfNeededAndUpdateSelection(thisSelection);
			Paragraph paragraph = thisSelection.Start.Paragraph;
			Invariant.Assert(paragraph != null, "EnsureInsertionPosition must guarantee a position in text content");
			if (paragraph.TextIndent < 0.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 0.0, PropertyValueAction.SetValue);
				return;
			}
			if (paragraph.TextIndent < 20.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 20.0, PropertyValueAction.SetValue);
				return;
			}
			TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.IncreaseByAbsoluteValue);
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x000FD0E8 File Offset: 0x000FB2E8
		private static void DecreaseIndentation(TextSelection thisSelection, ListItem parentListItem, ListItem immediateListItem)
		{
			if (immediateListItem != null)
			{
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			if (parentListItem != null)
			{
				TextRangeEditLists.ConvertParagraphsToListItems(thisSelection, TextMarkerStyle.Disc);
				TextRangeEditLists.UnindentListItems(thisSelection);
				return;
			}
			if (!thisSelection.IsEmpty)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
				return;
			}
			Block paragraphOrBlockUIContainer = thisSelection.Start.ParagraphOrBlockUIContainer;
			if (paragraphOrBlockUIContainer is BlockUIContainer)
			{
				TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
				return;
			}
			TextEditorLists.CreateImplicitParagraphIfNeededAndUpdateSelection(thisSelection);
			Paragraph paragraph = thisSelection.Start.Paragraph;
			Invariant.Assert(paragraph != null, "EnsureInsertionPosition must guarantee a position in text content");
			if (paragraph.TextIndent > 20.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 20.0, PropertyValueAction.SetValue);
				return;
			}
			if (paragraph.TextIndent > 0.0)
			{
				TextRangeEdit.SetParagraphProperty(thisSelection.Start, thisSelection.End, Paragraph.TextIndentProperty, 0.0, PropertyValueAction.SetValue);
				return;
			}
			TextRangeEdit.IncrementParagraphLeadingMargin(thisSelection, 20.0, PropertyValueAction.DecreaseByAbsoluteValue);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x000FD1F4 File Offset: 0x000FB3F4
		private static void CreateImplicitParagraphIfNeededAndUpdateSelection(TextSelection thisSelection)
		{
			TextPointer textPointer = thisSelection.Start;
			if (TextPointerBase.IsAtPotentialParagraphPosition(textPointer))
			{
				textPointer = TextRangeEditTables.EnsureInsertionPosition(textPointer);
				thisSelection.Select(textPointer, textPointer);
			}
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000FD220 File Offset: 0x000FB420
		private static bool HasBulletMarker(List list)
		{
			if (list == null)
			{
				return false;
			}
			TextMarkerStyle markerStyle = list.MarkerStyle;
			return TextMarkerStyle.Disc <= markerStyle && markerStyle <= TextMarkerStyle.Box;
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000FD248 File Offset: 0x000FB448
		private static bool HasNumericMarker(List list)
		{
			if (list == null)
			{
				return false;
			}
			TextMarkerStyle markerStyle = list.MarkerStyle;
			return TextMarkerStyle.LowerRoman <= markerStyle && markerStyle <= TextMarkerStyle.Decimal;
		}
	}
}
