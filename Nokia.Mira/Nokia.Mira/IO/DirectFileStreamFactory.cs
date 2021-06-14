using System;
using System.IO;

namespace Nokia.Mira.IO
{
	// Token: 0x0200000F RID: 15
	internal class DirectFileStreamFactory : IFileStreamFactory
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002844 File Offset: 0x00000A44
		public DirectFileStreamFactory(string targetPath)
		{
			this.targetPath = targetPath;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002853 File Offset: 0x00000A53
		public IFileStream Create(long initialPosition)
		{
			return new DirectFileStreamFactory.FileStreamAdapter(this.targetPath, initialPosition);
		}

		// Token: 0x04000019 RID: 25
		private readonly string targetPath;

		// Token: 0x02000011 RID: 17
		private class FileStreamAdapter : IFileStream, IDisposable
		{
			// Token: 0x06000034 RID: 52 RVA: 0x00002861 File Offset: 0x00000A61
			public FileStreamAdapter(string targetPath, long initialPosition)
			{
				this.fileStream = new FileStream(targetPath, FileMode.Open, FileAccess.Write, FileShare.Write, 1, FileOptions.WriteThrough);
				this.fileStream.Seek(initialPosition, SeekOrigin.Begin);
			}

			// Token: 0x06000035 RID: 53 RVA: 0x0000288C File Offset: 0x00000A8C
			public void Dispose()
			{
				if (this.disposed)
				{
					return;
				}
				this.fileStream.Dispose();
				this.disposed = true;
			}

			// Token: 0x06000036 RID: 54 RVA: 0x000028A9 File Offset: 0x00000AA9
			public void Flush()
			{
				this.fileStream.Flush(true);
			}

			// Token: 0x06000037 RID: 55 RVA: 0x000028B7 File Offset: 0x00000AB7
			public void Write(byte[] buffer, int offset, int count)
			{
				this.fileStream.Write(buffer, offset, count);
			}

			// Token: 0x0400001A RID: 26
			private readonly FileStream fileStream;

			// Token: 0x0400001B RID: 27
			private bool disposed;
		}
	}
}
