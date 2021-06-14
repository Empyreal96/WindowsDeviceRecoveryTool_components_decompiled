using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a selectable item inside a <see cref="T:System.Windows.Controls.Menu" />.</summary>
	// Token: 0x02000503 RID: 1283
	[DefaultEvent("Click")]
	[Localizability(LocalizationCategory.Menu)]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public class MenuItem : HeaderedItemsControl, ICommandSource
	{
		/// <summary>Gets the resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when it is a top-level <see cref="T:System.Windows.Controls.MenuItem" /> that can invoke commands.</summary>
		/// <returns>The resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when it is a top-level <see cref="T:System.Windows.Controls.MenuItem" /> that can invoke commands.</returns>
		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x0016E50E File Offset: 0x0016C70E
		public static ResourceKey TopLevelItemTemplateKey
		{
			get
			{
				if (MenuItem._topLevelItemTemplateKey == null)
				{
					MenuItem._topLevelItemTemplateKey = new ComponentResourceKey(typeof(MenuItem), "TopLevelItemTemplateKey");
				}
				return MenuItem._topLevelItemTemplateKey;
			}
		}

		/// <summary>Gets the resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a header of a top-level menu.</summary>
		/// <returns>The resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a header of a top-level menu.</returns>
		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x0016E535 File Offset: 0x0016C735
		public static ResourceKey TopLevelHeaderTemplateKey
		{
			get
			{
				if (MenuItem._topLevelHeaderTemplateKey == null)
				{
					MenuItem._topLevelHeaderTemplateKey = new ComponentResourceKey(typeof(MenuItem), "TopLevelHeaderTemplateKey");
				}
				return MenuItem._topLevelHeaderTemplateKey;
			}
		}

		/// <summary>Gets the resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a submenu.</summary>
		/// <returns>The resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a submenu.</returns>
		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x0016E55C File Offset: 0x0016C75C
		public static ResourceKey SubmenuItemTemplateKey
		{
			get
			{
				if (MenuItem._submenuItemTemplateKey == null)
				{
					MenuItem._submenuItemTemplateKey = new ComponentResourceKey(typeof(MenuItem), "SubmenuItemTemplateKey");
				}
				return MenuItem._submenuItemTemplateKey;
			}
		}

		/// <summary>Gets the resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a header of a submenu.</summary>
		/// <returns>The resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a header of a submenu.</returns>
		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x060051DA RID: 20954 RVA: 0x0016E583 File Offset: 0x0016C783
		public static ResourceKey SubmenuHeaderTemplateKey
		{
			get
			{
				if (MenuItem._submenuHeaderTemplateKey == null)
				{
					MenuItem._submenuHeaderTemplateKey = new ComponentResourceKey(typeof(MenuItem), "SubmenuHeaderTemplateKey");
				}
				return MenuItem._submenuHeaderTemplateKey;
			}
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x0016E5B4 File Offset: 0x0016C7B4
		static MenuItem()
		{
			MenuItem.ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.PreviewClickEvent = EventManager.RegisterRoutedEvent("PreviewClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.SubmenuOpenedEvent = EventManager.RegisterRoutedEvent("SubmenuOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.SubmenuClosedEvent = EventManager.RegisterRoutedEvent("SubmenuClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuItem));
			MenuItem.CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(MenuItem.OnCommandChanged)));
			MenuItem.CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.IsSubmenuOpenProperty = DependencyProperty.Register("IsSubmenuOpen", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MenuItem.OnIsSubmenuOpenChanged), new CoerceValueCallback(MenuItem.CoerceIsSubmenuOpen)));
			MenuItem.RolePropertyKey = DependencyProperty.RegisterReadOnly("Role", typeof(MenuItemRole), typeof(MenuItem), new FrameworkPropertyMetadata(MenuItemRole.TopLevelItem));
			MenuItem.RoleProperty = MenuItem.RolePropertyKey.DependencyProperty;
			MenuItem.IsCheckableProperty = DependencyProperty.Register("IsCheckable", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(MenuItem.OnIsCheckableChanged)));
			MenuItem.IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsPressedProperty = MenuItem.IsPressedPropertyKey.DependencyProperty;
			MenuItem.IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsHighlightedProperty = MenuItem.IsHighlightedPropertyKey.DependencyProperty;
			MenuItem.IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(MenuItem.OnIsCheckedChanged)));
			MenuItem.StaysOpenOnClickProperty = DependencyProperty.Register("StaysOpenOnClick", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MenuItem.OnIsSelectedChanged)));
			MenuItem.InputGestureTextProperty = DependencyProperty.Register("InputGestureText", typeof(string), typeof(MenuItem), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(MenuItem.OnInputGestureTextChanged), new CoerceValueCallback(MenuItem.CoerceInputGestureText)));
			MenuItem.IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(MenuItem), new FrameworkPropertyMetadata(null));
			MenuItem.IsSuspendingPopupAnimationPropertyKey = DependencyProperty.RegisterReadOnly("IsSuspendingPopupAnimation", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			MenuItem.IsSuspendingPopupAnimationProperty = MenuItem.IsSuspendingPopupAnimationPropertyKey.DependencyProperty;
			MenuItem.ItemContainerTemplateSelectorProperty = MenuBase.ItemContainerTemplateSelectorProperty.AddOwner(typeof(MenuItem), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));
			MenuItem.UsesItemContainerTemplateProperty = MenuBase.UsesItemContainerTemplateProperty.AddOwner(typeof(MenuItem));
			MenuItem.InsideContextMenuProperty = DependencyProperty.RegisterAttached("InsideContextMenu", typeof(bool), typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			MenuItem.BooleanFieldStoreProperty = DependencyProperty.RegisterAttached("BooleanFieldStore", typeof(MenuItem.BoolField), typeof(MenuItem), new FrameworkPropertyMetadata((MenuItem.BoolField)0));
			HeaderedItemsControl.HeaderProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null, new CoerceValueCallback(MenuItem.CoerceHeader)));
			EventManager.RegisterClassHandler(typeof(MenuItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(MenuItem.OnAccessKeyPressed));
			EventManager.RegisterClassHandler(typeof(MenuItem), MenuBase.IsSelectedChangedEvent, new RoutedPropertyChangedEventHandler<bool>(MenuItem.OnIsSelectedChanged));
			Control.ForegroundProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemColors.MenuTextBrush));
			Control.FontFamilyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily));
			Control.FontSizeProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize));
			Control.FontStyleProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle));
			Control.FontWeightProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight));
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null, new CoerceValueCallback(MenuItem.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(typeof(MenuItem)));
			MenuItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(MenuItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.None));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(null));
			InputMethod.IsInputMethodSuspendedProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.MenuItem" /> is clicked. </summary>
		// Token: 0x140000FC RID: 252
		// (add) Token: 0x060051DD RID: 20957 RVA: 0x0016EBFC File Offset: 0x0016CDFC
		// (remove) Token: 0x060051DE RID: 20958 RVA: 0x0016EC0A File Offset: 0x0016CE0A
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(MenuItem.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.ClickEvent, value);
			}
		}

		/// <summary>Occurs when a menu item is checked. </summary>
		// Token: 0x140000FD RID: 253
		// (add) Token: 0x060051DF RID: 20959 RVA: 0x0016EC18 File Offset: 0x0016CE18
		// (remove) Token: 0x060051E0 RID: 20960 RVA: 0x0016EC26 File Offset: 0x0016CE26
		[Category("Behavior")]
		public event RoutedEventHandler Checked
		{
			add
			{
				base.AddHandler(MenuItem.CheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.CheckedEvent, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.MenuItem" /> is unchecked.</summary>
		// Token: 0x140000FE RID: 254
		// (add) Token: 0x060051E1 RID: 20961 RVA: 0x0016EC34 File Offset: 0x0016CE34
		// (remove) Token: 0x060051E2 RID: 20962 RVA: 0x0016EC42 File Offset: 0x0016CE42
		[Category("Behavior")]
		public event RoutedEventHandler Unchecked
		{
			add
			{
				base.AddHandler(MenuItem.UncheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.UncheckedEvent, value);
			}
		}

		/// <summary>Occurs when the state of the <see cref="P:System.Windows.Controls.MenuItem.IsSubmenuOpen" /> property changes to <see langword="true" />. </summary>
		// Token: 0x140000FF RID: 255
		// (add) Token: 0x060051E3 RID: 20963 RVA: 0x0016EC50 File Offset: 0x0016CE50
		// (remove) Token: 0x060051E4 RID: 20964 RVA: 0x0016EC5E File Offset: 0x0016CE5E
		[Category("Behavior")]
		public event RoutedEventHandler SubmenuOpened
		{
			add
			{
				base.AddHandler(MenuItem.SubmenuOpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.SubmenuOpenedEvent, value);
			}
		}

		/// <summary>Occurs when the state of the <see cref="P:System.Windows.Controls.MenuItem.IsSubmenuOpen" /> property changes to <see langword="false" />. </summary>
		// Token: 0x14000100 RID: 256
		// (add) Token: 0x060051E5 RID: 20965 RVA: 0x0016EC6C File Offset: 0x0016CE6C
		// (remove) Token: 0x060051E6 RID: 20966 RVA: 0x0016EC7A File Offset: 0x0016CE7A
		[Category("Behavior")]
		public event RoutedEventHandler SubmenuClosed
		{
			add
			{
				base.AddHandler(MenuItem.SubmenuClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MenuItem.SubmenuClosedEvent, value);
			}
		}

		// Token: 0x060051E7 RID: 20967 RVA: 0x0016EC88 File Offset: 0x0016CE88
		private static object CoerceHeader(DependencyObject d, object value)
		{
			MenuItem menuItem = (MenuItem)d;
			RoutedUICommand routedUICommand;
			if (value == null && !menuItem.HasNonDefaultValue(HeaderedItemsControl.HeaderProperty))
			{
				routedUICommand = (menuItem.Command as RoutedUICommand);
				if (routedUICommand != null)
				{
					value = routedUICommand.Text;
				}
				return value;
			}
			routedUICommand = (value as RoutedUICommand);
			if (routedUICommand != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				if (itemsControl != null)
				{
					object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(menuItem);
					if (obj == value)
					{
						return routedUICommand.Text;
					}
				}
			}
			return value;
		}

		/// <summary>Gets or sets the command associated with the menu item.  </summary>
		/// <returns>The command associated with the <see cref="T:System.Windows.Controls.MenuItem" />.  The default is <see langword="null" />.</returns>
		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x060051E8 RID: 20968 RVA: 0x0016ECF2 File Offset: 0x0016CEF2
		// (set) Token: 0x060051E9 RID: 20969 RVA: 0x0016ED04 File Offset: 0x0016CF04
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(MenuItem.CommandProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandProperty, value);
			}
		}

		// Token: 0x060051EA RID: 20970 RVA: 0x0016ED14 File Offset: 0x0016CF14
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			menuItem.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x060051EB RID: 20971 RVA: 0x0016ED46 File Offset: 0x0016CF46
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
			base.CoerceValue(HeaderedItemsControl.HeaderProperty);
			base.CoerceValue(MenuItem.InputGestureTextProperty);
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x0016ED72 File Offset: 0x0016CF72
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x0016ED8C File Offset: 0x0016CF8C
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x0016EDA6 File Offset: 0x0016CFA6
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x0016EDB0 File Offset: 0x0016CFB0
		private void UpdateCanExecute()
		{
			MenuItem.SetBoolField(this, MenuItem.BoolField.CanExecuteInvalid, false);
			if (this.Command == null)
			{
				this.CanExecute = true;
				return;
			}
			MenuItem menuItem = ItemsControl.ItemsControlFromItemContainer(this) as MenuItem;
			if (menuItem == null || menuItem.IsSubmenuOpen)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
			MenuItem.SetBoolField(this, MenuItem.BoolField.CanExecuteInvalid, true);
		}

		/// <summary>Gets a value that indicates whether the <see cref="P:System.Windows.ContentElement.IsEnabled" /> property is <see langword="true" /> for the current menu item.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.MenuItem" /> can be selected; otherwise,<see langword=" false" />.</returns>
		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x060051F0 RID: 20976 RVA: 0x0016EE0A File Offset: 0x0016D00A
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		/// <summary>Gets or sets the parameter to pass to the <see cref="P:System.Windows.Controls.MenuItem.Command" /> property of a <see cref="T:System.Windows.Controls.MenuItem" />.  </summary>
		/// <returns>The parameter to pass to the <see cref="P:System.Windows.Controls.MenuItem.Command" /> property of a <see cref="T:System.Windows.Controls.MenuItem" />. The default is <see langword="null" />.</returns>
		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x060051F1 RID: 20977 RVA: 0x0016EE1C File Offset: 0x0016D01C
		// (set) Token: 0x060051F2 RID: 20978 RVA: 0x0016EE29 File Offset: 0x0016D029
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(MenuItem.CommandParameterProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandParameterProperty, value);
			}
		}

		/// <summary>Gets or sets the target element on which to raise the specified command.   </summary>
		/// <returns>The element on which to raise the specified command. The default is <see langword="null" />.</returns>
		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x060051F3 RID: 20979 RVA: 0x0016EE37 File Offset: 0x0016D037
		// (set) Token: 0x060051F4 RID: 20980 RVA: 0x0016EE49 File Offset: 0x0016D049
		[Bindable(true)]
		[Category("Action")]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(MenuItem.CommandTargetProperty);
			}
			set
			{
				base.SetValue(MenuItem.CommandTargetProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the submenu of the <see cref="T:System.Windows.Controls.MenuItem" /> is open.  </summary>
		/// <returns>
		///     <see langword="true" /> if the submenu of the <see cref="T:System.Windows.Controls.MenuItem" /> is open; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x0016EE57 File Offset: 0x0016D057
		// (set) Token: 0x060051F6 RID: 20982 RVA: 0x0016EE69 File Offset: 0x0016D069
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSubmenuOpen
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSubmenuOpenProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x0016EE7C File Offset: 0x0016D07C
		private static object CoerceIsSubmenuOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				MenuItem menuItem = (MenuItem)d;
				if (!menuItem.IsLoaded)
				{
					menuItem.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x0016EEB0 File Offset: 0x0016D0B0
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			MenuItem menuItem = (MenuItem)d;
			if (!menuItem.IsSubmenuOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x0016EED3 File Offset: 0x0016D0D3
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x0016EEE7 File Offset: 0x0016D0E7
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(MenuItem.IsSubmenuOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x0016EF04 File Offset: 0x0016D104
		private static void OnIsSubmenuOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			bool oldValue = (bool)e.OldValue;
			bool flag = (bool)e.NewValue;
			menuItem.StopTimer(ref menuItem._openHierarchyTimer);
			menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
			MenuItemAutomationPeer menuItemAutomationPeer = UIElementAutomationPeer.FromElement(menuItem) as MenuItemAutomationPeer;
			if (menuItemAutomationPeer != null)
			{
				menuItemAutomationPeer.ResetChildrenCache();
				menuItemAutomationPeer.RaiseExpandCollapseAutomationEvent(oldValue, flag);
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
				menuItem.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelHeader)
				{
					menuItem.SetMenuMode(true);
				}
				menuItem.CurrentSelection = null;
				menuItem.NotifySiblingsToSuspendAnimation();
				for (int i = 0; i < menuItem.Items.Count; i++)
				{
					MenuItem menuItem2 = menuItem.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem2 != null && MenuItem.GetBoolField(menuItem2, MenuItem.BoolField.CanExecuteInvalid))
					{
						menuItem2.UpdateCanExecute();
					}
				}
				menuItem.OnSubmenuOpened(new RoutedEventArgs(MenuItem.SubmenuOpenedEvent, menuItem));
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreMouseEvents, true);
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove, false);
				menuItem.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(object param)
				{
					MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreMouseEvents, false);
					return null;
				}), null);
			}
			else
			{
				if (menuItem.CurrentSelection != null)
				{
					if (menuItem.CurrentSelection.IsKeyboardFocusWithin)
					{
						menuItem.Focus();
					}
					if (menuItem.CurrentSelection.IsSubmenuOpen)
					{
						menuItem.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					}
				}
				else if (menuItem.IsKeyboardFocusWithin)
				{
					menuItem.Focus();
				}
				menuItem.CurrentSelection = null;
				if (menuItem.IsMouseOver && menuItem.Role == MenuItemRole.SubmenuHeader)
				{
					MenuItem.SetBoolField(menuItem, MenuItem.BoolField.IgnoreNextMouseLeave, true);
				}
				menuItem.NotifyChildrenToResumeAnimation();
				if (menuItem._submenuPopup == null)
				{
					menuItem.OnSubmenuClosed(new RoutedEventArgs(MenuItem.SubmenuClosedEvent, menuItem));
				}
			}
			menuItem.CoerceValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x0016F170 File Offset: 0x0016D370
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnSubmenuClosed(new RoutedEventArgs(MenuItem.SubmenuClosedEvent, this));
		}

		/// <summary>Called when the submenu of a <see cref="T:System.Windows.Controls.MenuItem" /> is opened. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.MenuItem.SubmenuOpened" /> event.</param>
		// Token: 0x060051FD RID: 20989 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSubmenuOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Called when the submenu of a <see cref="T:System.Windows.Controls.MenuItem" /> is closed. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.MenuItem.SubmenuClosed" /> event.</param>
		// Token: 0x060051FE RID: 20990 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSubmenuClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Gets a value that indicates the role of a <see cref="T:System.Windows.Controls.MenuItem" />.   </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.MenuItemRole" /> values. The default is <see cref="F:System.Windows.Controls.MenuItemRole.TopLevelItem" />.</returns>
		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x060051FF RID: 20991 RVA: 0x0016F183 File Offset: 0x0016D383
		[Category("Behavior")]
		public MenuItemRole Role
		{
			get
			{
				return (MenuItemRole)base.GetValue(MenuItem.RoleProperty);
			}
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x0016F198 File Offset: 0x0016D398
		private void UpdateRole()
		{
			MenuItemRole menuItemRole;
			if (!this.IsCheckable && base.HasItems)
			{
				if (this.LogicalParent is Menu)
				{
					menuItemRole = MenuItemRole.TopLevelHeader;
				}
				else
				{
					menuItemRole = MenuItemRole.SubmenuHeader;
				}
			}
			else if (this.LogicalParent is Menu)
			{
				menuItemRole = MenuItemRole.TopLevelItem;
			}
			else
			{
				menuItemRole = MenuItemRole.SubmenuItem;
			}
			base.SetValue(MenuItem.RolePropertyKey, menuItemRole);
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.MenuItem" /> can be checked.   </summary>
		/// <returns>
		///     <see langword="true" /> if the menu item can be checked; otherwise, <see langword="false" />.  The default is <see langword="false" />.</returns>
		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06005201 RID: 20993 RVA: 0x0016F1EE File Offset: 0x0016D3EE
		// (set) Token: 0x06005202 RID: 20994 RVA: 0x0016F200 File Offset: 0x0016D400
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsCheckable
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsCheckableProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsCheckableProperty, value);
			}
		}

		// Token: 0x06005203 RID: 20995 RVA: 0x0016F20E File Offset: 0x0016D40E
		private static void OnIsCheckableChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			((MenuItem)target).UpdateRole();
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.MenuItem" /> is pressed.  </summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.MenuItem" /> is pressed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06005204 RID: 20996 RVA: 0x0016F21B File Offset: 0x0016D41B
		// (set) Token: 0x06005205 RID: 20997 RVA: 0x0016F22D File Offset: 0x0016D42D
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsPressed
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsPressedProperty);
			}
			protected set
			{
				base.SetValue(MenuItem.IsPressedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005206 RID: 20998 RVA: 0x0016F240 File Offset: 0x0016D440
		private void UpdateIsPressed()
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (Mouse.LeftButton == MouseButtonState.Pressed && base.IsMouseOver && rect.Contains(Mouse.GetPosition(this)))
			{
				this.IsPressed = true;
				return;
			}
			base.ClearValue(MenuItem.IsPressedPropertyKey);
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.MenuItem" /> is highlighted.  </summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.MenuItem" /> is highlighted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06005207 RID: 20999 RVA: 0x0016F295 File Offset: 0x0016D495
		// (set) Token: 0x06005208 RID: 21000 RVA: 0x0016F2A7 File Offset: 0x0016D4A7
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsHighlightedProperty);
			}
			protected set
			{
				base.SetValue(MenuItem.IsHighlightedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.MenuItem" /> is checked.  </summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.MenuItem" /> is checked; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x0016F2BA File Offset: 0x0016D4BA
		// (set) Token: 0x0600520A RID: 21002 RVA: 0x0016F2CC File Offset: 0x0016D4CC
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsChecked
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsCheckedProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsCheckedProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.MenuItem.IsChecked" /> property becomes <see langword="true" />. This method raises the <see cref="E:System.Windows.Controls.MenuItem.Checked" /> routed event. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.MenuItem.Checked" /> event.</param>
		// Token: 0x0600520B RID: 21003 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnChecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.MenuItem.IsChecked" /> property becomes <see langword="false" />. This method raises the <see cref="E:System.Windows.Controls.MenuItem.Unchecked" /> routed event. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.MenuItem.Unchecked" /> event.</param>
		// Token: 0x0600520C RID: 21004 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnUnchecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x0016F2E0 File Offset: 0x0016D4E0
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			if ((bool)e.NewValue)
			{
				menuItem.OnChecked(new RoutedEventArgs(MenuItem.CheckedEvent));
				return;
			}
			menuItem.OnUnchecked(new RoutedEventArgs(MenuItem.UncheckedEvent));
		}

		/// <summary>Gets or sets a value that indicates that the submenu in which this <see cref="T:System.Windows.Controls.MenuItem" /> is located should not close when this item is clicked.   </summary>
		/// <returns>
		///     <see langword="true" /> if the submenu in which this <see cref="T:System.Windows.Controls.MenuItem" /> is located should not close when this item is clicked; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x0600520E RID: 21006 RVA: 0x0016F323 File Offset: 0x0016D523
		// (set) Token: 0x0600520F RID: 21007 RVA: 0x0016F335 File Offset: 0x0016D535
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpenOnClick
		{
			get
			{
				return (bool)base.GetValue(MenuItem.StaysOpenOnClickProperty);
			}
			set
			{
				base.SetValue(MenuItem.StaysOpenOnClickProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06005210 RID: 21008 RVA: 0x0016F348 File Offset: 0x0016D548
		// (set) Token: 0x06005211 RID: 21009 RVA: 0x0016F35A File Offset: 0x0016D55A
		internal bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(MenuItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005212 RID: 21010 RVA: 0x0016F370 File Offset: 0x0016D570
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MenuItem menuItem = (MenuItem)d;
			menuItem.SetValue(MenuItem.IsHighlightedPropertyKey, e.NewValue);
			if ((bool)e.OldValue)
			{
				if (menuItem.IsSubmenuOpen)
				{
					menuItem.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
				}
				menuItem.StopTimer(ref menuItem._openHierarchyTimer);
				menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
			}
			menuItem.RaiseEvent(new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue, MenuBase.IsSelectedChangedEvent));
		}

		// Token: 0x06005213 RID: 21011 RVA: 0x0016F3FC File Offset: 0x0016D5FC
		private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
		{
			if (sender != e.OriginalSource)
			{
				MenuItem menuItem = (MenuItem)sender;
				MenuItem menuItem2 = e.OriginalSource as MenuItem;
				if (menuItem2 != null)
				{
					if (e.NewValue)
					{
						if (menuItem.CurrentSelection == menuItem2)
						{
							menuItem.StopTimer(ref menuItem._closeHierarchyTimer);
						}
						if (menuItem.CurrentSelection != menuItem2 && menuItem2.LogicalParent == menuItem)
						{
							if (menuItem.CurrentSelection != null && menuItem.CurrentSelection.IsSubmenuOpen)
							{
								menuItem.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
							}
							menuItem.CurrentSelection = menuItem2;
						}
					}
					else if (menuItem.CurrentSelection == menuItem2)
					{
						menuItem.CurrentSelection = null;
					}
					e.Handled = true;
				}
			}
		}

		/// <summary> Sets the text describing an input gesture that will call the command tied to the specified item.  </summary>
		/// <returns>The text that describes the input gesture, such as Ctrl+C for the Copy command. The default is an empty string ("").</returns>
		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06005214 RID: 21012 RVA: 0x0016F4A4 File Offset: 0x0016D6A4
		// (set) Token: 0x06005215 RID: 21013 RVA: 0x0016F4B6 File Offset: 0x0016D6B6
		[Bindable(true)]
		[CustomCategory("Content")]
		public string InputGestureText
		{
			get
			{
				return (string)base.GetValue(MenuItem.InputGestureTextProperty);
			}
			set
			{
				base.SetValue(MenuItem.InputGestureTextProperty, value);
			}
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x00002137 File Offset: 0x00000337
		private static void OnInputGestureTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x0016F4C4 File Offset: 0x0016D6C4
		private static object CoerceInputGestureText(DependencyObject d, object value)
		{
			MenuItem menuItem = (MenuItem)d;
			RoutedCommand routedCommand;
			if (string.IsNullOrEmpty((string)value) && !menuItem.HasNonDefaultValue(MenuItem.InputGestureTextProperty) && (routedCommand = (menuItem.Command as RoutedCommand)) != null)
			{
				InputGestureCollection inputGestures = routedCommand.InputGestures;
				if (inputGestures != null && inputGestures.Count >= 1)
				{
					for (int i = 0; i < inputGestures.Count; i++)
					{
						KeyGesture keyGesture = ((IList)inputGestures)[i] as KeyGesture;
						if (keyGesture != null)
						{
							return keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
						}
					}
				}
			}
			return value;
		}

		/// <summary>Gets or sets the icon that appears in a <see cref="T:System.Windows.Controls.MenuItem" />.  </summary>
		/// <returns>The icon that appears in a <see cref="T:System.Windows.Controls.MenuItem" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06005218 RID: 21016 RVA: 0x0016F545 File Offset: 0x0016D745
		// (set) Token: 0x06005219 RID: 21017 RVA: 0x0016F552 File Offset: 0x0016D752
		[Bindable(true)]
		[CustomCategory("Content")]
		public object Icon
		{
			get
			{
				return base.GetValue(MenuItem.IconProperty);
			}
			set
			{
				base.SetValue(MenuItem.IconProperty, value);
			}
		}

		/// <summary>Gets whether a menu suspends animations on its <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.  </summary>
		/// <returns>
		///     <see langword="true" /> if the menu should suspend animations on its popup; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x0600521A RID: 21018 RVA: 0x0016F560 File Offset: 0x0016D760
		// (set) Token: 0x0600521B RID: 21019 RVA: 0x0016F572 File Offset: 0x0016D772
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSuspendingPopupAnimation
		{
			get
			{
				return (bool)base.GetValue(MenuItem.IsSuspendingPopupAnimationProperty);
			}
			internal set
			{
				base.SetValue(MenuItem.IsSuspendingPopupAnimationPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x0016F588 File Offset: 0x0016D788
		private void NotifySiblingsToSuspendAnimation()
		{
			if (!this.IsSuspendingPopupAnimation)
			{
				bool boolField = MenuItem.GetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard);
				MenuItem ignore = boolField ? null : this;
				ItemsControl menu = ItemsControl.ItemsControlFromItemContainer(this);
				MenuBase.SetSuspendingPopupAnimation(menu, ignore, true);
				if (!boolField)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object arg)
					{
						((MenuItem)arg).IsSuspendingPopupAnimation = true;
						return null;
					}), this);
					return;
				}
				MenuItem.SetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard, false);
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x0016F5F5 File Offset: 0x0016D7F5
		private void NotifyChildrenToResumeAnimation()
		{
			MenuBase.SetSuspendingPopupAnimation(this, null, false);
		}

		/// <summary>Gets or sets the custom logic for choosing a template used to display each item. </summary>
		/// <returns>A custom object that provides logic and returns an item container. </returns>
		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x0600521E RID: 21022 RVA: 0x0016F5FF File Offset: 0x0016D7FF
		// (set) Token: 0x0600521F RID: 21023 RVA: 0x0016F611 File Offset: 0x0016D811
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(MenuItem.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(MenuItem.ItemContainerTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the menu selects different item containers, depending on the type of the item in the underlying collection or some other heuristic.</summary>
		/// <returns>
		///     <see langword="true" /> the menu selects different item containers; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x0016F61F File Offset: 0x0016D81F
		// (set) Token: 0x06005221 RID: 21025 RVA: 0x0016F631 File Offset: 0x0016D831
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(MenuItem.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(MenuItem.UsesItemContainerTemplateProperty, value);
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" /> for this <see cref="T:System.Windows.Controls.MenuItem" />.</returns>
		// Token: 0x06005222 RID: 21026 RVA: 0x0016F63F File Offset: 0x0016D83F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MenuItemAutomationPeer(this);
		}

		/// <summary>Called when the <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> property is set to <see langword="true" /> and raises an <see cref="E:System.Windows.FrameworkElement.Initialized" /> event. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.FrameworkElement.Initialized" /> event.</param>
		// Token: 0x06005223 RID: 21027 RVA: 0x0016F647 File Offset: 0x0016D847
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			this.UpdateRole();
		}

		/// <summary> Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x06005224 RID: 21028 RVA: 0x0013719E File Offset: 0x0013539E
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x0016F658 File Offset: 0x0016D858
		internal static void PrepareMenuItem(DependencyObject element, object item)
		{
			MenuItem menuItem = element as MenuItem;
			if (menuItem != null)
			{
				ICommand command = item as ICommand;
				if (command != null && !menuItem.HasNonDefaultValue(MenuItem.CommandProperty))
				{
					menuItem.Command = command;
				}
				if (MenuItem.GetBoolField(menuItem, MenuItem.BoolField.CanExecuteInvalid))
				{
					menuItem.UpdateCanExecute();
					return;
				}
			}
			else
			{
				Separator separator = item as Separator;
				if (separator != null)
				{
					bool flag;
					BaseValueSourceInternal valueSource = separator.GetValueSource(FrameworkElement.StyleProperty, null, out flag);
					if (valueSource <= BaseValueSourceInternal.ImplicitReference)
					{
						separator.SetResourceReference(FrameworkElement.StyleProperty, MenuItem.SeparatorStyleKey);
					}
					separator.DefaultStyleKey = MenuItem.SeparatorStyleKey;
				}
			}
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.MenuItem" /> is clicked and raises a <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event.</summary>
		// Token: 0x06005226 RID: 21030 RVA: 0x0016F6D8 File Offset: 0x0016D8D8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected virtual void OnClick()
		{
			this.OnClickImpl(false);
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x0016F6E1 File Offset: 0x0016D8E1
		[SecurityCritical]
		internal virtual void OnClickCore(bool userInitiated)
		{
			this.OnClick();
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x0016F6EC File Offset: 0x0016D8EC
		[SecurityCritical]
		internal void OnClickImpl(bool userInitiated)
		{
			if (this.IsCheckable)
			{
				base.SetCurrentValueInternal(MenuItem.IsCheckedProperty, BooleanBoxes.Box(!this.IsChecked));
			}
			if (!base.IsKeyboardFocusWithin)
			{
				this.FocusOrSelect();
			}
			base.RaiseEvent(new RoutedEventArgs(MenuItem.PreviewClickEvent, this));
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(this.InvokeClickAfterRender), userInitiated);
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x0016F774 File Offset: 0x0016D974
		[SecurityCritical]
		private object InvokeClickAfterRender(object arg)
		{
			bool userInitiated = (bool)arg;
			base.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent, this));
			CommandHelpers.CriticalExecuteCommandSource(this, userInitiated);
			return null;
		}

		/// <summary>Called when the left mouse button is pressed. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event.</param>
		// Token: 0x0600522A RID: 21034 RVA: 0x0016F7A1 File Offset: 0x0016D9A1
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseDown(e);
				this.UpdateIsPressed();
				if (e.UserInitiated)
				{
					this._userInitiatedPress = true;
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Called when the right mouse button is pressed. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseRightButtonDown" /> event.</param>
		// Token: 0x0600522B RID: 21035 RVA: 0x0016F7CE File Offset: 0x0016D9CE
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseDown(e);
				if (e.UserInitiated)
				{
					this._userInitiatedPress = true;
				}
			}
			base.OnMouseRightButtonDown(e);
		}

		/// <summary>Called when the left mouse button is released. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event.</param>
		// Token: 0x0600522C RID: 21036 RVA: 0x0016F7F5 File Offset: 0x0016D9F5
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseUp(e);
				this.UpdateIsPressed();
				this._userInitiatedPress = false;
			}
			base.OnMouseLeftButtonUp(e);
		}

		/// <summary>Called when the right mouse button is released. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseRightButtonUp" /> event.</param>
		// Token: 0x0600522D RID: 21037 RVA: 0x0016F81A File Offset: 0x0016DA1A
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				this.HandleMouseUp(e);
				this._userInitiatedPress = false;
			}
			base.OnMouseRightButtonUp(e);
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x0016F83C File Offset: 0x0016DA3C
		private void HandleMouseDown(MouseButtonEventArgs e)
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(e.GetPosition(this)) && (e.ChangedButton == MouseButton.Left || (e.ChangedButton == MouseButton.Right && this.InsideContextMenu)))
			{
				MenuItemRole role = this.Role;
				if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
				{
					this.ClickHeader();
				}
			}
			e.Handled = true;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x0016F8A4 File Offset: 0x0016DAA4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void HandleMouseUp(MouseButtonEventArgs e)
		{
			Rect rect = new Rect(default(Point), base.RenderSize);
			if (rect.Contains(e.GetPosition(this)) && (e.ChangedButton == MouseButton.Left || (e.ChangedButton == MouseButton.Right && this.InsideContextMenu)))
			{
				MenuItemRole role = this.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
				{
					if (this._userInitiatedPress)
					{
						this.ClickItem(e.UserInitiated);
					}
					else
					{
						this.ClickItem(false);
					}
				}
			}
			if (e.ChangedButton != MouseButton.Right || this.InsideContextMenu)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06005230 RID: 21040 RVA: 0x0016F934 File Offset: 0x0016DB34
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			bool flag = false;
			if (e.Target == null)
			{
				if (Mouse.Captured == null || Mouse.Captured is MenuBase)
				{
					e.Target = menuItem;
					if (e.OriginalSource == menuItem && menuItem.IsSubmenuOpen)
					{
						flag = true;
					}
				}
				else
				{
					e.Handled = true;
				}
			}
			else if (e.Scope == null)
			{
				if (e.Target != menuItem && e.Target is MenuItem)
				{
					flag = true;
				}
				else
				{
					DependencyObject dependencyObject = e.Source as DependencyObject;
					while (dependencyObject != null && dependencyObject != menuItem)
					{
						UIElement uielement = dependencyObject as UIElement;
						if (uielement != null && ItemsControl.ItemsControlFromItemContainer(uielement) == menuItem)
						{
							flag = true;
							break;
						}
						dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
					}
				}
			}
			if (flag)
			{
				e.Scope = menuItem;
				e.Handled = true;
			}
		}

		/// <summary>Called whenever the mouse leaves a <see cref="T:System.Windows.Controls.MenuItem" />. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseLeave" /> event.</param>
		// Token: 0x06005231 RID: 21041 RVA: 0x0016F9F0 File Offset: 0x0016DBF0
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			MenuItemRole role = this.Role;
			if (((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem) && this.IsInMenuMode) || role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
			{
				this.MouseLeaveInMenuMode(role);
			}
			else if (base.IsMouseOver != this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.Box(base.IsMouseOver));
			}
			this.UpdateIsPressed();
		}

		/// <summary>Called when the mouse is moved over a menu item.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseMove" /> event.</param>
		// Token: 0x06005232 RID: 21042 RVA: 0x0016FA58 File Offset: 0x0016DC58
		protected override void OnMouseMove(MouseEventArgs e)
		{
			MenuItem menuItem = ItemsControl.ItemsControlFromItemContainer(this) as MenuItem;
			if (menuItem != null && MenuItem.GetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove))
			{
				MenuItem.SetBoolField(menuItem, MenuItem.BoolField.MouseEnterOnMouseMove, false);
				this.MouseEnterHelper();
			}
		}

		/// <summary>Called whenever the mouse enters a <see cref="T:System.Windows.Controls.MenuItem" />. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.Mouse.MouseEnter" /> event.</param>
		// Token: 0x06005233 RID: 21043 RVA: 0x0016FA8B File Offset: 0x0016DC8B
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			this.MouseEnterHelper();
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x0016FA9C File Offset: 0x0016DC9C
		private void MouseEnterHelper()
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
			if (itemsControl == null || !MenuItem.GetBoolField(itemsControl, MenuItem.BoolField.IgnoreMouseEvents))
			{
				MenuItemRole role = this.Role;
				if (((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem) && this.OpenOnMouseEnter) || role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
				{
					this.MouseEnterInMenuMode(role);
				}
				else if (base.IsMouseOver != this.IsSelected)
				{
					base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.Box(base.IsMouseOver));
				}
				this.UpdateIsPressed();
				return;
			}
			if (itemsControl is MenuItem)
			{
				MenuItem.SetBoolField(itemsControl, MenuItem.BoolField.MouseEnterOnMouseMove, true);
			}
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x0016FB20 File Offset: 0x0016DD20
		private void MouseEnterInMenuMode(MenuItemRole role)
		{
			if (role > MenuItemRole.TopLevelHeader)
			{
				if (role - MenuItemRole.SubmenuItem <= 1)
				{
					MenuItem currentSibling = this.CurrentSibling;
					if (currentSibling == null || !currentSibling.IsSubmenuOpen)
					{
						if (!this.IsSubmenuOpen)
						{
							this.FocusOrSelect();
						}
						else
						{
							this.IsHighlighted = true;
						}
					}
					else
					{
						currentSibling.IsHighlighted = false;
						this.IsHighlighted = true;
					}
					if (!this.IsSelected || !this.IsSubmenuOpen)
					{
						this.SetTimerToOpenHierarchy();
					}
				}
			}
			else if (!this.IsSubmenuOpen)
			{
				this.OpenHierarchy(role);
			}
			this.StopTimer(ref this._closeHierarchyTimer);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x0016FBA8 File Offset: 0x0016DDA8
		private void MouseLeaveInMenuMode(MenuItemRole role)
		{
			if (role == MenuItemRole.SubmenuHeader || role == MenuItemRole.SubmenuItem)
			{
				if (MenuItem.GetBoolField(this, MenuItem.BoolField.IgnoreNextMouseLeave))
				{
					MenuItem.SetBoolField(this, MenuItem.BoolField.IgnoreNextMouseLeave, false);
				}
				else if (!this.IsSubmenuOpen)
				{
					if (this.IsSelected)
					{
						base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
					}
					else
					{
						this.IsHighlighted = false;
					}
					if (base.IsKeyboardFocusWithin)
					{
						ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
						if (itemsControl != null)
						{
							itemsControl.Focus();
						}
					}
				}
				else if (this.IsMouseOverSibling)
				{
					this.SetTimerToCloseHierarchy();
				}
			}
			this.StopTimer(ref this._openHierarchyTimer);
		}

		/// <summary>Announces that the keyboard is focused on this element. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.GotFocus" /> event.</param>
		// Token: 0x06005237 RID: 21047 RVA: 0x0016FC2E File Offset: 0x0016DE2E
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			if (!e.Handled && e.NewFocus == this)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
		}

		/// <summary>Called when the focus is no longer on or within a <see cref="T:System.Windows.Controls.MenuItem" />.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.ContentElement.IsKeyboardFocusWithinChanged" /> event.</param>
		// Token: 0x06005238 RID: 21048 RVA: 0x0016FC58 File Offset: 0x0016DE58
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin && !this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
		}

		/// <summary>Gets whether the control supports scrolling.</summary>
		/// <returns>
		///     <see langword="true" /> in all cases.</returns>
		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06005239 RID: 21049 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		/// <summary> Responds to the <see cref="E:System.Windows.UIElement.KeyDown" /> event. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x0600523A RID: 21050 RVA: 0x0016FC84 File Offset: 0x0016DE84
		[SecurityCritical]
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = false;
			Key key = e.Key;
			MenuItemRole role = this.Role;
			FlowDirection flowDirection = base.FlowDirection;
			if (flowDirection == FlowDirection.RightToLeft)
			{
				if (key == Key.Right)
				{
					key = Key.Left;
				}
				else if (key == Key.Left)
				{
					key = Key.Right;
				}
			}
			if (key <= Key.Return)
			{
				if (key != Key.Tab)
				{
					if (key == Key.Return)
					{
						if (role == MenuItemRole.SubmenuItem || role == MenuItemRole.TopLevelItem)
						{
							this.ClickItem(e.UserInitiated);
							flag = true;
						}
						else if (role == MenuItemRole.TopLevelHeader)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
						}
						else if (role == MenuItemRole.SubmenuHeader && !this.IsSubmenuOpen)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
						}
					}
				}
				else if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
				{
					if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
					{
						base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					}
					else
					{
						base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					}
					flag = true;
				}
			}
			else
			{
				if (key != Key.Escape)
				{
					switch (key)
					{
					case Key.Left:
						break;
					case Key.Up:
						if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
						{
							base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_1AD;
						}
						goto IL_1AD;
					case Key.Right:
						if (role == MenuItemRole.SubmenuHeader && !this.IsSubmenuOpen)
						{
							this.OpenSubmenuWithKeyboard();
							flag = true;
							goto IL_1AD;
						}
						goto IL_1AD;
					case Key.Down:
						if (role == MenuItemRole.SubmenuHeader && this.IsSubmenuOpen && this.CurrentSelection == null)
						{
							base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
							flag = true;
							goto IL_1AD;
						}
						goto IL_1AD;
					default:
						goto IL_1AD;
					}
				}
				if (role != MenuItemRole.TopLevelHeader && role != MenuItemRole.TopLevelItem && this.IsSubmenuOpen)
				{
					base.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					flag = true;
				}
			}
			IL_1AD:
			if (!flag)
			{
				ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(this);
				if (parent != null && !MenuItem.GetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents))
				{
					MenuItem.SetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents, true);
					parent.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(object param)
					{
						MenuItem.SetBoolField(parent, MenuItem.BoolField.IgnoreMouseEvents, false);
						return null;
					}), null);
				}
				flag = this.MenuItemNavigate(e.Key, e.KeyboardDevice.Modifiers);
			}
			if (flag)
			{
				e.Handled = true;
			}
		}

		/// <summary>Responds when the <see cref="P:System.Windows.Controls.AccessText.AccessKey" /> for this control is invoked. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Input.AccessKeyManager.AccessKeyPressed" /> event.</param>
		// Token: 0x0600523B RID: 21051 RVA: 0x0016FEC0 File Offset: 0x0016E0C0
		[SecurityCritical]
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			base.OnAccessKey(e);
			if (!e.IsMultiple)
			{
				switch (this.Role)
				{
				case MenuItemRole.TopLevelItem:
				case MenuItemRole.SubmenuItem:
					this.ClickItem(e.UserInitiated);
					return;
				case MenuItemRole.TopLevelHeader:
				case MenuItemRole.SubmenuHeader:
					this.OpenSubmenuWithKeyboard();
					break;
				default:
					return;
				}
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property changes. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</param>
		// Token: 0x0600523C RID: 21052 RVA: 0x0016FF0D File Offset: 0x0016E10D
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			this.UpdateRole();
			base.OnItemsChanged(e);
		}

		/// <summary> Determines if the specified item is (or is eligible to be) its own ItemContainer. </summary>
		/// <param name="item">Specified item.</param>
		/// <returns>
		///     <see langword="true" /> if the item is its own ItemContainer; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600523D RID: 21053 RVA: 0x0016FF1C File Offset: 0x0016E11C
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is MenuItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		/// <summary>Used to determine whether to apply a style to the item container.</summary>
		/// <param name="container">Container to which the style will be applied.</param>
		/// <param name="item">Item to which the container belongs.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.MenuItem" /> is not a <see cref="T:System.Windows.Controls.Separator" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600523E RID: 21054 RVA: 0x0016FF49 File Offset: 0x0016E149
		protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return !(item is Separator) && base.ShouldApplyItemContainerStyle(container, item);
		}

		/// <summary>Creates or identifies the element used to display a specified item.</summary>
		/// <returns>The element used to display a specified item.</returns>
		// Token: 0x0600523F RID: 21055 RVA: 0x0016FF60 File Offset: 0x0016E160
		protected override DependencyObject GetContainerForItemOverride()
		{
			object currentItem = this._currentItem;
			this._currentItem = null;
			if (this.UsesItemContainerTemplate)
			{
				DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
				if (dataTemplate != null)
				{
					object obj = dataTemplate.LoadContent();
					if (obj is MenuItem || obj is Separator)
					{
						return obj as DependencyObject;
					}
					throw new InvalidOperationException(SR.Get("InvalidItemContainer", new object[]
					{
						base.GetType().Name,
						typeof(MenuItem).Name,
						typeof(Separator).Name,
						obj
					}));
				}
			}
			return new MenuItem();
		}

		/// <summary>Called when the parent of the visual <see cref="T:System.Windows.Controls.MenuItem" /> changes. </summary>
		/// <param name="oldParent">Old value of the parent of the visual, or null if the visual did not have a parent.</param>
		// Token: 0x06005240 RID: 21056 RVA: 0x00170004 File Offset: 0x0016E204
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			this.UpdateRole();
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(this);
			if (base.Parent != null && parentInternal != null && base.Parent != parentInternal)
			{
				Binding binding = new Binding();
				binding.Path = new PropertyPath(DefinitionBase.PrivateSharedSizeScopeProperty);
				binding.Mode = BindingMode.OneWay;
				binding.Source = parentInternal;
				BindingOperations.SetBinding(this, DefinitionBase.PrivateSharedSizeScopeProperty, binding);
			}
			if (parentInternal == null)
			{
				BindingOperations.ClearBinding(this, DefinitionBase.PrivateSharedSizeScopeProperty);
			}
		}

		/// <summary>Called when the template's tree is generated.</summary>
		// Token: 0x06005241 RID: 21057 RVA: 0x00170078 File Offset: 0x0016E278
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._submenuPopup != null)
			{
				this._submenuPopup.Closed -= this.OnPopupClosed;
			}
			this._submenuPopup = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this._submenuPopup != null)
			{
				this._submenuPopup.Closed += this.OnPopupClosed;
			}
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001700E0 File Offset: 0x0016E2E0
		private void SetMenuMode(bool menuMode)
		{
			MenuBase menuBase = this.LogicalParent as MenuBase;
			if (menuBase != null && menuBase.IsMenuMode != menuMode)
			{
				menuBase.IsMenuMode = menuMode;
			}
		}

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06005243 RID: 21059 RVA: 0x0017010C File Offset: 0x0016E30C
		private bool IsInMenuMode
		{
			get
			{
				MenuBase menuBase = this.LogicalParent as MenuBase;
				return menuBase != null && menuBase.IsMenuMode;
			}
		}

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06005244 RID: 21060 RVA: 0x00170130 File Offset: 0x0016E330
		private bool OpenOnMouseEnter
		{
			get
			{
				MenuBase menuBase = this.LogicalParent as MenuBase;
				return menuBase != null && menuBase.OpenOnMouseEnter;
			}
		}

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06005245 RID: 21061 RVA: 0x00170154 File Offset: 0x0016E354
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private bool InsideContextMenu
		{
			get
			{
				return (bool)base.GetValue(MenuItem.InsideContextMenuProperty);
			}
		}

		// Token: 0x06005246 RID: 21062 RVA: 0x00170166 File Offset: 0x0016E366
		internal static void SetInsideContextMenuProperty(UIElement element, bool value)
		{
			element.SetValue(MenuItem.InsideContextMenuProperty, BooleanBoxes.Box(value));
		}

		// Token: 0x06005247 RID: 21063 RVA: 0x00170179 File Offset: 0x0016E379
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void ClickItem()
		{
			this.ClickItem(false);
		}

		// Token: 0x06005248 RID: 21064 RVA: 0x00170184 File Offset: 0x0016E384
		[SecurityCritical]
		private void ClickItem(bool userInitiated)
		{
			try
			{
				this.OnClickCore(userInitiated);
			}
			finally
			{
				if (this.Role == MenuItemRole.TopLevelItem && !this.StaysOpenOnClick)
				{
					this.SetMenuMode(false);
				}
			}
		}

		// Token: 0x06005249 RID: 21065 RVA: 0x001701C4 File Offset: 0x0016E3C4
		internal void ClickHeader()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				this.FocusOrSelect();
			}
			if (this.IsSubmenuOpen)
			{
				if (this.Role == MenuItemRole.TopLevelHeader)
				{
					this.SetMenuMode(false);
					return;
				}
			}
			else
			{
				this.OpenMenu();
			}
		}

		// Token: 0x0600524A RID: 21066 RVA: 0x001701F4 File Offset: 0x0016E3F4
		internal bool OpenMenu()
		{
			if (!this.IsSubmenuOpen)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
				if (itemsControl == null)
				{
					itemsControl = (VisualTreeHelper.GetParent(this) as ItemsControl);
				}
				if (itemsControl != null && (itemsControl is MenuItem || itemsControl is MenuBase))
				{
					base.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.TrueBox);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600524B RID: 21067 RVA: 0x00170245 File Offset: 0x0016E445
		internal void OpenSubmenuWithKeyboard()
		{
			MenuItem.SetBoolField(this, MenuItem.BoolField.OpenedWithKeyboard, true);
			if (this.OpenMenu())
			{
				base.NavigateToStart(new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers));
			}
		}

		// Token: 0x0600524C RID: 21068 RVA: 0x0017026C File Offset: 0x0016E46C
		private bool MenuItemNavigate(Key key, ModifierKeys modifiers)
		{
			if (key == Key.Left || key == Key.Right || key == Key.Up || key == Key.Down)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(this);
				if (itemsControl != null)
				{
					if (!itemsControl.HasItems)
					{
						return false;
					}
					int count = itemsControl.Items.Count;
					if (count == 1 && !(itemsControl is Menu) && key == Key.Up && key == Key.Down)
					{
						return true;
					}
					object focusedElement = Keyboard.FocusedElement;
					itemsControl.NavigateByLine(itemsControl.FocusedInfo, KeyboardNavigation.KeyToTraversalDirection(key), new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, modifiers));
					object focusedElement2 = Keyboard.FocusedElement;
					if (focusedElement2 != focusedElement && focusedElement2 != this)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x0600524D RID: 21069 RVA: 0x001702FA File Offset: 0x0016E4FA
		internal object LogicalParent
		{
			get
			{
				if (base.Parent != null)
				{
					return base.Parent;
				}
				return ItemsControl.ItemsControlFromItemContainer(this);
			}
		}

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x0600524E RID: 21070 RVA: 0x00170314 File Offset: 0x0016E514
		private MenuItem CurrentSibling
		{
			get
			{
				object logicalParent = this.LogicalParent;
				MenuItem menuItem = logicalParent as MenuItem;
				MenuItem menuItem2 = null;
				if (menuItem != null)
				{
					menuItem2 = menuItem.CurrentSelection;
				}
				else
				{
					MenuBase menuBase = logicalParent as MenuBase;
					if (menuBase != null)
					{
						menuItem2 = menuBase.CurrentSelection;
					}
				}
				if (menuItem2 == this)
				{
					menuItem2 = null;
				}
				return menuItem2;
			}
		}

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x0600524F RID: 21071 RVA: 0x00170358 File Offset: 0x0016E558
		private bool IsMouseOverSibling
		{
			get
			{
				FrameworkElement frameworkElement = this.LogicalParent as FrameworkElement;
				return frameworkElement != null && MenuItem.IsMouseReallyOver(frameworkElement) && !base.IsMouseOver;
			}
		}

		// Token: 0x06005250 RID: 21072 RVA: 0x00170388 File Offset: 0x0016E588
		private static bool IsMouseReallyOver(FrameworkElement elem)
		{
			bool isMouseOver = elem.IsMouseOver;
			return (!isMouseOver || Mouse.Captured != elem || Mouse.DirectlyOver != elem) && isMouseOver;
		}

		// Token: 0x06005251 RID: 21073 RVA: 0x001703B2 File Offset: 0x0016E5B2
		private void OpenHierarchy(MenuItemRole role)
		{
			this.FocusOrSelect();
			if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
			{
				this.OpenMenu();
			}
		}

		// Token: 0x06005252 RID: 21074 RVA: 0x001703C9 File Offset: 0x0016E5C9
		private void FocusOrSelect()
		{
			if (!base.IsKeyboardFocusWithin)
			{
				base.Focus();
			}
			if (!this.IsSelected)
			{
				base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
			}
			if (this.IsSelected && !this.IsHighlighted)
			{
				this.IsHighlighted = true;
			}
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x0017040C File Offset: 0x0016E60C
		private void SetTimerToOpenHierarchy()
		{
			if (this._openHierarchyTimer == null)
			{
				this._openHierarchyTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._openHierarchyTimer.Tick += delegate(object sender, EventArgs e)
				{
					this.OpenHierarchy(this.Role);
					this.StopTimer(ref this._openHierarchyTimer);
				};
			}
			else
			{
				this._openHierarchyTimer.Stop();
			}
			this.StartTimer(this._openHierarchyTimer);
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x00170460 File Offset: 0x0016E660
		private void SetTimerToCloseHierarchy()
		{
			if (this._closeHierarchyTimer == null)
			{
				this._closeHierarchyTimer = new DispatcherTimer(DispatcherPriority.Normal);
				this._closeHierarchyTimer.Tick += delegate(object sender, EventArgs e)
				{
					base.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
					this.StopTimer(ref this._closeHierarchyTimer);
				};
			}
			else
			{
				this._closeHierarchyTimer.Stop();
			}
			this.StartTimer(this._closeHierarchyTimer);
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x001704B2 File Offset: 0x0016E6B2
		private void StopTimer(ref DispatcherTimer timer)
		{
			if (timer != null)
			{
				timer.Stop();
				timer = null;
			}
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x001704C2 File Offset: 0x0016E6C2
		private void StartTimer(DispatcherTimer timer)
		{
			timer.Interval = TimeSpan.FromMilliseconds((double)SystemParameters.MenuShowDelay);
			timer.Start();
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001704DC File Offset: 0x0016E6DC
		private static object OnCoerceAcceleratorKey(DependencyObject d, object value)
		{
			if (value == null)
			{
				string inputGestureText = ((MenuItem)d).InputGestureText;
				if (inputGestureText != string.Empty)
				{
					value = inputGestureText;
				}
			}
			return value;
		}

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06005258 RID: 21080 RVA: 0x00170509 File Offset: 0x0016E709
		// (set) Token: 0x06005259 RID: 21081 RVA: 0x00170514 File Offset: 0x0016E714
		private MenuItem CurrentSelection
		{
			get
			{
				return this._currentSelection;
			}
			set
			{
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this._currentSelection = value;
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
				}
			}
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x00170562 File Offset: 0x0016E762
		private static bool GetBoolField(UIElement element, MenuItem.BoolField field)
		{
			return ((MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) & field) > (MenuItem.BoolField)0;
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x0017057C File Offset: 0x0016E77C
		private static void SetBoolField(UIElement element, MenuItem.BoolField field, bool value)
		{
			if (value)
			{
				element.SetValue(MenuItem.BooleanFieldStoreProperty, (MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) | field);
				return;
			}
			element.SetValue(MenuItem.BooleanFieldStoreProperty, (MenuItem.BoolField)element.GetValue(MenuItem.BooleanFieldStoreProperty) & ~field);
		}

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x001705D2 File Offset: 0x0016E7D2
		// (set) Token: 0x0600525E RID: 21086 RVA: 0x001705DE File Offset: 0x0016E7DE
		private bool CanExecute
		{
			get
			{
				return !base.ReadControlFlag(Control.ControlBoolFlags.CommandDisabled);
			}
			set
			{
				if (value != this.CanExecute)
				{
					base.WriteControlFlag(Control.ControlBoolFlags.CommandDisabled, !value);
					base.CoerceValue(UIElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x001705FF File Offset: 0x0016E7FF
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return MenuItem._dType;
			}
		}

		/// <summary>Gets the resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a <see cref="T:System.Windows.Controls.Separator" />.</summary>
		/// <returns>The resource key for a style applied to a <see cref="T:System.Windows.Controls.MenuItem" /> when the <see cref="T:System.Windows.Controls.MenuItem" /> is a <see cref="T:System.Windows.Controls.Separator" />.</returns>
		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x00170606 File Offset: 0x0016E806
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.MenuItemSeparatorStyleKey;
			}
		}

		// Token: 0x04002C87 RID: 11399
		private static ComponentResourceKey _topLevelItemTemplateKey;

		// Token: 0x04002C88 RID: 11400
		private static ComponentResourceKey _topLevelHeaderTemplateKey;

		// Token: 0x04002C89 RID: 11401
		private static ComponentResourceKey _submenuItemTemplateKey;

		// Token: 0x04002C8A RID: 11402
		private static ComponentResourceKey _submenuHeaderTemplateKey;

		// Token: 0x04002C8C RID: 11404
		internal static readonly RoutedEvent PreviewClickEvent;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.Command" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.Command" /> dependency property.</returns>
		// Token: 0x04002C91 RID: 11409
		public static readonly DependencyProperty CommandProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.CommandParameter" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.CommandParameter" /> dependency property.</returns>
		// Token: 0x04002C92 RID: 11410
		public static readonly DependencyProperty CommandParameterProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.CommandTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.CommandTarget" /> dependency property.</returns>
		// Token: 0x04002C93 RID: 11411
		public static readonly DependencyProperty CommandTargetProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsSubmenuOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsSubmenuOpen" /> dependency property.</returns>
		// Token: 0x04002C94 RID: 11412
		public static readonly DependencyProperty IsSubmenuOpenProperty;

		// Token: 0x04002C95 RID: 11413
		private static readonly DependencyPropertyKey RolePropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.Role" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="T:System.Windows.Controls.MenuItemRole" /> dependency property.</returns>
		// Token: 0x04002C96 RID: 11414
		public static readonly DependencyProperty RoleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsCheckable" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsCheckable" /> dependency property.</returns>
		// Token: 0x04002C97 RID: 11415
		public static readonly DependencyProperty IsCheckableProperty;

		// Token: 0x04002C98 RID: 11416
		private static readonly DependencyPropertyKey IsPressedPropertyKey;

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsPressed" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsPressed" /> dependency property.</returns>
		// Token: 0x04002C99 RID: 11417
		public static readonly DependencyProperty IsPressedProperty;

		// Token: 0x04002C9A RID: 11418
		private static readonly DependencyPropertyKey IsHighlightedPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsHighlighted" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsHighlighted" /> dependency property.</returns>
		// Token: 0x04002C9B RID: 11419
		public static readonly DependencyProperty IsHighlightedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsChecked" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsChecked" /> dependency property.</returns>
		// Token: 0x04002C9C RID: 11420
		public static readonly DependencyProperty IsCheckedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.StaysOpenOnClick" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.StaysOpenOnClick" /> dependency property.</returns>
		// Token: 0x04002C9D RID: 11421
		public static readonly DependencyProperty StaysOpenOnClickProperty;

		// Token: 0x04002C9E RID: 11422
		internal static readonly DependencyProperty IsSelectedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.InputGestureText" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.InputGestureText" /> dependency property.</returns>
		// Token: 0x04002C9F RID: 11423
		public static readonly DependencyProperty InputGestureTextProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.Icon" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.Icon" /> dependency property.</returns>
		// Token: 0x04002CA0 RID: 11424
		public static readonly DependencyProperty IconProperty;

		// Token: 0x04002CA1 RID: 11425
		private static readonly DependencyPropertyKey IsSuspendingPopupAnimationPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.IsSuspendingPopupAnimation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.IsSuspendingPopupAnimation" /> dependency property.</returns>
		// Token: 0x04002CA2 RID: 11426
		public static readonly DependencyProperty IsSuspendingPopupAnimationProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.ItemContainerTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.ItemContainerTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002CA3 RID: 11427
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.MenuItem.UsesItemContainerTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.MenuItem.UsesItemContainerTemplate" /> dependency property.</returns>
		// Token: 0x04002CA4 RID: 11428
		public static readonly DependencyProperty UsesItemContainerTemplateProperty;

		// Token: 0x04002CA5 RID: 11429
		private object _currentItem;

		// Token: 0x04002CA6 RID: 11430
		internal static readonly DependencyProperty InsideContextMenuProperty;

		// Token: 0x04002CA7 RID: 11431
		private static readonly DependencyProperty BooleanFieldStoreProperty;

		// Token: 0x04002CA8 RID: 11432
		private const string PopupTemplateName = "PART_Popup";

		// Token: 0x04002CA9 RID: 11433
		private MenuItem _currentSelection;

		// Token: 0x04002CAA RID: 11434
		private Popup _submenuPopup;

		// Token: 0x04002CAB RID: 11435
		private DispatcherTimer _openHierarchyTimer;

		// Token: 0x04002CAC RID: 11436
		private DispatcherTimer _closeHierarchyTimer;

		// Token: 0x04002CAD RID: 11437
		[SecurityCritical]
		private bool _userInitiatedPress;

		// Token: 0x04002CAE RID: 11438
		private static DependencyObjectType _dType;

		// Token: 0x020009A9 RID: 2473
		[Flags]
		private enum BoolField
		{
			// Token: 0x04004504 RID: 17668
			OpenedWithKeyboard = 1,
			// Token: 0x04004505 RID: 17669
			IgnoreNextMouseLeave = 2,
			// Token: 0x04004506 RID: 17670
			IgnoreMouseEvents = 4,
			// Token: 0x04004507 RID: 17671
			MouseEnterOnMouseMove = 8,
			// Token: 0x04004508 RID: 17672
			CanExecuteInvalid = 16
		}
	}
}
