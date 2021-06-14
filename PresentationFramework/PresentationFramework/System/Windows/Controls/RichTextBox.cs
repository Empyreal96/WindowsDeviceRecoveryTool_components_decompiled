using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Documents;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a rich editing control which operates on <see cref="T:System.Windows.Documents.FlowDocument" /> objects.</summary>
	// Token: 0x02000520 RID: 1312
	[Localizability(LocalizationCategory.Inherit)]
	[ContentProperty("Document")]
	public class RichTextBox : TextBoxBase, IAddChild
	{
		// Token: 0x060054B9 RID: 21689 RVA: 0x0017743C File Offset: 0x0017563C
		static RichTextBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(typeof(RichTextBox)));
			RichTextBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RichTextBox));
			KeyboardNavigation.AcceptsReturnProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(true));
			TextBoxBase.AutoWordSelectionProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(true));
			if (!FrameworkAppContextSwitches.UseAdornerForTextboxSelectionRendering)
			{
				TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(0.4));
			}
			RichTextBox.HookupInheritablePropertyListeners();
			ControlsTraceLogger.AddControl(TelemetryControls.RichTextBox);
		}

		/// <summary>Initializes a new, default instance of the <see cref="T:System.Windows.Controls.RichTextBox" /> class.</summary>
		// Token: 0x060054BA RID: 21690 RVA: 0x00177536 File Offset: 0x00175736
		public RichTextBox() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.RichTextBox" /> class, adding a specified <see cref="T:System.Windows.Documents.FlowDocument" /> as the initial content.</summary>
		/// <param name="document">A <see cref="T:System.Windows.Documents.FlowDocument" /> to be added as the initial contents of the new <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		// Token: 0x060054BB RID: 21691 RVA: 0x00177540 File Offset: 0x00175740
		public RichTextBox(FlowDocument document)
		{
			TextEditor.RegisterCommandHandlers(typeof(RichTextBox), true, false, false);
			if (document == null)
			{
				document = new FlowDocument();
				document.Blocks.Add(new Paragraph());
				this._implicitDocument = true;
			}
			this.Document = document;
			Invariant.Assert(base.TextContainer != null);
			Invariant.Assert(base.TextEditor != null);
			Invariant.Assert(base.TextEditor.TextContainer == base.TextContainer);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value"> An object to add as a child.</param>
		// Token: 0x060054BC RID: 21692 RVA: 0x001775C4 File Offset: 0x001757C4
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is FlowDocument))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(FlowDocument)
				}), "value");
			}
			if (!this._implicitDocument)
			{
				throw new ArgumentException(SR.Get("CanOnlyHaveOneChild", new object[]
				{
					base.GetType(),
					value.GetType()
				}));
			}
			this.Document = (FlowDocument)value;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text"> A string to add to the object.</param>
		// Token: 0x060054BD RID: 21693 RVA: 0x000868A2 File Offset: 0x00084AA2
		void IAddChild.AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> that points to the insertion point closest to the specified position.</summary>
		/// <param name="point">A <see cref="T:System.Windows.Point" /> object specifying the position to retrieve a <see cref="T:System.Windows.Documents.TextPointer" /> for.</param>
		/// <param name="snapToText">If <see langword="true" />, this method always returns a <see cref="T:System.Windows.Documents.TextPointer" /> specifying the closest insertion position for the <see cref="T:System.Windows.Point" /> specified, regardless or whether or not the supplied <see cref="T:System.Windows.Point" /> is inside a character's bounding box.If <see langword="false" />, this method returns <see langword="null" /> if the specified <see cref="T:System.Windows.Point" /> does not fall within any character bounding box.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> specifying the closest insertion position for the supplied point, or <see langword="null" /> if <paramref name="snapToText" /> is <see langword="false" /> and the supplied <see cref="T:System.Windows.Point" /> is not within any character's bounding box. Note that the <see cref="T:System.Windows.Documents.TextPointer" /> returned is usually the position between two characters. Use the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> property of the returned <see cref="T:System.Windows.Documents.TextPointer" /> to determine which of the two characters the <see cref="T:System.Windows.Documents.TextPointer" /> corresponds to.</returns>
		/// <exception cref="T:System.InvalidOperationException">Raised if layout information for the <see cref="T:System.Windows.Controls.RichTextBox" /> is not current.</exception>
		// Token: 0x060054BE RID: 21694 RVA: 0x00177654 File Offset: 0x00175854
		public TextPointer GetPositionFromPoint(Point point, bool snapToText)
		{
			if (base.RenderScope == null)
			{
				return null;
			}
			return base.GetTextPositionFromPointInternal(point, snapToText);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Controls.SpellingError" /> object associated with any spelling error at a specified position in the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="position">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies a position and logical direction that resolves to a character to examine for a spelling error. Use the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> property of this <see cref="T:System.Windows.Documents.TextPointer" /> to specify the direction of the character to examine.</param>
		/// <returns>A <see cref="T:System.Windows.Controls.SpellingError" /> object containing the details of the spelling error found at the character indicated by <paramref name="position" />, or <see langword="null" /> if no spelling error exists at the specified character. </returns>
		// Token: 0x060054BF RID: 21695 RVA: 0x00177668 File Offset: 0x00175868
		public SpellingError GetSpellingError(TextPointer position)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			return base.TextEditor.GetSpellingErrorAtPosition(position, position.LogicalDirection);
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextRange" /> object covering any misspelled word at a specified position in the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="position">A <see cref="T:System.Windows.Documents.TextPointer" /> that specifies a position and logical direction that resolves to a character to examine for a spelling error. Use the <see cref="P:System.Windows.Documents.TextPointer.LogicalDirection" /> property of this <see cref="T:System.Windows.Documents.TextPointer" /> to specify the direction of the character to examine.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextRange" /> object covering any misspelled word that includes the character specified by <paramref name="position" />, or <see langword="null" /> if no spelling error exists at the specified character.</returns>
		// Token: 0x060054C0 RID: 21696 RVA: 0x00177688 File Offset: 0x00175888
		public TextRange GetSpellingErrorRange(TextPointer position)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			SpellingError spellingErrorAtPosition = base.TextEditor.GetSpellingErrorAtPosition(position, position.LogicalDirection);
			if (spellingErrorAtPosition != null)
			{
				return new TextRange(spellingErrorAtPosition.Start, spellingErrorAtPosition.End);
			}
			return null;
		}

		/// <summary>Returns a <see cref="T:System.Windows.Documents.TextPointer" /> that points to the next spelling error in the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="position">A <see cref="T:System.Windows.Documents.TextPointer" /> indicating a position from which to search for the next spelling error.</param>
		/// <param name="direction">A <see cref="T:System.Windows.Documents.LogicalDirection" /> in which to search for the next spelling error, starting at the specified <paramref name="posision" />.</param>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> that points to the next spelling error in the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />, or <see langword="null" /> if no next spelling error exists.</returns>
		// Token: 0x060054C1 RID: 21697 RVA: 0x001776CA File Offset: 0x001758CA
		public TextPointer GetNextSpellingErrorPosition(TextPointer position, LogicalDirection direction)
		{
			ValidationHelper.VerifyPosition(base.TextContainer, position);
			return (TextPointer)base.TextEditor.GetNextSpellingErrorPosition(position, direction);
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.RichTextBox" />.</returns>
		// Token: 0x060054C2 RID: 21698 RVA: 0x001776EA File Offset: 0x001758EA
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RichTextBoxAutomationPeer(this);
		}

		/// <summary>Called when the DPI at which this Rich Text Box is rendered changes.</summary>
		/// <param name="oldDpiScaleInfo">The previous DPI scale setting.</param>
		/// <param name="newDpiScaleInfo">The new DPI scale setting.</param>
		// Token: 0x060054C3 RID: 21699 RVA: 0x001776F2 File Offset: 0x001758F2
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			FlowDocument document = this.Document;
			if (document == null)
			{
				return;
			}
			document.SetDpi(newDpiScaleInfo);
		}

		/// <summary>Called to re-measure the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <param name="constraint">A <see cref="T:System.Windows.Size" /> structure specifying constraints on the size of the <see cref="T:System.Windows.Controls.RichTextBox" />.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> structure indicating the new size of the <see cref="T:System.Windows.Controls.RichTextBox" />.</returns>
		// Token: 0x060054C4 RID: 21700 RVA: 0x00177705 File Offset: 0x00175905
		protected override Size MeasureOverride(Size constraint)
		{
			if (constraint.Width == double.PositiveInfinity)
			{
				constraint.Width = base.MinWidth;
			}
			return base.MeasureOverride(constraint);
		}

		// Token: 0x060054C5 RID: 21701 RVA: 0x00177730 File Offset: 0x00175930
		internal override FrameworkElement CreateRenderScope()
		{
			return new FlowDocumentView
			{
				Document = this.Document,
				Document = 
				{
					PagePadding = new Thickness(5.0, 0.0, 5.0, 0.0)
				},
				OverridesDefaultStyle = true
			};
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Documents.FlowDocument" /> that represents the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.FlowDocument" /> object that represents the contents of the <see cref="T:System.Windows.Controls.RichTextBox" />.By default, this property is set to an empty <see cref="T:System.Windows.Documents.FlowDocument" />.  Specifically, the empty <see cref="T:System.Windows.Documents.FlowDocument" /> contains a single <see cref="T:System.Windows.Documents.Paragraph" />, which contains a single <see cref="T:System.Windows.Documents.Run" /> which contains no text.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set this property to <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a <see cref="T:System.Windows.Documents.FlowDocument" /> that represents the contents of another <see cref="T:System.Windows.Controls.RichTextBox" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This property is set while a change block has been activated.</exception>
		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x0017778B File Offset: 0x0017598B
		// (set) Token: 0x060054C7 RID: 21703 RVA: 0x001777A4 File Offset: 0x001759A4
		public FlowDocument Document
		{
			get
			{
				Invariant.Assert(this._document != null);
				return this._document;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value != this._document && value.StructuralCache != null && value.StructuralCache.TextContainer != null && value.StructuralCache.TextContainer.TextSelection != null)
				{
					throw new ArgumentException(SR.Get("RichTextBox_DocumentBelongsToAnotherRichTextBoxAlready"));
				}
				if (this._document != null && base.TextSelectionInternal.ChangeBlockLevel > 0)
				{
					throw new InvalidOperationException(SR.Get("RichTextBox_CantSetDocumentInsideChangeBlock"));
				}
				if (value == this._document)
				{
					return;
				}
				bool flag = this._document == null;
				if (this._document != null)
				{
					this._document.PageSizeChanged -= this.OnPageSizeChangedHandler;
					base.RemoveLogicalChild(this._document);
					this._document.TextContainer.CollectTextChanges = false;
					this._document = null;
				}
				if (!flag)
				{
					this._implicitDocument = false;
				}
				this._document = value;
				this._document.SetDpi(base.GetDpi());
				UIElement renderScope = base.RenderScope;
				this._document.TextContainer.CollectTextChanges = true;
				base.InitializeTextContainer(this._document.TextContainer);
				this._document.PageSizeChanged += this.OnPageSizeChangedHandler;
				base.AddLogicalChild(this._document);
				if (renderScope != null)
				{
					this.AttachToVisualTree();
				}
				this.TransferInheritedPropertiesToFlowDocument();
				if (!flag)
				{
					base.ChangeUndoLimit(base.UndoLimit);
					base.ChangeUndoEnabled(base.IsUndoEnabled);
					Invariant.Assert(base.PendingUndoAction == UndoAction.None);
					base.PendingUndoAction = UndoAction.Clear;
					try
					{
						this.OnTextChanged(new TextChangedEventArgs(TextBoxBase.TextChangedEvent, UndoAction.Clear));
					}
					finally
					{
						base.PendingUndoAction = UndoAction.None;
					}
				}
			}
		}

		/// <summary>Returns a value that indicates whether or not the effective value of the <see cref="P:System.Windows.Controls.RichTextBox.Document" /> property should be serialized during serialization of a <see cref="T:System.Windows.Controls.RichTextBox" /> object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.RichTextBox.Document" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x060054C8 RID: 21704 RVA: 0x0017795C File Offset: 0x00175B5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeDocument()
		{
			Block firstBlock = this._document.Blocks.FirstBlock;
			if (this._implicitDocument && (firstBlock == null || (firstBlock == this._document.Blocks.LastBlock && firstBlock is Paragraph)))
			{
				Inline inline = (firstBlock == null) ? null : ((Paragraph)firstBlock).Inlines.FirstInline;
				if (inline == null || (inline == ((Paragraph)firstBlock).Inlines.LastInline && inline is Run && inline.ContentStart.CompareTo(inline.ContentEnd) == 0))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Gets or sets a value that indicates whether the user can interact with <see cref="T:System.Windows.UIElement" /> and <see cref="T:System.Windows.ContentElement" /> objects within the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can interact with <see cref="T:System.Windows.UIElement" /> and <see cref="T:System.Windows.ContentElement" /> objects within the <see cref="T:System.Windows.Controls.RichTextBox" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x060054C9 RID: 21705 RVA: 0x001779EB File Offset: 0x00175BEB
		// (set) Token: 0x060054CA RID: 21706 RVA: 0x001779FD File Offset: 0x00175BFD
		public bool IsDocumentEnabled
		{
			get
			{
				return (bool)base.GetValue(RichTextBox.IsDocumentEnabledProperty);
			}
			set
			{
				base.SetValue(RichTextBox.IsDocumentEnabledProperty, value);
			}
		}

		/// <summary>Gets an enumerator that can iterate the logical children of the <see langword="RichTextBox" />.</summary>
		/// <returns>An enumerator for the logical children.</returns>
		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x060054CB RID: 21707 RVA: 0x00177A0B File Offset: 0x00175C0B
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this._document == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new SingleChildEnumerator(this._document);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TextSelection" /> object containing the current selection in the <see cref="T:System.Windows.Controls.RichTextBox" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextSelection" /> object containing the current selection in the <see cref="T:System.Windows.Controls.RichTextBox" />.The default returned <see cref="T:System.Windows.Documents.TextSelection" /> has an <see cref="P:System.Windows.Documents.TextRange.IsEmpty" /> property value of <see langword="True" />. An empty <see cref="T:System.Windows.Documents.TextSelection" /> renders as a caret in the text area with no selection.</returns>
		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x060054CC RID: 21708 RVA: 0x00177A26 File Offset: 0x00175C26
		public TextSelection Selection
		{
			get
			{
				return base.TextSelectionInternal;
			}
		}

		/// <summary>Gets or sets the position of the input caret.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TextPointer" /> object specifying the position of the input caret.By default, the caret is at the first insertion position at the beginning of the content hosted by the <see cref="T:System.Windows.Controls.RichTextBox" />. See <see cref="T:System.Windows.Documents.TextPointer" /> for more information on text position terminology like "insertion position".</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set this property to <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a <see cref="T:System.Windows.Documents.TextPointer" /> that references a position outside of the current document.</exception>
		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x060054CD RID: 21709 RVA: 0x00177A2E File Offset: 0x00175C2E
		// (set) Token: 0x060054CE RID: 21710 RVA: 0x00177A3C File Offset: 0x00175C3C
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextPointer CaretPosition
		{
			get
			{
				return this.Selection.MovingPosition;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.Selection.Start.IsInSameDocument(value))
				{
					throw new ArgumentException(SR.Get("RichTextBox_PointerNotInSameDocument"), "value");
				}
				this.Selection.SetCaretToPosition(value, value.LogicalDirection, true, false);
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x060054CF RID: 21711 RVA: 0x00177A93 File Offset: 0x00175C93
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RichTextBox._dType;
			}
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x00177A9C File Offset: 0x00175C9C
		private static void HookupInheritablePropertyListeners()
		{
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(RichTextBox.OnFormattingPropertyChanged);
			DependencyProperty[] inheritableProperties = TextSchema.GetInheritableProperties(typeof(FlowDocument));
			for (int i = 0; i < inheritableProperties.Length; i++)
			{
				inheritableProperties[i].OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(propertyChangedCallback));
			}
			PropertyChangedCallback propertyChangedCallback2 = new PropertyChangedCallback(RichTextBox.OnBehavioralPropertyChanged);
			DependencyProperty[] behavioralProperties = TextSchema.BehavioralProperties;
			for (int j = 0; j < behavioralProperties.Length; j++)
			{
				behavioralProperties[j].OverrideMetadata(typeof(RichTextBox), new FrameworkPropertyMetadata(propertyChangedCallback2));
			}
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x00177B30 File Offset: 0x00175D30
		private void TransferInheritedPropertiesToFlowDocument()
		{
			if (this._implicitDocument)
			{
				foreach (DependencyProperty dependencyProperty in TextSchema.GetInheritableProperties(typeof(FlowDocument)))
				{
					this.TransferFormattingProperty(dependencyProperty, base.GetValue(dependencyProperty));
				}
			}
			foreach (DependencyProperty dependencyProperty2 in TextSchema.BehavioralProperties)
			{
				this.TransferBehavioralProperty(dependencyProperty2, base.GetValue(dependencyProperty2));
			}
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x00177BA4 File Offset: 0x00175DA4
		private static void OnFormattingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			if (richTextBox._implicitDocument)
			{
				richTextBox.TransferFormattingProperty(e.Property, e.NewValue);
			}
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x00177BD4 File Offset: 0x00175DD4
		private void TransferFormattingProperty(DependencyProperty property, object inheritedValue)
		{
			Invariant.Assert(this._implicitDocument, "We only supposed to do this for implicit documents");
			object value = this._document.GetValue(property);
			if (!TextSchema.ValuesAreEqual(inheritedValue, value))
			{
				this._document.ClearValue(property);
				value = this._document.GetValue(property);
				if (!TextSchema.ValuesAreEqual(inheritedValue, value))
				{
					this._document.SetValue(property, inheritedValue);
				}
			}
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x00177C38 File Offset: 0x00175E38
		private static void OnBehavioralPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			richTextBox.TransferBehavioralProperty(e.Property, e.NewValue);
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x00177C60 File Offset: 0x00175E60
		private void TransferBehavioralProperty(DependencyProperty property, object inheritedValue)
		{
			this._document.SetValue(property, inheritedValue);
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x00177C70 File Offset: 0x00175E70
		private void OnPageSizeChangedHandler(object sender, EventArgs e)
		{
			if (base.RenderScope == null)
			{
				return;
			}
			if (this.Document != null)
			{
				this.Document.TextWrapping = TextWrapping.Wrap;
			}
			base.RenderScope.ClearValue(FrameworkElement.WidthProperty);
			base.RenderScope.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
			if (base.RenderScope.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				base.RenderScope.HorizontalAlignment = HorizontalAlignment.Stretch;
			}
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x00177CD4 File Offset: 0x00175ED4
		private static void OnIsDocumentEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RichTextBox richTextBox = (RichTextBox)d;
			if (richTextBox.Document != null)
			{
				richTextBox.Document.CoerceValue(UIElement.IsEnabledProperty);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RichTextBox.IsDocumentEnabled" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RichTextBox.IsDocumentEnabled" /> dependency property.</returns>
		// Token: 0x04002DB0 RID: 11696
		public static readonly DependencyProperty IsDocumentEnabledProperty = DependencyProperty.Register("IsDocumentEnabled", typeof(bool), typeof(RichTextBox), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(RichTextBox.OnIsDocumentEnabledChanged)));

		// Token: 0x04002DB1 RID: 11697
		private FlowDocument _document;

		// Token: 0x04002DB2 RID: 11698
		private bool _implicitDocument;

		// Token: 0x04002DB3 RID: 11699
		private static DependencyObjectType _dType;
	}
}
