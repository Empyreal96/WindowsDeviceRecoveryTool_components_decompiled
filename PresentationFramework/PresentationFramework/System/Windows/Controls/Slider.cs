using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal;
using MS.Internal.Commands;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that lets the user select from a range of values by moving a <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> control along a <see cref="T:System.Windows.Controls.Primitives.Track" />.</summary>
	// Token: 0x02000532 RID: 1330
	[Localizability(LocalizationCategory.Ignore)]
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	[TemplatePart(Name = "PART_Track", Type = typeof(Track))]
	[TemplatePart(Name = "PART_SelectionRange", Type = typeof(FrameworkElement))]
	public class Slider : RangeBase
	{
		// Token: 0x060055FE RID: 22014 RVA: 0x0017D2A0 File Offset: 0x0017B4A0
		static Slider()
		{
			Slider.InitializeCommands();
			RangeBase.MinimumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			RangeBase.MaximumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			RangeBase.ValueProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragStartedEvent, new DragStartedEventHandler(Slider.OnThumbDragStarted));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(Slider.OnThumbDragDelta));
			EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragCompletedEvent, new DragCompletedEventHandler(Slider.OnThumbDragCompleted));
			EventManager.RegisterClassHandler(typeof(Slider), Mouse.MouseDownEvent, new MouseButtonEventHandler(Slider._OnMouseLeftButtonDown), true);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(typeof(Slider)));
			Slider._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Slider));
			ControlsTraceLogger.AddControl(TelemetryControls.Slider);
		}

		/// <summary>Gets a command that increases the value of the slider by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> property.</summary>
		/// <returns>The <see cref="T:System.Windows.Input.RoutedCommand" /> that increases the value of the <see cref="F:System.Windows.Controls.Slider.SelectionStartProperty" /> by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> property. The default <see cref="T:System.Windows.Input.InputGesture" /> for this command is <see cref="F:System.Windows.Input.Key.PageUp" />. </returns>
		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x060055FF RID: 22015 RVA: 0x0017D733 File Offset: 0x0017B933
		public static RoutedCommand IncreaseLarge
		{
			get
			{
				return Slider._increaseLargeCommand;
			}
		}

		/// <summary>Gets a command that decreases the value of the <see cref="T:System.Windows.Controls.Slider" /> by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> property.</summary>
		/// <returns>The <see cref="T:System.Windows.Input.RoutedCommand" /> that decreases the value of the <see cref="T:System.Windows.Controls.Slider" /> by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> property. The default <see cref="T:System.Windows.Input.InputGesture" /> is <see cref="F:System.Windows.Input.Key.PageDown" />.</returns>
		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x0017D73A File Offset: 0x0017B93A
		public static RoutedCommand DecreaseLarge
		{
			get
			{
				return Slider._decreaseLargeCommand;
			}
		}

		/// <summary>Gets a command that increases the value of the slider by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> property.</summary>
		/// <returns>Returns the <see cref="T:System.Windows.Input.RoutedCommand" /> that increases the value of the slider by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> property. The default <see cref="T:System.Windows.Input.InputGesture" /> objects for this command are <see cref="F:System.Windows.Input.Key.Up" /> and <see cref="F:System.Windows.Input.Key.Right" />. </returns>
		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x0017D741 File Offset: 0x0017B941
		public static RoutedCommand IncreaseSmall
		{
			get
			{
				return Slider._increaseSmallCommand;
			}
		}

		/// <summary>Gets a command that decreases the value of the <see cref="T:System.Windows.Controls.Slider" /> by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> property.</summary>
		/// <returns>The <see cref="T:System.Windows.Input.RoutedCommand" /> that decreases the value of the <see cref="T:System.Windows.Controls.Slider" /> by the same amount as the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> property. The default <see cref="T:System.Windows.Input.InputGesture" /> objects are <see cref="F:System.Windows.Input.Key.Down" /> and <see cref="F:System.Windows.Input.Key.Left" />. </returns>
		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x06005602 RID: 22018 RVA: 0x0017D748 File Offset: 0x0017B948
		public static RoutedCommand DecreaseSmall
		{
			get
			{
				return Slider._decreaseSmallCommand;
			}
		}

		/// <summary>Gets a command that sets the <see cref="T:System.Windows.Controls.Slider" /> <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> value.</summary>
		/// <returns>The <see cref="T:System.Windows.Input.RoutedCommand" /> to use. The default is <see cref="F:System.Windows.Input.Key.Home" />.</returns>
		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x06005603 RID: 22019 RVA: 0x0017D74F File Offset: 0x0017B94F
		public static RoutedCommand MinimizeValue
		{
			get
			{
				return Slider._minimizeValueCommand;
			}
		}

		/// <summary>Gets a command that sets the <see cref="T:System.Windows.Controls.Slider" /> <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> value.</summary>
		/// <returns>The <see cref="T:System.Windows.Input.RoutedCommand" /> to use. The default is <see cref="F:System.Windows.Input.Key.End" />.</returns>
		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x06005604 RID: 22020 RVA: 0x0017D756 File Offset: 0x0017B956
		public static RoutedCommand MaximizeValue
		{
			get
			{
				return Slider._maximizeValueCommand;
			}
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x0017D760 File Offset: 0x0017B960
		private static void InitializeCommands()
		{
			Slider._increaseLargeCommand = new RoutedCommand("IncreaseLarge", typeof(Slider));
			Slider._decreaseLargeCommand = new RoutedCommand("DecreaseLarge", typeof(Slider));
			Slider._increaseSmallCommand = new RoutedCommand("IncreaseSmall", typeof(Slider));
			Slider._decreaseSmallCommand = new RoutedCommand("DecreaseSmall", typeof(Slider));
			Slider._minimizeValueCommand = new RoutedCommand("MinimizeValue", typeof(Slider));
			Slider._maximizeValueCommand = new RoutedCommand("MaximizeValue", typeof(Slider));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._increaseLargeCommand, new ExecutedRoutedEventHandler(Slider.OnIncreaseLargeCommand), new Slider.SliderGesture(Key.Prior, Key.Next, false));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._decreaseLargeCommand, new ExecutedRoutedEventHandler(Slider.OnDecreaseLargeCommand), new Slider.SliderGesture(Key.Next, Key.Prior, false));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._increaseSmallCommand, new ExecutedRoutedEventHandler(Slider.OnIncreaseSmallCommand), new Slider.SliderGesture(Key.Up, Key.Down, false), new Slider.SliderGesture(Key.Right, Key.Left, true));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._decreaseSmallCommand, new ExecutedRoutedEventHandler(Slider.OnDecreaseSmallCommand), new Slider.SliderGesture(Key.Down, Key.Up, false), new Slider.SliderGesture(Key.Left, Key.Right, true));
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._minimizeValueCommand, new ExecutedRoutedEventHandler(Slider.OnMinimizeValueCommand), Key.Home);
			CommandHelpers.RegisterCommandHandler(typeof(Slider), Slider._maximizeValueCommand, new ExecutedRoutedEventHandler(Slider.OnMaximizeValueCommand), Key.End);
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x0017D904 File Offset: 0x0017BB04
		private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnIncreaseSmall();
			}
		}

		// Token: 0x06005607 RID: 22023 RVA: 0x0017D924 File Offset: 0x0017BB24
		private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnDecreaseSmall();
			}
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x0017D944 File Offset: 0x0017BB44
		private static void OnMaximizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnMaximizeValue();
			}
		}

		// Token: 0x06005609 RID: 22025 RVA: 0x0017D964 File Offset: 0x0017BB64
		private static void OnMinimizeValueCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnMinimizeValue();
			}
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x0017D984 File Offset: 0x0017BB84
		private static void OnIncreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnIncreaseLarge();
			}
		}

		// Token: 0x0600560B RID: 22027 RVA: 0x0017D9A4 File Offset: 0x0017BBA4
		private static void OnDecreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
		{
			Slider slider = sender as Slider;
			if (slider != null)
			{
				slider.OnDecreaseLarge();
			}
		}

		/// <summary>Gets or sets the orientation of a <see cref="T:System.Windows.Controls.Slider" />.  </summary>
		/// <returns>One of the <see cref="P:System.Windows.Controls.Slider.Orientation" /> values. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x0600560C RID: 22028 RVA: 0x0017D9C1 File Offset: 0x0017BBC1
		// (set) Token: 0x0600560D RID: 22029 RVA: 0x0017D9D3 File Offset: 0x0017BBD3
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(Slider.OrientationProperty);
			}
			set
			{
				base.SetValue(Slider.OrientationProperty, value);
			}
		}

		/// <summary>Gets or sets the direction of increasing value. </summary>
		/// <returns>
		///     <see langword="true" /> if the direction of increasing value is to the left for a horizontal slider or down for a vertical slider; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x0600560E RID: 22030 RVA: 0x0017D9E6 File Offset: 0x0017BBE6
		// (set) Token: 0x0600560F RID: 22031 RVA: 0x0017D9F8 File Offset: 0x0017BBF8
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsDirectionReversed
		{
			get
			{
				return (bool)base.GetValue(Slider.IsDirectionReversedProperty);
			}
			set
			{
				base.SetValue(Slider.IsDirectionReversedProperty, value);
			}
		}

		/// <summary>Gets or sets the amount of time in milliseconds that a <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> waits, while it is pressed, before a command to move the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> executes, such as a <see cref="P:System.Windows.Controls.Slider.DecreaseLarge" /> command. </summary>
		/// <returns>A time delay in milliseconds. The default is the system key press delay. For more information, see <see cref="P:System.Windows.SystemParameters.KeyboardDelay" />.</returns>
		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06005610 RID: 22032 RVA: 0x0017DA06 File Offset: 0x0017BC06
		// (set) Token: 0x06005611 RID: 22033 RVA: 0x0017DA18 File Offset: 0x0017BC18
		[Bindable(true)]
		[Category("Behavior")]
		public int Delay
		{
			get
			{
				return (int)base.GetValue(Slider.DelayProperty);
			}
			set
			{
				base.SetValue(Slider.DelayProperty, value);
			}
		}

		/// <summary>Gets or sets the amount of time in milliseconds between increase or decrease commands when a user clicks the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> of a <see cref="T:System.Windows.Controls.Slider" />. </summary>
		/// <returns>A time in milliseconds between commands that change the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of a <see cref="T:System.Windows.Controls.Slider" />. The default is the system key repeat rate. For more information, see SystemParametersInfo (SPI_GETKEYBOARDSPEED).</returns>
		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06005612 RID: 22034 RVA: 0x0017DA2B File Offset: 0x0017BC2B
		// (set) Token: 0x06005613 RID: 22035 RVA: 0x0017DA3D File Offset: 0x0017BC3D
		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)base.GetValue(Slider.IntervalProperty);
			}
			set
			{
				base.SetValue(Slider.IntervalProperty, value);
			}
		}

		/// <summary>Gets or sets whether a tooltip that contains the current value of the <see cref="T:System.Windows.Controls.Slider" /> displays when the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> is pressed. If a tooltip is displayed, this property also specifies the placement of the tooltip. </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.Primitives.AutoToolTipPlacement" /> values that determines where to display the tooltip with respect to the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> of the <see cref="T:System.Windows.Controls.Slider" />, or that specifies to not show a tooltip. The default is <see cref="F:System.Windows.Controls.Primitives.AutoToolTipPlacement.None" />, which specifies that a tooltip is not displayed.</returns>
		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06005614 RID: 22036 RVA: 0x0017DA50 File Offset: 0x0017BC50
		// (set) Token: 0x06005615 RID: 22037 RVA: 0x0017DA62 File Offset: 0x0017BC62
		[Bindable(true)]
		[Category("Behavior")]
		public AutoToolTipPlacement AutoToolTipPlacement
		{
			get
			{
				return (AutoToolTipPlacement)base.GetValue(Slider.AutoToolTipPlacementProperty);
			}
			set
			{
				base.SetValue(Slider.AutoToolTipPlacementProperty, value);
			}
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x0017DA78 File Offset: 0x0017BC78
		private static bool IsValidAutoToolTipPlacement(object o)
		{
			AutoToolTipPlacement autoToolTipPlacement = (AutoToolTipPlacement)o;
			return autoToolTipPlacement == AutoToolTipPlacement.None || autoToolTipPlacement == AutoToolTipPlacement.TopLeft || autoToolTipPlacement == AutoToolTipPlacement.BottomRight;
		}

		/// <summary>Gets or sets the number of digits that are displayed to the right side of the decimal point for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Slider" /> in a tooltip. </summary>
		/// <returns>The precision of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> that displays in the tooltip, specified as the number of digits that appear to the right of the decimal point. The default is zero (0).</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <see cref="P:System.Windows.Controls.Slider.AutoToolTipPrecision" /> is set to a value other than a non-negative integer.</exception>
		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x06005617 RID: 22039 RVA: 0x0017DA99 File Offset: 0x0017BC99
		// (set) Token: 0x06005618 RID: 22040 RVA: 0x0017DAAB File Offset: 0x0017BCAB
		[Bindable(true)]
		[Category("Appearance")]
		public int AutoToolTipPrecision
		{
			get
			{
				return (int)base.GetValue(Slider.AutoToolTipPrecisionProperty);
			}
			set
			{
				base.SetValue(Slider.AutoToolTipPrecisionProperty, value);
			}
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x0015A58B File Offset: 0x0015878B
		private static bool IsValidAutoToolTipPrecision(object o)
		{
			return (int)o >= 0;
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Slider" /> automatically moves the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> to the closest tick mark.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Slider" /> requires the position of the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> to be a tick mark; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x0600561A RID: 22042 RVA: 0x0017DABE File Offset: 0x0017BCBE
		// (set) Token: 0x0600561B RID: 22043 RVA: 0x0017DAD0 File Offset: 0x0017BCD0
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsSnapToTickEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsSnapToTickEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsSnapToTickEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets the position of tick marks with respect to the <see cref="T:System.Windows.Controls.Primitives.Track" /> of the <see cref="T:System.Windows.Controls.Slider" />.  </summary>
		/// <returns>A <see cref="P:System.Windows.Controls.Slider.TickPlacement" /> value that defines how to position the tick marks in a <see cref="T:System.Windows.Controls.Slider" /> with respect to the slider bar. The default is <see cref="F:System.Windows.Controls.Primitives.TickPlacement.None" />.</returns>
		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x0600561C RID: 22044 RVA: 0x0017DADE File Offset: 0x0017BCDE
		// (set) Token: 0x0600561D RID: 22045 RVA: 0x0017DAF0 File Offset: 0x0017BCF0
		[Bindable(true)]
		[Category("Appearance")]
		public TickPlacement TickPlacement
		{
			get
			{
				return (TickPlacement)base.GetValue(Slider.TickPlacementProperty);
			}
			set
			{
				base.SetValue(Slider.TickPlacementProperty, value);
			}
		}

		// Token: 0x0600561E RID: 22046 RVA: 0x0017DB04 File Offset: 0x0017BD04
		private static bool IsValidTickPlacement(object o)
		{
			TickPlacement tickPlacement = (TickPlacement)o;
			return tickPlacement == TickPlacement.None || tickPlacement == TickPlacement.TopLeft || tickPlacement == TickPlacement.BottomRight || tickPlacement == TickPlacement.Both;
		}

		/// <summary>Gets or sets the interval between tick marks.  </summary>
		/// <returns>The distance between tick marks. The default is (1.0).</returns>
		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x0017DB29 File Offset: 0x0017BD29
		// (set) Token: 0x06005620 RID: 22048 RVA: 0x0017DB3B File Offset: 0x0017BD3B
		[Bindable(true)]
		[Category("Appearance")]
		public double TickFrequency
		{
			get
			{
				return (double)base.GetValue(Slider.TickFrequencyProperty);
			}
			set
			{
				base.SetValue(Slider.TickFrequencyProperty, value);
			}
		}

		/// <summary>Gets or sets the positions of the tick marks to display for a <see cref="T:System.Windows.Controls.Slider" />. </summary>
		/// <returns>A set of tick marks to display for a <see cref="T:System.Windows.Controls.Slider" />. The default is <see langword="null" />.</returns>
		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06005621 RID: 22049 RVA: 0x0017DB4E File Offset: 0x0017BD4E
		// (set) Token: 0x06005622 RID: 22050 RVA: 0x0017DB60 File Offset: 0x0017BD60
		[Bindable(true)]
		[Category("Appearance")]
		public DoubleCollection Ticks
		{
			get
			{
				return (DoubleCollection)base.GetValue(Slider.TicksProperty);
			}
			set
			{
				base.SetValue(Slider.TicksProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.Slider" /> displays a selection range along the <see cref="T:System.Windows.Controls.Slider" />.  </summary>
		/// <returns>
		///     <see langword="true" /> if a selection range is displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06005623 RID: 22051 RVA: 0x0017DB6E File Offset: 0x0017BD6E
		// (set) Token: 0x06005624 RID: 22052 RVA: 0x0017DB80 File Offset: 0x0017BD80
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelectionRangeEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsSelectionRangeEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsSelectionRangeEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets the smallest value of a specified selection for a <see cref="T:System.Windows.Controls.Slider" />.  </summary>
		/// <returns>The largest value of a selected range of values of a <see cref="T:System.Windows.Controls.Slider" />. The default is zero (0.0).</returns>
		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06005625 RID: 22053 RVA: 0x0017DB8E File Offset: 0x0017BD8E
		// (set) Token: 0x06005626 RID: 22054 RVA: 0x0017DBA0 File Offset: 0x0017BDA0
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionStart
		{
			get
			{
				return (double)base.GetValue(Slider.SelectionStartProperty);
			}
			set
			{
				base.SetValue(Slider.SelectionStartProperty, value);
			}
		}

		// Token: 0x06005627 RID: 22055 RVA: 0x0017DBB4 File Offset: 0x0017BDB4
		private static void OnSelectionStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Slider slider = (Slider)d;
			double num = (double)e.OldValue;
			double num2 = (double)e.NewValue;
			slider.CoerceValue(Slider.SelectionEndProperty);
			slider.UpdateSelectionRangeElementPositionAndSize();
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x0017DBF4 File Offset: 0x0017BDF4
		private static object CoerceSelectionStart(DependencyObject d, object value)
		{
			Slider slider = (Slider)d;
			double num = (double)value;
			double minimum = slider.Minimum;
			double maximum = slider.Maximum;
			if (num < minimum)
			{
				return minimum;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		/// <summary>Gets or sets the largest value of a specified selection for a <see cref="T:System.Windows.Controls.Slider" />.  </summary>
		/// <returns>The largest value of a selected range of values of a <see cref="T:System.Windows.Controls.Slider" />. The default is zero (0.0).</returns>
		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06005629 RID: 22057 RVA: 0x0017DC34 File Offset: 0x0017BE34
		// (set) Token: 0x0600562A RID: 22058 RVA: 0x0017DC46 File Offset: 0x0017BE46
		[Bindable(true)]
		[Category("Appearance")]
		public double SelectionEnd
		{
			get
			{
				return (double)base.GetValue(Slider.SelectionEndProperty);
			}
			set
			{
				base.SetValue(Slider.SelectionEndProperty, value);
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x0017DC5C File Offset: 0x0017BE5C
		private static void OnSelectionEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Slider slider = (Slider)d;
			slider.UpdateSelectionRangeElementPositionAndSize();
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x0017DC78 File Offset: 0x0017BE78
		private static object CoerceSelectionEnd(DependencyObject d, object value)
		{
			Slider slider = (Slider)d;
			double num = (double)value;
			double selectionStart = slider.SelectionStart;
			double maximum = slider.Maximum;
			if (num < selectionStart)
			{
				return selectionStart;
			}
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x0017DCB8 File Offset: 0x0017BEB8
		private static object OnGetSelectionEnd(DependencyObject d)
		{
			return ((Slider)d).SelectionEnd;
		}

		/// <summary>Responds to a change in the value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</summary>
		/// <param name="oldMinimum">The old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		/// <param name="newMinimum">The new value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		// Token: 0x0600562E RID: 22062 RVA: 0x0017DCCA File Offset: 0x0017BECA
		protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
			base.CoerceValue(Slider.SelectionStartProperty);
		}

		/// <summary>Responds to a change in the value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</summary>
		/// <param name="oldMaximum">The old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		/// <param name="newMaximum">The new value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		// Token: 0x0600562F RID: 22063 RVA: 0x0017DCD7 File Offset: 0x0017BED7
		protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
			base.CoerceValue(Slider.SelectionStartProperty);
			base.CoerceValue(Slider.SelectionEndProperty);
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> of a <see cref="T:System.Windows.Controls.Slider" /> moves immediately to the location of the mouse click that occurs while the mouse pointer pauses on the <see cref="T:System.Windows.Controls.Slider" /> track.  </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.Primitives.Track.Thumb" /> moves immediately to the location of a mouse click; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06005630 RID: 22064 RVA: 0x0017DCEF File Offset: 0x0017BEEF
		// (set) Token: 0x06005631 RID: 22065 RVA: 0x0017DD01 File Offset: 0x0017BF01
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsMoveToPointEnabled
		{
			get
			{
				return (bool)base.GetValue(Slider.IsMoveToPointEnabledProperty);
			}
			set
			{
				base.SetValue(Slider.IsMoveToPointEnabledProperty, value);
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.PreviewMouseLeftButtonDown" /> routed event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005632 RID: 22066 RVA: 0x0017DD10 File Offset: 0x0017BF10
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (this.IsMoveToPointEnabled && this.Track != null && this.Track.Thumb != null && !this.Track.Thumb.IsMouseOver)
			{
				Point position = e.MouseDevice.GetPosition(this.Track);
				double num = this.Track.ValueFromPoint(position);
				if (Shape.IsDoubleFinite(num))
				{
					this.UpdateValue(num);
				}
				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonDown(e);
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x0017DD90 File Offset: 0x0017BF90
		private static void OnThumbDragStarted(object sender, DragStartedEventArgs e)
		{
			Slider slider = sender as Slider;
			slider.OnThumbDragStarted(e);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x0017DDAC File Offset: 0x0017BFAC
		private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
		{
			Slider slider = sender as Slider;
			slider.OnThumbDragDelta(e);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x0017DDC8 File Offset: 0x0017BFC8
		private static void OnThumbDragCompleted(object sender, DragCompletedEventArgs e)
		{
			Slider slider = sender as Slider;
			slider.OnThumbDragCompleted(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragStarted" /> event that occurs when the user starts to drag the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the <see cref="T:System.Windows.Controls.Slider" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005636 RID: 22070 RVA: 0x0017DDE4 File Offset: 0x0017BFE4
		protected virtual void OnThumbDragStarted(DragStartedEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb == null || this.AutoToolTipPlacement == AutoToolTipPlacement.None)
			{
				return;
			}
			this._thumbOriginalToolTip = thumb.ToolTip;
			if (this._autoToolTip == null)
			{
				this._autoToolTip = new ToolTip();
				this._autoToolTip.Placement = PlacementMode.Custom;
				this._autoToolTip.PlacementTarget = thumb;
				this._autoToolTip.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.AutoToolTipCustomPlacementCallback);
			}
			thumb.ToolTip = this._autoToolTip;
			this._autoToolTip.Content = this.GetAutoToolTipNumber();
			this._autoToolTip.IsOpen = true;
			((Popup)this._autoToolTip.Parent).Reposition();
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragDelta" /> event that occurs when the user drags the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the <see cref="T:System.Windows.Controls.Slider" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005637 RID: 22071 RVA: 0x0017DE98 File Offset: 0x0017C098
		protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (this.Track != null && thumb == this.Track.Thumb)
			{
				double num = base.Value + this.Track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
				if (Shape.IsDoubleFinite(num))
				{
					this.UpdateValue(num);
				}
				if (this.AutoToolTipPlacement != AutoToolTipPlacement.None)
				{
					if (this._autoToolTip == null)
					{
						this._autoToolTip = new ToolTip();
					}
					this._autoToolTip.Content = this.GetAutoToolTipNumber();
					if (thumb.ToolTip != this._autoToolTip)
					{
						thumb.ToolTip = this._autoToolTip;
					}
					if (!this._autoToolTip.IsOpen)
					{
						this._autoToolTip.IsOpen = true;
					}
					((Popup)this._autoToolTip.Parent).Reposition();
				}
			}
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x0017DF74 File Offset: 0x0017C174
		private string GetAutoToolTipNumber()
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
			numberFormatInfo.NumberDecimalDigits = this.AutoToolTipPrecision;
			return base.Value.ToString("N", numberFormatInfo);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.Controls.Primitives.Thumb.DragCompleted" /> event that occurs when the user stops dragging the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> of the <see cref="T:System.Windows.Controls.Slider" />.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x06005639 RID: 22073 RVA: 0x0017DFB4 File Offset: 0x0017C1B4
		protected virtual void OnThumbDragCompleted(DragCompletedEventArgs e)
		{
			Thumb thumb = e.OriginalSource as Thumb;
			if (thumb == null || this.AutoToolTipPlacement == AutoToolTipPlacement.None)
			{
				return;
			}
			if (this._autoToolTip != null)
			{
				this._autoToolTip.IsOpen = false;
			}
			thumb.ToolTip = this._thumbOriginalToolTip;
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x0017DFFC File Offset: 0x0017C1FC
		private CustomPopupPlacement[] AutoToolTipCustomPlacementCallback(Size popupSize, Size targetSize, Point offset)
		{
			AutoToolTipPlacement autoToolTipPlacement = this.AutoToolTipPlacement;
			if (autoToolTipPlacement != AutoToolTipPlacement.TopLeft)
			{
				if (autoToolTipPlacement != AutoToolTipPlacement.BottomRight)
				{
					return new CustomPopupPlacement[0];
				}
				if (this.Orientation == Orientation.Horizontal)
				{
					return new CustomPopupPlacement[]
					{
						new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, targetSize.Height), PopupPrimaryAxis.Horizontal)
					};
				}
				return new CustomPopupPlacement[]
				{
					new CustomPopupPlacement(new Point(targetSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
				};
			}
			else
			{
				if (this.Orientation == Orientation.Horizontal)
				{
					return new CustomPopupPlacement[]
					{
						new CustomPopupPlacement(new Point((targetSize.Width - popupSize.Width) * 0.5, -popupSize.Height), PopupPrimaryAxis.Horizontal)
					};
				}
				return new CustomPopupPlacement[]
				{
					new CustomPopupPlacement(new Point(-popupSize.Width, (targetSize.Height - popupSize.Height) * 0.5), PopupPrimaryAxis.Vertical)
				};
			}
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x0017E11C File Offset: 0x0017C31C
		private void UpdateSelectionRangeElementPositionAndSize()
		{
			Size renderSize = new Size(0.0, 0.0);
			Size size = new Size(0.0, 0.0);
			if (this.Track == null || DoubleUtil.LessThan(this.SelectionEnd, this.SelectionStart))
			{
				return;
			}
			renderSize = this.Track.RenderSize;
			size = ((this.Track.Thumb != null) ? this.Track.Thumb.RenderSize : new Size(0.0, 0.0));
			double num = base.Maximum - base.Minimum;
			FrameworkElement selectionRangeElement = this.SelectionRangeElement;
			if (selectionRangeElement == null)
			{
				return;
			}
			if (this.Orientation == Orientation.Horizontal)
			{
				double num2;
				if (DoubleUtil.AreClose(num, 0.0) || DoubleUtil.AreClose(renderSize.Width, size.Width))
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Max(0.0, (renderSize.Width - size.Width) / num);
				}
				selectionRangeElement.Width = (this.SelectionEnd - this.SelectionStart) * num2;
				if (this.IsDirectionReversed)
				{
					Canvas.SetLeft(selectionRangeElement, size.Width * 0.5 + Math.Max(base.Maximum - this.SelectionEnd, 0.0) * num2);
					return;
				}
				Canvas.SetLeft(selectionRangeElement, size.Width * 0.5 + Math.Max(this.SelectionStart - base.Minimum, 0.0) * num2);
				return;
			}
			else
			{
				double num2;
				if (DoubleUtil.AreClose(num, 0.0) || DoubleUtil.AreClose(renderSize.Height, size.Height))
				{
					num2 = 0.0;
				}
				else
				{
					num2 = Math.Max(0.0, (renderSize.Height - size.Height) / num);
				}
				selectionRangeElement.Height = (this.SelectionEnd - this.SelectionStart) * num2;
				if (this.IsDirectionReversed)
				{
					Canvas.SetTop(selectionRangeElement, size.Height * 0.5 + Math.Max(this.SelectionStart - base.Minimum, 0.0) * num2);
					return;
				}
				Canvas.SetTop(selectionRangeElement, size.Height * 0.5 + Math.Max(base.Maximum - this.SelectionEnd, 0.0) * num2);
				return;
			}
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x0600563C RID: 22076 RVA: 0x0017E39B File Offset: 0x0017C59B
		// (set) Token: 0x0600563D RID: 22077 RVA: 0x0017E3A3 File Offset: 0x0017C5A3
		internal Track Track
		{
			get
			{
				return this._track;
			}
			set
			{
				this._track = value;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x0600563E RID: 22078 RVA: 0x0017E3AC File Offset: 0x0017C5AC
		// (set) Token: 0x0600563F RID: 22079 RVA: 0x0017E3B4 File Offset: 0x0017C5B4
		internal FrameworkElement SelectionRangeElement
		{
			get
			{
				return this._selectionRangeElement;
			}
			set
			{
				this._selectionRangeElement = value;
			}
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x0017E3C0 File Offset: 0x0017C5C0
		private double SnapToTick(double value)
		{
			if (this.IsSnapToTickEnabled)
			{
				double num = base.Minimum;
				double num2 = base.Maximum;
				DoubleCollection doubleCollection = null;
				bool flag;
				if (base.GetValueSource(Slider.TicksProperty, null, out flag) != BaseValueSourceInternal.Default || flag)
				{
					doubleCollection = this.Ticks;
				}
				if (doubleCollection != null && doubleCollection.Count > 0)
				{
					for (int i = 0; i < doubleCollection.Count; i++)
					{
						double num3 = doubleCollection[i];
						if (DoubleUtil.AreClose(num3, value))
						{
							return value;
						}
						if (DoubleUtil.LessThan(num3, value) && DoubleUtil.GreaterThan(num3, num))
						{
							num = num3;
						}
						else if (DoubleUtil.GreaterThan(num3, value) && DoubleUtil.LessThan(num3, num2))
						{
							num2 = num3;
						}
					}
				}
				else if (DoubleUtil.GreaterThan(this.TickFrequency, 0.0))
				{
					num = base.Minimum + Math.Round((value - base.Minimum) / this.TickFrequency) * this.TickFrequency;
					num2 = Math.Min(base.Maximum, num + this.TickFrequency);
				}
				value = (DoubleUtil.GreaterThanOrClose(value, (num + num2) * 0.5) ? num2 : num);
			}
			return value;
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x0017E4DC File Offset: 0x0017C6DC
		private void MoveToNextTick(double direction)
		{
			if (direction != 0.0)
			{
				double value = base.Value;
				double num = this.SnapToTick(Math.Max(base.Minimum, Math.Min(base.Maximum, value + direction)));
				bool flag = direction > 0.0;
				if (num == value && (!flag || value != base.Maximum) && (flag || value != base.Minimum))
				{
					DoubleCollection doubleCollection = null;
					bool flag2;
					if (base.GetValueSource(Slider.TicksProperty, null, out flag2) != BaseValueSourceInternal.Default || flag2)
					{
						doubleCollection = this.Ticks;
					}
					if (doubleCollection != null && doubleCollection.Count > 0)
					{
						for (int i = 0; i < doubleCollection.Count; i++)
						{
							double num2 = doubleCollection[i];
							if ((flag && DoubleUtil.GreaterThan(num2, value) && (DoubleUtil.LessThan(num2, num) || num == value)) || (!flag && DoubleUtil.LessThan(num2, value) && (DoubleUtil.GreaterThan(num2, num) || num == value)))
							{
								num = num2;
							}
						}
					}
					else if (DoubleUtil.GreaterThan(this.TickFrequency, 0.0))
					{
						double num3 = Math.Round((value - base.Minimum) / this.TickFrequency);
						if (flag)
						{
							num3 += 1.0;
						}
						else
						{
							num3 -= 1.0;
						}
						num = base.Minimum + num3 * this.TickFrequency;
					}
				}
				if (num != value)
				{
					base.SetCurrentValueInternal(RangeBase.ValueProperty, num);
				}
			}
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Slider" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.SliderAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Slider" />.</returns>
		// Token: 0x06005642 RID: 22082 RVA: 0x0017E64C File Offset: 0x0017C84C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SliderAutomationPeer(this);
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x0017E654 File Offset: 0x0017C854
		private static void _OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton != MouseButton.Left)
			{
				return;
			}
			Slider slider = (Slider)sender;
			if (!slider.IsKeyboardFocusWithin)
			{
				e.Handled = (slider.Focus() || e.Handled);
			}
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.Slider" /> and determines its <see cref="T:System.Windows.Size" />.</summary>
		/// <param name="finalSize">The computed <see cref="T:System.Windows.Size" /> that is used to arrange the control.</param>
		/// <returns>The computed <see cref="T:System.Windows.Size" /> of the <see cref="T:System.Windows.Controls.Slider" />.</returns>
		// Token: 0x06005644 RID: 22084 RVA: 0x0017E690 File Offset: 0x0017C890
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			this.UpdateSelectionRangeElementPositionAndSize();
			return result;
		}

		/// <summary>Updates the current position of the <see cref="T:System.Windows.Controls.Slider" /> when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property changes.</summary>
		/// <param name="oldValue">The old <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Slider" />.</param>
		/// <param name="newValue">The new <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Slider" />.</param>
		// Token: 0x06005645 RID: 22085 RVA: 0x0017E6AC File Offset: 0x0017C8AC
		protected override void OnValueChanged(double oldValue, double newValue)
		{
			base.OnValueChanged(oldValue, newValue);
			this.UpdateSelectionRangeElementPositionAndSize();
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.Slider" /> control.</summary>
		// Token: 0x06005646 RID: 22086 RVA: 0x0017E6BC File Offset: 0x0017C8BC
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.SelectionRangeElement = (base.GetTemplateChild("PART_SelectionRange") as FrameworkElement);
			this.Track = (base.GetTemplateChild("PART_Track") as Track);
			if (this._autoToolTip != null)
			{
				this._autoToolTip.PlacementTarget = ((this.Track != null) ? this.Track.Thumb : null);
			}
		}

		/// <summary>Responds to an <see cref="P:System.Windows.Controls.Slider.IncreaseLarge" /> command.</summary>
		// Token: 0x06005647 RID: 22087 RVA: 0x0017E724 File Offset: 0x0017C924
		protected virtual void OnIncreaseLarge()
		{
			this.MoveToNextTick(base.LargeChange);
		}

		/// <summary>Responds to the <see cref="P:System.Windows.Controls.Slider.DecreaseLarge" /> command.</summary>
		// Token: 0x06005648 RID: 22088 RVA: 0x0017E732 File Offset: 0x0017C932
		protected virtual void OnDecreaseLarge()
		{
			this.MoveToNextTick(-base.LargeChange);
		}

		/// <summary>Responds to an <see cref="P:System.Windows.Controls.Slider.IncreaseSmall" /> command.</summary>
		// Token: 0x06005649 RID: 22089 RVA: 0x0017E741 File Offset: 0x0017C941
		protected virtual void OnIncreaseSmall()
		{
			this.MoveToNextTick(base.SmallChange);
		}

		/// <summary>Responds to a <see cref="P:System.Windows.Controls.Slider.DecreaseSmall" /> command.</summary>
		// Token: 0x0600564A RID: 22090 RVA: 0x0017E74F File Offset: 0x0017C94F
		protected virtual void OnDecreaseSmall()
		{
			this.MoveToNextTick(-base.SmallChange);
		}

		/// <summary>Responds to the <see cref="P:System.Windows.Controls.Slider.MaximizeValue" /> command.</summary>
		// Token: 0x0600564B RID: 22091 RVA: 0x0017E75E File Offset: 0x0017C95E
		protected virtual void OnMaximizeValue()
		{
			base.SetCurrentValueInternal(RangeBase.ValueProperty, base.Maximum);
		}

		/// <summary>Responds to a <see cref="P:System.Windows.Controls.Slider.MinimizeValue" /> command.</summary>
		// Token: 0x0600564C RID: 22092 RVA: 0x0017E776 File Offset: 0x0017C976
		protected virtual void OnMinimizeValue()
		{
			base.SetCurrentValueInternal(RangeBase.ValueProperty, base.Minimum);
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x0017E790 File Offset: 0x0017C990
		private void UpdateValue(double value)
		{
			double num = this.SnapToTick(value);
			if (num != base.Value)
			{
				base.SetCurrentValueInternal(RangeBase.ValueProperty, Math.Max(base.Minimum, Math.Min(base.Maximum, num)));
			}
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x0017E7D8 File Offset: 0x0017C9D8
		private static bool IsValidDoubleValue(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && !double.IsInfinity(num);
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x0600564F RID: 22095 RVA: 0x0017E7FF File Offset: 0x0017C9FF
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Slider._dType;
			}
		}

		// Token: 0x04002E23 RID: 11811
		private static RoutedCommand _increaseLargeCommand = null;

		// Token: 0x04002E24 RID: 11812
		private static RoutedCommand _increaseSmallCommand = null;

		// Token: 0x04002E25 RID: 11813
		private static RoutedCommand _decreaseLargeCommand = null;

		// Token: 0x04002E26 RID: 11814
		private static RoutedCommand _decreaseSmallCommand = null;

		// Token: 0x04002E27 RID: 11815
		private static RoutedCommand _minimizeValueCommand = null;

		// Token: 0x04002E28 RID: 11816
		private static RoutedCommand _maximizeValueCommand = null;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.Orientation" /> dependency property. </summary>
		// Token: 0x04002E29 RID: 11817
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Slider), new FrameworkPropertyMetadata(Orientation.Horizontal), new ValidateValueCallback(ScrollBar.IsValidOrientation));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.IsDirectionReversed" /> dependency property. </summary>
		// Token: 0x04002E2A RID: 11818
		public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.Delay" /> dependency property. </summary>
		// Token: 0x04002E2B RID: 11819
		public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(Slider), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardDelay()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.Interval" /> dependency property. </summary>
		// Token: 0x04002E2C RID: 11820
		public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(Slider), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardSpeed()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.AutoToolTipPlacement" /> dependency property. </summary>
		// Token: 0x04002E2D RID: 11821
		public static readonly DependencyProperty AutoToolTipPlacementProperty = DependencyProperty.Register("AutoToolTipPlacement", typeof(AutoToolTipPlacement), typeof(Slider), new FrameworkPropertyMetadata(AutoToolTipPlacement.None), new ValidateValueCallback(Slider.IsValidAutoToolTipPlacement));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.AutoToolTipPrecision" /> dependency property. </summary>
		// Token: 0x04002E2E RID: 11822
		public static readonly DependencyProperty AutoToolTipPrecisionProperty = DependencyProperty.Register("AutoToolTipPrecision", typeof(int), typeof(Slider), new FrameworkPropertyMetadata(0), new ValidateValueCallback(Slider.IsValidAutoToolTipPrecision));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.IsSnapToTickEnabled" /> dependency property. </summary>
		// Token: 0x04002E2F RID: 11823
		public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.TickPlacement" /> dependency property. </summary>
		// Token: 0x04002E30 RID: 11824
		public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(Slider), new FrameworkPropertyMetadata(TickPlacement.None), new ValidateValueCallback(Slider.IsValidTickPlacement));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.TickFrequency" /> dependency property. </summary>
		// Token: 0x04002E31 RID: 11825
		public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register("TickFrequency", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(Slider.IsValidDoubleValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.Ticks" /> dependency property. </summary>
		// Token: 0x04002E32 RID: 11826
		public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(Slider), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(DoubleCollection.Empty)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.IsSelectionRangeEnabled" /> dependency property. </summary>
		// Token: 0x04002E33 RID: 11827
		public static readonly DependencyProperty IsSelectionRangeEnabledProperty = DependencyProperty.Register("IsSelectionRangeEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.SelectionStart" /> dependency property. </summary>
		// Token: 0x04002E34 RID: 11828
		public static readonly DependencyProperty SelectionStartProperty = DependencyProperty.Register("SelectionStart", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Slider.OnSelectionStartChanged), new CoerceValueCallback(Slider.CoerceSelectionStart)), new ValidateValueCallback(Slider.IsValidDoubleValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.SelectionEnd" /> dependency property. </summary>
		// Token: 0x04002E35 RID: 11829
		public static readonly DependencyProperty SelectionEndProperty = DependencyProperty.Register("SelectionEnd", typeof(double), typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Slider.OnSelectionEndChanged), new CoerceValueCallback(Slider.CoerceSelectionEnd)), new ValidateValueCallback(Slider.IsValidDoubleValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Slider.IsMoveToPointEnabled" /> dependency property. </summary>
		// Token: 0x04002E36 RID: 11830
		public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register("IsMoveToPointEnabled", typeof(bool), typeof(Slider), new FrameworkPropertyMetadata(false));

		// Token: 0x04002E37 RID: 11831
		private const string TrackName = "PART_Track";

		// Token: 0x04002E38 RID: 11832
		private const string SelectionRangeElementName = "PART_SelectionRange";

		// Token: 0x04002E39 RID: 11833
		private FrameworkElement _selectionRangeElement;

		// Token: 0x04002E3A RID: 11834
		private Track _track;

		// Token: 0x04002E3B RID: 11835
		private ToolTip _autoToolTip;

		// Token: 0x04002E3C RID: 11836
		private object _thumbOriginalToolTip;

		// Token: 0x04002E3D RID: 11837
		private static DependencyObjectType _dType;

		// Token: 0x020009BB RID: 2491
		private class SliderGesture : InputGesture
		{
			// Token: 0x06008876 RID: 34934 RVA: 0x00252437 File Offset: 0x00250637
			public SliderGesture(Key normal, Key inverted, bool forHorizontal)
			{
				this._normal = normal;
				this._inverted = inverted;
				this._forHorizontal = forHorizontal;
			}

			// Token: 0x06008877 RID: 34935 RVA: 0x00252454 File Offset: 0x00250654
			public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
			{
				KeyEventArgs keyEventArgs = inputEventArgs as KeyEventArgs;
				Slider slider = targetElement as Slider;
				if (keyEventArgs != null && slider != null && Keyboard.Modifiers == ModifierKeys.None)
				{
					if (this._normal == keyEventArgs.RealKey)
					{
						return !this.IsInverted(slider);
					}
					if (this._inverted == keyEventArgs.RealKey)
					{
						return this.IsInverted(slider);
					}
				}
				return false;
			}

			// Token: 0x06008878 RID: 34936 RVA: 0x002524AC File Offset: 0x002506AC
			private bool IsInverted(Slider slider)
			{
				if (this._forHorizontal)
				{
					return slider.IsDirectionReversed != (slider.FlowDirection == FlowDirection.RightToLeft);
				}
				return slider.IsDirectionReversed;
			}

			// Token: 0x04004561 RID: 17761
			private Key _normal;

			// Token: 0x04004562 RID: 17762
			private Key _inverted;

			// Token: 0x04004563 RID: 17763
			private bool _forHorizontal;
		}
	}
}
