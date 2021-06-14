using System;

namespace FFUComponents
{
	// Token: 0x02000059 RID: 89
	internal class DTSFUsbStreamWriteAsyncResult : AsyncResultNoResult
	{
		// Token: 0x06000194 RID: 404 RVA: 0x000084E6 File Offset: 0x000066E6
		public DTSFUsbStreamWriteAsyncResult(AsyncCallback callback, object state) : base(callback, state)
		{
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000084F0 File Offset: 0x000066F0
		// (set) Token: 0x06000196 RID: 406 RVA: 0x000084F8 File Offset: 0x000066F8
		public byte[] Buffer { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008501 File Offset: 0x00006701
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00008509 File Offset: 0x00006709
		public int Offset { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00008512 File Offset: 0x00006712
		// (set) Token: 0x0600019A RID: 410 RVA: 0x0000851A File Offset: 0x0000671A
		public int Count { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600019B RID: 411 RVA: 0x00008523 File Offset: 0x00006723
		// (set) Token: 0x0600019C RID: 412 RVA: 0x0000852B File Offset: 0x0000672B
		public int RetryCount { get; set; }
	}
}
