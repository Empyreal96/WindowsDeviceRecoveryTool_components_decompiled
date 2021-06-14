using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000038 RID: 56
	public static class QueueHttpResponseParsers
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x000265C4 File Offset: 0x000247C4
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x000265CC File Offset: 0x000247CC
		public static string GetApproximateMessageCount(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return response.Headers["x-ms-approximate-messages-count"];
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x000265E9 File Offset: 0x000247E9
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x000265F1 File Offset: 0x000247F1
		public static string GetPopReceipt(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return response.Headers["x-ms-popreceipt"];
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0002660E File Offset: 0x0002480E
		public static DateTime GetNextVisibleTime(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return DateTime.Parse(response.Headers["x-ms-time-next-visible"], DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AdjustToUniversal);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00026637 File Offset: 0x00024837
		public static ServiceProperties ReadServiceProperties(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceProperties(inputStream);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002663F File Offset: 0x0002483F
		public static ServiceStats ReadServiceStats(Stream inputStream)
		{
			return HttpResponseParsers.ReadServiceStats(inputStream);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00026647 File Offset: 0x00024847
		public static void ReadSharedAccessIdentifiers(Stream inputStream, QueuePermissions permissions)
		{
			CommonUtility.AssertNotNull("permissions", permissions);
			Response.ReadSharedAccessIdentifiers<SharedAccessQueuePolicy>(permissions.SharedAccessPolicies, new QueueAccessPolicyResponse(inputStream));
		}
	}
}
