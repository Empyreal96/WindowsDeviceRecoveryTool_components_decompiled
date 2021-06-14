using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x0200007B RID: 123
	[Export]
	public class HtcBootloaderHelpViewModel : BaseViewModel
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00011380 File Offset: 0x0000F580
		public string GoToMyPhoneHasNotBeenDetectedText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("GoToMyPhoneHasNotBeenDetected"), LocalizationManager.GetTranslation("ButtonMyPhoneWasNotDetected"));
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000113AC File Offset: 0x0000F5AC
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), string.Format(LocalizationManager.GetTranslation("HtcBootloaderMode"), "boot-loader")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.HtcBootloader));
		}
	}
}
