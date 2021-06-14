using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000DC RID: 220
	[Export]
	public class TraceViewModel : BaseViewModel, ICanHandle<TraceParametersMessage>, ICanHandle<ApplicationDataSizeMessage>, ICanHandle
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000239D8 File Offset: 0x00021BD8
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000239FC File Offset: 0x00021BFC
		public string ZipFilePath
		{
			get
			{
				return Environment.ExpandEnvironmentVariables(Settings.Default.ZipFilePath);
			}
			set
			{
				Settings.Default.ZipFilePath = value;
				base.RaisePropertyChanged<string>(() => this.ZipFilePath);
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00023A54 File Offset: 0x00021C54
		public bool ExportEnable
		{
			get
			{
				return this.TraceEnabled || this.logsSize > 0L;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00023A7C File Offset: 0x00021C7C
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00023AA0 File Offset: 0x00021CA0
		public bool TraceEnabled
		{
			get
			{
				return Settings.Default.TraceEnabled;
			}
			set
			{
				Settings.Default.TraceEnabled = value;
				base.Commands.Run((SettingsController c) => c.SetTraceEnabled(value, CancellationToken.None));
				if (!value)
				{
					base.Commands.Run((SettingsController controller) => controller.CalculateLogsSize(null, CancellationToken.None));
				}
				base.RaisePropertyChanged<bool>(() => this.ExportEnable);
				base.RaisePropertyChanged<bool>(() => this.TraceEnabled);
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00023C5C File Offset: 0x00021E5C
		public void Handle(TraceParametersMessage message)
		{
			if (!string.IsNullOrWhiteSpace(message.LogZipFilePath))
			{
				this.ZipFilePath = message.LogZipFilePath;
			}
			base.RaisePropertyChanged<string>(() => this.ZipFilePath);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00023CC3 File Offset: 0x00021EC3
		public override void OnStopped()
		{
			Settings.Default.Save();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00023CD4 File Offset: 0x00021ED4
		public override void OnStarted()
		{
			base.RaisePropertyChanged<string>(() => this.ZipFilePath);
			if (!this.TraceEnabled)
			{
				base.Commands.Run((SettingsController controller) => controller.CalculateLogsSize(null, CancellationToken.None));
			}
			base.RaisePropertyChanged<bool>(() => this.TraceEnabled);
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Settings"), LocalizationManager.GetTranslation("Troubleshooting")));
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00023E0C File Offset: 0x0002200C
		public void Handle(ApplicationDataSizeMessage message)
		{
			if (base.IsStarted && message.Type == ApplicationDataSizeMessage.DataType.Logs)
			{
				this.logsSize = message.FilesSize;
				base.RaisePropertyChanged<bool>(() => this.ExportEnable);
			}
		}

		// Token: 0x040002E1 RID: 737
		private long logsSize = 0L;
	}
}
