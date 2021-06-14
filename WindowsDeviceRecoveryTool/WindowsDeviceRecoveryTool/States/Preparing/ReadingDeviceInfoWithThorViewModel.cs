using System;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Controllers;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Localization;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Preparing
{
	// Token: 0x020000AA RID: 170
	[Export]
	public class ReadingDeviceInfoWithThorViewModel : BaseViewModel
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x00018A13 File Offset: 0x00016C13
		[ImportingConstructor]
		public ReadingDeviceInfoWithThorViewModel(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.AppContext = appContext;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00018A28 File Offset: 0x00016C28
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00018A40 File Offset: 0x00016C40
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00018A90 File Offset: 0x00016C90
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00018AA8 File Offset: 0x00016CA8
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage(LocalizationManager.GetTranslation("ReadingDeviceInfo"), ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(false));
			base.EventAggregator.Publish<BlockWindowMessage>(new BlockWindowMessage(true, null, null));
			base.Commands.Run((LumiaController c) => c.TryReadMissingInfoWithThor(null, CancellationToken.None));
		}

		// Token: 0x0400021B RID: 539
		private Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
