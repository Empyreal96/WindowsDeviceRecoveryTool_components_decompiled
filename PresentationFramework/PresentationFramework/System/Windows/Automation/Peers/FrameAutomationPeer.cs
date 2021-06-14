using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Frame" /> types to UI Automation.</summary>
	// Token: 0x020002B4 RID: 692
	public class FrameAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FrameAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Frame" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameAutomationPeer" />.</param>
		// Token: 0x06002694 RID: 9876 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public FrameAutomationPeer(Frame owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Frame" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Frame".</returns>
		// Token: 0x06002695 RID: 9877 RVA: 0x000B786B File Offset: 0x000B5A6B
		protected override string GetClassNameCore()
		{
			return "Frame";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Frame" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FrameAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Pane" /> enumeration value.</returns>
		// Token: 0x06002696 RID: 9878 RVA: 0x00094CE7 File Offset: 0x00092EE7
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Pane;
		}
	}
}
