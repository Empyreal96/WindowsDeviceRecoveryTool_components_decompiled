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
	// Token: 0x0200009B RID: 155
	[Export]
	public class AwaitAnalogDeviceViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x000146C7 File Offset: 0x000128C7
		[ImportingConstructor]
		public AwaitAnalogDeviceViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x000146DC File Offset: 0x000128DC
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000146F4 File Offset: 0x000128F4
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x0001470C File Offset: 0x0001290C
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

		// Token: 0x06000445 RID: 1093 RVA: 0x00014764 File Offset: 0x00012964
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("StartRecoveryManually"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.Analog, PhoneModes.Flash);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00014834 File Offset: 0x00012A34
		public override void OnStopped()
		{
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00014890 File Offset: 0x00012A90
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (message.Phone.Type == PhoneTypes.Analog)
				{
					this.appContext.CurrentPhone = message.Phone;
					base.Commands.Run((AppController a) => a.SwitchToState("ReadingDeviceInfoState"));
				}
			}
		}

		// Token: 0x040001E0 RID: 480
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
