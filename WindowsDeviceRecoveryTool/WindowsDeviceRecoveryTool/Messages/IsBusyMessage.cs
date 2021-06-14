using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006D RID: 109
	public class IsBusyMessage
	{
		// Token: 0x0600032B RID: 811 RVA: 0x000103C8 File Offset: 0x0000E5C8
		public IsBusyMessage(bool isBusy, string message = "")
		{
			this.IsBusy = isBusy;
			this.Message = message;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600032C RID: 812 RVA: 0x000103E4 File Offset: 0x0000E5E4
		// (set) Token: 0x0600032D RID: 813 RVA: 0x000103FB File Offset: 0x0000E5FB
		public bool IsBusy { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00010404 File Offset: 0x0000E604
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0001041B File Offset: 0x0000E61B
		public string Message { get; private set; }
	}
}
