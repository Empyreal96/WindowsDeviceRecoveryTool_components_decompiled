using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions
{
	// Token: 0x02000017 RID: 23
	public class PropagateStateStatusTransition : BaseTransition
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00003B51 File Offset: 0x00001D51
		public PropagateStateStatusTransition(string statusKey) : base(new EndState(statusKey))
		{
			this.statusKey = statusKey;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003B6C File Offset: 0x00001D6C
		public override bool ConditionsAreMet(object sender, TransitionEventArgs eventArgs)
		{
			return eventArgs.Status == this.statusKey;
		}

		// Token: 0x04000027 RID: 39
		private readonly string statusKey;
	}
}
