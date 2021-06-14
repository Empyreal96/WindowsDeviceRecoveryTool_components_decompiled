using System;
using System.IO;

namespace Nokia.Mira.IO
{
	// Token: 0x0200000D RID: 13
	internal class CoreSynchronizedFileStream : IDisposable
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002736 File Offset: 0x00000936
		public CoreSynchronizedFileStream(string path) : this(new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Read, 1, FileOptions.WriteThrough))
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000274D File Offset: 0x0000094D
		private CoreSynchronizedFileStream(FileStream stream)
		{
			this.stream = stream;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002767 File Offset: 0x00000967
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.stream.Flush(true);
			this.stream.Dispose();
			this.disposed = true;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002790 File Offset: 0x00000990
		public void Flush()
		{
			lock (this.syncRoot)
			{
				this.stream.Flush(true);
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000027D8 File Offset: 0x000009D8
		public void Write(byte[] buffer, int offset, int count, long filePosition)
		{
			lock (this.syncRoot)
			{
				if (filePosition != this.currentPosition)
				{
					this.stream.Seek(filePosition, SeekOrigin.Begin);
				}
				this.stream.Write(buffer, offset, count);
				this.currentPosition = filePosition + (long)count;
			}
		}

		// Token: 0x04000015 RID: 21
		private readonly FileStream stream;

		// Token: 0x04000016 RID: 22
		private readonly object syncRoot = new object();

		// Token: 0x04000017 RID: 23
		private bool disposed;

		// Token: 0x04000018 RID: 24
		private long currentPosition;
	}
}
