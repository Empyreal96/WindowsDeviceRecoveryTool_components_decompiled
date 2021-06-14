using System;

namespace System.Windows.Forms
{
	/// <summary>Represents the method that will handle the <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event of a <see cref="T:System.Windows.Forms.ListView" />. </summary>
	/// <param name="sender">The source of the event. </param>
	/// <param name="e">A <see cref="T:System.Windows.Forms.RetrieveVirtualItemEventArgs" />  that contains the event data. </param>
	/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Windows.Forms.RetrieveVirtualItemEventArgs.Item" /> property is not set to an item when the <see cref="E:System.Windows.Forms.ListView.RetrieveVirtualItem" /> event is handled. </exception>
	// Token: 0x02000334 RID: 820
	// (Invoke) Token: 0x06003282 RID: 12930
	public delegate void RetrieveVirtualItemEventHandler(object sender, RetrieveVirtualItemEventArgs e);
}
