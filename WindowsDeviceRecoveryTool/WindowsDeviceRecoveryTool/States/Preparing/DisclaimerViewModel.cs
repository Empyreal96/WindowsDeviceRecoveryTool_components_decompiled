using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A4 RID: 164
	[Export]
	public class DisclaimerViewModel : BaseViewModel, ICanHandle<DeviceConnectionStatusReadMessage>, ICanHandle
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x00015FE8 File Offset: 0x000141E8
		[ImportingConstructor]
		public DisclaimerViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.NextButtonCommand = new DelegateCommand<object>(new Action<object>(this.NextButtonSelected));
			this.SurveyCommand = new DelegateCommand<object>(new Action<object>(this.OpenSurvey));
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00016038 File Offset: 0x00014238
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x0001604F File Offset: 0x0001424F
		public ICommand SurveyCommand { get; private set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00016058 File Offset: 0x00014258
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0001606F File Offset: 0x0001426F
		public ICommand NextButtonCommand { get; private set; }

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00016078 File Offset: 0x00014278
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00016090 File Offset: 0x00014290
		public string PhoneSettingsBackupPath
		{
			get
			{
				return this.phoneSettingsBackupPath;
			}
			set
			{
				base.SetValue<string>(() => this.PhoneSettingsBackupPath, ref this.phoneSettingsBackupPath, value);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x000160E0 File Offset: 0x000142E0
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000160F8 File Offset: 0x000142F8
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DisclaimerHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			this.PhoneSettingsBackupPath = LocalizationManager.GetTranslation(this.appContext.CurrentPhone.IsWp10Device() ? "PhoneSettingsBackupPathWin10" : "PhoneSettingsBackupPathWin8");
			if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.Type == PhoneTypes.Analog)
			{
				base.Commands.Run((FlowController c) => c.CheckIfDeviceStillConnected(this.appContext.CurrentPhone, CancellationToken.None));
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001623C File Offset: 0x0001443C
		private void NextButtonSelected(object obj)
		{
			if (this.appContext.CurrentPhone.IsPhoneDeviceType())
			{
				base.Commands.Run((AppController c) => c.SwitchToState("SurveyState"));
			}
			else if (this.appContext.CurrentPhone.PackageFileInfo.OfflinePackage)
			{
				base.Commands.Run((AppController c) => c.SwitchToState("PackageIntegrityCheckState"));
			}
			else
			{
				base.Commands.Run((AppController c) => c.SwitchToState("DownloadPackageState"));
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x000163BC File Offset: 0x000145BC
		public void Handle(DeviceConnectionStatusReadMessage message)
		{
			if (base.IsStarted && !message.Status)
			{
				throw new DeviceNotFoundException();
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000163E8 File Offset: 0x000145E8
		private void OpenSurvey(object obj)
		{
			base.Commands.Run((AppController c) => c.SwitchToState("SurveyState"));
		}

		// Token: 0x040001FB RID: 507
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001FC RID: 508
		private string phoneSettingsBackupPath;
	}
}
