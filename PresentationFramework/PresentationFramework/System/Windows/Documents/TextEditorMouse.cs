using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003F7 RID: 1015
	internal static class TextEditorMouse
	{
		// Token: 0x0600387D RID: 14461 RVA: 0x000FD270 File Offset: 0x000FB470
		internal static void _RegisterClassHandlers(Type controlType, bool registerEventListeners)
		{
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Mouse.QueryCursorEvent, new QueryCursorEventHandler(TextEditorMouse.OnQueryCursor));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseDownEvent, new MouseButtonEventHandler(TextEditorMouse.OnMouseDown));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseMoveEvent, new MouseEventHandler(TextEditorMouse.OnMouseMove));
				EventManager.RegisterClassHandler(controlType, Mouse.MouseUpEvent, new MouseButtonEventHandler(TextEditorMouse.OnMouseUp));
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x000FD2DC File Offset: 0x000FB4DC
		internal static void SetCaretPositionOnMouseEvent(TextEditor This, Point mouseDownPoint, MouseButton changedButton, int clickCount)
		{
			ITextPointer textPositionFromPoint = This.TextView.GetTextPositionFromPoint(mouseDownPoint, true);
			if (textPositionFromPoint == null)
			{
				TextEditorMouse.MoveFocusToUiScope(This);
				return;
			}
			TextEditorSelection._ClearSuggestedX(This);
			TextEditorTyping._BreakTypingSequence(This);
			if (This.Selection is TextSelection)
			{
				((TextSelection)This.Selection).ClearSpringloadFormatting();
			}
			This._forceWordSelection = false;
			This._forceParagraphSelection = false;
			if (changedButton == MouseButton.Right || clickCount == 1)
			{
				if (changedButton != MouseButton.Left || !This._dragDropProcess.SourceOnMouseLeftButtonDown(mouseDownPoint))
				{
					This.Selection.SetSelectionByMouse(textPositionFromPoint, mouseDownPoint);
					return;
				}
			}
			else
			{
				if (clickCount == 2 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None && This.Selection.IsEmpty)
				{
					This._forceWordSelection = true;
					This._forceParagraphSelection = false;
					This.Selection.SelectWord(textPositionFromPoint);
					return;
				}
				if (clickCount == 3 && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None && This.AcceptsRichContent)
				{
					This._forceParagraphSelection = true;
					This._forceWordSelection = false;
					This.Selection.SelectParagraph(textPositionFromPoint);
				}
			}
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x000FD3C4 File Offset: 0x000FB5C4
		internal static bool IsPointWithinInteractiveArea(TextEditor textEditor, Point point)
		{
			bool flag = TextEditorMouse.IsPointWithinRenderScope(textEditor, point);
			if (flag)
			{
				flag = textEditor.TextView.IsValid;
				if (flag)
				{
					GeneralTransform generalTransform = textEditor.UiScope.TransformToDescendant(textEditor.TextView.RenderScope);
					if (generalTransform != null)
					{
						generalTransform.TryTransform(point, out point);
					}
					ITextPointer textPositionFromPoint = textEditor.TextView.GetTextPositionFromPoint(point, true);
					flag = (textPositionFromPoint != null);
				}
			}
			return flag;
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x000FD424 File Offset: 0x000FB624
		internal static void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor.CloseToolTip();
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (!textEditor.UiScope.Focusable)
			{
				return;
			}
			if (e.ButtonState == MouseButtonState.Released)
			{
				return;
			}
			e.Handled = true;
			TextEditorMouse.MoveFocusToUiScope(textEditor);
			if (textEditor.UiScope != Keyboard.FocusedElement)
			{
				return;
			}
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			if (textEditor.TextView == null)
			{
				return;
			}
			textEditor.CompleteComposition();
			if (!textEditor.TextView.IsValid)
			{
				textEditor.TextView.RenderScope.UpdateLayout();
				if (textEditor.TextView == null || !textEditor.TextView.IsValid)
				{
					return;
				}
			}
			if (!TextEditorMouse.IsPointWithinInteractiveArea(textEditor, e.GetPosition(textEditor.UiScope)))
			{
				return;
			}
			textEditor.TextView.ThrottleBackgroundTasksForUserInput();
			Point position = e.GetPosition(textEditor.TextView.RenderScope);
			if (TextEditor.IsTableEditingEnabled && TextRangeEditTables.TableBorderHitTest(textEditor.TextView, position))
			{
				textEditor._tableColResizeInfo = TextRangeEditTables.StartColumnResize(textEditor.TextView, position);
				Invariant.Assert(textEditor._tableColResizeInfo != null);
				textEditor._mouseCapturingInProgress = true;
				try
				{
					textEditor.UiScope.CaptureMouse();
					return;
				}
				finally
				{
					textEditor._mouseCapturingInProgress = false;
				}
			}
			textEditor.Selection.BeginChange();
			try
			{
				TextEditorMouse.SetCaretPositionOnMouseEvent(textEditor, position, e.ChangedButton, e.ClickCount);
				textEditor._mouseCapturingInProgress = true;
				textEditor.UiScope.CaptureMouse();
			}
			finally
			{
				textEditor._mouseCapturingInProgress = false;
				textEditor.Selection.EndChange();
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x000FD5B0 File Offset: 0x000FB7B0
		internal static void OnMouseMove(object sender, MouseEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.TextView == null || !textEditor.TextView.IsValid)
			{
				return;
			}
			if (textEditor.UiScope.IsKeyboardFocused)
			{
				TextEditorMouse.OnMouseMoveWithFocus(textEditor, e);
				return;
			}
			TextEditorMouse.OnMouseMoveWithoutFocus(textEditor, e);
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x000FD604 File Offset: 0x000FB804
		internal static void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			if (e.RightButton != MouseButtonState.Released)
			{
				return;
			}
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.TextView == null || !textEditor.TextView.IsValid)
			{
				return;
			}
			if (!textEditor.UiScope.IsMouseCaptured)
			{
				return;
			}
			e.Handled = true;
			textEditor.CancelExtendSelection();
			Point position = e.GetPosition(textEditor.TextView.RenderScope);
			TextEditorMouse.UpdateCursor(textEditor, position);
			if (textEditor._tableColResizeInfo != null)
			{
				using (textEditor.Selection.DeclareChangeBlock())
				{
					textEditor._tableColResizeInfo.ResizeColumn(position);
					textEditor._tableColResizeInfo = null;
					goto IL_D5;
				}
			}
			using (textEditor.Selection.DeclareChangeBlock())
			{
				textEditor._dragDropProcess.DoMouseLeftButtonUp(e);
				textEditor._forceWordSelection = false;
				textEditor._forceParagraphSelection = false;
			}
			IL_D5:
			textEditor._mouseCapturingInProgress = true;
			try
			{
				textEditor.UiScope.ReleaseMouseCapture();
			}
			finally
			{
				textEditor._mouseCapturingInProgress = false;
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x000FD72C File Offset: 0x000FB92C
		internal static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (textEditor.TextView == null)
			{
				return;
			}
			if (TextEditorMouse.IsPointWithinInteractiveArea(textEditor, Mouse.GetPosition(textEditor.UiScope)))
			{
				e.Cursor = textEditor._cursor;
				e.Handled = true;
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000FD773 File Offset: 0x000FB973
		private static void OnMouseMoveWithoutFocus(TextEditor This, MouseEventArgs e)
		{
			TextEditorMouse.UpdateCursor(This, e.GetPosition(This.TextView.RenderScope));
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000FD78C File Offset: 0x000FB98C
		private static void OnMouseMoveWithFocus(TextEditor This, MouseEventArgs e)
		{
			if (This._mouseCapturingInProgress)
			{
				return;
			}
			TextEditor._ThreadLocalStore.PureControlShift = false;
			Point position = e.GetPosition(This.TextView.RenderScope);
			TextEditorMouse.UpdateCursor(This, position);
			Invariant.Assert(This.Selection != null);
			if (e.LeftButton != MouseButtonState.Pressed)
			{
				return;
			}
			if (!This.UiScope.IsMouseCaptured)
			{
				return;
			}
			This.TextView.ThrottleBackgroundTasksForUserInput();
			if (This._tableColResizeInfo != null)
			{
				This._tableColResizeInfo.UpdateAdorner(position);
				return;
			}
			e.Handled = true;
			Invariant.Assert(This.Selection != null);
			ITextPointer textPointer = This.TextView.GetTextPositionFromPoint(position, true);
			Invariant.Assert(This.Selection != null);
			if (textPointer == null)
			{
				This.RequestExtendSelection(position);
				return;
			}
			This.CancelExtendSelection();
			Invariant.Assert(This.Selection != null);
			if (!This._dragDropProcess.SourceOnMouseMove(position))
			{
				FrameworkElement scroller = This._Scroller;
				if (scroller != null && This.UiScope is TextBoxBase)
				{
					ITextPointer textPointer2 = null;
					Point point = new Point(position.X, position.Y);
					Point position2 = e.GetPosition(scroller);
					double num = ((TextBoxBase)This.UiScope).ViewportHeight;
					double num2 = 16.0;
					if (position2.Y < 0.0 - num2)
					{
						Rect rectangleFromTextPosition = This.TextView.GetRectangleFromTextPosition(textPointer);
						point = new Point(point.X, rectangleFromTextPosition.Bottom - num);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					else if (position2.Y > num + num2)
					{
						Rect rectangleFromTextPosition2 = This.TextView.GetRectangleFromTextPosition(textPointer);
						point = new Point(point.X, rectangleFromTextPosition2.Top + num);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					double num3 = ((TextBoxBase)This.UiScope).ViewportWidth;
					if (position2.X < 0.0)
					{
						point = new Point(point.X - num2, point.Y);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					else if (position2.X > num3)
					{
						point = new Point(point.X + num2, point.Y);
						textPointer2 = This.TextView.GetTextPositionFromPoint(point, true);
					}
					if (textPointer2 != null)
					{
						textPointer = textPointer2;
					}
				}
				using (This.Selection.DeclareChangeBlock())
				{
					if (textPointer.GetNextInsertionPosition(LogicalDirection.Forward) == null && textPointer.ParentType != null)
					{
						Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Backward);
						if (position.X > characterRect.X + characterRect.Width)
						{
							textPointer = This.TextContainer.End;
						}
					}
					This.Selection.ExtendSelectionByMouse(textPointer, This._forceWordSelection, This._forceParagraphSelection);
				}
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x000FDA64 File Offset: 0x000FBC64
		private static bool MoveFocusToUiScope(TextEditor This)
		{
			long contentChangeCounter = This._ContentChangeCounter;
			Visual visual = VisualTreeHelper.GetParent(This.UiScope) as Visual;
			while (visual != null && !(visual is ScrollViewer))
			{
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			if (visual != null)
			{
				((ScrollViewer)visual).AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
			}
			ITextSelection selection = This.Selection;
			try
			{
				selection.Changed += TextEditorMouse.OnSelectionChangedDuringGotFocus;
				TextEditorMouse._selectionChanged = false;
				This.UiScope.Focus();
			}
			finally
			{
				selection.Changed -= TextEditorMouse.OnSelectionChangedDuringGotFocus;
				if (visual != null)
				{
					((ScrollViewer)visual).RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
				}
			}
			return This.UiScope == Keyboard.FocusedElement && contentChangeCounter == This._ContentChangeCounter && !TextEditorMouse._selectionChanged;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x000FDB50 File Offset: 0x000FBD50
		private static void OnSelectionChangedDuringGotFocus(object sender, EventArgs e)
		{
			TextEditorMouse._selectionChanged = true;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x000FDB58 File Offset: 0x000FBD58
		private static void OnScrollChangedDuringGotFocus(object sender, ScrollChangedEventArgs e)
		{
			ScrollViewer scrollViewer = e.OriginalSource as ScrollViewer;
			if (scrollViewer != null)
			{
				scrollViewer.RemoveHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(TextEditorMouse.OnScrollChangedDuringGotFocus));
				scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - e.HorizontalChange);
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.VerticalChange);
			}
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x000FDBB4 File Offset: 0x000FBDB4
		private static void UpdateCursor(TextEditor This, Point mouseMovePoint)
		{
			Invariant.Assert(This.TextView != null && This.TextView.IsValid);
			Cursor cursor = Cursors.IBeam;
			if (TextEditor.IsTableEditingEnabled && TextRangeEditTables.TableBorderHitTest(This.TextView, mouseMovePoint))
			{
				cursor = Cursors.SizeWE;
			}
			else if (This.Selection != null && !This.UiScope.IsMouseCaptured)
			{
				if (This.Selection.IsEmpty)
				{
					UIElement uielementWhenMouseOver = TextEditorMouse.GetUIElementWhenMouseOver(This, mouseMovePoint);
					if (uielementWhenMouseOver != null && uielementWhenMouseOver.IsEnabled)
					{
						cursor = Cursors.Arrow;
					}
				}
				else if (This.UiScope.IsFocused && This.Selection.Contains(mouseMovePoint))
				{
					cursor = Cursors.Arrow;
				}
			}
			if (cursor != This._cursor)
			{
				This._cursor = cursor;
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x000FDC74 File Offset: 0x000FBE74
		private static UIElement GetUIElementWhenMouseOver(TextEditor This, Point mouseMovePoint)
		{
			ITextPointer textPositionFromPoint = This.TextView.GetTextPositionFromPoint(mouseMovePoint, false);
			if (textPositionFromPoint == null)
			{
				return null;
			}
			if (textPositionFromPoint.GetPointerContext(textPositionFromPoint.LogicalDirection) != TextPointerContext.EmbeddedElement)
			{
				return null;
			}
			ITextPointer textPointer = textPositionFromPoint.GetNextContextPosition(textPositionFromPoint.LogicalDirection);
			LogicalDirection gravity = (textPositionFromPoint.LogicalDirection == LogicalDirection.Forward) ? LogicalDirection.Backward : LogicalDirection.Forward;
			textPointer = textPointer.CreatePointer(0, gravity);
			Rect rectangleFromTextPosition = This.TextView.GetRectangleFromTextPosition(textPositionFromPoint);
			Rect rectangleFromTextPosition2 = This.TextView.GetRectangleFromTextPosition(textPointer);
			Rect rect = rectangleFromTextPosition;
			rect.Union(rectangleFromTextPosition2);
			if (!rect.Contains(mouseMovePoint))
			{
				return null;
			}
			return textPositionFromPoint.GetAdjacentElement(textPositionFromPoint.LogicalDirection) as UIElement;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000FDD10 File Offset: 0x000FBF10
		private static bool IsPointWithinRenderScope(TextEditor textEditor, Point point)
		{
			DependencyObject parent = textEditor.TextContainer.Parent;
			UIElement renderScope = textEditor.TextView.RenderScope;
			CaretElement caretElement = textEditor.Selection.CaretElement;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(textEditor.UiScope, point);
			if (hitTestResult != null)
			{
				bool flag = false;
				if (hitTestResult.VisualHit is Visual)
				{
					flag = ((Visual)hitTestResult.VisualHit).IsDescendantOf(renderScope);
				}
				if (hitTestResult.VisualHit is Visual3D)
				{
					flag = ((Visual3D)hitTestResult.VisualHit).IsDescendantOf(renderScope);
				}
				if (hitTestResult.VisualHit == renderScope || flag || hitTestResult.VisualHit == caretElement)
				{
					return true;
				}
			}
			DependencyObject dependencyObject = textEditor.UiScope.InputHitTest(point) as DependencyObject;
			while (dependencyObject != null)
			{
				if (dependencyObject == parent || dependencyObject == renderScope || dependencyObject == caretElement)
				{
					return true;
				}
				if (dependencyObject is FrameworkElement && ((FrameworkElement)dependencyObject).TemplatedParent == textEditor.UiScope)
				{
					dependencyObject = null;
				}
				else if (dependencyObject is Visual)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
				}
				else if (dependencyObject is FrameworkContentElement)
				{
					dependencyObject = ((FrameworkContentElement)dependencyObject).Parent;
				}
				else
				{
					dependencyObject = null;
				}
			}
			return false;
		}

		// Token: 0x04002599 RID: 9625
		private static bool _selectionChanged;
	}
}
