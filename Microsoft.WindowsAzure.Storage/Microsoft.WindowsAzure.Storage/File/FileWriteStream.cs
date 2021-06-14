using System;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x0200002C RID: 44
	internal sealed class FileWriteStream : FileWriteStreamBase
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x0002132F File Offset: 0x0001F52F
		internal FileWriteStream(CloudFile file, long fileSize, bool createNew, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext) : base(file, fileSize, createNew, accessCondition, options, operationContext)
		{
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00021340 File Offset: 0x0001F540
		public override long Seek(long offset, SeekOrigin origin)
		{
			long currentOffset = this.currentOffset;
			long newOffset = base.GetNewOffset(offset, origin);
			if (currentOffset != newOffset)
			{
				if (this.fileMD5 != null)
				{
					this.fileMD5.Dispose();
					this.fileMD5 = null;
				}
				this.Flush();
			}
			this.currentOffset = newOffset;
			this.currentFileOffset = newOffset;
			return this.currentOffset;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00021395 File Offset: 0x0001F595
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EndWrite(this.BeginWrite(buffer, offset, count, null, null));
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000213A8 File Offset: 0x0001F5A8
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.committed)
			{
				throw new InvalidOperationException("File stream has already been committed once.");
			}
			if (this.fileMD5 != null)
			{
				this.fileMD5.UpdateHash(buffer, offset, count);
			}
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			StorageAsyncResult<NullType> asyncResult = storageAsyncResult;
			this.currentOffset += (long)count;
			bool flag = false;
			if (this.lastException == null)
			{
				while (count > 0)
				{
					int num = this.streamWriteSizeInBytes - (int)this.internalBuffer.Length;
					int num2 = Math.Min(count, num);
					this.internalBuffer.Write(buffer, offset, num2);
					if (this.rangeMD5 != null)
					{
						this.rangeMD5.UpdateHash(buffer, offset, num2);
					}
					count -= num2;
					offset += num2;
					if (num2 == num)
					{
						this.DispatchWrite(asyncResult);
						flag = true;
						asyncResult = null;
					}
				}
			}
			if (!flag)
			{
				storageAsyncResult.OnComplete(this.lastException);
			}
			return storageAsyncResult;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000214AC File Offset: 0x0001F6AC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
			if (this.lastException != null)
			{
				throw this.lastException;
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000214DC File Offset: 0x0001F6DC
		public override void Flush()
		{
			if (this.lastException != null)
			{
				throw this.lastException;
			}
			if (this.committed)
			{
				throw new InvalidOperationException("File stream has already been committed once.");
			}
			this.DispatchWrite(null);
			this.noPendingWritesEvent.Wait();
			if (this.lastException != null)
			{
				throw this.lastException;
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00021574 File Offset: 0x0001F774
		public override ICancellableAsyncResult BeginFlush(AsyncCallback callback, object state)
		{
			if (this.committed)
			{
				throw new InvalidOperationException("File stream has already been committed once.");
			}
			if (this.flushPending)
			{
				throw new InvalidOperationException("File stream has a pending flush operation. Please call EndFlush first.");
			}
			ICancellableAsyncResult storageAsyncResult2;
			try
			{
				this.flushPending = true;
				this.DispatchWrite(null);
				StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
				if (this.lastException != null || this.noPendingWritesEvent.Wait(0))
				{
					storageAsyncResult.OnComplete(this.lastException);
				}
				else
				{
					RegisteredWaitHandle waitHandle = ThreadPool.RegisterWaitForSingleObject(this.noPendingWritesEvent.WaitHandle, new WaitOrTimerCallback(this.WaitForPendingWritesCallback), storageAsyncResult, -1, true);
					storageAsyncResult.OperationState = waitHandle;
					storageAsyncResult.CancelDelegate = delegate()
					{
						waitHandle.Unregister(null);
						storageAsyncResult.OnComplete(this.lastException);
					};
				}
				storageAsyncResult2 = storageAsyncResult;
			}
			catch (Exception)
			{
				this.flushPending = false;
				throw;
			}
			return storageAsyncResult2;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00021688 File Offset: 0x0001F888
		public override void EndFlush(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			this.flushPending = false;
			storageAsyncResult.End();
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x000216AC File Offset: 0x0001F8AC
		private void WaitForPendingWritesCallback(object state, bool timedOut)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)state;
			storageAsyncResult.UpdateCompletedSynchronously(false);
			storageAsyncResult.OnComplete(this.lastException);
			RegisteredWaitHandle registeredWaitHandle = (RegisteredWaitHandle)storageAsyncResult.OperationState;
			if (registeredWaitHandle != null)
			{
				registeredWaitHandle.Unregister(null);
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000216EC File Offset: 0x0001F8EC
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
				if (disposing && !this.committed)
				{
					this.Commit();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00021718 File Offset: 0x0001F918
		public override void Commit()
		{
			this.Flush();
			this.committed = true;
			try
			{
				if (this.fileMD5 != null)
				{
					this.file.Properties.ContentMD5 = this.fileMD5.ComputeHash();
					this.file.SetProperties(this.accessCondition, this.options, this.operationContext);
				}
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
				throw;
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00021794 File Offset: 0x0001F994
		public override ICancellableAsyncResult BeginCommit(AsyncCallback callback, object state)
		{
			if (this.committed)
			{
				throw new InvalidOperationException("File stream has already been committed once.");
			}
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult @object = this.BeginFlush(new AsyncCallback(this.CommitFlushCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000217E8 File Offset: 0x0001F9E8
		public override void EndCommit(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00021804 File Offset: 0x0001FA04
		private void CommitFlushCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			this.committed = true;
			lock (storageAsyncResult.CancellationLockerObject)
			{
				storageAsyncResult.CancelDelegate = null;
				try
				{
					this.EndFlush(ar);
					if (this.fileMD5 != null)
					{
						this.file.Properties.ContentMD5 = this.fileMD5.ComputeHash();
						ICancellableAsyncResult @object = this.file.BeginSetProperties(this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.SetPropertiesCallback), storageAsyncResult);
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
					else
					{
						storageAsyncResult.OnComplete();
					}
					if (storageAsyncResult.CancelRequested)
					{
						storageAsyncResult.Cancel();
					}
				}
				catch (Exception ex)
				{
					this.lastException = ex;
					storageAsyncResult.OnComplete(ex);
				}
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00021908 File Offset: 0x0001FB08
		private void SetPropertiesCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			try
			{
				this.file.EndSetProperties(ar);
				storageAsyncResult.OnComplete();
			}
			catch (Exception ex)
			{
				this.lastException = ex;
				storageAsyncResult.OnComplete(ex);
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00021964 File Offset: 0x0001FB64
		private void DispatchWrite(StorageAsyncResult<NullType> asyncResult)
		{
			if (this.internalBuffer.Length == 0L)
			{
				if (asyncResult != null)
				{
					asyncResult.OnComplete(this.lastException);
				}
				return;
			}
			MultiBufferMemoryStream internalBuffer = this.internalBuffer;
			this.internalBuffer = new MultiBufferMemoryStream(this.file.ServiceClient.BufferManager, 65536);
			internalBuffer.Seek(0L, SeekOrigin.Begin);
			string contentMD = null;
			if (this.rangeMD5 != null)
			{
				contentMD = this.rangeMD5.ComputeHash();
				this.rangeMD5.Dispose();
				this.rangeMD5 = new MD5Wrapper();
			}
			long currentFileOffset = this.currentFileOffset;
			this.currentFileOffset += internalBuffer.Length;
			this.WriteRange(internalBuffer, currentFileOffset, contentMD, asyncResult);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00021B20 File Offset: 0x0001FD20
		private void WriteRange(Stream rangeData, long offset, string contentMD5, StorageAsyncResult<NullType> asyncResult)
		{
			this.noPendingWritesEvent.Increment();
			this.parallelOperationSemaphore.WaitAsync(delegate(bool calledSynchronously)
			{
				try
				{
					ICancellableAsyncResult @object = this.file.BeginWriteRange(rangeData, offset, contentMD5, this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.WriteRangeCallback), null);
					if (asyncResult != null)
					{
						asyncResult.CancelDelegate = new Action(@object.Cancel);
					}
				}
				catch (Exception lastException)
				{
					this.lastException = lastException;
					this.noPendingWritesEvent.Decrement();
					this.parallelOperationSemaphore.Release();
				}
				finally
				{
					if (asyncResult != null)
					{
						asyncResult.UpdateCompletedSynchronously(calledSynchronously);
						asyncResult.OnComplete(this.lastException);
					}
				}
			});
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00021B7C File Offset: 0x0001FD7C
		private void WriteRangeCallback(IAsyncResult ar)
		{
			try
			{
				this.file.EndWriteRange(ar);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
			}
			this.noPendingWritesEvent.Decrement();
			this.parallelOperationSemaphore.Release();
		}

		// Token: 0x04000113 RID: 275
		private volatile bool flushPending;
	}
}
