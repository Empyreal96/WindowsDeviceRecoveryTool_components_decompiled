using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000A2 RID: 162
	[Export]
	public class ManualHtcRestartViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00015D38 File Offset: 0x00013F38
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x00015D50 File Offset: 0x00013F50
		public string SubHeader
		{
			get
			{
				return this.subHeader;
			}
			set
			{
				base.SetValue<string>(() => this.SubHeader, ref this.subHeader, value);
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00015DA8 File Offset: 0x00013FA8
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("RestartDeviceHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			this.SubHeader = string.Format(LocalizationManager.GetTranslation("ManualRestartHtcInfo"), LocalizationManager.GetTranslation("ButtonCancel"));
			DetectionParameters detectionParams = new DetectionParameters(PhoneTypes.Htc, PhoneModes.Normal);
			base.Commands.Run((FlowController c) => c.StartDeviceDetection(detectionParams));
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00015E8C File Offset: 0x0001408C
		public override void OnStopped()
		{
			base.OnStopped();
			base.Commands.Run((FlowController c) => c.StopDeviceDetection());
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00015EF0 File Offset: 0x000140F0
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (message.Phone.Type == PhoneTypes.Htc)
				{
					base.Commands.Run((AppController c) => c.SwitchToState("AwaitHtcState"));
				}
			}
		}

		// Token: 0x040001F9 RID: 505
		private string subHeader;
	}
}
