using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D7 RID: 215
	[Export]
	public class NetworkViewModel : BaseViewModel
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x00022154 File Offset: 0x00020354
		[ImportingConstructor]
		public NetworkViewModel(EventAggregator eventAggregator)
		{
			this.eventAggregator = eventAggregator;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00022168 File Offset: 0x00020368
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x000221A0 File Offset: 0x000203A0
		public bool UseManualProxy
		{
			get
			{
				return Settings.Default.UseManualProxy;
			}
			set
			{
				base.SetValue<bool>(() => this.UseManualProxy, delegate()
				{
					Settings.Default.UseManualProxy = value;
				});
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x0002220C File Offset: 0x0002040C
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x00022234 File Offset: 0x00020434
		public string Password
		{
			get
			{
				return new Credentials().DecryptString(Settings.Default.ProxyPassword);
			}
			set
			{
				Settings.Default.ProxyPassword = new Credentials().EncryptString(value);
				base.RaisePropertyChanged<string>(() => this.Password);
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00022294 File Offset: 0x00020494
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000222CC File Offset: 0x000204CC
		public string ProxyUsername
		{
			get
			{
				return Settings.Default.ProxyUsername;
			}
			set
			{
				base.SetValue<string>(() => this.ProxyUsername, delegate()
				{
					Settings.Default.ProxyUsername = value;
				});
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00022338 File Offset: 0x00020538
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00022370 File Offset: 0x00020570
		public int ProxyPort
		{
			get
			{
				return Settings.Default.ProxyPort;
			}
			set
			{
				base.SetValue<int>(() => this.ProxyPort, delegate()
				{
					Settings.Default.ProxyPort = value;
				});
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x000223DC File Offset: 0x000205DC
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00022414 File Offset: 0x00020614
		public string ProxyAddress
		{
			get
			{
				return Settings.Default.ProxyAddress;
			}
			set
			{
				base.SetValue<string>(() => this.ProxyAddress, delegate()
				{
					Settings.Default.ProxyAddress = value;
				});
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00022480 File Offset: 0x00020680
		public override void OnStarted()
		{
			this.eventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Settings"), LocalizationManager.GetTranslation("Network")));
			this.eventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.RaisePropertyChanged<int>(() => this.ProxyPort);
			base.RaisePropertyChanged<string>(() => this.ProxyAddress);
			base.RaisePropertyChanged<string>(() => this.ProxyUsername);
			base.RaisePropertyChanged<string>(() => this.Password);
			base.RaisePropertyChanged<bool>(() => this.UseManualProxy);
			base.OnStarted();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x000225F8 File Offset: 0x000207F8
		public override void OnStopped()
		{
			base.Commands.Run((SettingsController controller) => controller.SetProxy(null));
			Settings.Default.Save();
		}

		// Token: 0x040002C7 RID: 711
		private readonly EventAggregator eventAggregator;
	}
}
