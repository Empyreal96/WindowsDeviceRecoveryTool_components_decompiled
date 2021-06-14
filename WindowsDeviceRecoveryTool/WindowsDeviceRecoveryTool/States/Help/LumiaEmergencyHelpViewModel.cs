using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x02000083 RID: 131
	[Export]
	public class LumiaEmergencyHelpViewModel : BaseViewModel
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00011774 File Offset: 0x0000F974
		public string GoToMyPhoneHasNotBeenDetectedText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("GoToMyPhoneHasNotBeenDetected"), LocalizationManager.GetTranslation("ButtonMyPhoneWasNotDetected"));
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001179F File Offset: 0x0000F99F
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("EmergencyMode")));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.LumiaEmergency));
		}
	}
}
