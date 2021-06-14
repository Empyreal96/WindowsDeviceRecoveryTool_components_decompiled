using System;

namespace Nokia.Mira.IO
{
	// Token: 0x02000013 RID: 19
	internal class SynchronizedFileStreamFactory : IFileStreamFactory
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002974 File Offset: 0x00000B74
		public SynchronizedFileStreamFactory(string targetPath)
		{
			this.targetPath = targetPath;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002990 File Offset: 0x00000B90
		public IFileStream Create(long initialPosition)
		{
			IFileStream result;
			lock (this.syncRoot)
			{
				if (this.coreStream == null)
				{
					this.coreStream = new CoreSynchronizedFileStream(this.targetPath);
				}
				this.referencesCount++;
				result = new SynchronizedFileStream(this.coreStream, initialPosition, new Action(this.FileStreamOnDisposed));
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002A0C File Offset: 0x00000C0C
		private void FileStreamOnDisposed()
		{
			lock (this.syncRoot)
			{
				this.referencesCount--;
				if (this.referencesCount == 0)
				{
					this.coreStream.Dispose();
					this.coreStream = null;
				}
			}
		}

		// Token: 0x04000021 RID: 33
		private readonly string targetPath;

		// Token: 0x04000022 RID: 34
		private readonly object syncRoot = new object();

		// Token: 0x04000023 RID: 35
		private CoreSynchronizedFileStream coreStream;

		// Token: 0x04000024 RID: 36
		private int referencesCount;
	}
}
