using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace System.Windows.Controls
{
	/// <summary>Represents a collection of non-selectable dates in a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x02000476 RID: 1142
	public sealed class CalendarBlackoutDatesCollection : ObservableCollection<CalendarDateRange>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarBlackoutDatesCollection" /> class. </summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Calendar" /> whose dates this object represents.</param>
		// Token: 0x060042FD RID: 17149 RVA: 0x00132DCE File Offset: 0x00130FCE
		public CalendarBlackoutDatesCollection(Calendar owner)
		{
			this._owner = owner;
			this._dispatcherThread = Thread.CurrentThread;
		}

		/// <summary>Adds all dates before <see cref="P:System.DateTime.Today" /> to the collection.</summary>
		// Token: 0x060042FE RID: 17150 RVA: 0x00132DE8 File Offset: 0x00130FE8
		public void AddDatesInPast()
		{
			base.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1.0)));
		}

		/// <summary>Returns a value that represents whether this collection contains the specified date.</summary>
		/// <param name="date">The date to search for.</param>
		/// <returns>
		///     <see langword="true" /> if the collection contains the specified date; otherwise, <see langword="false" />.</returns>
		// Token: 0x060042FF RID: 17151 RVA: 0x00132E1B File Offset: 0x0013101B
		public bool Contains(DateTime date)
		{
			return this.GetContainingDateRange(date) != null;
		}

		/// <summary>Returns a value that represents whether this collection contains the specified range of dates.</summary>
		/// <param name="start">The start of the date range.</param>
		/// <param name="end">The end of the date range.</param>
		/// <returns>
		///     <see langword="true" /> if all dates in the range are contained in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004300 RID: 17152 RVA: 0x00132E28 File Offset: 0x00131028
		public bool Contains(DateTime start, DateTime end)
		{
			int count = base.Count;
			DateTime value;
			DateTime value2;
			if (DateTime.Compare(end, start) > -1)
			{
				value = DateTimeHelper.DiscardTime(new DateTime?(start)).Value;
				value2 = DateTimeHelper.DiscardTime(new DateTime?(end)).Value;
			}
			else
			{
				value = DateTimeHelper.DiscardTime(new DateTime?(end)).Value;
				value2 = DateTimeHelper.DiscardTime(new DateTime?(start)).Value;
			}
			for (int i = 0; i < count; i++)
			{
				if (DateTime.Compare(base[i].Start, value) == 0 && DateTime.Compare(base[i].End, value2) == 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>Returns a value that represents whether this collection contains any dates in the specified range of dates.</summary>
		/// <param name="range">The range of dates to search for.</param>
		/// <returns>
		///     <see langword="true" /> if any dates in the range are contained in the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004301 RID: 17153 RVA: 0x00132ED8 File Offset: 0x001310D8
		public bool ContainsAny(CalendarDateRange range)
		{
			foreach (CalendarDateRange calendarDateRange in this)
			{
				if (calendarDateRange.ContainsAny(range))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00132F2C File Offset: 0x0013112C
		internal DateTime? GetNonBlackoutDate(DateTime? requestedDate, int dayInterval)
		{
			DateTime? result = requestedDate;
			if (requestedDate == null)
			{
				return null;
			}
			CalendarDateRange containingDateRange;
			if ((containingDateRange = this.GetContainingDateRange(result.Value)) == null)
			{
				return requestedDate;
			}
			do
			{
				if (dayInterval > 0)
				{
					result = DateTimeHelper.AddDays(containingDateRange.End, dayInterval);
				}
				else
				{
					result = DateTimeHelper.AddDays(containingDateRange.Start, dayInterval);
				}
			}
			while (result != null && (containingDateRange = this.GetContainingDateRange(result.Value)) != null);
			return result;
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00132FA0 File Offset: 0x001311A0
		protected override void ClearItems()
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			foreach (CalendarDateRange item in base.Items)
			{
				this.UnRegisterItem(item);
			}
			base.ClearItems();
			this._owner.UpdateCellItems();
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x00133018 File Offset: 0x00131218
		protected override void InsertItem(int index, CalendarDateRange item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (this.IsValid(item))
			{
				this.RegisterItem(item);
				base.InsertItem(index, item);
				this._owner.UpdateCellItems();
				return;
			}
			throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00133070 File Offset: 0x00131270
		protected override void RemoveItem(int index)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (index >= 0 && index < base.Count)
			{
				this.UnRegisterItem(base.Items[index]);
			}
			base.RemoveItem(index);
			this._owner.UpdateCellItems();
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x001330C8 File Offset: 0x001312C8
		protected override void SetItem(int index, CalendarDateRange item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (this.IsValid(item))
			{
				CalendarDateRange item2 = null;
				if (index >= 0 && index < base.Count)
				{
					item2 = base.Items[index];
				}
				base.SetItem(index, item);
				this.UnRegisterItem(item2);
				this.RegisterItem(base.Items[index]);
				this._owner.UpdateCellItems();
				return;
			}
			throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0013314E File Offset: 0x0013134E
		private void RegisterItem(CalendarDateRange item)
		{
			if (item != null)
			{
				item.Changing += this.Item_Changing;
				item.PropertyChanged += this.Item_PropertyChanged;
			}
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x00133177 File Offset: 0x00131377
		private void UnRegisterItem(CalendarDateRange item)
		{
			if (item != null)
			{
				item.Changing -= this.Item_Changing;
				item.PropertyChanged -= this.Item_PropertyChanged;
			}
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x001331A0 File Offset: 0x001313A0
		private void Item_Changing(object sender, CalendarDateRangeChangingEventArgs e)
		{
			CalendarDateRange calendarDateRange = sender as CalendarDateRange;
			if (calendarDateRange != null && !this.IsValid(e.Start, e.End))
			{
				throw new ArgumentOutOfRangeException(SR.Get("Calendar_UnSelectableDates"));
			}
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x001331DB File Offset: 0x001313DB
		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender is CalendarDateRange)
			{
				this._owner.UpdateCellItems();
			}
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x001331F0 File Offset: 0x001313F0
		private bool IsValid(CalendarDateRange item)
		{
			return this.IsValid(item.Start, item.End);
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00133204 File Offset: 0x00131404
		private bool IsValid(DateTime start, DateTime end)
		{
			foreach (DateTime dateTime in this._owner.SelectedDates)
			{
				object obj = dateTime;
				if (DateTimeHelper.InRange((obj as DateTime?).Value, start, end))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00133278 File Offset: 0x00131478
		private bool IsValidThread()
		{
			return Thread.CurrentThread == this._dispatcherThread;
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00133288 File Offset: 0x00131488
		private CalendarDateRange GetContainingDateRange(DateTime date)
		{
			for (int i = 0; i < base.Count; i++)
			{
				if (DateTimeHelper.InRange(date, base[i]))
				{
					return base[i];
				}
			}
			return null;
		}

		// Token: 0x0400281F RID: 10271
		private Thread _dispatcherThread;

		// Token: 0x04002820 RID: 10272
		private Calendar _owner;
	}
}
