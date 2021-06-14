using System;
using Microsoft.WindowsDeviceRecoveryTool.StateMachine.BaseTypes;

namespace Microsoft.WindowsDeviceRecoveryTool.StateMachine.DefaultTypes
{
	// Token: 0x0200000A RID: 10
	public class DefaultTransition : BaseTransition
	{
		// Token: 0x0600005A RID: 90 RVA: 0x000033F4 File Offset: 0x000015F4
		public DefaultTransition(BaseState state) : base(state)
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003400 File Offset: 0x00001600
		public override bool ConditionsAreMet(object sender, TransitionEventArgs eventArgs)
		{
			return true;
		}
	}
}
