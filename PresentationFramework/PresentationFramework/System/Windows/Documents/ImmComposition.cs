using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200037B RID: 891
	internal class ImmComposition
	{
		// Token: 0x0600305C RID: 12380 RVA: 0x000D93F8 File Offset: 0x000D75F8
		[SecurityCritical]
		internal ImmComposition(HwndSource source)
		{
			this.UpdateSource(null, source);
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000D9408 File Offset: 0x000D7608
		[SecurityCritical]
		internal static ImmComposition GetImmComposition(FrameworkElement scope)
		{
			HwndSource hwndSource = PresentationSource.CriticalFromVisual(scope) as HwndSource;
			ImmComposition immComposition = null;
			if (hwndSource != null)
			{
				Hashtable list = ImmComposition._list;
				lock (list)
				{
					immComposition = (ImmComposition)ImmComposition._list[hwndSource];
					if (immComposition == null)
					{
						immComposition = new ImmComposition(hwndSource);
						ImmComposition._list[hwndSource] = immComposition;
					}
				}
			}
			return immComposition;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000D947C File Offset: 0x000D767C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDetach(TextEditor editor)
		{
			if (editor != this._editor)
			{
				return;
			}
			if (this._editor != null)
			{
				PresentationSource.RemoveSourceChangedHandler(this.UiScope, new SourceChangedEventHandler(this.OnSourceChanged));
				this._editor.TextContainer.Change -= this.OnTextContainerChange;
			}
			this._editor = null;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000D94D8 File Offset: 0x000D76D8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnGotFocus(TextEditor editor)
		{
			if (editor == this._editor)
			{
				return;
			}
			if (this._editor != null)
			{
				PresentationSource.RemoveSourceChangedHandler(this.UiScope, new SourceChangedEventHandler(this.OnSourceChanged));
				this._editor.TextContainer.Change -= this.OnTextContainerChange;
			}
			this._editor = editor;
			PresentationSource.AddSourceChangedHandler(this.UiScope, new SourceChangedEventHandler(this.OnSourceChanged));
			this._editor.TextContainer.Change += this.OnTextContainerChange;
			this.UpdateNearCaretCompositionWindow();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000D956C File Offset: 0x000D776C
		internal void OnLostFocus()
		{
			if (this._editor == null)
			{
				return;
			}
			this._losingFocus = true;
			try
			{
				this.CompleteComposition();
			}
			finally
			{
				this._losingFocus = false;
			}
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000D95AC File Offset: 0x000D77AC
		internal void OnLayoutUpdated()
		{
			if (this._updateCompWndPosAtNextLayoutUpdate && this.IsReadingWindowIme())
			{
				this.UpdateNearCaretCompositionWindow();
			}
			this._updateCompWndPosAtNextLayoutUpdate = false;
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000D95CC File Offset: 0x000D77CC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void CompleteComposition()
		{
			this.UnregisterMouseListeners();
			if (this._source == null)
			{
				return;
			}
			this._compositionModifiedByEventListener = true;
			IntPtr handle = IntPtr.Zero;
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			try
			{
				handle = ((IWin32Window)this._source).Handle;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(this, handle));
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.ImmNotifyIME(new HandleRef(this, intPtr), 21, 1, 0);
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(this, handle), new HandleRef(this, intPtr));
			}
			if (this._compositionAdorner != null)
			{
				this._compositionAdorner.Uninitialize();
				this._compositionAdorner = null;
			}
			this._startComposition = null;
			this._endComposition = null;
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000D968C File Offset: 0x000D788C
		internal void OnSelectionChange()
		{
			this._compositionModifiedByEventListener = true;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000D9695 File Offset: 0x000D7895
		internal void OnSelectionChanged()
		{
			if (!this.IsInKeyboardFocus)
			{
				return;
			}
			this.UpdateNearCaretCompositionWindow();
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x000D96A6 File Offset: 0x000D78A6
		internal bool IsComposition
		{
			get
			{
				return this._startComposition != null;
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000D96B4 File Offset: 0x000D78B4
		[SecurityCritical]
		private void OnSourceChanged(object sender, SourceChangedEventArgs e)
		{
			HwndSource newSource = null;
			HwndSource hwndSource = null;
			new UIPermission(PermissionState.Unrestricted).Assert();
			try
			{
				newSource = (e.NewSource as HwndSource);
				hwndSource = (e.OldSource as HwndSource);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.UpdateSource(hwndSource, newSource);
			if (hwndSource != null && this.UiScope != null)
			{
				PresentationSource.RemoveSourceChangedHandler(this.UiScope, new SourceChangedEventHandler(this.OnSourceChanged));
			}
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000D972C File Offset: 0x000D792C
		[SecurityCritical]
		private void UpdateSource(HwndSource oldSource, HwndSource newSource)
		{
			this.OnDetach(this._editor);
			if (this._source != null)
			{
				new UIPermission(UIPermissionWindow.AllWindows).Assert();
				try
				{
					this._source.RemoveHook(new HwndSourceHook(this.ImmCompositionFilterMessage));
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this._source.Disposed -= this.OnHwndDisposed;
				ImmComposition._list.Remove(this._source);
				this._source = null;
			}
			if (newSource != null)
			{
				ImmComposition._list[newSource] = this;
				this._source = newSource;
				new UIPermission(UIPermissionWindow.AllWindows).Assert();
				try
				{
					this._source.AddHook(new HwndSourceHook(this.ImmCompositionFilterMessage));
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this._source.Disposed += this.OnHwndDisposed;
			}
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000D9818 File Offset: 0x000D7A18
		[SecurityCritical]
		private IntPtr ImmCompositionFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr result = IntPtr.Zero;
			if (msg <= 271)
			{
				if (msg != 81)
				{
					if (msg - 269 > 1)
					{
						if (msg == 271)
						{
							this.OnWmImeComposition(hwnd, lParam, ref handled);
						}
					}
					else if (this.IsInKeyboardFocus && !this.IsReadOnly && !this.IsReadingWindowIme())
					{
						handled = true;
					}
				}
				else if (this.IsReadingWindowIme())
				{
					this.UpdateNearCaretCompositionWindow();
				}
			}
			else if (msg != 642)
			{
				if (msg != 646)
				{
					if (msg == 648)
					{
						result = this.OnWmImeRequest(wParam, lParam, ref handled);
					}
				}
				else
				{
					this.OnWmImeChar(wParam, ref handled);
				}
			}
			else
			{
				this.OnWmImeNotify(hwnd, wParam);
			}
			return result;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000D98C8 File Offset: 0x000D7AC8
		[SecurityCritical]
		private void OnWmImeComposition(IntPtr hwnd, IntPtr lParam, ref bool handled)
		{
			int caretOffset = 0;
			int deltaStart = 0;
			char[] array = null;
			char[] array2 = null;
			byte[] array3 = null;
			if (this.IsReadingWindowIme())
			{
				return;
			}
			if (!this.IsInKeyboardFocus && !this._losingFocus)
			{
				return;
			}
			if (this.IsReadOnly)
			{
				return;
			}
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(this, hwnd));
			if (intPtr == IntPtr.Zero)
			{
				return;
			}
			if (((int)lParam & 2048) != 0)
			{
				int num = UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 2048, IntPtr.Zero, 0);
				if (num > 0)
				{
					array = new char[num / Marshal.SizeOf(typeof(short))];
					UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 2048, array, num);
				}
			}
			if (((int)lParam & 8) != 0)
			{
				int num = UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 8, IntPtr.Zero, 0);
				if (num > 0)
				{
					array2 = new char[num / Marshal.SizeOf(typeof(short))];
					UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 8, array2, num);
					if (((int)lParam & 128) != 0)
					{
						caretOffset = UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 128, IntPtr.Zero, 0);
					}
					if (((int)lParam & 256) != 0)
					{
						deltaStart = UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 256, IntPtr.Zero, 0);
					}
					if (((int)lParam & 16) != 0)
					{
						num = UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 16, IntPtr.Zero, 0);
						if (num > 0)
						{
							array3 = new byte[num / Marshal.SizeOf(typeof(byte))];
							UnsafeNativeMethods.ImmGetCompositionString(new HandleRef(this, intPtr), 16, array3, num);
						}
					}
				}
			}
			this.UpdateCompositionString(array, array2, caretOffset, deltaStart, array3);
			UnsafeNativeMethods.ImmReleaseContext(new HandleRef(this, hwnd), new HandleRef(this, intPtr));
			handled = true;
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000D9A88 File Offset: 0x000D7C88
		[SecurityCritical]
		private void OnWmImeChar(IntPtr wParam, ref bool handled)
		{
			if (!this.IsInKeyboardFocus && !this._losingFocus)
			{
				return;
			}
			if (this.IsReadOnly)
			{
				return;
			}
			if (this._handlingImeMessage)
			{
				return;
			}
			this._handlingImeMessage = true;
			try
			{
				int resultLength;
				string text = this.BuildCompositionString(null, new char[]
				{
					(char)((int)wParam)
				}, out resultLength);
				if (text == null)
				{
					this.CompleteComposition();
				}
				else
				{
					FrameworkTextComposition composition = TextStore.CreateComposition(this._editor, this);
					this._compositionModifiedByEventListener = false;
					this._caretOffset = 1;
					bool flag = this.RaiseTextInputStartEvent(composition, resultLength, text);
					if (flag)
					{
						this.CompleteComposition();
					}
					else
					{
						bool flag2 = this.RaiseTextInputEvent(composition, text);
						if (flag2)
						{
							this.CompleteComposition();
							goto IL_AD;
						}
					}
				}
			}
			finally
			{
				this._handlingImeMessage = false;
			}
			if (this.IsReadingWindowIme())
			{
				this.UpdateNearCaretCompositionWindow();
			}
			IL_AD:
			handled = true;
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000D9B58 File Offset: 0x000D7D58
		[SecurityCritical]
		private void OnWmImeNotify(IntPtr hwnd, IntPtr wParam)
		{
			if (!this.IsInKeyboardFocus)
			{
				return;
			}
			if ((int)wParam == 5)
			{
				IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(this, hwnd));
				if (intPtr != IntPtr.Zero)
				{
					NativeMethods.CANDIDATEFORM candidateform = default(NativeMethods.CANDIDATEFORM);
					if (this.IsReadingWindowIme())
					{
						candidateform.dwIndex = 0;
						candidateform.dwStyle = 0;
						candidateform.rcArea.left = 0;
						candidateform.rcArea.right = 0;
						candidateform.rcArea.top = 0;
						candidateform.rcArea.bottom = 0;
						candidateform.ptCurrentPos = new NativeMethods.POINT(0, 0);
					}
					else
					{
						CompositionTarget compositionTarget = this._source.CompositionTarget;
						ITextPointer textPointer;
						if (this._startComposition != null)
						{
							textPointer = this._startComposition.CreatePointer();
						}
						else
						{
							textPointer = this._editor.Selection.Start.CreatePointer();
						}
						ITextPointer textPointer2;
						if (this._endComposition != null)
						{
							textPointer2 = this._endComposition.CreatePointer();
						}
						else
						{
							textPointer2 = this._editor.Selection.End.CreatePointer();
						}
						ITextPointer textPointer3;
						if (this._startComposition != null)
						{
							textPointer3 = ((this._caretOffset > 0) ? this._startComposition.CreatePointer(this._caretOffset, LogicalDirection.Forward) : this._endComposition);
						}
						else
						{
							textPointer3 = this._editor.Selection.End.CreatePointer();
						}
						ITextPointer textPointer4 = textPointer.CreatePointer(LogicalDirection.Forward);
						ITextPointer textPointer5 = textPointer2.CreatePointer(LogicalDirection.Backward);
						ITextPointer textPointer6 = textPointer3.CreatePointer(LogicalDirection.Forward);
						if (!textPointer4.ValidateLayout() || !textPointer5.ValidateLayout() || !textPointer6.ValidateLayout())
						{
							return;
						}
						ITextView textView = TextEditor.GetTextView(this.RenderScope);
						Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPointer4);
						Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer5);
						Rect rectangleFromTextPosition3 = textView.GetRectangleFromTextPosition(textPointer6);
						Point point = new Point(Math.Min(rectangleFromTextPosition.Left, rectangleFromTextPosition2.Left), Math.Min(rectangleFromTextPosition.Top, rectangleFromTextPosition2.Top));
						Point point2 = new Point(Math.Max(rectangleFromTextPosition.Left, rectangleFromTextPosition2.Left), Math.Max(rectangleFromTextPosition.Bottom, rectangleFromTextPosition2.Bottom));
						Point point3 = new Point(rectangleFromTextPosition3.Left, rectangleFromTextPosition3.Bottom);
						GeneralTransform generalTransform = this.RenderScope.TransformToAncestor(compositionTarget.RootVisual);
						generalTransform.TryTransform(point, out point);
						generalTransform.TryTransform(point2, out point2);
						generalTransform.TryTransform(point3, out point3);
						point = compositionTarget.TransformToDevice.Transform(point);
						point2 = compositionTarget.TransformToDevice.Transform(point2);
						point3 = compositionTarget.TransformToDevice.Transform(point3);
						candidateform.dwIndex = 0;
						candidateform.dwStyle = 128;
						candidateform.rcArea.left = this.ConvertToInt32(point.X);
						candidateform.rcArea.right = this.ConvertToInt32(point2.X);
						candidateform.rcArea.top = this.ConvertToInt32(point.Y);
						candidateform.rcArea.bottom = this.ConvertToInt32(point2.Y);
						candidateform.ptCurrentPos = new NativeMethods.POINT(this.ConvertToInt32(point3.X), this.ConvertToInt32(point3.Y));
					}
					UnsafeNativeMethods.ImmSetCandidateWindow(new HandleRef(this, intPtr), ref candidateform);
					UnsafeNativeMethods.ImmReleaseContext(new HandleRef(this, hwnd), new HandleRef(this, intPtr));
				}
			}
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000D9EB4 File Offset: 0x000D80B4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void UpdateNearCaretCompositionWindow()
		{
			if (!this.IsInKeyboardFocus)
			{
				return;
			}
			if (this._source == null)
			{
				return;
			}
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			IntPtr handle;
			try
			{
				handle = ((IWin32Window)this._source).Handle;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			Rect visualContentBounds = this.UiScope.VisualContentBounds;
			ITextView textView = this._editor.TextView;
			if (!this._editor.Selection.End.HasValidLayout)
			{
				this._updateCompWndPosAtNextLayoutUpdate = true;
				return;
			}
			CompositionTarget compositionTarget = this._source.CompositionTarget;
			if (compositionTarget == null || compositionTarget.RootVisual == null)
			{
				return;
			}
			if (!compositionTarget.RootVisual.IsAncestorOf(this.RenderScope))
			{
				return;
			}
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(this, handle));
			if (intPtr != IntPtr.Zero)
			{
				Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(this._editor.Selection.End.CreatePointer(LogicalDirection.Backward));
				Point point = new Point(visualContentBounds.Left, visualContentBounds.Top);
				Point point2 = new Point(visualContentBounds.Right, visualContentBounds.Bottom);
				Point point3 = new Point(rectangleFromTextPosition.Left, rectangleFromTextPosition.Bottom);
				GeneralTransform generalTransform = this.RenderScope.TransformToAncestor(compositionTarget.RootVisual);
				generalTransform.TryTransform(point, out point);
				generalTransform.TryTransform(point2, out point2);
				generalTransform.TryTransform(point3, out point3);
				point = compositionTarget.TransformToDevice.Transform(point);
				point2 = compositionTarget.TransformToDevice.Transform(point2);
				point3 = compositionTarget.TransformToDevice.Transform(point3);
				NativeMethods.COMPOSITIONFORM compositionform = default(NativeMethods.COMPOSITIONFORM);
				compositionform.dwStyle = 1;
				compositionform.rcArea.left = this.ConvertToInt32(point.X);
				compositionform.rcArea.right = this.ConvertToInt32(point2.X);
				compositionform.rcArea.top = this.ConvertToInt32(point.Y);
				compositionform.rcArea.bottom = this.ConvertToInt32(point2.Y);
				compositionform.ptCurrentPos = new NativeMethods.POINT(this.ConvertToInt32(point3.X), this.ConvertToInt32(point3.Y));
				UnsafeNativeMethods.ImmSetCompositionWindow(new HandleRef(this, intPtr), ref compositionform);
				UnsafeNativeMethods.ImmReleaseContext(new HandleRef(this, handle), new HandleRef(this, intPtr));
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000DA114 File Offset: 0x000D8314
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnHwndDisposed(object sender, EventArgs args)
		{
			this.UpdateSource(this._source, null);
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000DA124 File Offset: 0x000D8324
		private void UpdateCompositionString(char[] resultChars, char[] compositionChars, int caretOffset, int deltaStart, byte[] attributes)
		{
			if (this._handlingImeMessage)
			{
				return;
			}
			this._handlingImeMessage = true;
			try
			{
				if (this._compositionAdorner != null)
				{
					this._compositionAdorner.Uninitialize();
					this._compositionAdorner = null;
				}
				int resultLength;
				string text = this.BuildCompositionString(resultChars, compositionChars, out resultLength);
				if (text == null)
				{
					this.CompleteComposition();
				}
				else
				{
					this.RecordCaretOffset(caretOffset, attributes, text.Length);
					FrameworkTextComposition composition = TextStore.CreateComposition(this._editor, this);
					this._compositionModifiedByEventListener = false;
					if (this._startComposition == null)
					{
						Invariant.Assert(this._endComposition == null);
						bool flag = this.RaiseTextInputStartEvent(composition, resultLength, text);
						if (flag)
						{
							this.CompleteComposition();
							return;
						}
					}
					else if (compositionChars != null)
					{
						bool flag2 = this.RaiseTextInputUpdateEvent(composition, resultLength, text);
						if (flag2)
						{
							this.CompleteComposition();
							return;
						}
					}
					if (compositionChars == null)
					{
						bool flag3 = this.RaiseTextInputEvent(composition, text);
						if (flag3)
						{
							this.CompleteComposition();
							return;
						}
					}
					if (this._startComposition != null)
					{
						this.SetCompositionAdorner(attributes);
					}
				}
			}
			finally
			{
				this._handlingImeMessage = false;
			}
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000DA220 File Offset: 0x000D8420
		private string BuildCompositionString(char[] resultChars, char[] compositionChars, out int resultLength)
		{
			int num = (compositionChars == null) ? 0 : compositionChars.Length;
			resultLength = ((resultChars == null) ? 0 : resultChars.Length);
			char[] array;
			if (resultChars == null)
			{
				array = compositionChars;
			}
			else if (compositionChars == null)
			{
				array = resultChars;
			}
			else
			{
				array = new char[resultLength + num];
				Array.Copy(resultChars, 0, array, 0, resultLength);
				Array.Copy(compositionChars, 0, array, resultLength, num);
			}
			string text = new string(array);
			int num2 = (array == null) ? 0 : array.Length;
			if (text.Length != num2)
			{
				return null;
			}
			return text;
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000DA28D File Offset: 0x000D848D
		private void RecordCaretOffset(int caretOffset, byte[] attributes, int compositionLength)
		{
			if (attributes != null && ((caretOffset >= 0 && caretOffset < attributes.Length && attributes[caretOffset] == 0) || (caretOffset > 0 && caretOffset - 1 < attributes.Length && attributes[caretOffset - 1] == 0)))
			{
				this._caretOffset = caretOffset;
				return;
			}
			this._caretOffset = -1;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000DA2C4 File Offset: 0x000D84C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool RaiseTextInputStartEvent(FrameworkTextComposition composition, int resultLength, string compositionString)
		{
			composition.Stage = TextCompositionStage.None;
			composition.SetCompositionPositions(this._editor.Selection.Start, this._editor.Selection.End, compositionString);
			if (TextCompositionManager.StartComposition(composition) || composition.PendingComplete || this._compositionModifiedByEventListener)
			{
				return true;
			}
			this.UpdateCompositionText(composition, resultLength, true, out this._startComposition, out this._endComposition);
			if (this._compositionModifiedByEventListener)
			{
				return true;
			}
			this.RegisterMouseListeners();
			return false;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000DA344 File Offset: 0x000D8544
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool RaiseTextInputUpdateEvent(FrameworkTextComposition composition, int resultLength, string compositionString)
		{
			composition.Stage = TextCompositionStage.Started;
			composition.SetCompositionPositions(this._startComposition, this._endComposition, compositionString);
			if (TextCompositionManager.UpdateComposition(composition) || composition.PendingComplete || this._compositionModifiedByEventListener)
			{
				return true;
			}
			this.UpdateCompositionText(composition, resultLength, false, out this._startComposition, out this._endComposition);
			return this._compositionModifiedByEventListener;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000DA3A8 File Offset: 0x000D85A8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool RaiseTextInputEvent(FrameworkTextComposition composition, string compositionString)
		{
			composition.Stage = TextCompositionStage.Started;
			composition.SetResultPositions(this._startComposition, this._endComposition, compositionString);
			this._startComposition = null;
			this._endComposition = null;
			this.UnregisterMouseListeners();
			this._handledByEditorListener = false;
			TextCompositionManager.CompleteComposition(composition);
			this._compositionUndoUnit = null;
			return !this._handledByEditorListener || composition.PendingComplete || this._compositionModifiedByEventListener;
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000DA410 File Offset: 0x000D8610
		internal void UpdateCompositionText(FrameworkTextComposition composition)
		{
			ITextPointer textPointer;
			ITextPointer textPointer2;
			this.UpdateCompositionText(composition, 0, true, out textPointer, out textPointer2);
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000DA42C File Offset: 0x000D862C
		internal void UpdateCompositionText(FrameworkTextComposition composition, int resultLength, bool includeResultText, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			if (this._compositionModifiedByEventListener)
			{
				return;
			}
			this._handledByEditorListener = true;
			bool flag = false;
			UndoCloseAction undoCloseAction = UndoCloseAction.Rollback;
			this.OpenCompositionUndoUnit();
			try
			{
				this._editor.Selection.BeginChange();
				try
				{
					ITextRange textRange;
					string text;
					if (composition._ResultStart != null)
					{
						textRange = new TextRange(composition._ResultStart, composition._ResultEnd, true);
						text = this._editor._FilterText(composition.Text, textRange);
						flag = (text != composition.Text);
						if (flag)
						{
							this._caretOffset = Math.Min(this._caretOffset, text.Length);
						}
					}
					else
					{
						textRange = new TextRange(composition._CompositionStart, composition._CompositionEnd, true);
						text = composition.CompositionText;
					}
					this._editor.SetText(textRange, text, InputLanguageManager.Current.CurrentInputLanguage);
					if (includeResultText)
					{
						start = textRange.Start;
					}
					else
					{
						start = textRange.Start.CreatePointer(resultLength, LogicalDirection.Forward);
					}
					end = textRange.End;
					ITextPointer textPointer = (this._caretOffset >= 0) ? start.CreatePointer(this._caretOffset, LogicalDirection.Forward) : end;
					this._editor.Selection.Select(textPointer, textPointer);
				}
				finally
				{
					this._compositionModifiedByEventListener = false;
					this._editor.Selection.EndChange();
					if (flag)
					{
						this._compositionModifiedByEventListener = true;
					}
				}
				undoCloseAction = UndoCloseAction.Commit;
			}
			finally
			{
				this.CloseCompositionUndoUnit(undoCloseAction, end);
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000DA5B4 File Offset: 0x000D87B4
		private void SetCompositionAdorner(byte[] attributes)
		{
			if (attributes != null)
			{
				int offset = 0;
				for (int i = 0; i < attributes.Length; i++)
				{
					if (i + 1 >= attributes.Length || attributes[i] != attributes[i + 1])
					{
						ITextPointer start = this._startComposition.CreatePointer(offset, LogicalDirection.Backward);
						ITextPointer end = this._startComposition.CreatePointer(i + 1, LogicalDirection.Forward);
						if (this._compositionAdorner == null)
						{
							this._compositionAdorner = new CompositionAdorner(this._editor.TextView);
							this._compositionAdorner.Initialize(this._editor.TextView);
						}
						UnsafeNativeMethods.TF_DISPLAYATTRIBUTE attr = default(UnsafeNativeMethods.TF_DISPLAYATTRIBUTE);
						attr.crLine.type = UnsafeNativeMethods.TF_DA_COLORTYPE.TF_CT_COLORREF;
						attr.crLine.indexOrColorRef = 0;
						attr.lsStyle = UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_NONE;
						attr.fBoldLine = false;
						switch (attributes[i])
						{
						case 0:
							attr.lsStyle = UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DOT;
							break;
						case 1:
							attr.lsStyle = UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SOLID;
							attr.fBoldLine = true;
							break;
						case 2:
							attr.lsStyle = UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_DOT;
							break;
						case 3:
							attr.lsStyle = UnsafeNativeMethods.TF_DA_LINESTYLE.TF_LS_SOLID;
							break;
						}
						TextServicesDisplayAttribute textServiceDisplayAttribute = new TextServicesDisplayAttribute(attr);
						this._compositionAdorner.AddAttributeRange(start, end, textServiceDisplayAttribute);
						offset = i + 1;
					}
				}
				if (this._compositionAdorner != null)
				{
					this._editor.TextView.RenderScope.UpdateLayout();
					this._compositionAdorner.InvalidateAdorner();
				}
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000DA710 File Offset: 0x000D8910
		private void RegisterMouseListeners()
		{
			this.UiScope.PreviewMouseLeftButtonDown += this.OnMouseButtonEvent;
			this.UiScope.PreviewMouseLeftButtonUp += this.OnMouseButtonEvent;
			this.UiScope.PreviewMouseRightButtonDown += this.OnMouseButtonEvent;
			this.UiScope.PreviewMouseRightButtonUp += this.OnMouseButtonEvent;
			this.UiScope.PreviewMouseMove += this.OnMouseEvent;
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000DA790 File Offset: 0x000D8990
		private void UnregisterMouseListeners()
		{
			if (this.UiScope != null)
			{
				this.UiScope.PreviewMouseLeftButtonDown -= this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseLeftButtonUp -= this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonDown -= this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseRightButtonUp -= this.OnMouseButtonEvent;
				this.UiScope.PreviewMouseMove -= this.OnMouseEvent;
			}
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000DA818 File Offset: 0x000D8A18
		[SecurityCritical]
		private IntPtr OnWmImeRequest(IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			IntPtr result = IntPtr.Zero;
			switch ((int)wParam)
			{
			case 4:
				result = this.OnWmImeRequest_ReconvertString(lParam, ref handled, false);
				break;
			case 5:
				result = this.OnWmImeRequest_ConfirmReconvertString(lParam, ref handled);
				break;
			case 7:
				result = this.OnWmImeRequest_ReconvertString(lParam, ref handled, true);
				break;
			}
			return result;
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000DA870 File Offset: 0x000D8A70
		[SecurityCritical]
		private IntPtr OnWmImeRequest_ReconvertString(IntPtr lParam, ref bool handled, bool fDocFeed)
		{
			if (!fDocFeed)
			{
				this._isReconvReady = false;
			}
			if (!this.IsInKeyboardFocus)
			{
				return IntPtr.Zero;
			}
			ITextRange textRange;
			if (fDocFeed && this._startComposition != null && this._endComposition != null)
			{
				textRange = new TextRange(this._startComposition, this._endComposition);
			}
			else
			{
				textRange = this._editor.Selection;
			}
			string text = textRange.Text;
			int num = Marshal.SizeOf(typeof(NativeMethods.RECONVERTSTRING)) + text.Length * Marshal.SizeOf(typeof(short)) + 33 * Marshal.SizeOf(typeof(short)) * 2;
			IntPtr result = new IntPtr(num);
			if (lParam != IntPtr.Zero)
			{
				int num2;
				string surroundingText = ImmComposition.GetSurroundingText(textRange, out num2);
				NativeMethods.RECONVERTSTRING reconvertstring = (NativeMethods.RECONVERTSTRING)Marshal.PtrToStructure(lParam, typeof(NativeMethods.RECONVERTSTRING));
				reconvertstring.dwSize = num;
				reconvertstring.dwVersion = 0;
				reconvertstring.dwStrLen = surroundingText.Length;
				reconvertstring.dwStrOffset = Marshal.SizeOf(typeof(NativeMethods.RECONVERTSTRING));
				reconvertstring.dwCompStrLen = text.Length;
				reconvertstring.dwCompStrOffset = num2 * Marshal.SizeOf(typeof(short));
				reconvertstring.dwTargetStrLen = text.Length;
				reconvertstring.dwTargetStrOffset = num2 * Marshal.SizeOf(typeof(short));
				if (!fDocFeed)
				{
					this._reconv = reconvertstring;
					this._isReconvReady = true;
				}
				Marshal.StructureToPtr(reconvertstring, lParam, true);
				ImmComposition.StoreSurroundingText(lParam, surroundingText);
			}
			handled = true;
			return result;
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000DA9F0 File Offset: 0x000D8BF0
		[SecurityCritical]
		private unsafe static void StoreSurroundingText(IntPtr reconv, string surrounding)
		{
			byte* ptr = (byte*)reconv.ToPointer();
			ptr += Marshal.SizeOf(typeof(NativeMethods.RECONVERTSTRING));
			Marshal.Copy(surrounding.ToCharArray(), 0, new IntPtr((void*)ptr), surrounding.Length);
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000DAA30 File Offset: 0x000D8C30
		private static string GetSurroundingText(ITextRange range, out int offsetStart)
		{
			string text = "";
			ITextPointer textPointer = range.Start.CreatePointer();
			bool flag = false;
			int num = 32;
			while (!flag && num > 0)
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Backward))
				{
				case TextPointerContext.None:
					flag = true;
					break;
				case TextPointerContext.Text:
				{
					char[] array = new char[num];
					int textInRun = textPointer.GetTextInRun(LogicalDirection.Backward, array, 0, array.Length);
					Invariant.Assert(textInRun != 0);
					textPointer.MoveByOffset(0 - textInRun);
					num -= textInRun;
					text = text.Insert(0, new string(array, 0, textInRun));
					break;
				}
				case TextPointerContext.EmbeddedElement:
					flag = true;
					break;
				case TextPointerContext.ElementStart:
				case TextPointerContext.ElementEnd:
					if (!textPointer.GetElementType(LogicalDirection.Backward).IsSubclassOf(typeof(Inline)))
					{
						flag = true;
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
					break;
				default:
					textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
					break;
				}
			}
			offsetStart = text.Length;
			text += range.Text;
			textPointer = range.End.CreatePointer();
			flag = false;
			num = 32;
			while (!flag && num > 0)
			{
				switch (textPointer.GetPointerContext(LogicalDirection.Forward))
				{
				case TextPointerContext.None:
					flag = true;
					break;
				case TextPointerContext.Text:
				{
					char[] array2 = new char[num];
					int textInRun2 = textPointer.GetTextInRun(LogicalDirection.Forward, array2, 0, array2.Length);
					textPointer.MoveByOffset(textInRun2);
					num -= textInRun2;
					text += new string(array2, 0, textInRun2);
					break;
				}
				case TextPointerContext.EmbeddedElement:
					flag = true;
					break;
				case TextPointerContext.ElementStart:
				case TextPointerContext.ElementEnd:
					if (!textPointer.GetElementType(LogicalDirection.Forward).IsSubclassOf(typeof(Inline)))
					{
						flag = true;
					}
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				default:
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					break;
				}
			}
			return text;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000DABCC File Offset: 0x000D8DCC
		[SecurityCritical]
		private IntPtr OnWmImeRequest_ConfirmReconvertString(IntPtr lParam, ref bool handled)
		{
			if (!this.IsInKeyboardFocus)
			{
				return IntPtr.Zero;
			}
			if (!this._isReconvReady)
			{
				return IntPtr.Zero;
			}
			NativeMethods.RECONVERTSTRING reconvertstring = (NativeMethods.RECONVERTSTRING)Marshal.PtrToStructure(lParam, typeof(NativeMethods.RECONVERTSTRING));
			if (this._reconv.dwStrLen != reconvertstring.dwStrLen)
			{
				handled = true;
				return IntPtr.Zero;
			}
			if (this._reconv.dwCompStrLen != reconvertstring.dwCompStrLen || this._reconv.dwCompStrOffset != reconvertstring.dwCompStrOffset)
			{
				ITextRange selection = this._editor.Selection;
				ITextPointer textPointer = selection.Start.CreatePointer(LogicalDirection.Backward);
				textPointer = ImmComposition.MoveToNextCharPos(textPointer, (reconvertstring.dwCompStrOffset - this._reconv.dwCompStrOffset) / Marshal.SizeOf(typeof(short)));
				ITextPointer textPointer2 = textPointer.CreatePointer(LogicalDirection.Forward);
				textPointer2 = ImmComposition.MoveToNextCharPos(textPointer2, reconvertstring.dwCompStrLen);
				this._editor.Selection.Select(textPointer, textPointer2);
			}
			this._isReconvReady = false;
			handled = true;
			return new IntPtr(1);
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000DACC8 File Offset: 0x000D8EC8
		private static ITextPointer MoveToNextCharPos(ITextPointer position, int offset)
		{
			bool flag = false;
			if (offset < 0)
			{
				while (offset < 0)
				{
					if (flag)
					{
						break;
					}
					TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Backward);
					if (pointerContext != TextPointerContext.None)
					{
						if (pointerContext == TextPointerContext.Text)
						{
							offset++;
						}
					}
					else
					{
						flag = true;
					}
					position.MoveByOffset(-1);
				}
			}
			else if (offset > 0)
			{
				while (offset > 0 && !flag)
				{
					TextPointerContext pointerContext = position.GetPointerContext(LogicalDirection.Forward);
					if (pointerContext != TextPointerContext.None)
					{
						if (pointerContext == TextPointerContext.Text)
						{
							offset--;
						}
					}
					else
					{
						flag = true;
					}
					position.MoveByOffset(1);
				}
			}
			return position;
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000DAD34 File Offset: 0x000D8F34
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool IsReadingWindowIme()
		{
			int num = UnsafeNativeMethods.ImmGetProperty(new HandleRef(this, SafeNativeMethods.GetKeyboardLayout(0)), 4);
			return (num & 65536) == 0 || (num & 131072) != 0;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000DAD69 File Offset: 0x000D8F69
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnMouseButtonEvent(object sender, MouseButtonEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000DAD69 File Offset: 0x000D8F69
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnMouseEvent(object sender, MouseEventArgs e)
		{
			e.Handled = this.InternalMouseEventHandler();
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x000DAD78 File Offset: 0x000D8F78
		[SecurityCritical]
		private bool InternalMouseEventHandler()
		{
			int num = 0;
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				num = 1;
			}
			if (Mouse.RightButton == MouseButtonState.Pressed)
			{
				num = 2;
			}
			Point position = Mouse.GetPosition(this.RenderScope);
			ITextView textView = TextEditor.GetTextView(this.RenderScope);
			if (!textView.Validate(position))
			{
				return false;
			}
			ITextPointer textPositionFromPoint = textView.GetTextPositionFromPoint(position, false);
			if (textPositionFromPoint == null)
			{
				return false;
			}
			Rect rectangleFromTextPosition = textView.GetRectangleFromTextPosition(textPositionFromPoint);
			ITextPointer textPointer = textPositionFromPoint.CreatePointer();
			if (textPointer == null)
			{
				return false;
			}
			if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward);
			}
			else
			{
				textPointer.MoveToNextInsertionPosition(LogicalDirection.Backward);
			}
			Rect rectangleFromTextPosition2 = textView.GetRectangleFromTextPosition(textPointer);
			int num2 = this._editor.TextContainer.Start.GetOffsetToPosition(textPositionFromPoint);
			int offsetToPosition = this._editor.TextContainer.Start.GetOffsetToPosition(this._startComposition);
			int offsetToPosition2 = this._editor.TextContainer.Start.GetOffsetToPosition(this._endComposition);
			if (num2 < offsetToPosition)
			{
				return false;
			}
			if (num2 > offsetToPosition2)
			{
				return false;
			}
			int num3;
			if (rectangleFromTextPosition2.Left == rectangleFromTextPosition.Left)
			{
				num3 = 0;
			}
			else if (position.X - rectangleFromTextPosition.Left >= 0.0)
			{
				if ((position.X - rectangleFromTextPosition.Left) * 4.0 / (rectangleFromTextPosition2.Left - rectangleFromTextPosition.Left) <= 1.0)
				{
					num3 = 2;
				}
				else
				{
					num3 = 3;
				}
			}
			else if ((position.X - rectangleFromTextPosition2.Left) * 4.0 / (rectangleFromTextPosition.Left - rectangleFromTextPosition2.Left) <= 3.0)
			{
				num3 = 0;
			}
			else
			{
				num3 = 1;
			}
			if (num2 == offsetToPosition && num3 <= 1)
			{
				return false;
			}
			if (num2 == offsetToPosition2 && num3 >= 2)
			{
				return false;
			}
			num2 -= offsetToPosition;
			int value = (num2 << 16) + (num3 << 8) + num;
			IntPtr handle = IntPtr.Zero;
			new UIPermission(UIPermissionWindow.AllWindows).Assert();
			try
			{
				handle = ((IWin32Window)this._source).Handle;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			IntPtr intPtr = UnsafeNativeMethods.ImmGetContext(new HandleRef(this, handle));
			IntPtr value2 = IntPtr.Zero;
			if (intPtr != IntPtr.Zero)
			{
				IntPtr hWnd = IntPtr.Zero;
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					hWnd = UnsafeNativeMethods.ImmGetDefaultIMEWnd(new HandleRef(this, handle));
					value2 = UnsafeNativeMethods.SendMessage(hWnd, ImmComposition.s_MsImeMouseMessage, new IntPtr(value), intPtr);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			return value2 != IntPtr.Zero;
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000DB010 File Offset: 0x000D9210
		private void OpenCompositionUndoUnit()
		{
			DependencyObject parent = this._editor.TextContainer.Parent;
			UndoManager undoManager = UndoManager.GetUndoManager(parent);
			if (undoManager == null || !undoManager.IsEnabled || undoManager.OpenedUnit != null)
			{
				this._compositionUndoUnit = null;
				return;
			}
			if (this._compositionUndoUnit != null && this._compositionUndoUnit == undoManager.LastUnit && !this._compositionUndoUnit.Locked)
			{
				undoManager.Reopen(this._compositionUndoUnit);
				return;
			}
			this._compositionUndoUnit = new TextParentUndoUnit(this._editor.Selection);
			undoManager.Open(this._compositionUndoUnit);
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000DB0A4 File Offset: 0x000D92A4
		private void CloseCompositionUndoUnit(UndoCloseAction undoCloseAction, ITextPointer compositionEnd)
		{
			DependencyObject parent = this._editor.TextContainer.Parent;
			UndoManager undoManager = UndoManager.GetUndoManager(parent);
			if (undoManager != null && undoManager.IsEnabled && undoManager.OpenedUnit != null)
			{
				if (this._compositionUndoUnit != null)
				{
					if (undoCloseAction == UndoCloseAction.Commit)
					{
						this._compositionUndoUnit.RecordRedoSelectionState(compositionEnd, compositionEnd);
					}
					undoManager.Close(this._compositionUndoUnit, undoCloseAction);
					return;
				}
			}
			else
			{
				this._compositionUndoUnit = null;
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000DB10C File Offset: 0x000D930C
		private int ConvertToInt32(double value)
		{
			int result;
			if (double.IsNaN(value))
			{
				result = int.MinValue;
			}
			else if (value < -2147483648.0)
			{
				result = int.MinValue;
			}
			else if (value > 2147483647.0)
			{
				result = int.MaxValue;
			}
			else
			{
				result = Convert.ToInt32(value);
			}
			return result;
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000DB159 File Offset: 0x000D9359
		private void OnTextContainerChange(object sender, TextContainerChangeEventArgs args)
		{
			if (args.IMECharCount > 0 && (args.TextChange == TextChangeType.ContentAdded || args.TextChange == TextChangeType.ContentRemoved))
			{
				this._compositionModifiedByEventListener = true;
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x000DB17C File Offset: 0x000D937C
		private UIElement RenderScope
		{
			get
			{
				if (this._editor.TextView != null)
				{
					return this._editor.TextView.RenderScope;
				}
				return null;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x000DB19D File Offset: 0x000D939D
		private FrameworkElement UiScope
		{
			get
			{
				if (this._editor != null)
				{
					return this._editor.UiScope;
				}
				return null;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003089 RID: 12425 RVA: 0x000DB1B4 File Offset: 0x000D93B4
		private bool IsReadOnly
		{
			get
			{
				return (bool)this.UiScope.GetValue(TextEditor.IsReadOnlyProperty) || this._editor.IsReadOnly;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600308A RID: 12426 RVA: 0x000DB1DA File Offset: 0x000D93DA
		private bool IsInKeyboardFocus
		{
			get
			{
				return this._editor != null && this.UiScope != null && this.UiScope.IsKeyboardFocused;
			}
		}

		// Token: 0x04001E76 RID: 7798
		[SecurityCritical]
		private HwndSource _source;

		// Token: 0x04001E77 RID: 7799
		private TextEditor _editor;

		// Token: 0x04001E78 RID: 7800
		private ITextPointer _startComposition;

		// Token: 0x04001E79 RID: 7801
		private ITextPointer _endComposition;

		// Token: 0x04001E7A RID: 7802
		private int _caretOffset;

		// Token: 0x04001E7B RID: 7803
		private CompositionAdorner _compositionAdorner;

		// Token: 0x04001E7C RID: 7804
		private static Hashtable _list = new Hashtable(1);

		// Token: 0x04001E7D RID: 7805
		private const double _dashLength = 2.0;

		// Token: 0x04001E7E RID: 7806
		private const int _maxSrounding = 32;

		// Token: 0x04001E7F RID: 7807
		private NativeMethods.RECONVERTSTRING _reconv;

		// Token: 0x04001E80 RID: 7808
		private bool _isReconvReady;

		// Token: 0x04001E81 RID: 7809
		[SecurityCritical]
		private static WindowMessage s_MsImeMouseMessage = UnsafeNativeMethods.RegisterWindowMessage("MSIMEMouseOperation");

		// Token: 0x04001E82 RID: 7810
		private TextParentUndoUnit _compositionUndoUnit;

		// Token: 0x04001E83 RID: 7811
		private bool _handlingImeMessage;

		// Token: 0x04001E84 RID: 7812
		private bool _updateCompWndPosAtNextLayoutUpdate;

		// Token: 0x04001E85 RID: 7813
		private bool _compositionModifiedByEventListener;

		// Token: 0x04001E86 RID: 7814
		private bool _handledByEditorListener;

		// Token: 0x04001E87 RID: 7815
		private bool _losingFocus;
	}
}
