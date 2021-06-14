using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using ComponentAce.Compression.Archiver;
using ComponentAce.Compression.ZipForge;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.Controllers
{
	// Token: 0x02000027 RID: 39
	[Export("Microsoft.WindowsDeviceRecoveryTool.Controllers.SettingsController", typeof(IController))]
	public class SettingsController : BaseController
	{
		// Token: 0x0600011B RID: 283 RVA: 0x00008C0A File Offset: 0x00006E0A
		[ImportingConstructor]
		public SettingsController(ICommandRepository commandRepository, LogicContext logics, EventAggregator eventAggregator) : base(commandRepository, eventAggregator)
		{
			this.logics = logics;
			this.SetProxy(null);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008C28 File Offset: 0x00006E28
		[CustomCommand(IsAsynchronous = true)]
		public void ChangePackagesPathDirectory(object parameter, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("ChangePackagesPathDirectory");
			base.EventAggregator.Publish<SettingsPreviousStateMessage>(new SettingsPreviousStateMessage("PackagesState"));
			base.EventAggregator.Publish<SelectedPathMessage>(new SelectedPathMessage(Settings.Default.PackagesPath));
			base.Commands.Run((AppController c) => c.SwitchSettingsState("FolderBrowseAreaState"));
			Tracer<SettingsController>.LogExit("ChangePackagesPathDirectory");
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008CE4 File Offset: 0x00006EE4
		[CustomCommand(IsAsynchronous = true)]
		public void SetPackagesPathDirectory(string packagesPath, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("SetPackagesPathDirectory");
			if (!string.IsNullOrWhiteSpace(packagesPath))
			{
				base.EventAggregator.Publish<PackageDirectoryMessage>(new PackageDirectoryMessage(packagesPath));
				Tracer<SettingsController>.LogExit("SetPackagesPathDirectory");
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008D2C File Offset: 0x00006F2C
		[CustomCommand(IsAsynchronous = true)]
		public void CollectLogs(string zipLogFilePath, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("CollectLogs");
			if (!string.IsNullOrWhiteSpace(zipLogFilePath))
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("CollectingLogFilesInfo")));
				try
				{
					string appNamePrefix = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppNamePrefix;
					string text = Path.Combine(zipLogFilePath, string.Format("{0}_{1}.zip", appNamePrefix, DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)));
					this.CreateLogZipFile(text, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Traces), appNamePrefix);
					Process.Start("explorer.exe", string.Format("/select, {0}", text));
				}
				catch (ArchiverException ex)
				{
					if (ex.ErrorCode == ErrorCode.DiskIsFull)
					{
						throw new NotEnoughSpaceException(ex.Message, ex);
					}
					throw;
				}
				finally
				{
					base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
					base.EventAggregator.Publish<TraceParametersMessage>(new TraceParametersMessage(null, true));
				}
				Tracer<SettingsController>.LogExit("CollectLogs");
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008E48 File Offset: 0x00007048
		[CustomCommand(IsAsynchronous = true)]
		public void DeleteLogs(bool skipDialogQuestion, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("DeleteLogs");
			DialogMessageManager dialogMessageManager = new DialogMessageManager();
			if (skipDialogQuestion || dialogMessageManager.ShowQuestionDialog(LocalizationManager.GetTranslation("DeleteLogsQuestion"), null, true) == true)
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("DeletingLogFilesInfo")));
				try
				{
					TraceManager.Instance.RemoveDiagnosticLogs(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Traces), Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppNamePrefix, Settings.Default.TraceEnabled);
				}
				finally
				{
					base.EventAggregator.Publish<ApplicationInvalidateSizeMessage>(new ApplicationInvalidateSizeMessage(ApplicationInvalidateSizeMessage.DataType.Logs));
					if (!skipDialogQuestion)
					{
						base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
					}
				}
			}
			Tracer<SettingsController>.LogExit("DeleteLogs");
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008F30 File Offset: 0x00007130
		[CustomCommand(IsAsynchronous = true)]
		public void SetTraceEnabled(bool traceEnabled, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("SetTraceEnabled");
			if (traceEnabled)
			{
				TraceManager.Instance.EnableDiagnosticLogs(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Traces), Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppNamePrefix);
				Tracer<SettingsController>.WriteInformation("App version: {0} (running on: {1})", new object[]
				{
					AppInfo.Version,
					Environment.OSVersion
				});
			}
			else
			{
				TraceManager.Instance.DisableDiagnosticLogs(false);
			}
			Tracer<SettingsController>.LogExit("SetTraceEnabled");
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008FA7 File Offset: 0x000071A7
		[CustomCommand(IsAsynchronous = true)]
		public void CalculateLogsSize(object parameter, CancellationToken token)
		{
			base.EventAggregator.Publish<ApplicationDataSizeMessage>(new ApplicationDataSizeMessage(ApplicationDataSizeMessage.DataType.Logs, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Traces))));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008FC7 File Offset: 0x000071C7
		[CustomCommand(IsAsynchronous = true)]
		public void CalculateReportsSize(object parameter, CancellationToken token)
		{
			base.EventAggregator.Publish<ApplicationDataSizeMessage>(new ApplicationDataSizeMessage(ApplicationDataSizeMessage.DataType.Reports, Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Reports))));
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008FE8 File Offset: 0x000071E8
		[CustomCommand(IsAsynchronous = true)]
		public void CalculatePackagesSize(object parameter, CancellationToken token)
		{
			token.ThrowIfCancellationRequested();
			long num;
			if (string.IsNullOrEmpty(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetCustomProductsPath()))
			{
				num = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.NokiaPackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.HtcPackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.LgePackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.McjPackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.BluPackagesPath);
				token.ThrowIfCancellationRequested();
				num += Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AlcatelPackagesPath);
			}
			else
			{
				num = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetDirectorySize(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetCustomProductsPath());
			}
			token.ThrowIfCancellationRequested();
			base.EventAggregator.Publish<ApplicationDataSizeMessage>(new ApplicationDataSizeMessage(ApplicationDataSizeMessage.DataType.Packages, num));
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000090C8 File Offset: 0x000072C8
		[CustomCommand(IsAsynchronous = true)]
		public void ResetSettings(object parameter, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("ResetSettings");
			string translation = LocalizationManager.GetTranslation("ResetSettingsQuestion");
			if (!(new DialogMessageManager().ShowQuestionDialog(translation, null, true) != true))
			{
				ApplicationInfo.CurrentLanguageInRegistry = ApplicationInfo.DefaultLanguageInRegistry;
				base.EventAggregator.Publish<LanguageChangedMessage>(new LanguageChangedMessage(ApplicationInfo.DefaultLanguageInRegistry));
				base.EventAggregator.Publish<ThemeColorChangedMessage>(new ThemeColorChangedMessage((string)Settings.Default.Properties["Theme"].DefaultValue, (string)Settings.Default.Properties["Style"].DefaultValue));
				Settings.Default.Reset();
				Settings.Default.CallUpgrade = false;
				this.SetTraceEnabled(Settings.Default.TraceEnabled, CancellationToken.None);
				Settings.Default.Save();
				Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CustomPackagesPath = string.Empty;
				Tracer<SettingsController>.LogExit("ResetSettings");
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000091DC File Offset: 0x000073DC
		[CustomCommand(IsAsynchronous = true)]
		public void DeleteReports(bool skipDialogQuestion, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("DeleteReports");
			DialogMessageManager dialogMessageManager = new DialogMessageManager();
			if (skipDialogQuestion || dialogMessageManager.ShowQuestionDialog(LocalizationManager.GetTranslation("DeleteReportsQuestion"), null, true) == true)
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("DeletingReportsInfo")));
				try
				{
					this.DeleteAllReports();
				}
				finally
				{
					base.EventAggregator.Publish<ApplicationInvalidateSizeMessage>(new ApplicationInvalidateSizeMessage(ApplicationInvalidateSizeMessage.DataType.Reports));
					if (!skipDialogQuestion)
					{
						base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
					}
				}
			}
			Tracer<SettingsController>.LogExit("DeleteReports");
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000092A8 File Offset: 0x000074A8
		[CustomCommand(IsAsynchronous = true)]
		public void DeletePackages(bool skipDialogQuestion, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("DeletePackages");
			DialogMessageManager dialogMessageManager = new DialogMessageManager();
			if (skipDialogQuestion || dialogMessageManager.ShowQuestionDialog(LocalizationManager.GetTranslation("DeletePackagesQuestion"), null, true) == true)
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("DeletingPackagesInfo")));
				try
				{
					this.DeleteAllPackages();
				}
				finally
				{
					base.EventAggregator.Publish<ApplicationInvalidateSizeMessage>(new ApplicationInvalidateSizeMessage(ApplicationInvalidateSizeMessage.DataType.Packages));
					if (!skipDialogQuestion)
					{
						base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
					}
				}
			}
			Tracer<SettingsController>.LogExit("DeletePackages");
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00009374 File Offset: 0x00007574
		[CustomCommand(IsAsynchronous = true)]
		public void CleanUserData(object parameter, CancellationToken token)
		{
			Tracer<SettingsController>.LogEntry("CleanUserData");
			if (new DialogMessageManager().ShowQuestionDialog(LocalizationManager.GetTranslation("CleanUserDataQuestion"), null, true) == true)
			{
				try
				{
					this.DeleteLogs(true, CancellationToken.None);
					this.DeleteReports(true, CancellationToken.None);
					this.DeletePackages(true, CancellationToken.None);
				}
				finally
				{
					base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
				}
			}
			Tracer<SettingsController>.LogExit("CleanUserData");
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00009424 File Offset: 0x00007624
		[CustomCommand]
		public void SetProxy(object parameter)
		{
			Tracer<SettingsController>.LogEntry("SetProxy");
			IWebProxy proxy = null;
			if (Settings.Default.UseManualProxy)
			{
				if (!string.IsNullOrEmpty(Settings.Default.ProxyAddress))
				{
					proxy = new WebProxy(Settings.Default.ProxyAddress, Settings.Default.ProxyPort)
					{
						Credentials = new NetworkCredential(Settings.Default.ProxyUsername, new Credentials().DecryptString(Settings.Default.ProxyPassword))
					};
				}
			}
			this.logics.SetProxy(proxy);
			Settings.Default.Save();
			Tracer<SettingsController>.LogExit("SetProxy");
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000094D0 File Offset: 0x000076D0
		[CustomCommand]
		public void SetApplicationSettings()
		{
			if (Settings.Default.CustomPackagesPathEnabled && !string.IsNullOrWhiteSpace(Settings.Default.PackagesPath))
			{
				Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CustomPackagesPath = Settings.Default.PackagesPath;
			}
			else
			{
				Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CustomPackagesPath = null;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009520 File Offset: 0x00007720
		[CustomCommand]
		public void ChangeZipLogPath()
		{
			Tracer<SettingsController>.LogEntry("ChangeZipLogPath");
			base.EventAggregator.Publish<SettingsPreviousStateMessage>(new SettingsPreviousStateMessage("TraceState"));
			base.EventAggregator.Publish<SelectedPathMessage>(new SelectedPathMessage(Environment.ExpandEnvironmentVariables(Settings.Default.ZipFilePath)));
			base.Commands.Run((AppController c) => c.SwitchSettingsState("FolderBrowseAreaState"));
			Tracer<SettingsController>.LogExit("ChangeZipLogPath");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000095E0 File Offset: 0x000077E0
		private void DeleteAllReports()
		{
			Tracer<SettingsController>.LogEntry("DeleteAllReports");
			base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("RemovingReports")));
			try
			{
				string path = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Reports);
				this.DeleteDirContent(path);
			}
			finally
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
			}
			Tracer<SettingsController>.LogExit("DeleteAllReports");
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009660 File Offset: 0x00007860
		private void DeleteAllPackages()
		{
			Tracer<SettingsController>.LogEntry("DeleteAllPackages");
			base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(true, LocalizationManager.GetTranslation("RemovingPackages")));
			try
			{
				if (string.IsNullOrEmpty(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetCustomProductsPath()))
				{
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.DefaultPackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.NokiaPackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.HtcPackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.LgePackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.McjPackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.BluPackagesPath);
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AlcatelPackagesPath);
				}
				else
				{
					this.DeleteDirContent(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.GetCustomProductsPath());
				}
			}
			finally
			{
				base.EventAggregator.Publish<IsBusyMessage>(new IsBusyMessage(false, ""));
			}
			Tracer<SettingsController>.LogExit("DeleteAllPackages");
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009748 File Offset: 0x00007948
		private void DeleteDirContent(string path)
		{
			Tracer<SettingsController>.LogEntry("DeleteDirContent");
			if (!Directory.Exists(path))
			{
				Tracer<SettingsController>.WriteWarning("Directory not found!", new object[0]);
				Tracer<SettingsController>.LogExit("DeleteDirContent");
			}
			else
			{
				foreach (DirectoryInfo directoryInfo in new DirectoryInfo(path).GetDirectories())
				{
					try
					{
						directoryInfo.Delete(true);
						Tracer<SettingsController>.WriteInformation("Successfully removed directory: {0}", new object[]
						{
							path + directoryInfo
						});
					}
					catch (Exception ex)
					{
						Tracer<SettingsController>.WriteInformation("Skipped {0} directory when cleaning up data - {1}", new object[]
						{
							directoryInfo,
							ex.Message
						});
					}
				}
				foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
				{
					try
					{
						fileInfo.Delete();
						Tracer<SettingsController>.WriteInformation("Successfully removed file {0}", new object[]
						{
							path + fileInfo
						});
					}
					catch (Exception)
					{
						Tracer<SettingsController>.WriteInformation("Skipped {0} file when cleaning up data", new object[]
						{
							fileInfo
						});
					}
				}
				Tracer<SettingsController>.LogExit("DeleteDirContent");
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009AA0 File Offset: 0x00007CA0
		private void CreateLogZipFile(string zipFilePath, string logPath, string appNamePrefix)
		{
			Tracer<SettingsController>.LogEntry("CreateLogZipFile");
			Tracer<SettingsController>.WriteInformation("Creating log .zip file: {0}", new object[]
			{
				zipFilePath
			});
			using (ZipForge zipForge = new ZipForge())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					zipForge.OpenArchive(memoryStream, true);
					zipForge.BaseDir = logPath;
					string[] files = Directory.GetFiles(logPath);
					foreach (string fileMask in from filePath in files
					let fileInfo = new FileInfo(filePath)
					where fileInfo.Name.StartsWith(appNamePrefix, true, CultureInfo.CurrentCulture)
					where (DateTime.Now - fileInfo.CreationTime).Days < 7
					select filePath)
					{
						zipForge.AddFiles(fileMask);
					}
					if (zipForge.Size > new DriveInfo(Path.GetPathRoot(zipFilePath)).AvailableFreeSpace)
					{
						throw new ArchiverException("The disk is full", ErrorCode.DiskIsFull, null, null);
					}
					zipForge.CloseArchive();
					using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create))
					{
						memoryStream.WriteTo(fileStream);
					}
				}
			}
			Tracer<SettingsController>.LogExit("CreateLogZipFile");
		}

		// Token: 0x0400008E RID: 142
		private readonly LogicContext logics;
	}
}
