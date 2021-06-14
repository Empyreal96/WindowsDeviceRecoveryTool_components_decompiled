using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x02000010 RID: 16
	public class StartState : BaseState
	{
		// Token: 0x06000073 RID: 115 RVA: 0x000036E5 File Offset: 0x000018E5
		public override void Start()
		{
			this.RaiseStateFinished(TransitionEventArgs.Empty);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000036F4 File Offset: 0x000018F4
		public override void Stop()
		{
		}
	}
}
