using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x0200000F RID: 15
	public class LambdaTransition : BaseTransition
	{
		// Token: 0x06000071 RID: 113 RVA: 0x000036B4 File Offset: 0x000018B4
		public LambdaTransition(Func<bool> predicate, BaseState state) : base(state)
		{
			this.predicate = predicate;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000036C8 File Offset: 0x000018C8
		public override bool ConditionsAreMet(object sender, TransitionEventArgs eventArgs)
		{
			return this.predicate();
		}

		// Token: 0x04000023 RID: 35
		private readonly Func<bool> predicate;
	}
}
