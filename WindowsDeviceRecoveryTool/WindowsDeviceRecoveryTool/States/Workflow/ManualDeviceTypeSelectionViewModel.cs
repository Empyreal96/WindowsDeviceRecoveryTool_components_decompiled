using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.DataPackage;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000B6 RID: 182
	[Export]
	public class ManualDeviceTypeSelectionViewModel : BaseViewModel, ICanHandle<CompatibleFfuFilesMessage>, ICanHandle
	{
		// Token: 0x0600054F RID: 1359 RVA: 0x0001B4E4 File Offset: 0x000196E4
		[ImportingConstructor]
		public ManualDeviceTypeSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.SelectDeviceCommand = new DelegateCommand<object>(new Action<object>(this.DeviceSelected));
			this.ShowDeviceCannotBeRecoveredInfo = new DelegateCommand<object>(delegate(object o)
			{
				this.ChangeViewState();
			});
			this.FillSupportedDeviceTypes();
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0001B544 File Offset: 0x00019744
		// (set) Token: 0x06000551 RID: 1361 RVA: 0x0001B55B File Offset: 0x0001975B
		public ICommand SelectDeviceCommand { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0001B564 File Offset: 0x00019764
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x0001B57B File Offset: 0x0001977B
		public ICommand ShowDeviceCannotBeRecoveredInfo { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001B584 File Offset: 0x00019784
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x0001B59C File Offset: 0x0001979C
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x0001B628 File Offset: 0x00019828
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0001B640 File Offset: 0x00019840
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001B6D4 File Offset: 0x000198D4
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x0001B6EC File Offset: 0x000198EC
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001B73C File Offset: 0x0001993C
		public bool PackagesListVisible
		{
			get
			{
				return this.FoundPackages != null && this.FoundPackages.Any<PackageFileInfo>();
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001B768 File Offset: 0x00019968
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001B780 File Offset: 0x00019980
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x0001B798 File Offset: 0x00019998
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

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001B7E8 File Offset: 0x000199E8
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001B800 File Offset: 0x00019A00
		internal ManualDeviceTypeSelectionState CurrentState
		{
			get
			{
				return this.currentState;
			}
			set
			{
				base.SetValue<ManualDeviceTypeSelectionState>(() => this.CurrentState, ref this.currentState, value);
				base.RaisePropertyChanged<bool>(() => this.DeviceCannotBeRecovered);
				base.RaisePropertyChanged<bool>(() => this.FfuSelection);
				base.RaisePropertyChanged<bool>(() => this.TypeDesignatorSelection);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001B904 File Offset: 0x00019B04
		public bool DeviceCannotBeRecovered
		{
			get
			{
				return this.currentState == ManualDeviceTypeSelectionState.DeviceCannotBeRecovered;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0001B920 File Offset: 0x00019B20
		public bool FfuSelection
		{
			get
			{
				return this.currentState == ManualDeviceTypeSelectionState.FfuSelection;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0001B93C File Offset: 0x00019B3C
		public bool TypeDesignatorSelection
		{
			get
			{
				return this.currentState == ManualDeviceTypeSelectionState.TypeDesignatorSelection;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001B958 File Offset: 0x00019B58
		// (set) Token: 0x06000564 RID: 1380 RVA: 0x0001B970 File Offset: 0x00019B70
		public List<Phone> SupportedDeviceTypes
		{
			get
			{
				return this.supportedDeviceTypes;
			}
			set
			{
				base.SetValue<List<Phone>>(() => this.SupportedDeviceTypes, ref this.supportedDeviceTypes, value);
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		private void FillSupportedDeviceTypes()
		{
			this.SupportedDeviceTypes = new List<Phone>();
			List<string> list = new List<string>
			{
				"RM-927",
				"RM-937",
				"RM-938",
				"RM-939",
				"RM-940",
				"RM-974",
				"RM-975",
				"RM-976",
				"RM-977",
				"RM-978",
				"RM-979",
				"RM-983",
				"RM-984",
				"RM-985",
				"RM-1010",
				"RM-1017",
				"RM-1018",
				"RM-1019",
				"RM-1020",
				"RM-1027",
				"RM-1031",
				"RM-1032",
				"RM-1034",
				"RM-1038",
				"RM-1039",
				"RM-1040",
				"RM-1041",
				"RM-1045",
				"RM-1049",
				"RM-1050",
				"RM-1051",
				"RM-1052",
				"RM-1062",
				"RM-1064",
				"RM-1066",
				"RM-1067",
				"RM-1068",
				"RM-1069",
				"RM-1070",
				"RM-1071",
				"RM-1072",
				"RM-1073",
				"RM-1075",
				"RM-1077",
				"RM-1078",
				"RM-1087",
				"RM-1089",
				"RM-1090",
				"RM-1091",
				"RM-1092",
				"RM-1109",
				"RM-1113",
				"RM-1114",
				"RM-1115"
			};
			foreach (string text in list)
			{
				this.SupportedDeviceTypes.Add(new Phone(SalesNames.NameForProductType(text), text));
			}
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001BCD0 File Offset: 0x00019ED0
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ManualDeviceSelection"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			base.OnStarted();
			this.FoundPackages = null;
			this.CurrentState = ManualDeviceTypeSelectionState.FfuSelection;
			this.StatusInfo = LocalizationManager.GetTranslation("ConnectedDeviceCannotBeRecovered");
			string productsPath = FileSystemInfo.GetLumiaProductsPath("");
			base.Commands.Run((FlowController c) => c.FindAllLumiaPackages(productsPath, CancellationToken.None));
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001BDE4 File Offset: 0x00019FE4
		public void Handle(CompatibleFfuFilesMessage message)
		{
			if (message.Packages.Count == 0)
			{
				this.CurrentState = ManualDeviceTypeSelectionState.TypeDesignatorSelection;
			}
			else
			{
				this.FoundPackages = message.Packages;
				foreach (PackageFileInfo packageFileInfo in this.FoundPackages)
				{
					packageFileInfo.SalesName = SalesNames.NameForPackageName(packageFileInfo.Name);
				}
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001BE78 File Offset: 0x0001A078
		private void DeviceSelected(object obj)
		{
			if (obj is Phone && string.IsNullOrWhiteSpace(this.appContext.CurrentPhone.HardwareModel))
			{
				this.appContext.CurrentPhone.HardwareModel = ((Phone)obj).HardwareModel;
			}
			else
			{
				this.SelectedPackage = (PackageFileInfo)obj;
			}
			base.Commands.Run((AppController c) => c.SwitchToState("DownloadEmergencyPackageState"));
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001BF44 File Offset: 0x0001A144
		private void ChangeViewState()
		{
			if (this.currentState == ManualDeviceTypeSelectionState.FfuSelection)
			{
				this.CurrentState = ManualDeviceTypeSelectionState.TypeDesignatorSelection;
			}
			else
			{
				this.CurrentState = ManualDeviceTypeSelectionState.DeviceCannotBeRecovered;
			}
		}

		// Token: 0x0400024E RID: 590
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x0400024F RID: 591
		private bool nextButtonEnabled;

		// Token: 0x04000250 RID: 592
		private List<PackageFileInfo> foundPackages;

		// Token: 0x04000251 RID: 593
		private PackageFileInfo selectedPackage;

		// Token: 0x04000252 RID: 594
		private string statusInfo;

		// Token: 0x04000253 RID: 595
		private List<Phone> supportedDeviceTypes;

		// Token: 0x04000254 RID: 596
		private ManualDeviceTypeSelectionState currentState;
	}
}
