using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Accessibility;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a pop-up window that has content.</summary>
	// Token: 0x0200059B RID: 1435
	[DefaultEvent("Opened")]
	[DefaultProperty("Child")]
	[Localizability(LocalizationCategory.None)]
	[ContentProperty("Child")]
	public class Popup : FrameworkElement, IAddChild
	{
		// Token: 0x06005EA4 RID: 24228 RVA: 0x001A8308 File Offset: 0x001A6508
		static Popup()
		{
			EventManager.RegisterClassHandler(typeof(Popup), Mouse.LostMouseCaptureEvent, new MouseEventHandler(Popup.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(Popup), DragDrop.DragDropStartedEvent, new RoutedEventHandler(Popup.OnDragDropStarted), true);
			EventManager.RegisterClassHandler(typeof(Popup), DragDrop.DragDropCompletedEvent, new RoutedEventHandler(Popup.OnDragDropCompleted), true);
			UIElement.VisibilityProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(VisibilityBoxes.CollapsedBox, null, new CoerceValueCallback(Popup.CoerceVisibility)));
		}

		// Token: 0x06005EA5 RID: 24229 RVA: 0x001A86FE File Offset: 0x001A68FE
		private static object CoerceVisibility(DependencyObject d, object value)
		{
			return VisibilityBoxes.CollapsedBox;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> class. </summary>
		// Token: 0x06005EA6 RID: 24230 RVA: 0x001A8705 File Offset: 0x001A6905
		public Popup()
		{
			this._secHelper = new Popup.PopupSecurityHelper();
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x06005EA7 RID: 24231 RVA: 0x001A8724 File Offset: 0x001A6924
		// (set) Token: 0x06005EA8 RID: 24232 RVA: 0x001A8736 File Offset: 0x001A6936
		internal bool TreatMousePlacementAsBottom
		{
			get
			{
				return (bool)base.GetValue(Popup.TreatMousePlacementAsBottomProperty);
			}
			set
			{
				base.SetValue(Popup.TreatMousePlacementAsBottomProperty, value);
			}
		}

		/// <summary>Gets or sets the content of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.  </summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> content of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x06005EA9 RID: 24233 RVA: 0x001A8744 File Offset: 0x001A6944
		// (set) Token: 0x06005EAA RID: 24234 RVA: 0x001A8756 File Offset: 0x001A6956
		[Bindable(true)]
		[CustomCategory("Content")]
		public UIElement Child
		{
			get
			{
				return (UIElement)base.GetValue(Popup.ChildProperty);
			}
			set
			{
				base.SetValue(Popup.ChildProperty, value);
			}
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x001A8764 File Offset: 0x001A6964
		private static void OnChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			UIElement child = (UIElement)e.OldValue;
			UIElement child2 = (UIElement)e.NewValue;
			if (popup._popupRoot.Value != null && (popup.IsOpen || popup._popupRoot.Value.Child != null))
			{
				popup._popupRoot.Value.Child = child2;
			}
			popup.RemoveLogicalChild(child);
			popup.AddLogicalChild(child2);
			popup.Reposition();
			popup.pushTextRenderingMode();
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x001A87E4 File Offset: 0x001A69E4
		internal override void pushTextRenderingMode()
		{
			if (this.Child != null && DependencyPropertyHelper.GetValueSource(this.Child, TextOptions.TextRenderingModeProperty).BaseValueSource <= BaseValueSource.Inherited)
			{
				this.Child.VisualTextRenderingMode = TextOptions.GetTextRenderingMode(this);
			}
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x001A8828 File Offset: 0x001A6A28
		private static void RegisterPopupWithPlacementTarget(Popup popup, UIElement placementTarget)
		{
			List<Popup> list = Popup.RegisteredPopupsField.GetValue(placementTarget);
			if (list == null)
			{
				list = new List<Popup>();
				Popup.RegisteredPopupsField.SetValue(placementTarget, list);
			}
			if (!list.Contains(popup))
			{
				list.Add(popup);
			}
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x001A8868 File Offset: 0x001A6A68
		private static void UnregisterPopupFromPlacementTarget(Popup popup, UIElement placementTarget)
		{
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(placementTarget);
			if (value != null)
			{
				value.Remove(popup);
				if (value.Count == 0)
				{
					Popup.RegisteredPopupsField.SetValue(placementTarget, null);
				}
			}
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x001A88A0 File Offset: 0x001A6AA0
		private void UpdatePlacementTargetRegistration(UIElement oldValue, UIElement newValue)
		{
			if (oldValue != null)
			{
				Popup.UnregisterPopupFromPlacementTarget(this, oldValue);
				if (newValue == null && VisualTreeHelper.GetParent(this) == null)
				{
					TreeWalkHelper.InvalidateOnTreeChange(this, null, oldValue, false);
				}
			}
			if (newValue != null && VisualTreeHelper.GetParent(this) == null)
			{
				Popup.RegisterPopupWithPlacementTarget(this, newValue);
				if (!base.IsSelfInheritanceParent)
				{
					base.SetIsSelfInheritanceParent();
				}
				TreeWalkHelper.InvalidateOnTreeChange(this, null, newValue, true);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x06005EB0 RID: 24240 RVA: 0x001A88F4 File Offset: 0x001A6AF4
		// (set) Token: 0x06005EB1 RID: 24241 RVA: 0x001A8906 File Offset: 0x001A6B06
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsOpen
		{
			get
			{
				return (bool)base.GetValue(Popup.IsOpenProperty);
			}
			set
			{
				base.SetValue(Popup.IsOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005EB2 RID: 24242 RVA: 0x001A891C File Offset: 0x001A6B1C
		private static object CoerceIsOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				Popup popup = (Popup)d;
				if (!popup.IsLoaded && VisualTreeHelper.GetParent(popup) != null)
				{
					popup.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x06005EB3 RID: 24243 RVA: 0x001A8955 File Offset: 0x001A6B55
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x06005EB4 RID: 24244 RVA: 0x001A8969 File Offset: 0x001A6B69
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(Popup.IsOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x001A8988 File Offset: 0x001A6B88
		private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			bool flag = (popup._secHelper.IsWindowAlive() && popup._asyncDestroy == null) || popup._asyncCreate != null;
			bool flag2 = (bool)e.NewValue;
			if (flag2 != flag)
			{
				if (flag2)
				{
					if (popup._cacheValid[4])
					{
						throw new InvalidOperationException(SR.Get("PopupReopeningNotAllowed"));
					}
					popup.CancelAsyncDestroy();
					popup.CancelAsyncCreate();
					popup.CreateWindow(false);
					if (popup._secHelper.IsWindowAlive())
					{
						if (Popup.CloseOnUnloadedHandler == null)
						{
							Popup.CloseOnUnloadedHandler = new RoutedEventHandler(Popup.CloseOnUnloaded);
						}
						popup.Unloaded += Popup.CloseOnUnloadedHandler;
						return;
					}
				}
				else
				{
					popup.CancelAsyncCreate();
					if (popup._secHelper.IsWindowAlive() && popup._asyncDestroy == null)
					{
						popup.HideWindow();
						if (Popup.CloseOnUnloadedHandler != null)
						{
							popup.Unloaded -= Popup.CloseOnUnloadedHandler;
						}
					}
				}
			}
		}

		/// <summary>Responds to the condition in which the value of the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> property changes from <see langword="false" /> to <see langword="true" />. </summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x06005EB6 RID: 24246 RVA: 0x001A8A6A File Offset: 0x001A6C6A
		protected virtual void OnOpened(EventArgs e)
		{
			base.RaiseClrEvent(Popup.OpenedKey, e);
		}

		/// <summary>Responds when the value of the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> property changes from to <see langword="true" /> to <see langword="false" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005EB7 RID: 24247 RVA: 0x001A8A78 File Offset: 0x001A6C78
		protected virtual void OnClosed(EventArgs e)
		{
			this._cacheValid[4] = true;
			try
			{
				base.RaiseClrEvent(Popup.ClosedKey, e);
			}
			finally
			{
				this._cacheValid[4] = false;
			}
		}

		// Token: 0x06005EB8 RID: 24248 RVA: 0x001A8AC0 File Offset: 0x001A6CC0
		private static void CloseOnUnloaded(object sender, RoutedEventArgs e)
		{
			((Popup)sender).SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
		}

		/// <summary>Gets or sets the orientation of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the control opens, and specifies the behavior of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when it overlaps screen boundaries.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.Primitives.PlacementMode" /> enumeration value that determines the orientation of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the control opens, and that specifies how the control interacts with screen boundaries. The default is <see cref="F:System.Windows.Controls.Primitives.PlacementMode.Bottom" />. </returns>
		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06005EB9 RID: 24249 RVA: 0x001A8AD7 File Offset: 0x001A6CD7
		// (set) Token: 0x06005EBA RID: 24250 RVA: 0x001A8AE9 File Offset: 0x001A6CE9
		[Bindable(true)]
		[Category("Layout")]
		public PlacementMode Placement
		{
			get
			{
				return (PlacementMode)base.GetValue(Popup.PlacementProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementProperty, value);
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06005EBB RID: 24251 RVA: 0x001A8AFC File Offset: 0x001A6CFC
		internal PlacementMode PlacementInternal
		{
			get
			{
				PlacementMode placementMode = this.Placement;
				bool flag = placementMode == PlacementMode.Mouse || placementMode == PlacementMode.MousePoint;
				if (flag && this.TreatMousePlacementAsBottom)
				{
					placementMode = PlacementMode.Bottom;
				}
				return placementMode;
			}
		}

		// Token: 0x06005EBC RID: 24252 RVA: 0x001A8B2C File Offset: 0x001A6D2C
		private static void OnPlacementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			popup.Reposition();
		}

		// Token: 0x06005EBD RID: 24253 RVA: 0x001A8B48 File Offset: 0x001A6D48
		private static bool IsValidPlacementMode(object o)
		{
			PlacementMode placementMode = (PlacementMode)o;
			return placementMode == PlacementMode.Absolute || placementMode == PlacementMode.AbsolutePoint || placementMode == PlacementMode.Bottom || placementMode == PlacementMode.Center || placementMode == PlacementMode.Mouse || placementMode == PlacementMode.MousePoint || placementMode == PlacementMode.Relative || placementMode == PlacementMode.RelativePoint || placementMode == PlacementMode.Right || placementMode == PlacementMode.Left || placementMode == PlacementMode.Top || placementMode == PlacementMode.Custom;
		}

		/// <summary>Gets or sets a delegate handler method that positions the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacementCallback" /> delegate method that provides placement information for the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06005EBE RID: 24254 RVA: 0x001A8B90 File Offset: 0x001A6D90
		// (set) Token: 0x06005EBF RID: 24255 RVA: 0x001A8BA2 File Offset: 0x001A6DA2
		[Bindable(false)]
		[Category("Layout")]
		public CustomPopupPlacementCallback CustomPopupPlacementCallback
		{
			get
			{
				return (CustomPopupPlacementCallback)base.GetValue(Popup.CustomPopupPlacementCallbackProperty);
			}
			set
			{
				base.SetValue(Popup.CustomPopupPlacementCallbackProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control closes when the control is no longer in focus.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control closes when <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> property is set to <see langword="false" />; <see langword="false" /> if the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control closes when a mouse or keyboard event occurs outside the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="true" />.</returns>
		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06005EC0 RID: 24256 RVA: 0x001A8BB0 File Offset: 0x001A6DB0
		// (set) Token: 0x06005EC1 RID: 24257 RVA: 0x001A8BC2 File Offset: 0x001A6DC2
		[Bindable(true)]
		[Category("Behavior")]
		public bool StaysOpen
		{
			get
			{
				return (bool)base.GetValue(Popup.StaysOpenProperty);
			}
			set
			{
				base.SetValue(Popup.StaysOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005EC2 RID: 24258 RVA: 0x001A8BD8 File Offset: 0x001A6DD8
		private static void OnStaysOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			if (popup.IsOpen)
			{
				if ((bool)e.NewValue)
				{
					popup.ReleasePopupCapture();
					return;
				}
				popup.EstablishPopupCapture(false);
			}
		}

		/// <summary>Get or sets the horizontal distance between the target origin and the popup alignment point. </summary>
		/// <returns>The horizontal distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06005EC3 RID: 24259 RVA: 0x001A8C10 File Offset: 0x001A6E10
		// (set) Token: 0x06005EC4 RID: 24260 RVA: 0x001A8C22 File Offset: 0x001A6E22
		[Bindable(true)]
		[Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double HorizontalOffset
		{
			get
			{
				return (double)base.GetValue(Popup.HorizontalOffsetProperty);
			}
			set
			{
				base.SetValue(Popup.HorizontalOffsetProperty, value);
			}
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x001A8C38 File Offset: 0x001A6E38
		private static void OnOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			popup.Reposition();
		}

		/// <summary>Gets or sets the vertical distance between the target origin and the popup alignment point.  </summary>
		/// <returns>The vertical distance between the target origin and the popup alignment point. For information about the target origin and popup alignment point, see Popup Placement Behavior. The default is 0.</returns>
		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06005EC6 RID: 24262 RVA: 0x001A8C52 File Offset: 0x001A6E52
		// (set) Token: 0x06005EC7 RID: 24263 RVA: 0x001A8C64 File Offset: 0x001A6E64
		[Bindable(true)]
		[Category("Layout")]
		[TypeConverter(typeof(LengthConverter))]
		public double VerticalOffset
		{
			get
			{
				return (double)base.GetValue(Popup.VerticalOffsetProperty);
			}
			set
			{
				base.SetValue(Popup.VerticalOffsetProperty, value);
			}
		}

		/// <summary>Gets or sets the element relative to which the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is positioned when it opens.  </summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> that is the logical parent of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x001A8C77 File Offset: 0x001A6E77
		// (set) Token: 0x06005EC9 RID: 24265 RVA: 0x001A8C89 File Offset: 0x001A6E89
		[Bindable(true)]
		[Category("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UIElement PlacementTarget
		{
			get
			{
				return (UIElement)base.GetValue(Popup.PlacementTargetProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementTargetProperty, value);
			}
		}

		// Token: 0x06005ECA RID: 24266 RVA: 0x001A8C98 File Offset: 0x001A6E98
		private static void OnPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Popup popup = (Popup)d;
			if (popup.IsOpen)
			{
				popup.UpdatePlacementTargetRegistration((UIElement)e.OldValue, (UIElement)e.NewValue);
				return;
			}
			if (e.OldValue != null)
			{
				Popup.UnregisterPopupFromPlacementTarget(popup, (UIElement)e.OldValue);
			}
		}

		/// <summary>Gets or sets the rectangle relative to which the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control is positioned when it opens.  </summary>
		/// <returns>The rectangle that is used to position the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06005ECB RID: 24267 RVA: 0x001A8CEE File Offset: 0x001A6EEE
		// (set) Token: 0x06005ECC RID: 24268 RVA: 0x001A8D00 File Offset: 0x001A6F00
		[Bindable(true)]
		[Category("Layout")]
		public Rect PlacementRectangle
		{
			get
			{
				return (Rect)base.GetValue(Popup.PlacementRectangleProperty);
			}
			set
			{
				base.SetValue(Popup.PlacementRectangleProperty, value);
			}
		}

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x06005ECD RID: 24269 RVA: 0x001A8D14 File Offset: 0x001A6F14
		// (set) Token: 0x06005ECE RID: 24270 RVA: 0x001A8D87 File Offset: 0x001A6F87
		internal bool DropOpposite
		{
			get
			{
				bool result = false;
				if (this._cacheValid[8])
				{
					result = this._cacheValid[16];
				}
				else
				{
					DependencyObject dependencyObject = this;
					Popup popup;
					for (;;)
					{
						dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
						PopupRoot popupRoot = dependencyObject as PopupRoot;
						if (popupRoot != null)
						{
							popup = (popupRoot.Parent as Popup);
							dependencyObject = popup;
							if (popup != null && popup._cacheValid[8])
							{
								break;
							}
						}
						if (dependencyObject == null)
						{
							return result;
						}
					}
					result = popup._cacheValid[16];
				}
				return result;
			}
			set
			{
				this._cacheValid[16] = value;
				this._cacheValid[8] = true;
			}
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x001A8DA4 File Offset: 0x001A6FA4
		private void ClearDropOpposite()
		{
			this._cacheValid[8] = false;
		}

		/// <summary>Gets or sets an animation for the opening and closing of a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.  </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.PopupAnimation" /> enumeration value that defines an animation to open and close a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see cref="F:System.Windows.Controls.Primitives.PopupAnimation.None" />.</returns>
		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x06005ED0 RID: 24272 RVA: 0x001A8DB3 File Offset: 0x001A6FB3
		// (set) Token: 0x06005ED1 RID: 24273 RVA: 0x001A8DC5 File Offset: 0x001A6FC5
		[Bindable(true)]
		[Category("Appearance")]
		public PopupAnimation PopupAnimation
		{
			get
			{
				return (PopupAnimation)base.GetValue(Popup.PopupAnimationProperty);
			}
			set
			{
				base.SetValue(Popup.PopupAnimationProperty, value);
			}
		}

		// Token: 0x06005ED2 RID: 24274 RVA: 0x001A8DD8 File Offset: 0x001A6FD8
		private static object CoercePopupAnimation(DependencyObject o, object value)
		{
			if (!((Popup)o).AllowsTransparency)
			{
				return PopupAnimation.None;
			}
			return value;
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x001A8DF0 File Offset: 0x001A6FF0
		private static bool IsValidPopupAnimation(object o)
		{
			PopupAnimation popupAnimation = (PopupAnimation)o;
			return popupAnimation == PopupAnimation.None || popupAnimation == PopupAnimation.Fade || popupAnimation == PopupAnimation.Slide || popupAnimation == PopupAnimation.Scroll;
		}

		/// <summary>Gets or sets a value that indicates whether a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control can contain transparent content.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control can contain transparent content; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x06005ED4 RID: 24276 RVA: 0x001A8E15 File Offset: 0x001A7015
		// (set) Token: 0x06005ED5 RID: 24277 RVA: 0x001A8E27 File Offset: 0x001A7027
		public bool AllowsTransparency
		{
			get
			{
				return (bool)base.GetValue(Popup.AllowsTransparencyProperty);
			}
			set
			{
				base.SetValue(Popup.AllowsTransparencyProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005ED6 RID: 24278 RVA: 0x001A8E3A File Offset: 0x001A703A
		private static void OnAllowsTransparencyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(Popup.PopupAnimationProperty);
		}

		// Token: 0x06005ED7 RID: 24279 RVA: 0x001A8E47 File Offset: 0x001A7047
		private static object CoerceAllowsTransparency(DependencyObject d, object value)
		{
			if (!((Popup)d)._secHelper.IsChildPopup)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.Primitives.Popup" /> is displayed with a drop shadow effect.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is displayed with a drop shadow; otherwise, <see langword="false" />.</returns>
		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x06005ED8 RID: 24280 RVA: 0x001A8E62 File Offset: 0x001A7062
		public bool HasDropShadow
		{
			get
			{
				return (bool)base.GetValue(Popup.HasDropShadowProperty);
			}
		}

		// Token: 0x06005ED9 RID: 24281 RVA: 0x001A8E74 File Offset: 0x001A7074
		private static object CoerceHasDropShadow(DependencyObject d, object value)
		{
			return BooleanBoxes.Box(SystemParameters.DropShadow && ((Popup)d).AllowsTransparency);
		}

		/// <summary>Attaches a child element to a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. </summary>
		/// <param name="popup">The <see cref="T:System.Windows.Controls.Primitives.Popup" /> to which to add child content. </param>
		/// <param name="child">The <see cref="T:System.Windows.UIElement" /> child content. </param>
		// Token: 0x06005EDA RID: 24282 RVA: 0x001A8E90 File Offset: 0x001A7090
		public static void CreateRootPopup(Popup popup, UIElement child)
		{
			Popup.CreateRootPopupInternal(popup, child, false);
		}

		// Token: 0x06005EDB RID: 24283 RVA: 0x001A8E9C File Offset: 0x001A709C
		internal static void CreateRootPopupInternal(Popup popup, UIElement child, bool bindTreatMousePlacementAsBottomProperty)
		{
			if (popup == null)
			{
				throw new ArgumentNullException("popup");
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			object parent;
			if ((parent = LogicalTreeHelper.GetParent(child)) != null)
			{
				throw new InvalidOperationException(SR.Get("CreateRootPopup_ChildHasLogicalParent", new object[]
				{
					child,
					parent
				}));
			}
			if ((parent = VisualTreeHelper.GetParent(child)) != null)
			{
				throw new InvalidOperationException(SR.Get("CreateRootPopup_ChildHasVisualParent", new object[]
				{
					child,
					parent
				}));
			}
			Binding binding = new Binding("PlacementTarget");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementTargetProperty, binding);
			popup.Child = child;
			binding = new Binding("VerticalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.VerticalOffsetProperty, binding);
			binding = new Binding("HorizontalOffset");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.HorizontalOffsetProperty, binding);
			binding = new Binding("PlacementRectangle");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementRectangleProperty, binding);
			binding = new Binding("Placement");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.PlacementProperty, binding);
			binding = new Binding("StaysOpen");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.StaysOpenProperty, binding);
			binding = new Binding("CustomPopupPlacementCallback");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.CustomPopupPlacementCallbackProperty, binding);
			if (bindTreatMousePlacementAsBottomProperty)
			{
				binding = new Binding("FromKeyboard");
				binding.Mode = BindingMode.OneWay;
				binding.Source = child;
				popup.SetBinding(Popup.TreatMousePlacementAsBottomProperty, binding);
			}
			binding = new Binding("IsOpen");
			binding.Mode = BindingMode.OneWay;
			binding.Source = child;
			popup.SetBinding(Popup.IsOpenProperty, binding);
		}

		// Token: 0x06005EDC RID: 24284 RVA: 0x001A9078 File Offset: 0x001A7278
		internal static bool IsRootedInPopup(Popup parentPopup, UIElement element)
		{
			object parent = LogicalTreeHelper.GetParent(element);
			return (parent != null || VisualTreeHelper.GetParent(element) == null) && parent == parentPopup;
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> property changes to <see langword="true" />. </summary>
		// Token: 0x14000117 RID: 279
		// (add) Token: 0x06005EDD RID: 24285 RVA: 0x001A90A0 File Offset: 0x001A72A0
		// (remove) Token: 0x06005EDE RID: 24286 RVA: 0x001A90AE File Offset: 0x001A72AE
		public event EventHandler Opened
		{
			add
			{
				base.EventHandlersStoreAdd(Popup.OpenedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(Popup.OpenedKey, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> property changes to <see langword="false" />. </summary>
		// Token: 0x14000118 RID: 280
		// (add) Token: 0x06005EDF RID: 24287 RVA: 0x001A90BC File Offset: 0x001A72BC
		// (remove) Token: 0x06005EE0 RID: 24288 RVA: 0x001A90CA File Offset: 0x001A72CA
		public event EventHandler Closed
		{
			add
			{
				base.EventHandlersStoreAdd(Popup.ClosedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(Popup.ClosedKey, value);
			}
		}

		// Token: 0x06005EE1 RID: 24289 RVA: 0x001A90D8 File Offset: 0x001A72D8
		private void FirePopupCouldClose()
		{
			if (this.PopupCouldClose != null)
			{
				this.PopupCouldClose(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000119 RID: 281
		// (add) Token: 0x06005EE2 RID: 24290 RVA: 0x001A90F4 File Offset: 0x001A72F4
		// (remove) Token: 0x06005EE3 RID: 24291 RVA: 0x001A912C File Offset: 0x001A732C
		internal event EventHandler PopupCouldClose;

		/// <summary>Determines the required size of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> content within the visual tree of the logical parent.</summary>
		/// <param name="availableSize">The available size that this element can give to the child. You can use infinity as a value to indicate that the element can size to whatever content is available.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> structure that has the <see cref="P:System.Windows.Size.Height" /> and <see cref="P:System.Windows.Size.Width" /> properties both equal to zero (0).</returns>
		// Token: 0x06005EE4 RID: 24292 RVA: 0x001A9164 File Offset: 0x001A7364
		protected override Size MeasureOverride(Size availableSize)
		{
			return default(Size);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005EE5 RID: 24293 RVA: 0x001A917A File Offset: 0x001A737A
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			this.OnPreviewMouseButton(e);
			base.OnPreviewMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseRightButtonUp" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005EE6 RID: 24294 RVA: 0x001A918A File Offset: 0x001A738A
		protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonDown(e);
			this.OnPreviewMouseButton(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonUp" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005EE7 RID: 24295 RVA: 0x001A919A File Offset: 0x001A739A
		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			this.OnPreviewMouseButton(e);
			base.OnPreviewMouseLeftButtonUp(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseRightButtonDown" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005EE8 RID: 24296 RVA: 0x001A91AA File Offset: 0x001A73AA
		protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseRightButtonUp(e);
			this.OnPreviewMouseButton(e);
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x001A91BC File Offset: 0x001A73BC
		private void OnPreviewMouseButton(MouseButtonEventArgs e)
		{
			if (this._cacheValid[1] && !this.StaysOpen && !this._cacheValid[512] && this._popupRoot.Value != null && e.OriginalSource == this._popupRoot.Value && this._popupRoot.Value.InputHitTest(e.GetPosition(this._popupRoot.Value)) == null)
			{
				base.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
			}
			if (this._cacheValid[512] && e.LeftButton == MouseButtonState.Released && e.RightButton == MouseButtonState.Released)
			{
				this._cacheValid[512] = false;
			}
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x001A9278 File Offset: 0x001A7478
		private void EstablishPopupCapture(bool isRestoringCapture = false)
		{
			if (!this._cacheValid[1] && this._popupRoot.Value != null && !this.StaysOpen)
			{
				IInputElement inputElement = Mouse.Captured;
				PopupRoot popupRoot = inputElement as PopupRoot;
				if (popupRoot != null)
				{
					if (isRestoringCapture)
					{
						if (Mouse.LeftButton != MouseButtonState.Released || Mouse.RightButton != MouseButtonState.Released)
						{
							this._cacheValid[512] = true;
						}
					}
					else
					{
						Popup.ParentPopupRootField.SetValue(this, popupRoot);
					}
					inputElement = null;
				}
				if (inputElement == null)
				{
					Mouse.Capture(this._popupRoot.Value, CaptureMode.SubTree);
					this._cacheValid[1] = true;
				}
			}
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x001A930C File Offset: 0x001A750C
		private void ReleasePopupCapture()
		{
			if (this._cacheValid[1])
			{
				PopupRoot value = Popup.ParentPopupRootField.GetValue(this);
				Popup.ParentPopupRootField.ClearValue(this);
				if (Mouse.Captured == this._popupRoot.Value)
				{
					if (value == null)
					{
						Mouse.Capture(null);
					}
					else
					{
						Popup popup = value.Parent as Popup;
						if (popup != null)
						{
							popup.EstablishPopupCapture(true);
						}
					}
				}
				this._cacheValid[1] = false;
			}
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x001A9380 File Offset: 0x001A7580
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			Popup popup = sender as Popup;
			if (!popup.StaysOpen)
			{
				PopupRoot value = popup._popupRoot.Value;
				bool flag = e.OriginalSource != value && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero;
				if (flag)
				{
					popup.EstablishPopupCapture(false);
					e.Handled = true;
					return;
				}
				if (Mouse.Captured != value)
				{
					popup._cacheValid[1] = false;
				}
				PopupRoot popupRoot = Mouse.Captured as PopupRoot;
				Popup popup2 = (popupRoot == null) ? null : (popupRoot.Parent as Popup);
				bool flag2 = (popup2 == null || value == null || value != Popup.ParentPopupRootField.GetValue(popup2)) && (Mouse.Captured == null || !MenuBase.IsDescendant(value, Mouse.Captured as DependencyObject)) && Mouse.Captured != value;
				if (flag2 && !popup.IsDragDropActive)
				{
					popup.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
				}
			}
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x001A9484 File Offset: 0x001A7684
		private static void OnDragDropStarted(object sender, RoutedEventArgs e)
		{
			Popup popup = (Popup)sender;
			popup.IsDragDropActive = true;
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x001A94A0 File Offset: 0x001A76A0
		private static void OnDragDropCompleted(object sender, RoutedEventArgs e)
		{
			Popup popup = (Popup)sender;
			popup.IsDragDropActive = false;
			if (!popup.StaysOpen)
			{
				popup.EstablishPopupCapture(false);
			}
		}

		/// <summary>This member supports the WPF infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">An object to add as a child. </param>
		// Token: 0x06005EEF RID: 24303 RVA: 0x001A94CC File Offset: 0x001A76CC
		void IAddChild.AddChild(object value)
		{
			UIElement uielement = value as UIElement;
			if (uielement == null && value != null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(UIElement)
				}), "value");
			}
			this.Child = uielement;
		}

		/// <summary>This member supports the WPF infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">A string to add to the object. </param>
		// Token: 0x06005EF0 RID: 24304 RVA: 0x001A9520 File Offset: 0x001A7720
		void IAddChild.AddText(string text)
		{
			this.Child = new TextBlock
			{
				Text = text
			};
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x001A9541 File Offset: 0x001A7741
		internal override void OnThemeChanged()
		{
			if (this._popupRoot.Value != null)
			{
				TreeWalkHelper.InvalidateOnResourcesChange(this._popupRoot.Value, null, ResourcesChangeInfo.ThemeChangeInfo);
			}
		}

		// Token: 0x06005EF2 RID: 24306 RVA: 0x001A9566 File Offset: 0x001A7766
		internal override bool BlockReverseInheritance()
		{
			return base.TemplatedParent == null;
		}

		/// <summary>Returns the logical parent of a <see cref="T:System.Windows.Controls.Primitives.Popup" />. </summary>
		/// <returns>If the <see cref="T:System.Windows.Controls.Primitives.Popup" /> does not have a defined parent and the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> is not <see langword="null" />, the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> is returned. Otherwise, the return values are the same as <see cref="M:System.Windows.FrameworkElement.GetUIParentCore" />.</returns>
		// Token: 0x06005EF3 RID: 24307 RVA: 0x001A9574 File Offset: 0x001A7774
		protected internal override DependencyObject GetUIParentCore()
		{
			if (base.Parent == null)
			{
				UIElement placementTarget = this.PlacementTarget;
				if (placementTarget != null && (this.IsOpen || this._secHelper.IsWindowAlive()))
				{
					return placementTarget;
				}
			}
			return base.GetUIParentCore();
		}

		// Token: 0x06005EF4 RID: 24308 RVA: 0x001A95B0 File Offset: 0x001A77B0
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			return base.Parent == null && e.RoutedEvent != Mouse.LostMouseCaptureEvent;
		}

		/// <summary>Gets an enumerator that you can use to access the logical child elements of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that you can use to access the logical child elements of a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default is <see langword="null" />.</returns>
		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06005EF5 RID: 24309 RVA: 0x001A95CC File Offset: 0x001A77CC
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object child = this.Child;
				if (child == null)
				{
					return EmptyEnumerator.Instance;
				}
				return new Popup.PopupModelTreeEnumerator(this, child);
			}
		}

		// Token: 0x06005EF6 RID: 24310 RVA: 0x001A95F0 File Offset: 0x001A77F0
		private static Visual GetRootVisual(Visual child)
		{
			DependencyObject dependencyObject = child;
			DependencyObject parent;
			while ((parent = VisualTreeHelper.GetParent(dependencyObject)) != null)
			{
				dependencyObject = parent;
			}
			return dependencyObject as Visual;
		}

		// Token: 0x06005EF7 RID: 24311 RVA: 0x001A9614 File Offset: 0x001A7814
		private Visual GetTarget()
		{
			Visual visual = this.PlacementTarget;
			if (visual == null)
			{
				visual = VisualTreeHelper.GetContainingVisual2D(VisualTreeHelper.GetParent(this));
			}
			return visual;
		}

		// Token: 0x06005EF8 RID: 24312 RVA: 0x001A9638 File Offset: 0x001A7838
		private void SetHitTestable(bool hitTestable)
		{
			this._popupRoot.Value.IsHitTestVisible = hitTestable;
			if (this.IsTransparent)
			{
				this._secHelper.SetHitTestable(hitTestable);
			}
		}

		// Token: 0x06005EF9 RID: 24313 RVA: 0x001A9660 File Offset: 0x001A7860
		private static object AsyncCreateWindow(object arg)
		{
			Popup popup = (Popup)arg;
			popup._asyncCreate = null;
			popup.CreateWindow(true);
			return null;
		}

		// Token: 0x06005EFA RID: 24314 RVA: 0x001A9684 File Offset: 0x001A7884
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CreateNewPopupRoot()
		{
			if (this._popupRoot.Value == null)
			{
				this._popupRoot.Value = new PopupRoot();
				base.AddLogicalChild(this._popupRoot.Value);
				this._popupRoot.Value.SetupLayoutBindings(this);
			}
		}

		// Token: 0x06005EFB RID: 24315 RVA: 0x001A96D0 File Offset: 0x001A78D0
		private void CreateWindow(bool asyncCall)
		{
			this.ClearDropOpposite();
			Visual target = this.GetTarget();
			if (target != null && Popup.PopupSecurityHelper.IsVisualPresentationSourceNull(target))
			{
				if (!asyncCall)
				{
					this._asyncCreate = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(Popup.AsyncCreateWindow), this);
				}
				return;
			}
			if (this._positionInfo != null)
			{
				this._positionInfo.MouseRect = Rect.Empty;
				this._positionInfo.ChildSize = Size.Empty;
			}
			bool flag = !this._secHelper.IsWindowAlive();
			if (Popup.PopupInitialPlacementHelper.IsPerMonitorDpiScalingActive)
			{
				this.DestroyWindowImpl();
				this._positionInfo = null;
				flag = true;
			}
			if (flag)
			{
				this.BuildWindow(target);
				this.CreateNewPopupRoot();
			}
			UIElement child = this.Child;
			if (this._popupRoot.Value.Child != child)
			{
				this._popupRoot.Value.Child = child;
			}
			this.UpdatePlacementTargetRegistration(null, this.PlacementTarget);
			this.UpdateTransform();
			bool flag2;
			if (flag)
			{
				this.SetRootVisualToPopupRoot();
				flag2 = this._secHelper.IsWindowAlive();
				if (flag2)
				{
					this._secHelper.ForceMsaaToUiaBridge(this._popupRoot.Value);
				}
			}
			else
			{
				this.UpdatePosition();
				flag2 = this._secHelper.IsWindowAlive();
			}
			if (flag2)
			{
				this.ShowWindow();
				this.OnOpened(EventArgs.Empty);
			}
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x001A980C File Offset: 0x001A7A0C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SetRootVisualToPopupRoot()
		{
			if (this.PopupAnimation != PopupAnimation.None && this.IsTransparent)
			{
				this._popupRoot.Value.Opacity = 0.0;
			}
			this._secHelper.SetWindowRootVisual(this._popupRoot.Value);
		}

		// Token: 0x06005EFD RID: 24317 RVA: 0x001A9858 File Offset: 0x001A7A58
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void BuildWindow(Visual targetVisual)
		{
			base.CoerceValue(Popup.AllowsTransparencyProperty);
			base.CoerceValue(Popup.HasDropShadowProperty);
			this.IsTransparent = this.AllowsTransparency;
			NativeMethods.POINTSTRUCT pointstruct = (this._positionInfo != null) ? new NativeMethods.POINTSTRUCT(this._positionInfo.X, this._positionInfo.Y) : Popup.PopupInitialPlacementHelper.GetPlacementOrigin(this);
			this._secHelper.BuildWindow(pointstruct.x, pointstruct.y, targetVisual, this.IsTransparent, new HwndSourceHook(this.PopupFilterMessage), new AutoResizedEventHandler(this.OnWindowResize), new HwndDpiChangedEventHandler(this.OnDpiChanged));
		}

		// Token: 0x06005EFE RID: 24318 RVA: 0x001A98F8 File Offset: 0x001A7AF8
		[SecuritySafeCritical]
		private bool DestroyWindowImpl()
		{
			if (this._secHelper.IsWindowAlive())
			{
				this._secHelper.DestroyWindow(new HwndSourceHook(this.PopupFilterMessage), new AutoResizedEventHandler(this.OnWindowResize), new HwndDpiChangedEventHandler(this.OnDpiChanged));
				return true;
			}
			return false;
		}

		// Token: 0x06005EFF RID: 24319 RVA: 0x001A9944 File Offset: 0x001A7B44
		private void DestroyWindow()
		{
			if (this.DestroyWindowImpl())
			{
				this.ReleasePopupCapture();
				this.OnClosed(EventArgs.Empty);
				this.UpdatePlacementTargetRegistration(this.PlacementTarget, null);
			}
		}

		// Token: 0x06005F00 RID: 24320 RVA: 0x001A996C File Offset: 0x001A7B6C
		private void ShowWindow()
		{
			if (this._secHelper.IsWindowAlive())
			{
				this._popupRoot.Value.Opacity = 1.0;
				this.SetupAnimations(true);
				this.SetHitTestable(this.HitTestable || !this.IsTransparent);
				this.EstablishPopupCapture(false);
				this._secHelper.ShowWindow();
			}
		}

		// Token: 0x06005F01 RID: 24321 RVA: 0x001A99D4 File Offset: 0x001A7BD4
		private void HideWindow()
		{
			bool flag = this.SetupAnimations(false);
			this.SetHitTestable(false);
			this.ReleasePopupCapture();
			this._asyncDestroy = new DispatcherTimer(DispatcherPriority.Input);
			this._asyncDestroy.Tick += delegate(object sender, EventArgs args)
			{
				this._asyncDestroy.Stop();
				this._asyncDestroy = null;
				this.DestroyWindow();
			};
			this._asyncDestroy.Interval = (flag ? Popup.AnimationDelayTime : TimeSpan.Zero);
			this._asyncDestroy.Start();
			if (!flag)
			{
				this._secHelper.HideWindow();
			}
		}

		// Token: 0x06005F02 RID: 24322 RVA: 0x001A9A4C File Offset: 0x001A7C4C
		private bool SetupAnimations(bool visible)
		{
			PopupAnimation popupAnimation = this.PopupAnimation;
			this._popupRoot.Value.StopAnimations();
			if (popupAnimation != PopupAnimation.None && this.IsTransparent)
			{
				if (popupAnimation == PopupAnimation.Fade)
				{
					this._popupRoot.Value.SetupFadeAnimation(Popup.AnimationDelayTime, visible);
					return true;
				}
				if (visible)
				{
					this._popupRoot.Value.SetupTranslateAnimations(popupAnimation, Popup.AnimationDelayTime, this.AnimateFromRight, this.AnimateFromBottom);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005F03 RID: 24323 RVA: 0x001A9AC9 File Offset: 0x001A7CC9
		private void CancelAsyncCreate()
		{
			if (this._asyncCreate != null)
			{
				this._asyncCreate.Abort();
				this._asyncCreate = null;
			}
		}

		// Token: 0x06005F04 RID: 24324 RVA: 0x001A9AE6 File Offset: 0x001A7CE6
		private void CancelAsyncDestroy()
		{
			if (this._asyncDestroy != null)
			{
				this._asyncDestroy.Stop();
				this._asyncDestroy = null;
			}
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x001A9B02 File Offset: 0x001A7D02
		internal void ForceClose()
		{
			if (this._asyncDestroy != null)
			{
				this.CancelAsyncDestroy();
				this.DestroyWindow();
			}
		}

		// Token: 0x06005F06 RID: 24326 RVA: 0x001A9B18 File Offset: 0x001A7D18
		[SecuritySafeCritical]
		private unsafe IntPtr PopupFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg != 28)
			{
				if (msg == 33)
				{
					handled = true;
					return new IntPtr(3);
				}
				if (msg == 70)
				{
					if (this._secHelper.IsChildPopup)
					{
						NativeMethods.WINDOWPOS* ptr = (NativeMethods.WINDOWPOS*)((void*)lParam);
						ptr->flags |= 256;
					}
				}
			}
			else if (wParam == IntPtr.Zero)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.HandleDeactivateApp), null);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06005F07 RID: 24327 RVA: 0x001A9B96 File Offset: 0x001A7D96
		private object HandleDeactivateApp(object arg)
		{
			if (!this.StaysOpen)
			{
				base.SetCurrentValueInternal(Popup.IsOpenProperty, BooleanBoxes.FalseBox);
			}
			this.FirePopupCouldClose();
			return null;
		}

		// Token: 0x06005F08 RID: 24328 RVA: 0x001A9BB8 File Offset: 0x001A7DB8
		private void UpdateTransform()
		{
			Matrix matrix = base.LayoutTransform.Value * base.RenderTransform.Value;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			Visual visual = (parent == null) ? null : Popup.GetRootVisual(this);
			if (visual != null)
			{
				matrix = matrix * base.TransformToAncestor(visual).AffineTransform.Value * PointUtil.GetVisualTransform(visual);
			}
			if (this.IsTransparent)
			{
				if (parent != null && (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty) == FlowDirection.RightToLeft)
				{
					matrix.Scale(-1.0, 1.0);
				}
			}
			else
			{
				Vector vector = matrix.Transform(new Vector(1.0, 0.0));
				Vector vector2 = matrix.Transform(new Vector(0.0, 1.0));
				matrix = default(Matrix);
				matrix.Scale(vector.Length, vector2.Length);
			}
			this._popupRoot.Value.Transform = new MatrixTransform(matrix);
		}

		// Token: 0x06005F09 RID: 24329 RVA: 0x001A9CCC File Offset: 0x001A7ECC
		private void OnWindowResize(object sender, AutoResizedEventArgs e)
		{
			if (this._positionInfo == null)
			{
				Exception ex = new NullReferenceException();
				throw new NullReferenceException(ex.Message, this.SavedException);
			}
			Popup.SavedExceptionField.ClearValue(this);
			if (e.Size != this._positionInfo.ChildSize)
			{
				this._positionInfo.ChildSize = e.Size;
				this.Reposition();
			}
		}

		// Token: 0x06005F0A RID: 24330 RVA: 0x001A9D33 File Offset: 0x001A7F33
		private void OnDpiChanged(object sender, HwndDpiChangedEventArgs e)
		{
			if (this.IsOpen)
			{
				e.Handled = true;
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06005F0B RID: 24331 RVA: 0x001A9D44 File Offset: 0x001A7F44
		// (set) Token: 0x06005F0C RID: 24332 RVA: 0x001A9D51 File Offset: 0x001A7F51
		internal Exception SavedException
		{
			get
			{
				return Popup.SavedExceptionField.GetValue(this);
			}
			set
			{
				Popup.SavedExceptionField.SetValue(this, value);
			}
		}

		// Token: 0x06005F0D RID: 24333 RVA: 0x001A9D60 File Offset: 0x001A7F60
		internal void Reposition()
		{
			if (this.IsOpen && this._secHelper.IsWindowAlive())
			{
				if (base.CheckAccess())
				{
					this.UpdatePosition();
					return;
				}
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object param)
				{
					this.Reposition();
					return null;
				}), null);
			}
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x001A9DAC File Offset: 0x001A7FAC
		private static bool IsAbsolutePlacementMode(PlacementMode placement)
		{
			return placement == PlacementMode.Absolute || placement == PlacementMode.AbsolutePoint || placement - PlacementMode.Mouse <= 1;
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x001A9DC0 File Offset: 0x001A7FC0
		private void UpdatePosition()
		{
			if (this._popupRoot.Value == null)
			{
				return;
			}
			PlacementMode placementInternal = this.PlacementInternal;
			Point[] placementTargetInterestPoints = this.GetPlacementTargetInterestPoints(placementInternal);
			Point[] childInterestPoints = this.GetChildInterestPoints(placementInternal);
			Rect bounds = this.GetBounds(placementTargetInterestPoints);
			Rect bounds2 = this.GetBounds(childInterestPoints);
			double num = bounds2.Width * bounds2.Height;
			int num2 = -1;
			Vector offsetVector = new Vector((double)this._positionInfo.X, (double)this._positionInfo.Y);
			double num3 = -1.0;
			CustomPopupPlacement[] array = null;
			int num4;
			if (placementInternal == PlacementMode.Custom)
			{
				CustomPopupPlacementCallback customPopupPlacementCallback = this.CustomPopupPlacementCallback;
				if (customPopupPlacementCallback != null)
				{
					array = customPopupPlacementCallback(bounds2.Size, bounds.Size, new Point(this.HorizontalOffset, this.VerticalOffset));
				}
				num4 = ((array == null) ? 0 : array.Length);
				if (!this.IsOpen)
				{
					return;
				}
			}
			else
			{
				num4 = Popup.GetNumberOfCombinations(placementInternal);
			}
			Rect screenBounds;
			for (int i = 0; i < num4; i++)
			{
				bool animateFromRight = false;
				bool animateFromBottom = false;
				Vector vector;
				if (placementInternal == PlacementMode.Custom)
				{
					vector = (Vector)placementTargetInterestPoints[0] + (Vector)array[i].Point;
					PopupPrimaryAxis primaryAxis = array[i].PrimaryAxis;
				}
				else
				{
					PopupPrimaryAxis primaryAxis;
					Popup.PointCombination pointCombination = this.GetPointCombination(placementInternal, i, out primaryAxis);
					Popup.InterestPoint targetInterestPoint = pointCombination.TargetInterestPoint;
					Popup.InterestPoint childInterestPoint = pointCombination.ChildInterestPoint;
					vector = placementTargetInterestPoints[(int)targetInterestPoint] - childInterestPoints[(int)childInterestPoint];
					animateFromRight = (childInterestPoint == Popup.InterestPoint.TopRight || childInterestPoint == Popup.InterestPoint.BottomRight);
					animateFromBottom = (childInterestPoint == Popup.InterestPoint.BottomLeft || childInterestPoint == Popup.InterestPoint.BottomRight);
				}
				Rect rect = Rect.Offset(bounds2, vector);
				screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
				Rect rect2 = Rect.Intersect(screenBounds, rect);
				double num5 = (rect2 != Rect.Empty) ? (rect2.Width * rect2.Height) : 0.0;
				if (num5 - num3 > 0.01)
				{
					num2 = i;
					offsetVector = vector;
					num3 = num5;
					this.AnimateFromRight = animateFromRight;
					this.AnimateFromBottom = animateFromBottom;
					if (Math.Abs(num5 - num) < 0.01)
					{
						break;
					}
				}
			}
			if (num2 >= 2 && (placementInternal == PlacementMode.Right || placementInternal == PlacementMode.Left))
			{
				this.DropOpposite = !this.DropOpposite;
			}
			bounds2 = new Rect((Size)this._secHelper.GetTransformToDevice().Transform((Point)this._popupRoot.Value.RenderSize));
			bounds2.Offset(offsetVector);
			screenBounds = this.GetScreenBounds(bounds, placementTargetInterestPoints[0]);
			Rect rect3 = Rect.Intersect(screenBounds, bounds2);
			if (Math.Abs(rect3.Width - bounds2.Width) > 0.01 || Math.Abs(rect3.Height - bounds2.Height) > 0.01)
			{
				Point point = placementTargetInterestPoints[0];
				Point point2 = placementTargetInterestPoints[1];
				Vector vector2 = point2 - point;
				vector2.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector2.Y) || Math.Abs(vector2.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						offsetVector.X = screenBounds.Right - bounds2.Width;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						offsetVector.X = screenBounds.Left;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector2.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						offsetVector.Y = screenBounds.Bottom - bounds2.Height;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						offsetVector.Y = screenBounds.Top;
					}
				}
				Point point3 = placementTargetInterestPoints[2];
				Vector vector3 = point - point3;
				vector3.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector3.X) || Math.Abs(vector3.X) < 0.01)
				{
					if (bounds2.Bottom > screenBounds.Bottom)
					{
						offsetVector.Y = screenBounds.Bottom - bounds2.Height;
					}
					else if (bounds2.Top < screenBounds.Top)
					{
						offsetVector.Y = screenBounds.Top;
					}
				}
				else if (this.IsTransparent && Math.Abs(vector3.Y) < 0.01)
				{
					if (bounds2.Right > screenBounds.Right)
					{
						offsetVector.X = screenBounds.Right - bounds2.Width;
					}
					else if (bounds2.Left < screenBounds.Left)
					{
						offsetVector.X = screenBounds.Left;
					}
				}
			}
			int num6 = DoubleUtil.DoubleToInt(offsetVector.X);
			int num7 = DoubleUtil.DoubleToInt(offsetVector.Y);
			if (num6 != this._positionInfo.X || num7 != this._positionInfo.Y)
			{
				this._positionInfo.X = num6;
				this._positionInfo.Y = num7;
				this._secHelper.SetPopupPos(true, num6, num7, false, 0, 0);
			}
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x001AA2F8 File Offset: 0x001A84F8
		private void GetPopupRootLimits(out Rect targetBounds, out Rect screenBounds, out Size limitSize)
		{
			PlacementMode placementInternal = this.PlacementInternal;
			Point[] placementTargetInterestPoints = this.GetPlacementTargetInterestPoints(placementInternal);
			targetBounds = this.GetBounds(placementTargetInterestPoints);
			screenBounds = this.GetScreenBounds(targetBounds, placementTargetInterestPoints[0]);
			PopupPrimaryAxis primaryAxis = Popup.GetPrimaryAxis(placementInternal);
			limitSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			if (primaryAxis == PopupPrimaryAxis.Horizontal)
			{
				Point point = placementTargetInterestPoints[0];
				Point point2 = placementTargetInterestPoints[2];
				Vector vector = point2 - point;
				vector.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector.X) || Math.Abs(vector.X) < 0.01)
				{
					limitSize.Height = Math.Max(0.0, Math.Max(screenBounds.Bottom - targetBounds.Bottom, targetBounds.Top - screenBounds.Top));
					return;
				}
				if (this.IsTransparent && Math.Abs(vector.Y) < 0.01)
				{
					limitSize.Width = Math.Max(0.0, Math.Max(screenBounds.Right - targetBounds.Right, targetBounds.Left - screenBounds.Left));
					return;
				}
			}
			else if (primaryAxis == PopupPrimaryAxis.Vertical)
			{
				Point point3 = placementTargetInterestPoints[0];
				Point point4 = placementTargetInterestPoints[1];
				Vector vector2 = point4 - point3;
				vector2.Normalize();
				if (!this.IsTransparent || double.IsNaN(vector2.X) || Math.Abs(vector2.Y) < 0.01)
				{
					limitSize.Width = Math.Max(0.0, Math.Max(screenBounds.Right - targetBounds.Right, targetBounds.Left - screenBounds.Left));
					return;
				}
				if (this.IsTransparent && Math.Abs(vector2.X) < 0.01)
				{
					limitSize.Height = Math.Max(0.0, Math.Max(screenBounds.Bottom - targetBounds.Bottom, targetBounds.Top - screenBounds.Top));
				}
			}
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x001AA520 File Offset: 0x001A8720
		private Point[] GetPlacementTargetInterestPoints(PlacementMode placement)
		{
			if (this._positionInfo == null)
			{
				this._positionInfo = new Popup.PositionInfo();
			}
			Rect rect = this.PlacementRectangle;
			UIElement uielement = this.GetTarget() as UIElement;
			Vector vector = new Vector(this.HorizontalOffset, this.VerticalOffset);
			Point[] array;
			if (uielement == null || Popup.IsAbsolutePlacementMode(placement))
			{
				if (placement == PlacementMode.Mouse || placement == PlacementMode.MousePoint)
				{
					if (this._positionInfo.MouseRect == Rect.Empty)
					{
						this._positionInfo.MouseRect = this.GetMouseRect(placement);
					}
					rect = this._positionInfo.MouseRect;
				}
				else if (rect == Rect.Empty)
				{
					rect = default(Rect);
				}
				vector = this._secHelper.GetTransformToDevice().Transform(vector);
				rect.Offset(vector);
				array = Popup.InterestPointsFromRect(rect);
			}
			else
			{
				if (rect == Rect.Empty)
				{
					if (placement != PlacementMode.Relative && placement != PlacementMode.RelativePoint)
					{
						rect = new Rect(0.0, 0.0, uielement.RenderSize.Width, uielement.RenderSize.Height);
					}
					else
					{
						rect = default(Rect);
					}
				}
				rect.Offset(vector);
				array = Popup.InterestPointsFromRect(rect);
				Visual rootVisual = Popup.GetRootVisual(uielement);
				GeneralTransform generalTransform = Popup.TransformToClient(uielement, rootVisual);
				for (int i = 0; i < 5; i++)
				{
					generalTransform.TryTransform(array[i], out array[i]);
					array[i] = this._secHelper.ClientToScreen(rootVisual, array[i]);
				}
			}
			return array;
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x001AA6AC File Offset: 0x001A88AC
		private static void SwapPoints(ref Point p1, ref Point p2)
		{
			Point point = p1;
			p1 = p2;
			p2 = point;
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x001AA6D4 File Offset: 0x001A88D4
		private Point[] GetChildInterestPoints(PlacementMode placement)
		{
			UIElement child = this.Child;
			if (child == null)
			{
				return Popup.InterestPointsFromRect(default(Rect));
			}
			Point[] array = Popup.InterestPointsFromRect(new Rect(default(Point), child.RenderSize));
			UIElement uielement = this.GetTarget() as UIElement;
			if (uielement != null && !Popup.IsAbsolutePlacementMode(placement) && (FlowDirection)uielement.GetValue(FrameworkElement.FlowDirectionProperty) != (FlowDirection)child.GetValue(FrameworkElement.FlowDirectionProperty))
			{
				Popup.SwapPoints(ref array[0], ref array[1]);
				Popup.SwapPoints(ref array[2], ref array[3]);
			}
			Vector animationOffset = this._popupRoot.Value.AnimationOffset;
			GeneralTransform generalTransform = Popup.TransformToClient(child, this._popupRoot.Value);
			for (int i = 0; i < 5; i++)
			{
				generalTransform.TryTransform(array[i] - animationOffset, out array[i]);
			}
			return array;
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x001AA7C8 File Offset: 0x001A89C8
		private static Point[] InterestPointsFromRect(Rect rect)
		{
			return new Point[]
			{
				rect.TopLeft,
				rect.TopRight,
				rect.BottomLeft,
				rect.BottomRight,
				new Point(rect.Left + rect.Width / 2.0, rect.Top + rect.Height / 2.0)
			};
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x001AA854 File Offset: 0x001A8A54
		private static GeneralTransform TransformToClient(Visual visual, Visual rootVisual)
		{
			return new GeneralTransformGroup
			{
				Children = 
				{
					visual.TransformToAncestor(rootVisual),
					new MatrixTransform(PointUtil.GetVisualTransform(rootVisual) * Popup.PopupSecurityHelper.GetTransformToDevice(rootVisual))
				}
			};
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x001AA89C File Offset: 0x001A8A9C
		private Rect GetBounds(Point[] interestPoints)
		{
			double num2;
			double num = num2 = interestPoints[0].X;
			double num4;
			double num3 = num4 = interestPoints[0].Y;
			for (int i = 1; i < interestPoints.Length; i++)
			{
				double x = interestPoints[i].X;
				double y = interestPoints[i].Y;
				if (x < num2)
				{
					num2 = x;
				}
				if (x > num)
				{
					num = x;
				}
				if (y < num4)
				{
					num4 = y;
				}
				if (y > num3)
				{
					num3 = y;
				}
			}
			return new Rect(num2, num4, num - num2, num3 - num4);
		}

		// Token: 0x06005F17 RID: 24343 RVA: 0x001AA924 File Offset: 0x001A8B24
		private static int GetNumberOfCombinations(PlacementMode placement)
		{
			switch (placement)
			{
			case PlacementMode.Bottom:
			case PlacementMode.Mouse:
			case PlacementMode.Top:
				return 2;
			case PlacementMode.Right:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.MousePoint:
			case PlacementMode.Left:
				return 4;
			case PlacementMode.Custom:
				return 0;
			}
			return 1;
		}

		// Token: 0x06005F18 RID: 24344 RVA: 0x001AA970 File Offset: 0x001A8B70
		private Popup.PointCombination GetPointCombination(PlacementMode placement, int i, out PopupPrimaryAxis axis)
		{
			bool flag = SystemParameters.MenuDropAlignment;
			switch (placement)
			{
			case PlacementMode.Relative:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.MousePoint:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomRight);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Bottom:
			case PlacementMode.Mouse:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Center:
				axis = PopupPrimaryAxis.None;
				return new Popup.PointCombination(Popup.InterestPoint.Center, Popup.InterestPoint.Center);
			case PlacementMode.Right:
			case PlacementMode.Left:
				axis = PopupPrimaryAxis.Vertical;
				flag |= this.DropOpposite;
				if ((flag && placement == PlacementMode.Right) || (!flag && placement == PlacementMode.Left))
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.BottomRight);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.TopLeft);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.BottomLeft);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.TopLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.BottomLeft);
					}
					if (i == 2)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
					}
					if (i == 3)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.BottomRight);
					}
					goto IL_1AE;
				}
				break;
			case PlacementMode.Top:
				axis = PopupPrimaryAxis.Horizontal;
				if (flag)
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopRight, Popup.InterestPoint.BottomRight);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomRight, Popup.InterestPoint.TopRight);
					}
					goto IL_1AE;
				}
				else
				{
					if (i == 0)
					{
						return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.BottomLeft);
					}
					if (i == 1)
					{
						return new Popup.PointCombination(Popup.InterestPoint.BottomLeft, Popup.InterestPoint.TopLeft);
					}
					goto IL_1AE;
				}
				break;
			}
			axis = PopupPrimaryAxis.None;
			return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopLeft);
			IL_1AE:
			return new Popup.PointCombination(Popup.InterestPoint.TopLeft, Popup.InterestPoint.TopRight);
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x001AAB32 File Offset: 0x001A8D32
		private static PopupPrimaryAxis GetPrimaryAxis(PlacementMode placement)
		{
			switch (placement)
			{
			case PlacementMode.Bottom:
			case PlacementMode.AbsolutePoint:
			case PlacementMode.RelativePoint:
			case PlacementMode.Top:
				return PopupPrimaryAxis.Horizontal;
			case PlacementMode.Right:
			case PlacementMode.Left:
				return PopupPrimaryAxis.Vertical;
			}
			return PopupPrimaryAxis.None;
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x001AAB74 File Offset: 0x001A8D74
		internal Size RestrictSize(Size desiredSize)
		{
			Rect rect;
			Rect rect2;
			Size size;
			this.GetPopupRootLimits(out rect, out rect2, out size);
			desiredSize = (Size)this._secHelper.GetTransformToDevice().Transform((Point)desiredSize);
			desiredSize.Width = Math.Min(desiredSize.Width, rect2.Width);
			desiredSize.Width = Math.Min(desiredSize.Width, size.Width);
			double val = 0.75 * rect2.Width * rect2.Height / desiredSize.Width;
			desiredSize.Height = Math.Min(desiredSize.Height, rect2.Height);
			desiredSize.Height = Math.Min(desiredSize.Height, val);
			desiredSize.Height = Math.Min(desiredSize.Height, size.Height);
			desiredSize = (Size)this._secHelper.GetTransformFromDevice().Transform((Point)desiredSize);
			return desiredSize;
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x001AAC70 File Offset: 0x001A8E70
		private Rect GetScreenBounds(Rect boundingBox, Point p)
		{
			if (this._secHelper.IsChildPopup)
			{
				return this._secHelper.GetParentWindowRect();
			}
			NativeMethods.RECT rc = new NativeMethods.RECT(0, 0, 0, 0);
			NativeMethods.RECT rect = PointUtil.FromRect(boundingBox);
			IntPtr intPtr = SafeNativeMethods.MonitorFromRect(ref rect, 2);
			if (intPtr != IntPtr.Zero)
			{
				NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
				monitorinfoex.cbSize = Marshal.SizeOf(typeof(NativeMethods.MONITORINFOEX));
				SafeNativeMethods.GetMonitorInfo(new HandleRef(null, intPtr), monitorinfoex);
				if ((this.Child is MenuBase || this.Child is ToolTip || base.TemplatedParent is MenuItem) && p.X >= (double)monitorinfoex.rcWork.left && p.X <= (double)monitorinfoex.rcWork.right && p.Y >= (double)monitorinfoex.rcWork.top && p.Y <= (double)monitorinfoex.rcWork.bottom)
				{
					rc = monitorinfoex.rcWork;
				}
				else
				{
					rc = monitorinfoex.rcMonitor;
				}
			}
			return PointUtil.ToRect(rc);
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x001AAD7C File Offset: 0x001A8F7C
		private Rect GetMouseRect(PlacementMode placement)
		{
			NativeMethods.POINT mouseCursorPos = this._secHelper.GetMouseCursorPos(this.GetTarget());
			if (placement == PlacementMode.Mouse)
			{
				int num;
				int num2;
				int num3;
				int num4;
				Popup.GetMouseCursorSize(out num, out num2, out num3, out num4);
				return new Rect((double)mouseCursorPos.x, (double)(mouseCursorPos.y - 1), (double)Math.Max(0, num - num3), (double)Math.Max(0, num2 - num4 + 2));
			}
			return new Rect((double)mouseCursorPos.x, (double)mouseCursorPos.y, 0.0, 0.0);
		}

		// Token: 0x06005F1D RID: 24349 RVA: 0x001AAE00 File Offset: 0x001A9000
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void GetMouseCursorSize(out int width, out int height, out int hotX, out int hotY)
		{
			width = (height = (hotX = (hotY = 0)));
			IntPtr cursor = SafeNativeMethods.GetCursor();
			if (cursor != IntPtr.Zero)
			{
				width = (height = 16);
				NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
				bool flag = true;
				try
				{
					UnsafeNativeMethods.GetIconInfo(new HandleRef(null, cursor), out iconinfo);
				}
				catch (Win32Exception)
				{
					flag = false;
				}
				if (flag)
				{
					NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
					int num = 0;
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
					try
					{
						num = UnsafeNativeMethods.GetObject(iconinfo.hbmMask.MakeHandleRef(null), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (num != 0)
					{
						int num2 = bitmap.bmWidth * bitmap.bmHeight / 8;
						byte[] array = new byte[num2 * 2];
						if (UnsafeNativeMethods.GetBitmapBits(iconinfo.hbmMask.MakeHandleRef(null), array.Length, array) != 0)
						{
							bool flag2 = false;
							if (iconinfo.hbmColor.IsInvalid)
							{
								flag2 = true;
								num2 /= 2;
							}
							bool flag3 = true;
							int i = num2;
							for (i--; i >= 0; i--)
							{
								if (array[i] != 255 || (flag2 && array[i + num2] != 0))
								{
									flag3 = false;
									break;
								}
							}
							if (!flag3)
							{
								int num3 = 0;
								while (num3 < num2 && array[num3] == 255 && (!flag2 || array[num3 + num2] == 0))
								{
									num3++;
								}
								int num4 = bitmap.bmWidth / 8;
								int num5 = i % num4 * 8;
								i /= num4;
								int num6 = num3 % num4 * 8;
								num3 /= num4;
								width = num5 - num6 + 1;
								height = i - num3 + 1;
								hotX = iconinfo.xHotspot - num6;
								hotY = iconinfo.yHotspot - num3;
							}
							else
							{
								width = bitmap.bmWidth;
								height = bitmap.bmHeight;
								hotX = iconinfo.xHotspot;
								hotY = iconinfo.yHotspot;
							}
						}
					}
					iconinfo.hbmColor.Dispose();
					iconinfo.hbmMask.Dispose();
				}
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06005F1E RID: 24350 RVA: 0x001AB010 File Offset: 0x001A9210
		// (set) Token: 0x06005F1F RID: 24351 RVA: 0x001AB01E File Offset: 0x001A921E
		private bool IsTransparent
		{
			get
			{
				return this._cacheValid[2];
			}
			set
			{
				this._cacheValid[2] = value;
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06005F20 RID: 24352 RVA: 0x001AB02D File Offset: 0x001A922D
		// (set) Token: 0x06005F21 RID: 24353 RVA: 0x001AB03C File Offset: 0x001A923C
		private bool AnimateFromRight
		{
			get
			{
				return this._cacheValid[32];
			}
			set
			{
				this._cacheValid[32] = value;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06005F22 RID: 24354 RVA: 0x001AB04C File Offset: 0x001A924C
		// (set) Token: 0x06005F23 RID: 24355 RVA: 0x001AB05B File Offset: 0x001A925B
		private bool AnimateFromBottom
		{
			get
			{
				return this._cacheValid[64];
			}
			set
			{
				this._cacheValid[64] = value;
			}
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x06005F24 RID: 24356 RVA: 0x001AB06B File Offset: 0x001A926B
		// (set) Token: 0x06005F25 RID: 24357 RVA: 0x001AB080 File Offset: 0x001A9280
		internal bool HitTestable
		{
			get
			{
				return !this._cacheValid[128];
			}
			set
			{
				this._cacheValid[128] = !value;
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x06005F26 RID: 24358 RVA: 0x001AB096 File Offset: 0x001A9296
		// (set) Token: 0x06005F27 RID: 24359 RVA: 0x001AB0A8 File Offset: 0x001A92A8
		private bool IsDragDropActive
		{
			get
			{
				return this._cacheValid[256];
			}
			set
			{
				this._cacheValid[256] = value;
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x06005F28 RID: 24360 RVA: 0x0003BCFF File Offset: 0x00039EFF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x0400306C RID: 12396
		internal static readonly DependencyProperty TreatMousePlacementAsBottomProperty = DependencyProperty.Register("TreatMousePlacementAsBottom", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.Child" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.Child" /> dependency property.</returns>
		// Token: 0x0400306D RID: 12397
		public static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child", typeof(UIElement), typeof(Popup), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Popup.OnChildChanged)));

		// Token: 0x0400306E RID: 12398
		internal static readonly UncommonField<List<Popup>> RegisteredPopupsField = new UncommonField<List<Popup>>();

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.IsOpen" /> dependency property.</returns>
		// Token: 0x0400306F RID: 12399
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Popup.OnIsOpenChanged), new CoerceValueCallback(Popup.CoerceIsOpen)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.Placement" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.Placement" /> dependency property.</returns>
		// Token: 0x04003070 RID: 12400
		[CommonDependencyProperty]
		public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(Popup), new FrameworkPropertyMetadata(PlacementMode.Bottom, new PropertyChangedCallback(Popup.OnPlacementChanged)), new ValidateValueCallback(Popup.IsValidPlacementMode));

		/// <summary>Identifies the <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacementCallback" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacementCallback" /> dependency property.</returns>
		// Token: 0x04003071 RID: 12401
		public static readonly DependencyProperty CustomPopupPlacementCallbackProperty = DependencyProperty.Register("CustomPopupPlacementCallback", typeof(CustomPopupPlacementCallback), typeof(Popup), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.StaysOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.StaysOpen" /> dependency property.</returns>
		// Token: 0x04003072 RID: 12402
		public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register("StaysOpen", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Popup.OnStaysOpenChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> dependency property.</returns>
		// Token: 0x04003073 RID: 12403
		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(Popup), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> dependency property.</returns>
		// Token: 0x04003074 RID: 12404
		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(Popup), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> dependency property.</returns>
		// Token: 0x04003075 RID: 12405
		public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register("PlacementTarget", typeof(UIElement), typeof(Popup), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Popup.OnPlacementTargetChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementRectangle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementRectangle" /> dependency property.</returns>
		// Token: 0x04003076 RID: 12406
		public static readonly DependencyProperty PlacementRectangleProperty = DependencyProperty.Register("PlacementRectangle", typeof(Rect), typeof(Popup), new FrameworkPropertyMetadata(Rect.Empty, new PropertyChangedCallback(Popup.OnOffsetChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.PopupAnimation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Popup.PopupAnimation" /> dependency property.</returns>
		// Token: 0x04003077 RID: 12407
		[CommonDependencyProperty]
		public static readonly DependencyProperty PopupAnimationProperty = DependencyProperty.Register("PopupAnimation", typeof(PopupAnimation), typeof(Popup), new FrameworkPropertyMetadata(PopupAnimation.None, null, new CoerceValueCallback(Popup.CoercePopupAnimation)), new ValidateValueCallback(Popup.IsValidPopupAnimation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.AllowsTransparency" /> dependency property. </summary>
		// Token: 0x04003078 RID: 12408
		public static readonly DependencyProperty AllowsTransparencyProperty = Window.AllowsTransparencyProperty.AddOwner(typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Popup.OnAllowsTransparencyChanged), new CoerceValueCallback(Popup.CoerceAllowsTransparency)));

		// Token: 0x04003079 RID: 12409
		private static readonly DependencyPropertyKey HasDropShadowPropertyKey = DependencyProperty.RegisterReadOnly("HasDropShadow", typeof(bool), typeof(Popup), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(Popup.CoerceHasDropShadow)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Popup.HasDropShadow" /> dependency property. </summary>
		// Token: 0x0400307A RID: 12410
		public static readonly DependencyProperty HasDropShadowProperty = Popup.HasDropShadowPropertyKey.DependencyProperty;

		// Token: 0x0400307B RID: 12411
		private static readonly EventPrivateKey OpenedKey = new EventPrivateKey();

		// Token: 0x0400307C RID: 12412
		private static readonly EventPrivateKey ClosedKey = new EventPrivateKey();

		// Token: 0x0400307E RID: 12414
		private static readonly UncommonField<Exception> SavedExceptionField = new UncommonField<Exception>();

		// Token: 0x0400307F RID: 12415
		internal const double Tolerance = 0.01;

		// Token: 0x04003080 RID: 12416
		private const int AnimationDelay = 150;

		// Token: 0x04003081 RID: 12417
		internal static TimeSpan AnimationDelayTime = new TimeSpan(0, 0, 0, 0, 150);

		// Token: 0x04003082 RID: 12418
		internal static RoutedEventHandler CloseOnUnloadedHandler;

		// Token: 0x04003083 RID: 12419
		private static readonly UncommonField<PopupRoot> ParentPopupRootField = new UncommonField<PopupRoot>();

		// Token: 0x04003084 RID: 12420
		private Popup.PositionInfo _positionInfo;

		// Token: 0x04003085 RID: 12421
		private SecurityCriticalDataForSet<PopupRoot> _popupRoot;

		// Token: 0x04003086 RID: 12422
		private DispatcherOperation _asyncCreate;

		// Token: 0x04003087 RID: 12423
		private DispatcherTimer _asyncDestroy;

		// Token: 0x04003088 RID: 12424
		private Popup.PopupSecurityHelper _secHelper;

		// Token: 0x04003089 RID: 12425
		private BitVector32 _cacheValid = new BitVector32(0);

		// Token: 0x0400308A RID: 12426
		private const double RestrictPercentage = 0.75;

		// Token: 0x020009EB RID: 2539
		private class PopupModelTreeEnumerator : ModelTreeEnumerator
		{
			// Token: 0x06008978 RID: 35192 RVA: 0x00255161 File Offset: 0x00253361
			internal PopupModelTreeEnumerator(Popup popup, object child) : base(child)
			{
				this._popup = popup;
			}

			// Token: 0x17001F15 RID: 7957
			// (get) Token: 0x06008979 RID: 35193 RVA: 0x00255171 File Offset: 0x00253371
			protected override bool IsUnchanged
			{
				get
				{
					return base.Content == this._popup.Child;
				}
			}

			// Token: 0x0400466F RID: 18031
			private Popup _popup;
		}

		// Token: 0x020009EC RID: 2540
		private enum InterestPoint
		{
			// Token: 0x04004671 RID: 18033
			TopLeft,
			// Token: 0x04004672 RID: 18034
			TopRight,
			// Token: 0x04004673 RID: 18035
			BottomLeft,
			// Token: 0x04004674 RID: 18036
			BottomRight,
			// Token: 0x04004675 RID: 18037
			Center
		}

		// Token: 0x020009ED RID: 2541
		private struct PointCombination
		{
			// Token: 0x0600897A RID: 35194 RVA: 0x00255186 File Offset: 0x00253386
			public PointCombination(Popup.InterestPoint targetInterestPoint, Popup.InterestPoint childInterestPoint)
			{
				this.TargetInterestPoint = targetInterestPoint;
				this.ChildInterestPoint = childInterestPoint;
			}

			// Token: 0x04004676 RID: 18038
			public Popup.InterestPoint TargetInterestPoint;

			// Token: 0x04004677 RID: 18039
			public Popup.InterestPoint ChildInterestPoint;
		}

		// Token: 0x020009EE RID: 2542
		private class PositionInfo
		{
			// Token: 0x04004678 RID: 18040
			public int X;

			// Token: 0x04004679 RID: 18041
			public int Y;

			// Token: 0x0400467A RID: 18042
			public Size ChildSize;

			// Token: 0x0400467B RID: 18043
			public Rect MouseRect = Rect.Empty;
		}

		// Token: 0x020009EF RID: 2543
		private enum CacheBits
		{
			// Token: 0x0400467D RID: 18045
			CaptureEngaged = 1,
			// Token: 0x0400467E RID: 18046
			IsTransparent,
			// Token: 0x0400467F RID: 18047
			OnClosedHandlerReopen = 4,
			// Token: 0x04004680 RID: 18048
			DropOppositeSet = 8,
			// Token: 0x04004681 RID: 18049
			DropOpposite = 16,
			// Token: 0x04004682 RID: 18050
			AnimateFromRight = 32,
			// Token: 0x04004683 RID: 18051
			AnimateFromBottom = 64,
			// Token: 0x04004684 RID: 18052
			HitTestable = 128,
			// Token: 0x04004685 RID: 18053
			IsDragDropActive = 256,
			// Token: 0x04004686 RID: 18054
			IsIgnoringMouseEvents = 512
		}

		// Token: 0x020009F0 RID: 2544
		private class PopupSecurityHelper
		{
			// Token: 0x0600897C RID: 35196 RVA: 0x0000326D File Offset: 0x0000146D
			internal PopupSecurityHelper()
			{
			}

			// Token: 0x17001F16 RID: 7958
			// (get) Token: 0x0600897D RID: 35197 RVA: 0x002551A9 File Offset: 0x002533A9
			internal bool IsChildPopup
			{
				[SecurityCritical]
				[SecurityTreatAsSafe]
				get
				{
					if (!this._isChildPopupInitialized)
					{
						this._isChildPopup = (BrowserInteropHelper.IsBrowserHosted || !SecurityHelper.CheckUnmanagedCodePermission());
						this._isChildPopupInitialized = true;
					}
					return this._isChildPopup;
				}
			}

			// Token: 0x0600897E RID: 35198 RVA: 0x002551D8 File Offset: 0x002533D8
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal bool IsWindowAlive()
			{
				if (this._window != null)
				{
					HwndSource value = this._window.Value;
					return value != null && !value.IsDisposed;
				}
				return false;
			}

			// Token: 0x0600897F RID: 35199 RVA: 0x0025520C File Offset: 0x0025340C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Point ClientToScreen(Visual rootVisual, Point clientPoint)
			{
				HwndSource hwndSource = Popup.PopupSecurityHelper.GetPresentationSource(rootVisual) as HwndSource;
				if (hwndSource != null)
				{
					return PointUtil.ToPoint(this.ClientToScreen(hwndSource, clientPoint));
				}
				return clientPoint;
			}

			// Token: 0x06008980 RID: 35200 RVA: 0x00255238 File Offset: 0x00253438
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private NativeMethods.POINT ClientToScreen(HwndSource hwnd, Point clientPt)
			{
				bool isChildPopup = this.IsChildPopup;
				HwndSource hwndSource = null;
				if (isChildPopup)
				{
					hwndSource = HwndSource.CriticalFromHwnd(this.ParentHandle);
				}
				Point pointScreen = clientPt;
				if (!isChildPopup || hwndSource != hwnd)
				{
					pointScreen = PointUtil.ClientToScreen(clientPt, hwnd);
				}
				if (isChildPopup && hwndSource != hwnd)
				{
					pointScreen = PointUtil.ScreenToClient(pointScreen, hwndSource);
				}
				return new NativeMethods.POINT((int)pointScreen.X, (int)pointScreen.Y);
			}

			// Token: 0x06008981 RID: 35201 RVA: 0x00255294 File Offset: 0x00253494
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal NativeMethods.POINT GetMouseCursorPos(Visual targetVisual)
			{
				if (Mouse.DirectlyOver != null)
				{
					HwndSource hwndSource = null;
					if (targetVisual != null)
					{
						hwndSource = (Popup.PopupSecurityHelper.GetPresentationSource(targetVisual) as HwndSource);
					}
					IInputElement inputElement = targetVisual as IInputElement;
					if (inputElement != null)
					{
						Point point = Mouse.GetPosition(inputElement);
						if (hwndSource != null && !hwndSource.IsDisposed)
						{
							Visual rootVisual = hwndSource.RootVisual;
							CompositionTarget compositionTarget = hwndSource.CompositionTarget;
							if (rootVisual != null && compositionTarget != null)
							{
								GeneralTransform generalTransform = targetVisual.TransformToAncestor(rootVisual);
								Matrix matrix = PointUtil.GetVisualTransform(rootVisual) * compositionTarget.TransformToDevice;
								generalTransform.TryTransform(point, out point);
								point = matrix.Transform(point);
								return this.ClientToScreen(hwndSource, point);
							}
						}
					}
				}
				NativeMethods.POINT point2 = new NativeMethods.POINT(0, 0);
				UnsafeNativeMethods.TryGetCursorPos(point2);
				return point2;
			}

			// Token: 0x06008982 RID: 35202 RVA: 0x0025533C File Offset: 0x0025353C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void SetPopupPos(bool position, int x, int y, bool size, int width, int height)
			{
				int num = 20;
				if (!position)
				{
					num |= 2;
				}
				if (!size)
				{
					num |= 1;
				}
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), new HandleRef(null, IntPtr.Zero), x, y, width, height, num);
			}

			// Token: 0x06008983 RID: 35203 RVA: 0x00255380 File Offset: 0x00253580
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Rect GetParentWindowRect()
			{
				NativeMethods.RECT rc = new NativeMethods.RECT(0, 0, 0, 0);
				IntPtr parentHandle = this.ParentHandle;
				if (parentHandle != IntPtr.Zero)
				{
					SafeNativeMethods.GetClientRect(new HandleRef(null, parentHandle), ref rc);
				}
				return PointUtil.ToRect(rc);
			}

			// Token: 0x06008984 RID: 35204 RVA: 0x002553C0 File Offset: 0x002535C0
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Matrix GetTransformToDevice()
			{
				CompositionTarget compositionTarget = this._window.Value.CompositionTarget;
				if (compositionTarget != null && !compositionTarget.IsDisposed)
				{
					return compositionTarget.TransformToDevice;
				}
				return Matrix.Identity;
			}

			// Token: 0x06008985 RID: 35205 RVA: 0x002553F8 File Offset: 0x002535F8
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal static Matrix GetTransformToDevice(Visual targetVisual)
			{
				HwndSource hwndSource = null;
				if (targetVisual != null)
				{
					hwndSource = (Popup.PopupSecurityHelper.GetPresentationSource(targetVisual) as HwndSource);
				}
				if (hwndSource != null)
				{
					CompositionTarget compositionTarget = hwndSource.CompositionTarget;
					if (compositionTarget != null && !compositionTarget.IsDisposed)
					{
						return compositionTarget.TransformToDevice;
					}
				}
				return Matrix.Identity;
			}

			// Token: 0x06008986 RID: 35206 RVA: 0x00255438 File Offset: 0x00253638
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal Matrix GetTransformFromDevice()
			{
				CompositionTarget compositionTarget = this._window.Value.CompositionTarget;
				if (compositionTarget != null && !compositionTarget.IsDisposed)
				{
					return compositionTarget.TransformFromDevice;
				}
				return Matrix.Identity;
			}

			// Token: 0x06008987 RID: 35207 RVA: 0x0025546D File Offset: 0x0025366D
			[SecurityCritical]
			internal void SetWindowRootVisual(Visual v)
			{
				this._window.Value.RootVisual = v;
			}

			// Token: 0x06008988 RID: 35208 RVA: 0x00255480 File Offset: 0x00253680
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal static bool IsVisualPresentationSourceNull(Visual visual)
			{
				return Popup.PopupSecurityHelper.GetPresentationSource(visual) == null;
			}

			// Token: 0x06008989 RID: 35209 RVA: 0x0025548C File Offset: 0x0025368C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void ShowWindow()
			{
				if (this.IsChildPopup)
				{
					IntPtr lastWebOCHwnd = this.GetLastWebOCHwnd();
					UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), (lastWebOCHwnd == IntPtr.Zero) ? NativeMethods.HWND_TOP : new HandleRef(null, lastWebOCHwnd), 0, 0, 0, 0, 83);
					return;
				}
				if (!FrameworkCompatibilityPreferences.GetUseSetWindowPosForTopmostWindows())
				{
					UnsafeNativeMethods.ShowWindow(new HandleRef(null, this.Handle), 8);
					return;
				}
				UnsafeNativeMethods.SetWindowPos(new HandleRef(null, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 595);
			}

			// Token: 0x0600898A RID: 35210 RVA: 0x00255517 File Offset: 0x00253717
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void HideWindow()
			{
				UnsafeNativeMethods.ShowWindow(new HandleRef(null, this.Handle), 0);
			}

			// Token: 0x0600898B RID: 35211 RVA: 0x0025552C File Offset: 0x0025372C
			[SecurityCritical]
			private IntPtr GetLastWebOCHwnd()
			{
				IntPtr window = UnsafeNativeMethods.GetWindow(new HandleRef(null, this.Handle), 1);
				StringBuilder stringBuilder = new StringBuilder(260);
				while (window != IntPtr.Zero)
				{
					if (UnsafeNativeMethods.GetClassName(new HandleRef(null, window), stringBuilder, 260) == 0)
					{
						throw new Win32Exception();
					}
					if (string.Compare(stringBuilder.ToString(), "Shell Embedding", StringComparison.OrdinalIgnoreCase) == 0)
					{
						break;
					}
					window = UnsafeNativeMethods.GetWindow(new HandleRef(null, window), 3);
				}
				return window;
			}

			// Token: 0x0600898C RID: 35212 RVA: 0x002555A4 File Offset: 0x002537A4
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void SetHitTestable(bool hitTestable)
			{
				if (!this.IsChildPopup)
				{
					SecurityHelper.DemandUnmanagedCode();
				}
				IntPtr handle = this.Handle;
				int num = UnsafeNativeMethods.GetWindowLong(new HandleRef(this, handle), -20);
				int num2 = num;
				if ((num2 & 32) == 0 != hitTestable)
				{
					if (hitTestable)
					{
						num = (num2 & -33);
					}
					else
					{
						num = (num2 | 32);
					}
					UnsafeNativeMethods.CriticalSetWindowLong(new HandleRef(null, handle), -20, (IntPtr)num);
				}
			}

			// Token: 0x0600898D RID: 35213 RVA: 0x00255604 File Offset: 0x00253804
			private static Visual FindMainTreeVisual(Visual v)
			{
				DependencyObject dependencyObject = null;
				DependencyObject dependencyObject2 = v;
				while (dependencyObject2 != null)
				{
					dependencyObject = dependencyObject2;
					PopupRoot popupRoot = dependencyObject2 as PopupRoot;
					if (popupRoot != null)
					{
						dependencyObject2 = popupRoot.Parent;
						Popup popup = dependencyObject2 as Popup;
						if (popup != null)
						{
							UIElement placementTarget = popup.PlacementTarget;
							if (placementTarget != null)
							{
								dependencyObject2 = placementTarget;
							}
						}
					}
					else
					{
						dependencyObject2 = VisualTreeHelper.GetParent(dependencyObject2);
					}
				}
				return dependencyObject as Visual;
			}

			// Token: 0x0600898E RID: 35214 RVA: 0x00255658 File Offset: 0x00253858
			[SecurityCritical]
			internal void BuildWindow(int x, int y, Visual placementTarget, bool transparent, HwndSourceHook hook, AutoResizedEventHandler handler, HwndDpiChangedEventHandler dpiChangedHandler)
			{
				transparent = (transparent && !this.IsChildPopup);
				Visual visual = placementTarget;
				if (this.IsChildPopup)
				{
					visual = Popup.PopupSecurityHelper.FindMainTreeVisual(placementTarget);
				}
				HwndSource hwndSource = Popup.PopupSecurityHelper.GetPresentationSource(visual) as HwndSource;
				IntPtr intPtr = IntPtr.Zero;
				if (hwndSource != null)
				{
					intPtr = Popup.PopupSecurityHelper.GetHandle(hwndSource);
				}
				int windowClassStyle = 0;
				int num = 67108864;
				int num2 = 134217856;
				if (this.IsChildPopup)
				{
					num |= 1073741824;
				}
				else
				{
					num |= int.MinValue;
					num2 |= 8;
				}
				HwndSourceParameters parameters = new HwndSourceParameters(string.Empty);
				parameters.WindowClassStyle = windowClassStyle;
				parameters.WindowStyle = num;
				parameters.ExtendedWindowStyle = num2;
				parameters.SetPosition(x, y);
				if (this.IsChildPopup)
				{
					if (intPtr != IntPtr.Zero)
					{
						parameters.ParentWindow = intPtr;
					}
					else
					{
						SecurityHelper.DemandUIWindowPermission();
					}
				}
				else
				{
					parameters.UsesPerPixelOpacity = transparent;
					if (intPtr != IntPtr.Zero && Popup.PopupSecurityHelper.ConnectedToForegroundWindow(intPtr))
					{
						parameters.ParentWindow = intPtr;
					}
				}
				HwndSource hwndSource2 = new HwndSource(parameters);
				new UIPermission(UIPermissionWindow.AllWindows).Assert();
				try
				{
					hwndSource2.AddHook(hook);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				this._window = new SecurityCriticalDataClass<HwndSource>(hwndSource2);
				HwndTarget compositionTarget = hwndSource2.CompositionTarget;
				compositionTarget.BackgroundColor = (transparent ? Colors.Transparent : Colors.Black);
				hwndSource2.AutoResized += handler;
				hwndSource2.DpiChanged += dpiChangedHandler;
			}

			// Token: 0x0600898F RID: 35215 RVA: 0x002557C8 File Offset: 0x002539C8
			[SecurityCritical]
			private static bool ConnectedToForegroundWindow(IntPtr window)
			{
				IntPtr foregroundWindow = UnsafeNativeMethods.GetForegroundWindow();
				while (window != IntPtr.Zero)
				{
					if (window == foregroundWindow)
					{
						return true;
					}
					window = UnsafeNativeMethods.GetParent(new HandleRef(null, window));
				}
				return false;
			}

			// Token: 0x06008990 RID: 35216 RVA: 0x00255804 File Offset: 0x00253A04
			[SecurityCritical]
			private static IntPtr GetHandle(HwndSource hwnd)
			{
				if (hwnd == null)
				{
					return IntPtr.Zero;
				}
				return hwnd.CriticalHandle;
			}

			// Token: 0x06008991 RID: 35217 RVA: 0x00255818 File Offset: 0x00253A18
			[SecurityCritical]
			private static IntPtr GetParentHandle(HwndSource hwnd)
			{
				if (hwnd != null)
				{
					IntPtr handle = Popup.PopupSecurityHelper.GetHandle(hwnd);
					if (handle != IntPtr.Zero)
					{
						return UnsafeNativeMethods.GetParent(new HandleRef(null, handle));
					}
				}
				return IntPtr.Zero;
			}

			// Token: 0x17001F17 RID: 7959
			// (get) Token: 0x06008992 RID: 35218 RVA: 0x0025584E File Offset: 0x00253A4E
			private IntPtr Handle
			{
				[SecurityCritical]
				get
				{
					return Popup.PopupSecurityHelper.GetHandle(this._window.Value);
				}
			}

			// Token: 0x17001F18 RID: 7960
			// (get) Token: 0x06008993 RID: 35219 RVA: 0x00255860 File Offset: 0x00253A60
			private IntPtr ParentHandle
			{
				[SecurityCritical]
				get
				{
					return Popup.PopupSecurityHelper.GetParentHandle(this._window.Value);
				}
			}

			// Token: 0x06008994 RID: 35220 RVA: 0x00255872 File Offset: 0x00253A72
			[SecurityCritical]
			private static PresentationSource GetPresentationSource(Visual visual)
			{
				if (visual == null)
				{
					return null;
				}
				return PresentationSource.CriticalFromVisual(visual);
			}

			// Token: 0x06008995 RID: 35221 RVA: 0x00255880 File Offset: 0x00253A80
			[SecurityCritical]
			[SecurityTreatAsSafe]
			internal void ForceMsaaToUiaBridge(PopupRoot popupRoot)
			{
				if (this.Handle != IntPtr.Zero && (UnsafeNativeMethods.IsWinEventHookInstalled(32773) || UnsafeNativeMethods.IsWinEventHookInstalled(32778)))
				{
					PopupRootAutomationPeer popupRootAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(popupRoot) as PopupRootAutomationPeer;
					if (popupRootAutomationPeer != null)
					{
						if (popupRootAutomationPeer.Hwnd == IntPtr.Zero)
						{
							popupRootAutomationPeer.Hwnd = this.Handle;
						}
						IRawElementProviderSimple el = popupRootAutomationPeer.ProviderFromPeer(popupRootAutomationPeer);
						IntPtr intPtr = AutomationInteropProvider.ReturnRawElementProvider(this.Handle, IntPtr.Zero, new IntPtr(-4), el);
						if (intPtr != IntPtr.Zero)
						{
							IAccessible accessible = null;
							Guid guid = new Guid("618736e0-3c3d-11cf-810c-00aa00389b71");
							if (UnsafeNativeMethods.ObjectFromLresult(intPtr, ref guid, IntPtr.Zero, ref accessible) == 0)
							{
							}
						}
					}
				}
			}

			// Token: 0x06008996 RID: 35222 RVA: 0x00255940 File Offset: 0x00253B40
			[SecurityCritical]
			internal void DestroyWindow(HwndSourceHook hook, AutoResizedEventHandler onAutoResizedEventHandler, HwndDpiChangedEventHandler onDpiChagnedEventHandler)
			{
				HwndSource value = this._window.Value;
				this._window = null;
				if (!value.IsDisposed)
				{
					value.AutoResized -= onAutoResizedEventHandler;
					value.DpiChanged -= onDpiChagnedEventHandler;
					new UIPermission(UIPermissionWindow.AllWindows).Assert();
					try
					{
						value.RemoveHook(hook);
						value.RootVisual = null;
						value.Dispose();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}

			// Token: 0x04004687 RID: 18055
			[SecurityCritical]
			private bool _isChildPopup;

			// Token: 0x04004688 RID: 18056
			[SecurityCritical]
			private bool _isChildPopupInitialized;

			// Token: 0x04004689 RID: 18057
			private SecurityCriticalDataClass<HwndSource> _window;

			// Token: 0x0400468A RID: 18058
			private const string WebOCWindowClassName = "Shell Embedding";
		}

		// Token: 0x020009F1 RID: 2545
		private static class PopupInitialPlacementHelper
		{
			// Token: 0x17001F19 RID: 7961
			// (get) Token: 0x06008997 RID: 35223 RVA: 0x002559B0 File Offset: 0x00253BB0
			internal static bool IsPerMonitorDpiScalingActive
			{
				[SecuritySafeCritical]
				get
				{
					if (!HwndTarget.IsPerMonitorDpiScalingEnabled)
					{
						return false;
					}
					if (HwndTarget.IsProcessPerMonitorDpiAware != null)
					{
						return HwndTarget.IsProcessPerMonitorDpiAware.Value;
					}
					return DpiUtil.GetProcessDpiAwareness(IntPtr.Zero) == NativeMethods.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE;
				}
			}

			// Token: 0x06008998 RID: 35224 RVA: 0x002559F0 File Offset: 0x00253BF0
			private static NativeMethods.POINTSTRUCT? GetPlacementTargetOriginInScreenCoordinates(Popup popup)
			{
				UIElement uielement = ((popup != null) ? popup.GetTarget() : null) as UIElement;
				if (uielement != null)
				{
					Visual rootVisual = Popup.GetRootVisual(uielement);
					GeneralTransform generalTransform = Popup.TransformToClient(uielement, rootVisual);
					Point clientPoint;
					if (generalTransform.TryTransform(new Point(0.0, 0.0), out clientPoint))
					{
						Point point = popup._secHelper.ClientToScreen(rootVisual, clientPoint);
						return new NativeMethods.POINTSTRUCT?(new NativeMethods.POINTSTRUCT((int)point.X, (int)point.Y));
					}
				}
				return null;
			}

			// Token: 0x06008999 RID: 35225 RVA: 0x00255A78 File Offset: 0x00253C78
			[SecuritySafeCritical]
			internal static NativeMethods.POINTSTRUCT GetPlacementOrigin(Popup popup)
			{
				NativeMethods.POINTSTRUCT result = new NativeMethods.POINTSTRUCT(0, 0);
				if (Popup.PopupInitialPlacementHelper.IsPerMonitorDpiScalingActive)
				{
					NativeMethods.POINTSTRUCT? placementTargetOriginInScreenCoordinates = Popup.PopupInitialPlacementHelper.GetPlacementTargetOriginInScreenCoordinates(popup);
					if (placementTargetOriginInScreenCoordinates != null)
					{
						try
						{
							IntPtr handle = SafeNativeMethods.MonitorFromPoint(placementTargetOriginInScreenCoordinates.Value, 2);
							NativeMethods.MONITORINFOEX monitorinfoex = new NativeMethods.MONITORINFOEX();
							SafeNativeMethods.GetMonitorInfo(new HandleRef(null, handle), monitorinfoex);
							result.x = monitorinfoex.rcMonitor.left;
							result.y = monitorinfoex.rcMonitor.top;
						}
						catch (Win32Exception)
						{
						}
					}
				}
				return result;
			}
		}
	}
}
