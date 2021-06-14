using System;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x020002A1 RID: 673
	internal static class JsonConstants
	{
		// Token: 0x04000932 RID: 2354
		internal const string ODataResultsName = "results";

		// Token: 0x04000933 RID: 2355
		internal const string ODataDataWrapper = "\"d\":";

		// Token: 0x04000934 RID: 2356
		internal const string ODataDataWrapperPropertyName = "d";

		// Token: 0x04000935 RID: 2357
		internal const string ODataEntryIdName = "id";

		// Token: 0x04000936 RID: 2358
		internal const string ODataMetadataName = "__metadata";

		// Token: 0x04000937 RID: 2359
		internal const string ODataMetadataUriName = "uri";

		// Token: 0x04000938 RID: 2360
		internal const string ODataMetadataTypeName = "type";

		// Token: 0x04000939 RID: 2361
		internal const string ODataMetadataETagName = "etag";

		// Token: 0x0400093A RID: 2362
		internal const string ODataMetadataMediaResourceName = "__mediaresource";

		// Token: 0x0400093B RID: 2363
		internal const string ODataMetadataMediaUriName = "media_src";

		// Token: 0x0400093C RID: 2364
		internal const string ODataMetadataContentTypeName = "content_type";

		// Token: 0x0400093D RID: 2365
		internal const string ODataMetadataMediaETagName = "media_etag";

		// Token: 0x0400093E RID: 2366
		internal const string ODataMetadataEditMediaName = "edit_media";

		// Token: 0x0400093F RID: 2367
		internal const string ODataMetadataPropertiesName = "properties";

		// Token: 0x04000940 RID: 2368
		internal const string ODataMetadataPropertiesAssociationUriName = "associationuri";

		// Token: 0x04000941 RID: 2369
		internal const string ODataCountName = "__count";

		// Token: 0x04000942 RID: 2370
		internal const string ODataNextLinkName = "__next";

		// Token: 0x04000943 RID: 2371
		internal const string ODataDeferredName = "__deferred";

		// Token: 0x04000944 RID: 2372
		internal const string ODataNavigationLinkUriName = "uri";

		// Token: 0x04000945 RID: 2373
		internal const string ODataUriName = "uri";

		// Token: 0x04000946 RID: 2374
		internal const string ODataActionsMetadataName = "actions";

		// Token: 0x04000947 RID: 2375
		internal const string ODataFunctionsMetadataName = "functions";

		// Token: 0x04000948 RID: 2376
		internal const string ODataOperationTitleName = "title";

		// Token: 0x04000949 RID: 2377
		internal const string ODataOperationMetadataName = "metadata";

		// Token: 0x0400094A RID: 2378
		internal const string ODataOperationTargetName = "target";

		// Token: 0x0400094B RID: 2379
		internal const string ODataErrorName = "error";

		// Token: 0x0400094C RID: 2380
		internal const string ODataErrorCodeName = "code";

		// Token: 0x0400094D RID: 2381
		internal const string ODataErrorMessageName = "message";

		// Token: 0x0400094E RID: 2382
		internal const string ODataErrorMessageLanguageName = "lang";

		// Token: 0x0400094F RID: 2383
		internal const string ODataErrorMessageValueName = "value";

		// Token: 0x04000950 RID: 2384
		internal const string ODataErrorInnerErrorName = "innererror";

		// Token: 0x04000951 RID: 2385
		internal const string ODataErrorInnerErrorMessageName = "message";

		// Token: 0x04000952 RID: 2386
		internal const string ODataErrorInnerErrorTypeNameName = "type";

		// Token: 0x04000953 RID: 2387
		internal const string ODataErrorInnerErrorStackTraceName = "stacktrace";

		// Token: 0x04000954 RID: 2388
		internal const string ODataErrorInnerErrorInnerErrorName = "internalexception";

		// Token: 0x04000955 RID: 2389
		internal const string ODataDateTimeFormat = "\\/Date({0})\\/";

		// Token: 0x04000956 RID: 2390
		internal const string ODataDateTimeOffsetFormat = "\\/Date({0}{1}{2:D4})\\/";

		// Token: 0x04000957 RID: 2391
		internal const string ODataDateTimeOffsetPlusSign = "+";

		// Token: 0x04000958 RID: 2392
		internal const string ODataServiceDocumentEntitySetsName = "EntitySets";

		// Token: 0x04000959 RID: 2393
		internal const string JsonTrueLiteral = "true";

		// Token: 0x0400095A RID: 2394
		internal const string JsonFalseLiteral = "false";

		// Token: 0x0400095B RID: 2395
		internal const string JsonNullLiteral = "null";

		// Token: 0x0400095C RID: 2396
		internal const string StartObjectScope = "{";

		// Token: 0x0400095D RID: 2397
		internal const string EndObjectScope = "}";

		// Token: 0x0400095E RID: 2398
		internal const string StartArrayScope = "[";

		// Token: 0x0400095F RID: 2399
		internal const string EndArrayScope = "]";

		// Token: 0x04000960 RID: 2400
		internal const string StartPaddingFunctionScope = "(";

		// Token: 0x04000961 RID: 2401
		internal const string EndPaddingFunctionScope = ")";

		// Token: 0x04000962 RID: 2402
		internal const string ObjectMemberSeparator = ",";

		// Token: 0x04000963 RID: 2403
		internal const string ArrayElementSeparator = ",";

		// Token: 0x04000964 RID: 2404
		internal const string NameValueSeparator = ":";

		// Token: 0x04000965 RID: 2405
		internal const char QuoteCharacter = '"';
	}
}
