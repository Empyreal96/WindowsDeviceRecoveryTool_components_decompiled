using System;
using MS.Internal.AppModel;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002F4 RID: 756
	internal class RootBrowserWindowAutomationPeer : WindowAutomationPeer
	{
		// Token: 0x0600289D RID: 10397 RVA: 0x000BA639 File Offset: 0x000B8839
		public RootBrowserWindowAutomationPeer(RootBrowserWindow owner) : base(owner)
		{
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000BCF5F File Offset: 0x000BB15F
		protected override string GetClassNameCore()
		{
			return "RootBrowserWindow";
		}
	}
}
