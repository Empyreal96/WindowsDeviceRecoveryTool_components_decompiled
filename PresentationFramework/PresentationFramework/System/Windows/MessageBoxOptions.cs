using System;

namespace System.Windows
{
	/// <summary>Specifies special display options for a message box.</summary>
	// Token: 0x020000DA RID: 218
	[Flags]
	public enum MessageBoxOptions
	{
		/// <summary>No options are set.</summary>
		// Token: 0x0400074C RID: 1868
		None = 0,
		/// <summary>The message box is displayed on the currently active desktop even if a user is not logged on to the computer. Specifies that the message box is displayed from a Microsoft .NET Framework windows service application in order to notify the user of an event. </summary>
		// Token: 0x0400074D RID: 1869
		ServiceNotification = 2097152,
		/// <summary>The message box is displayed on the default desktop of the interactive window station. Specifies that the message box is displayed from a Microsoft .NET Framework windows service application in order to notify the user of an event. </summary>
		// Token: 0x0400074E RID: 1870
		DefaultDesktopOnly = 131072,
		/// <summary>The message box text and title bar caption are right-aligned.</summary>
		// Token: 0x0400074F RID: 1871
		RightAlign = 524288,
		/// <summary>All text, buttons, icons, and title bars are displayed right-to-left.</summary>
		// Token: 0x04000750 RID: 1872
		RtlReading = 1048576
	}
}
