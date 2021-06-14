using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Properties;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Settings
{
	// Token: 0x020000AF RID: 175
	[Export]
	public class PackagesViewModel : BaseViewModel, ICanHandle<PackageDirectoryMessage>, ICanHandle
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001A994 File Offset: 0x00018B94
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x0001A9CC File Offset: 0x00018BCC
		public string PackagesPath
		{
			get
			{
				return Settings.Default.PackagesPath;
			}
			set
			{
				base.SetValue<string>(() => this.PackagesPath, delegate()
				{
					Settings.Default.PackagesPath = value;
				});
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001AA38 File Offset: 0x00018C38
		// (set) Token: 0x06000528 RID: 1320 RVA: 0x0001AA54 File Offset: 0x00018C54
		public bool CustomPackagesPathEnabled
		{
			get
			{
				return Settings.Default.CustomPackagesPathEnabled;
			}
			set
			{
				Settings.Default.CustomPackagesPathEnabled = value;
				FileSystemInfo.CustomPackagesPath = (value ? this.PackagesPath : string.Empty);
				base.RaisePropertyChanged<bool>(() => this.CustomPackagesPathEnabled);
				base.RaisePropertyChanged<bool>(() => this.CustomPathVisible);
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0001AAFC File Offset: 0x00018CFC
		public bool CustomPathVisible
		{
			get
			{
				return this.CustomPackagesPathEnabled && !string.IsNullOrEmpty(this.PackagesPath);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001AB28 File Offset: 0x00018D28
		public override void OnStopped()
		{
			if (this.CustomPackagesPathEnabled && string.IsNullOrEmpty(this.PackagesPath))
			{
				this.CustomPackagesPathEnabled = false;
			}
			else
			{
				Settings.Default.Save();
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001AB6C File Offset: 0x00018D6C
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("Settings"), LocalizationManager.GetTranslation("Packages")));
			base.RaisePropertyChanged<bool>(() => this.CustomPackagesPathEnabled);
			base.RaisePropertyChanged<string>(() => this.PackagesPath);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001AC18 File Offset: 0x00018E18
		public void Handle(PackageDirectoryMessage message)
		{
			if (base.IsStarted)
			{
				this.PackagesPath = message.Directory;
				this.CustomPackagesPathEnabled = !string.IsNullOrEmpty(this.PackagesPath);
				base.RaisePropertyChanged<bool>(() => this.CustomPathVisible);
			}
		}
	}
}
