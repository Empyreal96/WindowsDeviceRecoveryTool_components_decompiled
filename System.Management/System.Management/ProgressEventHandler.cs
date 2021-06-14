using System;

namespace System.Management
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Management.ManagementOperationObserver.Progress" /> event.</summary>
	/// <param name="sender">The instance of the object for which to invoke this method.</param>
	/// <param name="e">The <see cref="T:System.Management.ProgressEventArgs" /> that specifies the reason the event was invoked.</param>
	// Token: 0x02000023 RID: 35
	// (Invoke) Token: 0x06000117 RID: 279
	public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
}
