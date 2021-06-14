using System;

namespace ClickerUtilityLibrary
{
	// Token: 0x02000002 RID: 2
	public class BlUpdaterUpdateProgressEventParameters
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public double Progress { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public BlUpdaterUpdateProgressEventParameters.EventSubType UpdateType { get; set; }

		// Token: 0x0200003D RID: 61
		public enum EventSubType
		{
			// Token: 0x04000168 RID: 360
			BootLoader,
			// Token: 0x04000169 RID: 361
			BootLoaderUpdater
		}
	}
}
