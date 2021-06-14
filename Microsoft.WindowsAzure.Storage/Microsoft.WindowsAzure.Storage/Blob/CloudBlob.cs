using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000016 RID: 22
	public class CloudBlob : IListBlobItem
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00007B40 File Offset: 0x00005D40
		[DoesServiceRequest]
		public Stream OpenRead(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.FetchAttributes(accessCondition, options, operationContext);
			AccessCondition accessCondition2 = AccessCondition.CloneConditionWithETag(accessCondition, this.Properties.ETag);
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, false);
			return new BlobReadStream(this, accessCondition2, options2, operationContext);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007B80 File Offset: 0x00005D80
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenRead(AsyncCallback callback, object state)
		{
			return this.BeginOpenRead(null, null, null, callback, state);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007C44 File Offset: 0x00005E44
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenRead(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			StorageAsyncResult<Stream> storageAsyncResult = new StorageAsyncResult<Stream>(callback, state);
			ICancellableAsyncResult @object = this.BeginFetchAttributes(accessCondition, options, operationContext, delegate(IAsyncResult ar)
			{
				try
				{
					this.EndFetchAttributes(ar);
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					AccessCondition accessCondition2 = AccessCondition.CloneConditionWithETag(accessCondition, this.Properties.ETag);
					BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, false);
					storageAsyncResult.Result = new BlobReadStream(this, accessCondition2, options2, operationContext);
					storageAsyncResult.OnComplete();
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007CC8 File Offset: 0x00005EC8
		public Stream EndOpenRead(IAsyncResult asyncResult)
		{
			StorageAsyncResult<Stream> storageAsyncResult = (StorageAsyncResult<Stream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007CE8 File Offset: 0x00005EE8
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync()
		{
			return this.OpenReadAsync(CancellationToken.None);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007CF5 File Offset: 0x00005EF5
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Stream>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenRead), new Func<IAsyncResult, Stream>(this.EndOpenRead), cancellationToken);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007D15 File Offset: 0x00005F15
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.OpenReadAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007D25 File Offset: 0x00005F25
		[DoesServiceRequest]
		public Task<Stream> OpenReadAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, BlobRequestOptions, OperationContext, Stream>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenRead), new Func<IAsyncResult, Stream>(this.EndOpenRead), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007D4C File Offset: 0x00005F4C
		[DoesServiceRequest]
		public void DownloadToStream(Stream target, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.DownloadRangeToStream(target, null, null, accessCondition, options, operationContext);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007D76 File Offset: 0x00005F76
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToStream(Stream target, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToStream(target, null, null, null, callback, state);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007D84 File Offset: 0x00005F84
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToStream(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToStream(target, null, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007DB2 File Offset: 0x00005FB2
		public void EndDownloadToStream(IAsyncResult asyncResult)
		{
			this.EndDownloadRangeToStream(asyncResult);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007DBB File Offset: 0x00005FBB
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target)
		{
			return this.DownloadToStreamAsync(target, CancellationToken.None);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007DC9 File Offset: 0x00005FC9
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToStream), new Action<IAsyncResult>(this.EndDownloadToStream), target, cancellationToken);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007DEA File Offset: 0x00005FEA
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToStreamAsync(target, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007DFC File Offset: 0x00005FFC
		[DoesServiceRequest]
		public Task DownloadToStreamAsync(Stream target, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToStream), new Action<IAsyncResult>(this.EndDownloadToStream), target, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007E24 File Offset: 0x00006024
		[DoesServiceRequest]
		public void DownloadToFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			try
			{
				using (FileStream fileStream = new FileStream(path, mode, FileAccess.Write))
				{
					this.DownloadToStream(fileStream, accessCondition, options, operationContext);
				}
			}
			catch (Exception)
			{
				if (mode != FileMode.Create)
				{
					if (mode != FileMode.CreateNew)
					{
						goto IL_42;
					}
				}
				try
				{
					File.Delete(path);
				}
				catch (Exception)
				{
				}
				IL_42:
				throw;
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007EA0 File Offset: 0x000060A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007EB0 File Offset: 0x000060B0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToFile(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("path", path);
			FileStream fileStream = new FileStream(path, mode, FileAccess.Write);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state)
			{
				OperationState = Tuple.Create<FileStream, FileMode>(fileStream, mode)
			};
			ICancellableAsyncResult result;
			try
			{
				ICancellableAsyncResult @object = this.BeginDownloadToStream(fileStream, accessCondition, options, operationContext, new AsyncCallback(this.DownloadToFileCallback), storageAsyncResult);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
				result = storageAsyncResult;
			}
			catch (Exception)
			{
				fileStream.Dispose();
				throw;
			}
			return result;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007F38 File Offset: 0x00006138
		private void DownloadToFileCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult.AsyncState;
			Exception exception = null;
			bool flag = false;
			string path = null;
			try
			{
				this.EndDownloadToStream(asyncResult);
			}
			catch (Exception ex)
			{
				exception = ex;
				flag = true;
			}
			try
			{
				FileStream item = ((Tuple<FileStream, FileMode>)storageAsyncResult.OperationState).Item1;
				path = item.Name;
				item.Dispose();
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			if (flag)
			{
				try
				{
					FileMode item2 = ((Tuple<FileStream, FileMode>)storageAsyncResult.OperationState).Item2;
					if (item2 == FileMode.Create || item2 == FileMode.CreateNew)
					{
						File.Delete(path);
					}
				}
				catch (Exception)
				{
				}
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007FEC File Offset: 0x000061EC
		public void EndDownloadToFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008006 File Offset: 0x00006206
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode)
		{
			return this.DownloadToFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008015 File Offset: 0x00006215
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToFile), new Action<IAsyncResult>(this.EndDownloadToFile), path, mode, cancellationToken);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008037 File Offset: 0x00006237
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000804B File Offset: 0x0000624B
		[DoesServiceRequest]
		public Task DownloadToFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToFile), new Action<IAsyncResult>(this.EndDownloadToFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008074 File Offset: 0x00006274
		[DoesServiceRequest]
		public int DownloadToByteArray(byte[] target, int index, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.DownloadRangeToByteArray(target, index, null, null, accessCondition, options, operationContext);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000080A0 File Offset: 0x000062A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AsyncCallback callback, object state)
		{
			return this.BeginDownloadToByteArray(target, index, null, null, null, callback, state);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000080B0 File Offset: 0x000062B0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadToByteArray(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToByteArray(target, index, null, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000080E0 File Offset: 0x000062E0
		public int EndDownloadToByteArray(IAsyncResult asyncResult)
		{
			return this.EndDownloadRangeToByteArray(asyncResult);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000080E9 File Offset: 0x000062E9
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index)
		{
			return this.DownloadToByteArrayAsync(target, index, CancellationToken.None);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000080F8 File Offset: 0x000062F8
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, int>(new Func<byte[], int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToByteArray), new Func<IAsyncResult, int>(this.EndDownloadToByteArray), target, index, cancellationToken);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000811A File Offset: 0x0000631A
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadToByteArrayAsync(target, index, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000812E File Offset: 0x0000632E
		[DoesServiceRequest]
		public Task<int> DownloadToByteArrayAsync(byte[] target, int index, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, AccessCondition, BlobRequestOptions, OperationContext, int>(new Func<byte[], int, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadToByteArray), new Func<IAsyncResult, int>(this.EndDownloadToByteArray), target, index, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008158 File Offset: 0x00006358
		[DoesServiceRequest]
		public void DownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("target", target);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.GetBlobImpl(this.attributes, target, offset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000081A0 File Offset: 0x000063A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToStream(target, offset, length, null, null, null, callback, state);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000081C0 File Offset: 0x000063C0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToStream(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("target", target);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.GetBlobImpl(this.attributes, target, offset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000820B File Offset: 0x0000640B
		public void EndDownloadRangeToStream(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008214 File Offset: 0x00006414
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length)
		{
			return this.DownloadRangeToStreamAsync(target, offset, length, CancellationToken.None);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008224 File Offset: 0x00006424
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long?, long?>(new Func<Stream, long?, long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToStream), new Action<IAsyncResult>(this.EndDownloadRangeToStream), target, offset, length, cancellationToken);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008248 File Offset: 0x00006448
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadRangeToStreamAsync(target, offset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008260 File Offset: 0x00006460
		[DoesServiceRequest]
		public Task DownloadRangeToStreamAsync(Stream target, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long?, long?, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long?, long?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToStream), new Action<IAsyncResult>(this.EndDownloadRangeToStream), target, offset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008298 File Offset: 0x00006498
		[DoesServiceRequest]
		public int DownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			int result;
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(target, index))
			{
				this.DownloadRangeToStream(syncMemoryStream, blobOffset, length, accessCondition, options, operationContext);
				result = (int)syncMemoryStream.Position;
			}
			return result;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000082E4 File Offset: 0x000064E4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AsyncCallback callback, object state)
		{
			return this.BeginDownloadRangeToByteArray(target, index, blobOffset, length, null, null, null, callback, state);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008304 File Offset: 0x00006504
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadRangeToByteArray(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			SyncMemoryStream syncMemoryStream = new SyncMemoryStream(target, index);
			StorageAsyncResult<int> storageAsyncResult = new StorageAsyncResult<int>(callback, state)
			{
				OperationState = syncMemoryStream
			};
			ICancellableAsyncResult @object = this.BeginDownloadRangeToStream(syncMemoryStream, blobOffset, length, accessCondition, options, operationContext, new AsyncCallback(this.DownloadRangeToByteArrayCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008360 File Offset: 0x00006560
		private void DownloadRangeToByteArrayCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult.AsyncState;
			try
			{
				this.EndDownloadRangeToStream(asyncResult);
				SyncMemoryStream syncMemoryStream = (SyncMemoryStream)storageAsyncResult.OperationState;
				storageAsyncResult.Result = (int)syncMemoryStream.Position;
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000083BC File Offset: 0x000065BC
		public int EndDownloadRangeToByteArray(IAsyncResult asyncResult)
		{
			StorageAsyncResult<int> storageAsyncResult = (StorageAsyncResult<int>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000083DC File Offset: 0x000065DC
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length)
		{
			return this.DownloadRangeToByteArrayAsync(target, index, blobOffset, length, CancellationToken.None);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000083EE File Offset: 0x000065EE
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, long?, long?, int>(new Func<byte[], int, long?, long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToByteArray), new Func<IAsyncResult, int>(this.EndDownloadRangeToByteArray), target, index, blobOffset, length, cancellationToken);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008414 File Offset: 0x00006614
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadRangeToByteArrayAsync(target, index, blobOffset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008438 File Offset: 0x00006638
		[DoesServiceRequest]
		public Task<int> DownloadRangeToByteArrayAsync(byte[] target, int index, long? blobOffset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<byte[], int, long?, long?, AccessCondition, BlobRequestOptions, OperationContext, int>(new Func<byte[], int, long?, long?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadRangeToByteArray), new Func<IAsyncResult, int>(this.EndDownloadRangeToByteArray), target, index, blobOffset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000846F File Offset: 0x0000666F
		[DoesServiceRequest]
		public bool Exists(BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.Exists(false, options, operationContext);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000847C File Offset: 0x0000667C
		private bool Exists(bool primaryOnly, BlobRequestOptions options, OperationContext operationContext)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<bool>(this.ExistsImpl(this.attributes, blobRequestOptions, primaryOnly), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000084B2 File Offset: 0x000066B2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000084BE File Offset: 0x000066BE
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginExists(false, options, operationContext, callback, state);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000084CC File Offset: 0x000066CC
		private ICancellableAsyncResult BeginExists(bool primaryOnly, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(this.attributes, blobRequestOptions, primaryOnly), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00008506 File Offset: 0x00006706
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000850E File Offset: 0x0000670E
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000851B File Offset: 0x0000671B
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000853B File Offset: 0x0000673B
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000854A File Offset: 0x0000674A
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobRequestOptions, OperationContext, bool>(new Func<BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000856C File Offset: 0x0000676C
		[DoesServiceRequest]
		public void FetchAttributes(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000085A3 File Offset: 0x000067A3
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, null, callback, state);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000085B0 File Offset: 0x000067B0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000085EA File Offset: 0x000067EA
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000085F3 File Offset: 0x000067F3
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008600 File Offset: 0x00006800
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008620 File Offset: 0x00006820
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008630 File Offset: 0x00006830
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008654 File Offset: 0x00006854
		[DoesServiceRequest]
		public void SetMetadata(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00008696 File Offset: 0x00006896
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, null, callback, state);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000086A4 File Offset: 0x000068A4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000086E9 File Offset: 0x000068E9
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000086F2 File Offset: 0x000068F2
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000086FF File Offset: 0x000068FF
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000871F File Offset: 0x0000691F
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000872F File Offset: 0x0000692F
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00008754 File Offset: 0x00006954
		[DoesServiceRequest]
		public void SetProperties(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetPropertiesImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008796 File Offset: 0x00006996
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AsyncCallback callback, object state)
		{
			return this.BeginSetProperties(null, null, null, callback, state);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000087A4 File Offset: 0x000069A4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetPropertiesImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000087E9 File Offset: 0x000069E9
		public void EndSetProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000087F2 File Offset: 0x000069F2
		[DoesServiceRequest]
		public Task SetPropertiesAsync()
		{
			return this.SetPropertiesAsync(CancellationToken.None);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000087FF File Offset: 0x000069FF
		[DoesServiceRequest]
		public Task SetPropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), cancellationToken);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000881F File Offset: 0x00006A1F
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SetPropertiesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000882F File Offset: 0x00006A2F
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00008854 File Offset: 0x00006A54
		[DoesServiceRequest]
		public void Delete(DeleteSnapshotsOption deleteSnapshotsOption = DeleteSnapshotsOption.None, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.DeleteBlobImpl(this.attributes, deleteSnapshotsOption, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000888D File Offset: 0x00006A8D
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(DeleteSnapshotsOption.None, null, null, null, callback, state);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000889C File Offset: 0x00006A9C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.DeleteBlobImpl(this.attributes, deleteSnapshotsOption, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000088D8 File Offset: 0x00006AD8
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000088E1 File Offset: 0x00006AE1
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000088EE File Offset: 0x00006AEE
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000890E File Offset: 0x00006B0E
		[DoesServiceRequest]
		public Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(deleteSnapshotsOption, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00008920 File Offset: 0x00006B20
		[DoesServiceRequest]
		public Task DeleteAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<DeleteSnapshotsOption, AccessCondition, BlobRequestOptions, OperationContext>(new Func<DeleteSnapshotsOption, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), deleteSnapshotsOption, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008948 File Offset: 0x00006B48
		[DoesServiceRequest]
		public bool DeleteIfExists(DeleteSnapshotsOption deleteSnapshotsOption = DeleteSnapshotsOption.None, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(true, options2, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(deleteSnapshotsOption, accessCondition, options2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.BlobNotFound))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000089E8 File Offset: 0x00006BE8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(DeleteSnapshotsOption.None, null, null, null, callback, state);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x000089F8 File Offset: 0x00006BF8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = blobRequestOptions,
				OperationContext = operationContext
			};
			this.DeleteIfExistsHandler(deleteSnapshotsOption, accessCondition, blobRequestOptions, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008C58 File Offset: 0x00006E58
		private void DeleteIfExistsHandler(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
		{
			ICancellableAsyncResult @object = this.BeginExists(true, options, operationContext, delegate(IAsyncResult existsResult)
			{
				storageAsyncResult.UpdateCompletedSynchronously(existsResult.CompletedSynchronously);
				lock (storageAsyncResult.CancellationLockerObject)
				{
					storageAsyncResult.CancelDelegate = null;
					try
					{
						if (!this.EndExists(existsResult))
						{
							storageAsyncResult.Result = false;
							storageAsyncResult.OnComplete();
						}
						else
						{
							ICancellableAsyncResult object2 = this.BeginDelete(deleteSnapshotsOption, accessCondition, options, operationContext, delegate(IAsyncResult deleteResult)
							{
								storageAsyncResult.UpdateCompletedSynchronously(deleteResult.CompletedSynchronously);
								storageAsyncResult.CancelDelegate = null;
								try
								{
									this.EndDelete(deleteResult);
									storageAsyncResult.Result = true;
									storageAsyncResult.OnComplete();
								}
								catch (StorageException ex)
								{
									if (ex.RequestInformation.HttpStatusCode == 404)
									{
										if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.BlobNotFound)
										{
											storageAsyncResult.Result = false;
											storageAsyncResult.OnComplete();
										}
										else
										{
											storageAsyncResult.OnComplete(ex);
										}
									}
									else
									{
										storageAsyncResult.OnComplete(ex);
									}
								}
								catch (Exception exception2)
								{
									storageAsyncResult.OnComplete(exception2);
								}
							}, null);
							storageAsyncResult.CancelDelegate = new Action(object2.Cancel);
							if (storageAsyncResult.CancelRequested)
							{
								storageAsyncResult.Cancel();
							}
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008CD0 File Offset: 0x00006ED0
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = (StorageAsyncResult<bool>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008CF0 File Offset: 0x00006EF0
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008CFD File Offset: 0x00006EFD
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008D1D File Offset: 0x00006F1D
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(deleteSnapshotsOption, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00008D2F File Offset: 0x00006F2F
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<DeleteSnapshotsOption, AccessCondition, BlobRequestOptions, OperationContext, bool>(new Func<DeleteSnapshotsOption, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), deleteSnapshotsOption, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00008D58 File Offset: 0x00006F58
		[DoesServiceRequest]
		public string AcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.AcquireLeaseImpl(this.attributes, leaseTime, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008D92 File Offset: 0x00006F92
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AsyncCallback callback, object state)
		{
			return this.BeginAcquireLease(leaseTime, proposedLeaseId, null, null, null, callback, state);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008DA4 File Offset: 0x00006FA4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.AcquireLeaseImpl(this.attributes, leaseTime, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008DE2 File Offset: 0x00006FE2
		public string EndAcquireLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008DEA File Offset: 0x00006FEA
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId)
		{
			return this.AcquireLeaseAsync(leaseTime, proposedLeaseId, CancellationToken.None);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008DF9 File Offset: 0x00006FF9
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, string, string>(new Func<TimeSpan?, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAcquireLease), new Func<IAsyncResult, string>(this.EndAcquireLease), leaseTime, proposedLeaseId, cancellationToken);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008E1B File Offset: 0x0000701B
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AcquireLeaseAsync(leaseTime, proposedLeaseId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00008E2F File Offset: 0x0000702F
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, string, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<TimeSpan?, string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAcquireLease), new Func<IAsyncResult, string>(this.EndAcquireLease), leaseTime, proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008E58 File Offset: 0x00007058
		[DoesServiceRequest]
		public void RenewLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.RenewLeaseImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008E8F File Offset: 0x0000708F
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginRenewLease(accessCondition, null, null, callback, state);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008E9C File Offset: 0x0000709C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.RenewLeaseImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008ED6 File Offset: 0x000070D6
		public void EndRenewLease(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008EDF File Offset: 0x000070DF
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition)
		{
			return this.RenewLeaseAsync(accessCondition, CancellationToken.None);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008EED File Offset: 0x000070ED
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition>(new Func<AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginRenewLease), new Action<IAsyncResult>(this.EndRenewLease), accessCondition, cancellationToken);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008F0E File Offset: 0x0000710E
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.RenewLeaseAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00008F1E File Offset: 0x0000711E
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginRenewLease), new Action<IAsyncResult>(this.EndRenewLease), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00008F44 File Offset: 0x00007144
		[DoesServiceRequest]
		public string ChangeLease(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.ChangeLeaseImpl(this.attributes, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00008F7C File Offset: 0x0000717C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginChangeLease(proposedLeaseId, accessCondition, null, null, callback, state);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008F8C File Offset: 0x0000718C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.ChangeLeaseImpl(this.attributes, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00008FC8 File Offset: 0x000071C8
		public string EndChangeLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008FD0 File Offset: 0x000071D0
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition)
		{
			return this.ChangeLeaseAsync(proposedLeaseId, accessCondition, CancellationToken.None);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008FDF File Offset: 0x000071DF
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, AccessCondition, string>(new Func<string, AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginChangeLease), new Func<IAsyncResult, string>(this.EndChangeLease), proposedLeaseId, accessCondition, cancellationToken);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009001 File Offset: 0x00007201
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ChangeLeaseAsync(proposedLeaseId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009013 File Offset: 0x00007213
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginChangeLease), new Func<IAsyncResult, string>(this.EndChangeLease), proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000903C File Offset: 0x0000723C
		[DoesServiceRequest]
		public void ReleaseLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ReleaseLeaseImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009073 File Offset: 0x00007273
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginReleaseLease(accessCondition, null, null, callback, state);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009080 File Offset: 0x00007280
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ReleaseLeaseImpl(this.attributes, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000090BA File Offset: 0x000072BA
		public void EndReleaseLease(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000090C3 File Offset: 0x000072C3
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition)
		{
			return this.ReleaseLeaseAsync(accessCondition, CancellationToken.None);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000090D1 File Offset: 0x000072D1
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition>(new Func<AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginReleaseLease), new Action<IAsyncResult>(this.EndReleaseLease), accessCondition, cancellationToken);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000090F2 File Offset: 0x000072F2
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ReleaseLeaseAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009102 File Offset: 0x00007302
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginReleaseLease), new Action<IAsyncResult>(this.EndReleaseLease), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009128 File Offset: 0x00007328
		[DoesServiceRequest]
		public TimeSpan BreakLease(TimeSpan? breakPeriod = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<TimeSpan>(this.BreakLeaseImpl(this.attributes, breakPeriod, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00009160 File Offset: 0x00007360
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AsyncCallback callback, object state)
		{
			return this.BeginBreakLease(breakPeriod, null, null, null, callback, state);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009170 File Offset: 0x00007370
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<TimeSpan>(this.BreakLeaseImpl(this.attributes, breakPeriod, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000091AC File Offset: 0x000073AC
		public TimeSpan EndBreakLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TimeSpan>(asyncResult);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000091B4 File Offset: 0x000073B4
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod)
		{
			return this.BreakLeaseAsync(breakPeriod, CancellationToken.None);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000091C2 File Offset: 0x000073C2
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, TimeSpan>(new Func<TimeSpan?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginBreakLease), new Func<IAsyncResult, TimeSpan>(this.EndBreakLease), breakPeriod, cancellationToken);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000091E3 File Offset: 0x000073E3
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.BreakLeaseAsync(breakPeriod, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000091F5 File Offset: 0x000073F5
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, AccessCondition, BlobRequestOptions, OperationContext, TimeSpan>(new Func<TimeSpan?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginBreakLease), new Func<IAsyncResult, TimeSpan>(this.EndBreakLease), breakPeriod, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000921B File Offset: 0x0000741B
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of StartCopy.")]
		public string StartCopyFromBlob(Uri source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.StartCopy(source, sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000922A File Offset: 0x0000742A
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		public ICancellableAsyncResult BeginStartCopyFromBlob(Uri source, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(source, null, null, null, null, callback, state);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00009239 File Offset: 0x00007439
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopyFromBlob(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(source, sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000924C File Offset: 0x0000744C
		[Obsolete("Deprecated this method in favor of EndStartCopy.")]
		public string EndStartCopyFromBlob(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009254 File Offset: 0x00007454
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(Uri source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009262 File Offset: 0x00007462
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		public Task<string> StartCopyFromBlobAsync(Uri source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, string>(new Func<Uri, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009283 File Offset: 0x00007483
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009297 File Offset: 0x00007497
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<Uri, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000092C0 File Offset: 0x000074C0
		[DoesServiceRequest]
		public string StartCopy(Uri source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("source", source);
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.StartCopyImpl(this.attributes, source, sourceAccessCondition, destAccessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009310 File Offset: 0x00007510
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(Uri source, AsyncCallback callback, object state)
		{
			return this.BeginStartCopy(source, null, null, null, null, callback, state);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00009320 File Offset: 0x00007520
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("source", source);
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.StartCopyImpl(this.attributes, source, sourceAccessCondition, destAccessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00009374 File Offset: 0x00007574
		public string EndStartCopy(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000937C File Offset: 0x0000757C
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000938A File Offset: 0x0000758A
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, string>(new Func<Uri, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000093AB File Offset: 0x000075AB
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x000093BF File Offset: 0x000075BF
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<Uri, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(this.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000093E8 File Offset: 0x000075E8
		[DoesServiceRequest]
		public void AbortCopy(string copyId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.AbortCopyImpl(this.attributes, copyId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009421 File Offset: 0x00007621
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAbortCopy(string copyId, AsyncCallback callback, object state)
		{
			return this.BeginAbortCopy(copyId, null, null, null, callback, state);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009430 File Offset: 0x00007630
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAbortCopy(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.AbortCopyImpl(this.attributes, copyId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000946C File Offset: 0x0000766C
		public void EndAbortCopy(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009475 File Offset: 0x00007675
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId)
		{
			return this.AbortCopyAsync(copyId, CancellationToken.None);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00009483 File Offset: 0x00007683
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAbortCopy), new Action<IAsyncResult>(this.EndAbortCopy), copyId, cancellationToken);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000094A4 File Offset: 0x000076A4
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AbortCopyAsync(copyId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000094B6 File Offset: 0x000076B6
		[DoesServiceRequest]
		public Task AbortCopyAsync(string copyId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAbortCopy), new Action<IAsyncResult>(this.EndAbortCopy), copyId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000094DC File Offset: 0x000076DC
		[DoesServiceRequest]
		public CloudBlob Snapshot(IDictionary<string, string> metadata = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<CloudBlob>(this.SnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00009519 File Offset: 0x00007719
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSnapshot(AsyncCallback callback, object state)
		{
			return this.BeginSnapshot(null, null, null, null, callback, state);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009528 File Offset: 0x00007728
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSnapshot(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<CloudBlob>(this.SnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009569 File Offset: 0x00007769
		public CloudBlob EndSnapshot(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<CloudBlob>(asyncResult);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009571 File Offset: 0x00007771
		[DoesServiceRequest]
		public Task<CloudBlob> SnapshotAsync()
		{
			return this.SnapshotAsync(CancellationToken.None);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000957E File Offset: 0x0000777E
		[DoesServiceRequest]
		public Task<CloudBlob> SnapshotAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlob>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSnapshot), new Func<IAsyncResult, CloudBlob>(this.EndSnapshot), cancellationToken);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000959E File Offset: 0x0000779E
		[DoesServiceRequest]
		public Task<CloudBlob> SnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SnapshotAsync(metadata, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000095B0 File Offset: 0x000077B0
		[DoesServiceRequest]
		public Task<CloudBlob> SnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, CloudBlob>(new Func<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSnapshot), new Func<IAsyncResult, CloudBlob>(this.EndSnapshot), metadata, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000993C File Offset: 0x00007B3C
		private RESTCommand<NullType> GetBlobImpl(BlobAttributes blobAttributes, Stream destStream, long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options)
		{
			string lockedETag = null;
			AccessCondition lockedAccessCondition = null;
			bool isRangeGet = offset != null;
			int discardFirst = 0;
			long? endOffset = null;
			bool bufferIV = false;
			long? userSpecifiedLength = length;
			options.AssertPolicyIfRequired();
			if (isRangeGet && options.EncryptionPolicy != null)
			{
				if (length != null)
				{
					endOffset = new long?(offset.Value + length.Value - 1L);
					if ((endOffset.Value + 1L) % 16L != 0L)
					{
						endOffset += (long)((int)(16L - (endOffset.Value + 1L) % 16L));
					}
				}
				discardFirst = (int)(offset.Value % 16L);
				offset -= (long)discardFirst;
				if (offset > 15L)
				{
					offset -= 16L;
					bufferIV = true;
				}
				if (endOffset != null)
				{
					length = new long?(endOffset.Value - offset.Value + 1L);
				}
			}
			bool arePropertiesPopulated = false;
			bool decryptStreamCreated = false;
			ICryptoTransform transform = null;
			string storedMD5 = null;
			long startingOffset = (offset != null) ? offset.Value : 0L;
			long? startingLength = length;
			long? validateLength = null;
			RESTCommand<NullType> getCmd = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(getCmd);
			getCmd.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			getCmd.RetrieveResponseStream = true;
			getCmd.DestinationStream = destStream;
			getCmd.CalculateMd5ForResponseStream = !options.DisableContentMD5Validation.Value;
			getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Get(uri, serverTimeout, blobAttributes.SnapshotTime, offset, length, options.UseTransactionalMD5.Value, accessCondition, useVersionHeader, ctx));
			getCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			getCmd.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				if (lockedAccessCondition == null && !string.IsNullOrEmpty(lockedETag))
				{
					lockedAccessCondition = AccessCondition.GenerateIfMatchCondition(lockedETag);
					if (accessCondition != null)
					{
						lockedAccessCondition.LeaseId = accessCondition.LeaseId;
					}
				}
				if (cmd.StreamCopyState != null)
				{
					offset = new long?(startingOffset + cmd.StreamCopyState.Length);
					if (startingLength != null)
					{
						length = new long?(startingLength.Value - cmd.StreamCopyState.Length);
					}
				}
				getCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext context) => BlobHttpWebRequestFactory.Get(uri, serverTimeout, blobAttributes.SnapshotTime, offset, length, options.UseTransactionalMD5.Value && !arePropertiesPopulated, lockedAccessCondition ?? accessCondition, useVersionHeader, context));
			};
			getCmd.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>((offset != null) ? HttpStatusCode.PartialContent : HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				if (!arePropertiesPopulated)
				{
					CloudBlob.UpdateAfterFetchAttributes(blobAttributes, resp, isRangeGet);
					storedMD5 = resp.Headers[HttpResponseHeader.ContentMd5];
					if (options.EncryptionPolicy != null)
					{
						cmd.DestinationStream = BlobEncryptionPolicy.WrapUserStreamWithDecryptStream(this, cmd.DestinationStream, options, blobAttributes, isRangeGet, out transform, endOffset, userSpecifiedLength, discardFirst, bufferIV);
						decryptStreamCreated = true;
					}
					if (!options.DisableContentMD5Validation.Value && options.UseTransactionalMD5.Value && string.IsNullOrEmpty(storedMD5))
					{
						throw new StorageException(cmd.CurrentResult, "MD5 does not exist. If you do not want to force validation, please disable UseTransactionalMD5.", null)
						{
							IsRetryable = false
						};
					}
					getCmd.CommandLocationMode = ((cmd.CurrentResult.TargetLocation == StorageLocation.Primary) ? CommandLocationMode.PrimaryOnly : CommandLocationMode.SecondaryOnly);
					lockedETag = blobAttributes.Properties.ETag;
					if (resp.ContentLength >= 0L)
					{
						validateLength = new long?(resp.ContentLength);
					}
					arePropertiesPopulated = true;
				}
				return NullType.Value;
			};
			getCmd.PostProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				HttpResponseParsers.ValidateResponseStreamMd5AndLength<NullType>(validateLength, storedMD5, cmd);
				return NullType.Value;
			};
			getCmd.DisposeAction = delegate(RESTCommand<NullType> cmd)
			{
				if (decryptStreamCreated)
				{
					try
					{
						cmd.DestinationStream.Close();
						if (transform != null)
						{
							transform.Dispose();
						}
					}
					catch (Exception inner)
					{
						throw new StorageException(cmd.CurrentResult, "Cryptographic error occurred. Please check the inner exception for more details.", inner)
						{
							IsRetryable = false
						};
					}
				}
			};
			return getCmd;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009D18 File Offset: 0x00007F18
		private RESTCommand<NullType> FetchAttributesImpl(BlobAttributes blobAttributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetProperties(uri, serverTimeout, blobAttributes.SnapshotTime, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateAfterFetchAttributes(blobAttributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009DF4 File Offset: 0x00007FF4
		private RESTCommand<bool> ExistsImpl(BlobAttributes blobAttributes, BlobRequestOptions options, bool primaryOnly)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = (primaryOnly ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetProperties(uri, serverTimeout, blobAttributes.SnapshotTime, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				CloudBlob.UpdateAfterFetchAttributes(blobAttributes, resp, false);
				return true;
			};
			return restcommand;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009ED8 File Offset: 0x000080D8
		private RESTCommand<NullType> SetMetadataImpl(BlobAttributes blobAttributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, blobAttributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009FD0 File Offset: 0x000081D0
		private RESTCommand<NullType> SetPropertiesImpl(BlobAttributes blobAttributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.SetProperties(uri, serverTimeout, blobAttributes.Properties, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, blobAttributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000A0A8 File Offset: 0x000082A8
		private RESTCommand<NullType> DeleteBlobImpl(BlobAttributes blobAttributes, DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Delete(uri, serverTimeout, blobAttributes.SnapshotTime, deleteSnapshotsOption, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000A1B0 File Offset: 0x000083B0
		private RESTCommand<string> AcquireLeaseImpl(BlobAttributes blobAttributes, TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int leaseDuration = -1;
			if (leaseTime != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("leaseTime", leaseTime.Value, TimeSpan.FromSeconds(1.0), TimeSpan.MaxValue);
				leaseDuration = (int)leaseTime.Value.TotalSeconds;
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Acquire, proposedLeaseId, new int?(leaseDuration), null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Created, resp, null, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000A2F0 File Offset: 0x000084F0
		private RESTCommand<NullType> RenewLeaseImpl(BlobAttributes blobAttributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when renewing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Renew, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000A408 File Offset: 0x00008608
		private RESTCommand<string> ChangeLeaseImpl(BlobAttributes blobAttributes, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			CommonUtility.AssertNotNull("proposedLeaseId", proposedLeaseId);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when changing a lease.", "accessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Change, proposedLeaseId, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.OK, resp, null, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000A538 File Offset: 0x00008738
		private RESTCommand<NullType> ReleaseLeaseImpl(BlobAttributes blobAttributes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when releasing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Release, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000A680 File Offset: 0x00008880
		private RESTCommand<TimeSpan> BreakLeaseImpl(BlobAttributes blobAttributes, TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int? breakSeconds = null;
			if (breakPeriod != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("breakPeriod", breakPeriod.Value, TimeSpan.Zero, TimeSpan.MaxValue);
				breakSeconds = new int?((int)breakPeriod.Value.TotalSeconds);
			}
			RESTCommand<TimeSpan> restcommand = new RESTCommand<TimeSpan>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<TimeSpan>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Break, null, null, breakSeconds, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<TimeSpan> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<TimeSpan>(HttpStatusCode.Accepted, resp, TimeSpan.Zero, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				int? remainingLeaseTime = BlobHttpResponseParsers.GetRemainingLeaseTime(resp);
				if (remainingLeaseTime == null)
				{
					throw new StorageException(cmd.CurrentResult, "Valid lease time expected but not received from the service.", null);
				}
				return TimeSpan.FromSeconds((double)remainingLeaseTime.Value);
			};
			return restcommand;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000A7D8 File Offset: 0x000089D8
		private RESTCommand<string> StartCopyImpl(BlobAttributes blobAttributes, Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options)
		{
			if (sourceAccessCondition != null && !string.IsNullOrEmpty(sourceAccessCondition.LeaseId))
			{
				throw new ArgumentException("A lease condition cannot be specified on the source of a copy.", "sourceAccessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.CopyFrom(uri, serverTimeout, source, sourceAccessCondition, destAccessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, blobAttributes.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Accepted, resp, null, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(blobAttributes, resp, false);
				CopyState copyAttributes = BlobHttpResponseParsers.GetCopyAttributes(resp);
				blobAttributes.CopyState = copyAttributes;
				return copyAttributes.CopyId;
			};
			return restcommand;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		private RESTCommand<NullType> AbortCopyImpl(BlobAttributes blobAttributes, string copyId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("copyId", copyId);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, blobAttributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.AbortCopy(uri, serverTimeout, copyId, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000AA4C File Offset: 0x00008C4C
		private RESTCommand<CloudBlob> SnapshotImpl(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<CloudBlob> restcommand = new RESTCommand<CloudBlob>(this.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<CloudBlob>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Snapshot(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (metadata != null)
				{
					BlobHttpWebRequestFactory.AddMetadata(r, metadata);
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<CloudBlob> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<CloudBlob>(HttpStatusCode.Created, resp, null, cmd, ex);
				DateTimeOffset value = NavigationHelper.ParseSnapshotTime(BlobHttpResponseParsers.GetSnapshotTime(resp));
				CloudBlob cloudBlob = new CloudBlob(this.Name, new DateTimeOffset?(value), this.Container);
				cloudBlob.attributes.Metadata = new Dictionary<string, string>(metadata ?? this.Metadata);
				cloudBlob.attributes.Properties = new BlobProperties(this.Properties);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(cloudBlob.attributes, resp, false);
				return cloudBlob;
			};
			return restcommand;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000AAEC File Offset: 0x00008CEC
		internal static void BlobOutputStreamCommitCallback(IAsyncResult result)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)result.AsyncState;
			CloudBlobStream cloudBlobStream = (CloudBlobStream)storageAsyncResult.OperationState;
			storageAsyncResult.UpdateCompletedSynchronously(result.CompletedSynchronously);
			try
			{
				cloudBlobStream.EndCommit(result);
				cloudBlobStream.Dispose();
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000AB4C File Offset: 0x00008D4C
		internal static void UpdateAfterFetchAttributes(BlobAttributes blobAttributes, HttpWebResponse response, bool ignoreMD5)
		{
			BlobProperties properties = BlobHttpResponseParsers.GetProperties(response);
			if (blobAttributes.Properties.BlobType != BlobType.Unspecified && blobAttributes.Properties.BlobType != properties.BlobType)
			{
				throw new InvalidOperationException("Blob type of the blob reference doesn't match blob type of the blob.");
			}
			if (ignoreMD5)
			{
				properties.ContentMD5 = blobAttributes.Properties.ContentMD5;
			}
			blobAttributes.Properties = properties;
			blobAttributes.Metadata = BlobHttpResponseParsers.GetMetadata(response);
			blobAttributes.CopyState = BlobHttpResponseParsers.GetCopyAttributes(response);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000ABC0 File Offset: 0x00008DC0
		internal static void UpdateETagLMTLengthAndSequenceNumber(BlobAttributes blobAttributes, HttpWebResponse response, bool updateLength)
		{
			BlobProperties properties = BlobHttpResponseParsers.GetProperties(response);
			blobAttributes.Properties.ETag = (properties.ETag ?? blobAttributes.Properties.ETag);
			BlobProperties properties2 = blobAttributes.Properties;
			DateTimeOffset? lastModified = properties.LastModified;
			properties2.LastModified = ((lastModified != null) ? new DateTimeOffset?(lastModified.GetValueOrDefault()) : blobAttributes.Properties.LastModified);
			BlobProperties properties3 = blobAttributes.Properties;
			long? pageBlobSequenceNumber = properties.PageBlobSequenceNumber;
			properties3.PageBlobSequenceNumber = ((pageBlobSequenceNumber != null) ? new long?(pageBlobSequenceNumber.GetValueOrDefault()) : blobAttributes.Properties.PageBlobSequenceNumber);
			BlobProperties properties4 = blobAttributes.Properties;
			int? appendBlobCommittedBlockCount = properties.AppendBlobCommittedBlockCount;
			properties4.AppendBlobCommittedBlockCount = ((appendBlobCommittedBlockCount != null) ? new int?(appendBlobCommittedBlockCount.GetValueOrDefault()) : blobAttributes.Properties.AppendBlobCommittedBlockCount);
			if (updateLength)
			{
				blobAttributes.Properties.Length = properties.Length;
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000ACA4 File Offset: 0x00008EA4
		internal static Uri SourceBlobToUri(CloudBlob source)
		{
			CommonUtility.AssertNotNull("source", source);
			return source.ServiceClient.Credentials.TransformUri(source.SnapshotQualifiedUri);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000ACC7 File Offset: 0x00008EC7
		public CloudBlob(Uri blobAbsoluteUri) : this(blobAbsoluteUri, null)
		{
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000ACD4 File Offset: 0x00008ED4
		public CloudBlob(Uri blobAbsoluteUri, StorageCredentials credentials) : this(blobAbsoluteUri, null, credentials)
		{
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000ACF2 File Offset: 0x00008EF2
		public CloudBlob(Uri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials) : this(new StorageUri(blobAbsoluteUri), snapshotTime, credentials)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000AD04 File Offset: 0x00008F04
		public CloudBlob(StorageUri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials)
		{
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			CommonUtility.AssertNotNull("blobAbsoluteUri", blobAbsoluteUri);
			CommonUtility.AssertNotNull("blobAbsoluteUri", blobAbsoluteUri.PrimaryUri);
			this.attributes = new BlobAttributes();
			this.SnapshotTime = snapshotTime;
			this.ParseQueryAndVerify(blobAbsoluteUri, credentials);
			this.Properties.BlobType = BlobType.Unspecified;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000AD64 File Offset: 0x00008F64
		internal CloudBlob(string blobName, DateTimeOffset? snapshotTime, CloudBlobContainer container)
		{
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			CommonUtility.AssertNotNull("container", container);
			this.attributes = new BlobAttributes();
			this.attributes.StorageUri = NavigationHelper.AppendPathToUri(container.StorageUri, blobName);
			this.Name = blobName;
			this.ServiceClient = container.ServiceClient;
			this.container = container;
			this.SnapshotTime = snapshotTime;
			this.Properties.BlobType = BlobType.Unspecified;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000ADE8 File Offset: 0x00008FE8
		internal CloudBlob(BlobAttributes attributes, CloudBlobClient serviceClient)
		{
			this.streamMinimumReadSizeInBytes = 4194304;
			base..ctor();
			this.attributes = attributes;
			this.ServiceClient = serviceClient;
			this.ParseQueryAndVerify(this.StorageUri, this.ServiceClient.Credentials);
			this.Properties.BlobType = BlobType.Unspecified;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000AE37 File Offset: 0x00009037
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000AE3F File Offset: 0x0000903F
		public CloudBlobClient ServiceClient { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000AE48 File Offset: 0x00009048
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000AE50 File Offset: 0x00009050
		public int StreamMinimumReadSizeInBytes
		{
			get
			{
				return this.streamMinimumReadSizeInBytes;
			}
			set
			{
				CommonUtility.AssertInBounds<long>("StreamMinimumReadSizeInBytes", (long)value, 16384L);
				this.streamMinimumReadSizeInBytes = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000AE6B File Offset: 0x0000906B
		public BlobProperties Properties
		{
			get
			{
				return this.attributes.Properties;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000AE78 File Offset: 0x00009078
		public IDictionary<string, string> Metadata
		{
			get
			{
				return this.attributes.Metadata;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000AE85 File Offset: 0x00009085
		public Uri Uri
		{
			get
			{
				return this.attributes.Uri;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000AE92 File Offset: 0x00009092
		public StorageUri StorageUri
		{
			get
			{
				return this.attributes.StorageUri;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000AE9F File Offset: 0x0000909F
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000AEAC File Offset: 0x000090AC
		public DateTimeOffset? SnapshotTime
		{
			get
			{
				return this.attributes.SnapshotTime;
			}
			private set
			{
				this.attributes.SnapshotTime = value;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000AEBC File Offset: 0x000090BC
		public bool IsSnapshot
		{
			get
			{
				return this.SnapshotTime != null;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000AED8 File Offset: 0x000090D8
		public Uri SnapshotQualifiedUri
		{
			get
			{
				if (this.SnapshotTime != null)
				{
					UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
					uriQueryBuilder.Add("snapshot", Request.ConvertDateTimeToSnapshotString(this.SnapshotTime.Value));
					return uriQueryBuilder.AddToUri(this.Uri);
				}
				return this.Uri;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000AF2C File Offset: 0x0000912C
		public StorageUri SnapshotQualifiedStorageUri
		{
			get
			{
				if (this.SnapshotTime != null)
				{
					UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
					uriQueryBuilder.Add("snapshot", Request.ConvertDateTimeToSnapshotString(this.SnapshotTime.Value));
					return uriQueryBuilder.AddToUri(this.StorageUri);
				}
				return this.StorageUri;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000AF80 File Offset: 0x00009180
		public CopyState CopyState
		{
			get
			{
				return this.attributes.CopyState;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000AF8D File Offset: 0x0000918D
		// (set) Token: 0x0600025D RID: 605 RVA: 0x0000AF95 File Offset: 0x00009195
		public string Name { get; private set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000AF9E File Offset: 0x0000919E
		public CloudBlobContainer Container
		{
			get
			{
				if (this.container == null)
				{
					this.container = this.ServiceClient.GetContainerReference(NavigationHelper.GetContainerName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris)));
				}
				return this.container;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000AFDC File Offset: 0x000091DC
		public CloudBlobDirectory Parent
		{
			get
			{
				string prefix;
				StorageUri uri;
				if (this.parent == null && NavigationHelper.GetBlobParentNameAndAddress(this.StorageUri, this.ServiceClient.DefaultDelimiter, new bool?(this.ServiceClient.UsePathStyleUris), out prefix, out uri))
				{
					this.parent = new CloudBlobDirectory(uri, prefix, this.Container);
				}
				return this.parent;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000B036 File Offset: 0x00009236
		// (set) Token: 0x06000261 RID: 609 RVA: 0x0000B043 File Offset: 0x00009243
		public BlobType BlobType
		{
			get
			{
				return this.Properties.BlobType;
			}
			internal set
			{
				this.Properties.BlobType = value;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B051 File Offset: 0x00009251
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null, null);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B05C File Offset: 0x0000925C
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier)
		{
			return this.GetSharedAccessSignature(policy, null, groupPolicyIdentifier);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000B067 File Offset: 0x00009267
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers)
		{
			return this.GetSharedAccessSignature(policy, headers, null);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000B074 File Offset: 0x00009274
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string groupPolicyIdentifier)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string canonicalName = this.GetCanonicalName(true, "2015-02-21");
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, headers, groupPolicyIdentifier, canonicalName, "2015-02-21", key.KeyValue);
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, headers, groupPolicyIdentifier, "b", hash, key.KeyName, "2015-02-21");
			return signature.ToString();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000B108 File Offset: 0x00009308
		[Obsolete("This overload has been deprecated because the SAS tokens generated using the current version work fine with old libraries. Please use the other overloads.")]
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, SharedAccessBlobHeaders headers, string groupPolicyIdentifier, string sasVersion)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string sasVersion2 = SharedAccessSignatureHelper.ValidateSASVersionString(sasVersion);
			string canonicalName = this.GetCanonicalName(true, sasVersion2);
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, headers, groupPolicyIdentifier, canonicalName, sasVersion2, key.KeyValue);
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, headers, groupPolicyIdentifier, "b", hash, key.KeyName, sasVersion2);
			return signature.ToString();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000B19C File Offset: 0x0000939C
		private string GetCanonicalName(bool ignoreSnapshotTime, string sasVersion)
		{
			string accountName = this.ServiceClient.Credentials.AccountName;
			string name = this.Container.Name;
			string text = this.Name.Replace('\\', '/');
			string format = "/{0}/{1}/{2}/{3}";
			if (sasVersion == "2012-02-12" || sasVersion == "2013-08-15")
			{
				format = "/{1}/{2}/{3}";
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				"blob",
				accountName,
				name,
				text
			});
			if (!ignoreSnapshotTime && this.SnapshotTime != null)
			{
				text2 = text2 + "?snapshot=" + Request.ConvertDateTimeToSnapshotString(this.SnapshotTime.Value);
			}
			return text2;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000B264 File Offset: 0x00009464
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			DateTimeOffset? dateTimeOffset;
			this.attributes.StorageUri = NavigationHelper.ParseBlobQueryAndVerify(address, out storageCredentials, out dateTimeOffset);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			if (dateTimeOffset != null && this.SnapshotTime != null && !dateTimeOffset.Value.Equals(this.SnapshotTime.Value))
			{
				string message2 = string.Format(CultureInfo.CurrentCulture, "Multiple different snapshot times provided as part of query '{0}' and as constructor parameter '{1}'.", new object[]
				{
					dateTimeOffset,
					this.SnapshotTime
				});
				throw new ArgumentException(message2);
			}
			if (dateTimeOffset != null)
			{
				this.SnapshotTime = dateTimeOffset;
			}
			if (this.ServiceClient == null)
			{
				this.ServiceClient = new CloudBlobClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			}
			this.Name = NavigationHelper.GetBlobName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x0400009F RID: 159
		private int streamMinimumReadSizeInBytes;

		// Token: 0x040000A0 RID: 160
		private CloudBlobContainer container;

		// Token: 0x040000A1 RID: 161
		private CloudBlobDirectory parent;

		// Token: 0x040000A2 RID: 162
		internal readonly BlobAttributes attributes;
	}
}
