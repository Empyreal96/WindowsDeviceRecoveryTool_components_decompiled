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
	// Token: 0x020000CE RID: 206
	[Export]
	public class FlashingViewModel : BaseViewModel, ICanHandle<ProgressMessage>, ICanHandle<DetectionTypeMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0002084F File Offset: 0x0001EA4F
		[ImportingConstructor]
		public FlashingViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000643 RID: 1603 RVA: 0x00020864 File Offset: 0x0001EA64
		// (remove) Token: 0x06000644 RID: 1604 RVA: 0x000208A0 File Offset: 0x0001EAA0
		public event EventHandler LiveRegionChanged;

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x000208DC File Offset: 0x0001EADC
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x000208F4 File Offset: 0x0001EAF4
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

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00020944 File Offset: 0x0001EB44
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0002095C File Offset: 0x0001EB5C
		public int Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				base.SetValue<int>(() => this.Progress, ref this.progress, value);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x000209AC File Offset: 0x0001EBAC
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x000209C4 File Offset: 0x0001EBC4
		public bool ProgressPercentageVisible
		{
			get
			{
				return this.progressPercentageVisible;
			}
			set
			{
				base.SetValue<bool>(() => this.ProgressPercentageVisible, ref this.progressPercentageVisible, value);
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00020A14 File Offset: 0x0001EC14
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00020A2C File Offset: 0x0001EC2C
		public bool IsProgressIndeterminate
		{
			get
			{
				return this.isProgressIndeterminate;
			}
			set
			{
				base.SetValue<bool>(() => this.IsProgressIndeterminate, ref this.isProgressIndeterminate, value);
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00020A7C File Offset: 0x0001EC7C
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x00020A94 File Offset: 0x0001EC94
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00020AFC File Offset: 0x0001ECFC
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00020B14 File Offset: 0x0001ED14
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

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00020B64 File Offset: 0x0001ED64
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00020B7C File Offset: 0x0001ED7C
		public DetectionType DetectionType
		{
			get
			{
				return this.detectionType;
			}
			set
			{
				base.SetValue<DetectionType>(() => this.DetectionType, ref this.detectionType, value);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00020BCC File Offset: 0x0001EDCC
		public override void OnStarted()
		{
			base.OnStarted();
			this.Progress = 0;
			this.Message = string.Empty;
			this.LiveText = string.Empty;
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DeviceFlashing"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, LocalizationManager.GetTranslation("FlashCancelMessage"), null));
			if (this.appContext.CurrentPhone.IsDeviceInEmergencyMode())
			{
				base.Commands.Run((FlowController c) => c.EmergencyFlashDevice(null, CancellationToken.None));
			}
			else
			{
				base.Commands.Run((FlowController c) => c.FlashDevice(this.DetectionType, CancellationToken.None));
			}
			this.LiveText = LocalizationManager.GetTranslation("InstallationStarted");
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00020D96 File Offset: 0x0001EF96
		public override void OnStopped()
		{
			base.OnStopped();
			this.LiveText = LocalizationManager.GetTranslation("InstallationCompleted");
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00020DB4 File Offset: 0x0001EFB4
		public void Handle(ProgressMessage progressMessage)
		{
			if (base.IsStarted)
			{
				this.Progress = progressMessage.Progress;
				this.Message = progressMessage.Message;
				if (!string.IsNullOrEmpty(progressMessage.Message))
				{
					string translation = LocalizationManager.GetTranslation(progressMessage.Message);
					if (!translation.Contains("NOT FOUND"))
					{
						this.Message = translation;
					}
				}
				this.ProgressPercentageVisible = (this.Progress >= 0 || !string.IsNullOrWhiteSpace(this.Message));
				this.IsProgressIndeterminate = (this.Progress == 0 || this.Progress > 100);
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00020E63 File Offset: 0x0001F063
		public void Handle(DetectionTypeMessage detectionMessage)
		{
			this.DetectionType = detectionMessage.DetectionType;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00020E74 File Offset: 0x0001F074
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x040002A4 RID: 676
		private int progress;

		// Token: 0x040002A5 RID: 677
		private string liveText;

		// Token: 0x040002A6 RID: 678
		private string message;

		// Token: 0x040002A7 RID: 679
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040002A8 RID: 680
		private bool progressPercentageVisible;

		// Token: 0x040002A9 RID: 681
		private DetectionType detectionType;

		// Token: 0x040002AA RID: 682
		private bool isProgressIndeterminate;
	}
}
