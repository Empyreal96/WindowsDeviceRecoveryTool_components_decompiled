using System;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes
{
	// Token: 0x02000006 RID: 6
	public abstract class BaseTransition
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000032C4 File Offset: 0x000014C4
		protected BaseTransition(BaseState next)
		{
			this.Next = next;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000032D8 File Offset: 0x000014D8
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000032EF File Offset: 0x000014EF
		public virtual BaseState Next { get; protected set; }

		// Token: 0x0600004D RID: 77 RVA: 0x000032F8 File Offset: 0x000014F8
		public override string ToString()
		{
			string text = base.ToString();
			return text.Substring(text.LastIndexOf('.') + 1);
		}

		// Token: 0x0600004E RID: 78
		public abstract bool ConditionsAreMet(object sender, TransitionEventArgs eventArgs);
	}
}
