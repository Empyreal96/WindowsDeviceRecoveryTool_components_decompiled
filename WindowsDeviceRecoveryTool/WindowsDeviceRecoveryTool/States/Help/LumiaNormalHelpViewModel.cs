using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000087 RID: 135
	[Export]
	public class LumiaNormalHelpViewModel : BaseViewModel
	{
		// Token: 0x060003B3 RID: 947 RVA: 0x000118FB File Offset: 0x0000FAFB
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("NormalMode")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.LumiaNormal));
		}
	}
}
