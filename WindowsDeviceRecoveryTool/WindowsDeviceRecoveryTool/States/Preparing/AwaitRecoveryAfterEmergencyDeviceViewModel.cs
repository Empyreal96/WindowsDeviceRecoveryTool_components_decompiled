using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000097 RID: 151
	[Export]
	public class AwaitRecoveryAfterEmergencyDeviceViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x00014082 File Offset: 0x00012282
		[ImportingConstructor]
		public AwaitRecoveryAfterEmergencyDeviceViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00014098 File Offset: 0x00012298
		public string RebootPhoneInstructions
		{
			get
			{
				return LocalizationManager.GetTranslation("AwaitRecoveryAfterEmergencyFlashingInstruction");
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x000140B4 File Offset: 0x000122B4
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x000140CC File Offset: 0x000122CC
		public bool AreInstructionsVisible
		{
			get
			{
				return this.areInstructionsVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.AreInstructionsVisible, ref this.areInstructionsVisible, value);
				base.RaisePropertyChanged<bool>(() => this.IsCancelVisible);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00014158 File Offset: 0x00012358
		public bool IsCancelVisible
		{
			get
			{
				return this.areInstructionsVisible;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00014170 File Offset: 0x00012370
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00014188 File Offset: 0x00012388
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

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x000141D8 File Offset: 0x000123D8
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000141F0 File Offset: 0x000123F0
		public override void OnStarted()
		{
			this.timer = new Timer(new TimerCallback(this.OnTimerCallback), null, 30000, 0);
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("WaitingForConnection"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, null, null));
			this.AreInstructionsVisible = false;
			base.Commands.Run((LumiaController c) => c.StartLumiaDetection(DetectionType.RecoveryModeAfterEmergencyFlashing, CancellationToken.None));
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000142E9 File Offset: 0x000124E9
		private void OnTimerCallback(object state)
		{
			this.timer.Dispose();
			this.AreInstructionsVisible = true;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00014300 File Offset: 0x00012500
		public override void OnStopped()
		{
			this.timer.Dispose();
			base.Commands.Run((LumiaController c) => c.StopLumiaDetection());
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00014368 File Offset: 0x00012568
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted && message.Phone.Type == PhoneTypes.Lumia && this.appContext.CurrentPhone != null)
			{
				base.Commands.Run((FlowController a) => a.FinishAwaitRecoveryAfterEmergency(false, CancellationToken.None));
			}
		}

		// Token: 0x040001D9 RID: 473
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040001DA RID: 474
		private Timer timer;

		// Token: 0x040001DB RID: 475
		private bool areInstructionsVisible;
	}
}
