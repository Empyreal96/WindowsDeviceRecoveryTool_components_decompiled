using System;
using System.Windows.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Documents.FixedPage" /> types to UI Automation.</summary>
	// Token: 0x020002B0 RID: 688
	public class FixedPageAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FixedPageAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Documents.FixedPage" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FixedPageAutomationPeer" />.</param>
		// Token: 0x0600267B RID: 9851 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public FixedPageAutomationPeer(FixedPage owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Documents.FixedPage" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FixedPageAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "FixedPage".</returns>
		// Token: 0x0600267C RID: 9852 RVA: 0x000B73F0 File Offset: 0x000B55F0
		protected override string GetClassNameCore()
		{
			return "FixedPage";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Documents.FixedPage" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FixedPageAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Pane" /> enumeration value.</returns>
		// Token: 0x0600267D RID: 9853 RVA: 0x00094CE7 File Offset: 0x00092EE7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}
	}
}
