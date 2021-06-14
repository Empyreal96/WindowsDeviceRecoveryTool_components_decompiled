using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000074 RID: 116
	[Export]
	public class LumiaOldFlashingHelpViewModel : BaseViewModel
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00010A04 File Offset: 0x0000EC04
		public string GoToMyPhoneHasNotBeenDetectedIfNotDetectedText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("GoToMyPhoneHasNotBeenDetectedIfNotDetected"), LocalizationManager.GetTranslation("ButtonMyPhoneWasNotDetected"));
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010A2F File Offset: 0x0000EC2F
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("FlashMode")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.LumiaFlashing));
		}
	}
}
