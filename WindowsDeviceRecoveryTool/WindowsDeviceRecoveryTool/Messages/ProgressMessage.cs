using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Messages
{
	// Token: 0x02000065 RID: 101
	public class ProgressMessage
	{
		// Token: 0x06000300 RID: 768 RVA: 0x000100B0 File Offset: 0x0000E2B0
		public ProgressMessage(int progress, string message)
		{
			this.Progress = progress;
			this.Message = message;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000100CB File Offset: 0x0000E2CB
		public ProgressMessage(int progress, string message, long downloadedSize, long totalSize, double bytesPerSecond, long secondsLeft) : this(progress, message)
		{
			this.DownloadedSize = downloadedSize;
			this.TotalSize = totalSize;
			this.BytesPerSecond = bytesPerSecond;
			this.SecondsLeft = secondsLeft;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000302 RID: 770 RVA: 0x000100FC File Offset: 0x0000E2FC
		// (set) Token: 0x06000303 RID: 771 RVA: 0x00010113 File Offset: 0x0000E313
		public int Progress { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0001011C File Offset: 0x0000E31C
		// (set) Token: 0x06000305 RID: 773 RVA: 0x00010133 File Offset: 0x0000E333
		public string Message { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0001013C File Offset: 0x0000E33C
		// (set) Token: 0x06000307 RID: 775 RVA: 0x00010153 File Offset: 0x0000E353
		public long TotalSize { get; private set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0001015C File Offset: 0x0000E35C
		// (set) Token: 0x06000309 RID: 777 RVA: 0x00010173 File Offset: 0x0000E373
		public long DownloadedSize { get; private set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600030A RID: 778 RVA: 0x0001017C File Offset: 0x0000E37C
		// (set) Token: 0x0600030B RID: 779 RVA: 0x00010193 File Offset: 0x0000E393
		public long SecondsLeft { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0001019C File Offset: 0x0000E39C
		// (set) Token: 0x0600030D RID: 781 RVA: 0x000101B3 File Offset: 0x0000E3B3
		public double BytesPerSecond { get; private set; }
	}
}
