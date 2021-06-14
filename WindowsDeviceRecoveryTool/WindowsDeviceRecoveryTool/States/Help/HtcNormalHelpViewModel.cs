using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x0200007F RID: 127
	[Export]
	public class HtcNormalHelpViewModel : BaseViewModel
	{
		// Token: 0x0600039D RID: 925 RVA: 0x000115AF File Offset: 0x0000F7AF
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("NormalMode")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.HtcNormal));
		}
	}
}
