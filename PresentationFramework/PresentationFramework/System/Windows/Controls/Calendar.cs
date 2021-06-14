using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that enables a user to select a date by using a visual calendar display. </summary>
	// Token: 0x02000475 RID: 1141
	[TemplatePart(Name = "PART_Root", Type = typeof(Panel))]
	[TemplatePart(Name = "PART_CalendarItem", Type = typeof(CalendarItem))]
	public class Calendar : Control
	{
		/// <summary>Occurs when the collection returned by the <see cref="P:System.Windows.Controls.Calendar.SelectedDates" /> property is changed. </summary>
		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06004290 RID: 17040 RVA: 0x00131063 File Offset: 0x0012F263
		// (remove) Token: 0x06004291 RID: 17041 RVA: 0x00131071 File Offset: 0x0012F271
		public event EventHandler<SelectionChangedEventArgs> SelectedDatesChanged
		{
			add
			{
				base.AddHandler(Calendar.SelectedDatesChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Calendar.SelectedDatesChangedEvent, value);
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.Calendar.DisplayDate" /> property is changed.</summary>
		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06004292 RID: 17042 RVA: 0x00131080 File Offset: 0x0012F280
		// (remove) Token: 0x06004293 RID: 17043 RVA: 0x001310B8 File Offset: 0x0012F2B8
		public event EventHandler<CalendarDateChangedEventArgs> DisplayDateChanged;

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.Calendar.DisplayMode" /> property is changed. </summary>
		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06004294 RID: 17044 RVA: 0x001310F0 File Offset: 0x0012F2F0
		// (remove) Token: 0x06004295 RID: 17045 RVA: 0x00131128 File Offset: 0x0012F328
		public event EventHandler<CalendarModeChangedEventArgs> DisplayModeChanged;

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.Calendar.SelectionMode" /> changes.</summary>
		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x06004296 RID: 17046 RVA: 0x00131160 File Offset: 0x0012F360
		// (remove) Token: 0x06004297 RID: 17047 RVA: 0x00131198 File Offset: 0x0012F398
		public event EventHandler<EventArgs> SelectionModeChanged;

		// Token: 0x06004298 RID: 17048 RVA: 0x001311D0 File Offset: 0x0012F3D0
		static Calendar()
		{
			Calendar.SelectedDatesChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesChanged", RoutingStrategy.Direct, typeof(EventHandler<SelectionChangedEventArgs>), typeof(Calendar));
			Calendar.CalendarButtonStyleProperty = DependencyProperty.Register("CalendarButtonStyle", typeof(Style), typeof(Calendar));
			Calendar.CalendarDayButtonStyleProperty = DependencyProperty.Register("CalendarDayButtonStyle", typeof(Style), typeof(Calendar));
			Calendar.CalendarItemStyleProperty = DependencyProperty.Register("CalendarItemStyle", typeof(Style), typeof(Calendar));
			Calendar.DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(Calendar), new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateChanged), new CoerceValueCallback(Calendar.CoerceDisplayDate)));
			Calendar.DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateEndChanged), new CoerceValueCallback(Calendar.CoerceDisplayDateEnd)));
			Calendar.DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayDateStartChanged), new CoerceValueCallback(Calendar.CoerceDisplayDateStart)));
			Calendar.DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(CalendarMode), typeof(Calendar), new FrameworkPropertyMetadata(CalendarMode.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnDisplayModePropertyChanged)), new ValidateValueCallback(Calendar.IsValidDisplayMode));
			Calendar.FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(Calendar), new FrameworkPropertyMetadata(DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek, new PropertyChangedCallback(Calendar.OnFirstDayOfWeekChanged)), new ValidateValueCallback(Calendar.IsValidFirstDayOfWeek));
			Calendar.IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(Calendar), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(Calendar.OnIsTodayHighlightedChanged)));
			Calendar.SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(Calendar), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Calendar.OnSelectedDateChanged)));
			Calendar.SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(CalendarSelectionMode), typeof(Calendar), new FrameworkPropertyMetadata(CalendarSelectionMode.SingleDate, new PropertyChangedCallback(Calendar.OnSelectionModeChanged)), new ValidateValueCallback(Calendar.IsValidSelectionMode));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			FrameworkElement.LanguageProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(new PropertyChangedCallback(Calendar.OnLanguageChanged)));
			EventManager.RegisterClassHandler(typeof(Calendar), UIElement.GotFocusEvent, new RoutedEventHandler(Calendar.OnGotFocus));
			ControlsTraceLogger.AddControl(TelemetryControls.Calendar);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Calendar" /> class. </summary>
		// Token: 0x06004299 RID: 17049 RVA: 0x00131546 File Offset: 0x0012F746
		public Calendar()
		{
			this._blackoutDates = new CalendarBlackoutDatesCollection(this);
			this._selectedDates = new SelectedDatesCollection(this);
			base.SetCurrentValueInternal(Calendar.DisplayDateProperty, DateTime.Today);
		}

		/// <summary>Gets a collection of dates that are marked as not selectable.</summary>
		/// <returns>A collection of dates that cannot be selected. The default value is an empty collection.</returns>
		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x0600429A RID: 17050 RVA: 0x0013157B File Offset: 0x0012F77B
		public CalendarBlackoutDatesCollection BlackoutDates
		{
			get
			{
				return this._blackoutDates;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> associated with the control's internal <see cref="T:System.Windows.Controls.Primitives.CalendarButton" /> object.</summary>
		/// <returns>The current style of the <see cref="T:System.Windows.Controls.Primitives.CalendarButton" /> object.</returns>
		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x0600429B RID: 17051 RVA: 0x00131583 File Offset: 0x0012F783
		// (set) Token: 0x0600429C RID: 17052 RVA: 0x00131595 File Offset: 0x0012F795
		public Style CalendarButtonStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarButtonStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarButtonStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> associated with the control's internal <see cref="T:System.Windows.Controls.Primitives.CalendarDayButton" /> object.</summary>
		/// <returns>The current style of the <see cref="T:System.Windows.Controls.Primitives.CalendarDayButton" /> object.</returns>
		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x0600429D RID: 17053 RVA: 0x001315A3 File Offset: 0x0012F7A3
		// (set) Token: 0x0600429E RID: 17054 RVA: 0x001315B5 File Offset: 0x0012F7B5
		public Style CalendarDayButtonStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarDayButtonStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarDayButtonStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> associated with the control's internal <see cref="T:System.Windows.Controls.Primitives.CalendarItem" /> object.</summary>
		/// <returns>The current style of the <see cref="T:System.Windows.Controls.Primitives.CalendarItem" /> object.</returns>
		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x0600429F RID: 17055 RVA: 0x001315C3 File Offset: 0x0012F7C3
		// (set) Token: 0x060042A0 RID: 17056 RVA: 0x001315D5 File Offset: 0x0012F7D5
		public Style CalendarItemStyle
		{
			get
			{
				return (Style)base.GetValue(Calendar.CalendarItemStyleProperty);
			}
			set
			{
				base.SetValue(Calendar.CalendarItemStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the date to display.</summary>
		/// <returns>The date to display. The default is <see cref="P:System.DateTime.Today" />.</returns>
		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x001315E3 File Offset: 0x0012F7E3
		// (set) Token: 0x060042A2 RID: 17058 RVA: 0x001315F5 File Offset: 0x0012F7F5
		public DateTime DisplayDate
		{
			get
			{
				return (DateTime)base.GetValue(Calendar.DisplayDateProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateProperty, value);
			}
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x00131608 File Offset: 0x0012F808
		private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.DisplayDateInternal = DateTimeHelper.DiscardDayTime((DateTime)e.NewValue);
			calendar.UpdateCellItems();
			calendar.OnDisplayDateChanged(new CalendarDateChangedEventArgs(new DateTime?((DateTime)e.OldValue), new DateTime?((DateTime)e.NewValue)));
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x00131668 File Offset: 0x0012F868
		private static object CoerceDisplayDate(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime t = (DateTime)value;
			if (calendar.DisplayDateStart != null && t < calendar.DisplayDateStart.Value)
			{
				value = calendar.DisplayDateStart.Value;
			}
			else if (calendar.DisplayDateEnd != null && t > calendar.DisplayDateEnd.Value)
			{
				value = calendar.DisplayDateEnd.Value;
			}
			return value;
		}

		/// <summary>Gets or sets the last date in the date range that is available in the calendar.</summary>
		/// <returns>The last date that is available in the calendar.</returns>
		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x001316FC File Offset: 0x0012F8FC
		// (set) Token: 0x060042A6 RID: 17062 RVA: 0x0013170E File Offset: 0x0012F90E
		public DateTime? DisplayDateEnd
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.DisplayDateEndProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateEndProperty, value);
			}
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x00131724 File Offset: 0x0012F924
		private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.CoerceValue(Calendar.DisplayDateProperty);
			calendar.UpdateCellItems();
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x0013174C File Offset: 0x0012F94C
		private static object CoerceDisplayDateEnd(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime? dateTime = (DateTime?)value;
			if (dateTime != null)
			{
				if (calendar.DisplayDateStart != null && dateTime.Value < calendar.DisplayDateStart.Value)
				{
					value = calendar.DisplayDateStart;
				}
				DateTime? maximumDate = calendar.SelectedDates.MaximumDate;
				if (maximumDate != null && dateTime.Value < maximumDate.Value)
				{
					value = maximumDate;
				}
			}
			return value;
		}

		/// <summary>Gets or sets the first date that is available in the calendar.</summary>
		/// <returns>The first date that is available in the calendar. The default is <see langword="null" />.</returns>
		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x001317DC File Offset: 0x0012F9DC
		// (set) Token: 0x060042AA RID: 17066 RVA: 0x001317EE File Offset: 0x0012F9EE
		public DateTime? DisplayDateStart
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.DisplayDateStartProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayDateStartProperty, value);
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x00131804 File Offset: 0x0012FA04
		private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.CoerceValue(Calendar.DisplayDateEndProperty);
			calendar.CoerceValue(Calendar.DisplayDateProperty);
			calendar.UpdateCellItems();
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x00131834 File Offset: 0x0012FA34
		private static object CoerceDisplayDateStart(DependencyObject d, object value)
		{
			Calendar calendar = d as Calendar;
			DateTime? dateTime = (DateTime?)value;
			if (dateTime != null)
			{
				DateTime? minimumDate = calendar.SelectedDates.MinimumDate;
				if (minimumDate != null && dateTime.Value > minimumDate.Value)
				{
					value = minimumDate;
				}
			}
			return value;
		}

		/// <summary>Gets or sets a value that indicates whether the calendar displays a month, year, or decade.</summary>
		/// <returns>A value that indicates what length of time the <see cref="T:System.Windows.Controls.Calendar" /> should display.</returns>
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x0013188B File Offset: 0x0012FA8B
		// (set) Token: 0x060042AE RID: 17070 RVA: 0x0013189D File Offset: 0x0012FA9D
		public CalendarMode DisplayMode
		{
			get
			{
				return (CalendarMode)base.GetValue(Calendar.DisplayModeProperty);
			}
			set
			{
				base.SetValue(Calendar.DisplayModeProperty, value);
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x001318B0 File Offset: 0x0012FAB0
		private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			CalendarMode calendarMode = (CalendarMode)e.NewValue;
			CalendarMode calendarMode2 = (CalendarMode)e.OldValue;
			CalendarItem monthControl = calendar.MonthControl;
			if (calendarMode != CalendarMode.Month)
			{
				if (calendarMode - CalendarMode.Year <= 1)
				{
					if (calendarMode2 == CalendarMode.Month)
					{
						calendar.SetCurrentValueInternal(Calendar.DisplayDateProperty, calendar.CurrentDate);
					}
					calendar.UpdateCellItems();
				}
			}
			else
			{
				if (calendarMode2 == CalendarMode.Year || calendarMode2 == CalendarMode.Decade)
				{
					Calendar calendar2 = calendar;
					Calendar calendar3 = calendar;
					DateTime? dateTime = null;
					calendar3.HoverEnd = dateTime;
					calendar2.HoverStart = dateTime;
					calendar.CurrentDate = calendar.DisplayDate;
				}
				calendar.UpdateCellItems();
			}
			calendar.OnDisplayModeChanged(new CalendarModeChangedEventArgs((CalendarMode)e.OldValue, calendarMode));
		}

		/// <summary>Gets or sets the day that is considered the beginning of the week.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> that represents the beginning of the week. The default is the <see cref="P:System.Globalization.DateTimeFormatInfo.FirstDayOfWeek" /> that is determined by the current culture.</returns>
		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x0013195B File Offset: 0x0012FB5B
		// (set) Token: 0x060042B1 RID: 17073 RVA: 0x0013196D File Offset: 0x0012FB6D
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)base.GetValue(Calendar.FirstDayOfWeekProperty);
			}
			set
			{
				base.SetValue(Calendar.FirstDayOfWeekProperty, value);
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x00131980 File Offset: 0x0012FB80
		private static void OnFirstDayOfWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			calendar.UpdateCellItems();
		}

		/// <summary>Gets or sets a value that indicates whether the current date is highlighted.</summary>
		/// <returns>
		///     <see langword="true" /> if the current date is highlighted; otherwise, <see langword="false" />. The default is <see langword="true" />. </returns>
		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x0013199A File Offset: 0x0012FB9A
		// (set) Token: 0x060042B4 RID: 17076 RVA: 0x001319AC File Offset: 0x0012FBAC
		public bool IsTodayHighlighted
		{
			get
			{
				return (bool)base.GetValue(Calendar.IsTodayHighlightedProperty);
			}
			set
			{
				base.SetValue(Calendar.IsTodayHighlightedProperty, value);
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x001319BC File Offset: 0x0012FBBC
		private static void OnIsTodayHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			int num = DateTimeHelper.CompareYearMonth(calendar.DisplayDateInternal, DateTime.Today);
			if (num > -2 && num < 2)
			{
				calendar.UpdateCellItems();
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x001319F0 File Offset: 0x0012FBF0
		private static void OnLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			if (DependencyPropertyHelper.GetValueSource(d, Calendar.FirstDayOfWeekProperty).BaseValueSource == BaseValueSource.Default)
			{
				calendar.SetCurrentValueInternal(Calendar.FirstDayOfWeekProperty, DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(calendar)).FirstDayOfWeek);
				calendar.UpdateCellItems();
			}
		}

		/// <summary>Gets or sets the currently selected date.</summary>
		/// <returns>The date currently selected. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified date is outside the range specified by <see cref="P:System.Windows.Controls.Calendar.DisplayDateStart" /> and <see cref="P:System.Windows.Controls.Calendar.DisplayDateEnd" />-or-The specified date is in the <see cref="P:System.Windows.Controls.Calendar.BlackoutDates" /> collection.</exception>
		/// <exception cref="T:System.InvalidOperationException">If set to anything other than <see langword="null" /> when <see cref="P:System.Windows.Controls.Calendar.SelectionMode" /> is set to <see cref="F:System.Windows.Controls.CalendarSelectionMode.None" />.</exception>
		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00131A40 File Offset: 0x0012FC40
		// (set) Token: 0x060042B8 RID: 17080 RVA: 0x00131A52 File Offset: 0x0012FC52
		public DateTime? SelectedDate
		{
			get
			{
				return (DateTime?)base.GetValue(Calendar.SelectedDateProperty);
			}
			set
			{
				base.SetValue(Calendar.SelectedDateProperty, value);
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x00131A68 File Offset: 0x0012FC68
		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			if (calendar.SelectionMode == CalendarSelectionMode.None && e.NewValue != null)
			{
				throw new InvalidOperationException(SR.Get("Calendar_OnSelectedDateChanged_InvalidOperation"));
			}
			DateTime? dateTime = (DateTime?)e.NewValue;
			if (!Calendar.IsValidDateSelection(calendar, dateTime))
			{
				throw new ArgumentOutOfRangeException("d", SR.Get("Calendar_OnSelectedDateChanged_InvalidValue"));
			}
			if (dateTime == null)
			{
				calendar.SelectedDates.ClearInternal(true);
			}
			else if (dateTime != null && (calendar.SelectedDates.Count <= 0 || !(calendar.SelectedDates[0] == dateTime.Value)))
			{
				calendar.SelectedDates.ClearInternal();
				calendar.SelectedDates.Add(dateTime.Value);
			}
			if (calendar.SelectionMode == CalendarSelectionMode.SingleDate)
			{
				if (dateTime != null)
				{
					calendar.CurrentDate = dateTime.Value;
				}
				calendar.UpdateCellItems();
				return;
			}
		}

		/// <summary>Gets a collection of selected dates.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.SelectedDatesCollection" /> object that contains the currently selected dates. The default is an empty collection.</returns>
		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x00131B5E File Offset: 0x0012FD5E
		public SelectedDatesCollection SelectedDates
		{
			get
			{
				return this._selectedDates;
			}
		}

		/// <summary>Gets or sets a value that indicates what kind of selections are allowed.</summary>
		/// <returns>A value that indicates the current selection mode. The default is <see cref="F:System.Windows.Controls.CalendarSelectionMode.SingleDate" />.</returns>
		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x060042BB RID: 17083 RVA: 0x00131B66 File Offset: 0x0012FD66
		// (set) Token: 0x060042BC RID: 17084 RVA: 0x00131B78 File Offset: 0x0012FD78
		public CalendarSelectionMode SelectionMode
		{
			get
			{
				return (CalendarSelectionMode)base.GetValue(Calendar.SelectionModeProperty);
			}
			set
			{
				base.SetValue(Calendar.SelectionModeProperty, value);
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x00131B8C File Offset: 0x0012FD8C
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Calendar calendar = d as Calendar;
			Calendar calendar2 = calendar;
			Calendar calendar3 = calendar;
			DateTime? dateTime = null;
			calendar3.HoverEnd = dateTime;
			calendar2.HoverStart = dateTime;
			calendar.SelectedDates.ClearInternal(true);
			calendar.OnSelectionModeChanged(EventArgs.Empty);
		}

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x060042BE RID: 17086 RVA: 0x00131BD0 File Offset: 0x0012FDD0
		// (remove) Token: 0x060042BF RID: 17087 RVA: 0x00131C08 File Offset: 0x0012FE08
		internal event MouseButtonEventHandler DayButtonMouseUp;

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060042C0 RID: 17088 RVA: 0x00131C40 File Offset: 0x0012FE40
		// (remove) Token: 0x060042C1 RID: 17089 RVA: 0x00131C78 File Offset: 0x0012FE78
		internal event RoutedEventHandler DayOrMonthPreviewKeyDown;

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x060042C2 RID: 17090 RVA: 0x00131CAD File Offset: 0x0012FEAD
		// (set) Token: 0x060042C3 RID: 17091 RVA: 0x00131CB5 File Offset: 0x0012FEB5
		internal bool DatePickerDisplayDateFlag { get; set; }

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x060042C4 RID: 17092 RVA: 0x00131CBE File Offset: 0x0012FEBE
		// (set) Token: 0x060042C5 RID: 17093 RVA: 0x00131CC6 File Offset: 0x0012FEC6
		internal DateTime DisplayDateInternal { get; private set; }

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x060042C6 RID: 17094 RVA: 0x00131CD0 File Offset: 0x0012FED0
		internal DateTime DisplayDateEndInternal
		{
			get
			{
				return this.DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x00131CF0 File Offset: 0x0012FEF0
		internal DateTime DisplayDateStartInternal
		{
			get
			{
				return this.DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x060042C8 RID: 17096 RVA: 0x00131D10 File Offset: 0x0012FF10
		// (set) Token: 0x060042C9 RID: 17097 RVA: 0x00131D23 File Offset: 0x0012FF23
		internal DateTime CurrentDate
		{
			get
			{
				return this._currentDate.GetValueOrDefault(this.DisplayDateInternal);
			}
			set
			{
				this._currentDate = new DateTime?(value);
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x060042CA RID: 17098 RVA: 0x00131D34 File Offset: 0x0012FF34
		// (set) Token: 0x060042CB RID: 17099 RVA: 0x00131D5A File Offset: 0x0012FF5A
		internal DateTime? HoverStart
		{
			get
			{
				if (this.SelectionMode != CalendarSelectionMode.None)
				{
					return this._hoverStart;
				}
				return null;
			}
			set
			{
				this._hoverStart = value;
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x060042CC RID: 17100 RVA: 0x00131D64 File Offset: 0x0012FF64
		// (set) Token: 0x060042CD RID: 17101 RVA: 0x00131D8A File Offset: 0x0012FF8A
		internal DateTime? HoverEnd
		{
			get
			{
				if (this.SelectionMode != CalendarSelectionMode.None)
				{
					return this._hoverEnd;
				}
				return null;
			}
			set
			{
				this._hoverEnd = value;
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x00131D93 File Offset: 0x0012FF93
		internal CalendarItem MonthControl
		{
			get
			{
				return this._monthControl;
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x00131D9B File Offset: 0x0012FF9B
		internal DateTime DisplayMonth
		{
			get
			{
				return DateTimeHelper.DiscardDayTime(this.DisplayDate);
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x00131DA8 File Offset: 0x0012FFA8
		internal DateTime DisplayYear
		{
			get
			{
				return new DateTime(this.DisplayDate.Year, 1, 1);
			}
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.Calendar" /> control when a new template is applied.</summary>
		// Token: 0x060042D1 RID: 17105 RVA: 0x00131DCC File Offset: 0x0012FFCC
		public override void OnApplyTemplate()
		{
			if (this._monthControl != null)
			{
				this._monthControl.Owner = null;
			}
			base.OnApplyTemplate();
			this._monthControl = (base.GetTemplateChild("PART_CalendarItem") as CalendarItem);
			if (this._monthControl != null)
			{
				this._monthControl.Owner = this;
			}
			this.CurrentDate = this.DisplayDate;
			this.UpdateCellItems();
		}

		/// <summary>Provides a text representation of the selected date.</summary>
		/// <returns>A text representation of the selected date, or an empty string if <see cref="P:System.Windows.Controls.Calendar.SelectedDate" /> is <see langword="null" />.</returns>
		// Token: 0x060042D2 RID: 17106 RVA: 0x00131E30 File Offset: 0x00130030
		public override string ToString()
		{
			if (this.SelectedDate != null)
			{
				return this.SelectedDate.Value.ToString(DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
			}
			return string.Empty;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Calendar.SelectedDatesChanged" /> routed event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D3 RID: 17107 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelectedDatesChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Calendar.DisplayDateChanged" /> event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D4 RID: 17108 RVA: 0x00131E74 File Offset: 0x00130074
		protected virtual void OnDisplayDateChanged(CalendarDateChangedEventArgs e)
		{
			EventHandler<CalendarDateChangedEventArgs> displayDateChanged = this.DisplayDateChanged;
			if (displayDateChanged != null)
			{
				displayDateChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Calendar.DisplayModeChanged" /> event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D5 RID: 17109 RVA: 0x00131E94 File Offset: 0x00130094
		protected virtual void OnDisplayModeChanged(CalendarModeChangedEventArgs e)
		{
			EventHandler<CalendarModeChangedEventArgs> displayModeChanged = this.DisplayModeChanged;
			if (displayModeChanged != null)
			{
				displayModeChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Calendar.SelectionModeChanged" /> event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D6 RID: 17110 RVA: 0x00131EB4 File Offset: 0x001300B4
		protected virtual void OnSelectionModeChanged(EventArgs e)
		{
			EventHandler<EventArgs> selectionModeChanged = this.SelectionModeChanged;
			if (selectionModeChanged != null)
			{
				selectionModeChanged(this, e);
			}
		}

		/// <summary>Returns a <see cref="T:System.Windows.Automation.Peers.CalendarAutomationPeer" /> for use by the Silverlight automation infrastructure.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.CalendarAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Calendar" /> object.</returns>
		// Token: 0x060042D7 RID: 17111 RVA: 0x00131ED3 File Offset: 0x001300D3
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarAutomationPeer(this);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when the user presses a key while this control has focus.</summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D8 RID: 17112 RVA: 0x00131EDB File Offset: 0x001300DB
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = this.ProcessCalendarKey(e);
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyUp" /> routed event that occurs when the user releases a key while this control has focus.</summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x060042D9 RID: 17113 RVA: 0x00131EF2 File Offset: 0x001300F2
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (!e.Handled && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
			{
				this.ProcessShiftKeyUp();
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x00131F18 File Offset: 0x00130118
		internal CalendarDayButton FindDayButtonFromDay(DateTime day)
		{
			if (this.MonthControl != null)
			{
				foreach (CalendarDayButton calendarDayButton in this.MonthControl.GetCalendarDayButtons())
				{
					if (calendarDayButton.DataContext is DateTime && DateTimeHelper.CompareDays((DateTime)calendarDayButton.DataContext, day) == 0)
					{
						return calendarDayButton;
					}
				}
			}
			return null;
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x00131F94 File Offset: 0x00130194
		internal static bool IsValidDateSelection(Calendar cal, object value)
		{
			return value == null || !cal.BlackoutDates.Contains((DateTime)value);
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x00131FB0 File Offset: 0x001301B0
		internal void OnDayButtonMouseUp(MouseButtonEventArgs e)
		{
			MouseButtonEventHandler dayButtonMouseUp = this.DayButtonMouseUp;
			if (dayButtonMouseUp != null)
			{
				dayButtonMouseUp(this, e);
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x00131FD0 File Offset: 0x001301D0
		internal void OnDayOrMonthPreviewKeyDown(RoutedEventArgs e)
		{
			RoutedEventHandler dayOrMonthPreviewKeyDown = this.DayOrMonthPreviewKeyDown;
			if (dayOrMonthPreviewKeyDown != null)
			{
				dayOrMonthPreviewKeyDown(this, e);
			}
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00131FEF File Offset: 0x001301EF
		internal void OnDayClick(DateTime selectedDate)
		{
			if (this.SelectionMode == CalendarSelectionMode.None)
			{
				this.CurrentDate = selectedDate;
			}
			if (DateTimeHelper.CompareYearMonth(selectedDate, this.DisplayDateInternal) != 0)
			{
				this.MoveDisplayTo(new DateTime?(selectedDate));
				return;
			}
			this.UpdateCellItems();
			this.FocusDate(selectedDate);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0013202C File Offset: 0x0013022C
		internal void OnCalendarButtonPressed(CalendarButton b, bool switchDisplayMode)
		{
			if (b.DataContext is DateTime)
			{
				DateTime yearMonth = (DateTime)b.DataContext;
				DateTime? dateTime = null;
				CalendarMode calendarMode = CalendarMode.Month;
				switch (this.DisplayMode)
				{
				case CalendarMode.Year:
					dateTime = DateTimeHelper.SetYearMonth(this.DisplayDate, yearMonth);
					calendarMode = CalendarMode.Month;
					break;
				case CalendarMode.Decade:
					dateTime = DateTimeHelper.SetYear(this.DisplayDate, yearMonth.Year);
					calendarMode = CalendarMode.Year;
					break;
				}
				if (dateTime != null)
				{
					this.DisplayDate = dateTime.Value;
					if (switchDisplayMode)
					{
						base.SetCurrentValueInternal(Calendar.DisplayModeProperty, calendarMode);
						this.FocusDate((this.DisplayMode == CalendarMode.Month) ? this.CurrentDate : this.DisplayDate);
					}
				}
			}
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x001320E8 File Offset: 0x001302E8
		private DateTime? GetDateOffset(DateTime date, int offset, CalendarMode displayMode)
		{
			DateTime? result = null;
			switch (displayMode)
			{
			case CalendarMode.Month:
				result = DateTimeHelper.AddMonths(date, offset);
				break;
			case CalendarMode.Year:
				result = DateTimeHelper.AddYears(date, offset);
				break;
			case CalendarMode.Decade:
				result = DateTimeHelper.AddYears(this.DisplayDate, offset * 10);
				break;
			}
			return result;
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x00132138 File Offset: 0x00130338
		private void MoveDisplayTo(DateTime? date)
		{
			if (date != null)
			{
				DateTime date2 = date.Value.Date;
				CalendarMode displayMode = this.DisplayMode;
				if (displayMode != CalendarMode.Month)
				{
					if (displayMode - CalendarMode.Year <= 1)
					{
						base.SetCurrentValueInternal(Calendar.DisplayDateProperty, date2);
						this.UpdateCellItems();
					}
				}
				else
				{
					base.SetCurrentValueInternal(Calendar.DisplayDateProperty, DateTimeHelper.DiscardDayTime(date2));
					this.CurrentDate = date2;
					this.UpdateCellItems();
				}
				this.FocusDate(date2);
			}
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001321B4 File Offset: 0x001303B4
		internal void OnNextClick()
		{
			DateTime? dateOffset = this.GetDateOffset(this.DisplayDate, 1, this.DisplayMode);
			if (dateOffset != null)
			{
				this.MoveDisplayTo(new DateTime?(DateTimeHelper.DiscardDayTime(dateOffset.Value)));
			}
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x001321F8 File Offset: 0x001303F8
		internal void OnPreviousClick()
		{
			DateTime? dateOffset = this.GetDateOffset(this.DisplayDate, -1, this.DisplayMode);
			if (dateOffset != null)
			{
				this.MoveDisplayTo(new DateTime?(DateTimeHelper.DiscardDayTime(dateOffset.Value)));
			}
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x0013223C File Offset: 0x0013043C
		internal void OnSelectedDatesCollectionChanged(SelectionChangedEventArgs e)
		{
			if (Calendar.IsSelectionChanged(e))
			{
				if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
				{
					CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.FromElement(this) as CalendarAutomationPeer;
					if (calendarAutomationPeer != null)
					{
						calendarAutomationPeer.RaiseSelectionEvents(e);
					}
				}
				this.CoerceFromSelection();
				this.OnSelectedDatesChanged(e);
			}
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x0013228C File Offset: 0x0013048C
		internal void UpdateCellItems()
		{
			CalendarItem monthControl = this.MonthControl;
			if (monthControl != null)
			{
				switch (this.DisplayMode)
				{
				case CalendarMode.Month:
					monthControl.UpdateMonthMode();
					return;
				case CalendarMode.Year:
					monthControl.UpdateYearMode();
					return;
				case CalendarMode.Decade:
					monthControl.UpdateDecadeMode();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x001322D1 File Offset: 0x001304D1
		private void CoerceFromSelection()
		{
			base.CoerceValue(Calendar.DisplayDateStartProperty);
			base.CoerceValue(Calendar.DisplayDateEndProperty);
			base.CoerceValue(Calendar.DisplayDateProperty);
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x001322F4 File Offset: 0x001304F4
		private void AddKeyboardSelection()
		{
			if (this.HoverStart != null)
			{
				this.SelectedDates.ClearInternal();
				this.SelectedDates.AddRange(this.HoverStart.Value, this.CurrentDate);
			}
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x0013233C File Offset: 0x0013053C
		private static bool IsSelectionChanged(SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count != e.RemovedItems.Count)
			{
				return true;
			}
			foreach (object obj in e.AddedItems)
			{
				DateTime dateTime = (DateTime)obj;
				if (!e.RemovedItems.Contains(dateTime))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x001323C4 File Offset: 0x001305C4
		private static bool IsValidDisplayMode(object value)
		{
			CalendarMode calendarMode = (CalendarMode)value;
			return calendarMode == CalendarMode.Month || calendarMode == CalendarMode.Year || calendarMode == CalendarMode.Decade;
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x001323E8 File Offset: 0x001305E8
		internal static bool IsValidFirstDayOfWeek(object value)
		{
			DayOfWeek dayOfWeek = (DayOfWeek)value;
			return dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday || dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Friday || dayOfWeek == DayOfWeek.Saturday;
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x0013241C File Offset: 0x0013061C
		private static bool IsValidKeyboardSelection(Calendar cal, object value)
		{
			return value == null || (!cal.BlackoutDates.Contains((DateTime)value) && DateTime.Compare((DateTime)value, cal.DisplayDateStartInternal) >= 0 && DateTime.Compare((DateTime)value, cal.DisplayDateEndInternal) <= 0);
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00132470 File Offset: 0x00130670
		private static bool IsValidSelectionMode(object value)
		{
			CalendarSelectionMode calendarSelectionMode = (CalendarSelectionMode)value;
			return calendarSelectionMode == CalendarSelectionMode.SingleDate || calendarSelectionMode == CalendarSelectionMode.SingleRange || calendarSelectionMode == CalendarSelectionMode.MultipleRange || calendarSelectionMode == CalendarSelectionMode.None;
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00132495 File Offset: 0x00130695
		private void OnSelectedMonthChanged(DateTime? selectedMonth)
		{
			if (selectedMonth != null)
			{
				base.SetCurrentValueInternal(Calendar.DisplayDateProperty, selectedMonth.Value);
				this.UpdateCellItems();
				this.FocusDate(selectedMonth.Value);
			}
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00132495 File Offset: 0x00130695
		private void OnSelectedYearChanged(DateTime? selectedYear)
		{
			if (selectedYear != null)
			{
				base.SetCurrentValueInternal(Calendar.DisplayDateProperty, selectedYear.Value);
				this.UpdateCellItems();
				this.FocusDate(selectedYear.Value);
			}
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x001324CA File Offset: 0x001306CA
		internal void FocusDate(DateTime date)
		{
			if (this.MonthControl != null)
			{
				this.MonthControl.FocusDate(date);
			}
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x001324E0 File Offset: 0x001306E0
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			Calendar calendar = (Calendar)sender;
			if (!e.Handled && e.OriginalSource == calendar)
			{
				if (calendar.SelectedDate != null && DateTimeHelper.CompareYearMonth(calendar.SelectedDate.Value, calendar.DisplayDateInternal) == 0)
				{
					calendar.FocusDate(calendar.SelectedDate.Value);
				}
				else
				{
					calendar.FocusDate(calendar.DisplayDate);
				}
				e.Handled = true;
			}
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x0013255C File Offset: 0x0013075C
		private bool ProcessCalendarKey(KeyEventArgs e)
		{
			if (this.DisplayMode == CalendarMode.Month)
			{
				CalendarDayButton calendarDayButton = (this.MonthControl != null) ? this.MonthControl.GetCalendarDayButton(this.CurrentDate) : null;
				if (DateTimeHelper.CompareYearMonth(this.CurrentDate, this.DisplayDateInternal) != 0 && calendarDayButton != null && !calendarDayButton.IsInactive)
				{
					return false;
				}
			}
			bool ctrl;
			bool shift;
			CalendarKeyboardHelper.GetMetaKeyState(out ctrl, out shift);
			Key key = e.Key;
			if (key != Key.Return)
			{
				switch (key)
				{
				case Key.Space:
					break;
				case Key.Prior:
					this.ProcessPageUpKey(shift);
					return true;
				case Key.Next:
					this.ProcessPageDownKey(shift);
					return true;
				case Key.End:
					this.ProcessEndKey(shift);
					return true;
				case Key.Home:
					this.ProcessHomeKey(shift);
					return true;
				case Key.Left:
					this.ProcessLeftKey(shift);
					return true;
				case Key.Up:
					this.ProcessUpKey(ctrl, shift);
					return true;
				case Key.Right:
					this.ProcessRightKey(shift);
					return true;
				case Key.Down:
					this.ProcessDownKey(ctrl, shift);
					return true;
				default:
					return false;
				}
			}
			return this.ProcessEnterKey();
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00132644 File Offset: 0x00130844
		private void ProcessDownKey(bool ctrl, bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
				if (!ctrl || shift)
				{
					DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, 7), 1);
					this.ProcessSelection(shift, nonBlackoutDate);
					return;
				}
				break;
			case CalendarMode.Year:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Month);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, 4);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, 4);
				this.OnSelectedYearChanged(selectedYear);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x00132704 File Offset: 0x00130904
		private void ProcessEndKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime displayDate = this.DisplayDate;
				DateTime? lastSelectedDate = new DateTime?(new DateTime(this.DisplayDateInternal.Year, this.DisplayDateInternal.Month, 1));
				if (DateTimeHelper.CompareYearMonth(DateTime.MaxValue, lastSelectedDate.Value) > 0)
				{
					lastSelectedDate = new DateTime?(DateTimeHelper.AddMonths(lastSelectedDate.Value, 1).Value);
					lastSelectedDate = new DateTime?(DateTimeHelper.AddDays(lastSelectedDate.Value, -1).Value);
				}
				else
				{
					lastSelectedDate = new DateTime?(DateTime.MaxValue);
				}
				this.ProcessSelection(shift, lastSelectedDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime value = new DateTime(this.DisplayDate.Year, 12, 1);
				this.OnSelectedMonthChanged(new DateTime?(value));
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = new DateTime?(new DateTime(DateTimeHelper.EndOfDecade(this.DisplayDate), 1, 1));
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00132808 File Offset: 0x00130A08
		private bool ProcessEnterKey()
		{
			CalendarMode displayMode = this.DisplayMode;
			if (displayMode == CalendarMode.Year)
			{
				base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Month);
				this.FocusDate(this.DisplayDate);
				return true;
			}
			if (displayMode != CalendarMode.Decade)
			{
				return false;
			}
			base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
			this.FocusDate(this.DisplayDate);
			return true;
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00132868 File Offset: 0x00130A68
		private void ProcessHomeKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? lastSelectedDate = new DateTime?(new DateTime(this.DisplayDateInternal.Year, this.DisplayDateInternal.Month, 1));
				this.ProcessSelection(shift, lastSelectedDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime value = new DateTime(this.DisplayDate.Year, 1, 1);
				this.OnSelectedMonthChanged(new DateTime?(value));
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = new DateTime?(new DateTime(DateTimeHelper.DecadeOfDate(this.DisplayDate), 1, 1));
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00132908 File Offset: 0x00130B08
		private void ProcessLeftKey(bool shift)
		{
			int num = (!base.IsRightToLeft) ? -1 : 1;
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, num), num);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, num);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, num);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x0013298C File Offset: 0x00130B8C
		private void ProcessPageDownKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddMonths(this.CurrentDate, 1), 1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddYears(this.DisplayDate, 1);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, 10);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x00132A00 File Offset: 0x00130C00
		private void ProcessPageUpKey(bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddMonths(this.CurrentDate, -1), -1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddYears(this.DisplayDate, -1);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, -10);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x00132A74 File Offset: 0x00130C74
		private void ProcessRightKey(bool shift)
		{
			int num = (!base.IsRightToLeft) ? 1 : -1;
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, num), num);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, num);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
			{
				DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, num);
				this.OnSelectedYearChanged(selectedYear);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x00132AF8 File Offset: 0x00130CF8
		private void ProcessSelection(bool shift, DateTime? lastSelectedDate)
		{
			if (this.SelectionMode == CalendarSelectionMode.None && lastSelectedDate != null)
			{
				this.OnDayClick(lastSelectedDate.Value);
				return;
			}
			if (lastSelectedDate != null && Calendar.IsValidKeyboardSelection(this, lastSelectedDate.Value))
			{
				if (this.SelectionMode == CalendarSelectionMode.SingleRange || this.SelectionMode == CalendarSelectionMode.MultipleRange)
				{
					this.SelectedDates.ClearInternal();
					if (shift)
					{
						this._isShiftPressed = true;
						DateTime? dateTime = this.HoverStart;
						if (dateTime == null)
						{
							dateTime = new DateTime?(this.CurrentDate);
							this.HoverEnd = dateTime;
							this.HoverStart = dateTime;
						}
						dateTime = this.HoverStart;
						CalendarDateRange range;
						if (DateTime.Compare(dateTime.Value, lastSelectedDate.Value) < 0)
						{
							dateTime = this.HoverStart;
							range = new CalendarDateRange(dateTime.Value, lastSelectedDate.Value);
						}
						else
						{
							DateTime value = lastSelectedDate.Value;
							dateTime = this.HoverStart;
							range = new CalendarDateRange(value, dateTime.Value);
						}
						if (!this.BlackoutDates.ContainsAny(range))
						{
							this._currentDate = lastSelectedDate;
							this.HoverEnd = lastSelectedDate;
						}
						this.OnDayClick(this.CurrentDate);
					}
					else
					{
						DateTime? dateTime = new DateTime?(this.CurrentDate = lastSelectedDate.Value);
						this.HoverEnd = dateTime;
						this.HoverStart = dateTime;
						this.AddKeyboardSelection();
						this.OnDayClick(lastSelectedDate.Value);
					}
				}
				else
				{
					this.CurrentDate = lastSelectedDate.Value;
					DateTime? dateTime = null;
					this.HoverEnd = dateTime;
					this.HoverStart = dateTime;
					if (this.SelectedDates.Count > 0)
					{
						this.SelectedDates[0] = lastSelectedDate.Value;
					}
					else
					{
						this.SelectedDates.Add(lastSelectedDate.Value);
					}
					this.OnDayClick(lastSelectedDate.Value);
				}
				this.UpdateCellItems();
			}
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x00132CC8 File Offset: 0x00130EC8
		private void ProcessShiftKeyUp()
		{
			if (this._isShiftPressed && (this.SelectionMode == CalendarSelectionMode.SingleRange || this.SelectionMode == CalendarSelectionMode.MultipleRange))
			{
				this.AddKeyboardSelection();
				this._isShiftPressed = false;
				DateTime? dateTime = null;
				this.HoverEnd = dateTime;
				this.HoverStart = dateTime;
			}
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00132D14 File Offset: 0x00130F14
		private void ProcessUpKey(bool ctrl, bool shift)
		{
			switch (this.DisplayMode)
			{
			case CalendarMode.Month:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? nonBlackoutDate = this._blackoutDates.GetNonBlackoutDate(DateTimeHelper.AddDays(this.CurrentDate, -7), -1);
				this.ProcessSelection(shift, nonBlackoutDate);
				return;
			}
			case CalendarMode.Year:
			{
				if (ctrl)
				{
					base.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Decade);
					this.FocusDate(this.DisplayDate);
					return;
				}
				DateTime? selectedMonth = DateTimeHelper.AddMonths(this.DisplayDate, -4);
				this.OnSelectedMonthChanged(selectedMonth);
				return;
			}
			case CalendarMode.Decade:
				if (!ctrl)
				{
					DateTime? selectedYear = DateTimeHelper.AddYears(this.DisplayDate, -4);
					this.OnSelectedYearChanged(selectedYear);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x040027FE RID: 10238
		private const string ElementRoot = "PART_Root";

		// Token: 0x040027FF RID: 10239
		private const string ElementMonth = "PART_CalendarItem";

		// Token: 0x04002800 RID: 10240
		private const int COLS = 7;

		// Token: 0x04002801 RID: 10241
		private const int ROWS = 7;

		// Token: 0x04002802 RID: 10242
		private const int YEAR_ROWS = 3;

		// Token: 0x04002803 RID: 10243
		private const int YEAR_COLS = 4;

		// Token: 0x04002804 RID: 10244
		private const int YEARS_PER_DECADE = 10;

		// Token: 0x04002805 RID: 10245
		private DateTime? _hoverStart;

		// Token: 0x04002806 RID: 10246
		private DateTime? _hoverEnd;

		// Token: 0x04002807 RID: 10247
		private bool _isShiftPressed;

		// Token: 0x04002808 RID: 10248
		private DateTime? _currentDate;

		// Token: 0x04002809 RID: 10249
		private CalendarItem _monthControl;

		// Token: 0x0400280A RID: 10250
		private CalendarBlackoutDatesCollection _blackoutDates;

		// Token: 0x0400280B RID: 10251
		private SelectedDatesCollection _selectedDates;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.CalendarButtonStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.CalendarButtonStyle" /> dependency property.</returns>
		// Token: 0x04002810 RID: 10256
		public static readonly DependencyProperty CalendarButtonStyleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.CalendarDayButtonStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.CalendarDayButtonStyle" /> dependency property.</returns>
		// Token: 0x04002811 RID: 10257
		public static readonly DependencyProperty CalendarDayButtonStyleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.CalendarItemStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.CalendarItemStyle" /> dependency property.</returns>
		// Token: 0x04002812 RID: 10258
		public static readonly DependencyProperty CalendarItemStyleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.DisplayDate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.DisplayDate" /> dependency property.</returns>
		// Token: 0x04002813 RID: 10259
		public static readonly DependencyProperty DisplayDateProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.DisplayDateEnd" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.DisplayDateEnd" /> dependency property.</returns>
		// Token: 0x04002814 RID: 10260
		public static readonly DependencyProperty DisplayDateEndProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.DisplayDateStart" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.DisplayDateStart" /> dependency property.</returns>
		// Token: 0x04002815 RID: 10261
		public static readonly DependencyProperty DisplayDateStartProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.DisplayMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.DisplayMode" /> dependency property.</returns>
		// Token: 0x04002816 RID: 10262
		public static readonly DependencyProperty DisplayModeProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.FirstDayOfWeek" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.FirstDayOfWeek" /> dependency property.</returns>
		// Token: 0x04002817 RID: 10263
		public static readonly DependencyProperty FirstDayOfWeekProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.IsTodayHighlighted" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.IsTodayHighlighted" /> dependency property.</returns>
		// Token: 0x04002818 RID: 10264
		public static readonly DependencyProperty IsTodayHighlightedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.SelectedDate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.SelectedDate" /> dependency property.</returns>
		// Token: 0x04002819 RID: 10265
		public static readonly DependencyProperty SelectedDateProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Calendar.SelectionMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Calendar.SelectionMode" /> dependency property.</returns>
		// Token: 0x0400281A RID: 10266
		public static readonly DependencyProperty SelectionModeProperty;
	}
}
