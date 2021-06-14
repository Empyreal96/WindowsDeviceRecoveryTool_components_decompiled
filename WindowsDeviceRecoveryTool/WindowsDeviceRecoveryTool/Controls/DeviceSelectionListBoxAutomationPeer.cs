using System;
using System.Windows.Automation.Peers;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000009 RID: 9
	public class DeviceSelectionListBoxAutomationPeer : ListBoxAutomationPeer
	{
		// Token: 0x0600004C RID: 76 RVA: 0x0000339C File Offset: 0x0000159C
		public DeviceSelectionListBoxAutomationPeer(DeviceSelectionListBox owner) : base(owner)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000033A8 File Offset: 0x000015A8
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000033BC File Offset: 0x000015BC
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer itemAutomationPeer = base.CreateItemAutomationPeer(item);
			return new DeviceSelectionItemAutomationPeer(itemAutomationPeer.Item, itemAutomationPeer.ItemsControlAutomationPeer);
		}
	}
}
