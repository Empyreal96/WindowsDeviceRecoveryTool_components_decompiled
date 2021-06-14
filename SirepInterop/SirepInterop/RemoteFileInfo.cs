using System;
using System.IO;

namespace Interop.SirepClient
{
	// Token: 0x0200000C RID: 12
	public class RemoteFileInfo
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002854 File Offset: 0x00000A54
		public RemoteFileInfo()
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000285C File Offset: 0x00000A5C
		public RemoteFileInfo(ref FileInfox fileInfo)
		{
			this.FileAttributes = (FileAttributes)fileInfo.m_FileAttributes;
			this.FileSize = fileInfo.m_FileSize;
			this.CreationTime = DateTime.FromFileTime(fileInfo.m_CreationTime);
			this.LastAccessTime = DateTime.FromFileTime(fileInfo.m_LastAccessTime);
			this.LastWriteTime = DateTime.FromFileTime(fileInfo.m_LastWriteTime);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000028BA File Offset: 0x00000ABA
		// (set) Token: 0x0600007F RID: 127 RVA: 0x000028C2 File Offset: 0x00000AC2
		public FileAttributes FileAttributes { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000080 RID: 128 RVA: 0x000028CB File Offset: 0x00000ACB
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000028D3 File Offset: 0x00000AD3
		public long FileSize { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000082 RID: 130 RVA: 0x000028DC File Offset: 0x00000ADC
		// (set) Token: 0x06000083 RID: 131 RVA: 0x000028E4 File Offset: 0x00000AE4
		public DateTime CreationTime { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000028ED File Offset: 0x00000AED
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000028F5 File Offset: 0x00000AF5
		public DateTime LastAccessTime { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000086 RID: 134 RVA: 0x000028FE File Offset: 0x00000AFE
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002906 File Offset: 0x00000B06
		public DateTime LastWriteTime { get; set; }
	}
}
