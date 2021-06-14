using System;

namespace System.Windows.Controls
{
	// Token: 0x02000479 RID: 1145
	internal class CalendarDateRangeChangingEventArgs : EventArgs
	{
		// Token: 0x06004323 RID: 17187 RVA: 0x00133556 File Offset: 0x00131756
		public CalendarDateRangeChangingEventArgs(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06004324 RID: 17188 RVA: 0x0013356C File Offset: 0x0013176C
		public DateTime Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06004325 RID: 17189 RVA: 0x00133574 File Offset: 0x00131774
		public DateTime End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x04002827 RID: 10279
		private DateTime _start;

		// Token: 0x04002828 RID: 10280
		private DateTime _end;
	}
}
