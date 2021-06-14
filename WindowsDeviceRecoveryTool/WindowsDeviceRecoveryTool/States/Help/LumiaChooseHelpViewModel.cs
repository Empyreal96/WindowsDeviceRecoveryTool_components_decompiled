using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000080 RID: 128
	[Export]
	public class LumiaChooseHelpViewModel : BaseViewModel
	{
		// Token: 0x0600039F RID: 927 RVA: 0x000115F1 File Offset: 0x0000F7F1
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), "Lumia"));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.LumiaChoose));
		}
	}
}
