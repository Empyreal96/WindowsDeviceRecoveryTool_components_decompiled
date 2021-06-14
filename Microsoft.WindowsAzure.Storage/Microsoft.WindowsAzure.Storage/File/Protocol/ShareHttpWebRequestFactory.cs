using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x02000032 RID: 50
	public static class ShareHttpWebRequestFactory
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x00022698 File Offset: 0x00020898
		public static HttpWebRequest Create(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return ShareHttpWebRequestFactory.Create(uri, null, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x000226A4 File Offset: 0x000208A4
		public static HttpWebRequest Create(Uri uri, FileShareProperties properties, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Create(uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
			if (properties != null && properties.Quota != null)
			{
				httpWebRequest.AddOptionalHeader("x-ms-share-quota", properties.Quota);
			}
			return httpWebRequest;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000226E8 File Offset: 0x000208E8
		public static HttpWebRequest Delete(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Delete(uri, shareUriQueryBuilder, timeout, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00022710 File Offset: 0x00020910
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			HttpWebRequest metadata = HttpWebRequestFactory.GetMetadata(uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
			metadata.ApplyAccessCondition(accessCondition);
			return metadata;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00022738 File Offset: 0x00020938
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			HttpWebRequest properties = HttpWebRequestFactory.GetProperties(uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
			properties.ApplyAccessCondition(accessCondition);
			return properties;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00022760 File Offset: 0x00020960
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetMetadata(uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00022788 File Offset: 0x00020988
		public static HttpWebRequest SetProperties(Uri uri, int? timeout, FileShareProperties properties, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			shareUriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
			if (properties.Quota != null)
			{
				httpWebRequest.AddOptionalHeader("x-ms-share-quota", new int?(properties.Quota.Value));
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00022800 File Offset: 0x00020A00
		public static HttpWebRequest GetStats(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder shareUriQueryBuilder = ShareHttpWebRequestFactory.GetShareUriQueryBuilder();
			shareUriQueryBuilder.Add("comp", "stats");
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, shareUriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00022834 File Offset: 0x00020A34
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002283D File Offset: 0x00020A3D
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00022848 File Offset: 0x00020A48
		public static HttpWebRequest List(Uri uri, int? timeout, ListingContext listingContext, ShareListingDetails detailsIncluded, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "list");
			if (listingContext != null)
			{
				if (listingContext.Prefix != null)
				{
					uriQueryBuilder.Add("prefix", listingContext.Prefix);
				}
				if (listingContext.Marker != null)
				{
					uriQueryBuilder.Add("marker", listingContext.Marker);
				}
				if (listingContext.MaxResults != null)
				{
					uriQueryBuilder.Add("maxresults", listingContext.MaxResults.ToString());
				}
			}
			if ((detailsIncluded & ShareListingDetails.Metadata) != ShareListingDetails.None)
			{
				uriQueryBuilder.Add("include", "metadata");
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x000228F8 File Offset: 0x00020AF8
		public static HttpWebRequest GetAcl(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest acl = HttpWebRequestFactory.GetAcl(uri, ShareHttpWebRequestFactory.GetShareUriQueryBuilder(), timeout, useVersionHeader, operationContext);
			acl.ApplyAccessCondition(accessCondition);
			return acl;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00022920 File Offset: 0x00020B20
		public static HttpWebRequest SetAcl(Uri uri, int? timeout, FileSharePublicAccessType publicAccess, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetAcl(uri, ShareHttpWebRequestFactory.GetShareUriQueryBuilder(), timeout, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00022948 File Offset: 0x00020B48
		internal static UriQueryBuilder GetShareUriQueryBuilder()
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("restype", "share");
			return uriQueryBuilder;
		}
	}
}
