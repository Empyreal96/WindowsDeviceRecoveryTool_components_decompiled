using System;

namespace Nokia.Mira
{
	// Token: 0x02000017 RID: 23
	public sealed class DownloadSettings
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002AF7 File Offset: 0x00000CF7
		public DownloadSettings(int maxChunks, long chunkSize, bool resumeDownload, bool overwriteFile)
		{
			this.MaxChunks = maxChunks;
			this.ChunkSize = chunkSize;
			this.ResumeDownload = resumeDownload;
			this.OverwriteExistingFile = overwriteFile;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002B1C File Offset: 0x00000D1C
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002B24 File Offset: 0x00000D24
		public int MaxChunks { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002B2D File Offset: 0x00000D2D
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002B35 File Offset: 0x00000D35
		public long ChunkSize { get; private set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002B3E File Offset: 0x00000D3E
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002B46 File Offset: 0x00000D46
		public bool ResumeDownload { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002B4F File Offset: 0x00000D4F
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002B57 File Offset: 0x00000D57
		public bool OverwriteExistingFile { get; private set; }

		// Token: 0x04000027 RID: 39
		private const long DefaultChunkSize = 3145728L;

		// Token: 0x04000028 RID: 40
		public static readonly DownloadSettings Default = new DownloadSettings(5, 3145728L, true, true);
	}
}
