using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000D1 RID: 209
	[Export]
	public class RebootHtcViewModel : BaseViewModel, ICanHandle<FlashResultMessage>, ICanHandle<FirmwareVersionsCompareMessage>, ICanHandle
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00021618 File Offset: 0x0001F818
		// (set) Token: 0x0600066F RID: 1647 RVA: 0x00021630 File Offset: 0x0001F830
		public string Header
		{
			get
			{
				return this.header;
			}
			set
			{
				base.SetValue<string>(() => this.Header, ref this.header, value);
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00021680 File Offset: 0x0001F880
		public override void OnStarted()
		{
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			this.ResetText();
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002169C File Offset: 0x0001F89C
		public void Handle(FlashResultMessage message)
		{
			this.packageNotFound = message.Result;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x000216AC File Offset: 0x0001F8AC
		public void Handle(FirmwareVersionsCompareMessage message)
		{
			SwVersionComparisonResult status = message.Status;
			if (status == SwVersionComparisonResult.PackageNotFound)
			{
				this.packageNotFound = true;
				this.ResetText();
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x000216D8 File Offset: 0x0001F8D8
		private void ResetText()
		{
			this.Header = LocalizationManager.GetTranslation(this.packageNotFound ? "HtcPackageNotFound" : "OperationFailed");
			string translation = LocalizationManager.GetTranslation(this.packageNotFound ? "SoftwarePackage" : "RestartDeviceHeader");
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(translation, ""));
		}

		// Token: 0x040002B2 RID: 690
		private bool packageNotFound = true;

		// Token: 0x040002B3 RID: 691
		private string header;
	}
}
