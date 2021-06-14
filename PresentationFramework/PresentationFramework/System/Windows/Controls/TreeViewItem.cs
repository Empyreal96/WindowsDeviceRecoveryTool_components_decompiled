using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Implements a selectable item in a <see cref="T:System.Windows.Controls.TreeView" /> control.</summary>
	// Token: 0x0200054D RID: 1357
	[TemplatePart(Name = "PART_Header", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "ItemsHost", Type = typeof(ItemsPresenter))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeViewItem))]
	public class TreeViewItem : HeaderedItemsControl, IHierarchicalVirtualizationAndScrollInfo
	{
		// Token: 0x060058F7 RID: 22775 RVA: 0x00189C58 File Offset: 0x00187E58
		static TreeViewItem()
		{
			TreeViewItem.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			TreeViewItem.UnselectedEvent = EventManager.RegisterRoutedEvent("Unselected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeViewItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(typeof(TreeViewItem)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			TreeViewItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TreeViewItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			Control.IsTabStopProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TreeViewItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TreeViewItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			EventManager.RegisterClassHandler(typeof(TreeViewItem), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(TreeViewItem.OnRequestBringIntoView));
			EventManager.RegisterClassHandler(typeof(TreeViewItem), Mouse.MouseDownEvent, new MouseButtonEventHandler(TreeViewItem.OnMouseButtonDown), true);
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(TreeViewItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Gets or sets whether the nested items in a <see cref="T:System.Windows.Controls.TreeViewItem" /> are expanded or collapsed.  </summary>
		/// <returns>
		///     <see langword="true" /> if the nested items of a <see cref="T:System.Windows.Controls.TreeViewItem" /> are visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x060058F9 RID: 22777 RVA: 0x00189F05 File Offset: 0x00188105
		// (set) Token: 0x060058FA RID: 22778 RVA: 0x00189F17 File Offset: 0x00188117
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsExpandedProperty);
			}
			set
			{
				base.SetValue(TreeViewItem.IsExpandedProperty, value);
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x060058FB RID: 22779 RVA: 0x00168D5A File Offset: 0x00166F5A
		private bool CanExpand
		{
			get
			{
				return base.HasItems;
			}
		}

		// Token: 0x060058FC RID: 22780 RVA: 0x00189F28 File Offset: 0x00188128
		private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)d;
			bool flag = (bool)e.NewValue;
			TreeView parentTreeView = treeViewItem.ParentTreeView;
			if (parentTreeView != null && !flag)
			{
				parentTreeView.HandleSelectionAndCollapsed(treeViewItem);
			}
			ItemsPresenter itemsHostPresenter = treeViewItem.ItemsHostPresenter;
			if (itemsHostPresenter != null)
			{
				treeViewItem.InvalidateMeasure();
				Helper.InvalidateMeasureOnPath(itemsHostPresenter, treeViewItem, false);
			}
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.FromElement(treeViewItem) as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.RaiseExpandCollapseAutomationEvent((bool)e.OldValue, flag);
			}
			if (flag)
			{
				treeViewItem.OnExpanded(new RoutedEventArgs(TreeViewItem.ExpandedEvent, treeViewItem));
			}
			else
			{
				treeViewItem.OnCollapsed(new RoutedEventArgs(TreeViewItem.CollapsedEvent, treeViewItem));
			}
			treeViewItem.UpdateVisualState();
		}

		/// <summary>Gets or sets whether a <see cref="T:System.Windows.Controls.TreeViewItem" /> control is selected.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.TreeViewItem" /> is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x060058FD RID: 22781 RVA: 0x00189FC7 File Offset: 0x001881C7
		// (set) Token: 0x060058FE RID: 22782 RVA: 0x00189FD9 File Offset: 0x001881D9
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(TreeViewItem.IsSelectedProperty, value);
			}
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x00189FE8 File Offset: 0x001881E8
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)d;
			bool flag = (bool)e.NewValue;
			treeViewItem.Select(flag);
			TreeViewItemAutomationPeer treeViewItemAutomationPeer = UIElementAutomationPeer.FromElement(treeViewItem) as TreeViewItemAutomationPeer;
			if (treeViewItemAutomationPeer != null)
			{
				treeViewItemAutomationPeer.RaiseAutomationIsSelectedChanged(flag);
			}
			if (flag)
			{
				treeViewItem.OnSelected(new RoutedEventArgs(TreeViewItem.SelectedEvent, treeViewItem));
			}
			else
			{
				treeViewItem.OnUnselected(new RoutedEventArgs(TreeViewItem.UnselectedEvent, treeViewItem));
			}
			treeViewItem.UpdateVisualState();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.TreeViewItem" /> has keyboard focus.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.TreeViewItem" /> has keyboard focus; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x06005900 RID: 22784 RVA: 0x0018A053 File Offset: 0x00188253
		[Browsable(false)]
		[Category("Appearance")]
		[ReadOnly(true)]
		public bool IsSelectionActive
		{
			get
			{
				return (bool)base.GetValue(TreeViewItem.IsSelectionActiveProperty);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.TreeViewItem.IsExpanded" /> property changes from <see langword="false" /> to <see langword="true" />.</summary>
		// Token: 0x1400010B RID: 267
		// (add) Token: 0x06005901 RID: 22785 RVA: 0x0018A065 File Offset: 0x00188265
		// (remove) Token: 0x06005902 RID: 22786 RVA: 0x0018A073 File Offset: 0x00188273
		[Category("Behavior")]
		public event RoutedEventHandler Expanded
		{
			add
			{
				base.AddHandler(TreeViewItem.ExpandedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.ExpandedEvent, value);
			}
		}

		/// <summary>Raises an <see cref="E:System.Windows.Controls.TreeViewItem.Expanded" /> event when the <see cref="P:System.Windows.Controls.TreeViewItem.IsExpanded" /> property changes from <see langword="false" /> to <see langword="true" />.</summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06005903 RID: 22787 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnExpanded(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.TreeViewItem.IsExpanded" /> property changes from <see langword="true" /> to <see langword="false" />.</summary>
		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06005904 RID: 22788 RVA: 0x0018A081 File Offset: 0x00188281
		// (remove) Token: 0x06005905 RID: 22789 RVA: 0x0018A08F File Offset: 0x0018828F
		[Category("Behavior")]
		public event RoutedEventHandler Collapsed
		{
			add
			{
				base.AddHandler(TreeViewItem.CollapsedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.CollapsedEvent, value);
			}
		}

		/// <summary>Raises a <see cref="E:System.Windows.Controls.TreeViewItem.Collapsed" /> event when the <see cref="P:System.Windows.Controls.TreeViewItem.IsExpanded" /> property changes from <see langword="true" /> to <see langword="false" />. </summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06005906 RID: 22790 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnCollapsed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelected" /> property of a <see cref="T:System.Windows.Controls.TreeViewItem" /> changes from <see langword="false" /> to <see langword="true" />.</summary>
		// Token: 0x1400010D RID: 269
		// (add) Token: 0x06005907 RID: 22791 RVA: 0x0018A09D File Offset: 0x0018829D
		// (remove) Token: 0x06005908 RID: 22792 RVA: 0x0018A0AB File Offset: 0x001882AB
		[Category("Behavior")]
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(TreeViewItem.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.SelectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.TreeViewItem.Selected" /> routed event when the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelected" /> property changes from <see langword="false" /> to <see langword="true" />.</summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06005909 RID: 22793 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelected" /> property of a <see cref="T:System.Windows.Controls.TreeViewItem" /> changes from <see langword="true" /> to <see langword="false" />.</summary>
		// Token: 0x1400010E RID: 270
		// (add) Token: 0x0600590A RID: 22794 RVA: 0x0018A0B9 File Offset: 0x001882B9
		// (remove) Token: 0x0600590B RID: 22795 RVA: 0x0018A0C7 File Offset: 0x001882C7
		[Category("Behavior")]
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(TreeViewItem.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(TreeViewItem.UnselectedEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.TreeViewItem.Unselected" /> routed event when the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelected" /> property changes from <see langword="true" /> to <see langword="false" />.</summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x0600590C RID: 22796 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Expands the <see cref="T:System.Windows.Controls.TreeViewItem" /> control and all its child <see cref="T:System.Windows.Controls.TreeViewItem" /> elements.</summary>
		// Token: 0x0600590D RID: 22797 RVA: 0x0018A0D5 File Offset: 0x001882D5
		public void ExpandSubtree()
		{
			TreeViewItem.ExpandRecursive(this);
		}

		/// <summary>Arranges the content of the <see cref="T:System.Windows.Controls.TreeViewItem" />.</summary>
		/// <param name="arrangeSize">The final area within the parent that the <see cref="T:System.Windows.Controls.TreeViewItem" /> should use to arrange itself and its children.</param>
		/// <returns>The actual size used by the <see cref="T:System.Windows.Controls.TreeViewItem" />.</returns>
		// Token: 0x0600590E RID: 22798 RVA: 0x0018A0DD File Offset: 0x001882DD
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			arrangeSize = base.ArrangeOverride(arrangeSize);
			Helper.ComputeCorrectionFactor(this.ParentTreeView, this, base.ItemsHost, this.HeaderElement);
			return arrangeSize;
		}

		/// <summary>Gets or sets an object that represents the viewport and cache sizes of the <see cref="T:System.Windows.Controls.TreeViewItem" />.</summary>
		/// <returns>An object that represents the viewport and cache sizes of the <see cref="T:System.Windows.Controls.TreeViewItem" />.</returns>
		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600590F RID: 22799 RVA: 0x00160137 File Offset: 0x0015E337
		// (set) Token: 0x06005910 RID: 22800 RVA: 0x00160144 File Offset: 0x0015E344
		HierarchicalVirtualizationConstraints IHierarchicalVirtualizationAndScrollInfo.Constraints
		{
			get
			{
				return GroupItem.HierarchicalVirtualizationConstraintsField.GetValue(this);
			}
			set
			{
				if (value.CacheLengthUnit == VirtualizationCacheLengthUnit.Page)
				{
					throw new InvalidOperationException(SR.Get("PageCacheSizeNotAllowed"));
				}
				GroupItem.HierarchicalVirtualizationConstraintsField.SetValue(this, value);
			}
		}

		/// <summary>Gets an object that represents the desired size of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />, in pixels and in logical units.</summary>
		/// <returns>An object that represents the desired size of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />, in pixels and in logical units.</returns>
		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x06005911 RID: 22801 RVA: 0x0018A104 File Offset: 0x00188304
		HierarchicalVirtualizationHeaderDesiredSizes IHierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes
		{
			get
			{
				FrameworkElement headerElement = this.HeaderElement;
				Size pixelSize = (base.IsVisible && headerElement != null) ? headerElement.DesiredSize : default(Size);
				Helper.ApplyCorrectionFactorToPixelHeaderSize(this.ParentTreeView, this, base.ItemsHost, ref pixelSize);
				Size logicalSize = new Size((double)(DoubleUtil.GreaterThan(pixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(pixelSize.Height, 0.0) ? 1 : 0));
				return new HierarchicalVirtualizationHeaderDesiredSizes(logicalSize, pixelSize);
			}
		}

		/// <summary>Gets or sets an object that represents the desired size of the control's items, in pixels and in logical units.</summary>
		/// <returns>An object that represents the desired size of the control's items, in pixels and in logical units.</returns>
		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06005912 RID: 22802 RVA: 0x0018A18F File Offset: 0x0018838F
		// (set) Token: 0x06005913 RID: 22803 RVA: 0x00160202 File Offset: 0x0015E402
		HierarchicalVirtualizationItemDesiredSizes IHierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes
		{
			get
			{
				return Helper.ApplyCorrectionFactorToItemDesiredSizes(this, base.ItemsHost);
			}
			set
			{
				GroupItem.HierarchicalVirtualizationItemDesiredSizesField.SetValue(this, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Panel" /> that displays the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> of the <see cref="T:System.Windows.Controls.TreeViewItem" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Panel" /> that displays the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> of the <see cref="T:System.Windows.Controls.TreeViewItem" />.</returns>
		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06005914 RID: 22804 RVA: 0x0018A19D File Offset: 0x0018839D
		Panel IHierarchicalVirtualizationAndScrollInfo.ItemsHost
		{
			get
			{
				return base.ItemsHost;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items.</summary>
		/// <returns>
		///     <see langword="true" /> if the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items; otherwise, <see langword="false" />.</returns>
		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06005915 RID: 22805 RVA: 0x00160218 File Offset: 0x0015E418
		// (set) Token: 0x06005916 RID: 22806 RVA: 0x00160225 File Offset: 0x0015E425
		bool IHierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization
		{
			get
			{
				return GroupItem.MustDisableVirtualizationField.GetValue(this);
			}
			set
			{
				GroupItem.MustDisableVirtualizationField.SetValue(this, value);
			}
		}

		/// <summary>Gets a value that indicates whether the control's layout pass occurs at a lower priority.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's layout pass occurs at a lower priority; otherwise, <see langword="false" />.</returns>
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06005917 RID: 22807 RVA: 0x00160233 File Offset: 0x0015E433
		// (set) Token: 0x06005918 RID: 22808 RVA: 0x00160240 File Offset: 0x0015E440
		bool IHierarchicalVirtualizationAndScrollInfo.InBackgroundLayout
		{
			get
			{
				return GroupItem.InBackgroundLayoutField.GetValue(this);
			}
			set
			{
				GroupItem.InBackgroundLayoutField.SetValue(this, value);
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06005919 RID: 22809 RVA: 0x0018A1A8 File Offset: 0x001883A8
		internal TreeView ParentTreeView
		{
			get
			{
				for (ItemsControl itemsControl = this.ParentItemsControl; itemsControl != null; itemsControl = ItemsControl.ItemsControlFromItemContainer(itemsControl))
				{
					TreeView treeView = itemsControl as TreeView;
					if (treeView != null)
					{
						return treeView;
					}
				}
				return null;
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x0600591A RID: 22810 RVA: 0x0018A1D5 File Offset: 0x001883D5
		internal TreeViewItem ParentTreeViewItem
		{
			get
			{
				return this.ParentItemsControl as TreeViewItem;
			}
		}

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x0600591B RID: 22811 RVA: 0x0018A1E2 File Offset: 0x001883E2
		internal ItemsControl ParentItemsControl
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this);
			}
		}

		/// <summary>Responds to a change in the visual parent of a <see cref="T:System.Windows.Controls.TreeViewItem" />.</summary>
		/// <param name="oldParent">The previous visual parent.</param>
		// Token: 0x0600591C RID: 22812 RVA: 0x0018A1EA File Offset: 0x001883EA
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			if (VisualTreeHelper.GetParent(this) != null && this.IsSelected)
			{
				this.Select(true);
			}
			base.OnVisualParentChanged(oldParent);
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x0018A20C File Offset: 0x0018840C
		private void Select(bool selected)
		{
			TreeView parentTreeView = this.ParentTreeView;
			ItemsControl parentItemsControl = this.ParentItemsControl;
			if (parentTreeView != null && parentItemsControl != null && !parentTreeView.IsSelectionChangeActive)
			{
				object itemOrContainerFromContainer = parentItemsControl.GetItemOrContainerFromContainer(this);
				parentTreeView.ChangeSelection(itemOrContainerFromContainer, this, selected);
				if (selected && parentTreeView.IsKeyboardFocusWithin && !base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x0600591E RID: 22814 RVA: 0x0018A260 File Offset: 0x00188460
		// (set) Token: 0x0600591F RID: 22815 RVA: 0x0018A26D File Offset: 0x0018846D
		private bool ContainsSelection
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.ContainsSelection);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.ContainsSelection, value);
			}
		}

		// Token: 0x06005920 RID: 22816 RVA: 0x0018A27C File Offset: 0x0018847C
		internal void UpdateContainsSelection(bool selected)
		{
			for (TreeViewItem parentTreeViewItem = this.ParentTreeViewItem; parentTreeViewItem != null; parentTreeViewItem = parentTreeViewItem.ParentTreeViewItem)
			{
				parentTreeViewItem.ContainsSelection = selected;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.GotFocus" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005921 RID: 22817 RVA: 0x0018A2A3 File Offset: 0x001884A3
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			this.Select(true);
			base.OnGotFocus(e);
		}

		/// <summary>Provides class handling for a <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005922 RID: 22818 RVA: 0x0018A2B4 File Offset: 0x001884B4
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled && base.IsEnabled)
			{
				bool isFocused = base.IsFocused;
				if (base.Focus())
				{
					if (isFocused && !this.IsSelected)
					{
						this.Select(true);
					}
					e.Handled = true;
				}
				if (e.ClickCount % 2 == 0)
				{
					base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.Box(!this.IsExpanded));
					e.Handled = true;
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for a <see cref="E:System.Windows.UIElement.KeyDown" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005923 RID: 22819 RVA: 0x0018A32C File Offset: 0x0018852C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				Key key = e.Key;
				switch (key)
				{
				case Key.Left:
				case Key.Right:
					if (this.LogicalLeft(e.Key))
					{
						if (!TreeViewItem.IsControlKeyDown && this.CanExpandOnInput && this.IsExpanded)
						{
							if (base.IsFocused)
							{
								base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.FalseBox);
							}
							else
							{
								base.Focus();
							}
							e.Handled = true;
							return;
						}
					}
					else if (!TreeViewItem.IsControlKeyDown && this.CanExpandOnInput)
					{
						if (!this.IsExpanded)
						{
							base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
							e.Handled = true;
							return;
						}
						if (this.HandleDownKey(e))
						{
							e.Handled = true;
							return;
						}
					}
					break;
				case Key.Up:
					if (!TreeViewItem.IsControlKeyDown && this.HandleUpKey(e))
					{
						e.Handled = true;
					}
					break;
				case Key.Down:
					if (!TreeViewItem.IsControlKeyDown && this.HandleDownKey(e))
					{
						e.Handled = true;
						return;
					}
					break;
				default:
					if (key != Key.Add)
					{
						if (key != Key.Subtract)
						{
							return;
						}
						if (this.CanExpandOnInput && this.IsExpanded)
						{
							base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.FalseBox);
							e.Handled = true;
							return;
						}
					}
					else if (this.CanExpandOnInput && !this.IsExpanded)
					{
						base.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
						e.Handled = true;
						return;
					}
					break;
				}
			}
		}

		// Token: 0x06005924 RID: 22820 RVA: 0x0018A498 File Offset: 0x00188698
		private bool LogicalLeft(Key key)
		{
			bool flag = base.FlowDirection == FlowDirection.RightToLeft;
			return (!flag && key == Key.Left) || (flag && key == Key.Right);
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x06005925 RID: 22821 RVA: 0x0013E253 File Offset: 0x0013C453
		private static bool IsControlKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x06005926 RID: 22822 RVA: 0x0018A4C3 File Offset: 0x001886C3
		private bool CanExpandOnInput
		{
			get
			{
				return this.CanExpand && base.IsEnabled;
			}
		}

		// Token: 0x06005927 RID: 22823 RVA: 0x0018A4D5 File Offset: 0x001886D5
		internal bool HandleUpKey(KeyEventArgs e)
		{
			return this.HandleUpDownKey(true, e);
		}

		// Token: 0x06005928 RID: 22824 RVA: 0x0018A4DF File Offset: 0x001886DF
		internal bool HandleDownKey(KeyEventArgs e)
		{
			return this.HandleUpDownKey(false, e);
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x0018A4EC File Offset: 0x001886EC
		private bool HandleUpDownKey(bool up, KeyEventArgs e)
		{
			FocusNavigationDirection direction = up ? FocusNavigationDirection.Up : FocusNavigationDirection.Down;
			if (this.AllowHandleKeyEvent(direction))
			{
				TreeView parentTreeView = this.ParentTreeView;
				IInputElement focusedElement = Keyboard.FocusedElement;
				if (parentTreeView != null)
				{
					FrameworkElement frameworkElement = this.HeaderElement;
					if (frameworkElement == null)
					{
						frameworkElement = this;
					}
					ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
					ItemsControl.ItemInfo startingInfo = (itemsControl != null) ? itemsControl.ItemInfoFromContainer(this) : null;
					return parentTreeView.NavigateByLine(startingInfo, frameworkElement, direction, new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
				}
			}
			return false;
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x0018A55C File Offset: 0x0018875C
		private bool AllowHandleKeyEvent(FocusNavigationDirection direction)
		{
			if (!this.IsSelected)
			{
				return false;
			}
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			if (dependencyObject != null)
			{
				DependencyObject dependencyObject2 = UIElementHelper.PredictFocus(dependencyObject, direction);
				if (dependencyObject2 != dependencyObject)
				{
					while (dependencyObject2 != null)
					{
						TreeViewItem treeViewItem = dependencyObject2 as TreeViewItem;
						if (treeViewItem == this)
						{
							return false;
						}
						if (treeViewItem != null || dependencyObject2 is TreeView)
						{
							return true;
						}
						dependencyObject2 = KeyboardNavigation.GetParent(dependencyObject2);
					}
				}
			}
			return true;
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x0018A5B4 File Offset: 0x001887B4
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem treeViewItem = (TreeViewItem)sender;
			TreeView parentTreeView = treeViewItem.ParentTreeView;
			if (parentTreeView != null)
			{
				parentTreeView.HandleMouseButtonDown();
			}
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x0018A5D8 File Offset: 0x001887D8
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
		{
			if (e.TargetObject == sender)
			{
				((TreeViewItem)sender).HandleBringIntoView(e);
			}
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x0018A5F0 File Offset: 0x001887F0
		private void HandleBringIntoView(RequestBringIntoViewEventArgs e)
		{
			for (TreeViewItem parentTreeViewItem = this.ParentTreeViewItem; parentTreeViewItem != null; parentTreeViewItem = parentTreeViewItem.ParentTreeViewItem)
			{
				if (!parentTreeViewItem.IsExpanded)
				{
					parentTreeViewItem.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
				}
			}
			if (e.TargetRect.IsEmpty)
			{
				FrameworkElement headerElement = this.HeaderElement;
				if (headerElement != null)
				{
					e.Handled = true;
					headerElement.BringIntoView();
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.BringItemIntoView), null);
			}
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x0018A66C File Offset: 0x0018886C
		private object BringItemIntoView(object args)
		{
			FrameworkElement headerElement = this.HeaderElement;
			if (headerElement != null)
			{
				headerElement.BringIntoView();
			}
			return null;
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x0600592F RID: 22831 RVA: 0x0018A68A File Offset: 0x0018888A
		internal FrameworkElement HeaderElement
		{
			get
			{
				return base.GetTemplateChild("PART_Header") as FrameworkElement;
			}
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x0018A69C File Offset: 0x0018889C
		internal FrameworkElement TryGetHeaderElement()
		{
			FrameworkElement frameworkElement = this.HeaderElement;
			if (frameworkElement != null)
			{
				return frameworkElement;
			}
			FrameworkTemplate templateInternal = this.TemplateInternal;
			if (templateInternal == null)
			{
				return this;
			}
			int i = StyleHelper.QueryChildIndexFromChildName("PART_Header", templateInternal.ChildIndexFromChildName);
			if (i < 0)
			{
				ToggleButton toggleButton = Helper.FindTemplatedDescendant<ToggleButton>(this, this);
				if (toggleButton != null)
				{
					FrameworkElement frameworkElement2 = VisualTreeHelper.GetParent(toggleButton) as FrameworkElement;
					if (frameworkElement2 != null)
					{
						int childrenCount = VisualTreeHelper.GetChildrenCount(frameworkElement2);
						i = 0;
						while (i < childrenCount - 1)
						{
							if (VisualTreeHelper.GetChild(frameworkElement2, i) == toggleButton)
							{
								frameworkElement = (VisualTreeHelper.GetChild(frameworkElement2, i + 1) as FrameworkElement);
								if (frameworkElement != null)
								{
									return frameworkElement;
								}
								break;
							}
							else
							{
								i++;
							}
						}
					}
				}
			}
			return this;
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06005931 RID: 22833 RVA: 0x0018A72B File Offset: 0x0018892B
		private ItemsPresenter ItemsHostPresenter
		{
			get
			{
				return base.GetTemplateChild("ItemsHost") as ItemsPresenter;
			}
		}

		/// <summary>Determines whether an object is a <see cref="T:System.Windows.Controls.TreeViewItem" />.</summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="item" /> is a <see cref="T:System.Windows.Controls.TreeViewItem" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005932 RID: 22834 RVA: 0x001896DB File Offset: 0x001878DB
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeViewItem;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Controls.TreeViewItem" /> to use to display the object.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.TreeViewItem" /> to use to display the object.</returns>
		// Token: 0x06005933 RID: 22835 RVA: 0x001896E6 File Offset: 0x001878E6
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeViewItem();
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x0018A73D File Offset: 0x0018893D
		internal void PrepareItemContainer(object item, ItemsControl parentItemsControl)
		{
			Helper.ClearVirtualizingElement(this);
			TreeViewItem.IsVirtualizingPropagationHelper(parentItemsControl, this);
			if (VirtualizingPanel.GetIsVirtualizing(parentItemsControl))
			{
				Helper.SetItemValuesOnContainer(parentItemsControl, this, item);
			}
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x0018A75C File Offset: 0x0018895C
		internal void ClearItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (VirtualizingPanel.GetIsVirtualizing(parentItemsControl))
			{
				Helper.StoreItemValues(parentItemsControl, this, item);
				VirtualizingPanel virtualizingPanel = base.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.OnClearChildrenInternal();
				}
				base.ItemContainerGenerator.RemoveAllInternal(true);
			}
			this.ContainsSelection = false;
		}

		// Token: 0x06005936 RID: 22838 RVA: 0x0018A7A1 File Offset: 0x001889A1
		internal static void IsVirtualizingPropagationHelper(DependencyObject parent, DependencyObject element)
		{
			TreeViewItem.SynchronizeValue(VirtualizingPanel.IsVirtualizingProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.VirtualizationModeProperty, parent, element);
			TreeViewItem.SynchronizeValue(VirtualizingPanel.ScrollUnitProperty, parent, element);
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x0018A7D4 File Offset: 0x001889D4
		internal static void SynchronizeValue(DependencyProperty dp, DependencyObject parent, DependencyObject child)
		{
			object value = parent.GetValue(dp);
			child.SetValue(dp, value);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event that occurs when there is a change in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005938 RID: 22840 RVA: 0x0018A7F4 File Offset: 0x001889F4
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Remove:
			case NotifyCollectionChangedAction.Reset:
				if (this.ContainsSelection)
				{
					TreeView parentTreeView = this.ParentTreeView;
					if (parentTreeView != null && !parentTreeView.IsSelectedContainerHookedUp)
					{
						this.ContainsSelection = false;
						this.Select(true);
						return;
					}
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (this.ContainsSelection)
				{
					TreeView parentTreeView2 = this.ParentTreeView;
					if (parentTreeView2 != null)
					{
						object selectedItem = parentTreeView2.SelectedItem;
						if (selectedItem != null && selectedItem.Equals(e.OldItems[0]))
						{
							parentTreeView2.ChangeSelection(selectedItem, parentTreeView2.SelectedContainer, false);
							return;
						}
					}
				}
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x0018A8B4 File Offset: 0x00188AB4
		private static void ExpandRecursive(TreeViewItem item)
		{
			if (item == null)
			{
				return;
			}
			if (!item.IsExpanded)
			{
				item.SetCurrentValueInternal(TreeViewItem.IsExpandedProperty, BooleanBoxes.TrueBox);
			}
			item.ApplyTemplate();
			ItemsPresenter itemsPresenter = (ItemsPresenter)item.Template.FindName("ItemsHost", item);
			if (itemsPresenter != null)
			{
				itemsPresenter.ApplyTemplate();
			}
			else
			{
				item.UpdateLayout();
			}
			VirtualizingPanel virtualizingPanel = item.ItemsHost as VirtualizingPanel;
			item.ItemsHost.EnsureGenerator();
			int i = 0;
			int count = item.Items.Count;
			while (i < count)
			{
				TreeViewItem treeViewItem;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(i);
					treeViewItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
				}
				else
				{
					treeViewItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
					treeViewItem.BringIntoView();
				}
				if (treeViewItem != null)
				{
					TreeViewItem.ExpandRecursive(treeViewItem);
				}
				i++;
			}
		}

		/// <summary>Defines an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.TreeViewItem" />. </summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.TreeViewItemAutomationPeer" /> object for the <see cref="T:System.Windows.Controls.TreeViewItem" />.</returns>
		// Token: 0x0600593A RID: 22842 RVA: 0x0018A980 File Offset: 0x00188B80
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TreeViewItemAutomationPeer(this);
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x0600593B RID: 22843 RVA: 0x0018A988 File Offset: 0x00188B88
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TreeViewItem._dType;
			}
		}

		// Token: 0x0600593C RID: 22844 RVA: 0x0018A990 File Offset: 0x00188B90
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
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
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
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unfocused"
				});
			}
			if (this.IsExpanded)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Expanded"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Collapsed"
				});
			}
			if (base.HasItems)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"HasItems"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"NoItems"
				});
			}
			if (this.IsSelected)
			{
				if (this.IsSelectionActive)
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"Selected"
					});
				}
				else
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SelectedInactive",
						"Selected"
					});
				}
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unselected"
				});
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeViewItem.IsExpanded" /> dependency property. </summary>
		// Token: 0x04002EF2 RID: 12018
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(TreeViewItem.OnIsExpandedChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelected" /> dependency property. </summary>
		// Token: 0x04002EF3 RID: 12019
		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TreeViewItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TreeViewItem.OnIsSelectedChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TreeViewItem.IsSelectionActive" /> dependency property. </summary>
		// Token: 0x04002EF4 RID: 12020
		public static readonly DependencyProperty IsSelectionActiveProperty = Selector.IsSelectionActiveProperty.AddOwner(typeof(TreeViewItem));

		// Token: 0x04002EF9 RID: 12025
		private static DependencyObjectType _dType;

		// Token: 0x04002EFA RID: 12026
		private const string HeaderPartName = "PART_Header";

		// Token: 0x04002EFB RID: 12027
		private const string ItemsHostPartName = "ItemsHost";
	}
}
