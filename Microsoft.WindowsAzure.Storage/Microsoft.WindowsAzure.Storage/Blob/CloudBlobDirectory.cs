using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x0200001B RID: 27
	public sealed class CloudBlobDirectory : IListBlobItem
	{
		// Token: 0x06000502 RID: 1282 RVA: 0x000110EC File Offset: 0x0000F2EC
		[DoesServiceRequest]
		public IEnumerable<IListBlobItem> ListBlobs(bool useFlatBlobListing = false, BlobListingDetails blobListingDetails = BlobListingDetails.None, BlobRequestOptions options = null, OperationContext operationContext = null)
		{
			return this.Container.ListBlobs(this.Prefix, useFlatBlobListing, blobListingDetails, options, operationContext);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00011104 File Offset: 0x0000F304
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmented(false, BlobListingDetails.None, null, currentToken, null, null);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00011125 File Offset: 0x0000F325
		[DoesServiceRequest]
		public BlobResultSegment ListBlobsSegmented(bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.Container.ListBlobsSegmented(this.Prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00011144 File Offset: 0x0000F344
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(BlobContinuationToken currentToken, AsyncCallback callback, object state)
		{
			return this.BeginListBlobsSegmented(false, BlobListingDetails.None, null, currentToken, null, null, callback, state);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00011168 File Offset: 0x0000F368
		[DoesServiceRequest]
		public ICancellableAsyncResult BeginListBlobsSegmented(bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, AsyncCallback callback, object state)
		{
			return this.Container.BeginListBlobsSegmented(this.Prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, callback, state);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00011193 File Offset: 0x0000F393
		public BlobResultSegment EndListBlobsSegmented(IAsyncResult asyncResult)
		{
			return this.Container.EndListBlobsSegmented(asyncResult);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000111A1 File Offset: 0x0000F3A1
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(BlobContinuationToken currentToken)
		{
			return this.ListBlobsSegmentedAsync(currentToken, CancellationToken.None);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000111AF File Offset: 0x0000F3AF
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(BlobContinuationToken currentToken, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<BlobContinuationToken, BlobResultSegment>(new Func<BlobContinuationToken, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), currentToken, cancellationToken);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000111D0 File Offset: 0x0000F3D0
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext)
		{
			return this.ListBlobsSegmentedAsync(useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, CancellationToken.None);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000111E8 File Offset: 0x0000F3E8
		[DoesServiceRequest]
		public Task<BlobResultSegment> ListBlobsSegmentedAsync(bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken)
		{
			return AsyncExtensions.TaskFromApm<bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, BlobResultSegment>(new Func<bool, BlobListingDetails, int?, BlobContinuationToken, BlobRequestOptions, OperationContext, AsyncCallback, object, ICancellableAsyncResult>(this.BeginListBlobsSegmented), new Func<IAsyncResult, BlobResultSegment>(this.EndListBlobsSegmented), useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, cancellationToken);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00011220 File Offset: 0x0000F420
		internal CloudBlobDirectory(StorageUri uri, string prefix, CloudBlobContainer container)
		{
			CommonUtility.AssertNotNull("uri", uri);
			CommonUtility.AssertNotNull("prefix", prefix);
			CommonUtility.AssertNotNull("container", container);
			this.ServiceClient = container.ServiceClient;
			this.Container = container;
			this.Prefix = prefix;
			this.StorageUri = uri;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x00011275 File Offset: 0x0000F475
		// (set) Token: 0x0600050E RID: 1294 RVA: 0x0001127D File Offset: 0x0000F47D
		public CloudBlobClient ServiceClient { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x00011286 File Offset: 0x0000F486
		public Uri Uri
		{
			get
			{
				return this.StorageUri.PrimaryUri;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00011293 File Offset: 0x0000F493
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x0001129B File Offset: 0x0000F49B
		public StorageUri StorageUri { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x000112A4 File Offset: 0x0000F4A4
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x000112AC File Offset: 0x0000F4AC
		public CloudBlobContainer Container { get; private set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x000112B8 File Offset: 0x0000F4B8
		public CloudBlobDirectory Parent
		{
			get
			{
				string prefix;
				StorageUri uri;
				if (this.parent == null && NavigationHelper.GetBlobParentNameAndAddress(this.StorageUri, this.ServiceClient.DefaultDelimiter, new bool?(this.ServiceClient.UsePathStyleUris), out prefix, out uri))
				{
					this.parent = new CloudBlobDirectory(uri, prefix, this.Container);
				}
				return this.parent;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00011312 File Offset: 0x0000F512
		// (set) Token: 0x06000516 RID: 1302 RVA: 0x0001131A File Offset: 0x0000F51A
		public string Prefix { get; private set; }

		// Token: 0x06000517 RID: 1303 RVA: 0x00011324 File Offset: 0x0000F524
		public CloudPageBlob GetPageBlobReference(string blobName)
		{
			return this.GetPageBlobReference(blobName, null);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00011344 File Offset: 0x0000F544
		public CloudPageBlob GetPageBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobAbsoluteUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName, this.ServiceClient.DefaultDelimiter);
			return new CloudPageBlob(blobAbsoluteUri, snapshotTime, this.ServiceClient.Credentials);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00011388 File Offset: 0x0000F588
		public CloudBlockBlob GetBlockBlobReference(string blobName)
		{
			return this.GetBlockBlobReference(blobName, null);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public CloudBlockBlob GetBlockBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobAbsoluteUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName, this.ServiceClient.DefaultDelimiter);
			return new CloudBlockBlob(blobAbsoluteUri, snapshotTime, this.ServiceClient.Credentials);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000113EC File Offset: 0x0000F5EC
		public CloudAppendBlob GetAppendBlobReference(string blobName)
		{
			return this.GetAppendBlobReference(blobName, null);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001140C File Offset: 0x0000F60C
		public CloudAppendBlob GetAppendBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobAbsoluteUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName, this.ServiceClient.DefaultDelimiter);
			return new CloudAppendBlob(blobAbsoluteUri, snapshotTime, this.ServiceClient.Credentials);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00011450 File Offset: 0x0000F650
		public CloudBlob GetBlobReference(string blobName)
		{
			return this.GetBlobReference(blobName, null);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00011470 File Offset: 0x0000F670
		public CloudBlob GetBlobReference(string blobName, DateTimeOffset? snapshotTime)
		{
			CommonUtility.AssertNotNullOrEmpty("blobName", blobName);
			StorageUri blobAbsoluteUri = NavigationHelper.AppendPathToUri(this.StorageUri, blobName, this.ServiceClient.DefaultDelimiter);
			return new CloudBlob(blobAbsoluteUri, snapshotTime, this.ServiceClient.Credentials);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000114B4 File Offset: 0x0000F6B4
		public CloudBlobDirectory GetDirectoryReference(string itemName)
		{
			CommonUtility.AssertNotNullOrEmpty("itemName", itemName);
			if (!itemName.EndsWith(this.ServiceClient.DefaultDelimiter, StringComparison.Ordinal))
			{
				itemName += this.ServiceClient.DefaultDelimiter;
			}
			StorageUri uri = NavigationHelper.AppendPathToUri(this.StorageUri, itemName, this.ServiceClient.DefaultDelimiter);
			return new CloudBlobDirectory(uri, this.Prefix + itemName, this.Container);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00011523 File Offset: 0x0000F723
		[Obsolete("GetSubdirectoryReference has been renamed to GetDirectoryReference.")]
		public CloudBlobDirectory GetSubdirectoryReference(string itemName)
		{
			return this.GetDirectoryReference(itemName);
		}

		// Token: 0x040000BE RID: 190
		private CloudBlobDirectory parent;
	}
}
