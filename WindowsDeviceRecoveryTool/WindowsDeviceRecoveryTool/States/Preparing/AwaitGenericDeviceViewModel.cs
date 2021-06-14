using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000092 RID: 146
	[Export]
	public class AwaitGenericDeviceViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x000131C0 File Offset: 0x000113C0
		[ImportingConstructor]
		public AwaitGenericDeviceViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000131D4 File Offset: 0x000113D4
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000131EC File Offset: 0x000113EC
		public string FlashModeText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("Mode"), "Flash");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00013214 File Offset: 0x00011414
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0001322C File Offset: 0x0001142C
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

		// Token: 0x06000402 RID: 1026 RVA: 0x00013284 File Offset: 0x00011484
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("PhoneRestart"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			Tracer<AwaitGenericDeviceViewModel>.WriteInformation("Current phone: ", new object[]
			{
				this.AppContext.CurrentPhone
			});
			PhoneTypes phoneTypes = this.AppContext.SelectedManufacturer;
			if (this.AppContext.CurrentPhone != null)
			{
				if (this.AppContext.CurrentPhone.Type == PhoneTypes.UnknownWp)
				{
					this.AppContext.CurrentPhone.Type = this.AppContext.SelectedManufacturer;
				}
				phoneTypes = this.AppContext.CurrentPhone.Type;
			}
			DetectionParameters detectionParams = new DetectionParameters(phoneTypes, PhoneModes.Flash);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x000133E8 File Offset: 0x000115E8
		public override void OnStopped()
		{
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00013444 File Offset: 0x00011644
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				PhoneTypes type = message.Phone.Type;
				switch (type)
				{
				case PhoneTypes.Lg:
					break;
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
					if (this.appContext.CurrentPhone.LocationPath == null || this.appContext.CurrentPhone.LocationPath == message.Phone.LocationPath)
					{
						this.appContext.CurrentPhone.Path = message.Phone.Path;
						base.Commands.Run((AppController a) => a.SwitchToState("FlashingState"));
					}
					else
					{
						Tracer<AwaitGenericDeviceViewModel>.WriteWarning("Found device but location paths are different!", new object[]
						{
							message.Phone
						});
					}
					goto IL_25A;
				case PhoneTypes.HoloLensAccessory:
					goto IL_25A;
				default:
					if (type != PhoneTypes.Generic)
					{
						goto IL_25A;
					}
					break;
				}
				if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.LocationPath == message.Phone.LocationPath)
				{
					message.Phone.SalesName = this.appContext.CurrentPhone.SalesName;
				}
				this.appContext.CurrentPhone = message.Phone;
				base.Commands.Run((AppController a) => a.SwitchToState("ReadingDeviceInfoState"));
				IL_25A:;
			}
		}

		// Token: 0x040001CD RID: 461
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
