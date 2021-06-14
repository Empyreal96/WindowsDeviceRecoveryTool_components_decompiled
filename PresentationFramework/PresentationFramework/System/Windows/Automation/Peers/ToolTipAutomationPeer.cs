using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ToolTip" /> types to UI Automation.</summary>
	// Token: 0x020002EF RID: 751
	public class ToolTipAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />.</param>
		// Token: 0x0600285D RID: 10333 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ToolTipAutomationPeer(ToolTip owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ToolTip".</returns>
		// Token: 0x0600285E RID: 10334 RVA: 0x000BC4BB File Offset: 0x000BA6BB
		protected override string GetClassNameCore()
		{
			return "ToolTip";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ToolTip" /> enumeration value.</returns>
		// Token: 0x0600285F RID: 10335 RVA: 0x000BC4C2 File Offset: 0x000BA6C2
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ToolTip;
		}
	}
}
