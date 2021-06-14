using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.EventArgs
{
	// Token: 0x02000031 RID: 49
	public class ProgressChangedEventArgs
	{
		// Token: 0x06000158 RID: 344 RVA: 0x000049A4 File Offset: 0x00002BA4
		public ProgressChangedEventArgs(int percentage, string message = null)
		{
			this.Percentage = percentage;
			this.Message = message;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000049BF File Offset: 0x00002BBF
		public ProgressChangedEventArgs(int percentage, string message, long downloadedSize, long totalSize, double bytesPerSecond, long secondsLeft) : this(percentage, message)
		{
			this.DownloadedSize = downloadedSize;
			this.TotalSize = totalSize;
			this.BytesPerSecond = bytesPerSecond;
			this.SecondsLeft = secondsLeft;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000049F0 File Offset: 0x00002BF0
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00004A07 File Offset: 0x00002C07
		public int Percentage { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00004A10 File Offset: 0x00002C10
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00004A27 File Offset: 0x00002C27
		public string Message { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00004A30 File Offset: 0x00002C30
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00004A47 File Offset: 0x00002C47
		public long TotalSize { get; private set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00004A50 File Offset: 0x00002C50
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00004A67 File Offset: 0x00002C67
		public long DownloadedSize { get; private set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004A70 File Offset: 0x00002C70
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00004A87 File Offset: 0x00002C87
		public long SecondsLeft { get; private set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00004A90 File Offset: 0x00002C90
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00004AA7 File Offset: 0x00002CA7
		public double BytesPerSecond { get; private set; }
	}
}
