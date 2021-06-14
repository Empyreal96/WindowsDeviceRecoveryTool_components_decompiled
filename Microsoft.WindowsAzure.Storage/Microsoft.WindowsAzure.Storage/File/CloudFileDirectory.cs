using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File.Protocol;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000026 RID: 38
	public sealed class CloudFileDirectory : IListFileItem
	{
		// Token: 0x06000806 RID: 2054 RVA: 0x0001D998 File Offset: 0x0001BB98
		[DoesServiceRequest]
		public void Create(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateDirectoryImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001D9C7 File Offset: 0x0001BBC7
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(AsyncCallback callback, object state)
		{
			return this.BeginCreate(null, null, callback, state);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001D9D4 File Offset: 0x0001BBD4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateDirectoryImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001DA05 File Offset: 0x0001BC05
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001DA0E File Offset: 0x0001BC0E
		[DoesServiceRequest]
		public Task CreateAsync()
		{
			return this.CreateAsync(CancellationToken.None);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001DA1B File Offset: 0x0001BC1B
		[DoesServiceRequest]
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), cancellationToken);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001DA3B File Offset: 0x0001BC3B
		[DoesServiceRequest]
		public Task CreateAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001DA4A File Offset: 0x0001BC4A
		[DoesServiceRequest]
		public Task CreateAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileRequestOptions, OperationContext>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), options, operationContext, cancellationToken);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001DA6C File Offset: 0x0001BC6C
		[DoesServiceRequest]
		public bool CreateIfNotExists(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			bool flag = this.Exists(requestOptions, operationContext);
			if (flag)
			{
				return false;
			}
			bool result;
			try
			{
				this.Create(requestOptions, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.ExtendedErrorInformation == null || !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ResourceAlreadyExists))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(null, null, callback, state);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions requestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = requestOptions,
				OperationContext = operationContext
			};
			this.CreateIfNotExistsHandler(options, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001DCF8 File Offset: 0x0001BEF8
		private void CreateIfNotExistsHandler(FileRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
		{
			ICancellableAsyncResult @object = this.BeginExists(options, operationContext, delegate(IAsyncResult existsResult)
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
							return;
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
						return;
					}
					ICancellableAsyncResult object2 = this.BeginCreate(options, operationContext, delegate(IAsyncResult createResult)
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
							if (ex.RequestInformation.ExtendedErrorInformation != null && ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ResourceAlreadyExists)
							{
								storageAsyncResult.Result = false;
								storageAsyncResult.OnComplete();
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
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001DD60 File Offset: 0x0001BF60
		public bool EndCreateIfNotExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001DD8B File Offset: 0x0001BF8B
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync()
		{
			return this.CreateIfNotExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001DD98 File Offset: 0x0001BF98
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), cancellationToken);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001DDB8 File Offset: 0x0001BFB8
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001DDC7 File Offset: 0x0001BFC7
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, bool>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001DDEC File Offset: 0x0001BFEC
		[DoesServiceRequest]
		public void Delete(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.DeleteDirectoryImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001DE1C File Offset: 0x0001C01C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, null, callback, state);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001DE2C File Offset: 0x0001C02C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.DeleteDirectoryImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001DE5F File Offset: 0x0001C05F
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001DE68 File Offset: 0x0001C068
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001DE75 File Offset: 0x0001C075
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001DE95 File Offset: 0x0001C095
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001DEA5 File Offset: 0x0001C0A5
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001DECC File Offset: 0x0001C0CC
		[DoesServiceRequest]
		public bool DeleteIfExists(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			if (!this.Exists(options, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(accessCondition, options, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001DF20 File Offset: 0x0001C120
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, null, callback, state);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001DF30 File Offset: 0x0001C130
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions requestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = requestOptions,
				OperationContext = operationContext
			};
			this.DeleteIfExistsHandler(accessCondition, options, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001E13C File Offset: 0x0001C33C
		private void DeleteIfExistsHandler(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, StorageAsyncResult<bool> storageAsyncResult)
		{
			ICancellableAsyncResult @object = this.BeginExists(options, operationContext, delegate(IAsyncResult existsResult)
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
							return;
						}
					}
					catch (Exception exception)
					{
						storageAsyncResult.OnComplete(exception);
						return;
					}
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
								storageAsyncResult.Result = false;
								storageAsyncResult.OnComplete();
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
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001E1AC File Offset: 0x0001C3AC
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001E1D7 File Offset: 0x0001C3D7
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001E1E4 File Offset: 0x0001C3E4
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001E204 File Offset: 0x0001C404
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001E214 File Offset: 0x0001C414
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, FileRequestOptions, OperationContext, bool>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001E238 File Offset: 0x0001C438
		[DoesServiceRequest]
		public bool Exists(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient, true);
			return Executor.ExecuteSync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001E266 File Offset: 0x0001C466
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001E274 File Offset: 0x0001C474
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001E2A5 File Offset: 0x0001C4A5
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001E2AD File Offset: 0x0001C4AD
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001E2BA File Offset: 0x0001C4BA
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001E2DA File Offset: 0x0001C4DA
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001E2E9 File Offset: 0x0001C4E9
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, bool>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001E30C File Offset: 0x0001C50C
		[DoesServiceRequest]
		public void FetchAttributes(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001E33C File Offset: 0x0001C53C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, null, callback, state);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001E34C File Offset: 0x0001C54C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001E37F File Offset: 0x0001C57F
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001E388 File Offset: 0x0001C588
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001E395 File Offset: 0x0001C595
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001E3B5 File Offset: 0x0001C5B5
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001E3C5 File Offset: 0x0001C5C5
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001E428 File Offset: 0x0001C628
		[DoesServiceRequest]
		public IEnumerable<IListFileItem> ListFilesAndDirectories(FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions modifiedOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return CommonUtility.LazyEnumerable<IListFileItem>((IContinuationToken token) => this.ListFilesAndDirectoriesSegmentedCore(null, (FileContinuationToken)token, modifiedOptions, operationContext), long.MaxValue);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001E478 File Offset: 0x0001C678
		[DoesServiceRequest]
		public FileResultSegment ListFilesAndDirectoriesSegmented(FileContinuationToken currentToken)
		{
			return this.ListFilesAndDirectoriesSegmented(null, currentToken, null, null);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001E498 File Offset: 0x0001C698
		[DoesServiceRequest]
		public FileResultSegment ListFilesAndDirectoriesSegmented(int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext)
		{
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			ResultSegment<IListFileItem> resultSegment = this.ListFilesAndDirectoriesSegmentedCore(maxResults, currentToken, options2, operationContext);
			return new FileResultSegment(resultSegment.Results, (FileContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001E4D5 File Offset: 0x0001C6D5
		private ResultSegment<IListFileItem> ListFilesAndDirectoriesSegmentedCore(int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext)
		{
			return Executor.ExecuteSync<ResultSegment<IListFileItem>>(this.ListFilesAndDirectoriesImpl(maxResults, options, currentToken), options.RetryPolicy, operationContext);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001E4F0 File Offset: 0x0001C6F0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListFilesAndDirectoriesSegmented(FileContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListFilesAndDirectoriesSegmented(null, currentToken, null, null, callback, state);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001E514 File Offset: 0x0001C714
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListFilesAndDirectoriesSegmented(int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<ResultSegment<IListFileItem>>(this.ListFilesAndDirectoriesImpl(maxResults, fileRequestOptions, currentToken), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001E54C File Offset: 0x0001C74C
		public FileResultSegment EndListFilesAndDirectoriesSegmented(IAsyncResult asyncResult)
		{
			ResultSegment<IListFileItem> resultSegment = Executor.EndExecuteAsync<ResultSegment<IListFileItem>>(asyncResult);
			return new FileResultSegment(resultSegment.Results, (FileContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001E576 File Offset: 0x0001C776
		[DoesServiceRequest]
		public Task<FileResultSegment> ListFilesAndDirectoriesSegmentedAsync(FileContinuationToken currentToken)
		{
			return this.ListFilesAndDirectoriesSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001E584 File Offset: 0x0001C784
		[DoesServiceRequest]
		public Task<FileResultSegment> ListFilesAndDirectoriesSegmentedAsync(FileContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileContinuationToken, FileResultSegment>(new Func<FileContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListFilesAndDirectoriesSegmented), new Func<IAsyncResult, FileResultSegment>(this.EndListFilesAndDirectoriesSegmented), currentToken, cancellationToken);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001E5A5 File Offset: 0x0001C7A5
		[DoesServiceRequest]
		public Task<FileResultSegment> ListFilesAndDirectoriesSegmentedAsync(int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext)
		{
			return this.ListFilesAndDirectoriesSegmentedAsync(maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001E5B7 File Offset: 0x0001C7B7
		[DoesServiceRequest]
		public Task<FileResultSegment> ListFilesAndDirectoriesSegmentedAsync(int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<int?, FileContinuationToken, FileRequestOptions, OperationContext, FileResultSegment>(new Func<int?, FileContinuationToken, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListFilesAndDirectoriesSegmented), new Func<IAsyncResult, FileResultSegment>(this.EndListFilesAndDirectoriesSegmented), maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
		[DoesServiceRequest]
		public void SetMetadata(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001E610 File Offset: 0x0001C810
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, null, callback, state);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001E620 File Offset: 0x0001C820
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001E653 File Offset: 0x0001C853
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001E65C File Offset: 0x0001C85C
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001E669 File Offset: 0x0001C869
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001E689 File Offset: 0x0001C889
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001E699 File Offset: 0x0001C899
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001E6F8 File Offset: 0x0001C8F8
		private RESTCommand<NullType> CreateDirectoryImpl(FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.Create(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				DirectoryHttpWebRequestFactory.AddMetadata(r, this.Metadata);
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

		// Token: 0x0600084C RID: 2124 RVA: 0x0001E7B8 File Offset: 0x0001C9B8
		private RESTCommand<NullType> DeleteDirectoryImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.Delete(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001E88C File Offset: 0x0001CA8C
		private RESTCommand<bool> ExistsImpl(FileRequestOptions options)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.GetProperties(uri, serverTimeout, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				if (resp.StatusCode == HttpStatusCode.PreconditionFailed)
				{
					return true;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				this.Properties = DirectoryHttpResponseParsers.GetProperties(resp);
				return true;
			};
			return restcommand;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001E958 File Offset: 0x0001CB58
		private RESTCommand<NullType> FetchAttributesImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.GetProperties(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.Properties = DirectoryHttpResponseParsers.GetProperties(resp);
				this.Metadata = DirectoryHttpResponseParsers.GetMetadata(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001EA8C File Offset: 0x0001CC8C
		private RESTCommand<ResultSegment<IListFileItem>> ListFilesAndDirectoriesImpl(int? maxResults, FileRequestOptions options, FileContinuationToken currentToken)
		{
			FileListingContext listingContext = new FileListingContext(maxResults)
			{
				Marker = ((currentToken != null) ? currentToken.NextMarker : null)
			};
			RESTCommand<ResultSegment<IListFileItem>> restcommand = new RESTCommand<ResultSegment<IListFileItem>>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ResultSegment<IListFileItem>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(currentToken);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.List(uri, serverTimeout, listingContext, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<ResultSegment<IListFileItem>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ResultSegment<IListFileItem>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<ResultSegment<IListFileItem>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ListFilesAndDirectoriesResponse listFilesAndDirectoriesResponse = new ListFilesAndDirectoriesResponse(cmd.ResponseStream);
				List<IListFileItem> result = (from item in listFilesAndDirectoriesResponse.Files
				select this.SelectListFileItem(item)).ToList<IListFileItem>();
				FileContinuationToken continuationToken = null;
				if (listFilesAndDirectoriesResponse.NextMarker != null)
				{
					continuationToken = new FileContinuationToken
					{
						NextMarker = listFilesAndDirectoriesResponse.NextMarker,
						TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation)
					};
				}
				return new ResultSegment<IListFileItem>(result)
				{
					ContinuationToken = continuationToken
				};
			};
			return restcommand;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001EBA8 File Offset: 0x0001CDA8
		private RESTCommand<NullType> SetMetadataImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => DirectoryHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				DirectoryHttpWebRequestFactory.AddMetadata(r, this.Metadata);
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

		// Token: 0x06000851 RID: 2129 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		private void UpdateETagAndLastModified(HttpWebResponse response)
		{
			FileDirectoryProperties properties = DirectoryHttpResponseParsers.GetProperties(response);
			this.Properties.ETag = properties.ETag;
			this.Properties.LastModified = properties.LastModified;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001EC72 File Offset: 0x0001CE72
		public CloudFileDirectory(Uri directoryAbsoluteUri) : this(new StorageUri(directoryAbsoluteUri), null)
		{
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001EC81 File Offset: 0x0001CE81
		public CloudFileDirectory(Uri directoryAbsoluteUri, StorageCredentials credentials) : this(new StorageUri(directoryAbsoluteUri), credentials)
		{
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001EC90 File Offset: 0x0001CE90
		public CloudFileDirectory(StorageUri directoryAbsoluteUri, StorageCredentials credentials)
		{
			this.Metadata = new Dictionary<string, string>();
			this.Properties = new FileDirectoryProperties();
			this.ParseQueryAndVerify(directoryAbsoluteUri, credentials);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001ECB8 File Offset: 0x0001CEB8
		internal CloudFileDirectory(StorageUri uri, string directoryName, CloudFileShare share)
		{
			CommonUtility.AssertNotNull("uri", uri);
			CommonUtility.AssertNotNull("directoryName", directoryName);
			CommonUtility.AssertNotNull("share", share);
			this.Metadata = new Dictionary<string, string>();
			this.Properties = new FileDirectoryProperties();
			this.StorageUri = uri;
			this.ServiceClient = share.ServiceClient;
			this.share = share;
			this.Name = directoryName;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0001ED23 File Offset: 0x0001CF23
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0001ED2B File Offset: 0x0001CF2B
		public CloudFileClient ServiceClient { get; private set; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0001ED34 File Offset: 0x0001CF34
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0001ED41 File Offset: 0x0001CF41
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0001ED49 File Offset: 0x0001CF49
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001ED52 File Offset: 0x0001CF52
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0001ED5A File Offset: 0x0001CF5A
		public FileDirectoryProperties Properties { get; internal set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0001ED63 File Offset: 0x0001CF63
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0001ED6B File Offset: 0x0001CF6B
		public IDictionary<string, string> Metadata { get; internal set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0001ED74 File Offset: 0x0001CF74
		public CloudFileShare Share
		{
			get
			{
				if (this.share == null)
				{
					this.share = this.ServiceClient.GetShareReference(NavigationHelper.GetShareName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris)));
				}
				return this.share;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x0001EDB0 File Offset: 0x0001CFB0
		public CloudFileDirectory Parent
		{
			get
			{
				string directoryName;
				StorageUri uri;
				if (this.parent == null && NavigationHelper.GetFileParentNameAndAddress(this.StorageUri, new bool?(this.ServiceClient.UsePathStyleUris), out directoryName, out uri))
				{
					this.parent = new CloudFileDirectory(uri, directoryName, this.Share);
				}
				return this.parent;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001EDFF File Offset: 0x0001CFFF
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0001EE07 File Offset: 0x0001D007
		public string Name { get; private set; }

		// Token: 0x06000863 RID: 2147 RVA: 0x0001EE10 File Offset: 0x0001D010
		private IListFileItem SelectListFileItem(IListFileEntry protocolItem)
		{
			ListFileEntry listFileEntry = protocolItem as ListFileEntry;
			if (listFileEntry != null)
			{
				CloudFileAttributes attributes = listFileEntry.Attributes;
				attributes.StorageUri = NavigationHelper.AppendPathToUri(this.StorageUri, listFileEntry.Name);
				return new CloudFile(attributes, this.ServiceClient);
			}
			ListFileDirectoryEntry listFileDirectoryEntry = protocolItem as ListFileDirectoryEntry;
			if (listFileDirectoryEntry != null)
			{
				CloudFileDirectory directoryReference = this.GetDirectoryReference(listFileDirectoryEntry.Name);
				directoryReference.Properties = listFileDirectoryEntry.Properties;
				return directoryReference;
			}
			throw new InvalidOperationException("Invalid file list item returned");
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001EE84 File Offset: 0x0001D084
		public CloudFile GetFileReference(string fileName)
		{
			CommonUtility.AssertNotNullOrEmpty("fileName", fileName);
			StorageUri uri = NavigationHelper.AppendPathToUri(this.StorageUri, fileName);
			return new CloudFile(uri, fileName, this.Share);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
		public CloudFileDirectory GetDirectoryReference(string itemName)
		{
			CommonUtility.AssertNotNullOrEmpty("itemName", itemName);
			StorageUri uri = NavigationHelper.AppendPathToUri(this.StorageUri, itemName);
			return new CloudFileDirectory(uri, itemName, this.Share);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001EEEC File Offset: 0x0001D0EC
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			this.StorageUri = NavigationHelper.ParseFileQueryAndVerify(address, out storageCredentials);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			if (this.ServiceClient == null)
			{
				this.ServiceClient = new CloudFileClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			}
			this.Name = NavigationHelper.GetFileName(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x040000E2 RID: 226
		private CloudFileShare share;

		// Token: 0x040000E3 RID: 227
		private CloudFileDirectory parent;
	}
}
