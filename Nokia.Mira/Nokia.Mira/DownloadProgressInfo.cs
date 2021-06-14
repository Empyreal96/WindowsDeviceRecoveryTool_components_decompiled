using System;

namespace Nokia.Mira
{
	// Token: 0x02000023 RID: 35
	public sealed class DownloadProgressInfo
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00003308 File Offset: 0x00001508
		public DownloadProgressInfo(long bytesReceived) : this(bytesReceived, null)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003325 File Offset: 0x00001525
		public DownloadProgressInfo(long bytesReceived, long? totalBytes)
		{
			this.bytesReceived = bytesReceived;
			this.totalBytes = totalBytes;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000333B File Offset: 0x0000153B
		public long BytesReceived
		{
			get
			{
				return this.bytesReceived;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00003343 File Offset: 0x00001543
		public long? TotalBytes
		{
			get
			{
				return this.totalBytes;
			}
		}

		// Token: 0x04000044 RID: 68
		private readonly long bytesReceived;

		// Token: 0x04000045 RID: 69
		private readonly long? totalBytes;
	}
}
