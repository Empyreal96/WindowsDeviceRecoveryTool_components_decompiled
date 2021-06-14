using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000A8 RID: 168
	public sealed class BlobContainerProperties
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600106B RID: 4203 RVA: 0x0003E496 File Offset: 0x0003C696
		// (set) Token: 0x0600106C RID: 4204 RVA: 0x0003E49E File Offset: 0x0003C69E
		public string ETag { get; internal set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600106D RID: 4205 RVA: 0x0003E4A7 File Offset: 0x0003C6A7
		// (set) Token: 0x0600106E RID: 4206 RVA: 0x0003E4AF File Offset: 0x0003C6AF
		public DateTimeOffset? LastModified { get; internal set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x0003E4B8 File Offset: 0x0003C6B8
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x0003E4C0 File Offset: 0x0003C6C0
		public LeaseStatus LeaseStatus { get; internal set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x0003E4C9 File Offset: 0x0003C6C9
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x0003E4D1 File Offset: 0x0003C6D1
		public LeaseState LeaseState { get; internal set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x0003E4DA File Offset: 0x0003C6DA
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x0003E4E2 File Offset: 0x0003C6E2
		public LeaseDuration LeaseDuration { get; internal set; }
	}
}
