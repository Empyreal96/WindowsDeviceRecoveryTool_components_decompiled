using System;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000011 RID: 17
	internal abstract class BlobReadStreamBase : Stream
	{
		// Token: 0x06000143 RID: 323 RVA: 0x0000614C File Offset: 0x0000434C
		protected BlobReadStreamBase(CloudBlob blob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			if (options.UseTransactionalMD5.Value)
			{
				CommonUtility.AssertInBounds<int>("StreamMinimumReadSizeInBytes", blob.StreamMinimumReadSizeInBytes, 1, 4194304);
			}
			this.blob = blob;
			this.blobProperties = new BlobProperties(blob.Properties);
			this.currentOffset = 0L;
			this.streamMinimumReadSizeInBytes = this.blob.StreamMinimumReadSizeInBytes;
			this.internalBuffer = new MultiBufferMemoryStream(blob.ServiceClient.BufferManager, 65536);
			this.accessCondition = accessCondition;
			this.options = options;
			this.operationContext = operationContext;
			this.blobMD5 = ((this.options.DisableContentMD5Validation.Value || string.IsNullOrEmpty(this.blobProperties.ContentMD5)) ? null : new MD5Wrapper());
			this.lastException = null;
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00006225 File Offset: 0x00004425
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00006228 File Offset: 0x00004428
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000622B File Offset: 0x0000442B
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000622E File Offset: 0x0000442E
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00006236 File Offset: 0x00004436
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

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006241 File Offset: 0x00004441
		public override long Length
		{
			get
			{
				return this.blobProperties.Length;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006250 File Offset: 0x00004450
		public override long Seek(long offset, SeekOrigin origin)
		{
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
			if (num != this.currentOffset)
			{
				long num2 = this.internalBuffer.Position + (num - this.currentOffset);
				if (num2 >= 0L && num2 < this.internalBuffer.Length)
				{
					this.internalBuffer.Position = num2;
				}
				else
				{
					this.internalBuffer.SetLength(0L);
				}
				this.blobMD5 = null;
				this.currentOffset = num;
			}
			return this.currentOffset;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000632E File Offset: 0x0000452E
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006335 File Offset: 0x00004535
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000633C File Offset: 0x0000453C
		public override void Flush()
		{
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006340 File Offset: 0x00004540
		protected int ConsumeBuffer(byte[] buffer, int offset, int count)
		{
			int num = this.internalBuffer.Read(buffer, offset, count);
			this.currentOffset += (long)num;
			this.VerifyBlobMD5(buffer, offset, num);
			return num;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006375 File Offset: 0x00004575
		protected int GetReadSize()
		{
			if (this.currentOffset < this.Length)
			{
				return (int)Math.Min((long)this.streamMinimumReadSizeInBytes, this.Length - this.currentOffset);
			}
			return 0;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000063A4 File Offset: 0x000045A4
		protected void VerifyBlobMD5(byte[] buffer, int offset, int count)
		{
			if (this.blobMD5 != null && this.lastException == null && count > 0)
			{
				this.blobMD5.UpdateHash(buffer, offset, count);
				if (this.currentOffset == this.Length && !string.IsNullOrEmpty(this.blobProperties.ContentMD5))
				{
					string text = this.blobMD5.ComputeHash();
					this.blobMD5.Dispose();
					this.blobMD5 = null;
					if (!text.Equals(this.blobProperties.ContentMD5))
					{
						this.lastException = new IOException(string.Format(CultureInfo.InvariantCulture, "Blob data corrupted (integrity check failed), Expected value is '{0}', retrieved '{1}'", new object[]
						{
							this.blobProperties.ContentMD5,
							text
						}));
					}
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006465 File Offset: 0x00004665
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.internalBuffer != null)
				{
					this.internalBuffer.Dispose();
					this.internalBuffer = null;
				}
				if (this.blobMD5 != null)
				{
					this.blobMD5.Dispose();
					this.blobMD5 = null;
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400007C RID: 124
		protected CloudBlob blob;

		// Token: 0x0400007D RID: 125
		protected BlobProperties blobProperties;

		// Token: 0x0400007E RID: 126
		protected long currentOffset;

		// Token: 0x0400007F RID: 127
		protected MultiBufferMemoryStream internalBuffer;

		// Token: 0x04000080 RID: 128
		protected int streamMinimumReadSizeInBytes;

		// Token: 0x04000081 RID: 129
		protected AccessCondition accessCondition;

		// Token: 0x04000082 RID: 130
		protected BlobRequestOptions options;

		// Token: 0x04000083 RID: 131
		protected OperationContext operationContext;

		// Token: 0x04000084 RID: 132
		protected MD5Wrapper blobMD5;

		// Token: 0x04000085 RID: 133
		protected volatile Exception lastException;
	}
}
