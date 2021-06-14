using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Calendar.DisplayDateChanged" /> event.</summary>
	// Token: 0x02000477 RID: 1143
	public class CalendarDateChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600430F RID: 17167 RVA: 0x001332BE File Offset: 0x001314BE
		internal CalendarDateChangedEventArgs(DateTime? removedDate, DateTime? addedDate)
		{
			this.RemovedDate = removedDate;
			this.AddedDate = addedDate;
		}

		/// <summary>Gets or sets the date to be newly displayed.</summary>
		/// <returns>The date to be newly displayed.</returns>
		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06004310 RID: 17168 RVA: 0x001332D4 File Offset: 0x001314D4
		// (set) Token: 0x06004311 RID: 17169 RVA: 0x001332DC File Offset: 0x001314DC
		public DateTime? AddedDate { get; private set; }

		/// <summary>Gets or sets the date that was previously displayed.</summary>
		/// <returns>The date that was previously displayed.</returns>
		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06004312 RID: 17170 RVA: 0x001332E5 File Offset: 0x001314E5
		// (set) Token: 0x06004313 RID: 17171 RVA: 0x001332ED File Offset: 0x001314ED
		public DateTime? RemovedDate { get; private set; }
	}
}
