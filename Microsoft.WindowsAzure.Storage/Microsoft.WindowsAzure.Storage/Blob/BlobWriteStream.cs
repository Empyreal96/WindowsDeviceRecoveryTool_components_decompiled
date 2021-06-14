using System;
using System.IO;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000014 RID: 20
	internal sealed class BlobWriteStream : BlobWriteStreamBase
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00006C03 File Offset: 0x00004E03
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00006C0B File Offset: 0x00004E0B
		internal bool IgnoreFlush { get; set; }

		// Token: 0x0600016C RID: 364 RVA: 0x00006C14 File Offset: 0x00004E14
		internal BlobWriteStream(CloudBlockBlob blockBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : base(blockBlob, accessCondition, options, operationContext)
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006C21 File Offset: 0x00004E21
		internal BlobWriteStream(CloudPageBlob pageBlob, long pageBlobSize, bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : base(pageBlob, pageBlobSize, createNew, accessCondition, options, operationContext)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006C32 File Offset: 0x00004E32
		internal BlobWriteStream(CloudAppendBlob appendBlob, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext) : base(appendBlob, accessCondition, options, operationContext)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006C40 File Offset: 0x00004E40
		public override long Seek(long offset, SeekOrigin origin)
		{
			long currentOffset = this.currentOffset;
			long newOffset = base.GetNewOffset(offset, origin);
			if (currentOffset != newOffset)
			{
				if (this.blobMD5 != null)
				{
					this.blobMD5.Dispose();
					this.blobMD5 = null;
				}
				this.Flush();
			}
			this.currentOffset = newOffset;
			this.currentBlobOffset = newOffset;
			return this.currentOffset;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006C95 File Offset: 0x00004E95
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.EndWrite(this.BeginWrite(buffer, offset, count, null, null));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			CommonUtility.AssertInBounds<int>("offset", offset, 0, buffer.Length);
			CommonUtility.AssertInBounds<int>("count", count, 0, buffer.Length - offset);
			if (this.committed)
			{
				throw new InvalidOperationException("Blob stream has already been committed once.");
			}
			if (this.blobMD5 != null)
			{
				this.blobMD5.UpdateHash(buffer, offset, count);
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
					if (this.blockMD5 != null)
					{
						this.blockMD5.UpdateHash(buffer, offset, num2);
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

		// Token: 0x06000172 RID: 370 RVA: 0x00006DAC File Offset: 0x00004FAC
		public override void EndWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
			if (this.lastException != null)
			{
				throw this.lastException;
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006DDC File Offset: 0x00004FDC
		public override void Flush()
		{
			if (!this.IgnoreFlush)
			{
				if (this.lastException != null)
				{
					throw this.lastException;
				}
				if (this.committed)
				{
					throw new InvalidOperationException("Blob stream has already been committed once.");
				}
				this.DispatchWrite(null);
				this.noPendingWritesEvent.Wait();
				if (this.lastException != null)
				{
					throw this.lastException;
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006E80 File Offset: 0x00005080
		public override ICancellableAsyncResult BeginFlush(AsyncCallback callback, object state)
		{
			if (this.committed)
			{
				throw new InvalidOperationException("Blob stream has already been committed once.");
			}
			if (this.flushPending)
			{
				throw new InvalidOperationException("Blob stream has a pending flush operation. Please call EndFlush first.");
			}
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult storageAsyncResult2;
			try
			{
				if (this.IgnoreFlush)
				{
					storageAsyncResult.OnComplete();
				}
				else
				{
					this.flushPending = true;
					this.DispatchWrite(null);
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

		// Token: 0x06000175 RID: 373 RVA: 0x00006FAC File Offset: 0x000051AC
		public override void EndFlush(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			this.flushPending = false;
			storageAsyncResult.End();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006FD0 File Offset: 0x000051D0
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

		// Token: 0x06000177 RID: 375 RVA: 0x00007010 File Offset: 0x00005210
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

		// Token: 0x06000178 RID: 376 RVA: 0x0000703C File Offset: 0x0000523C
		public override void Commit()
		{
			this.Flush();
			this.committed = true;
			try
			{
				if (this.blockBlob != null)
				{
					if (this.blobMD5 != null)
					{
						this.blockBlob.Properties.ContentMD5 = this.blobMD5.ComputeHash();
					}
					this.blockBlob.PutBlockList(this.blockList, this.accessCondition, this.options, this.operationContext);
				}
				else if (this.blobMD5 != null)
				{
					base.Blob.Properties.ContentMD5 = this.blobMD5.ComputeHash();
					base.Blob.SetProperties(this.accessCondition, this.options, this.operationContext);
				}
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
				throw;
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007108 File Offset: 0x00005308
		public override ICancellableAsyncResult BeginCommit(AsyncCallback callback, object state)
		{
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult @object = this.BeginFlush(new AsyncCallback(this.CommitFlushCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007148 File Offset: 0x00005348
		public override void EndCommit(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007164 File Offset: 0x00005364
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
					if (this.blockBlob != null)
					{
						if (this.blobMD5 != null)
						{
							this.blockBlob.Properties.ContentMD5 = this.blobMD5.ComputeHash();
						}
						ICancellableAsyncResult @object = this.blockBlob.BeginPutBlockList(this.blockList, this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.PutBlockListCallback), storageAsyncResult);
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
					else if (this.blobMD5 != null)
					{
						base.Blob.Properties.ContentMD5 = this.blobMD5.ComputeHash();
						ICancellableAsyncResult object2 = base.Blob.BeginSetProperties(this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.SetPropertiesCallback), storageAsyncResult);
						storageAsyncResult.CancelDelegate = new Action(object2.Cancel);
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

		// Token: 0x0600017C RID: 380 RVA: 0x000072F4 File Offset: 0x000054F4
		private void PutBlockListCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			try
			{
				this.blockBlob.EndPutBlockList(ar);
				storageAsyncResult.OnComplete();
			}
			catch (Exception ex)
			{
				this.lastException = ex;
				storageAsyncResult.OnComplete(ex);
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007350 File Offset: 0x00005550
		private void SetPropertiesCallback(IAsyncResult ar)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)ar.AsyncState;
			storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
			try
			{
				base.Blob.EndSetProperties(ar);
				storageAsyncResult.OnComplete();
			}
			catch (Exception ex)
			{
				this.lastException = ex;
				storageAsyncResult.OnComplete(ex);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000073AC File Offset: 0x000055AC
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
			this.internalBuffer = new MultiBufferMemoryStream(base.Blob.ServiceClient.BufferManager, 65536);
			internalBuffer.Seek(0L, SeekOrigin.Begin);
			string text = null;
			if (this.blockMD5 != null)
			{
				text = this.blockMD5.ComputeHash();
				this.blockMD5.Dispose();
				this.blockMD5 = new MD5Wrapper();
			}
			if (this.blockBlob != null)
			{
				string currentBlockId = base.GetCurrentBlockId();
				this.blockList.Add(currentBlockId);
				this.WriteBlock(internalBuffer, currentBlockId, text, asyncResult);
				return;
			}
			if (this.pageBlob != null)
			{
				if (internalBuffer.Length % 512L != 0L)
				{
					this.lastException = new IOException("Page data must be a multiple of 512 bytes.");
					throw this.lastException;
				}
				long currentBlobOffset = this.currentBlobOffset;
				this.currentBlobOffset += internalBuffer.Length;
				this.WritePages(internalBuffer, currentBlobOffset, text, asyncResult);
				return;
			}
			else
			{
				long currentBlobOffset2 = this.currentBlobOffset;
				this.currentBlobOffset += internalBuffer.Length;
				if (this.accessCondition.IfMaxSizeLessThanOrEqual != null && this.currentBlobOffset > this.accessCondition.IfMaxSizeLessThanOrEqual.Value)
				{
					this.lastException = new IOException("Append block data should not exceed the maximum blob size condition value.");
					throw this.lastException;
				}
				this.WriteAppendBlock(internalBuffer, currentBlobOffset2, text, asyncResult);
				return;
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007638 File Offset: 0x00005838
		private void WriteBlock(Stream blockData, string blockId, string blockMD5, StorageAsyncResult<NullType> asyncResult)
		{
			this.noPendingWritesEvent.Increment();
			this.parallelOperationSemaphore.WaitAsync(delegate(bool calledSynchronously)
			{
				try
				{
					ICancellableAsyncResult @object = this.blockBlob.BeginPutBlock(blockId, blockData, blockMD5, this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.PutBlockCallback), null);
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

		// Token: 0x06000180 RID: 384 RVA: 0x00007694 File Offset: 0x00005894
		private void PutBlockCallback(IAsyncResult ar)
		{
			try
			{
				this.blockBlob.EndPutBlock(ar);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
			}
			this.noPendingWritesEvent.Decrement();
			this.parallelOperationSemaphore.Release();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000077F0 File Offset: 0x000059F0
		private void WritePages(Stream pageData, long offset, string contentMD5, StorageAsyncResult<NullType> asyncResult)
		{
			this.noPendingWritesEvent.Increment();
			this.parallelOperationSemaphore.WaitAsync(delegate(bool calledSynchronously)
			{
				try
				{
					ICancellableAsyncResult @object = this.pageBlob.BeginWritePages(pageData, offset, contentMD5, this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.WritePagesCallback), null);
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

		// Token: 0x06000182 RID: 386 RVA: 0x0000784C File Offset: 0x00005A4C
		private void WritePagesCallback(IAsyncResult ar)
		{
			try
			{
				this.pageBlob.EndWritePages(ar);
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
			}
			this.noPendingWritesEvent.Decrement();
			this.parallelOperationSemaphore.Release();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000079D8 File Offset: 0x00005BD8
		private void WriteAppendBlock(Stream blockData, long offset, string blockMD5, StorageAsyncResult<NullType> asyncResult)
		{
			this.noPendingWritesEvent.Increment();
			this.parallelOperationSemaphore.WaitAsync(delegate(bool calledSynchronously)
			{
				try
				{
					this.accessCondition.IfAppendPositionEqual = new long?(offset);
					int count = this.operationContext.RequestResults.Count;
					ICancellableAsyncResult @object = this.appendBlob.BeginAppendBlock(blockData, blockMD5, this.accessCondition, this.options, this.operationContext, new AsyncCallback(this.AppendBlockCallback), count);
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

		// Token: 0x06000184 RID: 388 RVA: 0x00007A34 File Offset: 0x00005C34
		private void AppendBlockCallback(IAsyncResult ar)
		{
			try
			{
				this.appendBlob.EndAppendBlock(ar);
			}
			catch (StorageException ex)
			{
				if (this.options.AbsorbConditionalErrorsOnRetry.Value && ex.RequestInformation.HttpStatusCode == 412)
				{
					int num = (int)ar.AsyncState;
					StorageExtendedErrorInformation extendedErrorInformation = ex.RequestInformation.ExtendedErrorInformation;
					if (extendedErrorInformation != null && (extendedErrorInformation.ErrorCode == BlobErrorCodeStrings.InvalidAppendCondition || extendedErrorInformation.ErrorCode == BlobErrorCodeStrings.InvalidMaxBlobSizeCondition) && this.operationContext.RequestResults.Count - num > 1)
					{
						Logger.LogWarning(this.operationContext, "Pre-condition failure on a retry is being ignored since the request should have succeeded in the first attempt.", new object[0]);
					}
					else
					{
						this.lastException = ex;
					}
				}
				else
				{
					this.lastException = ex;
				}
			}
			catch (Exception lastException)
			{
				this.lastException = lastException;
			}
			this.noPendingWritesEvent.Decrement();
			this.parallelOperationSemaphore.Release();
		}

		// Token: 0x0400009D RID: 157
		private volatile bool flushPending;
	}
}
