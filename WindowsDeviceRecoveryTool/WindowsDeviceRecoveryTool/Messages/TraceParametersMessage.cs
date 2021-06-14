using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x0200006C RID: 108
	public class TraceParametersMessage
	{
		// Token: 0x06000326 RID: 806 RVA: 0x0001036C File Offset: 0x0000E56C
		public TraceParametersMessage(string logZipFilePath = null, bool collectingLogsCompleted = false)
		{
			this.LogZipFilePath = logZipFilePath;
			this.CollectingLogsCompleted = collectingLogsCompleted;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00010388 File Offset: 0x0000E588
		// (set) Token: 0x06000328 RID: 808 RVA: 0x0001039F File Offset: 0x0000E59F
		public string LogZipFilePath { get; private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000329 RID: 809 RVA: 0x000103A8 File Offset: 0x0000E5A8
		// (set) Token: 0x0600032A RID: 810 RVA: 0x000103BF File Offset: 0x0000E5BF
		public bool CollectingLogsCompleted { get; private set; }
	}
}
