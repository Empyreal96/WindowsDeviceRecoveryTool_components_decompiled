using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies whether a single or multiple dates can be selected in a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x0200047E RID: 1150
	public enum CalendarSelectionMode
	{
		/// <summary>A single date can be selected. Use the <see cref="P:System.Windows.Controls.Calendar.SelectedDate" /> property to retrieve the selected date.</summary>
		// Token: 0x04002830 RID: 10288
		SingleDate,
		/// <summary>A single range of dates can be selected. Use the <see cref="P:System.Windows.Controls.Calendar.SelectedDates" /> property to retrieve the selected dates.</summary>
		// Token: 0x04002831 RID: 10289
		SingleRange,
		/// <summary>Multiple non-contiguous ranges of dates can be selected. Use the <see cref="P:System.Windows.Controls.Calendar.SelectedDates" /> property to retrieve the selected dates.</summary>
		// Token: 0x04002832 RID: 10290
		MultipleRange,
		/// <summary>No selections are allowed.</summary>
		// Token: 0x04002833 RID: 10291
		None
	}
}
