using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions;

namespace Microsoft.WindowsDeviceRecoveryTool.States.BaseStates
{
	// Token: 0x02000072 RID: 114
	public class PreviousStateTransition : StateStatusTransition
	{
		// Token: 0x06000363 RID: 867 RVA: 0x00010993 File Offset: 0x0000EB93
		public PreviousStateTransition(BaseState next, string stateName) : base(next, stateName)
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000109A0 File Offset: 0x0000EBA0
		public void SetNextState(BaseState state)
		{
			this.Next = state;
		}
	}
}
