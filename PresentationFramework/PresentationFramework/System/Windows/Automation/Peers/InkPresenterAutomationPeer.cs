using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.InkPresenter" /> types to UI Automation.</summary>
	// Token: 0x020002C3 RID: 707
	public class InkPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.InkPresenterAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.InkPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkPresenterAutomationPeer" />.</param>
		// Token: 0x06002700 RID: 9984 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public InkPresenterAutomationPeer(InkPresenter owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.InkPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkPresenterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "InkPresenter".</returns>
		// Token: 0x06002701 RID: 9985 RVA: 0x000B8B47 File Offset: 0x000B6D47
		protected override string GetClassNameCore()
		{
			return "InkPresenter";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.InkPresenter" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkPresenterAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x06002702 RID: 9986 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
