using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies options on a <see cref="T:System.Windows.Forms.MessageBox" />.</summary>
	// Token: 0x020002ED RID: 749
	[Flags]
	public enum MessageBoxOptions
	{
		/// <summary>The message box is displayed on the active desktop.</summary>
		// Token: 0x0400134B RID: 4939
		ServiceNotification = 2097152,
		/// <summary>The message box is displayed on the active desktop.</summary>
		// Token: 0x0400134C RID: 4940
		DefaultDesktopOnly = 131072,
		/// <summary>The message box text is right-aligned.</summary>
		// Token: 0x0400134D RID: 4941
		RightAlign = 524288,
		/// <summary>Specifies that the message box text is displayed with right to left reading order.</summary>
		// Token: 0x0400134E RID: 4942
		RtlReading = 1048576
	}
}
