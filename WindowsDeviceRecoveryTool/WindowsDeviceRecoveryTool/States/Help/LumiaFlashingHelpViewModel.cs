using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000085 RID: 133
	[Export]
	public class LumiaFlashingHelpViewModel : BaseViewModel
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00011838 File Offset: 0x0000FA38
		public string GoToMyPhoneHasNotBeenDetectedIfNotDetectedText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("GoToMyPhoneHasNotBeenDetectedIfNotDetected"), LocalizationManager.GetTranslation("ButtonMyPhoneWasNotDetected"));
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00011863 File Offset: 0x0000FA63
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("FlashMode")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.LumiaFlashing));
		}
	}
}
