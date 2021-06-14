using System;
using System.Windows.Input;
using MS.Internal.Commands;

namespace System.Windows.Documents
{
	// Token: 0x020003FB RID: 1019
	internal static class TextEditorTables
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x00101214 File Offset: 0x000FF414
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(TextEditorTables.OnTableCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(TextEditorTables.OnQueryStatusNYI);
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertTable, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyInsertTable", "KeyInsertTableDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertRows, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyInsertRows", "KeyInsertRowsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.InsertColumns, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyInsertColumns", "KeyInsertColumnsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteRows, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyDeleteRows", "KeyDeleteRowsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.DeleteColumns, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyDeleteColumns", "KeyDeleteColumnsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.MergeCells, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeyMergeCells", "KeyMergeCellsDisplayString");
			CommandHelpers.RegisterCommandHandler(controlType, EditingCommands.SplitCell, executedRoutedEventHandler, canExecuteRoutedEventHandler, "KeySplitCell", "KeySplitCellDisplayString");
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x001012DC File Offset: 0x000FF4DC
		private static void OnTableCommand(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(target);
			if (textEditor == null || !textEditor._IsEnabled || textEditor.IsReadOnly || !textEditor.AcceptsRichContent || !(textEditor.Selection is TextSelection))
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorSelection._ClearSuggestedX(textEditor);
			if (args.Command == EditingCommands.InsertTable)
			{
				((TextSelection)textEditor.Selection).InsertTable(4, 4);
				return;
			}
			if (args.Command == EditingCommands.InsertRows)
			{
				((TextSelection)textEditor.Selection).InsertRows(1);
				return;
			}
			if (args.Command == EditingCommands.InsertColumns)
			{
				((TextSelection)textEditor.Selection).InsertColumns(1);
				return;
			}
			if (args.Command == EditingCommands.DeleteRows)
			{
				((TextSelection)textEditor.Selection).DeleteRows();
				return;
			}
			if (args.Command == EditingCommands.DeleteColumns)
			{
				((TextSelection)textEditor.Selection).DeleteColumns();
				return;
			}
			if (args.Command == EditingCommands.MergeCells)
			{
				((TextSelection)textEditor.Selection).MergeCells();
				return;
			}
			if (args.Command == EditingCommands.SplitCell)
			{
				((TextSelection)textEditor.Selection).SplitCell(1000, 1000);
			}
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x0010140C File Offset: 0x000FF60C
		private static void OnQueryStatusNYI(object target, CanExecuteRoutedEventArgs args)
		{
			if (TextEditor._GetTextEditor(target) == null)
			{
				return;
			}
			args.CanExecute = true;
		}
	}
}
