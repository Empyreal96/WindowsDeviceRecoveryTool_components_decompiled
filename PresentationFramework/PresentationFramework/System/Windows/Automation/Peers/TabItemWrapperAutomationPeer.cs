using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the <see cref="T:System.Windows.UIElement" /> subtree for the data items in a <see cref="T:System.Windows.Controls.TabControl" /> to UI Automation.</summary>
	// Token: 0x020002E5 RID: 741
	public class TabItemWrapperAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.TabItemWrapperAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.TabItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.TabItemWrapperAutomationPeer" />.</param>
		// Token: 0x06002819 RID: 10265 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public TabItemWrapperAutomationPeer(TabItem owner) : base(owner)
		{
		}
	}
}
