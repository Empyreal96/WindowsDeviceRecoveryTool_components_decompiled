using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Calendar.DisplayModeChanged" /> event.</summary>
	// Token: 0x0200047C RID: 1148
	public class CalendarModeChangedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarModeChangedEventArgs" /> class. </summary>
		/// <param name="oldMode">The previous mode.</param>
		/// <param name="newMode">The new mode.</param>
		// Token: 0x06004327 RID: 17191 RVA: 0x00133596 File Offset: 0x00131796
		public CalendarModeChangedEventArgs(CalendarMode oldMode, CalendarMode newMode)
		{
			this.OldMode = oldMode;
			this.NewMode = newMode;
		}

		/// <summary>Gets the new mode of the <see cref="T:System.Windows.Controls.Calendar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.CalendarMode" /> that represents the new mode.</returns>
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06004328 RID: 17192 RVA: 0x001335AC File Offset: 0x001317AC
		// (set) Token: 0x06004329 RID: 17193 RVA: 0x001335B4 File Offset: 0x001317B4
		public CalendarMode NewMode { get; private set; }

		/// <summary>Gets the previous mode of the <see cref="T:System.Windows.Controls.Calendar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Controls.CalendarMode" /> that represents the old mode.</returns>
		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x0600432A RID: 17194 RVA: 0x001335BD File Offset: 0x001317BD
		// (set) Token: 0x0600432B RID: 17195 RVA: 0x001335C5 File Offset: 0x001317C5
		public CalendarMode OldMode { get; private set; }
	}
}
