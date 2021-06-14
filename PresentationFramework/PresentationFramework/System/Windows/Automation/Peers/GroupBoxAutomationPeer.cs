using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.GroupBox" /> types to UI Automation.</summary>
	// Token: 0x020002BD RID: 701
	public class GroupBoxAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.GroupBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.GroupBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupBoxAutomationPeer" />.</param>
		// Token: 0x060026E0 RID: 9952 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public GroupBoxAutomationPeer(GroupBox owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.GroupBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "GroupBox".</returns>
		// Token: 0x060026E1 RID: 9953 RVA: 0x000B850C File Offset: 0x000B670C
		protected override string GetClassNameCore()
		{
			return "GroupBox";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.GroupBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.GroupBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Group" /> enumeration value.</returns>
		// Token: 0x060026E2 RID: 9954 RVA: 0x000B7289 File Offset: 0x000B5489
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Group;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The string that contains the label.</returns>
		// Token: 0x060026E3 RID: 9955 RVA: 0x000B8514 File Offset: 0x000B6714
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore))
			{
				GroupBox groupBox = (GroupBox)base.Owner;
				if (groupBox.Header is string)
				{
					return AccessText.RemoveAccessKeyMarker(nameCore);
				}
			}
			return nameCore;
		}
	}
}
