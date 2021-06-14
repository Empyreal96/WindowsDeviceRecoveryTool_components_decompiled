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
	// Token: 0x020000C5 RID: 197
	[Export]
	public class AwaitRecoveryDeviceViewModel : BaseViewModel, ICanHandle<DeviceConnectedMessage>, ICanHandle
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x0001EC2B File Offset: 0x0001CE2B
		[ImportingConstructor]
		public AwaitRecoveryDeviceViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0001EC40 File Offset: 0x0001CE40
		public string RebootPhoneInstructions
		{
			get
			{
				return LocalizationManager.GetTranslation("RebootPhoneInstructions");
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x0001EC5C File Offset: 0x0001CE5C
		// (set) Token: 0x060005EE RID: 1518 RVA: 0x0001EC74 File Offset: 0x0001CE74
		public bool AreInstructionsVisible
		{
			get
			{
				return this.areInstructionsVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.AreInstructionsVisible, ref this.areInstructionsVisible, value);
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001ECC4 File Offset: 0x0001CEC4
		public bool IsCancelVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001ECD8 File Offset: 0x0001CED8
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0001ED40 File Offset: 0x0001CF40
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001ED58 File Offset: 0x0001CF58
		public override void OnStarted()
		{
			this.timer = new Timer(new TimerCallback(this.OnTimerCallback), null, 30000, 0);
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ConnectPhone"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.AreInstructionsVisible = false;
			base.Commands.Run((LumiaController c) => c.StartLumiaDetection(DetectionType.RecoveryMode, CancellationToken.None));
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001EE51 File Offset: 0x0001D051
		private void OnTimerCallback(object state)
		{
			this.timer.Dispose();
			this.AreInstructionsVisible = true;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001EE68 File Offset: 0x0001D068
		public override void OnStopped()
		{
			this.timer.Dispose();
			base.Commands.Run((LumiaController c) => c.StopLumiaDetection());
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001EED0 File Offset: 0x0001D0D0
		public void Handle(DeviceConnectedMessage message)
		{
			if (base.IsStarted)
			{
				if (message.Phone.Type == PhoneTypes.Lumia && this.appContext.CurrentPhone != null && this.appContext.CurrentPhone.IsDeviceInEmergencyMode())
				{
					base.Commands.Run((AppController a) => a.SwitchToState("ManualDeviceTypeSelectionState"));
				}
			}
		}

		// Token: 0x04000282 RID: 642
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x04000283 RID: 643
		private Timer timer;

		// Token: 0x04000284 RID: 644
		private bool areInstructionsVisible;
	}
}
