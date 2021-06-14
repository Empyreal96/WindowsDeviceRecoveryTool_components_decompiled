using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for events that are internal to the <see cref="T:System.Windows.Forms.MonthCalendar" /> control.</summary>
	// Token: 0x02000216 RID: 534
	public class DateBoldEventArgs : EventArgs
	{
		// Token: 0x06002074 RID: 8308 RVA: 0x000A270C File Offset: 0x000A090C
		internal DateBoldEventArgs(DateTime start, int size)
		{
			this.startDate = start;
			this.size = size;
		}

		/// <summary>Gets the first date that is bold.</summary>
		/// <returns>The first date that is bold.</returns>
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x000A2722 File Offset: 0x000A0922
		public DateTime StartDate
		{
			get
			{
				return this.startDate;
			}
		}

		/// <summary>Gets the number of dates that are bold.</summary>
		/// <returns>The number of dates that are bold.</returns>
		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000A272A File Offset: 0x000A092A
		public int Size
		{
			get
			{
				return this.size;
			}
		}

		/// <summary>Gets or sets dates that are bold.</summary>
		/// <returns>The dates that are bold.</returns>
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x000A2732 File Offset: 0x000A0932
		// (set) Token: 0x06002078 RID: 8312 RVA: 0x000A273A File Offset: 0x000A093A
		public int[] DaysToBold
		{
			get
			{
				return this.daysToBold;
			}
			set
			{
				this.daysToBold = value;
			}
		}

		// Token: 0x04000DFC RID: 3580
		private readonly DateTime startDate;

		// Token: 0x04000DFD RID: 3581
		private readonly int size;

		// Token: 0x04000DFE RID: 3582
		private int[] daysToBold;
	}
}
