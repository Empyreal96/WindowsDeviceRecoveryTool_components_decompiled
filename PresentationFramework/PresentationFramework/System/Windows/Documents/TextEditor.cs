using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Documents;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003F1 RID: 1009
	internal class TextEditor
	{
		// Token: 0x060037CE RID: 14286 RVA: 0x000F91D4 File Offset: 0x000F73D4
		internal TextEditor(ITextContainer textContainer, FrameworkElement uiScope, bool isUndoEnabled)
		{
			Invariant.Assert(uiScope != null);
			this._acceptsRichContent = true;
			this._textContainer = textContainer;
			this._uiScope = uiScope;
			if (isUndoEnabled && this._textContainer is TextContainer)
			{
				((TextContainer)this._textContainer).EnableUndo(this._uiScope);
			}
			this._selection = new TextSelection(this);
			textContainer.TextSelection = this._selection;
			this._dragDropProcess = new TextEditorDragDrop._DragDropProcess(this);
			this._cursor = Cursors.IBeam;
			TextEditorTyping._AddInputLanguageChangedEventHandler(this);
			this.TextContainer.Changed += this.OnTextContainerChanged;
			this._uiScope.IsEnabledChanged += TextEditor.OnIsEnabledChanged;
			this._uiScope.SetValue(TextEditor.InstanceProperty, this);
			if ((bool)this._uiScope.GetValue(SpellCheck.IsEnabledProperty))
			{
				this.SetSpellCheckEnabled(true);
				this.SetCustomDictionaries(true);
			}
			if (!TextServicesLoader.ServicesInstalled)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000F92D4 File Offset: 0x000F74D4
		~TextEditor()
		{
			this.DetachTextStore(true);
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000F9304 File Offset: 0x000F7504
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnDetach()
		{
			Invariant.Assert(this._textContainer != null);
			this.SetSpellCheckEnabled(false);
			UndoManager undoManager = UndoManager.GetUndoManager(this._uiScope);
			if (undoManager != null)
			{
				if (this._textContainer is TextContainer)
				{
					((TextContainer)this._textContainer).DisableUndo(this._uiScope);
				}
				else
				{
					UndoManager.DetachUndoManager(this._uiScope);
				}
			}
			this._textContainer.TextSelection = null;
			TextEditorTyping._RemoveInputLanguageChangedEventHandler(this);
			this._textContainer.Changed -= this.OnTextContainerChanged;
			this._uiScope.IsEnabledChanged -= TextEditor.OnIsEnabledChanged;
			this._pendingTextStoreInit = false;
			this.DetachTextStore(false);
			if (this._immCompositionForDetach != null)
			{
				ImmComposition immComposition;
				if (this._immCompositionForDetach.TryGetTarget(out immComposition))
				{
					immComposition.OnDetach(this);
				}
				this._immComposition = null;
				this._immCompositionForDetach = null;
			}
			this.TextView = null;
			this._selection.OnDetach();
			this._selection = null;
			this._uiScope.ClearValue(TextEditor.InstanceProperty);
			this._uiScope = null;
			this._textContainer = null;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000F9416 File Offset: 0x000F7616
		private void DetachTextStore(bool finalizer)
		{
			if (this._textstore != null)
			{
				this._textstore.OnDetach(finalizer);
				this._textstore = null;
			}
			if (this._weakThis != null)
			{
				this._weakThis.StopListening();
				this._weakThis = null;
			}
			if (!finalizer)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000F9458 File Offset: 0x000F7658
		internal void SetSpellCheckEnabled(bool value)
		{
			value = (value && !this.IsReadOnly && this._IsEnabled);
			if (value && this._speller == null)
			{
				this._speller = new Speller(this);
				return;
			}
			if (!value && this._speller != null)
			{
				this._speller.Detach();
				this._speller = null;
			}
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000F94B0 File Offset: 0x000F76B0
		internal void SetCustomDictionaries(bool add)
		{
			TextBoxBase textBoxBase = this._uiScope as TextBoxBase;
			if (textBoxBase == null)
			{
				return;
			}
			if (this._speller != null)
			{
				CustomDictionarySources dictionaryLocations = (CustomDictionarySources)SpellCheck.GetCustomDictionaries(textBoxBase);
				this._speller.SetCustomDictionaries(dictionaryLocations, add);
			}
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000F94EE File Offset: 0x000F76EE
		internal void SetSpellingReform(SpellingReform spellingReform)
		{
			if (this._speller != null)
			{
				this._speller.SetSpellingReform(spellingReform);
			}
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x000F9504 File Offset: 0x000F7704
		internal static ITextView GetTextView(UIElement scope)
		{
			IServiceProvider serviceProvider = scope as IServiceProvider;
			if (serviceProvider == null)
			{
				return null;
			}
			return serviceProvider.GetService(typeof(ITextView)) as ITextView;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000F9534 File Offset: 0x000F7734
		internal static ITextSelection GetTextSelection(FrameworkElement frameworkElement)
		{
			TextEditor textEditor = TextEditor._GetTextEditor(frameworkElement);
			if (textEditor != null)
			{
				return textEditor.Selection;
			}
			return null;
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x000F9554 File Offset: 0x000F7754
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void RegisterCommandHandlers(Type controlType, bool acceptsRichContent, bool readOnly, bool registerEventListeners)
		{
			Invariant.Assert(TextEditor._registeredEditingTypes != null);
			ArrayList registeredEditingTypes = TextEditor._registeredEditingTypes;
			lock (registeredEditingTypes)
			{
				for (int i = 0; i < TextEditor._registeredEditingTypes.Count; i++)
				{
					if (((Type)TextEditor._registeredEditingTypes[i]).IsAssignableFrom(controlType))
					{
						return;
					}
					if (controlType.IsAssignableFrom((Type)TextEditor._registeredEditingTypes[i]))
					{
						throw new InvalidOperationException(SR.Get("TextEditorCanNotRegisterCommandHandler", new object[]
						{
							((Type)TextEditor._registeredEditingTypes[i]).Name,
							controlType.Name
						}));
					}
				}
				TextEditor._registeredEditingTypes.Add(controlType);
			}
			TextEditorMouse._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorTyping._RegisterClassHandlers(controlType, registerEventListeners);
			}
			TextEditorDragDrop._RegisterClassHandlers(controlType, readOnly, registerEventListeners);
			TextEditorCopyPaste._RegisterClassHandlers(controlType, acceptsRichContent, readOnly, registerEventListeners);
			TextEditorSelection._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorParagraphs._RegisterClassHandlers(controlType, acceptsRichContent, registerEventListeners);
			}
			TextEditorContextMenu._RegisterClassHandlers(controlType, registerEventListeners);
			if (!readOnly)
			{
				TextEditorSpelling._RegisterClassHandlers(controlType, registerEventListeners);
			}
			if (acceptsRichContent && !readOnly)
			{
				TextEditorCharacters._RegisterClassHandlers(controlType, registerEventListeners);
				TextEditorLists._RegisterClassHandlers(controlType, registerEventListeners);
				if (TextEditor._isTableEditingEnabled)
				{
					TextEditorTables._RegisterClassHandlers(controlType, registerEventListeners);
				}
			}
			if (registerEventListeners)
			{
				EventManager.RegisterClassHandler(controlType, Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(TextEditor.OnGotKeyboardFocus));
				EventManager.RegisterClassHandler(controlType, Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(TextEditor.OnLostKeyboardFocus));
				EventManager.RegisterClassHandler(controlType, UIElement.LostFocusEvent, new RoutedEventHandler(TextEditor.OnLostFocus));
			}
			if (!readOnly)
			{
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Undo, new ExecutedRoutedEventHandler(TextEditor.OnUndo), new CanExecuteRoutedEventHandler(TextEditor.OnQueryStatusUndo), KeyGesture.CreateFromResourceStrings(SR.Get("KeyUndo"), SR.Get("KeyUndoDisplayString")), KeyGesture.CreateFromResourceStrings(SR.Get("KeyAltUndo"), SR.Get("KeyAltUndoDisplayString")));
				CommandHelpers.RegisterCommandHandler(controlType, ApplicationCommands.Redo, new ExecutedRoutedEventHandler(TextEditor.OnRedo), new CanExecuteRoutedEventHandler(TextEditor.OnQueryStatusRedo), "KeyRedo", "KeyRedoDisplayString");
			}
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x000F9760 File Offset: 0x000F7960
		internal SpellingError GetSpellingErrorAtPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextEditorSpelling.GetSpellingErrorAtPosition(this, position, direction);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000F976A File Offset: 0x000F796A
		internal SpellingError GetSpellingErrorAtSelection()
		{
			return TextEditorSpelling.GetSpellingErrorAtSelection(this);
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000F9772 File Offset: 0x000F7972
		internal ITextPointer GetNextSpellingErrorPosition(ITextPointer position, LogicalDirection direction)
		{
			return TextEditorSpelling.GetNextSpellingErrorPosition(this, position, direction);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000F977C File Offset: 0x000F797C
		internal void SetText(ITextRange range, string text, CultureInfo cultureInfo)
		{
			range.Text = text;
			if (range is TextRange)
			{
				this.MarkCultureProperty((TextRange)range, cultureInfo);
			}
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000F979A File Offset: 0x000F799A
		internal void SetSelectedText(string text, CultureInfo cultureInfo)
		{
			this.SetText(this.Selection, text, cultureInfo);
			((TextSelection)this.Selection).ApplySpringloadFormatting();
			TextEditorSelection._ClearSuggestedX(this);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000F97C0 File Offset: 0x000F79C0
		internal void MarkCultureProperty(TextRange range, CultureInfo inputCultureInfo)
		{
			Invariant.Assert(this.UiScope != null);
			if (!this.AcceptsRichContent)
			{
				return;
			}
			XmlLanguage xmlLanguage = (XmlLanguage)((ITextPointer)range.Start).GetValue(FrameworkElement.LanguageProperty);
			Invariant.Assert(xmlLanguage != null);
			if (!string.Equals(inputCultureInfo.IetfLanguageTag, xmlLanguage.IetfLanguageTag, StringComparison.OrdinalIgnoreCase))
			{
				range.ApplyPropertyValue(FrameworkElement.LanguageProperty, XmlLanguage.GetLanguage(inputCultureInfo.IetfLanguageTag));
			}
			FlowDirection flowDirection;
			if (inputCultureInfo.TextInfo.IsRightToLeft)
			{
				flowDirection = FlowDirection.RightToLeft;
			}
			else
			{
				flowDirection = FlowDirection.LeftToRight;
			}
			FlowDirection flowDirection2 = (FlowDirection)((ITextPointer)range.Start).GetValue(FrameworkElement.FlowDirectionProperty);
			if (flowDirection2 != flowDirection)
			{
				range.ApplyPropertyValue(FrameworkElement.FlowDirectionProperty, flowDirection);
			}
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000F986C File Offset: 0x000F7A6C
		internal void RequestExtendSelection(Point point)
		{
			if (this._mouseSelectionState == null)
			{
				this._mouseSelectionState = new TextEditor.MouseSelectionState();
				this._mouseSelectionState.Timer = new DispatcherTimer(DispatcherPriority.Normal);
				this._mouseSelectionState.Timer.Tick += this.HandleMouseSelectionTick;
				this._mouseSelectionState.Timer.Interval = TimeSpan.FromMilliseconds((double)Math.Max(SystemParameters.MenuShowDelay, 200));
				this._mouseSelectionState.Timer.Start();
				this._mouseSelectionState.Point = point;
				this.HandleMouseSelectionTick(this._mouseSelectionState.Timer, EventArgs.Empty);
				return;
			}
			this._mouseSelectionState.Point = point;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000F9921 File Offset: 0x000F7B21
		internal void CancelExtendSelection()
		{
			if (this._mouseSelectionState != null)
			{
				this._mouseSelectionState.Timer.Stop();
				this._mouseSelectionState.Timer.Tick -= this.HandleMouseSelectionTick;
				this._mouseSelectionState = null;
			}
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000F9960 File Offset: 0x000F7B60
		internal void CloseToolTip()
		{
			PopupControlService popupControlService = PopupControlService.Current;
			if (popupControlService.CurrentToolTip != null && popupControlService.CurrentToolTip.IsOpen && popupControlService.CurrentToolTip.PlacementTarget == this._uiScope)
			{
				popupControlService.CurrentToolTip.IsOpen = false;
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000F99A8 File Offset: 0x000F7BA8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Undo()
		{
			TextEditorTyping._FlushPendingInputItems(this);
			this.CompleteComposition();
			this._undoState = UndoState.Undo;
			bool coversEntireContent = this.Selection.CoversEntireContent;
			try
			{
				this._selection.BeginChangeNoUndo();
				try
				{
					UndoManager undoManager = this._GetUndoManager();
					if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
					{
						undoManager.Undo(1);
					}
					TextEditorSelection._ClearSuggestedX(this);
					TextEditorTyping._BreakTypingSequence(this);
					if (this._selection is TextSelection)
					{
						((TextSelection)this._selection).ClearSpringloadFormatting();
					}
				}
				finally
				{
					this._selection.EndChange();
				}
			}
			finally
			{
				this._undoState = UndoState.Normal;
			}
			if (coversEntireContent)
			{
				this.Selection.ValidateLayout();
			}
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000F9A68 File Offset: 0x000F7C68
		internal void Redo()
		{
			TextEditorTyping._FlushPendingInputItems(this);
			this._undoState = UndoState.Redo;
			bool coversEntireContent = this.Selection.CoversEntireContent;
			try
			{
				this._selection.BeginChangeNoUndo();
				try
				{
					UndoManager undoManager = this._GetUndoManager();
					if (undoManager != null && undoManager.RedoCount > 0)
					{
						undoManager.Redo(1);
					}
					TextEditorSelection._ClearSuggestedX(this);
					TextEditorTyping._BreakTypingSequence(this);
					if (this._selection is TextSelection)
					{
						((TextSelection)this._selection).ClearSpringloadFormatting();
					}
				}
				finally
				{
					this._selection.EndChange();
				}
			}
			finally
			{
				this._undoState = UndoState.Normal;
			}
			if (coversEntireContent)
			{
				this.Selection.ValidateLayout();
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000F9B1C File Offset: 0x000F7D1C
		internal void OnPreviewKeyDown(KeyEventArgs e)
		{
			TextEditorTyping.OnPreviewKeyDown(this._uiScope, e);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000F9B2A File Offset: 0x000F7D2A
		internal void OnKeyDown(KeyEventArgs e)
		{
			TextEditorTyping.OnKeyDown(this._uiScope, e);
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000F9B38 File Offset: 0x000F7D38
		internal void OnKeyUp(KeyEventArgs e)
		{
			TextEditorTyping.OnKeyUp(this._uiScope, e);
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x000F9B46 File Offset: 0x000F7D46
		internal void OnTextInput(TextCompositionEventArgs e)
		{
			TextEditorTyping.OnTextInput(this._uiScope, e);
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x000F9B54 File Offset: 0x000F7D54
		internal void OnMouseDown(MouseButtonEventArgs e)
		{
			TextEditorMouse.OnMouseDown(this._uiScope, e);
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000F9B62 File Offset: 0x000F7D62
		internal void OnMouseMove(MouseEventArgs e)
		{
			TextEditorMouse.OnMouseMove(this._uiScope, e);
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x000F9B70 File Offset: 0x000F7D70
		internal void OnMouseUp(MouseButtonEventArgs e)
		{
			TextEditorMouse.OnMouseUp(this._uiScope, e);
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x000F9B7E File Offset: 0x000F7D7E
		internal void OnQueryCursor(QueryCursorEventArgs e)
		{
			TextEditorMouse.OnQueryCursor(this._uiScope, e);
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x000F9B8C File Offset: 0x000F7D8C
		internal void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			TextEditorDragDrop.OnQueryContinueDrag(this._uiScope, e);
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x000F9B9A File Offset: 0x000F7D9A
		internal void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			TextEditorDragDrop.OnGiveFeedback(this._uiScope, e);
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000F9BA8 File Offset: 0x000F7DA8
		internal void OnDragEnter(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragEnter(this._uiScope, e);
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000F9BB6 File Offset: 0x000F7DB6
		internal void OnDragOver(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragOver(this._uiScope, e);
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x000F9BC4 File Offset: 0x000F7DC4
		internal void OnDragLeave(DragEventArgs e)
		{
			TextEditorDragDrop.OnDragLeave(this._uiScope, e);
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x000F9BD2 File Offset: 0x000F7DD2
		internal void OnDrop(DragEventArgs e)
		{
			TextEditorDragDrop.OnDrop(this._uiScope, e);
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x000F9BE0 File Offset: 0x000F7DE0
		internal void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			TextEditorContextMenu.OnContextMenuOpening(this._uiScope, e);
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x000F9BEE File Offset: 0x000F7DEE
		internal void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			TextEditor.OnGotKeyboardFocus(this._uiScope, e);
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x000F9BFC File Offset: 0x000F7DFC
		internal void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			TextEditor.OnLostKeyboardFocus(this._uiScope, e);
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x000F9C0A File Offset: 0x000F7E0A
		internal void OnLostFocus(RoutedEventArgs e)
		{
			TextEditor.OnLostFocus(this._uiScope, e);
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000F9C18 File Offset: 0x000F7E18
		internal ITextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x000F9C20 File Offset: 0x000F7E20
		internal FrameworkElement UiScope
		{
			get
			{
				return this._uiScope;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060037F7 RID: 14327 RVA: 0x000F9C28 File Offset: 0x000F7E28
		// (set) Token: 0x060037F8 RID: 14328 RVA: 0x000F9C30 File Offset: 0x000F7E30
		internal ITextView TextView
		{
			get
			{
				return this._textView;
			}
			set
			{
				if (value != this._textView)
				{
					if (this._textView != null)
					{
						this._textView.Updated -= this.OnTextViewUpdated;
						this._textView = null;
						this._selection.UpdateCaretAndHighlight();
					}
					if (value != null)
					{
						this._textView = value;
						this._textView.Updated += this.OnTextViewUpdated;
						this._selection.UpdateCaretAndHighlight();
					}
				}
			}
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000F9CA3 File Offset: 0x000F7EA3
		internal ITextSelection Selection
		{
			get
			{
				return this._selection;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x060037FA RID: 14330 RVA: 0x000F9CAB File Offset: 0x000F7EAB
		internal TextStore TextStore
		{
			get
			{
				return this._textstore;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x000F9CB3 File Offset: 0x000F7EB3
		internal ImmComposition ImmComposition
		{
			get
			{
				if (!TextEditor._immEnabled)
				{
					return null;
				}
				return this._immComposition;
			}
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x000F9CC4 File Offset: 0x000F7EC4
		internal bool AcceptsReturn
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(KeyboardNavigation.AcceptsReturnProperty);
			}
		}

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x000F9CE5 File Offset: 0x000F7EE5
		// (set) Token: 0x060037FE RID: 14334 RVA: 0x000F9D06 File Offset: 0x000F7F06
		internal bool AcceptsTab
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(TextBoxBase.AcceptsTabProperty);
			}
			set
			{
				Invariant.Assert(this._uiScope != null);
				if (this.AcceptsTab != value)
				{
					this._uiScope.SetValue(TextBoxBase.AcceptsTabProperty, value);
				}
			}
		}

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x060037FF RID: 14335 RVA: 0x000F9D30 File Offset: 0x000F7F30
		// (set) Token: 0x06003800 RID: 14336 RVA: 0x000F9D5B File Offset: 0x000F7F5B
		internal bool IsReadOnly
		{
			get
			{
				return this._isReadOnly || (this._uiScope != null && (bool)this._uiScope.GetValue(TextEditor.IsReadOnlyProperty));
			}
			set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06003801 RID: 14337 RVA: 0x000F9D64 File Offset: 0x000F7F64
		// (set) Token: 0x06003802 RID: 14338 RVA: 0x000F9D85 File Offset: 0x000F7F85
		internal bool IsSpellCheckEnabled
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(SpellCheck.IsEnabledProperty);
			}
			set
			{
				Invariant.Assert(this._uiScope != null);
				this._uiScope.SetValue(SpellCheck.IsEnabledProperty, value);
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x000F9DA6 File Offset: 0x000F7FA6
		// (set) Token: 0x06003804 RID: 14340 RVA: 0x000F9DAE File Offset: 0x000F7FAE
		internal bool AcceptsRichContent
		{
			get
			{
				return this._acceptsRichContent;
			}
			set
			{
				this._acceptsRichContent = value;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06003805 RID: 14341 RVA: 0x000F9DB7 File Offset: 0x000F7FB7
		internal bool AllowOvertype
		{
			get
			{
				return this._uiScope == null || (bool)this._uiScope.GetValue(TextEditor.AllowOvertypeProperty);
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06003806 RID: 14342 RVA: 0x000F9DD8 File Offset: 0x000F7FD8
		internal int MaxLength
		{
			get
			{
				if (this._uiScope != null)
				{
					return (int)this._uiScope.GetValue(TextBox.MaxLengthProperty);
				}
				return 0;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x000F9DF9 File Offset: 0x000F7FF9
		internal CharacterCasing CharacterCasing
		{
			get
			{
				if (this._uiScope != null)
				{
					return (CharacterCasing)this._uiScope.GetValue(TextBox.CharacterCasingProperty);
				}
				return CharacterCasing.Normal;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x06003808 RID: 14344 RVA: 0x000F9E1A File Offset: 0x000F801A
		internal bool AutoWordSelection
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(TextBoxBase.AutoWordSelectionProperty);
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x000F9E3B File Offset: 0x000F803B
		internal bool IsReadOnlyCaretVisible
		{
			get
			{
				return this._uiScope != null && (bool)this._uiScope.GetValue(TextBoxBase.IsReadOnlyCaretVisibleProperty);
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600380A RID: 14346 RVA: 0x000F9E5C File Offset: 0x000F805C
		internal UndoState UndoState
		{
			get
			{
				return this._undoState;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600380B RID: 14347 RVA: 0x000F9E64 File Offset: 0x000F8064
		// (set) Token: 0x0600380C RID: 14348 RVA: 0x000F9E6C File Offset: 0x000F806C
		internal bool IsContextMenuOpen
		{
			get
			{
				return this._isContextMenuOpen;
			}
			set
			{
				this._isContextMenuOpen = value;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x0600380D RID: 14349 RVA: 0x000F9E75 File Offset: 0x000F8075
		internal Speller Speller
		{
			get
			{
				return this._speller;
			}
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x000F9E7D File Offset: 0x000F807D
		internal static TextEditor _GetTextEditor(object element)
		{
			if (!(element is DependencyObject))
			{
				return null;
			}
			return ((DependencyObject)element).ReadLocalValue(TextEditor.InstanceProperty) as TextEditor;
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x000F9EA0 File Offset: 0x000F80A0
		internal UndoManager _GetUndoManager()
		{
			UndoManager result = null;
			if (this.TextContainer is TextContainer)
			{
				result = ((TextContainer)this.TextContainer).UndoManager;
			}
			return result;
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x000F9ECE File Offset: 0x000F80CE
		internal string _FilterText(string textData, ITextRange range)
		{
			return this._FilterText(textData, range.Start.GetOffsetToPosition(range.End));
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x000F9EE8 File Offset: 0x000F80E8
		internal string _FilterText(string textData, int charsToReplaceCount)
		{
			return this._FilterText(textData, charsToReplaceCount, true);
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000F9EF3 File Offset: 0x000F80F3
		internal string _FilterText(string textData, ITextRange range, bool filterMaxLength)
		{
			return this._FilterText(textData, range.Start.GetOffsetToPosition(range.End), filterMaxLength);
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000F9F10 File Offset: 0x000F8110
		internal string _FilterText(string textData, int charsToReplaceCount, bool filterMaxLength)
		{
			if (!this.AcceptsRichContent)
			{
				if (filterMaxLength && this.MaxLength > 0)
				{
					ITextContainer textContainer = this.TextContainer;
					int num = textContainer.SymbolCount - charsToReplaceCount;
					int num2 = Math.Max(0, this.MaxLength - num);
					if (num2 == 0)
					{
						return string.Empty;
					}
					if (textData.Length > num2)
					{
						int num3 = num2;
						if (this.IsBadSplitPosition(textData, num3))
						{
							num3--;
						}
						textData = textData.Substring(0, num3);
					}
					if (textData.Length == num2 && char.IsHighSurrogate(textData, num2 - 1))
					{
						textData = textData.Substring(0, num2 - 1);
					}
					if (!string.IsNullOrEmpty(textData) && char.IsLowSurrogate(textData, 0))
					{
						string textInRun = textContainer.TextSelection.AnchorPosition.GetTextInRun(LogicalDirection.Backward);
						if (string.IsNullOrEmpty(textInRun) || !char.IsHighSurrogate(textInRun, textInRun.Length - 1))
						{
							return string.Empty;
						}
					}
				}
				if (string.IsNullOrEmpty(textData))
				{
					return textData;
				}
				if (this.CharacterCasing == CharacterCasing.Upper)
				{
					textData = textData.ToUpper(InputLanguageManager.Current.CurrentInputLanguage);
				}
				else if (this.CharacterCasing == CharacterCasing.Lower)
				{
					textData = textData.ToLower(InputLanguageManager.Current.CurrentInputLanguage);
				}
				if (!this.AcceptsReturn)
				{
					int num4 = textData.IndexOf(Environment.NewLine, StringComparison.Ordinal);
					if (num4 >= 0)
					{
						textData = textData.Substring(0, num4);
					}
					num4 = textData.IndexOfAny(TextPointerBase.NextLineCharacters);
					if (num4 >= 0)
					{
						textData = textData.Substring(0, num4);
					}
				}
				if (!this.AcceptsTab)
				{
					textData = textData.Replace('\t', ' ');
				}
			}
			return textData;
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x000FA084 File Offset: 0x000F8284
		internal bool _IsSourceInScope(object source)
		{
			return source == this.UiScope || (source is FrameworkElement && ((FrameworkElement)source).TemplatedParent == this.UiScope);
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x000FA0AF File Offset: 0x000F82AF
		internal void CompleteComposition()
		{
			if (this.TextStore != null)
			{
				this.TextStore.CompleteComposition();
			}
			if (this.ImmComposition != null)
			{
				this.ImmComposition.CompleteComposition();
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06003816 RID: 14358 RVA: 0x000FA0D7 File Offset: 0x000F82D7
		internal bool _IsEnabled
		{
			get
			{
				return this._uiScope != null && this._uiScope.IsEnabled;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06003817 RID: 14359 RVA: 0x000FA0EE File Offset: 0x000F82EE
		// (set) Token: 0x06003818 RID: 14360 RVA: 0x000FA0F6 File Offset: 0x000F82F6
		internal bool _OvertypeMode
		{
			get
			{
				return this._overtypeMode;
			}
			set
			{
				this._overtypeMode = value;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x06003819 RID: 14361 RVA: 0x000FA100 File Offset: 0x000F8300
		internal FrameworkElement _Scroller
		{
			get
			{
				FrameworkElement frameworkElement = (this.TextView == null) ? null : (this.TextView.RenderScope as FrameworkElement);
				while (frameworkElement != null && frameworkElement != this.UiScope)
				{
					frameworkElement = (FrameworkElement.GetFrameworkParent(frameworkElement) as FrameworkElement);
					if (frameworkElement is ScrollViewer || frameworkElement is ScrollContentPresenter)
					{
						return frameworkElement;
					}
				}
				return null;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600381A RID: 14362 RVA: 0x000FA158 File Offset: 0x000F8358
		internal static TextEditorThreadLocalStore _ThreadLocalStore
		{
			get
			{
				TextEditorThreadLocalStore textEditorThreadLocalStore = (TextEditorThreadLocalStore)Thread.GetData(TextEditor._threadLocalStoreSlot);
				if (textEditorThreadLocalStore == null)
				{
					textEditorThreadLocalStore = new TextEditorThreadLocalStore();
					Thread.SetData(TextEditor._threadLocalStoreSlot, textEditorThreadLocalStore);
				}
				return textEditorThreadLocalStore;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x0600381B RID: 14363 RVA: 0x000FA18A File Offset: 0x000F838A
		internal long _ContentChangeCounter
		{
			get
			{
				return this._contentChangeCounter;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x0600381C RID: 14364 RVA: 0x000FA192 File Offset: 0x000F8392
		// (set) Token: 0x0600381D RID: 14365 RVA: 0x000FA199 File Offset: 0x000F8399
		internal static bool IsTableEditingEnabled
		{
			get
			{
				return TextEditor._isTableEditingEnabled;
			}
			set
			{
				TextEditor._isTableEditingEnabled = value;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x0600381E RID: 14366 RVA: 0x000FA1A1 File Offset: 0x000F83A1
		// (set) Token: 0x0600381F RID: 14367 RVA: 0x000FA1A9 File Offset: 0x000F83A9
		internal ITextPointer _NextLineAdvanceMovingPosition
		{
			get
			{
				return this._nextLineAdvanceMovingPosition;
			}
			set
			{
				this._nextLineAdvanceMovingPosition = value;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003820 RID: 14368 RVA: 0x000FA1B2 File Offset: 0x000F83B2
		// (set) Token: 0x06003821 RID: 14369 RVA: 0x000FA1BA File Offset: 0x000F83BA
		internal bool _IsNextLineAdvanceMovingPositionAtDocumentHead
		{
			get
			{
				return this._isNextLineAdvanceMovingPositionAtDocumentHead;
			}
			set
			{
				this._isNextLineAdvanceMovingPositionAtDocumentHead = value;
			}
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000FA1C3 File Offset: 0x000F83C3
		private bool IsBadSplitPosition(string text, int position)
		{
			return (text[position - 1] == '\r' && text[position] == '\n') || (char.IsHighSurrogate(text, position - 1) && char.IsLowSurrogate(text, position));
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000FA1F4 File Offset: 0x000F83F4
		private void HandleMouseSelectionTick(object sender, EventArgs e)
		{
			if (this._mouseSelectionState != null && !this._mouseSelectionState.BringIntoViewInProgress && this.TextView != null && this.TextView.IsValid && TextEditorSelection.IsPaginated(this.TextView))
			{
				this._mouseSelectionState.BringIntoViewInProgress = true;
				this.TextView.BringPointIntoViewCompleted += this.HandleBringPointIntoViewCompleted;
				this.TextView.BringPointIntoViewAsync(this._mouseSelectionState.Point, this);
			}
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000FA274 File Offset: 0x000F8474
		private void HandleBringPointIntoViewCompleted(object sender, BringPointIntoViewCompletedEventArgs e)
		{
			Invariant.Assert(sender is ITextView);
			((ITextView)sender).BringPointIntoViewCompleted -= this.HandleBringPointIntoViewCompleted;
			if (this._mouseSelectionState == null)
			{
				return;
			}
			this._mouseSelectionState.BringIntoViewInProgress = false;
			if (e == null || e.Cancelled || e.Error != null)
			{
				this.CancelExtendSelection();
				return;
			}
			Invariant.Assert(e.UserState == this && this.TextView == sender);
			ITextPointer textPointer = e.Position;
			if (textPointer != null)
			{
				if (textPointer.GetNextInsertionPosition(LogicalDirection.Forward) == null && textPointer.ParentType != null)
				{
					Rect characterRect = textPointer.GetCharacterRect(LogicalDirection.Backward);
					if (e.Point.X > characterRect.X + characterRect.Width)
					{
						textPointer = this.TextContainer.End;
					}
				}
				this.Selection.ExtendSelectionByMouse(textPointer, this._forceWordSelection, this._forceParagraphSelection);
				return;
			}
			this.CancelExtendSelection();
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000FA36C File Offset: 0x000F856C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private object InitTextStore(object o)
		{
			if (!this._pendingTextStoreInit)
			{
				return null;
			}
			if (this._textContainer is TextContainer && TextServicesHost.Current != null)
			{
				UnsafeNativeMethods.ITfThreadMgr tfThreadMgr = TextServicesLoader.Load();
				if (tfThreadMgr != null)
				{
					if (this._textstore == null)
					{
						this._textstore = new TextStore(this);
						this._weakThis = new TextEditor.TextEditorShutDownListener(this);
					}
					this._textstore.OnAttach();
					Marshal.ReleaseComObject(tfThreadMgr);
				}
			}
			this._pendingTextStoreInit = false;
			return null;
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000FA3DA File Offset: 0x000F85DA
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			this._contentChangeCounter += 1L;
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000FA3EC File Offset: 0x000F85EC
		private void OnTextViewUpdated(object sender, EventArgs e)
		{
			this._selection.OnTextViewUpdated();
			this.UiScope.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnTextViewUpdatedWorker), EventArgs.Empty);
			if (!this._textStoreInitStarted)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.InitTextStore), null);
				this._pendingTextStoreInit = true;
				this._textStoreInitStarted = true;
			}
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000FA457 File Offset: 0x000F8657
		private object OnTextViewUpdatedWorker(object o)
		{
			if (this.TextView == null)
			{
				return null;
			}
			if (this._textstore != null)
			{
				this._textstore.OnLayoutUpdated();
			}
			if (TextEditor._immEnabled && this._immComposition != null)
			{
				this._immComposition.OnLayoutUpdated();
			}
			return null;
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000FA494 File Offset: 0x000F8694
		private static void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			textEditor._selection.UpdateCaretAndHighlight();
			textEditor.SetSpellCheckEnabled(textEditor.IsSpellCheckEnabled);
			textEditor.SetCustomDictionaries(textEditor.IsSpellCheckEnabled);
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000FA4D4 File Offset: 0x000F86D4
		private static void OnIsReadOnlyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor(frameworkElement);
			if (textEditor == null)
			{
				return;
			}
			textEditor.SetSpellCheckEnabled(textEditor.IsSpellCheckEnabled);
			if ((bool)e.NewValue && textEditor._textstore != null)
			{
				textEditor._textstore.CompleteCompositionAsync();
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x000FA524 File Offset: 0x000F8724
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender != e.NewFocus)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor._textstore != null)
			{
				textEditor._textstore.OnGotFocus();
			}
			if (TextEditor._immEnabled)
			{
				textEditor._immComposition = ImmComposition.GetImmComposition(textEditor._uiScope);
				if (textEditor._immComposition != null)
				{
					textEditor._immCompositionForDetach = new WeakReference<ImmComposition>(textEditor._immComposition);
					textEditor._immComposition.OnGotFocus(textEditor);
				}
				else
				{
					textEditor._immCompositionForDetach = null;
				}
			}
			textEditor._selection.RefreshCaret();
			textEditor._selection.UpdateCaretAndHighlight();
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000FA5C4 File Offset: 0x000F87C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender != e.OldFocus)
			{
				return;
			}
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			textEditor._selection.UpdateCaretAndHighlight();
			if (textEditor._textstore != null)
			{
				textEditor._textstore.OnLostFocus();
			}
			if (TextEditor._immEnabled && textEditor._immComposition != null)
			{
				textEditor._immComposition.OnLostFocus();
				textEditor._immComposition = null;
			}
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000FA634 File Offset: 0x000F8834
		private static void OnLostFocus(object sender, RoutedEventArgs e)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			TextEditorTyping._ShowCursor();
			if (!textEditor._IsEnabled)
			{
				return;
			}
			TextEditorTyping._FlushPendingInputItems(textEditor);
			TextEditorTyping._BreakTypingSequence(textEditor);
			if (textEditor._tableColResizeInfo != null)
			{
				textEditor._tableColResizeInfo.DisposeAdorner();
				textEditor._tableColResizeInfo = null;
			}
			textEditor._selection.UpdateCaretAndHighlight();
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x000FA690 File Offset: 0x000F8890
		private static void OnUndo(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)target);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.Undo();
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x000FA6C8 File Offset: 0x000F88C8
		private static void OnRedo(object target, ExecutedRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)target);
			if (textEditor == null)
			{
				return;
			}
			if (!textEditor._IsEnabled)
			{
				return;
			}
			if (textEditor.IsReadOnly)
			{
				return;
			}
			textEditor.Redo();
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x000FA700 File Offset: 0x000F8900
		private static void OnQueryStatusUndo(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			UndoManager undoManager = textEditor._GetUndoManager();
			if (undoManager != null && undoManager.UndoCount > undoManager.MinUndoStackCount)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x000FA73C File Offset: 0x000F893C
		private static void OnQueryStatusRedo(object sender, CanExecuteRoutedEventArgs args)
		{
			TextEditor textEditor = TextEditor._GetTextEditor((FrameworkElement)sender);
			if (textEditor == null)
			{
				return;
			}
			UndoManager undoManager = textEditor._GetUndoManager();
			if (undoManager != null && undoManager.RedoCount > 0)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x04002572 RID: 9586
		internal static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(TextEditor), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(TextEditor.OnIsReadOnlyChanged)));

		// Token: 0x04002573 RID: 9587
		internal static readonly DependencyProperty AllowOvertypeProperty = DependencyProperty.RegisterAttached("AllowOvertype", typeof(bool), typeof(TextEditor), new FrameworkPropertyMetadata(true));

		// Token: 0x04002574 RID: 9588
		internal static readonly DependencyProperty PageHeightProperty = DependencyProperty.RegisterAttached("PageHeight", typeof(double), typeof(TextEditor), new FrameworkPropertyMetadata(0.0));

		// Token: 0x04002575 RID: 9589
		private static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance", typeof(TextEditor), typeof(TextEditor), new FrameworkPropertyMetadata(null));

		// Token: 0x04002576 RID: 9590
		internal Dispatcher _dispatcher;

		// Token: 0x04002577 RID: 9591
		private bool _isReadOnly;

		// Token: 0x04002578 RID: 9592
		private static ArrayList _registeredEditingTypes = new ArrayList(4);

		// Token: 0x04002579 RID: 9593
		private ITextContainer _textContainer;

		// Token: 0x0400257A RID: 9594
		private long _contentChangeCounter;

		// Token: 0x0400257B RID: 9595
		private FrameworkElement _uiScope;

		// Token: 0x0400257C RID: 9596
		private ITextView _textView;

		// Token: 0x0400257D RID: 9597
		private ITextSelection _selection;

		// Token: 0x0400257E RID: 9598
		private bool _overtypeMode;

		// Token: 0x0400257F RID: 9599
		internal double _suggestedX;

		// Token: 0x04002580 RID: 9600
		private TextStore _textstore;

		// Token: 0x04002581 RID: 9601
		private TextEditor.TextEditorShutDownListener _weakThis;

		// Token: 0x04002582 RID: 9602
		private Speller _speller;

		// Token: 0x04002583 RID: 9603
		private bool _textStoreInitStarted;

		// Token: 0x04002584 RID: 9604
		private bool _pendingTextStoreInit;

		// Token: 0x04002585 RID: 9605
		internal Cursor _cursor;

		// Token: 0x04002586 RID: 9606
		internal IParentUndoUnit _typingUndoUnit;

		// Token: 0x04002587 RID: 9607
		internal TextEditorDragDrop._DragDropProcess _dragDropProcess;

		// Token: 0x04002588 RID: 9608
		internal bool _forceWordSelection;

		// Token: 0x04002589 RID: 9609
		internal bool _forceParagraphSelection;

		// Token: 0x0400258A RID: 9610
		internal TextRangeEditTables.TableColumnResizeInfo _tableColResizeInfo;

		// Token: 0x0400258B RID: 9611
		private UndoState _undoState;

		// Token: 0x0400258C RID: 9612
		private bool _acceptsRichContent;

		// Token: 0x0400258D RID: 9613
		private static bool _immEnabled = SafeSystemMetrics.IsImmEnabled;

		// Token: 0x0400258E RID: 9614
		private ImmComposition _immComposition;

		// Token: 0x0400258F RID: 9615
		private WeakReference<ImmComposition> _immCompositionForDetach;

		// Token: 0x04002590 RID: 9616
		private static LocalDataStoreSlot _threadLocalStoreSlot = Thread.AllocateDataSlot();

		// Token: 0x04002591 RID: 9617
		internal bool _mouseCapturingInProgress;

		// Token: 0x04002592 RID: 9618
		private TextEditor.MouseSelectionState _mouseSelectionState;

		// Token: 0x04002593 RID: 9619
		private bool _isContextMenuOpen;

		// Token: 0x04002594 RID: 9620
		private static bool _isTableEditingEnabled;

		// Token: 0x04002595 RID: 9621
		private ITextPointer _nextLineAdvanceMovingPosition;

		// Token: 0x04002596 RID: 9622
		internal bool _isNextLineAdvanceMovingPositionAtDocumentHead;

		// Token: 0x020008FB RID: 2299
		private sealed class TextEditorShutDownListener : ShutDownListener
		{
			// Token: 0x060085A6 RID: 34214 RVA: 0x0024A295 File Offset: 0x00248495
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public TextEditorShutDownListener(TextEditor target) : base(target, ShutDownEvents.DomainUnload | ShutDownEvents.DispatcherShutdown)
			{
			}

			// Token: 0x060085A7 RID: 34215 RVA: 0x0024A2A0 File Offset: 0x002484A0
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				TextEditor textEditor = (TextEditor)target;
				textEditor.DetachTextStore(false);
			}
		}

		// Token: 0x020008FC RID: 2300
		private class MouseSelectionState
		{
			// Token: 0x040042F2 RID: 17138
			internal DispatcherTimer Timer;

			// Token: 0x040042F3 RID: 17139
			internal Point Point;

			// Token: 0x040042F4 RID: 17140
			internal bool BringIntoViewInProgress;
		}
	}
}
