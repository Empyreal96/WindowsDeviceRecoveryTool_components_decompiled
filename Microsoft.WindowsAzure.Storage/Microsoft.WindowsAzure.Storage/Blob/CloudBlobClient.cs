using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Auth.Protocol;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Executor;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x02000019 RID: 25
	public sealed class CloudBlobClient
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003C6 RID: 966 RVA: 0x0000D5FC File Offset: 0x0000B7FC
		// (set) Token: 0x060003C7 RID: 967 RVA: 0x0000D604 File Offset: 0x0000B804
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x0000D620 File Offset: 0x0000B820
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

		// Token: 0x060003C9 RID: 969 RVA: 0x0000D6BC File Offset: 0x0000B8BC
		[DoesServiceRequest]
		public IEnumerable<CloudBlobContainer> ListContainers(string prefix = null, ContainerListingDetails detailsIncluded = ContainerListingDetails.None, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions modifiedOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this, true);
			return CommonUtility.LazyEnumerable<CloudBlobContainer>((IContinuationToken token) => this.ListContainersSegmentedCore(prefix, detailsIncluded, null, (BlobContinuationToken)token, modifiedOptions, operationContext), long.MaxValue);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000D718 File Offset: 0x0000B918
		[DoesServiceRequest]
		public ContainerResultSegment ListContainersSegmented(BlobContinuationToken currentToken)
		{
			return this.ListContainersSegmented(null, ContainerListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000D73C File Offset: 0x0000B93C
		[DoesServiceRequest]
		public ContainerResultSegment ListContainersSegmented(string prefix, BlobContinuationToken currentToken)
		{
			return this.ListContainersSegmented(prefix, ContainerListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000D760 File Offset: 0x0000B960
		[DoesServiceRequest]
		public ContainerResultSegment ListContainersSegmented(string prefix, ContainerListingDetails detailsIncluded, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			BlobRequestOptions options2 = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this, true);
			ResultSegment<CloudBlobContainer> resultSegment = this.ListContainersSegmentedCore(prefix, detailsIncluded, maxResults, currentToken, options2, operationContext);
			return new ContainerResultSegment(resultSegment.Results, (BlobContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000D79D File Offset: 0x0000B99D
		private ResultSegment<CloudBlobContainer> ListContainersSegmentedCore(string prefix, ContainerListingDetails detailsIncluded, int? maxResults, BlobContinuationToken continuationToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return Executor.ExecuteSync<ResultSegment<CloudBlobContainer>>(this.ListContainersImpl(prefix, detailsIncluded, continuationToken, maxResults, options), options.RetryPolicy, operationContext);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000D7BC File Offset: 0x0000B9BC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListContainersSegmented(BlobContinuationToken continuationToken, AsyncCallback callback, object state)
		{
			return this.BeginListContainersSegmented(null, ContainerListingDetails.None, null, continuationToken, null, null, callback, state);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000D7E0 File Offset: 0x0000B9E0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListContainersSegmented(string prefix, BlobContinuationToken continuationToken, AsyncCallback callback, object state)
		{
			return this.BeginListContainersSegmented(prefix, ContainerListingDetails.None, null, continuationToken, null, null, callback, state);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000D804 File Offset: 0x0000BA04
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListContainersSegmented(string prefix, ContainerListingDetails detailsIncluded, int? maxResults, BlobContinuationToken continuationToken, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this, true);
			return Executor.BeginExecuteAsync<ResultSegment<CloudBlobContainer>>(this.ListContainersImpl(prefix, detailsIncluded, continuationToken, maxResults, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000D83C File Offset: 0x0000BA3C
		public ContainerResultSegment EndListContainersSegmented(IAsyncResult asyncResult)
		{
			ResultSegment<CloudBlobContainer> resultSegment = Executor.EndExecuteAsync<ResultSegment<CloudBlobContainer>>(asyncResult);
			return new ContainerResultSegment(resultSegment.Results, (BlobContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000D866 File Offset: 0x0000BA66
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(BlobContinuationToken continuationToken)
		{
			return this.ListContainersSegmentedAsync(continuationToken, CancellationToken.None);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000D874 File Offset: 0x0000BA74
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(BlobContinuationToken continuationToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobContinuationToken, ContainerResultSegment>(new Func<BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListContainersSegmented), new Func<IAsyncResult, ContainerResultSegment>(this.EndListContainersSegmented), continuationToken, cancellationToken);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000D895 File Offset: 0x0000BA95
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(string prefix, BlobContinuationToken continuationToken)
		{
			return this.ListContainersSegmentedAsync(prefix, continuationToken, CancellationToken.None);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(string prefix, BlobContinuationToken continuationToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, BlobContinuationToken, ContainerResultSegment>(new Func<string, BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListContainersSegmented), new Func<IAsyncResult, ContainerResultSegment>(this.EndListContainersSegmented), prefix, continuationToken, cancellationToken);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000D8C6 File Offset: 0x0000BAC6
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(string prefix, ContainerListingDetails detailsIncluded, int? maxResults, BlobContinuationToken continuationToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ListContainersSegmentedAsync(prefix, detailsIncluded, maxResults, continuationToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000D8DC File Offset: 0x0000BADC
		[DoesServiceRequest]
		public Task<ContainerResultSegment> ListContainersSegmentedAsync(string prefix, ContainerListingDetails detailsIncluded, int? maxResults, BlobContinuationToken continuationToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, ContainerListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, ContainerResultSegment>(new Func<string, ContainerListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListContainersSegmented), new Func<IAsyncResult, ContainerResultSegment>(this.EndListContainersSegmented), prefix, detailsIncluded, maxResults, continuationToken, options, operationContext, cancellationToken);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000D914 File Offset: 0x0000BB14
		[DoesServiceRequest]
		public IEnumerable<IListBlobItem> ListBlobs(string prefix, bool useFlatBlobListing = false, BlobListingDetails blobListingDetails = BlobListingDetails.None, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			string containerName;
			string prefix2;
			CloudBlobClient.ParseUserPrefix(prefix, out containerName, out prefix2);
			CloudBlobContainer containerReference = this.GetContainerReference(containerName);
			return containerReference.ListBlobs(prefix2, useFlatBlobListing, blobListingDetails, options, operationContext);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000D940 File Offset: 0x0000BB40
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(string prefix, BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmented(prefix, false, BlobListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000D964 File Offset: 0x0000BB64
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			string containerName;
			string prefix2;
			CloudBlobClient.ParseUserPrefix(prefix, out containerName, out prefix2);
			CloudBlobContainer containerReference = this.GetContainerReference(containerName);
			return containerReference.ListBlobsSegmented(prefix2, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000D994 File Offset: 0x0000BB94
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(string prefix, BlobContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListBlobsSegmented(prefix, false, BlobListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000DA28 File Offset: 0x0000BC28
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			string containerName;
			string prefix2;
			CloudBlobClient.ParseUserPrefix(prefix, out containerName, out prefix2);
			CloudBlobContainer container = this.GetContainerReference(containerName);
			StorageAsyncResult<BlobResultSegment> result = new StorageAsyncResult<BlobResultSegment>(callback, state);
			ICancellableAsyncResult @object = container.BeginListBlobsSegmented(prefix2, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, delegate(IAsyncResult ar)
			{
				result.UpdateCompletedSynchronously(ar.CompletedSynchronously);
				try
				{
					result.Result = container.EndListBlobsSegmented(ar);
					result.OnComplete();
				}
				catch (Exception exception)
				{
					result.OnComplete(exception);
				}
			}, null);
			result.CancelDelegate = new Action(@object.Cancel);
			return result;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
		public BlobResultSegment EndListBlobsSegmented(IAsyncResult asyncResult)
		{
			StorageAsyncResult<BlobResultSegment> storageAsyncResult = (StorageAsyncResult<BlobResultSegment>)asyncResult;
			storageAsyncResult.End();
			return storageAsyncResult.Result;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmentedAsync(prefix, currentToken, CancellationToken.None);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000DAD3 File Offset: 0x0000BCD3
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, BlobContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, BlobContinuationToken, BlobResultSegment>(new Func<string, BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), prefix, currentToken, cancellationToken);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ListBlobsSegmentedAsync(prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, BlobResultSegment>(new Func<string, bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000DB53 File Offset: 0x0000BD53
		[DoesServiceRequest]
		public ICloudBlob GetBlobReferenceFromServer(Uri blobUri, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("blobUri", blobUri);
			return this.GetBlobReferenceFromServer(new StorageUri(blobUri), accessCondition, options, operationContext);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000DB70 File Offset: 0x0000BD70
		[DoesServiceRequest]
		public ICloudBlob GetBlobReferenceFromServer(StorageUri blobUri, AccessCondition accessCondition = null, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			CommonUtility.AssertNotNull("blobUri", blobUri);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this, true);
			return Executor.ExecuteSync<ICloudBlob>(this.GetBlobReferenceImpl(blobUri, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000DBA8 File Offset: 0x0000BDA8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetBlobReferenceFromServer(Uri blobUri, AsyncCallback callback, object state)
		{
			return this.BeginGetBlobReferenceFromServer(blobUri, null, null, null, callback, state);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000DBB6 File Offset: 0x0000BDB6
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetBlobReferenceFromServer(Uri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("blobUri", blobUri);
			return this.BeginGetBlobReferenceFromServer(new StorageUri(blobUri), accessCondition, options, operationContext, callback, state);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetBlobReferenceFromServer(StorageUri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			CommonUtility.AssertNotNull("blobUri", blobUri);
			BlobRequestOptions blobRequestOptions = BlobRequestOptions.ApplyDefaults(options, BlobType.Unspecified, this, true);
			return Executor.BeginExecuteAsync<ICloudBlob>(this.GetBlobReferenceImpl(blobUri, accessCondition, blobRequestOptions), blobRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000DC14 File Offset: 0x0000BE14
		public ICloudBlob EndGetBlobReferenceFromServer(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ICloudBlob>(asyncResult);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(Uri blobUri)
		{
			return this.GetBlobReferenceFromServerAsync(blobUri, CancellationToken.None);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000DC2A File Offset: 0x0000BE2A
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(Uri blobUri, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, ICloudBlob>(new Func<Uri, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetBlobReferenceFromServer), new Func<IAsyncResult, ICloudBlob>(this.EndGetBlobReferenceFromServer), blobUri, cancellationToken);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000DC4B File Offset: 0x0000BE4B
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(Uri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.GetBlobReferenceFromServerAsync(blobUri, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000DC5D File Offset: 0x0000BE5D
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(Uri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<Uri, AccessCondition, BlobRequestOptions, OperationContext, ICloudBlob>(new Func<Uri, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetBlobReferenceFromServer), new Func<IAsyncResult, ICloudBlob>(this.EndGetBlobReferenceFromServer), blobUri, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000DC83 File Offset: 0x0000BE83
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(StorageUri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.GetBlobReferenceFromServerAsync(blobUri, accessCondition, options, operationContext, CancellationToken.None);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000DC95 File Offset: 0x0000BE95
		[DoesServiceRequest]
		public Task<ICloudBlob> GetBlobReferenceFromServerAsync(StorageUri blobUri, AccessCondition accessCondition, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<StorageUri, AccessCondition, BlobRequestOptions, OperationContext, ICloudBlob>(new Func<StorageUri, AccessCondition, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetBlobReferenceFromServer), new Func<IAsyncResult, ICloudBlob>(this.EndGetBlobReferenceFromServer), blobUri, accessCondition, options, operationContext, cancellationToken);
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000DD84 File Offset: 0x0000BF84
		private RESTCommand<ResultSegment<CloudBlobContainer>> ListContainersImpl(string prefix, ContainerListingDetails detailsIncluded, BlobContinuationToken currentToken, int? maxResults, BlobRequestOptions options)
		{
			ListingContext listingContext = new ListingContext(prefix, maxResults)
			{
				Marker = ((currentToken != null) ? currentToken.NextMarker : null)
			};
			RESTCommand<ResultSegment<CloudBlobContainer>> restcommand = new RESTCommand<ResultSegment<CloudBlobContainer>>(this.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ResultSegment<CloudBlobContainer>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(currentToken);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => ContainerHttpWebRequestFactory.List(uri, serverTimeout, listingContext, detailsIncluded, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<ResultSegment<CloudBlobContainer>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ResultSegment<CloudBlobContainer>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<ResultSegment<CloudBlobContainer>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ListContainersResponse listContainersResponse = new ListContainersResponse(cmd.ResponseStream);
				List<CloudBlobContainer> result = (from item in listContainersResponse.Containers
				select new CloudBlobContainer(item.Properties, item.Metadata, item.Name, this)).ToList<CloudBlobContainer>();
				BlobContinuationToken continuationToken = null;
				if (listContainersResponse.NextMarker != null)
				{
					continuationToken = new BlobContinuationToken
					{
						NextMarker = listContainersResponse.NextMarker,
						TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation)
					};
				}
				return new ResultSegment<CloudBlobContainer>(result)
				{
					ContinuationToken = continuationToken
				};
			};
			return restcommand;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000DF0C File Offset: 0x0000C10C
		private RESTCommand<ICloudBlob> GetBlobReferenceImpl(StorageUri blobUri, AccessCondition accessCondition, BlobRequestOptions options)
		{
			DateTimeOffset? parsedSnapshot;
			StorageCredentials storageCredentials;
			blobUri = NavigationHelper.ParseBlobQueryAndVerify(blobUri, out storageCredentials, out parsedSnapshot);
			CloudBlobClient client = (storageCredentials != null) ? new CloudBlobClient(this.StorageUri, storageCredentials) : this;
			RESTCommand<ICloudBlob> restcommand = new RESTCommand<ICloudBlob>(client.Credentials, blobUri);
			options.ApplyToStorageCommand<ICloudBlob>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => BlobHttpWebRequestFactory.GetProperties(uri, serverTimeout, parsedSnapshot, accessCondition, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(client.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = delegate(RESTCommand<ICloudBlob> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx)
			{
				HttpResponseParsers.ProcessExpectedStatusCodeNoException<ICloudBlob>(HttpStatusCode.OK, resp, null, cmd, ex);
				BlobAttributes blobAttributes = new BlobAttributes
				{
					StorageUri = blobUri,
					SnapshotTime = parsedSnapshot
				};
				CloudBlob.UpdateAfterFetchAttributes(blobAttributes, resp, false);
				switch (blobAttributes.Properties.BlobType)
				{
				case BlobType.PageBlob:
					return new CloudPageBlob(blobAttributes, client);
				case BlobType.BlockBlob:
					return new CloudBlockBlob(blobAttributes, client);
				case BlobType.AppendBlob:
					return new CloudAppendBlob(blobAttributes, client);
				default:
					throw new InvalidOperationException();
				}
			};
			return restcommand;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000DFC5 File Offset: 0x0000C1C5
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceProperties(null, null, callback, state);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000DFD1 File Offset: 0x0000C1D1
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(BlobRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E000 File Offset: 0x0000C200
		public ServiceProperties EndGetServiceProperties(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceProperties>(asyncResult);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000E008 File Offset: 0x0000C208
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync()
		{
			return this.GetServicePropertiesAsync(CancellationToken.None);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E015 File Offset: 0x0000C215
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceProperties>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), cancellationToken);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E035 File Offset: 0x0000C235
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(BlobRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServicePropertiesAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E044 File Offset: 0x0000C244
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(BlobRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobRequestOptions, OperationContext, ServiceProperties>(new Func<BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E066 File Offset: 0x0000C266
		[DoesServiceRequest]
		public ServiceProperties GetServiceProperties(BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E092 File Offset: 0x0000C292
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, AsyncCallback callback, object state)
		{
			return this.BeginSetServiceProperties(properties, null, null, callback, state);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E09F File Offset: 0x0000C29F
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, BlobRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
		public void EndSetServiceProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E0D9 File Offset: 0x0000C2D9
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties)
		{
			return this.SetServicePropertiesAsync(properties, CancellationToken.None);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E0E7 File Offset: 0x0000C2E7
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties>(new Func<ServiceProperties, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, cancellationToken);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000E108 File Offset: 0x0000C308
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, BlobRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.SetServicePropertiesAsync(properties, requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000E118 File Offset: 0x0000C318
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, BlobRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties, BlobRequestOptions, OperationContext>(new Func<ServiceProperties, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000E13C File Offset: 0x0000C33C
		[DoesServiceRequest]
		public void SetServiceProperties(ServiceProperties properties, BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000E16A File Offset: 0x0000C36A
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceStats(null, null, callback, state);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E176 File Offset: 0x0000C376
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(BlobRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000E1A5 File Offset: 0x0000C3A5
		public ServiceStats EndGetServiceStats(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceStats>(asyncResult);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000E1AD File Offset: 0x0000C3AD
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync()
		{
			return this.GetServiceStatsAsync(CancellationToken.None);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000E1BA File Offset: 0x0000C3BA
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceStats>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), cancellationToken);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E1DA File Offset: 0x0000C3DA
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(BlobRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServiceStatsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000E1E9 File Offset: 0x0000C3E9
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(BlobRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobRequestOptions, OperationContext, ServiceStats>(new Func<BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000E20B File Offset: 0x0000C40B
		[DoesServiceRequest]
		public ServiceStats GetServiceStats(BlobRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = BlobRequestOptions.ApplyDefaults(requestOptions, BlobType.Unspecified, this, true);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000E254 File Offset: 0x0000C454
		private RESTCommand<ServiceProperties> GetServicePropertiesImpl(BlobRequestOptions requestOptions)
		{
			RESTCommand<ServiceProperties> restcommand = new RESTCommand<ServiceProperties>(this.Credentials, this.StorageUri);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(BlobHttpWebRequestFactory.GetServiceProperties);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceProperties>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, OperationContext ctx) => BlobHttpResponseParsers.ReadServiceProperties(cmd.ResponseStream));
			requestOptions.ApplyToStorageCommand<ServiceProperties>(restcommand);
			return restcommand;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000E310 File Offset: 0x0000C510
		private RESTCommand<NullType> SetServicePropertiesImpl(ServiceProperties properties, BlobRequestOptions requestOptions)
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
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(BlobHttpWebRequestFactory.SetServiceProperties);
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			requestOptions.ApplyToStorageCommand<NullType>(restcommand);
			return restcommand;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000E40C File Offset: 0x0000C60C
		private RESTCommand<ServiceStats> GetServiceStatsImpl(BlobRequestOptions requestOptions)
		{
			RESTCommand<ServiceStats> restcommand = new RESTCommand<ServiceStats>(this.Credentials, this.StorageUri);
			requestOptions.ApplyToStorageCommand<ServiceStats>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(BlobHttpWebRequestFactory.GetServiceStats);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceStats>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, OperationContext ctx) => BlobHttpResponseParsers.ReadServiceStats(cmd.ResponseStream));
			return restcommand;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000E4B1 File Offset: 0x0000C6B1
		public CloudBlobClient(Uri baseUri) : this(baseUri, null)
		{
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000E4BB File Offset: 0x0000C6BB
		public CloudBlobClient(Uri baseUri, StorageCredentials credentials) : this(new StorageUri(baseUri), credentials)
		{
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		public CloudBlobClient(StorageUri storageUri, StorageCredentials credentials)
		{
			this.StorageUri = storageUri;
			this.Credentials = (credentials ?? new StorageCredentials());
			this.DefaultRequestOptions = new BlobRequestOptions();
			this.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
			this.DefaultRequestOptions.LocationMode = new LocationMode?(Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			this.DefaultRequestOptions.SingleBlobUploadThresholdInBytes = new long?(33554432L);
			this.DefaultRequestOptions.ParallelOperationThreadCount = new int?(1);
			this.DefaultDelimiter = "/";
			this.AuthenticationScheme = AuthenticationScheme.SharedKey;
			this.UsePathStyleUris = CommonUtility.UsePathStyleAddressing(this.BaseUri);
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000E56C File Offset: 0x0000C76C
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x0000E574 File Offset: 0x0000C774
		public IBufferManager BufferManager { get; set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000E57D File Offset: 0x0000C77D
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000E585 File Offset: 0x0000C785
		public StorageCredentials Credentials { get; private set; }

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000E58E File Offset: 0x0000C78E
		public Uri BaseUri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000E59B File Offset: 0x0000C79B
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0000E5A3 File Offset: 0x0000C7A3
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000E5AC File Offset: 0x0000C7AC
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		public BlobRequestOptions DefaultRequestOptions { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000E5BD File Offset: 0x0000C7BD
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000E5CA File Offset: 0x0000C7CA
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

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0000E5D8 File Offset: 0x0000C7D8
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x0000E5E5 File Offset: 0x0000C7E5
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0000E600 File Offset: 0x0000C800
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0000E60E File Offset: 0x0000C80E
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x0000E61B File Offset: 0x0000C81B
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000E629 File Offset: 0x0000C829
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000E631 File Offset: 0x0000C831
		public string DefaultDelimiter
		{
			get
			{
				return this.defaultDelimiter;
			}
			set
			{
				CommonUtility.AssertNotNullOrEmpty("DefaultDelimiter", value);
				CommonUtility.AssertNotNullOrEmpty("DefaultDelimiter", value);
				if (value == "\\")
				{
					throw new ArgumentException("\\ is an invalid delimiter.");
				}
				this.defaultDelimiter = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000E668 File Offset: 0x0000C868
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x0000E675 File Offset: 0x0000C875
		[Obsolete("Use DefaultRequestOptions.SingleBlobUploadThresholdInBytes.")]
		public long? SingleBlobUploadThresholdInBytes
		{
			get
			{
				return this.DefaultRequestOptions.SingleBlobUploadThresholdInBytes;
			}
			set
			{
				this.DefaultRequestOptions.SingleBlobUploadThresholdInBytes = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000E683 File Offset: 0x0000C883
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000E690 File Offset: 0x0000C890
		[Obsolete("Use DefaultRequestOptions.ParallelOperationThreadCount.")]
		public int? ParallelOperationThreadCount
		{
			get
			{
				return this.DefaultRequestOptions.ParallelOperationThreadCount;
			}
			set
			{
				this.DefaultRequestOptions.ParallelOperationThreadCount = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000E69E File Offset: 0x0000C89E
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x0000E6A6 File Offset: 0x0000C8A6
		internal bool UsePathStyleUris { get; private set; }

		// Token: 0x06000427 RID: 1063 RVA: 0x0000E6AF File Offset: 0x0000C8AF
		public CloudBlobContainer GetRootContainerReference()
		{
			return new CloudBlobContainer("$root", this);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000E6BC File Offset: 0x0000C8BC
		public CloudBlobContainer GetContainerReference(string containerName)
		{
			CommonUtility.AssertNotNullOrEmpty("containerName", containerName);
			return new CloudBlobContainer(containerName, this);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		private ICanonicalizer GetCanonicalizer()
		{
			if (this.AuthenticationScheme == AuthenticationScheme.SharedKeyLite)
			{
				return SharedKeyLiteCanonicalizer.Instance;
			}
			return SharedKeyCanonicalizer.Instance;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000E6E8 File Offset: 0x0000C8E8
		private static void ParseUserPrefix(string prefix, out string containerName, out string listingPrefix)
		{
			CommonUtility.AssertNotNull("prefix", prefix);
			containerName = null;
			listingPrefix = null;
			string[] array = prefix.Split(NavigationHelper.SlashAsSplitOptions, 2, StringSplitOptions.None);
			if (array.Length == 1)
			{
				listingPrefix = array[0];
			}
			else
			{
				containerName = array[0];
				listingPrefix = array[1];
			}
			if (string.IsNullOrEmpty(containerName))
			{
				containerName = "$root";
			}
			if (string.IsNullOrEmpty(listingPrefix))
			{
				listingPrefix = null;
			}
		}

		// Token: 0x040000A8 RID: 168
		private IAuthenticationHandler authenticationHandler;

		// Token: 0x040000A9 RID: 169
		private string defaultDelimiter;

		// Token: 0x040000AA RID: 170
		private AuthenticationScheme authenticationScheme;
	}
}
