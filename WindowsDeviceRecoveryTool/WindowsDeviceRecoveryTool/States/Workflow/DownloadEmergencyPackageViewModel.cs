using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000B3 RID: 179
	[Export]
	public sealed class DownloadEmergencyPackageViewModel : BaseViewModel, ICanHandle<ProgressMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x06000539 RID: 1337 RVA: 0x0001ADEB File Offset: 0x00018FEB
		[ImportingConstructor]
		public DownloadEmergencyPackageViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600053A RID: 1338 RVA: 0x0001AE00 File Offset: 0x00019000
		// (remove) Token: 0x0600053B RID: 1339 RVA: 0x0001AE3C File Offset: 0x0001903C
		public event EventHandler LiveRegionChanged;

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001AE78 File Offset: 0x00019078
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0001AE90 File Offset: 0x00019090
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001AEE0 File Offset: 0x000190E0
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0001AEF8 File Offset: 0x000190F8
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

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001AF48 File Offset: 0x00019148
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001AF60 File Offset: 0x00019160
		public bool ProgressUpdated
		{
			get
			{
				return this.progressUpdated;
			}
			set
			{
				base.SetValue<bool>(() => this.ProgressUpdated, ref this.progressUpdated, value);
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001AFB0 File Offset: 0x000191B0
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001AFC8 File Offset: 0x000191C8
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

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001B030 File Offset: 0x00019230
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0001B048 File Offset: 0x00019248
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

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001B098 File Offset: 0x00019298
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001B0B0 File Offset: 0x000192B0
		public string TimeLeftMessage
		{
			get
			{
				return this.timeLeftMessage;
			}
			set
			{
				base.SetValue<string>(() => this.TimeLeftMessage, ref this.timeLeftMessage, value);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001B100 File Offset: 0x00019300
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DownloadingEmergencyPackage"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, LocalizationManager.GetTranslation("DownloadingCancelMessage"), null));
			this.ProgressUpdated = false;
			this.Message = string.Empty;
			this.TimeLeftMessage = string.Empty;
			this.LiveText = string.Empty;
			if (string.IsNullOrWhiteSpace(this.AppContext.CurrentPhone.HardwareModel))
			{
				VariantInfo variantInfo = VariantInfo.GetVariantInfo(this.AppContext.CurrentPhone.PackageFilePath);
				this.AppContext.CurrentPhone.HardwareModel = variantInfo.ProductType;
			}
			Tracer<DownloadEmergencyPackageViewModel>.WriteInformation("Selected device type: {0}", new object[]
			{
				this.AppContext.CurrentPhone.HardwareModel
			});
			base.Commands.Run((FlowController c) => c.DownloadEmergencyPackage(null, CancellationToken.None));
			this.LiveText = LocalizationManager.GetTranslation("DownloadStarted");
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001B298 File Offset: 0x00019498
		public override void OnStopped()
		{
			base.OnStopped();
			this.LiveText = LocalizationManager.GetTranslation("DownloadCompleted");
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001B2B4 File Offset: 0x000194B4
		public void Handle(ProgressMessage progressMessage)
		{
			if (base.IsStarted)
			{
				this.Progress = progressMessage.Progress;
				if (!string.IsNullOrEmpty(progressMessage.Message))
				{
					if (progressMessage.Message == "DownloadingFiles")
					{
						if (progressMessage.TotalSize > 0L)
						{
							this.ProgressUpdated = true;
						}
						string arg = "...";
						if (progressMessage.BytesPerSecond > 0.0)
						{
							arg = ComputerUnitsConverter.SpeedToString(progressMessage.BytesPerSecond);
							if (progressMessage.SecondsLeft < 60L)
							{
								this.TimeLeftMessage = LocalizationManager.GetTranslation("DownloadProgressMinuteLess");
							}
							else if (progressMessage.SecondsLeft > 60L && progressMessage.SecondsLeft < 120L)
							{
								this.TimeLeftMessage = LocalizationManager.GetTranslation("DownloadProgressMinute");
							}
							else
							{
								this.TimeLeftMessage = string.Format(LocalizationManager.GetTranslation("DownloadProgressExactMinute"), TimeSpan.FromSeconds((double)progressMessage.SecondsLeft).TotalMinutes.ToString("F0"));
							}
						}
						this.Message = string.Format(LocalizationManager.GetTranslation("DownloadingFiles"), ComputerUnitsConverter.SizeToString(progressMessage.DownloadedSize), ComputerUnitsConverter.SizeToString(progressMessage.TotalSize), arg);
					}
					else
					{
						this.Message = LocalizationManager.GetTranslation(progressMessage.Message);
					}
				}
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001B434 File Offset: 0x00019634
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x04000241 RID: 577
		private bool progressUpdated;

		// Token: 0x04000242 RID: 578
		private int progress;

		// Token: 0x04000243 RID: 579
		private string liveText;

		// Token: 0x04000244 RID: 580
		private string message;

		// Token: 0x04000245 RID: 581
		private string timeLeftMessage;

		// Token: 0x04000246 RID: 582
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
