using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Controls;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000AE RID: 174
	[Export]
	public class FolderBrowsingViewModel : BaseViewModel, ICanHandle<SettingsPreviousStateMessage>, ICanHandle<SelectedPathMessage>, ICanHandle
	{
		// Token: 0x060004FE RID: 1278 RVA: 0x00019A14 File Offset: 0x00017C14
		public FolderBrowsingViewModel()
		{
			this.SelectFolderCommand = new DelegateCommand<FolderItem>(new Action<FolderItem>(this.OnFolderSelection));
			this.OkClickedCommand = new DelegateCommand<object>(new Action<object>(this.OkClicked));
			this.GoUpCommand = new DelegateCommand<object>(new Action<object>(this.GoUpButtonClicked));
			this.NewFolderCommand = new DelegateCommand<object>(new Action<object>(this.NewFolderCreation));
			this.CancelCommand = new DelegateCommand<object>(new Action<object>(this.CancelClicked));
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x00019AA4 File Offset: 0x00017CA4
		// (set) Token: 0x06000500 RID: 1280 RVA: 0x00019ABB File Offset: 0x00017CBB
		public ICommand SelectFolderCommand { get; private set; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000501 RID: 1281 RVA: 0x00019AC4 File Offset: 0x00017CC4
		// (set) Token: 0x06000502 RID: 1282 RVA: 0x00019ADB File Offset: 0x00017CDB
		public ICommand OkClickedCommand { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00019AE4 File Offset: 0x00017CE4
		// (set) Token: 0x06000504 RID: 1284 RVA: 0x00019AFB File Offset: 0x00017CFB
		public ICommand CancelCommand { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00019B04 File Offset: 0x00017D04
		// (set) Token: 0x06000506 RID: 1286 RVA: 0x00019B1B File Offset: 0x00017D1B
		public ICommand GoUpCommand { get; private set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x00019B24 File Offset: 0x00017D24
		// (set) Token: 0x06000508 RID: 1288 RVA: 0x00019B3B File Offset: 0x00017D3B
		public ICommand NewFolderCommand { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00019B44 File Offset: 0x00017D44
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x00019B5C File Offset: 0x00017D5C
		public ObservableCollection<FolderItem> RootCollection
		{
			get
			{
				return this.rootCollection;
			}
			set
			{
				base.SetValue<ObservableCollection<FolderItem>>(() => this.RootCollection, ref this.rootCollection, value);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00019BAC File Offset: 0x00017DAC
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x00019BC4 File Offset: 0x00017DC4
		public ObservableCollection<FolderItem> FolderListItems
		{
			get
			{
				return this.folderListItems;
			}
			set
			{
				base.SetValue<ObservableCollection<FolderItem>>(() => this.folderListItems, ref this.folderListItems, value);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00019C04 File Offset: 0x00017E04
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x00019C1C File Offset: 0x00017E1C
		public FolderItem SelectedRoot
		{
			get
			{
				return this.selectedRoot;
			}
			set
			{
				base.SetValue<FolderItem>(() => this.SelectedRoot, ref this.selectedRoot, value);
				this.OnRootListSelectionChanged(this.selectedRoot);
				this.selectedRoot = null;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00019C80 File Offset: 0x00017E80
		// (set) Token: 0x06000510 RID: 1296 RVA: 0x00019C98 File Offset: 0x00017E98
		public bool GoUpButtonEnable
		{
			get
			{
				return this.enableGoUpButton;
			}
			set
			{
				base.SetValue<bool>(() => this.GoUpButtonEnable, ref this.enableGoUpButton, value);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x00019CE8 File Offset: 0x00017EE8
		// (set) Token: 0x06000512 RID: 1298 RVA: 0x00019D00 File Offset: 0x00017F00
		public bool OkButtonEnable
		{
			get
			{
				return this.enableOkButton;
			}
			set
			{
				base.SetValue<bool>(() => this.OkButtonEnable, ref this.enableOkButton, value);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x00019D50 File Offset: 0x00017F50
		// (set) Token: 0x06000514 RID: 1300 RVA: 0x00019D68 File Offset: 0x00017F68
		public string SelectedPath
		{
			get
			{
				return this.selectedPath;
			}
			set
			{
				base.SetValue<string>(() => this.SelectedPath, ref this.selectedPath, value);
				this.OkButtonEnable = (this.GoUpButtonEnable = !string.IsNullOrWhiteSpace(this.selectedPath));
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00019DD8 File Offset: 0x00017FD8
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("PleaseSelectAFolder"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			this.InitializeRootCollection();
			base.RaisePropertyChanged<FolderItem>(() => this.SelectedRoot);
			base.RaisePropertyChanged<ObservableCollection<FolderItem>>(() => this.RootCollection);
			base.RaisePropertyChanged<string>(() => this.SelectedPath);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00019ED0 File Offset: 0x000180D0
		private void InitializeRootCollection()
		{
			this.rootCollection = new ObservableCollection<FolderItem>();
			FolderItem item = new FolderItem
			{
				Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				Title = LocalizationManager.GetTranslation("Desktop"),
				Type = FolderType.Desktop
			};
			this.rootCollection.Add(item);
			FolderItem item2 = new FolderItem
			{
				Path = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
				Title = LocalizationManager.GetTranslation("MyComputer"),
				Type = FolderType.Folder
			};
			this.rootCollection.Add(item2);
			FolderItem item3 = new FolderItem
			{
				Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
				Title = Environment.UserName,
				Type = FolderType.User
			};
			this.rootCollection.Add(item3);
			this.selectedRoot = null;
			if (string.IsNullOrWhiteSpace(this.SelectedPath))
			{
				this.FolderListItems = this.GetSubfolders(this.SelectedPath);
			}
			else
			{
				this.FolderListItems = this.GetSubfolders(Path.GetDirectoryName(this.SelectedPath));
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001A220 File Offset: 0x00018420
		private ObservableCollection<FolderItem> GetSubfolders(string path)
		{
			ObservableCollection<FolderItem> observableCollection = new ObservableCollection<FolderItem>();
			try
			{
				if (string.IsNullOrWhiteSpace(path))
				{
					foreach (FolderItem item in from drive in Directory.GetLogicalDrives()
					select new FolderItem
					{
						Path = drive,
						Title = drive,
						Items = this.GetSubfolders(drive)
					})
					{
						observableCollection.Add(item);
					}
				}
				else
				{
					foreach (FolderItem item2 in from dir in Directory.GetDirectories(path)
					let info = new DirectoryInfo(dir)
					where info.Exists && !info.Attributes.HasFlag(FileAttributes.Hidden)
					select new FolderItem
					{
						Path = dir,
						Title = dir.Substring(dir.LastIndexOf("\\", StringComparison.Ordinal) + 1),
						Type = FolderType.Folder
					})
					{
						observableCollection.Add(item2);
					}
				}
			}
			catch (Exception)
			{
			}
			return observableCollection;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001A3BC File Offset: 0x000185BC
		private void OnFolderSelection(FolderItem folder)
		{
			if (folder != null)
			{
				ObservableCollection<FolderItem> subfolders = this.GetSubfolders(folder.Path);
				if (subfolders.Count > 0)
				{
					this.FolderListItems = subfolders;
				}
				this.SelectedPath = folder.Path;
				this.CheckBackButtonEnable(folder.Path);
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001A41C File Offset: 0x0001861C
		private void OnRootListSelectionChanged(FolderItem item)
		{
			if (item != null)
			{
				this.FolderListItems = this.GetSubfolders(item.Path);
				this.SelectedPath = item.Path;
				this.CheckBackButtonEnable(item.Path);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001A461 File Offset: 0x00018661
		private void CheckBackButtonEnable(string path)
		{
			this.GoUpButtonEnable = !string.IsNullOrWhiteSpace(path);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001A474 File Offset: 0x00018674
		private void GoUpButtonClicked(object sender)
		{
			if (this.FolderListItems != null)
			{
				string path = string.Empty;
				if (this.FolderListItems.Count > 0)
				{
					FolderItem folderItem = this.FolderListItems[0];
					if (folderItem != null)
					{
						path = Path.GetDirectoryName(Path.GetDirectoryName(folderItem.Path));
						this.SelectedPath = path;
						this.FolderListItems = this.GetSubfolders(path);
					}
				}
				else
				{
					path = Path.GetDirectoryName(this.SelectedPath);
					this.SelectedPath = path;
					this.FolderListItems = this.GetSubfolders(path);
				}
				this.CheckBackButtonEnable(path);
			}
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001A524 File Offset: 0x00018724
		private void OkClicked(object sender)
		{
			base.Commands.Run((AppController c) => c.SwitchToState("SettingsState"));
			base.Commands.Run((AppController c) => c.SwitchSettingsState(this.previousState));
			if ("PackagesState".Equals(this.previousState))
			{
				base.Commands.Run((SettingsController s) => s.SetPackagesPathDirectory(this.selectedPath, CancellationToken.None));
			}
			else
			{
				base.EventAggregator.Publish<TraceParametersMessage>(new TraceParametersMessage(this.selectedPath, false));
				base.Commands.Run((SettingsController s) => s.CollectLogs(this.selectedPath, CancellationToken.None));
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001A768 File Offset: 0x00018968
		private void CancelClicked(object sender)
		{
			base.Commands.Run((AppController c) => c.SwitchToState("SettingsState"));
			base.Commands.Run((AppController c) => c.SwitchSettingsState(this.previousState));
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001A854 File Offset: 0x00018A54
		private void NewFolderCreation(object sender)
		{
			if (!string.IsNullOrWhiteSpace(this.selectedPath))
			{
				string path = this.selectedPath;
				MetroTextBlockDialog metroTextBlockDialog = new MetroTextBlockDialog
				{
					MessageTitle = LocalizationManager.GetTranslation("CreatingNewFolderMessage"),
					InputText = LocalizationManager.GetTranslation("ButtonNewFolder"),
					NoButtonText = LocalizationManager.GetTranslation("ButtonCancel"),
					YesButtonText = LocalizationManager.GetTranslation("ButtonOk")
				};
				if (metroTextBlockDialog.ShowDialog() == true)
				{
					try
					{
						string path2 = Path.Combine(path, metroTextBlockDialog.InputText);
						Directory.CreateDirectory(path2);
						this.FolderListItems = this.GetSubfolders(path2);
						this.SelectedPath = path2;
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001A93C File Offset: 0x00018B3C
		public void Handle(SettingsPreviousStateMessage message)
		{
			if (!string.IsNullOrEmpty(message.PreviousState))
			{
				this.previousState = message.PreviousState;
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001A968 File Offset: 0x00018B68
		public void Handle(SelectedPathMessage message)
		{
			if (!string.IsNullOrEmpty(message.SelectedPath))
			{
				this.SelectedPath = message.SelectedPath;
			}
		}

		// Token: 0x04000229 RID: 553
		private string selectedPath;

		// Token: 0x0400022A RID: 554
		private ObservableCollection<FolderItem> rootCollection;

		// Token: 0x0400022B RID: 555
		private ObservableCollection<FolderItem> folderListItems;

		// Token: 0x0400022C RID: 556
		private FolderItem selectedRoot;

		// Token: 0x0400022D RID: 557
		private bool enableGoUpButton;

		// Token: 0x0400022E RID: 558
		private bool enableOkButton;

		// Token: 0x0400022F RID: 559
		private string previousState;
	}
}
