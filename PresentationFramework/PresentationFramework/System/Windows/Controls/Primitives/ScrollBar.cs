using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that provides a scroll bar that has a sliding <see cref="T:System.Windows.Controls.Primitives.Thumb" /> whose position corresponds to a value.</summary>
	// Token: 0x020005A4 RID: 1444
	[Localizability(LocalizationCategory.NeverLocalize)]
	[TemplatePart(Name = "PART_Track", Type = typeof(Track))]
	public class ScrollBar : RangeBase
	{
		/// <summary>Occurs one or more times as content scrolls in a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> when the user moves the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> by using the mouse.</summary>
		// Token: 0x1400011B RID: 283
		// (add) Token: 0x06005F7E RID: 24446 RVA: 0x001AC2B4 File Offset: 0x001AA4B4
		// (remove) Token: 0x06005F7F RID: 24447 RVA: 0x001AC2C2 File Offset: 0x001AA4C2
		[Category("Behavior")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.AddHandler(ScrollBar.ScrollEvent, value);
			}
			remove
			{
				base.RemoveHandler(ScrollBar.ScrollEvent, value);
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is displayed horizontally or vertically.  </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> enumeration value that defines whether the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is displayed horizontally or vertically. The default is <see cref="F:System.Windows.Controls.Orientation.Vertical" />.</returns>
		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x06005F80 RID: 24448 RVA: 0x001AC2D0 File Offset: 0x001AA4D0
		// (set) Token: 0x06005F81 RID: 24449 RVA: 0x001AC2E2 File Offset: 0x001AA4E2
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ScrollBar.OrientationProperty);
			}
			set
			{
				base.SetValue(ScrollBar.OrientationProperty, value);
			}
		}

		/// <summary>Gets or sets the amount of the scrollable content that is currently visible.  </summary>
		/// <returns>The amount of the scrollable content that is currently visible. The default is 0.</returns>
		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x001AC2F5 File Offset: 0x001AA4F5
		// (set) Token: 0x06005F83 RID: 24451 RVA: 0x001AC307 File Offset: 0x001AA507
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double ViewportSize
		{
			get
			{
				return (double)base.GetValue(ScrollBar.ViewportSizeProperty);
			}
			set
			{
				base.SetValue(ScrollBar.ViewportSizeProperty, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Primitives.Track" /> for a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> control.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Primitives.Track" /> that is used with a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> control.</returns>
		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x001AC31A File Offset: 0x001AA51A
		public Track Track
		{
			get
			{
				return this._track;
			}
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for this <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.ScrollBarAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> control.</returns>
		// Token: 0x06005F85 RID: 24453 RVA: 0x001AC322 File Offset: 0x001AA522
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ScrollBarAutomationPeer(this);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseLeftButtonDown" /> event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005F86 RID: 24454 RVA: 0x001AC32C File Offset: 0x001AA52C
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			this._thumbOffset = default(Vector);
			if (this.Track != null && this.Track.IsMouseOver && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				Point position = e.MouseDevice.GetPosition(this.Track);
				double num = this.Track.ValueFromPoint(position);
				if (Shape.IsDoubleFinite(num))
				{
					this.ChangeValue(num, false);
				}
				if (this.Track.Thumb != null && this.Track.Thumb.IsMouseOver)
				{
					Point position2 = e.MouseDevice.GetPosition(this.Track.Thumb);
					this._thumbOffset = position2 - new Point(this.Track.Thumb.ActualWidth * 0.5, this.Track.Thumb.ActualHeight * 0.5);
				}
				else
				{
					e.Handled = true;
				}
			}
			base.OnPreviewMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.PreviewMouseRightButtonUp" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005F87 RID: 24455 RVA: 0x001AC42C File Offset: 0x001AA62C
		protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
		{
			if (this.Track != null)
			{
				this._latestRightButtonClickPoint = e.MouseDevice.GetPosition(this.Track);
			}
			else
			{
				this._latestRightButtonClickPoint = new Point(-1.0, -1.0);
			}
			base.OnPreviewMouseRightButtonUp(e);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> is enabled in a <see cref="T:System.Windows.Controls.ScrollViewer" /> and the size of the content is larger than the display area; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x001AC47E File Offset: 0x001AA67E
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this._canScroll;
			}
		}

		/// <summary>Creates the visual tree for the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />.</summary>
		// Token: 0x06005F89 RID: 24457 RVA: 0x001AC490 File Offset: 0x001AA690
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._track = (base.GetTemplateChild("PART_Track") as Track);
		}

		// Token: 0x06005F8A RID: 24458 RVA: 0x001AC4B0 File Offset: 0x001AA6B0
		private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			ScrollBar scrollBar = sender as ScrollBar;
			if (scrollBar == null)
			{
				return;
			}
			scrollBar._hasScrolled = false;
			scrollBar._previousValue = scrollBar.Value;
		}

		// Token: 0x06005F8B RID: 24459 RVA: 0x001AC4DC File Offset: 0x001AA6DC
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			ScrollBar scrollBar = sender as ScrollBar;
			if (scrollBar == null)
			{
				return;
			}
			scrollBar.UpdateValue(e.HorizontalChange + scrollBar._thumbOffset.X, e.VerticalChange + scrollBar._thumbOffset.Y);
		}

		// Token: 0x06005F8C RID: 24460 RVA: 0x001AC520 File Offset: 0x001AA720
		private void UpdateValue(double horizontalDragDelta, double verticalDragDelta)
		{
			if (this.Track != null)
			{
				double num = this.Track.ValueFromDistance(horizontalDragDelta, verticalDragDelta);
				if (Shape.IsDoubleFinite(num) && !DoubleUtil.IsZero(num))
				{
					double value = base.Value;
					double num2 = value + num;
					double value2;
					if (this.Orientation == Orientation.Horizontal)
					{
						value2 = Math.Abs(verticalDragDelta);
					}
					else
					{
						value2 = Math.Abs(horizontalDragDelta);
					}
					if (DoubleUtil.GreaterThan(value2, 150.0))
					{
						num2 = this._previousValue;
					}
					if (!DoubleUtil.AreClose(value, num2))
					{
						this._hasScrolled = true;
						this.ChangeValue(num2, true);
						this.RaiseScrollEvent(ScrollEventType.ThumbTrack);
					}
				}
			}
		}

		// Token: 0x06005F8D RID: 24461 RVA: 0x001AC5B2 File Offset: 0x001AA7B2
		private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			((ScrollBar)sender).OnThumbDragCompleted(e);
		}

		// Token: 0x06005F8E RID: 24462 RVA: 0x001AC5C0 File Offset: 0x001AA7C0
		private void OnThumbDragCompleted(DragCompletedEventArgs e)
		{
			if (this._hasScrolled)
			{
				this.FinishDrag();
				this.RaiseScrollEvent(ScrollEventType.EndScroll);
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x001AC5D8 File Offset: 0x001AA7D8
		private IInputElement CommandTarget
		{
			get
			{
				IInputElement inputElement = base.TemplatedParent as IInputElement;
				if (inputElement == null)
				{
					inputElement = this;
				}
				return inputElement;
			}
		}

		// Token: 0x06005F90 RID: 24464 RVA: 0x001AC5F8 File Offset: 0x001AA7F8
		private void FinishDrag()
		{
			double value = base.Value;
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = (this.Orientation == Orientation.Horizontal) ? ScrollBar.DeferScrollToHorizontalOffsetCommand : ScrollBar.DeferScrollToVerticalOffsetCommand;
			if (routedCommand.CanExecute(value, commandTarget))
			{
				this.ChangeValue(value, false);
			}
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x001AC640 File Offset: 0x001AA840
		private void ChangeValue(double newValue, bool defer)
		{
			newValue = Math.Min(Math.Max(newValue, base.Minimum), base.Maximum);
			if (this.IsStandalone)
			{
				base.Value = newValue;
				return;
			}
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = null;
			bool flag = this.Orientation == Orientation.Horizontal;
			if (defer)
			{
				routedCommand = (flag ? ScrollBar.DeferScrollToHorizontalOffsetCommand : ScrollBar.DeferScrollToVerticalOffsetCommand);
				if (routedCommand.CanExecute(newValue, commandTarget))
				{
					routedCommand.Execute(newValue, commandTarget);
				}
				else
				{
					routedCommand = null;
				}
			}
			if (routedCommand == null)
			{
				routedCommand = (flag ? ScrollBar.ScrollToHorizontalOffsetCommand : ScrollBar.ScrollToVerticalOffsetCommand);
				if (routedCommand.CanExecute(newValue, commandTarget))
				{
					routedCommand.Execute(newValue, commandTarget);
				}
			}
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x001AC6EC File Offset: 0x001AA8EC
		internal void ScrollToLastMousePoint()
		{
			Point point = new Point(-1.0, -1.0);
			if (this.Track != null && this._latestRightButtonClickPoint != point)
			{
				double num = this.Track.ValueFromPoint(this._latestRightButtonClickPoint);
				if (Shape.IsDoubleFinite(num))
				{
					this.ChangeValue(num, false);
					this._latestRightButtonClickPoint = point;
					this.RaiseScrollEvent(ScrollEventType.ThumbPosition);
				}
			}
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x001AC760 File Offset: 0x001AA960
		internal void RaiseScrollEvent(ScrollEventType scrollEventType)
		{
			base.RaiseEvent(new ScrollEventArgs(scrollEventType, base.Value)
			{
				Source = this
			});
		}

		// Token: 0x06005F94 RID: 24468 RVA: 0x001AC788 File Offset: 0x001AA988
		private static void OnScrollCommand(object target, ExecutedRoutedEventArgs args)
		{
			ScrollBar scrollBar = (ScrollBar)target;
			if (args.Command == ScrollBar.ScrollHereCommand)
			{
				scrollBar.ScrollToLastMousePoint();
			}
			if (scrollBar.IsStandalone)
			{
				if (scrollBar.Orientation == Orientation.Vertical)
				{
					if (args.Command == ScrollBar.LineUpCommand)
					{
						scrollBar.LineUp();
						return;
					}
					if (args.Command == ScrollBar.LineDownCommand)
					{
						scrollBar.LineDown();
						return;
					}
					if (args.Command == ScrollBar.PageUpCommand)
					{
						scrollBar.PageUp();
						return;
					}
					if (args.Command == ScrollBar.PageDownCommand)
					{
						scrollBar.PageDown();
						return;
					}
					if (args.Command == ScrollBar.ScrollToTopCommand)
					{
						scrollBar.ScrollToTop();
						return;
					}
					if (args.Command == ScrollBar.ScrollToBottomCommand)
					{
						scrollBar.ScrollToBottom();
						return;
					}
				}
				else
				{
					if (args.Command == ScrollBar.LineLeftCommand)
					{
						scrollBar.LineLeft();
						return;
					}
					if (args.Command == ScrollBar.LineRightCommand)
					{
						scrollBar.LineRight();
						return;
					}
					if (args.Command == ScrollBar.PageLeftCommand)
					{
						scrollBar.PageLeft();
						return;
					}
					if (args.Command == ScrollBar.PageRightCommand)
					{
						scrollBar.PageRight();
						return;
					}
					if (args.Command == ScrollBar.ScrollToLeftEndCommand)
					{
						scrollBar.ScrollToLeftEnd();
						return;
					}
					if (args.Command == ScrollBar.ScrollToRightEndCommand)
					{
						scrollBar.ScrollToRightEnd();
					}
				}
			}
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x001AC8B4 File Offset: 0x001AAAB4
		private void SmallDecrement()
		{
			double num = Math.Max(base.Value - base.SmallChange, base.Minimum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.SmallDecrement);
			}
		}

		// Token: 0x06005F96 RID: 24470 RVA: 0x001AC8F4 File Offset: 0x001AAAF4
		private void SmallIncrement()
		{
			double num = Math.Min(base.Value + base.SmallChange, base.Maximum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.SmallIncrement);
			}
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x001AC934 File Offset: 0x001AAB34
		private void LargeDecrement()
		{
			double num = Math.Max(base.Value - base.LargeChange, base.Minimum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.LargeDecrement);
			}
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x001AC974 File Offset: 0x001AAB74
		private void LargeIncrement()
		{
			double num = Math.Min(base.Value + base.LargeChange, base.Maximum);
			if (base.Value != num)
			{
				base.Value = num;
				this.RaiseScrollEvent(ScrollEventType.LargeIncrement);
			}
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x001AC9B1 File Offset: 0x001AABB1
		private void ToMinimum()
		{
			if (base.Value != base.Minimum)
			{
				base.Value = base.Minimum;
				this.RaiseScrollEvent(ScrollEventType.First);
			}
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x001AC9D4 File Offset: 0x001AABD4
		private void ToMaximum()
		{
			if (base.Value != base.Maximum)
			{
				base.Value = base.Maximum;
				this.RaiseScrollEvent(ScrollEventType.Last);
			}
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x001AC9F7 File Offset: 0x001AABF7
		private void LineUp()
		{
			this.SmallDecrement();
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x001AC9FF File Offset: 0x001AABFF
		private void LineDown()
		{
			this.SmallIncrement();
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x001ACA07 File Offset: 0x001AAC07
		private void PageUp()
		{
			this.LargeDecrement();
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x001ACA0F File Offset: 0x001AAC0F
		private void PageDown()
		{
			this.LargeIncrement();
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x001ACA17 File Offset: 0x001AAC17
		private void ScrollToTop()
		{
			this.ToMinimum();
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x001ACA1F File Offset: 0x001AAC1F
		private void ScrollToBottom()
		{
			this.ToMaximum();
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x001AC9F7 File Offset: 0x001AABF7
		private void LineLeft()
		{
			this.SmallDecrement();
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x001AC9FF File Offset: 0x001AABFF
		private void LineRight()
		{
			this.SmallIncrement();
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x001ACA07 File Offset: 0x001AAC07
		private void PageLeft()
		{
			this.LargeDecrement();
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x001ACA0F File Offset: 0x001AAC0F
		private void PageRight()
		{
			this.LargeIncrement();
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x001ACA17 File Offset: 0x001AAC17
		private void ScrollToLeftEnd()
		{
			this.ToMinimum();
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x001ACA1F File Offset: 0x001AAC1F
		private void ScrollToRightEnd()
		{
			this.ToMaximum();
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x001ACA27 File Offset: 0x001AAC27
		private static void OnQueryScrollHereCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (args.Command == ScrollBar.ScrollHereCommand);
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x001ACA3C File Offset: 0x001AAC3C
		private static void OnQueryScrollCommand(object target, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = ((ScrollBar)target).IsStandalone;
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x001ACA50 File Offset: 0x001AAC50
		static ScrollBar()
		{
			ScrollBar.ScrollEvent = EventManager.RegisterRoutedEvent("Scroll", RoutingStrategy.Bubble, typeof(ScrollEventHandler), typeof(ScrollBar));
			ScrollBar.OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollBar), new FrameworkPropertyMetadata(Orientation.Vertical), new ValidateValueCallback(ScrollBar.IsValidOrientation));
			ScrollBar.ViewportSizeProperty = DependencyProperty.Register("ViewportSize", typeof(double), typeof(ScrollBar), new FrameworkPropertyMetadata(0.0), new ValidateValueCallback(Shape.IsDoubleFiniteNonNegative));
			ScrollBar.LineUpCommand = new RoutedCommand("LineUp", typeof(ScrollBar));
			ScrollBar.LineDownCommand = new RoutedCommand("LineDown", typeof(ScrollBar));
			ScrollBar.LineLeftCommand = new RoutedCommand("LineLeft", typeof(ScrollBar));
			ScrollBar.LineRightCommand = new RoutedCommand("LineRight", typeof(ScrollBar));
			ScrollBar.PageUpCommand = new RoutedCommand("PageUp", typeof(ScrollBar));
			ScrollBar.PageDownCommand = new RoutedCommand("PageDown", typeof(ScrollBar));
			ScrollBar.PageLeftCommand = new RoutedCommand("PageLeft", typeof(ScrollBar));
			ScrollBar.PageRightCommand = new RoutedCommand("PageRight", typeof(ScrollBar));
			ScrollBar.ScrollToEndCommand = new RoutedCommand("ScrollToEnd", typeof(ScrollBar));
			ScrollBar.ScrollToHomeCommand = new RoutedCommand("ScrollToHome", typeof(ScrollBar));
			ScrollBar.ScrollToRightEndCommand = new RoutedCommand("ScrollToRightEnd", typeof(ScrollBar));
			ScrollBar.ScrollToLeftEndCommand = new RoutedCommand("ScrollToLeftEnd", typeof(ScrollBar));
			ScrollBar.ScrollToTopCommand = new RoutedCommand("ScrollToTop", typeof(ScrollBar));
			ScrollBar.ScrollToBottomCommand = new RoutedCommand("ScrollToBottom", typeof(ScrollBar));
			ScrollBar.ScrollToHorizontalOffsetCommand = new RoutedCommand("ScrollToHorizontalOffset", typeof(ScrollBar));
			ScrollBar.ScrollToVerticalOffsetCommand = new RoutedCommand("ScrollToVerticalOffset", typeof(ScrollBar));
			ScrollBar.DeferScrollToHorizontalOffsetCommand = new RoutedCommand("DeferScrollToToHorizontalOffset", typeof(ScrollBar));
			ScrollBar.DeferScrollToVerticalOffsetCommand = new RoutedCommand("DeferScrollToVerticalOffset", typeof(ScrollBar));
			ScrollBar.ScrollHereCommand = new RoutedCommand("ScrollHere", typeof(ScrollBar));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(typeof(ScrollBar)));
			ScrollBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ScrollBar));
			ExecutedRoutedEventHandler executedRoutedEventHandler = new ExecutedRoutedEventHandler(ScrollBar.OnScrollCommand);
			CanExecuteRoutedEventHandler canExecuteRoutedEventHandler = new CanExecuteRoutedEventHandler(ScrollBar.OnQueryScrollCommand);
			UIElement.FocusableProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragStartedEvent, new DragStartedEventHandler(ScrollBar.OnThumbDragStarted));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragDeltaEvent, new DragDeltaEventHandler(ScrollBar.OnThumbDragDelta));
			EventManager.RegisterClassHandler(typeof(ScrollBar), Thumb.DragCompletedEvent, new DragCompletedEventHandler(ScrollBar.OnThumbDragCompleted));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollHereCommand, executedRoutedEventHandler, new CanExecuteRoutedEventHandler(ScrollBar.OnQueryScrollHereCommand));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Up);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Down);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageUpCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Prior);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageDownCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Next);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToTopCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.Home, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToBottomCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(Key.End, ModifierKeys.Control));
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Left);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.LineRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Right);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageLeftCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.PageRightCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToLeftEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.Home);
			CommandHelpers.RegisterCommandHandler(typeof(ScrollBar), ScrollBar.ScrollToRightEndCommand, executedRoutedEventHandler, canExecuteRoutedEventHandler, Key.End);
			RangeBase.MaximumProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollBar.ViewChanged)));
			RangeBase.MinimumProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollBar.ViewChanged)));
			FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof(ScrollBar), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ScrollBar.CoerceContextMenu)));
			ControlsTraceLogger.AddControl(TelemetryControls.ScrollBar);
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x001ACF6C File Offset: 0x001AB16C
		private static void ViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollBar scrollBar = (ScrollBar)d;
			bool flag = scrollBar.Maximum > scrollBar.Minimum;
			if (flag != scrollBar._canScroll)
			{
				scrollBar._canScroll = flag;
				scrollBar.CoerceValue(UIElement.IsEnabledProperty);
			}
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x001ACFAC File Offset: 0x001AB1AC
		internal static bool IsValidOrientation(object o)
		{
			Orientation orientation = (Orientation)o;
			return orientation == Orientation.Horizontal || orientation == Orientation.Vertical;
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x06005FAC RID: 24492 RVA: 0x001ACFC9 File Offset: 0x001AB1C9
		// (set) Token: 0x06005FAD RID: 24493 RVA: 0x001ACFD1 File Offset: 0x001AB1D1
		internal bool IsStandalone
		{
			get
			{
				return this._isStandalone;
			}
			set
			{
				this._isStandalone = value;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x06005FAE RID: 24494 RVA: 0x001ACFDA File Offset: 0x001AB1DA
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ScrollBar._dType;
			}
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x001ACFE4 File Offset: 0x001AB1E4
		private static object CoerceContextMenu(DependencyObject o, object value)
		{
			ScrollBar scrollBar = (ScrollBar)o;
			bool flag;
			if (!scrollBar._openingContextMenu || scrollBar.GetValueSource(FrameworkElement.ContextMenuProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
			{
				return value;
			}
			if (scrollBar.Orientation == Orientation.Vertical)
			{
				return ScrollBar.VerticalContextMenu;
			}
			if (scrollBar.FlowDirection == FlowDirection.LeftToRight)
			{
				return ScrollBar.HorizontalContextMenuLTR;
			}
			return ScrollBar.HorizontalContextMenuRTL;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.FrameworkElement.ContextMenuOpening" /> event that occurs when the <see cref="T:System.Windows.Controls.ContextMenu" /> for a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> opens.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005FB0 RID: 24496 RVA: 0x001AD038 File Offset: 0x001AB238
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			base.OnContextMenuOpening(e);
			if (!e.Handled)
			{
				this._openingContextMenu = true;
				base.CoerceValue(FrameworkElement.ContextMenuProperty);
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.FrameworkElement.ContextMenuClosing" /> event that occurs when the <see cref="T:System.Windows.Controls.ContextMenu" /> for a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> closes.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005FB1 RID: 24497 RVA: 0x001AD05B File Offset: 0x001AB25B
		protected override void OnContextMenuClosing(ContextMenuEventArgs e)
		{
			base.OnContextMenuClosing(e);
			this._openingContextMenu = false;
			base.CoerceValue(FrameworkElement.ContextMenuProperty);
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x06005FB2 RID: 24498 RVA: 0x001AD078 File Offset: 0x001AB278
		private static ContextMenu VerticalContextMenu
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_Top", "Top", ScrollBar.ScrollToTopCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_Bottom", "Bottom", ScrollBar.ScrollToBottomCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageUp", "PageUp", ScrollBar.PageUpCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageDown", "PageDown", ScrollBar.PageDownCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollUp", "ScrollUp", ScrollBar.LineUpCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollDown", "ScrollDown", ScrollBar.LineDownCommand)
					}
				};
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x001AD1A0 File Offset: 0x001AB3A0
		private static ContextMenu HorizontalContextMenuLTR
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_LeftEdge", "LeftEdge", ScrollBar.ScrollToLeftEndCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_RightEdge", "RightEdge", ScrollBar.ScrollToRightEndCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageLeft", "PageLeft", ScrollBar.PageLeftCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageRight", "PageRight", ScrollBar.PageRightCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollLeft", "ScrollLeft", ScrollBar.LineLeftCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollRight", "ScrollRight", ScrollBar.LineRightCommand)
					}
				};
			}
		}

		// Token: 0x1700170B RID: 5899
		// (get) Token: 0x06005FB4 RID: 24500 RVA: 0x001AD2C8 File Offset: 0x001AB4C8
		private static ContextMenu HorizontalContextMenuRTL
		{
			get
			{
				return new ContextMenu
				{
					Items = 
					{
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollHere", "ScrollHere", ScrollBar.ScrollHereCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_LeftEdge", "LeftEdge", ScrollBar.ScrollToRightEndCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_RightEdge", "RightEdge", ScrollBar.ScrollToLeftEndCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageLeft", "PageLeft", ScrollBar.PageRightCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_PageRight", "PageRight", ScrollBar.PageLeftCommand),
						new Separator(),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollLeft", "ScrollLeft", ScrollBar.LineRightCommand),
						ScrollBar.CreateMenuItem("ScrollBar_ContextMenu_ScrollRight", "ScrollRight", ScrollBar.LineLeftCommand)
					}
				};
			}
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x001AD3F0 File Offset: 0x001AB5F0
		private static MenuItem CreateMenuItem(string name, string automationId, RoutedCommand command)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.Header = SR.Get(name);
			menuItem.Command = command;
			AutomationProperties.SetAutomationId(menuItem, automationId);
			Binding binding = new Binding();
			binding.Path = new PropertyPath(ContextMenu.PlacementTargetProperty);
			binding.Mode = BindingMode.OneWay;
			binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContextMenu), 1);
			menuItem.SetBinding(MenuItem.CommandTargetProperty, binding);
			return menuItem;
		}

		// Token: 0x1700170C RID: 5900
		// (get) Token: 0x06005FB6 RID: 24502 RVA: 0x000957A4 File Offset: 0x000939A4
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ScrollBar.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ScrollBar.Orientation" /> dependency property.</returns>
		// Token: 0x040030A8 RID: 12456
		public static readonly DependencyProperty OrientationProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ScrollBar.ViewportSize" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ScrollBar.ViewportSize" /> dependency property.</returns>
		// Token: 0x040030A9 RID: 12457
		public static readonly DependencyProperty ViewportSizeProperty;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the vertical direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the vertical direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AA RID: 12458
		public static readonly RoutedCommand LineUpCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the vertical direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the vertical direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AB RID: 12459
		public static readonly RoutedCommand LineDownCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the horizontal direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the horizontal direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AC RID: 12460
		public static readonly RoutedCommand LineLeftCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the horizontal direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a small amount in the horizontal direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AD RID: 12461
		public static readonly RoutedCommand LineRightCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the vertical direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the vertical direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AE RID: 12462
		public static readonly RoutedCommand PageUpCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the vertical direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the vertical direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030AF RID: 12463
		public static readonly RoutedCommand PageDownCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the horizontal direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the horizontal direction of decreasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030B0 RID: 12464
		public static readonly RoutedCommand PageLeftCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the horizontal direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> by a large amount in the horizontal direction of increasing value of its <see cref="T:System.Windows.Controls.Primitives.Track" />. </returns>
		// Token: 0x040030B1 RID: 12465
		public static readonly RoutedCommand PageRightCommand;

		/// <summary>The command that scrolls the content to the lower-right corner of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control. </summary>
		/// <returns>The command that scrolls the content to the lower-right corner of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control. </returns>
		// Token: 0x040030B2 RID: 12466
		public static readonly RoutedCommand ScrollToEndCommand;

		/// <summary>The command that scrolls the content to the upper-left corner of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control. </summary>
		/// <returns>The command that scrolls the content to the upper-left corner of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control. </returns>
		// Token: 0x040030B3 RID: 12467
		public static readonly RoutedCommand ScrollToHomeCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value for a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value for a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </returns>
		// Token: 0x040030B4 RID: 12468
		public static readonly RoutedCommand ScrollToRightEndCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> value for a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> value for a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </returns>
		// Token: 0x040030B5 RID: 12469
		public static readonly RoutedCommand ScrollToLeftEndCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value for a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value for a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </returns>
		// Token: 0x040030B6 RID: 12470
		public static readonly RoutedCommand ScrollToTopCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value. </returns>
		// Token: 0x040030B7 RID: 12471
		public static readonly RoutedCommand ScrollToBottomCommand;

		/// <summary>The command that scrolls a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> in a <see cref="T:System.Windows.Controls.ScrollViewer" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />. </summary>
		/// <returns>The command that scrolls a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> in a <see cref="T:System.Windows.Controls.ScrollViewer" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />. </returns>
		// Token: 0x040030B8 RID: 12472
		public static readonly RoutedCommand ScrollToHorizontalOffsetCommand;

		/// <summary>The command that scrolls a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> in a <see cref="T:System.Windows.Controls.ScrollViewer" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />. </summary>
		/// <returns>The command that scrolls a vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> in a <see cref="T:System.Windows.Controls.ScrollViewer" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />. </returns>
		// Token: 0x040030B9 RID: 12473
		public static readonly RoutedCommand ScrollToVerticalOffsetCommand;

		/// <summary>The command that notifies the <see cref="T:System.Windows.Controls.ScrollViewer" /> that the user is dragging the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.  </summary>
		/// <returns>The command that occurs when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of a horizontal <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> on a <see cref="T:System.Windows.Controls.ScrollViewer" /> that has deferred scrolling enabled. </returns>
		// Token: 0x040030BA RID: 12474
		public static readonly RoutedCommand DeferScrollToHorizontalOffsetCommand;

		/// <summary>The command that notifies the <see cref="T:System.Windows.Controls.ScrollViewer" /> that the user is dragging the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.  </summary>
		/// <returns>The command that notifies the <see cref="T:System.Windows.Controls.ScrollViewer" /> that the user is dragging the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the vertical <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the value that is provided in <see cref="P:System.Windows.Input.ExecutedRoutedEventArgs.Parameter" />.  </returns>
		// Token: 0x040030BB RID: 12475
		public static readonly RoutedCommand DeferScrollToVerticalOffsetCommand;

		/// <summary>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the point of the mouse click that opened the <see cref="T:System.Windows.Controls.ContextMenu" /> in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </summary>
		/// <returns>The command that scrolls a <see cref="T:System.Windows.Controls.Primitives.ScrollBar" /> to the point of the mouse click that opened the <see cref="T:System.Windows.Controls.ContextMenu" /> in the <see cref="T:System.Windows.Controls.Primitives.ScrollBar" />. </returns>
		// Token: 0x040030BC RID: 12476
		public static readonly RoutedCommand ScrollHereCommand;

		// Token: 0x040030BD RID: 12477
		private const double MaxPerpendicularDelta = 150.0;

		// Token: 0x040030BE RID: 12478
		private const string TrackName = "PART_Track";

		// Token: 0x040030BF RID: 12479
		private Track _track;

		// Token: 0x040030C0 RID: 12480
		private Point _latestRightButtonClickPoint = new Point(-1.0, -1.0);

		// Token: 0x040030C1 RID: 12481
		private bool _canScroll = true;

		// Token: 0x040030C2 RID: 12482
		private bool _hasScrolled;

		// Token: 0x040030C3 RID: 12483
		private bool _isStandalone = true;

		// Token: 0x040030C4 RID: 12484
		private bool _openingContextMenu;

		// Token: 0x040030C5 RID: 12485
		private double _previousValue;

		// Token: 0x040030C6 RID: 12486
		private Vector _thumbOffset;

		// Token: 0x040030C7 RID: 12487
		private static DependencyObjectType _dType;
	}
}
