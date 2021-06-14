using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles.Controls
{
	// Token: 0x0200000D RID: 13
	public sealed class TitleBar : Border
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00003690 File Offset: 0x00001890
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TitleBar.TitleBarAutomationPeer(this);
		}

		// Token: 0x0200000E RID: 14
		private sealed class TitleBarAutomationPeer : FrameworkElementAutomationPeer
		{
			// Token: 0x06000062 RID: 98 RVA: 0x000036B0 File Offset: 0x000018B0
			public TitleBarAutomationPeer(TitleBar owner) : base(owner)
			{
			}

			// Token: 0x06000063 RID: 99 RVA: 0x000036BC File Offset: 0x000018BC
			protected override AutomationControlType GetAutomationControlTypeCore()
			{
				return AutomationControlType.TitleBar;
			}

			// Token: 0x06000064 RID: 100 RVA: 0x000036D0 File Offset: 0x000018D0
			protected override string GetNameCore()
			{
				return string.Empty;
			}

			// Token: 0x06000065 RID: 101 RVA: 0x000036E8 File Offset: 0x000018E8
			protected override bool IsContentElementCore()
			{
				return false;
			}

			// Token: 0x06000066 RID: 102 RVA: 0x000036FC File Offset: 0x000018FC
			protected override string GetAutomationIdCore()
			{
				return "TitleBar";
			}
		}
	}
}
