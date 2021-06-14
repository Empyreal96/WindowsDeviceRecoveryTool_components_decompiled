using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x0200002E RID: 46
	public static class DirectoryHttpWebRequestFactory
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x00021C25 File Offset: 0x0001FE25
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00021C2E File Offset: 0x0001FE2E
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00021C38 File Offset: 0x0001FE38
		public static HttpWebRequest Create(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			return HttpWebRequestFactory.Create(uri, timeout, directoryUriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00021C58 File Offset: 0x0001FE58
		public static HttpWebRequest Delete(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Delete(uri, directoryUriQueryBuilder, timeout, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00021C80 File Offset: 0x0001FE80
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			HttpWebRequest properties = HttpWebRequestFactory.GetProperties(uri, timeout, directoryUriQueryBuilder, useVersionHeader, operationContext);
			properties.ApplyAccessCondition(accessCondition);
			return properties;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00021CA8 File Offset: 0x0001FEA8
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			HttpWebRequest metadata = HttpWebRequestFactory.GetMetadata(uri, timeout, directoryUriQueryBuilder, useVersionHeader, operationContext);
			metadata.ApplyAccessCondition(accessCondition);
			return metadata;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00021CD0 File Offset: 0x0001FED0
		public static HttpWebRequest List(Uri uri, int? timeout, FileListingContext listingContext, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			directoryUriQueryBuilder.Add("comp", "list");
			if (listingContext != null)
			{
				if (listingContext.Marker != null)
				{
					directoryUriQueryBuilder.Add("marker", listingContext.Marker);
				}
				if (listingContext.MaxResults != null)
				{
					directoryUriQueryBuilder.Add("maxresults", listingContext.MaxResults.ToString());
				}
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, directoryUriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00021D50 File Offset: 0x0001FF50
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder directoryUriQueryBuilder = DirectoryHttpWebRequestFactory.GetDirectoryUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetMetadata(uri, timeout, directoryUriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00021D78 File Offset: 0x0001FF78
		internal static UriQueryBuilder GetDirectoryUriQueryBuilder()
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("restype", "directory");
			return uriQueryBuilder;
		}
	}
}
