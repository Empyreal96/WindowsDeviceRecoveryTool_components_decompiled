using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x02000030 RID: 48
	public static class FileHttpWebRequestFactory
	{
		// Token: 0x06000946 RID: 2374 RVA: 0x00021F60 File Offset: 0x00020160
		public static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00021F6D File Offset: 0x0002016D
		public static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00021F7A File Offset: 0x0002017A
		public static void WriteServiceProperties(FileServiceProperties properties, Stream outputStream)
		{
			CommonUtility.AssertNotNull("properties", properties);
			properties.WriteServiceProperties(outputStream);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00021F90 File Offset: 0x00020190
		public static HttpWebRequest Create(Uri uri, int? timeout, FileProperties properties, long fileSize, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, null, useVersionHeader, operationContext);
			if (properties.CacheControl != null)
			{
				httpWebRequest.Headers[HttpRequestHeader.CacheControl] = properties.CacheControl;
			}
			if (properties.ContentType != null)
			{
				httpWebRequest.ContentType = properties.ContentType;
			}
			if (properties.ContentMD5 != null)
			{
				httpWebRequest.Headers[HttpRequestHeader.ContentMd5] = properties.ContentMD5;
			}
			if (properties.ContentLanguage != null)
			{
				httpWebRequest.Headers[HttpRequestHeader.ContentLanguage] = properties.ContentLanguage;
			}
			if (properties.ContentEncoding != null)
			{
				httpWebRequest.Headers[HttpRequestHeader.ContentEncoding] = properties.ContentEncoding;
			}
			if (properties.ContentDisposition != null)
			{
				httpWebRequest.Headers["x-ms-content-disposition"] = properties.ContentDisposition;
			}
			httpWebRequest.Headers["x-ms-type"] = "File";
			httpWebRequest.Headers["x-ms-content-length"] = fileSize.ToString(NumberFormatInfo.InvariantInfo);
			properties.Length = fileSize;
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00022098 File Offset: 0x00020298
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder builder = new UriQueryBuilder();
			HttpWebRequest properties = HttpWebRequestFactory.GetProperties(uri, timeout, builder, useVersionHeader, operationContext);
			properties.ApplyAccessCondition(accessCondition);
			return properties;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000220C0 File Offset: 0x000202C0
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder builder = new UriQueryBuilder();
			HttpWebRequest metadata = HttpWebRequestFactory.GetMetadata(uri, timeout, builder, useVersionHeader, operationContext);
			metadata.ApplyAccessCondition(accessCondition);
			return metadata;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x000220E7 File Offset: 0x000202E7
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000220F0 File Offset: 0x000202F0
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000220FC File Offset: 0x000202FC
		public static HttpWebRequest Delete(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Delete(uri, null, timeout, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00022120 File Offset: 0x00020320
		private static void AddRange(HttpWebRequest request, long? offset, long? count)
		{
			if (count != null)
			{
				CommonUtility.AssertNotNull("offset", offset);
				CommonUtility.AssertInBounds<long>("count", count.Value, 1L, long.MaxValue);
			}
			if (offset != null)
			{
				string text = offset.ToString();
				string text2 = string.Empty;
				if (count != null)
				{
					text2 = (offset + count.Value - 1L).ToString();
				}
				string value = string.Format(CultureInfo.InvariantCulture, "bytes={0}-{1}", new object[]
				{
					text,
					text2
				});
				request.Headers.Add("x-ms-range", value);
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00022228 File Offset: 0x00020428
		public static HttpWebRequest ListRanges(Uri uri, int? timeout, long? offset, long? count, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			if (offset != null)
			{
				CommonUtility.AssertNotNull("count", count);
			}
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "rangelist");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			FileHttpWebRequestFactory.AddRange(httpWebRequest, offset, count);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00022288 File Offset: 0x00020488
		public static HttpWebRequest SetProperties(Uri uri, int? timeout, FileProperties properties, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			if (properties != null)
			{
				httpWebRequest.AddOptionalHeader("x-ms-cache-control", properties.CacheControl);
				httpWebRequest.AddOptionalHeader("x-ms-content-encoding", properties.ContentEncoding);
				httpWebRequest.AddOptionalHeader("x-ms-content-disposition", properties.ContentDisposition);
				httpWebRequest.AddOptionalHeader("x-ms-content-language", properties.ContentLanguage);
				httpWebRequest.AddOptionalHeader("x-ms-content-md5", properties.ContentMD5);
				httpWebRequest.AddOptionalHeader("x-ms-content-type", properties.ContentType);
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002233C File Offset: 0x0002053C
		public static HttpWebRequest Resize(Uri uri, int? timeout, long newFileSize, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-content-length", newFileSize.ToString(NumberFormatInfo.InvariantInfo));
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00022398 File Offset: 0x00020598
		public static HttpWebRequest Get(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder builder = new UriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, builder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x000223C4 File Offset: 0x000205C4
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetMetadata(uri, timeout, null, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000223E8 File Offset: 0x000205E8
		public static HttpWebRequest Get(Uri uri, int? timeout, long? offset, long? count, bool rangeContentMD5, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			if (offset != null && offset.Value < 0L)
			{
				CommonUtility.ArgumentOutOfRange("offset", offset);
			}
			if (offset != null && rangeContentMD5)
			{
				CommonUtility.AssertNotNull("count", count);
				CommonUtility.AssertInBounds<long>("count", count.Value, 1L, 4194304L);
			}
			HttpWebRequest httpWebRequest = FileHttpWebRequestFactory.Get(uri, timeout, accessCondition, useVersionHeader, operationContext);
			FileHttpWebRequestFactory.AddRange(httpWebRequest, offset, count);
			if (offset != null && rangeContentMD5)
			{
				httpWebRequest.Headers.Add("x-ms-range-get-content-md5", "true");
			}
			return httpWebRequest;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00022488 File Offset: 0x00020688
		public static HttpWebRequest PutRange(Uri uri, int? timeout, FileRange fileRange, FileRangeWrite fileRangeWrite, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("fileRange", fileRange);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "range");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.AddOptionalHeader("x-ms-range", fileRange.ToString());
			httpWebRequest.Headers.Add("x-ms-write", fileRangeWrite.ToString());
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00022500 File Offset: 0x00020700
		public static HttpWebRequest CopyFrom(Uri uri, int? timeout, Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("source", source);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, null, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-copy-source", source.AbsoluteUri);
			httpWebRequest.ApplyAccessCondition(destAccessCondition);
			httpWebRequest.ApplyAccessConditionToSource(sourceAccessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00022550 File Offset: 0x00020750
		public static HttpWebRequest AbortCopy(Uri uri, int? timeout, string copyId, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "copy");
			uriQueryBuilder.Add("copyid", copyId);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-copy-action", "abort");
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}
	}
}
