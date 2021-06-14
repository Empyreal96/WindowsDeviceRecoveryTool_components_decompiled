using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the <see cref="T:System.Windows.UIElement" /> sub-tree for the data items in a <see cref="T:System.Windows.Controls.ListBox" /> to UI Automation. </summary>
	// Token: 0x020002CE RID: 718
	public class ListBoxItemWrapperAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ListBoxItemWrapperAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemWrapperAutomationPeer" />.</param>
		// Token: 0x06002771 RID: 10097 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ListBoxItemWrapperAutomationPeer(ListBoxItem owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemWrapperAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ListBoxItem".</returns>
		// Token: 0x06002772 RID: 10098 RVA: 0x000BA0E2 File Offset: 0x000B82E2
		protected override string GetClassNameCore()
		{
			return "ListBoxItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxItemWrapperAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ListItem" /> enumeration value.</returns>
		// Token: 0x06002773 RID: 10099 RVA: 0x0001321D File Offset: 0x0001141D
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ListItem;
		}
	}
}
