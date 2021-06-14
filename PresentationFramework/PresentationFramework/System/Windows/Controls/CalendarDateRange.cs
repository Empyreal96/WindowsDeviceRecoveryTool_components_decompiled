using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Represents a range of dates in a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x02000478 RID: 1144
	public sealed class CalendarDateRange : INotifyPropertyChanged
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class. </summary>
		// Token: 0x06004314 RID: 17172 RVA: 0x001332F6 File Offset: 0x001314F6
		public CalendarDateRange() : this(DateTime.MinValue, DateTime.MaxValue)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class with a single date.</summary>
		/// <param name="day">The date to add.</param>
		// Token: 0x06004315 RID: 17173 RVA: 0x00133308 File Offset: 0x00131508
		public CalendarDateRange(DateTime day) : this(day, day)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class with a range of dates.</summary>
		/// <param name="start">The start of the range to be represented.</param>
		/// <param name="end">The end of the range to be represented.</param>
		// Token: 0x06004316 RID: 17174 RVA: 0x00133312 File Offset: 0x00131512
		public CalendarDateRange(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06004317 RID: 17175 RVA: 0x00133328 File Offset: 0x00131528
		// (remove) Token: 0x06004318 RID: 17176 RVA: 0x00133360 File Offset: 0x00131560
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Gets the last date in the represented range.</summary>
		/// <returns>The last date in the represented range.</returns>
		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06004319 RID: 17177 RVA: 0x00133395 File Offset: 0x00131595
		// (set) Token: 0x0600431A RID: 17178 RVA: 0x001333A8 File Offset: 0x001315A8
		public DateTime End
		{
			get
			{
				return CalendarDateRange.CoerceEnd(this._start, this._end);
			}
			set
			{
				DateTime dateTime = CalendarDateRange.CoerceEnd(this._start, value);
				if (dateTime != this.End)
				{
					this.OnChanging(new CalendarDateRangeChangingEventArgs(this._start, dateTime));
					this._end = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
				}
			}
		}

		/// <summary>Gets the first date in the represented range.</summary>
		/// <returns>The first date in the represented range.</returns>
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x0600431B RID: 17179 RVA: 0x001333F9 File Offset: 0x001315F9
		// (set) Token: 0x0600431C RID: 17180 RVA: 0x00133404 File Offset: 0x00131604
		public DateTime Start
		{
			get
			{
				return this._start;
			}
			set
			{
				if (this._start != value)
				{
					DateTime end = this.End;
					DateTime dateTime = CalendarDateRange.CoerceEnd(value, this._end);
					this.OnChanging(new CalendarDateRangeChangingEventArgs(value, dateTime));
					this._start = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Start"));
					if (dateTime != end)
					{
						this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
					}
				}
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x0600431D RID: 17181 RVA: 0x00133470 File Offset: 0x00131670
		// (remove) Token: 0x0600431E RID: 17182 RVA: 0x001334A8 File Offset: 0x001316A8
		internal event EventHandler<CalendarDateRangeChangingEventArgs> Changing;

		// Token: 0x0600431F RID: 17183 RVA: 0x001334DD File Offset: 0x001316DD
		internal bool ContainsAny(CalendarDateRange range)
		{
			return range.End >= this.Start && this.End >= range.Start;
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x00133508 File Offset: 0x00131708
		private void OnChanging(CalendarDateRangeChangingEventArgs e)
		{
			EventHandler<CalendarDateRangeChangingEventArgs> changing = this.Changing;
			if (changing != null)
			{
				changing(this, e);
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x00133528 File Offset: 0x00131728
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00133547 File Offset: 0x00131747
		private static DateTime CoerceEnd(DateTime start, DateTime end)
		{
			if (DateTime.Compare(start, end) > 0)
			{
				return start;
			}
			return end;
		}

		// Token: 0x04002823 RID: 10275
		private DateTime _end;

		// Token: 0x04002824 RID: 10276
		private DateTime _start;
	}
}
