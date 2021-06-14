using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000012 RID: 18
	internal sealed class BlobReadStream : BlobReadStreamBase
	{
		// Token: 0x06000152 RID: 338 RVA: 0x000064A5 File Offset: 0x000046A5
		internal BlobReadStream(CloudBlob blob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : base(blob, accessCondition, options, operationContext)
		{
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000064B4 File Offset: 0x000046B4
		public override int Read(byte[] buffer, int offset, int count)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.lastException != null)
			{
				throw this.lastException;
			}
			if (this.currentOffset == this.Length || count == 0)
			{
				return 0;
			}
			int num = base.ConsumeBuffer(buffer, offset, count);
			if (num > 0)
			{
				return num;
			}
			return this.DispatchReadSync(buffer, offset, count);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000652C File Offset: 0x0000472C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.readPending)
			{
				throw new InvalidOperationException("Blob stream has a pending read operation. Please call EndRead first.");
			}
			IAsyncResult result;
			try
			{
				this.readPending = true;
				StorageAsyncResult<int> storageAsyncResult = new StorageAsyncResult<int>(callback, state);
				if (this.lastException != null)
				{
					storageAsyncResult.OnComplete(this.lastException);
					result = storageAsyncResult;
				}
				else if (this.currentOffset == this.Length || count == 0)
				{
					storageAsyncResult.Result = 0;
					storageAsyncResult.OnComplete();
					result = storageAsyncResult;
				}
				else
				{
					int num = base.ConsumeBuffer(buffer, offset, count);
					if (num > 0)
					{
						storageAsyncResult.Result = num;
						storageAsyncResult.OnComplete();
						result = storageAsyncResult;
					}
					else
					{
						this.DispatchReadAsync(storageAsyncResult, buffer, offset, count);
						result = storageAsyncResult;
					}
				}
			}
			catch (Exception)
			{
				this.readPending = false;
				throw;
			}
			return result;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006614 File Offset: 0x00004814
		public override int EndRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			this.readPending = false;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006640 File Offset: 0x00004840
		private void DispatchReadAsync(StorageAsyncResult<int> storageAsyncResult, byte[] buffer, int offset, int count)
		{
			storageAsyncResult.OperationState = new ArraySegment<byte>(buffer, offset, count);
			try
			{
				this.internalBuffer.SetLength(0L);
				this.blob.BeginDownloadRangeToStream(this.internalBuffer, new long?(this.currentOffset), new long?((long)base.GetReadSize()), this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.DownloadRangeToStreamCallback), storageAsyncResult);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
				throw;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000066D4 File Offset: 0x000048D4
		private void DownloadRangeToStreamCallback(IAsyncResult ar)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			try
			{
				this.blob.EndDownloadRangeToStream(ar);
				ArraySegment<byte> arraySegment = (ArraySegment<byte>)storageAsyncResult.OperationState;
				this.internalBuffer.Seek(0L, SeekOrigin.Begin);
				storageAsyncResult.Result = base.ConsumeBuffer(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
			}
			storageAsyncResult.OnComplete(this.lastException);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006770 File Offset: 0x00004970
		private int DispatchReadSync(byte[] buffer, int offset, int count)
		{
			int result;
			try
			{
				this.internalBuffer.SetLength(0L);
				this.blob.DownloadRangeToStream(this.internalBuffer, new long?(this.currentOffset), new long?((long)base.GetReadSize()), this.accessCondition, this.options, this.operationContext);
				this.internalBuffer.Seek(0L, SeekOrigin.Begin);
				result = base.ConsumeBuffer(buffer, offset, count);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
				throw;
			}
			return result;
		}

		// Token: 0x04000086 RID: 134
		private volatile bool readPending;
	}
}
