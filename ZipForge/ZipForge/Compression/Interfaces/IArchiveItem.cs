using System;
using System.IO;

namespace ComponentAce.Compression.Interfaces
{
	// Token: 0x0200001D RID: 29
	public interface IArchiveItem
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000128 RID: 296
		string FullName { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000129 RID: 297
		// (set) Token: 0x0600012A RID: 298
		string SrcFileName { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600012B RID: 299
		// (set) Token: 0x0600012C RID: 300
		string FileName { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600012D RID: 301
		// (set) Token: 0x0600012E RID: 302
		string StoredPath { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600012F RID: 303
		long UncompressedSize { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000130 RID: 304
		// (set) Token: 0x06000131 RID: 305
		FileAttributes ExternalFileAttributes { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000132 RID: 306
		// (set) Token: 0x06000133 RID: 307
		DateTime FileModificationDateTime { get; set; }

		// Token: 0x06000134 RID: 308
		void Reset();
	}
}
