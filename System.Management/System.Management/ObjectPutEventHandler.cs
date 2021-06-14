using System;

namespace System.Management
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Management.ManagementOperationObserver.ObjectPut" /> event.  </summary>
	/// <param name="sender">The instance of the object for which to invoke this method.</param>
	/// <param name="e">The <see cref="T:System.Management.ObjectPutEventArgs" /> that specifies the reason the event was invoked.</param>
	// Token: 0x02000024 RID: 36
	// (Invoke) Token: 0x0600011B RID: 283
	public delegate void ObjectPutEventHandler(object sender, ObjectPutEventArgs e);
}
