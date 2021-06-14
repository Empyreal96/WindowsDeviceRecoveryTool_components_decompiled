using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000054 RID: 84
	internal class BlobDecryptStream : Stream
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x000308CC File Offset: 0x0002EACC
		public BlobDecryptStream(Stream userStream, IDictionary<string, string> metadata, long? userProvidedLength, int discardFirst, bool bufferIV, bool noPadding, BlobEncryptionPolicy policy, bool? requireEncryption)
		{
			this.userStream = userStream;
			this.metadata = metadata;
			this.userProvidedLength = userProvidedLength;
			this.discardFirst = discardFirst;
			this.encryptionPolicy = policy;
			this.bufferIV = bufferIV;
			this.noPadding = noPadding;
			this.requireEncryption = requireEncryption;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00030929 File Offset: 0x0002EB29
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0003092C File Offset: 0x0002EB2C
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0003092F File Offset: 0x0002EB2F
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x00030932 File Offset: 0x0002EB32
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x00030939 File Offset: 0x0002EB39
		// (set) Token: 0x06000D19 RID: 3353 RVA: 0x00030941 File Offset: 0x0002EB41
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00030948 File Offset: 0x0002EB48
		public override void Flush()
		{
			this.userStream.Flush();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00030955 File Offset: 0x0002EB55
		public override void SetLength(long value)
		{
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00030957 File Offset: 0x0002EB57
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0003095E File Offset: 0x0002EB5E
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00030968 File Offset: 0x0002EB68
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.bufferIV && this.position < 16L)
			{
				int num = 16 - (int)this.position;
				num = ((count > num) ? num : count);
				Array.Copy(buffer, offset, this.iv, (int)this.position, num);
				this.position += (long)num;
				offset += num;
				count -= num;
			}
			if (this.cryptoStream == null)
			{
				LengthLimitingStream userProvidedStream = new LengthLimitingStream(this.userStream, (long)this.discardFirst, this.userProvidedLength);
				this.cryptoStream = this.encryptionPolicy.DecryptBlob(userProvidedStream, this.metadata, out this.transform, this.requireEncryption, (!this.bufferIV) ? null : this.iv, this.noPadding);
			}
			if (count > 0)
			{
				this.cryptoStream.Write(buffer, offset, count);
				this.position += (long)count;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00030A48 File Offset: 0x0002EC48
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.bufferIV && this.position < 16L)
			{
				int num = 16 - (int)this.position;
				num = ((count > num) ? num : count);
				Array.Copy(buffer, offset, this.iv, (int)this.position, num);
				this.position += (long)num;
				offset += num;
				count -= num;
			}
			if (this.cryptoStream == null)
			{
				LengthLimitingStream userProvidedStream = new LengthLimitingStream(this.userStream, (long)this.discardFirst, this.userProvidedLength);
				this.cryptoStream = this.encryptionPolicy.DecryptBlob(userProvidedStream, this.metadata, out this.transform, this.requireEncryption, (!this.bufferIV) ? null : this.iv, this.noPadding);
			}
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			if (count <= 0)
			{
				storageAsyncResult.OnComplete();
			}
			else
			{
				storageAsyncResult.OperationState = count;
				this.cryptoStream.BeginWrite(buffer, offset, count, new AsyncCallback(this.WriteStreamCallback), storageAsyncResult);
			}
			return storageAsyncResult;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00030B48 File Offset: 0x0002ED48
		private void WriteStreamCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			Exception exception = null;
			try
			{
				this.cryptoStream.EndWrite(ar);
				this.position += (long)((int)storageAsyncResult.OperationState);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00030BB4 File Offset: 0x0002EDB4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00030BCE File Offset: 0x0002EDCE
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (disposing)
				{
					this.cryptoStream.Close();
				}
				if (this.transform != null)
				{
					this.transform.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000196 RID: 406
		private readonly Stream userStream;

		// Token: 0x04000197 RID: 407
		private readonly IDictionary<string, string> metadata;

		// Token: 0x04000198 RID: 408
		private long position;

		// Token: 0x04000199 RID: 409
		private long? userProvidedLength;

		// Token: 0x0400019A RID: 410
		private byte[] iv = new byte[16];

		// Token: 0x0400019B RID: 411
		private BlobEncryptionPolicy encryptionPolicy;

		// Token: 0x0400019C RID: 412
		private int discardFirst;

		// Token: 0x0400019D RID: 413
		private Stream cryptoStream;

		// Token: 0x0400019E RID: 414
		private bool bufferIV;

		// Token: 0x0400019F RID: 415
		private bool noPadding;

		// Token: 0x040001A0 RID: 416
		private bool disposed;

		// Token: 0x040001A1 RID: 417
		private bool? requireEncryption;

		// Token: 0x040001A2 RID: 418
		private ICryptoTransform transform;
	}
}
