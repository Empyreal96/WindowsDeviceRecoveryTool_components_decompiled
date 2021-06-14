using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.CheckBox" /> types to UI Automation.</summary>
	// Token: 0x0200029B RID: 667
	public class CheckBoxAutomationPeer : ToggleButtonAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.CheckBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.CheckBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.CheckBoxAutomationPeer" />.</param>
		// Token: 0x0600254B RID: 9547 RVA: 0x000B3DE4 File Offset: 0x000B1FE4
		public CheckBoxAutomationPeer(CheckBox owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the element associated with this <see cref="T:System.Windows.Automation.Peers.CheckBoxAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "CheckBox".</returns>
		// Token: 0x0600254C RID: 9548 RVA: 0x000B3DED File Offset: 0x000B1FED
		protected override string GetClassNameCore()
		{
			return "CheckBox";
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.AutomationControlType" /> for the element associated with this <see cref="T:System.Windows.Automation.Peers.CheckBoxAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.CheckBox" /> enumeration value.</returns>
		// Token: 0x0600254D RID: 9549 RVA: 0x00094C44 File Offset: 0x00092E44
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.CheckBox;
		}
	}
}
