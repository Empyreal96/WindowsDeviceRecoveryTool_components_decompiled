using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200000F RID: 15
	internal sealed class BlobEncryptedWriteStream : CloudBlobStream
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00005A04 File Offset: 0x00003C04
		internal BlobEncryptedWriteStream(CloudBlockBlob blockBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, ICryptoTransform transform)
		{
			CommonUtility.AssertNotNull("transform", transform);
			if (options.EncryptionPolicy.EncryptionMode != BlobEncryptionMode.FullBlob)
			{
				throw new InvalidOperationException("Invalid BlobEncryptionMode set on the policy. Please set it to FullBlob when the policy is used with UploadFromStream.", null);
			}
			options.SkipEncryptionPolicyValidation = true;
			this.transform = transform;
			this.writeStream = new BlobWriteStream(blockBlob, accessCondition, options, operationContext)
			{
				IgnoreFlush = true
			};
			this.cryptoStream = new CryptoStream(this.writeStream, transform, CryptoStreamMode.Write);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005A7C File Offset: 0x00003C7C
		internal BlobEncryptedWriteStream(CloudPageBlob pageBlob, long pageBlobSize, bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, ICryptoTransform transform)
		{
			CommonUtility.AssertNotNull("transform", transform);
			if (options.EncryptionPolicy.EncryptionMode != BlobEncryptionMode.FullBlob)
			{
				throw new InvalidOperationException("Invalid BlobEncryptionMode set on the policy. Please set it to FullBlob when the policy is used with UploadFromStream.", null);
			}
			options.SkipEncryptionPolicyValidation = true;
			this.transform = transform;
			this.writeStream = new BlobWriteStream(pageBlob, pageBlobSize, createNew, accessCondition, options, operationContext)
			{
				IgnoreFlush = true
			};
			this.cryptoStream = new CryptoStream(this.writeStream, transform, CryptoStreamMode.Write);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005AF8 File Offset: 0x00003CF8
		internal BlobEncryptedWriteStream(CloudAppendBlob appendBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, ICryptoTransform transform)
		{
			CommonUtility.AssertNotNull("transform", transform);
			if (options.EncryptionPolicy.EncryptionMode != BlobEncryptionMode.FullBlob)
			{
				throw new InvalidOperationException("Invalid BlobEncryptionMode set on the policy. Please set it to FullBlob when the policy is used with UploadFromStream.", null);
			}
			options.SkipEncryptionPolicyValidation = true;
			this.transform = transform;
			this.writeStream = new BlobWriteStream(appendBlob, accessCondition, options, operationContext)
			{
				IgnoreFlush = true
			};
			this.cryptoStream = new CryptoStream(this.writeStream, transform, CryptoStreamMode.Write);
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005B6D File Offset: 0x00003D6D
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005B70 File Offset: 0x00003D70
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005B73 File Offset: 0x00003D73
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005B76 File Offset: 0x00003D76
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00005B7D File Offset: 0x00003D7D
		// (set) Token: 0x0600012B RID: 299 RVA: 0x00005B84 File Offset: 0x00003D84
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005B8B File Offset: 0x00003D8B
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005B92 File Offset: 0x00003D92
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005B99 File Offset: 0x00003D99
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005BA0 File Offset: 0x00003DA0
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EndWrite(this.BeginWrite(buffer, offset, count, null, null));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005BB3 File Offset: 0x00003DB3
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			return this.cryptoStream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005BF2 File Offset: 0x00003DF2
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.cryptoStream.EndWrite(asyncResult);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005C00 File Offset: 0x00003E00
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005C07 File Offset: 0x00003E07
		public override ICancellableAsyncResult BeginFlush(AsyncCallback callback, object state)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005C0E File Offset: 0x00003E0E
		public override void EndFlush(IAsyncResult asyncResult)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005C15 File Offset: 0x00003E15
		public override void Commit()
		{
			this.cryptoStream.FlushFinalBlock();
			this.writeStream.IgnoreFlush = false;
			this.writeStream.Commit();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005C39 File Offset: 0x00003E39
		public override ICancellableAsyncResult BeginCommit(AsyncCallback callback, object state)
		{
			this.cryptoStream.FlushFinalBlock();
			this.writeStream.IgnoreFlush = false;
			return this.writeStream.BeginCommit(callback, state);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005C5F File Offset: 0x00003E5F
		public override void EndCommit(IAsyncResult asyncResult)
		{
			this.writeStream.EndCommit(asyncResult);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005C6D File Offset: 0x00003E6D
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (disposing)
				{
					this.writeStream.IgnoreFlush = false;
					this.cryptoStream.Dispose();
					if (this.transform != null)
					{
						this.transform.Dispose();
					}
				}
			}
		}

		// Token: 0x04000075 RID: 117
		private bool disposed;

		// Token: 0x04000076 RID: 118
		private BlobWriteStream writeStream;

		// Token: 0x04000077 RID: 119
		private CryptoStream cryptoStream;

		// Token: 0x04000078 RID: 120
		private ICryptoTransform transform;
	}
}
