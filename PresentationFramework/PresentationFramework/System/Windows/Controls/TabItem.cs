using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a selectable item inside a <see cref="T:System.Windows.Controls.TabControl" />. </summary>
	// Token: 0x0200053A RID: 1338
	[DefaultEvent("IsSelectedChanged")]
	public class TabItem : HeaderedContentControl
	{
		// Token: 0x060056D3 RID: 22227 RVA: 0x001804EC File Offset: 0x0017E6EC
		static TabItem()
		{
			EventManager.RegisterClassHandler(typeof(TabItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(TabItem.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
			TabItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TabItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TabItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TabItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.TabItem" /> is selected.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.TabItem" /> is selected; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x060056D4 RID: 22228 RVA: 0x00180675 File Offset: 0x0017E875
		// (set) Token: 0x060056D5 RID: 22229 RVA: 0x00180687 File Offset: 0x0017E887
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(TabItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(TabItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x0018069C File Offset: 0x0017E89C
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabItem tabItem = d as TabItem;
			bool flag = (bool)e.NewValue;
			TabControl tabControlParent = tabItem.TabControlParent;
			if (tabControlParent != null)
			{
				tabControlParent.RaiseIsSelectedChangedAutomationEvent(tabItem, flag);
			}
			if (flag)
			{
				tabItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, tabItem));
			}
			else
			{
				tabItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, tabItem));
			}
			if (flag)
			{
				Binding binding = new Binding("Margin");
				binding.Source = tabItem;
				BindingOperations.SetBinding(tabItem, KeyboardNavigation.DirectionalNavigationMarginProperty, binding);
			}
			else
			{
				BindingOperations.ClearBinding(tabItem, KeyboardNavigation.DirectionalNavigationMarginProperty);
			}
			tabItem.UpdateVisualState();
		}

		/// <summary>Called to indicate that the <see cref="P:System.Windows.Controls.TabItem.IsSelected" /> property has changed to <see langword="true" />. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.Primitives.Selector.Selected" /> event.</param>
		// Token: 0x060056D7 RID: 22231 RVA: 0x0018072B File Offset: 0x0017E92B
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(true, e);
		}

		/// <summary> Called to indicate that the <see cref="P:System.Windows.Controls.TabItem.IsSelected" /> property has changed to <see langword="false" />. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.Primitives.Selector.Unselected" /> event.</param>
		// Token: 0x060056D8 RID: 22232 RVA: 0x00180735 File Offset: 0x0017E935
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(false, e);
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x0016D200 File Offset: 0x0016B400
		private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x00180740 File Offset: 0x0017E940
		private static object CoerceTabStripPlacement(DependencyObject d, object value)
		{
			TabControl tabControlParent = ((TabItem)d).TabControlParent;
			if (tabControlParent == null)
			{
				return value;
			}
			return tabControlParent.TabStripPlacement;
		}

		/// <summary>Gets the tab strip placement.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.Dock" /> values. The default is <see cref="F:System.Windows.Controls.Dock.Top" />. </returns>
		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x060056DB RID: 22235 RVA: 0x00180769 File Offset: 0x0017E969
		public Dock TabStripPlacement
		{
			get
			{
				return (Dock)base.GetValue(TabItem.TabStripPlacementProperty);
			}
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x0018077B File Offset: 0x0017E97B
		internal override void OnAncestorChanged()
		{
			base.CoerceValue(TabItem.TabStripPlacementProperty);
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x00180788 File Offset: 0x0017E988
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
			if (this.IsSelected)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
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

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.TabItemAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060056DE RID: 22238 RVA: 0x0018082F File Offset: 0x0017EA2F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TabItemWrapperAutomationPeer(this);
		}

		/// <summary> Responds to the <see cref="E:System.Windows.ContentElement.MouseLeftButtonDown" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.MouseButtonEventArgs" />.</param>
		// Token: 0x060056DF RID: 22239 RVA: 0x00180837 File Offset: 0x0017EA37
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if ((e.Source == this || !this.IsSelected) && this.SetFocus())
			{
				e.Handled = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary> Announces that the keyboard is focused on this element. </summary>
		/// <param name="e">Keyboard input event arguments.</param>
		// Token: 0x060056E0 RID: 22240 RVA: 0x00180860 File Offset: 0x0017EA60
		protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnPreviewGotKeyboardFocus(e);
			if (!e.Handled && e.NewFocus == this)
			{
				if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
				{
					if (!this.IsSelected && this.TabControlParent != null)
					{
						base.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
						if (e.OldFocus != Keyboard.FocusedElement)
						{
							e.Handled = true;
							return;
						}
						if (this.GetBoolField(TabItem.BoolField.SetFocusOnContent))
						{
							TabControl tabControlParent = this.TabControlParent;
							if (tabControlParent != null)
							{
								ContentPresenter selectedContentPresenter = tabControlParent.SelectedContentPresenter;
								if (selectedContentPresenter != null)
								{
									tabControlParent.UpdateLayout();
									bool flag = selectedContentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
									if (flag)
									{
										e.Handled = true;
										return;
									}
								}
							}
						}
					}
				}
				else
				{
					if (!this.IsSelected && this.TabControlParent != null)
					{
						base.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
						if (e.OldFocus != Keyboard.FocusedElement)
						{
							e.Handled = true;
						}
					}
					if (!e.Handled && this.GetBoolField(TabItem.BoolField.SetFocusOnContent))
					{
						TabControl tabControlParent2 = this.TabControlParent;
						if (tabControlParent2 != null)
						{
							ContentPresenter selectedContentPresenter2 = tabControlParent2.SelectedContentPresenter;
							if (selectedContentPresenter2 != null)
							{
								tabControlParent2.UpdateLayout();
								bool flag2 = selectedContentPresenter2.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
								if (flag2)
								{
									e.Handled = true;
								}
							}
						}
					}
				}
			}
		}

		/// <summary> Responds when an <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for a <see cref="T:System.Windows.Controls.TabControl" /> is called. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.AccessKeyEventArgs" />.</param>
		// Token: 0x060056E1 RID: 22241 RVA: 0x00180994 File Offset: 0x0017EB94
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.SetFocus();
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes. </summary>
		/// <param name="oldContent">Old value of the content property.</param>
		/// <param name="newContent">New value of the content property.</param>
		// Token: 0x060056E2 RID: 22242 RVA: 0x001809A0 File Offset: 0x0017EBA0
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					if (newContent == BindingExpressionBase.DisconnectedItem)
					{
						newContent = null;
					}
					tabControlParent.SelectedContent = newContent;
				}
			}
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.TabControl.ContentTemplate" /> property changes. </summary>
		/// <param name="oldContentTemplate">Old value of the content template property.</param>
		/// <param name="newContentTemplate">New value of the content template property.</param>
		// Token: 0x060056E3 RID: 22243 RVA: 0x001809DC File Offset: 0x0017EBDC
		protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					tabControlParent.SelectedContentTemplate = newContentTemplate;
				}
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.TabControl.ContentTemplateSelector" /> property changes. </summary>
		/// <param name="oldContentTemplateSelector">Old value of the content template selector.</param>
		/// <param name="newContentTemplateSelector">New value of the content template selector.</param>
		// Token: 0x060056E4 RID: 22244 RVA: 0x00180A0C File Offset: 0x0017EC0C
		protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					tabControlParent.SelectedContentTemplateSelector = newContentTemplateSelector;
				}
			}
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x00180A3C File Offset: 0x0017EC3C
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null)
			{
				TabItem tabItem = sender as TabItem;
				if (e.Target == null)
				{
					e.Target = tabItem;
					return;
				}
				if (!tabItem.IsSelected)
				{
					e.Scope = tabItem;
					e.Handled = true;
				}
			}
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x00180A88 File Offset: 0x0017EC88
		internal bool SetFocus()
		{
			bool result = false;
			if (!this.GetBoolField(TabItem.BoolField.SettingFocus))
			{
				TabItem tabItem = Keyboard.FocusedElement as TabItem;
				bool flag = (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent || !base.IsKeyboardFocusWithin) && (tabItem == this || tabItem == null || tabItem.TabControlParent != this.TabControlParent);
				this.SetBoolField(TabItem.BoolField.SettingFocus, true);
				this.SetBoolField(TabItem.BoolField.SetFocusOnContent, flag);
				try
				{
					result = (base.Focus() || flag);
				}
				finally
				{
					this.SetBoolField(TabItem.BoolField.SettingFocus, false);
					this.SetBoolField(TabItem.BoolField.SetFocusOnContent, false);
				}
			}
			return result;
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x060056E7 RID: 22247 RVA: 0x00180B1C File Offset: 0x0017ED1C
		private TabControl TabControlParent
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
			}
		}

		// Token: 0x060056E8 RID: 22248 RVA: 0x00180B29 File Offset: 0x0017ED29
		private bool GetBoolField(TabItem.BoolField field)
		{
			return (this._tabItemBoolFieldStore & field) > TabItem.BoolField.DefaultValue;
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x00180B36 File Offset: 0x0017ED36
		private void SetBoolField(TabItem.BoolField field, bool value)
		{
			if (value)
			{
				this._tabItemBoolFieldStore |= field;
				return;
			}
			this._tabItemBoolFieldStore &= ~field;
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x060056EA RID: 22250 RVA: 0x00180B59 File Offset: 0x0017ED59
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TabItem._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabItem.IsSelected" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabItem.IsSelected" /> dependency property.</returns>
		// Token: 0x04002E5D RID: 11869
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(TabItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TabItem.OnIsSelectedChanged)));

		// Token: 0x04002E5E RID: 11870
		private static readonly DependencyPropertyKey TabStripPlacementPropertyKey = DependencyProperty.RegisterReadOnly("TabStripPlacement", typeof(Dock), typeof(TabItem), new FrameworkPropertyMetadata(Dock.Top, null, new CoerceValueCallback(TabItem.CoerceTabStripPlacement)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabItem.TabStripPlacement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabItem.TabStripPlacement" /> dependency property.</returns>
		// Token: 0x04002E5F RID: 11871
		public static readonly DependencyProperty TabStripPlacementProperty = TabItem.TabStripPlacementPropertyKey.DependencyProperty;

		// Token: 0x04002E60 RID: 11872
		private TabItem.BoolField _tabItemBoolFieldStore;

		// Token: 0x04002E61 RID: 11873
		private static DependencyObjectType _dType;

		// Token: 0x020009BF RID: 2495
		[Flags]
		private enum BoolField
		{
			// Token: 0x0400456D RID: 17773
			SetFocusOnContent = 16,
			// Token: 0x0400456E RID: 17774
			SettingFocus = 32,
			// Token: 0x0400456F RID: 17775
			DefaultValue = 0
		}
	}
}
