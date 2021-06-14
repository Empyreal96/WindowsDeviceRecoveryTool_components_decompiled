using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006B RID: 107
	public class WaitMessage
	{
		// Token: 0x06000323 RID: 803 RVA: 0x00010338 File Offset: 0x0000E538
		public WaitMessage(int seconds)
		{
			this.WaitSeconds = seconds;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0001034C File Offset: 0x0000E54C
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00010363 File Offset: 0x0000E563
		public int WaitSeconds { get; private set; }
	}
}
