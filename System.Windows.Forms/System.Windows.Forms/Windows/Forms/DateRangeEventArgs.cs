using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.MonthCalendar.DateChanged" /> or <see cref="E:System.Windows.Forms.MonthCalendar.DateSelected" /> events of the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
	// Token: 0x02000218 RID: 536
	public class DateRangeEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DateRangeEventArgs" /> class.</summary>
		/// <param name="start">The first date/time value in the range that the user has selected. </param>
		/// <param name="end">The last date/time value in the range that the user has selected. </param>
		// Token: 0x0600207D RID: 8317 RVA: 0x000A2743 File Offset: 0x000A0943
		public DateRangeEventArgs(DateTime start, DateTime end)
		{
			this.start = start;
			this.end = end;
		}

		/// <summary>Gets the first date/time value in the range that the user has selected.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the first date in the date range that the user has selected.</returns>
		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x000A2759 File Offset: 0x000A0959
		public DateTime Start
		{
			get
			{
				return this.start;
			}
		}

		/// <summary>Gets the last date/time value in the range that the user has selected.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the last date in the date range that the user has selected.</returns>
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x000A2761 File Offset: 0x000A0961
		public DateTime End
		{
			get
			{
				return this.end;
			}
		}

		// Token: 0x04000DFF RID: 3583
		private readonly DateTime start;

		// Token: 0x04000E00 RID: 3584
		private readonly DateTime end;
	}
}
