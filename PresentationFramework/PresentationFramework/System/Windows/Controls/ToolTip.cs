using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that creates a pop-up window that displays information for an element in the interface. </summary>
	// Token: 0x02000546 RID: 1350
	[DefaultEvent("Opened")]
	[Localizability(LocalizationCategory.ToolTip)]
	public class ToolTip : ContentControl
	{
		// Token: 0x0600586C RID: 22636 RVA: 0x00188214 File Offset: 0x00186414
		static ToolTip()
		{
			System.Windows.Controls.ToolTip.OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToolTip));
			System.Windows.Controls.ToolTip.ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToolTip));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(typeof(ToolTip)));
			System.Windows.Controls.ToolTip._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolTip));
			Control.BackgroundProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(SystemColors.InfoBrush));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ToolTip), new FrameworkPropertyMetadata(false));
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x0600586E RID: 22638 RVA: 0x0018847E File Offset: 0x0018667E
		// (set) Token: 0x0600586F RID: 22639 RVA: 0x00188490 File Offset: 0x00186690
		[Bindable(true)]
		[Category("Behavior")]
		internal bool FromKeyboard
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.FromKeyboardProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.FromKeyboardProperty, value);
			}
		}

		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06005870 RID: 22640 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool ShouldShowOnKeyboardFocus
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x0018849E File Offset: 0x0018669E
		private static object CoerceHorizontalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.HorizontalOffsetProperty);
		}

		/// <summary>Get or sets the horizontal distance between the target origin and the popup alignment point. </summary>
		/// <returns>The horizontal distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06005872 RID: 22642 RVA: 0x001884AC File Offset: 0x001866AC
		// (set) Token: 0x06005873 RID: 22643 RVA: 0x001884BE File Offset: 0x001866BE
		[TypeConverter(typeof(LengthConverter))]
		[Bindable(true)]
		[Category("Layout")]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(System.Windows.Controls.ToolTip.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001884D1 File Offset: 0x001866D1
		private static object CoerceVerticalOffset(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.VerticalOffsetProperty);
		}

		/// <summary>Get or sets the vertical distance between the target origin and the popup alignment point. </summary>
		/// <returns>The vertical distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x1700158E RID: 5518
		// (get) Token: 0x06005875 RID: 22645 RVA: 0x001884DF File Offset: 0x001866DF
		// (set) Token: 0x06005876 RID: 22646 RVA: 0x001884F1 File Offset: 0x001866F1
		[TypeConverter(typeof(LengthConverter))]
		[Bindable(true)]
		[Category("Layout")]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(System.Windows.Controls.ToolTip.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.VerticalOffsetProperty, value);
			}
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x00188504 File Offset: 0x00186704
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToolTip toolTip = (ToolTip)d;
			if ((bool)e.NewValue)
			{
				if (toolTip._parentPopup == null)
				{
					toolTip.HookupParentPopup();
				}
			}
			else if (AutomationPeer.ListenerExists(AutomationEvents.ToolTipClosed))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(toolTip);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.ToolTipClosed);
				}
			}
			Control.OnVisualStatePropertyChanged(d, e);
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.ToolTip" /> is visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ToolTip" /> is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700158F RID: 5519
		// (get) Token: 0x06005878 RID: 22648 RVA: 0x00188556 File Offset: 0x00186756
		// (set) Token: 0x06005879 RID: 22649 RVA: 0x00188568 File Offset: 0x00186768
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.IsOpenProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.IsOpenProperty, value);
			}
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x00188578 File Offset: 0x00186778
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			ToolTip toolTip = (ToolTip)d;
			if (toolTip._parentPopup == null || !toolTip._parentPopup.AllowsTransparency || !SystemParameters.DropShadow)
			{
				return BooleanBoxes.FalseBox;
			}
			return PopupControlService.CoerceProperty(d, value, ToolTipService.HasDropShadowProperty);
		}

		/// <summary>Gets or sets a value that indicates whether the control has a drop shadow.  </summary>
		/// <returns>
		///     <see langword="true" /> if the control has a drop shadow; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x0600587B RID: 22651 RVA: 0x001885BA File Offset: 0x001867BA
		// (set) Token: 0x0600587C RID: 22652 RVA: 0x001885CC File Offset: 0x001867CC
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.HasDropShadowProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.HasDropShadowProperty, value);
			}
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x001885DA File Offset: 0x001867DA
		private static object CoercePlacementTarget(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementTargetProperty);
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.UIElement" /> relative to which the <see cref="T:System.Windows.Controls.ToolTip" /> is positioned when it opens.  </summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that is the logical parent of the <see cref="T:System.Windows.Controls.ToolTip" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x0600587E RID: 22654 RVA: 0x001885E8 File Offset: 0x001867E8
		// (set) Token: 0x0600587F RID: 22655 RVA: 0x001885FA File Offset: 0x001867FA
		[Bindable(true)]
		[Category("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(System.Windows.Controls.ToolTip.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementTargetProperty, value);
			}
		}

		// Token: 0x06005880 RID: 22656 RVA: 0x00188608 File Offset: 0x00186808
		private static object CoercePlacementRectangle(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementRectangleProperty);
		}

		/// <summary>Gets or sets the rectangular area relative to which the <see cref="T:System.Windows.Controls.ToolTip" /> control is positioned when it opens.  </summary>
		/// <returns>The <see cref="T:System.Windows.Rect" /> structure that defines the rectangle that is used to position the <see cref="T:System.Windows.Controls.ToolTip" /> control. The default is <see cref="P:System.Windows.Rect.Empty" />.</returns>
		// Token: 0x17001592 RID: 5522
		// (get) Token: 0x06005881 RID: 22657 RVA: 0x00188616 File Offset: 0x00186816
		// (set) Token: 0x06005882 RID: 22658 RVA: 0x00188628 File Offset: 0x00186828
		[Bindable(true)]
		[Category("Layout")]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(System.Windows.Controls.ToolTip.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x06005883 RID: 22659 RVA: 0x0018863B File Offset: 0x0018683B
		private static object CoercePlacement(DependencyObject d, object value)
		{
			return PopupControlService.CoerceProperty(d, value, ToolTipService.PlacementProperty);
		}

		/// <summary>Gets or sets the orientation of the <see cref="T:System.Windows.Controls.ToolTip" /> control when it opens, and specifies how the <see cref="T:System.Windows.Controls.ToolTip" /> control behaves when it overlaps screen boundaries.   </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Primitives.PlacementMode" /> enumeration value that determines the orientation of the <see cref="T:System.Windows.Controls.ToolTip" /> control when it opens, and that specifies how the control interacts with screen boundaries. The default is <see cref="F:System.Windows.Controls.Primitives.PlacementMode.Mouse" />.</returns>
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x06005884 RID: 22660 RVA: 0x00188649 File Offset: 0x00186849
		// (set) Token: 0x06005885 RID: 22661 RVA: 0x0018865B File Offset: 0x0018685B
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(System.Windows.Controls.ToolTip.PlacementProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.PlacementProperty, value);
			}
		}

		/// <summary>Gets or sets the delegate handler method to use to position the <see cref="T:System.Windows.Controls.ToolTip" />.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacementCallback" /> delegate method that provides placement information for the <see cref="T:System.Windows.Controls.ToolTip" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x06005886 RID: 22662 RVA: 0x0018866E File Offset: 0x0018686E
		// (set) Token: 0x06005887 RID: 22663 RVA: 0x00188680 File Offset: 0x00186880
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.CustomPopupPlacementCallbackProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether an open <see cref="T:System.Windows.Controls.ToolTip" /> remains open until the user clicks the mouse when the mouse is not over the <see cref="T:System.Windows.Controls.ToolTip" />.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.ToolTip" /> stays open until it is closed by the user clicking the mouse button outside the <see cref="T:System.Windows.Controls.ToolTip" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Occurs when <see cref="P:System.Windows.Controls.ToolTip.StaysOpen" /> is set to <see langword="false" /> when a tooltip is not open.</exception>
		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x06005888 RID: 22664 RVA: 0x0018868E File Offset: 0x0018688E
		// (set) Token: 0x06005889 RID: 22665 RVA: 0x001886A0 File Offset: 0x001868A0
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(System.Windows.Controls.ToolTip.StaysOpenProperty);
			}
			set
			{
				base.SetValue(System.Windows.Controls.ToolTip.StaysOpenProperty, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ToolTip" /> becomes visible.</summary>
		// Token: 0x14000108 RID: 264
		// (add) Token: 0x0600588A RID: 22666 RVA: 0x001886AE File Offset: 0x001868AE
		// (remove) Token: 0x0600588B RID: 22667 RVA: 0x001886BC File Offset: 0x001868BC
		public event RoutedEventHandler Opened
		{
			add
			{
				base.AddHandler(System.Windows.Controls.ToolTip.OpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(System.Windows.Controls.ToolTip.OpenedEvent, value);
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.Controls.ToolTip.Opened" /> event. </summary>
		/// <param name="e">The event information.</param>
		// Token: 0x0600588C RID: 22668 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnOpened(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.ToolTip" /> is closed and is no longer visible. </summary>
		// Token: 0x14000109 RID: 265
		// (add) Token: 0x0600588D RID: 22669 RVA: 0x001886CA File Offset: 0x001868CA
		// (remove) Token: 0x0600588E RID: 22670 RVA: 0x001886D8 File Offset: 0x001868D8
		public event RoutedEventHandler Closed
		{
			add
			{
				base.AddHandler(System.Windows.Controls.ToolTip.ClosedEvent, value);
			}
			remove
			{
				base.RemoveHandler(System.Windows.Controls.ToolTip.ClosedEvent, value);
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.Controls.ToolTip.Closed" /> event.</summary>
		/// <param name="e">The event information.</param>
		// Token: 0x0600588F RID: 22671 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnClosed(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x06005890 RID: 22672 RVA: 0x001886E6 File Offset: 0x001868E6
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.IsOpen)
			{
				VisualStateManager.GoToState(this, "Open", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Closed", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Creates the implementation of <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.ToolTip" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" /> for this <see cref="T:System.Windows.Controls.ToolTip" /> control.</returns>
		// Token: 0x06005891 RID: 22673 RVA: 0x00188713 File Offset: 0x00186913
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToolTipAutomationPeer(this);
		}

		/// <summary>Responds to a change in the visual parent of a <see cref="T:System.Windows.Controls.ToolTip" />.</summary>
		/// <param name="oldParent">The previous visual parent.</param>
		// Token: 0x06005892 RID: 22674 RVA: 0x0018871B File Offset: 0x0018691B
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ToolTip"
				}));
			}
		}

		// Token: 0x06005893 RID: 22675 RVA: 0x00188750 File Offset: 0x00186950
		internal override void OnAncestorChanged()
		{
			base.OnAncestorChanged();
			if (!Popup.IsRootedInPopup(this._parentPopup, this))
			{
				throw new InvalidOperationException(SR.Get("ElementMustBeInPopup", new object[]
				{
					"ToolTip"
				}));
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes. </summary>
		/// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
		/// <param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property.</param>
		// Token: 0x06005894 RID: 22676 RVA: 0x00188784 File Offset: 0x00186984
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			PopupControlService popupControlService = PopupControlService.Current;
			if (this == popupControlService.CurrentToolTip && (bool)base.GetValue(PopupControlService.ServiceOwnedProperty) && newContent is ToolTip)
			{
				popupControlService.OnRaiseToolTipClosingEvent(null, EventArgs.Empty);
				popupControlService.OnRaiseToolTipOpeningEvent(null, EventArgs.Empty);
				return;
			}
			base.OnContentChanged(oldContent, newContent);
		}

		// Token: 0x06005895 RID: 22677 RVA: 0x001887DC File Offset: 0x001869DC
		private void HookupParentPopup()
		{
			this._parentPopup = new Popup();
			this._parentPopup.AllowsTransparency = true;
			this._parentPopup.HitTestable = !this.StaysOpen;
			base.CoerceValue(System.Windows.Controls.ToolTip.HasDropShadowProperty);
			this._parentPopup.Opened += this.OnPopupOpened;
			this._parentPopup.Closed += this.OnPopupClosed;
			this._parentPopup.PopupCouldClose += this.OnPopupCouldClose;
			this._parentPopup.SetResourceReference(Popup.PopupAnimationProperty, SystemParameters.ToolTipPopupAnimationKey);
			Popup.CreateRootPopupInternal(this._parentPopup, this, true);
		}

		// Token: 0x06005896 RID: 22678 RVA: 0x00188886 File Offset: 0x00186A86
		internal void ForceClose()
		{
			if (this._parentPopup != null)
			{
				this._parentPopup.ForceClose();
			}
		}

		// Token: 0x06005897 RID: 22679 RVA: 0x0018889B File Offset: 0x00186A9B
		private void OnPopupCouldClose(object sender, EventArgs e)
		{
			base.SetCurrentValueInternal(System.Windows.Controls.ToolTip.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x06005898 RID: 22680 RVA: 0x001888B0 File Offset: 0x00186AB0
		private void OnPopupOpened(object source, EventArgs e)
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.ToolTipOpened))
			{
				AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (peer != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
					{
						peer.RaiseAutomationEvent(AutomationEvents.ToolTipOpened);
						return null;
					}), null);
				}
			}
			this.OnOpened(new RoutedEventArgs(System.Windows.Controls.ToolTip.OpenedEvent, this));
		}

		// Token: 0x06005899 RID: 22681 RVA: 0x0018890A File Offset: 0x00186B0A
		private void OnPopupClosed(object source, EventArgs e)
		{
			this.OnClosed(new RoutedEventArgs(System.Windows.Controls.ToolTip.ClosedEvent, this));
		}

		// Token: 0x17001596 RID: 5526
		// (get) Token: 0x0600589A RID: 22682 RVA: 0x0018891D File Offset: 0x00186B1D
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return System.Windows.Controls.ToolTip._dType;
			}
		}

		// Token: 0x04002EC5 RID: 11973
		internal static readonly DependencyProperty FromKeyboardProperty = DependencyProperty.Register("FromKeyboard", typeof(bool), typeof(ToolTip), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.HorizontalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.HorizontalOffset" /> dependency property.</returns>
		// Token: 0x04002EC6 RID: 11974
		public static readonly DependencyProperty HorizontalOffsetProperty = ToolTipService.HorizontalOffsetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceHorizontalOffset)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.VerticalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.VerticalOffset" /> dependency property.</returns>
		// Token: 0x04002EC7 RID: 11975
		public static readonly DependencyProperty VerticalOffsetProperty = ToolTipService.VerticalOffsetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceVerticalOffset)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.IsOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.IsOpen" /> dependency property.</returns>
		// Token: 0x04002EC8 RID: 11976
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(ToolTip), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(System.Windows.Controls.ToolTip.OnIsOpenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.HasDropShadow" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.HasDropShadow" /> dependency property.</returns>
		// Token: 0x04002EC9 RID: 11977
		public static readonly DependencyProperty HasDropShadowProperty = ToolTipService.HasDropShadowProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoerceHasDropShadow)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.PlacementTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.PlacementTarget" /> dependency property.</returns>
		// Token: 0x04002ECA RID: 11978
		public static readonly DependencyProperty PlacementTargetProperty = ToolTipService.PlacementTargetProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacementTarget)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.PlacementRectangle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.PlacementRectangle" /> dependency property.</returns>
		// Token: 0x04002ECB RID: 11979
		public static readonly DependencyProperty PlacementRectangleProperty = ToolTipService.PlacementRectangleProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacementRectangle)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.Placement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.Placement" /> dependency property.</returns>
		// Token: 0x04002ECC RID: 11980
		public static readonly DependencyProperty PlacementProperty = ToolTipService.PlacementProperty.AddOwner(typeof(ToolTip), new FrameworkPropertyMetadata(null, new CoerceValueCallback(System.Windows.Controls.ToolTip.CoercePlacement)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.CustomPopupPlacementCallback" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.CustomPopupPlacementCallback" /> dependency property.</returns>
		// Token: 0x04002ECD RID: 11981
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = Popup.CustomPopupPlacementCallbackProperty.AddOwner(typeof(ToolTip));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolTip.StaysOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolTip.StaysOpen" /> dependency property.</returns>
		// Token: 0x04002ECE RID: 11982
		public static readonly DependencyProperty StaysOpenProperty = Popup.StaysOpenProperty.AddOwner(typeof(ToolTip));

		// Token: 0x04002ED1 RID: 11985
		private Popup _parentPopup;

		// Token: 0x04002ED2 RID: 11986
		private static DependencyObjectType _dType;

		// Token: 0x020009C5 RID: 2501
		internal enum ToolTipTrigger
		{
			// Token: 0x0400458A RID: 17802
			Mouse,
			// Token: 0x0400458B RID: 17803
			KeyboardFocus,
			// Token: 0x0400458C RID: 17804
			KeyboardShortcut
		}
	}
}
