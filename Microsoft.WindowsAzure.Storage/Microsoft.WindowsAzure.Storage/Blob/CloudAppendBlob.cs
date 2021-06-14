using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000018 RID: 24
	public sealed class CloudAppendBlob : CloudBlob, ICloudBlob, IListBlobItem
	{
		// Token: 0x06000335 RID: 821 RVA: 0x0000B380 File Offset: 0x00009580
		[DoesServiceRequest]
		public CloudBlobStream OpenWrite(bool createNew, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			ICryptoTransform transform = null;
			if (createNew)
			{
				if (options != null && options.EncryptionPolicy != null)
				{
					transform = options.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, false);
				}
				this.CreateOrReplace(accessCondition, options, operationContext);
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
			}
			if (accessCondition != null)
			{
				accessCondition = new AccessCondition
				{
					LeaseId = accessCondition.LeaseId,
					IfAppendPositionEqual = accessCondition.IfAppendPositionEqual,
					IfMaxSizeLessThanOrEqual = accessCondition.IfMaxSizeLessThanOrEqual
				};
			}
			if (blobRequestOptions.EncryptionPolicy != null)
			{
				return new BlobEncryptedWriteStream(this, accessCondition, blobRequestOptions, operationContext, transform);
			}
			return new BlobWriteStream(this, accessCondition, blobRequestOptions, operationContext);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B461 File Offset: 0x00009661
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(bool createNew, AsyncCallback callback, object state)
		{
			return this.BeginOpenWrite(createNew, null, null, null, callback, state);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000B634 File Offset: 0x00009834
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginOpenWrite(bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, base.BlobType, base.ServiceClient, false);
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = new StorageAsyncResult<CloudBlobStream>(callback, state);
			ICryptoTransform transform = null;
			ICancellableAsyncResult @object;
			if (createNew)
			{
				if (options != null && options.EncryptionPolicy != null)
				{
					transform = options.EncryptionPolicy.CreateAndSetEncryptionContext(base.Metadata, false);
				}
				@object = this.BeginCreateOrReplace(accessCondition, options, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						this.EndCreateOrReplace(ar);
						if (accessCondition != null)
						{
							accessCondition = new AccessCondition
							{
								LeaseId = accessCondition.LeaseId,
								IfAppendPositionEqual = accessCondition.IfAppendPositionEqual,
								IfMaxSizeLessThanOrEqual = accessCondition.IfMaxSizeLessThanOrEqual
							};
						}
						if (modifiedOptions.EncryptionPolicy != null)
						{
							storageAsyncResult.Result = new BlobEncryptedWriteStream(this, accessCondition, modifiedOptions, operationContext, transform);
						}
						else
						{
							storageAsyncResult.Result = new BlobWriteStream(this, accessCondition, modifiedOptions, operationContext);
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
							accessCondition = new AccessCondition
							{
								LeaseId = accessCondition.LeaseId,
								IfAppendPositionEqual = accessCondition.IfAppendPositionEqual
							};
						}
						storageAsyncResult.Result = new BlobWriteStream(this, accessCondition, modifiedOptions, operationContext);
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

		// Token: 0x06000338 RID: 824 RVA: 0x0000B768 File Offset: 0x00009968
		public CloudBlobStream EndOpenWrite(IAsyncResult asyncResult)
		{
			StorageAsyncResult<CloudBlobStream> storageAsyncResult = (StorageAsyncResult<CloudBlobStream>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000B788 File Offset: 0x00009988
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(bool createNew)
		{
			return this.OpenWriteAsync(createNew, CancellationToken.None);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000B796 File Offset: 0x00009996
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(bool createNew, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool, CloudBlobStream>(new Func<bool, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), createNew, cancellationToken);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000B7B7 File Offset: 0x000099B7
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.OpenWriteAsync(createNew, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000B7C9 File Offset: 0x000099C9
		[DoesServiceRequest]
		public Task<CloudBlobStream> OpenWriteAsync(bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool, AccessCondition, BlobRequestOptions, OperationContext, CloudBlobStream>(new Func<bool, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginOpenWrite), new Func<IAsyncResult, CloudBlobStream>(this.EndOpenWrite), createNew, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000B7F0 File Offset: 0x000099F0
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, null, true, accessCondition, options, operationContext);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000B812 File Offset: 0x00009A12
		[DoesServiceRequest]
		public void UploadFromStream(Stream source, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, new long?(length), true, accessCondition, options, operationContext);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000B828 File Offset: 0x00009A28
		[DoesServiceRequest]
		public void AppendFromStream(Stream source, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, null, false, accessCondition, options, operationContext);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000B84A File Offset: 0x00009A4A
		[DoesServiceRequest]
		public void AppendFromStream(Stream source, long length, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.UploadFromStreamHelper(source, new long?(length), false, accessCondition, options, operationContext);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000B860 File Offset: 0x00009A60
		internal void UploadFromStreamHelper(Stream source, long? length, bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
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
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			using (CloudBlobStream cloudBlobStream = this.OpenWrite(createNew, accessCondition, options2, operationContext))
			{
				using (ExecutionState<NullType> executionState = CommonUtility.CreateTemporaryExecutionState(options2))
				{
					source.WriteToSync(cloudBlobStream, length, null, false, true, executionState, null);
					cloudBlobStream.Commit();
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000B960 File Offset: 0x00009B60
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, true, null, null, null, callback, state);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000B984 File Offset: 0x00009B84
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, true, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000B9AC File Offset: 0x00009BAC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), true, null, null, null, callback, state);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000B9D0 File Offset: 0x00009BD0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromStream(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), true, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromStream(Stream source, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, false, null, null, null, callback, state);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000BA18 File Offset: 0x00009C18
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromStream(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, null, false, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000BA40 File Offset: 0x00009C40
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromStream(Stream source, long length, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), false, null, null, null, callback, state);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000BA64 File Offset: 0x00009C64
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromStream(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromStreamHelper(source, new long?(length), false, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000BCBC File Offset: 0x00009EBC
		[DoesServiceRequest]
		internal ICancellableAsyncResult BeginUploadFromStreamHelper(Stream source, long? length, bool createNew, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
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
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			ExecutionState<NullType> tempExecutionState = CommonUtility.CreateTemporaryExecutionState(options2);
			StorageAsyncResult<NullType> storageAsyncResult = new StorageAsyncResult<NullType>(callback, state);
			ICancellableAsyncResult @object = this.BeginOpenWrite(createNew, accessCondition, options2, operationContext, delegate(IAsyncResult ar)
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

		// Token: 0x0600034B RID: 843 RVA: 0x0000BDDC File Offset: 0x00009FDC
		public void EndUploadFromStream(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		public void EndAppendFromStream(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000BE12 File Offset: 0x0000A012
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source)
		{
			return this.UploadFromStreamAsync(source, CancellationToken.None);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000BE20 File Offset: 0x0000A020
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, cancellationToken);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000BE41 File Offset: 0x0000A041
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000BE53 File Offset: 0x0000A053
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000BE79 File Offset: 0x0000A079
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length)
		{
			return this.UploadFromStreamAsync(source, length, CancellationToken.None);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000BE88 File Offset: 0x0000A088
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long>(new Func<Stream, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, cancellationToken);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000BEAA File Offset: 0x0000A0AA
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromStreamAsync(source, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		[DoesServiceRequest]
		public Task UploadFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromStream), new Action<IAsyncResult>(this.EndUploadFromStream), source, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000BEE6 File Offset: 0x0000A0E6
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source)
		{
			return this.AppendFromStreamAsync(source, CancellationToken.None);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000BEF4 File Offset: 0x0000A0F4
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream>(new Func<Stream, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromStream), new Action<IAsyncResult>(this.EndAppendFromStream), source, cancellationToken);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000BF15 File Offset: 0x0000A115
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendFromStreamAsync(source, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BF27 File Offset: 0x0000A127
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromStream), new Action<IAsyncResult>(this.EndAppendFromStream), source, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000BF4D File Offset: 0x0000A14D
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, long length)
		{
			return this.AppendFromStreamAsync(source, length, CancellationToken.None);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BF5C File Offset: 0x0000A15C
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, long length, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long>(new Func<Stream, long, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromStream), new Action<IAsyncResult>(this.EndAppendFromStream), source, length, cancellationToken);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000BF7E File Offset: 0x0000A17E
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendFromStreamAsync(source, length, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000BF92 File Offset: 0x0000A192
		[DoesServiceRequest]
		public Task AppendFromStreamAsync(Stream source, long length, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<Stream, long, AccessCondition, BlobRequestOptions, OperationContext>(new Func<Stream, long, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromStream), new Action<IAsyncResult>(this.EndAppendFromStream), source, length, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000BFBC File Offset: 0x0000A1BC
		[DoesServiceRequest]
		public void UploadFromFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			using (FileStream fileStream = new FileStream(path, mode, FileAccess.Read))
			{
				this.UploadFromStream(fileStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C008 File Offset: 0x0000A208
		[DoesServiceRequest]
		public void AppendFromFile(string path, FileMode mode, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("path", path);
			using (FileStream fileStream = new FileStream(path, mode, FileAccess.Read))
			{
				this.AppendFromStream(fileStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000C054 File Offset: 0x0000A254
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C064 File Offset: 0x0000A264
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

		// Token: 0x06000361 RID: 865 RVA: 0x0000C0E8 File Offset: 0x0000A2E8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromFile(string path, FileMode mode, AsyncCallback callback, object state)
		{
			return this.BeginAppendFromFile(path, mode, null, null, null, callback, state);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C0F8 File Offset: 0x0000A2F8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromFile(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
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
				ICancellableAsyncResult @object = this.BeginAppendFromStream(fileStream, accessCondition, options, operationContext, new AsyncCallback(this.UploadFromFileCallback), storageAsyncResult);
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

		// Token: 0x06000363 RID: 867 RVA: 0x0000C17C File Offset: 0x0000A37C
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

		// Token: 0x06000364 RID: 868 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
		public void EndUploadFromFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C200 File Offset: 0x0000A400
		public void EndAppendFromFile(IAsyncResult asyncResult)
		{
			StorageAsyncResult<NullType> storageAsyncResult = (StorageAsyncResult<NullType>)asyncResult;
			storageAsyncResult.End();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C21A File Offset: 0x0000A41A
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode)
		{
			return this.UploadFromFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000C229 File Offset: 0x0000A429
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, cancellationToken);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000C24B File Offset: 0x0000A44B
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000C25F File Offset: 0x0000A45F
		[DoesServiceRequest]
		public Task UploadFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromFile), new Action<IAsyncResult>(this.EndUploadFromFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C287 File Offset: 0x0000A487
		[DoesServiceRequest]
		public Task AppendFromFileAsync(string path, FileMode mode)
		{
			return this.AppendFromFileAsync(path, mode, CancellationToken.None);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000C296 File Offset: 0x0000A496
		[DoesServiceRequest]
		public Task AppendFromFileAsync(string path, FileMode mode, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode>(new Func<string, FileMode, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromFile), new Action<IAsyncResult>(this.EndAppendFromFile), path, mode, cancellationToken);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
		[DoesServiceRequest]
		public Task AppendFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendFromFileAsync(path, mode, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		[DoesServiceRequest]
		public Task AppendFromFileAsync(string path, FileMode mode, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, FileMode, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromFile), new Action<IAsyncResult>(this.EndAppendFromFile), path, mode, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		[DoesServiceRequest]
		public void UploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(buffer, index, count))
			{
				this.UploadFromStream(syncMemoryStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C340 File Offset: 0x0000A540
		[DoesServiceRequest]
		public void AppendFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			using (SyncMemoryStream syncMemoryStream = new SyncMemoryStream(buffer, index, count))
			{
				this.AppendFromStream(syncMemoryStream, accessCondition, options, operationContext);
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C38C File Offset: 0x0000A58C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return this.BeginUploadFromByteArray(buffer, index, count, null, null, null, callback, state);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C3AC File Offset: 0x0000A5AC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			SyncMemoryStream source = new SyncMemoryStream(buffer, index, count);
			return this.BeginUploadFromStream(source, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C3DE File Offset: 0x0000A5DE
		public void EndUploadFromByteArray(IAsyncResult asyncResult)
		{
			this.EndUploadFromStream(asyncResult);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C3E8 File Offset: 0x0000A5E8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromByteArray(byte[] buffer, int index, int count, AsyncCallback callback, object state)
		{
			return this.BeginAppendFromByteArray(buffer, index, count, null, null, null, callback, state);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C408 File Offset: 0x0000A608
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendFromByteArray(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("buffer", buffer);
			SyncMemoryStream source = new SyncMemoryStream(buffer, index, count);
			return this.BeginAppendFromStream(source, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C43A File Offset: 0x0000A63A
		public void EndAppendFromByteArray(IAsyncResult asyncResult)
		{
			this.EndAppendFromStream(asyncResult);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C443 File Offset: 0x0000A643
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, CancellationToken.None);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C453 File Offset: 0x0000A653
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, cancellationToken);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000C477 File Offset: 0x0000A677
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000C490 File Offset: 0x0000A690
		[DoesServiceRequest]
		public Task UploadFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext>(new Func<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadFromByteArray), new Action<IAsyncResult>(this.EndUploadFromByteArray), buffer, index, count, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C4C5 File Offset: 0x0000A6C5
		[DoesServiceRequest]
		public Task AppendFromByteArrayAsync(byte[] buffer, int index, int count)
		{
			return this.AppendFromByteArrayAsync(buffer, index, count, CancellationToken.None);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C4D5 File Offset: 0x0000A6D5
		[DoesServiceRequest]
		public Task AppendFromByteArrayAsync(byte[] buffer, int index, int count, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromByteArray), new Action<IAsyncResult>(this.EndAppendFromByteArray), buffer, index, count, cancellationToken);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C4F9 File Offset: 0x0000A6F9
		[DoesServiceRequest]
		public Task AppendFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendFromByteArrayAsync(buffer, index, count, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C510 File Offset: 0x0000A710
		[DoesServiceRequest]
		public Task AppendFromByteArrayAsync(byte[] buffer, int index, int count, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext>(new Func<byte[], int, int, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendFromByteArray), new Action<IAsyncResult>(this.EndAppendFromByteArray), buffer, index, count, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C548 File Offset: 0x0000A748
		[DoesServiceRequest]
		public void UploadText(string content, Encoding encoding = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			this.UploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C584 File Offset: 0x0000A784
		[DoesServiceRequest]
		public void AppendText(string content, Encoding encoding = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			this.AppendFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000C5BD File Offset: 0x0000A7BD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, AsyncCallback callback, object state)
		{
			return this.BeginUploadText(content, null, null, null, null, callback, state);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUploadText(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			return this.BeginUploadFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C609 File Offset: 0x0000A809
		public void EndUploadText(IAsyncResult asyncResult)
		{
			this.EndUploadFromByteArray(asyncResult);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000C612 File Offset: 0x0000A812
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendText(string content, AsyncCallback callback, object state)
		{
			return this.BeginAppendText(content, null, null, null, null, callback, state);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C624 File Offset: 0x0000A824
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendText(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("content", content);
			byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(content);
			return this.BeginAppendFromByteArray(bytes, 0, bytes.Length, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000C661 File Offset: 0x0000A861
		public void EndAppendText(IAsyncResult asyncResult)
		{
			this.EndAppendFromByteArray(asyncResult);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000C66A File Offset: 0x0000A86A
		[DoesServiceRequest]
		public Task UploadTextAsync(string content)
		{
			return this.UploadTextAsync(content, CancellationToken.None);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000C678 File Offset: 0x0000A878
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, cancellationToken);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000C699 File Offset: 0x0000A899
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.UploadTextAsync(content, encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000C6AD File Offset: 0x0000A8AD
		[DoesServiceRequest]
		public Task UploadTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUploadText), new Action<IAsyncResult>(this.EndUploadText), content, encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000C6D5 File Offset: 0x0000A8D5
		[DoesServiceRequest]
		public Task AppendTextAsync(string content)
		{
			return this.AppendTextAsync(content, CancellationToken.None);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000C6E3 File Offset: 0x0000A8E3
		[DoesServiceRequest]
		public Task AppendTextAsync(string content, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendText), new Action<IAsyncResult>(this.EndAppendText), content, cancellationToken);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000C704 File Offset: 0x0000A904
		[DoesServiceRequest]
		public Task AppendTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendTextAsync(content, encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000C718 File Offset: 0x0000A918
		[DoesServiceRequest]
		public Task AppendTextAsync(string content, Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext>(new Func<string, Encoding, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendText), new Action<IAsyncResult>(this.EndAppendText), content, encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000C740 File Offset: 0x0000A940
		[DoesServiceRequest]
		public void CreateOrReplace(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000C77C File Offset: 0x0000A97C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateOrReplace(AsyncCallback callback, object state)
		{
			return this.BeginCreateOrReplace(null, null, null, callback, state);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000C78C File Offset: 0x0000A98C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateOrReplace(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
		public void EndCreateOrReplace(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000C7C9 File Offset: 0x0000A9C9
		[DoesServiceRequest]
		public Task CreateOrReplaceAsync()
		{
			return this.CreateOrReplaceAsync(CancellationToken.None);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000C7D6 File Offset: 0x0000A9D6
		[DoesServiceRequest]
		public Task CreateOrReplaceAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateOrReplace), new Action<IAsyncResult>(this.EndCreateOrReplace), cancellationToken);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000C7F6 File Offset: 0x0000A9F6
		[DoesServiceRequest]
		public Task CreateOrReplaceAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateOrReplaceAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000C806 File Offset: 0x0000AA06
		[DoesServiceRequest]
		public Task CreateOrReplaceAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateOrReplace), new Action<IAsyncResult>(this.EndCreateOrReplace), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000C82C File Offset: 0x0000AA2C
		[DoesServiceRequest]
		public long AppendBlock(Stream blockData, string contentMD5 = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("blockData", blockData);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			bool flag = string.IsNullOrEmpty(contentMD5) && blobRequestOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			Stream stream = blockData;
			bool flag2 = false;
			long result;
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
				result = Executor.ExecuteSync<long>(this.AppendBlockImpl(stream, contentMD5, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
			}
			finally
			{
				if (flag2)
				{
					stream.Dispose();
				}
			}
			return result;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000C93C File Offset: 0x0000AB3C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendBlock(Stream blockData, AsyncCallback callback, object state)
		{
			return this.BeginAppendBlock(blockData, null, null, null, null, callback, state);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000C94B File Offset: 0x0000AB4B
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendBlock(Stream blockData, string contentMD5, AsyncCallback callback, object state)
		{
			return this.BeginAppendBlock(blockData, contentMD5, null, null, null, callback, state);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000CA58 File Offset: 0x0000AC58
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAppendBlock(Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("blockData", blockData);
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			bool requiresContentMD5 = string.IsNullOrEmpty(contentMD5) && modifiedOptions.UseTransactionalMD5.Value;
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<long> storageAsyncResult = new StorageAsyncResult<long>(callback, state);
			if (blockData.CanSeek && !requiresContentMD5)
			{
				this.AppendBlockHandler(blockData, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
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
						this.AppendBlockHandler(seekableStream, contentMD5, accessCondition, modifiedOptions, operationContext, storageAsyncResult);
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
					}
				});
			}
			return storageAsyncResult;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000CC68 File Offset: 0x0000AE68
		private void AppendBlockHandler(Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<long> storageAsyncResult)
		{
			lock (storageAsyncResult.CancellationLockerObject)
			{
				ICancellableAsyncResult @object = Executor.BeginExecuteAsync<long>(this.AppendBlockImpl(blockData, contentMD5, accessCondition, options), options.RetryPolicy, operationContext, delegate(IAsyncResult ar)
				{
					storageAsyncResult.UpdateCompletedSynchronously(ar.CompletedSynchronously);
					try
					{
						storageAsyncResult.Result = Executor.EndExecuteAsync<long>(ar);
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

		// Token: 0x0600039B RID: 923 RVA: 0x0000CD18 File Offset: 0x0000AF18
		public long EndAppendBlock(IAsyncResult asyncResult)
		{
			StorageAsyncResult<long> storageAsyncResult = (StorageAsyncResult<long>)asyncResult;
			long result;
			try
			{
				storageAsyncResult.End();
				result = storageAsyncResult.Result;
			}
			finally
			{
				if (storageAsyncResult.OperationState != null)
				{
					MultiBufferMemoryStream multiBufferMemoryStream = (MultiBufferMemoryStream)storageAsyncResult.OperationState;
					multiBufferMemoryStream.Dispose();
				}
			}
			return result;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000CD68 File Offset: 0x0000AF68
		[DoesServiceRequest]
		public Task<long> AppendBlockAsync(Stream blockData, string contentMD5 = null)
		{
			return this.AppendBlockAsync(blockData, contentMD5, CancellationToken.None);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000CD77 File Offset: 0x0000AF77
		[DoesServiceRequest]
		public Task<long> AppendBlockAsync(Stream blockData, string contentMD5, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Stream, string, long>(new Func<Stream, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendBlock), new Func<IAsyncResult, long>(this.EndAppendBlock), blockData, contentMD5, cancellationToken);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000CD99 File Offset: 0x0000AF99
		[DoesServiceRequest]
		public Task<long> AppendBlockAsync(Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AppendBlockAsync(blockData, contentMD5, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CDAD File Offset: 0x0000AFAD
		[DoesServiceRequest]
		public Task<long> AppendBlockAsync(Stream blockData, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Stream, string, AccessCondition, BlobRequestOptions, OperationContext, long>(new Func<Stream, string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAppendBlock), new Func<IAsyncResult, long>(this.EndAppendBlock), blockData, contentMD5, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000CDD8 File Offset: 0x0000AFD8
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

		// Token: 0x060003A1 RID: 929 RVA: 0x0000CE34 File Offset: 0x0000B034
		public ICancellableAsyncResult BeginDownloadText(AsyncCallback callback, object state)
		{
			return this.BeginDownloadText(null, null, null, null, callback, state);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000CE44 File Offset: 0x0000B044
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

		// Token: 0x060003A3 RID: 931 RVA: 0x0000CEA0 File Offset: 0x0000B0A0
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

		// Token: 0x060003A4 RID: 932 RVA: 0x0000CF24 File Offset: 0x0000B124
		public string EndDownloadText(IAsyncResult asyncResult)
		{
			StorageAsyncResult<string> storageAsyncResult = (StorageAsyncResult<string>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000CF44 File Offset: 0x0000B144
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync()
		{
			return this.DownloadTextAsync(CancellationToken.None);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000CF51 File Offset: 0x0000B151
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), cancellationToken);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000CF71 File Offset: 0x0000B171
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DownloadTextAsync(encoding, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000CF83 File Offset: 0x0000B183
		[DoesServiceRequest]
		public Task<string> DownloadTextAsync(Encoding encoding, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Encoding, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<Encoding, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDownloadText), new Func<IAsyncResult, string>(this.EndDownloadText), encoding, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000CFA9 File Offset: 0x0000B1A9
		[DoesServiceRequest]
		public string StartCopy(CloudAppendBlob source, AccessCondition sourceAccessCondition = null, AccessCondition destAccessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return base.StartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000CFBD File Offset: 0x0000B1BD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudAppendBlob source, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), callback, state);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000CFCD File Offset: 0x0000B1CD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginStartCopy(CloudAppendBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return base.BeginStartCopy(CloudBlob.SourceBlobToUri(source), sourceAccessCondition, destAccessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000CFE5 File Offset: 0x0000B1E5
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudAppendBlob source)
		{
			return this.StartCopyAsync(source, CancellationToken.None);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000CFF3 File Offset: 0x0000B1F3
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudAppendBlob source, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudAppendBlob, string>(new Func<CloudAppendBlob, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, cancellationToken);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000D014 File Offset: 0x0000B214
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudAppendBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.StartCopyAsync(source, sourceAccessCondition, destAccessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000D028 File Offset: 0x0000B228
		[DoesServiceRequest]
		public Task<string> StartCopyAsync(CloudAppendBlob source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudAppendBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<CloudAppendBlob, AccessCondition, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginStartCopy), new Func<IAsyncResult, string>(base.EndStartCopy), source, sourceAccessCondition, destAccessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000D050 File Offset: 0x0000B250
		[DoesServiceRequest]
		public CloudAppendBlob CreateSnapshot(IDictionary<string, string> metadata = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			return Executor.ExecuteSync<CloudAppendBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000D08D File Offset: 0x0000B28D
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(AsyncCallback callback, object state)
		{
			return this.BeginCreateSnapshot(null, null, null, null, callback, state);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000D09C File Offset: 0x0000B29C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateSnapshot(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			this.attributes.AssertNoSnapshot();
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.AppendBlob, base.ServiceClient, true);
			return Executor.BeginExecuteAsync<CloudAppendBlob>(this.CreateSnapshotImpl(metadata, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000D0DD File Offset: 0x0000B2DD
		public CloudAppendBlob EndCreateSnapshot(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<CloudAppendBlob>(asyncResult);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D0E5 File Offset: 0x0000B2E5
		[DoesServiceRequest]
		public Task<CloudAppendBlob> CreateSnapshotAsync()
		{
			return this.CreateSnapshotAsync(CancellationToken.None);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000D0F2 File Offset: 0x0000B2F2
		[DoesServiceRequest]
		public Task<CloudAppendBlob> CreateSnapshotAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudAppendBlob>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudAppendBlob>(this.EndCreateSnapshot), cancellationToken);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D112 File Offset: 0x0000B312
		[DoesServiceRequest]
		public Task<CloudAppendBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateSnapshotAsync(metadata, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D124 File Offset: 0x0000B324
		[DoesServiceRequest]
		public Task<CloudAppendBlob> CreateSnapshotAsync(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, CloudAppendBlob>(new Func<IDictionary<string, string>, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateSnapshot), new Func<IAsyncResult, CloudAppendBlob>(this.EndCreateSnapshot), metadata, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D1B8 File Offset: 0x0000B3B8
		private RESTCommand<NullType> CreateImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Put(uri, serverTimeout, this.Properties, BlobType.AppendBlob, 0L, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				BlobHttpWebRequestFactory.AddMetadata(r, base.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				base.Properties.Length = 0L;
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D2FC File Offset: 0x0000B4FC
		internal RESTCommand<long> AppendBlockImpl(Stream source, string contentMD5, AccessCondition accessCondition, BlobRequestOptions options)
		{
			options.AssertNoEncryptionPolicyOrStrictMode();
			long offset = source.Position;
			RESTCommand<long> restcommand = new RESTCommand<long>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<long>(restcommand);
			restcommand.SendStream = source;
			restcommand.RecoveryAction = delegate(StorageCommandBase<long> cmd, Exception ex, OperationContext ctx)
			{
				RecoveryActions.SeekStream<long>(cmd, offset);
			};
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.AppendBlock(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (!string.IsNullOrEmpty(contentMD5))
				{
					r.Headers[HttpRequestHeader.ContentMd5] = contentMD5;
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<long> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				long num = -1L;
				if (resp.Headers.AllKeys.Contains("x-ms-blob-append-offset"))
				{
					num = long.Parse(resp.Headers["x-ms-blob-append-offset"], CultureInfo.InvariantCulture);
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<long>(HttpStatusCode.Created, resp, num, cmd, ex);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(this.attributes, resp, false);
				return num;
			};
			return restcommand;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D494 File Offset: 0x0000B694
		internal RESTCommand<CloudAppendBlob> CreateSnapshotImpl(IDictionary<string, string> metadata, AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<CloudAppendBlob> restcommand = new RESTCommand<CloudAppendBlob>(base.ServiceClient.Credentials, this.attributes.StorageUri);
			options.ApplyToStorageCommand<CloudAppendBlob>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.Snapshot(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (metadata != null)
				{
					BlobHttpWebRequestFactory.AddMetadata(r, metadata);
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(base.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<CloudAppendBlob> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<CloudAppendBlob>(HttpStatusCode.Created, resp, null, cmd, ex);
				DateTimeOffset value = NavigationHelper.ParseSnapshotTime(BlobHttpResponseParsers.GetSnapshotTime(resp));
				CloudAppendBlob cloudAppendBlob = new CloudAppendBlob(this.Name, new DateTimeOffset?(value), this.Container);
				cloudAppendBlob.attributes.Metadata = new Dictionary<string, string>(metadata ?? this.Metadata);
				cloudAppendBlob.attributes.Properties = new BlobProperties(this.Properties);
				CloudBlob.UpdateETagLMTLengthAndSequenceNumber(cloudAppendBlob.attributes, resp, false);
				return cloudAppendBlob;
			};
			return restcommand;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D533 File Offset: 0x0000B733
		public CloudAppendBlob(Uri blobAbsoluteUri) : this(blobAbsoluteUri, null)
		{
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000D540 File Offset: 0x0000B740
		public CloudAppendBlob(Uri blobAbsoluteUri, StorageCredentials credentials) : this(blobAbsoluteUri, null, credentials)
		{
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000D55E File Offset: 0x0000B75E
		public CloudAppendBlob(Uri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials) : this(new StorageUri(blobAbsoluteUri), snapshotTime, credentials)
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000D56E File Offset: 0x0000B76E
		public CloudAppendBlob(StorageUri blobAbsoluteUri, DateTimeOffset? snapshotTime, StorageCredentials credentials)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobAbsoluteUri, snapshotTime, credentials);
			base.Properties.BlobType = BlobType.AppendBlob;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000D590 File Offset: 0x0000B790
		internal CloudAppendBlob(string blobName, DateTimeOffset? snapshotTime, CloudBlobContainer container)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(blobName, snapshotTime, container);
			base.Properties.BlobType = BlobType.AppendBlob;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000D5B2 File Offset: 0x0000B7B2
		internal CloudAppendBlob(BlobAttributes attributes, CloudBlobClient serviceClient)
		{
			this.streamWriteSizeInBytes = 4194304;
			base..ctor(attributes, serviceClient);
			base.Properties.BlobType = BlobType.AppendBlob;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000D5D3 File Offset: 0x0000B7D3
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x0000D5DB File Offset: 0x0000B7DB
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

		// Token: 0x040000A7 RID: 167
		private int streamWriteSizeInBytes;
	}
}
