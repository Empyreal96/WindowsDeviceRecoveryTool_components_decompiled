using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents the currently displayed month or year on a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x02000579 RID: 1401
	[TemplatePart(Name = "PART_Root", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_HeaderButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_PreviousButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_NextButton", Type = typeof(Button))]
	[TemplatePart(Name = "DayTitleTemplate", Type = typeof(DataTemplate))]
	[TemplatePart(Name = "PART_MonthView", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_YearView", Type = typeof(Grid))]
	[TemplatePart(Name = "PART_DisabledVisual", Type = typeof(FrameworkElement))]
	public sealed class CalendarItem : Control
	{
		// Token: 0x06005C4B RID: 23627 RVA: 0x0019EE5C File Offset: 0x0019D05C
		static CalendarItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(typeof(CalendarItem)));
			UIElement.FocusableProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(false));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(CalendarItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(CalendarItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06005C4D RID: 23629 RVA: 0x0019EF21 File Offset: 0x0019D121
		internal Grid MonthView
		{
			get
			{
				return this._monthView;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x0019EF29 File Offset: 0x0019D129
		// (set) Token: 0x06005C4F RID: 23631 RVA: 0x0019EF31 File Offset: 0x0019D131
		internal Calendar Owner { get; set; }

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06005C50 RID: 23632 RVA: 0x0019EF3A File Offset: 0x0019D13A
		internal Grid YearView
		{
			get
			{
				return this._yearView;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06005C51 RID: 23633 RVA: 0x0019EF42 File Offset: 0x0019D142
		private CalendarMode DisplayMode
		{
			get
			{
				if (this.Owner == null)
				{
					return CalendarMode.Month;
				}
				return this.Owner.DisplayMode;
			}
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x06005C52 RID: 23634 RVA: 0x0019EF59 File Offset: 0x0019D159
		internal Button HeaderButton
		{
			get
			{
				return this._headerButton;
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x06005C53 RID: 23635 RVA: 0x0019EF61 File Offset: 0x0019D161
		internal Button NextButton
		{
			get
			{
				return this._nextButton;
			}
		}

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x06005C54 RID: 23636 RVA: 0x0019EF69 File Offset: 0x0019D169
		internal Button PreviousButton
		{
			get
			{
				return this._previousButton;
			}
		}

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x06005C55 RID: 23637 RVA: 0x0019EF71 File Offset: 0x0019D171
		private DateTime DisplayDate
		{
			get
			{
				if (this.Owner == null)
				{
					return DateTime.Today;
				}
				return this.Owner.DisplayDate;
			}
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.Primitives.CalendarItem" /> when a new template is applied.</summary>
		// Token: 0x06005C56 RID: 23638 RVA: 0x0019EF8C File Offset: 0x0019D18C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (this._previousButton != null)
			{
				this._previousButton.Click -= this.PreviousButton_Click;
			}
			if (this._nextButton != null)
			{
				this._nextButton.Click -= this.NextButton_Click;
			}
			if (this._headerButton != null)
			{
				this._headerButton.Click -= this.HeaderButton_Click;
			}
			this._monthView = (base.GetTemplateChild("PART_MonthView") as Grid);
			this._yearView = (base.GetTemplateChild("PART_YearView") as Grid);
			this._previousButton = (base.GetTemplateChild("PART_PreviousButton") as Button);
			this._nextButton = (base.GetTemplateChild("PART_NextButton") as Button);
			this._headerButton = (base.GetTemplateChild("PART_HeaderButton") as Button);
			this._disabledVisual = (base.GetTemplateChild("PART_DisabledVisual") as FrameworkElement);
			this._dayTitleTemplate = null;
			if (base.Template != null && base.Template.Resources.Contains(CalendarItem.DayTitleTemplateResourceKey))
			{
				this._dayTitleTemplate = (base.Template.Resources[CalendarItem.DayTitleTemplateResourceKey] as DataTemplate);
			}
			if (this._previousButton != null)
			{
				if (this._previousButton.Content == null)
				{
					this._previousButton.Content = SR.Get("Calendar_PreviousButtonName");
				}
				this._previousButton.Click += this.PreviousButton_Click;
			}
			if (this._nextButton != null)
			{
				if (this._nextButton.Content == null)
				{
					this._nextButton.Content = SR.Get("Calendar_NextButtonName");
				}
				this._nextButton.Click += this.NextButton_Click;
			}
			if (this._headerButton != null)
			{
				this._headerButton.Click += this.HeaderButton_Click;
			}
			this.PopulateGrids();
			if (this.Owner == null)
			{
				this.UpdateMonthMode();
				return;
			}
			switch (this.Owner.DisplayMode)
			{
			case CalendarMode.Month:
				this.UpdateMonthMode();
				return;
			case CalendarMode.Year:
				this.UpdateYearMode();
				return;
			case CalendarMode.Decade:
				this.UpdateDecadeMode();
				return;
			default:
				return;
			}
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x0019F1AF File Offset: 0x0019D3AF
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x0019F1DC File Offset: 0x0019D3DC
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);
			if (base.IsMouseCaptured)
			{
				base.ReleaseMouseCapture();
			}
			this._isMonthPressed = false;
			this._isDayPressed = false;
			if (!e.Handled && this.Owner.DisplayMode == CalendarMode.Month && this.Owner.HoverEnd != null)
			{
				this.FinishSelection(this.Owner.HoverEnd.Value);
			}
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x0019F24F File Offset: 0x0019D44F
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (!base.IsMouseCaptured)
			{
				this._isDayPressed = false;
				this._isMonthPressed = false;
			}
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x0019F270 File Offset: 0x0019D470
		internal void UpdateDecadeMode()
		{
			DateTime selectedYear;
			if (this.Owner != null)
			{
				selectedYear = this.Owner.DisplayYear;
			}
			else
			{
				selectedYear = DateTime.Today;
			}
			int decadeForDecadeMode = this.GetDecadeForDecadeMode(selectedYear);
			int num = decadeForDecadeMode + 9;
			this.SetDecadeModeHeaderButton(decadeForDecadeMode);
			this.SetDecadeModePreviousButton(decadeForDecadeMode);
			this.SetDecadeModeNextButton(num);
			if (this._yearView != null)
			{
				this.SetYearButtons(decadeForDecadeMode, num);
			}
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x0019F2CB File Offset: 0x0019D4CB
		internal void UpdateMonthMode()
		{
			this.SetMonthModeHeaderButton();
			this.SetMonthModePreviousButton();
			this.SetMonthModeNextButton();
			if (this._monthView != null)
			{
				this.SetMonthModeDayTitles();
				this.SetMonthModeCalendarDayButtons();
				this.AddMonthModeHighlight();
			}
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x0019F2F9 File Offset: 0x0019D4F9
		internal void UpdateYearMode()
		{
			this.SetYearModeHeaderButton();
			this.SetYearModePreviousButton();
			this.SetYearModeNextButton();
			if (this._yearView != null)
			{
				this.SetYearModeMonthButtons();
			}
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x0019F31B File Offset: 0x0019D51B
		internal IEnumerable<CalendarDayButton> GetCalendarDayButtons()
		{
			int count = 49;
			if (this.MonthView != null)
			{
				UIElementCollection dayButtonsHost = this.MonthView.Children;
				int num;
				for (int childIndex = 7; childIndex < count; childIndex = num + 1)
				{
					CalendarDayButton calendarDayButton = dayButtonsHost[childIndex] as CalendarDayButton;
					if (calendarDayButton != null)
					{
						yield return calendarDayButton;
					}
					num = childIndex;
				}
				dayButtonsHost = null;
			}
			yield break;
		}

		// Token: 0x06005C5E RID: 23646 RVA: 0x0019F32C File Offset: 0x0019D52C
		internal CalendarDayButton GetFocusedCalendarDayButton()
		{
			foreach (CalendarDayButton calendarDayButton in this.GetCalendarDayButtons())
			{
				if (calendarDayButton != null && calendarDayButton.IsFocused)
				{
					return calendarDayButton;
				}
			}
			return null;
		}

		// Token: 0x06005C5F RID: 23647 RVA: 0x0019F384 File Offset: 0x0019D584
		internal CalendarDayButton GetCalendarDayButton(DateTime date)
		{
			foreach (CalendarDayButton calendarDayButton in this.GetCalendarDayButtons())
			{
				if (calendarDayButton != null && calendarDayButton.DataContext is DateTime && DateTimeHelper.CompareDays(date, (DateTime)calendarDayButton.DataContext) == 0)
				{
					return calendarDayButton;
				}
			}
			return null;
		}

		// Token: 0x06005C60 RID: 23648 RVA: 0x0019F3F4 File Offset: 0x0019D5F4
		internal CalendarButton GetCalendarButton(DateTime date, CalendarMode mode)
		{
			foreach (CalendarButton calendarButton in this.GetCalendarButtons())
			{
				if (calendarButton != null && calendarButton.DataContext is DateTime)
				{
					if (mode == CalendarMode.Year)
					{
						if (DateTimeHelper.CompareYearMonth(date, (DateTime)calendarButton.DataContext) == 0)
						{
							return calendarButton;
						}
					}
					else if (date.Year == ((DateTime)calendarButton.DataContext).Year)
					{
						return calendarButton;
					}
				}
			}
			return null;
		}

		// Token: 0x06005C61 RID: 23649 RVA: 0x0019F488 File Offset: 0x0019D688
		internal CalendarButton GetFocusedCalendarButton()
		{
			foreach (CalendarButton calendarButton in this.GetCalendarButtons())
			{
				if (calendarButton != null && calendarButton.IsFocused)
				{
					return calendarButton;
				}
			}
			return null;
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x0019F4E0 File Offset: 0x0019D6E0
		private IEnumerable<CalendarButton> GetCalendarButtons()
		{
			foreach (object obj in this.YearView.Children)
			{
				UIElement uielement = (UIElement)obj;
				CalendarButton calendarButton = uielement as CalendarButton;
				if (calendarButton != null)
				{
					yield return calendarButton;
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x0019F4F0 File Offset: 0x0019D6F0
		internal void FocusDate(DateTime date)
		{
			FrameworkElement frameworkElement = null;
			CalendarMode displayMode = this.DisplayMode;
			if (displayMode != CalendarMode.Month)
			{
				if (displayMode - CalendarMode.Year <= 1)
				{
					frameworkElement = this.GetCalendarButton(date, this.DisplayMode);
				}
			}
			else
			{
				frameworkElement = this.GetCalendarDayButton(date);
			}
			if (frameworkElement != null && !frameworkElement.IsFocused)
			{
				frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			}
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x0019F544 File Offset: 0x0019D744
		private int GetDecadeForDecadeMode(DateTime selectedYear)
		{
			int num = DateTimeHelper.DecadeOfDate(selectedYear);
			if (this._isMonthPressed && this._yearView != null)
			{
				UIElementCollection children = this._yearView.Children;
				int count = children.Count;
				if (count > 0)
				{
					CalendarButton calendarButton = children[0] as CalendarButton;
					if (calendarButton != null && calendarButton.DataContext is DateTime && ((DateTime)calendarButton.DataContext).Year == selectedYear.Year)
					{
						return num + 10;
					}
				}
				if (count > 1)
				{
					CalendarButton calendarButton2 = children[count - 1] as CalendarButton;
					if (calendarButton2 != null && calendarButton2.DataContext is DateTime && ((DateTime)calendarButton2.DataContext).Year == selectedYear.Year)
					{
						return num - 10;
					}
				}
			}
			return num;
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x0019F610 File Offset: 0x0019D810
		private void EndDrag(bool ctrl, DateTime selectedDate)
		{
			if (this.Owner != null)
			{
				this.Owner.CurrentDate = selectedDate;
				if (this.Owner.HoverStart != null)
				{
					if (ctrl && DateTime.Compare(this.Owner.HoverStart.Value, selectedDate) == 0 && (this.Owner.SelectionMode == CalendarSelectionMode.SingleDate || this.Owner.SelectionMode == CalendarSelectionMode.MultipleRange))
					{
						this.Owner.SelectedDates.Toggle(selectedDate);
					}
					else
					{
						this.Owner.SelectedDates.AddRangeInternal(this.Owner.HoverStart.Value, selectedDate);
					}
					this.Owner.OnDayClick(selectedDate);
				}
			}
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x0019F6C5 File Offset: 0x0019D8C5
		private void CellOrMonth_PreviewKeyDown(object sender, RoutedEventArgs e)
		{
			if (this.Owner == null)
			{
				return;
			}
			this.Owner.OnDayOrMonthPreviewKeyDown(e);
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x0019F6DC File Offset: 0x0019D8DC
		private void Cell_Clicked(object sender, RoutedEventArgs e)
		{
			if (this.Owner == null)
			{
				return;
			}
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (!(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			if (!calendarDayButton.IsBlackedOut)
			{
				DateTime dateTime = (DateTime)calendarDayButton.DataContext;
				bool flag;
				bool flag2;
				CalendarKeyboardHelper.GetMetaKeyState(out flag, out flag2);
				switch (this.Owner.SelectionMode)
				{
				case CalendarSelectionMode.SingleDate:
					if (!flag)
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
					}
					else
					{
						this.Owner.SelectedDates.Toggle(dateTime);
					}
					break;
				case CalendarSelectionMode.SingleRange:
				{
					DateTime? dateTime2 = new DateTime?(this.Owner.CurrentDate);
					this.Owner.SelectedDates.ClearInternal(true);
					if (flag2 && dateTime2 != null)
					{
						this.Owner.SelectedDates.AddRangeInternal(dateTime2.Value, dateTime);
					}
					else
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
						this.Owner.HoverStart = null;
						this.Owner.HoverEnd = null;
					}
					break;
				}
				case CalendarSelectionMode.MultipleRange:
					if (!flag)
					{
						this.Owner.SelectedDates.ClearInternal(true);
					}
					if (flag2)
					{
						this.Owner.SelectedDates.AddRangeInternal(this.Owner.CurrentDate, dateTime);
					}
					else if (!flag)
					{
						this.Owner.SelectedDate = new DateTime?(dateTime);
					}
					else
					{
						this.Owner.SelectedDates.Toggle(dateTime);
						this.Owner.HoverStart = null;
						this.Owner.HoverEnd = null;
					}
					break;
				}
				this.Owner.OnDayClick(dateTime);
			}
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x0019F8A0 File Offset: 0x0019DAA0
		private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (this.Owner == null || !(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			if (calendarDayButton.IsBlackedOut)
			{
				Calendar owner = this.Owner;
				DateTime? dateTime = null;
				owner.HoverStart = dateTime;
				return;
			}
			this._isDayPressed = true;
			Mouse.Capture(this, CaptureMode.SubTree);
			calendarDayButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
			bool flag;
			bool flag2;
			CalendarKeyboardHelper.GetMetaKeyState(out flag, out flag2);
			DateTime dateTime2 = (DateTime)calendarDayButton.DataContext;
			switch (this.Owner.SelectionMode)
			{
			case CalendarSelectionMode.SingleDate:
				this.Owner.DatePickerDisplayDateFlag = true;
				if (!flag)
				{
					this.Owner.SelectedDate = new DateTime?(dateTime2);
				}
				else
				{
					this.Owner.SelectedDates.Toggle(dateTime2);
				}
				break;
			case CalendarSelectionMode.SingleRange:
				this.Owner.SelectedDates.ClearInternal();
				if (flag2)
				{
					DateTime? dateTime = this.Owner.HoverStart;
					if (dateTime == null)
					{
						Calendar owner2 = this.Owner;
						Calendar owner3 = this.Owner;
						dateTime = new DateTime?(this.Owner.CurrentDate);
						owner3.HoverEnd = dateTime;
						owner2.HoverStart = dateTime;
					}
				}
				else
				{
					Calendar owner4 = this.Owner;
					Calendar owner5 = this.Owner;
					DateTime? dateTime = new DateTime?(dateTime2);
					owner5.HoverEnd = dateTime;
					owner4.HoverStart = dateTime;
				}
				break;
			case CalendarSelectionMode.MultipleRange:
				if (!flag)
				{
					this.Owner.SelectedDates.ClearInternal();
				}
				if (flag2)
				{
					DateTime? dateTime = this.Owner.HoverStart;
					if (dateTime == null)
					{
						Calendar owner6 = this.Owner;
						Calendar owner7 = this.Owner;
						dateTime = new DateTime?(this.Owner.CurrentDate);
						owner7.HoverEnd = dateTime;
						owner6.HoverStart = dateTime;
					}
				}
				else
				{
					Calendar owner8 = this.Owner;
					Calendar owner9 = this.Owner;
					DateTime? dateTime = new DateTime?(dateTime2);
					owner9.HoverEnd = dateTime;
					owner8.HoverStart = dateTime;
				}
				break;
			}
			this.Owner.CurrentDate = dateTime2;
			this.Owner.UpdateCellItems();
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x0019FA90 File Offset: 0x0019DC90
		private void Cell_MouseEnter(object sender, MouseEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (calendarDayButton.IsBlackedOut)
			{
				return;
			}
			if (e.LeftButton == MouseButtonState.Pressed && this._isDayPressed)
			{
				calendarDayButton.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
				if (this.Owner == null || !(calendarDayButton.DataContext is DateTime))
				{
					return;
				}
				DateTime dateTime = (DateTime)calendarDayButton.DataContext;
				if (this.Owner.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.Owner.DatePickerDisplayDateFlag = true;
					Calendar owner = this.Owner;
					Calendar owner2 = this.Owner;
					DateTime? dateTime2 = null;
					owner2.HoverEnd = dateTime2;
					owner.HoverStart = dateTime2;
					if (this.Owner.SelectedDates.Count == 0)
					{
						this.Owner.SelectedDates.Add(dateTime);
						return;
					}
					this.Owner.SelectedDates[0] = dateTime;
					return;
				}
				else
				{
					this.Owner.HoverEnd = new DateTime?(dateTime);
					this.Owner.CurrentDate = dateTime;
					this.Owner.UpdateCellItems();
				}
			}
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x0019FB94 File Offset: 0x0019DD94
		private void Cell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CalendarDayButton calendarDayButton = sender as CalendarDayButton;
			if (calendarDayButton == null)
			{
				return;
			}
			if (this.Owner == null)
			{
				return;
			}
			if (!calendarDayButton.IsBlackedOut)
			{
				this.Owner.OnDayButtonMouseUp(e);
			}
			if (!(calendarDayButton.DataContext is DateTime))
			{
				return;
			}
			this.FinishSelection((DateTime)calendarDayButton.DataContext);
			e.Handled = true;
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x0019FBF0 File Offset: 0x0019DDF0
		private void FinishSelection(DateTime selectedDate)
		{
			bool ctrl;
			bool flag;
			CalendarKeyboardHelper.GetMetaKeyState(out ctrl, out flag);
			if (this.Owner.SelectionMode == CalendarSelectionMode.None || this.Owner.SelectionMode == CalendarSelectionMode.SingleDate)
			{
				this.Owner.OnDayClick(selectedDate);
				return;
			}
			if (this.Owner.HoverStart == null)
			{
				CalendarDayButton calendarDayButton = this.GetCalendarDayButton(selectedDate);
				if (calendarDayButton != null && calendarDayButton.IsInactive && calendarDayButton.IsBlackedOut)
				{
					this.Owner.OnDayClick(selectedDate);
				}
				return;
			}
			CalendarSelectionMode selectionMode = this.Owner.SelectionMode;
			if (selectionMode == CalendarSelectionMode.SingleRange)
			{
				this.Owner.SelectedDates.ClearInternal();
				this.EndDrag(ctrl, selectedDate);
				return;
			}
			if (selectionMode != CalendarSelectionMode.MultipleRange)
			{
				return;
			}
			this.EndDrag(ctrl, selectedDate);
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x0019FCA8 File Offset: 0x0019DEA8
		private void Month_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null)
			{
				this._isMonthPressed = true;
				Mouse.Capture(this, CaptureMode.SubTree);
				if (this.Owner != null)
				{
					this.Owner.OnCalendarButtonPressed(calendarButton, false);
				}
			}
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x0019FCE4 File Offset: 0x0019DEE4
		private void Month_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null && this.Owner != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, true);
			}
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x0019FD10 File Offset: 0x0019DF10
		private void Month_MouseEnter(object sender, MouseEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null && this._isMonthPressed && this.Owner != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, false);
			}
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x0019FD44 File Offset: 0x0019DF44
		private void Month_Clicked(object sender, RoutedEventArgs e)
		{
			CalendarButton calendarButton = sender as CalendarButton;
			if (calendarButton != null)
			{
				this.Owner.OnCalendarButtonPressed(calendarButton, true);
			}
		}

		// Token: 0x06005C70 RID: 23664 RVA: 0x0019FD68 File Offset: 0x0019DF68
		private void HeaderButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				if (this.Owner.DisplayMode == CalendarMode.Month)
				{
					this.Owner.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Year);
				}
				else
				{
					this.Owner.SetCurrentValueInternal(Calendar.DisplayModeProperty, CalendarMode.Decade);
				}
				this.FocusDate(this.DisplayDate);
			}
		}

		// Token: 0x06005C71 RID: 23665 RVA: 0x0019FDC4 File Offset: 0x0019DFC4
		private void PreviousButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				this.Owner.OnPreviousClick();
			}
		}

		// Token: 0x06005C72 RID: 23666 RVA: 0x0019FDD9 File Offset: 0x0019DFD9
		private void NextButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Owner != null)
			{
				this.Owner.OnNextClick();
			}
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x0019FDF0 File Offset: 0x0019DFF0
		private void PopulateGrids()
		{
			if (this._monthView != null)
			{
				for (int i = 0; i < 7; i++)
				{
					FrameworkElement frameworkElement = (this._dayTitleTemplate != null) ? ((FrameworkElement)this._dayTitleTemplate.LoadContent()) : new ContentControl();
					frameworkElement.SetValue(Grid.RowProperty, 0);
					frameworkElement.SetValue(Grid.ColumnProperty, i);
					this._monthView.Children.Add(frameworkElement);
				}
				for (int j = 1; j < 7; j++)
				{
					for (int k = 0; k < 7; k++)
					{
						CalendarDayButton calendarDayButton = new CalendarDayButton();
						calendarDayButton.Owner = this.Owner;
						calendarDayButton.SetValue(Grid.RowProperty, j);
						calendarDayButton.SetValue(Grid.ColumnProperty, k);
						calendarDayButton.SetBinding(FrameworkElement.StyleProperty, this.GetOwnerBinding("CalendarDayButtonStyle"));
						calendarDayButton.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonDown), true);
						calendarDayButton.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Cell_MouseLeftButtonUp), true);
						calendarDayButton.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(this.Cell_MouseEnter), true);
						calendarDayButton.Click += this.Cell_Clicked;
						calendarDayButton.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(this.CellOrMonth_PreviewKeyDown), true);
						this._monthView.Children.Add(calendarDayButton);
					}
				}
			}
			if (this._yearView != null)
			{
				int num = 0;
				for (int l = 0; l < 3; l++)
				{
					for (int m = 0; m < 4; m++)
					{
						CalendarButton calendarButton = new CalendarButton();
						calendarButton.Owner = this.Owner;
						calendarButton.SetValue(Grid.RowProperty, l);
						calendarButton.SetValue(Grid.ColumnProperty, m);
						calendarButton.SetBinding(FrameworkElement.StyleProperty, this.GetOwnerBinding("CalendarButtonStyle"));
						calendarButton.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.Month_MouseLeftButtonDown), true);
						calendarButton.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Month_MouseLeftButtonUp), true);
						calendarButton.AddHandler(UIElement.MouseEnterEvent, new MouseEventHandler(this.Month_MouseEnter), true);
						calendarButton.AddHandler(UIElement.PreviewKeyDownEvent, new RoutedEventHandler(this.CellOrMonth_PreviewKeyDown), true);
						calendarButton.Click += this.Month_Clicked;
						this._yearView.Children.Add(calendarButton);
						num++;
					}
				}
			}
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x001A0084 File Offset: 0x0019E284
		private void SetMonthModeDayTitles()
		{
			if (this._monthView != null)
			{
				string[] shortestDayNames = DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).ShortestDayNames;
				for (int i = 0; i < 7; i++)
				{
					FrameworkElement frameworkElement = this._monthView.Children[i] as FrameworkElement;
					if (frameworkElement != null && shortestDayNames != null && shortestDayNames.Length != 0)
					{
						if (this.Owner != null)
						{
							frameworkElement.DataContext = shortestDayNames[(int)((i + this.Owner.FirstDayOfWeek) % (DayOfWeek)shortestDayNames.Length)];
						}
						else
						{
							frameworkElement.DataContext = shortestDayNames[(int)((i + DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).FirstDayOfWeek) % (DayOfWeek)shortestDayNames.Length)];
						}
					}
				}
			}
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x001A0118 File Offset: 0x0019E318
		private void SetMonthModeCalendarDayButtons()
		{
			DateTime dateTime = DateTimeHelper.DiscardDayTime(this.DisplayDate);
			int numberOfDisplayedDaysFromPreviousMonth = this.GetNumberOfDisplayedDaysFromPreviousMonth(dateTime);
			bool flag = DateTimeHelper.CompareYearMonth(dateTime, DateTime.MinValue) <= 0;
			bool flag2 = DateTimeHelper.CompareYearMonth(dateTime, DateTime.MaxValue) >= 0;
			int daysInMonth = this._calendar.GetDaysInMonth(dateTime.Year, dateTime.Month);
			CultureInfo culture = DateTimeHelper.GetCulture(this);
			int num = 49;
			for (int i = 7; i < num; i++)
			{
				CalendarDayButton calendarDayButton = this._monthView.Children[i] as CalendarDayButton;
				int num2 = i - numberOfDisplayedDaysFromPreviousMonth - 7;
				if ((!flag || num2 >= 0) && (!flag2 || num2 < daysInMonth))
				{
					DateTime dateTime2 = this._calendar.AddDays(dateTime, num2);
					this.SetMonthModeDayButtonState(calendarDayButton, new DateTime?(dateTime2));
					calendarDayButton.DataContext = dateTime2;
					calendarDayButton.SetContentInternal(DateTimeHelper.ToDayString(new DateTime?(dateTime2), culture));
				}
				else
				{
					this.SetMonthModeDayButtonState(calendarDayButton, null);
					calendarDayButton.DataContext = null;
					calendarDayButton.SetContentInternal(DateTimeHelper.ToDayString(null, culture));
				}
			}
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x001A0244 File Offset: 0x0019E444
		private void SetMonthModeDayButtonState(CalendarDayButton childButton, DateTime? dateToAdd)
		{
			if (this.Owner != null)
			{
				if (dateToAdd != null)
				{
					childButton.Visibility = Visibility.Visible;
					if (DateTimeHelper.CompareDays(dateToAdd.Value, this.Owner.DisplayDateStartInternal) < 0 || DateTimeHelper.CompareDays(dateToAdd.Value, this.Owner.DisplayDateEndInternal) > 0)
					{
						childButton.IsEnabled = false;
						childButton.Visibility = Visibility.Hidden;
						return;
					}
					childButton.IsEnabled = true;
					childButton.SetValue(CalendarDayButton.IsBlackedOutPropertyKey, this.Owner.BlackoutDates.Contains(dateToAdd.Value));
					childButton.SetValue(CalendarDayButton.IsInactivePropertyKey, DateTimeHelper.CompareYearMonth(dateToAdd.Value, this.Owner.DisplayDateInternal) != 0);
					if (DateTimeHelper.CompareDays(dateToAdd.Value, DateTime.Today) == 0)
					{
						childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, true);
					}
					else
					{
						childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, false);
					}
					childButton.NotifyNeedsVisualStateUpdate();
					bool flag = false;
					foreach (DateTime dt in this.Owner.SelectedDates)
					{
						flag |= (DateTimeHelper.CompareDays(dateToAdd.Value, dt) == 0);
					}
					childButton.SetValue(CalendarDayButton.IsSelectedPropertyKey, flag);
					return;
				}
				else
				{
					childButton.Visibility = Visibility.Hidden;
					childButton.IsEnabled = false;
					childButton.SetValue(CalendarDayButton.IsBlackedOutPropertyKey, false);
					childButton.SetValue(CalendarDayButton.IsInactivePropertyKey, true);
					childButton.SetValue(CalendarDayButton.IsTodayPropertyKey, false);
					childButton.SetValue(CalendarDayButton.IsSelectedPropertyKey, false);
				}
			}
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x001A03D4 File Offset: 0x0019E5D4
		private void AddMonthModeHighlight()
		{
			Calendar owner = this.Owner;
			if (owner == null)
			{
				return;
			}
			if (owner.HoverStart != null && owner.HoverEnd != null)
			{
				DateTime value = owner.HoverEnd.Value;
				DateTime value2 = owner.HoverEnd.Value;
				int num = DateTimeHelper.CompareDays(owner.HoverEnd.Value, owner.HoverStart.Value);
				if (num < 0)
				{
					value2 = owner.HoverStart.Value;
				}
				else
				{
					value = owner.HoverStart.Value;
				}
				int num2 = 49;
				for (int i = 7; i < num2; i++)
				{
					CalendarDayButton calendarDayButton = this._monthView.Children[i] as CalendarDayButton;
					if (calendarDayButton.DataContext is DateTime)
					{
						DateTime date = (DateTime)calendarDayButton.DataContext;
						calendarDayButton.SetValue(CalendarDayButton.IsHighlightedPropertyKey, num != 0 && DateTimeHelper.InRange(date, value, value2));
					}
					else
					{
						calendarDayButton.SetValue(CalendarDayButton.IsHighlightedPropertyKey, false);
					}
				}
				return;
			}
			int num3 = 49;
			for (int j = 7; j < num3; j++)
			{
				CalendarDayButton calendarDayButton2 = this._monthView.Children[j] as CalendarDayButton;
				calendarDayButton2.SetValue(CalendarDayButton.IsHighlightedPropertyKey, false);
			}
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x001A0529 File Offset: 0x0019E729
		private void SetMonthModeHeaderButton()
		{
			if (this._headerButton != null)
			{
				this._headerButton.Content = DateTimeHelper.ToYearMonthPatternString(new DateTime?(this.DisplayDate), DateTimeHelper.GetCulture(this));
				if (this.Owner != null)
				{
					this._headerButton.IsEnabled = true;
				}
			}
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x001A0568 File Offset: 0x0019E768
		private void SetMonthModeNextButton()
		{
			if (this.Owner != null && this._nextButton != null)
			{
				DateTime dateTime = DateTimeHelper.DiscardDayTime(this.DisplayDate);
				if (DateTimeHelper.CompareYearMonth(dateTime, DateTime.MaxValue) == 0)
				{
					this._nextButton.IsEnabled = false;
					return;
				}
				DateTime dt = this._calendar.AddMonths(dateTime, 1);
				this._nextButton.IsEnabled = (DateTimeHelper.CompareDays(this.Owner.DisplayDateEndInternal, dt) > -1);
			}
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x001A05D8 File Offset: 0x0019E7D8
		private void SetMonthModePreviousButton()
		{
			if (this.Owner != null && this._previousButton != null)
			{
				DateTime dt = DateTimeHelper.DiscardDayTime(this.DisplayDate);
				this._previousButton.IsEnabled = (DateTimeHelper.CompareDays(this.Owner.DisplayDateStartInternal, dt) < 0);
			}
		}

		// Token: 0x06005C7B RID: 23675 RVA: 0x001A0620 File Offset: 0x0019E820
		private void SetYearButtons(int decade, int decadeEnd)
		{
			int num = -1;
			foreach (object obj in this._yearView.Children)
			{
				CalendarButton calendarButton = obj as CalendarButton;
				int num2 = decade + num;
				if (num2 <= DateTime.MaxValue.Year && num2 >= DateTime.MinValue.Year)
				{
					DateTime dateTime = new DateTime(num2, 1, 1);
					calendarButton.DataContext = dateTime;
					calendarButton.SetContentInternal(DateTimeHelper.ToYearString(new DateTime?(dateTime), DateTimeHelper.GetCulture(this)));
					calendarButton.Visibility = Visibility.Visible;
					if (this.Owner != null)
					{
						calendarButton.HasSelectedDays = (this.Owner.DisplayDate.Year == num2);
						if (num2 < this.Owner.DisplayDateStartInternal.Year || num2 > this.Owner.DisplayDateEndInternal.Year)
						{
							calendarButton.IsEnabled = false;
							calendarButton.Opacity = 0.0;
						}
						else
						{
							calendarButton.IsEnabled = true;
							calendarButton.Opacity = 1.0;
						}
					}
					calendarButton.IsInactive = (num2 < decade || num2 > decadeEnd);
				}
				else
				{
					calendarButton.DataContext = null;
					calendarButton.IsEnabled = false;
					calendarButton.Opacity = 0.0;
				}
				num++;
			}
		}

		// Token: 0x06005C7C RID: 23676 RVA: 0x001A07B4 File Offset: 0x0019E9B4
		private void SetYearModeMonthButtons()
		{
			int num = 0;
			foreach (object obj in this._yearView.Children)
			{
				CalendarButton calendarButton = obj as CalendarButton;
				DateTime dateTime = new DateTime(this.DisplayDate.Year, num + 1, 1);
				calendarButton.DataContext = dateTime;
				calendarButton.SetContentInternal(DateTimeHelper.ToAbbreviatedMonthString(new DateTime?(dateTime), DateTimeHelper.GetCulture(this)));
				calendarButton.Visibility = Visibility.Visible;
				if (this.Owner != null)
				{
					calendarButton.HasSelectedDays = (DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateInternal) == 0);
					if (DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateStartInternal) < 0 || DateTimeHelper.CompareYearMonth(dateTime, this.Owner.DisplayDateEndInternal) > 0)
					{
						calendarButton.IsEnabled = false;
						calendarButton.Opacity = 0.0;
					}
					else
					{
						calendarButton.IsEnabled = true;
						calendarButton.Opacity = 1.0;
					}
				}
				calendarButton.IsInactive = false;
				num++;
			}
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x001A08E4 File Offset: 0x0019EAE4
		private void SetYearModeHeaderButton()
		{
			if (this._headerButton != null)
			{
				this._headerButton.IsEnabled = true;
				this._headerButton.Content = DateTimeHelper.ToYearString(new DateTime?(this.DisplayDate), DateTimeHelper.GetCulture(this));
			}
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x001A091C File Offset: 0x0019EB1C
		private void SetYearModeNextButton()
		{
			if (this.Owner != null && this._nextButton != null)
			{
				this._nextButton.IsEnabled = (this.Owner.DisplayDateEndInternal.Year != this.DisplayDate.Year);
			}
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x001A096C File Offset: 0x0019EB6C
		private void SetYearModePreviousButton()
		{
			if (this.Owner != null && this._previousButton != null)
			{
				this._previousButton.IsEnabled = (this.Owner.DisplayDateStartInternal.Year != this.DisplayDate.Year);
			}
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x001A09BA File Offset: 0x0019EBBA
		private void SetDecadeModeHeaderButton(int decade)
		{
			if (this._headerButton != null)
			{
				this._headerButton.Content = DateTimeHelper.ToDecadeRangeString(decade, this);
				this._headerButton.IsEnabled = false;
			}
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x001A09E4 File Offset: 0x0019EBE4
		private void SetDecadeModeNextButton(int decadeEnd)
		{
			if (this.Owner != null && this._nextButton != null)
			{
				this._nextButton.IsEnabled = (this.Owner.DisplayDateEndInternal.Year > decadeEnd);
			}
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x001A0A24 File Offset: 0x0019EC24
		private void SetDecadeModePreviousButton(int decade)
		{
			if (this.Owner != null && this._previousButton != null)
			{
				this._previousButton.IsEnabled = (decade > this.Owner.DisplayDateStartInternal.Year);
			}
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x001A0A64 File Offset: 0x0019EC64
		private int GetNumberOfDisplayedDaysFromPreviousMonth(DateTime firstOfMonth)
		{
			DayOfWeek dayOfWeek = this._calendar.GetDayOfWeek(firstOfMonth);
			int num;
			if (this.Owner != null)
			{
				num = (dayOfWeek - this.Owner.FirstDayOfWeek + 7) % 7;
			}
			else
			{
				num = (dayOfWeek - DateTimeHelper.GetDateFormat(DateTimeHelper.GetCulture(this)).FirstDayOfWeek + 7) % 7;
			}
			if (num == 0)
			{
				return 7;
			}
			return num;
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x001A0AB8 File Offset: 0x0019ECB8
		private BindingBase GetOwnerBinding(string propertyName)
		{
			return new Binding(propertyName)
			{
				Source = this.Owner
			};
		}

		/// <summary>Gets or sets the resource key for the <see cref="T:System.Windows.DataTemplate" /> that displays the days of the week.</summary>
		/// <returns>The resource key for the <see cref="T:System.Windows.DataTemplate" /> that displays the days of the week.</returns>
		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06005C85 RID: 23685 RVA: 0x001A0AD9 File Offset: 0x0019ECD9
		public static ComponentResourceKey DayTitleTemplateResourceKey
		{
			get
			{
				if (CalendarItem._dayTitleTemplateResourceKey == null)
				{
					CalendarItem._dayTitleTemplateResourceKey = new ComponentResourceKey(typeof(CalendarItem), "DayTitleTemplate");
				}
				return CalendarItem._dayTitleTemplateResourceKey;
			}
		}

		// Token: 0x04002FBB RID: 12219
		private const string ElementRoot = "PART_Root";

		// Token: 0x04002FBC RID: 12220
		private const string ElementHeaderButton = "PART_HeaderButton";

		// Token: 0x04002FBD RID: 12221
		private const string ElementPreviousButton = "PART_PreviousButton";

		// Token: 0x04002FBE RID: 12222
		private const string ElementNextButton = "PART_NextButton";

		// Token: 0x04002FBF RID: 12223
		private const string ElementDayTitleTemplate = "DayTitleTemplate";

		// Token: 0x04002FC0 RID: 12224
		private const string ElementMonthView = "PART_MonthView";

		// Token: 0x04002FC1 RID: 12225
		private const string ElementYearView = "PART_YearView";

		// Token: 0x04002FC2 RID: 12226
		private const string ElementDisabledVisual = "PART_DisabledVisual";

		// Token: 0x04002FC3 RID: 12227
		private const int COLS = 7;

		// Token: 0x04002FC4 RID: 12228
		private const int ROWS = 7;

		// Token: 0x04002FC5 RID: 12229
		private const int YEAR_COLS = 4;

		// Token: 0x04002FC6 RID: 12230
		private const int YEAR_ROWS = 3;

		// Token: 0x04002FC7 RID: 12231
		private const int NUMBER_OF_DAYS_IN_WEEK = 7;

		// Token: 0x04002FC8 RID: 12232
		private static ComponentResourceKey _dayTitleTemplateResourceKey;

		// Token: 0x04002FC9 RID: 12233
		private Calendar _calendar = new GregorianCalendar();

		// Token: 0x04002FCA RID: 12234
		private DataTemplate _dayTitleTemplate;

		// Token: 0x04002FCB RID: 12235
		private FrameworkElement _disabledVisual;

		// Token: 0x04002FCC RID: 12236
		private Button _headerButton;

		// Token: 0x04002FCD RID: 12237
		private Grid _monthView;

		// Token: 0x04002FCE RID: 12238
		private Button _nextButton;

		// Token: 0x04002FCF RID: 12239
		private Button _previousButton;

		// Token: 0x04002FD0 RID: 12240
		private Grid _yearView;

		// Token: 0x04002FD1 RID: 12241
		private bool _isMonthPressed;

		// Token: 0x04002FD2 RID: 12242
		private bool _isDayPressed;
	}
}
