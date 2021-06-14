using System;

namespace System.Management
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Management.ManagementEventWatcher.Stopped" /> event.</summary>
	/// <param name="sender">The instance of the object for which to invoke this method.</param>
	/// <param name="e">The <see cref="T:System.Management.StoppedEventArgs" /> that specifies the reason the event was invoked.</param>
	// Token: 0x02000016 RID: 22
	// (Invoke) Token: 0x06000066 RID: 102
	public delegate void StoppedEventHandler(object sender, StoppedEventArgs e);
}
