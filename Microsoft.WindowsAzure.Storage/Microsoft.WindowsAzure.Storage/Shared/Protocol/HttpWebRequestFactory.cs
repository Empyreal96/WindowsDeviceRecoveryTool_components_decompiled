using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000051 RID: 81
	internal static class HttpWebRequestFactory
	{
		// Token: 0x06000CFC RID: 3324 RVA: 0x000302B0 File Offset: 0x0002E4B0
		internal static HttpWebRequest CreateWebRequest(string method, Uri uri, int? timeout, UriQueryBuilder builder, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			if (timeout != null && timeout.Value > 0)
			{
				builder.Add("timeout", timeout.Value.ToString(CultureInfo.InvariantCulture));
			}
			Uri requestUri = builder.AddToUri(uri);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
			httpWebRequest.Method = method;
			if (method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
			{
				httpWebRequest.ContentLength = 0L;
			}
			httpWebRequest.UserAgent = Constants.HeaderConstants.UserAgent;
			if (useVersionHeader)
			{
				httpWebRequest.Headers["x-ms-version"] = "2015-02-21";
			}
			httpWebRequest.KeepAlive = true;
			httpWebRequest.ServicePoint.Expect100Continue = false;
			return httpWebRequest;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00030364 File Offset: 0x0002E564
		internal static HttpWebRequest Create(Uri uri, int? timeout, UriQueryBuilder builder, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00030384 File Offset: 0x0002E584
		internal static HttpWebRequest GetAcl(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "acl");
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000303C0 File Offset: 0x0002E5C0
		internal static HttpWebRequest SetAcl(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "acl");
			return HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000303FC File Offset: 0x0002E5FC
		internal static HttpWebRequest GetProperties(Uri uri, int? timeout, UriQueryBuilder builder, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.CreateWebRequest("HEAD", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0003041C File Offset: 0x0002E61C
		internal static HttpWebRequest GetMetadata(Uri uri, int? timeout, UriQueryBuilder builder, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "metadata");
			return HttpWebRequestFactory.CreateWebRequest("HEAD", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00030458 File Offset: 0x0002E658
		internal static HttpWebRequest SetMetadata(Uri uri, int? timeout, UriQueryBuilder builder, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "metadata");
			return HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00030494 File Offset: 0x0002E694
		internal static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			if (metadata != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in metadata)
				{
					HttpWebRequestFactory.AddMetadata(request, keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000304EC File Offset: 0x0002E6EC
		internal static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			CommonUtility.AssertNotNull("value", value);
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("The argument must not be empty string.", value);
			}
			request.Headers.Add("x-ms-meta-" + name, value);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00030524 File Offset: 0x0002E724
		internal static HttpWebRequest Delete(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.CreateWebRequest("DELETE", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00030543 File Offset: 0x0002E743
		internal static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "properties");
			builder.Add("restype", "service");
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0003057F File Offset: 0x0002E77F
		internal static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "properties");
			builder.Add("restype", "service");
			return HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000305BB File Offset: 0x0002E7BB
		internal static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			if (builder == null)
			{
				builder = new UriQueryBuilder();
			}
			builder.Add("comp", "stats");
			builder.Add("restype", "service");
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, builder, useVersionHeader, operationContext);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000305F8 File Offset: 0x0002E7F8
		internal static UriQueryBuilder GetServiceUriQueryBuilder()
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("restype", "service");
			return uriQueryBuilder;
		}
	}
}
