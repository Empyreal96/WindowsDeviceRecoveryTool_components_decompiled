using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000069 RID: 105
	public class SettingsPreviousStateMessage
	{
		// Token: 0x0600031B RID: 795 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public SettingsPreviousStateMessage(string previousState = null)
		{
			this.PreviousState = previousState;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600031C RID: 796 RVA: 0x000102BC File Offset: 0x0000E4BC
		// (set) Token: 0x0600031D RID: 797 RVA: 0x000102D3 File Offset: 0x0000E4D3
		public string PreviousState { get; private set; }
	}
}
