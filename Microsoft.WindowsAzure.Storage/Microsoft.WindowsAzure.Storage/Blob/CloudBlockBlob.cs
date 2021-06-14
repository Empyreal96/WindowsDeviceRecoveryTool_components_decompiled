using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200001D RID: 29
	public sealed class CloudBlockBlob : CloudBlob, ICloudBlob, IListBlobItem
	{
		// Token: 0x06000534 RID: 1332 RVA: 0x000127E4 File Offset: 0x000109E4
		public CloudBlobStream OpenWrite(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			if (accessCondition != null && accessCondition.IsConditional)
			{
				try
				{
					base.FetchAttributes(accessCondition, options, operationContext);
				}
				catch (StorageException ex)
				{
					if (ex.RequestInformation == null || ex.RequestInformation.HttpStatusCode != 404 || !string.IsNullOrEmpty(accessCondition.IfMatchETag))
					{
						throw;
					}
				}
			}
			blobRequestOptions.AssertPolicyIfRequired();
			if (blobRequestOptions.EncryptionPolicy != null)
			{
				ICryptoTransform transform = blobRequestOptions.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, false);
				return new BlobEncryptedWriteStream(this, accessCondition, blobRequestOptions, operationContext, transform);
			}
			return new BlobWriteStream(this, accessCondition, blobRequestOptions, operationContext);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00012898 File Offset: 0x00010A98
		public ICancellableAsyncResult BeginOpenWrite(AsyncCallback callback, object state)
		{
			return this.BeginOpenWrite(null, null, null, callback, state);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00012978 File Offset: 0x00010B78
		public ICancellableAsyncResult BeginOpenWrite(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = new StorageAsyncResult<CloudBlobStream>(callback, state);
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			if (accessCondition != null && accessCondition.IsConditional)
			{
				ICancellableAsyncResult @object = base.BeginFetchAttributes(accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndFetchAttributes(ar);
					}
					catch (StorageException ex)
					{
						if (ex.RequestInformation == null || ex.RequestInformation.HttpStatusCode != 404 || !string.IsNullOrEmpty(accessCondition.IfMatchETag))
						{
							storageAsyncResult.OnComplete(ex);
							return;
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
						return;
					}
					storageAsyncResult.Result = new BlobWriteStream(this, accessCondition, modifiedOptions, operationContext);
					storageAsyncResult.OnComplete();
				}, null);
				storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			}
			else
			{
				modifiedOptions.AssertPolicyIfRequired();
				if (modifiedOptions.EncryptionPolicy != null)
				{
					ICryptoTransform transform = modifiedOptions.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, false);
					storageAsyncResult.Result = new BlobEncryptedWriteStream(this, accessCondition, modifiedOptions, operationContext, transform);
				}
				else
				{
					storageAsyncResult.Result = new BlobWriteStream(this, accessCondition, modifiedOptions, operationContext);
				}
				storageAsyncResult.OnComplete();
			}
			return storageAsyncResult;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00012AB8 File Offset: 0x00010CB8
		public CloudBlobStream EndOpenWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = (StorageAsyncResult<CloudBlobStream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00012AD8 File Offset: 0x00010CD8
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync()
		{
			return this.OpenWriteAsync(CancellationToken.None);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00012AE5 File Offset: 0x00010CE5
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlobStream>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), cancellationToken);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00012B05 File Offset: 0x00010D05
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.OpenWriteAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00012B15 File Offset: 0x00010D15
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, BlobRequestOptions, OperationContext, CloudBlobStream>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00012B3C File Offset: 0x00010D3C
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, null, accessCondition, options, operationContext);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00012B5D File Offset: 0x00010D5D
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00012B74 File Offset: 0x00010D74
		[DoesServiceRequest]
		internal void UploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("source", source);
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L);
				if (source.CanSeek && length > source.Length - source.Position)
				{
					throw new ArgumentOutOfRangeException("length", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
				}
			}
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			bool flag = source.CanSeek && (length ?? (source.Length - source.Position)) <= blobRequestOptions.SingleBlobUploadThresholdInBytes.Value;
			blobRequestOptions.AssertPolicyIfRequired();
			if (blobRequestOptions.ParallelOperationThreadCount.Value == 1 && flag && blobRequestOptions.EncryptionPolicy == null)
			{
				string contentMD = null;
				if (blobRequestOptions.StoreBlobContentMD5.Value)
				{
					using (ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(blobRequestOptions))
					{
						StreamDescriptor streamDescriptor = new StreamDescriptor();
						long position = source.Position;
						source.WriteToSync(Stream.Null, length, null, true, true, executionState, streamDescriptor);
						source.Position = position;
						contentMD = streamDescriptor.Md5;
						goto IL_17F;
					}
				}
				if (blobRequestOptions.UseTransactionalMD5.Value)
				{
					throw new ArgumentException("When uploading a blob in a single request, StoreBlobContentMD5 must be set to true if UseTransactionalMD5 is true, because the MD5 calculated for the transaction will be stored in the blob.", "options");
				}
				IL_17F:
				Executor.ExecuteSync<NullType>(this.PutBlobImpl(source, length, contentMD, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
				return;
			}
			using (CloudBlobStream cloudBlobStream = this.OpenWrite(accessCondition, blobRequestOptions, operationContext))
			{
				using (ExecutionState<NullType> executionState2 = CommonUtility.CreateTemporaryExecutionState(blobRequestOptions))
				{
					source.WriteToSync(cloudBlobStream, length, null, false, true, executionState2, null);
					cloudBlobStream.Commit();
				}
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012D94 File Offset: 0x00010F94
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, null, null, null, callback, state);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00012DB8 File Offset: 0x00010FB8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00012DDD File Offset: 0x00010FDD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), null, null, null, callback, state);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00012DF2 File Offset: 0x00010FF2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00013168 File Offset: 0x00011368
		internal ICancellableAsyncResult BeginUploadFromStreamHelper(Stream source, long? length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("source", source);
			if (length != null)
			{
				CommonUtility.AssertInBounds<long>("length", length.Value, 1L);
				if (source.CanSeek && length > source.Length - source.Position)
				{
					throw new ArgumentOutOfRangeException("length", "The requested number of bytes exceeds the length of the stream remaining from the specified position.");
				}
			}
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			ExecutionState<NullType> tempExecutionState = CommonUtility.CreateTemporaryExecutionState(modifiedOptions);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			bool flag = source.CanSeek && (length ?? (source.Length - source.Position)) <= modifiedOptions.SingleBlobUploadThresholdInBytes.Value;
			modifiedOptions.AssertPolicyIfRequired();
			if (modifiedOptions.ParallelOperationThreadCount.Value == 1 && flag && modifiedOptions.EncryptionPolicy == null)
			{
				if (modifiedOptions.StoreBlobContentMD5.Value)
				{
					long startPosition = source.Position;
					StreamDescriptor streamCopyState = new StreamDescriptor();
					source.WriteToAsync(Stream.Null, length, null, true, tempExecutionState, streamCopyState, delegate(ExecutionState<NullType> completedState)
					{
						storageAsyncResult.UpdateCompletedSynchronously(completedState.CompletedSynchronously);
						try
						{
							lock (storageAsyncResult.CancellationLockerObject)
							{
								storageAsyncResult.CancelDelegate = null;
								if (completedState.ExceptionRef != null)
								{
									storageAsyncResult.OnComplete(completedState.ExceptionRef);
								}
								else
								{
									source.Position = startPosition;
									this.UploadFromStreamHandler(source, length, streamCopyState.Md5, accessCondition, operationContext, modifiedOptions, storageAsyncResult);
								}
							}
						}
						catch (Exception exception)
						{
							storageAsyncResult.OnComplete(exception);
						}
					});
					storageAsyncResult.CancelDelegate = new Action(tempExecutionState.Cancel);
				}
				else
				{
					if (modifiedOptions.UseTransactionalMD5.Value)
					{
						throw new ArgumentException("When uploading a blob in a single request, StoreBlobContentMD5 must be set to true if UseTransactionalMD5 is true, because the MD5 calculated for the transaction will be stored in the blob.", "options");
					}
					this.UploadFromStreamHandler(source, length, null, accessCondition, operationContext, modifiedOptions, storageAsyncResult);
				}
			}
			else
			{
				ICancellableAsyncResult @object = this.BeginOpenWrite(accessCondition, modifiedOptions, operationContext, delegate(IAsyncResult ar)
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
			}
			return storageAsyncResult;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000134C4 File Offset: 0x000116C4
		private void UploadFromStreamHandler(Stream source, long? length, string contentMD5, AccessCondition accessCondition, OperationContext operationContext, BlobRequestOptions options, StorageAsyncResult<NullType> storageAsyncResult)
		{
			ICancellableAsyncResult @object = Executor.BeginExecuteAsync<NullType>(this.PutBlobImpl(source, length, contentMD5, accessCondition, options), options.RetryPolicy, operationContext, delegate(IAsyncResult ar)
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

		// Token: 0x06000545 RID: 1349 RVA: 0x00013538 File Offset: 0x00011738
		public void EndUploadFromStream(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00013552 File Offset: 0x00011752
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source)
		{
			return this.UploadFromStreamAsync(source, CancellationToken.None);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00013560 File Offset: 0x00011760
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, cancellationToken);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00013581 File Offset: 0x00011781
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00013593 File Offset: 0x00011793
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000135B9 File Offset: 0x000117B9
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length)
		{
			return this.UploadFromStreamAsync(source, length, CancellationToken.None);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000135C8 File Offset: 0x000117C8
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long>(new Func<Stream, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, cancellationToken);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x000135EA File Offset: 0x000117EA
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000135FE File Offset: 0x000117FE
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00013628 File Offset: 0x00011828
		[DoesServiceRequest]
		public void UploadFromFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			using (FileStream fileStream = new FileStream(path, mode, FileAccess.Read))
			{
				this.UploadFromStream(fileStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00013674 File Offset: 0x00011874
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00013684 File Offset: 0x00011884
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

		// Token: 0x06000551 RID: 1361 RVA: 0x00013708 File Offset: 0x00011908
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

		// Token: 0x06000552 RID: 1362 RVA: 0x00013770 File Offset: 0x00011970
		public void EndUploadFromFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001378A File Offset: 0x0001198A
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode)
		{
			return this.UploadFromFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00013799 File Offset: 0x00011999
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, cancellationToken);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000137BB File Offset: 0x000119BB
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000137CF File Offset: 0x000119CF
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x000137F8 File Offset: 0x000119F8
		[DoesServiceRequest]
		public void UploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(buffer, index, count))
			{
				this.UploadFromStream(syncMemoryStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00013844 File Offset: 0x00011A44
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromByteArray(buffer, index, count, null, null, null, callback, state);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00013864 File Offset: 0x00011A64
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			SyncMemoryStream source = new SyncMemoryStream(buffer, index, count);
			return this.BeginUploadFromStream(source, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00013896 File Offset: 0x00011A96
		public void EndUploadFromByteArray(IAsyncResult asyncResult)
		{
			this.EndUploadFromStream(asyncResult);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001389F File Offset: 0x00011A9F
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000138AF File Offset: 0x00011AAF
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, cancellationToken);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000138D3 File Offset: 0x00011AD3
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000138EC File Offset: 0x00011AEC
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext>(new Func<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00013924 File Offset: 0x00011B24
		[DoesServiceRequest]
		public void UploadText(string content, Encoding encoding = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			this.UploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001395D File Offset: 0x00011B5D
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, AsyncCallback callback, object state)
		{
			return this.BeginUploadText(content, null, null, null, null, callback, state);
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001396C File Offset: 0x00011B6C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			return this.BeginUploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000139A9 File Offset: 0x00011BA9
		public void EndUploadText(IAsyncResult asyncResult)
		{
			this.EndUploadFromByteArray(asyncResult);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000139B2 File Offset: 0x00011BB2
		[DoesServiceRequest]
		public Task UploadTextAsync(string content)
		{
			return this.UploadTextAsync(content, CancellationToken.None);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000139C0 File Offset: 0x00011BC0
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, cancellationToken);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000139E1 File Offset: 0x00011BE1
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadTextAsync(content, encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x000139F5 File Offset: 0x00011BF5
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013A20 File Offset: 0x00011C20
		public string DownloadText(Encoding encoding = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			string @string;
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream())
			{
				base.DownloadToStream(syncMemoryStream, accessCondition, options, operationContext);
				byte[] buffer = syncMemoryStream.GetBuffer();
				@string = (encoding ?? Encoding.UTF8).GetString(buffer, 0, (int)syncMemoryStream.Length);
			}
			return @string;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013A7C File Offset: 0x00011C7C
		public ICancellableAsyncResult BeginDownloadText(AsyncCallback callback, object state)
		{
			return this.BeginDownloadText(null, null, null, null, callback, state);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013A8C File Offset: 0x00011C8C
		public ICancellableAsyncResult BeginDownloadText(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			SyncMemoryStream syncMemoryStream = new SyncMemoryStream();
			StorageAsyncResult<string> storageAsyncResult = new StorageAsyncResult<string>(callback, state)
			{
				OperationState = Tuple.Create<SyncMemoryStream, Encoding>(syncMemoryStream, encoding)
			};
			ICancellableAsyncResult @object = base.BeginDownloadToStream(syncMemoryStream, accessCondition, options, operationContext, new AsyncCallback(this.DownloadTextCallback), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013AE8 File Offset: 0x00011CE8
		private void DownloadTextCallback(IAsyncResult asyncResult)
		{
			StorageAsyncResult<string> storageAsyncResult = (StorageAsyncResult<string>)asyncResult.AsyncState;
			try
			{
				base.EndDownloadToStream(asyncResult);
				Tuple<SyncMemoryStream, Encoding> tuple = (Tuple<SyncMemoryStream, Encoding>)storageAsyncResult.OperationState;
				byte[] buffer = tuple.Item1.GetBuffer();
				storageAsyncResult.Result = (tuple.Item2 ?? Encoding.UTF8).GetString(buffer, 0, (int)tuple.Item1.Length);
				storageAsyncResult.OnComplete();
			}
			catch (Exception exception)
			{
				storageAsyncResult.OnComplete(exception);
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013B6C File Offset: 0x00011D6C
		public string EndDownloadText(IAsyncResult asyncResult)
		{
			StorageAsyncResult<string> storageAsyncResult = (StorageAsyncResult<string>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013B8C File Offset: 0x00011D8C
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync()
		{
			return this.DownloadTextAsync(CancellationToken.None);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013B99 File Offset: 0x00011D99
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), cancellationToken);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00013BB9 File Offset: 0x00011DB9
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadTextAsync(encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00013BCB File Offset: 0x00011DCB
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Encoding, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<Encoding, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013BF4 File Offset: 0x00011DF4
		[DoesServiceRequest]
		public void PutBlock(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("blockData", blockData);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			bool flag = string.IsNullOrEmpty(contentMD5) && blobRequestOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			Stream stream = blockData;
			bool flag2 = false;
			try
			{
				if (!blockData.CanSeek || flag)
				{
					ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(blobRequestOptions);
					Stream toStream;
					if (blockData.CanSeek)
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
					blockData.WriteToSync(toStream, null, new long?(4194304L), flag, true, executionState, streamDescriptor);
					stream.Position = position;
					if (flag)
					{
						contentMD5 = streamDescriptor.Md5;
					}
				}
				Executor.ExecuteSync<NullType>(this.PutBlockImpl(stream, blockId, contentMD5, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
			}
			finally
			{
				if (flag2)
				{
					stream.Dispose();
				}
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00013D04 File Offset: 0x00011F04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPutBlock(string blockId, Stream blockData, string contentMD5, AsyncCallback callback, object state)
		{
			return this.BeginPutBlock(blockId, blockData, contentMD5, null, null, null, callback, state);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013D24 File Offset: 0x00011F24
		[DoesServiceRequest]
		public IEnumerable<ListBlockItem> DownloadBlockList(BlockListingFilter blockListingFilter = BlockListingFilter.Committed, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			return Executor.ExecuteSync<IEnumerable<ListBlockItem>>(this.GetBlockListImpl(blockListingFilter, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013E5C File Offset: 0x0001205C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPutBlock(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("blockData", blockData);
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			bool requiresContentMD5 = string.IsNullOrEmpty(contentMD5) && modifiedOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			if (blockData.CanSeek && !requiresContentMD5)
			{
				this.PutBlockHandler(blockId, blockData, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
			}
			else
			{
				ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(modifiedOptions);
				storageAsyncResult.CancelDelegate = new Action(executionState.Cancel);
				Stream toStream;
				Stream seekableStream;
				if (blockData.CanSeek)
				{
					seekableStream = blockData;
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
				blockData.WriteToAsync(toStream, null, new long?(4194304L), requiresContentMD5, executionState, streamCopyState, delegate(ExecutionState<NullType> completedState)
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
						this.PutBlockHandler(blockId, seekableStream, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				});
			}
			return storageAsyncResult;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00014014 File Offset: 0x00012214
		public void EndPutBlock(IAsyncResult asyncResult)
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

		// Token: 0x06000575 RID: 1397 RVA: 0x000140B8 File Offset: 0x000122B8
		private void PutBlockHandler(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<NullType> storageAsyncResult)
		{
			lock (storageAsyncResult.CancellationLockerObject)
			{
				ICancellableAsyncResult @object = Executor.BeginExecuteAsync<NullType>(this.PutBlockImpl(blockData, blockId, contentMD5, accessCondition, options), options.RetryPolicy, operationContext, delegate(IAsyncResult ar)
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

		// Token: 0x06000576 RID: 1398 RVA: 0x00014168 File Offset: 0x00012368
		[DoesServiceRequest]
		public Task PutBlockAsync(string blockId, Stream blockData, string contentMD5)
		{
			return this.PutBlockAsync(blockId, blockData, contentMD5, CancellationToken.None);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00014178 File Offset: 0x00012378
		[DoesServiceRequest]
		public Task PutBlockAsync(string blockId, Stream blockData, string contentMD5, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Stream, string>(new Func<string, Stream, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPutBlock), new Action<IAsyncResult>(this.EndPutBlock), blockId, blockData, contentMD5, cancellationToken);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001419C File Offset: 0x0001239C
		[DoesServiceRequest]
		public Task PutBlockAsync(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.PutBlockAsync(blockId, blockData, contentMD5, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x000141B4 File Offset: 0x000123B4
		[DoesServiceRequest]
		public Task PutBlockAsync(string blockId, Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Stream, string, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, Stream, string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPutBlock), new Action<IAsyncResult>(this.EndPutBlock), blockId, blockData, contentMD5, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x000141E9 File Offset: 0x000123E9
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of StartCopy.")]
		public string StartCopyFromBlob(CloudBlockBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000141FD File Offset: 0x000123FD
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopyFromBlob(CloudBlockBlob source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001420D File Offset: 0x0001240D
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of BeginStartCopy.")]
		public ICancellableAsyncResult BeginStartCopyFromBlob(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00014225 File Offset: 0x00012425
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudBlockBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00014233 File Offset: 0x00012433
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudBlockBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlockBlob, string>(new Func<CloudBlockBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00014254 File Offset: 0x00012454
		[DoesServiceRequest]
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		public Task<string> StartCopyFromBlobAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00014268 File Offset: 0x00012468
		[Obsolete("Deprecated this method in favor of StartCopyAsync.")]
		[DoesServiceRequest]
		public Task<string> StartCopyFromBlobAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlockBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudBlockBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00014290 File Offset: 0x00012490
		[DoesServiceRequest]
		public string StartCopy(CloudFile source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudFile.SourceFileToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000142A4 File Offset: 0x000124A4
		[DoesServiceRequest]
		public string StartCopy(CloudBlockBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000142B8 File Offset: 0x000124B8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudFile source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudFile.SourceFileToUri(source), callback, state);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000142C8 File Offset: 0x000124C8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudBlockBlob source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000142D8 File Offset: 0x000124D8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudFile.SourceFileToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000142F0 File Offset: 0x000124F0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00014308 File Offset: 0x00012508
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00014316 File Offset: 0x00012516
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlockBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00014324 File Offset: 0x00012524
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudFile, string>(new Func<CloudFile, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00014345 File Offset: 0x00012545
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlockBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlockBlob, string>(new Func<CloudBlockBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00014366 File Offset: 0x00012566
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001437A File Offset: 0x0001257A
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001438E File Offset: 0x0001258E
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudFile source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudFile, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudFile, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000143B6 File Offset: 0x000125B6
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudBlockBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlockBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudBlockBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000143DE File Offset: 0x000125DE
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadBlockList(AsyncCallback callback, object state)
		{
			return this.BeginDownloadBlockList(BlockListingFilter.Committed, null, null, null, callback, state);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000143EC File Offset: 0x000125EC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDownloadBlockList(BlockListingFilter blockListingFilter, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<IEnumerable<ListBlockItem>>(this.GetBlockListImpl(blockListingFilter, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00014422 File Offset: 0x00012622
		public IEnumerable<ListBlockItem> EndDownloadBlockList(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IEnumerable<ListBlockItem>>(asyncResult);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001442A File Offset: 0x0001262A
		[DoesServiceRequest]
		public Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync()
		{
			return this.DownloadBlockListAsync(CancellationToken.None);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00014437 File Offset: 0x00012637
		[DoesServiceRequest]
		public Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IEnumerable<ListBlockItem>>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadBlockList), new Func<IAsyncResult, IEnumerable<ListBlockItem>>(this.EndDownloadBlockList), cancellationToken);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00014457 File Offset: 0x00012657
		[DoesServiceRequest]
		public Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync(BlockListingFilter blockListingFilter, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadBlockListAsync(blockListingFilter, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00014469 File Offset: 0x00012669
		[DoesServiceRequest]
		public Task<IEnumerable<ListBlockItem>> DownloadBlockListAsync(BlockListingFilter blockListingFilter, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlockListingFilter, AccessCondition, BlobRequestOptions, OperationContext, IEnumerable<ListBlockItem>>(new Func<BlockListingFilter, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadBlockList), new Func<IAsyncResult, IEnumerable<ListBlockItem>>(this.EndDownloadBlockList), blockListingFilter, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00014490 File Offset: 0x00012690
		[DoesServiceRequest]
		public CloudBlockBlob CreateSnapshot(IDictionary<string, string> metadata = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			return Executor.ExecuteSync<CloudBlockBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000144CD File Offset: 0x000126CD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(AsyncCallback callback, object state)
		{
			return this.BeginCreateSnapshot(null, null, null, null, callback, state);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000144DC File Offset: 0x000126DC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<CloudBlockBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001451D File Offset: 0x0001271D
		public CloudBlockBlob EndCreateSnapshot(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<CloudBlockBlob>(asyncResult);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014525 File Offset: 0x00012725
		[DoesServiceRequest]
		public Task<CloudBlockBlob> CreateSnapshotAsync()
		{
			return this.CreateSnapshotAsync(CancellationToken.None);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00014532 File Offset: 0x00012732
		[DoesServiceRequest]
		public Task<CloudBlockBlob> CreateSnapshotAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudBlockBlob>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudBlockBlob>(this.EndCreateSnapshot), cancellationToken);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00014552 File Offset: 0x00012752
		[DoesServiceRequest]
		public Task<CloudBlockBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateSnapshotAsync(metadata, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00014564 File Offset: 0x00012764
		[DoesServiceRequest]
		public Task<CloudBlockBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, CloudBlockBlob>(new Func<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudBlockBlob>(this.EndCreateSnapshot), metadata, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00014594 File Offset: 0x00012794
		[DoesServiceRequest]
		public void PutBlockList(IEnumerable<string> blockList, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			IEnumerable<PutBlockListItem> blocks = from i in blockList
			select new PutBlockListItem(i, BlockSearchMode.Latest);
			Executor.ExecuteSync<NullType>(this.PutBlockListImpl(blocks, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000145EB File Offset: 0x000127EB
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPutBlockList(IEnumerable<string> blockList, AsyncCallback callback, object state)
		{
			return this.BeginPutBlockList(blockList, null, null, null, callback, state);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00014604 File Offset: 0x00012804
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPutBlockList(IEnumerable<string> blockList, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.BlockBlob, base.ServiceClient, true);
			IEnumerable<PutBlockListItem> blocks = from i in blockList
			select new PutBlockListItem(i, BlockSearchMode.Latest);
			return Executor.BeginExecuteAsync<NullType>(this.PutBlockListImpl(blocks, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001465E File Offset: 0x0001285E
		public void EndPutBlockList(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00014667 File Offset: 0x00012867
		[DoesServiceRequest]
		public Task PutBlockListAsync(IEnumerable<string> blockList)
		{
			return this.PutBlockListAsync(blockList, CancellationToken.None);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00014675 File Offset: 0x00012875
		[DoesServiceRequest]
		public Task PutBlockListAsync(IEnumerable<string> blockList, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<IEnumerable<string>>(new Func<IEnumerable<string>, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPutBlockList), new Action<IAsyncResult>(this.EndPutBlockList), blockList, cancellationToken);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00014696 File Offset: 0x00012896
		[DoesServiceRequest]
		public Task PutBlockListAsync(IEnumerable<string> blockList, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.PutBlockListAsync(blockList, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000146A8 File Offset: 0x000128A8
		[DoesServiceRequest]
		public Task PutBlockListAsync(IEnumerable<string> blockList, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<IEnumerable<string>, AccessCondition, BlobRequestOptions, OperationContext>(new Func<IEnumerable<string>, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPutBlockList), new Action<IAsyncResult>(this.EndPutBlockList), blockList, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001476C File Offset: 0x0001296C
		private RESTCommand<NullType> PutBlobImpl(Stream stream, long? length, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options)
		{
			long offset = stream.Position;
			base.Properties.ContentMD5 = contentMD5;
			RESTCommand<NullType> putCmd = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(putCmd);
			putCmd.SendStream = stream;
			putCmd.SendStreamLength = new long?(length ?? (stream.Length - offset));
			putCmd.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				RecoveryActions.SeekStream<NullType>(cmd, offset);
			};
			putCmd.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Put(uri, serverTimeout, this.Properties, BlobType.BlockBlob, 0L, accessCondition, useVersionHeader, ctx));
			putCmd.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, base.Metadata);
			};
			putCmd.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			putCmd.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				this.Properties.Length = putCmd.SendStreamLength.Value;
				return NullType.Value;
			};
			return putCmd;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000148FC File Offset: 0x00012AFC
		internal RESTCommand<NullType> PutBlockImpl(Stream source, string blockId, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options)
		{
			options.AssertNoEncryptionPolicyOrStrictMode();
			long offset = source.Position;
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.SendStream = source;
			restcommand.RecoveryAction = delegate(StorageCommandBase<NullType> cmd, Exception ex, OperationContext ctx)
			{
				RecoveryActions.SeekStream<NullType>(cmd, offset);
			};
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.PutBlock(uri, serverTimeout, blockId, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (!string.IsNullOrEmpty(contentMD5))
				{
					r.Headers[HttpRequestHeader.ContentMd5] = contentMD5;
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00014A64 File Offset: 0x00012C64
		internal RESTCommand<NullType> PutBlockListImpl(IEnumerable<PutBlockListItem> blocks, AccessCondition accessCondition, BlobRequestOptions options)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(base.ServiceClient.BufferManager, 65536);
			BlobRequest.WriteBlockListBody(blocks, multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			string contentMD5 = null;
			if (options.UseTransactionalMD5 != null && options.UseTransactionalMD5.Value)
			{
				contentMD5 = multiBufferMemoryStream.ComputeMD5Hash();
				multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.PutBlockList(uri, serverTimeout, this.Properties, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (contentMD5 != null)
				{
					r.Headers[HttpRequestHeader.ContentMd5] = contentMD5;
				}
				BlobHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				base.Properties.Length = -1L;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014BF4 File Offset: 0x00012DF4
		internal RESTCommand<IEnumerable<ListBlockItem>> GetBlockListImpl(BlockListingFilter typesOfBlocks, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<IEnumerable<ListBlockItem>> restcommand = new RESTCommand<IEnumerable<ListBlockItem>>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<IEnumerable<ListBlockItem>>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetBlockList(uri, serverTimeout, this.SnapshotTime, typesOfBlocks, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<IEnumerable<ListBlockItem>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IEnumerable<ListBlockItem>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<IEnumerable<ListBlockItem>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, true);
				GetBlockListResponse getBlockListResponse = new GetBlockListResponse(cmd.ResponseStream);
				return new List<ListBlockItem>(getBlockListResponse.Blocks);
			};
			return restcommand;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00014D7C File Offset: 0x00012F7C
		internal RESTCommand<CloudBlockBlob> CreateSnapshotImpl(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<CloudBlockBlob> restcommand = new RESTCommand<CloudBlockBlob>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<CloudBlockBlob>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Snapshot(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (metadata != null)
				{
					BlobHttpWebRequestFactory.AddMetadata(r, metadata);
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<CloudBlockBlob> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<CloudBlockBlob>(HttpStatusCode.Created, resp, null, cmd, ex);
				DateTimeOffset value = NavigationHelper.ParseSnapshotTime(BlobHttpResponseParsers.GetSnapshotTime(resp));
				CloudBlockBlob cloudBlockBlob = new CloudBlockBlob(this.Name, new DateTimeOffset?(value), this.Container);
				cloudBlockBlob.attributes.Metadata = new Dictionary<string, string>(metadata ?? this.Metadata);
				cloudBlockBlob.attributes.Properties = new BlobProperties(this.Properties);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(cloudBlockBlob.attributes, resp, false);
				return cloudBlockBlob;
			};
			return restcommand;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00014E1B File Offset: 0x0001301B
		public CloudBlockBlob(Uri blobAbsoluteUri) : this(blobAbsoluteUri, null)
		{
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014E28 File Offset: 0x00013028
		public CloudBlockBlob(Uri blobAbsoluteUri, StorageCredentials credentials) : this(blobAbsoluteUri, null, credentials)
		{
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00014E46 File Offset: 0x00013046
		public CloudBlockBlob(Uri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials) : this(new StorageUri(blobAbsoluteUri), snapshotTime, credentials)
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00014E56 File Offset: 0x00013056
		public CloudBlockBlob(StorageUri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobAbsoluteUri, snapshotTime, credentials);
			base.Properties.BlobType = BlobType.BlockBlob;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00014E78 File Offset: 0x00013078
		internal CloudBlockBlob(string blobName, DateTimeOffset? snapshotTime, CloudBlobContainer container)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobName, snapshotTime, container);
			base.Properties.BlobType = BlobType.BlockBlob;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00014E9A File Offset: 0x0001309A
		internal CloudBlockBlob(BlobAttributes attributes, CloudBlobClient serviceClient)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(attributes, serviceClient);
			base.Properties.BlobType = BlobType.BlockBlob;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00014EBB File Offset: 0x000130BB
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00014EC3 File Offset: 0x000130C3
		public int StreamWriteSizeInBytes
		{
			get
			{
				return this.streamWriteSizeInBytes;
			}
			set
			{
				CommonUtility.AssertInBounds<long>("StreamWriteSizeInBytes", (long)value, 16384L, 4194304L);
				this.streamWriteSizeInBytes = value;
			}
		}

		// Token: 0x040000C5 RID: 197
		private int streamWriteSizeInBytes;
	}
}
