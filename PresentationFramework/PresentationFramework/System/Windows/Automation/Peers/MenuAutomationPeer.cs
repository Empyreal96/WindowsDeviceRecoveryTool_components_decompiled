using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Menu" /> types to UI Automation.</summary>
	// Token: 0x020002D1 RID: 721
	public class MenuAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.MenuAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Menu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuAutomationPeer" />.</param>
		// Token: 0x0600277F RID: 10111 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public MenuAutomationPeer(Menu owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Menu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Menu".</returns>
		// Token: 0x06002780 RID: 10112 RVA: 0x000BA27A File Offset: 0x000B847A
		protected override string GetClassNameCore()
		{
			return "Menu";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Menu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Menu" /> enumeration value.</returns>
		// Token: 0x06002781 RID: 10113 RVA: 0x0009580F File Offset: 0x00093A0F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Menu;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x06002782 RID: 10114 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
