using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x0200007D RID: 125
	[Export]
	public class HtcChooseHelpViewModel : BaseViewModel
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000396 RID: 918 RVA: 0x0001149C File Offset: 0x0000F69C
		// (set) Token: 0x06000397 RID: 919 RVA: 0x000114B4 File Offset: 0x0000F6B4
		public string HTCBootloaderModeText
		{
			get
			{
				return this.htcBootloaderModeText;
			}
			set
			{
				base.SetValue<string>(() => this.htcBootloaderModeText, ref this.htcBootloaderModeText, value);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000114F4 File Offset: 0x0000F6F4
		public override void OnStarted()
		{
			this.HTCBootloaderModeText = string.Format(LocalizationManager.GetTranslation("HtcBootloaderMode"), "boot-loader");
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), "HTC"));
			base.EventAggregator.Publish<HelpScreenChangedMessage>(new HelpScreenChangedMessage(HelpTabs.HtcChoose));
		}

		// Token: 0x040001A4 RID: 420
		private string htcBootloaderModeText;
	}
}
