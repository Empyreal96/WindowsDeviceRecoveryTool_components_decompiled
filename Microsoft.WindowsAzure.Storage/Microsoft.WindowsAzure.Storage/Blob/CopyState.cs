using System;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000B6 RID: 182
	public sealed class CopyState
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x0003F021 File Offset: 0x0003D221
		// (set) Token: 0x060010D2 RID: 4306 RVA: 0x0003F029 File Offset: 0x0003D229
		public string CopyId { get; internal set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x0003F032 File Offset: 0x0003D232
		// (set) Token: 0x060010D4 RID: 4308 RVA: 0x0003F03A File Offset: 0x0003D23A
		public DateTimeOffset? CompletionTime { get; internal set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0003F043 File Offset: 0x0003D243
		// (set) Token: 0x060010D6 RID: 4310 RVA: 0x0003F04B File Offset: 0x0003D24B
		public CopyStatus Status { get; internal set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0003F054 File Offset: 0x0003D254
		// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0003F05C File Offset: 0x0003D25C
		public Uri Source { get; internal set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x0003F065 File Offset: 0x0003D265
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x0003F06D File Offset: 0x0003D26D
		public long? BytesCopied { get; internal set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x0003F076 File Offset: 0x0003D276
		// (set) Token: 0x060010DC RID: 4316 RVA: 0x0003F07E File Offset: 0x0003D27E
		public long? TotalBytes { get; internal set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0003F087 File Offset: 0x0003D287
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x0003F08F File Offset: 0x0003D28F
		public string StatusDescription { get; internal set; }
	}
}
