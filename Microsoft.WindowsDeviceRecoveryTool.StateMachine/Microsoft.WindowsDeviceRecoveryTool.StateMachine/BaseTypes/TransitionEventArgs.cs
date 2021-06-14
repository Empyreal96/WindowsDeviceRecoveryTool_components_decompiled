using System;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000009 RID: 9
	public class TransitionEventArgs : EventArgs
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000033A4 File Offset: 0x000015A4
		public TransitionEventArgs(string status)
		{
			this.Status = status;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000033B8 File Offset: 0x000015B8
		public new static TransitionEventArgs Empty
		{
			get
			{
				return new TransitionEventArgs(string.Empty);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000033D4 File Offset: 0x000015D4
		// (set) Token: 0x06000059 RID: 89 RVA: 0x000033EB File Offset: 0x000015EB
		public string Status { get; private set; }
	}
}
