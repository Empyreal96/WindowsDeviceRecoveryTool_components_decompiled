using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x0200002B RID: 43
	internal abstract class FileWriteStreamBase : CloudFileStream
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x000210F4 File Offset: 0x0001F2F4
		protected FileWriteStreamBase(CloudFile file, long fileSize, bool createNew, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			this.internalBuffer = new MultiBufferMemoryStream(file.ServiceClient.BufferManager, 65536);
			this.currentOffset = 0L;
			this.accessCondition = accessCondition;
			this.options = options;
			this.operationContext = operationContext;
			this.noPendingWritesEvent = new CounterEvent();
			this.fileMD5 = (this.options.StoreFileContentMD5.Value ? new MD5Wrapper() : null);
			this.rangeMD5 = (this.options.UseTransactionalMD5.Value ? new MD5Wrapper() : null);
			this.parallelOperationSemaphore = new AsyncSemaphore(options.ParallelOperationThreadCount.Value);
			this.lastException = null;
			this.committed = false;
			this.disposed = false;
			this.currentFileOffset = 0L;
			this.file = file;
			this.fileSize = fileSize;
			this.streamWriteSizeInBytes = file.StreamWriteSizeInBytes;
			this.newFile = createNew;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x000211EF File Offset: 0x0001F3EF
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x000211F2 File Offset: 0x0001F3F2
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x000211F5 File Offset: 0x0001F3F5
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x000211F8 File Offset: 0x0001F3F8
		public override long Length
		{
			get
			{
				return this.fileSize;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00021200 File Offset: 0x0001F400
		// (set) Token: 0x0600091D RID: 2333 RVA: 0x00021208 File Offset: 0x0001F408
		public override long Position
		{
			get
			{
				return this.currentOffset;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00021213 File Offset: 0x0001F413
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002121C File Offset: 0x0001F41C
		protected long GetNewOffset(long offset, SeekOrigin origin)
		{
			if (!this.CanSeek)
			{
				throw new NotSupportedException();
			}
			if (this.lastException != null)
			{
				throw this.lastException;
			}
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this.currentOffset + offset;
				break;
			case SeekOrigin.End:
				num = this.Length + offset;
				break;
			default:
				CommonUtility.ArgumentOutOfRange("origin", origin);
				throw new ArgumentOutOfRangeException("origin");
			}
			CommonUtility.AssertInBounds<long>("offset", num, 0L, this.Length);
			return num;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x000212A9 File Offset: 0x0001F4A9
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000212B0 File Offset: 0x0001F4B0
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.fileMD5 != null)
				{
					this.fileMD5.Dispose();
					this.fileMD5 = null;
				}
				if (this.rangeMD5 != null)
				{
					this.rangeMD5.Dispose();
					this.rangeMD5 = null;
				}
				if (this.internalBuffer != null)
				{
					this.internalBuffer.Dispose();
					this.internalBuffer = null;
				}
				if (this.noPendingWritesEvent != null)
				{
					this.noPendingWritesEvent.Dispose();
					this.noPendingWritesEvent = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000102 RID: 258
		protected CloudFile file;

		// Token: 0x04000103 RID: 259
		protected long fileSize;

		// Token: 0x04000104 RID: 260
		protected bool newFile;

		// Token: 0x04000105 RID: 261
		protected long currentOffset;

		// Token: 0x04000106 RID: 262
		protected long currentFileOffset;

		// Token: 0x04000107 RID: 263
		protected int streamWriteSizeInBytes;

		// Token: 0x04000108 RID: 264
		protected MultiBufferMemoryStream internalBuffer;

		// Token: 0x04000109 RID: 265
		protected AccessCondition accessCondition;

		// Token: 0x0400010A RID: 266
		protected FileRequestOptions options;

		// Token: 0x0400010B RID: 267
		protected OperationContext operationContext;

		// Token: 0x0400010C RID: 268
		protected CounterEvent noPendingWritesEvent;

		// Token: 0x0400010D RID: 269
		protected MD5Wrapper fileMD5;

		// Token: 0x0400010E RID: 270
		protected MD5Wrapper rangeMD5;

		// Token: 0x0400010F RID: 271
		protected AsyncSemaphore parallelOperationSemaphore;

		// Token: 0x04000110 RID: 272
		protected volatile Exception lastException;

		// Token: 0x04000111 RID: 273
		protected volatile bool committed;

		// Token: 0x04000112 RID: 274
		protected bool disposed;
	}
}
