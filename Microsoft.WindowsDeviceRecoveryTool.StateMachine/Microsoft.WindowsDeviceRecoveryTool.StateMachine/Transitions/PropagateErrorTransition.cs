using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.Transitions
{
	// Token: 0x02000016 RID: 22
	public class PropagateErrorTransition : ErrorTransition
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00003B41 File Offset: 0x00001D41
		public PropagateErrorTransition() : base(new ErrorEndState())
		{
		}
	}
}
