using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Auth.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200003B RID: 59
	public sealed class CloudTableClient
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x000282EF File Offset: 0x000264EF
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x000282F7 File Offset: 0x000264F7
		public AuthenticationScheme AuthenticationScheme
		{
			get
			{
				return this.authenticationScheme;
			}
			set
			{
				if (value != this.authenticationScheme)
				{
					this.authenticationScheme = value;
					this.authenticationHandler = null;
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00028310 File Offset: 0x00026510
		internal IAuthenticationHandler AuthenticationHandler
		{
			get
			{
				IAuthenticationHandler authenticationHandler = this.authenticationHandler;
				if (authenticationHandler == null)
				{
					if (this.Credentials.IsSharedKey)
					{
						authenticationHandler = new SharedKeyAuthenticationHandler(this.GetCanonicalizer(), this.Credentials, this.Credentials.AccountName);
					}
					else
					{
						authenticationHandler = new NoOpAuthenticationHandler();
					}
					this.authenticationHandler = authenticationHandler;
				}
				return authenticationHandler;
			}
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002837C File Offset: 0x0002657C
		[DoesServiceRequest]
		public IEnumerable<CloudTable> ListTables(string prefix = null, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			CloudTable tableReference = this.GetTableReference("Tables");
			return from tbl in CloudTableClient.GenerateListTablesQuery(prefix, null).ExecuteInternal(this, tableReference, requestOptions, operationContext)
			select new CloudTable(tbl["TableName"].StringValue, this);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x000283D3 File Offset: 0x000265D3
		[DoesServiceRequest]
		public TableResultSegment ListTablesSegmented(TableContinuationToken currentToken)
		{
			return this.ListTablesSegmented(null, currentToken);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x000283E0 File Offset: 0x000265E0
		[DoesServiceRequest]
		public TableResultSegment ListTablesSegmented(string prefix, TableContinuationToken currentToken)
		{
			return this.ListTablesSegmented(prefix, null, currentToken, null, null);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00028420 File Offset: 0x00026620
		[DoesServiceRequest]
		public TableResultSegment ListTablesSegmented(string prefix, int? maxResults, TableContinuationToken currentToken, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			CloudTable tableReference = this.GetTableReference("Tables");
			TableQuerySegment<DynamicTableEntity> tableQuerySegment = CloudTableClient.GenerateListTablesQuery(prefix, maxResults).ExecuteQuerySegmentedInternal(currentToken, this, tableReference, requestOptions, operationContext);
			List<CloudTable> result = (from tbl in tableQuerySegment.Results
			select new CloudTable(tbl.Properties["TableName"].StringValue, this)).ToList<CloudTable>();
			return new TableResultSegment(result)
			{
				ContinuationToken = tableQuerySegment.ContinuationToken
			};
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002849A File Offset: 0x0002669A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListTablesSegmented(TableContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListTablesSegmented(null, currentToken, callback, state);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x000284A8 File Offset: 0x000266A8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListTablesSegmented(string prefix, TableContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListTablesSegmented(prefix, null, currentToken, null, null, callback, state);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x000284CC File Offset: 0x000266CC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListTablesSegmented(string prefix, int? maxResults, TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			CloudTable tableReference = this.GetTableReference("Tables");
			return CloudTableClient.GenerateListTablesQuery(prefix, maxResults).BeginExecuteQuerySegmentedInternal(currentToken, this, tableReference, requestOptions, operationContext, callback, state);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00028530 File Offset: 0x00026730
		public TableResultSegment EndListTablesSegmented(IAsyncResult asyncResult)
		{
			TableQuerySegment<DynamicTableEntity> tableQuerySegment = Executor.EndExecuteAsync<TableQuerySegment<DynamicTableEntity>>(asyncResult);
			List<CloudTable> result = (from tbl in tableQuerySegment.Results
			select new CloudTable(tbl.Properties["TableName"].StringValue, this)).ToList<CloudTable>();
			return new TableResultSegment(result)
			{
				ContinuationToken = tableQuerySegment.ContinuationToken
			};
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00028577 File Offset: 0x00026777
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(TableContinuationToken currentToken)
		{
			return this.ListTablesSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x00028585 File Offset: 0x00026785
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(TableContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableContinuationToken, TableResultSegment>(new Func<TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListTablesSegmented), new Func<IAsyncResult, TableResultSegment>(this.EndListTablesSegmented), currentToken, cancellationToken);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000285A6 File Offset: 0x000267A6
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(string prefix, TableContinuationToken currentToken)
		{
			return this.ListTablesSegmentedAsync(prefix, currentToken, CancellationToken.None);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x000285B5 File Offset: 0x000267B5
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(string prefix, TableContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, TableContinuationToken, TableResultSegment>(new Func<string, TableContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListTablesSegmented), new Func<IAsyncResult, TableResultSegment>(this.EndListTablesSegmented), prefix, currentToken, cancellationToken);
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000285D7 File Offset: 0x000267D7
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(string prefix, int? maxResults, TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.ListTablesSegmentedAsync(prefix, maxResults, currentToken, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x000285EB File Offset: 0x000267EB
		[DoesServiceRequest]
		public Task<TableResultSegment> ListTablesSegmentedAsync(string prefix, int? maxResults, TableContinuationToken currentToken, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, int?, TableContinuationToken, TableRequestOptions, OperationContext, TableResultSegment>(new Func<string, int?, TableContinuationToken, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListTablesSegmented), new Func<IAsyncResult, TableResultSegment>(this.EndListTablesSegmented), prefix, maxResults, currentToken, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00028614 File Offset: 0x00026814
		private static TableQuery<DynamicTableEntity> GenerateListTablesQuery(string prefix, int? maxResults)
		{
			TableQuery<DynamicTableEntity> tableQuery = new TableQuery<DynamicTableEntity>();
			if (!string.IsNullOrEmpty(prefix))
			{
				string givenValue = prefix + '{';
				tableQuery = tableQuery.Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("TableName", "ge", prefix), "and", TableQuery.GenerateFilterCondition("TableName", "lt", givenValue)));
			}
			if (maxResults != null)
			{
				tableQuery = tableQuery.Take(new int?(maxResults.Value));
			}
			return tableQuery;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002868B File Offset: 0x0002688B
		[DoesServiceRequest]
		public ServiceProperties GetServiceProperties(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x000286B5 File Offset: 0x000268B5
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceProperties(null, null, callback, state);
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x000286C1 File Offset: 0x000268C1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x000286EE File Offset: 0x000268EE
		public ServiceProperties EndGetServiceProperties(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceProperties>(asyncResult);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x000286F6 File Offset: 0x000268F6
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync()
		{
			return this.GetServicePropertiesAsync(CancellationToken.None);
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00028703 File Offset: 0x00026903
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceProperties>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), cancellationToken);
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00028723 File Offset: 0x00026923
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServicePropertiesAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00028732 File Offset: 0x00026932
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, ServiceProperties>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00028754 File Offset: 0x00026954
		[DoesServiceRequest]
		public void SetServiceProperties(ServiceProperties properties, TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00028780 File Offset: 0x00026980
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, AsyncCallback callback, object state)
		{
			return this.BeginSetServiceProperties(properties, null, null, callback, state);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002878D File Offset: 0x0002698D
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x000287BC File Offset: 0x000269BC
		public void EndSetServiceProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x000287C5 File Offset: 0x000269C5
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties)
		{
			return this.SetServicePropertiesAsync(properties, CancellationToken.None);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000287D3 File Offset: 0x000269D3
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties>(new Func<ServiceProperties, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, cancellationToken);
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x000287F4 File Offset: 0x000269F4
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.SetServicePropertiesAsync(properties, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00028804 File Offset: 0x00026A04
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties, TableRequestOptions, OperationContext>(new Func<ServiceProperties, TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00028828 File Offset: 0x00026A28
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceStats(null, null, callback, state);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00028834 File Offset: 0x00026A34
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(TableRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00028861 File Offset: 0x00026A61
		public ServiceStats EndGetServiceStats(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceStats>(asyncResult);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00028869 File Offset: 0x00026A69
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync()
		{
			return this.GetServiceStatsAsync(CancellationToken.None);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00028876 File Offset: 0x00026A76
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceStats>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), cancellationToken);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00028896 File Offset: 0x00026A96
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(TableRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServiceStatsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x000288A5 File Offset: 0x00026AA5
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<TableRequestOptions, OperationContext, ServiceStats>(new Func<TableRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000288C7 File Offset: 0x00026AC7
		[DoesServiceRequest]
		public ServiceStats GetServiceStats(TableRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = TableRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00028910 File Offset: 0x00026B10
		private RESTCommand<ServiceProperties> GetServicePropertiesImpl(TableRequestOptions requestOptions)
		{
			RESTCommand<ServiceProperties> restcommand = new RESTCommand<ServiceProperties>(this.Credentials, this.StorageUri);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(TableHttpWebRequestFactory.GetServiceProperties);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceProperties>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, OperationContext ctx) => TableHttpWebResponseParsers.ReadServiceProperties(cmd.ResponseStream));
			requestOptions.ApplyToStorageCommand<ServiceProperties>(restcommand);
			return restcommand;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000289DC File Offset: 0x00026BDC
		private RESTCommand<NullType> SetServicePropertiesImpl(ServiceProperties properties, TableRequestOptions requestOptions)
		{
			MultiBufferMemoryStream multiBufferMemoryStream = new MultiBufferMemoryStream(null, 1024);
			try
			{
				properties.WriteServiceProperties(multiBufferMemoryStream);
			}
			catch (InvalidOperationException ex)
			{
				multiBufferMemoryStream.Dispose();
				InvalidOperationException ex2;
				throw new ArgumentException(ex2.Message, "properties");
			}
			multiBufferMemoryStream.Seek(0L, SeekOrigin.Begin);
			RESTCommand<NullType> restcommand = new RESTCommand<NullType>(this.Credentials, this.StorageUri);
			restcommand.SendStream = multiBufferMemoryStream;
			restcommand.StreamToDispose = multiBufferMemoryStream;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(TableHttpWebRequestFactory.SetServiceProperties);
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.ParseError = new Func<Stream, HttpWebResponse, string, StorageExtendedErrorInformation>(StorageExtendedErrorInformation.ReadFromStreamUsingODataLib);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			requestOptions.ApplyToStorageCommand<NullType>(restcommand);
			return restcommand;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00028AEC File Offset: 0x00026CEC
		private RESTCommand<ServiceStats> GetServiceStatsImpl(TableRequestOptions requestOptions)
		{
			RESTCommand<ServiceStats> restcommand = new RESTCommand<ServiceStats>(this.Credentials, this.StorageUri);
			requestOptions.ApplyToStorageCommand<ServiceStats>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(TableHttpWebRequestFactory.GetServiceStats);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceStats>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, OperationContext ctx) => TableHttpWebResponseParsers.ReadServiceStats(cmd.ResponseStream));
			return restcommand;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00028B91 File Offset: 0x00026D91
		[Obsolete("Support for accessing Windows Azure Tables via WCF Data Services is now obsolete. It's recommended that you use the Microsoft.WindowsAzure.Storage.Table namespace for working with tables.")]
		public TableServiceContext GetTableServiceContext()
		{
			return new TableServiceContext(this);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00028B99 File Offset: 0x00026D99
		public CloudTableClient(Uri baseUri, StorageCredentials credentials) : this(new StorageUri(baseUri), credentials)
		{
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00028BA8 File Offset: 0x00026DA8
		public CloudTableClient(StorageUri storageUri, StorageCredentials credentials)
		{
			this.StorageUri = storageUri;
			this.Credentials = (credentials ?? new StorageCredentials());
			this.DefaultRequestOptions = new TableRequestOptions();
			this.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
			this.DefaultRequestOptions.LocationMode = new LocationMode?(Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			this.DefaultRequestOptions.PayloadFormat = new TablePayloadFormat?(TablePayloadFormat.Json);
			this.AuthenticationScheme = AuthenticationScheme.SharedKey;
			this.UsePathStyleUris = CommonUtility.UsePathStyleAddressing(this.BaseUri);
			if (!this.Credentials.IsSharedKey)
			{
				this.AccountName = NavigationHelper.GetAccountNameFromUri(this.BaseUri, new bool?(this.UsePathStyleUris));
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00028C50 File Offset: 0x00026E50
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00028C58 File Offset: 0x00026E58
		public IBufferManager BufferManager { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00028C61 File Offset: 0x00026E61
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00028C69 File Offset: 0x00026E69
		public StorageCredentials Credentials { get; private set; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00028C72 File Offset: 0x00026E72
		public Uri BaseUri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00028C7F File Offset: 0x00026E7F
		// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00028C87 File Offset: 0x00026E87
		public StorageUri StorageUri { get; private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00028C90 File Offset: 0x00026E90
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x00028C98 File Offset: 0x00026E98
		public TableRequestOptions DefaultRequestOptions { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00028CA1 File Offset: 0x00026EA1
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x00028CAE File Offset: 0x00026EAE
		[Obsolete("Use DefaultRequestOptions.RetryPolicy.")]
		public IRetryPolicy RetryPolicy
		{
			get
			{
				return this.DefaultRequestOptions.RetryPolicy;
			}
			set
			{
				this.DefaultRequestOptions.RetryPolicy = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00028CBC File Offset: 0x00026EBC
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x00028CC9 File Offset: 0x00026EC9
		[Obsolete("Use DefaultRequestOptions.LocationMode.")]
		public LocationMode? LocationMode
		{
			get
			{
				return this.DefaultRequestOptions.LocationMode;
			}
			set
			{
				this.DefaultRequestOptions.LocationMode = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00028CD7 File Offset: 0x00026ED7
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x00028CE4 File Offset: 0x00026EE4
		[Obsolete("Use DefaultRequestOptions.ServerTimeout.")]
		public TimeSpan? ServerTimeout
		{
			get
			{
				return this.DefaultRequestOptions.ServerTimeout;
			}
			set
			{
				this.DefaultRequestOptions.ServerTimeout = value;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00028CF2 File Offset: 0x00026EF2
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x00028CFF File Offset: 0x00026EFF
		[Obsolete("Use DefaultRequestOptions.MaximumExecutionTime.")]
		public TimeSpan? MaximumExecutionTime
		{
			get
			{
				return this.DefaultRequestOptions.MaximumExecutionTime;
			}
			set
			{
				this.DefaultRequestOptions.MaximumExecutionTime = value;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00028D0D File Offset: 0x00026F0D
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x00028D1A File Offset: 0x00026F1A
		[Obsolete("Use DefaultRequestOptions.PayloadFormat.")]
		public TablePayloadFormat? PayloadFormat
		{
			get
			{
				return this.DefaultRequestOptions.PayloadFormat;
			}
			set
			{
				this.DefaultRequestOptions.PayloadFormat = value;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00028D28 File Offset: 0x00026F28
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00028D30 File Offset: 0x00026F30
		internal bool UsePathStyleUris { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00028D39 File Offset: 0x00026F39
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00028D50 File Offset: 0x00026F50
		internal string AccountName
		{
			get
			{
				return this.accountName ?? this.Credentials.AccountName;
			}
			private set
			{
				this.accountName = value;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00028D59 File Offset: 0x00026F59
		public CloudTable GetTableReference(string tableName)
		{
			CommonUtility.AssertNotNullOrEmpty("tableName", tableName);
			return new CloudTable(tableName, this);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00028D6D File Offset: 0x00026F6D
		private ICanonicalizer GetCanonicalizer()
		{
			if (this.AuthenticationScheme == AuthenticationScheme.SharedKeyLite)
			{
				return SharedKeyLiteTableCanonicalizer.Instance;
			}
			return SharedKeyTableCanonicalizer.Instance;
		}

		// Token: 0x04000152 RID: 338
		private IAuthenticationHandler authenticationHandler;

		// Token: 0x04000153 RID: 339
		private AuthenticationScheme authenticationScheme;

		// Token: 0x04000154 RID: 340
		private string accountName;
	}
}
