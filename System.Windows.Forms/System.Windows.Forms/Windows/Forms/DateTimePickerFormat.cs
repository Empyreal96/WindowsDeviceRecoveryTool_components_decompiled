using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the date and time format the <see cref="T:System.Windows.Forms.DateTimePicker" /> control displays.</summary>
	// Token: 0x0200021B RID: 539
	public enum DateTimePickerFormat
	{
		/// <summary>The <see cref="T:System.Windows.Forms.DateTimePicker" /> control displays the date/time value in the long date format set by the user's operating system.</summary>
		// Token: 0x04000E21 RID: 3617
		Long = 1,
		/// <summary>The <see cref="T:System.Windows.Forms.DateTimePicker" /> control displays the date/time value in the short date format set by the user's operating system.</summary>
		// Token: 0x04000E22 RID: 3618
		Short,
		/// <summary>The <see cref="T:System.Windows.Forms.DateTimePicker" /> control displays the date/time value in the time format set by the user's operating system.</summary>
		// Token: 0x04000E23 RID: 3619
		Time = 4,
		/// <summary>The <see cref="T:System.Windows.Forms.DateTimePicker" /> control displays the date/time value in a custom format. For more information, see <see cref="P:System.Windows.Forms.DateTimePicker.CustomFormat" />.</summary>
		// Token: 0x04000E24 RID: 3620
		Custom = 8
	}
}
