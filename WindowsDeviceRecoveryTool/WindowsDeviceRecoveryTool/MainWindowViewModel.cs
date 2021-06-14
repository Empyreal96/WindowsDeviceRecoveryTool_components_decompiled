using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool
{
	// Token: 0x0200004E RID: 78
	[Export]
	public class MainWindowViewModel : BaseViewModel
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000F890 File Offset: 0x0000DA90
		public MainWindowViewModel()
		{
			this.ConfigureWindowButton();
			this.AppName = string.Format("{0} {1}", AppInfo.AppTitle(), AppInfo.AppVersion());
			if (ApplicationInfo.IsInternal())
			{
				this.AppName = "[INT] " + this.AppName;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x0000F908 File Offset: 0x0000DB08
		public string AppName
		{
			get
			{
				return this.appName;
			}
			set
			{
				base.SetValue<string>(() => this.AppName, ref this.appName, value);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000F958 File Offset: 0x0000DB58
		private void ConfigureWindowButton()
		{
			Type typeFromHandle = typeof(MainWindow);
			CommandBinding commandBinding = new CommandBinding(SystemCommands.MinimizeWindowCommand, new ExecutedRoutedEventHandler(this.MinimizeWindowCommandOnExecuted), new CanExecuteRoutedEventHandler(this.MinimizeWindowCommandOnCanExecute));
			CommandBinding commandBinding2 = new CommandBinding(SystemCommands.MaximizeWindowCommand, new ExecutedRoutedEventHandler(this.MaximizeWindowCommandOnExecuted), new CanExecuteRoutedEventHandler(this.MaximizeWindowCommandOnCanExecute));
			CommandBinding commandBinding3 = new CommandBinding(SystemCommands.RestoreWindowCommand, new ExecutedRoutedEventHandler(this.RestoreWindowCommandOnExecuted), new CanExecuteRoutedEventHandler(this.RestoreWindowCommandOnCanExecute));
			CommandBinding commandBinding4 = new CommandBinding(SystemCommands.CloseWindowCommand, new ExecutedRoutedEventHandler(this.CloseWindowCommandOnExecuted), new CanExecuteRoutedEventHandler(this.CloseWindowCommandOnCanExecute));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, commandBinding);
			CommandManager.RegisterClassCommandBinding(typeFromHandle, commandBinding2);
			CommandManager.RegisterClassCommandBinding(typeFromHandle, commandBinding3);
			CommandManager.RegisterClassCommandBinding(typeFromHandle, commandBinding4);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000FA1F File Offset: 0x0000DC1F
		private void MinimizeWindowCommandOnCanExecute(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		private void MinimizeWindowCommandOnExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			MainWindow mainWindow = sender as MainWindow;
			if (mainWindow != null)
			{
				SystemCommands.MinimizeWindow(mainWindow);
				args.Handled = true;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000FA5A File Offset: 0x0000DC5A
		private void MaximizeWindowCommandOnCanExecute(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000FA68 File Offset: 0x0000DC68
		private void MaximizeWindowCommandOnExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			MainWindow mainWindow = sender as MainWindow;
			if (mainWindow != null)
			{
				SystemCommands.MaximizeWindow(mainWindow);
				args.Handled = true;
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000FA96 File Offset: 0x0000DC96
		private void RestoreWindowCommandOnCanExecute(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		private void RestoreWindowCommandOnExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			MainWindow mainWindow = sender as MainWindow;
			if (mainWindow != null)
			{
				SystemCommands.RestoreWindow(mainWindow);
				args.Handled = true;
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000FAD2 File Offset: 0x0000DCD2
		private void CloseWindowCommandOnCanExecute(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = true;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000FAE0 File Offset: 0x0000DCE0
		private void CloseWindowCommandOnExecuted(object sender, ExecutedRoutedEventArgs args)
		{
			MainWindow mainWindow = sender as MainWindow;
			if (mainWindow != null)
			{
				SystemCommands.CloseWindow(mainWindow);
				args.Handled = true;
			}
		}

		// Token: 0x04000105 RID: 261
		private string appName;
	}
}
