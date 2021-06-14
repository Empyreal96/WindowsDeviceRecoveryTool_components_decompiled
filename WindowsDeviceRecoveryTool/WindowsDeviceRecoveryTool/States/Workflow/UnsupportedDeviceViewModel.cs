using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Messages;

namespace Microsoft.WindowsDeviceRecoveryTool.States.Workflow
{
	// Token: 0x020000E3 RID: 227
	[Export]
	public class UnsupportedDeviceViewModel : BaseViewModel
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x00026B4C File Offset: 0x00024D4C
		public override string PreviousStateName
		{
			get
			{
				return "AutomaticManufacturerSelectionState";
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00026B63 File Offset: 0x00024D63
		public override void OnStarted()
		{
			base.EventAggregator.Publish<HeaderMessage>(new HeaderMessage("Unsupported device", ""));
			base.EventAggregator.Publish<IsBackButtonMessage>(new IsBackButtonMessage(true));
		}
	}
}
