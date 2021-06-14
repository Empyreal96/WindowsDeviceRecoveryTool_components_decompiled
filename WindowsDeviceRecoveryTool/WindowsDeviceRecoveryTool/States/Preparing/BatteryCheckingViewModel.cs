using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000094 RID: 148
	[Export]
	public class BatteryCheckingViewModel : BaseViewModel, ICanHandle<DeviceBatteryStatusReadMessage>, ICanHandle
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x00013703 File Offset: 0x00011903
		[ImportingConstructor]
		public BatteryCheckingViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.ReadDeviceBatteryStatusCommand = new DelegateCommand<object>(new Action<object>(this.ReadDeviceBatteryStatus));
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00013730 File Offset: 0x00011930
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00013747 File Offset: 0x00011947
		public ICommand ReadDeviceBatteryStatusCommand { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00013750 File Offset: 0x00011950
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00013768 File Offset: 0x00011968
		public string NextCommand
		{
			get
			{
				switch (this.appContext.CurrentPhone.Type)
				{
				case PhoneTypes.Analog:
					return "AbsoluteConfirmationState";
				case PhoneTypes.Mcj:
				case PhoneTypes.Blu:
				case PhoneTypes.Alcatel:
				case PhoneTypes.Acer:
				case PhoneTypes.Trinity:
				case PhoneTypes.Unistrong:
				case PhoneTypes.YEZZ:
				case PhoneTypes.Micromax:
				case PhoneTypes.Funker:
				case PhoneTypes.Diginnos:
				case PhoneTypes.VAIO:
				case PhoneTypes.HP:
				case PhoneTypes.Inversenet:
				case PhoneTypes.Freetel:
				case PhoneTypes.XOLO:
				case PhoneTypes.KM:
				case PhoneTypes.Jenesis:
				case PhoneTypes.Gomobile:
				case PhoneTypes.Lenovo:
				case PhoneTypes.Zebra:
				case PhoneTypes.Honeywell:
				case PhoneTypes.Panasonic:
				case PhoneTypes.TrekStor:
				case PhoneTypes.Wileyfox:
					return "AwaitGenericDeviceState";
				}
				return "FlashingState";
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00013818 File Offset: 0x00011A18
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00013830 File Offset: 0x00011A30
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				base.SetValue<string>(() => this.Description, ref this.description, value);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00013880 File Offset: 0x00011A80
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x00013898 File Offset: 0x00011A98
		public bool BlockFlow
		{
			get
			{
				return this.blockFlow;
			}
			set
			{
				base.SetValue<bool>(() => this.BlockFlow, ref this.blockFlow, value);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000138E8 File Offset: 0x00011AE8
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00013900 File Offset: 0x00011B00
		public bool CheckingBatteryStatus
		{
			get
			{
				return this.checkingBatteryStatus;
			}
			set
			{
				base.SetValue<bool>(() => this.CheckingBatteryStatus, ref this.checkingBatteryStatus, value);
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00013950 File Offset: 0x00011B50
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("BatteryCheckHeader"), ""));
			base.RaisePropertyChanged<string>(() => this.NextCommand);
			this.CheckingBatteryStatus = true;
			this.ReadDeviceBatteryStatus(null);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000139F8 File Offset: 0x00011BF8
		private void CheckBatteries(BatteryStatus status)
		{
			this.SetupPageContent(status);
			bool flag = this.CheckComputerBattery();
			this.BlockFlow = (this.BlockFlow || !flag);
			if (status == BatteryStatus.BatteryOk && flag)
			{
				base.Commands.Run((AppController c) => c.SwitchToState(this.NextCommand));
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00013AC0 File Offset: 0x00011CC0
		private bool CheckComputerBattery()
		{
			PowerStatus powerStatus = SystemInformation.PowerStatus;
			Tracer<BatteryCheckingViewModel>.WriteInformation(string.Concat(new object[]
			{
				"ComputerBattery PowerStatus: ",
				powerStatus.BatteryChargeStatus,
				", Percent: ",
				powerStatus.BatteryLifePercent
			}));
			bool result;
			if ((double)powerStatus.BatteryLifePercent < 0.25 && powerStatus.PowerLineStatus == PowerLineStatus.Offline)
			{
				base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("BatteryCheckHeader"), ""));
				this.Description = LocalizationManager.GetTranslation("ComputerBatteryWarning");
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00013B74 File Offset: 0x00011D74
		private string GetDescription(PhoneTypes phoneType, BatteryStatus deviceBatteryStatus)
		{
			if (deviceBatteryStatus != BatteryStatus.BatteryUnknown)
			{
				switch (phoneType)
				{
				case PhoneTypes.Lumia:
					return LocalizationManager.GetTranslation("LumiaBatteryChecking");
				case PhoneTypes.Analog:
					return LocalizationManager.GetTranslation("AnalogBatteryChecking");
				}
			}
			return LocalizationManager.GetTranslation("BatteryWarning");
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00013BCC File Offset: 0x00011DCC
		private void SetupPageContent(BatteryStatus deviceBatteryStatus)
		{
			switch (deviceBatteryStatus)
			{
			case BatteryStatus.BatteryNotOkBlock:
				this.CheckingBatteryStatus = false;
				this.Description = this.GetDescription(this.appContext.CurrentPhone.Type, deviceBatteryStatus);
				this.BlockFlow = true;
				break;
			case BatteryStatus.BatteryNotOkDoNotBlock:
			case BatteryStatus.BatteryUnknown:
				this.CheckingBatteryStatus = false;
				this.Description = this.GetDescription(this.appContext.CurrentPhone.Type, deviceBatteryStatus);
				this.BlockFlow = false;
				break;
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00013C58 File Offset: 0x00011E58
		private void ReadDeviceBatteryStatus(object obj)
		{
			this.CheckingBatteryStatus = true;
			base.Commands.Run((FlowController c) => c.ReadDeviceBatteryStatus(this.appContext.CurrentPhone, CancellationToken.None));
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00013D10 File Offset: 0x00011F10
		public void Handle(DeviceBatteryStatusReadMessage message)
		{
			if (base.IsStarted)
			{
				this.CheckBatteries(message.Status);
			}
		}

		// Token: 0x040001CF RID: 463
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001D0 RID: 464
		private string description;

		// Token: 0x040001D1 RID: 465
		private bool blockFlow;

		// Token: 0x040001D2 RID: 466
		private bool checkingBatteryStatus;
	}
}
