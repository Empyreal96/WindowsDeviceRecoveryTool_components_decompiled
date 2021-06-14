using System;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000102 RID: 258
	public class QueueMessage
	{
		// Token: 0x06001289 RID: 4745 RVA: 0x00044F07 File Offset: 0x00043107
		internal QueueMessage()
		{
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600128A RID: 4746 RVA: 0x00044F0F File Offset: 0x0004310F
		// (set) Token: 0x0600128B RID: 4747 RVA: 0x00044F17 File Offset: 0x00043117
		public DateTimeOffset? ExpirationTime { get; internal set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600128C RID: 4748 RVA: 0x00044F20 File Offset: 0x00043120
		// (set) Token: 0x0600128D RID: 4749 RVA: 0x00044F28 File Offset: 0x00043128
		public string Id { get; internal set; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00044F31 File Offset: 0x00043131
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x00044F39 File Offset: 0x00043139
		public DateTimeOffset? InsertionTime { get; internal set; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00044F42 File Offset: 0x00043142
		// (set) Token: 0x06001291 RID: 4753 RVA: 0x00044F4A File Offset: 0x0004314A
		public DateTimeOffset? NextVisibleTime { get; internal set; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00044F53 File Offset: 0x00043153
		// (set) Token: 0x06001293 RID: 4755 RVA: 0x00044F5B File Offset: 0x0004315B
		public string PopReceipt { get; internal set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x00044F64 File Offset: 0x00043164
		// (set) Token: 0x06001295 RID: 4757 RVA: 0x00044F6C File Offset: 0x0004316C
		public string Text { get; internal set; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001296 RID: 4758 RVA: 0x00044F75 File Offset: 0x00043175
		// (set) Token: 0x06001297 RID: 4759 RVA: 0x00044F7D File Offset: 0x0004317D
		public int DequeueCount { get; internal set; }
	}
}
