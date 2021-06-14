using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000C8 RID: 200
	[Export]
	public class PackageIntegrityCheckViewModel : BaseViewModel, ICanHandle<FfuIntegrityCheckMessage>, ICanHandle<ProgressMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x0600060D RID: 1549 RVA: 0x0001F727 File Offset: 0x0001D927
		[ImportingConstructor]
		public PackageIntegrityCheckViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600060E RID: 1550 RVA: 0x0001F73C File Offset: 0x0001D93C
		// (remove) Token: 0x0600060F RID: 1551 RVA: 0x0001F778 File Offset: 0x0001D978
		public event EventHandler LiveRegionChanged;

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001F7B4 File Offset: 0x0001D9B4
		// (set) Token: 0x06000611 RID: 1553 RVA: 0x0001F7CC File Offset: 0x0001D9CC
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001F81C File Offset: 0x0001DA1C
		// (set) Token: 0x06000613 RID: 1555 RVA: 0x0001F834 File Offset: 0x0001DA34
		public bool CheckInProgress
		{
			get
			{
				return this.checkInProgress;
			}
			set
			{
				base.SetValue<bool>(() => this.CheckInProgress, ref this.checkInProgress, value);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001F884 File Offset: 0x0001DA84
		public bool ProgressBarVisible
		{
			get
			{
				return this.progress != -1;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001F8A4 File Offset: 0x0001DAA4
		public bool ProgressRingVisible
		{
			get
			{
				return this.progress == -1;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001F8C0 File Offset: 0x0001DAC0
		// (set) Token: 0x06000617 RID: 1559 RVA: 0x0001F8D8 File Offset: 0x0001DAD8
		public string LiveText
		{
			get
			{
				return this.liveText;
			}
			set
			{
				base.SetValue<string>(() => this.LiveText, ref this.liveText, value);
				if (!string.IsNullOrWhiteSpace(this.liveText))
				{
					this.OnLiveRegionChanged();
				}
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001F940 File Offset: 0x0001DB40
		// (set) Token: 0x06000619 RID: 1561 RVA: 0x0001F958 File Offset: 0x0001DB58
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				base.SetValue<string>(() => this.Message, ref this.message, value);
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x0001F9C0 File Offset: 0x0001DBC0
		public int Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				base.SetValue<int>(() => this.Progress, ref this.progress, value);
				base.RaisePropertyChanged<bool>(() => this.ProgressBarVisible);
				base.RaisePropertyChanged<bool>(() => this.ProgressRingVisible);
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001FA88 File Offset: 0x0001DC88
		public override void OnStarted()
		{
			base.OnStarted();
			this.CheckInProgress = true;
			this.Message = string.Empty;
			this.LiveText = string.Empty;
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("PackageIntegrityCheck"), ""));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, LocalizationManager.GetTranslation("FlashCancelMessage"), null));
			base.Commands.Run((FlowController c) => c.CheckPackageIntegrity(null, CancellationToken.None));
			this.Progress = -1;
			this.LiveText = LocalizationManager.GetTranslation("VerificationStarted");
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001FB92 File Offset: 0x0001DD92
		public override void OnStopped()
		{
			((IAsyncDelegateCommand)base.Commands["CheckPackageIntegrity"]).Cancel();
			base.OnStopped();
			this.LiveText = LocalizationManager.GetTranslation("VerificationCompleted");
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001FBC8 File Offset: 0x0001DDC8
		public void Handle(ProgressMessage progressMessage)
		{
			if (base.IsStarted)
			{
				this.Progress = progressMessage.Progress;
				this.Message = progressMessage.Message;
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001FC08 File Offset: 0x0001DE08
		public void Handle(FfuIntegrityCheckMessage integrityCheckMessage)
		{
			this.CheckInProgress = false;
			if (integrityCheckMessage.Result)
			{
				string nextState = (this.AppContext.CurrentPhone.Type == PhoneTypes.HoloLensAccessory) ? "FlashingState" : "BatteryCheckingState";
				base.Commands.Run((AppController c) => c.SwitchToState(nextState));
			}
			else
			{
				this.Message = string.Format(LocalizationManager.GetTranslation("FirmwareIntegrityError"), this.AppContext.CurrentPhone.PackageFilePath);
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x04000290 RID: 656
		private string liveText;

		// Token: 0x04000291 RID: 657
		private string message;

		// Token: 0x04000292 RID: 658
		private bool checkInProgress;

		// Token: 0x04000293 RID: 659
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x04000294 RID: 660
		private int progress;
	}
}
