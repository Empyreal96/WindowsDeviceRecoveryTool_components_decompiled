using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000091 RID: 145
	[Export]
	public class AwaitHtcViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x00012EC8 File Offset: 0x000110C8
		[ImportingConstructor]
		public AwaitHtcViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00012EDC File Offset: 0x000110DC
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x00012EF4 File Offset: 0x000110F4
		public string HtcBootLoaderModeText
		{
			get
			{
				return string.Format(LocalizationManager.GetTranslation("HtcBootloaderMode"), "boot-loader");
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00012F1C File Offset: 0x0001111C
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00012F34 File Offset: 0x00011134
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

		// Token: 0x060003FA RID: 1018 RVA: 0x00012F8C File Offset: 0x0001118C
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("PhoneRestart"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.Htc, PhoneModes.Flash);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001305C File Offset: 0x0001125C
		public override void OnStopped()
		{
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000130B8 File Offset: 0x000112B8
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (message.Phone.Type == PhoneTypes.Htc)
				{
					if (this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.LocationPath == message.Phone.LocationPath)
					{
						message.Phone.SalesName = this.appContext.CurrentPhone.SalesName;
					}
					this.appContext.CurrentPhone = message.Phone;
					base.Commands.Run((AppController a) => a.SwitchToState("ReadingDeviceInfoState"));
				}
			}
		}

		// Token: 0x040001CC RID: 460
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
