using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x02000013 RID: 19
	public class TransitionToErrorState : BaseState
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00003AAA File Offset: 0x00001CAA
		public TransitionToErrorState(BaseErrorState errorState, Exception exception)
		{
			this.exception = exception;
			base.AddErrorTransition(new ErrorTransition(errorState), exception);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003ACA File Offset: 0x00001CCA
		public override void Start()
		{
			this.RaiseStateErrored(new Error(this.exception));
		}

		// Token: 0x04000026 RID: 38
		private readonly Exception exception;
	}
}
