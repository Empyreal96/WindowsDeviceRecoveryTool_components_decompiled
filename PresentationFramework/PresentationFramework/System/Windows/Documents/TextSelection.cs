using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	/// <summary>Encapsulates the selection state for the <see cref="T:System.Windows.Controls.RichTextBox" /> control.</summary>
	// Token: 0x02000413 RID: 1043
	public sealed class TextSelection : TextRange, ITextSelection, ITextRange
	{
		// Token: 0x06003C0E RID: 15374 RVA: 0x001151FC File Offset: 0x001133FC
		internal TextSelection(TextEditor textEditor) : base(textEditor.TextContainer.Start, textEditor.TextContainer.Start)
		{
			Invariant.Assert(textEditor.UiScope != null);
			this._textEditor = textEditor;
			this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
			((ITextSelection)this).UpdateCaretAndHighlight();
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00115254 File Offset: 0x00113454
		void ITextRange.Select(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.Select(this, anchorPosition, movingPosition);
				this.SetActivePositions(anchorPosition, movingPosition);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00115290 File Offset: 0x00113490
		void ITextRange.SelectWord(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.SelectWord(this, position);
				this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x001152D8 File Offset: 0x001134D8
		void ITextRange.SelectParagraph(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.SelectParagraph(this, position);
				this.SetActivePositions(position, ((ITextRange)this).End);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0011531C File Offset: 0x0011351C
		void ITextRange.ApplyTypingHeuristics(bool overType)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.ApplyInitialTypingHeuristics(this);
				if (!base.IsEmpty && this._textEditor.AcceptsRichContent)
				{
					this.SpringloadCurrentFormatting();
				}
				TextRangeBase.ApplyFinalTypingHeuristics(this, overType);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00115370 File Offset: 0x00113570
		object ITextRange.GetPropertyValue(DependencyProperty formattingProperty)
		{
			object result;
			if (base.IsEmpty && TextSchema.IsCharacterProperty(formattingProperty))
			{
				result = this.GetCurrentValue(formattingProperty);
			}
			else
			{
				result = TextRangeBase.GetPropertyValue(this, formattingProperty);
			}
			return result;
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x06003C14 RID: 15380 RVA: 0x0010A22C File Offset: 0x0010842C
		// (set) Token: 0x06003C15 RID: 15381 RVA: 0x001153A0 File Offset: 0x001135A0
		bool ITextRange._IsChanged
		{
			get
			{
				return base._IsChanged;
			}
			set
			{
				if (!base._IsChanged && value)
				{
					if (this.TextStore != null)
					{
						this.TextStore.OnSelectionChange();
					}
					if (this.ImmComposition != null)
					{
						this.ImmComposition.OnSelectionChange();
					}
				}
				base._IsChanged = value;
			}
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x001153DC File Offset: 0x001135DC
		void ITextRange.NotifyChanged(bool disableScroll, bool skipEvents)
		{
			if (this.TextStore != null)
			{
				this.TextStore.OnSelectionChanged();
			}
			if (this.ImmComposition != null)
			{
				this.ImmComposition.OnSelectionChanged();
			}
			if (!skipEvents)
			{
				TextRangeBase.NotifyChanged(this, disableScroll);
			}
			if (!disableScroll)
			{
				ITextPointer movingPosition = ((ITextSelection)this).MovingPosition;
				if (this.TextView != null && this.TextView.IsValid && !this.TextView.Contains(movingPosition))
				{
					movingPosition.ValidateLayout();
				}
			}
			this.UpdateCaretState(disableScroll ? CaretScrollMethod.None : CaretScrollMethod.Simple);
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x0010A190 File Offset: 0x00108390
		// (set) Token: 0x06003C18 RID: 15384 RVA: 0x0011545C File Offset: 0x0011365C
		string ITextRange.Text
		{
			get
			{
				return TextRangeBase.GetText(this);
			}
			set
			{
				TextRangeBase.BeginChange(this);
				try
				{
					TextRangeBase.SetText(this, value);
					if (base.IsEmpty)
					{
						((ITextSelection)this).SetCaretToPosition(((ITextRange)this).End, LogicalDirection.Forward, false, false);
					}
					Invariant.Assert(!base.IsTableCellRange);
					this.SetActivePositions(((ITextRange)this).Start, ((ITextRange)this).End);
				}
				finally
				{
					TextRangeBase.EndChange(this);
				}
			}
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x001154C8 File Offset: 0x001136C8
		void ITextSelection.UpdateCaretAndHighlight()
		{
			FrameworkElement uiScope = this.UiScope;
			FrameworkElement ownerElement = CaretElement.GetOwnerElement(uiScope);
			bool flag = false;
			bool isBlinkEnabled = false;
			bool flag2 = false;
			if (uiScope.IsEnabled && this.TextView != null)
			{
				if (uiScope.IsKeyboardFocused)
				{
					flag = true;
					isBlinkEnabled = true;
					flag2 = true;
				}
				else if (uiScope.IsFocused && ((TextSelection.IsRootElement(FocusManager.GetFocusScope(uiScope)) && this.IsFocusWithinRoot()) || this._textEditor.IsContextMenuOpen))
				{
					flag = true;
					isBlinkEnabled = false;
					flag2 = true;
				}
				else if (!base.IsEmpty && (bool)ownerElement.GetValue(TextBoxBase.IsInactiveSelectionHighlightEnabledProperty))
				{
					flag = true;
					isBlinkEnabled = false;
					flag2 = false;
				}
			}
			ownerElement.SetValue(TextBoxBase.IsSelectionActivePropertyKey, flag2);
			if (flag)
			{
				if (flag2)
				{
					this.SetThreadSelection();
				}
				this.EnsureCaret(isBlinkEnabled, flag2, CaretScrollMethod.None);
				this.Highlight();
				return;
			}
			this.ClearThreadSelection();
			this.DetachCaretFromVisualTree();
			this.Unhighlight();
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x001155A0 File Offset: 0x001137A0
		ITextPointer ITextSelection.AnchorPosition
		{
			get
			{
				Invariant.Assert(base.IsEmpty || this._anchorPosition != null);
				Invariant.Assert(this._anchorPosition == null || this._anchorPosition.IsFrozen);
				if (!base.IsEmpty)
				{
					return this._anchorPosition;
				}
				return ((ITextRange)this).Start;
			}
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x001155F8 File Offset: 0x001137F8
		ITextPointer ITextSelection.MovingPosition
		{
			get
			{
				ITextPointer textPointer;
				if (base.IsEmpty)
				{
					textPointer = ((ITextRange)this).Start;
				}
				else
				{
					switch (this._movingPositionEdge)
					{
					case TextSelection.MovingEdge.Start:
						textPointer = ((ITextRange)this).Start;
						goto IL_92;
					case TextSelection.MovingEdge.StartInner:
						textPointer = ((ITextRange)this).TextSegments[0].End;
						goto IL_92;
					case TextSelection.MovingEdge.EndInner:
						textPointer = ((ITextRange)this).TextSegments[((ITextRange)this).TextSegments.Count - 1].Start;
						goto IL_92;
					case TextSelection.MovingEdge.End:
						textPointer = ((ITextRange)this).End;
						goto IL_92;
					}
					Invariant.Assert(false, "MovingEdge should never be None with non-empty TextSelection!");
					textPointer = null;
					IL_92:
					textPointer = textPointer.GetFrozenPointer(this._movingPositionDirection);
				}
				return textPointer;
			}
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x001156A8 File Offset: 0x001138A8
		void ITextSelection.SetCaretToPosition(ITextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace)
		{
			caretPosition = caretPosition.CreatePointer(direction);
			caretPosition.MoveToInsertionPosition(direction);
			ITextPointer position = caretPosition.CreatePointer((direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward);
			if (!allowStopAtLineEnd && ((TextPointerBase.IsAtLineWrappingPosition(caretPosition, this.TextView) && TextPointerBase.IsAtLineWrappingPosition(position, this.TextView)) || TextPointerBase.IsNextToPlainLineBreak(caretPosition, LogicalDirection.Backward) || TextSchema.IsBreak(caretPosition.GetElementType(LogicalDirection.Backward))))
			{
				caretPosition.SetLogicalDirection(LogicalDirection.Forward);
			}
			else if ((caretPosition.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text || caretPosition.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text) && !allowStopNearSpace)
			{
				char[] array = new char[1];
				if (caretPosition.GetPointerContext(direction) == TextPointerContext.Text && caretPosition.GetTextInRun(direction, array, 0, 1) == 1 && char.IsWhiteSpace(array[0]))
				{
					LogicalDirection logicalDirection = (direction == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
					FlowDirection flowDirection = (FlowDirection)caretPosition.GetValue(FrameworkElement.FlowDirectionProperty);
					bool flag = caretPosition.MoveToInsertionPosition(logicalDirection);
					if (flag && flowDirection == (FlowDirection)caretPosition.GetValue(FrameworkElement.FlowDirectionProperty) && (caretPosition.GetPointerContext(logicalDirection) != TextPointerContext.Text || caretPosition.GetTextInRun(logicalDirection, array, 0, 1) != 1 || !char.IsWhiteSpace(array[0])))
					{
						direction = logicalDirection;
						caretPosition.SetLogicalDirection(direction);
					}
				}
			}
			TextRangeBase.BeginChange(this);
			try
			{
				TextRangeBase.Select(this, caretPosition, caretPosition);
				Invariant.Assert(((ITextRange)this).Start.LogicalDirection == caretPosition.LogicalDirection);
				Invariant.Assert(base.IsEmpty);
				this.SetActivePositions(null, null);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C1D RID: 15389 RVA: 0x00115818 File Offset: 0x00113A18
		void ITextSelection.ExtendToPosition(ITextPointer position)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
				TextRangeBase.Select(this, anchorPosition, position);
				this.SetActivePositions(anchorPosition, position);
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x00115860 File Offset: 0x00113A60
		bool ITextSelection.ExtendToNextInsertionPosition(LogicalDirection direction)
		{
			bool result = false;
			TextRangeBase.BeginChange(this);
			try
			{
				ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
				ITextPointer movingPosition = ((ITextSelection)this).MovingPosition;
				ITextPointer textPointer;
				if (base.IsTableCellRange)
				{
					textPointer = TextRangeEditTables.GetNextTableCellRangeInsertionPosition(this, direction);
				}
				else if (movingPosition is TextPointer && TextPointerBase.IsAtRowEnd(movingPosition))
				{
					textPointer = TextRangeEditTables.GetNextRowEndMovingPosition(this, direction);
				}
				else if (movingPosition is TextPointer && TextRangeEditTables.MovingPositionCrossesCellBoundary(this))
				{
					textPointer = TextRangeEditTables.GetNextRowStartMovingPosition(this, direction);
				}
				else
				{
					textPointer = this.GetNextTextSegmentInsertionPosition(direction);
				}
				if (textPointer == null && direction == LogicalDirection.Forward && movingPosition.CompareTo(movingPosition.TextContainer.End) != 0)
				{
					textPointer = movingPosition.TextContainer.End;
				}
				if (textPointer != null)
				{
					result = true;
					TextRangeBase.Select(this, anchorPosition, textPointer);
					LogicalDirection logicalDirection = (anchorPosition.CompareTo(textPointer) <= 0) ? LogicalDirection.Backward : LogicalDirection.Forward;
					textPointer = textPointer.GetFrozenPointer(logicalDirection);
					this.SetActivePositions(anchorPosition, textPointer);
				}
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
			return result;
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x00115940 File Offset: 0x00113B40
		private ITextPointer GetNextTextSegmentInsertionPosition(LogicalDirection direction)
		{
			return ((ITextSelection)this).MovingPosition.GetNextInsertionPosition(direction);
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0011595C File Offset: 0x00113B5C
		bool ITextSelection.Contains(Point point)
		{
			if (((ITextRange)this).IsEmpty)
			{
				return false;
			}
			if (this.TextView == null || !this.TextView.IsValid)
			{
				return false;
			}
			bool flag = false;
			ITextPointer textPointer = this.TextView.GetTextPositionFromPoint(point, false);
			if (textPointer != null && ((ITextRange)this).Contains(textPointer))
			{
				textPointer = textPointer.GetNextInsertionPosition(textPointer.LogicalDirection);
				if (textPointer != null && ((ITextRange)this).Contains(textPointer))
				{
					flag = true;
				}
			}
			if (!flag && this._caretElement != null && this._caretElement.SelectionGeometry != null && this._caretElement.SelectionGeometry.FillContains(point))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x001159F4 File Offset: 0x00113BF4
		void ITextSelection.OnDetach()
		{
			((ITextSelection)this).UpdateCaretAndHighlight();
			if (this._highlightLayer != null && ((ITextRange)this).Start.TextContainer.Highlights.GetLayer(typeof(TextSelection)) == this._highlightLayer)
			{
				((ITextRange)this).Start.TextContainer.Highlights.RemoveLayer(this._highlightLayer);
			}
			this._highlightLayer = null;
			this._textEditor = null;
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x00115A64 File Offset: 0x00113C64
		void ITextSelection.OnTextViewUpdated()
		{
			if (this.UiScope.IsKeyboardFocused || this.UiScope.IsFocused)
			{
				CaretElement caretElement = this._caretElement;
				if (caretElement != null)
				{
					caretElement.OnTextViewUpdated();
				}
			}
			if (this._pendingUpdateCaretStateCallback)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.UpdateCaretStateWorker), null);
			}
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x00115ABC File Offset: 0x00113CBC
		void ITextSelection.DetachFromVisualTree()
		{
			this.DetachCaretFromVisualTree();
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x00115AC4 File Offset: 0x00113CC4
		void ITextSelection.RefreshCaret()
		{
			TextSelection.RefreshCaret(this._textEditor, this._textEditor.Selection);
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x00115ADC File Offset: 0x00113CDC
		void ITextSelection.OnInterimSelectionChanged(bool interimSelection)
		{
			this.UpdateCaretState(CaretScrollMethod.None);
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x00115AE8 File Offset: 0x00113CE8
		void ITextSelection.SetSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
			{
				((ITextSelection)this).ExtendSelectionByMouse(cursorPosition, false, false);
				return;
			}
			this.MoveSelectionByMouse(cursorPosition, cursorMousePoint);
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x00115B14 File Offset: 0x00113D14
		void ITextSelection.ExtendSelectionByMouse(ITextPointer cursorPosition, bool forceWordSelection, bool forceParagraphSelection)
		{
			if (forceParagraphSelection || (this._previousCursorPosition != null && cursorPosition.CompareTo(this._previousCursorPosition) == 0))
			{
				return;
			}
			((ITextRange)this).BeginChange();
			try
			{
				if (this.BeginMouseSelectionProcess(cursorPosition))
				{
					ITextPointer anchorPosition = ((ITextSelection)this).AnchorPosition;
					TextSegment textSegment;
					TextSegment textSegment2;
					this.IdentifyWordsOnSelectionEnds(anchorPosition, cursorPosition, forceWordSelection, out textSegment, out textSegment2);
					ITextPointer frozenPointer;
					ITextPointer frozenPointer2;
					if (textSegment.Start.CompareTo(textSegment2.Start) <= 0)
					{
						frozenPointer = textSegment.Start.GetFrozenPointer(LogicalDirection.Forward);
						frozenPointer2 = textSegment2.End.GetFrozenPointer(LogicalDirection.Backward);
					}
					else
					{
						frozenPointer = textSegment.End.GetFrozenPointer(LogicalDirection.Backward);
						frozenPointer2 = textSegment2.Start.GetFrozenPointer(LogicalDirection.Forward);
					}
					TextRangeBase.Select(this, frozenPointer, frozenPointer2, true);
					this.SetActivePositions(anchorPosition, frozenPointer2);
					this._previousCursorPosition = cursorPosition.CreatePointer();
					Invariant.Assert(((ITextRange)this).Contains(((ITextSelection)this).AnchorPosition));
				}
			}
			finally
			{
				((ITextRange)this).EndChange();
			}
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x00115C04 File Offset: 0x00113E04
		private bool BeginMouseSelectionProcess(ITextPointer cursorPosition)
		{
			if (this._previousCursorPosition == null)
			{
				this._anchorWordRangeHasBeenCrossedOnce = false;
				this._allowWordExpansionOnAnchorEnd = true;
				this._reenterPosition = null;
				if (base.GetUIElementSelected() != null)
				{
					this._previousCursorPosition = cursorPosition.CreatePointer();
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x00115C3C File Offset: 0x00113E3C
		private void IdentifyWordsOnSelectionEnds(ITextPointer anchorPosition, ITextPointer cursorPosition, bool forceWordSelection, out TextSegment anchorWordRange, out TextSegment cursorWordRange)
		{
			if (forceWordSelection)
			{
				anchorWordRange = TextPointerBase.GetWordRange(anchorPosition);
				cursorWordRange = TextPointerBase.GetWordRange(cursorPosition, cursorPosition.LogicalDirection);
				return;
			}
			bool flag = !this._textEditor.AutoWordSelection || ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Control) > ModifierKeys.None);
			if (flag)
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			anchorWordRange = TextPointerBase.GetWordRange(anchorPosition);
			if (this._previousCursorPosition != null && ((anchorPosition.CompareTo(cursorPosition) < 0 && cursorPosition.CompareTo(this._previousCursorPosition) < 0) || (this._previousCursorPosition.CompareTo(cursorPosition) < 0 && cursorPosition.CompareTo(anchorPosition) < 0)))
			{
				this._reenterPosition = cursorPosition.CreatePointer();
				if (this._anchorWordRangeHasBeenCrossedOnce && anchorWordRange.Contains(cursorPosition))
				{
					this._allowWordExpansionOnAnchorEnd = false;
				}
			}
			else if (this._reenterPosition != null && !TextPointerBase.GetWordRange(this._reenterPosition).Contains(cursorPosition))
			{
				this._reenterPosition = null;
			}
			if (anchorWordRange.Contains(cursorPosition) || anchorWordRange.Contains(cursorPosition.GetInsertionPosition(LogicalDirection.Forward)) || anchorWordRange.Contains(cursorPosition.GetInsertionPosition(LogicalDirection.Backward)))
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			this._anchorWordRangeHasBeenCrossedOnce = true;
			if (!this._allowWordExpansionOnAnchorEnd || TextPointerBase.IsAtWordBoundary(anchorPosition, LogicalDirection.Forward))
			{
				anchorWordRange = new TextSegment(anchorPosition, anchorPosition);
			}
			if (TextPointerBase.IsAfterLastParagraph(cursorPosition) || TextPointerBase.IsAtWordBoundary(cursorPosition, LogicalDirection.Forward))
			{
				cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
				return;
			}
			if (this._reenterPosition == null)
			{
				cursorWordRange = TextPointerBase.GetWordRange(cursorPosition, cursorPosition.LogicalDirection);
				return;
			}
			cursorWordRange = new TextSegment(cursorPosition, cursorPosition);
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x00115E00 File Offset: 0x00114000
		bool ITextSelection.ExtendToNextTableRow(LogicalDirection direction)
		{
			if (!base.IsTableCellRange)
			{
				return false;
			}
			Invariant.Assert(!base.IsEmpty);
			Invariant.Assert(this._anchorPosition != null);
			Invariant.Assert(this._movingPositionEdge != TextSelection.MovingEdge.None);
			TableCell tableCell;
			TableCell tableCell2;
			if (!TextRangeEditTables.IsTableCellRange((TextPointer)this._anchorPosition, (TextPointer)((ITextSelection)this).MovingPosition, false, out tableCell, out tableCell2))
			{
				return false;
			}
			Invariant.Assert(tableCell != null && tableCell2 != null);
			TableRowGroup rowGroup = tableCell2.Row.RowGroup;
			TableCell tableCell3 = null;
			if (direction == LogicalDirection.Forward)
			{
				int i = tableCell2.Row.Index + tableCell2.RowSpan;
				if (i < rowGroup.Rows.Count)
				{
					tableCell3 = TextSelection.FindCellAtColumnIndex(rowGroup.Rows[i].Cells, tableCell2.ColumnIndex);
				}
			}
			else
			{
				for (int i = tableCell2.Row.Index - 1; i >= 0; i--)
				{
					tableCell3 = TextSelection.FindCellAtColumnIndex(rowGroup.Rows[i].Cells, tableCell2.ColumnIndex);
					if (tableCell3 != null)
					{
						break;
					}
				}
			}
			if (tableCell3 != null)
			{
				ITextPointer textPointer = tableCell3.ContentEnd.CreatePointer();
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				TextRangeBase.Select(this, this._anchorPosition, textPointer);
				this.SetActivePositions(this._anchorPosition, textPointer);
				return true;
			}
			return false;
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06003C2B RID: 15403 RVA: 0x00115F3F File Offset: 0x0011413F
		internal bool IsInterimSelection
		{
			get
			{
				return this.TextStore != null && this.TextStore.IsInterimSelection;
			}
		}

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06003C2C RID: 15404 RVA: 0x00115F56 File Offset: 0x00114156
		bool ITextSelection.IsInterimSelection
		{
			get
			{
				return this.IsInterimSelection;
			}
		}

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x00115F5E File Offset: 0x0011415E
		internal TextPointer AnchorPosition
		{
			get
			{
				return (TextPointer)((ITextSelection)this).AnchorPosition;
			}
		}

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06003C2E RID: 15406 RVA: 0x00115F6B File Offset: 0x0011416B
		internal TextPointer MovingPosition
		{
			get
			{
				return (TextPointer)((ITextSelection)this).MovingPosition;
			}
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x00115F78 File Offset: 0x00114178
		internal void SetCaretToPosition(TextPointer caretPosition, LogicalDirection direction, bool allowStopAtLineEnd, bool allowStopNearSpace)
		{
			((ITextSelection)this).SetCaretToPosition(caretPosition, direction, allowStopAtLineEnd, allowStopNearSpace);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x00115F85 File Offset: 0x00114185
		internal bool ExtendToNextInsertionPosition(LogicalDirection direction)
		{
			return ((ITextSelection)this).ExtendToNextInsertionPosition(direction);
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x00115F90 File Offset: 0x00114190
		internal static void OnInputLanguageChanged(CultureInfo cultureInfo)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (TextSelection.IsBidiInputLanguage(cultureInfo))
			{
				threadLocalStore.Bidi = true;
			}
			else
			{
				threadLocalStore.Bidi = false;
			}
			if (threadLocalStore.FocusedTextSelection != null)
			{
				((ITextSelection)threadLocalStore.FocusedTextSelection).RefreshCaret();
			}
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x00115FCE File Offset: 0x001141CE
		internal bool Contains(Point point)
		{
			return ((ITextSelection)this).Contains(point);
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x00115FD8 File Offset: 0x001141D8
		internal override void InsertEmbeddedUIElementVirtual(FrameworkElement embeddedElement)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.InsertEmbeddedUIElementVirtual(embeddedElement);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x00116014 File Offset: 0x00114214
		internal override void ApplyPropertyToTextVirtual(DependencyProperty formattingProperty, object value, bool applyToParagraphs, PropertyValueAction propertyValueAction)
		{
			if (!TextSchema.IsParagraphProperty(formattingProperty) && !TextSchema.IsCharacterProperty(formattingProperty))
			{
				return;
			}
			if (!base.IsEmpty || !TextSchema.IsCharacterProperty(formattingProperty) || applyToParagraphs || formattingProperty == FrameworkElement.FlowDirectionProperty)
			{
				base.ApplyPropertyToTextVirtual(formattingProperty, value, applyToParagraphs, propertyValueAction);
				this.ClearSpringloadFormatting();
				return;
			}
			TextSegment autoWord = TextRangeBase.GetAutoWord(this);
			if (autoWord.IsNull)
			{
				if (this._springloadFormatting == null)
				{
					this._springloadFormatting = new DependencyObject();
				}
				this._springloadFormatting.SetValue(formattingProperty, value);
				return;
			}
			new TextRange(autoWord.Start, autoWord.End).ApplyPropertyValue(formattingProperty, value);
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x001160AC File Offset: 0x001142AC
		internal override void ClearAllPropertiesVirtual()
		{
			if (base.IsEmpty)
			{
				this.ClearSpringloadFormatting();
				return;
			}
			TextRangeBase.BeginChange(this);
			try
			{
				base.ClearAllPropertiesVirtual();
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x001160F4 File Offset: 0x001142F4
		internal override void SetXmlVirtual(TextElement fragment)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.SetXmlVirtual(fragment);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x00116130 File Offset: 0x00114330
		internal override void LoadVirtual(Stream stream, string dataFormat)
		{
			TextRangeBase.BeginChange(this);
			try
			{
				base.LoadVirtual(stream, dataFormat);
				this.ClearSpringloadFormatting();
			}
			finally
			{
				TextRangeBase.EndChange(this);
			}
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0011616C File Offset: 0x0011436C
		internal override Table InsertTableVirtual(int rowCount, int columnCount)
		{
			Table result;
			using (base.DeclareChangeBlock())
			{
				Table table = base.InsertTableVirtual(rowCount, columnCount);
				if (table != null)
				{
					TextPointer contentStart = table.RowGroups[0].Rows[0].Cells[0].ContentStart;
					this.SetCaretToPosition(contentStart, LogicalDirection.Backward, false, false);
				}
				result = table;
			}
			return result;
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x001161E0 File Offset: 0x001143E0
		internal object GetCurrentValue(DependencyProperty formattingProperty)
		{
			object obj = DependencyProperty.UnsetValue;
			if (((ITextRange)this).Start is TextPointer && this._springloadFormatting != null && base.IsEmpty)
			{
				obj = this._springloadFormatting.ReadLocalValue(formattingProperty);
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = base.Start.Parent.GetValue(formattingProperty);
				}
			}
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = this.PropertyPosition.GetValue(formattingProperty);
			}
			return obj;
		}

		// Token: 0x06003C3A RID: 15418 RVA: 0x00116250 File Offset: 0x00114450
		internal void SpringloadCurrentFormatting()
		{
			if (((ITextRange)this).Start is TextPointer)
			{
				TextPointer textPointer = base.Start;
				Inline nonMergeableInlineAncestor = textPointer.GetNonMergeableInlineAncestor();
				if (nonMergeableInlineAncestor != null && base.End.GetNonMergeableInlineAncestor() != nonMergeableInlineAncestor)
				{
					textPointer = nonMergeableInlineAncestor.ElementEnd;
				}
				if (this._springloadFormatting == null)
				{
					this.SpringloadCurrentFormatting(textPointer.Parent);
				}
			}
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x001162A4 File Offset: 0x001144A4
		private void SpringloadCurrentFormatting(DependencyObject parent)
		{
			this._springloadFormatting = new DependencyObject();
			if (parent == null)
			{
				return;
			}
			DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(typeof(Inline));
			DependencyProperty[] noninheritableProperties = TextSchema.GetNoninheritableProperties(typeof(Span));
			DependencyObject dependencyObject = parent;
			while (dependencyObject is Inline)
			{
				TextElementEditingBehaviorAttribute textElementEditingBehaviorAttribute = (TextElementEditingBehaviorAttribute)Attribute.GetCustomAttribute(dependencyObject.GetType(), typeof(TextElementEditingBehaviorAttribute));
				if (textElementEditingBehaviorAttribute.IsTypographicOnly)
				{
					for (int i = 0; i < inheritableProperties.Length; i++)
					{
						if (this._springloadFormatting.ReadLocalValue(inheritableProperties[i]) == DependencyProperty.UnsetValue && inheritableProperties[i] != FrameworkElement.LanguageProperty && inheritableProperties[i] != FrameworkElement.FlowDirectionProperty && DependencyPropertyHelper.GetValueSource(dependencyObject, inheritableProperties[i]).BaseValueSource != BaseValueSource.Inherited)
						{
							object value = parent.GetValue(inheritableProperties[i]);
							this._springloadFormatting.SetValue(inheritableProperties[i], value);
						}
					}
					for (int j = 0; j < noninheritableProperties.Length; j++)
					{
						if (this._springloadFormatting.ReadLocalValue(noninheritableProperties[j]) == DependencyProperty.UnsetValue && noninheritableProperties[j] != TextElement.TextEffectsProperty && DependencyPropertyHelper.GetValueSource(dependencyObject, noninheritableProperties[j]).BaseValueSource != BaseValueSource.Inherited)
						{
							object value2 = parent.GetValue(noninheritableProperties[j]);
							this._springloadFormatting.SetValue(noninheritableProperties[j], value2);
						}
					}
				}
				dependencyObject = ((TextElement)dependencyObject).Parent;
			}
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x001163FB File Offset: 0x001145FB
		internal void ClearSpringloadFormatting()
		{
			if (((ITextRange)this).Start is TextPointer)
			{
				this._springloadFormatting = null;
				((ITextSelection)this).RefreshCaret();
			}
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x00116418 File Offset: 0x00114618
		internal void ApplySpringloadFormatting()
		{
			if (!(((ITextRange)this).Start is TextPointer))
			{
				return;
			}
			if (base.IsEmpty)
			{
				return;
			}
			if (this._springloadFormatting != null)
			{
				Invariant.Assert(base.Start.LogicalDirection == LogicalDirection.Backward);
				Invariant.Assert(base.End.LogicalDirection == LogicalDirection.Forward);
				LocalValueEnumerator localValueEnumerator = this._springloadFormatting.GetLocalValueEnumerator();
				while (!base.IsEmpty && localValueEnumerator.MoveNext())
				{
					LocalValueEntry localValueEntry = localValueEnumerator.Current;
					Invariant.Assert(TextSchema.IsCharacterProperty(localValueEntry.Property));
					base.ApplyPropertyValue(localValueEntry.Property, localValueEntry.Value);
				}
				this.ClearSpringloadFormatting();
			}
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x001164BC File Offset: 0x001146BC
		internal void UpdateCaretState(CaretScrollMethod caretScrollMethod)
		{
			Invariant.Assert(caretScrollMethod > CaretScrollMethod.Unset);
			if (this._pendingCaretNavigation)
			{
				caretScrollMethod = CaretScrollMethod.Navigation;
				this._pendingCaretNavigation = false;
			}
			if (this._caretScrollMethod != CaretScrollMethod.Unset)
			{
				if (caretScrollMethod != CaretScrollMethod.None)
				{
					this._caretScrollMethod = caretScrollMethod;
				}
				return;
			}
			this._caretScrollMethod = caretScrollMethod;
			if (this._textEditor.TextView != null && this._textEditor.TextView.IsValid)
			{
				this.UpdateCaretStateWorker(null);
				return;
			}
			this._pendingUpdateCaretStateCallback = true;
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x00116530 File Offset: 0x00114730
		internal static Brush GetCaretBrush(TextEditor textEditor)
		{
			Brush brush = (Brush)textEditor.UiScope.GetValue(TextBoxBase.CaretBrushProperty);
			if (brush != null)
			{
				return brush;
			}
			object obj = textEditor.UiScope.GetValue(Panel.BackgroundProperty);
			Color color;
			if (obj != null && obj != DependencyProperty.UnsetValue && obj is SolidColorBrush)
			{
				color = ((SolidColorBrush)obj).Color;
			}
			else
			{
				color = SystemColors.WindowColor;
			}
			ITextSelection selection = textEditor.Selection;
			if (selection is TextSelection)
			{
				obj = ((TextSelection)selection).GetCurrentValue(TextElement.BackgroundProperty);
				if (obj != null && obj != DependencyProperty.UnsetValue && obj is SolidColorBrush)
				{
					color = ((SolidColorBrush)obj).Color;
				}
			}
			byte r = ~color.R;
			byte g = ~color.G;
			byte b = ~color.B;
			brush = new SolidColorBrush(Color.FromRgb(r, g, b));
			brush.Freeze();
			return brush;
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x00116608 File Offset: 0x00114808
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static bool IsBidiInputLanguageInstalled()
		{
			bool result = false;
			int keyboardLayoutList = SafeNativeMethods.GetKeyboardLayoutList(0, null);
			if (keyboardLayoutList > 0)
			{
				IntPtr[] array = new IntPtr[keyboardLayoutList];
				keyboardLayoutList = SafeNativeMethods.GetKeyboardLayoutList(keyboardLayoutList, array);
				int num = 0;
				while (num < array.Length && num < keyboardLayoutList)
				{
					CultureInfo cultureInfo = new CultureInfo((int)((short)((int)array[num])));
					if (TextSelection.IsBidiInputLanguage(cultureInfo))
					{
						result = true;
						break;
					}
					num++;
				}
			}
			return result;
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x00116662 File Offset: 0x00114862
		void ITextSelection.ValidateLayout()
		{
			((ITextSelection)this).MovingPosition.ValidateLayout();
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x00116670 File Offset: 0x00114870
		internal CaretElement CaretElement
		{
			get
			{
				return this._caretElement;
			}
		}

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06003C43 RID: 15427 RVA: 0x00116678 File Offset: 0x00114878
		CaretElement ITextSelection.CaretElement
		{
			get
			{
				return this.CaretElement;
			}
		}

		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x00116680 File Offset: 0x00114880
		bool ITextSelection.CoversEntireContent
		{
			get
			{
				return ((ITextRange)this).Start.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && ((ITextRange)this).End.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text && ((ITextRange)this).Start.GetNextInsertionPosition(LogicalDirection.Backward) == null && ((ITextRange)this).End.GetNextInsertionPosition(LogicalDirection.Forward) == null;
			}
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x001166CC File Offset: 0x001148CC
		private void SetThreadSelection()
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			threadLocalStore.FocusedTextSelection = this;
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x001166E6 File Offset: 0x001148E6
		private void ClearThreadSelection()
		{
			if (TextEditor._ThreadLocalStore.FocusedTextSelection == this)
			{
				TextEditor._ThreadLocalStore.FocusedTextSelection = null;
			}
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x00116700 File Offset: 0x00114900
		private void Highlight()
		{
			ITextContainer textContainer = ((ITextRange)this).Start.TextContainer;
			if (FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering && (textContainer is TextContainer || textContainer is PasswordTextContainer))
			{
				return;
			}
			if (this._highlightLayer == null)
			{
				this._highlightLayer = new TextSelectionHighlightLayer(this);
			}
			if (textContainer.Highlights.GetLayer(typeof(TextSelection)) == null)
			{
				textContainer.Highlights.AddLayer(this._highlightLayer);
			}
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x00116770 File Offset: 0x00114970
		private void Unhighlight()
		{
			ITextContainer textContainer = ((ITextRange)this).Start.TextContainer;
			TextSelectionHighlightLayer textSelectionHighlightLayer = textContainer.Highlights.GetLayer(typeof(TextSelection)) as TextSelectionHighlightLayer;
			if (textSelectionHighlightLayer != null)
			{
				textContainer.Highlights.RemoveLayer(textSelectionHighlightLayer);
				Invariant.Assert(textContainer.Highlights.GetLayer(typeof(TextSelection)) == null);
			}
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x001167D0 File Offset: 0x001149D0
		private void SetActivePositions(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			this._previousCursorPosition = null;
			if (base.IsEmpty)
			{
				this._anchorPosition = null;
				this._movingPositionEdge = TextSelection.MovingEdge.None;
				return;
			}
			Invariant.Assert(anchorPosition != null);
			this._anchorPosition = anchorPosition.GetInsertionPosition(anchorPosition.LogicalDirection);
			if (this._anchorPosition.CompareTo(((ITextRange)this).Start) < 0)
			{
				this._anchorPosition = ((ITextRange)this).Start.GetFrozenPointer(this._anchorPosition.LogicalDirection);
			}
			else if (this._anchorPosition.CompareTo(((ITextRange)this).End) > 0)
			{
				this._anchorPosition = ((ITextRange)this).End.GetFrozenPointer(this._anchorPosition.LogicalDirection);
			}
			this._movingPositionEdge = this.ConvertToMovingEdge(anchorPosition, movingPosition);
			this._movingPositionDirection = movingPosition.LogicalDirection;
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x00116894 File Offset: 0x00114A94
		private TextSelection.MovingEdge ConvertToMovingEdge(ITextPointer anchorPosition, ITextPointer movingPosition)
		{
			TextSelection.MovingEdge result;
			if (((ITextRange)this).IsEmpty)
			{
				result = TextSelection.MovingEdge.None;
			}
			else if (((ITextRange)this).TextSegments.Count < 2)
			{
				result = ((anchorPosition.CompareTo(movingPosition) <= 0) ? TextSelection.MovingEdge.End : TextSelection.MovingEdge.Start);
			}
			else if (movingPosition.CompareTo(((ITextRange)this).Start) == 0)
			{
				result = TextSelection.MovingEdge.Start;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).End) == 0)
			{
				result = TextSelection.MovingEdge.End;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).TextSegments[0].End) == 0)
			{
				result = TextSelection.MovingEdge.StartInner;
			}
			else if (movingPosition.CompareTo(((ITextRange)this).TextSegments[((ITextRange)this).TextSegments.Count - 1].Start) == 0)
			{
				result = TextSelection.MovingEdge.EndInner;
			}
			else
			{
				result = ((anchorPosition.CompareTo(movingPosition) <= 0) ? TextSelection.MovingEdge.End : TextSelection.MovingEdge.Start);
			}
			return result;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x00116954 File Offset: 0x00114B54
		private void MoveSelectionByMouse(ITextPointer cursorPosition, Point cursorMousePoint)
		{
			if (this.TextView == null)
			{
				return;
			}
			Invariant.Assert(this.TextView.IsValid);
			ITextPointer textPointer = null;
			if (cursorPosition.GetPointerContext(cursorPosition.LogicalDirection) == TextPointerContext.EmbeddedElement)
			{
				Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(cursorPosition);
				if (!this._textEditor.IsReadOnly && this.ShouldSelectEmbeddedObject(cursorPosition, cursorMousePoint, rectangleFromTextPosition))
				{
					textPointer = cursorPosition.GetNextContextPosition(cursorPosition.LogicalDirection);
				}
			}
			if (textPointer == null)
			{
				((ITextSelection)this).SetCaretToPosition(cursorPosition, cursorPosition.LogicalDirection, true, false);
				return;
			}
			((ITextRange)this).Select(cursorPosition, textPointer);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x001169DC File Offset: 0x00114BDC
		private bool ShouldSelectEmbeddedObject(ITextPointer cursorPosition, Point cursorMousePoint, Rect objectEdgeRect)
		{
			if (!objectEdgeRect.IsEmpty && cursorMousePoint.Y >= objectEdgeRect.Y && cursorMousePoint.Y < objectEdgeRect.Y + objectEdgeRect.Height)
			{
				FlowDirection flowDirection = (FlowDirection)this.TextView.RenderScope.GetValue(Block.FlowDirectionProperty);
				FlowDirection flowDirection2 = (FlowDirection)cursorPosition.GetValue(Block.FlowDirectionProperty);
				if (flowDirection == FlowDirection.LeftToRight)
				{
					if (flowDirection2 == FlowDirection.LeftToRight && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X < cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X < objectEdgeRect.X)))
					{
						return true;
					}
					if (flowDirection2 == FlowDirection.RightToLeft && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X > cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X > objectEdgeRect.X)))
					{
						return true;
					}
				}
				else
				{
					if (flowDirection2 == FlowDirection.LeftToRight && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X > cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X > objectEdgeRect.X)))
					{
						return true;
					}
					if (flowDirection2 == FlowDirection.RightToLeft && ((cursorPosition.LogicalDirection == LogicalDirection.Forward && objectEdgeRect.X < cursorMousePoint.X) || (cursorPosition.LogicalDirection == LogicalDirection.Backward && cursorMousePoint.X < objectEdgeRect.X)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00116B30 File Offset: 0x00114D30
		private static void RefreshCaret(TextEditor textEditor, ITextSelection textSelection)
		{
			if (textSelection == null || textSelection.CaretElement == null)
			{
				return;
			}
			object currentValue = ((TextSelection)textSelection).GetCurrentValue(TextElement.FontStyleProperty);
			bool italic = textEditor.AcceptsRichContent && currentValue != DependencyProperty.UnsetValue && (FontStyle)currentValue == FontStyles.Italic;
			textSelection.CaretElement.RefreshCaret(italic);
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00116B8A File Offset: 0x00114D8A
		internal void OnCaretNavigation()
		{
			this._pendingCaretNavigation = true;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x00116B93 File Offset: 0x00114D93
		void ITextSelection.OnCaretNavigation()
		{
			this.OnCaretNavigation();
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x00116B9C File Offset: 0x00114D9C
		private object UpdateCaretStateWorker(object o)
		{
			this._pendingUpdateCaretStateCallback = false;
			if (this._textEditor == null)
			{
				return null;
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			CaretScrollMethod caretScrollMethod = this._caretScrollMethod;
			this._caretScrollMethod = CaretScrollMethod.Unset;
			CaretElement caretElement = this._caretElement;
			if (caretElement == null)
			{
				return null;
			}
			if (threadLocalStore.FocusedTextSelection == null)
			{
				if (!base.IsEmpty)
				{
					caretElement.Hide();
				}
				return null;
			}
			if (this._textEditor.TextView == null || !this._textEditor.TextView.IsValid)
			{
				return null;
			}
			if (!this.VerifyAdornerLayerExists())
			{
				caretElement.Hide();
			}
			ITextPointer textPointer = TextSelection.IdentifyCaretPosition(this);
			if (textPointer.HasValidLayout)
			{
				bool italic = false;
				bool visible = base.IsEmpty && (!this._textEditor.IsReadOnly || this._textEditor.IsReadOnlyCaretVisible);
				Rect caretRectangle;
				if (!this.IsInterimSelection)
				{
					caretRectangle = TextSelection.CalculateCaretRectangle(this, textPointer);
					if (base.IsEmpty)
					{
						object propertyValue = base.GetPropertyValue(TextElement.FontStyleProperty);
						italic = (this._textEditor.AcceptsRichContent && propertyValue != DependencyProperty.UnsetValue && (FontStyle)propertyValue == FontStyles.Italic);
					}
				}
				else
				{
					caretRectangle = TextSelection.CalculateInterimCaretRectangle(this);
					visible = true;
				}
				Brush caretBrush = TextSelection.GetCaretBrush(this._textEditor);
				double scrollToOriginPosition = TextSelection.CalculateScrollToOriginPosition(this._textEditor, textPointer, caretRectangle.X);
				caretElement.Update(visible, caretRectangle, caretBrush, 1.0, italic, caretScrollMethod, scrollToOriginPosition);
			}
			if (this.TextView.IsValid && !this.TextView.RendersOwnSelection)
			{
				caretElement.UpdateSelection();
			}
			return null;
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x00116D1C File Offset: 0x00114F1C
		private static ITextPointer IdentifyCaretPosition(ITextSelection currentTextSelection)
		{
			ITextPointer textPointer = currentTextSelection.MovingPosition;
			if (!currentTextSelection.IsEmpty && ((textPointer.LogicalDirection == LogicalDirection.Backward && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart) || TextPointerBase.IsAfterLastParagraph(textPointer)))
			{
				textPointer = textPointer.CreatePointer();
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
			}
			if (textPointer.LogicalDirection == LogicalDirection.Backward && textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart && (textPointer.GetNextInsertionPosition(LogicalDirection.Backward) == null || TextPointerBase.IsNextToAnyBreak(textPointer, LogicalDirection.Backward)))
			{
				textPointer = textPointer.CreatePointer();
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
			}
			return textPointer;
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x00116D9C File Offset: 0x00114F9C
		private static Rect CalculateCaretRectangle(ITextSelection currentTextSelection, ITextPointer caretPosition)
		{
			Transform transform;
			Rect rect = currentTextSelection.TextView.GetRawRectangleFromTextPosition(caretPosition, out transform);
			if (rect.IsEmpty)
			{
				return Rect.Empty;
			}
			rect = transform.TransformBounds(rect);
			rect.Width = 0.0;
			if (currentTextSelection.IsEmpty)
			{
				double num = (double)currentTextSelection.GetPropertyValue(TextElement.FontSizeProperty);
				FontFamily fontFamily = (FontFamily)currentTextSelection.GetPropertyValue(TextElement.FontFamilyProperty);
				double num2 = fontFamily.LineSpacing * num;
				if (num2 < rect.Height)
				{
					rect.Y += rect.Height - num2;
					rect.Height = num2;
				}
				if (!transform.IsIdentity)
				{
					Point inPoint = new Point(rect.X, rect.Y);
					Point inPoint2 = new Point(rect.X, rect.Y + rect.Height);
					transform.TryTransform(inPoint, out inPoint);
					transform.TryTransform(inPoint2, out inPoint2);
					rect.Y += rect.Height - Math.Abs(inPoint2.Y - inPoint.Y);
					rect.Height = Math.Abs(inPoint2.Y - inPoint.Y);
				}
			}
			return rect;
		}

		// Token: 0x06003C53 RID: 15443 RVA: 0x00116EDC File Offset: 0x001150DC
		private static Rect CalculateInterimCaretRectangle(ITextSelection focusedTextSelection)
		{
			FlowDirection flowDirection = (FlowDirection)focusedTextSelection.Start.GetValue(FrameworkElement.FlowDirectionProperty);
			Rect rectangleFromTextPosition;
			Rect rectangleFromTextPosition2;
			if (flowDirection != FlowDirection.RightToLeft)
			{
				ITextPointer textPointer = focusedTextSelection.Start.CreatePointer(LogicalDirection.Forward);
				rectangleFromTextPosition = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
				textPointer.SetLogicalDirection(LogicalDirection.Backward);
				rectangleFromTextPosition2 = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
			}
			else
			{
				ITextPointer textPointer = focusedTextSelection.End.CreatePointer(LogicalDirection.Backward);
				rectangleFromTextPosition = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
				textPointer.SetLogicalDirection(LogicalDirection.Forward);
				rectangleFromTextPosition2 = focusedTextSelection.TextView.GetRectangleFromTextPosition(textPointer);
			}
			if (!rectangleFromTextPosition.IsEmpty && !rectangleFromTextPosition2.IsEmpty && rectangleFromTextPosition2.Left > rectangleFromTextPosition.Left)
			{
				rectangleFromTextPosition.Width = rectangleFromTextPosition2.Left - rectangleFromTextPosition.Left;
			}
			return rectangleFromTextPosition;
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x00116FAC File Offset: 0x001151AC
		private static double CalculateScrollToOriginPosition(TextEditor textEditor, ITextPointer caretPosition, double horizontalCaretPosition)
		{
			double num = double.NaN;
			if (textEditor.UiScope is TextBoxBase)
			{
				double viewportWidth = ((TextBoxBase)textEditor.UiScope).ViewportWidth;
				double extentWidth = ((TextBoxBase)textEditor.UiScope).ExtentWidth;
				if (viewportWidth != 0.0 && extentWidth != 0.0 && viewportWidth < extentWidth)
				{
					bool flag = false;
					if (horizontalCaretPosition < 0.0 || horizontalCaretPosition >= viewportWidth)
					{
						flag = true;
					}
					if (flag)
					{
						num = 0.0;
						FlowDirection flowDirection = (FlowDirection)textEditor.UiScope.GetValue(FrameworkElement.FlowDirectionProperty);
						Block block = (caretPosition is TextPointer) ? ((TextPointer)caretPosition).ParagraphOrBlockUIContainer : null;
						if (block != null)
						{
							FlowDirection flowDirection2 = block.FlowDirection;
							if (flowDirection != flowDirection2)
							{
								num = extentWidth;
							}
						}
						num -= ((TextBoxBase)textEditor.UiScope).HorizontalOffset;
					}
				}
			}
			return num;
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x00117090 File Offset: 0x00115290
		private CaretElement EnsureCaret(bool isBlinkEnabled, bool isSelectionActive, CaretScrollMethod scrollMethod)
		{
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			if (this._caretElement == null)
			{
				this._caretElement = new CaretElement(this._textEditor, isBlinkEnabled);
				this._caretElement.IsSelectionActive = isSelectionActive;
				if (TextSelection.IsBidiInputLanguage(InputLanguageManager.Current.CurrentInputLanguage))
				{
					TextEditor._ThreadLocalStore.Bidi = true;
				}
				else
				{
					TextEditor._ThreadLocalStore.Bidi = false;
				}
			}
			else
			{
				this._caretElement.IsSelectionActive = isSelectionActive;
				this._caretElement.SetBlinking(isBlinkEnabled);
			}
			this.UpdateCaretState(scrollMethod);
			return this._caretElement;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0011711C File Offset: 0x0011531C
		private bool VerifyAdornerLayerExists()
		{
			DependencyObject dependencyObject = this.TextView.RenderScope;
			while (dependencyObject != this._textEditor.UiScope && dependencyObject != null)
			{
				if (dependencyObject is AdornerDecorator || dependencyObject is ScrollContentPresenter)
				{
					return true;
				}
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
			return false;
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x00117164 File Offset: 0x00115364
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool IsBidiInputLanguage(CultureInfo cultureInfo)
		{
			bool result = false;
			string text = new string(new char[16]);
			if (UnsafeNativeMethods.GetLocaleInfoW(cultureInfo.LCID, 88, text, 16) != 0 && (text[7] & 'ࠀ') != '\0')
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x001171A4 File Offset: 0x001153A4
		private static TableCell FindCellAtColumnIndex(TableCellCollection cells, int columnIndex)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				TableCell tableCell = cells[i];
				int columnIndex2 = tableCell.ColumnIndex;
				int num = columnIndex2 + tableCell.ColumnSpan - 1;
				if (columnIndex2 <= columnIndex && columnIndex <= num)
				{
					return tableCell;
				}
			}
			return null;
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x001171E7 File Offset: 0x001153E7
		private static bool IsRootElement(DependencyObject element)
		{
			return TextSelection.GetParentElement(element) == null;
		}

		// Token: 0x06003C5A RID: 15450 RVA: 0x001171F4 File Offset: 0x001153F4
		private bool IsFocusWithinRoot()
		{
			DependencyObject dependencyObject = this.UiScope;
			for (DependencyObject dependencyObject2 = this.UiScope; dependencyObject2 != null; dependencyObject2 = TextSelection.GetParentElement(dependencyObject))
			{
				dependencyObject = dependencyObject2;
			}
			return dependencyObject is UIElement && ((UIElement)dependencyObject).IsKeyboardFocusWithin;
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x00117238 File Offset: 0x00115438
		private static DependencyObject GetParentElement(DependencyObject element)
		{
			DependencyObject dependencyObject;
			if (element is FrameworkElement || element is FrameworkContentElement)
			{
				dependencyObject = LogicalTreeHelper.GetParent(element);
				if (dependencyObject == null && element is FrameworkElement)
				{
					dependencyObject = ((FrameworkElement)element).TemplatedParent;
					if (dependencyObject == null && element is Visual)
					{
						dependencyObject = VisualTreeHelper.GetParent(element);
					}
				}
			}
			else if (element is Visual)
			{
				dependencyObject = VisualTreeHelper.GetParent(element);
			}
			else
			{
				dependencyObject = null;
			}
			return dependencyObject;
		}

		// Token: 0x06003C5C RID: 15452 RVA: 0x0011729B File Offset: 0x0011549B
		private void DetachCaretFromVisualTree()
		{
			if (this._caretElement != null)
			{
				this._caretElement.DetachFromView();
				this._caretElement = null;
			}
		}

		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x001172B7 File Offset: 0x001154B7
		TextEditor ITextSelection.TextEditor
		{
			get
			{
				return this._textEditor;
			}
		}

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06003C5E RID: 15454 RVA: 0x001172BF File Offset: 0x001154BF
		ITextView ITextSelection.TextView
		{
			get
			{
				return this._textEditor.TextView;
			}
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06003C5F RID: 15455 RVA: 0x001172CC File Offset: 0x001154CC
		private ITextView TextView
		{
			get
			{
				return ((ITextSelection)this).TextView;
			}
		}

		// Token: 0x17000EF4 RID: 3828
		// (get) Token: 0x06003C60 RID: 15456 RVA: 0x001172D4 File Offset: 0x001154D4
		private TextStore TextStore
		{
			get
			{
				return this._textEditor.TextStore;
			}
		}

		// Token: 0x17000EF5 RID: 3829
		// (get) Token: 0x06003C61 RID: 15457 RVA: 0x001172E1 File Offset: 0x001154E1
		private ImmComposition ImmComposition
		{
			get
			{
				return this._textEditor.ImmComposition;
			}
		}

		// Token: 0x17000EF6 RID: 3830
		// (get) Token: 0x06003C62 RID: 15458 RVA: 0x001172EE File Offset: 0x001154EE
		private FrameworkElement UiScope
		{
			get
			{
				return this._textEditor.UiScope;
			}
		}

		// Token: 0x17000EF7 RID: 3831
		// (get) Token: 0x06003C63 RID: 15459 RVA: 0x001172FC File Offset: 0x001154FC
		private ITextPointer PropertyPosition
		{
			get
			{
				ITextPointer textPointer = null;
				if (!((ITextRange)this).IsEmpty)
				{
					textPointer = TextPointerBase.GetFollowingNonMergeableInlineContentStart(((ITextRange)this).Start);
				}
				if (textPointer == null)
				{
					textPointer = ((ITextRange)this).Start;
				}
				textPointer.Freeze();
				return textPointer;
			}
		}

		// Token: 0x04002605 RID: 9733
		private TextEditor _textEditor;

		// Token: 0x04002606 RID: 9734
		private TextSelectionHighlightLayer _highlightLayer;

		// Token: 0x04002607 RID: 9735
		private DependencyObject _springloadFormatting;

		// Token: 0x04002608 RID: 9736
		private ITextPointer _anchorPosition;

		// Token: 0x04002609 RID: 9737
		private TextSelection.MovingEdge _movingPositionEdge;

		// Token: 0x0400260A RID: 9738
		private LogicalDirection _movingPositionDirection;

		// Token: 0x0400260B RID: 9739
		private ITextPointer _previousCursorPosition;

		// Token: 0x0400260C RID: 9740
		private ITextPointer _reenterPosition;

		// Token: 0x0400260D RID: 9741
		private bool _anchorWordRangeHasBeenCrossedOnce;

		// Token: 0x0400260E RID: 9742
		private bool _allowWordExpansionOnAnchorEnd;

		// Token: 0x0400260F RID: 9743
		private const int FONTSIGNATURE_SIZE = 16;

		// Token: 0x04002610 RID: 9744
		private const int FONTSIGNATURE_BIDI_INDEX = 7;

		// Token: 0x04002611 RID: 9745
		private const int FONTSIGNATURE_BIDI = 2048;

		// Token: 0x04002612 RID: 9746
		private CaretScrollMethod _caretScrollMethod;

		// Token: 0x04002613 RID: 9747
		private bool _pendingCaretNavigation;

		// Token: 0x04002614 RID: 9748
		private CaretElement _caretElement;

		// Token: 0x04002615 RID: 9749
		private bool _pendingUpdateCaretStateCallback;

		// Token: 0x0200090C RID: 2316
		private enum MovingEdge
		{
			// Token: 0x0400431F RID: 17183
			Start,
			// Token: 0x04004320 RID: 17184
			StartInner,
			// Token: 0x04004321 RID: 17185
			EndInner,
			// Token: 0x04004322 RID: 17186
			End,
			// Token: 0x04004323 RID: 17187
			None
		}
	}
}
