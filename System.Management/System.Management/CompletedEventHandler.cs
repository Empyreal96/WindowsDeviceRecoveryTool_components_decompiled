using System;

namespace System.Management
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Management.ManagementOperationObserver.Completed" /> event.     </summary>
	/// <param name="sender">The instance of the object for which to invoke this method.</param>
	/// <param name="e">The <see cref="T:System.Management.CompletedEventArgs" /> that specifies the reason the event was invoked.</param>
	// Token: 0x02000022 RID: 34
	// (Invoke) Token: 0x06000113 RID: 275
	public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);
}
