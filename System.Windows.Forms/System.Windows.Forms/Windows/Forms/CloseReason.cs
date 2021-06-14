using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies the reason that a form was closed.</summary>
	// Token: 0x02000140 RID: 320
	public enum CloseReason
	{
		/// <summary>The cause of the closure was not defined or could not be determined.</summary>
		// Token: 0x040006CE RID: 1742
		None,
		/// <summary>The operating system is closing all applications before shutting down.</summary>
		// Token: 0x040006CF RID: 1743
		WindowsShutDown,
		/// <summary>The parent form of this multiple document interface (MDI) form is closing.</summary>
		// Token: 0x040006D0 RID: 1744
		MdiFormClosing,
		/// <summary>The user is closing the form through the user interface (UI), for example by clicking the Close button on the form window, selecting Close from the window's control menu, or pressing ALT+F4.</summary>
		// Token: 0x040006D1 RID: 1745
		UserClosing,
		/// <summary>The Microsoft Windows Task Manager is closing the application.</summary>
		// Token: 0x040006D2 RID: 1746
		TaskManagerClosing,
		/// <summary>The owner form is closing.</summary>
		// Token: 0x040006D3 RID: 1747
		FormOwnerClosing,
		/// <summary>The <see cref="M:System.Windows.Forms.Application.Exit" /> method of the <see cref="T:System.Windows.Forms.Application" /> class was invoked. </summary>
		// Token: 0x040006D4 RID: 1748
		ApplicationExitCall
	}
}
