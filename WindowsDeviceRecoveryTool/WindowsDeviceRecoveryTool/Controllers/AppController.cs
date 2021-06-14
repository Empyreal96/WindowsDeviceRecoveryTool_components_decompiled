using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;
using Microsoft.WindowsDeviceRecoveryTool.Properties;
using Microsoft.WindowsDeviceRecoveryTool.States.Help;
using Microsoft.WindowsDeviceRecoveryTool.States.Settings;
using Microsoft.WindowsDeviceRecoveryTool.States.Shell;

namespace Microsoft.WindowsDeviceRecoveryTool.Controllers
{
	// Token: 0x020000E4 RID: 228
	[Export("Microsoft.WindowsDeviceRecoveryTool.Controllers.AppController", typeof(IController))]
	public class AppController : BaseController, ICanHandle<BlockWindowMessage>, ICanHandle
	{
		// Token: 0x0600074F RID: 1871 RVA: 0x00026B9C File Offset: 0x00024D9C
		[ImportingConstructor]
		public AppController(ICommandRepository commandRepository, LogicContext logics, EventAggregator eventAggregator) : base(commandRepository, eventAggregator)
		{
			this.logics = logics;
			if (base.EventAggregator != null)
			{
				base.EventAggregator.Subscribe(this);
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x00026BF4 File Offset: 0x00024DF4
		[CustomCommand]
		public void EndCurrentState()
		{
			this.shellState.CurrentState.Finish(string.Empty);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00026C10 File Offset: 0x00024E10
		[CustomCommand]
		public void SwitchSettingsState(string stateName)
		{
			SettingsState settingsState = this.shellState.CurrentState as SettingsState;
			if (settingsState != null)
			{
				settingsState.CurrentState.Finish(stateName);
			}
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00026C48 File Offset: 0x00024E48
		[CustomCommand]
		public void SwitchHelpState(string stateName)
		{
			HelpState helpState = this.shellState.CurrentState as HelpState;
			if (helpState != null)
			{
				helpState.CurrentState.Finish(stateName);
			}
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00026C7E File Offset: 0x00024E7E
		[CustomCommand]
		public void SwitchToState(string stateName)
		{
			this.shellState.CurrentState.Finish(stateName);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00026CA0 File Offset: 0x00024EA0
		[CustomCommand]
		public void ExitApplication()
		{
			Tracer<AppController>.WriteInformation("Shutdown the application");
			this.logics.Dispose();
			Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
			{
				Application.Current.Shutdown();
			}), new object[0]);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00026CFC File Offset: 0x00024EFC
		[CustomCommand]
		public void PreviousState(object parameter)
		{
			BaseViewModel baseViewModel = parameter as BaseViewModel;
			if (baseViewModel != null)
			{
				this.SwitchToState(baseViewModel.PreviousStateName);
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00026D28 File Offset: 0x00024F28
		[CustomCommand(IsAsynchronous = true)]
		public void CheckForAppUpdate(object parameter, CancellationToken cancellationToken)
		{
			try
			{
				if (ApplicationBuildSettings.SkipApplicationUpdate || !this.IsAvailableAppUpdate())
				{
					base.Commands.Run((AppController c) => c.SwitchToState("AutomaticManufacturerSelectionState"));
				}
			}
			catch (Exception ex)
			{
				if (ex is PlannedServiceBreakException || ex is IOException)
				{
					throw;
				}
				throw new AutoUpdateException("Checking for application auto update failed.", ex);
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00026DF8 File Offset: 0x00024FF8
		[CustomCommand(IsAsynchronous = true)]
		public void SendNotification(NotificationMessage notificationData, CancellationToken cancellationToken)
		{
			lock (this.notificationLock)
			{
				Tracer<AppController>.WriteInformation("Sending notification - Header: {0}    message: {1}", new object[]
				{
					notificationData.Header,
					notificationData.Text
				});
				base.EventAggregator.Publish<NotificationMessage>(new NotificationMessage(true, notificationData.Header, notificationData.Text));
				Thread.Sleep(5000);
				base.EventAggregator.Publish<NotificationMessage>(new NotificationMessage(false, null, null));
			}
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00026EA0 File Offset: 0x000250A0
		[CustomCommand(IsAsynchronous = true)]
		public void InstallAppUpdate(object parameter, CancellationToken cancellationToken)
		{
			base.Commands.Run((AppController c) => c.SwitchToState("AppAutoUpdateState"));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00026F14 File Offset: 0x00025114
		[CustomCommand(IsAsynchronous = true)]
		public void UpdateApplication(object parameter, CancellationToken token)
		{
			Tracer<AppController>.WriteInformation("Start update application");
			string downloadPath = Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.AppUpdate);
			try
			{
				this.CheckFreeDiskSpace();
				this.logics.AutoUpdateService.DownloadProgressChanged += this.AutoUpdateServiceOnDownloadProgressChanged;
				string text = this.logics.AutoUpdateService.DownloadAppPacket(this.packageToDownload, downloadPath, token);
				if (!string.IsNullOrEmpty(text))
				{
					this.InstallPacket(text);
				}
			}
			catch (AutoUpdateNotEnoughSpaceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				Tracer<AppController>.WriteError(ex);
				if (!token.IsCancellationRequested)
				{
					throw new AutoUpdateException("Application auto update failed.", ex);
				}
			}
			finally
			{
				this.logics.AutoUpdateService.DownloadProgressChanged -= this.AutoUpdateServiceOnDownloadProgressChanged;
			}
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00027000 File Offset: 0x00025200
		[CustomCommand]
		public void StartSoftwareInstall(SwVersionComparisonResult softwareComparisonStatus)
		{
			this.StartSoftwareInstallStatus(new Tuple<SwVersionComparisonResult, string>(softwareComparisonStatus, "DisclaimerState"));
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00027020 File Offset: 0x00025220
		[CustomCommand]
		public void StartSoftwareInstallStatus(Tuple<SwVersionComparisonResult, string> comparisonStatusAndStateTuple)
		{
			if (comparisonStatusAndStateTuple.Item1 == SwVersionComparisonResult.FirstIsGreater)
			{
				if (new DialogMessageManager().ShowQuestionDialog(LocalizationManager.GetTranslation("DowngradeSoftwareQuestion"), null, true) == false)
				{
					return;
				}
			}
			base.Commands.Run((AppController c) => c.SwitchToState(comparisonStatusAndStateTuple.Item2));
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00027118 File Offset: 0x00025318
		[CustomCommand]
		public void CancelDownloadAppUpdate()
		{
			((IAsyncDelegateCommand)base.Commands["UpdateApplication"]).Cancel();
			base.Commands.Run((AppController c) => c.SwitchToState("CheckAppAutoUpdateState"));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000271A7 File Offset: 0x000253A7
		public void Handle(BlockWindowMessage blockWindowMessage)
		{
			this.isBlock = blockWindowMessage.Block;
			this.message = blockWindowMessage.Message;
			this.title = blockWindowMessage.Title;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000271D0 File Offset: 0x000253D0
		[CustomCommand(IsAsynchronous = true)]
		public void CloseAppOperations(MainWindow mainWindow, CancellationToken token)
		{
			Tracer<AppController>.LogEntry("CloseAppOperations");
			lock (this.appClosingLock)
			{
				if (this.appClosing)
				{
					Tracer<AppController>.WriteInformation("App is already being closed. Skipping operation");
					return;
				}
				this.appClosing = true;
			}
			Settings.Default.Save();
			if (this.CanAppClose(mainWindow))
			{
				this.logics.ReportingService.MsrReportingService.SessionReportsSendingCompleted += this.OnSessionReportsSendingCompleted;
				this.logics.ReportingService.SendSessionReports();
			}
			else
			{
				this.appClosing = false;
			}
			Tracer<AppController>.LogExit("CloseAppOperations");
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000272C4 File Offset: 0x000254C4
		private bool CanAppClose(MainWindow mainWindow)
		{
			if (this.isBlock)
			{
				Application.Current.Dispatcher.Invoke(delegate()
				{
					this.RestoreWindow(mainWindow);
				});
				if (new DialogMessageManager().ShowQuestionDialog(this.message, this.title, true) == false)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00027364 File Offset: 0x00025564
		private void RestoreWindow(MainWindow mainWindow)
		{
			if (mainWindow.WindowState == WindowState.Minimized)
			{
				SystemCommands.RestoreWindow(mainWindow);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002738B File Offset: 0x0002558B
		private void OnSessionReportsSendingCompleted()
		{
			this.ExitApplication();
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00027395 File Offset: 0x00025595
		private void AutoUpdateServiceOnDownloadProgressChanged(DownloadingProgressChangedEventArgs args)
		{
			base.EventAggregator.Publish<ProgressMessage>(new ProgressMessage(args.Percentage, args.Message, args.DownloadedSize, args.TotalSize, args.BytesPerSecond, args.SecondsLeft));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000273DC File Offset: 0x000255DC
		private void InstallPacket(string path)
		{
			if (this.IsInstallFile(path))
			{
				try
				{
					Process.Start(path);
					Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						Application.Current.Shutdown();
					}), new object[0]);
				}
				catch (Exception error)
				{
					Tracer<AppController>.WriteError(error);
				}
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00027458 File Offset: 0x00025658
		private bool IsInstallFile(string path)
		{
			return !string.IsNullOrEmpty(path) && (path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".msi", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".com", StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x000274A4 File Offset: 0x000256A4
		private void CheckFreeDiskSpace()
		{
			long size = this.packageToDownload.Size;
			long availableFreeSpace = this.GetAvailableFreeSpace();
			long num = Math.Max(size, 157286400L);
			if (num > availableFreeSpace)
			{
				Tracer<AppController>.WriteError("Not enough space on the disk", new object[0]);
				throw new AutoUpdateNotEnoughSpaceException
				{
					Available = availableFreeSpace,
					Needed = num,
					Disk = this.driveInfo.Name
				};
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002751C File Offset: 0x0002571C
		private long GetAvailableFreeSpace()
		{
			this.driveInfo = new DriveInfo(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.AppUpdate));
			return this.driveInfo.AvailableFreeSpace;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002754C File Offset: 0x0002574C
		private bool IsAvailableAppUpdate()
		{
			bool result;
			if (this.isDebugSession)
			{
				Tracer<AppController>.WriteInformation("Debug session: Skipping looking for app updates");
				result = false;
			}
			else
			{
				int applicationId = AppInfo.ApplicationId;
				string version = AppInfo.Version;
				bool useTestServer = this.CheckIfUseTestServer();
				ApplicationUpdate applicationUpdate = this.logics.AutoUpdateService.ReadLatestAppVersion(applicationId, version, useTestServer);
				if (applicationUpdate != null && !string.IsNullOrWhiteSpace(applicationUpdate.PackageUri))
				{
					base.EventAggregator.Publish<ApplicationUpdateMessage>(new ApplicationUpdateMessage(applicationUpdate));
					this.packageToDownload = applicationUpdate;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000275E0 File Offset: 0x000257E0
		private bool CheckIfUseTestServer()
		{
			string registryValue = ApplicationInfo.GetRegistryValue("UseTestServer");
			if (!string.IsNullOrEmpty(registryValue))
			{
				Tracer<AppController>.WriteInformation("Registry value data: {0}", new object[]
				{
					registryValue
				});
				bool result;
				if (bool.TryParse(registryValue, out result))
				{
					Tracer<AppController>.WriteInformation("Registry value parsed succesfully");
					return result;
				}
			}
			return false;
		}

		// Token: 0x04000347 RID: 839
		public const string IsTestServer = "UseTestServer";

		// Token: 0x04000348 RID: 840
		private readonly bool isDebugSession = false;

		// Token: 0x04000349 RID: 841
		private readonly object appClosingLock = new object();

		// Token: 0x0400034A RID: 842
		private readonly object notificationLock = new object();

		// Token: 0x0400034B RID: 843
		private readonly LogicContext logics;

		// Token: 0x0400034C RID: 844
		private ApplicationUpdate packageToDownload;

		// Token: 0x0400034D RID: 845
		private DriveInfo driveInfo;

		// Token: 0x0400034E RID: 846
		private string message;

		// Token: 0x0400034F RID: 847
		private string title;

		// Token: 0x04000350 RID: 848
		private bool isBlock;

		// Token: 0x04000351 RID: 849
		[Import]
		private ShellState shellState;

		// Token: 0x04000352 RID: 850
		private bool appClosing;
	}
}
