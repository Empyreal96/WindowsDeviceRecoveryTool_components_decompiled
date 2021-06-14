using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000008 RID: 8
	public class DeviceSelectionListBox : ListBox
	{
		// Token: 0x0600004A RID: 74 RVA: 0x0000337C File Offset: 0x0000157C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DeviceSelectionListBoxAutomationPeer(this);
		}
	}
}
