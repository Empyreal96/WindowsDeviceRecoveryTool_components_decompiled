using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x0200000C RID: 12
	public class EndState : BaseState
	{
		// Token: 0x06000063 RID: 99 RVA: 0x000035F0 File Offset: 0x000017F0
		public EndState()
		{
			this.Status = string.Empty;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003607 File Offset: 0x00001807
		public EndState(string status)
		{
			this.Status = status;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000361C File Offset: 0x0000181C
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003633 File Offset: 0x00001833
		public string Status { get; private set; }

		// Token: 0x06000067 RID: 103 RVA: 0x0000363C File Offset: 0x0000183C
		public override void Start()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000363F File Offset: 0x0000183F
		public override void Stop()
		{
		}
	}
}
