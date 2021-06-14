using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x0200000E RID: 14
	public class ErrorEndState : BaseErrorState
	{
		// Token: 0x0600006E RID: 110 RVA: 0x0000369E File Offset: 0x0000189E
		public override void Start(Error error)
		{
			this.RaiseStateErrored(error);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000036A9 File Offset: 0x000018A9
		public override void Stop()
		{
		}
	}
}
