using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Nokia.Mira.Chunks;

namespace Nokia.Mira.Strategies
{
	// Token: 0x02000021 RID: 33
	internal sealed class EnvironmentSetup
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00002EB9 File Offset: 0x000010B9
		public EnvironmentSetup(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002EC8 File Offset: 0x000010C8
		public void PrepareForNewDownload()
		{
			this.EnsureEmptyTargetFile();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00002ED0 File Offset: 0x000010D0
		public void EnsureTargetDirectory()
		{
			string directoryName = Path.GetDirectoryName(this.fileName);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002F00 File Offset: 0x00001100
		public ReadOnlyCollection<ChunkInformation> PrepareForResumedDownload(long chunkSize, IChunkInformationReader reader)
		{
			ReadOnlyCollection<ChunkRaw> readOnlyCollection = reader.Read();
			if (readOnlyCollection.Count == 0 || !this.TargetFileExists())
			{
				this.EnsureEmptyTargetFile();
				return new List<ChunkInformation>().AsReadOnly();
			}
			IChunkInformationConverter chunkInformationConverter = new ChunkInformationConverter(readOnlyCollection);
			return chunkInformationConverter.ConvertToChunkSize(chunkSize);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002F43 File Offset: 0x00001143
		private bool TargetFileExists()
		{
			return File.Exists(this.fileName);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002F50 File Offset: 0x00001150
		private void EnsureEmptyTargetFile()
		{
			string directoryName = Path.GetDirectoryName(this.fileName);
			if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (File.Exists(this.fileName))
			{
				File.Delete(this.fileName);
			}
			File.Create(this.fileName).Dispose();
		}

		// Token: 0x0400003D RID: 61
		private readonly string fileName;
	}
}
