using System;

namespace System.Windows.Shell
{
	/// <summary>Specifies the state of the progress indicator in the Windows taskbar.</summary>
	// Token: 0x0200014B RID: 331
	public enum TaskbarItemProgressState
	{
		/// <summary>No progress indicator is displayed in the taskbar button.</summary>
		// Token: 0x04001138 RID: 4408
		None,
		/// <summary>A pulsing green indicator is displayed in the taskbar button.</summary>
		// Token: 0x04001139 RID: 4409
		Indeterminate,
		/// <summary>A green progress indicator is displayed in the taskbar button.</summary>
		// Token: 0x0400113A RID: 4410
		Normal,
		/// <summary>A red progress indicator is displayed in the taskbar button.</summary>
		// Token: 0x0400113B RID: 4411
		Error,
		/// <summary>A yellow progress indicator is displayed in the taskbar button.</summary>
		// Token: 0x0400113C RID: 4412
		Paused
	}
}
