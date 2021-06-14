using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200001E RID: 30
	public sealed class CloudPageBlob : CloudBlob, ICloudBlob, IListBlobItem
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x00014EE4 File Offset: 0x000130E4
		[DoesServiceRequest]
		public CloudBlobStream OpenWrite(long? size, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			bool flag = size != null;
			ICryptoTransform transform = null;
			blobRequestOptions.AssertPolicyIfRequired();
			if (blobRequestOptions.EncryptionPolicy != null)
			{
				transform = options.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, true);
			}
			if (flag)
			{
				this.Create(size.Value, accessCondition, options, operationContext);
			}
			else
			{
				if (blobRequestOptions.StoreBlobContentMD5.Value)
				{
					throw new ArgumentException("MD5 cannot be calculated for an existing blob because it would require reading the existing data. Please disable StoreBlobContentMD5.");
				}
				if (blobRequestOptions.EncryptionPolicy != null)
				{
					throw new ArgumentException("Encryption is not supported for a blob that already exists. Please do not specify an encryption policy.");
				}
				base.FetchAttributes(accessCondition, options, operationContext);
				size = new long?(base.Properties.Length);
			}
			if (accessCondition != null)
			{
				accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
			}
			if (blobRequestOptions.EncryptionPolicy != null)
			{
				return new BlobEncryptedWriteStream(this, size.Value, flag, accessCondition, blobRequestOptions, operationContext, transform);
			}
			return new BlobWriteStream(this, size.Value, flag, accessCondition, blobRequestOptions, operationContext);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00014FD9 File Offset: 0x000131D9
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(long? size, AsyncCallback callback, object state)
		{
			return this.BeginOpenWrite(size, null, null, null, callback, state);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0001522C File Offset: 0x0001342C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(long? size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			bool createNew = size != null;
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = new StorageAsyncResult<CloudBlobStream>(callback, state);
			modifiedOptions.AssertPolicyIfRequired();
			ICancellableAsyncResult @object;
			if (createNew)
			{
				ICryptoTransform transform = null;
				if (options != null && options.EncryptionPolicy != null)
				{
					transform = options.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, true);
				}
				@object = this.BeginCreate(size.Value, accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndCreate(ar);
						if (accessCondition != null)
						{
							accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
						}
						if (modifiedOptions.EncryptionPolicy != null)
						{
							storageAsyncResult.Result = new BlobEncryptedWriteStream(this, this.Properties.Length, createNew, accessCondition, modifiedOptions, operationContext, transform);
						}
						else
						{
							storageAsyncResult.Result = new BlobWriteStream(this, this.Properties.Length, createNew, accessCondition, modifiedOptions, operationContext);
						}
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
			}
			else
			{
				if (modifiedOptions.StoreBlobContentMD5.Value)
				{
					throw new ArgumentException("MD5 cannot be calculated for an existing blob because it would require reading the existing data. Please disable StoreBlobContentMD5.");
				}
				if (modifiedOptions.EncryptionPolicy != null)
				{
					throw new ArgumentException("Encryption is not supported for a blob that already exists. Please do not specify an encryption policy.");
				}
				@object = base.BeginFetchAttributes(accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndFetchAttributes(ar);
						if (accessCondition != null)
						{
							accessCondition = AccessCondition.GenerateLeaseCondition(accessCondition.LeaseId);
						}
						storageAsyncResult.Result = new BlobWriteStream(this, this.Properties.Length, createNew, accessCondition, modifiedOptions, operationContext);
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
			}
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00015388 File Offset: 0x00013588
		public CloudBlobStream EndOpenWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = (StorageAsyncResult<CloudBlobStream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000153A8 File Offset: 0x000135A8
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(long? size)
		{
			return this.OpenWriteAsync(size, CancellationToken.None);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000153B6 File Offset: 0x000135B6
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(long? size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, CloudBlobStream>(new Func<long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), size, cancellationToken);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000153D7 File Offset: 0x000135D7
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(long? size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.OpenWriteAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000153E9 File Offset: 0x000135E9
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(long? size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, AccessCondition, BlobRequestOptions, OperationContext, CloudBlobStream>(new Func<long?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00015410 File Offset: 0x00013610
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, null, accessCondition, options, operationContext);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015431 File Offset: 0x00013631
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00015448 File Offset: 0x00013648
		internal void UploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("source", source);
			if (!source.CanSeek)
			{
				throw new InvalidOperationException();
			}
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L, source.Length - source.Position);
			}
			else
			{
				length = new long?(source.Length - source.Position);
			}
			if (length % 512L != 0L)
			{
				throw new ArgumentException("Page data must be a multiple of 512 bytes.", "source");
			}
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			using (CloudBlobStream cloudBlobStream = this.OpenWrite(length, accessCondition, options2, operationContext))
			{
				using (ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(options2))
				{
					source.WriteToSync(cloudBlobStream, length, null, false, true, executionState, null);
					cloudBlobStream.Commit();
				}
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00015590 File Offset: 0x00013790
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, null, null, null, callback, state);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000155B4 File Offset: 0x000137B4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000155D9 File Offset: 0x000137D9
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), null, null, null, callback, state);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000155EE File Offset: 0x000137EE
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0001583C File Offset: 0x00013A3C
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginUploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("source", source);
			if (!source.CanSeek)
			{
				throw new InvalidOperationException();
			}
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L, source.Length - source.Position);
			}
			else
			{
				length = new long?(source.Length - source.Position);
			}
			if (length % 512L != 0L)
			{
				throw new ArgumentException("Page data must be a multiple of 512 bytes.", "source");
			}
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			ExecutionState<NullType> tempExecutionState = CommonUtility.CreateTemporaryExecutionState(options2);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult @object = this.BeginOpenWrite(length, accessCondition, options2, operationContext, delegate(IAsyncResult ar)
			{
				storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
				lock (storageAsyncResult.CancellationLockerObject)
				{
					storageAsyncResult.CancelDelegate = null;
					try
					{
						CloudBlobStream blobStream = this.EndOpenWrite(ar);
						storageAsyncResult.OperationState = blobStream;
						source.WriteToAsync(blobStream, length, null, false, tempExecutionState, null, delegate(ExecutionState<NullType> completedState)
						{
							storageAsyncResult.UpdateCompletedSynchronously(completedState.CompletedSynchronously);
							if (completedState.ExceptionRef != null)
							{
								storageAsyncResult.OnComplete(completedState.ExceptionRef);
								return;
							}
							try
							{
								lock (storageAsyncResult.CancellationLockerObject)
								{
									storageAsyncResult.CancelDelegate = null;
									ICancellableAsyncResult object2 = blobStream.BeginCommit(new AsyncCallback(CloudBlob.BlobOutputStreamCommitCallback), storageAsyncResult);
									storageAsyncResult.CancelDelegate = new Action(object2.Cancel);
									if (storageAsyncResult.CancelRequested)
									{
										storageAsyncResult.Cancel();
									}
								}
							}
							catch (Exception exception2)
							{
								storageAsyncResult.OnComplete(exception2);
							}
						});
						storageAsyncResult.CancelDelegate = new Action(tempExecutionState.Cancel);
						if (storageAsyncResult.CancelRequested)
						{
							storageAsyncResult.Cancel();
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000159B8 File Offset: 0x00013BB8
		public void EndUploadFromStream(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000159D2 File Offset: 0x00013BD2
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source)
		{
			return this.UploadFromStreamAsync(source, CancellationToken.None);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000159E0 File Offset: 0x00013BE0
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, cancellationToken);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00015A01 File Offset: 0x00013C01
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015A13 File Offset: 0x00013C13
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00015A39 File Offset: 0x00013C39
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length)
		{
			return this.UploadFromStreamAsync(source, length, CancellationToken.None);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00015A48 File Offset: 0x00013C48
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long>(new Func<Stream, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, cancellationToken);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00015A6A File Offset: 0x00013C6A
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00015A7E File Offset: 0x00013C7E
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00015AA8 File Offset: 0x00013CA8
		[DoesServiceRequest]
		public void UploadFromFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			using (FileStream fileStream = new FileStream(path, mode, FileAccess.Read))
			{
				this.UploadFromStream(fileStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00015AF4 File Offset: 0x00013CF4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00015B04 File Offset: 0x00013D04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("path", path);
			FileStream fileStream = new FileStream(path, mode, FileAccess.Read);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state)
			{
				OperationState = fileStream
			};
			ICancellableAsyncResult result;
			try
			{
				ICancellableAsyncResult @object = this.BeginUploadFromStream(fileStream, accessCondition, options, operationContext, new AsyncCallback(this.UploadFromFileCallback), storageAsyncResult);
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

		// Token: 0x060005D6 RID: 1494 RVA: 0x00015B88 File Offset: 0x00013D88
		private void UploadFromFileCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult.AsyncState;
			Exception exception = null;
			try
			{
				this.EndUploadFromStream(asyncResult);
			}
			catch (Exception ex)
			{
				exception = ex;
			}
			try
			{
				FileStream fileStream = (FileStream)storageAsyncResult.OperationState;
				fileStream.Dispose();
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			storageAsyncResult.OnComplete(exception);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00015BF0 File Offset: 0x00013DF0
		public void EndUploadFromFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00015C0A File Offset: 0x00013E0A
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode)
		{
			return this.UploadFromFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00015C19 File Offset: 0x00013E19
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, cancellationToken);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00015C3B File Offset: 0x00013E3B
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00015C4F File Offset: 0x00013E4F
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00015C78 File Offset: 0x00013E78
		[DoesServiceRequest]
		public void UploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(buffer, index, count))
			{
				this.UploadFromStream(syncMemoryStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00015CC4 File Offset: 0x00013EC4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromByteArray(buffer, index, count, null, null, null, callback, state);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00015CE4 File Offset: 0x00013EE4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			SyncMemoryStream source = new SyncMemoryStream(buffer, index, count);
			return this.BeginUploadFromStream(source, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00015D16 File Offset: 0x00013F16
		public void EndUploadFromByteArray(IAsyncResult asyncResult)
		{
			this.EndUploadFromStream(asyncResult);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00015D1F File Offset: 0x00013F1F
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00015D2F File Offset: 0x00013F2F
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, cancellationToken);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00015D53 File Offset: 0x00013F53
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00015D6C File Offset: 0x00013F6C
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext>(new Func<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00015DA4 File Offset: 0x00013FA4
		[DoesServiceRequest]
		public void Create(long size, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateImpl(size, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00015DE2 File Offset: 0x00013FE2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(long size, AsyncCallback callback, object state)
		{
			return this.BeginCreate(size, null, null, null, callback, state);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00015DF0 File Offset: 0x00013FF0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateImpl(size, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00015E26 File Offset: 0x00014026
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00015E2F File Offset: 0x0001402F
		[DoesServiceRequest]
		public Task CreateAsync(long size)
		{
			return this.CreateAsync(size, CancellationToken.None);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00015E3D File Offset: 0x0001403D
		[DoesServiceRequest]
		public Task CreateAsync(long size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long>(new Func<long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), size, cancellationToken);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00015E5E File Offset: 0x0001405E
		[DoesServiceRequest]
		public Task CreateAsync(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00015E70 File Offset: 0x00014070
		[DoesServiceRequest]
		public Task CreateAsync(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00015E98 File Offset: 0x00014098
		[DoesServiceRequest]
		public void Resize(long size, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ResizeImpl(size, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00015ED6 File Offset: 0x000140D6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginResize(long size, AsyncCallback callback, object state)
		{
			return this.BeginResize(size, null, null, null, callback, state);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00015EE4 File Offset: 0x000140E4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginResize(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ResizeImpl(size, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00015F1A File Offset: 0x0001411A
		public void EndResize(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00015F23 File Offset: 0x00014123
		[DoesServiceRequest]
		public Task ResizeAsync(long size)
		{
			return this.ResizeAsync(size, CancellationToken.None);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00015F31 File Offset: 0x00014131
		[DoesServiceRequest]
		public Task ResizeAsync(long size, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long>(new Func<long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginResize), new Action<IAsyncResult>(this.EndResize), size, cancellationToken);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00015F52 File Offset: 0x00014152
		[DoesServiceRequest]
		public Task ResizeAsync(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ResizeAsync(size, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00015F64 File Offset: 0x00014164
		[DoesServiceRequest]
		public Task ResizeAsync(long size, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginResize), new Action<IAsyncResult>(this.EndResize), size, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00015F8C File Offset: 0x0001418C
		[DoesServiceRequest]
		public void SetSequenceNumber(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetSequenceNumberImpl(sequenceNumberAction, sequenceNumber, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00015FCC File Offset: 0x000141CC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetSequenceNumber(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AsyncCallback callback, object state)
		{
			return this.BeginSetSequenceNumber(sequenceNumberAction, sequenceNumber, null, null, null, callback, state);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00015FDC File Offset: 0x000141DC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetSequenceNumber(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetSequenceNumberImpl(sequenceNumberAction, sequenceNumber, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00016014 File Offset: 0x00014214
		public void EndSetSequenceNumber(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001601D File Offset: 0x0001421D
		[DoesServiceRequest]
		public Task SetSequenceNumberAsync(SequenceNumberAction sequenceNumberAction, long? sequenceNumber)
		{
			return this.SetSequenceNumberAsync(sequenceNumberAction, sequenceNumber, CancellationToken.None);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0001602C File Offset: 0x0001422C
		[DoesServiceRequest]
		public Task SetSequenceNumberAsync(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<SequenceNumberAction, long?>(new Func<SequenceNumberAction, long?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetSequenceNumber), new Action<IAsyncResult>(this.EndSetSequenceNumber), sequenceNumberAction, sequenceNumber, cancellationToken);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0001604E File Offset: 0x0001424E
		[DoesServiceRequest]
		public Task SetSequenceNumberAsync(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SetSequenceNumberAsync(sequenceNumberAction, sequenceNumber, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00016062 File Offset: 0x00014262
		[DoesServiceRequest]
		public Task SetSequenceNumberAsync(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<SequenceNumberAction, long?, AccessCondition, BlobRequestOptions, OperationContext>(new Func<SequenceNumberAction, long?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetSequenceNumber), new Action<IAsyncResult>(this.EndSetSequenceNumber), sequenceNumberAction, sequenceNumber, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0001608C File Offset: 0x0001428C
		[DoesServiceRequest]
		public IEnumerable<PageRange> GetPageRanges(long? offset = null, long? length = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.ExecuteSync<IEnumerable<PageRange>>(this.GetPageRangesImpl(offset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000160C0 File Offset: 0x000142C0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPageRanges(AsyncCallback callback, object state)
		{
			return this.BeginGetPageRanges(null, null, null, null, null, callback, state);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000160EC File Offset: 0x000142EC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPageRanges(long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<IEnumerable<PageRange>>(this.GetPageRangesImpl(offset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00016124 File Offset: 0x00014324
		public IEnumerable<PageRange> EndGetPageRanges(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IEnumerable<PageRange>>(asyncResult);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001612C File Offset: 0x0001432C
		[DoesServiceRequest]
		public Task<IEnumerable<PageRange>> GetPageRangesAsync()
		{
			return this.GetPageRangesAsync(CancellationToken.None);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00016139 File Offset: 0x00014339
		[DoesServiceRequest]
		public Task<IEnumerable<PageRange>> GetPageRangesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IEnumerable<PageRange>>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPageRanges), new Func<IAsyncResult, IEnumerable<PageRange>>(this.EndGetPageRanges), cancellationToken);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00016159 File Offset: 0x00014359
		[DoesServiceRequest]
		public Task<IEnumerable<PageRange>> GetPageRangesAsync(long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.GetPageRangesAsync(offset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001616D File Offset: 0x0001436D
		[DoesServiceRequest]
		public Task<IEnumerable<PageRange>> GetPageRangesAsync(long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<long?, long?, AccessCondition, BlobRequestOptions, OperationContext, IEnumerable<PageRange>>(new Func<long?, long?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPageRanges), new Func<IAsyncResult, IEnumerable<PageRange>>(this.EndGetPageRanges), offset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00016198 File Offset: 0x00014398
		[DoesServiceRequest]
		public CloudPageBlob CreateSnapshot(IDictionary<string, string> metadata = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.ExecuteSync<CloudPageBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000161D5 File Offset: 0x000143D5
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(AsyncCallback callback, object state)
		{
			return this.BeginCreateSnapshot(null, null, null, null, callback, state);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x000161E4 File Offset: 0x000143E4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<CloudPageBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00016225 File Offset: 0x00014425
		public CloudPageBlob EndCreateSnapshot(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<CloudPageBlob>(asyncResult);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001622D File Offset: 0x0001442D
		[DoesServiceRequest]
		public Task<CloudPageBlob> CreateSnapshotAsync()
		{
			return this.CreateSnapshotAsync(CancellationToken.None);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001623A File Offset: 0x0001443A
		[DoesServiceRequest]
		public Task<CloudPageBlob> CreateSnapshotAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudPageBlob>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudPageBlob>(this.EndCreateSnapshot), cancellationToken);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001625A File Offset: 0x0001445A
		[DoesServiceRequest]
		public Task<CloudPageBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateSnapshotAsync(metadata, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001626C File Offset: 0x0001446C
		[DoesServiceRequest]
		public Task<CloudPageBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, CloudPageBlob>(new Func<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudPageBlob>(this.EndCreateSnapshot), metadata, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00016294 File Offset: 0x00014494
		[DoesServiceRequest]
		public void WritePages(Stream pageData, long startOffset, string contentMD5 = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("pageData", pageData);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			bool flag = contentMD5 == null && blobRequestOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			Stream stream = pageData;
			bool flag2 = false;
			try
			{
				if (!pageData.CanSeek || flag)
				{
					ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(blobRequestOptions);
					Stream toStream;
					if (pageData.CanSeek)
					{
						toStream = Stream.Null;
					}
					else
					{
						stream = new MultiBufferMemoryStream(base.ServiceClient.BufferManager, 65536);
						flag2 = true;
						toStream = stream;
					}
					long position = stream.Position;
					StreamDescriptor streamDescriptor = new StreamDescriptor();
					pageData.WriteToSync(toStream, null, new long?(4194304L), flag, true, executionState, streamDescriptor);
					stream.Position = position;
					if (flag)
					{
						contentMD5 = streamDescriptor.Md5;
					}
				}
				Executor.ExecuteSync<NullType>(this.PutPageImpl(stream, startOffset, contentMD5, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
			}
			finally
			{
				if (flag2)
				{
					stream.Dispose();
				}
			}
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000163A0 File Offset: 0x000145A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginWritePages(Stream pageData, long startOffset, string contentMD5, AsyncCallback callback, object state)
		{
			return this.BeginWritePages(pageData, startOffset, contentMD5, null, null, null, callback, state);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000164C4 File Offset: 0x000146C4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginWritePages(Stream pageData, long startOffset, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("pageData", pageData);
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			bool requiresContentMD5 = contentMD5 == null && modifiedOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			if (pageData.CanSeek && !requiresContentMD5)
			{
				this.WritePagesHandler(pageData, startOffset, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
			}
			else
			{
				ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(modifiedOptions);
				storageAsyncResult.CancelDelegate = new Action(executionState.Cancel);
				Stream toStream;
				Stream seekableStream;
				if (pageData.CanSeek)
				{
					seekableStream = pageData;
					toStream = Stream.Null;
				}
				else
				{
					seekableStream = new MultiBufferMemoryStream(base.ServiceClient.BufferManager, 65536);
					storageAsyncResult.OperationState = seekableStream;
					toStream = seekableStream;
				}
				long startPosition = seekableStream.Position;
				StreamDescriptor streamCopyState = new StreamDescriptor();
				pageData.WriteToAsync(toStream, null, new long?(4194304L), requiresContentMD5, executionState, streamCopyState, delegate(ExecutionState<NullType> completedState)
				{
					storageAsyncResult.UpdateCompletedSynchronously(completedState.CompletedSynchronously);
					if (completedState.ExceptionRef != null)
					{
						storageAsyncResult.OnComplete(completedState.ExceptionRef);
						return;
					}
					try
					{
						if (requiresContentMD5)
						{
							contentMD5 = streamCopyState.Md5;
						}
						seekableStream.Position = startPosition;
						this.WritePagesHandler(seekableStream, startOffset, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				});
			}
			return storageAsyncResult;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000166D4 File Offset: 0x000148D4
		private void WritePagesHandler(Stream pageData, long startOffset, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<NullType> storageAsyncResult)
		{
			lock (storageAsyncResult.CancellationLockerObject)
			{
				ICancellableAsyncResult @object = Executor.BeginExecuteAsync<NullType>(this.PutPageImpl(pageData, startOffset, contentMD5, accessCondition, options), options.RetryPolicy, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						Executor.EndExecuteAsync<NullType>(ar);
						storageAsyncResult.OnComplete();
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				}, null);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
				if (storageAsyncResult.CancelRequested)
				{
					storageAsyncResult.Cancel();
				}
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00016784 File Offset: 0x00014984
		public void EndWritePages(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			try
			{
				storageAsyncResult.End();
			}
			finally
			{
				if (storageAsyncResult.OperationState != null)
				{
					MultiBufferMemoryStream multiBufferMemoryStream = (MultiBufferMemoryStream)storageAsyncResult.OperationState;
					multiBufferMemoryStream.Dispose();
				}
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000167CC File Offset: 0x000149CC
		[DoesServiceRequest]
		public Task WritePagesAsync(Stream pageData, long startOffset, string contentMD5)
		{
			return this.WritePagesAsync(pageData, startOffset, contentMD5, CancellationToken.None);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000167DC File Offset: 0x000149DC
		[DoesServiceRequest]
		public Task WritePagesAsync(Stream pageData, long startOffset, string contentMD5, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, string>(new Func<Stream, long, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginWritePages), new Action<IAsyncResult>(this.EndWritePages), pageData, startOffset, contentMD5, cancellationToken);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00016800 File Offset: 0x00014A00
		[DoesServiceRequest]
		public Task WritePagesAsync(Stream pageData, long startOffset, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.WritePagesAsync(pageData, startOffset, contentMD5, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00016818 File Offset: 0x00014A18
		[DoesServiceRequest]
		public Task WritePagesAsync(Stream pageData, long startOffset, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, string, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long, string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginWritePages), new Action<IAsyncResult>(this.EndWritePages), pageData, startOffset, contentMD5, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016850 File Offset: 0x00014A50
		[DoesServiceRequest]
		public void ClearPages(long startOffset, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ClearPageImpl(startOffset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016885 File Offset: 0x00014A85
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClearPages(long startOffset, long length, AsyncCallback callback, object state)
		{
			return this.BeginClearPages(startOffset, length, null, null, null, callback, state);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00016898 File Offset: 0x00014A98
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClearPages(long startOffset, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.PageBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ClearPageImpl(startOffset, length, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000168D0 File Offset: 0x00014AD0
		public void EndClearPages(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000168D9 File Offset: 0x00014AD9
		[DoesServiceRequest]
		public Task ClearPagesAsync(long startOffset, long length)
		{
			return this.ClearPagesAsync(startOffset, length, CancellationToken.None);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000168E8 File Offset: 0x00014AE8
		[DoesServiceRequest]
		public Task ClearPagesAsync(long startOffset, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, long>(new Func<long, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginClearPages), new Action<IAsyncResult>(this.EndClearPages), startOffset, length, cancellationToken);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001690A File Offset: 0x00014B0A
		[DoesServiceRequest]
		public Task ClearPagesAsync(long startOffset, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ClearPagesAsync(startOffset, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001691E File Offset: 0x00014B1E
		[DoesServiceRequest]
		public Task ClearPagesAsync(long startOffset, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<long, long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<long, long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginClearPages), new Action<IAsyncResult>(this.EndClearPages), startOffset, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00016946 File Offset: 0x00014B46
		[Obsolete("Deprecated this method in favor of StartCopy.")]
		[DoesServiceRequest]
		public string StartCopyFromBlob(CloudPageBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001695A File Offset: 0x00014B5A
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopyFromBlob(CloudPageBlob source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001696A File Offset: 0x00014B6A
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		public ICancellableAsyncResult BeginStartCopyFromBlob(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00016982 File Offset: 0x00014B82
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudPageBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016990 File Offset: 0x00014B90
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudPageBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudPageBlob, string>(new Func<CloudPageBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000169B1 File Offset: 0x00014BB1
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x000169C5 File Offset: 0x00014BC5
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudPageBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudPageBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000169ED File Offset: 0x00014BED
		[DoesServiceRequest]
		public string StartCopy(CloudPageBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00016A01 File Offset: 0x00014C01
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudPageBlob source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00016A11 File Offset: 0x00014C11
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00016A29 File Offset: 0x00014C29
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudPageBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00016A37 File Offset: 0x00014C37
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudPageBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudPageBlob, string>(new Func<CloudPageBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00016A58 File Offset: 0x00014C58
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00016A6C File Offset: 0x00014C6C
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudPageBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudPageBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudPageBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00016B20 File Offset: 0x00014D20
		private RESTCommand<NullType> CreateImpl(long sizeInBytes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Put(uri, serverTimeout, this.Properties, BlobType.PageBlob, sizeInBytes, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, base.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				this.Properties.Length = sizeInBytes;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00016C30 File Offset: 0x00014E30
		private RESTCommand<NullType> ResizeImpl(long sizeInBytes, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Resize(uri, serverTimeout, sizeInBytes, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				this.Properties.Length = sizeInBytes;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00016D0C File Offset: 0x00014F0C
		private RESTCommand<NullType> SetSequenceNumberImpl(SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.SetSequenceNumber(uri, serverTimeout, sequenceNumberAction, sequenceNumber, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00016E28 File Offset: 0x00015028
		private RESTCommand<IEnumerable<PageRange>> GetPageRangesImpl(long? offset, long? length, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<IEnumerable<PageRange>> restcommand = new RESTCommand<IEnumerable<PageRange>>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<IEnumerable<PageRange>>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetPageRanges(uri, serverTimeout, this.SnapshotTime, offset, length, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, base.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<IEnumerable<PageRange>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IEnumerable<PageRange>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<IEnumerable<PageRange>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, true);
				GetPageRangesResponse getPageRangesResponse = new GetPageRangesResponse(cmd.ResponseStream);
				return new List<PageRange>(getPageRangesResponse.PageRanges);
			};
			return restcommand;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00016F80 File Offset: 0x00015180
		private RESTCommand<NullType> PutPageImpl(Stream pageData, long startOffset, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options)
		{
			options.AssertNoEncryptionPolicyOrStrictMode();
			if (startOffset % 512L != 0L)
			{
				CommonUtility.ArgumentOutOfRange("startOffset", startOffset);
			}
			long offset = pageData.Position;
			long num = pageData.Length - offset;
			PageRange pageRange = new PageRange(startOffset, startOffset + num - 1L);
			PageWrite pageWrite = PageWrite.Update;
			if ((1L + pageRange.EndOffset - pageRange.StartOffset) % 512L != 0L || 1L + pageRange.EndOffset - pageRange.StartOffset == 0L)
			{
				CommonUtility.ArgumentOutOfRange("pageData", pageData);
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.SendStream = pageData;
			restcommand.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				RecoveryActions.SeekStream<NullType>(cmd, offset);
			};
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.PutPage(uri, serverTimeout, pageRange, pageWrite, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (!string.IsNullOrEmpty(contentMD5))
				{
					r.Headers[HttpRequestHeader.ContentMd5] = contentMD5;
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0001712C File Offset: 0x0001532C
		private RESTCommand<NullType> ClearPageImpl(long startOffset, long length, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("options", options);
			options.AssertNoEncryptionPolicyOrStrictMode();
			if (startOffset < 0L || startOffset % 512L != 0L)
			{
				CommonUtility.ArgumentOutOfRange("startOffset", startOffset);
			}
			if (length <= 0L || length % 512L != 0L)
			{
				CommonUtility.ArgumentOutOfRange("length", length);
			}
			PageRange pageRange = new PageRange(startOffset, startOffset + length - 1L);
			PageWrite pageWrite = PageWrite.Clear;
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.PutPage(uri, serverTimeout, pageRange, pageWrite, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x000172EC File Offset: 0x000154EC
		internal RESTCommand<CloudPageBlob> CreateSnapshotImpl(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<CloudPageBlob> restcommand = new RESTCommand<CloudPageBlob>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<CloudPageBlob>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Snapshot(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (metadata != null)
				{
					BlobHttpWebRequestFactory.AddMetadata(r, metadata);
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<CloudPageBlob> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<CloudPageBlob>(HttpStatusCode.Created, resp, null, cmd, ex);
				DateTimeOffset value = NavigationHelper.ParseSnapshotTime(BlobHttpResponseParsers.GetSnapshotTime(resp));
				CloudPageBlob cloudPageBlob = new CloudPageBlob(this.Name, new DateTimeOffset?(value), this.Container);
				cloudPageBlob.attributes.Metadata = new Dictionary<string, string>(metadata ?? this.Metadata);
				cloudPageBlob.attributes.Properties = new BlobProperties(this.Properties);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(cloudPageBlob.attributes, resp, false);
				return cloudPageBlob;
			};
			return restcommand;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0001738B File Offset: 0x0001558B
		public CloudPageBlob(Uri blobAbsoluteUri) : this(blobAbsoluteUri, null)
		{
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00017398 File Offset: 0x00015598
		public CloudPageBlob(Uri blobAbsoluteUri, StorageCredentials credentials) : this(blobAbsoluteUri, null, credentials)
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000173B6 File Offset: 0x000155B6
		public CloudPageBlob(Uri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials) : this(new StorageUri(blobAbsoluteUri), snapshotTime, credentials)
		{
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000173C6 File Offset: 0x000155C6
		public CloudPageBlob(StorageUri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobAbsoluteUri, snapshotTime, credentials);
			base.Properties.BlobType = BlobType.PageBlob;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000173E8 File Offset: 0x000155E8
		internal CloudPageBlob(string blobName, DateTimeOffset? snapshotTime, CloudBlobContainer container)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobName, snapshotTime, container);
			base.Properties.BlobType = BlobType.PageBlob;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001740A File Offset: 0x0001560A
		internal CloudPageBlob(BlobAttributes attributes, CloudBlobClient serviceClient)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(attributes, serviceClient);
			base.Properties.BlobType = BlobType.PageBlob;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001742B File Offset: 0x0001562B
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00017433 File Offset: 0x00015633
		public int StreamWriteSizeInBytes
		{
			get
			{
				return this.streamWriteSizeInBytes;
			}
			set
			{
				CommonUtility.AssertInBounds<int>("StreamWriteSizeInBytes", value, 512, 4194304);
				this.streamWriteSizeInBytes = value;
			}
		}

		// Token: 0x040000CA RID: 202
		private int streamWriteSizeInBytes;
	}
}
