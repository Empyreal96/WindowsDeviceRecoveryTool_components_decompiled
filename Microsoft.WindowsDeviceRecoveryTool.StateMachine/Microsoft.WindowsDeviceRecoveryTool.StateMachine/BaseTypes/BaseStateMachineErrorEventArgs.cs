using System;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000005 RID: 5
	public class BaseStateMachineErrorEventArgs : EventArgs
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00003291 File Offset: 0x00001491
		public BaseStateMachineErrorEventArgs(Error error)
		{
			this.Error = error;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000048 RID: 72 RVA: 0x000032A4 File Offset: 0x000014A4
		// (set) Token: 0x06000049 RID: 73 RVA: 0x000032BB File Offset: 0x000014BB
		public Error Error { get; private set; }
	}
}
