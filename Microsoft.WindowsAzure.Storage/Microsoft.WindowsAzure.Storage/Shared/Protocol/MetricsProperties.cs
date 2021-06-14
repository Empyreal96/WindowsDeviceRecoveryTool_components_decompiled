using System;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000163 RID: 355
	public sealed class MetricsProperties
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0005036E File Offset: 0x0004E56E
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x00050376 File Offset: 0x0004E576
		public string Version { get; set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0005037F File Offset: 0x0004E57F
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x00050387 File Offset: 0x0004E587
		public MetricsLevel MetricsLevel { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x00050390 File Offset: 0x0004E590
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x00050398 File Offset: 0x0004E598
		public int? RetentionDays { get; set; }
	}
}
