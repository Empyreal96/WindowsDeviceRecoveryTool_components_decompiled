using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.File.Protocol;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000027 RID: 39
	public sealed class CloudFileShare
	{
		// Token: 0x06000873 RID: 2163 RVA: 0x0001EF78 File Offset: 0x0001D178
		[DoesServiceRequest]
		public void Create(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.CreateShareImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001EFA7 File Offset: 0x0001D1A7
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(AsyncCallback callback, object state)
		{
			return this.BeginCreate(null, null, callback, state);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.CreateShareImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001EFE5 File Offset: 0x0001D1E5
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001EFEE File Offset: 0x0001D1EE
		[DoesServiceRequest]
		public Task CreateAsync()
		{
			return this.CreateAsync(CancellationToken.None);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001EFFB File Offset: 0x0001D1FB
		[DoesServiceRequest]
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), cancellationToken);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001F01B File Offset: 0x0001D21B
		[DoesServiceRequest]
		public Task CreateAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001F02A File Offset: 0x0001D22A
		[DoesServiceRequest]
		public Task CreateAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileRequestOptions, OperationContext>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), options, operationContext, cancellationToken);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001F04C File Offset: 0x0001D24C
		[DoesServiceRequest]
		public bool CreateIfNotExists(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			FileRequestOptions requestOptions2 = FileRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			bool flag = this.Exists(requestOptions2, operationContext);
			if (flag)
			{
				return false;
			}
			bool result;
			try
			{
				this.Create(requestOptions2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 409)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ShareAlreadyExists))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(null, null, callback, state);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001F0F0 File Offset: 0x0001D2F0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = fileRequestOptions,
				OperationContext = operationContext
			};
			this.CreateIfNotExistsHandler(fileRequestOptions, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001F338 File Offset: 0x0001D538
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
							if (ex.RequestInformation.HttpStatusCode == 409)
							{
								if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ShareAlreadyExists)
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
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001F3A0 File Offset: 0x0001D5A0
		public bool EndCreateIfNotExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001F3CB File Offset: 0x0001D5CB
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync()
		{
			return this.CreateIfNotExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001F3D8 File Offset: 0x0001D5D8
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), cancellationToken);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001F3F8 File Offset: 0x0001D5F8
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001F407 File Offset: 0x0001D607
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, bool>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001F42C File Offset: 0x0001D62C
		[DoesServiceRequest]
		public void Delete(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.DeleteShareImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001F45C File Offset: 0x0001D65C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, null, callback, state);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001F46C File Offset: 0x0001D66C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.DeleteShareImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001F49F File Offset: 0x0001D69F
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001F4A8 File Offset: 0x0001D6A8
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001F4B5 File Offset: 0x0001D6B5
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001F4D5 File Offset: 0x0001D6D5
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001F4E5 File Offset: 0x0001D6E5
		[DoesServiceRequest]
		public Task DeleteAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001F50C File Offset: 0x0001D70C
		[DoesServiceRequest]
		public bool DeleteIfExists(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(fileRequestOptions, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(accessCondition, fileRequestOptions, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ShareNotFound))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001F5A4 File Offset: 0x0001D7A4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, null, callback, state);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001F5B4 File Offset: 0x0001D7B4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = fileRequestOptions,
				OperationContext = operationContext
			};
			this.DeleteIfExistsHandler(accessCondition, fileRequestOptions, operationContext, storageAsyncResult);
			return storageAsyncResult;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001F808 File Offset: 0x0001DA08
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
								if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == FileErrorCodeStrings.ShareNotFound)
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
			}, null);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001F878 File Offset: 0x0001DA78
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001F8A3 File Offset: 0x0001DAA3
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001F8B0 File Offset: 0x0001DAB0
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001F8D0 File Offset: 0x0001DAD0
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001F8E0 File Offset: 0x0001DAE0
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, FileRequestOptions, OperationContext, bool>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001F904 File Offset: 0x0001DB04
		[DoesServiceRequest]
		public bool Exists(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient, true);
			return Executor.ExecuteSync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001F932 File Offset: 0x0001DB32
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001F940 File Offset: 0x0001DB40
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001F971 File Offset: 0x0001DB71
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001F979 File Offset: 0x0001DB79
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001F986 File Offset: 0x0001DB86
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001F9A6 File Offset: 0x0001DBA6
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001F9B5 File Offset: 0x0001DBB5
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, bool>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
		[DoesServiceRequest]
		public void FetchAttributes(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001FA08 File Offset: 0x0001DC08
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, null, callback, state);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001FA18 File Offset: 0x0001DC18
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001FA4B File Offset: 0x0001DC4B
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001FA54 File Offset: 0x0001DC54
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001FA61 File Offset: 0x0001DC61
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001FA81 File Offset: 0x0001DC81
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001FA91 File Offset: 0x0001DC91
		[DoesServiceRequest]
		public Task FetchAttributesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001FAB8 File Offset: 0x0001DCB8
		[DoesServiceRequest]
		public FileSharePermissions GetPermissions(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.ExecuteSync<FileSharePermissions>(this.GetPermissionsImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001FAE7 File Offset: 0x0001DCE7
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AsyncCallback callback, object state)
		{
			return this.BeginGetPermissions(null, null, null, callback, state);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<FileSharePermissions>(this.GetPermissionsImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001FB27 File Offset: 0x0001DD27
		public FileSharePermissions EndGetPermissions(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<FileSharePermissions>(asyncResult);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001FB2F File Offset: 0x0001DD2F
		[DoesServiceRequest]
		public Task<FileSharePermissions> GetPermissionsAsync()
		{
			return this.GetPermissionsAsync(CancellationToken.None);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		[DoesServiceRequest]
		public Task<FileSharePermissions> GetPermissionsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileSharePermissions>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, FileSharePermissions>(this.EndGetPermissions), cancellationToken);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001FB5C File Offset: 0x0001DD5C
		[DoesServiceRequest]
		public Task<FileSharePermissions> GetPermissionsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.GetPermissionsAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001FB6C File Offset: 0x0001DD6C
		[DoesServiceRequest]
		public Task<FileSharePermissions> GetPermissionsAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<AccessCondition, FileRequestOptions, OperationContext, FileSharePermissions>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, FileSharePermissions>(this.EndGetPermissions), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001FB90 File Offset: 0x0001DD90
		[DoesServiceRequest]
		public ShareStats GetStats(FileRequestOptions options = null, OperationContext operationContext = null)
		{
			options = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ShareStats>(this.GetStatsImpl(options), options.RetryPolicy, operationContext);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetStats(AsyncCallback callback, object state)
		{
			return this.BeginGetStats(null, null, callback, state);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001FBCC File Offset: 0x0001DDCC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetStats(FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			options = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ShareStats>(this.GetStatsImpl(options), options.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001FBFF File Offset: 0x0001DDFF
		public ShareStats EndGetStats(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ShareStats>(asyncResult);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001FC07 File Offset: 0x0001DE07
		[DoesServiceRequest]
		public Task<ShareStats> GetStatsAsync()
		{
			return this.GetStatsAsync(CancellationToken.None);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001FC14 File Offset: 0x0001DE14
		[DoesServiceRequest]
		public Task<ShareStats> GetStatsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ShareStats>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetStats), new Func<IAsyncResult, ShareStats>(this.EndGetStats), cancellationToken);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001FC34 File Offset: 0x0001DE34
		[DoesServiceRequest]
		public Task<ShareStats> GetStatsAsync(FileRequestOptions options, OperationContext operationContext)
		{
			return this.GetStatsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001FC43 File Offset: 0x0001DE43
		[DoesServiceRequest]
		public Task<ShareStats> GetStatsAsync(FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, ShareStats>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetStats), new Func<IAsyncResult, ShareStats>(this.EndGetStats), options, operationContext, cancellationToken);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001FC68 File Offset: 0x0001DE68
		[DoesServiceRequest]
		public void SetMetadata(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001FC98 File Offset: 0x0001DE98
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, null, callback, state);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001FCDB File Offset: 0x0001DEDB
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001FCE4 File Offset: 0x0001DEE4
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001FCF1 File Offset: 0x0001DEF1
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001FD11 File Offset: 0x0001DF11
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001FD21 File Offset: 0x0001DF21
		[DoesServiceRequest]
		public Task SetMetadataAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001FD48 File Offset: 0x0001DF48
		[DoesServiceRequest]
		public void SetPermissions(FileSharePermissions permissions, AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetPermissionsImpl(permissions, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0001FD7A File Offset: 0x0001DF7A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(FileSharePermissions permissions, AsyncCallback callback, object state)
		{
			return this.BeginSetPermissions(permissions, null, null, null, callback, state);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001FD88 File Offset: 0x0001DF88
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(FileSharePermissions permissions, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetPermissionsImpl(permissions, accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001FDBD File Offset: 0x0001DFBD
		public void EndSetPermissions(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001FDC6 File Offset: 0x0001DFC6
		[DoesServiceRequest]
		public Task SetPermissionsAsync(FileSharePermissions permissions)
		{
			return this.SetPermissionsAsync(permissions, CancellationToken.None);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
		[DoesServiceRequest]
		public Task SetPermissionsAsync(FileSharePermissions permissions, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileSharePermissions>(new Func<FileSharePermissions, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, cancellationToken);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001FDF5 File Offset: 0x0001DFF5
		[DoesServiceRequest]
		public Task SetPermissionsAsync(FileSharePermissions permissions, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetPermissionsAsync(permissions, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001FE07 File Offset: 0x0001E007
		[DoesServiceRequest]
		public Task SetPermissionsAsync(FileSharePermissions permissions, AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileSharePermissions, AccessCondition, FileRequestOptions, OperationContext>(new Func<FileSharePermissions, AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001FE30 File Offset: 0x0001E030
		[DoesServiceRequest]
		public void SetProperties(AccessCondition accessCondition = null, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			Executor.ExecuteSync<NullType>(this.SetPropertiesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001FE60 File Offset: 0x0001E060
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AsyncCallback callback, object state)
		{
			return this.BeginSetProperties(null, null, null, callback, state);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001FE70 File Offset: 0x0001E070
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetProperties(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this.ServiceClient, true);
			return Executor.BeginExecuteAsync<NullType>(this.SetPropertiesImpl(accessCondition, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001FEA3 File Offset: 0x0001E0A3
		public void EndSetProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001FEAC File Offset: 0x0001E0AC
		[DoesServiceRequest]
		public Task SetPropertiesAsync()
		{
			return this.SetPropertiesAsync(CancellationToken.None);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001FEB9 File Offset: 0x0001E0B9
		[DoesServiceRequest]
		public Task SetPropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), cancellationToken);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001FED9 File Offset: 0x0001E0D9
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext)
		{
			return this.SetPropertiesAsync(accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001FEE9 File Offset: 0x0001E0E9
		[DoesServiceRequest]
		public Task SetPropertiesAsync(AccessCondition accessCondition, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<AccessCondition, FileRequestOptions, OperationContext>(new Func<AccessCondition, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetProperties), new Action<IAsyncResult>(this.EndSetProperties), accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001FF50 File Offset: 0x0001E150
		private RESTCommand<NullType> CreateShareImpl(FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.Create(uri, this.Properties, serverTimeout, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				ShareHttpWebRequestFactory.AddMetadata(r, this.Metadata);
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

		// Token: 0x060008CE RID: 2254 RVA: 0x00020000 File Offset: 0x0001E200
		private RESTCommand<NullType> DeleteShareImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.Delete(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000200D8 File Offset: 0x0001E2D8
		private RESTCommand<NullType> FetchAttributesImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.GetProperties(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.Properties = ShareHttpResponseParsers.GetProperties(resp);
				this.Metadata = ShareHttpResponseParsers.GetMetadata(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000201A8 File Offset: 0x0001E3A8
		private RESTCommand<bool> ExistsImpl(FileRequestOptions options)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.GetProperties(uri, serverTimeout, null, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				if (resp.StatusCode == HttpStatusCode.NotFound)
				{
					return false;
				}
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex);
				this.Properties = ShareHttpResponseParsers.GetProperties(resp);
				this.Metadata = ShareHttpResponseParsers.GetMetadata(resp);
				return true;
			};
			return restcommand;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00020290 File Offset: 0x0001E490
		private RESTCommand<FileSharePermissions> GetPermissionsImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			FileSharePermissions shareAcl = null;
			RESTCommand<FileSharePermissions> restcommand = new RESTCommand<FileSharePermissions>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<FileSharePermissions>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.GetAcl(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<FileSharePermissions> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<FileSharePermissions>(HttpStatusCode.OK, resp, null, cmd, ex);
				shareAcl = new FileSharePermissions();
				return shareAcl;
			};
			restcommand.PostProcessResponse = delegate(RESTCommand<FileSharePermissions> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ShareHttpResponseParsers.ReadSharedAccessIdentifiers(cmd.ResponseStream, shareAcl);
				this.UpdateETagAndLastModified(resp);
				return shareAcl;
			};
			return restcommand;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00020364 File Offset: 0x0001E564
		private RESTCommand<ShareStats> GetStatsImpl(FileRequestOptions options)
		{
			RESTCommand<ShareStats> restcommand = new RESTCommand<ShareStats>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ShareStats>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.GetStats(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ShareStats> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ShareStats>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ShareStats> cmd, HttpWebResponse resp, OperationContext ctx) => ShareHttpResponseParsers.ReadShareStats(cmd.ResponseStream));
			return restcommand;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00020470 File Offset: 0x0001E670
		private RESTCommand<NullType> SetMetadataImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.SetMetadata(uri, serverTimeout, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				ShareHttpWebRequestFactory.AddMetadata(r, this.Metadata);
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

		// Token: 0x060008D4 RID: 2260 RVA: 0x00020540 File Offset: 0x0001E740
		private RESTCommand<NullType> SetPermissionsImpl(FileSharePermissions acl, AccessCondition accessCondition, FileRequestOptions options)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			FileRequest.WriteSharedAccessIdentifiers(acl.SharedAccessPolicies, multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.SetAcl(uri, serverTimeout, FileSharePublicAccessType.Off, accessCondition, useVersionHeader, ctx));
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

		// Token: 0x060008D5 RID: 2261 RVA: 0x00020658 File Offset: 0x0001E858
		private RESTCommand<NullType> SetPropertiesImpl(AccessCondition accessCondition, FileRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.SetProperties(uri, serverTimeout, this.Properties, accessCondition, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				ShareHttpWebRequestFactory.AddMetadata(r, this.Metadata);
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

		// Token: 0x060008D6 RID: 2262 RVA: 0x000206EC File Offset: 0x0001E8EC
		private void UpdateETagAndLastModified(HttpWebResponse response)
		{
			FileShareProperties properties = ShareHttpResponseParsers.GetProperties(response);
			this.Properties.ETag = properties.ETag;
			this.Properties.LastModified = properties.LastModified;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00020722 File Offset: 0x0001E922
		public CloudFileShare(Uri shareAddress) : this(shareAddress, null)
		{
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002072C File Offset: 0x0001E92C
		public CloudFileShare(Uri shareAddress, StorageCredentials credentials) : this(new StorageUri(shareAddress), credentials)
		{
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002073C File Offset: 0x0001E93C
		public CloudFileShare(StorageUri shareAddress, StorageCredentials credentials)
		{
			CommonUtility.AssertNotNull("shareAddress", shareAddress);
			CommonUtility.AssertNotNull("shareAddress", shareAddress.PrimaryUri);
			this.ParseQueryAndVerify(shareAddress, credentials);
			this.Metadata = new Dictionary<string, string>();
			this.Properties = new FileShareProperties();
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00020788 File Offset: 0x0001E988
		internal CloudFileShare(string shareName, CloudFileClient serviceClient) : this(new FileShareProperties(), new Dictionary<string, string>(), shareName, serviceClient)
		{
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002079C File Offset: 0x0001E99C
		internal CloudFileShare(FileShareProperties properties, IDictionary<string, string> metadata, string shareName, CloudFileClient serviceClient)
		{
			this.StorageUri = NavigationHelper.AppendPathToUri(serviceClient.StorageUri, shareName);
			this.ServiceClient = serviceClient;
			this.Name = shareName;
			this.Metadata = metadata;
			this.Properties = properties;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x000207D4 File Offset: 0x0001E9D4
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x000207DC File Offset: 0x0001E9DC
		public CloudFileClient ServiceClient { get; private set; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000207E5 File Offset: 0x0001E9E5
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x000207F2 File Offset: 0x0001E9F2
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x000207FA File Offset: 0x0001E9FA
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00020803 File Offset: 0x0001EA03
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x0002080B File Offset: 0x0001EA0B
		public string Name { get; private set; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00020814 File Offset: 0x0001EA14
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x0002081C File Offset: 0x0001EA1C
		public IDictionary<string, string> Metadata { get; private set; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00020825 File Offset: 0x0001EA25
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0002082D File Offset: 0x0001EA2D
		public FileShareProperties Properties { get; private set; }

		// Token: 0x060008E7 RID: 2279 RVA: 0x00020838 File Offset: 0x0001EA38
		private string GetSharedAccessCanonicalName()
		{
			string accountName = this.ServiceClient.Credentials.AccountName;
			string name = this.Name;
			return string.Format(CultureInfo.InvariantCulture, "/{0}/{1}/{2}", new object[]
			{
				"file",
				accountName,
				name
			});
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00020884 File Offset: 0x0001EA84
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00020890 File Offset: 0x0001EA90
		public string GetSharedAccessSignature(SharedAccessFilePolicy policy, string groupPolicyIdentifier)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string sharedAccessCanonicalName = this.GetSharedAccessCanonicalName();
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, null, groupPolicyIdentifier, sharedAccessCanonicalName, "2015-02-21", key.KeyValue);
			string keyName = key.KeyName;
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, null, groupPolicyIdentifier, "s", hash, keyName, "2015-02-21");
			return signature.ToString();
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00020924 File Offset: 0x0001EB24
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
			this.Name = NavigationHelper.GetShareNameFromShareAddress(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x000209B0 File Offset: 0x0001EBB0
		public CloudFileDirectory GetRootDirectoryReference()
		{
			return new CloudFileDirectory(this.StorageUri, string.Empty, this);
		}
	}
}
