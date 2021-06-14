using System;

namespace FFUComponents
{
	// Token: 0x02000058 RID: 88
	internal class DTSFUsbStreamReadAsyncResult : AsyncResult<int>
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00008498 File Offset: 0x00006698
		public DTSFUsbStreamReadAsyncResult(AsyncCallback callback, object state) : base(callback, state)
		{
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000084A2 File Offset: 0x000066A2
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000084AA File Offset: 0x000066AA
		public byte[] Buffer { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000084B3 File Offset: 0x000066B3
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000084BB File Offset: 0x000066BB
		public int Offset { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000084C4 File Offset: 0x000066C4
		// (set) Token: 0x06000191 RID: 401 RVA: 0x000084CC File Offset: 0x000066CC
		public int Count { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000084D5 File Offset: 0x000066D5
		// (set) Token: 0x06000193 RID: 403 RVA: 0x000084DD File Offset: 0x000066DD
		public int RetryCount { get; set; }
	}
}
