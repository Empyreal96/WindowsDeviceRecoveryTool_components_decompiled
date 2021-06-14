using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002D5 RID: 725
	internal class PopupRootAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060027A1 RID: 10145 RVA: 0x000B30F9 File Offset: 0x000B12F9
		public PopupRootAutomationPeer(PopupRoot owner) : base(owner)
		{
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000BA7B4 File Offset: 0x000B89B4
		protected override string GetClassNameCore()
		{
			return "Popup";
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x00096AE4 File Offset: 0x00094CE4
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Window;
		}
	}
}
