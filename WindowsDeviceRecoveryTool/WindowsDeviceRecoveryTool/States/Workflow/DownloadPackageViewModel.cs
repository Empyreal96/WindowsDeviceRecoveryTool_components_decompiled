using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Helpers;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Msr;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000C6 RID: 198
	[Export]
	public sealed class DownloadPackageViewModel : BaseViewModel, ICanHandle<ProgressMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x0001EF8D File Offset: 0x0001D18D
		[ImportingConstructor]
		public DownloadPackageViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
			this.uiUpdateAccessTimer = new IntervalResetAccessTimer(MsrDownloadConfig.Instance.DownloadProgressUpdateIntervalMillis, true);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060005F8 RID: 1528 RVA: 0x0001EFB8 File Offset: 0x0001D1B8
		// (remove) Token: 0x060005F9 RID: 1529 RVA: 0x0001EFF4 File Offset: 0x0001D1F4
		public event EventHandler LiveRegionChanged;

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0001F030 File Offset: 0x0001D230
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0001F048 File Offset: 0x0001D248
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0001F098 File Offset: 0x0001D298
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x0001F0B0 File Offset: 0x0001D2B0
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001F100 File Offset: 0x0001D300
		// (set) Token: 0x060005FF RID: 1535 RVA: 0x0001F118 File Offset: 0x0001D318
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0001F168 File Offset: 0x0001D368
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x0001F180 File Offset: 0x0001D380
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

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x0001F1E8 File Offset: 0x0001D3E8
		// (set) Token: 0x06000603 RID: 1539 RVA: 0x0001F200 File Offset: 0x0001D400
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

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x0001F250 File Offset: 0x0001D450
		// (set) Token: 0x06000605 RID: 1541 RVA: 0x0001F268 File Offset: 0x0001D468
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

		// Token: 0x06000606 RID: 1542 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DownloadingPackage"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, LocalizationManager.GetTranslation("DownloadingCancelMessage"), null));
			this.ProgressUpdated = false;
			this.Message = string.Empty;
			this.TimeLeftMessage = string.Empty;
			this.LiveText = string.Empty;
			this.progressCount = 0L;
			this.uiUpdateAccessTimer.StartTimer();
			base.Commands.Run((FlowController c) => c.DownloadPackage(null, CancellationToken.None));
			this.LiveText = LocalizationManager.GetTranslation("DownloadStarted");
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001F3EC File Offset: 0x0001D5EC
		public override void OnStopped()
		{
			base.OnStopped();
			this.uiUpdateAccessTimer.StopTimer();
			this.LiveText = LocalizationManager.GetTranslation("DownloadCompleted");
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001F600 File Offset: 0x0001D800
		public void Handle(ProgressMessage progressMessage)
		{
			if ((this.progressCount += 1L) % (long)(MsrDownloadConfig.Instance.NumberOfProgressEventsToSkipInUI + 1) == 0L)
			{
				if (base.IsStarted)
				{
					this.uiUpdateAccessTimer.RunIfAccessAvailable(delegate
					{
						this.Progress = progressMessage.Progress;
						Tracer<DownloadPackageViewModel>.WriteVerbose("Download progress: {0}", new object[]
						{
							this.progressCount
						});
						if (!string.IsNullOrEmpty(progressMessage.Message))
						{
							if (progressMessage.Message == "DownloadingFiles")
							{
								this.ProgressUpdated = true;
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
					});
				}
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001F680 File Offset: 0x0001D880
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x04000285 RID: 645
		private long progressCount;

		// Token: 0x04000286 RID: 646
		private bool progressUpdated;

		// Token: 0x04000287 RID: 647
		private int progress;

		// Token: 0x04000288 RID: 648
		private string liveText;

		// Token: 0x04000289 RID: 649
		private string message;

		// Token: 0x0400028A RID: 650
		private string timeLeftMessage;

		// Token: 0x0400028B RID: 651
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400028C RID: 652
		private readonly IntervalResetAccessTimer uiUpdateAccessTimer;
	}
}
