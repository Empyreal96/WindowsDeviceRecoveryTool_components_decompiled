using System;

namespace Nokia.Mira.IO
{
	// Token: 0x02000012 RID: 18
	internal class SynchronizedFileStream : IFileStream, IDisposable
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000028C7 File Offset: 0x00000AC7
		public SynchronizedFileStream(CoreSynchronizedFileStream coreStream, long initialPosition, Action onDispose)
		{
			if (coreStream == null)
			{
				throw new ArgumentNullException("coreStream");
			}
			if (onDispose == null)
			{
				throw new ArgumentNullException("onDispose");
			}
			this.coreStream = coreStream;
			this.initialPosition = initialPosition;
			this.onDispose = onDispose;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002900 File Offset: 0x00000B00
		public void Flush()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(null);
			}
			this.coreStream.Flush();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000291C File Offset: 0x00000B1C
		public void Write(byte[] buffer, int offset, int count)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(null);
			}
			this.coreStream.Write(buffer, offset, count, this.initialPosition + this.totalWritten);
			this.totalWritten += (long)count;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002957 File Offset: 0x00000B57
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.onDispose();
			this.disposed = true;
		}

		// Token: 0x0400001C RID: 28
		private readonly CoreSynchronizedFileStream coreStream;

		// Token: 0x0400001D RID: 29
		private readonly long initialPosition;

		// Token: 0x0400001E RID: 30
		private readonly Action onDispose;

		// Token: 0x0400001F RID: 31
		private long totalWritten;

		// Token: 0x04000020 RID: 32
		private bool disposed;
	}
}
