using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon
{
	// Token: 0x02000008 RID: 8
	public sealed class File4
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000045 RID: 69 RVA: 0x000027D8 File Offset: 0x000009D8
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000027F0 File Offset: 0x000009F0
		public string RelativePath
		{
			get
			{
				return this.relativePath;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002808 File Offset: 0x00000A08
		public long FileSize
		{
			get
			{
				return this.fileSize;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002820 File Offset: 0x00000A20
		public string DownloadUrl
		{
			get
			{
				return this.downloadUrl;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002838 File Offset: 0x00000A38
		public string Checksum
		{
			get
			{
				return this.checksum;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002850 File Offset: 0x00000A50
		public int ChecksumType
		{
			get
			{
				return this.checksumType;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002868 File Offset: 0x00000A68
		public File4(string fileName, string relativePath, long fileSize, string downloadUrl, string checksum, int checksumType)
		{
			this.fileName = fileName;
			this.relativePath = relativePath;
			this.fileSize = fileSize;
			this.downloadUrl = downloadUrl;
			this.checksum = checksum;
			this.checksumType = checksumType;
		}

		// Token: 0x04000013 RID: 19
		private readonly string fileName;

		// Token: 0x04000014 RID: 20
		private readonly string relativePath;

		// Token: 0x04000015 RID: 21
		private readonly long fileSize;

		// Token: 0x04000016 RID: 22
		private readonly string downloadUrl;

		// Token: 0x04000017 RID: 23
		private readonly string checksum;

		// Token: 0x04000018 RID: 24
		private readonly int checksumType;
	}
}
