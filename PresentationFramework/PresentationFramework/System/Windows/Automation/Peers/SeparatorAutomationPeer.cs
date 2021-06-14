using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Separator" /> types to UI Automation.</summary>
	// Token: 0x020002DF RID: 735
	public class SeparatorAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Separator" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" />.</param>
		// Token: 0x060027FB RID: 10235 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public SeparatorAutomationPeer(Separator owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Separator" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Separator".</returns>
		// Token: 0x060027FC RID: 10236 RVA: 0x000BB681 File Offset: 0x000B9881
		protected override string GetClassNameCore()
		{
			return "Separator";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Separator" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Separator" /> enumeration value.</returns>
		// Token: 0x060027FD RID: 10237 RVA: 0x00094927 File Offset: 0x00092B27
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Separator;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x060027FE RID: 10238 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
