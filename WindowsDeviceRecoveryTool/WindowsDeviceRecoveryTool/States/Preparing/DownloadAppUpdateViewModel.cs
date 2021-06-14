using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x0200009E RID: 158
	[Export]
	public class DownloadAppUpdateViewModel : BaseViewModel, ICanHandle<ProgressMessage>, ICanHandle, INotifyLiveRegionChanged
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x00014BDF File Offset: 0x00012DDF
		[ImportingConstructor]
		public DownloadAppUpdateViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000453 RID: 1107 RVA: 0x00014BF4 File Offset: 0x00012DF4
		// (remove) Token: 0x06000454 RID: 1108 RVA: 0x00014C30 File Offset: 0x00012E30
		public event EventHandler LiveRegionChanged;

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00014C6C File Offset: 0x00012E6C
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00014C84 File Offset: 0x00012E84
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

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00014CD4 File Offset: 0x00012ED4
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x00014CEC File Offset: 0x00012EEC
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00014D3C File Offset: 0x00012F3C
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x00014D54 File Offset: 0x00012F54
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

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00014DA4 File Offset: 0x00012FA4
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x00014DBC File Offset: 0x00012FBC
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

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x00014E24 File Offset: 0x00013024
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x00014E3C File Offset: 0x0001303C
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

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00014E8C File Offset: 0x0001308C
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x00014EA4 File Offset: 0x000130A4
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

		// Token: 0x06000461 RID: 1121 RVA: 0x00014EF4 File Offset: 0x000130F4
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("DownloadingInstallPacket"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			this.ProgressUpdated = false;
			this.Message = string.Empty;
			this.TimeLeftMessage = string.Empty;
			this.LiveText = string.Empty;
			base.Commands.Run((AppController c) => c.UpdateApplication(null, CancellationToken.None));
			this.LiveText = LocalizationManager.GetTranslation("DownloadStarted");
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00014FF7 File Offset: 0x000131F7
		public override void OnStopped()
		{
			base.OnStopped();
			this.LiveText = LocalizationManager.GetTranslation("DownloadCompleted");
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00015014 File Offset: 0x00013214
		public void Handle(ProgressMessage progressMessage)
		{
			if (base.IsStarted)
			{
				this.Progress = progressMessage.Progress;
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
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00015184 File Offset: 0x00013384
		private void OnLiveRegionChanged()
		{
			EventHandler liveRegionChanged = this.LiveRegionChanged;
			if (liveRegionChanged != null)
			{
				liveRegionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x040001E5 RID: 485
		private bool progressUpdated;

		// Token: 0x040001E6 RID: 486
		private int progress;

		// Token: 0x040001E7 RID: 487
		private string liveText;

		// Token: 0x040001E8 RID: 488
		private string message;

		// Token: 0x040001E9 RID: 489
		private string timeLeftMessage;

		// Token: 0x040001EA RID: 490
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
