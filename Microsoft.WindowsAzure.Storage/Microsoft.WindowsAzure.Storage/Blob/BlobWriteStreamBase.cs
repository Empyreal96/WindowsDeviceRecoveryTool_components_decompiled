using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000013 RID: 19
	internal abstract class BlobWriteStreamBase : CloudBlobStream
	{
		// Token: 0x06000159 RID: 345 RVA: 0x000067FC File Offset: 0x000049FC
		private BlobWriteStreamBase(CloudBlobClient serviceClient, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			this.internalBuffer = new MultiBufferMemoryStream(serviceClient.BufferManager, 65536);
			this.accessCondition = accessCondition;
			this.currentOffset = 0L;
			this.options = options;
			this.operationContext = operationContext;
			this.noPendingWritesEvent = new CounterEvent();
			this.blobMD5 = (this.options.StoreBlobContentMD5.Value ? new MD5Wrapper() : null);
			this.blockMD5 = (this.options.UseTransactionalMD5.Value ? new MD5Wrapper() : null);
			this.parallelOperationSemaphore = new AsyncSemaphore(options.ParallelOperationThreadCount.Value);
			this.lastException = null;
			this.committed = false;
			this.disposed = false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000068C8 File Offset: 0x00004AC8
		protected BlobWriteStreamBase(CloudBlockBlob blockBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : this(blockBlob.ServiceClient, accessCondition, options, operationContext)
		{
			this.blockBlob = blockBlob;
			this.Blob = this.blockBlob;
			this.blockList = new List<string>();
			this.blockIdPrefix = Guid.NewGuid().ToString("N") + "-";
			this.streamWriteSizeInBytes = blockBlob.StreamWriteSizeInBytes;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006934 File Offset: 0x00004B34
		protected BlobWriteStreamBase(CloudPageBlob pageBlob, long pageBlobSize, bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : this(pageBlob.ServiceClient, accessCondition, options, operationContext)
		{
			this.currentBlobOffset = 0L;
			this.pageBlob = pageBlob;
			this.Blob = this.pageBlob;
			this.pageBlobSize = pageBlobSize;
			this.streamWriteSizeInBytes = pageBlob.StreamWriteSizeInBytes;
			this.newPageBlob = createNew;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006988 File Offset: 0x00004B88
		protected BlobWriteStreamBase(CloudAppendBlob appendBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : this(appendBlob.ServiceClient, accessCondition, options, operationContext)
		{
			this.accessCondition = (this.accessCondition ?? new AccessCondition());
			this.currentBlobOffset = ((this.accessCondition.IfAppendPositionEqual != null) ? this.accessCondition.IfAppendPositionEqual.Value : appendBlob.Properties.Length);
			this.operationContext = (this.operationContext ?? new OperationContext());
			this.appendBlob = appendBlob;
			this.Blob = this.appendBlob;
			this.parallelOperationSemaphore = new AsyncSemaphore(1);
			this.streamWriteSizeInBytes = appendBlob.StreamWriteSizeInBytes;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00006A35 File Offset: 0x00004C35
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00006A3D File Offset: 0x00004C3D
		private protected CloudBlob Blob { protected get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00006A46 File Offset: 0x00004C46
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00006A49 File Offset: 0x00004C49
		public override bool CanSeek
		{
			get
			{
				return this.pageBlob != null;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006A57 File Offset: 0x00004C57
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00006A5A File Offset: 0x00004C5A
		public override long Length
		{
			get
			{
				if (this.pageBlob != null)
				{
					return this.pageBlobSize;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006A70 File Offset: 0x00004C70
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00006A78 File Offset: 0x00004C78
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

		// Token: 0x06000165 RID: 357 RVA: 0x00006A83 File Offset: 0x00004C83
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006A8C File Offset: 0x00004C8C
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
			if (num % 512L != 0L)
			{
				CommonUtility.ArgumentOutOfRange("offset", offset);
			}
			return num;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00006B35 File Offset: 0x00004D35
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00006B3C File Offset: 0x00004D3C
		protected string GetCurrentBlockId()
		{
			string str = this.blockList.Count.ToString("D6", CultureInfo.InvariantCulture);
			byte[] bytes = Encoding.UTF8.GetBytes(this.blockIdPrefix + str);
			return Convert.ToBase64String(bytes);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006B84 File Offset: 0x00004D84
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.blobMD5 != null)
				{
					this.blobMD5.Dispose();
					this.blobMD5 = null;
				}
				if (this.blockMD5 != null)
				{
					this.blockMD5.Dispose();
					this.blockMD5 = null;
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

		// Token: 0x04000087 RID: 135
		protected CloudBlockBlob blockBlob;

		// Token: 0x04000088 RID: 136
		protected CloudPageBlob pageBlob;

		// Token: 0x04000089 RID: 137
		protected CloudAppendBlob appendBlob;

		// Token: 0x0400008A RID: 138
		protected long pageBlobSize;

		// Token: 0x0400008B RID: 139
		protected bool newPageBlob;

		// Token: 0x0400008C RID: 140
		protected long currentOffset;

		// Token: 0x0400008D RID: 141
		protected long currentBlobOffset;

		// Token: 0x0400008E RID: 142
		protected int streamWriteSizeInBytes;

		// Token: 0x0400008F RID: 143
		protected MultiBufferMemoryStream internalBuffer;

		// Token: 0x04000090 RID: 144
		protected List<string> blockList;

		// Token: 0x04000091 RID: 145
		protected string blockIdPrefix;

		// Token: 0x04000092 RID: 146
		protected AccessCondition accessCondition;

		// Token: 0x04000093 RID: 147
		protected BlobRequestOptions options;

		// Token: 0x04000094 RID: 148
		protected OperationContext operationContext;

		// Token: 0x04000095 RID: 149
		protected CounterEvent noPendingWritesEvent;

		// Token: 0x04000096 RID: 150
		protected MD5Wrapper blobMD5;

		// Token: 0x04000097 RID: 151
		protected MD5Wrapper blockMD5;

		// Token: 0x04000098 RID: 152
		protected AsyncSemaphore parallelOperationSemaphore;

		// Token: 0x04000099 RID: 153
		protected volatile Exception lastException;

		// Token: 0x0400009A RID: 154
		protected volatile bool committed;

		// Token: 0x0400009B RID: 155
		protected bool disposed;
	}
}
