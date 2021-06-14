using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000064 RID: 100
	public class IsBackButtonMessage
	{
		// Token: 0x060002FD RID: 765 RVA: 0x0001007C File Offset: 0x0000E27C
		public IsBackButtonMessage(bool isBackButtton)
		{
			this.IsBackButton = isBackButtton;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00010090 File Offset: 0x0000E290
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000100A7 File Offset: 0x0000E2A7
		public bool IsBackButton { get; private set; }
	}
}
