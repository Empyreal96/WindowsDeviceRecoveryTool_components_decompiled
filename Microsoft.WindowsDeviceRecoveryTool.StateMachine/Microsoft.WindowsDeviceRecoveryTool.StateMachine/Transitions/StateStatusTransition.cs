using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions
{
	// Token: 0x02000018 RID: 24
	public class StateStatusTransition : BaseTransition
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00003B8F File Offset: 0x00001D8F
		public StateStatusTransition(BaseState next, string statusKey) : base(next)
		{
			this.statusKey = statusKey;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public override bool ConditionsAreMet(object sender, TransitionEventArgs eventArgs)
		{
			return eventArgs.Status == this.statusKey;
		}

		// Token: 0x04000028 RID: 40
		private readonly string statusKey;
	}
}
