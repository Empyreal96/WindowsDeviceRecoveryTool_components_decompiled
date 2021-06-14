using System;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014D RID: 333
	public sealed class TableResult
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060014EC RID: 5356 RVA: 0x0004FC72 File Offset: 0x0004DE72
		// (set) Token: 0x060014ED RID: 5357 RVA: 0x0004FC7A File Offset: 0x0004DE7A
		public object Result { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x0004FC83 File Offset: 0x0004DE83
		// (set) Token: 0x060014EF RID: 5359 RVA: 0x0004FC8B File Offset: 0x0004DE8B
		public int HttpStatusCode { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x0004FC94 File Offset: 0x0004DE94
		// (set) Token: 0x060014F1 RID: 5361 RVA: 0x0004FC9C File Offset: 0x0004DE9C
		public string Etag { get; set; }
	}
}
