using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x020000BC RID: 188
	[Export]
	public class Conditions
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001D87B File Offset: 0x0001BA7B
		[ImportingConstructor]
		public Conditions(Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext)
		{
			this.appContext = appContext;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001D890 File Offset: 0x0001BA90
		public bool CanChangeToFlashingState()
		{
			return this.appContext.CurrentPhone != null;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001D8B4 File Offset: 0x0001BAB4
		public bool IsHtcConnected()
		{
			return this.appContext.SelectedManufacturer == PhoneTypes.Htc && this.appContext.CurrentPhone != null;
		}

		// Token: 0x0400026C RID: 620
		private readonly Microsoft.WindowsDeviceRecoveryTool.ApplicationLogic.AppContext appContext;
	}
}
