using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Viewport3D" /> types to UI Automation.</summary>
	// Token: 0x020002F5 RID: 757
	public class Viewport3DAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Viewport3D" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" />.</param>
		// Token: 0x0600289F RID: 10399 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public Viewport3DAutomationPeer(Viewport3D owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Viewport3D" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Viewport3D".</returns>
		// Token: 0x060028A0 RID: 10400 RVA: 0x000BCF66 File Offset: 0x000BB166
		protected override string GetClassNameCore()
		{
			return "Viewport3D";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Viewport3D" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.Viewport3DAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x060028A1 RID: 10401 RVA: 0x00094A87 File Offset: 0x00092C87
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
