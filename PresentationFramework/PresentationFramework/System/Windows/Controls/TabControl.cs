using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that contains multiple items that share the same space on the screen. </summary>
	// Token: 0x02000539 RID: 1337
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TabItem))]
	[TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
	public class TabControl : Selector
	{
		// Token: 0x060056B0 RID: 22192 RVA: 0x0017FD20 File Offset: 0x0017DF20
		static TabControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));
			TabControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TabControl));
			Control.IsTabStopProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TabControl), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.TabControl);
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x0017FF80 File Offset: 0x0017E180
		private static void OnTabStripPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabControl tabControl = (TabControl)d;
			ItemCollection items = tabControl.Items;
			for (int i = 0; i < items.Count; i++)
			{
				TabItem tabItem = tabControl.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;
				if (tabItem != null)
				{
					tabItem.CoerceValue(TabItem.TabStripPlacementProperty);
				}
			}
		}

		/// <summary>Gets or sets how tab headers align relative to the tab content. </summary>
		/// <returns>The alignment of tab headers relative to tab content. The default is <see cref="F:System.Windows.Controls.Dock.Top" />.</returns>
		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x060056B3 RID: 22195 RVA: 0x0017FFCC File Offset: 0x0017E1CC
		// (set) Token: 0x060056B4 RID: 22196 RVA: 0x0017FFDE File Offset: 0x0017E1DE
		[Bindable(true)]
		[Category("Behavior")]
		public Dock TabStripPlacement
		{
			get
			{
				return (Dock)base.GetValue(TabControl.TabStripPlacementProperty);
			}
			set
			{
				base.SetValue(TabControl.TabStripPlacementProperty, value);
			}
		}

		/// <summary>Gets the content of the currently selected <see cref="T:System.Windows.Controls.TabItem" />.</summary>
		/// <returns>The content of the currently selected <see cref="T:System.Windows.Controls.TabItem" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x060056B5 RID: 22197 RVA: 0x0017FFF1 File Offset: 0x0017E1F1
		// (set) Token: 0x060056B6 RID: 22198 RVA: 0x0017FFFE File Offset: 0x0017E1FE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedContent
		{
			get
			{
				return base.GetValue(TabControl.SelectedContentProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentPropertyKey, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.DataTemplate" /> of the currently selected item.</summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> of the selected item.</returns>
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x060056B7 RID: 22199 RVA: 0x0018000C File Offset: 0x0017E20C
		// (set) Token: 0x060056B8 RID: 22200 RVA: 0x0018001E File Offset: 0x0017E21E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplate SelectedContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(TabControl.SelectedContentTemplateProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentTemplatePropertyKey, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.DataTemplateSelector" /> of the currently selected item. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> of the currently selected item. The default is <see langword="null" />.</returns>
		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x060056B9 RID: 22201 RVA: 0x0018002C File Offset: 0x0017E22C
		// (set) Token: 0x060056BA RID: 22202 RVA: 0x0018003E File Offset: 0x0017E23E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector SelectedContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(TabControl.SelectedContentTemplateSelectorProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentTemplateSelectorPropertyKey, value);
			}
		}

		/// <summary>Gets a composite string that specifies how to format the content of the currently selected <see cref="T:System.Windows.Controls.TabItem" /> if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the content of the currently selected <see cref="T:System.Windows.Controls.TabItem" /> if it is displayed as a string.</returns>
		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x060056BB RID: 22203 RVA: 0x0018004C File Offset: 0x0017E24C
		// (set) Token: 0x060056BC RID: 22204 RVA: 0x0018005E File Offset: 0x0017E25E
		public string SelectedContentStringFormat
		{
			get
			{
				return (string)base.GetValue(TabControl.SelectedContentStringFormatProperty);
			}
			internal set
			{
				base.SetValue(TabControl.SelectedContentStringFormatPropertyKey, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.DataTemplate" /> to apply to any <see cref="T:System.Windows.Controls.TabItem" /> that does not have a <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> or <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> property defined. </summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> to apply to any <see cref="T:System.Windows.Controls.TabItem" /> that does not have a <see cref="P:System.Windows.Controls.ContentControl.ContentTemplate" /> or <see cref="P:System.Windows.Controls.ContentControl.ContentTemplateSelector" /> property defined. The default is <see langword="null" />.</returns>
		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x060056BD RID: 22205 RVA: 0x0018006C File Offset: 0x0017E26C
		// (set) Token: 0x060056BE RID: 22206 RVA: 0x0018007E File Offset: 0x0017E27E
		public DataTemplate ContentTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(TabControl.ContentTemplateProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.DataTemplateSelector" /> that provides custom logic for choosing the template that is used to display the content of the control.</summary>
		/// <returns>A <see cref="P:System.Windows.Controls.TabControl.ContentTemplateSelector" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x060056BF RID: 22207 RVA: 0x0018008C File Offset: 0x0017E28C
		// (set) Token: 0x060056C0 RID: 22208 RVA: 0x0018009E File Offset: 0x0017E29E
		public DataTemplateSelector ContentTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(TabControl.ContentTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets a composite string that specifies how to format the contents of the <see cref="T:System.Windows.Controls.TabItem" /> objects if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the contents of the <see cref="T:System.Windows.Controls.TabItem" /> objects if they are displayed as strings.</returns>
		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x060056C1 RID: 22209 RVA: 0x001800AC File Offset: 0x0017E2AC
		// (set) Token: 0x060056C2 RID: 22210 RVA: 0x001800BE File Offset: 0x0017E2BE
		public string ContentStringFormat
		{
			get
			{
				return (string)base.GetValue(TabControl.ContentStringFormatProperty);
			}
			set
			{
				base.SetValue(TabControl.ContentStringFormatProperty, value);
			}
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x0014EA1C File Offset: 0x0014CC1C
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
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.TabControlAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060056C4 RID: 22212 RVA: 0x001800CC File Offset: 0x0017E2CC
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TabControlAutomationPeer(this);
		}

		/// <summary>Called when <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> is set to true.</summary>
		/// <param name="e">Provides data for the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event.</param>
		// Token: 0x060056C5 RID: 22213 RVA: 0x001800D4 File Offset: 0x0017E2D4
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			base.CanSelectMultiple = false;
			base.ItemContainerGenerator.StatusChanged += this.OnGeneratorStatusChanged;
		}

		/// <summary>Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> is called.</summary>
		// Token: 0x060056C6 RID: 22214 RVA: 0x001800FB File Offset: 0x0017E2FB
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateSelectedContent();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.Selector.SelectionChanged" /> routed event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Controls.SelectionChangedEventArgs" />. </param>
		// Token: 0x060056C7 RID: 22215 RVA: 0x0018010C File Offset: 0x0017E30C
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
			{
				base.OnSelectionChanged(e);
				if (base.IsKeyboardFocusWithin)
				{
					TabItem selectedTabItem = this.GetSelectedTabItem();
					if (selectedTabItem != null)
					{
						selectedTabItem.SetFocus();
					}
				}
				this.UpdateSelectedContent();
			}
			else
			{
				bool isKeyboardFocusWithin = base.IsKeyboardFocusWithin;
				this.UpdateSelectedContent();
				if (isKeyboardFocusWithin)
				{
					TabItem selectedTabItem2 = this.GetSelectedTabItem();
					if (selectedTabItem2 != null)
					{
						selectedTabItem2.SetFocus();
					}
				}
				base.OnSelectionChanged(e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				TabControlAutomationPeer tabControlAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this) as TabControlAutomationPeer;
				if (tabControlAutomationPeer != null)
				{
					tabControlAutomationPeer.RaiseSelectionEvents(e);
				}
			}
		}

		/// <summary>Called to update the current selection when items change.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</param>
		// Token: 0x060056C8 RID: 22216 RVA: 0x001801A8 File Offset: 0x0017E3A8
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Remove && base.SelectedIndex == -1)
			{
				int num = e.OldStartingIndex + 1;
				if (num > base.Items.Count)
				{
					num = 0;
				}
				TabItem tabItem = this.FindNextTabItem(num, -1);
				if (tabItem != null)
				{
					tabItem.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.KeyDown" /> routed event that occurs when the user presses a key.</summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.KeyEventArgs" />.</param>
		// Token: 0x060056C9 RID: 22217 RVA: 0x00180204 File Offset: 0x0017E404
		protected override void OnKeyDown(KeyEventArgs e)
		{
			int direction = 0;
			int startIndex = -1;
			Key key = e.Key;
			if (key != Key.Tab)
			{
				if (key != Key.End)
				{
					if (key == Key.Home)
					{
						direction = 1;
						startIndex = -1;
					}
				}
				else
				{
					direction = -1;
					startIndex = base.Items.Count;
				}
			}
			else if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				startIndex = base.ItemContainerGenerator.IndexFromContainer(base.ItemContainerGenerator.ContainerFromItem(base.SelectedItem));
				if ((e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
				{
					direction = -1;
				}
				else
				{
					direction = 1;
				}
			}
			TabItem tabItem = this.FindNextTabItem(startIndex, direction);
			if (tabItem != null && tabItem != base.SelectedItem)
			{
				e.Handled = tabItem.SetFocus();
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x001802B8 File Offset: 0x0017E4B8
		private TabItem FindNextTabItem(int startIndex, int direction)
		{
			TabItem result = null;
			if (direction != 0)
			{
				int num = startIndex;
				for (int i = 0; i < base.Items.Count; i++)
				{
					num += direction;
					if (num >= base.Items.Count)
					{
						num = 0;
					}
					else if (num < 0)
					{
						num = base.Items.Count - 1;
					}
					TabItem tabItem = base.ItemContainerGenerator.ContainerFromIndex(num) as TabItem;
					if (tabItem != null && tabItem.IsEnabled && tabItem.Visibility == Visibility.Visible)
					{
						result = tabItem;
						break;
					}
				}
			}
			return result;
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own <see langword="ItemContainer" />. </summary>
		/// <param name="item">Specified item.</param>
		/// <returns>Returns <see langword="true" /> if the item is its own <see langword="ItemContainer" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060056CB RID: 22219 RVA: 0x00180334 File Offset: 0x0017E534
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TabItem;
		}

		/// <summary>Creates or identifies the element used to display the specified item.</summary>
		/// <returns>The element used to display the specified item.</returns>
		// Token: 0x060056CC RID: 22220 RVA: 0x0018033F File Offset: 0x0017E53F
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TabItem();
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x060056CD RID: 22221 RVA: 0x00180346 File Offset: 0x0017E546
		internal ContentPresenter SelectedContentPresenter
		{
			get
			{
				return base.GetTemplateChild("PART_SelectedContentHost") as ContentPresenter;
			}
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x00180358 File Offset: 0x0017E558
		private void OnGeneratorStatusChanged(object sender, EventArgs e)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				if (base.HasItems && this._selectedItems.Count == 0)
				{
					base.SetCurrentValueInternal(Selector.SelectedIndexProperty, 0);
				}
				this.UpdateSelectedContent();
			}
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x00180394 File Offset: 0x0017E594
		private TabItem GetSelectedTabItem()
		{
			object selectedItem = base.SelectedItem;
			if (selectedItem != null)
			{
				TabItem tabItem = selectedItem as TabItem;
				if (tabItem == null)
				{
					tabItem = (base.ItemContainerGenerator.ContainerFromIndex(base.SelectedIndex) as TabItem);
					if (tabItem == null || !ItemsControl.EqualsEx(selectedItem, base.ItemContainerGenerator.ItemFromContainer(tabItem)))
					{
						tabItem = (base.ItemContainerGenerator.ContainerFromItem(selectedItem) as TabItem);
					}
				}
				return tabItem;
			}
			return null;
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x001803F8 File Offset: 0x0017E5F8
		private void UpdateSelectedContent()
		{
			if (base.SelectedIndex < 0)
			{
				this.SelectedContent = null;
				this.SelectedContentTemplate = null;
				this.SelectedContentTemplateSelector = null;
				this.SelectedContentStringFormat = null;
				return;
			}
			TabItem selectedTabItem = this.GetSelectedTabItem();
			if (selectedTabItem != null)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(selectedTabItem) as FrameworkElement;
				if (frameworkElement != null)
				{
					KeyboardNavigation.SetTabOnceActiveElement(frameworkElement, selectedTabItem);
					KeyboardNavigation.SetTabOnceActiveElement(this, frameworkElement);
				}
				this.SelectedContent = selectedTabItem.Content;
				ContentPresenter selectedContentPresenter = this.SelectedContentPresenter;
				if (selectedContentPresenter != null)
				{
					selectedContentPresenter.HorizontalAlignment = selectedTabItem.HorizontalContentAlignment;
					selectedContentPresenter.VerticalAlignment = selectedTabItem.VerticalContentAlignment;
				}
				if (selectedTabItem.ContentTemplate != null || selectedTabItem.ContentTemplateSelector != null || selectedTabItem.ContentStringFormat != null)
				{
					this.SelectedContentTemplate = selectedTabItem.ContentTemplate;
					this.SelectedContentTemplateSelector = selectedTabItem.ContentTemplateSelector;
					this.SelectedContentStringFormat = selectedTabItem.ContentStringFormat;
					return;
				}
				this.SelectedContentTemplate = this.ContentTemplate;
				this.SelectedContentTemplateSelector = this.ContentTemplateSelector;
				this.SelectedContentStringFormat = this.ContentStringFormat;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x060056D1 RID: 22225 RVA: 0x001804E4 File Offset: 0x0017E6E4
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TabControl._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.TabStripPlacement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.TabStripPlacement" /> dependency property.</returns>
		// Token: 0x04002E4F RID: 11855
		public static readonly DependencyProperty TabStripPlacementProperty = DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(TabControl), new FrameworkPropertyMetadata(Dock.Top, new PropertyChangedCallback(TabControl.OnTabStripPlacementPropertyChanged)), new ValidateValueCallback(DockPanel.IsValidDock));

		// Token: 0x04002E50 RID: 11856
		private static readonly DependencyPropertyKey SelectedContentPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContent", typeof(object), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.SelectedContent" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.SelectedContent" /> dependency property.</returns>
		// Token: 0x04002E51 RID: 11857
		public static readonly DependencyProperty SelectedContentProperty = TabControl.SelectedContentPropertyKey.DependencyProperty;

		// Token: 0x04002E52 RID: 11858
		private static readonly DependencyPropertyKey SelectedContentTemplatePropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentTemplate", typeof(DataTemplate), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.SelectedContentTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.SelectedContentTemplate" /> dependency property.</returns>
		// Token: 0x04002E53 RID: 11859
		public static readonly DependencyProperty SelectedContentTemplateProperty = TabControl.SelectedContentTemplatePropertyKey.DependencyProperty;

		// Token: 0x04002E54 RID: 11860
		private static readonly DependencyPropertyKey SelectedContentTemplateSelectorPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentTemplateSelector", typeof(DataTemplateSelector), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.SelectedContentTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.SelectedContentTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002E55 RID: 11861
		public static readonly DependencyProperty SelectedContentTemplateSelectorProperty = TabControl.SelectedContentTemplateSelectorPropertyKey.DependencyProperty;

		// Token: 0x04002E56 RID: 11862
		private static readonly DependencyPropertyKey SelectedContentStringFormatPropertyKey = DependencyProperty.RegisterReadOnly("SelectedContentStringFormat", typeof(string), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.SelectedContentStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.SelectedContentStringFormat" /> dependency property.</returns>
		// Token: 0x04002E57 RID: 11863
		public static readonly DependencyProperty SelectedContentStringFormatProperty = TabControl.SelectedContentStringFormatPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.ContentTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.ContentTemplate" /> dependency property.</returns>
		// Token: 0x04002E58 RID: 11864
		public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.ContentTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.ContentTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002E59 RID: 11865
		public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector", typeof(DataTemplateSelector), typeof(TabControl), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.TabControl.ContentStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.TabControl.ContentStringFormat" /> dependency property.</returns>
		// Token: 0x04002E5A RID: 11866
		public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(TabControl), new FrameworkPropertyMetadata(null));

		// Token: 0x04002E5B RID: 11867
		private const string SelectedContentHostTemplateName = "PART_SelectedContentHost";

		// Token: 0x04002E5C RID: 11868
		private static DependencyObjectType _dType;
	}
}
