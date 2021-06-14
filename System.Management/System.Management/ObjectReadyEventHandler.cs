using System;

namespace System.Management
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Management.ManagementOperationObserver.ObjectReady" /> event.</summary>
	/// <param name="sender">The instance of the object for which to invoke this method.</param>
	/// <param name="e">The <see cref="T:System.Management.ObjectReadyEventArgs" /> that specifies the reason the event was invoked.</param>
	// Token: 0x02000021 RID: 33
	// (Invoke) Token: 0x0600010F RID: 271
	public delegate void ObjectReadyEventHandler(object sender, ObjectReadyEventArgs e);
}
