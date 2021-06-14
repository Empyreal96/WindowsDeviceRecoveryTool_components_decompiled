using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Calendar" /> types to UI Automation.</summary>
	// Token: 0x02000298 RID: 664
	public sealed class CalendarAutomationPeer : FrameworkElementAutomationPeer, IGridProvider, IMultipleViewProvider, ISelectionProvider, ITableProvider, IItemContainerProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.CalendarAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x06002525 RID: 9509 RVA: 0x000B320A File Offset: 0x000B140A
		public CalendarAutomationPeer(System.Windows.Controls.Calendar owner) : base(owner)
		{
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000B3229 File Offset: 0x000B1429
		private System.Windows.Controls.Calendar OwningCalendar
		{
			get
			{
				return base.Owner as System.Windows.Controls.Calendar;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x000B3238 File Offset: 0x000B1438
		private Grid OwningGrid
		{
			get
			{
				if (this.OwningCalendar == null || this.OwningCalendar.MonthControl == null)
				{
					return null;
				}
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
				{
					return this.OwningCalendar.MonthControl.MonthView;
				}
				return this.OwningCalendar.MonthControl.YearView;
			}
		}

		/// <summary>Gets the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration value that specifies the control pattern.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Grid" />, <see cref="F:System.Windows.Automation.Peers.PatternInterface.Table" />, <see cref="F:System.Windows.Automation.Peers.PatternInterface.MultipleView" />, or <see cref="F:System.Windows.Automation.Peers.PatternInterface.Selection" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x06002528 RID: 9512 RVA: 0x000B328A File Offset: 0x000B148A
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.Grid)
			{
				if (patternInterface != PatternInterface.Selection && patternInterface != PatternInterface.Grid)
				{
					goto IL_27;
				}
			}
			else if (patternInterface != PatternInterface.MultipleView && patternInterface != PatternInterface.Table && patternInterface != PatternInterface.ItemContainer)
			{
				goto IL_27;
			}
			if (this.OwningGrid != null)
			{
				return this;
			}
			IL_27:
			return base.GetPattern(patternInterface);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x00016748 File Offset: 0x00014948
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Calendar;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000B32BC File Offset: 0x000B14BC
		protected override List<AutomationPeer> GetChildrenCore()
		{
			if (this.OwningCalendar.MonthControl == null)
			{
				return null;
			}
			List<AutomationPeer> list = new List<AutomationPeer>();
			Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> dictionary = new Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer>();
			AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.PreviousButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.HeaderButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar.MonthControl.NextButton);
			if (automationPeer != null)
			{
				list.Add(automationPeer);
			}
			foreach (object obj in this.OwningGrid.Children)
			{
				UIElement uielement = (UIElement)obj;
				int num = (int)uielement.GetValue(Grid.RowProperty);
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month && num == 0)
				{
					AutomationPeer automationPeer2 = UIElementAutomationPeer.CreatePeerForElement(uielement);
					if (automationPeer2 != null)
					{
						list.Add(automationPeer2);
					}
				}
				else
				{
					Button button = uielement as Button;
					if (button != null && button.DataContext is DateTime)
					{
						DateTime date = (DateTime)button.DataContext;
						DateTimeAutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, this.OwningCalendar.DisplayMode, false);
						list.Add(orCreateDateTimeAutomationPeer);
						DateTimeCalendarModePair key = new DateTimeCalendarModePair(date, this.OwningCalendar.DisplayMode);
						dictionary.Add(key, orCreateDateTimeAutomationPeer);
					}
				}
			}
			this.DateTimePeers = dictionary;
			return list;
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000B3458 File Offset: 0x000B1658
		protected override void SetFocusCore()
		{
			System.Windows.Controls.Calendar owningCalendar = this.OwningCalendar;
			if (owningCalendar.Focusable)
			{
				if (!owningCalendar.Focus())
				{
					DateTime date;
					if (owningCalendar.SelectedDate != null && DateTimeHelper.CompareYearMonth(owningCalendar.SelectedDate.Value, owningCalendar.DisplayDateInternal) == 0)
					{
						date = owningCalendar.SelectedDate.Value;
					}
					else
					{
						date = owningCalendar.DisplayDate;
					}
					DateTimeAutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, owningCalendar.DisplayMode, false);
					FrameworkElement owningButton = orCreateDateTimeAutomationPeer.OwningButton;
					if (owningButton == null || !owningButton.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x000B3509 File Offset: 0x000B1709
		private DateTimeAutomationPeer GetOrCreateDateTimeAutomationPeer(DateTime date, CalendarMode buttonMode)
		{
			return this.GetOrCreateDateTimeAutomationPeer(date, buttonMode, true);
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000B3514 File Offset: 0x000B1714
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private DateTimeAutomationPeer GetOrCreateDateTimeAutomationPeer(DateTime date, CalendarMode buttonMode, bool addParentInfo)
		{
			DateTimeCalendarModePair dateTimeCalendarModePair = new DateTimeCalendarModePair(date, buttonMode);
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			this.DateTimePeers.TryGetValue(dateTimeCalendarModePair, out dateTimeAutomationPeer);
			if (dateTimeAutomationPeer == null)
			{
				dateTimeAutomationPeer = this.GetPeerFromWeakRefStorage(dateTimeCalendarModePair);
				if (dateTimeAutomationPeer != null && !addParentInfo)
				{
					dateTimeAutomationPeer.AncestorsInvalid = false;
					dateTimeAutomationPeer.ChildrenValid = false;
				}
			}
			if (dateTimeAutomationPeer == null)
			{
				dateTimeAutomationPeer = new DateTimeAutomationPeer(date, this.OwningCalendar, buttonMode);
				if (addParentInfo && dateTimeAutomationPeer != null)
				{
					dateTimeAutomationPeer.TrySetParentInfo(this);
				}
			}
			AutomationPeer wrapperPeer = dateTimeAutomationPeer.WrapperPeer;
			if (wrapperPeer != null)
			{
				wrapperPeer.EventsSource = dateTimeAutomationPeer;
			}
			return dateTimeAutomationPeer;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x000B358C File Offset: 0x000B178C
		private DateTimeAutomationPeer GetPeerFromWeakRefStorage(DateTimeCalendarModePair dateTimeCalendarModePairKey)
		{
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			WeakReference weakReference = null;
			this.WeakRefElementProxyStorage.TryGetValue(dateTimeCalendarModePairKey, out weakReference);
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					dateTimeAutomationPeer = (base.PeerFromProvider(elementProxy) as DateTimeAutomationPeer);
					if (dateTimeAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(dateTimeCalendarModePairKey);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(dateTimeCalendarModePairKey);
				}
			}
			return dateTimeAutomationPeer;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x000B35EC File Offset: 0x000B17EC
		internal void AddProxyToWeakRefStorage(WeakReference wr, DateTimeAutomationPeer dateTimePeer)
		{
			DateTimeCalendarModePair dateTimeCalendarModePair = new DateTimeCalendarModePair(dateTimePeer.Date, dateTimePeer.ButtonMode);
			if (this.GetPeerFromWeakRefStorage(dateTimeCalendarModePair) == null)
			{
				this.WeakRefElementProxyStorage.Add(dateTimeCalendarModePair, wr);
			}
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x000B3624 File Offset: 0x000B1824
		internal void RaiseSelectionEvents(SelectionChangedEventArgs e)
		{
			int count = this.OwningCalendar.SelectedDates.Count;
			int count2 = e.AddedItems.Count;
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && count == 1 && count2 == 1)
			{
				DateTimeAutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer((DateTime)e.AddedItems[0], CalendarMode.Month);
				if (orCreateDateTimeAutomationPeer != null)
				{
					orCreateDateTimeAutomationPeer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
				}
			}
			else if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
			{
				foreach (object obj in e.AddedItems)
				{
					DateTime date = (DateTime)obj;
					DateTimeAutomationPeer orCreateDateTimeAutomationPeer2 = this.GetOrCreateDateTimeAutomationPeer(date, CalendarMode.Month);
					if (orCreateDateTimeAutomationPeer2 != null)
					{
						orCreateDateTimeAutomationPeer2.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
					}
				}
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				foreach (object obj2 in e.RemovedItems)
				{
					DateTime date2 = (DateTime)obj2;
					DateTimeAutomationPeer orCreateDateTimeAutomationPeer3 = this.GetOrCreateDateTimeAutomationPeer(date2, CalendarMode.Month);
					if (orCreateDateTimeAutomationPeer3 != null)
					{
						orCreateDateTimeAutomationPeer3.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
					}
				}
			}
		}

		/// <summary>Gets the total number of columns in a grid.</summary>
		/// <returns>The total number of columns in a grid.</returns>
		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06002532 RID: 9522 RVA: 0x000B3754 File Offset: 0x000B1954
		int IGridProvider.ColumnCount
		{
			get
			{
				if (this.OwningGrid != null)
				{
					return this.OwningGrid.ColumnDefinitions.Count;
				}
				return 0;
			}
		}

		/// <summary>Gets the total number of rows in a grid.</summary>
		/// <returns>The total number of rows in a grid.</returns>
		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06002533 RID: 9523 RVA: 0x000B3770 File Offset: 0x000B1970
		int IGridProvider.RowCount
		{
			get
			{
				if (this.OwningGrid == null)
				{
					return 0;
				}
				if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
				{
					return Math.Max(0, this.OwningGrid.RowDefinitions.Count - 1);
				}
				return this.OwningGrid.RowDefinitions.Count;
			}
		}

		/// <summary>Retrieves the UI Automation provider for the specified cell.</summary>
		/// <param name="row">The ordinal number of the row of interest.</param>
		/// <param name="column">The ordinal number of the column of interest.</param>
		/// <returns>The UI Automation provider for the specified cell.</returns>
		// Token: 0x06002534 RID: 9524 RVA: 0x000B37C0 File Offset: 0x000B19C0
		IRawElementProviderSimple IGridProvider.GetItem(int row, int column)
		{
			if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
			{
				row++;
			}
			if (this.OwningGrid != null && row >= 0 && row < this.OwningGrid.RowDefinitions.Count && column >= 0 && column < this.OwningGrid.ColumnDefinitions.Count)
			{
				foreach (object obj in this.OwningGrid.Children)
				{
					UIElement uielement = (UIElement)obj;
					int num = (int)uielement.GetValue(Grid.RowProperty);
					int num2 = (int)uielement.GetValue(Grid.ColumnProperty);
					if (num == row && num2 == column)
					{
						object dataContext = (uielement as FrameworkElement).DataContext;
						if (dataContext is DateTime)
						{
							DateTime date = (DateTime)dataContext;
							AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, this.OwningCalendar.DisplayMode);
							return base.ProviderFromPeer(orCreateDateTimeAutomationPeer);
						}
					}
				}
			}
			return null;
		}

		/// <summary>Gets the current control-specific view.</summary>
		/// <returns>The value for the current view of the UI Automation element. </returns>
		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06002535 RID: 9525 RVA: 0x000B38E0 File Offset: 0x000B1AE0
		int IMultipleViewProvider.CurrentView
		{
			get
			{
				return (int)this.OwningCalendar.DisplayMode;
			}
		}

		/// <summary>Retrieves a collection of control-specific view identifiers.</summary>
		/// <returns>A collection of values that identifies the views available for a UI Automation element. </returns>
		// Token: 0x06002536 RID: 9526 RVA: 0x000B38F0 File Offset: 0x000B1AF0
		int[] IMultipleViewProvider.GetSupportedViews()
		{
			return new int[]
			{
				0,
				1,
				2
			};
		}

		/// <summary>Retrieves the name of a control-specific view.</summary>
		/// <param name="viewId">The view identifier.</param>
		/// <returns>A localized name for the view.</returns>
		// Token: 0x06002537 RID: 9527 RVA: 0x000B3911 File Offset: 0x000B1B11
		string IMultipleViewProvider.GetViewName(int viewId)
		{
			switch (viewId)
			{
			case 0:
				return SR.Get("CalendarAutomationPeer_MonthMode");
			case 1:
				return SR.Get("CalendarAutomationPeer_YearMode");
			case 2:
				return SR.Get("CalendarAutomationPeer_DecadeMode");
			default:
				return string.Empty;
			}
		}

		/// <summary>Sets the current control-specific view. </summary>
		/// <param name="viewId">A view identifier.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="viewId" /> is not a member of the supported views collection.</exception>
		// Token: 0x06002538 RID: 9528 RVA: 0x000B394D File Offset: 0x000B1B4D
		void IMultipleViewProvider.SetCurrentView(int viewId)
		{
			this.OwningCalendar.DisplayMode = (CalendarMode)viewId;
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider allows more than one child element to be selected concurrently.</summary>
		/// <returns>
		///     <see langword="true" /> if multiple selection is allowed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06002539 RID: 9529 RVA: 0x000B395B File Offset: 0x000B1B5B
		bool ISelectionProvider.CanSelectMultiple
		{
			get
			{
				return this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleRange || this.OwningCalendar.SelectionMode == CalendarSelectionMode.MultipleRange;
			}
		}

		/// <summary>Gets a value that specifies whether the UI Automation provider requires at least one child element to be selected.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600253A RID: 9530 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ISelectionProvider.IsSelectionRequired
		{
			get
			{
				return false;
			}
		}

		/// <summary>Retrieves a UI Automation provider for each child element that is selected.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x0600253B RID: 9531 RVA: 0x000B397C File Offset: 0x000B1B7C
		IRawElementProviderSimple[] ISelectionProvider.GetSelection()
		{
			List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
			foreach (DateTime date in this.OwningCalendar.SelectedDates)
			{
				AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(date, CalendarMode.Month);
				list.Add(base.ProviderFromPeer(orCreateDateTimeAutomationPeer));
			}
			if (list.Count > 0)
			{
				return list.ToArray();
			}
			return null;
		}

		/// <summary>Retrieves an element by the specified property value.</summary>
		/// <param name="startAfterProvider">The item in the container after which to begin the search.</param>
		/// <param name="propertyId">The property that contains the value to retrieve.</param>
		/// <param name="value">The value to retrieve.</param>
		/// <returns>The first item that matches the search criterion; otherwise, <see langword="null" /> if no items match.</returns>
		// Token: 0x0600253C RID: 9532 RVA: 0x000B39F4 File Offset: 0x000B1BF4
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfterProvider, int propertyId, object value)
		{
			DateTimeAutomationPeer dateTimeAutomationPeer = null;
			if (startAfterProvider != null)
			{
				dateTimeAutomationPeer = (base.PeerFromProvider(startAfterProvider) as DateTimeAutomationPeer);
				if (dateTimeAutomationPeer == null)
				{
					throw new InvalidOperationException(SR.Get("InavalidStartItem"));
				}
			}
			DateTime? dateTime = null;
			CalendarMode calendarMode;
			if (propertyId == SelectionItemPatternIdentifiers.IsSelectedProperty.Id)
			{
				calendarMode = CalendarMode.Month;
				dateTime = this.GetNextSelectedDate(dateTimeAutomationPeer, (bool)value);
			}
			else if (propertyId == AutomationElementIdentifiers.NameProperty.Id)
			{
				DateTimeFormatInfo currentDateFormat = DateTimeHelper.GetCurrentDateFormat();
				DateTime value2;
				if (DateTime.TryParse(value as string, currentDateFormat, DateTimeStyles.None, out value2))
				{
					dateTime = new DateTime?(value2);
				}
				if (dateTime == null || (dateTimeAutomationPeer != null && dateTime <= dateTimeAutomationPeer.Date))
				{
					throw new InvalidOperationException(SR.Get("CalendarNamePropertyValueNotValid"));
				}
				calendarMode = ((dateTimeAutomationPeer != null) ? dateTimeAutomationPeer.ButtonMode : this.OwningCalendar.DisplayMode);
			}
			else
			{
				if (propertyId != 0 && propertyId != AutomationElementIdentifiers.ControlTypeProperty.Id)
				{
					throw new ArgumentException(SR.Get("PropertyNotSupported"));
				}
				if (propertyId == AutomationElementIdentifiers.ControlTypeProperty.Id && (int)value != ControlType.Button.Id)
				{
					return null;
				}
				calendarMode = ((dateTimeAutomationPeer != null) ? dateTimeAutomationPeer.ButtonMode : this.OwningCalendar.DisplayMode);
				dateTime = this.GetNextDate(dateTimeAutomationPeer, calendarMode);
			}
			if (dateTime != null)
			{
				AutomationPeer orCreateDateTimeAutomationPeer = this.GetOrCreateDateTimeAutomationPeer(dateTime.Value, calendarMode);
				if (orCreateDateTimeAutomationPeer != null)
				{
					return base.ProviderFromPeer(orCreateDateTimeAutomationPeer);
				}
			}
			return null;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000B3B68 File Offset: 0x000B1D68
		private DateTime? GetNextDate(DateTimeAutomationPeer currentDatePeer, CalendarMode currentMode)
		{
			DateTime? result = null;
			DateTime dateTime = (currentDatePeer != null) ? currentDatePeer.Date : this.OwningCalendar.DisplayDate;
			if (currentMode == CalendarMode.Month)
			{
				result = new DateTime?(dateTime.AddDays(1.0));
			}
			else if (currentMode == CalendarMode.Year)
			{
				result = new DateTime?(dateTime.AddMonths(1));
			}
			else if (currentMode == CalendarMode.Decade)
			{
				result = new DateTime?(dateTime.AddYears(1));
			}
			return result;
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000B3BDC File Offset: 0x000B1DDC
		private DateTime? GetNextSelectedDate(DateTimeAutomationPeer currentDatePeer, bool isSelected)
		{
			DateTime dateTime = (currentDatePeer != null) ? currentDatePeer.Date : this.OwningCalendar.DisplayDate;
			if (isSelected)
			{
				if (this.OwningCalendar.SelectedDates.MaximumDate == null || this.OwningCalendar.SelectedDates.MaximumDate <= dateTime)
				{
					return null;
				}
				if (this.OwningCalendar.SelectedDates.MinimumDate != null && dateTime < this.OwningCalendar.SelectedDates.MinimumDate)
				{
					return this.OwningCalendar.SelectedDates.MinimumDate;
				}
			}
			do
			{
				dateTime = dateTime.AddDays(1.0);
			}
			while (this.OwningCalendar.SelectedDates.Contains(dateTime) != isSelected);
			return new DateTime?(dateTime);
		}

		/// <summary>Retrieves the primary direction of traversal for the table.</summary>
		/// <returns>The primary direction of traversal. </returns>
		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600253F RID: 9535 RVA: 0x0000B02A File Offset: 0x0000922A
		RowOrColumnMajor ITableProvider.RowOrColumnMajor
		{
			get
			{
				return RowOrColumnMajor.RowMajor;
			}
		}

		/// <summary>Gets a collection of UI Automation providers that represents all the column headers in a table.</summary>
		/// <returns>A collection of UI Automation providers. </returns>
		// Token: 0x06002540 RID: 9536 RVA: 0x000B3CDC File Offset: 0x000B1EDC
		IRawElementProviderSimple[] ITableProvider.GetColumnHeaders()
		{
			if (this.OwningCalendar.DisplayMode == CalendarMode.Month)
			{
				List<IRawElementProviderSimple> list = new List<IRawElementProviderSimple>();
				foreach (object obj in this.OwningGrid.Children)
				{
					UIElement uielement = (UIElement)obj;
					if ((int)uielement.GetValue(Grid.RowProperty) == 0)
					{
						AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(uielement);
						if (automationPeer != null)
						{
							list.Add(base.ProviderFromPeer(automationPeer));
						}
					}
				}
				if (list.Count > 0)
				{
					return list.ToArray();
				}
			}
			return null;
		}

		/// <summary>Retrieves a collection of UI Automation providers that represents all row headers in the table.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x06002541 RID: 9537 RVA: 0x0000C238 File Offset: 0x0000A438
		IRawElementProviderSimple[] ITableProvider.GetRowHeaders()
		{
			return null;
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x000B3D8C File Offset: 0x000B1F8C
		// (set) Token: 0x06002543 RID: 9539 RVA: 0x000B3D94 File Offset: 0x000B1F94
		private Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> DateTimePeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x000B3D9D File Offset: 0x000B1F9D
		private Dictionary<DateTimeCalendarModePair, WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._weakRefElementProxyStorage;
			}
		}

		// Token: 0x04001B64 RID: 7012
		private Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer> _dataChildren = new Dictionary<DateTimeCalendarModePair, DateTimeAutomationPeer>();

		// Token: 0x04001B65 RID: 7013
		private Dictionary<DateTimeCalendarModePair, WeakReference> _weakRefElementProxyStorage = new Dictionary<DateTimeCalendarModePair, WeakReference>();
	}
}
