using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Contains a list of selectable items. </summary>
	// Token: 0x020004FA RID: 1274
	[Localizability(LocalizationCategory.ListBox)]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListBoxItem))]
	public class ListBox : Selector
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ListBox" /> class. </summary>
		// Token: 0x06005123 RID: 20771 RVA: 0x0016C18C File Offset: 0x0016A38C
		public ListBox()
		{
			this.Initialize();
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x0016C19C File Offset: 0x0016A39C
		private void Initialize()
		{
			SelectionMode mode = (SelectionMode)ListBox.SelectionModeProperty.GetDefaultValue(base.DependencyObjectType);
			this.ValidateSelectionMode(mode);
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x0016C1C8 File Offset: 0x0016A3C8
		static ListBox()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(typeof(ListBox)));
			ListBox._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ListBox));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			ItemsControl.IsTextSearchEnabledProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(VirtualizingStackPanel)));
			itemsPanelTemplate.Seal();
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(ListBox), new FrameworkPropertyMetadata(itemsPanelTemplate));
			EventManager.RegisterClassHandler(typeof(ListBox), Mouse.MouseUpEvent, new MouseButtonEventHandler(ListBox.OnMouseButtonUp), true);
			EventManager.RegisterClassHandler(typeof(ListBox), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ListBox.OnGotKeyboardFocus));
			CommandHelpers.RegisterCommandHandler(typeof(ListBox), ListBox.SelectAllCommand, new ExecutedRoutedEventHandler(ListBox.OnSelectAll), new CanExecuteRoutedEventHandler(ListBox.OnQueryStatusSelectAll), KeyGesture.CreateFromResourceStrings(SR.Get("ListBoxSelectAllKey"), SR.Get("ListBoxSelectAllKeyDisplayString")));
			ControlsTraceLogger.AddControl(TelemetryControls.ListBox);
		}

		/// <summary>Selects all the items in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> property is set to <see cref="F:System.Windows.Controls.SelectionMode.Single" />.</exception>
		// Token: 0x06005126 RID: 20774 RVA: 0x0016C3BF File Offset: 0x0016A5BF
		public void SelectAll()
		{
			if (base.CanSelectMultiple)
			{
				this.SelectAllImpl();
				return;
			}
			throw new NotSupportedException(SR.Get("ListBoxSelectAllSelectionMode"));
		}

		/// <summary>Clears all the selection in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
		// Token: 0x06005127 RID: 20775 RVA: 0x0016C3DF File Offset: 0x0016A5DF
		public void UnselectAll()
		{
			this.UnselectAllImpl();
		}

		/// <summary>Causes the object to scroll into view. </summary>
		/// <param name="item">Object to scroll.</param>
		// Token: 0x06005128 RID: 20776 RVA: 0x0016C3E7 File Offset: 0x0016A5E7
		public void ScrollIntoView(object item)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.OnBringItemIntoView(item);
				return;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(base.OnBringItemIntoView), item);
		}

		/// <summary>Gets or sets the selection behavior for a <see cref="T:System.Windows.Controls.ListBox" />.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.SelectionMode" /> values. The default is <see cref="F:System.Windows.Controls.SelectionMode.Single" /> selection. </returns>
		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06005129 RID: 20777 RVA: 0x0016C41A File Offset: 0x0016A61A
		// (set) Token: 0x0600512A RID: 20778 RVA: 0x0016C42C File Offset: 0x0016A62C
		public SelectionMode SelectionMode
		{
			get
			{
				return (SelectionMode)base.GetValue(ListBox.SelectionModeProperty);
			}
			set
			{
				base.SetValue(ListBox.SelectionModeProperty, value);
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x0016C440 File Offset: 0x0016A640
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBox listBox = (ListBox)d;
			listBox.ValidateSelectionMode(listBox.SelectionMode);
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x0016C460 File Offset: 0x0016A660
		private static object OnGetSelectionMode(DependencyObject d)
		{
			return ((ListBox)d).SelectionMode;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x0016C474 File Offset: 0x0016A674
		private static bool IsValidSelectionMode(object o)
		{
			SelectionMode selectionMode = (SelectionMode)o;
			return selectionMode == SelectionMode.Single || selectionMode == SelectionMode.Multiple || selectionMode == SelectionMode.Extended;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x0016C495 File Offset: 0x0016A695
		private void ValidateSelectionMode(SelectionMode mode)
		{
			base.CanSelectMultiple = (mode > SelectionMode.Single);
		}

		/// <summary>Gets the currently selected items.  </summary>
		/// <returns>Returns a collection of the currently selected items.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> property is set to <see cref="F:System.Windows.Controls.SelectionMode.Single" />.</exception>
		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x0016C4A1 File Offset: 0x0016A6A1
		[Bindable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList SelectedItems
		{
			get
			{
				return base.SelectedItemsImpl;
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06005130 RID: 20784 RVA: 0x0016C4A9 File Offset: 0x0016A6A9
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ListBoxAutomationPeer(this);
		}

		/// <summary>Sets a collection of selected items. </summary>
		/// <param name="selectedItems">Collection of items to be selected.</param>
		/// <returns>
		///     <see langword="true" /> if all items have been selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005131 RID: 20785 RVA: 0x0016C4B1 File Offset: 0x0016A6B1
		protected bool SetSelectedItems(IEnumerable selectedItems)
		{
			return base.SetSelectedItemsImpl(selectedItems);
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x06005132 RID: 20786 RVA: 0x00134CAA File Offset: 0x00132EAA
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			if (item is Separator)
			{
				Separator.PrepareContainer(element as Control);
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x0016C4BA File Offset: 0x0016A6BA
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			base.AdjustItemInfo(e, this._anchorItem);
			if (this._anchorItem != null && this._anchorItem.Index < 0)
			{
				this._anchorItem = null;
			}
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x0016C4F3 File Offset: 0x0016A6F3
		internal override void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			base.AdjustItemInfoAfterGeneratorChange(this._anchorItem);
			base.AdjustItemInfosAfterGeneratorChangeOverride();
		}

		/// <summary>Responds to a list box selection change by raising a <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" />. </param>
		// Token: 0x06005135 RID: 20789 RVA: 0x0016C508 File Offset: 0x0016A708
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			if (this.SelectionMode == SelectionMode.Single)
			{
				ItemsControl.ItemInfo internalSelectedInfo = base.InternalSelectedInfo;
				ListBoxItem listBoxItem = (internalSelectedInfo != null) ? (internalSelectedInfo.Container as ListBoxItem) : null;
				if (listBoxItem != null)
				{
					this.UpdateAnchorAndActionItem(internalSelectedInfo);
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				ListBoxAutomationPeer listBoxAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as ListBoxAutomationPeer;
				if (listBoxAutomationPeer != null)
				{
					listBoxAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.UIElement.KeyDown" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.KeyEventArgs" />.</param>
		// Token: 0x06005136 RID: 20790 RVA: 0x0016C584 File Offset: 0x0016A784
		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool flag = true;
			Key key = e.Key;
			if (key <= Key.Down)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Space:
						break;
					case Key.Prior:
						base.NavigateByPage(FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Next:
						base.NavigateByPage(FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.End:
						base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Home:
						base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
						goto IL_379;
					case Key.Left:
					case Key.Up:
					case Key.Right:
					case Key.Down:
					{
						KeyboardNavigation.ShowFocusVisual();
						bool flag2 = base.ScrollHost != null;
						if (flag2)
						{
							flag2 = ((key == Key.Down && base.IsLogicalHorizontal && DoubleUtil.GreaterThan(base.ScrollHost.ScrollableHeight, base.ScrollHost.VerticalOffset)) || (key == Key.Up && base.IsLogicalHorizontal && DoubleUtil.GreaterThan(base.ScrollHost.VerticalOffset, 0.0)) || (key == Key.Right && base.IsLogicalVertical && DoubleUtil.GreaterThan(base.ScrollHost.ScrollableWidth, base.ScrollHost.HorizontalOffset)) || (key == Key.Left && base.IsLogicalVertical && DoubleUtil.GreaterThan(base.ScrollHost.HorizontalOffset, 0.0)));
						}
						if (flag2)
						{
							base.ScrollHost.ScrollInDirection(e);
							goto IL_379;
						}
						if ((base.ItemsHost == null || !base.ItemsHost.IsKeyboardFocusWithin) && !base.IsKeyboardFocused)
						{
							flag = false;
							goto IL_379;
						}
						if (!base.NavigateByLine(KeyboardNavigation.KeyToTraversalDirection(key), new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers)))
						{
							flag = false;
							goto IL_379;
						}
						goto IL_379;
					}
					default:
						goto IL_377;
					}
				}
				if (e.Key == Key.Return && !(bool)base.GetValue(KeyboardNavigation.AcceptsReturnProperty))
				{
					flag = false;
					goto IL_379;
				}
				ListBoxItem listBoxItem = e.OriginalSource as ListBoxItem;
				if ((Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control)) == ModifierKeys.Alt)
				{
					flag = false;
					goto IL_379;
				}
				if (base.IsTextSearchEnabled && Keyboard.Modifiers == ModifierKeys.None)
				{
					TextSearch textSearch = TextSearch.EnsureInstance(this);
					if (textSearch != null && textSearch.GetCurrentPrefix() != string.Empty)
					{
						flag = false;
						goto IL_379;
					}
				}
				if (listBoxItem == null || ItemsControl.ItemsControlFromItemContainer(listBoxItem) != this)
				{
					flag = false;
					goto IL_379;
				}
				switch (this.SelectionMode)
				{
				case SelectionMode.Single:
					if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
					{
						this.MakeToggleSelection(listBoxItem);
						goto IL_379;
					}
					this.MakeSingleSelection(listBoxItem);
					goto IL_379;
				case SelectionMode.Multiple:
					this.MakeToggleSelection(listBoxItem);
					goto IL_379;
				case SelectionMode.Extended:
					if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.Control)
					{
						this.MakeToggleSelection(listBoxItem);
						goto IL_379;
					}
					if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.Shift)
					{
						this.MakeAnchorSelection(listBoxItem, true);
						goto IL_379;
					}
					if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
					{
						this.MakeSingleSelection(listBoxItem);
						goto IL_379;
					}
					flag = false;
					goto IL_379;
				default:
					goto IL_379;
				}
			}
			else if (key != Key.Divide && key != Key.Oem2)
			{
				if (key == Key.Oem5)
				{
					if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control || this.SelectionMode != SelectionMode.Extended)
					{
						flag = false;
						goto IL_379;
					}
					ListBoxItem listBoxItem2 = (base.FocusedInfo != null) ? (base.FocusedInfo.Container as ListBoxItem) : null;
					if (listBoxItem2 != null)
					{
						this.MakeSingleSelection(listBoxItem2);
						goto IL_379;
					}
					goto IL_379;
				}
			}
			else
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && this.SelectionMode == SelectionMode.Extended)
				{
					this.SelectAll();
					goto IL_379;
				}
				flag = false;
				goto IL_379;
			}
			IL_377:
			flag = false;
			IL_379:
			if (flag)
			{
				e.Handled = true;
				return;
			}
			base.OnKeyDown(e);
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.ListBox" /> reports a mouse move. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.MouseEventArgs" />.</param>
		// Token: 0x06005137 RID: 20791 RVA: 0x0016C91C File Offset: 0x0016AB1C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.OriginalSource == this && Mouse.Captured == this)
			{
				if (Mouse.LeftButton == MouseButtonState.Pressed)
				{
					base.DoAutoScroll();
				}
				else
				{
					base.ReleaseMouseCapture();
					base.ResetLastMousePosition();
				}
			}
			base.OnMouseMove(e);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x0016C954 File Offset: 0x0016AB54
		private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				ListBox listBox = (ListBox)sender;
				listBox.ReleaseMouseCapture();
				listBox.ResetLastMousePosition();
			}
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x0016C97C File Offset: 0x0016AB7C
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ListBox listBox = (ListBox)sender;
			if (!KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				return;
			}
			ListBoxItem listBoxItem = e.NewFocus as ListBoxItem;
			if (listBoxItem != null && ItemsControl.ItemsControlFromItemContainer(listBoxItem) == listBox)
			{
				DependencyObject dependencyObject = e.OldFocus as DependencyObject;
				Visual visual = dependencyObject as Visual;
				if (visual == null)
				{
					ContentElement contentElement = dependencyObject as ContentElement;
					if (contentElement != null)
					{
						visual = KeyboardNavigation.GetParentUIElementFromContentElement(contentElement);
					}
				}
				if ((visual != null && listBox.IsAncestorOf(visual)) || dependencyObject == listBox)
				{
					listBox.LastActionItem = listBoxItem;
					listBox.MakeKeyboardSelection(listBoxItem);
				}
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.UIElement.IsMouseCaptured" /> property changes. </summary>
		/// <param name="e">Provides data for the <see cref="T:System.Windows.DependencyPropertyChangedEventArgs" />.</param>
		// Token: 0x0600513A RID: 20794 RVA: 0x0016C9F8 File Offset: 0x0016ABF8
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

		/// <summary>Determines if the specified item is (or is eligible to be) its own ItemContainer. </summary>
		/// <param name="item">Specified item.</param>
		/// <returns>
		///     <see langword="true" /> if the item is its own <see langword="ItemContainer" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600513B RID: 20795 RVA: 0x0016CA76 File Offset: 0x0016AC76
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListBoxItem;
		}

		/// <summary>Creates or identifies the element used to display a specified item. </summary>
		/// <returns>The element used to display a specified item.</returns>
		// Token: 0x0600513C RID: 20796 RVA: 0x0016CA81 File Offset: 0x0016AC81
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListBoxItem();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.ListBox" /> supports scrolling.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ListBox" /> supports scrolling; otherwise, <see langword="false" />.</returns>
		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x0600513D RID: 20797 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x0016CA88 File Offset: 0x0016AC88
		private static void OnQueryStatusSelectAll(object target, CanExecuteRoutedEventArgs args)
		{
			ListBox listBox = target as ListBox;
			if (listBox.SelectionMode == SelectionMode.Extended)
			{
				args.CanExecute = true;
			}
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x0016CAAC File Offset: 0x0016ACAC
		private static void OnSelectAll(object target, ExecutedRoutedEventArgs args)
		{
			ListBox listBox = target as ListBox;
			if (listBox.SelectionMode == SelectionMode.Extended)
			{
				listBox.SelectAll();
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x0016CAD0 File Offset: 0x0016ACD0
		internal void NotifyListItemClicked(ListBoxItem item, MouseButton mouseButton)
		{
			if (mouseButton == MouseButton.Left && Mouse.Captured != this)
			{
				Mouse.Capture(this, CaptureMode.SubTree);
				base.SetInitialMousePosition();
			}
			switch (this.SelectionMode)
			{
			case SelectionMode.Single:
				if (!item.IsSelected)
				{
					item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
				else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
				return;
			case SelectionMode.Multiple:
				this.MakeToggleSelection(item);
				return;
			case SelectionMode.Extended:
				if (mouseButton != MouseButton.Left)
				{
					if (mouseButton == MouseButton.Right && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == ModifierKeys.None)
					{
						if (item.IsSelected)
						{
							this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
							return;
						}
						this.MakeSingleSelection(item);
					}
					return;
				}
				if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
				{
					this.MakeAnchorSelection(item, false);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				{
					this.MakeToggleSelection(item);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					this.MakeAnchorSelection(item, true);
					return;
				}
				this.MakeSingleSelection(item);
				return;
			default:
				return;
			}
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x0016CBC7 File Offset: 0x0016ADC7
		internal void NotifyListItemMouseDragged(ListBoxItem listItem)
		{
			if (Mouse.Captured == this && base.DidMouseMove())
			{
				base.NavigateToItem(base.ItemInfoFromContainer(listItem), new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers), false);
			}
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x0016CBF8 File Offset: 0x0016ADF8
		private void UpdateAnchorAndActionItem(ItemsControl.ItemInfo info)
		{
			object item = info.Item;
			ListBoxItem listBoxItem = info.Container as ListBoxItem;
			if (item == DependencyProperty.UnsetValue)
			{
				this.AnchorItemInternal = null;
				this.LastActionItem = null;
			}
			else
			{
				this.AnchorItemInternal = info;
				this.LastActionItem = listBoxItem;
			}
			KeyboardNavigation.SetTabOnceActiveElement(this, listBoxItem);
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x0016CC48 File Offset: 0x0016AE48
		private void MakeSingleSelection(ListBoxItem listItem)
		{
			if (ItemsControl.ItemsControlFromItemContainer(listItem) == this)
			{
				ItemsControl.ItemInfo info = base.ItemInfoFromContainer(listItem);
				base.SelectionChange.SelectJustThisItem(info, true);
				listItem.Focus();
				this.UpdateAnchorAndActionItem(info);
			}
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x0016CC84 File Offset: 0x0016AE84
		private void MakeToggleSelection(ListBoxItem item)
		{
			bool value = !item.IsSelected;
			item.SetCurrentValueInternal(Selector.IsSelectedProperty, BooleanBoxes.Box(value));
			this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0016CCBC File Offset: 0x0016AEBC
		private void MakeAnchorSelection(ListBoxItem actionItem, bool clearCurrent)
		{
			ItemsControl.ItemInfo anchorItemInternal = this.AnchorItemInternal;
			if (anchorItemInternal == null)
			{
				if (this._selectedItems.Count > 0)
				{
					this.AnchorItemInternal = this._selectedItems[this._selectedItems.Count - 1];
				}
				else
				{
					this.AnchorItemInternal = base.NewItemInfo(base.Items[0], null, 0);
				}
				if ((anchorItemInternal = this.AnchorItemInternal) == null)
				{
					return;
				}
			}
			int num = this.ElementIndex(actionItem);
			int num2 = this.AnchorItemInternal.Index;
			if (num > num2)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			bool flag = false;
			if (!base.SelectionChange.IsActive)
			{
				flag = true;
				base.SelectionChange.Begin();
			}
			try
			{
				if (clearCurrent)
				{
					for (int i = 0; i < this._selectedItems.Count; i++)
					{
						ItemsControl.ItemInfo itemInfo = this._selectedItems[i];
						int index = itemInfo.Index;
						if (index < num || num2 < index)
						{
							base.SelectionChange.Unselect(itemInfo);
						}
					}
				}
				IEnumerator enumerator = ((IEnumerable)base.Items).GetEnumerator();
				for (int j = 0; j <= num2; j++)
				{
					enumerator.MoveNext();
					if (j >= num)
					{
						base.SelectionChange.Select(base.NewItemInfo(enumerator.Current, null, j), true);
					}
				}
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			finally
			{
				if (flag)
				{
					base.SelectionChange.End();
				}
			}
			this.LastActionItem = actionItem;
			GC.KeepAlive(anchorItemInternal);
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x0016CE48 File Offset: 0x0016B048
		private void MakeKeyboardSelection(ListBoxItem item)
		{
			if (item == null)
			{
				return;
			}
			switch (this.SelectionMode)
			{
			case SelectionMode.Single:
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					this.MakeSingleSelection(item);
					return;
				}
				break;
			case SelectionMode.Multiple:
				this.UpdateAnchorAndActionItem(base.ItemInfoFromContainer(item));
				return;
			case SelectionMode.Extended:
				if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					bool clearCurrent = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None;
					this.MakeAnchorSelection(item, clearCurrent);
					return;
				}
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					this.MakeSingleSelection(item);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x0016CEC0 File Offset: 0x0016B0C0
		private int ElementIndex(ListBoxItem listItem)
		{
			return base.ItemContainerGenerator.IndexFromContainer(listItem);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x0016CECE File Offset: 0x0016B0CE
		private ListBoxItem ElementAt(int index)
		{
			return base.ItemContainerGenerator.ContainerFromIndex(index) as ListBoxItem;
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x0016CEE1 File Offset: 0x0016B0E1
		private object GetWeakReferenceTarget(ref WeakReference weakReference)
		{
			if (weakReference != null)
			{
				return weakReference.Target;
			}
			return null;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0016CEF0 File Offset: 0x0016B0F0
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				base.DoAutoScroll();
			}
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x0016CF00 File Offset: 0x0016B100
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = base.FocusItem(info, itemNavigateArgs);
			ListBoxItem listBoxItem = info.Container as ListBoxItem;
			if (listBoxItem != null)
			{
				this.LastActionItem = listBoxItem;
				this.MakeKeyboardSelection(listBoxItem);
			}
			return result;
		}

		/// <summary>Gets or sets the item that is initially selected when <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> is <see cref="F:System.Windows.Controls.SelectionMode.Extended" />.</summary>
		/// <returns>The item that is initially selected when <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> is <see cref="F:System.Windows.Controls.SelectionMode.Extended" />.</returns>
		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x0600514C RID: 20812 RVA: 0x0016CF34 File Offset: 0x0016B134
		// (set) Token: 0x0600514D RID: 20813 RVA: 0x0016CF3C File Offset: 0x0016B13C
		protected object AnchorItem
		{
			get
			{
				return this.AnchorItemInternal;
			}
			set
			{
				if (value == null || value == DependencyProperty.UnsetValue)
				{
					this.AnchorItemInternal = null;
					this.LastActionItem = null;
					return;
				}
				ItemsControl.ItemInfo itemInfo = base.NewItemInfo(value, null, -1);
				ListBoxItem listBoxItem = itemInfo.Container as ListBoxItem;
				if (listBoxItem == null)
				{
					throw new InvalidOperationException(SR.Get("ListBoxInvalidAnchorItem", new object[]
					{
						value
					}));
				}
				this.AnchorItemInternal = itemInfo;
				this.LastActionItem = listBoxItem;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x0600514E RID: 20814 RVA: 0x0016CFA4 File Offset: 0x0016B1A4
		// (set) Token: 0x0600514F RID: 20815 RVA: 0x0016CFAC File Offset: 0x0016B1AC
		internal ItemsControl.ItemInfo AnchorItemInternal
		{
			get
			{
				return this._anchorItem;
			}
			set
			{
				this._anchorItem = ((value != null) ? value.Clone() : null);
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06005150 RID: 20816 RVA: 0x0016CFC6 File Offset: 0x0016B1C6
		// (set) Token: 0x06005151 RID: 20817 RVA: 0x0016CFD9 File Offset: 0x0016B1D9
		internal ListBoxItem LastActionItem
		{
			get
			{
				return this.GetWeakReferenceTarget(ref this._lastActionItem) as ListBoxItem;
			}
			set
			{
				this._lastActionItem = new WeakReference(value);
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x0016CFE7 File Offset: 0x0016B1E7
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ListBox._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListBox.SelectionMode" /> dependency property.</returns>
		// Token: 0x04002C57 RID: 11351
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(ListBox), new FrameworkPropertyMetadata(SelectionMode.Single, new PropertyChangedCallback(ListBox.OnSelectionModeChanged)), new ValidateValueCallback(ListBox.IsValidSelectionMode));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListBox.SelectedItems" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListBox.SelectedItems" /> dependency property.</returns>
		// Token: 0x04002C58 RID: 11352
		public static readonly DependencyProperty SelectedItemsProperty = Selector.SelectedItemsImplProperty;

		// Token: 0x04002C59 RID: 11353
		private ItemsControl.ItemInfo _anchorItem;

		// Token: 0x04002C5A RID: 11354
		private WeakReference _lastActionItem;

		// Token: 0x04002C5B RID: 11355
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x04002C5C RID: 11356
		private static RoutedUICommand SelectAllCommand = new RoutedUICommand(SR.Get("ListBoxSelectAllText"), "SelectAll", typeof(ListBox));

		// Token: 0x04002C5D RID: 11357
		private static DependencyObjectType _dType;
	}
}
