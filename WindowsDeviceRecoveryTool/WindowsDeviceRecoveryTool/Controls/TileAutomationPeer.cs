using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000011 RID: 17
	public class TileAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x06000072 RID: 114 RVA: 0x000038CC File Offset: 0x00001ACC
		public TileAutomationPeer(ListViewItem owner) : base(owner)
		{
			this._listViewItem = (ListViewItem)base.Owner;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000038EC File Offset: 0x00001AEC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003900 File Offset: 0x00001B00
		protected override string GetHelpTextCore()
		{
			return this._listViewItem.Content.ToString();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003924 File Offset: 0x00001B24
		protected override bool IsEnabledCore()
		{
			return this._listViewItem.IsFocused;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003944 File Offset: 0x00001B44
		protected override string GetNameCore()
		{
			return this._listViewItem.Content.ToString();
		}

		// Token: 0x0400001B RID: 27
		private readonly ListViewItem _listViewItem;
	}
}
