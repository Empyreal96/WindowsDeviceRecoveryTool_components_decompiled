using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x02000033 RID: 51
	public sealed class CloudQueue
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x0002296C File Offset: 0x00020B6C
		[DoesServiceRequest]
		public void Create(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.CreateQueueImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000229A6 File Offset: 0x00020BA6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(AsyncCallback callback, object state)
		{
			return this.BeginCreate(null, null, callback, state);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000229B4 File Offset: 0x00020BB4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreate(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.CreateQueueImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x000229F0 File Offset: 0x00020BF0
		public void EndCreate(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x000229F9 File Offset: 0x00020BF9
		[DoesServiceRequest]
		public Task CreateAsync()
		{
			return this.CreateAsync(CancellationToken.None);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00022A06 File Offset: 0x00020C06
		[DoesServiceRequest]
		public Task CreateAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), cancellationToken);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00022A26 File Offset: 0x00020C26
		[DoesServiceRequest]
		public Task CreateAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.CreateAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00022A35 File Offset: 0x00020C35
		[DoesServiceRequest]
		public Task CreateAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueueRequestOptions, OperationContext>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreate), new Action<IAsyncResult>(this.EndCreate), options, operationContext, cancellationToken);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00022A58 File Offset: 0x00020C58
		[DoesServiceRequest]
		public bool CreateIfNotExists(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions options2 = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			bool flag = this.Exists(true, options2, operationContext);
			if (flag)
			{
				return false;
			}
			bool result;
			try
			{
				this.Create(options2, operationContext);
				if (operationContext.LastResult.HttpStatusCode == 204)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 409)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == QueueErrorCodeStrings.QueueAlreadyExists))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00022B04 File Offset: 0x00020D04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(AsyncCallback callback, object state)
		{
			return this.BeginCreateIfNotExists(null, null, callback, state);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00022B10 File Offset: 0x00020D10
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginCreateIfNotExists(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = queueRequestOptions,
				OperationContext = operationContext
			};
			ICancellableAsyncResult @object = this.BeginExists(true, queueRequestOptions, operationContext, new AsyncCallback(this.CreateIfNotExistsHandler), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00022C74 File Offset: 0x00020E74
		private void CreateIfNotExistsHandler(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult.AsyncState as StorageAsyncResult<bool>;
			lock (storageAsyncResult.CancellationLockerObject)
			{
				storageAsyncResult.CancelDelegate = null;
				storageAsyncResult.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
				try
				{
					bool flag2 = this.EndExists(asyncResult);
					if (flag2)
					{
						storageAsyncResult.Result = false;
						storageAsyncResult.OnComplete();
					}
					else
					{
						ICancellableAsyncResult @object = this.BeginCreate((QueueRequestOptions)storageAsyncResult.RequestOptions, storageAsyncResult.OperationContext, delegate(IAsyncResult createRes)
						{
							storageAsyncResult.CancelDelegate = null;
							storageAsyncResult.UpdateCompletedSynchronously(createRes.CompletedSynchronously);
							try
							{
								this.EndCreate(createRes);
								storageAsyncResult.Result = true;
								storageAsyncResult.OnComplete();
							}
							catch (StorageException ex)
							{
								if (ex.RequestInformation.HttpStatusCode == 409)
								{
									if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == QueueErrorCodeStrings.QueueAlreadyExists)
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
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00022D94 File Offset: 0x00020F94
		public bool EndCreateIfNotExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00022DBF File Offset: 0x00020FBF
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync()
		{
			return this.CreateIfNotExistsAsync(CancellationToken.None);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00022DCC File Offset: 0x00020FCC
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), cancellationToken);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00022DEC File Offset: 0x00020FEC
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.CreateIfNotExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00022DFB File Offset: 0x00020FFB
		[DoesServiceRequest]
		public Task<bool> CreateIfNotExistsAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, bool>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginCreateIfNotExists), new Func<IAsyncResult, bool>(this.EndCreateIfNotExists), options, operationContext, cancellationToken);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00022E20 File Offset: 0x00021020
		[DoesServiceRequest]
		public bool DeleteIfExists(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions options2 = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			if (!this.Exists(true, options2, operationContext))
			{
				return false;
			}
			bool result;
			try
			{
				this.Delete(options2, operationContext);
				result = true;
			}
			catch (StorageException ex)
			{
				if (ex.RequestInformation.HttpStatusCode != 404)
				{
					throw;
				}
				if (ex.RequestInformation.ExtendedErrorInformation != null && !(ex.RequestInformation.ExtendedErrorInformation.ErrorCode == QueueErrorCodeStrings.QueueNotFound))
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00022EB8 File Offset: 0x000210B8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(AsyncCallback callback, object state)
		{
			return this.BeginDeleteIfExists(null, null, callback, state);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00022EC4 File Offset: 0x000210C4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteIfExists(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			StorageAsyncResult<bool> storageAsyncResult = new StorageAsyncResult<bool>(callback, state)
			{
				RequestOptions = queueRequestOptions,
				OperationContext = operationContext
			};
			ICancellableAsyncResult @object = this.BeginExists(true, queueRequestOptions, operationContext, new AsyncCallback(this.DeleteIfExistsHandler), storageAsyncResult);
			storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
			return storageAsyncResult;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00023028 File Offset: 0x00021228
		private void DeleteIfExistsHandler(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult.AsyncState as StorageAsyncResult<bool>;
			lock (storageAsyncResult.CancellationLockerObject)
			{
				storageAsyncResult.CancelDelegate = null;
				storageAsyncResult.UpdateCompletedSynchronously(asyncResult.CompletedSynchronously);
				try
				{
					if (!this.EndExists(asyncResult))
					{
						storageAsyncResult.Result = false;
						storageAsyncResult.OnComplete();
					}
					else
					{
						ICancellableAsyncResult @object = this.BeginDelete((QueueRequestOptions)storageAsyncResult.RequestOptions, storageAsyncResult.OperationContext, delegate(IAsyncResult deleteRes)
						{
							storageAsyncResult.CancelDelegate = null;
							storageAsyncResult.UpdateCompletedSynchronously(deleteRes.CompletedSynchronously);
							try
							{
								this.EndDelete(deleteRes);
								storageAsyncResult.Result = true;
								storageAsyncResult.OnComplete();
							}
							catch (StorageException ex)
							{
								if (ex.RequestInformation.HttpStatusCode == 404)
								{
									if (ex.RequestInformation.ExtendedErrorInformation == null || ex.RequestInformation.ExtendedErrorInformation.ErrorCode == QueueErrorCodeStrings.QueueNotFound)
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
						storageAsyncResult.CancelDelegate = new Action(@object.Cancel);
					}
				}
				catch (Exception exception)
				{
					storageAsyncResult.OnComplete(exception);
				}
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00023148 File Offset: 0x00021348
		public bool EndDeleteIfExists(IAsyncResult asyncResult)
		{
			StorageAsyncResult<bool> storageAsyncResult = asyncResult as StorageAsyncResult<bool>;
			CommonUtility.AssertNotNull("AsyncResult", storageAsyncResult);
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00023173 File Offset: 0x00021373
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync()
		{
			return this.DeleteIfExistsAsync(CancellationToken.None);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00023180 File Offset: 0x00021380
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), cancellationToken);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x000231A0 File Offset: 0x000213A0
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteIfExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x000231AF File Offset: 0x000213AF
		[DoesServiceRequest]
		public Task<bool> DeleteIfExistsAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, bool>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteIfExists), new Func<IAsyncResult, bool>(this.EndDeleteIfExists), options, operationContext, cancellationToken);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x000231D4 File Offset: 0x000213D4
		[DoesServiceRequest]
		public void Delete(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.DeleteQueueImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002320E File Offset: 0x0002140E
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(AsyncCallback callback, object state)
		{
			return this.BeginDelete(null, null, callback, state);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002321C File Offset: 0x0002141C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDelete(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.DeleteQueueImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x00023258 File Offset: 0x00021458
		public void EndDelete(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00023261 File Offset: 0x00021461
		[DoesServiceRequest]
		public Task DeleteAsync()
		{
			return this.DeleteAsync(CancellationToken.None);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002326E File Offset: 0x0002146E
		[DoesServiceRequest]
		public Task DeleteAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), cancellationToken);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002328E File Offset: 0x0002148E
		[DoesServiceRequest]
		public Task DeleteAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002329D File Offset: 0x0002149D
		[DoesServiceRequest]
		public Task DeleteAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueueRequestOptions, OperationContext>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDelete), new Action<IAsyncResult>(this.EndDelete), options, operationContext, cancellationToken);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000232C0 File Offset: 0x000214C0
		[DoesServiceRequest]
		public void SetPermissions(QueuePermissions permissions, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetPermissionsImpl(permissions, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000232FB File Offset: 0x000214FB
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(QueuePermissions permissions, AsyncCallback callback, object state)
		{
			return this.BeginSetPermissions(permissions, null, null, callback, state);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00023308 File Offset: 0x00021508
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetPermissions(QueuePermissions permissions, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetPermissionsImpl(permissions, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00023346 File Offset: 0x00021546
		public void EndSetPermissions(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002334F File Offset: 0x0002154F
		[DoesServiceRequest]
		public Task SetPermissionsAsync(QueuePermissions permissions)
		{
			return this.SetPermissionsAsync(permissions, CancellationToken.None);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002335D File Offset: 0x0002155D
		[DoesServiceRequest]
		public Task SetPermissionsAsync(QueuePermissions permissions, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueuePermissions>(new Func<QueuePermissions, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, cancellationToken);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002337E File Offset: 0x0002157E
		[DoesServiceRequest]
		public Task SetPermissionsAsync(QueuePermissions permissions, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.SetPermissionsAsync(permissions, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002338E File Offset: 0x0002158E
		[DoesServiceRequest]
		public Task SetPermissionsAsync(QueuePermissions permissions, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueuePermissions, QueueRequestOptions, OperationContext>(new Func<QueuePermissions, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetPermissions), new Action<IAsyncResult>(this.EndSetPermissions), permissions, options, operationContext, cancellationToken);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000233B4 File Offset: 0x000215B4
		[DoesServiceRequest]
		public QueuePermissions GetPermissions(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<QueuePermissions>(this.GetPermissionsImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000233ED File Offset: 0x000215ED
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(AsyncCallback callback, object state)
		{
			return this.BeginGetPermissions(null, null, callback, state);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000233FC File Offset: 0x000215FC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetPermissions(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<QueuePermissions>(this.GetPermissionsImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00023438 File Offset: 0x00021638
		public QueuePermissions EndGetPermissions(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<QueuePermissions>(asyncResult);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00023440 File Offset: 0x00021640
		[DoesServiceRequest]
		public Task<QueuePermissions> GetPermissionsAsync()
		{
			return this.GetPermissionsAsync(CancellationToken.None);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0002344D File Offset: 0x0002164D
		[DoesServiceRequest]
		public Task<QueuePermissions> GetPermissionsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueuePermissions>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, QueuePermissions>(this.EndGetPermissions), cancellationToken);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0002346D File Offset: 0x0002166D
		[DoesServiceRequest]
		public Task<QueuePermissions> GetPermissionsAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.GetPermissionsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0002347C File Offset: 0x0002167C
		[DoesServiceRequest]
		public Task<QueuePermissions> GetPermissionsAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, QueuePermissions>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetPermissions), new Func<IAsyncResult, QueuePermissions>(this.EndGetPermissions), options, operationContext, cancellationToken);
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0002349E File Offset: 0x0002169E
		[DoesServiceRequest]
		public bool Exists(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.Exists(false, options, operationContext);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000234AC File Offset: 0x000216AC
		private bool Exists(bool primaryOnly, QueueRequestOptions options, OperationContext operationContext)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<bool>(this.ExistsImpl(queueRequestOptions, primaryOnly), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000234E6 File Offset: 0x000216E6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(AsyncCallback callback, object state)
		{
			return this.BeginExists(null, null, callback, state);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x000234F2 File Offset: 0x000216F2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginExists(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginExists(false, options, operationContext, callback, state);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00023500 File Offset: 0x00021700
		private ICancellableAsyncResult BeginExists(bool primaryOnly, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<bool>(this.ExistsImpl(queueRequestOptions, primaryOnly), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0002353E File Offset: 0x0002173E
		public bool EndExists(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<bool>(asyncResult);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00023546 File Offset: 0x00021746
		[DoesServiceRequest]
		public Task<bool> ExistsAsync()
		{
			return this.ExistsAsync(CancellationToken.None);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00023553 File Offset: 0x00021753
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), cancellationToken);
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00023573 File Offset: 0x00021773
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.ExistsAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00023582 File Offset: 0x00021782
		[DoesServiceRequest]
		public Task<bool> ExistsAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, bool>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginExists), new Func<IAsyncResult, bool>(this.EndExists), options, operationContext, cancellationToken);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x000235A4 File Offset: 0x000217A4
		[DoesServiceRequest]
		public void SetMetadata(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetMetadataImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x000235DE File Offset: 0x000217DE
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(AsyncCallback callback, object state)
		{
			return this.BeginSetMetadata(null, null, callback, state);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000235EC File Offset: 0x000217EC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetMetadata(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetMetadataImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00023628 File Offset: 0x00021828
		public void EndSetMetadata(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00023631 File Offset: 0x00021831
		[DoesServiceRequest]
		public Task SetMetadataAsync()
		{
			return this.SetMetadataAsync(CancellationToken.None);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002363E File Offset: 0x0002183E
		[DoesServiceRequest]
		public Task SetMetadataAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), cancellationToken);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002365E File Offset: 0x0002185E
		[DoesServiceRequest]
		public Task SetMetadataAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.SetMetadataAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002366D File Offset: 0x0002186D
		[DoesServiceRequest]
		public Task SetMetadataAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueueRequestOptions, OperationContext>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetMetadata), new Action<IAsyncResult>(this.EndSetMetadata), options, operationContext, cancellationToken);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00023690 File Offset: 0x00021890
		[DoesServiceRequest]
		public void FetchAttributes(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.FetchAttributesImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000236CA File Offset: 0x000218CA
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(AsyncCallback callback, object state)
		{
			return this.BeginFetchAttributes(null, null, callback, state);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000236D8 File Offset: 0x000218D8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginFetchAttributes(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.FetchAttributesImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00023714 File Offset: 0x00021914
		public void EndFetchAttributes(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002371D File Offset: 0x0002191D
		[DoesServiceRequest]
		public Task FetchAttributesAsync()
		{
			return this.FetchAttributesAsync(CancellationToken.None);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002372A File Offset: 0x0002192A
		[DoesServiceRequest]
		public Task FetchAttributesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), cancellationToken);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002374A File Offset: 0x0002194A
		[DoesServiceRequest]
		public Task FetchAttributesAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.FetchAttributesAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00023759 File Offset: 0x00021959
		[DoesServiceRequest]
		public Task FetchAttributesAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueueRequestOptions, OperationContext>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginFetchAttributes), new Action<IAsyncResult>(this.EndFetchAttributes), options, operationContext, cancellationToken);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002377C File Offset: 0x0002197C
		[DoesServiceRequest]
		public void AddMessage(CloudQueueMessage message, TimeSpan? timeToLive = null, TimeSpan? initialVisibilityDelay = null, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("message", message);
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.AddMessageImpl(message, timeToLive, initialVisibilityDelay, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000237C8 File Offset: 0x000219C8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAddMessage(CloudQueueMessage message, AsyncCallback callback, object state)
		{
			return this.BeginAddMessage(message, null, null, null, null, callback, state);
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000237F4 File Offset: 0x000219F4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginAddMessage(CloudQueueMessage message, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("message", message);
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.AddMessageImpl(message, timeToLive, initialVisibilityDelay, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00023842 File Offset: 0x00021A42
		public void EndAddMessage(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0002384B File Offset: 0x00021A4B
		[DoesServiceRequest]
		public Task AddMessageAsync(CloudQueueMessage message)
		{
			return this.AddMessageAsync(message, CancellationToken.None);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00023859 File Offset: 0x00021A59
		[DoesServiceRequest]
		public Task AddMessageAsync(CloudQueueMessage message, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<CloudQueueMessage>(new Func<CloudQueueMessage, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAddMessage), new Action<IAsyncResult>(this.EndAddMessage), message, cancellationToken);
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002387A File Offset: 0x00021A7A
		[DoesServiceRequest]
		public Task AddMessageAsync(CloudQueueMessage message, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.AddMessageAsync(message, timeToLive, initialVisibilityDelay, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002388E File Offset: 0x00021A8E
		[DoesServiceRequest]
		public Task AddMessageAsync(CloudQueueMessage message, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<CloudQueueMessage, TimeSpan?, TimeSpan?, QueueRequestOptions, OperationContext>(new Func<CloudQueueMessage, TimeSpan?, TimeSpan?, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginAddMessage), new Action<IAsyncResult>(this.EndAddMessage), message, timeToLive, initialVisibilityDelay, options, operationContext, cancellationToken);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000238B8 File Offset: 0x00021AB8
		[DoesServiceRequest]
		public void UpdateMessage(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.UpdateMessageImpl(message, visibilityTimeout, updateFields, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000238F8 File Offset: 0x00021AF8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUpdateMessage(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, AsyncCallback callback, object state)
		{
			return this.BeginUpdateMessage(message, visibilityTimeout, updateFields, null, null, callback, state);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002390C File Offset: 0x00021B0C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginUpdateMessage(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("message", message);
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.UpdateMessageImpl(message, visibilityTimeout, updateFields, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002395A File Offset: 0x00021B5A
		public void EndUpdateMessage(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00023963 File Offset: 0x00021B63
		[DoesServiceRequest]
		public Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields)
		{
			return this.UpdateMessageAsync(message, visibilityTimeout, updateFields, CancellationToken.None);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00023973 File Offset: 0x00021B73
		[DoesServiceRequest]
		public Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<CloudQueueMessage, TimeSpan, MessageUpdateFields>(new Func<CloudQueueMessage, TimeSpan, MessageUpdateFields, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUpdateMessage), new Action<IAsyncResult>(this.EndUpdateMessage), message, visibilityTimeout, updateFields, cancellationToken);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00023997 File Offset: 0x00021B97
		[DoesServiceRequest]
		public Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.UpdateMessageAsync(message, visibilityTimeout, updateFields, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000239AB File Offset: 0x00021BAB
		[DoesServiceRequest]
		public Task UpdateMessageAsync(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<CloudQueueMessage, TimeSpan, MessageUpdateFields, QueueRequestOptions, OperationContext>(new Func<CloudQueueMessage, TimeSpan, MessageUpdateFields, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginUpdateMessage), new Action<IAsyncResult>(this.EndUpdateMessage), message, visibilityTimeout, updateFields, options, operationContext, cancellationToken);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000239D3 File Offset: 0x00021BD3
		[DoesServiceRequest]
		public void DeleteMessage(CloudQueueMessage message, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("message", message);
			this.DeleteMessage(message.Id, message.PopReceipt, options, operationContext);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000239F4 File Offset: 0x00021BF4
		[DoesServiceRequest]
		public void DeleteMessage(string messageId, string popReceipt, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("messageId", messageId);
			CommonUtility.AssertNotNull("popReceipt", popReceipt);
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.DeleteMessageImpl(messageId, popReceipt, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00023A48 File Offset: 0x00021C48
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteMessage(CloudQueueMessage message, AsyncCallback callback, object state)
		{
			return this.BeginDeleteMessage(message, null, null, callback, state);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00023A55 File Offset: 0x00021C55
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteMessage(CloudQueueMessage message, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("message", message);
			return this.BeginDeleteMessage(message.Id, message.PopReceipt, options, operationContext, callback, state);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00023A7A File Offset: 0x00021C7A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteMessage(string messageId, string popReceipt, AsyncCallback callback, object state)
		{
			return this.BeginDeleteMessage(messageId, popReceipt, null, null, callback, state);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00023A8C File Offset: 0x00021C8C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginDeleteMessage(string messageId, string popReceipt, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("messageId", messageId);
			CommonUtility.AssertNotNull("popReceipt", popReceipt);
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.DeleteMessageImpl(messageId, popReceipt, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00023AE3 File Offset: 0x00021CE3
		public void EndDeleteMessage(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00023AEC File Offset: 0x00021CEC
		[DoesServiceRequest]
		public Task DeleteMessageAsync(CloudQueueMessage message)
		{
			return this.DeleteMessageAsync(message, null, null, CancellationToken.None);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00023AFC File Offset: 0x00021CFC
		[DoesServiceRequest]
		public Task DeleteMessageAsync(CloudQueueMessage message, CancellationToken cancellationToken)
		{
			return this.DeleteMessageAsync(message, null, null, cancellationToken);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00023B08 File Offset: 0x00021D08
		[DoesServiceRequest]
		public Task DeleteMessageAsync(CloudQueueMessage message, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteMessageAsync(message, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00023B18 File Offset: 0x00021D18
		[DoesServiceRequest]
		public Task DeleteMessageAsync(CloudQueueMessage message, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<CloudQueueMessage, QueueRequestOptions, OperationContext>(new Func<CloudQueueMessage, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteMessage), new Action<IAsyncResult>(this.EndDeleteMessage), message, options, operationContext, cancellationToken);
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00023B3C File Offset: 0x00021D3C
		[DoesServiceRequest]
		public Task DeleteMessageAsync(string messageId, string popReceipt)
		{
			return this.DeleteMessageAsync(messageId, popReceipt, null, null, CancellationToken.None);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00023B4D File Offset: 0x00021D4D
		[DoesServiceRequest]
		public Task DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken)
		{
			return this.DeleteMessageAsync(messageId, popReceipt, null, null, cancellationToken);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00023B5A File Offset: 0x00021D5A
		[DoesServiceRequest]
		public Task DeleteMessageAsync(string messageId, string popReceipt, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.DeleteMessageAsync(messageId, popReceipt, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00023B6C File Offset: 0x00021D6C
		[DoesServiceRequest]
		public Task DeleteMessageAsync(string messageId, string popReceipt, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<string, string, QueueRequestOptions, OperationContext>(new Func<string, string, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginDeleteMessage), new Action<IAsyncResult>(this.EndDeleteMessage), messageId, popReceipt, options, operationContext, cancellationToken);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00023B94 File Offset: 0x00021D94
		[DoesServiceRequest]
		public IEnumerable<CloudQueueMessage> GetMessages(int messageCount, TimeSpan? visibilityTimeout = null, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<IEnumerable<CloudQueueMessage>>(this.GetMessagesImpl(messageCount, visibilityTimeout, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00023BD4 File Offset: 0x00021DD4
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetMessages(int messageCount, AsyncCallback callback, object state)
		{
			return this.BeginGetMessages(messageCount, null, null, null, callback, state);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00023BF8 File Offset: 0x00021DF8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetMessages(int messageCount, TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<IEnumerable<CloudQueueMessage>>(this.GetMessagesImpl(messageCount, visibilityTimeout, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00023C39 File Offset: 0x00021E39
		public IEnumerable<CloudQueueMessage> EndGetMessages(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IEnumerable<CloudQueueMessage>>(asyncResult);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00023C41 File Offset: 0x00021E41
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(int messageCount)
		{
			return this.GetMessagesAsync(messageCount, CancellationToken.None);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00023C4F File Offset: 0x00021E4F
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(int messageCount, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<int, IEnumerable<CloudQueueMessage>>(new Func<int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetMessages), new Func<IAsyncResult, IEnumerable<CloudQueueMessage>>(this.EndGetMessages), messageCount, cancellationToken);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00023C70 File Offset: 0x00021E70
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.GetMessagesAsync(messageCount, visibilityTimeout, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00023C82 File Offset: 0x00021E82
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> GetMessagesAsync(int messageCount, TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<int, TimeSpan?, QueueRequestOptions, OperationContext, IEnumerable<CloudQueueMessage>>(new Func<int, TimeSpan?, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetMessages), new Func<IAsyncResult, IEnumerable<CloudQueueMessage>>(this.EndGetMessages), messageCount, visibilityTimeout, options, operationContext, cancellationToken);
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00023CA8 File Offset: 0x00021EA8
		[DoesServiceRequest]
		public CloudQueueMessage GetMessage(TimeSpan? visibilityTimeout = null, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.GetMessages(1, visibilityTimeout, options, operationContext).FirstOrDefault<CloudQueueMessage>();
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00023CBC File Offset: 0x00021EBC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetMessage(AsyncCallback callback, object state)
		{
			return this.BeginGetMessage(null, null, null, callback, state);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00023CDC File Offset: 0x00021EDC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetMessage(TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginGetMessages(1, visibilityTimeout, options, operationContext, callback, state);
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00023CEC File Offset: 0x00021EEC
		public CloudQueueMessage EndGetMessage(IAsyncResult asyncResult)
		{
			IEnumerable<CloudQueueMessage> source = Executor.EndExecuteAsync<IEnumerable<CloudQueueMessage>>(asyncResult);
			return source.FirstOrDefault<CloudQueueMessage>();
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00023D06 File Offset: 0x00021F06
		[DoesServiceRequest]
		public Task<CloudQueueMessage> GetMessageAsync()
		{
			return this.GetMessageAsync(CancellationToken.None);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00023D13 File Offset: 0x00021F13
		[DoesServiceRequest]
		public Task<CloudQueueMessage> GetMessageAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudQueueMessage>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetMessage), new Func<IAsyncResult, CloudQueueMessage>(this.EndGetMessage), cancellationToken);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00023D33 File Offset: 0x00021F33
		[DoesServiceRequest]
		public Task<CloudQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.GetMessageAsync(visibilityTimeout, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00023D43 File Offset: 0x00021F43
		[DoesServiceRequest]
		public Task<CloudQueueMessage> GetMessageAsync(TimeSpan? visibilityTimeout, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TimeSpan?, QueueRequestOptions, OperationContext, CloudQueueMessage>(new Func<TimeSpan?, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetMessage), new Func<IAsyncResult, CloudQueueMessage>(this.EndGetMessage), visibilityTimeout, options, operationContext, cancellationToken);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00023D68 File Offset: 0x00021F68
		[DoesServiceRequest]
		public IEnumerable<CloudQueueMessage> PeekMessages(int messageCount, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<IEnumerable<CloudQueueMessage>>(this.PeekMessagesImpl(messageCount, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00023DA2 File Offset: 0x00021FA2
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPeekMessages(int messageCount, AsyncCallback callback, object state)
		{
			return this.BeginPeekMessages(messageCount, null, null, callback, state);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00023DB0 File Offset: 0x00021FB0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPeekMessages(int messageCount, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<IEnumerable<CloudQueueMessage>>(this.PeekMessagesImpl(messageCount, queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00023DEE File Offset: 0x00021FEE
		public IEnumerable<CloudQueueMessage> EndPeekMessages(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<IEnumerable<CloudQueueMessage>>(asyncResult);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00023DF6 File Offset: 0x00021FF6
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(int messageCount)
		{
			return this.PeekMessagesAsync(messageCount, CancellationToken.None);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00023E04 File Offset: 0x00022004
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(int messageCount, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<int, IEnumerable<CloudQueueMessage>>(new Func<int, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPeekMessages), new Func<IAsyncResult, IEnumerable<CloudQueueMessage>>(this.EndPeekMessages), messageCount, cancellationToken);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00023E25 File Offset: 0x00022025
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(int messageCount, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.PeekMessagesAsync(messageCount, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00023E35 File Offset: 0x00022035
		[DoesServiceRequest]
		public Task<IEnumerable<CloudQueueMessage>> PeekMessagesAsync(int messageCount, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<int, QueueRequestOptions, OperationContext, IEnumerable<CloudQueueMessage>>(new Func<int, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPeekMessages), new Func<IAsyncResult, IEnumerable<CloudQueueMessage>>(this.EndPeekMessages), messageCount, options, operationContext, cancellationToken);
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00023E59 File Offset: 0x00022059
		[DoesServiceRequest]
		public CloudQueueMessage PeekMessage(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.PeekMessages(1, options, operationContext).FirstOrDefault<CloudQueueMessage>();
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00023E69 File Offset: 0x00022069
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPeekMessage(AsyncCallback callback, object state)
		{
			return this.BeginPeekMessage(null, null, callback, state);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00023E75 File Offset: 0x00022075
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginPeekMessage(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.BeginPeekMessages(1, options, operationContext, callback, state);
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00023E84 File Offset: 0x00022084
		public CloudQueueMessage EndPeekMessage(IAsyncResult asyncResult)
		{
			IEnumerable<CloudQueueMessage> source = Executor.EndExecuteAsync<IEnumerable<CloudQueueMessage>>(asyncResult);
			return source.FirstOrDefault<CloudQueueMessage>();
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00023E9E File Offset: 0x0002209E
		[DoesServiceRequest]
		public Task<CloudQueueMessage> PeekMessageAsync()
		{
			return this.PeekMessageAsync(CancellationToken.None);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00023EAB File Offset: 0x000220AB
		[DoesServiceRequest]
		public Task<CloudQueueMessage> PeekMessageAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<CloudQueueMessage>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginPeekMessage), new Func<IAsyncResult, CloudQueueMessage>(this.EndPeekMessage), cancellationToken);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00023ECB File Offset: 0x000220CB
		[DoesServiceRequest]
		public Task<CloudQueueMessage> PeekMessageAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.PeekMessageAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00023EDA File Offset: 0x000220DA
		[DoesServiceRequest]
		public Task<CloudQueueMessage> PeekMessageAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, CloudQueueMessage>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginPeekMessage), new Func<IAsyncResult, CloudQueueMessage>(this.EndPeekMessage), options, operationContext, cancellationToken);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00023EFC File Offset: 0x000220FC
		[DoesServiceRequest]
		public void Clear(QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.ClearMessagesImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00023F36 File Offset: 0x00022136
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClear(AsyncCallback callback, object state)
		{
			return this.BeginClear(null, null, callback, state);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00023F44 File Offset: 0x00022144
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginClear(QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.ClearMessagesImpl(queueRequestOptions), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00023F80 File Offset: 0x00022180
		public void EndClear(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00023F89 File Offset: 0x00022189
		[DoesServiceRequest]
		public Task ClearAsync()
		{
			return this.ClearAsync(CancellationToken.None);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00023F96 File Offset: 0x00022196
		[DoesServiceRequest]
		public Task ClearAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginClear), new Action<IAsyncResult>(this.EndClear), cancellationToken);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00023FB6 File Offset: 0x000221B6
		[DoesServiceRequest]
		public Task ClearAsync(QueueRequestOptions options, OperationContext operationContext)
		{
			return this.ClearAsync(options, operationContext, CancellationToken.None);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00023FC5 File Offset: 0x000221C5
		[DoesServiceRequest]
		public Task ClearAsync(QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<QueueRequestOptions, OperationContext>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginClear), new Action<IAsyncResult>(this.EndClear), options, operationContext, cancellationToken);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00023FE7 File Offset: 0x000221E7
		[Obsolete("This class is improperly named; use class EndClear instead.")]
		public void EndBeginClear(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00024010 File Offset: 0x00022210
		private RESTCommand<NullType> ClearMessagesImpl(QueueRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.GetMessageRequestAddress());
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.ClearMessages(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x000240F4 File Offset: 0x000222F4
		private RESTCommand<NullType> CreateQueueImpl(QueueRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.Create(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				QueueHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(new HttpStatusCode[]
				{
					HttpStatusCode.Created,
					HttpStatusCode.NoContent
				}, resp, NullType.Value, cmd, ex);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000241B8 File Offset: 0x000223B8
		private RESTCommand<NullType> DeleteQueueImpl(QueueRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.Delete(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00024274 File Offset: 0x00022474
		private RESTCommand<NullType> FetchAttributesImpl(QueueRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.GetMetadata(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.OK, resp, NullType.Value, cmd, ex);
				this.GetMessageCountAndMetadataFromResponse(resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00024324 File Offset: 0x00022524
		private RESTCommand<bool> ExistsImpl(QueueRequestOptions options, bool primaryOnly)
		{
			RESTCommand<bool> restcommand = new RESTCommand<bool>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<bool>(restcommand);
			restcommand.CommandLocationMode = (primaryOnly ? CommandLocationMode.PrimaryOnly : CommandLocationMode.PrimaryOrSecondary);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.GetMetadata(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<bool> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => resp.StatusCode != HttpStatusCode.NotFound && HttpResponseParsers.ProcessExpectedStatusCodeNoException<bool>(HttpStatusCode.OK, resp, true, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x000243F4 File Offset: 0x000225F4
		private RESTCommand<NullType> SetMetadataImpl(QueueRequestOptions options)
		{
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.SetMetadata(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				QueueHttpWebRequestFactory.AddMetadata(r, this.Metadata);
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000244BC File Offset: 0x000226BC
		private RESTCommand<NullType> SetPermissionsImpl(QueuePermissions acl, QueueRequestOptions options)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			QueueRequest.WriteSharedAccessIdentifiers(acl.SharedAccessPolicies, multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.SetAcl(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x000245CC File Offset: 0x000227CC
		private RESTCommand<QueuePermissions> GetPermissionsImpl(QueueRequestOptions options)
		{
			RESTCommand<QueuePermissions> restcommand = new RESTCommand<QueuePermissions>(this.ServiceClient.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<QueuePermissions>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.GetAcl(uri, serverTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<QueuePermissions> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<QueuePermissions>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<QueuePermissions> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				QueuePermissions queuePermissions = new QueuePermissions();
				QueueHttpResponseParsers.ReadSharedAccessIdentifiers(cmd.ResponseStream, queuePermissions);
				return queuePermissions;
			};
			return restcommand;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00024754 File Offset: 0x00022954
		private RESTCommand<NullType> AddMessageImpl(CloudQueueMessage message, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay, QueueRequestOptions options)
		{
			int? timeToLiveInSeconds = null;
			int? initialVisibilityDelayInSeconds = null;
			if (timeToLive != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("timeToLive", timeToLive.Value, TimeSpan.Zero, CloudQueueMessage.MaxTimeToLive);
				timeToLiveInSeconds = new int?((int)timeToLive.Value.TotalSeconds);
			}
			if (initialVisibilityDelay != null)
			{
				CommonUtility.AssertInBounds<TimeSpan>("initialVisibilityDelay", initialVisibilityDelay.Value, TimeSpan.Zero, timeToLive ?? CloudQueueMessage.MaxTimeToLive);
				initialVisibilityDelayInSeconds = new int?((int)initialVisibilityDelay.Value.TotalSeconds);
			}
			CommonUtility.AssertNotNull("message", message);
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			QueueRequest.WriteMessageContent(message.GetMessageContentForTransfer(this.EncodeMessage, options), multiBufferMemoryStream);
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, this.GetMessageRequestAddress());
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.AddMessage(uri, serverTimeout, timeToLiveInSeconds, initialVisibilityDelayInSeconds, useVersionHeader, ctx));
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SetHeaders = delegate(HttpWebRequest r, OperationContext ctx)
			{
				if (timeToLive != null)
				{
					r.Headers.Set("messagettl", timeToLive.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
				}
				if (initialVisibilityDelay != null)
				{
					r.Headers.Set("visibilitytimeout", initialVisibilityDelay.Value.TotalSeconds.ToString(CultureInfo.InvariantCulture));
				}
			};
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Created, resp, NullType.Value, cmd, ex);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00024974 File Offset: 0x00022B74
		private RESTCommand<NullType> UpdateMessageImpl(CloudQueueMessage message, TimeSpan visibilityTimeout, MessageUpdateFields updateFields, QueueRequestOptions options)
		{
			CommonUtility.AssertNotNull("message", message);
			CommonUtility.AssertNotNullOrEmpty("messageId", message.Id);
			CommonUtility.AssertNotNullOrEmpty("popReceipt", message.PopReceipt);
			CommonUtility.AssertInBounds<TimeSpan>("visibilityTimeout", visibilityTimeout, TimeSpan.Zero, CloudQueueMessage.MaxTimeToLive);
			if ((updateFields & MessageUpdateFields.Visibility) == (MessageUpdateFields)0)
			{
				throw new ArgumentException("Calls to UpdateMessage must include the Visibility flag.", "updateFields");
			}
			StorageUri individualMessageAddress = this.GetIndividualMessageAddress(message.Id);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, individualMessageAddress);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.UpdateMessage(uri, serverTimeout, message.PopReceipt, visibilityTimeout.RoundUpToSeconds(), useVersionHeader, ctx));
			if ((updateFields & MessageUpdateFields.Content) != (MessageUpdateFields)0)
			{
				MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(this.ServiceClient.BufferManager, 1024);
				QueueRequest.WriteMessageContent(message.GetMessageContentForTransfer(this.EncodeMessage, options), multiBufferMemoryStream);
				multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
				restcommand.SendStream = multiBufferMemoryStream;
				restcommand.StreamToDispose = multiBufferMemoryStream;
				restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			}
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex);
				CloudQueue.GetPopReceiptAndNextVisibleTimeFromResponse(message, resp);
				return NullType.Value;
			};
			return restcommand;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00024AFC File Offset: 0x00022CFC
		private RESTCommand<NullType> DeleteMessageImpl(string messageId, string popReceipt, QueueRequestOptions options)
		{
			StorageUri individualMessageAddress = this.GetIndividualMessageAddress(messageId);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.ServiceClient.Credentials, individualMessageAddress);
			options.ApplyToStorageCommand<NullType>(restcommand);
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.DeleteMessage(uri, serverTimeout, popReceipt, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.NoContent, resp, NullType.Value, cmd, ex));
			return restcommand;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00024C08 File Offset: 0x00022E08
		private RESTCommand<IEnumerable<CloudQueueMessage>> GetMessagesImpl(int messageCount, TimeSpan? visibilityTimeout, QueueRequestOptions options)
		{
			options.AssertPolicyIfRequired();
			RESTCommand<IEnumerable<CloudQueueMessage>> restcommand = new RESTCommand<IEnumerable<CloudQueueMessage>>(this.ServiceClient.Credentials, this.GetMessageRequestAddress());
			options.ApplyToStorageCommand<IEnumerable<CloudQueueMessage>>(restcommand);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.GetMessages(uri, serverTimeout, messageCount, visibilityTimeout, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<IEnumerable<CloudQueueMessage>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IEnumerable<CloudQueueMessage>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<IEnumerable<CloudQueueMessage>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				GetMessagesResponse getMessagesResponse = new GetMessagesResponse(cmd.ResponseStream);
				return (from item in getMessagesResponse.Messages
				select this.SelectGetMessageResponse(item, options)).ToList<CloudQueueMessage>();
			};
			return restcommand;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00024D48 File Offset: 0x00022F48
		private RESTCommand<IEnumerable<CloudQueueMessage>> PeekMessagesImpl(int messageCount, QueueRequestOptions options)
		{
			options.AssertPolicyIfRequired();
			RESTCommand<IEnumerable<CloudQueueMessage>> restcommand = new RESTCommand<IEnumerable<CloudQueueMessage>>(this.ServiceClient.Credentials, this.GetMessageRequestAddress());
			options.ApplyToStorageCommand<IEnumerable<CloudQueueMessage>>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.PeekMessages(uri, serverTimeout, messageCount, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.ServiceClient.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<IEnumerable<CloudQueueMessage>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<IEnumerable<CloudQueueMessage>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<IEnumerable<CloudQueueMessage>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				GetMessagesResponse getMessagesResponse = new GetMessagesResponse(cmd.ResponseStream);
				return (from item in getMessagesResponse.Messages
				select this.SelectPeekMessageResponse(item, options)).ToList<CloudQueueMessage>();
			};
			return restcommand;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00024E14 File Offset: 0x00023014
		private void GetMessageCountAndMetadataFromResponse(HttpWebResponse webResponse)
		{
			this.Metadata = QueueHttpResponseParsers.GetMetadata(webResponse);
			string approximateMessageCount = QueueHttpResponseParsers.GetApproximateMessageCount(webResponse);
			this.ApproximateMessageCount = (string.IsNullOrEmpty(approximateMessageCount) ? null : new int?(int.Parse(approximateMessageCount, CultureInfo.InvariantCulture)));
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00024E60 File Offset: 0x00023060
		private static void GetPopReceiptAndNextVisibleTimeFromResponse(CloudQueueMessage message, HttpWebResponse webResponse)
		{
			message.PopReceipt = webResponse.Headers["x-ms-popreceipt"];
			message.NextVisibleTime = new DateTimeOffset?(DateTime.Parse(webResponse.Headers["x-ms-time-next-visible"], DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal));
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00024EAF File Offset: 0x000230AF
		public CloudQueue(Uri queueAddress) : this(queueAddress, null)
		{
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00024EB9 File Offset: 0x000230B9
		public CloudQueue(Uri queueAddress, StorageCredentials credentials) : this(new StorageUri(queueAddress), credentials)
		{
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00024EC8 File Offset: 0x000230C8
		public CloudQueue(StorageUri queueAddress, StorageCredentials credentials)
		{
			this.ParseQueryAndVerify(queueAddress, credentials);
			this.Metadata = new Dictionary<string, string>();
			this.EncodeMessage = true;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00024EEA File Offset: 0x000230EA
		internal CloudQueue(string queueName, CloudQueueClient serviceClient) : this(new Dictionary<string, string>(), queueName, serviceClient)
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00024EF9 File Offset: 0x000230F9
		internal CloudQueue(IDictionary<string, string> metadata, string queueName, CloudQueueClient serviceClient)
		{
			this.StorageUri = NavigationHelper.AppendPathToUri(serviceClient.StorageUri, queueName);
			this.ServiceClient = serviceClient;
			this.Name = queueName;
			this.Metadata = metadata;
			this.EncodeMessage = true;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00024F2F File Offset: 0x0002312F
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x00024F37 File Offset: 0x00023137
		public CloudQueueClient ServiceClient { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00024F40 File Offset: 0x00023140
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00024F4D File Offset: 0x0002314D
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x00024F55 File Offset: 0x00023155
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00024F5E File Offset: 0x0002315E
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00024F66 File Offset: 0x00023166
		public string Name { get; private set; }

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00024F6F File Offset: 0x0002316F
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x00024F77 File Offset: 0x00023177
		public int? ApproximateMessageCount { get; private set; }

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x00024F80 File Offset: 0x00023180
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x00024F88 File Offset: 0x00023188
		public bool EncodeMessage { get; set; }

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00024F91 File Offset: 0x00023191
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x00024F99 File Offset: 0x00023199
		public IDictionary<string, string> Metadata { get; private set; }

		// Token: 0x06000A21 RID: 2593 RVA: 0x00024FA2 File Offset: 0x000231A2
		internal StorageUri GetMessageRequestAddress()
		{
			if (this.messageRequestAddress == null)
			{
				this.messageRequestAddress = NavigationHelper.AppendPathToUri(this.StorageUri, "messages");
			}
			return this.messageRequestAddress;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00024FCE File Offset: 0x000231CE
		internal StorageUri GetIndividualMessageAddress(string messageId)
		{
			return NavigationHelper.AppendPathToUri(this.GetMessageRequestAddress(), messageId);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00024FDC File Offset: 0x000231DC
		private void ParseQueryAndVerify(StorageUri address, StorageCredentials credentials)
		{
			StorageCredentials storageCredentials;
			this.StorageUri = NavigationHelper.ParseQueueTableQueryAndVerify(address, out storageCredentials);
			if (storageCredentials != null && credentials != null)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot provide credentials as part of the address and as constructor parameter. Either pass in the address or use a different constructor.", new object[0]);
				throw new ArgumentException(message);
			}
			this.ServiceClient = new CloudQueueClient(NavigationHelper.GetServiceClientBaseAddress(this.StorageUri, null), credentials ?? storageCredentials);
			this.Name = NavigationHelper.GetQueueNameFromUri(this.Uri, new bool?(this.ServiceClient.UsePathStyleUris));
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00025060 File Offset: 0x00023260
		private string GetCanonicalName(string sasVersion)
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
				"queue",
				accountName,
				name
			});
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x000250D0 File Offset: 0x000232D0
		private CloudQueueMessage SelectGetMessageResponse(QueueMessage protocolMessage, QueueRequestOptions options = null)
		{
			CloudQueueMessage cloudQueueMessage = this.SelectPeekMessageResponse(protocolMessage, options);
			cloudQueueMessage.PopReceipt = protocolMessage.PopReceipt;
			if (protocolMessage.NextVisibleTime != null)
			{
				cloudQueueMessage.NextVisibleTime = new DateTimeOffset?(protocolMessage.NextVisibleTime.Value);
			}
			return cloudQueueMessage;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0002511C File Offset: 0x0002331C
		private CloudQueueMessage SelectPeekMessageResponse(QueueMessage protocolMessage, QueueRequestOptions options = null)
		{
			byte[] array = null;
			if (options != null && options.EncryptionPolicy != null)
			{
				array = options.EncryptionPolicy.DecryptMessage(protocolMessage.Text, options.RequireEncryption);
			}
			CloudQueueMessage cloudQueueMessage;
			if (this.EncodeMessage)
			{
				if (array != null)
				{
					protocolMessage.Text = Convert.ToBase64String(array, 0, array.Length);
				}
				cloudQueueMessage = new CloudQueueMessage(protocolMessage.Text, true);
			}
			else if (array != null)
			{
				cloudQueueMessage = new CloudQueueMessage(array);
			}
			else
			{
				cloudQueueMessage = new CloudQueueMessage(protocolMessage.Text);
			}
			cloudQueueMessage.Id = protocolMessage.Id;
			cloudQueueMessage.InsertionTime = protocolMessage.InsertionTime;
			cloudQueueMessage.ExpirationTime = protocolMessage.ExpirationTime;
			cloudQueueMessage.DequeueCount = protocolMessage.DequeueCount;
			return cloudQueueMessage;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x000251C3 File Offset: 0x000233C3
		public string GetSharedAccessSignature(SharedAccessQueuePolicy policy)
		{
			return this.GetSharedAccessSignature(policy, null);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x000251D0 File Offset: 0x000233D0
		public string GetSharedAccessSignature(SharedAccessQueuePolicy policy, string accessPolicyIdentifier)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string canonicalName = this.GetCanonicalName("2015-02-21");
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, accessPolicyIdentifier, canonicalName, "2015-02-21", key.KeyValue);
			string keyName = key.KeyName;
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, accessPolicyIdentifier, hash, keyName, "2015-02-21");
			return signature.ToString();
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00025260 File Offset: 0x00023460
		[Obsolete("This overload has been deprecated because the SAS tokens generated using the current version work fine with old libraries. Please use the other overloads.")]
		public string GetSharedAccessSignature(SharedAccessQueuePolicy policy, string accessPolicyIdentifier, string sasVersion)
		{
			if (!this.ServiceClient.Credentials.IsSharedKey)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Cannot create Shared Access Signature unless Account Key credentials are used.", new object[0]);
				throw new InvalidOperationException(message);
			}
			string sasVersion2 = SharedAccessSignatureHelper.ValidateSASVersionString(sasVersion);
			string canonicalName = this.GetCanonicalName(sasVersion2);
			StorageAccountKey key = this.ServiceClient.Credentials.Key;
			string hash = SharedAccessSignatureHelper.GetHash(policy, accessPolicyIdentifier, canonicalName, sasVersion2, key.KeyValue);
			string keyName = key.KeyName;
			UriQueryBuilder signature = SharedAccessSignatureHelper.GetSignature(policy, accessPolicyIdentifier, hash, keyName, sasVersion2);
			return signature.ToString();
		}

		// Token: 0x04000114 RID: 276
		private StorageUri messageRequestAddress;
	}
}
