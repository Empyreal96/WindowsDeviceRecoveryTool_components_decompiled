using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control designed for entering and handling passwords.</summary>
	// Token: 0x0200050F RID: 1295
	[TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
	public sealed class PasswordBox : Control, ITextBoxViewHost
	{
		// Token: 0x06005321 RID: 21281 RVA: 0x0017215C File Offset: 0x0017035C
		static PasswordBox()
		{
			PasswordBox.PasswordChangedEvent = EventManager.RegisterRoutedEvent("PasswordChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PasswordBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
			PasswordBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(PasswordBox));
			PasswordBox.PasswordCharProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnPasswordCharChanged)));
			Control.PaddingProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnPaddingChanged)));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.OnParentNavigationServiceChanged)));
			InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(PasswordBox.ForceToFalse)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(PasswordBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(PasswordBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			TextBoxBase.SelectionBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.SelectionTextBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			TextBoxBase.CaretBrushProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(PasswordBox.UpdateCaretElement)));
			ControlsTraceLogger.AddControl(TelemetryControls.PasswordBox);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.PasswordBox" /> class.</summary>
		// Token: 0x06005322 RID: 21282 RVA: 0x00172427 File Offset: 0x00170627
		public PasswordBox()
		{
			this.Initialize();
		}

		/// <summary>Replaces the current selection in the <see cref="T:System.Windows.Controls.PasswordBox" /> with the contents of the Clipboard.</summary>
		// Token: 0x06005323 RID: 21283 RVA: 0x00172438 File Offset: 0x00170638
		public void Paste()
		{
			RoutedCommand paste = ApplicationCommands.Paste;
			paste.Execute(null, this);
		}

		/// <summary>Selects the entire contents of the <see cref="T:System.Windows.Controls.PasswordBox" />.</summary>
		// Token: 0x06005324 RID: 21284 RVA: 0x00172453 File Offset: 0x00170653
		public void SelectAll()
		{
			this.Selection.Select(this.TextContainer.Start, this.TextContainer.End);
		}

		/// <summary>Clears the value of the <see cref="P:System.Windows.Controls.PasswordBox.Password" /> property.</summary>
		// Token: 0x06005325 RID: 21285 RVA: 0x00172476 File Offset: 0x00170676
		public void Clear()
		{
			this.Password = string.Empty;
		}

		/// <summary>Gets or sets the password currently held by the <see cref="T:System.Windows.Controls.PasswordBox" />.</summary>
		/// <returns>A string representing the password currently held by the <see cref="T:System.Windows.Controls.PasswordBox" />.The default value is <see cref="F:System.String.Empty" />.</returns>
		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x00172484 File Offset: 0x00170684
		// (set) Token: 0x06005327 RID: 21287 RVA: 0x001724DC File Offset: 0x001706DC
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public unsafe string Password
		{
			[SecurityCritical]
			get
			{
				string result;
				using (SecureString securePassword = this.SecurePassword)
				{
					IntPtr intPtr = Marshal.SecureStringToBSTR(securePassword);
					try
					{
						result = new string((char*)((void*)intPtr));
					}
					finally
					{
						Marshal.ZeroFreeBSTR(intPtr);
					}
				}
				return result;
			}
			[SecurityCritical]
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				using (SecureString secureString = new SecureString())
				{
					for (int i = 0; i < value.Length; i++)
					{
						secureString.AppendChar(value[i]);
					}
					this.SetSecurePassword(secureString);
				}
			}
		}

		/// <summary>Gets the password currently held by the <see cref="T:System.Windows.Controls.PasswordBox" /> as a <see cref="T:System.Security.SecureString" />.</summary>
		/// <returns>A secure string representing the password currently held by the <see cref="T:System.Windows.Controls.PasswordBox" />.</returns>
		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06005328 RID: 21288 RVA: 0x0017253C File Offset: 0x0017073C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SecureString SecurePassword
		{
			get
			{
				return this.TextContainer.GetPasswordCopy();
			}
		}

		/// <summary>Gets or sets the masking character for the <see cref="T:System.Windows.Controls.PasswordBox" />.  </summary>
		/// <returns>A masking character to echo when the user enters text into the <see cref="T:System.Windows.Controls.PasswordBox" />.The default value is a bullet character (●).</returns>
		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06005329 RID: 21289 RVA: 0x00172549 File Offset: 0x00170749
		// (set) Token: 0x0600532A RID: 21290 RVA: 0x0017255B File Offset: 0x0017075B
		public char PasswordChar
		{
			get
			{
				return (char)base.GetValue(PasswordBox.PasswordCharProperty);
			}
			set
			{
				base.SetValue(PasswordBox.PasswordCharProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum length for passwords to be handled by this <see cref="T:System.Windows.Controls.PasswordBox" />.  </summary>
		/// <returns>An integer specifying the maximum length, in characters, for passwords to be handled by this <see cref="T:System.Windows.Controls.PasswordBox" />.A value of zero (0) means no limit.The default value is 0 (no length limit).</returns>
		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x0600532B RID: 21291 RVA: 0x0017256E File Offset: 0x0017076E
		// (set) Token: 0x0600532C RID: 21292 RVA: 0x00172580 File Offset: 0x00170780
		[DefaultValue(0)]
		public int MaxLength
		{
			get
			{
				return (int)base.GetValue(PasswordBox.MaxLengthProperty);
			}
			set
			{
				base.SetValue(PasswordBox.MaxLengthProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that highlights selected text.</summary>
		/// <returns>A brush that highlights selected text.</returns>
		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x00172593 File Offset: 0x00170793
		// (set) Token: 0x0600532E RID: 21294 RVA: 0x001725A5 File Offset: 0x001707A5
		public Brush SelectionBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.SelectionBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionBrushProperty, value);
			}
		}

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x0600532F RID: 21295 RVA: 0x001725B3 File Offset: 0x001707B3
		// (set) Token: 0x06005330 RID: 21296 RVA: 0x001725C5 File Offset: 0x001707C5
		public Brush SelectionTextBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.SelectionTextBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionTextBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the opacity of the <see cref="P:System.Windows.Controls.PasswordBox.SelectionBrush" />.</summary>
		/// <returns>The opacity of the <see cref="P:System.Windows.Controls.PasswordBox.SelectionBrush" />. The default is 0.4.</returns>
		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06005331 RID: 21297 RVA: 0x001725D3 File Offset: 0x001707D3
		// (set) Token: 0x06005332 RID: 21298 RVA: 0x001725E5 File Offset: 0x001707E5
		public double SelectionOpacity
		{
			get
			{
				return (double)base.GetValue(PasswordBox.SelectionOpacityProperty);
			}
			set
			{
				base.SetValue(PasswordBox.SelectionOpacityProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that specifies the color of the password box's caret.</summary>
		/// <returns>A brush that describes the color of the password box's caret.</returns>
		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x001725F8 File Offset: 0x001707F8
		// (set) Token: 0x06005334 RID: 21300 RVA: 0x0017260A File Offset: 0x0017080A
		public Brush CaretBrush
		{
			get
			{
				return (Brush)base.GetValue(PasswordBox.CaretBrushProperty);
			}
			set
			{
				base.SetValue(PasswordBox.CaretBrushProperty, value);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.PasswordBox" /> has focus and selected text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.PasswordBox" /> has focus and selected text; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x00172618 File Offset: 0x00170818
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(PasswordBox.IsSelectionActiveProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.PasswordBox" /> displays selected text when the <see cref="T:System.Windows.Controls.PasswordBox" /> does not have focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.PasswordBox" /> displays selected text when the <see cref="T:System.Windows.Controls.PasswordBox" /> does not have focus; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x0017262A File Offset: 0x0017082A
		// (set) Token: 0x06005337 RID: 21303 RVA: 0x0017263C File Offset: 0x0017083C
		public bool IsInactiveSelectionHighlightEnabled
		{
			get
			{
				return (bool)base.GetValue(PasswordBox.IsInactiveSelectionHighlightEnabledProperty);
			}
			set
			{
				base.SetValue(PasswordBox.IsInactiveSelectionHighlightEnabledProperty, value);
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Controls.PasswordBox.Password" /> property changes.</summary>
		// Token: 0x14000103 RID: 259
		// (add) Token: 0x06005338 RID: 21304 RVA: 0x0017264A File Offset: 0x0017084A
		// (remove) Token: 0x06005339 RID: 21305 RVA: 0x00172658 File Offset: 0x00170858
		public event RoutedEventHandler PasswordChanged
		{
			add
			{
				base.AddHandler(PasswordBox.PasswordChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(PasswordBox.PasswordChangedEvent, value);
			}
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x00172668 File Offset: 0x00170868
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x0017270B File Offset: 0x0017090B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new PasswordBoxAutomationPeer(this);
		}

		/// <summary>Called when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />, which is used to build the current template's visual tree. </summary>
		// Token: 0x0600533C RID: 21308 RVA: 0x00172713 File Offset: 0x00170913
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.AttachToVisualTree();
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x00172721 File Offset: 0x00170921
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			if (oldTemplate != null && newTemplate != null && oldTemplate.VisualTree != newTemplate.VisualTree)
			{
				this.DetachFromVisualTree();
			}
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x00172748 File Offset: 0x00170948
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (this.RenderScope != null)
			{
				FrameworkPropertyMetadata frameworkPropertyMetadata = e.Property.GetMetadata(typeof(PasswordBox)) as FrameworkPropertyMetadata;
				if (frameworkPropertyMetadata != null && (e.IsAValueChange || e.IsASubPropertyChange))
				{
					if (frameworkPropertyMetadata.AffectsMeasure || frameworkPropertyMetadata.AffectsArrange || frameworkPropertyMetadata.AffectsParentMeasure || frameworkPropertyMetadata.AffectsParentArrange || e.Property == Control.HorizontalContentAlignmentProperty || e.Property == Control.VerticalContentAlignmentProperty)
					{
						((TextBoxView)this.RenderScope).Remeasure();
						return;
					}
					if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						((TextBoxView)this.RenderScope).Rerender();
					}
				}
			}
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x00172811 File Offset: 0x00170A11
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnKeyDown(e);
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0017282F File Offset: 0x00170A2F
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnKeyUp(e);
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0017284D File Offset: 0x00170A4D
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnTextInput(e);
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x0017286B File Offset: 0x00170A6B
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseDown(e);
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x00172889 File Offset: 0x00170A89
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseMove(e);
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x001728A7 File Offset: 0x00170AA7
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnMouseUp(e);
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x001728C5 File Offset: 0x00170AC5
		protected override void OnQueryCursor(QueryCursorEventArgs e)
		{
			base.OnQueryCursor(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnQueryCursor(e);
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x001728E3 File Offset: 0x00170AE3
		protected override void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
			base.OnQueryContinueDrag(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnQueryContinueDrag(e);
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x00172901 File Offset: 0x00170B01
		protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
			base.OnGiveFeedback(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnGiveFeedback(e);
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0017291F File Offset: 0x00170B1F
		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragEnter(e);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0017293D File Offset: 0x00170B3D
		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragOver(e);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x0017295B File Offset: 0x00170B5B
		protected override void OnDragLeave(DragEventArgs e)
		{
			base.OnDragLeave(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDragLeave(e);
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x00172979 File Offset: 0x00170B79
		protected override void OnDrop(DragEventArgs e)
		{
			base.OnDrop(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnDrop(e);
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x00172997 File Offset: 0x00170B97
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnContextMenuOpening(e);
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001729B5 File Offset: 0x00170BB5
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnGotKeyboardFocus(e);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001729D3 File Offset: 0x00170BD3
		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnLostKeyboardFocus(e);
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001729F1 File Offset: 0x00170BF1
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			if (e.Handled)
			{
				return;
			}
			this._textEditor.OnLostFocus(e);
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06005350 RID: 21328 RVA: 0x00172A0F File Offset: 0x00170C0F
		internal PasswordTextContainer TextContainer
		{
			get
			{
				return this._textContainer;
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06005351 RID: 21329 RVA: 0x00172A17 File Offset: 0x00170C17
		internal FrameworkElement RenderScope
		{
			get
			{
				return this._renderScope;
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x00172A1F File Offset: 0x00170C1F
		internal ScrollViewer ScrollViewer
		{
			get
			{
				if (this._scrollViewer == null && this._textEditor != null)
				{
					this._scrollViewer = (this._textEditor._Scroller as ScrollViewer);
				}
				return this._scrollViewer;
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06005353 RID: 21331 RVA: 0x00172A4D File Offset: 0x00170C4D
		ITextContainer ITextBoxViewHost.TextContainer
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06005354 RID: 21332 RVA: 0x00016748 File Offset: 0x00014948
		bool ITextBoxViewHost.IsTypographyDefaultValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005355 RID: 21333 RVA: 0x00172A55 File Offset: 0x00170C55
		private void Initialize()
		{
			TextEditor.RegisterCommandHandlers(typeof(PasswordBox), false, false, false);
			this.InitializeTextContainer(new PasswordTextContainer(this));
			this._textEditor.AcceptsRichContent = false;
			this._textEditor.AcceptsTab = false;
		}

		// Token: 0x06005356 RID: 21334 RVA: 0x00172A90 File Offset: 0x00170C90
		private void InitializeTextContainer(PasswordTextContainer textContainer)
		{
			Invariant.Assert(textContainer != null);
			if (this._textContainer != null)
			{
				Invariant.Assert(this._textEditor != null);
				Invariant.Assert(this._textEditor.TextContainer == this._textContainer);
				this.DetachFromVisualTree();
				this._textEditor.OnDetach();
			}
			this._textContainer = textContainer;
			((ITextContainer)this._textContainer).Changed += this.OnTextContainerChanged;
			this._textEditor = new TextEditor(this._textContainer, this, true);
		}

		// Token: 0x06005357 RID: 21335 RVA: 0x00172B16 File Offset: 0x00170D16
		private static object ForceToFalse(DependencyObject d, object value)
		{
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x00172B20 File Offset: 0x00170D20
		private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox._renderScope != null)
			{
				passwordBox._renderScope.InvalidateMeasure();
			}
		}

		// Token: 0x06005359 RID: 21337 RVA: 0x00172B47 File Offset: 0x00170D47
		private void OnTextContainerChanged(object sender, TextContainerChangedEventArgs e)
		{
			if (!e.HasContentAddedOrRemoved)
			{
				return;
			}
			base.RaiseEvent(new RoutedEventArgs(PasswordBox.PasswordChangedEvent));
		}

		// Token: 0x0600535A RID: 21338 RVA: 0x00172B64 File Offset: 0x00170D64
		private void SetRenderScopeToContentHost(TextBoxView renderScope)
		{
			this.ClearContentHost();
			this._passwordBoxContentHost = (base.GetTemplateChild("PART_ContentHost") as FrameworkElement);
			this._renderScope = renderScope;
			if (this._passwordBoxContentHost is ScrollViewer)
			{
				ScrollViewer scrollViewer = (ScrollViewer)this._passwordBoxContentHost;
				if (scrollViewer.Content != null)
				{
					throw new NotSupportedException(SR.Get("TextBoxScrollViewerMarkedAsTextBoxContentMustHaveNoContent"));
				}
				scrollViewer.Content = this._renderScope;
			}
			else if (this._passwordBoxContentHost is Decorator)
			{
				Decorator decorator = (Decorator)this._passwordBoxContentHost;
				if (decorator.Child != null)
				{
					throw new NotSupportedException(SR.Get("TextBoxDecoratorMarkedAsTextBoxContentMustHaveNoContent"));
				}
				decorator.Child = this._renderScope;
			}
			else
			{
				this._renderScope = null;
				if (this._passwordBoxContentHost != null)
				{
					this._passwordBoxContentHost = null;
					throw new NotSupportedException(SR.Get("PasswordBoxInvalidTextContainer"));
				}
			}
			this.InitializeRenderScope();
			FrameworkElement frameworkElement = this._renderScope;
			while (frameworkElement != this && frameworkElement != null)
			{
				if (frameworkElement is Border)
				{
					this._border = (Border)frameworkElement;
				}
				frameworkElement = (frameworkElement.Parent as FrameworkElement);
			}
		}

		// Token: 0x0600535B RID: 21339 RVA: 0x00172C70 File Offset: 0x00170E70
		private void ClearContentHost()
		{
			this.UninitializeRenderScope();
			if (this._passwordBoxContentHost is ScrollViewer)
			{
				((ScrollViewer)this._passwordBoxContentHost).Content = null;
			}
			else if (this._passwordBoxContentHost is Decorator)
			{
				((Decorator)this._passwordBoxContentHost).Child = null;
			}
			else
			{
				Invariant.Assert(this._passwordBoxContentHost == null, "_passwordBoxContentHost must be null here");
			}
			this._passwordBoxContentHost = null;
		}

		// Token: 0x0600535C RID: 21340 RVA: 0x00172CE0 File Offset: 0x00170EE0
		private void InitializeRenderScope()
		{
			if (this._renderScope == null)
			{
				return;
			}
			ITextView textView = TextEditor.GetTextView(this._renderScope);
			this._textEditor.TextView = textView;
			this.TextContainer.TextView = textView;
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.CanContentScroll = true;
			}
		}

		// Token: 0x0600535D RID: 21341 RVA: 0x00172D2E File Offset: 0x00170F2E
		private void UninitializeRenderScope()
		{
			this._textEditor.TextView = null;
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x00172D3C File Offset: 0x00170F3C
		private void ResetSelection()
		{
			this.Select(0, 0);
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x00172D5C File Offset: 0x00170F5C
		private void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.Get("ParameterCannotBeNegative"));
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", SR.Get("ParameterCannotBeNegative"));
			}
			ITextPointer textPointer = this.TextContainer.Start.CreatePointer();
			while (start-- > 0 && textPointer.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
			}
			ITextPointer textPointer2 = textPointer.CreatePointer();
			while (length-- > 0 && textPointer2.MoveToNextInsertionPosition(LogicalDirection.Forward))
			{
			}
			this.Selection.Select(textPointer, textPointer2);
		}

		// Token: 0x06005360 RID: 21344 RVA: 0x00172DE4 File Offset: 0x00170FE4
		private static void OnPaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox.ScrollViewer != null)
			{
				object value = passwordBox.GetValue(Control.PaddingProperty);
				if (value is Thickness)
				{
					passwordBox.ScrollViewer.Padding = (Thickness)value;
					return;
				}
				passwordBox.ScrollViewer.ClearValue(Control.PaddingProperty);
			}
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x00172E38 File Offset: 0x00171038
		private static void OnParentNavigationServiceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)o;
			NavigationService navigationService = NavigationService.GetNavigationService(o);
			if (passwordBox._navigationService != null)
			{
				passwordBox._navigationService.Navigating -= passwordBox.OnNavigating;
			}
			if (navigationService != null)
			{
				navigationService.Navigating += passwordBox.OnNavigating;
				passwordBox._navigationService = navigationService;
				return;
			}
			passwordBox._navigationService = null;
		}

		// Token: 0x06005362 RID: 21346 RVA: 0x00172476 File Offset: 0x00170676
		private void OnNavigating(object sender, NavigatingCancelEventArgs e)
		{
			this.Password = string.Empty;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x00172E98 File Offset: 0x00171098
		private void AttachToVisualTree()
		{
			this.DetachFromVisualTree();
			this.SetRenderScopeToContentHost(new TextBoxView(this));
			if (this.ScrollViewer != null)
			{
				this.ScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
				this.ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
				this.ScrollViewer.Focusable = false;
				if (this.ScrollViewer.Background == null)
				{
					this.ScrollViewer.Background = Brushes.Transparent;
				}
				PasswordBox.OnPaddingChanged(this, default(DependencyPropertyChangedEventArgs));
			}
			if (this._border != null)
			{
				this._border.Style = null;
			}
		}

		// Token: 0x06005364 RID: 21348 RVA: 0x00172F23 File Offset: 0x00171123
		private void DetachFromVisualTree()
		{
			if (this._textEditor != null)
			{
				this._textEditor.Selection.DetachFromVisualTree();
			}
			this._scrollViewer = null;
			this._border = null;
			this.ClearContentHost();
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x00172F54 File Offset: 0x00171154
		[SecurityCritical]
		private void SetSecurePassword(SecureString value)
		{
			this.TextContainer.BeginChange();
			try
			{
				this.TextContainer.SetPassword(value);
				this.ResetSelection();
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x00172F9C File Offset: 0x0017119C
		private static void UpdateCaretElement(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = (PasswordBox)d;
			if (passwordBox.Selection != null)
			{
				CaretElement caretElement = passwordBox.Selection.CaretElement;
				if (caretElement != null)
				{
					if (e.Property == PasswordBox.CaretBrushProperty)
					{
						caretElement.UpdateCaretBrush(TextSelection.GetCaretBrush(passwordBox.Selection.TextEditor));
					}
					caretElement.InvalidateVisual();
				}
				TextBoxView textBoxView = ((passwordBox != null) ? passwordBox.RenderScope : null) as TextBoxView;
				TextBoxView textBoxView2 = textBoxView;
				if (textBoxView2 != null && ((ITextView)textBoxView2).RendersOwnSelection)
				{
					textBoxView.InvalidateArrange();
				}
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06005367 RID: 21351 RVA: 0x00173018 File Offset: 0x00171218
		private ITextSelection Selection
		{
			get
			{
				return this._textEditor.Selection;
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06005368 RID: 21352 RVA: 0x00173025 File Offset: 0x00171225
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return PasswordBox._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.PasswordChar" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.PasswordBox.PasswordChar" /> dependency property.</returns>
		// Token: 0x04002CEA RID: 11498
		public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.RegisterAttached("PasswordChar", typeof(char), typeof(PasswordBox), new FrameworkPropertyMetadata('*'));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.MaxLength" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.PasswordBox.MaxLength" /> dependency property.</returns>
		// Token: 0x04002CEB RID: 11499
		public static readonly DependencyProperty MaxLengthProperty = TextBox.MaxLengthProperty.AddOwner(typeof(PasswordBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.SelectionBrush" /> dependency property.</summary>
		// Token: 0x04002CEC RID: 11500
		public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x04002CED RID: 11501
		public static readonly DependencyProperty SelectionTextBrushProperty = TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(PasswordBox));

		/// <summary>Identifies the <see cref="F:System.Windows.Controls.PasswordBox.SelectionOpacityProperty" /> dependency property.</summary>
		// Token: 0x04002CEE RID: 11502
		public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(PasswordBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.CaretBrush" /> dependency property.</summary>
		// Token: 0x04002CEF RID: 11503
		public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof(PasswordBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.IsSelectionActive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.PasswordBox.IsSelectionActive" /> dependency property.</returns>
		// Token: 0x04002CF0 RID: 11504
		public static readonly DependencyProperty IsSelectionActiveProperty = TextBoxBase.IsSelectionActiveProperty.AddOwner(typeof(PasswordBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.PasswordBox.IsInactiveSelectionHighlightEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.PasswordBox.IsInactiveSelectionHighlightEnabled" /> dependency property.</returns>
		// Token: 0x04002CF1 RID: 11505
		public static readonly DependencyProperty IsInactiveSelectionHighlightEnabledProperty = TextBoxBase.IsInactiveSelectionHighlightEnabledProperty.AddOwner(typeof(PasswordBox));

		// Token: 0x04002CF3 RID: 11507
		private TextEditor _textEditor;

		// Token: 0x04002CF4 RID: 11508
		private PasswordTextContainer _textContainer;

		// Token: 0x04002CF5 RID: 11509
		private TextBoxView _renderScope;

		// Token: 0x04002CF6 RID: 11510
		private ScrollViewer _scrollViewer;

		// Token: 0x04002CF7 RID: 11511
		private Border _border;

		// Token: 0x04002CF8 RID: 11512
		private FrameworkElement _passwordBoxContentHost;

		// Token: 0x04002CF9 RID: 11513
		private const int _defaultWidth = 100;

		// Token: 0x04002CFA RID: 11514
		private const int _defaultHeight = 20;

		// Token: 0x04002CFB RID: 11515
		private const string ContentHostTemplateName = "PART_ContentHost";

		// Token: 0x04002CFC RID: 11516
		private static DependencyObjectType _dType;

		// Token: 0x04002CFD RID: 11517
		private NavigationService _navigationService;
	}
}
