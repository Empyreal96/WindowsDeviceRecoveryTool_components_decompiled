using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.InkCanvas" /> types to UI Automation.</summary>
	// Token: 0x020002C2 RID: 706
	public class InkCanvasAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />.</param>
		// Token: 0x060026FD RID: 9981 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public InkCanvasAutomationPeer(InkCanvas owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "InkCanvas".</returns>
		// Token: 0x060026FE RID: 9982 RVA: 0x000B8B40 File Offset: 0x000B6D40
		protected override string GetClassNameCore()
		{
			return "InkCanvas";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x060026FF RID: 9983 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
