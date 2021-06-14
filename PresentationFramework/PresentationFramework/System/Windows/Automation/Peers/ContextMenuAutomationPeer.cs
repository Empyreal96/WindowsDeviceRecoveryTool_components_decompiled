using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ContextMenu" /> types to UI Automation.</summary>
	// Token: 0x0200029E RID: 670
	public class ContextMenuAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ContextMenu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" />.</param>
		// Token: 0x06002562 RID: 9570 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public ContextMenuAutomationPeer(ContextMenu owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ContextMenu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ContextMenu".</returns>
		// Token: 0x06002563 RID: 9571 RVA: 0x000B4102 File Offset: 0x000B2302
		protected override string GetClassNameCore()
		{
			return "ContextMenu";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ContextMenu" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContextMenuAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Menu" /> enumeration value.</returns>
		// Token: 0x06002564 RID: 9572 RVA: 0x0009580F File Offset: 0x00093A0F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Menu;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.IsContentElement" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x06002565 RID: 9573 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
