using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x0200002A RID: 42
	internal sealed class FileReadStream : FileReadStreamBase
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x00020D19 File Offset: 0x0001EF19
		internal FileReadStream(CloudFile file, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext) : base(file, accessCondition, options, operationContext)
		{
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00020D28 File Offset: 0x0001EF28
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

		// Token: 0x06000912 RID: 2322 RVA: 0x00020D9C File Offset: 0x0001EF9C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.readPending)
			{
				throw new InvalidOperationException("File stream has a pending read operation. Please call EndRead first.");
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

		// Token: 0x06000913 RID: 2323 RVA: 0x00020E80 File Offset: 0x0001F080
		public override int EndRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			this.readPending = false;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00020EAC File Offset: 0x0001F0AC
		private void DispatchReadAsync(StorageAsyncResult<int> storageAsyncResult, byte[] buffer, int offset, int count)
		{
			storageAsyncResult.OperationState = new ArraySegment<byte>(buffer, offset, count);
			try
			{
				this.internalBuffer.SetLength(0L);
				this.file.BeginDownloadRangeToStream(this.internalBuffer, new long?(this.currentOffset), new long?((long)base.GetReadSize()), null, this.options, this.operationContext, new AsyncCallback(this.DownloadRangeToStreamCallback), storageAsyncResult);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
				throw;
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00020F3C File Offset: 0x0001F13C
		private void DownloadRangeToStreamCallback(IAsyncResult ar)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			try
			{
				this.file.EndDownloadRangeToStream(ar);
				if (!this.file.Properties.ETag.Equals(this.accessCondition.IfMatchETag, StringComparison.Ordinal))
				{
					throw new StorageException(new RequestResult
					{
						HttpStatusMessage = null,
						HttpStatusCode = 412,
						ExtendedErrorInformation = null
					}, "The condition specified using HTTP conditional header(s) is not met.", null);
				}
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

		// Token: 0x06000916 RID: 2326 RVA: 0x00021020 File Offset: 0x0001F220
		private int DispatchReadSync(byte[] buffer, int offset, int count)
		{
			int result;
			try
			{
				this.internalBuffer.SetLength(0L);
				this.file.DownloadRangeToStream(this.internalBuffer, new long?(this.currentOffset), new long?((long)base.GetReadSize()), null, this.options, this.operationContext);
				if (!this.file.Properties.ETag.Equals(this.accessCondition.IfMatchETag, StringComparison.Ordinal))
				{
					throw new StorageException(new RequestResult
					{
						HttpStatusMessage = null,
						HttpStatusCode = 412,
						ExtendedErrorInformation = null
					}, "The condition specified using HTTP conditional header(s) is not met.", null);
				}
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

		// Token: 0x04000101 RID: 257
		private volatile bool readPending;
	}
}
