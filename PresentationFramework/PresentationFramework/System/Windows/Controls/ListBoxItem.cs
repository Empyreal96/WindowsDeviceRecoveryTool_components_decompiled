using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a selectable item in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
	// Token: 0x020004FC RID: 1276
	[DefaultEvent("Selected")]
	public class ListBoxItem : ContentControl
	{
		// Token: 0x06005154 RID: 20820 RVA: 0x0016CFF0 File Offset: 0x0016B1F0
		static ListBoxItem()
		{
			ListBoxItem.SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(ListBoxItem));
			ListBoxItem.UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(ListBoxItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(typeof(ListBoxItem)));
			ListBoxItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ListBoxItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected.  </summary>
		/// <returns>
		///     <see langword="true" /> if the item is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x0016D166 File Offset: 0x0016B366
		// (set) Token: 0x06005156 RID: 20822 RVA: 0x0016D178 File Offset: 0x0016B378
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(ListBoxItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(ListBoxItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x0016D18C File Offset: 0x0016B38C
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = d as ListBoxItem;
			bool flag = (bool)e.NewValue;
			Selector parentSelector = listBoxItem.ParentSelector;
			if (parentSelector != null)
			{
				parentSelector.RaiseIsSelectedChangedAutomationEvent(listBoxItem, flag);
			}
			if (flag)
			{
				listBoxItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, listBoxItem));
			}
			else
			{
				listBoxItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, listBoxItem));
			}
			listBoxItem.UpdateVisualState();
		}

		/// <summary>Called when the <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected in a <see cref="T:System.Windows.Controls.ListBox" />.  </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005158 RID: 20824 RVA: 0x0016D1EC File Offset: 0x0016B3EC
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(true, e);
		}

		/// <summary>Called when the <see cref="T:System.Windows.Controls.ListBoxItem" /> is unselected in a <see cref="T:System.Windows.Controls.ListBox" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005159 RID: 20825 RVA: 0x0016D1F6 File Offset: 0x0016B3F6
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(false, e);
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x0016D200 File Offset: 0x0016B400
		private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ListBoxItem" /> is selected.</summary>
		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x0600515B RID: 20827 RVA: 0x0016D209 File Offset: 0x0016B409
		// (remove) Token: 0x0600515C RID: 20828 RVA: 0x0016D217 File Offset: 0x0016B417
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(ListBoxItem.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.SelectedEvent, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ListBoxItem" /> is unselected.</summary>
		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x0600515D RID: 20829 RVA: 0x0016D225 File Offset: 0x0016B425
		// (remove) Token: 0x0600515E RID: 20830 RVA: 0x0016D233 File Offset: 0x0016B433
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(ListBoxItem.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.UnselectedEvent, value);
			}
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x0016D244 File Offset: 0x0016B444
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, (base.Content is Control) ? "Normal" : "Disabled", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (this.IsSelected)
			{
				if (Selector.GetIsSelectionActive(this))
				{
					VisualStateManager.GoToState(this, "Selected", useTransitions);
				}
				else
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SelectedUnfocused",
						"Selected"
					});
				}
			}
			else
			{
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ListBoxItemAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06005160 RID: 20832 RVA: 0x0016D316 File Offset: 0x0016B516
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ListBoxItemWrapperAutomationPeer(this);
		}

		/// <summary>Called when the user presses the right mouse button over the <see cref="T:System.Windows.Controls.ListBoxItem" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005161 RID: 20833 RVA: 0x0016D31E File Offset: 0x0016B51E
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Left);
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Called when the user presses the right mouse button over a <see cref="T:System.Windows.Controls.ListBoxItem" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005162 RID: 20834 RVA: 0x0016D33D File Offset: 0x0016B53D
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Right);
			}
			base.OnMouseRightButtonDown(e);
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x0016D35C File Offset: 0x0016B55C
		private void HandleMouseButtonDown(MouseButton mouseButton)
		{
			if (Selector.UiGetIsSelectable(this) && base.Focus())
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null)
				{
					parentListBox.NotifyListItemClicked(this, mouseButton);
				}
			}
		}

		/// <summary>Called when the mouse enters a <see cref="T:System.Windows.Controls.ListBoxItem" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005164 RID: 20836 RVA: 0x0016D38C File Offset: 0x0016B58C
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			if (base.IsMouseOver)
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null && Mouse.LeftButton == MouseButtonState.Pressed)
				{
					parentListBox.NotifyListItemMouseDragged(this);
				}
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Called when the mouse leaves a <see cref="T:System.Windows.Controls.ListBoxItem" />. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005165 RID: 20837 RVA: 0x0016D3DC File Offset: 0x0016B5DC
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			base.OnMouseLeave(e);
		}

		/// <summary>Called when the visual parent of a list box item changes. </summary>
		/// <param name="oldParent">The previous <see cref="P:System.Windows.FrameworkElement.Parent" /> property of the <see cref="T:System.Windows.Controls.ListBoxItem" />.</param>
		// Token: 0x06005166 RID: 20838 RVA: 0x0016D400 File Offset: 0x0016B600
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			ItemsControl itemsControl = null;
			if (VisualTreeHelper.GetParent(this) == null && base.IsKeyboardFocusWithin)
			{
				itemsControl = ItemsControl.GetItemsOwner(oldParent);
			}
			base.OnVisualParentChanged(oldParent);
			if (itemsControl != null)
			{
				itemsControl.Focus();
			}
		}

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06005167 RID: 20839 RVA: 0x0016D437 File Offset: 0x0016B637
		private ListBox ParentListBox
		{
			get
			{
				return this.ParentSelector as ListBox;
			}
		}

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x0016D444 File Offset: 0x0016B644
		internal Selector ParentSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06005169 RID: 20841 RVA: 0x0016D451 File Offset: 0x0016B651
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ListBoxItem._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListBoxItem.IsSelected" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListBoxItem.IsSelected" /> dependency property.</returns>
		// Token: 0x04002C62 RID: 11362
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ListBoxItem.OnIsSelectedChanged)));

		// Token: 0x04002C65 RID: 11365
		private DispatcherOperation parentNotifyDraggedOperation;

		// Token: 0x04002C66 RID: 11366
		private static DependencyObjectType _dType;
	}
}
