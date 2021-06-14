using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003F5 RID: 1013
	internal static class TextEditorDragDrop
	{
		// Token: 0x06003868 RID: 14440 RVA: 0x000FC8A0 File Offset: 0x000FAAA0
		internal static void _RegisterClassHandlers(Type controlType, bool readOnly, bool registerEventListeners)
		{
			if (!readOnly)
			{
				EventManager.RegisterClassHandler(controlType, DragDrop.DropEvent, new DragEventHandler(TextEditorDragDrop.OnClearState), true);
				EventManager.RegisterClassHandler(controlType, DragDrop.DragLeaveEvent, new DragEventHandler(TextEditorDragDrop.OnClearState), true);
			}
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, DragDrop.QueryContinueDragEvent, new QueryContinueDragEventHandler(TextEditorDragDrop.OnQueryContinueDrag));
				EventManager.RegisterClassHandler(controlType, DragDrop.GiveFeedbackEvent, new GiveFeedbackEventHandler(TextEditorDragDrop.OnGiveFeedback));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragEnterEvent, new DragEventHandler(TextEditorDragDrop.OnDragEnter));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragOverEvent, new DragEventHandler(TextEditorDragDrop.OnDragOver));
				EventManager.RegisterClassHandler(controlType, DragDrop.DragLeaveEvent, new DragEventHandler(TextEditorDragDrop.OnDragLeave));
				if (!readOnly)
				{
					EventManager.RegisterClassHandler(controlType, DragDrop.DropEvent, new DragEventHandler(TextEditorDragDrop.OnDrop));
				}
			}
		}

		// Token: 0x06003869 RID: 14441 RVA: 0x000FC974 File Offset: 0x000FAB74
		internal static void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
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
			e.Handled = true;
			e.Action = DragAction.Continue;
			bool flag = (e.KeyStates & DragDropKeyStates.LeftMouseButton) == DragDropKeyStates.None;
			if (e.EscapePressed)
			{
				e.Action = DragAction.Cancel;
				return;
			}
			if (flag)
			{
				e.Action = DragAction.Drop;
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x000FC9CC File Offset: 0x000FABCC
		internal static void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
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
			e.UseDefaultCursors = true;
			e.Handled = true;
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x000FC9FC File Offset: 0x000FABFC
		internal static void OnDragEnter(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (!textEditor._IsEnabled || textEditor.TextView == null || textEditor.TextView.RenderScope == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (e.Data == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (TextEditorCopyPaste.GetPasteApplyFormat(textEditor, e.Data) == string.Empty)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			textEditor._dragDropProcess.TargetOnDragEnter(e);
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x000FCAB0 File Offset: 0x000FACB0
		internal static void OnDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (!textEditor._IsEnabled || textEditor.TextView == null || textEditor.TextView.RenderScope == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (e.Data == null)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			if (TextEditorCopyPaste.GetPasteApplyFormat(textEditor, e.Data) == string.Empty)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			textEditor._dragDropProcess.TargetOnDragOver(e);
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x000FCB64 File Offset: 0x000FAD64
		internal static void OnDragLeave(object sender, DragEventArgs e)
		{
			e.Handled = true;
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				e.Effects = DragDropEffects.None;
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope));
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x000FCBB8 File Offset: 0x000FADB8
		internal static void OnDrop(object sender, DragEventArgs e)
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
			TextEditorTyping._FlushPendingInputItems(textEditor);
			if (!textEditor.TextView.Validate(e.GetPosition(textEditor.TextView.RenderScope)))
			{
				return;
			}
			textEditor._dragDropProcess.TargetOnDrop(e);
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x000FCC0C File Offset: 0x000FAE0C
		internal static void OnClearState(object sender, DragEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor._dragDropProcess.DeleteCaret();
		}

		// Token: 0x02000900 RID: 2304
		internal class _DragDropProcess
		{
			// Token: 0x060085B9 RID: 34233 RVA: 0x0024A744 File Offset: 0x00248944
			internal _DragDropProcess(TextEditor textEditor)
			{
				Invariant.Assert(textEditor != null);
				this._textEditor = textEditor;
			}

			// Token: 0x060085BA RID: 34234 RVA: 0x0024A75C File Offset: 0x0024895C
			internal bool SourceOnMouseLeftButtonDown(Point mouseDownPoint)
			{
				ITextSelection selection = this._textEditor.Selection;
				if (this._textEditor.UiScope is PasswordBox)
				{
					this._dragStarted = false;
				}
				else
				{
					int num = (int)SystemParameters.MinimumHorizontalDragDistance;
					int num2 = (int)SystemParameters.MinimumVerticalDragDistance;
					this._dragRect = new Rect(mouseDownPoint.X - (double)num, mouseDownPoint.Y - (double)num2, (double)(num * 2), (double)(num2 * 2));
					this._dragStarted = selection.Contains(mouseDownPoint);
				}
				return this._dragStarted;
			}

			// Token: 0x060085BB RID: 34235 RVA: 0x0024A7D8 File Offset: 0x002489D8
			internal void DoMouseLeftButtonUp(MouseButtonEventArgs e)
			{
				if (this._dragStarted)
				{
					if (this.TextView.IsValid)
					{
						Point position = e.GetPosition(this._textEditor.TextView.RenderScope);
						ITextPointer textPositionFromPoint = this.TextView.GetTextPositionFromPoint(position, true);
						if (textPositionFromPoint != null)
						{
							this._textEditor.Selection.SetSelectionByMouse(textPositionFromPoint, position);
						}
					}
					this._dragStarted = false;
				}
			}

			// Token: 0x060085BC RID: 34236 RVA: 0x0024A83C File Offset: 0x00248A3C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal bool SourceOnMouseMove(Point mouseMovePoint)
			{
				if (!this._dragStarted || !SecurityHelper.CheckUnmanagedCodePermission())
				{
					return false;
				}
				if (!this.InitialThresholdCrossed(mouseMovePoint))
				{
					return true;
				}
				ITextSelection selection = this._textEditor.Selection;
				this._dragStarted = false;
				this._dragSourceTextRange = new TextRange(selection.Start, selection.End);
				IDataObject dataObject = TextEditorCopyPaste._CreateDataObject(this._textEditor, true);
				if (dataObject != null)
				{
					this.SourceDoDragDrop(selection, dataObject);
					this._textEditor.UiScope.ReleaseMouseCapture();
					return true;
				}
				return false;
			}

			// Token: 0x060085BD RID: 34237 RVA: 0x0024A8BA File Offset: 0x00248ABA
			private bool InitialThresholdCrossed(Point dragPoint)
			{
				return !this._dragRect.Contains(dragPoint.X, dragPoint.Y);
			}

			// Token: 0x060085BE RID: 34238 RVA: 0x0024A8D8 File Offset: 0x00248AD8
			private void SourceDoDragDrop(ITextSelection selection, IDataObject dataObject)
			{
				DragDropEffects dragDropEffects = DragDropEffects.Copy;
				if (!this._textEditor.IsReadOnly)
				{
					dragDropEffects |= DragDropEffects.Move;
				}
				DragDropEffects dragDropEffects2 = DragDropEffects.None;
				try
				{
					dragDropEffects2 = DragDrop.DoDragDrop(this._textEditor.UiScope, dataObject, dragDropEffects);
				}
				catch (COMException ex) when (ex.HResult == -2147418113)
				{
				}
				if (!this._textEditor.IsReadOnly && dragDropEffects2 == DragDropEffects.Move && this._dragSourceTextRange != null && !this._dragSourceTextRange.IsEmpty)
				{
					using (selection.DeclareChangeBlock())
					{
						this._dragSourceTextRange.Text = string.Empty;
					}
				}
				this._dragSourceTextRange = null;
				if (!this._textEditor.IsReadOnly)
				{
					BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this._textEditor.UiScope, TextBox.TextProperty);
					if (bindingExpressionBase != null)
					{
						bindingExpressionBase.UpdateSource();
						bindingExpressionBase.UpdateTarget();
					}
				}
			}

			// Token: 0x060085BF RID: 34239 RVA: 0x0024A9D4 File Offset: 0x00248BD4
			internal void TargetEnsureDropCaret()
			{
				if (this._caretDragDrop == null)
				{
					this._caretDragDrop = new CaretElement(this._textEditor, false);
					this._caretDragDrop.Hide();
				}
			}

			// Token: 0x060085C0 RID: 34240 RVA: 0x0024A9FC File Offset: 0x00248BFC
			internal void TargetOnDragEnter(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				if ((e.AllowedEffects & DragDropEffects.Move) != DragDropEffects.None)
				{
					e.Effects = DragDropEffects.Move;
				}
				bool flag = (e.KeyStates & DragDropKeyStates.ControlKey) > DragDropKeyStates.None;
				if (flag)
				{
					e.Effects |= DragDropEffects.Copy;
				}
				this.TargetEnsureDropCaret();
			}

			// Token: 0x060085C1 RID: 34241 RVA: 0x0024AA48 File Offset: 0x00248C48
			internal void TargetOnDragOver(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				if ((e.AllowedEffects & DragDropEffects.Move) != DragDropEffects.None)
				{
					e.Effects = DragDropEffects.Move;
				}
				bool flag = (e.KeyStates & DragDropKeyStates.ControlKey) > DragDropKeyStates.None;
				if (flag)
				{
					e.Effects |= DragDropEffects.Copy;
				}
				if (this._caretDragDrop != null)
				{
					if (!this._textEditor.TextView.Validate(e.GetPosition(this._textEditor.TextView.RenderScope)))
					{
						return;
					}
					FrameworkElement scroller = this._textEditor._Scroller;
					if (scroller != null)
					{
						IScrollInfo scrollInfo = scroller as IScrollInfo;
						if (scrollInfo == null && scroller is ScrollViewer)
						{
							scrollInfo = ((ScrollViewer)scroller).ScrollInfo;
						}
						Invariant.Assert(scrollInfo != null);
						Point position = e.GetPosition(scroller);
						double num = (double)this._textEditor.UiScope.GetValue(TextEditor.PageHeightProperty);
						double num2 = 16.0;
						if (position.Y < num2)
						{
							if (position.Y > num2 / 2.0)
							{
								scrollInfo.LineUp();
							}
							else
							{
								scrollInfo.PageUp();
							}
						}
						else if (position.Y > num - num2)
						{
							if (position.Y < num - num2 / 2.0)
							{
								scrollInfo.LineDown();
							}
							else
							{
								scrollInfo.PageDown();
							}
						}
					}
					this._textEditor.TextView.RenderScope.UpdateLayout();
					if (this._textEditor.TextView.IsValid)
					{
						ITextPointer dropPosition = this.GetDropPosition(this._textEditor.TextView.RenderScope, e.GetPosition(this._textEditor.TextView.RenderScope));
						if (dropPosition != null)
						{
							Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(dropPosition);
							object value = dropPosition.GetValue(TextElement.FontStyleProperty);
							bool italic = this._textEditor.AcceptsRichContent && value != DependencyProperty.UnsetValue && (FontStyle)value == FontStyles.Italic;
							Brush caretBrush = TextSelection.GetCaretBrush(this._textEditor);
							this._caretDragDrop.Update(true, rectangleFromTextPosition, caretBrush, 0.5, italic, CaretScrollMethod.None, double.NaN);
						}
					}
				}
			}

			// Token: 0x060085C2 RID: 34242 RVA: 0x0024AC64 File Offset: 0x00248E64
			private ITextPointer GetDropPosition(Visual target, Point point)
			{
				Invariant.Assert(target != null);
				Invariant.Assert(this._textEditor.TextView.IsValid);
				if (target != this._textEditor.TextView.RenderScope && target != null && this._textEditor.TextView.RenderScope.IsAncestorOf(target))
				{
					GeneralTransform generalTransform = target.TransformToAncestor(this._textEditor.TextView.RenderScope);
					generalTransform.TryTransform(point, out point);
				}
				ITextPointer textPointer = this.TextView.GetTextPositionFromPoint(point, true);
				if (textPointer != null)
				{
					textPointer = textPointer.GetInsertionPosition(textPointer.LogicalDirection);
					if (this._textEditor.AcceptsRichContent)
					{
						TextSegment normalizedLineRange = TextEditorSelection.GetNormalizedLineRange(this.TextView, textPointer);
						if (!normalizedLineRange.IsNull && textPointer.CompareTo(normalizedLineRange.End) < 0 && !TextPointerBase.IsAtWordBoundary(textPointer, LogicalDirection.Forward) && this._dragSourceTextRange != null && TextPointerBase.IsAtWordBoundary(this._dragSourceTextRange.Start, LogicalDirection.Forward) && TextPointerBase.IsAtWordBoundary(this._dragSourceTextRange.End, LogicalDirection.Forward))
						{
							TextSegment wordRange = TextPointerBase.GetWordRange(textPointer);
							string textInternal = TextRangeBase.GetTextInternal(wordRange.Start, wordRange.End);
							int offsetToPosition = wordRange.Start.GetOffsetToPosition(textPointer);
							textPointer = ((offsetToPosition < textInternal.Length / 2) ? wordRange.Start : wordRange.End);
						}
					}
				}
				return textPointer;
			}

			// Token: 0x060085C3 RID: 34243 RVA: 0x0024ADBC File Offset: 0x00248FBC
			internal void DeleteCaret()
			{
				if (this._caretDragDrop != null)
				{
					AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.TextView.RenderScope);
					adornerLayer.Remove(this._caretDragDrop);
					this._caretDragDrop = null;
				}
			}

			// Token: 0x060085C4 RID: 34244 RVA: 0x0024ADF8 File Offset: 0x00248FF8
			internal void TargetOnDrop(DragEventArgs e)
			{
				if (!this.AllowDragDrop(e))
				{
					return;
				}
				ITextSelection selection = this._textEditor.Selection;
				Invariant.Assert(selection != null);
				if (e.Data == null || e.AllowedEffects == DragDropEffects.None)
				{
					e.Effects = DragDropEffects.None;
					return;
				}
				if ((e.KeyStates & DragDropKeyStates.ControlKey) != DragDropKeyStates.None)
				{
					e.Effects = DragDropEffects.Copy;
				}
				else if (e.Effects != DragDropEffects.Copy)
				{
					e.Effects = DragDropEffects.Move;
				}
				if (!this._textEditor.TextView.Validate(e.GetPosition(this._textEditor.TextView.RenderScope)))
				{
					e.Effects = DragDropEffects.None;
					return;
				}
				ITextPointer dropPosition = this.GetDropPosition(this._textEditor.TextView.RenderScope, e.GetPosition(this._textEditor.TextView.RenderScope));
				if (dropPosition != null)
				{
					if (this._dragSourceTextRange != null && this._dragSourceTextRange.Start.TextContainer == selection.Start.TextContainer && !selection.IsEmpty && this.IsSelectionContainsDropPosition(selection, dropPosition))
					{
						selection.SetCaretToPosition(dropPosition, LogicalDirection.Backward, false, true);
						e.Effects = DragDropEffects.None;
						e.Handled = true;
					}
					else
					{
						using (selection.DeclareChangeBlock())
						{
							if ((e.Effects & DragDropEffects.Move) != DragDropEffects.None && this._dragSourceTextRange != null && this._dragSourceTextRange.Start.TextContainer == selection.Start.TextContainer)
							{
								this._dragSourceTextRange.Text = string.Empty;
							}
							selection.SetCaretToPosition(dropPosition, LogicalDirection.Backward, false, true);
							e.Handled = TextEditorCopyPaste._DoPaste(this._textEditor, e.Data, true);
						}
					}
					if (e.Handled)
					{
						this.Win32SetForegroundWindow();
						this._textEditor.UiScope.Focus();
						return;
					}
					e.Effects = DragDropEffects.None;
				}
			}

			// Token: 0x060085C5 RID: 34245 RVA: 0x0024AFC0 File Offset: 0x002491C0
			private bool IsSelectionContainsDropPosition(ITextSelection selection, ITextPointer dropPosition)
			{
				bool flag = selection.Contains(dropPosition);
				if (flag && selection.IsTableCellRange)
				{
					for (int i = 0; i < selection.TextSegments.Count; i++)
					{
						if (dropPosition.CompareTo(selection._TextSegments[i].End) == 0)
						{
							flag = false;
							break;
						}
					}
				}
				return flag;
			}

			// Token: 0x060085C6 RID: 34246 RVA: 0x0024B018 File Offset: 0x00249218
			private bool AllowDragDrop(DragEventArgs e)
			{
				if (!this._textEditor.IsReadOnly && this._textEditor.TextView != null && this._textEditor.TextView.RenderScope != null)
				{
					Window window = Window.GetWindow(this._textEditor.TextView.RenderScope);
					if (window == null)
					{
						return true;
					}
					WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
					if (SafeNativeMethods.IsWindowEnabled(new HandleRef(null, windowInteropHelper.Handle)))
					{
						return true;
					}
				}
				e.Effects = DragDropEffects.None;
				return false;
			}

			// Token: 0x060085C7 RID: 34247 RVA: 0x0024B090 File Offset: 0x00249290
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private void Win32SetForegroundWindow()
			{
				IntPtr intPtr = IntPtr.Zero;
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(this._textEditor.UiScope);
				if (presentationSource != null)
				{
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					try
					{
						intPtr = (presentationSource as IWin32Window).Handle;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.SetForegroundWindow(new HandleRef(null, intPtr));
				}
			}

			// Token: 0x17001E32 RID: 7730
			// (get) Token: 0x060085C8 RID: 34248 RVA: 0x0024B104 File Offset: 0x00249304
			private ITextView TextView
			{
				get
				{
					return this._textEditor.TextView;
				}
			}

			// Token: 0x040042F8 RID: 17144
			private TextEditor _textEditor;

			// Token: 0x040042F9 RID: 17145
			private ITextRange _dragSourceTextRange;

			// Token: 0x040042FA RID: 17146
			private bool _dragStarted;

			// Token: 0x040042FB RID: 17147
			private CaretElement _caretDragDrop;

			// Token: 0x040042FC RID: 17148
			private Rect _dragRect;
		}
	}
}
