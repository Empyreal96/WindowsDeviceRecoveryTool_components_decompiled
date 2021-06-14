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
	// Token: 0x0200008E RID: 142
	[Export]
	public class AwaitFawkesDeviceViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x060003E7 RID: 999 RVA: 0x00012B83 File Offset: 0x00010D83
		[ImportingConstructor]
		public AwaitFawkesDeviceViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00012B98 File Offset: 0x00010D98
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x00012BB0 File Offset: 0x00010DB0
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00012BC8 File Offset: 0x00010DC8
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

		// Token: 0x060003EB RID: 1003 RVA: 0x00012C20 File Offset: 0x00010E20
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("StartRecoveryManually"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.HoloLensAccessory, PhoneModes.Normal);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00012CF0 File Offset: 0x00010EF0
		public override void OnStopped()
		{
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00012D4C File Offset: 0x00010F4C
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (message.Phone.Type == PhoneTypes.HoloLensAccessory)
				{
					this.appContext.CurrentPhone = message.Phone;
					base.Commands.Run((AppController a) => a.SwitchToState("ReadingDeviceInfoState"));
				}
			}
		}

		// Token: 0x040001C9 RID: 457
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
