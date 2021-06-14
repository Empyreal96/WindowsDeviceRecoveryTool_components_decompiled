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
using Microsoft.WindowsAzure.Storage.File.Protocol;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x02000025 RID: 37
	public sealed class CloudFileClient
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0001D07D File Offset: 0x0001B27D
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0001D085 File Offset: 0x0001B285
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001D0A0 File Offset: 0x0001B2A0
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

		// Token: 0x060007CF RID: 1999 RVA: 0x0001D13C File Offset: 0x0001B33C
		[DoesServiceRequest]
		public IEnumerable<CloudFileShare> ListShares(string prefix = null, ShareListingDetails detailsIncluded = ShareListingDetails.None, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions modifiedOptions = FileRequestOptions.ApplyDefaults(options, this, true);
			return CommonUtility.LazyEnumerable<CloudFileShare>((IContinuationToken token) => this.ListSharesSegmentedCore(prefix, detailsIncluded, null, (FileContinuationToken)token, modifiedOptions, operationContext), long.MaxValue);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001D194 File Offset: 0x0001B394
		[DoesServiceRequest]
		public ShareResultSegment ListSharesSegmented(FileContinuationToken currentToken)
		{
			return this.ListSharesSegmented(null, ShareListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001D1B8 File Offset: 0x0001B3B8
		[DoesServiceRequest]
		public ShareResultSegment ListSharesSegmented(string prefix, FileContinuationToken currentToken)
		{
			return this.ListSharesSegmented(prefix, ShareListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001D1DC File Offset: 0x0001B3DC
		[DoesServiceRequest]
		public ShareResultSegment ListSharesSegmented(string prefix, ShareListingDetails detailsIncluded, int? maxResults, FileContinuationToken currentToken, FileRequestOptions options = null, OperationContext operationContext = null)
		{
			FileRequestOptions options2 = FileRequestOptions.ApplyDefaults(options, this, true);
			ResultSegment<CloudFileShare> resultSegment = this.ListSharesSegmentedCore(prefix, detailsIncluded, maxResults, currentToken, options2, operationContext);
			return new ShareResultSegment(resultSegment.Results, (FileContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001D218 File Offset: 0x0001B418
		private ResultSegment<CloudFileShare> ListSharesSegmentedCore(string prefix, ShareListingDetails detailsIncluded, int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext)
		{
			return Executor.ExecuteSync<ResultSegment<CloudFileShare>>(this.ListSharesImpl(prefix, detailsIncluded, currentToken, maxResults, options), options.RetryPolicy, operationContext);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001D238 File Offset: 0x0001B438
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListSharesSegmented(FileContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListSharesSegmented(null, ShareListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001D25C File Offset: 0x0001B45C
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListSharesSegmented(string prefix, FileContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListSharesSegmented(prefix, ShareListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001D280 File Offset: 0x0001B480
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListSharesSegmented(string prefix, ShareListingDetails detailsIncluded, int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			FileRequestOptions fileRequestOptions = FileRequestOptions.ApplyDefaults(options, this, true);
			return Executor.BeginExecuteAsync<ResultSegment<CloudFileShare>>(this.ListSharesImpl(prefix, detailsIncluded, currentToken, maxResults, fileRequestOptions), fileRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001D2B4 File Offset: 0x0001B4B4
		public ShareResultSegment EndListSharesSegmented(IAsyncResult asyncResult)
		{
			ResultSegment<CloudFileShare> resultSegment = Executor.EndExecuteAsync<ResultSegment<CloudFileShare>>(asyncResult);
			return new ShareResultSegment(resultSegment.Results, (FileContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001D2DE File Offset: 0x0001B4DE
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(FileContinuationToken currentToken)
		{
			return this.ListSharesSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001D2EC File Offset: 0x0001B4EC
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(FileContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileContinuationToken, ShareResultSegment>(new Func<FileContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListSharesSegmented), new Func<IAsyncResult, ShareResultSegment>(this.EndListSharesSegmented), currentToken, cancellationToken);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001D30D File Offset: 0x0001B50D
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(string prefix, FileContinuationToken currentToken)
		{
			return this.ListSharesSegmentedAsync(prefix, currentToken, CancellationToken.None);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001D31C File Offset: 0x0001B51C
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(string prefix, FileContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, FileContinuationToken, ShareResultSegment>(new Func<string, FileContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListSharesSegmented), new Func<IAsyncResult, ShareResultSegment>(this.EndListSharesSegmented), prefix, currentToken, cancellationToken);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001D33E File Offset: 0x0001B53E
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(string prefix, ShareListingDetails detailsIncluded, int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext)
		{
			return this.ListSharesSegmentedAsync(prefix, detailsIncluded, maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001D354 File Offset: 0x0001B554
		[DoesServiceRequest]
		public Task<ShareResultSegment> ListSharesSegmentedAsync(string prefix, ShareListingDetails detailsIncluded, int? maxResults, FileContinuationToken currentToken, FileRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, ShareListingDetails, int?, FileContinuationToken, FileRequestOptions, OperationContext, ShareResultSegment>(new Func<string, ShareListingDetails, int?, FileContinuationToken, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListSharesSegmented), new Func<IAsyncResult, ShareResultSegment>(this.EndListSharesSegmented), prefix, detailsIncluded, maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001D389 File Offset: 0x0001B589
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceProperties(null, null, callback, state);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001D395 File Offset: 0x0001B595
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(FileRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<FileServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001D3C3 File Offset: 0x0001B5C3
		public FileServiceProperties EndGetServiceProperties(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<FileServiceProperties>(asyncResult);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001D3CB File Offset: 0x0001B5CB
		[DoesServiceRequest]
		public Task<FileServiceProperties> GetServicePropertiesAsync()
		{
			return this.GetServicePropertiesAsync(CancellationToken.None);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001D3D8 File Offset: 0x0001B5D8
		[DoesServiceRequest]
		public Task<FileServiceProperties> GetServicePropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileServiceProperties>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, FileServiceProperties>(this.EndGetServiceProperties), cancellationToken);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001D3F8 File Offset: 0x0001B5F8
		[DoesServiceRequest]
		public Task<FileServiceProperties> GetServicePropertiesAsync(FileRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServicePropertiesAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001D407 File Offset: 0x0001B607
		[DoesServiceRequest]
		public Task<FileServiceProperties> GetServicePropertiesAsync(FileRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<FileRequestOptions, OperationContext, FileServiceProperties>(new Func<FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, FileServiceProperties>(this.EndGetServiceProperties), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001D429 File Offset: 0x0001B629
		[DoesServiceRequest]
		public FileServiceProperties GetServiceProperties(FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<FileServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001D454 File Offset: 0x0001B654
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(FileServiceProperties properties, AsyncCallback callback, object state)
		{
			return this.BeginSetServiceProperties(properties, null, null, callback, state);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001D461 File Offset: 0x0001B661
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(FileServiceProperties properties, FileRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001D491 File Offset: 0x0001B691
		public void EndSetServiceProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001D49A File Offset: 0x0001B69A
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(FileServiceProperties properties)
		{
			return this.SetServicePropertiesAsync(properties, CancellationToken.None);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001D4A8 File Offset: 0x0001B6A8
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(FileServiceProperties properties, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileServiceProperties>(new Func<FileServiceProperties, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, cancellationToken);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001D4C9 File Offset: 0x0001B6C9
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(FileServiceProperties properties, FileRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.SetServicePropertiesAsync(properties, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001D4D9 File Offset: 0x0001B6D9
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(FileServiceProperties properties, FileRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<FileServiceProperties, FileRequestOptions, OperationContext>(new Func<FileServiceProperties, FileRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001D4FD File Offset: 0x0001B6FD
		[DoesServiceRequest]
		public void SetServiceProperties(FileServiceProperties properties, FileRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = FileRequestOptions.ApplyDefaults(requestOptions, this, true);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0001D5F4 File Offset: 0x0001B7F4
		private RESTCommand<ResultSegment<CloudFileShare>> ListSharesImpl(string prefix, ShareListingDetails detailsIncluded, FileContinuationToken currentToken, int? maxResults, FileRequestOptions options)
		{
			ListingContext listingContext = new ListingContext(prefix, maxResults)
			{
				Marker = ((currentToken != null) ? currentToken.NextMarker : null)
			};
			RESTCommand<ResultSegment<CloudFileShare>> restcommand = new RESTCommand<ResultSegment<CloudFileShare>>(this.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ResultSegment<CloudFileShare>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(currentToken);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ShareHttpWebRequestFactory.List(uri, serverTimeout, listingContext, detailsIncluded, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<ResultSegment<CloudFileShare>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ResultSegment<CloudFileShare>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<ResultSegment<CloudFileShare>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ListSharesResponse listSharesResponse = new ListSharesResponse(cmd.ResponseStream);
				List<CloudFileShare> result = new List<CloudFileShare>(from item in listSharesResponse.Shares
				select new CloudFileShare(item.Properties, item.Metadata, item.Name, this));
				FileContinuationToken continuationToken = null;
				if (listSharesResponse.NextMarker != null)
				{
					continuationToken = new FileContinuationToken
					{
						NextMarker = listSharesResponse.NextMarker,
						TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation)
					};
				}
				return new ResultSegment<CloudFileShare>(result)
				{
					ContinuationToken = continuationToken
				};
			};
			return restcommand;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001D6E4 File Offset: 0x0001B8E4
		private RESTCommand<FileServiceProperties> GetServicePropertiesImpl(FileRequestOptions requestOptions)
		{
			RESTCommand<FileServiceProperties> restcommand = new RESTCommand<FileServiceProperties>(this.Credentials, this.StorageUri);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(FileHttpWebRequestFactory.GetServiceProperties);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<FileServiceProperties> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<FileServiceProperties>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<FileServiceProperties> cmd, HttpWebResponse resp, OperationContext ctx) => FileHttpResponseParsers.ReadServiceProperties(cmd.ResponseStream));
			requestOptions.ApplyToStorageCommand<FileServiceProperties>(restcommand);
			return restcommand;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001D7A0 File Offset: 0x0001B9A0
		private RESTCommand<NullType> SetServicePropertiesImpl(FileServiceProperties properties, FileRequestOptions requestOptions)
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
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(FileHttpWebRequestFactory.SetServiceProperties);
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			requestOptions.ApplyToStorageCommand<NullType>(restcommand);
			return restcommand;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001D87C File Offset: 0x0001BA7C
		public CloudFileClient(Uri baseUri, StorageCredentials credentials) : this(new StorageUri(baseUri), credentials)
		{
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001D88C File Offset: 0x0001BA8C
		public CloudFileClient(StorageUri storageUri, StorageCredentials credentials)
		{
			this.StorageUri = storageUri;
			this.Credentials = (credentials ?? new StorageCredentials());
			this.DefaultRequestOptions = new FileRequestOptions();
			this.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
			this.DefaultRequestOptions.LocationMode = new LocationMode?(LocationMode.PrimaryOnly);
			this.DefaultRequestOptions.ParallelOperationThreadCount = new int?(1);
			this.AuthenticationScheme = AuthenticationScheme.SharedKey;
			this.UsePathStyleUris = CommonUtility.UsePathStyleAddressing(this.BaseUri);
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001D90B File Offset: 0x0001BB0B
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0001D913 File Offset: 0x0001BB13
		public IBufferManager BufferManager { get; set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x0001D91C File Offset: 0x0001BB1C
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x0001D924 File Offset: 0x0001BB24
		public StorageCredentials Credentials { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0001D92D File Offset: 0x0001BB2D
		public Uri BaseUri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001D93A File Offset: 0x0001BB3A
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001D942 File Offset: 0x0001BB42
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0001D94B File Offset: 0x0001BB4B
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x0001D953 File Offset: 0x0001BB53
		public FileRequestOptions DefaultRequestOptions { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x0001D95C File Offset: 0x0001BB5C
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0001D964 File Offset: 0x0001BB64
		internal bool UsePathStyleUris { get; private set; }

		// Token: 0x060007FE RID: 2046 RVA: 0x0001D96D File Offset: 0x0001BB6D
		public CloudFileShare GetShareReference(string shareName)
		{
			CommonUtility.AssertNotNullOrEmpty("shareName", shareName);
			return new CloudFileShare(shareName, this);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001D981 File Offset: 0x0001BB81
		private ICanonicalizer GetCanonicalizer()
		{
			if (this.AuthenticationScheme == AuthenticationScheme.SharedKeyLite)
			{
				return SharedKeyLiteCanonicalizer.Instance;
			}
			return SharedKeyCanonicalizer.Instance;
		}

		// Token: 0x040000D7 RID: 215
		private IAuthenticationHandler authenticationHandler;

		// Token: 0x040000D8 RID: 216
		private AuthenticationScheme authenticationScheme;
	}
}
