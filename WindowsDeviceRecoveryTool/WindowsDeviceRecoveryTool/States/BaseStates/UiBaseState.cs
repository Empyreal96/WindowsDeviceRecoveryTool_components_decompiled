using System;
using System.Collections.Generic;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x020000C0 RID: 192
	public abstract class UiBaseState : BaseState
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x0001E1EA File Offset: 0x0001C3EA
		public void ShowRegions(params string[] regions)
		{
			this.VisibleRegions.AddRange(regions);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001E1FA File Offset: 0x0001C3FA
		public void HideRegions(params string[] regions)
		{
			this.InvisibleRegions.AddRange(regions);
		}

		// Token: 0x04000274 RID: 628
		protected readonly List<string> VisibleRegions = new List<string>();

		// Token: 0x04000275 RID: 629
		protected readonly List<string> InvisibleRegions = new List<string>();
	}
}
