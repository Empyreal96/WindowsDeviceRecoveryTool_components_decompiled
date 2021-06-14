using System;
using System.Globalization;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000154 RID: 340
	public static class Constants
	{
		// Token: 0x04000861 RID: 2145
		public const int MaxParallelOperationThreadCount = 64;

		// Token: 0x04000862 RID: 2146
		public const int MaxSharedAccessPolicyIdentifiers = 5;

		// Token: 0x04000863 RID: 2147
		public const int DefaultWriteBlockSizeBytes = 4194304;

		// Token: 0x04000864 RID: 2148
		public const long MaxSingleUploadBlobSize = 67108864L;

		// Token: 0x04000865 RID: 2149
		public const int MaxBlockSize = 4194304;

		// Token: 0x04000866 RID: 2150
		public const int MaxRangeGetContentMD5Size = 4194304;

		// Token: 0x04000867 RID: 2151
		public const long MaxBlockNumber = 50000L;

		// Token: 0x04000868 RID: 2152
		public const long MaxBlobSize = 209715200000L;

		// Token: 0x04000869 RID: 2153
		internal const int DefaultBufferSize = 65536;

		// Token: 0x0400086A RID: 2154
		internal const string LogSourceName = "Microsoft.WindowsAzure.Storage";

		// Token: 0x0400086B RID: 2155
		public const int PageSize = 512;

		// Token: 0x0400086C RID: 2156
		public const long KB = 1024L;

		// Token: 0x0400086D RID: 2157
		public const long MB = 1048576L;

		// Token: 0x0400086E RID: 2158
		public const long GB = 1073741824L;

		// Token: 0x0400086F RID: 2159
		public const string CommittedBlocksElement = "CommittedBlocks";

		// Token: 0x04000870 RID: 2160
		public const string UncommittedBlocksElement = "UncommittedBlocks";

		// Token: 0x04000871 RID: 2161
		public const string BlockElement = "Block";

		// Token: 0x04000872 RID: 2162
		public const string NameElement = "Name";

		// Token: 0x04000873 RID: 2163
		public const string SizeElement = "Size";

		// Token: 0x04000874 RID: 2164
		public const string BlockListElement = "BlockList";

		// Token: 0x04000875 RID: 2165
		public const string MessagesElement = "QueueMessagesList";

		// Token: 0x04000876 RID: 2166
		public const string MessageElement = "QueueMessage";

		// Token: 0x04000877 RID: 2167
		public const string MessageIdElement = "MessageId";

		// Token: 0x04000878 RID: 2168
		public const string InsertionTimeElement = "InsertionTime";

		// Token: 0x04000879 RID: 2169
		public const string ExpirationTimeElement = "ExpirationTime";

		// Token: 0x0400087A RID: 2170
		public const string PopReceiptElement = "PopReceipt";

		// Token: 0x0400087B RID: 2171
		public const string TimeNextVisibleElement = "TimeNextVisible";

		// Token: 0x0400087C RID: 2172
		public const string MessageTextElement = "MessageText";

		// Token: 0x0400087D RID: 2173
		public const string DequeueCountElement = "DequeueCount";

		// Token: 0x0400087E RID: 2174
		public const string PageRangeElement = "PageRange";

		// Token: 0x0400087F RID: 2175
		public const string PageListElement = "PageList";

		// Token: 0x04000880 RID: 2176
		public const string StartElement = "Start";

		// Token: 0x04000881 RID: 2177
		public const string EndElement = "End";

		// Token: 0x04000882 RID: 2178
		public const string DelimiterElement = "Delimiter";

		// Token: 0x04000883 RID: 2179
		public const string BlobPrefixElement = "BlobPrefix";

		// Token: 0x04000884 RID: 2180
		public const string CacheControlElement = "Cache-Control";

		// Token: 0x04000885 RID: 2181
		public const string ContentTypeElement = "Content-Type";

		// Token: 0x04000886 RID: 2182
		public const string ContentEncodingElement = "Content-Encoding";

		// Token: 0x04000887 RID: 2183
		public const string ContentLanguageElement = "Content-Language";

		// Token: 0x04000888 RID: 2184
		public const string ContentLengthElement = "Content-Length";

		// Token: 0x04000889 RID: 2185
		public const string ContentMD5Element = "Content-MD5";

		// Token: 0x0400088A RID: 2186
		public const string EnumerationResultsElement = "EnumerationResults";

		// Token: 0x0400088B RID: 2187
		public const string ServiceEndpointElement = "ServiceEndpoint";

		// Token: 0x0400088C RID: 2188
		public const string ContainerNameElement = "ContainerName";

		// Token: 0x0400088D RID: 2189
		public const string ShareNameElement = "ShareName";

		// Token: 0x0400088E RID: 2190
		public const string DirectoryPathElement = "DirectoryPath";

		// Token: 0x0400088F RID: 2191
		public const string BlobsElement = "Blobs";

		// Token: 0x04000890 RID: 2192
		public const string PrefixElement = "Prefix";

		// Token: 0x04000891 RID: 2193
		public const string MaxResultsElement = "MaxResults";

		// Token: 0x04000892 RID: 2194
		public const string MarkerElement = "Marker";

		// Token: 0x04000893 RID: 2195
		public const string NextMarkerElement = "NextMarker";

		// Token: 0x04000894 RID: 2196
		public const string EtagElement = "Etag";

		// Token: 0x04000895 RID: 2197
		public const string LastModifiedElement = "Last-Modified";

		// Token: 0x04000896 RID: 2198
		public const string UrlElement = "Url";

		// Token: 0x04000897 RID: 2199
		public const string BlobElement = "Blob";

		// Token: 0x04000898 RID: 2200
		public const string CopyIdElement = "CopyId";

		// Token: 0x04000899 RID: 2201
		public const string CopyStatusElement = "CopyStatus";

		// Token: 0x0400089A RID: 2202
		public const string CopySourceElement = "CopySource";

		// Token: 0x0400089B RID: 2203
		public const string CopyProgressElement = "CopyProgress";

		// Token: 0x0400089C RID: 2204
		public const string CopyCompletionTimeElement = "CopyCompletionTime";

		// Token: 0x0400089D RID: 2205
		public const string CopyStatusDescriptionElement = "CopyStatusDescription";

		// Token: 0x0400089E RID: 2206
		public const string PageBlobValue = "PageBlob";

		// Token: 0x0400089F RID: 2207
		public const string BlockBlobValue = "BlockBlob";

		// Token: 0x040008A0 RID: 2208
		public const string AppendBlobValue = "AppendBlob";

		// Token: 0x040008A1 RID: 2209
		public const string LockedValue = "locked";

		// Token: 0x040008A2 RID: 2210
		public const string UnlockedValue = "unlocked";

		// Token: 0x040008A3 RID: 2211
		public const string LeaseAvailableValue = "available";

		// Token: 0x040008A4 RID: 2212
		public const string LeasedValue = "leased";

		// Token: 0x040008A5 RID: 2213
		public const string LeaseExpiredValue = "expired";

		// Token: 0x040008A6 RID: 2214
		public const string LeaseBreakingValue = "breaking";

		// Token: 0x040008A7 RID: 2215
		public const string LeaseBrokenValue = "broken";

		// Token: 0x040008A8 RID: 2216
		public const string LeaseInfiniteValue = "infinite";

		// Token: 0x040008A9 RID: 2217
		public const string LeaseFixedValue = "fixed";

		// Token: 0x040008AA RID: 2218
		public const string CopyPendingValue = "pending";

		// Token: 0x040008AB RID: 2219
		public const string CopySuccessValue = "success";

		// Token: 0x040008AC RID: 2220
		public const string CopyAbortedValue = "aborted";

		// Token: 0x040008AD RID: 2221
		public const string CopyFailedValue = "failed";

		// Token: 0x040008AE RID: 2222
		public const string GeoUnavailableValue = "unavailable";

		// Token: 0x040008AF RID: 2223
		public const string GeoLiveValue = "live";

		// Token: 0x040008B0 RID: 2224
		public const string GeoBootstrapValue = "bootstrap";

		// Token: 0x040008B1 RID: 2225
		public const string BlobTypeElement = "BlobType";

		// Token: 0x040008B2 RID: 2226
		public const string LeaseStatusElement = "LeaseStatus";

		// Token: 0x040008B3 RID: 2227
		public const string LeaseStateElement = "LeaseState";

		// Token: 0x040008B4 RID: 2228
		public const string LeaseDurationElement = "LeaseDuration";

		// Token: 0x040008B5 RID: 2229
		public const string SnapshotElement = "Snapshot";

		// Token: 0x040008B6 RID: 2230
		public const string ContainersElement = "Containers";

		// Token: 0x040008B7 RID: 2231
		public const string ContainerElement = "Container";

		// Token: 0x040008B8 RID: 2232
		public const string SharesElement = "Shares";

		// Token: 0x040008B9 RID: 2233
		public const string ShareElement = "Share";

		// Token: 0x040008BA RID: 2234
		public const string QuotaElement = "Quota";

		// Token: 0x040008BB RID: 2235
		public const string FileRangeElement = "Range";

		// Token: 0x040008BC RID: 2236
		public const string FileRangeListElement = "Ranges";

		// Token: 0x040008BD RID: 2237
		public const string EntriesElement = "Entries";

		// Token: 0x040008BE RID: 2238
		public const string FileElement = "File";

		// Token: 0x040008BF RID: 2239
		public const string FileDirectoryElement = "Directory";

		// Token: 0x040008C0 RID: 2240
		public const string QueuesElement = "Queues";

		// Token: 0x040008C1 RID: 2241
		public const string QueueNameElement = "Name";

		// Token: 0x040008C2 RID: 2242
		public const string QueueElement = "Queue";

		// Token: 0x040008C3 RID: 2243
		public const string PropertiesElement = "Properties";

		// Token: 0x040008C4 RID: 2244
		public const string MetadataElement = "Metadata";

		// Token: 0x040008C5 RID: 2245
		public const string InvalidMetadataName = "x-ms-invalid-name";

		// Token: 0x040008C6 RID: 2246
		public const string MaxResults = "MaxResults";

		// Token: 0x040008C7 RID: 2247
		public const string CommittedElement = "Committed";

		// Token: 0x040008C8 RID: 2248
		public const string UncommittedElement = "Uncommitted";

		// Token: 0x040008C9 RID: 2249
		public const string LatestElement = "Latest";

		// Token: 0x040008CA RID: 2250
		public const string SignedIdentifiers = "SignedIdentifiers";

		// Token: 0x040008CB RID: 2251
		public const string SignedIdentifier = "SignedIdentifier";

		// Token: 0x040008CC RID: 2252
		public const string AccessPolicy = "AccessPolicy";

		// Token: 0x040008CD RID: 2253
		public const string Id = "Id";

		// Token: 0x040008CE RID: 2254
		public const string Start = "Start";

		// Token: 0x040008CF RID: 2255
		public const string Expiry = "Expiry";

		// Token: 0x040008D0 RID: 2256
		public const string Permission = "Permission";

		// Token: 0x040008D1 RID: 2257
		public const string Messages = "messages";

		// Token: 0x040008D2 RID: 2258
		internal const string ErrorException = "exceptiondetails";

		// Token: 0x040008D3 RID: 2259
		public const string ErrorRootElement = "Error";

		// Token: 0x040008D4 RID: 2260
		public const string ErrorCode = "Code";

		// Token: 0x040008D5 RID: 2261
		internal const string ErrorCodePreview = "code";

		// Token: 0x040008D6 RID: 2262
		public const string ErrorMessage = "Message";

		// Token: 0x040008D7 RID: 2263
		internal const string ErrorMessagePreview = "message";

		// Token: 0x040008D8 RID: 2264
		public const string ErrorExceptionMessage = "ExceptionMessage";

		// Token: 0x040008D9 RID: 2265
		public const string ErrorExceptionStackTrace = "StackTrace";

		// Token: 0x040008DA RID: 2266
		internal const string EdmEntityTypeNamespaceName = "AzureTableStorage";

		// Token: 0x040008DB RID: 2267
		internal const string EdmEntityTypeName = "DefaultContainer";

		// Token: 0x040008DC RID: 2268
		internal const string EntitySetName = "Tables";

		// Token: 0x040008DD RID: 2269
		internal const string Edm = "Edm.";

		// Token: 0x040008DE RID: 2270
		internal const string DefaultNamespaceName = "account";

		// Token: 0x040008DF RID: 2271
		internal const string DefaultTableName = "TableName";

		// Token: 0x040008E0 RID: 2272
		internal const string XMLAcceptHeaderValue = "application/xml";

		// Token: 0x040008E1 RID: 2273
		internal const string AtomAcceptHeaderValue = "application/atom+xml,application/atomsvc+xml,application/xml";

		// Token: 0x040008E2 RID: 2274
		internal const string JsonLightAcceptHeaderValue = "application/json;odata=minimalmetadata";

		// Token: 0x040008E3 RID: 2275
		internal const string JsonFullMetadataAcceptHeaderValue = "application/json;odata=fullmetadata";

		// Token: 0x040008E4 RID: 2276
		internal const string JsonNoMetadataAcceptHeaderValue = "application/json;odata=nometadata";

		// Token: 0x040008E5 RID: 2277
		internal const string AtomContentTypeHeaderValue = "application/atom+xml";

		// Token: 0x040008E6 RID: 2278
		internal const string JsonContentTypeHeaderValue = "application/json";

		// Token: 0x040008E7 RID: 2279
		internal const string ETagPrefix = "\"datetime'";

		// Token: 0x040008E8 RID: 2280
		public static readonly TimeSpan MaxMaximumExecutionTime = TimeSpan.FromDays(24.0);

		// Token: 0x040008E9 RID: 2281
		public static readonly TimeSpan DefaultClientSideTimeout = TimeSpan.FromMinutes(5.0);

		// Token: 0x040008EA RID: 2282
		[Obsolete("Server-side timeout is not required by default.")]
		public static readonly TimeSpan DefaultServerSideTimeout = TimeSpan.FromSeconds(90.0);

		// Token: 0x040008EB RID: 2283
		public static readonly TimeSpan MaximumRetryBackoff = TimeSpan.FromHours(1.0);

		// Token: 0x040008EC RID: 2284
		public static readonly TimeSpan MaximumAllowedTimeout = TimeSpan.FromSeconds(2147483647.0);

		// Token: 0x02000155 RID: 341
		public static class HeaderConstants
		{
			// Token: 0x06001508 RID: 5384 RVA: 0x000500EC File Offset: 0x0004E2EC
			static HeaderConstants()
			{
				Constants.HeaderConstants.UserAgent = "WA-Storage/5.0.2 " + Constants.HeaderConstants.UserAgentComment;
			}

			// Token: 0x040008ED RID: 2285
			public const string UserAgentProductName = "WA-Storage";

			// Token: 0x040008EE RID: 2286
			public const string UserAgentProductVersion = "5.0.2";

			// Token: 0x040008EF RID: 2287
			public const string PrefixForStorageHeader = "x-ms-";

			// Token: 0x040008F0 RID: 2288
			public const string TrueHeader = "true";

			// Token: 0x040008F1 RID: 2289
			public const string FalseHeader = "false";

			// Token: 0x040008F2 RID: 2290
			public const string PrefixForStorageProperties = "x-ms-prop-";

			// Token: 0x040008F3 RID: 2291
			public const string PrefixForStorageMetadata = "x-ms-meta-";

			// Token: 0x040008F4 RID: 2292
			public const string ContentDispositionResponseHeader = "Content-Disposition";

			// Token: 0x040008F5 RID: 2293
			public const string ContentLengthHeader = "Content-Length";

			// Token: 0x040008F6 RID: 2294
			public const string ContentLanguageHeader = "Content-Language";

			// Token: 0x040008F7 RID: 2295
			public const string EtagHeader = "ETag";

			// Token: 0x040008F8 RID: 2296
			public const string RangeHeader = "x-ms-range";

			// Token: 0x040008F9 RID: 2297
			public const string RangeContentMD5Header = "x-ms-range-get-content-md5";

			// Token: 0x040008FA RID: 2298
			public const string StorageVersionHeader = "x-ms-version";

			// Token: 0x040008FB RID: 2299
			public const string CopySourceHeader = "x-ms-copy-source";

			// Token: 0x040008FC RID: 2300
			public const string SourceIfMatchHeader = "x-ms-source-if-match";

			// Token: 0x040008FD RID: 2301
			public const string SourceIfModifiedSinceHeader = "x-ms-source-if-modified-since";

			// Token: 0x040008FE RID: 2302
			public const string SourceIfNoneMatchHeader = "x-ms-source-if-none-match";

			// Token: 0x040008FF RID: 2303
			public const string SourceIfUnmodifiedSinceHeader = "x-ms-source-if-unmodified-since";

			// Token: 0x04000900 RID: 2304
			public const string FileType = "x-ms-type";

			// Token: 0x04000901 RID: 2305
			public const string FileCacheControlHeader = "x-ms-cache-control";

			// Token: 0x04000902 RID: 2306
			public const string FileContentDispositionRequestHeader = "x-ms-content-disposition";

			// Token: 0x04000903 RID: 2307
			public const string FileContentEncodingHeader = "x-ms-content-encoding";

			// Token: 0x04000904 RID: 2308
			public const string FileContentLanguageHeader = "x-ms-content-language";

			// Token: 0x04000905 RID: 2309
			public const string FileContentMD5Header = "x-ms-content-md5";

			// Token: 0x04000906 RID: 2310
			public const string FileContentTypeHeader = "x-ms-content-type";

			// Token: 0x04000907 RID: 2311
			public const string FileContentLengthHeader = "x-ms-content-length";

			// Token: 0x04000908 RID: 2312
			public const string FileRangeWrite = "x-ms-write";

			// Token: 0x04000909 RID: 2313
			public const string BlobType = "x-ms-blob-type";

			// Token: 0x0400090A RID: 2314
			public const string SnapshotHeader = "x-ms-snapshot";

			// Token: 0x0400090B RID: 2315
			public const string DeleteSnapshotHeader = "x-ms-delete-snapshots";

			// Token: 0x0400090C RID: 2316
			public const string BlobCacheControlHeader = "x-ms-blob-cache-control";

			// Token: 0x0400090D RID: 2317
			public const string BlobContentDispositionRequestHeader = "x-ms-blob-content-disposition";

			// Token: 0x0400090E RID: 2318
			public const string BlobContentEncodingHeader = "x-ms-blob-content-encoding";

			// Token: 0x0400090F RID: 2319
			public const string BlobContentLanguageHeader = "x-ms-blob-content-language";

			// Token: 0x04000910 RID: 2320
			public const string BlobContentMD5Header = "x-ms-blob-content-md5";

			// Token: 0x04000911 RID: 2321
			public const string BlobContentTypeHeader = "x-ms-blob-content-type";

			// Token: 0x04000912 RID: 2322
			public const string BlobContentLengthHeader = "x-ms-blob-content-length";

			// Token: 0x04000913 RID: 2323
			public const string BlobSequenceNumber = "x-ms-blob-sequence-number";

			// Token: 0x04000914 RID: 2324
			public const string SequenceNumberAction = "x-ms-sequence-number-action";

			// Token: 0x04000915 RID: 2325
			public const string BlobCommittedBlockCount = "x-ms-blob-committed-block-count";

			// Token: 0x04000916 RID: 2326
			public const string BlobAppendOffset = "x-ms-blob-append-offset";

			// Token: 0x04000917 RID: 2327
			public const string IfSequenceNumberLEHeader = "x-ms-if-sequence-number-le";

			// Token: 0x04000918 RID: 2328
			public const string IfSequenceNumberLTHeader = "x-ms-if-sequence-number-lt";

			// Token: 0x04000919 RID: 2329
			public const string IfSequenceNumberEqHeader = "x-ms-if-sequence-number-eq";

			// Token: 0x0400091A RID: 2330
			public const string IfMaxSizeLessThanOrEqualHeader = "x-ms-blob-condition-maxsize";

			// Token: 0x0400091B RID: 2331
			public const string IfAppendPositionEqualHeader = "x-ms-blob-condition-appendpos";

			// Token: 0x0400091C RID: 2332
			public const string LeaseIdHeader = "x-ms-lease-id";

			// Token: 0x0400091D RID: 2333
			public const string LeaseStatus = "x-ms-lease-status";

			// Token: 0x0400091E RID: 2334
			public const string LeaseState = "x-ms-lease-state";

			// Token: 0x0400091F RID: 2335
			public const string PageWrite = "x-ms-page-write";

			// Token: 0x04000920 RID: 2336
			public const string ApproximateMessagesCount = "x-ms-approximate-messages-count";

			// Token: 0x04000921 RID: 2337
			public const string Date = "x-ms-date";

			// Token: 0x04000922 RID: 2338
			public const string RequestIdHeader = "x-ms-request-id";

			// Token: 0x04000923 RID: 2339
			public const string ClientRequestIdHeader = "x-ms-client-request-id";

			// Token: 0x04000924 RID: 2340
			public const string BlobPublicAccess = "x-ms-blob-public-access";

			// Token: 0x04000925 RID: 2341
			public const string RangeHeaderFormat = "bytes={0}-{1}";

			// Token: 0x04000926 RID: 2342
			public const string TargetStorageVersion = "2015-02-21";

			// Token: 0x04000927 RID: 2343
			public const string File = "File";

			// Token: 0x04000928 RID: 2344
			public const string PageBlob = "PageBlob";

			// Token: 0x04000929 RID: 2345
			public const string BlockBlob = "BlockBlob";

			// Token: 0x0400092A RID: 2346
			public const string AppendBlob = "AppendBlob";

			// Token: 0x0400092B RID: 2347
			public const string SnapshotsOnlyValue = "only";

			// Token: 0x0400092C RID: 2348
			public const string IncludeSnapshotsValue = "include";

			// Token: 0x0400092D RID: 2349
			public const string PopReceipt = "x-ms-popreceipt";

			// Token: 0x0400092E RID: 2350
			public const string NextVisibleTime = "x-ms-time-next-visible";

			// Token: 0x0400092F RID: 2351
			public const string PeekOnly = "peekonly";

			// Token: 0x04000930 RID: 2352
			public const string ContainerPublicAccessType = "x-ms-blob-public-access";

			// Token: 0x04000931 RID: 2353
			public const string LeaseActionHeader = "x-ms-lease-action";

			// Token: 0x04000932 RID: 2354
			public const string ProposedLeaseIdHeader = "x-ms-proposed-lease-id";

			// Token: 0x04000933 RID: 2355
			public const string LeaseDurationHeader = "x-ms-lease-duration";

			// Token: 0x04000934 RID: 2356
			public const string LeaseBreakPeriodHeader = "x-ms-lease-break-period";

			// Token: 0x04000935 RID: 2357
			public const string LeaseTimeHeader = "x-ms-lease-time";

			// Token: 0x04000936 RID: 2358
			public const string KeyNameHeader = "x-ms-key-name";

			// Token: 0x04000937 RID: 2359
			public const string CopyIdHeader = "x-ms-copy-id";

			// Token: 0x04000938 RID: 2360
			public const string CopyCompletionTimeHeader = "x-ms-copy-completion-time";

			// Token: 0x04000939 RID: 2361
			public const string CopyStatusHeader = "x-ms-copy-status";

			// Token: 0x0400093A RID: 2362
			public const string CopyProgressHeader = "x-ms-copy-progress";

			// Token: 0x0400093B RID: 2363
			public const string CopyDescriptionHeader = "x-ms-copy-status-description";

			// Token: 0x0400093C RID: 2364
			public const string CopyActionHeader = "x-ms-copy-action";

			// Token: 0x0400093D RID: 2365
			public const string CopyActionAbort = "abort";

			// Token: 0x0400093E RID: 2366
			public const string ShareSize = "x-ms-share-size";

			// Token: 0x0400093F RID: 2367
			public const string ShareQuota = "x-ms-share-quota";

			// Token: 0x04000940 RID: 2368
			internal const string PayloadAcceptHeader = "Accept";

			// Token: 0x04000941 RID: 2369
			internal const string PayloadContentTypeHeader = "Content-Type";

			// Token: 0x04000942 RID: 2370
			public static readonly string UserAgent;

			// Token: 0x04000943 RID: 2371
			public static readonly string UserAgentComment = string.Format(CultureInfo.InvariantCulture, "(.NET CLR {0}; {1} {2})", new object[]
			{
				Environment.Version,
				Environment.OSVersion.Platform,
				Environment.OSVersion.Version
			});
		}

		// Token: 0x02000156 RID: 342
		public static class QueryConstants
		{
			// Token: 0x04000944 RID: 2372
			public const string Snapshot = "snapshot";

			// Token: 0x04000945 RID: 2373
			public const string SignedStart = "st";

			// Token: 0x04000946 RID: 2374
			public const string SignedExpiry = "se";

			// Token: 0x04000947 RID: 2375
			public const string SignedResource = "sr";

			// Token: 0x04000948 RID: 2376
			public const string SasTableName = "tn";

			// Token: 0x04000949 RID: 2377
			public const string SignedPermissions = "sp";

			// Token: 0x0400094A RID: 2378
			public const string StartPartitionKey = "spk";

			// Token: 0x0400094B RID: 2379
			public const string StartRowKey = "srk";

			// Token: 0x0400094C RID: 2380
			public const string EndPartitionKey = "epk";

			// Token: 0x0400094D RID: 2381
			public const string EndRowKey = "erk";

			// Token: 0x0400094E RID: 2382
			public const string SignedIdentifier = "si";

			// Token: 0x0400094F RID: 2383
			public const string SignedKey = "sk";

			// Token: 0x04000950 RID: 2384
			public const string SignedVersion = "sv";

			// Token: 0x04000951 RID: 2385
			public const string Signature = "sig";

			// Token: 0x04000952 RID: 2386
			public const string CacheControl = "rscc";

			// Token: 0x04000953 RID: 2387
			public const string ContentType = "rsct";

			// Token: 0x04000954 RID: 2388
			public const string ContentEncoding = "rsce";

			// Token: 0x04000955 RID: 2389
			public const string ContentLanguage = "rscl";

			// Token: 0x04000956 RID: 2390
			public const string ContentDisposition = "rscd";

			// Token: 0x04000957 RID: 2391
			public const string ApiVersion = "api-version";

			// Token: 0x04000958 RID: 2392
			public const string MessageTimeToLive = "messagettl";

			// Token: 0x04000959 RID: 2393
			public const string VisibilityTimeout = "visibilitytimeout";

			// Token: 0x0400095A RID: 2394
			public const string NumOfMessages = "numofmessages";

			// Token: 0x0400095B RID: 2395
			public const string PopReceipt = "popreceipt";

			// Token: 0x0400095C RID: 2396
			public const string ResourceType = "restype";

			// Token: 0x0400095D RID: 2397
			public const string Component = "comp";

			// Token: 0x0400095E RID: 2398
			public const string CopyId = "copyid";
		}

		// Token: 0x02000157 RID: 343
		public static class ContinuationConstants
		{
			// Token: 0x0400095F RID: 2399
			public const string ContinuationTopElement = "ContinuationToken";

			// Token: 0x04000960 RID: 2400
			public const string NextMarkerElement = "NextMarker";

			// Token: 0x04000961 RID: 2401
			public const string NextPartitionKeyElement = "NextPartitionKey";

			// Token: 0x04000962 RID: 2402
			public const string NextRowKeyElement = "NextRowKey";

			// Token: 0x04000963 RID: 2403
			public const string NextTableNameElement = "NextTableName";

			// Token: 0x04000964 RID: 2404
			public const string TargetLocationElement = "TargetLocation";

			// Token: 0x04000965 RID: 2405
			public const string VersionElement = "Version";

			// Token: 0x04000966 RID: 2406
			public const string CurrentVersion = "2.0";

			// Token: 0x04000967 RID: 2407
			public const string TypeElement = "Type";

			// Token: 0x04000968 RID: 2408
			public const string BlobType = "Blob";

			// Token: 0x04000969 RID: 2409
			public const string QueueType = "Queue";

			// Token: 0x0400096A RID: 2410
			public const string TableType = "Table";

			// Token: 0x0400096B RID: 2411
			public const string FileType = "File";
		}

		// Token: 0x02000158 RID: 344
		public static class VersionConstants
		{
			// Token: 0x0400096C RID: 2412
			public const string August2013 = "2013-08-15";

			// Token: 0x0400096D RID: 2413
			public const string February2012 = "2012-02-12";
		}

		// Token: 0x02000159 RID: 345
		public static class AnalyticsConstants
		{
			// Token: 0x0400096E RID: 2414
			public const string LogsContainer = "$logs";

			// Token: 0x0400096F RID: 2415
			public const string MetricsCapacityBlob = "$MetricsCapacityBlob";

			// Token: 0x04000970 RID: 2416
			public const string MetricsHourPrimaryTransactionsBlob = "$MetricsHourPrimaryTransactionsBlob";

			// Token: 0x04000971 RID: 2417
			public const string MetricsHourPrimaryTransactionsTable = "$MetricsHourPrimaryTransactionsTable";

			// Token: 0x04000972 RID: 2418
			public const string MetricsHourPrimaryTransactionsQueue = "$MetricsHourPrimaryTransactionsQueue";

			// Token: 0x04000973 RID: 2419
			public const string MetricsMinutePrimaryTransactionsBlob = "$MetricsMinutePrimaryTransactionsBlob";

			// Token: 0x04000974 RID: 2420
			public const string MetricsMinutePrimaryTransactionsTable = "$MetricsMinutePrimaryTransactionsTable";

			// Token: 0x04000975 RID: 2421
			public const string MetricsMinutePrimaryTransactionsQueue = "$MetricsMinutePrimaryTransactionsQueue";

			// Token: 0x04000976 RID: 2422
			public const string MetricsHourSecondaryTransactionsBlob = "$MetricsHourSecondaryTransactionsBlob";

			// Token: 0x04000977 RID: 2423
			public const string MetricsHourSecondaryTransactionsTable = "$MetricsHourSecondaryTransactionsTable";

			// Token: 0x04000978 RID: 2424
			public const string MetricsHourSecondaryTransactionsQueue = "$MetricsHourSecondaryTransactionsQueue";

			// Token: 0x04000979 RID: 2425
			public const string MetricsMinuteSecondaryTransactionsBlob = "$MetricsMinuteSecondaryTransactionsBlob";

			// Token: 0x0400097A RID: 2426
			public const string MetricsMinuteSecondaryTransactionsTable = "$MetricsMinuteSecondaryTransactionsTable";

			// Token: 0x0400097B RID: 2427
			public const string MetricsMinuteSecondaryTransactionsQueue = "$MetricsMinuteSecondaryTransactionsQueue";

			// Token: 0x0400097C RID: 2428
			public const string LoggingVersionV1 = "1.0";

			// Token: 0x0400097D RID: 2429
			public const string MetricsVersionV1 = "1.0";
		}

		// Token: 0x0200015A RID: 346
		public static class EncryptionConstants
		{
			// Token: 0x0400097E RID: 2430
			internal const string EncryptionProtocolV1 = "1.0";

			// Token: 0x0400097F RID: 2431
			internal const string KeyWrappingIV = "KeyWrappingIV";

			// Token: 0x04000980 RID: 2432
			public const string BlobEncryptionData = "encryptiondata";

			// Token: 0x04000981 RID: 2433
			public const string TableEncryptionKeyDetails = "_ClientEncryptionMetadata1";

			// Token: 0x04000982 RID: 2434
			public const string TableEncryptionPropertyDetails = "_ClientEncryptionMetadata2";
		}
	}
}
