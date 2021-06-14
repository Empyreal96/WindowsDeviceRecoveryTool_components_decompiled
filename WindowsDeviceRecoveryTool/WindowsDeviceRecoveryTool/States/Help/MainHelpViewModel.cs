using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Help
{
	// Token: 0x0200008A RID: 138
	[Export]
	public class MainHelpViewModel : BaseViewModel, ICanHandle<HelpScreenChangedMessage>, ICanHandle<SupportedManufacturersMessage>, ICanHandle
	{
		// Token: 0x060003BA RID: 954 RVA: 0x000119FF File Offset: 0x0000FBFF
		[ImportingConstructor]
		public MainHelpViewModel()
		{
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00011A0C File Offset: 0x0000FC0C
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00011A24 File Offset: 0x0000FC24
		public string HTCBootloaderModeText
		{
			get
			{
				return this.htcBootloaderModeText;
			}
			set
			{
				base.SetValue<string>(() => this.HTCBootloaderModeText, ref this.htcBootloaderModeText, value);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00011A74 File Offset: 0x0000FC74
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00011A8C File Offset: 0x0000FC8C
		public HelpTabs? SelectedTab
		{
			get
			{
				return this.selectedTab;
			}
			set
			{
				if (!(this.selectedTab == value))
				{
					base.SetValue<HelpTabs?>(() => this.SelectedTab, ref this.selectedTab, value);
					HelpTabs valueOrDefault = value.GetValueOrDefault();
					if (value != null)
					{
						switch (valueOrDefault)
						{
						case HelpTabs.LumiaChoose:
							base.Commands.Run((AppController c) => c.SwitchHelpState("LumiaChooseHelpState"));
							break;
						case HelpTabs.LumiaEmergency:
							base.Commands.Run((AppController c) => c.SwitchHelpState("LumiaEmergencyHelpState"));
							break;
						case HelpTabs.LumiaFlashing:
							base.Commands.Run((AppController c) => c.SwitchHelpState("LumiaFlashingHelpState"));
							break;
						case HelpTabs.LumiaNormal:
							base.Commands.Run((AppController c) => c.SwitchHelpState("LumiaNormalHelpState"));
							break;
						case HelpTabs.HtcChoose:
							base.Commands.Run((AppController c) => c.SwitchHelpState("HtcChooseHelpState"));
							break;
						case HelpTabs.HtcBootloader:
							base.Commands.Run((AppController c) => c.SwitchHelpState("HtcBootloaderHelpState"));
							break;
						case HelpTabs.HtcNormal:
							base.Commands.Run((AppController c) => c.SwitchHelpState("HtcNormalHelpState"));
							break;
						}
					}
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00011E78 File Offset: 0x00010078
		// (set) Token: 0x060003C0 RID: 960 RVA: 0x00011E90 File Offset: 0x00010090
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				base.SetValue<string>(() => this.Message, ref this.message, value);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00011EE0 File Offset: 0x000100E0
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00011EF8 File Offset: 0x000100F8
		public bool HtcPluginOn
		{
			get
			{
				return this.htcPluginOn;
			}
			set
			{
				base.SetValue<bool>(() => this.HtcPluginOn, ref this.htcPluginOn, value);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00011F48 File Offset: 0x00010148
		public void Handle(HelpScreenChangedMessage message)
		{
			this.SelectedTab = new HelpTabs?(message.SelectedTab);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00011F7C File Offset: 0x0001017C
		public void Handle(SupportedManufacturersMessage message)
		{
			if (message.Manufacturers != null)
			{
				this.HtcPluginOn = message.Manufacturers.Any((ManufacturerInfo manufacturer) => manufacturer.Type == PhoneTypes.Htc);
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00011FCC File Offset: 0x000101CC
		public override void OnStarted()
		{
			base.OnStarted();
			this.SelectedTab = new HelpTabs?(HelpTabs.LumiaChoose);
			this.Message = null;
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Help"), LocalizationManager.GetTranslation("ManufacturerHeader")));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			this.HTCBootloaderModeText = string.Format(LocalizationManager.GetTranslation("HtcBootloaderMode"), "boot-loader");
			base.Commands.Run((FlowController c) => c.GetSupportedManufacturers());
		}

		// Token: 0x040001B4 RID: 436
		private string message;

		// Token: 0x040001B5 RID: 437
		private HelpTabs? selectedTab;

		// Token: 0x040001B6 RID: 438
		private bool htcPluginOn;

		// Token: 0x040001B7 RID: 439
		private string htcBootloaderModeText;
	}
}
