using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000C3 RID: 195
	[Export]
	public class AutomaticPackageSelectionViewModel : BaseViewModel, ICanHandle<PackageDirectoryMessage>, ICanHandle<CompatibleFfuFilesMessage>, ICanHandle<DeviceConnectionStatusReadMessage>, ICanHandle
	{
		// Token: 0x060005D1 RID: 1489 RVA: 0x0001E2FB File Offset: 0x0001C4FB
		[ImportingConstructor]
		public AutomaticPackageSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001E310 File Offset: 0x0001C510
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0001E328 File Offset: 0x0001C528
		public List<PackageFileInfo> FoundPackages
		{
			get
			{
				return this.foundPackages;
			}
			set
			{
				base.SetValue<List<PackageFileInfo>>(() => this.FoundPackages, ref this.foundPackages, value);
				base.RaisePropertyChanged<bool>(() => this.PackagesListVisible);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001E3B4 File Offset: 0x0001C5B4
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0001E3CC File Offset: 0x0001C5CC
		public PackageFileInfo SelectedPackage
		{
			get
			{
				return this.selectedPackage;
			}
			set
			{
				base.SetValue<PackageFileInfo>(() => this.SelectedPackage, ref this.selectedPackage, value);
				if (value != null)
				{
					this.appContext.CurrentPhone.PackageFilePath = value.Path;
					this.appContext.CurrentPhone.PackageFileInfo = value;
				}
				this.NextButtonEnabled = (value != null);
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001E460 File Offset: 0x0001C660
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0001E478 File Offset: 0x0001C678
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001E4C8 File Offset: 0x0001C6C8
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0001E4E0 File Offset: 0x0001C6E0
		public string StatusInfo
		{
			get
			{
				return this.statusInfo;
			}
			set
			{
				base.SetValue<string>(() => this.StatusInfo, ref this.statusInfo, value);
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001E530 File Offset: 0x0001C730
		// (set) Token: 0x060005DB RID: 1499 RVA: 0x0001E548 File Offset: 0x0001C748
		public bool CheckingPackageDirectory
		{
			get
			{
				return this.checkingPackageDirectory;
			}
			set
			{
				base.SetValue<bool>(() => this.CheckingPackageDirectory, ref this.checkingPackageDirectory, value);
				base.RaisePropertyChanged<bool>(() => this.PackagesListVisible);
				this.NextButtonEnabled = false;
				this.FoundPackages = null;
				this.PackagePath = string.Empty;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x0001E608 File Offset: 0x0001C808
		public bool NextButtonEnabled
		{
			get
			{
				return this.nextButtonEnabled;
			}
			set
			{
				base.SetValue<bool>(() => this.NextButtonEnabled, ref this.nextButtonEnabled, value);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001E658 File Offset: 0x0001C858
		public bool PackagesListVisible
		{
			get
			{
				return this.FoundPackages != null && this.FoundPackages.Any<PackageFileInfo>();
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001E688 File Offset: 0x0001C888
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0001E6A0 File Offset: 0x0001C8A0
		public string PackageDirectory
		{
			get
			{
				return this.packageDirectory;
			}
			set
			{
				base.SetValue<string>(() => this.PackageDirectory, ref this.packageDirectory, value);
				base.RaisePropertyChanged<string>(() => this.SelectedDirectoryDescription);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001E72C File Offset: 0x0001C92C
		// (set) Token: 0x060005E2 RID: 1506 RVA: 0x0001E744 File Offset: 0x0001C944
		public string PackagePath
		{
			get
			{
				return this.packagePath;
			}
			set
			{
				base.SetValue<string>(() => this.PackagePath, ref this.packagePath, value);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001E794 File Offset: 0x0001C994
		public string SelectedDirectoryDescription
		{
			get
			{
				string result;
				if (!string.IsNullOrEmpty(this.PackageDirectory))
				{
					result = string.Format(LocalizationManager.GetTranslation("PackageDirectory"), this.PackageDirectory);
				}
				else
				{
					result = LocalizationManager.GetTranslation("DirectoryNotSet");
				}
				return result;
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
		public override void OnStarted()
		{
			this.PackageDirectory = FileSystemInfo.GetCustomProductsPath();
			if (string.IsNullOrEmpty(this.PackageDirectory))
			{
				this.PackageDirectory = FileSystemInfo.GetLumiaPackagesPath("");
			}
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("AutomaticPackageSelection"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.OnStarted();
			this.FoundPackages = null;
			this.PackagePath = string.Empty;
			this.CheckingPackageDirectory = true;
			base.Commands.Run((FlowController c) => c.CheckIfDeviceStillConnected(this.AppContext.CurrentPhone, CancellationToken.None));
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001E928 File Offset: 0x0001CB28
		public void Handle(PackageDirectoryMessage message)
		{
			if (base.IsStarted)
			{
				this.PackageDirectory = message.Directory;
				if (!string.IsNullOrEmpty(this.PackageDirectory))
				{
					this.CheckingPackageDirectory = true;
				}
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001E96C File Offset: 0x0001CB6C
		public void Handle(CompatibleFfuFilesMessage message)
		{
			if (base.IsStarted)
			{
				this.CheckingPackageDirectory = false;
				switch (message.Packages.Count)
				{
				case 0:
					this.StatusInfo = LocalizationManager.GetTranslation("NoPackagesFoundSelectAnotherDirectory");
					break;
				case 1:
				{
					this.NextButtonEnabled = true;
					this.StatusInfo = string.Format(LocalizationManager.GetTranslation("OnePackageFound"), message.Packages.First<PackageFileInfo>().PackageId, LocalizationManager.GetTranslation("ButtonNext"));
					PackageFileInfo packageFileInfo = message.Packages.First<PackageFileInfo>();
					this.appContext.CurrentPhone.PackageFileInfo = packageFileInfo;
					this.appContext.CurrentPhone.PackageFilePath = packageFileInfo.Path;
					this.PackagePath = packageFileInfo.Path;
					break;
				}
				default:
					this.StatusInfo = string.Format(LocalizationManager.GetTranslation("MoreThanOnePackageFound"), LocalizationManager.GetTranslation("ButtonNext"));
					this.FoundPackages = message.Packages;
					break;
				}
			}
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001EA74 File Offset: 0x0001CC74
		public void Handle(DeviceConnectionStatusReadMessage message)
		{
			if (base.IsStarted)
			{
				if (!message.Status)
				{
					throw new DeviceNotFoundException();
				}
				if (this.AppContext.CurrentPhone == null || this.AppContext.CurrentPhone.PlatformId == null)
				{
					base.Commands.Run((AppController c) => c.SwitchToState("ManualPackageSelectionState"));
				}
				base.Commands.Run((FlowController c) => c.FindCorrectPackage(this.PackageDirectory, CancellationToken.None));
			}
		}

		// Token: 0x04000279 RID: 633
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400027A RID: 634
		private string statusInfo;

		// Token: 0x0400027B RID: 635
		private string packageDirectory;

		// Token: 0x0400027C RID: 636
		private string packagePath;

		// Token: 0x0400027D RID: 637
		private bool checkingPackageDirectory;

		// Token: 0x0400027E RID: 638
		private bool nextButtonEnabled;

		// Token: 0x0400027F RID: 639
		private List<PackageFileInfo> foundPackages;

		// Token: 0x04000280 RID: 640
		private PackageFileInfo selectedPackage;
	}
}
