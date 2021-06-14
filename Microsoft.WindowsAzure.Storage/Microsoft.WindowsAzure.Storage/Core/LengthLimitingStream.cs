using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Core
{
	// Token: 0x02000056 RID: 86
	internal class LengthLimitingStream : Stream
	{
		// Token: 0x06000D3C RID: 3388 RVA: 0x00030E48 File Offset: 0x0002F048
		public LengthLimitingStream(Stream wrappedStream, long start, long? length = null)
		{
			this.wrappedStream = wrappedStream;
			this.startOffset = start;
			this.length = length;
			if (length != null)
			{
				this.endOffset = this.startOffset + (this.length - 1L);
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00030ED4 File Offset: 0x0002F0D4
		public override bool CanRead
		{
			get
			{
				return this.wrappedStream.CanRead;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00030EE1 File Offset: 0x0002F0E1
		public override bool CanSeek
		{
			get
			{
				return this.wrappedStream.CanSeek;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000D3F RID: 3391 RVA: 0x00030EEE File Offset: 0x0002F0EE
		public override bool CanWrite
		{
			get
			{
				return this.wrappedStream.CanWrite;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x00030EFB File Offset: 0x0002F0FB
		public override long Length
		{
			get
			{
				if (this.length == null)
				{
					return this.wrappedStream.Length;
				}
				return this.length.Value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x00030F21 File Offset: 0x0002F121
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x00030F29 File Offset: 0x0002F129
		public override long Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00030F34 File Offset: 0x0002F134
		public override void Flush()
		{
			this.wrappedStream.Flush();
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00030F41 File Offset: 0x0002F141
		public override void SetLength(long value)
		{
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00030F44 File Offset: 0x0002F144
		public override long Seek(long offset, SeekOrigin origin)
		{
			long val;
			switch (origin)
			{
			case SeekOrigin.Begin:
				val = offset;
				break;
			case SeekOrigin.Current:
				val = this.position + offset;
				break;
			case SeekOrigin.End:
				val = this.Length + offset;
				break;
			default:
				CommonUtility.ArgumentOutOfRange("origin", origin);
				throw new ArgumentOutOfRangeException("origin");
			}
			CommonUtility.AssertInBounds<long>("offset", val, 0L, this.Length);
			this.position = val;
			return this.position;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00030FBC File Offset: 0x0002F1BC
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.wrappedStream.Read(buffer, offset, count);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00030FCC File Offset: 0x0002F1CC
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this.wrappedStream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00030FE0 File Offset: 0x0002F1E0
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.wrappedStream.EndRead(asyncResult);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00030FF0 File Offset: 0x0002F1F0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.position < this.startOffset)
			{
				int num = (int)Math.Min(this.startOffset - this.position, (long)count);
				offset += num;
				count -= num;
				this.position += (long)num;
			}
			if (this.endOffset != null)
			{
				count = (int)Math.Min(this.endOffset.Value + 1L - this.position, (long)count);
			}
			if (count > 0)
			{
				this.wrappedStream.Write(buffer, offset, count);
				this.position += (long)count;
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00031088 File Offset: 0x0002F288
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (this.position < this.startOffset)
			{
				int num = (int)Math.Min(this.startOffset - this.position, (long)count);
				offset += num;
				count -= num;
				this.position += (long)num;
			}
			if (this.endOffset != null)
			{
				count = (int)Math.Min(this.endOffset.Value + 1L - this.position, (long)count);
			}
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			if (count <= 0)
			{
				storageAsyncResult.OnComplete();
			}
			else
			{
				storageAsyncResult.OperationState = count;
				this.wrappedStream.BeginWrite(buffer, offset, count, new AsyncCallback(this.WriteStreamCallback), storageAsyncResult);
			}
			return storageAsyncResult;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00031140 File Offset: 0x0002F340
		private void WriteStreamCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			Exception exception = null;
			try
			{
				this.wrappedStream.EndWrite(ar);
				this.position += (long)((int)storageAsyncResult.OperationState);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000311AC File Offset: 0x0002F3AC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000311C6 File Offset: 0x0002F3C6
		protected override void Dispose(bool disposing)
		{
		}

		// Token: 0x040001A5 RID: 421
		private readonly Stream wrappedStream;

		// Token: 0x040001A6 RID: 422
		private long startOffset;

		// Token: 0x040001A7 RID: 423
		private long? endOffset;

		// Token: 0x040001A8 RID: 424
		private long position;

		// Token: 0x040001A9 RID: 425
		private long? length;
	}
}
