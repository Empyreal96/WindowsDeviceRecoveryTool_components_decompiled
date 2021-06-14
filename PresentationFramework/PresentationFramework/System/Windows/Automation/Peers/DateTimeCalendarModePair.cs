using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000299 RID: 665
	internal struct DateTimeCalendarModePair
	{
		// Token: 0x06002545 RID: 9541 RVA: 0x000B3DA5 File Offset: 0x000B1FA5
		internal DateTimeCalendarModePair(DateTime date, CalendarMode mode)
		{
			this.ButtonMode = mode;
			this.Date = date;
		}

		// Token: 0x04001B66 RID: 7014
		private CalendarMode ButtonMode;

		// Token: 0x04001B67 RID: 7015
		private DateTime Date;
	}
}
