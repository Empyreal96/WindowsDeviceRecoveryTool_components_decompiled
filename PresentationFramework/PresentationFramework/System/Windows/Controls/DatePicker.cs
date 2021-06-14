using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that allows the user to select a date.</summary>
	// Token: 0x020004C1 RID: 1217
	[TemplatePart(Name = "PART_Root", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_TextBox", Type = typeof(DatePickerTextBox))]
	[TemplatePart(Name = "PART_Button", Type = typeof(Button))]
	[TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
	public class DatePicker : Control
	{
		/// <summary>Occurs when the drop-down <see cref="T:System.Windows.Controls.Calendar" /> is closed.</summary>
		// Token: 0x140000CB RID: 203
		// (add) Token: 0x060049CF RID: 18895 RVA: 0x0014DC14 File Offset: 0x0014BE14
		// (remove) Token: 0x060049D0 RID: 18896 RVA: 0x0014DC4C File Offset: 0x0014BE4C
		public event RoutedEventHandler CalendarClosed;

		/// <summary>Occurs when the drop-down <see cref="T:System.Windows.Controls.Calendar" /> is opened.</summary>
		// Token: 0x140000CC RID: 204
		// (add) Token: 0x060049D1 RID: 18897 RVA: 0x0014DC84 File Offset: 0x0014BE84
		// (remove) Token: 0x060049D2 RID: 18898 RVA: 0x0014DCBC File Offset: 0x0014BEBC
		public event RoutedEventHandler CalendarOpened;

		/// <summary>Occurs when <see cref="P:System.Windows.Controls.DatePicker.Text" /> is set to a value that cannot be interpreted as a date or when the date cannot be selected.</summary>
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x060049D3 RID: 18899 RVA: 0x0014DCF4 File Offset: 0x0014BEF4
		// (remove) Token: 0x060049D4 RID: 18900 RVA: 0x0014DD2C File Offset: 0x0014BF2C
		public event EventHandler<DatePickerDateValidationErrorEventArgs> DateValidationError;

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.DatePicker.SelectedDate" /> property is changed.</summary>
		// Token: 0x140000CE RID: 206
		// (add) Token: 0x060049D5 RID: 18901 RVA: 0x0014DD61 File Offset: 0x0014BF61
		// (remove) Token: 0x060049D6 RID: 18902 RVA: 0x0014DD6F File Offset: 0x0014BF6F
		public event EventHandler<SelectionChangedEventArgs> SelectedDateChanged
		{
			add
			{
				base.AddHandler(DatePicker.SelectedDateChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(DatePicker.SelectedDateChangedEvent, value);
			}
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x0014DD80 File Offset: 0x0014BF80
		static DatePicker()
		{
			DatePicker.SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Direct, typeof(EventHandler<SelectionChangedEventArgs>), typeof(DatePicker));
			DatePicker.CalendarStyleProperty = DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(DatePicker));
			DatePicker.DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(DatePicker), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, new CoerceValueCallback(DatePicker.CoerceDisplayDate)));
			DatePicker.DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnDisplayDateEndChanged), new CoerceValueCallback(DatePicker.CoerceDisplayDateEnd)));
			DatePicker.DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnDisplayDateStartChanged), new CoerceValueCallback(DatePicker.CoerceDisplayDateStart)));
			DatePicker.FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(DatePicker), null, new ValidateValueCallback(Calendar.IsValidFirstDayOfWeek));
			DatePicker.IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(DatePicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnIsDropDownOpenChanged), new CoerceValueCallback(DatePicker.OnCoerceIsDropDownOpen)));
			DatePicker.IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(DatePicker));
			DatePicker.SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(DatePicker.OnSelectedDateChanged), new CoerceValueCallback(DatePicker.CoerceSelectedDate)));
			DatePicker.SelectedDateFormatProperty = DependencyProperty.Register("SelectedDateFormat", typeof(DatePickerFormat), typeof(DatePicker), new FrameworkPropertyMetadata(DatePickerFormat.Long, new PropertyChangedCallback(DatePicker.OnSelectedDateFormatChanged)), new ValidateValueCallback(DatePicker.IsValidSelectedDateFormat));
			DatePicker.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DatePicker), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(DatePicker.OnTextChanged)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(typeof(DatePicker)));
			EventManager.RegisterClassHandler(typeof(DatePicker), UIElement.GotFocusEvent, new RoutedEventHandler(DatePicker.OnGotFocus));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DatePicker), new FrameworkPropertyMetadata(false));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(DatePicker), new UIPropertyMetadata(new PropertyChangedCallback(DatePicker.OnIsEnabledChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.DatePicker);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DatePicker" /> class. </summary>
		// Token: 0x060049D8 RID: 18904 RVA: 0x0014E0B8 File Offset: 0x0014C2B8
		public DatePicker()
		{
			this.InitializeCalendar();
			this._defaultText = string.Empty;
			base.SetCurrentValueInternal(DatePicker.FirstDayOfWeekProperty, DateTimeHelper.GetCurrentDateFormat().FirstDayOfWeek);
			base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, DateTime.Today);
		}

		/// <summary>Gets or sets a collection of dates that are marked as not selectable.</summary>
		/// <returns>A collection of dates that cannot be selected. The default value is an empty collection.</returns>
		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x0014E10B File Offset: 0x0014C30B
		public CalendarBlackoutDatesCollection BlackoutDates
		{
			get
			{
				return this._calendar.BlackoutDates;
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the calendar.</summary>
		/// <returns>The style that is used when rendering the calendar.</returns>
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x0014E118 File Offset: 0x0014C318
		// (set) Token: 0x060049DB RID: 18907 RVA: 0x0014E12A File Offset: 0x0014C32A
		public Style CalendarStyle
		{
			get
			{
				return (Style)base.GetValue(DatePicker.CalendarStyleProperty);
			}
			set
			{
				base.SetValue(DatePicker.CalendarStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the date to display.</summary>
		/// <returns>The date to display. The default is <see cref="P:System.DateTime.Today" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified date is not in the range defined by <see cref="P:System.Windows.Controls.DatePicker.DisplayDateStart" />. and <see cref="P:System.Windows.Controls.DatePicker.DisplayDateEnd" />.</exception>
		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x060049DC RID: 18908 RVA: 0x0014E138 File Offset: 0x0014C338
		// (set) Token: 0x060049DD RID: 18909 RVA: 0x0014E14A File Offset: 0x0014C34A
		public DateTime DisplayDate
		{
			get
			{
				return (DateTime)base.GetValue(DatePicker.DisplayDateProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateProperty, value);
			}
		}

		// Token: 0x060049DE RID: 18910 RVA: 0x0014E160 File Offset: 0x0014C360
		private static object CoerceDisplayDate(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDate = (DateTime)value;
			return datePicker._calendar.DisplayDate;
		}

		/// <summary>Gets or sets the last date to be displayed.</summary>
		/// <returns>The last date to display.</returns>
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x0014E195 File Offset: 0x0014C395
		// (set) Token: 0x060049E0 RID: 18912 RVA: 0x0014E1A7 File Offset: 0x0014C3A7
		public DateTime? DisplayDateEnd
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.DisplayDateEndProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateEndProperty, value);
			}
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x0014E1BC File Offset: 0x0014C3BC
		private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker.CoerceValue(DatePicker.DisplayDateProperty);
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x0014E1DC File Offset: 0x0014C3DC
		private static object CoerceDisplayDateEnd(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDateEnd = (DateTime?)value;
			return datePicker._calendar.DisplayDateEnd;
		}

		/// <summary>Gets or sets the first date to be displayed.</summary>
		/// <returns>The first date to display.</returns>
		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x0014E211 File Offset: 0x0014C411
		// (set) Token: 0x060049E4 RID: 18916 RVA: 0x0014E223 File Offset: 0x0014C423
		public DateTime? DisplayDateStart
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.DisplayDateStartProperty);
			}
			set
			{
				base.SetValue(DatePicker.DisplayDateStartProperty, value);
			}
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x0014E238 File Offset: 0x0014C438
		private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker.CoerceValue(DatePicker.DisplayDateEndProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateProperty);
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x0014E264 File Offset: 0x0014C464
		private static object CoerceDisplayDateStart(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.DisplayDateStart = (DateTime?)value;
			return datePicker._calendar.DisplayDateStart;
		}

		/// <summary>Gets or sets the day that is considered the beginning of the week.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> that represents the beginning of the week. The default is the <see cref="P:System.Globalization.DateTimeFormatInfo.FirstDayOfWeek" /> that is determined by the current culture.</returns>
		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x0014E299 File Offset: 0x0014C499
		// (set) Token: 0x060049E8 RID: 18920 RVA: 0x0014E2AB File Offset: 0x0014C4AB
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)base.GetValue(DatePicker.FirstDayOfWeekProperty);
			}
			set
			{
				base.SetValue(DatePicker.FirstDayOfWeekProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the drop-down <see cref="T:System.Windows.Controls.Calendar" /> is open or closed.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Calendar" /> is open; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x0014E2BE File Offset: 0x0014C4BE
		// (set) Token: 0x060049EA RID: 18922 RVA: 0x0014E2D0 File Offset: 0x0014C4D0
		public bool IsDropDownOpen
		{
			get
			{
				return (bool)base.GetValue(DatePicker.IsDropDownOpenProperty);
			}
			set
			{
				base.SetValue(DatePicker.IsDropDownOpenProperty, value);
			}
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x0014E2E0 File Offset: 0x0014C4E0
		private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
		{
			DatePicker datePicker = d as DatePicker;
			if (!datePicker.IsEnabled)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x0014E304 File Offset: 0x0014C504
		private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker dp = d as DatePicker;
			bool flag = (bool)e.NewValue;
			if (dp._popUp != null && dp._popUp.IsOpen != flag)
			{
				dp._popUp.IsOpen = flag;
				if (flag)
				{
					dp._originalSelectedDate = dp.SelectedDate;
					dp.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate()
					{
						dp._calendar.Focus();
					}));
				}
			}
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x0014E398 File Offset: 0x0014C598
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker.CoerceValue(DatePicker.IsDropDownOpenProperty);
			Control.OnVisualStatePropertyChanged(d, e);
		}

		/// <summary>Gets or sets a value that indicates whether the current date will be highlighted.</summary>
		/// <returns>
		///     <see langword="true" /> if the current date is highlighted; otherwise, <see langword="false" />. The default is <see langword="true" />. </returns>
		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x0014E3BE File Offset: 0x0014C5BE
		// (set) Token: 0x060049EF RID: 18927 RVA: 0x0014E3D0 File Offset: 0x0014C5D0
		public bool IsTodayHighlighted
		{
			get
			{
				return (bool)base.GetValue(DatePicker.IsTodayHighlightedProperty);
			}
			set
			{
				base.SetValue(DatePicker.IsTodayHighlightedProperty, value);
			}
		}

		/// <summary>Gets or sets the currently selected date.</summary>
		/// <returns>The date currently selected. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified date is not in the range defined by <see cref="P:System.Windows.Controls.DatePicker.DisplayDateStart" /> and <see cref="P:System.Windows.Controls.DatePicker.DisplayDateEnd" />, or the specified date is in the <see cref="P:System.Windows.Controls.DatePicker.BlackoutDates" /> collection. </exception>
		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x060049F0 RID: 18928 RVA: 0x0014E3DE File Offset: 0x0014C5DE
		// (set) Token: 0x060049F1 RID: 18929 RVA: 0x0014E3F0 File Offset: 0x0014C5F0
		public DateTime? SelectedDate
		{
			get
			{
				return (DateTime?)base.GetValue(DatePicker.SelectedDateProperty);
			}
			set
			{
				base.SetValue(DatePicker.SelectedDateProperty, value);
			}
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x0014E404 File Offset: 0x0014C604
		private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			Collection<DateTime> collection = new Collection<DateTime>();
			Collection<DateTime> collection2 = new Collection<DateTime>();
			datePicker.CoerceValue(DatePicker.DisplayDateStartProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateEndProperty);
			datePicker.CoerceValue(DatePicker.DisplayDateProperty);
			DateTime? dateTime = (DateTime?)e.NewValue;
			DateTime? dateTime2 = (DateTime?)e.OldValue;
			if (datePicker.SelectedDate != null)
			{
				DateTime value = datePicker.SelectedDate.Value;
				datePicker.SetTextInternal(datePicker.DateTimeToString(value));
				if ((value.Month != datePicker.DisplayDate.Month || value.Year != datePicker.DisplayDate.Year) && !datePicker._calendar.DatePickerDisplayDateFlag)
				{
					datePicker.SetCurrentValueInternal(DatePicker.DisplayDateProperty, value);
				}
				datePicker._calendar.DatePickerDisplayDateFlag = false;
			}
			else
			{
				datePicker.SetWaterMarkText();
			}
			if (dateTime != null)
			{
				collection.Add(dateTime.Value);
			}
			if (dateTime2 != null)
			{
				collection2.Add(dateTime2.Value);
			}
			datePicker.OnSelectedDateChanged(new CalendarSelectionChangedEventArgs(DatePicker.SelectedDateChangedEvent, collection2, collection));
			DatePickerAutomationPeer datePickerAutomationPeer = UIElementAutomationPeer.FromElement(datePicker) as DatePickerAutomationPeer;
			if (datePickerAutomationPeer != null)
			{
				string newValue = (dateTime != null) ? datePicker.DateTimeToString(dateTime.Value) : "";
				string oldValue = (dateTime2 != null) ? datePicker.DateTimeToString(dateTime2.Value) : "";
				datePickerAutomationPeer.RaiseValuePropertyChangedEvent(oldValue, newValue);
			}
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x0014E58C File Offset: 0x0014C78C
		private static object CoerceSelectedDate(DependencyObject d, object value)
		{
			DatePicker datePicker = d as DatePicker;
			datePicker._calendar.SelectedDate = (DateTime?)value;
			return datePicker._calendar.SelectedDate;
		}

		/// <summary>Gets or sets the format that is used to display the selected date.</summary>
		/// <returns>The format that is used to display the selected date. The default is <see cref="F:System.Windows.Controls.DatePickerFormat.Long" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified format is not valid.</exception>
		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x0014E5C1 File Offset: 0x0014C7C1
		// (set) Token: 0x060049F5 RID: 18933 RVA: 0x0014E5D3 File Offset: 0x0014C7D3
		public DatePickerFormat SelectedDateFormat
		{
			get
			{
				return (DatePickerFormat)base.GetValue(DatePicker.SelectedDateFormatProperty);
			}
			set
			{
				base.SetValue(DatePicker.SelectedDateFormatProperty, value);
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x0014E5E8 File Offset: 0x0014C7E8
		private static void OnSelectedDateFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			if (datePicker._textBox != null)
			{
				if (string.IsNullOrEmpty(datePicker._textBox.Text))
				{
					datePicker.SetWaterMarkText();
					return;
				}
				DateTime? dateTime = datePicker.ParseText(datePicker._textBox.Text);
				if (dateTime != null)
				{
					datePicker.SetTextInternal(datePicker.DateTimeToString(dateTime.Value));
				}
			}
		}

		/// <summary>Gets the text that is displayed by the <see cref="T:System.Windows.Controls.DatePicker" />, or sets the selected date.</summary>
		/// <returns>The text displayed by the <see cref="T:System.Windows.Controls.DatePicker" />. The default is an empty string.</returns>
		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x060049F7 RID: 18935 RVA: 0x0014E64B File Offset: 0x0014C84B
		// (set) Token: 0x060049F8 RID: 18936 RVA: 0x0014E65D File Offset: 0x0014C85D
		public string Text
		{
			get
			{
				return (string)base.GetValue(DatePicker.TextProperty);
			}
			set
			{
				base.SetValue(DatePicker.TextProperty, value);
			}
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x0014E66C File Offset: 0x0014C86C
		private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePicker datePicker = d as DatePicker;
			if (!datePicker.IsHandlerSuspended(DatePicker.TextProperty))
			{
				string text = e.NewValue as string;
				if (text != null)
				{
					if (datePicker._textBox != null)
					{
						datePicker._textBox.Text = text;
					}
					else
					{
						datePicker._defaultText = text;
					}
					datePicker.SetSelectedDate();
					return;
				}
				datePicker.SetValueNoCallback(DatePicker.SelectedDateProperty, null);
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0014E6CD File Offset: 0x0014C8CD
		private void SetTextInternal(string value)
		{
			base.SetCurrentValueInternal(DatePicker.TextProperty, value);
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x0014E6DB File Offset: 0x0014C8DB
		internal Calendar Calendar
		{
			get
			{
				return this._calendar;
			}
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x0014E6E3 File Offset: 0x0014C8E3
		internal TextBox TextBox
		{
			get
			{
				return this._textBox;
			}
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.DatePicker" /> control when a new template is applied.</summary>
		// Token: 0x060049FD RID: 18941 RVA: 0x0014E6EC File Offset: 0x0014C8EC
		public override void OnApplyTemplate()
		{
			if (this._popUp != null)
			{
				this._popUp.RemoveHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.PopUp_PreviewMouseLeftButtonDown));
				this._popUp.Opened -= this.PopUp_Opened;
				this._popUp.Closed -= this.PopUp_Closed;
				this._popUp.Child = null;
			}
			if (this._dropDownButton != null)
			{
				this._dropDownButton.Click -= this.DropDownButton_Click;
				this._dropDownButton.RemoveHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.DropDownButton_MouseLeave));
			}
			if (this._textBox != null)
			{
				this._textBox.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.TextBox_KeyDown));
				this._textBox.RemoveHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBox_TextChanged));
				this._textBox.RemoveHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBox_LostFocus));
			}
			base.OnApplyTemplate();
			this._popUp = (base.GetTemplateChild("PART_Popup") as Popup);
			if (this._popUp != null)
			{
				this._popUp.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(this.PopUp_PreviewMouseLeftButtonDown));
				this._popUp.Opened += this.PopUp_Opened;
				this._popUp.Closed += this.PopUp_Closed;
				this._popUp.Child = this._calendar;
				if (this.IsDropDownOpen)
				{
					this._popUp.IsOpen = true;
				}
			}
			this._dropDownButton = (base.GetTemplateChild("PART_Button") as Button);
			if (this._dropDownButton != null)
			{
				this._dropDownButton.Click += this.DropDownButton_Click;
				this._dropDownButton.AddHandler(UIElement.MouseLeaveEvent, new MouseEventHandler(this.DropDownButton_MouseLeave), true);
				if (this._dropDownButton.Content == null)
				{
					this._dropDownButton.Content = SR.Get("DatePicker_DropDownButtonName");
				}
			}
			this._textBox = (base.GetTemplateChild("PART_TextBox") as DatePickerTextBox);
			if (this.SelectedDate == null)
			{
				this.SetWaterMarkText();
			}
			if (this._textBox != null)
			{
				this._textBox.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.TextBox_KeyDown), true);
				this._textBox.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBox_TextChanged), true);
				this._textBox.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBox_LostFocus), true);
				if (this.SelectedDate == null)
				{
					if (!string.IsNullOrEmpty(this._defaultText))
					{
						this._textBox.Text = this._defaultText;
						this.SetSelectedDate();
						return;
					}
				}
				else
				{
					this._textBox.Text = this.DateTimeToString(this.SelectedDate.Value);
				}
			}
		}

		/// <summary>Provides a text representation of the selected date.</summary>
		/// <returns>A text representation of the selected date, or an empty string if <see cref="P:System.Windows.Controls.DatePicker.SelectedDate" /> is <see langword="null" />.</returns>
		// Token: 0x060049FE RID: 18942 RVA: 0x0014E9D8 File Offset: 0x0014CBD8
		public override string ToString()
		{
			if (this.SelectedDate != null)
			{
				return this.SelectedDate.Value.ToString(DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
			}
			return string.Empty;
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x0014EA1C File Offset: 0x0014CC1C
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

		/// <summary>Returns a <see cref="T:System.Windows.Automation.Peers.DatePickerAutomationPeer" /> for use by the automation infrastructure.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.DatePickerAutomationPeer" /> for the <see cref="T:System.Windows.Controls.DatePicker" /> object.</returns>
		// Token: 0x06004A00 RID: 18944 RVA: 0x0014EA59 File Offset: 0x0014CC59
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DatePickerAutomationPeer(this);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DatePicker.CalendarClosed" /> routed event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x06004A01 RID: 18945 RVA: 0x0014EA64 File Offset: 0x0014CC64
		protected virtual void OnCalendarClosed(RoutedEventArgs e)
		{
			RoutedEventHandler calendarClosed = this.CalendarClosed;
			if (calendarClosed != null)
			{
				calendarClosed(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DatePicker.CalendarOpened" /> routed event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x06004A02 RID: 18946 RVA: 0x0014EA84 File Offset: 0x0014CC84
		protected virtual void OnCalendarOpened(RoutedEventArgs e)
		{
			RoutedEventHandler calendarOpened = this.CalendarOpened;
			if (calendarOpened != null)
			{
				calendarOpened(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DatePicker.SelectedDateChanged" /> routed event. </summary>
		/// <param name="e">The data for the event. </param>
		// Token: 0x06004A03 RID: 18947 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnSelectedDateChanged(SelectionChangedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DatePicker.DateValidationError" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Controls.DatePickerDateValidationErrorEventArgs" /> that contains the event data.</param>
		// Token: 0x06004A04 RID: 18948 RVA: 0x0014EAA4 File Offset: 0x0014CCA4
		protected virtual void OnDateValidationError(DatePickerDateValidationErrorEventArgs e)
		{
			EventHandler<DatePickerDateValidationErrorEventArgs> dateValidationError = this.DateValidationError;
			if (dateValidationError != null)
			{
				dateValidationError(this, e);
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DatePicker" /> has focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.DatePicker" /> has focus; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06004A05 RID: 18949 RVA: 0x0014EAC3 File Offset: 0x0014CCC3
		protected internal override bool HasEffectiveKeyboardFocus
		{
			get
			{
				if (this._textBox != null)
				{
					return this._textBox.HasEffectiveKeyboardFocus;
				}
				return base.HasEffectiveKeyboardFocus;
			}
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0014EAE0 File Offset: 0x0014CCE0
		private static void OnGotFocus(object sender, RoutedEventArgs e)
		{
			DatePicker datePicker = (DatePicker)sender;
			if (!e.Handled && datePicker._textBox != null)
			{
				if (e.OriginalSource == datePicker)
				{
					datePicker._textBox.Focus();
					e.Handled = true;
					return;
				}
				if (e.OriginalSource == datePicker._textBox)
				{
					datePicker._textBox.SelectAll();
					e.Handled = true;
				}
			}
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0014EB44 File Offset: 0x0014CD44
		private void SetValueNoCallback(DependencyProperty property, object value)
		{
			this.SetIsHandlerSuspended(property, true);
			try
			{
				base.SetCurrentValue(property, value);
			}
			finally
			{
				this.SetIsHandlerSuspended(property, false);
			}
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0014EB7C File Offset: 0x0014CD7C
		private bool IsHandlerSuspended(DependencyProperty property)
		{
			return this._isHandlerSuspended != null && this._isHandlerSuspended.ContainsKey(property);
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x0014EB94 File Offset: 0x0014CD94
		private void SetIsHandlerSuspended(DependencyProperty property, bool value)
		{
			if (value)
			{
				if (this._isHandlerSuspended == null)
				{
					this._isHandlerSuspended = new Dictionary<DependencyProperty, bool>(2);
				}
				this._isHandlerSuspended[property] = true;
				return;
			}
			if (this._isHandlerSuspended != null)
			{
				this._isHandlerSuspended.Remove(property);
			}
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0014EBD0 File Offset: 0x0014CDD0
		private void PopUp_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Popup popup = sender as Popup;
			if (popup != null && !popup.StaysOpen && this._dropDownButton != null && this._dropDownButton.InputHitTest(e.GetPosition(this._dropDownButton)) != null)
			{
				this._disablePopupReopen = true;
			}
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0014EC18 File Offset: 0x0014CE18
		private void PopUp_Opened(object sender, EventArgs e)
		{
			if (!this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
			}
			if (this._calendar != null)
			{
				this._calendar.DisplayMode = CalendarMode.Month;
				this._calendar.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
			this.OnCalendarOpened(new RoutedEventArgs());
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x0014EC6E File Offset: 0x0014CE6E
		private void PopUp_Closed(object sender, EventArgs e)
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
			}
			if (this._calendar.IsKeyboardFocusWithin)
			{
				this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
			this.OnCalendarClosed(new RoutedEventArgs());
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x0014ECAD File Offset: 0x0014CEAD
		private void Calendar_DayButtonMouseUp(object sender, MouseButtonEventArgs e)
		{
			base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x0014ECC0 File Offset: 0x0014CEC0
		private void CalendarDayOrMonthButton_PreviewKeyDown(object sender, RoutedEventArgs e)
		{
			Calendar calendar = sender as Calendar;
			KeyEventArgs keyEventArgs = (KeyEventArgs)e;
			if (keyEventArgs.Key == Key.Escape || ((keyEventArgs.Key == Key.Return || keyEventArgs.Key == Key.Space) && calendar.DisplayMode == CalendarMode.Month))
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
				if (keyEventArgs.Key == Key.Escape)
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, this._originalSelectedDate);
				}
			}
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x0014ED30 File Offset: 0x0014CF30
		private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
		{
			DateTime? addedDate = e.AddedDate;
			DateTime displayDate = this.DisplayDate;
			if (addedDate == null || (addedDate != null && addedDate.GetValueOrDefault() != displayDate))
			{
				base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, e.AddedDate.Value);
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x0014ED90 File Offset: 0x0014CF90
		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0 && this.SelectedDate != null && DateTime.Compare((DateTime)e.AddedItems[0], this.SelectedDate.Value) != 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, (DateTime?)e.AddedItems[0]);
				return;
			}
			if (e.AddedItems.Count == 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, null);
				return;
			}
			if (this.SelectedDate == null && e.AddedItems.Count > 0)
			{
				base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, (DateTime?)e.AddedItems[0]);
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x0014EE5C File Offset: 0x0014D05C
		private string DateTimeToString(DateTime d)
		{
			DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this));
			DatePickerFormat selectedDateFormat = this.SelectedDateFormat;
			if (selectedDateFormat == DatePickerFormat.Long)
			{
				return string.Format(CultureInfo.CurrentCulture, d.ToString(dateFormat.LongDatePattern, dateFormat), new object[0]);
			}
			if (selectedDateFormat == DatePickerFormat.Short)
			{
				return string.Format(CultureInfo.CurrentCulture, d.ToString(dateFormat.ShortDatePattern, dateFormat), new object[0]);
			}
			return null;
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x0014EEC4 File Offset: 0x0014D0C4
		private static DateTime DiscardDayTime(DateTime d)
		{
			int year = d.Year;
			int month = d.Month;
			DateTime result = new DateTime(year, month, 1, 0, 0, 0);
			return result;
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0014EEF0 File Offset: 0x0014D0F0
		private static DateTime? DiscardTime(DateTime? d)
		{
			if (d == null)
			{
				return null;
			}
			DateTime value = d.Value;
			int year = value.Year;
			int month = value.Month;
			int day = value.Day;
			DateTime value2 = new DateTime(year, month, day, 0, 0, 0);
			return new DateTime?(value2);
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0014EF46 File Offset: 0x0014D146
		private void DropDownButton_Click(object sender, RoutedEventArgs e)
		{
			this.TogglePopUp();
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0014EF4E File Offset: 0x0014D14E
		private void DropDownButton_MouseLeave(object sender, MouseEventArgs e)
		{
			this._disablePopupReopen = false;
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x0014EF58 File Offset: 0x0014D158
		private void TogglePopUp()
		{
			if (this.IsDropDownOpen)
			{
				base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.FalseBox);
				return;
			}
			if (this._disablePopupReopen)
			{
				this._disablePopupReopen = false;
				return;
			}
			this.SetSelectedDate();
			base.SetCurrentValueInternal(DatePicker.IsDropDownOpenProperty, BooleanBoxes.TrueBox);
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0014EFA4 File Offset: 0x0014D1A4
		private void InitializeCalendar()
		{
			this._calendar = new Calendar();
			this._calendar.DayButtonMouseUp += this.Calendar_DayButtonMouseUp;
			this._calendar.DisplayDateChanged += this.Calendar_DisplayDateChanged;
			this._calendar.SelectedDatesChanged += this.Calendar_SelectedDatesChanged;
			this._calendar.DayOrMonthPreviewKeyDown += this.CalendarDayOrMonthButton_PreviewKeyDown;
			this._calendar.HorizontalAlignment = HorizontalAlignment.Left;
			this._calendar.VerticalAlignment = VerticalAlignment.Top;
			this._calendar.SelectionMode = CalendarSelectionMode.SingleDate;
			this._calendar.SetBinding(Control.ForegroundProperty, this.GetDatePickerBinding(Control.ForegroundProperty));
			this._calendar.SetBinding(FrameworkElement.StyleProperty, this.GetDatePickerBinding(DatePicker.CalendarStyleProperty));
			this._calendar.SetBinding(Calendar.IsTodayHighlightedProperty, this.GetDatePickerBinding(DatePicker.IsTodayHighlightedProperty));
			this._calendar.SetBinding(Calendar.FirstDayOfWeekProperty, this.GetDatePickerBinding(DatePicker.FirstDayOfWeekProperty));
			this._calendar.SetBinding(FrameworkElement.FlowDirectionProperty, this.GetDatePickerBinding(FrameworkElement.FlowDirectionProperty));
			RenderOptions.SetClearTypeHint(this._calendar, ClearTypeHint.Enabled);
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x0014F0D4 File Offset: 0x0014D2D4
		private BindingBase GetDatePickerBinding(DependencyProperty property)
		{
			return new Binding(property.Name)
			{
				Source = this
			};
		}

		// Token: 0x06004A19 RID: 18969 RVA: 0x0014F0F8 File Offset: 0x0014D2F8
		private static bool IsValidSelectedDateFormat(object value)
		{
			DatePickerFormat datePickerFormat = (DatePickerFormat)value;
			return datePickerFormat == DatePickerFormat.Long || datePickerFormat == DatePickerFormat.Short;
		}

		// Token: 0x06004A1A RID: 18970 RVA: 0x0014F118 File Offset: 0x0014D318
		private DateTime? ParseText(string text)
		{
			try
			{
				DateTime dateTime = DateTime.Parse(text, DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)));
				if (Calendar.IsValidDateSelection(this._calendar, dateTime))
				{
					return new DateTime?(dateTime);
				}
				DatePickerDateValidationErrorEventArgs datePickerDateValidationErrorEventArgs = new DatePickerDateValidationErrorEventArgs(new ArgumentOutOfRangeException("text", SR.Get("Calendar_OnSelectedDateChanged_InvalidValue")), text);
				this.OnDateValidationError(datePickerDateValidationErrorEventArgs);
				if (datePickerDateValidationErrorEventArgs.ThrowException)
				{
					throw datePickerDateValidationErrorEventArgs.Exception;
				}
			}
			catch (FormatException exception)
			{
				DatePickerDateValidationErrorEventArgs datePickerDateValidationErrorEventArgs2 = new DatePickerDateValidationErrorEventArgs(exception, text);
				this.OnDateValidationError(datePickerDateValidationErrorEventArgs2);
				if (datePickerDateValidationErrorEventArgs2.ThrowException && datePickerDateValidationErrorEventArgs2.Exception != null)
				{
					throw datePickerDateValidationErrorEventArgs2.Exception;
				}
			}
			return null;
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x0014F1D0 File Offset: 0x0014D3D0
		private bool ProcessDatePickerKey(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != Key.Return)
			{
				if (key == Key.System)
				{
					key = e.SystemKey;
					if (key == Key.Down && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
					{
						this.TogglePopUp();
						return true;
					}
				}
				return false;
			}
			this.SetSelectedDate();
			return true;
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0014F218 File Offset: 0x0014D418
		private void SetSelectedDate()
		{
			if (this._textBox != null)
			{
				if (!string.IsNullOrEmpty(this._textBox.Text))
				{
					string text = this._textBox.Text;
					if (this.SelectedDate != null)
					{
						string strA = this.DateTimeToString(this.SelectedDate.Value);
						if (string.Compare(strA, text, StringComparison.Ordinal) == 0)
						{
							return;
						}
					}
					DateTime? dateTime = this.SetTextBoxValue(text);
					if (!this.SelectedDate.Equals(dateTime))
					{
						base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, dateTime);
						base.SetCurrentValueInternal(DatePicker.DisplayDateProperty, dateTime);
						return;
					}
				}
				else if (this.SelectedDate != null)
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, null);
					return;
				}
			}
			else
			{
				DateTime? dateTime2 = this.SetTextBoxValue(this._defaultText);
				if (!this.SelectedDate.Equals(dateTime2))
				{
					base.SetCurrentValueInternal(DatePicker.SelectedDateProperty, dateTime2);
				}
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x0014F322 File Offset: 0x0014D522
		private void SafeSetText(string s)
		{
			if (string.Compare(this.Text, s, StringComparison.Ordinal) != 0)
			{
				base.SetCurrentValueInternal(DatePicker.TextProperty, s);
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x0014F340 File Offset: 0x0014D540
		private DateTime? SetTextBoxValue(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				this.SafeSetText(s);
				return this.SelectedDate;
			}
			DateTime? result = this.ParseText(s);
			if (result != null)
			{
				this.SafeSetText(this.DateTimeToString(result.Value));
				return result;
			}
			if (this.SelectedDate != null)
			{
				string s2 = this.DateTimeToString(this.SelectedDate.Value);
				this.SafeSetText(s2);
				return this.SelectedDate;
			}
			this.SetWaterMarkText();
			return null;
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x0014F3CC File Offset: 0x0014D5CC
		private void SetWaterMarkText()
		{
			if (this._textBox != null)
			{
				DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this));
				this.SetTextInternal(string.Empty);
				this._defaultText = string.Empty;
				DatePickerFormat selectedDateFormat = this.SelectedDateFormat;
				if (selectedDateFormat == DatePickerFormat.Long)
				{
					this._textBox.Watermark = string.Format(CultureInfo.CurrentCulture, SR.Get("DatePicker_WatermarkText"), new object[]
					{
						dateFormat.LongDatePattern.ToString()
					});
					return;
				}
				if (selectedDateFormat != DatePickerFormat.Short)
				{
					return;
				}
				this._textBox.Watermark = string.Format(CultureInfo.CurrentCulture, SR.Get("DatePicker_WatermarkText"), new object[]
				{
					dateFormat.ShortDatePattern.ToString()
				});
			}
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x0014F47C File Offset: 0x0014D67C
		private void TextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			this.SetSelectedDate();
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x0014F484 File Offset: 0x0014D684
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = (this.ProcessDatePickerKey(e) || e.Handled);
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x0014F49E File Offset: 0x0014D69E
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.SetValueNoCallback(DatePicker.TextProperty, this._textBox.Text);
		}

		// Token: 0x04002A30 RID: 10800
		private const string ElementRoot = "PART_Root";

		// Token: 0x04002A31 RID: 10801
		private const string ElementTextBox = "PART_TextBox";

		// Token: 0x04002A32 RID: 10802
		private const string ElementButton = "PART_Button";

		// Token: 0x04002A33 RID: 10803
		private const string ElementPopup = "PART_Popup";

		// Token: 0x04002A34 RID: 10804
		private Calendar _calendar;

		// Token: 0x04002A35 RID: 10805
		private string _defaultText;

		// Token: 0x04002A36 RID: 10806
		private ButtonBase _dropDownButton;

		// Token: 0x04002A37 RID: 10807
		private Popup _popUp;

		// Token: 0x04002A38 RID: 10808
		private bool _disablePopupReopen;

		// Token: 0x04002A39 RID: 10809
		private DatePickerTextBox _textBox;

		// Token: 0x04002A3A RID: 10810
		private IDictionary<DependencyProperty, bool> _isHandlerSuspended;

		// Token: 0x04002A3B RID: 10811
		private DateTime? _originalSelectedDate;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.CalendarStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.CalendarStyle" /> dependency property.</returns>
		// Token: 0x04002A40 RID: 10816
		public static readonly DependencyProperty CalendarStyleProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.DisplayDate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.DisplayDate" /> dependency property.</returns>
		// Token: 0x04002A41 RID: 10817
		public static readonly DependencyProperty DisplayDateProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.DisplayDateEnd" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.DisplayDateEnd" /> dependency property.</returns>
		// Token: 0x04002A42 RID: 10818
		public static readonly DependencyProperty DisplayDateEndProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.DisplayDateStart" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.DisplayDateStart" /> dependency property.</returns>
		// Token: 0x04002A43 RID: 10819
		public static readonly DependencyProperty DisplayDateStartProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.FirstDayOfWeek" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.FirstDayOfWeek" /> dependency property.</returns>
		// Token: 0x04002A44 RID: 10820
		public static readonly DependencyProperty FirstDayOfWeekProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.IsDropDownOpen" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.IsDropDownOpen" /> dependency property.</returns>
		// Token: 0x04002A45 RID: 10821
		public static readonly DependencyProperty IsDropDownOpenProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.IsTodayHighlighted" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.IsTodayHighlighted" /> dependency property.</returns>
		// Token: 0x04002A46 RID: 10822
		public static readonly DependencyProperty IsTodayHighlightedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.SelectedDate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.SelectedDate" /> dependency property.</returns>
		// Token: 0x04002A47 RID: 10823
		public static readonly DependencyProperty SelectedDateProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.SelectedDateFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.SelectedDateFormat" /> dependency property.</returns>
		// Token: 0x04002A48 RID: 10824
		public static readonly DependencyProperty SelectedDateFormatProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DatePicker.Text" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DatePicker.Text" /> dependency property.</returns>
		// Token: 0x04002A49 RID: 10825
		public static readonly DependencyProperty TextProperty;
	}
}
