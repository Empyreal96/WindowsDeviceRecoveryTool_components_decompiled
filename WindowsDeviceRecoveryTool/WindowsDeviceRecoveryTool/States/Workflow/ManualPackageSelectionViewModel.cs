using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000D0 RID: 208
	[Export]
	public class ManualPackageSelectionViewModel : BaseViewModel, ICanHandle<PackageDirectoryMessage>, ICanHandle<FfuFilePlatformIdMessage>, ICanHandle
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x00020EF7 File Offset: 0x0001F0F7
		[ImportingConstructor]
		public ManualPackageSelectionViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00020F0C File Offset: 0x0001F10C
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x00020F24 File Offset: 0x0001F124
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

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00020F74 File Offset: 0x0001F174
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00020FD8 File Offset: 0x0001F1D8
		public string FfuFilePath
		{
			get
			{
				string result;
				if (this.appContext.CurrentPhone != null)
				{
					result = this.appContext.CurrentPhone.PackageFilePath;
				}
				else
				{
					result = string.Empty;
				}
				return result;
			}
			set
			{
				if (this.appContext.CurrentPhone != null)
				{
					base.SetValue<string>(() => this.FfuFilePath, delegate()
					{
						this.appContext.CurrentPhone.PackageFilePath = value;
					});
					base.RaisePropertyChanged<string>(() => this.FilePathDescription);
					base.RaisePropertyChanged<bool>(() => this.IsNextCommandEnabled);
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000210D8 File Offset: 0x0001F2D8
		public string FilePathDescription
		{
			get
			{
				string result;
				if (!string.IsNullOrEmpty(this.FfuFilePath))
				{
					result = string.Format(LocalizationManager.GetTranslation("PackageFilePath"), this.FfuFilePath);
				}
				else
				{
					result = LocalizationManager.GetTranslation("PackagePathNotSet");
				}
				return result;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0002111C File Offset: 0x0001F31C
		public bool IsNextCommandEnabled
		{
			get
			{
				return this.appContext.CurrentPhone != null && (this.PlatformId != null || this.appContext.CurrentPhone.Type == PhoneTypes.Lumia) && !string.IsNullOrEmpty(this.FfuFilePath) && this.Compatibility;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00021184 File Offset: 0x0001F384
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x0002119C File Offset: 0x0001F39C
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x000211EC File Offset: 0x0001F3EC
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00021204 File Offset: 0x0001F404
		public PlatformId PlatformId
		{
			get
			{
				return this.platformId;
			}
			set
			{
				base.SetValue<PlatformId>(() => this.PlatformId, ref this.platformId, value);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00021254 File Offset: 0x0001F454
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x0002126C File Offset: 0x0001F46C
		public bool Compatibility
		{
			get
			{
				return this.compatibility;
			}
			set
			{
				base.SetValue<bool>(() => this.Compatibility, ref this.compatibility, value);
				base.RaisePropertyChanged<bool>(() => this.IsNextCommandEnabled);
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x000212F8 File Offset: 0x0001F4F8
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00021310 File Offset: 0x0001F510
		public bool CheckingPlatformId
		{
			get
			{
				return this.checkingPlatformId;
			}
			set
			{
				base.SetValue<bool>(() => this.CheckingPlatformId, ref this.checkingPlatformId, value);
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00021360 File Offset: 0x0001F560
		public void Handle(PackageDirectoryMessage message)
		{
			if (base.IsStarted)
			{
				this.PlatformId = null;
				this.FfuFilePath = message.Directory;
				if (!string.IsNullOrEmpty(this.FfuFilePath) && this.appContext.CurrentPhone.Type != PhoneTypes.Lumia)
				{
					this.CheckingPlatformId = true;
					this.StatusInfo = LocalizationManager.GetTranslation("FFUCheckingFile");
					base.Commands.Run((FfuController c) => c.ReadFfuFilePlatformId(this.FfuFilePath, CancellationToken.None));
				}
				else
				{
					this.CheckCompatibility();
				}
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002147C File Offset: 0x0001F67C
		public void Handle(FfuFilePlatformIdMessage message)
		{
			this.PlatformId = message.PlatformId;
			this.CheckingPlatformId = false;
			if (this.appContext.CurrentPhone != null)
			{
				this.CheckCompatibility();
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x000214C0 File Offset: 0x0001F6C0
		private void CheckCompatibility()
		{
			if (this.appContext.CurrentPhone.Type == PhoneTypes.Analog)
			{
				this.Compatibility = true;
				this.StatusInfo = (this.PlatformId.IsCompatibleWithDevicePlatformId(this.appContext.CurrentPhone.PlatformId) ? "File OK" : "Package do not match the device!");
			}
			else if (this.appContext.CurrentPhone.Type != PhoneTypes.Lumia)
			{
				this.Compatibility = this.PlatformId.IsCompatibleWithDevicePlatformId(this.appContext.CurrentPhone.PlatformId);
				this.StatusInfo = (this.Compatibility ? "File OK" : "Selected file is not compatible with connected phone!");
			}
			else
			{
				this.Compatibility = true;
				this.StatusInfo = "Compatibility forced to match for Lumia phones";
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00021594 File Offset: 0x0001F794
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("PackageSelection"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
			this.FfuFilePath = string.Empty;
			this.PlatformId = null;
			this.StatusInfo = string.Empty;
			this.Compatibility = false;
		}

		// Token: 0x040002AD RID: 685
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;

		// Token: 0x040002AE RID: 686
		private PlatformId platformId;

		// Token: 0x040002AF RID: 687
		private bool checkingPlatformId;

		// Token: 0x040002B0 RID: 688
		private bool compatibility;

		// Token: 0x040002B1 RID: 689
		private string statusInfo;
	}
}
