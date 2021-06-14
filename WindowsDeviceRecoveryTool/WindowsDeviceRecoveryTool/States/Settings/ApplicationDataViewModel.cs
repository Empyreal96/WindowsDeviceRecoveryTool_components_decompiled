using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000AC RID: 172
	[Export]
	public class ApplicationDataViewModel : BaseViewModel, ICanHandle<ApplicationDataSizeMessage>, ICanHandle<ApplicationInvalidateSizeMessage>, ICanHandle
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00018BD4 File Offset: 0x00016DD4
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00018BEC File Offset: 0x00016DEC
		public long LogFilesSize
		{
			get
			{
				return this.logFilesSize;
			}
			set
			{
				if (this.logFilesSize != value)
				{
					base.SetValue<long>(() => this.LogFilesSize, ref this.logFilesSize, value);
					base.RaisePropertyChanged<string>(() => this.LogFilesSizeString);
					base.RaisePropertyChanged<long>(() => this.AllFilesSize);
					base.RaisePropertyChanged<string>(() => this.AllFilesSizeString);
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00018D00 File Offset: 0x00016F00
		public string LogFilesSizeString
		{
			get
			{
				return ComputerUnitsConverter.SizeToString(this.LogFilesSize);
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00018D20 File Offset: 0x00016F20
		// (set) Token: 0x060004E5 RID: 1253 RVA: 0x00018D38 File Offset: 0x00016F38
		public long ReportsFilesSize
		{
			get
			{
				return this.reportsFilesSize;
			}
			set
			{
				if (this.reportsFilesSize != value)
				{
					base.SetValue<long>(() => this.ReportsFilesSize, ref this.reportsFilesSize, value);
					base.RaisePropertyChanged<string>(() => this.ReportsFilesSizeString);
					base.RaisePropertyChanged<long>(() => this.AllFilesSize);
					base.RaisePropertyChanged<string>(() => this.AllFilesSizeString);
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00018E4C File Offset: 0x0001704C
		public string ReportsFilesSizeString
		{
			get
			{
				return ComputerUnitsConverter.SizeToString(this.ReportsFilesSize);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00018E6C File Offset: 0x0001706C
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x00018E84 File Offset: 0x00017084
		public long PackagesFilesSize
		{
			get
			{
				return this.packagesFilesSize;
			}
			set
			{
				if (this.packagesFilesSize != value)
				{
					base.SetValue<long>(() => this.PackagesFilesSize, ref this.packagesFilesSize, value);
					base.RaisePropertyChanged<string>(() => this.PackagesFilesSizeString);
					base.RaisePropertyChanged<long>(() => this.AllFilesSize);
					base.RaisePropertyChanged<string>(() => this.AllFilesSizeString);
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00018F98 File Offset: 0x00017198
		public string PackagesFilesSizeString
		{
			get
			{
				return ComputerUnitsConverter.SizeToString(this.PackagesFilesSize);
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00018FB8 File Offset: 0x000171B8
		public long AllFilesSize
		{
			get
			{
				return this.LogFilesSize + this.ReportsFilesSize + this.PackagesFilesSize;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00018FE0 File Offset: 0x000171E0
		public bool AllCalculationCompleted
		{
			get
			{
				return !this.LogsCalculationInProgress && !this.ReportsCalculationInProgress && !this.PackagesCalculationInProgress;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x00019010 File Offset: 0x00017210
		public string AllFilesSizeString
		{
			get
			{
				return ComputerUnitsConverter.SizeToString(this.AllFilesSize);
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x00019030 File Offset: 0x00017230
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x00019048 File Offset: 0x00017248
		public bool LogsCalculationInProgress
		{
			get
			{
				return this.logsCalculationInProgress;
			}
			set
			{
				base.SetValue<bool>(() => this.LogsCalculationInProgress, ref this.logsCalculationInProgress, value);
				base.RaisePropertyChanged<bool>(() => this.AllCalculationCompleted);
				base.RaisePropertyChanged<bool>(() => this.CleanAllAppDataBtnEnabled);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00019110 File Offset: 0x00017310
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x00019128 File Offset: 0x00017328
		public bool ReportsCalculationInProgress
		{
			get
			{
				return this.reportsCalculationInProgress;
			}
			set
			{
				base.SetValue<bool>(() => this.ReportsCalculationInProgress, ref this.reportsCalculationInProgress, value);
				base.RaisePropertyChanged<bool>(() => this.AllCalculationCompleted);
				base.RaisePropertyChanged<bool>(() => this.CleanAllAppDataBtnEnabled);
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x000191F0 File Offset: 0x000173F0
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x00019208 File Offset: 0x00017408
		public bool PackagesCalculationInProgress
		{
			get
			{
				return this.packagesCalculationInProgress;
			}
			set
			{
				base.SetValue<bool>(() => this.PackagesCalculationInProgress, ref this.packagesCalculationInProgress, value);
				base.RaisePropertyChanged<bool>(() => this.AllCalculationCompleted);
				base.RaisePropertyChanged<bool>(() => this.CleanAllAppDataBtnEnabled);
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x000192D0 File Offset: 0x000174D0
		public bool CleanAllAppDataBtnEnabled
		{
			get
			{
				return this.AllCalculationCompleted && this.AllFilesSize > 0L;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x000192F8 File Offset: 0x000174F8
		public override void OnStopped()
		{
			this.cts.Cancel();
			Settings.Default.Save();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00019314 File Offset: 0x00017514
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Settings"), LocalizationManager.GetTranslation("ApplicationData")));
			this.cts = new CancellationTokenSource();
			this.LogFilesSize = 0L;
			this.ReportsFilesSize = 0L;
			this.PackagesFilesSize = 0L;
			this.LogsCalculationInProgress = true;
			this.ReportsCalculationInProgress = true;
			this.PackagesCalculationInProgress = true;
			base.Commands.Run((SettingsController controller) => controller.CalculateLogsSize(null, this.cts.Token));
			base.Commands.Run((SettingsController controller) => controller.CalculateReportsSize(null, this.cts.Token));
			base.Commands.Run((SettingsController controller) => controller.CalculatePackagesSize(null, this.cts.Token));
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001956C File Offset: 0x0001776C
		public void Handle(ApplicationDataSizeMessage message)
		{
			if (base.IsStarted)
			{
				switch (message.Type)
				{
				case ApplicationDataSizeMessage.DataType.Logs:
					this.LogFilesSize = message.FilesSize;
					this.LogsCalculationInProgress = false;
					break;
				case ApplicationDataSizeMessage.DataType.Reports:
					this.ReportsFilesSize = message.FilesSize;
					this.ReportsCalculationInProgress = false;
					break;
				case ApplicationDataSizeMessage.DataType.Packages:
					this.PackagesFilesSize = message.FilesSize;
					this.PackagesCalculationInProgress = false;
					break;
				}
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000195EC File Offset: 0x000177EC
		public void Handle(ApplicationInvalidateSizeMessage message)
		{
			if (base.IsStarted)
			{
				switch (message.Type)
				{
				case ApplicationInvalidateSizeMessage.DataType.Logs:
					this.LogFilesSize = 0L;
					this.LogsCalculationInProgress = true;
					base.Commands.Run((SettingsController controller) => controller.CalculateLogsSize(null, this.cts.Token));
					break;
				case ApplicationInvalidateSizeMessage.DataType.Reports:
					this.ReportsFilesSize = 0L;
					this.ReportsCalculationInProgress = true;
					base.Commands.Run((SettingsController controller) => controller.CalculateReportsSize(null, this.cts.Token));
					break;
				case ApplicationInvalidateSizeMessage.DataType.Packages:
					this.PackagesFilesSize = 0L;
					this.PackagesCalculationInProgress = true;
					base.Commands.Run((SettingsController controller) => controller.CalculatePackagesSize(null, this.cts.Token));
					break;
				}
			}
		}

		// Token: 0x0400021D RID: 541
		private long logFilesSize;

		// Token: 0x0400021E RID: 542
		private long reportsFilesSize;

		// Token: 0x0400021F RID: 543
		private long packagesFilesSize;

		// Token: 0x04000220 RID: 544
		private bool logsCalculationInProgress;

		// Token: 0x04000221 RID: 545
		private bool reportsCalculationInProgress;

		// Token: 0x04000222 RID: 546
		private bool packagesCalculationInProgress;

		// Token: 0x04000223 RID: 547
		private CancellationTokenSource cts;
	}
}
