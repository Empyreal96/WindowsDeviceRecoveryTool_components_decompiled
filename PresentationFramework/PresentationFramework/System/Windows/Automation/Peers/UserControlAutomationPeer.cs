using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.UserControl" /> types to UI Automation.</summary>
	// Token: 0x020002F3 RID: 755
	public class UserControlAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.UserControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" />.</param>
		// Token: 0x0600289A RID: 10394 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public UserControlAutomationPeer(UserControl owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.UserControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The string that contains the name of the control.</returns>
		// Token: 0x0600289B RID: 10395 RVA: 0x000B3444 File Offset: 0x000B1644
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.UserControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.UserControlAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x0600289C RID: 10396 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
