using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that can be dragged by the user. </summary>
	// Token: 0x020005AD RID: 1453
	[DefaultEvent("DragDelta")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Thumb : Control
	{
		// Token: 0x060060B6 RID: 24758 RVA: 0x001B1B30 File Offset: 0x001AFD30
		static Thumb()
		{
			Thumb.DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(Thumb));
			Thumb.DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(Thumb));
			Thumb.DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(Thumb));
			Thumb.IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Thumb.OnIsDraggingPropertyChanged)));
			Thumb.IsDraggingProperty = Thumb.IsDraggingPropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(typeof(Thumb)));
			Thumb._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Thumb));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			EventManager.RegisterClassHandler(typeof(Thumb), Mouse.LostMouseCaptureEvent, new MouseEventHandler(Thumb.OnLostMouseCapture));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control receives logical focus and mouse capture.</summary>
		// Token: 0x1400011F RID: 287
		// (add) Token: 0x060060B7 RID: 24759 RVA: 0x001B1CB0 File Offset: 0x001AFEB0
		// (remove) Token: 0x060060B8 RID: 24760 RVA: 0x001B1CBE File Offset: 0x001AFEBE
		[Category("Behavior")]
		public event DragStartedEventHandler DragStarted
		{
			add
			{
				base.AddHandler(Thumb.DragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragStartedEvent, value);
			}
		}

		/// <summary>Occurs one or more times as the mouse changes position when a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has logical focus and mouse capture. </summary>
		// Token: 0x14000120 RID: 288
		// (add) Token: 0x060060B9 RID: 24761 RVA: 0x001B1CCC File Offset: 0x001AFECC
		// (remove) Token: 0x060060BA RID: 24762 RVA: 0x001B1CDA File Offset: 0x001AFEDA
		[Category("Behavior")]
		public event DragDeltaEventHandler DragDelta
		{
			add
			{
				base.AddHandler(Thumb.DragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragDeltaEvent, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control loses mouse capture.</summary>
		// Token: 0x14000121 RID: 289
		// (add) Token: 0x060060BB RID: 24763 RVA: 0x001B1CE8 File Offset: 0x001AFEE8
		// (remove) Token: 0x060060BC RID: 24764 RVA: 0x001B1CF6 File Offset: 0x001AFEF6
		[Category("Behavior")]
		public event DragCompletedEventHandler DragCompleted
		{
			add
			{
				base.AddHandler(Thumb.DragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragCompletedEvent, value);
			}
		}

		/// <summary>Gets whether the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has logical focus and mouse capture and the left mouse button is pressed.   </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has focus and mouse capture; otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x001B1D04 File Offset: 0x001AFF04
		// (set) Token: 0x060060BE RID: 24766 RVA: 0x001B1D16 File Offset: 0x001AFF16
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsDragging
		{
			get
			{
				return (bool)base.GetValue(Thumb.IsDraggingProperty);
			}
			protected set
			{
				base.SetValue(Thumb.IsDraggingPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x001B1D2C File Offset: 0x001AFF2C
		private static void OnIsDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Thumb thumb = (Thumb)d;
			thumb.OnDraggingChanged(e);
			thumb.UpdateVisualState();
		}

		/// <summary>Cancels a drag operation for the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		// Token: 0x060060C0 RID: 24768 RVA: 0x001B1D50 File Offset: 0x001AFF50
		public void CancelDrag()
		{
			if (this.IsDragging)
			{
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.RaiseEvent(new DragCompletedEventArgs(this._previousScreenCoordPosition.X - this._originScreenCoordPosition.X, this._previousScreenCoordPosition.Y - this._originScreenCoordPosition.Y, true));
			}
		}

		/// <summary>Responds to a change in the value of the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> property. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C1 RID: 24769 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnDraggingChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x001B1DB8 File Offset: 0x001AFFB8
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsDragging)
			{
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
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

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control.</returns>
		// Token: 0x060060C3 RID: 24771 RVA: 0x001B1E42 File Offset: 0x001B0042
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ThumbAutomationPeer(this);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.MouseLeftButtonDown" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C4 RID: 24772 RVA: 0x001B1E4C File Offset: 0x001B004C
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!this.IsDragging)
			{
				e.Handled = true;
				base.Focus();
				base.CaptureMouse();
				base.SetValue(Thumb.IsDraggingPropertyKey, true);
				this._originThumbPoint = e.GetPosition(this);
				this._previousScreenCoordPosition = (this._originScreenCoordPosition = SafeSecurityHelper.ClientToScreen(this, this._originThumbPoint));
				bool flag = true;
				try
				{
					base.RaiseEvent(new DragStartedEventArgs(this._originThumbPoint.X, this._originThumbPoint.Y));
					flag = false;
				}
				finally
				{
					if (flag)
					{
						this.CancelDrag();
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.MouseLeftButtonUp" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C5 RID: 24773 RVA: 0x001B1EF4 File Offset: 0x001B00F4
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (base.IsMouseCaptured && this.IsDragging)
			{
				e.Handled = true;
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.ReleaseMouseCapture();
				Point point = SafeSecurityHelper.ClientToScreen(this, e.MouseDevice.GetPosition(this));
				base.RaiseEvent(new DragCompletedEventArgs(point.X - this._originScreenCoordPosition.X, point.Y - this._originScreenCoordPosition.Y, false));
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x001B1F78 File Offset: 0x001B0178
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			Thumb thumb = (Thumb)sender;
			if (Mouse.Captured != thumb)
			{
				thumb.CancelDrag();
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C7 RID: 24775 RVA: 0x001B1F9C File Offset: 0x001B019C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.IsDragging)
			{
				if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
				{
					Point position = e.GetPosition(this);
					Point point = SafeSecurityHelper.ClientToScreen(this, position);
					if (point != this._previousScreenCoordPosition)
					{
						this._previousScreenCoordPosition = point;
						e.Handled = true;
						base.RaiseEvent(new DragDeltaEventArgs(position.X - this._originThumbPoint.X, position.Y - this._originThumbPoint.Y));
						return;
					}
				}
				else
				{
					if (e.MouseDevice.Captured == this)
					{
						base.ReleaseMouseCapture();
					}
					base.ClearValue(Thumb.IsDraggingPropertyKey);
					this._originThumbPoint.X = 0.0;
					this._originThumbPoint.Y = 0.0;
				}
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x060060C8 RID: 24776 RVA: 0x0003BCFF File Offset: 0x00039EFF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x060060C9 RID: 24777 RVA: 0x001B2071 File Offset: 0x001B0271
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Thumb._dType;
			}
		}

		// Token: 0x04003114 RID: 12564
		private static readonly DependencyPropertyKey IsDraggingPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> dependency property.</returns>
		// Token: 0x04003115 RID: 12565
		public static readonly DependencyProperty IsDraggingProperty;

		// Token: 0x04003116 RID: 12566
		private Point _originThumbPoint;

		// Token: 0x04003117 RID: 12567
		private Point _originScreenCoordPosition;

		// Token: 0x04003118 RID: 12568
		private Point _previousScreenCoordPosition;

		// Token: 0x04003119 RID: 12569
		private static DependencyObjectType _dType;
	}
}
