using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Automation.Peers
{
	/// <summary>Allows a customized view of a <see cref="T:System.Windows.Controls.ListView" /> that derives from <see cref="T:System.Windows.Controls.ViewBase" /> to implement automation peer features that are specific to the custom view.</summary>
	// Token: 0x020002CA RID: 714
	public interface IViewAutomationPeer
	{
		/// <summary>Gets the control type for the element that is associated with this <see cref="T:System.Windows.Automation.Peers.IViewAutomationPeer" />.</summary>
		/// <returns>A value in the <see cref="T:System.Windows.Automation.Peers.AutomationControlType" /> enumeration.</returns>
		// Token: 0x0600275E RID: 10078
		AutomationControlType GetAutomationControlType();

		/// <summary>Gets the control pattern that is associated with the specified <paramref name="patternInterface" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>Return the object that implements the control pattern. If this method returns null, the return value from <see cref="M:System.Windows.Automation.Peers.IViewAutomationPeer.GetPattern(System.Windows.Automation.Peers.PatternInterface)" /> is used.</returns>
		// Token: 0x0600275F RID: 10079
		object GetPattern(PatternInterface patternInterface);

		/// <summary>Gets the collection of immediate child elements of the specified UI Automation peer.</summary>
		/// <param name="children">The automation peers for the list items.</param>
		/// <returns>The automation peers for all items in the control. If the view contains interactive or informational elements in addition to the list items, automation peers for these elements must be added to the list.</returns>
		// Token: 0x06002760 RID: 10080
		List<AutomationPeer> GetChildren(List<AutomationPeer> children);

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> that is associated with the <see cref="T:System.Windows.Controls.ListView" /> that is used by this <see cref="T:System.Windows.Automation.Peers.IViewAutomationPeer" />. </param>
		/// <returns>The new <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> instance.</returns>
		// Token: 0x06002761 RID: 10081
		ItemAutomationPeer CreateItemAutomationPeer(object item);

		/// <summary>Called by <see cref="T:System.Windows.Controls.ListView" /> when the collection of items changes.</summary>
		/// <param name="e">A <see cref="T:System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x06002762 RID: 10082
		void ItemsChanged(NotifyCollectionChangedEventArgs e);

		/// <summary>Called when the custom view is no longer applied to the <see cref="T:System.Windows.Controls.ListView" />.</summary>
		// Token: 0x06002763 RID: 10083
		void ViewDetached();
	}
}
