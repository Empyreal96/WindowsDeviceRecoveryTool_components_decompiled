using System;
using System.Windows.Input;

namespace System.Windows.Documents
{
	/// <summary>Provides a standard set of editing related commands.</summary>
	// Token: 0x02000340 RID: 832
	public static class EditingCommands
	{
		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleInsert" /> command, which toggles the typing mode between Insert and Overtype.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Insert" />.</returns>
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06002C40 RID: 11328 RVA: 0x000C8F92 File Offset: 0x000C7192
		public static RoutedUICommand ToggleInsert
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleInsert, "ToggleInsert");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.Delete" /> command, which requests that the current selection be deleted.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Delete" />.</returns>
		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000C8FA3 File Offset: 0x000C71A3
		public static RoutedUICommand Delete
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Delete, "Delete");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.Backspace" /> command, which requests that a backspace be entered at the current position or over the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Backspace" />.</returns>
		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x000C8FB4 File Offset: 0x000C71B4
		public static RoutedUICommand Backspace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Backspace, "Backspace");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.DeleteNextWord" /> command, which requests that the next word (relative to a current position) be deleted.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Delete" />.</returns>
		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x000C8FC5 File Offset: 0x000C71C5
		public static RoutedUICommand DeleteNextWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteNextWord, "DeleteNextWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.DeletePreviousWord" /> command, which requests that the previous word (relative to a current position) be deleted.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Backspace" />.</returns>
		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x000C8FD6 File Offset: 0x000C71D6
		public static RoutedUICommand DeletePreviousWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeletePreviousWord, "DeletePreviousWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.EnterParagraphBreak" /> command, which requests that a paragraph break be inserted at the current position or over the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Enter" />.</returns>
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x000C8FE7 File Offset: 0x000C71E7
		public static RoutedUICommand EnterParagraphBreak
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._EnterParagraphBreak, "EnterParagraphBreak");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.EnterLineBreak" /> command, which requests that a line break be inserted at the current position or over the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Enter" />.</returns>
		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x000C8FF8 File Offset: 0x000C71F8
		public static RoutedUICommand EnterLineBreak
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._EnterLineBreak, "EnterLineBreak");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.TabForward" /> command.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Tab" />.</returns>
		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002C47 RID: 11335 RVA: 0x000C9009 File Offset: 0x000C7209
		public static RoutedUICommand TabForward
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._TabForward, "TabForward");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.TabBackward" /> command.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Tab" />.</returns>
		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x000C901A File Offset: 0x000C721A
		public static RoutedUICommand TabBackward
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._TabBackward, "TabBackward");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveRightByCharacter" /> command, which requests that the caret move one character right.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Right" />.</returns>
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06002C49 RID: 11337 RVA: 0x000C902B File Offset: 0x000C722B
		public static RoutedUICommand MoveRightByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveRightByCharacter, "MoveRightByCharacter");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveLeftByCharacter" /> command, which requests that the caret move one character left.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Left" />.</returns>
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06002C4A RID: 11338 RVA: 0x000C903C File Offset: 0x000C723C
		public static RoutedUICommand MoveLeftByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveLeftByCharacter, "MoveLeftByCharacter");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveRightByWord" /> command, which requests that the caret move right by one word.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Right" />.</returns>
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06002C4B RID: 11339 RVA: 0x000C904D File Offset: 0x000C724D
		public static RoutedUICommand MoveRightByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveRightByWord, "MoveRightByWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveLeftByWord" /> command, which requests that the caret move one word left.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Left" />.</returns>
		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x000C905E File Offset: 0x000C725E
		public static RoutedUICommand MoveLeftByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveLeftByWord, "MoveLeftByWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveDownByLine" /> command, which requests that the caret move down by one line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Down" />.</returns>
		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x000C906F File Offset: 0x000C726F
		public static RoutedUICommand MoveDownByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByLine, "MoveDownByLine");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveUpByLine" /> command, which requests that the caret move up by one line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Up" />.</returns>
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x000C9080 File Offset: 0x000C7280
		public static RoutedUICommand MoveUpByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByLine, "MoveUpByLine");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveDownByParagraph" /> command, which requests that the caret move down by one paragraph.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Down" />.</returns>
		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x000C9091 File Offset: 0x000C7291
		public static RoutedUICommand MoveDownByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByParagraph, "MoveDownByParagraph");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveUpByParagraph" /> command, which requests that the caret move up by one paragraph.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Up" />.</returns>
		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x000C90A2 File Offset: 0x000C72A2
		public static RoutedUICommand MoveUpByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByParagraph, "MoveUpByParagraph");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveDownByPage" /> command, which requests that the caret move down by one page.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="PageDown" />.</returns>
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x000C90B3 File Offset: 0x000C72B3
		public static RoutedUICommand MoveDownByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByPage, "MoveDownByPage");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveUpByPage" /> command, which requests that the caret move up by one page.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="PageUp" />.</returns>
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x000C90C4 File Offset: 0x000C72C4
		public static RoutedUICommand MoveUpByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByPage, "MoveUpByPage");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveToLineStart" /> command, which requests that the caret move to the beginning of the current line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Home" />.</returns>
		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06002C53 RID: 11347 RVA: 0x000C90D5 File Offset: 0x000C72D5
		public static RoutedUICommand MoveToLineStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToLineStart, "MoveToLineStart");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveToLineEnd" /> command, which requests that the caret move to the end of the current line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="End" />.</returns>
		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x000C90E6 File Offset: 0x000C72E6
		public static RoutedUICommand MoveToLineEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToLineEnd, "MoveToLineEnd");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveToDocumentStart" /> command, which requests that the caret move to the very beginning of content.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Home" />.</returns>
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06002C55 RID: 11349 RVA: 0x000C90F7 File Offset: 0x000C72F7
		public static RoutedUICommand MoveToDocumentStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToDocumentStart, "MoveToDocumentStart");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.MoveToDocumentEnd" /> command, which requests that the caret move to the very end of content.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="End" />.</returns>
		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x000C9108 File Offset: 0x000C7308
		public static RoutedUICommand MoveToDocumentEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToDocumentEnd, "MoveToDocumentEnd");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectRightByCharacter" /> command, which requests that the current selection be expanded right by one character.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Right" />.</returns>
		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x000C9119 File Offset: 0x000C7319
		public static RoutedUICommand SelectRightByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectRightByCharacter, "SelectRightByCharacter");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectLeftByCharacter" /> command, which requests that the current selection be expanded left by one character.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Left" />.</returns>
		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x06002C58 RID: 11352 RVA: 0x000C912A File Offset: 0x000C732A
		public static RoutedUICommand SelectLeftByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectLeftByCharacter, "SelectLeftByCharacter");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectRightByWord" /> command, which requests that the current selection be expanded right by one word.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="Right" />.</returns>
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x000C913B File Offset: 0x000C733B
		public static RoutedUICommand SelectRightByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectRightByWord, "SelectRightByWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectLeftByWord" /> command, which requests that the current selection be expanded left by one word.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="Left" />.</returns>
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002C5A RID: 11354 RVA: 0x000C914C File Offset: 0x000C734C
		public static RoutedUICommand SelectLeftByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectLeftByWord, "SelectLeftByWord");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectDownByLine" /> command, which requests that the current selection be expanded down by one line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Down" />.</returns>
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002C5B RID: 11355 RVA: 0x000C915D File Offset: 0x000C735D
		public static RoutedUICommand SelectDownByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByLine, "SelectDownByLine");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectUpByLine" /> command, which requests that the current selection be expanded up by one line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Up" />.</returns>
		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002C5C RID: 11356 RVA: 0x000C916E File Offset: 0x000C736E
		public static RoutedUICommand SelectUpByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByLine, "SelectUpByLine");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectDownByParagraph" /> command, which requests that the current selection be expanded down by one paragraph.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="Down" />.</returns>
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002C5D RID: 11357 RVA: 0x000C917F File Offset: 0x000C737F
		public static RoutedUICommand SelectDownByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByParagraph, "SelectDownByParagraph");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectUpByParagraph" /> command, which requests that the current selection be expanded up by one paragraph.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="Up" />.</returns>
		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x000C9190 File Offset: 0x000C7390
		public static RoutedUICommand SelectUpByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByParagraph, "SelectUpByParagraph");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectDownByPage" /> command, which requests that the current selection be expanded down by one page.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="PageDown" />.</returns>
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06002C5F RID: 11359 RVA: 0x000C91A1 File Offset: 0x000C73A1
		public static RoutedUICommand SelectDownByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByPage, "SelectDownByPage");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectUpByPage" /> command, which requests that the current selection be expanded  up by one page.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="PageUp" />.</returns>
		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x000C91B2 File Offset: 0x000C73B2
		public static RoutedUICommand SelectUpByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByPage, "SelectUpByPage");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectToLineStart" /> command, which requests that the current selection be expanded to the beginning of the current line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="Home" />.</returns>
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x000C91C3 File Offset: 0x000C73C3
		public static RoutedUICommand SelectToLineStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToLineStart, "SelectToLineStart");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectToLineEnd" /> command, which requests that the current selection be expanded to the end of the current line.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Shift" />+<see langword="End" />.</returns>
		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000C91D4 File Offset: 0x000C73D4
		public static RoutedUICommand SelectToLineEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToLineEnd, "SelectToLineEnd");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectToDocumentStart" /> command, which requests that the current selection be expanded to the very beginning of content.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="Home" />.</returns>
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002C63 RID: 11363 RVA: 0x000C91E5 File Offset: 0x000C73E5
		public static RoutedUICommand SelectToDocumentStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToDocumentStart, "SelectToDocumentStart");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.SelectToDocumentEnd" /> command, which requests that the current selection be expanded to the very end of content.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="End" />.</returns>
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x000C91F6 File Offset: 0x000C73F6
		public static RoutedUICommand SelectToDocumentEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToDocumentEnd, "SelectToDocumentEnd");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleBold" /> command, which requests that <see cref="T:System.Windows.Documents.Bold" /> formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="B" />.</returns>
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000C9207 File Offset: 0x000C7407
		public static RoutedUICommand ToggleBold
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleBold, "ToggleBold");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleItalic" /> command, which requests that <see cref="T:System.Windows.Documents.Italic" /> formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="I" />.</returns>
		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x000C9218 File Offset: 0x000C7418
		public static RoutedUICommand ToggleItalic
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleItalic, "ToggleItalic");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleUnderline" /> command, which requests that <see cref="T:System.Windows.Documents.Underline" /> formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="U" />.</returns>
		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06002C67 RID: 11367 RVA: 0x000C9229 File Offset: 0x000C7429
		public static RoutedUICommand ToggleUnderline
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleUnderline, "ToggleUnderline");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleSubscript" /> command, which requests that subscript formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="OemPlus" />.</returns>
		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06002C68 RID: 11368 RVA: 0x000C923A File Offset: 0x000C743A
		public static RoutedUICommand ToggleSubscript
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSubscript, "ToggleSubscript");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleSuperscript" /> command, which requests that superscript formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="OemPlus" />.</returns>
		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06002C69 RID: 11369 RVA: 0x000C924B File Offset: 0x000C744B
		public static RoutedUICommand ToggleSuperscript
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSuperscript, "ToggleSuperscript");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.IncreaseFontSize" /> command, which requests that the font size for the current selection be increased by 1 point.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="OemCloseBrackets" />.</returns>
		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06002C6A RID: 11370 RVA: 0x000C925C File Offset: 0x000C745C
		public static RoutedUICommand IncreaseFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IncreaseFontSize, "IncreaseFontSize");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.DecreaseFontSize" /> command, which requests that the font size for the current selection be decreased by 1 point.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="OemOpenBrackets" />.</returns>
		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06002C6B RID: 11371 RVA: 0x000C926D File Offset: 0x000C746D
		public static RoutedUICommand DecreaseFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DecreaseFontSize, "DecreaseFontSize");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.AlignLeft" /> command, which requests that a selection of content be aligned left.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="L" />.</returns>
		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06002C6C RID: 11372 RVA: 0x000C927E File Offset: 0x000C747E
		public static RoutedUICommand AlignLeft
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignLeft, "AlignLeft");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.AlignCenter" /> command, which requests that the current paragraph or a selection of paragraphs be centered.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="E" />.</returns>
		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06002C6D RID: 11373 RVA: 0x000C928F File Offset: 0x000C748F
		public static RoutedUICommand AlignCenter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignCenter, "AlignCenter");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.AlignRight" /> command, which requests that a selection of content be aligned right.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="R" />.</returns>
		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06002C6E RID: 11374 RVA: 0x000C92A0 File Offset: 0x000C74A0
		public static RoutedUICommand AlignRight
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignRight, "AlignRight");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.AlignJustify" /> command, which requests that the current paragraph or a selection of paragraphs be justified.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="J" />.</returns>
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06002C6F RID: 11375 RVA: 0x000C92B1 File Offset: 0x000C74B1
		public static RoutedUICommand AlignJustify
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignJustify, "AlignJustify");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleBullets" /> command, which requests that unordered list (also referred to as bulleted list) formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="L" />.</returns>
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06002C70 RID: 11376 RVA: 0x000C92C2 File Offset: 0x000C74C2
		public static RoutedUICommand ToggleBullets
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleBullets, "ToggleBullets");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.ToggleNumbering" /> command, which requests that ordered list (also referred to as numbered list) formatting be toggled on the current selection.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="N" />.</returns>
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x000C92D3 File Offset: 0x000C74D3
		public static RoutedUICommand ToggleNumbering
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleNumbering, "ToggleNumbering");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.IncreaseIndentation" /> command, which requests that indentation for the current paragraph be increased by one tab stop.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="T" />.</returns>
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x000C92E4 File Offset: 0x000C74E4
		public static RoutedUICommand IncreaseIndentation
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IncreaseIndentation, "IncreaseIndentation");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.DecreaseIndentation" /> command, which requests that indentation for the current paragraph be decreased by one tab stop.</summary>
		/// <returns>The requested command.  The default key gesture for this command is <see langword="Ctrl" />+<see langword="Shift" />+<see langword="T" />.</returns>
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x000C92F5 File Offset: 0x000C74F5
		public static RoutedUICommand DecreaseIndentation
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DecreaseIndentation, "DecreaseIndentation");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.CorrectSpellingError" /> command, which requests that any misspelled word at the current position be corrected.</summary>
		/// <returns>The requested command.  This command has no default key gesture.</returns>
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000C9306 File Offset: 0x000C7506
		public static RoutedUICommand CorrectSpellingError
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._CorrectSpellingError, "CorrectSpellingError");
			}
		}

		/// <summary>Represents the <see cref="P:System.Windows.Documents.EditingCommands.IgnoreSpellingError" /> command, which requests that any instances of misspelled words at the current position or in the current selection be ignored.</summary>
		/// <returns>The requested command.  This command has no default key gesture.</returns>
		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002C75 RID: 11381 RVA: 0x000C9317 File Offset: 0x000C7517
		public static RoutedUICommand IgnoreSpellingError
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IgnoreSpellingError, "IgnoreSpellingError");
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002C76 RID: 11382 RVA: 0x000C9328 File Offset: 0x000C7528
		internal static RoutedUICommand Space
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Space, "Space");
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06002C77 RID: 11383 RVA: 0x000C9339 File Offset: 0x000C7539
		internal static RoutedUICommand ShiftSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ShiftSpace, "ShiftSpace");
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06002C78 RID: 11384 RVA: 0x000C934A File Offset: 0x000C754A
		internal static RoutedUICommand MoveToColumnStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToColumnStart, "MoveToColumnStart");
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x000C935B File Offset: 0x000C755B
		internal static RoutedUICommand MoveToColumnEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToColumnEnd, "MoveToColumnEnd");
			}
		}

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002C7A RID: 11386 RVA: 0x000C936C File Offset: 0x000C756C
		internal static RoutedUICommand MoveToWindowTop
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToWindowTop, "MoveToWindowTop");
			}
		}

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x06002C7B RID: 11387 RVA: 0x000C937D File Offset: 0x000C757D
		internal static RoutedUICommand MoveToWindowBottom
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToWindowBottom, "MoveToWindowBottom");
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x06002C7C RID: 11388 RVA: 0x000C938E File Offset: 0x000C758E
		internal static RoutedUICommand SelectToColumnStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToColumnStart, "SelectToColumnStart");
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x06002C7D RID: 11389 RVA: 0x000C939F File Offset: 0x000C759F
		internal static RoutedUICommand SelectToColumnEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToColumnEnd, "SelectToColumnEnd");
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06002C7E RID: 11390 RVA: 0x000C93B0 File Offset: 0x000C75B0
		internal static RoutedUICommand SelectToWindowTop
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToWindowTop, "SelectToWindowTop");
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x000C93C1 File Offset: 0x000C75C1
		internal static RoutedUICommand SelectToWindowBottom
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToWindowBottom, "SelectToWindowBottom");
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06002C80 RID: 11392 RVA: 0x000C93D2 File Offset: 0x000C75D2
		internal static RoutedUICommand ResetFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ResetFormat, "ResetFormat");
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x000C93E3 File Offset: 0x000C75E3
		internal static RoutedUICommand ToggleSpellCheck
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSpellCheck, "ToggleSpellCheck");
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06002C82 RID: 11394 RVA: 0x000C93F4 File Offset: 0x000C75F4
		internal static RoutedUICommand ApplyFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyFontSize, "ApplyFontSize");
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x000C9405 File Offset: 0x000C7605
		internal static RoutedUICommand ApplyFontFamily
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyFontFamily, "ApplyFontFamily");
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06002C84 RID: 11396 RVA: 0x000C9416 File Offset: 0x000C7616
		internal static RoutedUICommand ApplyForeground
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyForeground, "ApplyForeground");
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x000C9427 File Offset: 0x000C7627
		internal static RoutedUICommand ApplyBackground
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyBackground, "ApplyBackground");
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06002C86 RID: 11398 RVA: 0x000C9438 File Offset: 0x000C7638
		internal static RoutedUICommand ApplyInlineFlowDirectionRTL
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyInlineFlowDirectionRTL, "ApplyInlineFlowDirectionRTL");
			}
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x000C9449 File Offset: 0x000C7649
		internal static RoutedUICommand ApplyInlineFlowDirectionLTR
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyInlineFlowDirectionLTR, "ApplyInlineFlowDirectionLTR");
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000C945A File Offset: 0x000C765A
		internal static RoutedUICommand ApplySingleSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplySingleSpace, "ApplySingleSpace");
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000C946B File Offset: 0x000C766B
		internal static RoutedUICommand ApplyOneAndAHalfSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyOneAndAHalfSpace, "ApplyOneAndAHalfSpace");
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06002C8A RID: 11402 RVA: 0x000C947C File Offset: 0x000C767C
		internal static RoutedUICommand ApplyDoubleSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyDoubleSpace, "ApplyDoubleSpace");
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06002C8B RID: 11403 RVA: 0x000C948D File Offset: 0x000C768D
		internal static RoutedUICommand ApplyParagraphFlowDirectionRTL
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyParagraphFlowDirectionRTL, "ApplyParagraphFlowDirectionRTL");
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000C949E File Offset: 0x000C769E
		internal static RoutedUICommand ApplyParagraphFlowDirectionLTR
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyParagraphFlowDirectionLTR, "ApplyParagraphFlowDirectionLTR");
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x000C94AF File Offset: 0x000C76AF
		internal static RoutedUICommand CopyFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._CopyFormat, "CopyFormat");
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000C94C0 File Offset: 0x000C76C0
		internal static RoutedUICommand PasteFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._PasteFormat, "PasteFormat");
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002C8F RID: 11407 RVA: 0x000C94D1 File Offset: 0x000C76D1
		internal static RoutedUICommand RemoveListMarkers
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._RemoveListMarkers, "RemoveListMarkers");
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000C94E2 File Offset: 0x000C76E2
		internal static RoutedUICommand InsertTable
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertTable, "InsertTable");
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002C91 RID: 11409 RVA: 0x000C94F3 File Offset: 0x000C76F3
		internal static RoutedUICommand InsertRows
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertRows, "InsertRows");
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000C9504 File Offset: 0x000C7704
		internal static RoutedUICommand InsertColumns
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertColumns, "InsertColumns");
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000C9515 File Offset: 0x000C7715
		internal static RoutedUICommand DeleteRows
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteRows, "DeleteRows");
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000C9526 File Offset: 0x000C7726
		internal static RoutedUICommand DeleteColumns
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteColumns, "DeleteColumns");
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002C95 RID: 11413 RVA: 0x000C9537 File Offset: 0x000C7737
		internal static RoutedUICommand MergeCells
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MergeCells, "MergeCells");
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x000C9548 File Offset: 0x000C7748
		internal static RoutedUICommand SplitCell
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SplitCell, "SplitCell");
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000C955C File Offset: 0x000C775C
		private static RoutedUICommand EnsureCommand(ref RoutedUICommand command, string commandPropertyName)
		{
			object synchronize = EditingCommands._synchronize;
			lock (synchronize)
			{
				if (command == null)
				{
					command = new RoutedUICommand(commandPropertyName, commandPropertyName, typeof(EditingCommands));
				}
			}
			return command;
		}

		// Token: 0x04001CCD RID: 7373
		private static object _synchronize = new object();

		// Token: 0x04001CCE RID: 7374
		private static RoutedUICommand _ToggleInsert;

		// Token: 0x04001CCF RID: 7375
		private static RoutedUICommand _Delete;

		// Token: 0x04001CD0 RID: 7376
		private static RoutedUICommand _Backspace;

		// Token: 0x04001CD1 RID: 7377
		private static RoutedUICommand _DeleteNextWord;

		// Token: 0x04001CD2 RID: 7378
		private static RoutedUICommand _DeletePreviousWord;

		// Token: 0x04001CD3 RID: 7379
		private static RoutedUICommand _EnterParagraphBreak;

		// Token: 0x04001CD4 RID: 7380
		private static RoutedUICommand _EnterLineBreak;

		// Token: 0x04001CD5 RID: 7381
		private static RoutedUICommand _TabForward;

		// Token: 0x04001CD6 RID: 7382
		private static RoutedUICommand _TabBackward;

		// Token: 0x04001CD7 RID: 7383
		private static RoutedUICommand _Space;

		// Token: 0x04001CD8 RID: 7384
		private static RoutedUICommand _ShiftSpace;

		// Token: 0x04001CD9 RID: 7385
		private static RoutedUICommand _MoveRightByCharacter;

		// Token: 0x04001CDA RID: 7386
		private static RoutedUICommand _MoveLeftByCharacter;

		// Token: 0x04001CDB RID: 7387
		private static RoutedUICommand _MoveRightByWord;

		// Token: 0x04001CDC RID: 7388
		private static RoutedUICommand _MoveLeftByWord;

		// Token: 0x04001CDD RID: 7389
		private static RoutedUICommand _MoveDownByLine;

		// Token: 0x04001CDE RID: 7390
		private static RoutedUICommand _MoveUpByLine;

		// Token: 0x04001CDF RID: 7391
		private static RoutedUICommand _MoveDownByParagraph;

		// Token: 0x04001CE0 RID: 7392
		private static RoutedUICommand _MoveUpByParagraph;

		// Token: 0x04001CE1 RID: 7393
		private static RoutedUICommand _MoveDownByPage;

		// Token: 0x04001CE2 RID: 7394
		private static RoutedUICommand _MoveUpByPage;

		// Token: 0x04001CE3 RID: 7395
		private static RoutedUICommand _MoveToLineStart;

		// Token: 0x04001CE4 RID: 7396
		private static RoutedUICommand _MoveToLineEnd;

		// Token: 0x04001CE5 RID: 7397
		private static RoutedUICommand _MoveToColumnStart;

		// Token: 0x04001CE6 RID: 7398
		private static RoutedUICommand _MoveToColumnEnd;

		// Token: 0x04001CE7 RID: 7399
		private static RoutedUICommand _MoveToWindowTop;

		// Token: 0x04001CE8 RID: 7400
		private static RoutedUICommand _MoveToWindowBottom;

		// Token: 0x04001CE9 RID: 7401
		private static RoutedUICommand _MoveToDocumentStart;

		// Token: 0x04001CEA RID: 7402
		private static RoutedUICommand _MoveToDocumentEnd;

		// Token: 0x04001CEB RID: 7403
		private static RoutedUICommand _SelectRightByCharacter;

		// Token: 0x04001CEC RID: 7404
		private static RoutedUICommand _SelectLeftByCharacter;

		// Token: 0x04001CED RID: 7405
		private static RoutedUICommand _SelectRightByWord;

		// Token: 0x04001CEE RID: 7406
		private static RoutedUICommand _SelectLeftByWord;

		// Token: 0x04001CEF RID: 7407
		private static RoutedUICommand _SelectDownByLine;

		// Token: 0x04001CF0 RID: 7408
		private static RoutedUICommand _SelectUpByLine;

		// Token: 0x04001CF1 RID: 7409
		private static RoutedUICommand _SelectDownByParagraph;

		// Token: 0x04001CF2 RID: 7410
		private static RoutedUICommand _SelectUpByParagraph;

		// Token: 0x04001CF3 RID: 7411
		private static RoutedUICommand _SelectDownByPage;

		// Token: 0x04001CF4 RID: 7412
		private static RoutedUICommand _SelectUpByPage;

		// Token: 0x04001CF5 RID: 7413
		private static RoutedUICommand _SelectToLineStart;

		// Token: 0x04001CF6 RID: 7414
		private static RoutedUICommand _SelectToLineEnd;

		// Token: 0x04001CF7 RID: 7415
		private static RoutedUICommand _SelectToColumnStart;

		// Token: 0x04001CF8 RID: 7416
		private static RoutedUICommand _SelectToColumnEnd;

		// Token: 0x04001CF9 RID: 7417
		private static RoutedUICommand _SelectToWindowTop;

		// Token: 0x04001CFA RID: 7418
		private static RoutedUICommand _SelectToWindowBottom;

		// Token: 0x04001CFB RID: 7419
		private static RoutedUICommand _SelectToDocumentStart;

		// Token: 0x04001CFC RID: 7420
		private static RoutedUICommand _SelectToDocumentEnd;

		// Token: 0x04001CFD RID: 7421
		private static RoutedUICommand _CopyFormat;

		// Token: 0x04001CFE RID: 7422
		private static RoutedUICommand _PasteFormat;

		// Token: 0x04001CFF RID: 7423
		private static RoutedUICommand _ResetFormat;

		// Token: 0x04001D00 RID: 7424
		private static RoutedUICommand _ToggleBold;

		// Token: 0x04001D01 RID: 7425
		private static RoutedUICommand _ToggleItalic;

		// Token: 0x04001D02 RID: 7426
		private static RoutedUICommand _ToggleUnderline;

		// Token: 0x04001D03 RID: 7427
		private static RoutedUICommand _ToggleSubscript;

		// Token: 0x04001D04 RID: 7428
		private static RoutedUICommand _ToggleSuperscript;

		// Token: 0x04001D05 RID: 7429
		private static RoutedUICommand _IncreaseFontSize;

		// Token: 0x04001D06 RID: 7430
		private static RoutedUICommand _DecreaseFontSize;

		// Token: 0x04001D07 RID: 7431
		private static RoutedUICommand _ApplyFontSize;

		// Token: 0x04001D08 RID: 7432
		private static RoutedUICommand _ApplyFontFamily;

		// Token: 0x04001D09 RID: 7433
		private static RoutedUICommand _ApplyForeground;

		// Token: 0x04001D0A RID: 7434
		private static RoutedUICommand _ApplyBackground;

		// Token: 0x04001D0B RID: 7435
		private static RoutedUICommand _ToggleSpellCheck;

		// Token: 0x04001D0C RID: 7436
		private static RoutedUICommand _ApplyInlineFlowDirectionRTL;

		// Token: 0x04001D0D RID: 7437
		private static RoutedUICommand _ApplyInlineFlowDirectionLTR;

		// Token: 0x04001D0E RID: 7438
		private static RoutedUICommand _AlignLeft;

		// Token: 0x04001D0F RID: 7439
		private static RoutedUICommand _AlignCenter;

		// Token: 0x04001D10 RID: 7440
		private static RoutedUICommand _AlignRight;

		// Token: 0x04001D11 RID: 7441
		private static RoutedUICommand _AlignJustify;

		// Token: 0x04001D12 RID: 7442
		private static RoutedUICommand _ApplySingleSpace;

		// Token: 0x04001D13 RID: 7443
		private static RoutedUICommand _ApplyOneAndAHalfSpace;

		// Token: 0x04001D14 RID: 7444
		private static RoutedUICommand _ApplyDoubleSpace;

		// Token: 0x04001D15 RID: 7445
		private static RoutedUICommand _IncreaseIndentation;

		// Token: 0x04001D16 RID: 7446
		private static RoutedUICommand _DecreaseIndentation;

		// Token: 0x04001D17 RID: 7447
		private static RoutedUICommand _ApplyParagraphFlowDirectionRTL;

		// Token: 0x04001D18 RID: 7448
		private static RoutedUICommand _ApplyParagraphFlowDirectionLTR;

		// Token: 0x04001D19 RID: 7449
		private static RoutedUICommand _RemoveListMarkers;

		// Token: 0x04001D1A RID: 7450
		private static RoutedUICommand _ToggleBullets;

		// Token: 0x04001D1B RID: 7451
		private static RoutedUICommand _ToggleNumbering;

		// Token: 0x04001D1C RID: 7452
		private static RoutedUICommand _InsertTable;

		// Token: 0x04001D1D RID: 7453
		private static RoutedUICommand _InsertRows;

		// Token: 0x04001D1E RID: 7454
		private static RoutedUICommand _InsertColumns;

		// Token: 0x04001D1F RID: 7455
		private static RoutedUICommand _DeleteRows;

		// Token: 0x04001D20 RID: 7456
		private static RoutedUICommand _DeleteColumns;

		// Token: 0x04001D21 RID: 7457
		private static RoutedUICommand _MergeCells;

		// Token: 0x04001D22 RID: 7458
		private static RoutedUICommand _SplitCell;

		// Token: 0x04001D23 RID: 7459
		private static RoutedUICommand _CorrectSpellingError;

		// Token: 0x04001D24 RID: 7460
		private static RoutedUICommand _IgnoreSpellingError;
	}
}
