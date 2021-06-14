using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x02000030 RID: 48
	public class DownloadingProgressChangedEventArgs
	{
		// Token: 0x0600014B RID: 331 RVA: 0x000048A4 File Offset: 0x00002AA4
		public DownloadingProgressChangedEventArgs(int percentage, long downloadedSize, long totalSize, double bytesPerSecond, long secondsLeft, string message = null)
		{
			this.Percentage = percentage;
			this.Message = message;
			this.DownloadedSize = downloadedSize;
			this.TotalSize = totalSize;
			this.BytesPerSecond = bytesPerSecond;
			this.SecondsLeft = secondsLeft;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000048E4 File Offset: 0x00002AE4
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000048FB File Offset: 0x00002AFB
		public int Percentage { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00004904 File Offset: 0x00002B04
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000491B File Offset: 0x00002B1B
		public string Message { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00004924 File Offset: 0x00002B24
		// (set) Token: 0x06000151 RID: 337 RVA: 0x0000493B File Offset: 0x00002B3B
		public long TotalSize { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00004944 File Offset: 0x00002B44
		// (set) Token: 0x06000153 RID: 339 RVA: 0x0000495B File Offset: 0x00002B5B
		public long DownloadedSize { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00004964 File Offset: 0x00002B64
		// (set) Token: 0x06000155 RID: 341 RVA: 0x0000497B File Offset: 0x00002B7B
		public long SecondsLeft { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00004984 File Offset: 0x00002B84
		// (set) Token: 0x06000157 RID: 343 RVA: 0x0000499B File Offset: 0x00002B9B
		public double BytesPerSecond { get; private set; }
	}
}
