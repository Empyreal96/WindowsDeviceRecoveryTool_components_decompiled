using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006F RID: 111
	public class SwitchStateMessage
	{
		// Token: 0x06000333 RID: 819 RVA: 0x00010458 File Offset: 0x0000E658
		public SwitchStateMessage()
		{
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00010463 File Offset: 0x0000E663
		public SwitchStateMessage(string state)
		{
			this.State = state;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000335 RID: 821 RVA: 0x00010478 File Offset: 0x0000E678
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0001048F File Offset: 0x0000E68F
		public string State { get; set; }
	}
}
