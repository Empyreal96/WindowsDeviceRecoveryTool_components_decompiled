using System;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000008 RID: 8
	public class ErrorTransition
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00003370 File Offset: 0x00001570
		public ErrorTransition(BaseErrorState next)
		{
			this.Next = next;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003384 File Offset: 0x00001584
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000339B File Offset: 0x0000159B
		public virtual BaseErrorState Next { get; private set; }
	}
}
