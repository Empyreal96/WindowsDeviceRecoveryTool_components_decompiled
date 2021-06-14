using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000007 RID: 7
	public class AnnounceablePasswordBoxAutomationPeer : PasswordBoxAutomationPeer, IValueProvider
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003357 File Offset: 0x00001557
		public AnnounceablePasswordBoxAutomationPeer(PasswordBox owner) : base(owner)
		{
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003364 File Offset: 0x00001564
		string IValueProvider.Value
		{
			get
			{
				return string.Empty;
			}
		}
	}
}
