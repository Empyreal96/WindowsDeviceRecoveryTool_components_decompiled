using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows.Input;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x02000099 RID: 153
	[Export]
	public class AbsoluteConfirmationViewModel : BaseViewModel, ICanHandle<DeviceConnectionStatusReadMessage>, ICanHandle
	{
		// Token: 0x06000438 RID: 1080 RVA: 0x0001447B File Offset: 0x0001267B
		[ImportingConstructor]
		public AbsoluteConfirmationViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
			this.ContinueCommand = new DelegateCommand<object>(new Action<object>(this.ContinueClicked));
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x000144A8 File Offset: 0x000126A8
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x000144BF File Offset: 0x000126BF
		public ICommand ContinueCommand { get; private set; }

		// Token: 0x0600043B RID: 1083 RVA: 0x000144C8 File Offset: 0x000126C8
		private void ContinueClicked(object obj)
		{
			base.Commands.Run((FlowController c) => c.CheckIfDeviceStillConnected(this.appContext.CurrentPhone, CancellationToken.None));
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00014578 File Offset: 0x00012778
		public override void OnStarted()
		{
			base.OnStarted();
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("WarningHeader"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(false, null, null));
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000145D4 File Offset: 0x000127D4
		public void Handle(DeviceConnectionStatusReadMessage message)
		{
			if (base.IsStarted)
			{
				if (!message.Status)
				{
					throw new DeviceNotFoundException();
				}
				base.Commands.Run((AppController c) => c.SwitchToState("FlashingState"));
			}
		}

		// Token: 0x040001DD RID: 477
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
