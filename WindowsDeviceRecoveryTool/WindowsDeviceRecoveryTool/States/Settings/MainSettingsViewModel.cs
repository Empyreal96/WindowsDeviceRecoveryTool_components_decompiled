using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000D5 RID: 213
	[Export]
	public class MainSettingsViewModel : BaseViewModel, ICanHandle<SettingsPreviousStateMessage>, ICanHandle
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00021CCC File Offset: 0x0001FECC
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x00021CEC File Offset: 0x0001FEEC
		public SettingsPage? SelectedPage
		{
			get
			{
				return this.selectedPage;
			}
			set
			{
				if (!(this.selectedPage == value))
				{
					base.SetValue<SettingsPage?>(() => this.SelectedPage, ref this.selectedPage, value);
					SettingsPage valueOrDefault = value.GetValueOrDefault();
					if (value != null)
					{
						string nextState;
						switch (valueOrDefault)
						{
						case SettingsPage.Network:
							nextState = "NetworkState";
							break;
						case SettingsPage.Preferences:
							nextState = "PreferencesState";
							break;
						case SettingsPage.Troubleshooting:
							nextState = "TraceState";
							break;
						case SettingsPage.Packages:
							nextState = "PackagesState";
							break;
						case SettingsPage.ApplicationData:
							nextState = "ApplicationDataState";
							break;
						default:
							goto IL_F9;
						}
						base.Commands.Run((AppController c) => c.SwitchSettingsState(nextState));
					}
					IL_F9:;
				}
			}
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00021E68 File Offset: 0x00020068
		public override void OnStarted()
		{
			this.SelectedPage = new SettingsPage?(this.previousPage ?? SettingsPage.Preferences);
			this.previousPage = null;
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.OnStarted();
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00021ED8 File Offset: 0x000200D8
		public override void OnStopped()
		{
			Settings.Default.Save();
			base.Commands.Run((SettingsController c) => c.SetApplicationSettings());
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00021F40 File Offset: 0x00020140
		public void Handle(SettingsPreviousStateMessage message)
		{
			if (!string.IsNullOrEmpty(message.PreviousState))
			{
				string previousState = message.PreviousState;
				if (previousState != null)
				{
					if (!(previousState == "TraceState"))
					{
						if (previousState == "PackagesState")
						{
							this.previousPage = new SettingsPage?(SettingsPage.Packages);
						}
					}
					else
					{
						this.previousPage = new SettingsPage?(SettingsPage.Troubleshooting);
					}
				}
			}
		}

		// Token: 0x040002BD RID: 701
		private SettingsPage? selectedPage;

		// Token: 0x040002BE RID: 702
		private SettingsPage? previousPage;
	}
}
