using System;
using System.Windows.Automation.Peers;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x0200000A RID: 10
	public class DeviceSelectionItemAutomationPeer : ItemAutomationPeer
	{
		// Token: 0x0600004F RID: 79 RVA: 0x000033E7 File Offset: 0x000015E7
		public DeviceSelectionItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer) : base(item, itemsControlAutomationPeer)
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000033F4 File Offset: 0x000015F4
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003408 File Offset: 0x00001608
		protected override string GetClassNameCore()
		{
			return "Button";
		}
	}
}
