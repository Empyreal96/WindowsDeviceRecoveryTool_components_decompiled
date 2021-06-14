using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model
{
	// Token: 0x0200001E RID: 30
	public class DownloadedFile
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00003828 File Offset: 0x00001A28
		public DownloadedFile(string fileName, long fileSize)
		{
			this.FileName = fileName;
			this.TotalSize = fileSize;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00003844 File Offset: 0x00001A44
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000385B File Offset: 0x00001A5B
		public string FileName { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00003864 File Offset: 0x00001A64
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000387B File Offset: 0x00001A7B
		public long TotalSize { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00003884 File Offset: 0x00001A84
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000389B File Offset: 0x00001A9B
		public long TotalDownloaded { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000038A4 File Offset: 0x00001AA4
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000038BB File Offset: 0x00001ABB
		public long? PreviouslyDownloaded { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000038C4 File Offset: 0x00001AC4
		// (set) Token: 0x060000EC RID: 236 RVA: 0x000038DB File Offset: 0x00001ADB
		public long CurrentlyDownloaded { get; set; }
	}
}
