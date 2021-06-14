using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls
{
	/// <summary>Represents a selection control with a drop-down list that can be shown or hidden by clicking the arrow on the control. </summary>
	// Token: 0x02000485 RID: 1157
	[Localizability(LocalizationCategory.ComboBox)]
	[TemplatePart(Name = "PART_EditableTextBox", Type = typeof(TextBox))]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ComboBoxItem))]
	public class ComboBox : Selector
	{
		// Token: 0x0600434C RID: 17228 RVA: 0x00133BA0 File Offset: 0x00131DA0
		static ComboBox()
		{
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ComboBox.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(typeof(ComboBox)));
			ComboBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ComboBox));
			ItemsControl.IsTextSearchEnabledProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.LostMouseCaptureEvent, new MouseEventHandler(ComboBox.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseDownEvent, new MouseButtonEventHandler(ComboBox.OnMouseButtonDown), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseMoveEvent, new MouseEventHandler(ComboBox.OnMouseMove));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.PreviewMouseDownEvent, new MouseButtonEventHandler(ComboBox.OnPreviewMouseButtonDown));
			EventManager.RegisterClassHandler(typeof(ComboBox), Mouse.MouseWheelEvent, new MouseWheelEventHandler(ComboBox.OnMouseWheel), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), UIElement.GotFocusEvent, new RoutedEventHandler(ComboBox.OnGotFocus));
			EventManager.RegisterClassHandler(typeof(ComboBox), ContextMenuService.ContextMenuOpeningEvent, new ContextMenuEventHandler(ComboBox.OnContextMenuOpen), true);
			EventManager.RegisterClassHandler(typeof(ComboBox), ContextMenuService.ContextMenuClosingEvent, new ContextMenuEventHandler(ComboBox.OnContextMenuClose), true);
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ComboBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ComboBox), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.ComboBox);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ComboBox" /> class. </summary>
		// Token: 0x0600434D RID: 17229 RVA: 0x00134085 File Offset: 0x00132285
		public ComboBox()
		{
			this.Initialize();
		}

		/// <summary>Gets or sets the maximum height for a combo box drop-down. </summary>
		/// <returns>A double that represents the height that is retrieved or the height to set. The default value as defined to the property system is a calculated value based on taking a one-third fraction of the system max screen height parameters, but this default is potentially overridden by various control templates.</returns>
		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x0600434E RID: 17230 RVA: 0x0013409F File Offset: 0x0013229F
		// (set) Token: 0x0600434F RID: 17231 RVA: 0x001340B1 File Offset: 0x001322B1
		[Bindable(true)]
		[Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double MaxDropDownHeight
		{
			get
			{
				return (double)base.GetValue(ComboBox.MaxDropDownHeightProperty);
			}
			set
			{
				base.SetValue(ComboBox.MaxDropDownHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the drop-down for a combo box is currently open.  </summary>
		/// <returns>
		///     <see langword="true" /> if the drop-down is open; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06004350 RID: 17232 RVA: 0x001340C4 File Offset: 0x001322C4
		// (set) Token: 0x06004351 RID: 17233 RVA: 0x001340D6 File Offset: 0x001322D6
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsDropDownOpenProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsDropDownOpenProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ComboBox" /> keeps the user's input or replaces the input with a matching item.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> keeps the user's input; <see langword="false" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> replaces the input with a matching item The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x001340E9 File Offset: 0x001322E9
		// (set) Token: 0x06004353 RID: 17235 RVA: 0x001340FB File Offset: 0x001322FB
		public bool ShouldPreserveUserEnteredPrefix
		{
			get
			{
				return (bool)base.GetValue(ComboBox.ShouldPreserveUserEnteredPrefixProperty);
			}
			set
			{
				base.SetValue(ComboBox.ShouldPreserveUserEnteredPrefixProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x00134110 File Offset: 0x00132310
		private static object CoerceIsDropDownOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				ComboBox comboBox = (ComboBox)d;
				if (!comboBox.IsLoaded)
				{
					comboBox.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00134144 File Offset: 0x00132344
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			ComboBox comboBox = (ComboBox)d;
			if (!comboBox.IsDropDownOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x00134167 File Offset: 0x00132367
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x0013417B File Offset: 0x0013237B
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(ComboBox.IsDropDownOpenProperty);
				return null;
			}), null);
		}

		/// <summary>Reports when a combo box's popup opens. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ComboBox.DropDownOpened" /> event.</param>
		// Token: 0x06004358 RID: 17240 RVA: 0x00134197 File Offset: 0x00132397
		protected virtual void OnDropDownOpened(EventArgs e)
		{
			base.RaiseClrEvent(ComboBox.DropDownOpenedKey, e);
		}

		/// <summary>Reports when a combo box's popup closes. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ComboBox.DropDownClosed" /> event.</param>
		// Token: 0x06004359 RID: 17241 RVA: 0x001341A5 File Offset: 0x001323A5
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			base.RaiseClrEvent(ComboBox.DropDownClosedKey, e);
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x001341B4 File Offset: 0x001323B4
		private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)d;
			comboBox.HasMouseEnteredItemsHost = false;
			bool flag = (bool)e.NewValue;
			bool oldValue = !flag;
			ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.FromElement(comboBox) as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.RaiseExpandCollapseAutomationEvent(oldValue, flag);
			}
			if (flag)
			{
				Mouse.Capture(comboBox, CaptureMode.SubTree);
				if (comboBox.IsEditable && comboBox.EditableTextBoxSite != null)
				{
					comboBox.EditableTextBoxSite.SelectAll();
				}
				if (comboBox._clonedElement != null && VisualTreeHelper.GetParent(comboBox._clonedElement) == null)
				{
					comboBox.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(delegate(object arg)
					{
						ComboBox comboBox2 = (ComboBox)arg;
						comboBox2.UpdateSelectionBoxItem();
						if (comboBox2._clonedElement != null)
						{
							comboBox2._clonedElement.CoerceValue(FrameworkElement.FlowDirectionProperty);
						}
						return null;
					}), comboBox);
				}
				comboBox.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object arg)
				{
					ComboBox comboBox2 = (ComboBox)arg;
					if (comboBox2.IsItemsHostVisible)
					{
						comboBox2.NavigateToItem(comboBox2.InternalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, true);
					}
					return null;
				}), comboBox);
				comboBox.OnDropDownOpened(EventArgs.Empty);
			}
			else
			{
				if (comboBox.IsKeyboardFocusWithin)
				{
					if (comboBox.IsEditable)
					{
						if (comboBox.EditableTextBoxSite != null && !comboBox.EditableTextBoxSite.IsKeyboardFocusWithin)
						{
							comboBox.Focus();
						}
					}
					else
					{
						comboBox.Focus();
					}
				}
				comboBox.HighlightedInfo = null;
				if (comboBox.HasCapture)
				{
					Mouse.Capture(null);
				}
				if (comboBox._dropDownPopup == null)
				{
					comboBox.OnDropDownClosed(EventArgs.Empty);
				}
			}
			comboBox.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
			comboBox.CoerceValue(ToolTipService.IsEnabledProperty);
			comboBox.UpdateVisualState();
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x00134318 File Offset: 0x00132518
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnDropDownClosed(EventArgs.Empty);
		}

		/// <summary>Gets or sets a value that enables or disables editing of the text in text box of the <see cref="T:System.Windows.Controls.ComboBox" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> can be edited; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x0600435C RID: 17244 RVA: 0x00134325 File Offset: 0x00132525
		// (set) Token: 0x0600435D RID: 17245 RVA: 0x00134337 File Offset: 0x00132537
		public bool IsEditable
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsEditableProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsEditableProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0013434C File Offset: 0x0013254C
		private static void OnIsEditableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = d as ComboBox;
			comboBox.Update();
			comboBox.UpdateVisualState();
		}

		/// <summary>Gets or sets the text of the currently selected item. </summary>
		/// <returns>The string of the currently selected item. The default is an empty string ("").</returns>
		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x0600435F RID: 17247 RVA: 0x0013436C File Offset: 0x0013256C
		// (set) Token: 0x06004360 RID: 17248 RVA: 0x0013437E File Offset: 0x0013257E
		public string Text
		{
			get
			{
				return (string)base.GetValue(ComboBox.TextProperty);
			}
			set
			{
				base.SetValue(ComboBox.TextProperty, value);
			}
		}

		/// <summary>Gets or sets a value that enables selection-only mode, in which the contents of the combo box are selectable but not editable. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06004361 RID: 17249 RVA: 0x0013438C File Offset: 0x0013258C
		// (set) Token: 0x06004362 RID: 17250 RVA: 0x0013439E File Offset: 0x0013259E
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(ComboBox.IsReadOnlyProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets the item that is displayed in the selection box. </summary>
		/// <returns>The selected item.</returns>
		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06004363 RID: 17251 RVA: 0x001343B1 File Offset: 0x001325B1
		// (set) Token: 0x06004364 RID: 17252 RVA: 0x001343BE File Offset: 0x001325BE
		public object SelectionBoxItem
		{
			get
			{
				return base.GetValue(ComboBox.SelectionBoxItemProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemPropertyKey, value);
			}
		}

		/// <summary>Gets the item template of the selection box content. </summary>
		/// <returns>An item template.</returns>
		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06004365 RID: 17253 RVA: 0x001343CC File Offset: 0x001325CC
		// (set) Token: 0x06004366 RID: 17254 RVA: 0x001343DE File Offset: 0x001325DE
		public DataTemplate SelectionBoxItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ComboBox.SelectionBoxItemTemplateProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemTemplatePropertyKey, value);
			}
		}

		/// <summary>Gets a composite string that specifies how to format the selected item in the selection box if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the selected item in the selection box if it is displayed as a string.</returns>
		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06004367 RID: 17255 RVA: 0x001343EC File Offset: 0x001325EC
		// (set) Token: 0x06004368 RID: 17256 RVA: 0x001343FE File Offset: 0x001325FE
		public string SelectionBoxItemStringFormat
		{
			get
			{
				return (string)base.GetValue(ComboBox.SelectionBoxItemStringFormatProperty);
			}
			private set
			{
				base.SetValue(ComboBox.SelectionBoxItemStringFormatPropertyKey, value);
			}
		}

		/// <summary>Gets or sets whether a <see cref="T:System.Windows.Controls.ComboBox" /> that is open and displays a drop-down control will remain open when a user clicks the <see cref="T:System.Windows.Controls.TextBox" />. </summary>
		/// <returns>
		///     <see langword="true" /> to keep the drop-down control open when the user clicks on the text area to start editing; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06004369 RID: 17257 RVA: 0x0013440C File Offset: 0x0013260C
		// (set) Token: 0x0600436A RID: 17258 RVA: 0x0013441E File Offset: 0x0013261E
		public bool StaysOpenOnEdit
		{
			get
			{
				return (bool)base.GetValue(ComboBox.StaysOpenOnEditProperty);
			}
			set
			{
				base.SetValue(ComboBox.StaysOpenOnEditProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets whether the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItem" /> is highlighted.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItem" /> is highlighted; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x0600436B RID: 17259 RVA: 0x00134431 File Offset: 0x00132631
		public bool IsSelectionBoxHighlighted
		{
			get
			{
				return (bool)base.GetValue(ComboBox.IsSelectionBoxHighlightedProperty);
			}
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x00134444 File Offset: 0x00132644
		private static object CoerceIsSelectionBoxHighlighted(object o, object value)
		{
			ComboBox comboBox = (ComboBox)o;
			ComboBoxItem highlightedElement;
			return (!comboBox.IsDropDownOpen && comboBox.IsKeyboardFocusWithin) || ((highlightedElement = comboBox.HighlightedElement) != null && highlightedElement.Content == comboBox._clonedElement);
		}

		/// <summary>Occurs when the drop-down list of the combo box opens. </summary>
		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x0600436D RID: 17261 RVA: 0x0013448B File Offset: 0x0013268B
		// (remove) Token: 0x0600436E RID: 17262 RVA: 0x00134499 File Offset: 0x00132699
		public event EventHandler DropDownOpened
		{
			add
			{
				base.EventHandlersStoreAdd(ComboBox.DropDownOpenedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(ComboBox.DropDownOpenedKey, value);
			}
		}

		/// <summary>Occurs when the drop-down list of the combo box closes. </summary>
		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x0600436F RID: 17263 RVA: 0x001344A7 File Offset: 0x001326A7
		// (remove) Token: 0x06004370 RID: 17264 RVA: 0x001344B5 File Offset: 0x001326B5
		public event EventHandler DropDownClosed
		{
			add
			{
				base.EventHandlersStoreAdd(ComboBox.DropDownClosedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(ComboBox.DropDownClosedKey, value);
			}
		}

		/// <summary>Responds to a <see cref="T:System.Windows.Controls.ComboBox" /> selection change by raising a <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" />. </param>
		// Token: 0x06004371 RID: 17265 RVA: 0x001344C4 File Offset: 0x001326C4
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			this.SelectedItemUpdated();
			if (this.IsDropDownOpen)
			{
				ItemsControl.ItemInfo internalSelectedInfo = base.InternalSelectedInfo;
				if (internalSelectedInfo != null)
				{
					base.NavigateToItem(internalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, false);
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as ComboBoxAutomationPeer;
				if (comboBoxAutomationPeer != null)
				{
					comboBoxAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x0013453C File Offset: 0x0013273C
		internal void SelectedItemUpdated()
		{
			try
			{
				this.UpdatingSelectedItem = true;
				if (!this.UpdatingText)
				{
					string primaryTextFromItem = TextSearch.GetPrimaryTextFromItem(this, base.InternalSelectedItem);
					if (this.Text != primaryTextFromItem)
					{
						base.SetCurrentValueInternal(ComboBox.TextProperty, primaryTextFromItem);
					}
				}
				this.Update();
			}
			finally
			{
				this.UpdatingSelectedItem = false;
			}
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x001345A0 File Offset: 0x001327A0
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)d;
			ComboBoxAutomationPeer comboBoxAutomationPeer = UIElementAutomationPeer.FromElement(comboBox) as ComboBoxAutomationPeer;
			if (comboBoxAutomationPeer != null)
			{
				comboBoxAutomationPeer.RaiseValuePropertyChangedEvent((string)e.OldValue, (string)e.NewValue);
			}
			comboBox.TextUpdated((string)e.NewValue, false);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x001345F4 File Offset: 0x001327F4
		private void OnEditableTextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!this.IsEditable)
			{
				return;
			}
			this.TextUpdated(this.EditableTextBoxSite.Text, true);
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x00134611 File Offset: 0x00132811
		private void OnEditableTextBoxSelectionChanged(object sender, RoutedEventArgs e)
		{
			if (!Helper.IsComposing(this.EditableTextBoxSite))
			{
				this._textBoxSelectionStart = this.EditableTextBoxSite.SelectionStart;
			}
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x00134634 File Offset: 0x00132834
		private void OnEditableTextBoxPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (this.IsWaitingForTextComposition && e.TextComposition.Source == this.EditableTextBoxSite && e.TextComposition.Stage == TextCompositionStage.Done)
			{
				this.IsWaitingForTextComposition = false;
				this.TextUpdated(this.EditableTextBoxSite.Text, true);
				this.EditableTextBoxSite.RaiseCourtesyTextChangedEvent();
			}
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x00134690 File Offset: 0x00132890
		private void TextUpdated(string newText, bool textBoxUpdated)
		{
			if (!this.UpdatingText && !this.UpdatingSelectedItem)
			{
				if (Helper.IsComposing(this.EditableTextBoxSite))
				{
					this.IsWaitingForTextComposition = true;
					return;
				}
				try
				{
					this.UpdatingText = true;
					if (base.IsTextSearchEnabled)
					{
						if (this._updateTextBoxOperation != null)
						{
							this._updateTextBoxOperation.Abort();
							this._updateTextBoxOperation = null;
						}
						MatchedTextInfo matchedTextInfo = TextSearch.FindMatchingPrefix(this, newText);
						int num = matchedTextInfo.MatchedItemIndex;
						if (num >= 0)
						{
							if (textBoxUpdated)
							{
								int selectionStart = this.EditableTextBoxSite.SelectionStart;
								if (selectionStart == newText.Length && selectionStart > this._textBoxSelectionStart)
								{
									string text = matchedTextInfo.MatchedText;
									if (this.ShouldPreserveUserEnteredPrefix)
									{
										text = newText + text.Substring(matchedTextInfo.MatchedPrefixLength);
									}
									UndoManager undoManager = this.EditableTextBoxSite.TextContainer.UndoManager;
									if (undoManager != null && undoManager.OpenedUnit != null && undoManager.OpenedUnit.GetType() != typeof(TextParentUndoUnit))
									{
										this._updateTextBoxOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.UpdateTextBoxCallback), new object[]
										{
											text,
											matchedTextInfo
										});
									}
									else
									{
										this.UpdateTextBox(text, matchedTextInfo);
									}
									newText = text;
								}
							}
							else
							{
								string matchedText = matchedTextInfo.MatchedText;
								if (!string.Equals(newText, matchedText, StringComparison.CurrentCulture))
								{
									num = -1;
								}
							}
						}
						if (num != base.SelectedIndex)
						{
							base.SetCurrentValueInternal(Selector.SelectedIndexProperty, num);
						}
					}
					if (textBoxUpdated)
					{
						base.SetCurrentValueInternal(ComboBox.TextProperty, newText);
					}
					else if (this.EditableTextBoxSite != null)
					{
						this.EditableTextBoxSite.Text = newText;
					}
				}
				finally
				{
					this.UpdatingText = false;
				}
			}
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0013484C File Offset: 0x00132A4C
		private object UpdateTextBoxCallback(object arg)
		{
			this._updateTextBoxOperation = null;
			object[] array = (object[])arg;
			string matchedText = (string)array[0];
			MatchedTextInfo matchedTextInfo = (MatchedTextInfo)array[1];
			try
			{
				this.UpdatingText = true;
				this.UpdateTextBox(matchedText, matchedTextInfo);
			}
			finally
			{
				this.UpdatingText = false;
			}
			return null;
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x001348A4 File Offset: 0x00132AA4
		private void UpdateTextBox(string matchedText, MatchedTextInfo matchedTextInfo)
		{
			this.EditableTextBoxSite.Text = matchedText;
			this.EditableTextBoxSite.SelectionStart = matchedText.Length - matchedTextInfo.TextExcludingPrefixLength;
			this.EditableTextBoxSite.SelectionLength = matchedTextInfo.TextExcludingPrefixLength;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x001348DB File Offset: 0x00132ADB
		private void Update()
		{
			if (this.IsEditable)
			{
				this.UpdateEditableTextBox();
				return;
			}
			this.UpdateSelectionBoxItem();
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x001348F4 File Offset: 0x00132AF4
		private void UpdateEditableTextBox()
		{
			if (!this.UpdatingText)
			{
				try
				{
					this.UpdatingText = true;
					string text = this.Text;
					if (this.EditableTextBoxSite != null && this.EditableTextBoxSite.Text != text)
					{
						this.EditableTextBoxSite.Text = text;
						this.EditableTextBoxSite.SelectAll();
					}
				}
				finally
				{
					this.UpdatingText = false;
				}
			}
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x00134964 File Offset: 0x00132B64
		private void UpdateSelectionBoxItem()
		{
			object obj = base.InternalSelectedItem;
			DataTemplate dataTemplate = base.ItemTemplate;
			string text = base.ItemStringFormat;
			ContentControl contentControl = obj as ContentControl;
			if (contentControl != null)
			{
				obj = contentControl.Content;
				dataTemplate = contentControl.ContentTemplate;
				text = contentControl.ContentStringFormat;
			}
			if (this._clonedElement != null)
			{
				this._clonedElement.LayoutUpdated -= this.CloneLayoutUpdated;
				this._clonedElement = null;
			}
			if (dataTemplate == null && base.ItemTemplateSelector == null && text == null)
			{
				DependencyObject dependencyObject = obj as DependencyObject;
				if (dependencyObject != null)
				{
					this._clonedElement = (dependencyObject as UIElement);
					if (this._clonedElement != null)
					{
						VisualBrush visualBrush = new VisualBrush(this._clonedElement);
						visualBrush.Stretch = Stretch.None;
						visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
						visualBrush.Viewbox = new Rect(this._clonedElement.RenderSize);
						visualBrush.ViewportUnits = BrushMappingMode.Absolute;
						visualBrush.Viewport = new Rect(this._clonedElement.RenderSize);
						DependencyObject parent = VisualTreeHelper.GetParent(this._clonedElement);
						FlowDirection flowDirection = (parent == null) ? FlowDirection.LeftToRight : ((FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty));
						if (base.FlowDirection != flowDirection)
						{
							visualBrush.Transform = new MatrixTransform(new Matrix(-1.0, 0.0, 0.0, 1.0, this._clonedElement.RenderSize.Width, 0.0));
						}
						Rectangle rectangle = new Rectangle();
						rectangle.Fill = visualBrush;
						rectangle.Width = this._clonedElement.RenderSize.Width;
						rectangle.Height = this._clonedElement.RenderSize.Height;
						this._clonedElement.LayoutUpdated += this.CloneLayoutUpdated;
						obj = rectangle;
						dataTemplate = null;
					}
					else
					{
						obj = ComboBox.ExtractString(dependencyObject);
						dataTemplate = ContentPresenter.StringContentTemplate;
					}
				}
			}
			if (obj == null)
			{
				obj = string.Empty;
				dataTemplate = ContentPresenter.StringContentTemplate;
			}
			this.SelectionBoxItem = obj;
			this.SelectionBoxItemTemplate = dataTemplate;
			this.SelectionBoxItemStringFormat = text;
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x00134B78 File Offset: 0x00132D78
		private void CloneLayoutUpdated(object sender, EventArgs e)
		{
			Rectangle rectangle = (Rectangle)this.SelectionBoxItem;
			rectangle.Width = this._clonedElement.RenderSize.Width;
			rectangle.Height = this._clonedElement.RenderSize.Height;
			VisualBrush visualBrush = (VisualBrush)rectangle.Fill;
			visualBrush.Viewbox = new Rect(this._clonedElement.RenderSize);
			visualBrush.Viewport = new Rect(this._clonedElement.RenderSize);
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00134BFC File Offset: 0x00132DFC
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (!Selector.GetIsSelectionActive(this))
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			else if (this.IsDropDownOpen)
			{
				VisualStateManager.GoToState(this, "FocusedDropDown", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			if (this.IsEditable)
			{
				VisualStateManager.GoToState(this, "Editable", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Uneditable", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Gets a value that indicates whether a combo box supports scrolling.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> supports scrolling; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x0600437F RID: 17279 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x06004380 RID: 17280 RVA: 0x00134CAA File Offset: 0x00132EAA
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item is Separator)
			{
				Separator.PrepareContainer(element as Control);
			}
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x00134CC7 File Offset: 0x00132EC7
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfo(e, this._highlightedInfo);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x00134CE0 File Offset: 0x00132EE0
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (!e.Handled && comboBox.IsEditable && comboBox.EditableTextBoxSite != null)
			{
				if (e.OriginalSource == comboBox)
				{
					comboBox.EditableTextBoxSite.Focus();
					e.Handled = true;
					return;
				}
				if (e.OriginalSource == comboBox.EditableTextBoxSite)
				{
					comboBox.EditableTextBoxSite.SelectAll();
				}
			}
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00134D44 File Offset: 0x00132F44
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = false;
			if (!this.IsEditable)
			{
				result = base.FocusItem(info, itemNavigateArgs);
			}
			this.HighlightedInfo = ((info.Container is ComboBoxItem) ? info : null);
			if ((this.IsEditable || !this.IsDropDownOpen) && itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				int num = info.Index;
				if (num < 0)
				{
					num = base.Items.IndexOf(info.Item);
				}
				base.SetCurrentValueInternal(Selector.SelectedIndexProperty, num);
				result = true;
			}
			return result;
		}

		/// <summary>Reports that the <see cref="P:System.Windows.ContentElement.IsKeyboardFocusWithin" /> property changed. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> event.</param>
		// Token: 0x06004384 RID: 17284 RVA: 0x00134DCC File Offset: 0x00132FCC
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (this.IsDropDownOpen && !base.IsKeyboardFocusWithin)
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject == null || (!this.IsContextMenuOpen && ItemsControl.ItemsControlFromItemContainer(dependencyObject) != this))
				{
					this.Close();
				}
			}
			base.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x00134E20 File Offset: 0x00133020
		private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsKeyboardFocusWithin)
			{
				if (!comboBox.IsDropDownOpen)
				{
					if (e.Delta < 0)
					{
						comboBox.SelectNext();
					}
					else
					{
						comboBox.SelectPrev();
					}
				}
				e.Handled = true;
				return;
			}
			if (comboBox.IsDropDownOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x00134E72 File Offset: 0x00133072
		private static void OnContextMenuOpen(object sender, ContextMenuEventArgs e)
		{
			((ComboBox)sender).IsContextMenuOpen = true;
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x00134E80 File Offset: 0x00133080
		private static void OnContextMenuClose(object sender, ContextMenuEventArgs e)
		{
			((ComboBox)sender).IsContextMenuOpen = false;
		}

		/// <summary>Called when the <see cref="P:System.Windows.UIElement.IsMouseCaptured" /> property changes. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.IsMouseCapturedChanged" /> event.</param>
		// Token: 0x06004388 RID: 17288 RVA: 0x00134E90 File Offset: 0x00133090
		protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (base.IsMouseCaptured)
			{
				if (this._autoScrollTimer == null)
				{
					this._autoScrollTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
					this._autoScrollTimer.Interval = ItemsControl.AutoScrollTimeout;
					this._autoScrollTimer.Tick += this.OnAutoScrollTimeout;
					this._autoScrollTimer.Start();
				}
			}
			else if (this._autoScrollTimer != null)
			{
				this._autoScrollTimer.Stop();
				this._autoScrollTimer = null;
			}
			base.OnIsMouseCapturedChanged(e);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.ComboBox" /> has focus. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ComboBox" /> has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06004389 RID: 17289 RVA: 0x00134F0E File Offset: 0x0013310E
		protected internal override bool HasEffectiveKeyboardFocus
		{
			get
			{
				if (this.IsEditable && this.EditableTextBoxSite != null)
				{
					return this.EditableTextBoxSite.HasEffectiveKeyboardFocus;
				}
				return base.HasEffectiveKeyboardFocus;
			}
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00002137 File Offset: 0x00000337
		internal void NotifyComboBoxItemMouseDown(ComboBoxItem comboBoxItem)
		{
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x00134F34 File Offset: 0x00133134
		internal void NotifyComboBoxItemMouseUp(ComboBoxItem comboBoxItem)
		{
			object obj = base.ItemContainerGenerator.ItemFromContainer(comboBoxItem);
			if (obj != null)
			{
				base.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, comboBoxItem, -1), true);
			}
			this.Close();
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x00134F6C File Offset: 0x0013316C
		internal void NotifyComboBoxItemEnter(ComboBoxItem item)
		{
			if (this.IsDropDownOpen && Mouse.Captured == this && base.DidMouseMove())
			{
				this.HighlightedInfo = base.ItemInfoFromContainer(item);
				if (!this.IsEditable && !item.IsKeyboardFocusWithin)
				{
					item.Focus();
				}
			}
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own ItemContainer. </summary>
		/// <param name="item">Specified item.</param>
		/// <returns>
		///     <see langword="true" /> if the item is its own ItemContainer; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600438D RID: 17293 RVA: 0x00134FAA File Offset: 0x001331AA
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ComboBoxItem;
		}

		/// <summary>Creates or identifies the element used to display the specified item.</summary>
		/// <returns>The element used to display the specified item.</returns>
		// Token: 0x0600438E RID: 17294 RVA: 0x00134FB5 File Offset: 0x001331B5
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ComboBoxItem();
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00134FBC File Offset: 0x001331BC
		private void Initialize()
		{
			base.CanSelectMultiple = false;
		}

		/// <summary>Invoked when a <see cref="E:System.Windows.Input.Keyboard.PreviewKeyDown" /> attached routed event occurs.</summary>
		/// <param name="e">Event data.</param>
		// Token: 0x06004390 RID: 17296 RVA: 0x00134FC5 File Offset: 0x001331C5
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (this.IsEditable && e.OriginalSource == this.EditableTextBoxSite)
			{
				this.KeyDownHandler(e);
			}
		}

		/// <summary>Invoked when a <see cref="E:System.Windows.Input.Keyboard.KeyDown" /> attached routed event occurs.</summary>
		/// <param name="e">Event data.</param>
		// Token: 0x06004391 RID: 17297 RVA: 0x00134FE4 File Offset: 0x001331E4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			this.KeyDownHandler(e);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00134FF0 File Offset: 0x001331F0
		private void KeyDownHandler(KeyEventArgs e)
		{
			bool flag = false;
			Key key = e.Key;
			if (key == Key.System)
			{
				key = e.SystemKey;
			}
			bool flag2 = base.FlowDirection == FlowDirection.RightToLeft;
			if (key <= Key.Down)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Escape:
						if (this.IsDropDownOpen)
						{
							this.KeyboardCloseDropDown(false);
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Prior:
						if (this.IsItemsHostVisible)
						{
							base.NavigateByPage(this.HighlightedInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Next:
						if (this.IsItemsHostVisible)
						{
							base.NavigateByPage(this.HighlightedInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.End:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else
							{
								this.SelectLast();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Home:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else
							{
								this.SelectFirst();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Left:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Left, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else if (!flag2)
							{
								this.SelectPrev();
							}
							else
							{
								this.SelectNext();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Up:
						flag = true;
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
						{
							this.KeyboardToggleDropDown(true);
							goto IL_349;
						}
						if (this.IsItemsHostVisible)
						{
							base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							goto IL_349;
						}
						this.SelectPrev();
						goto IL_349;
					case Key.Right:
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) != ModifierKeys.Alt && !this.IsEditable)
						{
							if (this.IsItemsHostVisible)
							{
								base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Right, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							}
							else if (!flag2)
							{
								this.SelectNext();
							}
							else
							{
								this.SelectPrev();
							}
							flag = true;
							goto IL_349;
						}
						goto IL_349;
					case Key.Down:
						flag = true;
						if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
						{
							this.KeyboardToggleDropDown(true);
							goto IL_349;
						}
						if (this.IsItemsHostVisible)
						{
							base.NavigateByLine(this.HighlightedInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							goto IL_349;
						}
						this.SelectNext();
						goto IL_349;
					}
				}
				else
				{
					if (this.IsDropDownOpen)
					{
						this.KeyboardCloseDropDown(true);
						flag = true;
						goto IL_349;
					}
					goto IL_349;
				}
			}
			else if (key != Key.F4)
			{
				if (key == Key.Oem5)
				{
					if (Keyboard.Modifiers == ModifierKeys.Control)
					{
						base.NavigateToItem(base.InternalSelectedInfo, ItemsControl.ItemNavigateArgs.Empty, false);
						flag = true;
						goto IL_349;
					}
					goto IL_349;
				}
			}
			else
			{
				if ((e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.None)
				{
					this.KeyboardToggleDropDown(true);
					flag = true;
					goto IL_349;
				}
				goto IL_349;
			}
			flag = false;
			IL_349:
			if (flag)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00135350 File Offset: 0x00133550
		private void SelectPrev()
		{
			if (!base.Items.IsEmpty)
			{
				int internalSelectedIndex = base.InternalSelectedIndex;
				if (internalSelectedIndex > 0)
				{
					this.SelectItemHelper(internalSelectedIndex - 1, -1, -1);
				}
			}
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00135380 File Offset: 0x00133580
		private void SelectNext()
		{
			int count = base.Items.Count;
			if (count > 0)
			{
				int internalSelectedIndex = base.InternalSelectedIndex;
				if (internalSelectedIndex < count - 1)
				{
					this.SelectItemHelper(internalSelectedIndex + 1, 1, count);
				}
			}
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x001353B5 File Offset: 0x001335B5
		private void SelectFirst()
		{
			this.SelectItemHelper(0, 1, base.Items.Count);
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x001353CA File Offset: 0x001335CA
		private void SelectLast()
		{
			this.SelectItemHelper(base.Items.Count - 1, -1, -1);
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x001353E4 File Offset: 0x001335E4
		private void SelectItemHelper(int startIndex, int increment, int stopIndex)
		{
			for (int num = startIndex; num != stopIndex; num += increment)
			{
				object obj = base.Items[num];
				DependencyObject dependencyObject = base.ItemContainerGenerator.ContainerFromIndex(num);
				if (this.IsSelectableHelper(obj) && this.IsSelectableHelper(dependencyObject))
				{
					base.SelectionChange.SelectJustThisItem(base.NewItemInfo(obj, dependencyObject, num), true);
					return;
				}
			}
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x00135440 File Offset: 0x00133640
		private bool IsSelectableHelper(object o)
		{
			DependencyObject dependencyObject = o as DependencyObject;
			return dependencyObject == null || (bool)dependencyObject.GetValue(UIElement.IsEnabledProperty);
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0013546C File Offset: 0x0013366C
		private static string ExtractString(DependencyObject d)
		{
			string text = string.Empty;
			TextBlock textBlock;
			Visual reference;
			TextElement textElement;
			if ((textBlock = (d as TextBlock)) != null)
			{
				text = textBlock.Text;
			}
			else if ((reference = (d as Visual)) != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
				for (int i = 0; i < childrenCount; i++)
				{
					text += ComboBox.ExtractString(VisualTreeHelper.GetChild(reference, i));
				}
			}
			else if ((textElement = (d as TextElement)) != null)
			{
				text += TextRangeBase.GetTextInternal(textElement.ContentStart, textElement.ContentEnd);
			}
			return text;
		}

		/// <summary>Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> is called.</summary>
		// Token: 0x0600439A RID: 17306 RVA: 0x001354F0 File Offset: 0x001336F0
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._dropDownPopup != null)
			{
				this._dropDownPopup.Closed -= this.OnPopupClosed;
			}
			this.EditableTextBoxSite = (base.GetTemplateChild("PART_EditableTextBox") as TextBox);
			this._dropDownPopup = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this.EditableTextBoxSite != null)
			{
				this.EditableTextBoxSite.TextChanged += this.OnEditableTextBoxTextChanged;
				this.EditableTextBoxSite.SelectionChanged += this.OnEditableTextBoxSelectionChanged;
				this.EditableTextBoxSite.PreviewTextInput += this.OnEditableTextBoxPreviewTextInput;
			}
			if (this._dropDownPopup != null)
			{
				this._dropDownPopup.Closed += this.OnPopupClosed;
			}
			this.Update();
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x001355C0 File Offset: 0x001337C0
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
			if (this.EditableTextBoxSite != null)
			{
				this.EditableTextBoxSite.TextChanged -= this.OnEditableTextBoxTextChanged;
				this.EditableTextBoxSite.SelectionChanged -= this.OnEditableTextBoxSelectionChanged;
				this.EditableTextBoxSite.PreviewTextInput -= this.OnEditableTextBoxPreviewTextInput;
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x00135624 File Offset: 0x00133824
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (Mouse.Captured != comboBox)
			{
				if (e.OriginalSource == comboBox)
				{
					if (Mouse.Captured == null || !MenuBase.IsDescendant(comboBox, Mouse.Captured as DependencyObject))
					{
						comboBox.Close();
						return;
					}
				}
				else if (MenuBase.IsDescendant(comboBox, e.OriginalSource as DependencyObject))
				{
					if (comboBox.IsDropDownOpen && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero)
					{
						Mouse.Capture(comboBox, CaptureMode.SubTree);
						e.Handled = true;
						return;
					}
				}
				else
				{
					comboBox.Close();
				}
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x001356B4 File Offset: 0x001338B4
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (!comboBox.IsContextMenuOpen && !comboBox.IsKeyboardFocusWithin)
			{
				comboBox.Focus();
			}
			e.Handled = true;
			if (Mouse.Captured == comboBox && e.OriginalSource == comboBox)
			{
				comboBox.Close();
			}
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x00135700 File Offset: 0x00133900
		private static void OnPreviewMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsEditable)
			{
				Visual visual = e.OriginalSource as Visual;
				Visual editableTextBoxSite = comboBox.EditableTextBoxSite;
				if (visual != null && editableTextBoxSite != null && editableTextBoxSite.IsAncestorOf(visual))
				{
					if (comboBox.IsDropDownOpen && !comboBox.StaysOpenOnEdit)
					{
						comboBox.Close();
						return;
					}
					if (!comboBox.IsContextMenuOpen && !comboBox.IsKeyboardFocusWithin)
					{
						comboBox.Focus();
						e.Handled = true;
					}
				}
			}
		}

		/// <summary>Called to report that the left mouse button was released. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event.</param>
		// Token: 0x0600439F RID: 17311 RVA: 0x00135773 File Offset: 0x00133973
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (this.HasMouseEnteredItemsHost && !this.IsMouseOverItemsHost && this.IsDropDownOpen)
			{
				this.Close();
				e.Handled = true;
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x001357A4 File Offset: 0x001339A4
		private static void OnMouseMove(object sender, MouseEventArgs e)
		{
			ComboBox comboBox = (ComboBox)sender;
			if (comboBox.IsDropDownOpen)
			{
				bool flag = comboBox.ItemsHost != null && comboBox.ItemsHost.IsMouseOver;
				if (flag && !comboBox.HasMouseEnteredItemsHost)
				{
					comboBox.SetInitialMousePosition();
				}
				comboBox.IsMouseOverItemsHost = flag;
				comboBox.HasMouseEnteredItemsHost = flag;
			}
			if (Mouse.LeftButton == MouseButtonState.Pressed && comboBox.HasMouseEnteredItemsHost && Mouse.Captured == comboBox)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					comboBox.DoAutoScroll(comboBox.HighlightedInfo);
				}
				else
				{
					comboBox.ReleaseMouseCapture();
					comboBox.ResetLastMousePosition();
				}
				e.Handled = true;
			}
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x0013583E File Offset: 0x00133A3E
		private void KeyboardToggleDropDown(bool commitSelection)
		{
			this.KeyboardToggleDropDown(!this.IsDropDownOpen, commitSelection);
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x00135850 File Offset: 0x00133A50
		private void KeyboardCloseDropDown(bool commitSelection)
		{
			this.KeyboardToggleDropDown(false, commitSelection);
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x0013585C File Offset: 0x00133A5C
		private void KeyboardToggleDropDown(bool openDropDown, bool commitSelection)
		{
			ItemsControl.ItemInfo itemInfo = null;
			if (commitSelection)
			{
				itemInfo = this.HighlightedInfo;
			}
			base.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, BooleanBoxes.Box(openDropDown));
			if (!openDropDown && commitSelection && itemInfo != null)
			{
				base.SelectionChange.SelectJustThisItem(itemInfo, true);
			}
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x001358A4 File Offset: 0x00133AA4
		private void CommitSelection()
		{
			ItemsControl.ItemInfo highlightedInfo = this.HighlightedInfo;
			if (highlightedInfo != null)
			{
				base.SelectionChange.SelectJustThisItem(highlightedInfo, true);
			}
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x001358CE File Offset: 0x00133ACE
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed && this.HasMouseEnteredItemsHost)
			{
				base.DoAutoScroll(this.HighlightedInfo);
			}
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x001358EC File Offset: 0x00133AEC
		private void Close()
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(ComboBox.IsDropDownOpenProperty, false);
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ComboBoxAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060043A7 RID: 17319 RVA: 0x00135907 File Offset: 0x00133B07
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ComboBoxAutomationPeer(this);
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x060043A8 RID: 17320 RVA: 0x0013590F File Offset: 0x00133B0F
		// (set) Token: 0x060043A9 RID: 17321 RVA: 0x00135917 File Offset: 0x00133B17
		internal TextBox EditableTextBoxSite
		{
			get
			{
				return this._editableTextBoxSite;
			}
			set
			{
				this._editableTextBoxSite = value;
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x060043AA RID: 17322 RVA: 0x00135920 File Offset: 0x00133B20
		private bool HasCapture
		{
			get
			{
				return Mouse.Captured == this;
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x060043AB RID: 17323 RVA: 0x0013592C File Offset: 0x00133B2C
		private bool IsItemsHostVisible
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				Panel itemsHost = base.ItemsHost;
				if (itemsHost != null)
				{
					HwndSource hwndSource = PresentationSource.CriticalFromVisual(itemsHost) as HwndSource;
					if (hwndSource != null && !hwndSource.IsDisposed && hwndSource.RootVisual != null)
					{
						return hwndSource.RootVisual.IsAncestorOf(itemsHost);
					}
				}
				return false;
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x060043AC RID: 17324 RVA: 0x00135970 File Offset: 0x00133B70
		// (set) Token: 0x060043AD RID: 17325 RVA: 0x00135978 File Offset: 0x00133B78
		private ItemsControl.ItemInfo HighlightedInfo
		{
			get
			{
				return this._highlightedInfo;
			}
			set
			{
				ComboBoxItem comboBoxItem = (this._highlightedInfo != null) ? (this._highlightedInfo.Container as ComboBoxItem) : null;
				if (comboBoxItem != null)
				{
					comboBoxItem.SetIsHighlighted(false);
				}
				this._highlightedInfo = value;
				comboBoxItem = ((this._highlightedInfo != null) ? (this._highlightedInfo.Container as ComboBoxItem) : null);
				if (comboBoxItem != null)
				{
					comboBoxItem.SetIsHighlighted(true);
				}
				base.CoerceValue(ComboBox.IsSelectionBoxHighlightedProperty);
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x001359EF File Offset: 0x00133BEF
		private ComboBoxItem HighlightedElement
		{
			get
			{
				if (!(this._highlightedInfo == null))
				{
					return this._highlightedInfo.Container as ComboBoxItem;
				}
				return null;
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x060043AF RID: 17327 RVA: 0x00135A11 File Offset: 0x00133C11
		// (set) Token: 0x060043B0 RID: 17328 RVA: 0x00135A1F File Offset: 0x00133C1F
		private bool IsMouseOverItemsHost
		{
			get
			{
				return this._cacheValid[1];
			}
			set
			{
				this._cacheValid[1] = value;
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x00135A2E File Offset: 0x00133C2E
		// (set) Token: 0x060043B2 RID: 17330 RVA: 0x00135A3C File Offset: 0x00133C3C
		private bool HasMouseEnteredItemsHost
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				this._cacheValid[2] = value;
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x00135A4B File Offset: 0x00133C4B
		// (set) Token: 0x060043B4 RID: 17332 RVA: 0x00135A59 File Offset: 0x00133C59
		private bool IsContextMenuOpen
		{
			get
			{
				return this._cacheValid[4];
			}
			set
			{
				this._cacheValid[4] = value;
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x060043B5 RID: 17333 RVA: 0x00135A68 File Offset: 0x00133C68
		// (set) Token: 0x060043B6 RID: 17334 RVA: 0x00135A76 File Offset: 0x00133C76
		private bool UpdatingText
		{
			get
			{
				return this._cacheValid[8];
			}
			set
			{
				this._cacheValid[8] = value;
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x060043B7 RID: 17335 RVA: 0x00135A85 File Offset: 0x00133C85
		// (set) Token: 0x060043B8 RID: 17336 RVA: 0x00135A94 File Offset: 0x00133C94
		private bool UpdatingSelectedItem
		{
			get
			{
				return this._cacheValid[16];
			}
			set
			{
				this._cacheValid[16] = value;
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x060043B9 RID: 17337 RVA: 0x00135AA4 File Offset: 0x00133CA4
		// (set) Token: 0x060043BA RID: 17338 RVA: 0x00135AB3 File Offset: 0x00133CB3
		private bool IsWaitingForTextComposition
		{
			get
			{
				return this._cacheValid[32];
			}
			set
			{
				this._cacheValid[32] = value;
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x060043BB RID: 17339 RVA: 0x00135AC3 File Offset: 0x00133CC3
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ComboBox._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.MaxDropDownHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.MaxDropDownHeight" /> dependency property.</returns>
		// Token: 0x04002844 RID: 10308
		public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(ComboBox), new FrameworkPropertyMetadata(SystemParameters.PrimaryScreenHeight / 3.0, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.IsDropDownOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.IsDropDownOpen" /> dependency property.</returns>
		// Token: 0x04002845 RID: 10309
		public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ComboBox.OnIsDropDownOpenChanged), new CoerceValueCallback(ComboBox.CoerceIsDropDownOpen)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.ShouldPreserveUserEnteredPrefix" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.ShouldPreserveUserEnteredPrefix" /> dependency property.</returns>
		// Token: 0x04002846 RID: 10310
		public static readonly DependencyProperty ShouldPreserveUserEnteredPrefixProperty = DependencyProperty.Register("ShouldPreserveUserEnteredPrefix", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.IsEditable" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.IsEditable" /> dependency property.</returns>
		// Token: 0x04002847 RID: 10311
		public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ComboBox.OnIsEditableChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.Text" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.Text" /> dependency property.</returns>
		// Token: 0x04002848 RID: 10312
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ComboBox.OnTextChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.IsReadOnly" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.IsReadOnly" /> dependency property.</returns>
		// Token: 0x04002849 RID: 10313
		public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(ComboBox));

		// Token: 0x0400284A RID: 10314
		private static readonly DependencyPropertyKey SelectionBoxItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItem", typeof(object), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItem" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItem" /> dependency property.</returns>
		// Token: 0x0400284B RID: 10315
		public static readonly DependencyProperty SelectionBoxItemProperty = ComboBox.SelectionBoxItemPropertyKey.DependencyProperty;

		// Token: 0x0400284C RID: 10316
		private static readonly DependencyPropertyKey SelectionBoxItemTemplatePropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemTemplate", typeof(DataTemplate), typeof(ComboBox), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItemTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItemTemplate" /> dependency property.</returns>
		// Token: 0x0400284D RID: 10317
		public static readonly DependencyProperty SelectionBoxItemTemplateProperty = ComboBox.SelectionBoxItemTemplatePropertyKey.DependencyProperty;

		// Token: 0x0400284E RID: 10318
		private static readonly DependencyPropertyKey SelectionBoxItemStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectionBoxItemStringFormat", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItemStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.SelectionBoxItemStringFormat" /> dependency property.</returns>
		// Token: 0x0400284F RID: 10319
		public static readonly DependencyProperty SelectionBoxItemStringFormatProperty = ComboBox.SelectionBoxItemStringFormatPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBox.StaysOpenOnEdit" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBox.StaysOpenOnEdit" /> dependency property.</returns>
		// Token: 0x04002850 RID: 10320
		public static readonly DependencyProperty StaysOpenOnEditProperty = DependencyProperty.Register("StaysOpenOnEdit", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04002851 RID: 10321
		private static readonly DependencyPropertyKey IsSelectionBoxHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelectionBoxHighlighted", typeof(bool), typeof(ComboBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(ComboBox.CoerceIsSelectionBoxHighlighted)));

		// Token: 0x04002852 RID: 10322
		private static readonly DependencyProperty IsSelectionBoxHighlightedProperty = ComboBox.IsSelectionBoxHighlightedPropertyKey.DependencyProperty;

		// Token: 0x04002853 RID: 10323
		private static readonly EventPrivateKey DropDownOpenedKey = new EventPrivateKey();

		// Token: 0x04002854 RID: 10324
		private static readonly EventPrivateKey DropDownClosedKey = new EventPrivateKey();

		// Token: 0x04002855 RID: 10325
		private const string EditableTextBoxTemplateName = "PART_EditableTextBox";

		// Token: 0x04002856 RID: 10326
		private const string PopupTemplateName = "PART_Popup";

		// Token: 0x04002857 RID: 10327
		private TextBox _editableTextBoxSite;

		// Token: 0x04002858 RID: 10328
		private Popup _dropDownPopup;

		// Token: 0x04002859 RID: 10329
		private int _textBoxSelectionStart;

		// Token: 0x0400285A RID: 10330
		private BitVector32 _cacheValid = new BitVector32(0);

		// Token: 0x0400285B RID: 10331
		private ItemsControl.ItemInfo _highlightedInfo;

		// Token: 0x0400285C RID: 10332
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x0400285D RID: 10333
		private UIElement _clonedElement;

		// Token: 0x0400285E RID: 10334
		private DispatcherOperation _updateTextBoxOperation;

		// Token: 0x0400285F RID: 10335
		private static DependencyObjectType _dType;

		// Token: 0x0200095E RID: 2398
		private enum CacheBits
		{
			// Token: 0x04004407 RID: 17415
			IsMouseOverItemsHost = 1,
			// Token: 0x04004408 RID: 17416
			HasMouseEnteredItemsHost,
			// Token: 0x04004409 RID: 17417
			IsContextMenuOpen = 4,
			// Token: 0x0400440A RID: 17418
			UpdatingText = 8,
			// Token: 0x0400440B RID: 17419
			UpdatingSelectedItem = 16,
			// Token: 0x0400440C RID: 17420
			IsWaitingForTextComposition = 32
		}
	}
}
