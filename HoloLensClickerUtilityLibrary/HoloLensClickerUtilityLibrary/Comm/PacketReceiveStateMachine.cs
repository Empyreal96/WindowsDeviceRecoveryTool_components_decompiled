using System;

namespace ClickerUtilityLibrary.Comm
{
	// Token: 0x02000023 RID: 35
	internal enum PacketReceiveStateMachine
	{
		// Token: 0x040000DC RID: 220
		Idle,
		// Token: 0x040000DD RID: 221
		ParseHeader,
		// Token: 0x040000DE RID: 222
		ValidateBody
	}
}
