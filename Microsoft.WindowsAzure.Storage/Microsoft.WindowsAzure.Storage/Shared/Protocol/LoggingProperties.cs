using System;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000161 RID: 353
	public sealed class LoggingProperties
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600151E RID: 5406 RVA: 0x00050333 File Offset: 0x0004E533
		// (set) Token: 0x0600151F RID: 5407 RVA: 0x0005033B File Offset: 0x0004E53B
		public string Version { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x00050344 File Offset: 0x0004E544
		// (set) Token: 0x06001521 RID: 5409 RVA: 0x0005034C File Offset: 0x0004E54C
		public LoggingOperations LoggingOperations { get; set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001522 RID: 5410 RVA: 0x00050355 File Offset: 0x0004E555
		// (set) Token: 0x06001523 RID: 5411 RVA: 0x0005035D File Offset: 0x0004E55D
		public int? RetentionDays { get; set; }
	}
}
