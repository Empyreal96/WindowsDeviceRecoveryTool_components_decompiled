using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x02000050 RID: 80
	public class ServiceBreak
	{
		// Token: 0x0600027C RID: 636 RVA: 0x00007A50 File Offset: 0x00005C50
		public ServiceBreak(DateTime start, DateTime end)
		{
			this.Start = start;
			this.End = end;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00007A6C File Offset: 0x00005C6C
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00007A83 File Offset: 0x00005C83
		public DateTime Start { get; private set; }

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600027F RID: 639 RVA: 0x00007A8C File Offset: 0x00005C8C
		// (set) Token: 0x06000280 RID: 640 RVA: 0x00007AA3 File Offset: 0x00005CA3
		public DateTime End { get; private set; }
	}
}
