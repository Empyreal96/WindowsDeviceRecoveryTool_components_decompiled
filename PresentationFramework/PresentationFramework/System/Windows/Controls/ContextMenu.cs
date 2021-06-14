using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a pop-up menu that enables a control to expose functionality that is specific to the context of the control. </summary>
	// Token: 0x0200048A RID: 1162
	[DefaultEvent("Opened")]
	public class ContextMenu : MenuBase
	{
		// Token: 0x0600442E RID: 17454 RVA: 0x00136C74 File Offset: 0x00134E74
		static ContextMenu()
		{
			ContextMenu.OpenedEvent = PopupControlService.ContextMenuOpenedEvent.AddOwner(typeof(ContextMenu));
			ContextMenu.ClosedEvent = PopupControlService.ContextMenuClosedEvent.AddOwner(typeof(ContextMenu));
			ContextMenu.InsideContextMenuProperty = MenuItem.InsideContextMenuProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(typeof(ContextMenu), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(ContextMenu.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(typeof(ContextMenu)));
			ContextMenu._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ContextMenu));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof(ContextMenu), new FrameworkPropertyMetadata(null));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ContextMenu" /> class.</summary>
		// Token: 0x0600442F RID: 17455 RVA: 0x00136F2C File Offset: 0x0013512C
		public ContextMenu()
		{
			this.Initialize();
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x00136F3A File Offset: 0x0013513A
		private static object CoerceHorizontalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.HorizontalOffsetProperty);
		}

		/// <summary>Get or sets the horizontal distance between the target origin and the popup alignment point. </summary>
		/// <returns>The horizontal distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x00136F48 File Offset: 0x00135148
		// (set) Token: 0x06004432 RID: 17458 RVA: 0x00136F5A File Offset: 0x0013515A
		[TypeConverter(typeof(LengthConverter))]
		[Bindable(true)]
		[Category("Layout")]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(ContextMenu.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00136F6D File Offset: 0x0013516D
		private static object CoerceVerticalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.VerticalOffsetProperty);
		}

		/// <summary>Get or sets the vertical distance between the target origin and the popup alignment point. </summary>
		/// <returns>The vertical distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x00136F7B File Offset: 0x0013517B
		// (set) Token: 0x06004435 RID: 17461 RVA: 0x00136F8D File Offset: 0x0013518D
		[TypeConverter(typeof(LengthConverter))]
		[Bindable(true)]
		[Category("Layout")]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(ContextMenu.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.VerticalOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ContextMenu" /> is visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ContextMenu" /> is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x00136FA0 File Offset: 0x001351A0
		// (set) Token: 0x06004437 RID: 17463 RVA: 0x00136FB2 File Offset: 0x001351B2
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.IsOpenProperty);
			}
			set
			{
				base.SetValue(ContextMenu.IsOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x00136FC8 File Offset: 0x001351C8
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContextMenu contextMenu = (ContextMenu)d;
			if ((bool)e.NewValue)
			{
				if (contextMenu._parentPopup == null)
				{
					contextMenu.HookupParentPopup();
				}
				contextMenu._parentPopup.Unloaded += contextMenu.OnPopupUnloaded;
				contextMenu.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, KeyboardNavigation.IsKeyboardMostRecentInputDevice());
				return;
			}
			contextMenu.ClosingMenu();
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x00137026 File Offset: 0x00135226
		private static object CoercePlacementTarget(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementTargetProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.UIElement" /> relative to which the <see cref="T:System.Windows.Controls.ContextMenu" /> is positioned when it opens.  </summary>
		/// <returns>The element relative to which the <see cref="T:System.Windows.Controls.ContextMenu" /> is positioned when it opens. The default is <see langword="null" />.</returns>
		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x00137034 File Offset: 0x00135234
		// (set) Token: 0x0600443B RID: 17467 RVA: 0x00137046 File Offset: 0x00135246
		[Bindable(true)]
		[Category("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(ContextMenu.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementTargetProperty, value);
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x00137054 File Offset: 0x00135254
		private static object CoercePlacementRectangle(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementRectangleProperty);
		}

		/// <summary>Gets or sets the area relative to which the context menu is positioned when it opens.  </summary>
		/// <returns>The area that defines the rectangle that is used to position the context menu. The default is <see cref="P:System.Windows.Rect.Empty" />.</returns>
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x0600443D RID: 17469 RVA: 0x00137062 File Offset: 0x00135262
		// (set) Token: 0x0600443E RID: 17470 RVA: 0x00137074 File Offset: 0x00135274
		[Bindable(true)]
		[Category("Layout")]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(ContextMenu.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00137087 File Offset: 0x00135287
		private static object CoercePlacement(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.PlacementProperty);
		}

		/// <summary> Gets or sets the <see cref="P:System.Windows.Controls.ContextMenu.Placement" /> property of a <see cref="T:System.Windows.Controls.ContextMenu" />.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.Primitives.PlacementMode" /> enumeration. The default is <see cref="F:System.Windows.Controls.Primitives.PlacementMode.MousePoint" />.</returns>
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06004440 RID: 17472 RVA: 0x00137095 File Offset: 0x00135295
		// (set) Token: 0x06004441 RID: 17473 RVA: 0x001370A7 File Offset: 0x001352A7
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(ContextMenu.PlacementProperty);
			}
			set
			{
				base.SetValue(ContextMenu.PlacementProperty, value);
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x001370BC File Offset: 0x001352BC
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			ContextMenu contextMenu = (ContextMenu)d;
			if (contextMenu._parentPopup == null || !contextMenu._parentPopup.AllowsTransparency || !SystemParameters.DropShadow)
			{
				return BooleanBoxes.FalseBox;
			}
			return PopupControlService.CoerceProperty(d, value, ContextMenuService.HasDropShadowProperty);
		}

		/// <summary>Gets or sets a value that indicates whether the context menu appears with a dropped shadow.  </summary>
		/// <returns>
		///     <see langword="true" /> if the context menu appears with a dropped shadow; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x001370FE File Offset: 0x001352FE
		// (set) Token: 0x06004444 RID: 17476 RVA: 0x00137110 File Offset: 0x00135310
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.HasDropShadowProperty);
			}
			set
			{
				base.SetValue(ContextMenu.HasDropShadowProperty, value);
			}
		}

		/// <summary> Gets or sets a callback that indicates where a <see cref="T:System.Windows.Controls.ContextMenu" /> should be placed on the screen.  </summary>
		/// <returns>A callback that specifies the location of the <see cref="T:System.Windows.Controls.ContextMenu" />.</returns>
		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06004445 RID: 17477 RVA: 0x0013711E File Offset: 0x0013531E
		// (set) Token: 0x06004446 RID: 17478 RVA: 0x00137130 File Offset: 0x00135330
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(ContextMenu.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(ContextMenu.CustomPopupPlacementCallbackProperty, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ContextMenu" /> should close automatically.  </summary>
		/// <returns>
		///     <see langword="true" /> if the menu should stay open until the <see cref="P:System.Windows.Controls.ContextMenu.IsOpen" /> property changes to <see langword="false" />; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06004447 RID: 17479 RVA: 0x0013713E File Offset: 0x0013533E
		// (set) Token: 0x06004448 RID: 17480 RVA: 0x00137150 File Offset: 0x00135350
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(ContextMenu.StaysOpenProperty);
			}
			set
			{
				base.SetValue(ContextMenu.StaysOpenProperty, value);
			}
		}

		/// <summary>Occurs when a particular instance of a context menu opens. </summary>
		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06004449 RID: 17481 RVA: 0x0013715E File Offset: 0x0013535E
		// (remove) Token: 0x0600444A RID: 17482 RVA: 0x0013716C File Offset: 0x0013536C
		public event RoutedEventHandler Opened
		{
			add
			{
				base.AddHandler(ContextMenu.OpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ContextMenu.OpenedEvent, value);
			}
		}

		/// <summary>Called when the <see cref="E:System.Windows.Controls.ContextMenu.Opened" /> event occurs. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ContextMenu.Opened" /> event.</param>
		// Token: 0x0600444B RID: 17483 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when a particular instance of a <see cref="T:System.Windows.Controls.ContextMenu" /> closes. </summary>
		// Token: 0x140000AA RID: 170
		// (add) Token: 0x0600444C RID: 17484 RVA: 0x0013717A File Offset: 0x0013537A
		// (remove) Token: 0x0600444D RID: 17485 RVA: 0x00137188 File Offset: 0x00135388
		public event RoutedEventHandler Closed
		{
			add
			{
				base.AddHandler(ContextMenu.ClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ContextMenu.ClosedEvent, value);
			}
		}

		/// <summary>Called when the <see cref="E:System.Windows.Controls.ContextMenu.Closed" /> event occurs. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.ContextMenu.Closed" /> event.</param>
		// Token: 0x0600444E RID: 17486 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Creates and returns a <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.ContextMenu" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" /> object for this <see cref="T:System.Windows.Controls.ContextMenu" />.</returns>
		// Token: 0x0600444F RID: 17487 RVA: 0x00137196 File Offset: 0x00135396
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ContextMenuAutomationPeer(this);
		}

		/// <summary> Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x06004450 RID: 17488 RVA: 0x0013719E File Offset: 0x0013539E
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		/// <summary>Gets a value that indicates whether the control supports scrolling.</summary>
		/// <returns>Always <see langword="true" />.</returns>
		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06004451 RID: 17489 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		/// <summary>Called when a <see cref="E:System.Windows.ContentElement.KeyDown" /> event is raised by an object inside the <see cref="T:System.Windows.Controls.ContextMenu" />. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x06004452 RID: 17490 RVA: 0x001371B0 File Offset: 0x001353B0
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled || !this.IsOpen)
			{
				return;
			}
			Key key = e.Key;
			if (key != Key.Up)
			{
				if (key == Key.Down && base.CurrentSelection == null)
				{
					base.NavigateToStart(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
					e.Handled = true;
					return;
				}
			}
			else if (base.CurrentSelection == null)
			{
				base.NavigateToEnd(new ItemsControl.ItemNavigateArgs(e.Device, Keyboard.Modifiers));
				e.Handled = true;
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.ContentElement.KeyUp" /> event.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyUp" /> event.</param>
		// Token: 0x06004453 RID: 17491 RVA: 0x00137231 File Offset: 0x00135431
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!e.Handled && this.IsOpen && e.Key == Key.Apps)
			{
				base.KeyboardLeaveMenuMode();
				e.Handled = true;
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x00137261 File Offset: 0x00135461
		private void Initialize()
		{
			MenuItem.SetInsideContextMenuProperty(this, true);
			base.InternalMenuModeChanged += this.OnIsMenuModeChanged;
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0013727C File Offset: 0x0013547C
		private void HookupParentPopup()
		{
			this._parentPopup = new Popup();
			this._parentPopup.AllowsTransparency = true;
			base.CoerceValue(ContextMenu.HasDropShadowProperty);
			this._parentPopup.DropOpposite = false;
			this._parentPopup.Opened += this.OnPopupOpened;
			this._parentPopup.Closed += this.OnPopupClosed;
			this._parentPopup.PopupCouldClose += this.OnPopupCouldClose;
			this._parentPopup.SetResourceReference(Popup.PopupAnimationProperty, SystemParameters.MenuPopupAnimationKey);
			Popup.CreateRootPopup(this._parentPopup, this);
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0013731D File Offset: 0x0013551D
		private void OnPopupCouldClose(object sender, EventArgs e)
		{
			base.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x00137330 File Offset: 0x00135530
		private void OnPopupOpened(object source, EventArgs e)
		{
			if (base.CurrentSelection != null)
			{
				base.CurrentSelection = null;
			}
			base.IsMenuMode = true;
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				base.IgnoreNextLeftRelease = true;
			}
			if (Mouse.RightButton == MouseButtonState.Pressed)
			{
				base.IgnoreNextRightRelease = true;
			}
			this.OnOpened(new RoutedEventArgs(ContextMenu.OpenedEvent, this));
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00137382 File Offset: 0x00135582
		private void OnPopupClosed(object source, EventArgs e)
		{
			base.IgnoreNextLeftRelease = false;
			base.IgnoreNextRightRelease = false;
			base.IsMenuMode = false;
			this.OnClosed(new RoutedEventArgs(ContextMenu.ClosedEvent, this));
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x001373AC File Offset: 0x001355AC
		private void ClosingMenu()
		{
			if (this._parentPopup != null)
			{
				this._parentPopup.Unloaded -= this.OnPopupUnloaded;
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object arg)
				{
					ContextMenu contextMenu = (ContextMenu)arg;
					if (!contextMenu.IsOpen)
					{
						FocusManager.SetFocusedElement(contextMenu, null);
					}
					return null;
				}), this);
			}
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x00137406 File Offset: 0x00135606
		private void OnPopupUnloaded(object sender, RoutedEventArgs e)
		{
			if (this.IsOpen)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object arg)
				{
					ContextMenu contextMenu = (ContextMenu)arg;
					if (contextMenu.IsOpen)
					{
						contextMenu.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
					}
					return null;
				}), this);
			}
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x00137440 File Offset: 0x00135640
		private void OnIsMenuModeChanged(object sender, EventArgs e)
		{
			if (base.IsMenuMode)
			{
				if (Keyboard.FocusedElement != null)
				{
					this._weakRefToPreviousFocus = new WeakReference<IInputElement>(Keyboard.FocusedElement);
				}
				base.Focus();
				return;
			}
			base.SetCurrentValueInternal(ContextMenu.IsOpenProperty, BooleanBoxes.FalseBox);
			if (this._weakRefToPreviousFocus != null)
			{
				IInputElement inputElement;
				if (this._weakRefToPreviousFocus.TryGetTarget(out inputElement))
				{
					inputElement.Focus();
				}
				this._weakRefToPreviousFocus = null;
			}
		}

		/// <summary>Reports that the <see cref="P:System.Windows.UIElement.IsKeyboardFocusWithin" /> property changed.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> event.</param>
		// Token: 0x0600445C RID: 17500 RVA: 0x001374A9 File Offset: 0x001356A9
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!(bool)e.NewValue)
			{
				this._weakRefToPreviousFocus = null;
			}
			base.OnIsKeyboardFocusWithinChanged(e);
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x001374C7 File Offset: 0x001356C7
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			return e is KeyEventArgs || e is FindToolTipEventArgs;
		}

		// Token: 0x0600445E RID: 17502 RVA: 0x001374DC File Offset: 0x001356DC
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			e.Scope = sender;
			e.Handled = true;
		}

		/// <summary>Called when a context menu's visual parent changes. </summary>
		/// <param name="oldParent">The object that the context menu was previously attached to.</param>
		// Token: 0x0600445F RID: 17503 RVA: 0x001374EC File Offset: 0x001356EC
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ContextMenu"
				}));
			}
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x00137521 File Offset: 0x00135721
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ContextMenu"
				}));
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06004461 RID: 17505 RVA: 0x00137555 File Offset: 0x00135755
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ContextMenu._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.HorizontalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.HorizontalOffset" /> dependency property.</returns>
		// Token: 0x04002881 RID: 10369
		public static readonly DependencyProperty HorizontalOffsetProperty = ContextMenuService.HorizontalOffsetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceHorizontalOffset)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.VerticalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.VerticalOffset" /> dependency property.</returns>
		// Token: 0x04002882 RID: 10370
		public static readonly DependencyProperty VerticalOffsetProperty = ContextMenuService.VerticalOffsetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceVerticalOffset)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.IsOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.IsOpen" /> dependency property.</returns>
		// Token: 0x04002883 RID: 10371
		public static readonly DependencyProperty IsOpenProperty = Popup.IsOpenProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ContextMenu.OnIsOpenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.PlacementTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.PlacementTarget" /> dependency property.</returns>
		// Token: 0x04002884 RID: 10372
		public static readonly DependencyProperty PlacementTargetProperty = ContextMenuService.PlacementTargetProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacementTarget)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.PlacementRectangle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.PlacementRectangle" /> dependency property.</returns>
		// Token: 0x04002885 RID: 10373
		public static readonly DependencyProperty PlacementRectangleProperty = ContextMenuService.PlacementRectangleProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacementRectangle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.Placement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.Placement" /> dependency property.</returns>
		// Token: 0x04002886 RID: 10374
		public static readonly DependencyProperty PlacementProperty = ContextMenuService.PlacementProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoercePlacement)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.HasDropShadow" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.HasDropShadow" /> dependency property.</returns>
		// Token: 0x04002887 RID: 10375
		public static readonly DependencyProperty HasDropShadowProperty = ContextMenuService.HasDropShadowProperty.AddOwner(typeof(ContextMenu), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ContextMenu.CoerceHasDropShadow)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.CustomPopupPlacementCallback" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.CustomPopupPlacementCallback" /> dependency property.</returns>
		// Token: 0x04002888 RID: 10376
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = Popup.CustomPopupPlacementCallbackProperty.AddOwner(typeof(ContextMenu));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ContextMenu.StaysOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ContextMenu.StaysOpen" /> dependency property.</returns>
		// Token: 0x04002889 RID: 10377
		public static readonly DependencyProperty StaysOpenProperty = Popup.StaysOpenProperty.AddOwner(typeof(ContextMenu));

		// Token: 0x0400288C RID: 10380
		private static readonly DependencyProperty InsideContextMenuProperty;

		// Token: 0x0400288D RID: 10381
		private Popup _parentPopup;

		// Token: 0x0400288E RID: 10382
		private WeakReference<IInputElement> _weakRefToPreviousFocus;

		// Token: 0x0400288F RID: 10383
		private static DependencyObjectType _dType;
	}
}
