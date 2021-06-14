using System;

namespace Microsoft.Data.OData
{
	// Token: 0x02000299 RID: 665
	public static class ODataConstants
	{
		// Token: 0x040008FE RID: 2302
		public const string MethodGet = "GET";

		// Token: 0x040008FF RID: 2303
		public const string MethodPost = "POST";

		// Token: 0x04000900 RID: 2304
		public const string MethodPut = "PUT";

		// Token: 0x04000901 RID: 2305
		public const string MethodDelete = "DELETE";

		// Token: 0x04000902 RID: 2306
		public const string MethodPatch = "PATCH";

		// Token: 0x04000903 RID: 2307
		public const string MethodMerge = "MERGE";

		// Token: 0x04000904 RID: 2308
		public const string ContentTypeHeader = "Content-Type";

		// Token: 0x04000905 RID: 2309
		public const string DataServiceVersionHeader = "DataServiceVersion";

		// Token: 0x04000906 RID: 2310
		public const string ContentIdHeader = "Content-ID";

		// Token: 0x04000907 RID: 2311
		internal const string ContentLengthHeader = "Content-Length";

		// Token: 0x04000908 RID: 2312
		internal const string HttpQValueParameter = "q";

		// Token: 0x04000909 RID: 2313
		internal const string HttpVersionInBatching = "HTTP/1.1";

		// Token: 0x0400090A RID: 2314
		internal const string Charset = "charset";

		// Token: 0x0400090B RID: 2315
		internal const string HttpMultipartBoundary = "boundary";

		// Token: 0x0400090C RID: 2316
		internal const string ContentTransferEncoding = "Content-Transfer-Encoding";

		// Token: 0x0400090D RID: 2317
		internal const string BatchContentTransferEncoding = "binary";

		// Token: 0x0400090E RID: 2318
		internal const ODataVersion ODataDefaultProtocolVersion = ODataVersion.V3;

		// Token: 0x0400090F RID: 2319
		internal const string BatchRequestBoundaryTemplate = "batch_{0}";

		// Token: 0x04000910 RID: 2320
		internal const string BatchResponseBoundaryTemplate = "batchresponse_{0}";

		// Token: 0x04000911 RID: 2321
		internal const string RequestChangeSetBoundaryTemplate = "changeset_{0}";

		// Token: 0x04000912 RID: 2322
		internal const string ResponseChangeSetBoundaryTemplate = "changesetresponse_{0}";

		// Token: 0x04000913 RID: 2323
		internal const string HttpWeakETagPrefix = "W/\"";

		// Token: 0x04000914 RID: 2324
		internal const string HttpWeakETagSuffix = "\"";

		// Token: 0x04000915 RID: 2325
		internal const int DefaultMaxRecursionDepth = 100;

		// Token: 0x04000916 RID: 2326
		internal const long DefaultMaxReadMessageSize = 1048576L;

		// Token: 0x04000917 RID: 2327
		internal const int DefaultMaxPartsPerBatch = 100;

		// Token: 0x04000918 RID: 2328
		internal const int DefulatMaxOperationsPerChangeset = 1000;

		// Token: 0x04000919 RID: 2329
		internal const int DefaultMaxEntityPropertyMappingsPerType = 100;

		// Token: 0x0400091A RID: 2330
		internal const ODataVersion MaxODataVersion = ODataVersion.V3;

		// Token: 0x0400091B RID: 2331
		internal const string UriSegmentSeparator = "/";

		// Token: 0x0400091C RID: 2332
		internal const char UriSegmentSeparatorChar = '/';

		// Token: 0x0400091D RID: 2333
		internal const string AssociationLinkSegmentName = "$links";

		// Token: 0x0400091E RID: 2334
		internal const string DefaultStreamSegmentName = "$value";
	}
}
