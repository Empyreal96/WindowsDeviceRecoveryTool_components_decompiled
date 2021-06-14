using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth.Protocol;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.DataServices
{
	// Token: 0x02000045 RID: 69
	[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
	public class TableServiceContext : DataServiceContext, IDisposable
	{
		// Token: 0x06000C6D RID: 3181 RVA: 0x0002C934 File Offset: 0x0002AB34
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableServiceContext(CloudTableClient client) : base(client.BaseUri, DataServiceProtocolVersion.V3)
		{
			CommonUtility.AssertNotNull("client", client);
			if (client.BaseUri == null)
			{
				throw new ArgumentNullException("client");
			}
			if (!client.BaseUri.IsAbsoluteUri)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Address '{0}' is a relative address. Only absolute addresses are permitted.", new object[]
				{
					client.BaseUri.ToString()
				});
				throw new ArgumentException(message, "client");
			}
			base.SendingRequest += this.TableServiceContext_SendingRequest;
			base.IgnoreMissingProperties = true;
			base.MergeOption = MergeOption.PreserveChanges;
			this.ServiceClient = client;
			if (this.payloadFormat == TablePayloadFormat.Json)
			{
				base.Format.UseJson(new TableStorageModel(client.AccountName));
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002CA18 File Offset: 0x0002AC18
		internal void InternalCancel()
		{
			lock (this.cancellationLock)
			{
				this.cancellationRequested = true;
				if (this.currentRequest != null)
				{
					this.currentRequest.Abort();
				}
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0002CA6C File Offset: 0x0002AC6C
		internal void ResetCancellation()
		{
			lock (this.cancellationLock)
			{
				this.cancellationRequested = false;
				this.currentRequest = null;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0002CAB4 File Offset: 0x0002ACB4
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x0002CABC File Offset: 0x0002ACBC
		internal Action<HttpWebRequest> SendingSignedRequestAction
		{
			get
			{
				return this.sendingSignedRequestAction;
			}
			set
			{
				this.sendingSignedRequestAction = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x0002CAC5 File Offset: 0x0002ACC5
		internal Semaphore ContextSemaphore
		{
			get
			{
				return this.contextSemaphore;
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		private void TableServiceContext_SendingRequest(object sender, SendingRequestEventArgs e)
		{
			HttpWebRequest httpWebRequest = e.Request as HttpWebRequest;
			int num = httpWebRequest.RequestUri.Query.LastIndexOf("&timeout=", StringComparison.Ordinal);
			if (num > 0)
			{
				num += 9;
				int num2 = httpWebRequest.RequestUri.Query.IndexOf('&', num);
				string s = (num2 > 0) ? httpWebRequest.RequestUri.Query.Substring(num, num2 - num) : httpWebRequest.RequestUri.Query.Substring(num);
				int num3 = -1;
				if (int.TryParse(s, out num3) && num3 > 0)
				{
					httpWebRequest.Timeout = num3 * 1000;
				}
			}
			if (this.ServiceClient.Credentials.IsSharedKey)
			{
				this.AuthenticationHandler.SignRequest(httpWebRequest, null);
			}
			else if (this.ServiceClient.Credentials.IsSAS)
			{
				Uri requestUri = this.ServiceClient.Credentials.TransformUri(httpWebRequest.RequestUri);
				HttpWebRequest httpWebRequest2 = WebRequest.Create(requestUri) as HttpWebRequest;
				TableUtilities.CopyRequestData(httpWebRequest2, httpWebRequest);
				e.Request = httpWebRequest2;
				httpWebRequest = httpWebRequest2;
			}
			lock (this.cancellationLock)
			{
				if (this.cancellationRequested)
				{
					throw new OperationCanceledException("Operation was canceled by user.");
				}
				this.currentRequest = httpWebRequest;
			}
			if (!this.ServiceClient.Credentials.IsSAS)
			{
				httpWebRequest.Headers.Add("x-ms-version", "2015-02-21");
			}
			CommonUtility.ApplyRequestOptimizations(httpWebRequest, -1L);
			if (this.sendingSignedRequestAction != null)
			{
				this.sendingSignedRequestAction(httpWebRequest);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0002CC64 File Offset: 0x0002AE64
		// (set) Token: 0x06000C75 RID: 3189 RVA: 0x0002CC6C File Offset: 0x0002AE6C
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public CloudTableClient ServiceClient { get; private set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0002CC78 File Offset: 0x0002AE78
		private IAuthenticationHandler AuthenticationHandler
		{
			get
			{
				if (this.authenticationHandler == null)
				{
					if (this.ServiceClient.Credentials.IsSharedKey)
					{
						this.authenticationHandler = new SharedKeyAuthenticationHandler(SharedKeyLiteTableCanonicalizer.Instance, this.ServiceClient.Credentials, this.ServiceClient.Credentials.AccountName);
					}
					else
					{
						this.authenticationHandler = new NoOpAuthenticationHandler();
					}
				}
				return this.authenticationHandler;
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0002CCDD File Offset: 0x0002AEDD
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public DataServiceResponse SaveChangesWithRetries()
		{
			return this.SaveChangesWithRetries(base.SaveChangesDefaultOptions, null, null);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002CCF0 File Offset: 0x0002AEF0
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public DataServiceResponse SaveChangesWithRetries(SaveChangesOptions options, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableCommand<DataServiceResponse, DataServiceResponse> cmd = this.GenerateSaveChangesCommand(options, requestOptions);
			return TableExecutor.ExecuteSync<DataServiceResponse, DataServiceResponse>(cmd, requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0002CD2D File Offset: 0x0002AF2D
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public ICancellableAsyncResult BeginSaveChangesWithRetries(AsyncCallback callback, object state)
		{
			return this.BeginSaveChangesWithRetries(base.SaveChangesDefaultOptions, callback, state);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002CD3D File Offset: 0x0002AF3D
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public ICancellableAsyncResult BeginSaveChangesWithRetries(SaveChangesOptions options, AsyncCallback callback, object state)
		{
			return this.BeginSaveChangesWithRetries(options, null, null, callback, state);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0002CD4C File Offset: 0x0002AF4C
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public ICancellableAsyncResult BeginSaveChangesWithRetries(SaveChangesOptions options, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this.ServiceClient);
			operationContext = (operationContext ?? new OperationContext());
			TableCommand<DataServiceResponse, DataServiceResponse> cmd = this.GenerateSaveChangesCommand(options, requestOptions);
			return TableExecutor.BeginExecuteAsync<DataServiceResponse, DataServiceResponse>(cmd, requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0002CD8D File Offset: 0x0002AF8D
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public DataServiceResponse EndSaveChangesWithRetries(IAsyncResult asyncResult)
		{
			return TableExecutor.EndExecuteAsync<DataServiceResponse, DataServiceResponse>(asyncResult);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0002CD95 File Offset: 0x0002AF95
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync()
		{
			return this.SaveChangesWithRetriesAsync(CancellationToken.None);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0002CDA2 File Offset: 0x0002AFA2
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<DataServiceResponse>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginSaveChangesWithRetries), new Func<IAsyncResult, DataServiceResponse>(this.EndSaveChangesWithRetries), cancellationToken);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0002CDC2 File Offset: 0x0002AFC2
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		[DoesServiceRequest]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync(SaveChangesOptions options)
		{
			return this.SaveChangesWithRetriesAsync(options, CancellationToken.None);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002CDD0 File Offset: 0x0002AFD0
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync(SaveChangesOptions options, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<SaveChangesOptions, DataServiceResponse>(new Func<SaveChangesOptions, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSaveChangesWithRetries), new Func<IAsyncResult, DataServiceResponse>(this.EndSaveChangesWithRetries), options, cancellationToken);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002CDF1 File Offset: 0x0002AFF1
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync(SaveChangesOptions options, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.SaveChangesWithRetriesAsync(options, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002CE01 File Offset: 0x0002B001
		[DoesServiceRequest]
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public Task<DataServiceResponse> SaveChangesWithRetriesAsync(SaveChangesOptions options, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<SaveChangesOptions, TableRequestOptions, OperationContext, DataServiceResponse>(new Func<SaveChangesOptions, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSaveChangesWithRetries), new Func<IAsyncResult, DataServiceResponse>(this.EndSaveChangesWithRetries), options, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002CE58 File Offset: 0x0002B058
		internal TableCommand<DataServiceResponse, DataServiceResponse> GenerateSaveChangesCommand(SaveChangesOptions options, TableRequestOptions requestOptions)
		{
			TableCommand<DataServiceResponse, DataServiceResponse> tableCommand = new TableCommand<DataServiceResponse, DataServiceResponse>();
			if (requestOptions.ServerTimeout != null)
			{
				base.Timeout = (int)requestOptions.ServerTimeout.Value.TotalSeconds;
			}
			tableCommand.ExecuteFunc = (() => this.SaveChanges(options));
			tableCommand.Begin = ((AsyncCallback callback, object state) => this.BeginSaveChanges(options, callback, state));
			tableCommand.End = new Func<IAsyncResult, DataServiceResponse>(base.EndSaveChanges);
			tableCommand.ParseResponse = new Func<DataServiceResponse, RequestResult, TableCommand<DataServiceResponse, DataServiceResponse>, DataServiceResponse>(this.ParseDataServiceResponse);
			tableCommand.ParseDataServiceError = new Func<Stream, IDictionary<string, string>, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadDataServiceResponseFromStream);
			tableCommand.Context = this;
			requestOptions.ApplyToStorageCommand<DataServiceResponse, DataServiceResponse>(tableCommand);
			return tableCommand;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0002CF16 File Offset: 0x0002B116
		private DataServiceResponse ParseDataServiceResponse(DataServiceResponse resp, RequestResult reqResult, TableCommand<DataServiceResponse, DataServiceResponse> cmd)
		{
			if (reqResult.Exception != null)
			{
				throw reqResult.Exception;
			}
			return resp;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0002CF28 File Offset: 0x0002B128
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0002CF37 File Offset: 0x0002B137
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.contextSemaphore != null)
			{
				this.contextSemaphore.Dispose();
				this.contextSemaphore = null;
			}
		}

		// Token: 0x0400017E RID: 382
		private IAuthenticationHandler authenticationHandler;

		// Token: 0x0400017F RID: 383
		private TablePayloadFormat payloadFormat = TablePayloadFormat.Json;

		// Token: 0x04000180 RID: 384
		private object cancellationLock = new object();

		// Token: 0x04000181 RID: 385
		private bool cancellationRequested;

		// Token: 0x04000182 RID: 386
		private HttpWebRequest currentRequest;

		// Token: 0x04000183 RID: 387
		private Action<HttpWebRequest> sendingSignedRequestAction;

		// Token: 0x04000184 RID: 388
		private Semaphore contextSemaphore = new Semaphore(1, 1);
	}
}
