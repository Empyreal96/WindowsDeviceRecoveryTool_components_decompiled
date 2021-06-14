using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Shell
{
	// Token: 0x020000DE RID: 222
	[Export]
	public class ShellViewModel : BaseViewModel, ICanHandle<SwitchStateMessage>, ICanHandle<NotificationMessage>, ICanHandle<IsBusyMessage>, ICanHandle<HeaderMessage>, ICanHandle<IsBackButtonMessage>, ICanHandle
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x00025A55 File Offset: 0x00023C55
		[ImportingConstructor]
		public ShellViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00025A68 File Offset: 0x00023C68
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x00025A80 File Offset: 0x00023C80
		public bool ShowDetailedInfo
		{
			get
			{
				return this.showDetailedInfo;
			}
			set
			{
				base.SetValue<bool>(() => this.ShowDetailedInfo, ref this.showDetailedInfo, value);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00025AD0 File Offset: 0x00023CD0
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x00025AE8 File Offset: 0x00023CE8
		public Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext AppContext
		{
			get
			{
				return this.appContext;
			}
			set
			{
				base.SetValue<Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext>(() => this.AppContext, ref this.appContext, value);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00025B38 File Offset: 0x00023D38
		public string Title
		{
			get
			{
				return string.Format("{0} {1}", AppInfo.AppTitle(), AppInfo.AppVersion());
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x00025B60 File Offset: 0x00023D60
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x00025B78 File Offset: 0x00023D78
		public string HeaderText
		{
			get
			{
				return this.headerText;
			}
			set
			{
				base.SetValue<string>(() => this.HeaderText, ref this.headerText, value);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x00025BC8 File Offset: 0x00023DC8
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x00025BE0 File Offset: 0x00023DE0
		public string SubheaderText
		{
			get
			{
				return this.subheaderText;
			}
			set
			{
				base.SetValue<string>(() => this.SubheaderText, ref this.subheaderText, value);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00025C30 File Offset: 0x00023E30
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x00025C48 File Offset: 0x00023E48
		public bool IsBackButton
		{
			get
			{
				return this.isBackButton;
			}
			set
			{
				base.SetValue<bool>(() => this.IsBackButton, ref this.isBackButton, value);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x00025C98 File Offset: 0x00023E98
		// (set) Token: 0x06000718 RID: 1816 RVA: 0x00025CB0 File Offset: 0x00023EB0
		public bool IsAppBusy
		{
			get
			{
				return this.isAppBusy;
			}
			set
			{
				base.SetValue<bool>(() => this.IsAppBusy, ref this.isAppBusy, value);
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00025D00 File Offset: 0x00023F00
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x00025D18 File Offset: 0x00023F18
		public bool? IsNotificationVisible
		{
			get
			{
				return this.isNotificationVisible;
			}
			set
			{
				base.SetValue<bool?>(() => this.IsNotificationVisible, ref this.isNotificationVisible, value);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00025D68 File Offset: 0x00023F68
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x00025D80 File Offset: 0x00023F80
		public string NotificationHeader
		{
			get
			{
				return this.notificationHeader;
			}
			set
			{
				base.SetValue<string>(() => this.NotificationHeader, ref this.notificationHeader, value);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00025DD0 File Offset: 0x00023FD0
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x00025DE8 File Offset: 0x00023FE8
		public string NotificationText
		{
			get
			{
				return this.notificationText;
			}
			set
			{
				base.SetValue<string>(() => this.NotificationText, ref this.notificationText, value);
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00025E38 File Offset: 0x00024038
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x00025E50 File Offset: 0x00024050
		public string BusyMessage
		{
			get
			{
				return this.busyMessage;
			}
			set
			{
				base.SetValue<string>(() => this.BusyMessage, ref this.busyMessage, value);
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00025EA8 File Offset: 0x000240A8
		public void Handle(SwitchStateMessage message)
		{
			if (string.IsNullOrEmpty(message.State))
			{
				base.Commands.Run((AppController c) => c.EndCurrentState());
			}
			else
			{
				base.Commands.Run((AppController c) => c.SwitchToState(message.State));
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00025FAF File Offset: 0x000241AF
		public void Handle(IsBusyMessage message)
		{
			this.IsAppBusy = message.IsBusy;
			this.BusyMessage = message.Message;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00025FCC File Offset: 0x000241CC
		public void Handle(NotificationMessage message)
		{
			this.IsNotificationVisible = new bool?(message.ShowNotification);
			if (!(this.IsNotificationVisible != true))
			{
				this.NotificationHeader = message.Header;
				this.NotificationText = message.Text;
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0002602E File Offset: 0x0002422E
		public void Handle(HeaderMessage message)
		{
			this.HeaderText = message.Header;
			this.SubheaderText = message.Subheader;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002604B File Offset: 0x0002424B
		public void Handle(IsBackButtonMessage message)
		{
			this.IsBackButton = message.IsBackButton;
		}

		// Token: 0x0400032B RID: 811
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400032C RID: 812
		private string busyMessage;

		// Token: 0x0400032D RID: 813
		private string notificationHeader;

		// Token: 0x0400032E RID: 814
		private string notificationText;

		// Token: 0x0400032F RID: 815
		private string headerText;

		// Token: 0x04000330 RID: 816
		private string subheaderText;

		// Token: 0x04000331 RID: 817
		private bool isAppBusy;

		// Token: 0x04000332 RID: 818
		private bool? isNotificationVisible;

		// Token: 0x04000333 RID: 819
		private bool showDetailedInfo;

		// Token: 0x04000334 RID: 820
		private bool isBackButton;
	}
}
