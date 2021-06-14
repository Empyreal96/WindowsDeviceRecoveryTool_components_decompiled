using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Provides a container for a group of commands or controls.  </summary>
	// Token: 0x02000544 RID: 1348
	[TemplatePart(Name = "PART_ToolBarPanel", Type = typeof(ToolBarPanel))]
	[TemplatePart(Name = "PART_ToolBarOverflowPanel", Type = typeof(ToolBarOverflowPanel))]
	public class ToolBar : HeaderedItemsControl
	{
		// Token: 0x06005814 RID: 22548 RVA: 0x001865E8 File Offset: 0x001847E8
		static ToolBar()
		{
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ToolBar.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(typeof(ToolBar)));
			ToolBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolBar));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(ToolBar), Mouse.MouseDownEvent, new MouseButtonEventHandler(ToolBar.OnMouseButtonDown), true);
			EventManager.RegisterClassHandler(typeof(ToolBar), ButtonBase.ClickEvent, new RoutedEventHandler(ToolBar._OnClick));
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBar);
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x00186908 File Offset: 0x00184B08
		private static object CoerceOrientation(DependencyObject d, object value)
		{
			ToolBarTray toolBarTray = ((ToolBar)d).ToolBarTray;
			if (toolBarTray == null)
			{
				return value;
			}
			return toolBarTray.Orientation;
		}

		/// <summary> Gets the orientation of the <see cref="T:System.Windows.Controls.ToolBar" />.  </summary>
		/// <returns>The toolbar orientation. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06005817 RID: 22551 RVA: 0x00186931 File Offset: 0x00184B31
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ToolBar.OrientationProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates where the toolbar should be located in the <see cref="T:System.Windows.Controls.ToolBarTray" />.  </summary>
		/// <returns>The band of the <see cref="T:System.Windows.Controls.ToolBarTray" /> in which the toolbar is positioned. The default is 0.</returns>
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06005818 RID: 22552 RVA: 0x00186943 File Offset: 0x00184B43
		// (set) Token: 0x06005819 RID: 22553 RVA: 0x00186955 File Offset: 0x00184B55
		public int Band
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandProperty, value);
			}
		}

		/// <summary>Gets or sets the band index number that indicates the position of the toolbar on the band.  </summary>
		/// <returns>The position of a toolbar on the band of a <see cref="T:System.Windows.Controls.ToolBarTray" />.</returns>
		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x0600581A RID: 22554 RVA: 0x00186968 File Offset: 0x00184B68
		// (set) Token: 0x0600581B RID: 22555 RVA: 0x0018697A File Offset: 0x00184B7A
		public int BandIndex
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandIndexProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandIndexProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ToolBar" /> overflow area is currently visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the overflow area is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x0600581C RID: 22556 RVA: 0x0018698D File Offset: 0x00184B8D
		// (set) Token: 0x0600581D RID: 22557 RVA: 0x0018699F File Offset: 0x00184B9F
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOverflowOpen
		{
			get
			{
				return (bool)base.GetValue(ToolBar.IsOverflowOpenProperty);
			}
			set
			{
				base.SetValue(ToolBar.IsOverflowOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x001869B4 File Offset: 0x00184BB4
		private static object CoerceIsOverflowOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				ToolBar toolBar = (ToolBar)d;
				if (!toolBar.IsLoaded)
				{
					toolBar.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001869E8 File Offset: 0x00184BE8
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			ToolBar toolBar = (ToolBar)d;
			if (!toolBar.IsOverflowOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x00186A0B File Offset: 0x00184C0B
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x00186A1F File Offset: 0x00184C1F
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(ToolBar.IsOverflowOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x00186A3C File Offset: 0x00184C3C
		private static void OnOverflowOpenChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = (ToolBar)element;
			if ((bool)e.NewValue)
			{
				Mouse.Capture(toolBar, CaptureMode.SubTree);
				toolBar.SetFocusOnToolBarOverflowPanel();
			}
			else
			{
				ToolBarOverflowPanel toolBarOverflowPanel = toolBar.ToolBarOverflowPanel;
				if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
				{
					Keyboard.Focus(null);
				}
				if (Mouse.Captured == toolBar)
				{
					Mouse.Capture(null);
				}
			}
			toolBar.CoerceValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x00186AA1 File Offset: 0x00184CA1
		private void SetFocusOnToolBarOverflowPanel()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				if (this.ToolBarOverflowPanel != null)
				{
					if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
					{
						this.ToolBarOverflowPanel.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
					}
					else
					{
						this.ToolBarOverflowPanel.Focus();
					}
				}
				return null;
			}), null);
		}

		/// <summary>Gets a value that indicates whether the toolbar has items that are not visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if there are items on the toolbar that are not visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x06005824 RID: 22564 RVA: 0x00186ABD File Offset: 0x00184CBD
		public bool HasOverflowItems
		{
			get
			{
				return (bool)base.GetValue(ToolBar.HasOverflowItemsProperty);
			}
		}

		// Token: 0x06005825 RID: 22565 RVA: 0x00186ACF File Offset: 0x00184CCF
		internal static void SetIsOverflowItem(DependencyObject element, object value)
		{
			element.SetValue(ToolBar.IsOverflowItemPropertyKey, value);
		}

		/// <summary> Reads the value of the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> property from the specified element. </summary>
		/// <param name="element">The element from which to read the property.</param>
		/// <returns>The value of the property.</returns>
		// Token: 0x06005826 RID: 22566 RVA: 0x00186ADD File Offset: 0x00184CDD
		public static bool GetIsOverflowItem(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolBar.IsOverflowItemProperty);
		}

		// Token: 0x06005827 RID: 22567 RVA: 0x00186B00 File Offset: 0x00184D00
		private static void OnOverflowModeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = ItemsControl.ItemsControlFromItemContainer(element) as ToolBar;
			if (toolBar != null)
			{
				toolBar.InvalidateLayout();
			}
		}

		// Token: 0x06005828 RID: 22568 RVA: 0x00186B24 File Offset: 0x00184D24
		private void InvalidateLayout()
		{
			this._minLength = 0.0;
			this._maxLength = 0.0;
			base.InvalidateMeasure();
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				toolBarPanel.InvalidateMeasure();
			}
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x00186B68 File Offset: 0x00184D68
		private static bool IsValidOverflowMode(object o)
		{
			OverflowMode overflowMode = (OverflowMode)o;
			return overflowMode == OverflowMode.AsNeeded || overflowMode == OverflowMode.Always || overflowMode == OverflowMode.Never;
		}

		/// <summary>Writes the value of the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> property to the specified element. </summary>
		/// <param name="element">The element to write the property to.</param>
		/// <param name="mode">The property value to set.</param>
		// Token: 0x0600582A RID: 22570 RVA: 0x00186B89 File Offset: 0x00184D89
		public static void SetOverflowMode(DependencyObject element, OverflowMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolBar.OverflowModeProperty, mode);
		}

		/// <summary>Reads the value of the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> property from the specified element. </summary>
		/// <param name="element">The element from which to read the property.</param>
		/// <returns>The value of the property.</returns>
		// Token: 0x0600582B RID: 22571 RVA: 0x00186BAA File Offset: 0x00184DAA
		[AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
		public static OverflowMode GetOverflowMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (OverflowMode)element.GetValue(ToolBar.OverflowModeProperty);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x0600582C RID: 22572 RVA: 0x00186BCA File Offset: 0x00184DCA
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToolBarAutomationPeer(this);
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">The element that will display the item.</param>
		/// <param name="item">The item to display.</param>
		// Token: 0x0600582D RID: 22573 RVA: 0x00186BD4 File Offset: 0x00184DD4
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (frameworkElement != null)
			{
				Type type = frameworkElement.GetType();
				ResourceKey resourceKey = null;
				if (type == typeof(Button))
				{
					resourceKey = ToolBar.ButtonStyleKey;
				}
				else if (type == typeof(ToggleButton))
				{
					resourceKey = ToolBar.ToggleButtonStyleKey;
				}
				else if (type == typeof(Separator))
				{
					resourceKey = ToolBar.SeparatorStyleKey;
				}
				else if (type == typeof(CheckBox))
				{
					resourceKey = ToolBar.CheckBoxStyleKey;
				}
				else if (type == typeof(RadioButton))
				{
					resourceKey = ToolBar.RadioButtonStyleKey;
				}
				else if (type == typeof(ComboBox))
				{
					resourceKey = ToolBar.ComboBoxStyleKey;
				}
				else if (type == typeof(TextBox))
				{
					resourceKey = ToolBar.TextBoxStyleKey;
				}
				else if (type == typeof(Menu))
				{
					resourceKey = ToolBar.MenuStyleKey;
				}
				if (resourceKey != null)
				{
					bool flag;
					BaseValueSourceInternal valueSource = frameworkElement.GetValueSource(FrameworkElement.StyleProperty, null, out flag);
					if (valueSource <= BaseValueSourceInternal.ImplicitReference)
					{
						frameworkElement.SetResourceReference(FrameworkElement.StyleProperty, resourceKey);
					}
					frameworkElement.DefaultStyleKey = resourceKey;
				}
			}
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x00186D01 File Offset: 0x00184F01
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this._toolBarPanel = null;
			this._toolBarOverflowPanel = null;
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property changes. </summary>
		/// <param name="e">
		///
		///       The arguments for the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
		// Token: 0x0600582F RID: 22575 RVA: 0x00186D19 File Offset: 0x00184F19
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			this.InvalidateLayout();
			base.OnItemsChanged(e);
		}

		/// <summary> Remeasures a <see cref="T:System.Windows.Controls.ToolBar" />. </summary>
		/// <param name="constraint">The measurement constraints. A <see cref="T:System.Windows.Controls.ToolBar" /> cannot return a size larger than the constraint.</param>
		/// <returns>The size of the <see cref="T:System.Windows.Controls.ToolBar" />.</returns>
		// Token: 0x06005830 RID: 22576 RVA: 0x00186D28 File Offset: 0x00184F28
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				Thickness margin = toolBarPanel.Margin;
				double num;
				if (toolBarPanel.Orientation == Orientation.Horizontal)
				{
					num = Math.Max(0.0, result.Width - toolBarPanel.DesiredSize.Width + margin.Left + margin.Right);
				}
				else
				{
					num = Math.Max(0.0, result.Height - toolBarPanel.DesiredSize.Height + margin.Top + margin.Bottom);
				}
				this._minLength = toolBarPanel.MinLength + num;
				this._maxLength = toolBarPanel.MaxLength + num;
			}
			return result;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when the <see cref="T:System.Windows.Controls.ToolBar" /> loses mouse capture. </summary>
		/// <param name="e">The arguments for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> event.</param>
		// Token: 0x06005831 RID: 22577 RVA: 0x00186DE4 File Offset: 0x00184FE4
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (Mouse.Captured == null)
			{
				this.Close();
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x00186DFA File Offset: 0x00184FFA
		internal ToolBarPanel ToolBarPanel
		{
			get
			{
				if (this._toolBarPanel == null)
				{
					this._toolBarPanel = this.FindToolBarPanel();
				}
				return this._toolBarPanel;
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x00186E18 File Offset: 0x00185018
		private ToolBarPanel FindToolBarPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarPanel");
			ToolBarPanel toolBarPanel = templateChild as ToolBarPanel;
			if (templateChild != null && toolBarPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarPanel;
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x06005834 RID: 22580 RVA: 0x00186E5E File Offset: 0x0018505E
		internal ToolBarOverflowPanel ToolBarOverflowPanel
		{
			get
			{
				if (this._toolBarOverflowPanel == null)
				{
					this._toolBarOverflowPanel = this.FindToolBarOverflowPanel();
				}
				return this._toolBarOverflowPanel;
			}
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x00186E7C File Offset: 0x0018507C
		private ToolBarOverflowPanel FindToolBarOverflowPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarOverflowPanel");
			ToolBarOverflowPanel toolBarOverflowPanel = templateChild as ToolBarOverflowPanel;
			if (templateChild != null && toolBarOverflowPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarOverflowPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarOverflowPanel;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when a key is pressed on an item in the <see cref="T:System.Windows.Controls.ToolBar" />. </summary>
		/// <param name="e">The arguments for the <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x06005836 RID: 22582 RVA: 0x00186EC4 File Offset: 0x001850C4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			UIElement uielement = null;
			UIElement uielement2 = e.Source as UIElement;
			if (uielement2 != null && ItemsControl.ItemsControlFromItemContainer(uielement2) == this)
			{
				Panel panel = VisualTreeHelper.GetParent(uielement2) as Panel;
				if (panel != null)
				{
					Key key = e.Key;
					if (key != Key.Escape)
					{
						if (key != Key.End)
						{
							if (key == Key.Home)
							{
								uielement = (VisualTreeHelper.GetChild(panel, 0) as UIElement);
							}
						}
						else
						{
							uielement = (VisualTreeHelper.GetChild(panel, VisualTreeHelper.GetChildrenCount(panel) - 1) as UIElement);
						}
					}
					else
					{
						ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
						if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
						{
							this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Last));
						}
						else
						{
							Keyboard.Focus(null);
						}
						this.Close();
					}
					if (uielement != null && uielement.Focus())
					{
						e.Handled = true;
					}
				}
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x00186F90 File Offset: 0x00185190
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToolBar toolBar = (ToolBar)sender;
			if (!e.Handled)
			{
				toolBar.Close();
				e.Handled = true;
			}
		}

		// Token: 0x06005838 RID: 22584 RVA: 0x00186FBC File Offset: 0x001851BC
		private static void _OnClick(object e, RoutedEventArgs args)
		{
			ToolBar toolBar = (ToolBar)e;
			ButtonBase buttonBase = args.OriginalSource as ButtonBase;
			if (toolBar.IsOverflowOpen && buttonBase != null && buttonBase.Parent == toolBar)
			{
				toolBar.Close();
			}
		}

		// Token: 0x06005839 RID: 22585 RVA: 0x00186FF6 File Offset: 0x001851F6
		internal override void OnAncestorChanged()
		{
			base.CoerceValue(ToolBar.OrientationProperty);
		}

		// Token: 0x0600583A RID: 22586 RVA: 0x00187003 File Offset: 0x00185203
		private void Close()
		{
			base.SetCurrentValueInternal(ToolBar.IsOverflowOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600583B RID: 22587 RVA: 0x00187015 File Offset: 0x00185215
		private ToolBarTray ToolBarTray
		{
			get
			{
				return base.Parent as ToolBarTray;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x00187022 File Offset: 0x00185222
		internal double MinLength
		{
			get
			{
				return this._minLength;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x0600583D RID: 22589 RVA: 0x0018702A File Offset: 0x0018522A
		internal double MaxLength
		{
			get
			{
				return this._maxLength;
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x00187032 File Offset: 0x00185232
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToolBar._dType;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to buttons on a toolbar.</summary>
		/// <returns>A resource key that represents the default style for buttons on the toolbar.</returns>
		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x0600583F RID: 22591 RVA: 0x00187039 File Offset: 0x00185239
		public static ResourceKey ButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> controls on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for toggle buttons on the toolbar.</returns>
		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x06005840 RID: 22592 RVA: 0x00187040 File Offset: 0x00185240
		public static ResourceKey ToggleButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarToggleButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to separators on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for separators on the toolbar.</returns>
		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06005841 RID: 22593 RVA: 0x00187047 File Offset: 0x00185247
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarSeparatorStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to check boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for check boxes on the <see cref="T:System.Windows.Controls.ToolBar" />.</returns>
		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06005842 RID: 22594 RVA: 0x0018704E File Offset: 0x0018524E
		public static ResourceKey CheckBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarCheckBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to radio buttons on a toolbar.</summary>
		/// <returns>A resource key that represents the default style for radio buttons on the toolbar.</returns>
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x06005843 RID: 22595 RVA: 0x00187055 File Offset: 0x00185255
		public static ResourceKey RadioButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarRadioButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to combo boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for combo boxes on the toolbar.</returns>
		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x06005844 RID: 22596 RVA: 0x0018705C File Offset: 0x0018525C
		public static ResourceKey ComboBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarComboBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to text boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for text boxes on the toolbar</returns>
		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06005845 RID: 22597 RVA: 0x00187063 File Offset: 0x00185263
		public static ResourceKey TextBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarTextBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to menus on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for menus on the toolbar.</returns>
		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06005846 RID: 22598 RVA: 0x0018706A File Offset: 0x0018526A
		public static ResourceKey MenuStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarMenuStyleKey;
			}
		}

		// Token: 0x04002EAD RID: 11949
		private static readonly DependencyPropertyKey OrientationPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Orientation", typeof(Orientation), typeof(ToolBar), new FrameworkPropertyMetadata(Orientation.Horizontal, null, new CoerceValueCallback(ToolBar.CoerceOrientation)));

		/// <summary>Identifies the <see cref="T:System.Windows.Controls.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="T:System.Windows.Controls.Orientation" /> dependency property.</returns>
		// Token: 0x04002EAE RID: 11950
		public static readonly DependencyProperty OrientationProperty = ToolBar.OrientationPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.Band" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.Band" /> dependency property.</returns>
		// Token: 0x04002EAF RID: 11951
		public static readonly DependencyProperty BandProperty = DependencyProperty.Register("Band", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.BandIndex" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.BandIndex" /> dependency property.</returns>
		// Token: 0x04002EB0 RID: 11952
		public static readonly DependencyProperty BandIndexProperty = DependencyProperty.Register("BandIndex", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowOpen" /> dependency property.</returns>
		// Token: 0x04002EB1 RID: 11953
		public static readonly DependencyProperty IsOverflowOpenProperty = DependencyProperty.Register("IsOverflowOpen", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ToolBar.OnOverflowOpenChanged), new CoerceValueCallback(ToolBar.CoerceIsOverflowOpen)));

		// Token: 0x04002EB2 RID: 11954
		internal static readonly DependencyPropertyKey HasOverflowItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasOverflowItems", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.HasOverflowItems" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.HasOverflowItems" /> dependency property.</returns>
		// Token: 0x04002EB3 RID: 11955
		public static readonly DependencyProperty HasOverflowItemsProperty = ToolBar.HasOverflowItemsPropertyKey.DependencyProperty;

		// Token: 0x04002EB4 RID: 11956
		internal static readonly DependencyPropertyKey IsOverflowItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsOverflowItem", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> attached property.</returns>
		// Token: 0x04002EB5 RID: 11957
		public static readonly DependencyProperty IsOverflowItemProperty = ToolBar.IsOverflowItemPropertyKey.DependencyProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> attached property.</returns>
		// Token: 0x04002EB6 RID: 11958
		public static readonly DependencyProperty OverflowModeProperty = DependencyProperty.RegisterAttached("OverflowMode", typeof(OverflowMode), typeof(ToolBar), new FrameworkPropertyMetadata(OverflowMode.AsNeeded, new PropertyChangedCallback(ToolBar.OnOverflowModeChanged)), new ValidateValueCallback(ToolBar.IsValidOverflowMode));

		// Token: 0x04002EB7 RID: 11959
		private ToolBarPanel _toolBarPanel;

		// Token: 0x04002EB8 RID: 11960
		private ToolBarOverflowPanel _toolBarOverflowPanel;

		// Token: 0x04002EB9 RID: 11961
		private const string ToolBarPanelTemplateName = "PART_ToolBarPanel";

		// Token: 0x04002EBA RID: 11962
		private const string ToolBarOverflowPanelTemplateName = "PART_ToolBarOverflowPanel";

		// Token: 0x04002EBB RID: 11963
		private double _minLength;

		// Token: 0x04002EBC RID: 11964
		private double _maxLength;

		// Token: 0x04002EBD RID: 11965
		private static DependencyObjectType _dType;
	}
}
