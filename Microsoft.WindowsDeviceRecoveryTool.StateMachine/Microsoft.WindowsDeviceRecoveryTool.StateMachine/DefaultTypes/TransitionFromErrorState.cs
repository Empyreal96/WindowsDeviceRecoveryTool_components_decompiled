using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x02000012 RID: 18
	public class TransitionFromErrorState : BaseErrorState
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00003A83 File Offset: 0x00001C83
		public TransitionFromErrorState(BaseState state)
		{
			base.DefaultTransition = new DefaultTransition(state);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003A9B File Offset: 0x00001C9B
		public override void Start(Error error)
		{
			this.RaiseStateFinished(TransitionEventArgs.Empty);
		}
	}
}
