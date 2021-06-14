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
using Microsoft.WindowsAzure.Storage.Queue.Protocol;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x02000034 RID: 52
	public sealed class CloudQueueClient
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x000252EC File Offset: 0x000234EC
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x000252F4 File Offset: 0x000234F4
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

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x00025310 File Offset: 0x00023510
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

		// Token: 0x06000A44 RID: 2628 RVA: 0x000253AC File Offset: 0x000235AC
		public IEnumerable<CloudQueue> ListQueues(string prefix = null, QueueListingDetails queueListingDetails = QueueListingDetails.None, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions modifiedOptions = QueueRequestOptions.ApplyDefaults(options, this);
			operationContext = (operationContext ?? new OperationContext());
			return CommonUtility.LazyEnumerable<CloudQueue>((IContinuationToken token) => this.ListQueuesSegmentedCore(prefix, queueListingDetails, null, token as QueueContinuationToken, modifiedOptions, operationContext), long.MaxValue);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00025418 File Offset: 0x00023618
		public QueueResultSegment ListQueuesSegmented(QueueContinuationToken currentToken)
		{
			return this.ListQueuesSegmented(null, QueueListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002543C File Offset: 0x0002363C
		public QueueResultSegment ListQueuesSegmented(string prefix, QueueContinuationToken currentToken)
		{
			return this.ListQueuesSegmented(prefix, QueueListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00025460 File Offset: 0x00023660
		public QueueResultSegment ListQueuesSegmented(string prefix, QueueListingDetails queueListingDetails, int? maxResults, QueueContinuationToken currentToken, QueueRequestOptions options = null, OperationContext operationContext = null)
		{
			QueueRequestOptions options2 = QueueRequestOptions.ApplyDefaults(options, this);
			operationContext = (operationContext ?? new OperationContext());
			ResultSegment<CloudQueue> resultSegment = this.ListQueuesSegmentedCore(prefix, queueListingDetails, maxResults, currentToken, options2, operationContext);
			return new QueueResultSegment(resultSegment.Results, (QueueContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x000254A8 File Offset: 0x000236A8
		private ResultSegment<CloudQueue> ListQueuesSegmentedCore(string prefix, QueueListingDetails queueListingDetails, int? maxResults, QueueContinuationToken currentToken, QueueRequestOptions options, OperationContext operationContext)
		{
			return Executor.ExecuteSync<ResultSegment<CloudQueue>>(this.ListQueuesImpl(prefix, maxResults, queueListingDetails, options, currentToken), options.RetryPolicy, operationContext);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x000254C8 File Offset: 0x000236C8
		public ICancellableAsyncResult BeginListQueuesSegmented(QueueContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListQueuesSegmented(null, QueueListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000254EC File Offset: 0x000236EC
		public ICancellableAsyncResult BeginListQueuesSegmented(string prefix, QueueContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListQueuesSegmented(prefix, QueueListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00025510 File Offset: 0x00023710
		public ICancellableAsyncResult BeginListQueuesSegmented(string prefix, QueueListingDetails queueListingDetails, int? maxResults, QueueContinuationToken currentToken, QueueRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			QueueRequestOptions queueRequestOptions = QueueRequestOptions.ApplyDefaults(options, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ResultSegment<CloudQueue>>(this.ListQueuesImpl(prefix, maxResults, queueListingDetails, queueRequestOptions, currentToken), queueRequestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00025550 File Offset: 0x00023750
		public QueueResultSegment EndListQueuesSegmented(IAsyncResult asyncResult)
		{
			ResultSegment<CloudQueue> resultSegment = Executor.EndExecuteAsync<ResultSegment<CloudQueue>>(asyncResult);
			return new QueueResultSegment(resultSegment.Results, (QueueContinuationToken)resultSegment.ContinuationToken);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002557A File Offset: 0x0002377A
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(QueueContinuationToken currentToken)
		{
			return this.ListQueuesSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00025588 File Offset: 0x00023788
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(QueueContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueContinuationToken, QueueResultSegment>(new Func<QueueContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListQueuesSegmented), new Func<IAsyncResult, QueueResultSegment>(this.EndListQueuesSegmented), currentToken, cancellationToken);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000255A9 File Offset: 0x000237A9
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(string prefix, QueueContinuationToken currentToken)
		{
			return this.ListQueuesSegmentedAsync(prefix, currentToken, CancellationToken.None);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000255B8 File Offset: 0x000237B8
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(string prefix, QueueContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, QueueContinuationToken, QueueResultSegment>(new Func<string, QueueContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListQueuesSegmented), new Func<IAsyncResult, QueueResultSegment>(this.EndListQueuesSegmented), prefix, currentToken, cancellationToken);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000255DA File Offset: 0x000237DA
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(string prefix, QueueListingDetails queueListingDetails, int? maxResults, QueueContinuationToken currentToken, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.ListQueuesSegmentedAsync(prefix, queueListingDetails, maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000255F0 File Offset: 0x000237F0
		[DoesServiceRequest]
		public Task<QueueResultSegment> ListQueuesSegmentedAsync(string prefix, QueueListingDetails queueListingDetails, int? maxResults, QueueContinuationToken currentToken, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<string, QueueListingDetails, int?, QueueContinuationToken, QueueRequestOptions, OperationContext, QueueResultSegment>(new Func<string, QueueListingDetails, int?, QueueContinuationToken, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListQueuesSegmented), new Func<IAsyncResult, QueueResultSegment>(this.EndListQueuesSegmented), prefix, queueListingDetails, maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x000256E8 File Offset: 0x000238E8
		private RESTCommand<ResultSegment<CloudQueue>> ListQueuesImpl(string prefix, int? maxResults, QueueListingDetails queueListingDetails, QueueRequestOptions options, QueueContinuationToken currentToken)
		{
			QueueListingContext listingContext = new QueueListingContext(prefix, maxResults, queueListingDetails)
			{
				Marker = ((currentToken != null) ? currentToken.NextMarker : null)
			};
			RESTCommand<ResultSegment<CloudQueue>> restcommand = new RESTCommand<ResultSegment<CloudQueue>>(this.Credentials, this.StorageUri);
			options.ApplyToStorageCommand<ResultSegment<CloudQueue>>(restcommand);
			restcommand.CommandLocationMode = CommonUtility.GetListingLocationMode(currentToken);
			restcommand.RetrieveResponseStream = true;
			restcommand.BuildRequestDelegate = ((Uri uri, UriQueryBuilder builder, int? serverTimeout, bool useVersionHeader, OperationContext ctx) => QueueHttpWebRequestFactory.List(uri, serverTimeout, listingContext, queueListingDetails, useVersionHeader, ctx));
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<ResultSegment<CloudQueue>> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ResultSegment<CloudQueue>>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = delegate(RESTCommand<ResultSegment<CloudQueue>> cmd, HttpWebResponse resp, OperationContext ctx)
			{
				ListQueuesResponse listQueuesResponse = new ListQueuesResponse(cmd.ResponseStream);
				List<CloudQueue> result = (from item in listQueuesResponse.Queues
				select new CloudQueue(item.Metadata, item.Name, this)).ToList<CloudQueue>();
				QueueContinuationToken continuationToken = null;
				if (listQueuesResponse.NextMarker != null)
				{
					continuationToken = new QueueContinuationToken
					{
						NextMarker = listQueuesResponse.NextMarker,
						TargetLocation = new StorageLocation?(cmd.CurrentResult.TargetLocation)
					};
				}
				return new ResultSegment<CloudQueue>(result)
				{
					ContinuationToken = continuationToken
				};
			};
			return restcommand;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x000257C0 File Offset: 0x000239C0
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceProperties(null, null, callback, state);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000257CC File Offset: 0x000239CC
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceProperties(QueueRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x000257F9 File Offset: 0x000239F9
		public ServiceProperties EndGetServiceProperties(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceProperties>(asyncResult);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00025801 File Offset: 0x00023A01
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync()
		{
			return this.GetServicePropertiesAsync(CancellationToken.None);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0002580E File Offset: 0x00023A0E
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceProperties>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), cancellationToken);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0002582E File Offset: 0x00023A2E
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(QueueRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServicePropertiesAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0002583D File Offset: 0x00023A3D
		[DoesServiceRequest]
		public Task<ServiceProperties> GetServicePropertiesAsync(QueueRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, ServiceProperties>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceProperties), new Func<IAsyncResult, ServiceProperties>(this.EndGetServiceProperties), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0002585F File Offset: 0x00023A5F
		[DoesServiceRequest]
		public ServiceProperties GetServiceProperties(QueueRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceProperties>(this.GetServicePropertiesImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00025889 File Offset: 0x00023A89
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, AsyncCallback callback, object state)
		{
			return this.BeginSetServiceProperties(properties, null, null, callback, state);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00025896 File Offset: 0x00023A96
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginSetServiceProperties(ServiceProperties properties, QueueRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000258C5 File Offset: 0x00023AC5
		public void EndSetServiceProperties(IAsyncResult asyncResult)
		{
			Executor.EndExecuteAsync<NullType>(asyncResult);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000258CE File Offset: 0x00023ACE
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties)
		{
			return this.SetServicePropertiesAsync(properties, CancellationToken.None);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x000258DC File Offset: 0x00023ADC
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties>(new Func<ServiceProperties, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, cancellationToken);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000258FD File Offset: 0x00023AFD
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, QueueRequestOptions options, OperationContext operationContext)
		{
			return this.SetServicePropertiesAsync(properties, options, operationContext, CancellationToken.None);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0002590D File Offset: 0x00023B0D
		[DoesServiceRequest]
		public Task SetServicePropertiesAsync(ServiceProperties properties, QueueRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromVoidApm<ServiceProperties, QueueRequestOptions, OperationContext>(new Func<ServiceProperties, QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginSetServiceProperties), new Action<IAsyncResult>(this.EndSetServiceProperties), properties, options, operationContext, cancellationToken);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00025931 File Offset: 0x00023B31
		[DoesServiceRequest]
		public void SetServiceProperties(ServiceProperties properties, QueueRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			Executor.ExecuteSync<NullType>(this.SetServicePropertiesImpl(properties, requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x0002595D File Offset: 0x00023B5D
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(AsyncCallback callback, object state)
		{
			return this.BeginGetServiceStats(null, null, callback, state);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00025969 File Offset: 0x00023B69
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginGetServiceStats(QueueRequestOptions requestOptions, OperationContext operationContext, AsyncCallback callback, object state)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.BeginExecuteAsync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext, callback, state);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00025996 File Offset: 0x00023B96
		public ServiceStats EndGetServiceStats(IAsyncResult asyncResult)
		{
			return Executor.EndExecuteAsync<ServiceStats>(asyncResult);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x0002599E File Offset: 0x00023B9E
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync()
		{
			return this.GetServiceStatsAsync(CancellationToken.None);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000259AB File Offset: 0x00023BAB
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<ServiceStats>(new Func<AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), cancellationToken);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x000259CB File Offset: 0x00023BCB
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(QueueRequestOptions requestOptions, OperationContext operationContext)
		{
			return this.GetServiceStatsAsync(requestOptions, operationContext, CancellationToken.None);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x000259DA File Offset: 0x00023BDA
		[DoesServiceRequest]
		public Task<ServiceStats> GetServiceStatsAsync(QueueRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<QueueRequestOptions, OperationContext, ServiceStats>(new Func<QueueRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginGetServiceStats), new Func<IAsyncResult, ServiceStats>(this.EndGetServiceStats), requestOptions, operationContext, cancellationToken);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000259FC File Offset: 0x00023BFC
		[DoesServiceRequest]
		public ServiceStats GetServiceStats(QueueRequestOptions requestOptions = null, OperationContext operationContext = null)
		{
			requestOptions = QueueRequestOptions.ApplyDefaults(requestOptions, this);
			operationContext = (operationContext ?? new OperationContext());
			return Executor.ExecuteSync<ServiceStats>(this.GetServiceStatsImpl(requestOptions), requestOptions.RetryPolicy, operationContext);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00025A44 File Offset: 0x00023C44
		private RESTCommand<ServiceProperties> GetServicePropertiesImpl(QueueRequestOptions requestOptions)
		{
			RESTCommand<ServiceProperties> restcommand = new RESTCommand<ServiceProperties>(this.Credentials, this.StorageUri);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(QueueHttpWebRequestFactory.GetServiceProperties);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceProperties>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceProperties> cmd, HttpWebResponse resp, OperationContext ctx) => QueueHttpResponseParsers.ReadServiceProperties(cmd.ResponseStream));
			requestOptions.ApplyToStorageCommand<ServiceProperties>(restcommand);
			return restcommand;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00025B00 File Offset: 0x00023D00
		private RESTCommand<NullType> SetServicePropertiesImpl(ServiceProperties properties, QueueRequestOptions requestOptions)
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
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(QueueHttpWebRequestFactory.SetServiceProperties);
			restcommand.RecoveryAction = new Action<StorageCommandBase<NullType>, Exception, OperationContext>(RecoveryActions.RewindStream<NullType>);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.PreProcessResponse = ((RESTCommand<NullType> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<NullType>(HttpStatusCode.Accepted, resp, NullType.Value, cmd, ex));
			requestOptions.ApplyToStorageCommand<NullType>(restcommand);
			return restcommand;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00025BFC File Offset: 0x00023DFC
		private RESTCommand<ServiceStats> GetServiceStatsImpl(QueueRequestOptions requestOptions)
		{
			RESTCommand<ServiceStats> restcommand = new RESTCommand<ServiceStats>(this.Credentials, this.StorageUri);
			requestOptions.ApplyToStorageCommand<ServiceStats>(restcommand);
			restcommand.CommandLocationMode = CommandLocationMode.PrimaryOrSecondary;
			restcommand.BuildRequestDelegate = new Func<Uri, UriQueryBuilder, int?, bool, OperationContext, HttpWebRequest>(QueueHttpWebRequestFactory.GetServiceStats);
			restcommand.SignRequest = new Action<HttpWebRequest, OperationContext>(this.AuthenticationHandler.SignRequest);
			restcommand.RetrieveResponseStream = true;
			restcommand.PreProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, Exception ex, OperationContext ctx) => HttpResponseParsers.ProcessExpectedStatusCodeNoException<ServiceStats>(HttpStatusCode.OK, resp, null, cmd, ex));
			restcommand.PostProcessResponse = ((RESTCommand<ServiceStats> cmd, HttpWebResponse resp, OperationContext ctx) => QueueHttpResponseParsers.ReadServiceStats(cmd.ResponseStream));
			return restcommand;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00025CA1 File Offset: 0x00023EA1
		public CloudQueueClient(Uri baseUri, StorageCredentials credentials) : this(new StorageUri(baseUri), credentials)
		{
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00025CB0 File Offset: 0x00023EB0
		public CloudQueueClient(StorageUri storageUri, StorageCredentials credentials)
		{
			this.StorageUri = storageUri;
			this.Credentials = (credentials ?? new StorageCredentials());
			this.DefaultRequestOptions = new QueueRequestOptions();
			this.DefaultRequestOptions.RetryPolicy = new ExponentialRetry();
			this.DefaultRequestOptions.LocationMode = new LocationMode?(Microsoft.WindowsAzure.Storage.RetryPolicies.LocationMode.PrimaryOnly);
			this.AuthenticationScheme = AuthenticationScheme.SharedKey;
			this.UsePathStyleUris = CommonUtility.UsePathStyleAddressing(this.BaseUri);
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00025D1E File Offset: 0x00023F1E
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00025D26 File Offset: 0x00023F26
		public IBufferManager BufferManager { get; set; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00025D2F File Offset: 0x00023F2F
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00025D37 File Offset: 0x00023F37
		public StorageCredentials Credentials { get; private set; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00025D40 File Offset: 0x00023F40
		public Uri BaseUri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00025D4D File Offset: 0x00023F4D
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x00025D55 File Offset: 0x00023F55
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00025D5E File Offset: 0x00023F5E
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x00025D66 File Offset: 0x00023F66
		public QueueRequestOptions DefaultRequestOptions { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00025D6F File Offset: 0x00023F6F
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x00025D7C File Offset: 0x00023F7C
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

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00025D8A File Offset: 0x00023F8A
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x00025D97 File Offset: 0x00023F97
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

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00025DA5 File Offset: 0x00023FA5
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x00025DB2 File Offset: 0x00023FB2
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

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00025DC0 File Offset: 0x00023FC0
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x00025DCD File Offset: 0x00023FCD
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

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x00025DDB File Offset: 0x00023FDB
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x00025DE3 File Offset: 0x00023FE3
		internal bool UsePathStyleUris { get; private set; }

		// Token: 0x06000A84 RID: 2692 RVA: 0x00025DEC File Offset: 0x00023FEC
		public CloudQueue GetQueueReference(string queueName)
		{
			CommonUtility.AssertNotNullOrEmpty("queueName", queueName);
			return new CloudQueue(queueName, this);
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00025E00 File Offset: 0x00024000
		private ICanonicalizer GetCanonicalizer()
		{
			if (this.AuthenticationScheme == AuthenticationScheme.SharedKeyLite)
			{
				return SharedKeyLiteCanonicalizer.Instance;
			}
			return SharedKeyCanonicalizer.Instance;
		}

		// Token: 0x0400012F RID: 303
		private IAuthenticationHandler authenticationHandler;

		// Token: 0x04000130 RID: 304
		private AuthenticationScheme authenticationScheme;
	}
}
