using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that displays hierarchical data in a tree structure that has items that can expand and collapse. </summary>
	// Token: 0x0200054C RID: 1356
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
	public class TreeView : ItemsControl
	{
		// Token: 0x060058CE RID: 22734 RVA: 0x00189160 File Offset: 0x00187360
		static TreeView()
		{
			TreeView.SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(TreeView));
			TreeView.SelectedValuePathBindingExpression = new BindingExpressionUncommonField();
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(typeof(TreeView)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			TreeView._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TreeView));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			VirtualizingPanel.ScrollUnitProperty.OverrideMetadata(typeof(TreeView), new FrameworkPropertyMetadata(ScrollUnit.Pixel));
			ControlsTraceLogger.AddControl(TelemetryControls.TreeView);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.TreeView" /> class.</summary>
		// Token: 0x060058CF RID: 22735 RVA: 0x00189304 File Offset: 0x00187504
		public TreeView()
		{
			this._focusEnterMainFocusScopeEventHandler = new EventHandler(this.OnFocusEnterMainFocusScope);
			KeyboardNavigation.Current.FocusEnterMainFocusScope += this._focusEnterMainFocusScopeEventHandler;
		}

		/// <summary>Gets the selected item in a <see cref="T:System.Windows.Controls.TreeView" />.  </summary>
		/// <returns>The selected object in the <see cref="T:System.Windows.Controls.TreeView" />, or <see langword="null" /> if no item is selected. The default value is <see langword="null" />.</returns>
		// Token: 0x1700159A RID: 5530
		// (get) Token: 0x060058D0 RID: 22736 RVA: 0x0018933A File Offset: 0x0018753A
		[Bindable(true)]
		[Category("Appearance")]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return base.GetValue(TreeView.SelectedItemProperty);
			}
		}

		// Token: 0x060058D1 RID: 22737 RVA: 0x00189347 File Offset: 0x00187547
		private void SetSelectedItem(object data)
		{
			if (this.SelectedItem != data)
			{
				base.SetValue(TreeView.SelectedItemPropertyKey, data);
			}
		}

		/// <summary>Gets the value of the property that is the specified by <see cref="P:System.Windows.Controls.TreeView.SelectedValuePath" /> for the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" />.   </summary>
		/// <returns>The value of the property that is specified by the <see cref="P:System.Windows.Controls.TreeView.SelectedValuePath" /> for the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" />, or <see langword="null" /> if no item is selected. The default value is <see langword="null" />.</returns>
		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x060058D2 RID: 22738 RVA: 0x0018935E File Offset: 0x0018755E
		[Bindable(true)]
		[Category("Appearance")]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedValue
		{
			get
			{
				return base.GetValue(TreeView.SelectedValueProperty);
			}
		}

		// Token: 0x060058D3 RID: 22739 RVA: 0x0018936B File Offset: 0x0018756B
		private void SetSelectedValue(object data)
		{
			if (this.SelectedValue != data)
			{
				base.SetValue(TreeView.SelectedValuePropertyKey, data);
			}
		}

		/// <summary>Gets or sets the path that is used to get the <see cref="P:System.Windows.Controls.TreeView.SelectedValue" /> of the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" /> in a <see cref="T:System.Windows.Controls.TreeView" />.  </summary>
		/// <returns>A string that contains the path that is used to get the <see cref="P:System.Windows.Controls.TreeView.SelectedValue" />. The default value is <see langword="String.Empty" />.</returns>
		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x060058D4 RID: 22740 RVA: 0x00189382 File Offset: 0x00187582
		// (set) Token: 0x060058D5 RID: 22741 RVA: 0x00189394 File Offset: 0x00187594
		[Bindable(true)]
		[Category("Appearance")]
		public string SelectedValuePath
		{
			get
			{
				return (string)base.GetValue(TreeView.SelectedValuePathProperty);
			}
			set
			{
				base.SetValue(TreeView.SelectedValuePathProperty, value);
			}
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x001893A4 File Offset: 0x001875A4
		private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeView treeView = (TreeView)d;
			TreeView.SelectedValuePathBindingExpression.ClearValue(treeView);
			treeView.UpdateSelectedValue(treeView.SelectedItem);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" /> changes.</summary>
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x060058D7 RID: 22743 RVA: 0x001893CF File Offset: 0x001875CF
		// (remove) Token: 0x060058D8 RID: 22744 RVA: 0x001893DD File Offset: 0x001875DD
		[Category("Behavior")]
		public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged
		{
			add
			{
				base.AddHandler(TreeView.SelectedItemChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeView.SelectedItemChangedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.TreeView.SelectedItemChanged" /> event when the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" /> property value changes.</summary>
		/// <param name="e">Provides the item that was previously selected and the item that is currently selected for the <see cref="E:System.Windows.Controls.TreeView.SelectedItemChanged" /> event.</param>
		// Token: 0x060058D9 RID: 22745 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x060058DA RID: 22746 RVA: 0x001893EC File Offset: 0x001875EC
		internal void ChangeSelection(object data, TreeViewItem container, bool selected)
		{
			if (this.IsSelectionChangeActive)
			{
				return;
			}
			object oldValue = null;
			object newValue = null;
			bool flag = false;
			TreeViewItem selectedContainer = this._selectedContainer;
			this.IsSelectionChangeActive = true;
			try
			{
				if (selected)
				{
					if (container != this._selectedContainer)
					{
						oldValue = this.SelectedItem;
						newValue = data;
						if (this._selectedContainer != null)
						{
							this._selectedContainer.IsSelected = false;
							this._selectedContainer.UpdateContainsSelection(false);
						}
						this._selectedContainer = container;
						this._selectedContainer.UpdateContainsSelection(true);
						this.SetSelectedItem(data);
						this.UpdateSelectedValue(data);
						flag = true;
					}
				}
				else if (container == this._selectedContainer)
				{
					this._selectedContainer.UpdateContainsSelection(false);
					this._selectedContainer = null;
					this.SetSelectedItem(null);
					this.UpdateSelectedValue(null);
					oldValue = data;
					flag = true;
				}
				if (container.IsSelected != selected)
				{
					container.IsSelected = selected;
				}
			}
			finally
			{
				this.IsSelectionChangeActive = false;
			}
			if (flag)
			{
				if (this._selectedContainer != null && AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected))
				{
					TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this._selectedContainer) as TreeViewItemAutomationPeer;
					if (treeViewItemAutomationPeer != null)
					{
						treeViewItemAutomationPeer.RaiseAutomationSelectionEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
					}
				}
				if (selectedContainer != null && AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					TreeViewItemAutomationPeer treeViewItemAutomationPeer2 = UIElementAutomationPeer.CreatePeerForElement(selectedContainer) as TreeViewItemAutomationPeer;
					if (treeViewItemAutomationPeer2 != null)
					{
						treeViewItemAutomationPeer2.RaiseAutomationSelectionEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
				RoutedPropertyChangedEventArgs<object> e = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, TreeView.SelectedItemChangedEvent);
				this.OnSelectedItemChanged(e);
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x060058DB RID: 22747 RVA: 0x00189534 File Offset: 0x00187734
		// (set) Token: 0x060058DC RID: 22748 RVA: 0x00189542 File Offset: 0x00187742
		internal bool IsSelectionChangeActive
		{
			get
			{
				return this._bits[1];
			}
			set
			{
				this._bits[1] = value;
			}
		}

		// Token: 0x060058DD RID: 22749 RVA: 0x00189554 File Offset: 0x00187754
		private void UpdateSelectedValue(object selectedItem)
		{
			BindingExpression bindingExpression = this.PrepareSelectedValuePathBindingExpression(selectedItem);
			if (bindingExpression != null)
			{
				bindingExpression.Activate(selectedItem);
				object value = bindingExpression.Value;
				bindingExpression.Deactivate();
				base.SetValue(TreeView.SelectedValuePropertyKey, value);
				return;
			}
			base.ClearValue(TreeView.SelectedValuePropertyKey);
		}

		// Token: 0x060058DE RID: 22750 RVA: 0x00189598 File Offset: 0x00187798
		private BindingExpression PrepareSelectedValuePathBindingExpression(object item)
		{
			if (item == null)
			{
				return null;
			}
			bool flag = SystemXmlHelper.IsXmlNode(item);
			BindingExpression bindingExpression = TreeView.SelectedValuePathBindingExpression.GetValue(this);
			if (bindingExpression != null)
			{
				Binding binding = bindingExpression.ParentBinding;
				bool flag2 = binding.XPath != null;
				if (flag2 != flag)
				{
					bindingExpression = null;
				}
			}
			if (bindingExpression == null)
			{
				Binding binding = new Binding();
				binding.Source = null;
				if (flag)
				{
					binding.XPath = this.SelectedValuePath;
					binding.Path = new PropertyPath("/InnerText", new object[0]);
				}
				else
				{
					binding.Path = new PropertyPath(this.SelectedValuePath, new object[0]);
				}
				bindingExpression = (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(this, binding);
				TreeView.SelectedValuePathBindingExpression.SetValue(this, bindingExpression);
			}
			return bindingExpression;
		}

		// Token: 0x060058DF RID: 22751 RVA: 0x00189640 File Offset: 0x00187840
		internal void HandleSelectionAndCollapsed(TreeViewItem collapsed)
		{
			if (this._selectedContainer != null && this._selectedContainer != collapsed)
			{
				TreeViewItem treeViewItem = this._selectedContainer;
				for (;;)
				{
					treeViewItem = treeViewItem.ParentTreeViewItem;
					if (treeViewItem == collapsed)
					{
						break;
					}
					if (treeViewItem == null)
					{
						return;
					}
				}
				TreeViewItem selectedContainer = this._selectedContainer;
				this.ChangeSelection(collapsed.ParentItemsControl.ItemContainerGenerator.ItemFromContainer(collapsed), collapsed, true);
				if (selectedContainer.IsKeyboardFocusWithin)
				{
					this._selectedContainer.Focus();
					return;
				}
			}
		}

		// Token: 0x060058E0 RID: 22752 RVA: 0x001896A8 File Offset: 0x001878A8
		internal void HandleMouseButtonDown()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				if (this._selectedContainer != null)
				{
					if (!this._selectedContainer.IsKeyboardFocused)
					{
						this._selectedContainer.Focus();
						return;
					}
				}
				else
				{
					base.Focus();
				}
			}
		}

		/// <summary>Determines whether the specified item is its own container or can be its own container.</summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="item" /> is a <see cref="T:System.Windows.Controls.TreeViewItem" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060058E1 RID: 22753 RVA: 0x001896DB File Offset: 0x001878DB
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeViewItem;
		}

		/// <summary>Creates a <see cref="T:System.Windows.Controls.TreeViewItem" /> to use to display content.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.TreeViewItem" /> to use as a container for content.</returns>
		// Token: 0x060058E2 RID: 22754 RVA: 0x001896E6 File Offset: 0x001878E6
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeViewItem();
		}

		/// <summary>Provides class handling for an <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event that occurs when there is a change in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060058E3 RID: 22755 RVA: 0x001896F0 File Offset: 0x001878F0
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Reset:
				if (this.SelectedItem != null && !this.IsSelectedContainerHookedUp)
				{
					this.SelectFirstItem();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
			{
				object selectedItem = this.SelectedItem;
				if (selectedItem != null && selectedItem.Equals(e.OldItems[0]))
				{
					this.ChangeSelection(selectedItem, this._selectedContainer, false);
					return;
				}
				break;
			}
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x060058E4 RID: 22756 RVA: 0x00189788 File Offset: 0x00187988
		private void SelectFirstItem()
		{
			object selectedItem;
			TreeViewItem selectedContainer;
			bool firstItem = this.GetFirstItem(out selectedItem, out selectedContainer);
			if (!firstItem)
			{
				selectedItem = this.SelectedItem;
				selectedContainer = this._selectedContainer;
			}
			this.ChangeSelection(selectedItem, selectedContainer, firstItem);
		}

		// Token: 0x060058E5 RID: 22757 RVA: 0x001897BA File Offset: 0x001879BA
		private bool GetFirstItem(out object item, out TreeViewItem container)
		{
			if (base.HasItems)
			{
				item = base.Items[0];
				container = (base.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem);
				return item != null && container != null;
			}
			item = null;
			container = null;
			return false;
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x060058E6 RID: 22758 RVA: 0x001897F8 File Offset: 0x001879F8
		internal bool IsSelectedContainerHookedUp
		{
			get
			{
				return this._selectedContainer != null && this._selectedContainer.ParentTreeView == this;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x060058E7 RID: 22759 RVA: 0x00189812 File Offset: 0x00187A12
		internal TreeViewItem SelectedContainer
		{
			get
			{
				return this._selectedContainer;
			}
		}

		/// <summary>Gets whether the <see cref="T:System.Windows.Controls.TreeView" /> can scroll.</summary>
		/// <returns>Always returns <see langword="true" /> because the control has a <see cref="T:System.Windows.Controls.ScrollViewer" /> in its style.</returns>
		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x060058E8 RID: 22760 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> event for a <see cref="T:System.Windows.Controls.TreeView" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060058E9 RID: 22761 RVA: 0x0018981C File Offset: 0x00187A1C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				if (TreeView.IsControlKeyDown)
				{
					Key key = e.Key;
					if (key - Key.Prior <= 7 && this.HandleScrollKeys(e.Key))
					{
						e.Handled = true;
						return;
					}
				}
				else
				{
					Key key = e.Key;
					if (key != Key.Tab)
					{
						switch (key)
						{
						case Key.Prior:
						case Key.Next:
							if (this._selectedContainer == null)
							{
								if (this.FocusFirstItem())
								{
									e.Handled = true;
									return;
								}
							}
							else if (this.HandleScrollByPage(e))
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.End:
							if (this.FocusLastItem())
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.Home:
							if (this.FocusFirstItem())
							{
								e.Handled = true;
								return;
							}
							break;
						case Key.Left:
						case Key.Right:
							break;
						case Key.Up:
						case Key.Down:
							if (this._selectedContainer == null && this.FocusFirstItem())
							{
								e.Handled = true;
								return;
							}
							break;
						default:
							if (key != Key.Multiply)
							{
								return;
							}
							if (this.ExpandSubtree(this._selectedContainer))
							{
								e.Handled = true;
							}
							break;
						}
					}
					else if (TreeView.IsShiftKeyDown && base.IsKeyboardFocusWithin && this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous)))
					{
						e.Handled = true;
						return;
					}
				}
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x060058EA RID: 22762 RVA: 0x0013E253 File Offset: 0x0013C453
		private static bool IsControlKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x060058EB RID: 22763 RVA: 0x0018994D File Offset: 0x00187B4D
		private static bool IsShiftKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
			}
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x0018995C File Offset: 0x00187B5C
		private bool FocusFirstItem()
		{
			FrameworkElement frameworkElement;
			return base.NavigateToStartInternal(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), true, out frameworkElement);
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x00189984 File Offset: 0x00187B84
		private bool FocusLastItem()
		{
			FrameworkElement frameworkElement;
			return base.NavigateToEndInternal(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), true, out frameworkElement);
		}

		// Token: 0x060058EE RID: 22766 RVA: 0x001899AC File Offset: 0x00187BAC
		private bool HandleScrollKeys(Key key)
		{
			ScrollViewer scrollHost = base.ScrollHost;
			if (scrollHost != null)
			{
				bool flag = base.FlowDirection == FlowDirection.RightToLeft;
				switch (key)
				{
				case Key.Prior:
					if (DoubleUtil.GreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
					{
						scrollHost.PageUp();
					}
					else
					{
						scrollHost.PageLeft();
					}
					return true;
				case Key.Next:
					if (DoubleUtil.GreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
					{
						scrollHost.PageDown();
					}
					else
					{
						scrollHost.PageRight();
					}
					return true;
				case Key.End:
					scrollHost.ScrollToBottom();
					return true;
				case Key.Home:
					scrollHost.ScrollToTop();
					return true;
				case Key.Left:
					if (flag)
					{
						scrollHost.LineRight();
					}
					else
					{
						scrollHost.LineLeft();
					}
					return true;
				case Key.Up:
					scrollHost.LineUp();
					return true;
				case Key.Right:
					if (flag)
					{
						scrollHost.LineLeft();
					}
					else
					{
						scrollHost.LineRight();
					}
					return true;
				case Key.Down:
					scrollHost.LineDown();
					return true;
				}
			}
			return false;
		}

		// Token: 0x060058EF RID: 22767 RVA: 0x00189A8C File Offset: 0x00187C8C
		private bool HandleScrollByPage(KeyEventArgs e)
		{
			IInputElement focusedElement = Keyboard.FocusedElement;
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this._selectedContainer);
			ItemsControl.ItemInfo startingInfo = (itemsControl != null) ? itemsControl.ItemInfoFromContainer(this._selectedContainer) : null;
			FrameworkElement frameworkElement = this._selectedContainer.HeaderElement;
			if (frameworkElement == null)
			{
				frameworkElement = this._selectedContainer;
			}
			return base.NavigateByPage(startingInfo, frameworkElement, (e.Key == Key.Prior) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
		}

		/// <summary>Expands the specified <see cref="T:System.Windows.Controls.TreeViewItem" /> control and all its child <see cref="T:System.Windows.Controls.TreeViewItem" /> elements.</summary>
		/// <param name="container">The <see cref="T:System.Windows.Controls.TreeViewItem" /> to expand.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.TreeViewItem" /> and all its child elements were expanded; otherwise, <see langword="false" />.</returns>
		// Token: 0x060058F0 RID: 22768 RVA: 0x00189AFA File Offset: 0x00187CFA
		protected virtual bool ExpandSubtree(TreeViewItem container)
		{
			if (container != null)
			{
				container.ExpandSubtree();
				return true;
			}
			return false;
		}

		/// <summary>Provides class handling for an <see cref="E:System.Windows.ContentElement.IsKeyboardFocusWithinChanged" /> event when the keyboard focus changes for a <see cref="T:System.Windows.Controls.TreeView" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060058F1 RID: 22769 RVA: 0x00189B08 File Offset: 0x00187D08
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			bool flag = false;
			bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
			if (isKeyboardFocusWithin)
			{
				flag = true;
			}
			else
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject != null)
				{
					UIElement uielement = KeyboardNavigation.GetVisualRoot(this) as UIElement;
					if (uielement != null && uielement.IsKeyboardFocusWithin && FocusManager.GetFocusScope(dependencyObject) != uielement)
					{
						flag = true;
					}
				}
			}
			if ((bool)base.GetValue(Selector.IsSelectionActiveProperty) != flag)
			{
				base.SetValue(Selector.IsSelectionActivePropertyKey, BooleanBoxes.Box(flag));
			}
			if (isKeyboardFocusWithin && base.IsKeyboardFocused && this._selectedContainer != null && !this._selectedContainer.IsKeyboardFocusWithin)
			{
				this._selectedContainer.Focus();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.UIElement.GotFocus" /> routed event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060058F2 RID: 22770 RVA: 0x00189BAC File Offset: 0x00187DAC
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			if (base.IsKeyboardFocusWithin && base.IsKeyboardFocused && this._selectedContainer != null && !this._selectedContainer.IsKeyboardFocusWithin)
			{
				this._selectedContainer.Focus();
			}
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x00189BE6 File Offset: 0x00187DE6
		private void OnFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.ClearValue(Selector.IsSelectionActivePropertyKey);
			}
		}

		// Token: 0x060058F4 RID: 22772 RVA: 0x00189BFC File Offset: 0x00187DFC
		private static DependencyObject FindParent(DependencyObject o)
		{
			Visual visual = o as Visual;
			ContentElement contentElement = (visual == null) ? (o as ContentElement) : null;
			if (contentElement != null)
			{
				o = ContentOperations.GetParent(contentElement);
				if (o != null)
				{
					return o;
				}
				FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return frameworkContentElement.Parent;
				}
			}
			else if (visual != null)
			{
				return VisualTreeHelper.GetParent(visual);
			}
			return null;
		}

		/// <summary>Defines an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.TreeView" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.TreeViewAutomationPeer" /> for the <see cref="T:System.Windows.Controls.TreeView" /> control.</returns>
		// Token: 0x060058F5 RID: 22773 RVA: 0x00189C49 File Offset: 0x00187E49
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TreeViewAutomationPeer(this);
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x060058F6 RID: 22774 RVA: 0x00189C51 File Offset: 0x00187E51
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TreeView._dType;
			}
		}

		// Token: 0x04002EE7 RID: 12007
		private static readonly DependencyPropertyKey SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItem", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TreeView.SelectedItem" /> dependency property.</returns>
		// Token: 0x04002EE8 RID: 12008
		public static readonly DependencyProperty SelectedItemProperty = TreeView.SelectedItemPropertyKey.DependencyProperty;

		// Token: 0x04002EE9 RID: 12009
		private static readonly DependencyPropertyKey SelectedValuePropertyKey = DependencyProperty.RegisterReadOnly("SelectedValue", typeof(object), typeof(TreeView), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeView.SelectedValue" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="F:System.Windows.Controls.TreeView.SelectedValuePathProperty" /> dependency property.</returns>
		// Token: 0x04002EEA RID: 12010
		public static readonly DependencyProperty SelectedValueProperty = TreeView.SelectedValuePropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeView.SelectedValuePath" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TreeView.SelectedValuePath" /> dependency property.</returns>
		// Token: 0x04002EEB RID: 12011
		public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(TreeView), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(TreeView.OnSelectedValuePathChanged)));

		// Token: 0x04002EED RID: 12013
		private static DependencyObjectType _dType;

		// Token: 0x04002EEE RID: 12014
		private BitVector32 _bits = new BitVector32(0);

		// Token: 0x04002EEF RID: 12015
		private TreeViewItem _selectedContainer;

		// Token: 0x04002EF0 RID: 12016
		private static readonly BindingExpressionUncommonField SelectedValuePathBindingExpression;

		// Token: 0x04002EF1 RID: 12017
		private EventHandler _focusEnterMainFocusScopeEventHandler;

		// Token: 0x020009C7 RID: 2503
		private enum Bits
		{
			// Token: 0x0400458F RID: 17807
			IsSelectionChangeActive = 1
		}
	}
}
