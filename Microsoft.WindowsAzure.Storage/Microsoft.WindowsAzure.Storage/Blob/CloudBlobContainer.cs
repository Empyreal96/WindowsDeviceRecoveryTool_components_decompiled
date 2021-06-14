using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
	// Token: 0x0200001A RID: 26
	public sealed class CloudBlobContainer
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x0000E747 File Offset: 0x0000C947
		[DoesServiceRequest]
		public void Create(BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			this.Create(BlobContainerPublicAccessType.Off, requestOptions, operationContext);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000E754 File Offset: 0x0000C954
		[DoesServiceRequest]
		public void Create(BlobContainerPublicAccessType accessType, BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateContainerImpl(blobRequestOptions, accessType), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000E785 File Offset: 0x0000C985
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(AsyncCallback callback, object state)
		{
			return this.BeginCreate(null, null, callback, state);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000E791 File Offset: 0x0000C991
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginCreate(BlobContainerPublicAccessType.Off, options, operationContext, callback, state);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateContainerImpl(blobRequestOptions, accessType), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000E7D4 File Offset: 0x0000C9D4
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000E7DD File Offset: 0x0000C9DD
		[DoesServiceRequest]
		public Task CreateAsync()
		{
			return this.CreateAsync(CancellationToken.None);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000E7EA File Offset: 0x0000C9EA
		[DoesServiceRequest]
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), cancellationToken);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000E80A File Offset: 0x0000CA0A
		[DoesServiceRequest]
		public Task CreateAsync(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(accessType, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000E81A File Offset: 0x0000CA1A
		[DoesServiceRequest]
		public Task CreateAsync(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<BlobContainerPublicAccessType, BlobRequestOptions, OperationContext>(new Func<BlobContainerPublicAccessType, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), accessType, options, operationContext, cancellationToken);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000E83E File Offset: 0x0000CA3E
		[DoesServiceRequest]
		public bool CreateIfNotExists(BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			return this.CreateIfNotExists(BlobContainerPublicAccessType.Off, requestOptions, operationContext);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000E84C File Offset: 0x0000CA4C
		[DoesServiceRequest]
		public bool CreateIfNotExists(BlobContainerPublicAccessType accessType, BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			BlobRequestOptions requestOptions2 = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			bool flag = this.Exists(true, requestOptions2, operationContext);
			if (flag)
			{
				return false;
			}
			bool result;
			try
			{
				this.Create(accessType, requestOptions2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 409)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.ContainerAlreadyExists))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(null, null, callback, state);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(BlobContainerPublicAccessType.Off, options, operationContext, callback, state);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000E904 File Offset: 0x0000CB04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = blobRequestOptions,
				OperationContext = operationContext
			};
			this.CreateIfNotExistsHandler(accessType, blobRequestOptions, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000EB5C File Offset: 0x0000CD5C
		private void CreateIfNotExistsHandler(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
		{
			ICancellableAsyncResult @object = this.BeginExists(true, options, operationContext, delegate(IAsyncResult existsResult)
			{
				storageAsyncResult.UpdateCompletedSynchronously(existsResult.CompletedSynchronously);
				lock (storageAsyncResult.CancellationLockerObject)
				{
					storageAsyncResult.CancelDelegate = null;
					try
					{
						bool flag2 = this.EndExists(existsResult);
						if (flag2)
						{
							storageAsyncResult.Result = false;
							storageAsyncResult.OnComplete();
						}
						else
						{
							ICancellableAsyncResult object2 = this.BeginCreate(accessType, options, operationContext, delegate(IAsyncResult createResult)
							{
								storageAsyncResult.UpdateCompletedSynchronously(createResult.CompletedSynchronously);
								storageAsyncResult.CancelDelegate = null;
								try
								{
									this.EndCreate(createResult);
									storageAsyncResult.Result = true;
									storageAsyncResult.OnComplete();
								}
								catch (StorageException ex)
								{
									if (ex.RequestInformation.HttpStatusCode == 409)
									{
										if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.ContainerAlreadyExists)
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

		// Token: 0x06000443 RID: 1091 RVA: 0x0000EBCC File Offset: 0x0000CDCC
		public bool EndCreateIfNotExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000EBF7 File Offset: 0x0000CDF7
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync()
		{
			return this.CreateIfNotExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000EC04 File Offset: 0x0000CE04
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), cancellationToken);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000EC24 File Offset: 0x0000CE24
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000EC33 File Offset: 0x0000CE33
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobRequestOptions, OperationContext, bool>(new Func<BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000EC55 File Offset: 0x0000CE55
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(accessType, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000EC65 File Offset: 0x0000CE65
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(BlobContainerPublicAccessType accessType, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobContainerPublicAccessType, BlobRequestOptions, OperationContext, bool>(new Func<BlobContainerPublicAccessType, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), accessType, options, operationContext, cancellationToken);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		[DoesServiceRequest]
		public void Delete(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.DeleteContainerImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000ECBD File Offset: 0x0000CEBD
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, null, callback, state);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000ECCC File Offset: 0x0000CECC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.DeleteContainerImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000ED00 File Offset: 0x0000CF00
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000ED09 File Offset: 0x0000CF09
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000ED16 File Offset: 0x0000CF16
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000ED36 File Offset: 0x0000CF36
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000ED46 File Offset: 0x0000CF46
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000ED6C File Offset: 0x0000CF6C
		[DoesServiceRequest]
		public bool DeleteIfExists(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(true, blobRequestOptions, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(accessCondition, blobRequestOptions, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.ContainerNotFound))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000EE08 File Offset: 0x0000D008
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, null, callback, state);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000EE18 File Offset: 0x0000D018
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = blobRequestOptions,
				OperationContext = operationContext
			};
			this.DeleteIfExistsHandler(accessCondition, blobRequestOptions, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000F070 File Offset: 0x0000D270
		private void DeleteIfExistsHandler(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
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
							ICancellableAsyncResult object2 = this.BeginDelete(accessCondition, options, operationContext, delegate(IAsyncResult deleteResult)
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
										if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == BlobErrorCodeStrings.ContainerNotFound)
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

		// Token: 0x06000456 RID: 1110 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000F10B File Offset: 0x0000D30B
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000F118 File Offset: 0x0000D318
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000F138 File Offset: 0x0000D338
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000F148 File Offset: 0x0000D348
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, BlobRequestOptions, OperationContext, bool>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000F16C File Offset: 0x0000D36C
		[DoesServiceRequest]
		public ICloudBlob GetBlobReferenceFromServer(string blobName, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName);
			return this.ServiceClient.GetBlobReferenceFromServer(blobUri, accessCondition, options, operationContext);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000F1A1 File Offset: 0x0000D3A1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetBlobReferenceFromServer(string blobName, AsyncCallback callback, object state)
		{
			return this.BeginGetBlobReferenceFromServer(blobName, null, null, null, callback, state);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetBlobReferenceFromServer(string blobName, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName);
			return this.ServiceClient.BeginGetBlobReferenceFromServer(blobUri, accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000F1E9 File Offset: 0x0000D3E9
		public ICloudBlob EndGetBlobReferenceFromServer(IAsyncResult asyncResult)
		{
			return this.ServiceClient.EndGetBlobReferenceFromServer(asyncResult);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000F1F7 File Offset: 0x0000D3F7
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(string blobName)
		{
			return this.GetBlobReferenceFromServerAsync(blobName, CancellationToken.None);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000F205 File Offset: 0x0000D405
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(string blobName, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, ICloudBlob>(new Func<string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetBlobReferenceFromServer), new Func<IAsyncResult, ICloudBlob>(this.EndGetBlobReferenceFromServer), blobName, cancellationToken);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000F226 File Offset: 0x0000D426
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(string blobName, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.GetBlobReferenceFromServerAsync(blobName, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000F238 File Offset: 0x0000D438
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(string blobName, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, AccessCondition, BlobRequestOptions, OperationContext, ICloudBlob>(new Func<string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetBlobReferenceFromServer), new Func<IAsyncResult, ICloudBlob>(this.EndGetBlobReferenceFromServer), blobName, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000F2B0 File Offset: 0x0000D4B0
		[DoesServiceRequest]
		public IEnumerable<IListBlobItem> ListBlobs(string prefix = null, bool useFlatBlobListing = false, BlobListingDetails blobListingDetails = BlobListingDetails.None, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return CommonUtility.LazyEnumerable<IListBlobItem>((IContinuationToken token) => this.ListBlobsSegmentedCore(prefix, useFlatBlobListing, blobListingDetails, null, (BlobContinuationToken)token, modifiedOptions, operationContext), long.MaxValue);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000F318 File Offset: 0x0000D518
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmented(null, false, BlobListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000F33C File Offset: 0x0000D53C
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(string prefix, BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmented(prefix, false, BlobListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000F360 File Offset: 0x0000D560
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			ResultSegment<IListBlobItem> resultSegment = this.ListBlobsSegmentedCore(prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options2, operationContext);
			return new BlobResultSegment(resultSegment.Results, (BlobContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000F3A4 File Offset: 0x0000D5A4
		private ResultSegment<IListBlobItem> ListBlobsSegmentedCore(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return Executor.ExecuteSync<ResultSegment<IListBlobItem>>(this.ListBlobsImpl(prefix, maxResults, useFlatBlobListing, blobListingDetails, options, currentToken), options.RetryPolicy, operationContext);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000F3C4 File Offset: 0x0000D5C4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(BlobContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListBlobsSegmented(null, false, BlobListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000F3E8 File Offset: 0x0000D5E8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(string prefix, BlobContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListBlobsSegmented(prefix, false, BlobListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000F410 File Offset: 0x0000D610
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<ResultSegment<IListBlobItem>>(this.ListBlobsImpl(prefix, maxResults, useFlatBlobListing, blobListingDetails, blobRequestOptions, currentToken), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000F44C File Offset: 0x0000D64C
		public BlobResultSegment EndListBlobsSegmented(IAsyncResult asyncResult)
		{
			ResultSegment<IListBlobItem> resultSegment = Executor.EndExecuteAsync<ResultSegment<IListBlobItem>>(asyncResult);
			return new BlobResultSegment(resultSegment.Results, (BlobContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000F476 File Offset: 0x0000D676
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000F484 File Offset: 0x0000D684
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(BlobContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobContinuationToken, BlobResultSegment>(new Func<BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), currentToken, cancellationToken);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000F4A5 File Offset: 0x0000D6A5
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmentedAsync(prefix, currentToken, CancellationToken.None);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, BlobContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, BlobContinuationToken, BlobResultSegment>(new Func<string, BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), prefix, currentToken, cancellationToken);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000F4D8 File Offset: 0x0000D6D8
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ListBlobsSegmentedAsync(prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, BlobResultSegment>(new Func<string, bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000F534 File Offset: 0x0000D734
		[DoesServiceRequest]
		public void SetPermissions(BlobContainerPermissions permissions, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetPermissionsImpl(permissions, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000F567 File Offset: 0x0000D767
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(BlobContainerPermissions permissions, AsyncCallback callback, object state)
		{
			return this.BeginSetPermissions(permissions, null, null, null, callback, state);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000F578 File Offset: 0x0000D778
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(BlobContainerPermissions permissions, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetPermissionsImpl(permissions, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000F5AE File Offset: 0x0000D7AE
		public void EndSetPermissions(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000F5B7 File Offset: 0x0000D7B7
		[DoesServiceRequest]
		public Task SetPermissionsAsync(BlobContainerPermissions permissions)
		{
			return this.SetPermissionsAsync(permissions, CancellationToken.None);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000F5C5 File Offset: 0x0000D7C5
		[DoesServiceRequest]
		public Task SetPermissionsAsync(BlobContainerPermissions permissions, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<BlobContainerPermissions>(new Func<BlobContainerPermissions, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, cancellationToken);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000F5E6 File Offset: 0x0000D7E6
		[DoesServiceRequest]
		public Task SetPermissionsAsync(BlobContainerPermissions permissions, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SetPermissionsAsync(permissions, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		[DoesServiceRequest]
		public Task SetPermissionsAsync(BlobContainerPermissions permissions, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<BlobContainerPermissions, AccessCondition, BlobRequestOptions, OperationContext>(new Func<BlobContainerPermissions, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000F620 File Offset: 0x0000D820
		[DoesServiceRequest]
		public BlobContainerPermissions GetPermissions(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<BlobContainerPermissions>(this.GetPermissionsImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000F650 File Offset: 0x0000D850
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AsyncCallback callback, object state)
		{
			return this.BeginGetPermissions(null, null, null, callback, state);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000F660 File Offset: 0x0000D860
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<BlobContainerPermissions>(this.GetPermissionsImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000F694 File Offset: 0x0000D894
		public BlobContainerPermissions EndGetPermissions(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<BlobContainerPermissions>(asyncResult);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000F69C File Offset: 0x0000D89C
		[DoesServiceRequest]
		public Task<BlobContainerPermissions> GetPermissionsAsync()
		{
			return this.GetPermissionsAsync(CancellationToken.None);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000F6A9 File Offset: 0x0000D8A9
		[DoesServiceRequest]
		public Task<BlobContainerPermissions> GetPermissionsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobContainerPermissions>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, BlobContainerPermissions>(this.EndGetPermissions), cancellationToken);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000F6C9 File Offset: 0x0000D8C9
		[DoesServiceRequest]
		public Task<BlobContainerPermissions> GetPermissionsAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.GetPermissionsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000F6D9 File Offset: 0x0000D8D9
		[DoesServiceRequest]
		public Task<BlobContainerPermissions> GetPermissionsAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, BlobRequestOptions, OperationContext, BlobContainerPermissions>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, BlobContainerPermissions>(this.EndGetPermissions), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000F6FD File Offset: 0x0000D8FD
		[DoesServiceRequest]
		public bool Exists(BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			return this.Exists(false, requestOptions, operationContext);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000F708 File Offset: 0x0000D908
		private bool Exists(bool primaryOnly, BlobRequestOptions requestOptions, OperationContext operationContext)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<bool>(this.ExistsImpl(blobRequestOptions, primaryOnly), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000F738 File Offset: 0x0000D938
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0000F744 File Offset: 0x0000D944
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginExists(false, options, operationContext, callback, state);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x0000F754 File Offset: 0x0000D954
		private ICancellableAsyncResult BeginExists(bool primaryOnly, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(blobRequestOptions, primaryOnly), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0000F788 File Offset: 0x0000D988
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0000F790 File Offset: 0x0000D990
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0000F79D File Offset: 0x0000D99D
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000F7BD File Offset: 0x0000D9BD
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobRequestOptions, OperationContext, bool>(new Func<BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x0000F7F0 File Offset: 0x0000D9F0
		[DoesServiceRequest]
		public void FetchAttributes(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000F821 File Offset: 0x0000DA21
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, null, callback, state);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0000F830 File Offset: 0x0000DA30
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0000F864 File Offset: 0x0000DA64
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000F86D File Offset: 0x0000DA6D
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000F87A File Offset: 0x0000DA7A
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000F89A File Offset: 0x0000DA9A
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000F8AA File Offset: 0x0000DAAA
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
		[DoesServiceRequest]
		public void SetMetadata(AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000F901 File Offset: 0x0000DB01
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, null, callback, state);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000F910 File Offset: 0x0000DB10
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000F944 File Offset: 0x0000DB44
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000F94D File Offset: 0x0000DB4D
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000F95A File Offset: 0x0000DB5A
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000F97A File Offset: 0x0000DB7A
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000F98A File Offset: 0x0000DB8A
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		[DoesServiceRequest]
		public string AcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.AcquireLeaseImpl(leaseTime, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000F9E4 File Offset: 0x0000DBE4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AsyncCallback callback, object state)
		{
			return this.BeginAcquireLease(leaseTime, proposedLeaseId, null, null, null, callback, state);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAcquireLease(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.AcquireLeaseImpl(leaseTime, proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000FA2C File Offset: 0x0000DC2C
		public string EndAcquireLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000FA34 File Offset: 0x0000DC34
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId)
		{
			return this.AcquireLeaseAsync(leaseTime, proposedLeaseId, CancellationToken.None);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000FA43 File Offset: 0x0000DC43
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, string, string>(new Func<TimeSpan?, string, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAcquireLease), new Func<IAsyncResult, string>(this.EndAcquireLease), leaseTime, proposedLeaseId, cancellationToken);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000FA65 File Offset: 0x0000DC65
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.AcquireLeaseAsync(leaseTime, proposedLeaseId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000FA79 File Offset: 0x0000DC79
		[DoesServiceRequest]
		public Task<string> AcquireLeaseAsync(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, string, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<TimeSpan?, string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAcquireLease), new Func<IAsyncResult, string>(this.EndAcquireLease), leaseTime, proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		[DoesServiceRequest]
		public void RenewLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.RenewLeaseImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000FAD5 File Offset: 0x0000DCD5
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginRenewLease(accessCondition, null, null, callback, state);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000FAE4 File Offset: 0x0000DCE4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginRenewLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.RenewLeaseImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000FB18 File Offset: 0x0000DD18
		public void EndRenewLease(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000FB21 File Offset: 0x0000DD21
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition)
		{
			return this.RenewLeaseAsync(accessCondition, CancellationToken.None);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000FB2F File Offset: 0x0000DD2F
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition>(new Func<AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginRenewLease), new Action<IAsyncResult>(this.EndRenewLease), accessCondition, cancellationToken);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000FB50 File Offset: 0x0000DD50
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.RenewLeaseAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000FB60 File Offset: 0x0000DD60
		[DoesServiceRequest]
		public Task RenewLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginRenewLease), new Action<IAsyncResult>(this.EndRenewLease), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000FB84 File Offset: 0x0000DD84
		[DoesServiceRequest]
		public string ChangeLease(string proposedLeaseId, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<string>(this.ChangeLeaseImpl(proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000FBB6 File Offset: 0x0000DDB6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginChangeLease(proposedLeaseId, accessCondition, null, null, callback, state);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
		public ICancellableAsyncResult BeginChangeLease(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<string>(this.ChangeLeaseImpl(proposedLeaseId, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0000FBFE File Offset: 0x0000DDFE
		public string EndChangeLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<string>(asyncResult);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000FC06 File Offset: 0x0000DE06
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition)
		{
			return this.ChangeLeaseAsync(proposedLeaseId, accessCondition, CancellationToken.None);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000FC15 File Offset: 0x0000DE15
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, AccessCondition, string>(new Func<string, AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginChangeLease), new Func<IAsyncResult, string>(this.EndChangeLease), proposedLeaseId, accessCondition, cancellationToken);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000FC37 File Offset: 0x0000DE37
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ChangeLeaseAsync(proposedLeaseId, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000FC49 File Offset: 0x0000DE49
		[DoesServiceRequest]
		public Task<string> ChangeLeaseAsync(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, AccessCondition, BlobRequestOptions, OperationContext, string>(new Func<string, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginChangeLease), new Func<IAsyncResult, string>(this.EndChangeLease), proposedLeaseId, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0000FC70 File Offset: 0x0000DE70
		[DoesServiceRequest]
		public void ReleaseLease(AccessCondition accessCondition, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.ReleaseLeaseImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000FCA1 File Offset: 0x0000DEA1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, AsyncCallback callback, object state)
		{
			return this.BeginReleaseLease(accessCondition, null, null, callback, state);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000FCB0 File Offset: 0x0000DEB0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginReleaseLease(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.ReleaseLeaseImpl(accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0000FCE4 File Offset: 0x0000DEE4
		public void EndReleaseLease(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0000FCED File Offset: 0x0000DEED
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition)
		{
			return this.ReleaseLeaseAsync(accessCondition, CancellationToken.None);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0000FCFB File Offset: 0x0000DEFB
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition>(new Func<AccessCondition, AsyncCallback, object, ICancellableAsyncResult>(this.BeginReleaseLease), new Action<IAsyncResult>(this.EndReleaseLease), accessCondition, cancellationToken);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0000FD1C File Offset: 0x0000DF1C
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ReleaseLeaseAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		[DoesServiceRequest]
		public Task ReleaseLeaseAsync(AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, BlobRequestOptions, OperationContext>(new Func<AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginReleaseLease), new Action<IAsyncResult>(this.EndReleaseLease), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000FD50 File Offset: 0x0000DF50
		[DoesServiceRequest]
		public TimeSpan BreakLease(TimeSpan? breakPeriod = null, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.ExecuteSync<TimeSpan>(this.BreakLeaseImpl(breakPeriod, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000FD82 File Offset: 0x0000DF82
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AsyncCallback callback, object state)
		{
			return this.BeginBreakLease(breakPeriod, null, null, null, callback, state);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000FD90 File Offset: 0x0000DF90
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginBreakLease(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<TimeSpan>(this.BreakLeaseImpl(breakPeriod, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000FDC6 File Offset: 0x0000DFC6
		public TimeSpan EndBreakLease(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<TimeSpan>(asyncResult);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000FDCE File Offset: 0x0000DFCE
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod)
		{
			return this.BreakLeaseAsync(breakPeriod, CancellationToken.None);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, TimeSpan>(new Func<TimeSpan?, AsyncCallback, object, ICancellableAsyncResult>(this.BeginBreakLease), new Func<IAsyncResult, TimeSpan>(this.EndBreakLease), breakPeriod, cancellationToken);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000FDFD File Offset: 0x0000DFFD
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.BreakLeaseAsync(breakPeriod, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000FE0F File Offset: 0x0000E00F
		[DoesServiceRequest]
		public Task<TimeSpan> BreakLeaseAsync(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, AccessCondition, BlobRequestOptions, OperationContext, TimeSpan>(new Func<TimeSpan?, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginBreakLease), new Func<IAsyncResult, TimeSpan>(this.EndBreakLease), breakPeriod, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0000FE98 File Offset: 0x0000E098
		internal RESTCommand<string> AcquireLeaseImpl(TimeSpan? leaseTime, string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int leaseDuration = -1;
			if (leaseTime != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("leaseTime", leaseTime.Value, TimeSpan.FromSeconds(1.0), TimeSpan.MaxValue);
				leaseDuration = (int)leaseTime.Value.TotalSeconds;
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Acquire, proposedLeaseId, new int?(leaseDuration), null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.Created, resp, null, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		internal RESTCommand<NullType> RenewLeaseImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when renewing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Renew, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000100D8 File Offset: 0x0000E2D8
		internal RESTCommand<string> ChangeLeaseImpl(string proposedLeaseId, AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			CommonUtility.AssertNotNull("proposedLeaseId", proposedLeaseId);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when changing a lease.", "accessCondition");
			}
			RESTCommand<string> restcommand = new RESTCommand<string>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<string>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Change, proposedLeaseId, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<string> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<string>(HttpStatusCode.OK, resp, null, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return BlobHttpResponseParsers.GetLeaseId(resp);
			};
			return restcommand;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000101FC File Offset: 0x0000E3FC
		internal RESTCommand<NullType> ReleaseLeaseImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			CommonUtility.AssertNotNull("accessCondition", accessCondition);
			if (accessCondition.LeaseId == null)
			{
				throw new ArgumentException("A lease ID must be specified when releasing a lease.", "accessCondition");
			}
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Release, null, null, null, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001033C File Offset: 0x0000E53C
		internal RESTCommand<TimeSpan> BreakLeaseImpl(TimeSpan? breakPeriod, AccessCondition accessCondition, BlobRequestOptions options)
		{
			int? breakSeconds = null;
			if (breakPeriod != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("breakPeriod", breakPeriod.Value, TimeSpan.Zero, TimeSpan.MaxValue);
				breakSeconds = new int?((int)breakPeriod.Value.TotalSeconds);
			}
			RESTCommand<TimeSpan> restcommand = new RESTCommand<TimeSpan>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<TimeSpan>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Lease(uri, serverTimeout, LeaseAction.Break, null, null, breakSeconds, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<TimeSpan> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<TimeSpan>(HttpStatusCode.Accepted, resp, TimeSpan.Zero, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				int? remainingLeaseTime = BlobHttpResponseParsers.GetRemainingLeaseTime(resp);
				if (remainingLeaseTime == null)
				{
					throw new StorageException(cmd.CurrentResult, "Valid lease time expected but not received from the service.", null);
				}
				return TimeSpan.FromSeconds((double)remainingLeaseTime.Value);
			};
			return restcommand;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00010458 File Offset: 0x0000E658
		private RESTCommand<NullType> CreateContainerImpl(BlobRequestOptions options, BlobContainerPublicAccessType accessType)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Create(uri, serverTimeout, useVersionHeader, ctx, accessType));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				ContainerHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001051C File Offset: 0x0000E71C
		private RESTCommand<NullType> DeleteContainerImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.Delete(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000105F4 File Offset: 0x0000E7F4
		private RESTCommand<NullType> FetchAttributesImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.GetProperties(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.Properties = ContainerHttpResponseParsers.GetProperties(resp);
				this.Metadata = ContainerHttpResponseParsers.GetMetadata(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000106C4 File Offset: 0x0000E8C4
		private RESTCommand<bool> ExistsImpl(BlobRequestOptions options, bool primaryOnly)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = (primaryOnly ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.GetProperties(uri, serverTimeout, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				this.Properties = ContainerHttpResponseParsers.GetProperties(resp);
				this.Metadata = ContainerHttpResponseParsers.GetMetadata(resp);
				return true;
			};
			return restcommand;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001079C File Offset: 0x0000E99C
		private RESTCommand<NullType> SetMetadataImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				ContainerHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00010878 File Offset: 0x0000EA78
		private RESTCommand<NullType> SetPermissionsImpl(BlobContainerPermissions acl, AccessCondition accessCondition, BlobRequestOptions options)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			BlobRequest.WriteSharedAccessIdentifiers(acl.SharedAccessPolicies, multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.SetAcl(uri, serverTimeout, acl.PublicAccess, accessCondition, useVersionHeader, ctx));
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.UpdateETagAndLastModified(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x000109C4 File Offset: 0x0000EBC4
		private RESTCommand<BlobContainerPermissions> GetPermissionsImpl(AccessCondition accessCondition, BlobRequestOptions options)
		{
			BlobContainerPermissions containerAcl = null;
			RESTCommand<BlobContainerPermissions> restcommand = new RESTCommand<BlobContainerPermissions>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<BlobContainerPermissions>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.GetAcl(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<BlobContainerPermissions> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<BlobContainerPermissions>(HttpStatusCode.OK, resp, null, cmd, ex);
				containerAcl = new BlobContainerPermissions
				{
					PublicAccess = ContainerHttpResponseParsers.GetAcl(resp)
				};
				return containerAcl;
			};
			restcommand.PostProcessResponse = delegate(RESTCommand<BlobContainerPermissions> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ContainerHttpResponseParsers.ReadSharedAccessIdentifiers(cmd.ResponseStream, containerAcl);
				this.UpdateETagAndLastModified(resp);
				return containerAcl;
			};
			return restcommand;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00010A6C File Offset: 0x0000EC6C
		private IListBlobItem SelectListBlobItem(IListBlobEntry protocolItem)
		{
			ListBlobEntry listBlobEntry = protocolItem as ListBlobEntry;
			if (listBlobEntry != null)
			{
				BlobAttributes attributes = listBlobEntry.Attributes;
				attributes.StorageUri = NavigationHelper.AppendPathToUri(this.StorageUri, listBlobEntry.Name);
				if (attributes.Properties.BlobType == BlobType.BlockBlob)
				{
					return new CloudBlockBlob(attributes, this.ServiceClient);
				}
				if (attributes.Properties.BlobType == BlobType.PageBlob)
				{
					return new CloudPageBlob(attributes, this.ServiceClient);
				}
				if (attributes.Properties.BlobType == BlobType.AppendBlob)
				{
					return new CloudAppendBlob(attributes, this.ServiceClient);
				}
				throw new InvalidOperationException("Invalid blob list item returned");
			}
			else
			{
				ListBlobPrefixEntry listBlobPrefixEntry = protocolItem as ListBlobPrefixEntry;
				if (listBlobPrefixEntry != null)
				{
					return this.GetDirectoryReference(listBlobPrefixEntry.Name);
				}
				throw new InvalidOperationException("Invalid blob list item returned");
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00010BD0 File Offset: 0x0000EDD0
		private RESTCommand<ResultSegment<IListBlobItem>> ListBlobsImpl(string prefix, int? maxResults, bool useFlatBlobListing, BlobListingDetails blobListingDetails, BlobRequestOptions options, BlobContinuationToken currentToken)
		{
			if (!useFlatBlobListing && (blobListingDetails & BlobListingDetails.Snapshots) == BlobListingDetails.Snapshots)
			{
				throw new ArgumentException("Listing snapshots is only supported in flat mode (no delimiter). Consider setting the useFlatBlobListing parameter to true.", "blobListingDetails");
			}
			string delimiter = useFlatBlobListing ? null : this.ServiceClient.DefaultDelimiter;
			BlobListingContext listingContext = new BlobListingContext(prefix, maxResults, delimiter, blobListingDetails)
			{
				Marker = ((currentToken != null) ? currentToken.NextMarker : null)
			};
			RESTCommand<ResultSegment<IListBlobItem>> restcommand = new RESTCommand<ResultSegment<IListBlobItem>>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ResultSegment<IListBlobItem>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(currentToken);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.ListBlobs(uri, serverTimeout, listingContext, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<ResultSegment<IListBlobItem>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ResultSegment<IListBlobItem>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<ResultSegment<IListBlobItem>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ListBlobsResponse listBlobsResponse = new ListBlobsResponse(cmd.ResponseStream);
				List<IListBlobItem> result = (from item in listBlobsResponse.Blobs
				select this.SelectListBlobItem(item)).ToList<IListBlobItem>();
				BlobContinuationToken continuationToken = null;
				if (listBlobsResponse.NextMarker != null)
				{
					continuationToken = new BlobContinuationToken
					{
						NextMarker = listBlobsResponse.NextMarker,
						TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation)
					};
				}
				return new ResultSegment<IListBlobItem>(result)
				{
					ContinuationToken = continuationToken
				};
			};
			return restcommand;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00010CD4 File Offset: 0x0000EED4
		private void UpdateETagAndLastModified(HttpWebResponse response)
		{
			BlobContainerProperties properties = ContainerHttpResponseParsers.GetProperties(response);
			this.Properties.ETag = properties.ETag;
			this.Properties.LastModified = properties.LastModified;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00010D0A File Offset: 0x0000EF0A
		public CloudBlobContainer(Uri containerAddress) : this(containerAddress, null)
		{
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00010D14 File Offset: 0x0000EF14
		public CloudBlobContainer(Uri containerAddress, StorageCredentials credentials) : this(new StorageUri(containerAddress), credentials)
		{
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00010D24 File Offset: 0x0000EF24
		public CloudBlobContainer(StorageUri containerAddress, StorageCredentials credentials)
		{
			CommonUtility.AssertNotNull("containerAddress", containerAddress);
			CommonUtility.AssertNotNull("containerAddress", containerAddress.PrimaryUri);
			this.ParseQueryAndVerify(containerAddress, credentials);
			this.Metadata = new Dictionary<string, string>();
			this.Properties = new BlobContainerProperties();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00010D70 File Offset: 0x0000EF70
		internal CloudBlobContainer(string containerName, CloudBlobClient serviceClient) : this(new BlobContainerProperties(), new Dictionary<string, string>(), containerName, serviceClient)
		{
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00010D84 File Offset: 0x0000EF84
		internal CloudBlobContainer(BlobContainerProperties properties, IDictionary<string, string> metadata, string containerName, CloudBlobClient serviceClient)
		{
			this.StorageUri = NavigationHelper.AppendPathToUri(serviceClient.StorageUri, containerName);
			this.ServiceClient = serviceClient;
			this.Name = containerName;
			this.Metadata = metadata;
			this.Properties = properties;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00010DBC File Offset: 0x0000EFBC
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x00010DC4 File Offset: 0x0000EFC4
		public CloudBlobClient ServiceClient { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00010DCD File Offset: 0x0000EFCD
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00010DDA File Offset: 0x0000EFDA
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00010DE2 File Offset: 0x0000EFE2
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00010DEB File Offset: 0x0000EFEB
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00010DF3 File Offset: 0x0000EFF3
		public string Name { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x00010DFC File Offset: 0x0000EFFC
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00010E04 File Offset: 0x0000F004
		public IDictionary<string, string> Metadata { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00010E0D File Offset: 0x0000F00D
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00010E15 File Offset: 0x0000F015
		public BlobContainerProperties Properties { get; private set; }

		// Token: 0x060004E3 RID: 1251 RVA: 0x00010E20 File Offset: 0x0000F020
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			DateTimeOffset? dateTimeOffset;
			this.StorageUri = NavigationHelper.ParseBlobQueryAndVerify(address, out storageCredentials, out dateTimeOffset);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			this.ServiceClient = new CloudBlobClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			this.Name = NavigationHelper.GetContainerNameFromContainerAddress(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00010EA8 File Offset: 0x0000F0A8
		private string GetSharedAccessCanonicalName(string sasVersion)
		{
			string accountName = this.ServiceClient.Credentials.AccountName;
			string name = this.Name;
			string format = "/{0}/{1}/{2}";
			if (sasVersion == "2012-02-12" || sasVersion == "2013-08-15")
			{
				format = "/{1}/{2}";
			}
			return string.Format(CultureInfo.InvariantCulture, format, new object[]
			{
				"blob",
				accountName,
				name
			});
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00010F16 File Offset: 0x0000F116
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null, null);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00010F21 File Offset: 0x0000F121
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier)
		{
			return this.GetSharedAccessSignature(policy, groupPolicyIdentifier, null);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00010F2C File Offset: 0x0000F12C
		public string GetSharedAccessSignature(SharedAccessBlobPolicy policy, string groupPolicyIdentifier, string sasVersion)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string sasVersion2 = SharedAccessSignatureHelper.ValidateSASVersionString(sasVersion);
			string sharedAccessCanonicalName = this.GetSharedAccessCanonicalName(sasVersion2);
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, null, groupPolicyIdentifier, sharedAccessCanonicalName, sasVersion2, key.KeyValue);
			string keyName = key.KeyName;
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, null, groupPolicyIdentifier, "c", hash, keyName, sasVersion2);
			return signature.ToString();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00010FC0 File Offset: 0x0000F1C0
		public CloudPageBlob GetPageBlobReference(string blobName)
		{
			return this.GetPageBlobReference(blobName, null);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00010FDD File Offset: 0x0000F1DD
		public CloudPageBlob GetPageBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			return new CloudPageBlob(blobName, snapshotTime, this);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		public CloudBlockBlob GetBlockBlobReference(string blobName)
		{
			return this.GetBlockBlobReference(blobName, null);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00011011 File Offset: 0x0000F211
		public CloudBlockBlob GetBlockBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			return new CloudBlockBlob(blobName, snapshotTime, this);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00011028 File Offset: 0x0000F228
		public CloudAppendBlob GetAppendBlobReference(string blobName)
		{
			return this.GetAppendBlobReference(blobName, null);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00011045 File Offset: 0x0000F245
		public CloudAppendBlob GetAppendBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			return new CloudAppendBlob(blobName, snapshotTime, this);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001105C File Offset: 0x0000F25C
		public CloudBlob GetBlobReference(string blobName)
		{
			return this.GetBlobReference(blobName, null);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00011079 File Offset: 0x0000F279
		public CloudBlob GetBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			return new CloudBlob(blobName, snapshotTime, this);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00011090 File Offset: 0x0000F290
		public CloudBlobDirectory GetDirectoryReference(string relativeAddress)
		{
			CommonUtility.AssertNotNull("relativeAddress", relativeAddress);
			if (!string.IsNullOrEmpty(relativeAddress) && !relativeAddress.EndsWith(this.ServiceClient.DefaultDelimiter, StringComparison.Ordinal))
			{
				relativeAddress += this.ServiceClient.DefaultDelimiter;
			}
			StorageUri uri = NavigationHelper.AppendPathToUri(this.StorageUri, relativeAddress);
			return new CloudBlobDirectory(uri, relativeAddress, this);
		}
	}
}
