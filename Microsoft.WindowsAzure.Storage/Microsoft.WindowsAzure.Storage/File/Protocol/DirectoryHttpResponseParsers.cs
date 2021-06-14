using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x0200002D RID: 45
	public static class DirectoryHttpResponseParsers
	{
		// Token: 0x06000934 RID: 2356 RVA: 0x00021BCC File Offset: 0x0001FDCC
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		public static FileDirectoryProperties GetProperties(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return new FileDirectoryProperties
			{
				ETag = HttpResponseParsers.GetETag(response),
				LastModified = new DateTimeOffset?(response.LastModified.ToUniversalTime())
			};
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00021C1D File Offset: 0x0001FE1D
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}
	}
}
