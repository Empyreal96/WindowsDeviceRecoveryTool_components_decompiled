using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x0200004B RID: 75
	public static class TableHttpWebRequestFactory
	{
		// Token: 0x06000CBE RID: 3262 RVA: 0x0002D649 File Offset: 0x0002B849
		public static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return TableHttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002D655 File Offset: 0x0002B855
		internal static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002D662 File Offset: 0x0002B862
		public static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return TableHttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0002D66E File Offset: 0x0002B86E
		internal static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0002D67B File Offset: 0x0002B87B
		public static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return TableHttpWebRequestFactory.GetServiceStats(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002D687 File Offset: 0x0002B887
		internal static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceStats(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0002D694 File Offset: 0x0002B894
		public static void WriteServiceProperties(ServiceProperties properties, Stream outputStream)
		{
			CommonUtility.AssertNotNull("properties", properties);
			properties.WriteServiceProperties(outputStream);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0002D6A8 File Offset: 0x0002B8A8
		public static HttpWebRequest GetAcl(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return TableHttpWebRequestFactory.GetAcl(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0002D6B4 File Offset: 0x0002B8B4
		public static HttpWebRequest GetAcl(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetAcl(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0002D6C1 File Offset: 0x0002B8C1
		public static HttpWebRequest SetAcl(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return TableHttpWebRequestFactory.SetAcl(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002D6CD File Offset: 0x0002B8CD
		public static HttpWebRequest SetAcl(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetAcl(uri, builder, timeout, useVersionHeader, operationContext);
		}
	}
}
