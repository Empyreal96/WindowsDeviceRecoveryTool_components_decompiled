using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;
using Microsoft.WindowsDeviceRecoveryTool.Properties;
using Microsoft.WindowsDeviceRecoveryTool.States.Error;
using Microsoft.WindowsDeviceRecoveryTool.States.Shell;
using Microsoft.WindowsDeviceRecoveryTool.Styles.Assets;

namespace Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic
{
	// Token: 0x02000003 RID: 3
	public class AppBootstrapper
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020B8 File Offset: 0x000002B8
		public AppBootstrapper()
		{
			this.InitializeSettings();
			if (!this.CantRunApplication())
			{
				this.ShowSplashScreen();
				this.InitializeApplication();
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020F4 File Offset: 0x000002F4
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000210B File Offset: 0x0000030B
		private protected AppContext AppContext { protected get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002114 File Offset: 0x00000314
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000212B File Offset: 0x0000032B
		protected ShellState ShellState { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002134 File Offset: 0x00000334
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000214B File Offset: 0x0000034B
		private protected CompositionContainer Container { protected get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002154 File Offset: 0x00000354
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000216B File Offset: 0x0000036B
		private protected ICommandRepository CommandRepository { protected get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002174 File Offset: 0x00000374
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000218B File Offset: 0x0000038B
		private protected Application Application { protected get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002194 File Offset: 0x00000394
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000021AB File Offset: 0x000003AB
		private protected MainWindow ShellWindow { protected get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000021B4 File Offset: 0x000003B4
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000021CB File Offset: 0x000003CB
		private protected EventAggregator EventAggregator { protected get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000021D4 File Offset: 0x000003D4
		public static bool IsInDesignMode
		{
			get
			{
				if (AppBootstrapper.isInDesignMode == null)
				{
					AppBootstrapper.isInDesignMode = new bool?(Application.Current != null && (Application.Current.ToString() == "System.Windows.Application" || Application.Current.ToString() == "Microsoft.Expression.Blend.BlendApplication"));
				}
				return AppBootstrapper.isInDesignMode.GetValueOrDefault(false);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002244 File Offset: 0x00000444
		protected bool CantRunApplication()
		{
			bool result;
			if (AppBootstrapper.IsInDesignMode)
			{
				result = true;
			}
			else if (AppInfo.IsAnotherInstanceRunning())
			{
				string message = string.Format(LocalizationManager.GetTranslation("AnotherInstanceAlreadyRunning"), AppInfo.AppTitle());
				this.ShowSthWentWrongMessage(message);
				result = true;
			}
			else if (DateTime.UtcNow >= ApplicationBuildSettings.ExpirationDate)
			{
				Tracer<AppBootstrapper>.WriteInformation("Build is out of date!");
				this.ShowSthWentWrongMessage("Windows Device Recovery Tool build has expired, please install a new version.");
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022C8 File Offset: 0x000004C8
		protected void InitializeSettings()
		{
			this.RestorePreviousSettings();
			StyleLogic.RestoreStyle(Settings.Default.Style);
			StyleLogic.LoadTheme(Settings.GetSelectedThemeFileName());
			if (Settings.Default.CustomPackagesPathEnabled && !string.IsNullOrWhiteSpace(Settings.Default.PackagesPath))
			{
				Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CustomPackagesPath = Settings.Default.PackagesPath;
			}
			else
			{
				Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.CustomPackagesPath = null;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002338 File Offset: 0x00000538
		private void RestorePreviousSettings()
		{
			if (Settings.Default.CallUpgrade)
			{
				Tracer<AppBootstrapper>.WriteInformation("Settings upgrade needed");
				Settings.Default.Upgrade();
				Settings.Default.CallUpgrade = false;
				if (!StyleLogic.IfStyleExists(Settings.Default.Style))
				{
					Tracer<AppBootstrapper>.WriteInformation("Saved Style doesn't exist. Need to reset it to default");
					Settings.Default.Style = (Settings.Default.Properties["Style"].DefaultValue as string);
					Settings.Default.Theme = (Settings.Default.Properties["Theme"].DefaultValue as string);
				}
				Settings.Default.Save();
				Tracer<AppBootstrapper>.WriteInformation("Settings are upgraded");
			}
			LocalizationManager.Instance().CurrentLanguage = ApplicationInfo.CurrentLanguageInRegistry;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002418 File Offset: 0x00000618
		protected void InitializeApplication()
		{
			try
			{
				Tracer<AppBootstrapper>.WriteInformation("Operating system version: {0}", new object[]
				{
					Environment.OSVersion.ToString()
				});
				this.Application = Application.Current;
				this.ConfigureDispacher();
				this.ConfigureLogging();
				this.ConfigureMef();
				this.ClearUpdatesFolder();
			}
			catch (Exception ex)
			{
				this.ShowSthWentWrongMessage(ex);
			}
			this.InitializeBusinessLogicLayer();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002498 File Offset: 0x00000698
		protected void InitializeUiLogic()
		{
			try
			{
				this.ConfigureCommands();
				this.ConfigureApplication();
				this.ConfigureShellWindow();
				this.Application.MainWindow = this.ShellWindow;
				this.CloseSplashScreen();
				this.ShellWindow.Show();
				this.ShellWindow.DataContext = this.Container.GetExportedValue<MainWindowViewModel>();
				this.ShellWindow.ContentRendered += this.ShellWindowOnContentRendered;
			}
			catch (Exception ex)
			{
				this.ShowSthWentWrongMessage(ex);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002542 File Offset: 0x00000742
		protected void ShellWindowOnContentRendered(object sender, EventArgs eventArgs)
		{
			AppDispatcher.Execute(delegate
			{
				this.ShellState.StartStateMachine();
			}, false);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002558 File Offset: 0x00000758
		protected void InitializeBusinessLogicLayer()
		{
			this.businessLogicHostThread = new Thread(new ThreadStart(this.StartBusinessLogic));
			this.businessLogicHostThread.Start();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002580 File Offset: 0x00000780
		protected void StartBusinessLogic()
		{
			try
			{
				Tracer<AppBootstrapper>.WriteInformation("Start initializing a business logic.");
				this.logicContext = this.Container.GetExportedValue<LogicContext>();
				Tracer<AppBootstrapper>.WriteInformation("The business logic is initialized.");
				AppDispatcher.Execute(new Action(this.InitializeUiLogic), false);
			}
			catch (Exception ex)
			{
				this.ShowSthWentWrongMessage(ex);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000025EC File Offset: 0x000007EC
		protected void ClearUpdatesFolder()
		{
			try
			{
				string[] files = Directory.GetFiles(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.AppUpdate));
				if (files.Any<string>())
				{
					foreach (string path in files)
					{
						File.Delete(path);
					}
				}
			}
			catch (Exception ex)
			{
				Tracer<AppBootstrapper>.WriteError("Cannot delete content of Updates folder!", new object[]
				{
					ex
				});
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002674 File Offset: 0x00000874
		protected void ConfigureLogging()
		{
			try
			{
				if (Settings.Default.TraceEnabled)
				{
					TraceManager.Instance.EnableDiagnosticLogs(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppDataPath(SpecialFolder.Traces), Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppNamePrefix);
					Tracer<AppBootstrapper>.WriteInformation("App version: {0} (running on: {1})", new object[]
					{
						AppInfo.Version,
						Environment.OSVersion
					});
					this.isLoggerConfigured = true;
				}
			}
			catch (Exception ex)
			{
				this.ShowSthWentWrongMessage(ex);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002730 File Offset: 0x00000930
		protected void ConfigureCommands()
		{
			Tracer<AppBootstrapper>.LogEntry("ConfigureCommands");
			List<Type> list = (from type in Assembly.GetExecutingAssembly().GetTypes()
			where typeof(IController).IsAssignableFrom(type) && type.FullName.Contains("Microsoft.WindowsDeviceRecoveryTool.Controllers")
			select type).ToList<Type>();
			foreach (Type type2 in list)
			{
				this.Container.GetExportedValue<IController>(type2.FullName);
			}
			this.CommandRepository = this.Container.GetExportedValue<ICommandRepository>();
			Tracer<AppBootstrapper>.LogExit("ConfigureCommands");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000027F0 File Offset: 0x000009F0
		protected void ConfigureDispacher()
		{
			AppDispatcher.Initialize(Dispatcher.CurrentDispatcher);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002800 File Offset: 0x00000A00
		protected void ConfigureMef()
		{
			AggregateCatalog catalog = new AggregateCatalog(new ComposablePartCatalog[]
			{
				new AssemblyCatalog(Assembly.GetExecutingAssembly()),
				new DirectoryCatalog(Microsoft.WindowsDeviceRecoveryTool.Model.FileSystemInfo.AppPath, "Microsoft.WindowsDeviceRecoveryTool*.dll")
			});
			this.Container = new CompositionContainer(catalog, new ExportProvider[0]);
			this.Container.ComposeExportedValue(this.Container);
			try
			{
				this.Container.ComposeParts(new object[0]);
			}
			catch (CompositionException ex)
			{
				Tracer<AppBootstrapper>.WriteError(ex.Message, new object[0]);
			}
			this.AppContext = this.Container.GetExportedValue<AppContext>();
			this.EventAggregator = this.Container.GetExportedValue<EventAggregator>();
			Tracer<AppBootstrapper>.WriteInformation("The container configured.");
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028D0 File Offset: 0x00000AD0
		protected void ConfigureApplication()
		{
			Tracer<AppBootstrapper>.LogEntry("ConfigureApplication");
			this.Application.DispatcherUnhandledException += this.OnUnhandledException;
			AppDomain.CurrentDomain.UnhandledException += this.OnCurrentDomainUnhandledException;
			this.ShellState = this.Container.GetExportedValue<ShellState>();
			this.ShellState.Container = this.Container;
			this.ShellState.InitializeStateMachine();
			Tracer<AppBootstrapper>.LogExit("ConfigureApplication");
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002954 File Offset: 0x00000B54
		protected void ConfigureShellWindow()
		{
			Tracer<AppBootstrapper>.LogEntry("ConfigureShellWindow");
			this.ShellWindow = this.Container.GetExportedValue<MainWindow>("ShellWindow");
			ShellView exportedValue = this.Container.GetExportedValue<ShellView>();
			exportedValue.DataContext = this.Container.GetExportedValue<ShellViewModel>();
			this.ShellWindow.Root.Children.Add(exportedValue);
			this.ShellWindow.Closing += this.OnWindowClosing;
			Tracer<AppBootstrapper>.LogExit("ConfigureShellWindow");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029DC File Offset: 0x00000BDC
		protected void OnWindowClosing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.CommandRepository.Run((AppController c) => c.CloseAppOperations(this.ShellWindow, CancellationToken.None));
			Tracer<AppBootstrapper>.WriteInformation("Starting closing App.");
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A90 File Offset: 0x00000C90
		protected void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			this.OnExceptionOccured(e.ExceptionObject as Exception);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002AA5 File Offset: 0x00000CA5
		protected void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			e.Handled = true;
			this.OnExceptionOccured(e.Exception);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002AC0 File Offset: 0x00000CC0
		private void OnExceptionOccured(Exception exception)
		{
			Tracer<AppBootstrapper>.WriteError(exception);
			ErrorView exportedValue = this.Container.GetExportedValue<ErrorView>();
			ErrorTemplateSelector errorTemplateSelector = exportedValue.Resources["ErrorSelector"] as ErrorTemplateSelector;
			if (errorTemplateSelector != null && errorTemplateSelector.IsImplemented(exception))
			{
				this.EventAggregator.Publish<ErrorMessage>(new ErrorMessage(exception));
				this.CommandRepository.Run((AppController c) => c.SwitchToState("ErrorState"));
			}
			else
			{
				this.ShowSthWentWrongMessage(exception);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B9C File Offset: 0x00000D9C
		private void ShowSthWentWrongMessage(Exception ex)
		{
			if (this.isLoggerConfigured)
			{
				Tracer<AppBootstrapper>.WriteError(ex);
			}
			this.ShowSthWentWrongMessage(ex.Message);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002BD0 File Offset: 0x00000DD0
		private void ShowSthWentWrongMessage(string message)
		{
			ExtendedMessageBox extendedMessageBox = new ExtendedMessageBox();
			if (this.ShellWindow != null && this.ShellWindow.IsLoaded)
			{
				extendedMessageBox.Owner = this.ShellWindow;
			}
			extendedMessageBox.MessageBoxText = LocalizationManager.GetTranslation("SthWentWrongMessage");
			extendedMessageBox.MessageBoxAdvance = message;
			extendedMessageBox.Title = LocalizationManager.GetTranslation("Error");
			extendedMessageBox.ShowDialog();
			if ((this.ShellWindow == null || !this.ShellWindow.IsLoaded) && this.Application != null)
			{
				this.Application.Shutdown();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C76 File Offset: 0x00000E76
		private void ShowSplashScreen()
		{
			this.splashScreen = new SplashScreen("Resources/splashScreen.png");
			this.splashScreen.Show(false);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C96 File Offset: 0x00000E96
		private void CloseSplashScreen()
		{
			this.splashScreen.Close(new TimeSpan(100L));
		}

		// Token: 0x04000002 RID: 2
		private static bool? isInDesignMode;

		// Token: 0x04000003 RID: 3
		private Thread businessLogicHostThread;

		// Token: 0x04000004 RID: 4
		private LogicContext logicContext;

		// Token: 0x04000005 RID: 5
		private bool isLoggerConfigured;

		// Token: 0x04000006 RID: 6
		private SplashScreen splashScreen;
	}
}
