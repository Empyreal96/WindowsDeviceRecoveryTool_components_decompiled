using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000039 RID: 57
	public static class QueueHttpWebRequestFactory
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x00026665 File Offset: 0x00024865
		public static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00026671 File Offset: 0x00024871
		internal static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0002667E File Offset: 0x0002487E
		public static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002668A File Offset: 0x0002488A
		internal static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00026697 File Offset: 0x00024897
		public static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.GetServiceStats(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000266A3 File Offset: 0x000248A3
		internal static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceStats(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x000266B0 File Offset: 0x000248B0
		public static void WriteServiceProperties(ServiceProperties properties, Stream outputStream)
		{
			CommonUtility.AssertNotNull("properties", properties);
			properties.WriteServiceProperties(outputStream);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000266C4 File Offset: 0x000248C4
		public static HttpWebRequest Create(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.Create(uri, timeout, true, operationContext);
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x000266CF File Offset: 0x000248CF
		public static HttpWebRequest Create(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.Create(uri, timeout, null, useVersionHeader, operationContext);
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x000266DB File Offset: 0x000248DB
		public static HttpWebRequest Delete(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.Delete(uri, timeout, true, operationContext);
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x000266E8 File Offset: 0x000248E8
		public static HttpWebRequest Delete(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.Delete(uri, null, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00026701 File Offset: 0x00024901
		public static HttpWebRequest ClearMessages(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.ClearMessages(uri, timeout, true, operationContext);
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0002670C File Offset: 0x0002490C
		public static HttpWebRequest ClearMessages(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.Delete(uri, null, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00026725 File Offset: 0x00024925
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.GetMetadata(uri, timeout, true, operationContext);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00026730 File Offset: 0x00024930
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetMetadata(uri, timeout, null, useVersionHeader, operationContext);
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00026749 File Offset: 0x00024949
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.SetMetadata(uri, timeout, true, operationContext);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x00026754 File Offset: 0x00024954
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetMetadata(uri, timeout, null, useVersionHeader, operationContext);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0002676D File Offset: 0x0002496D
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00026776 File Offset: 0x00024976
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00026780 File Offset: 0x00024980
		public static HttpWebRequest List(Uri uri, int? timeout, ListingContext listingContext, QueueListingDetails detailsIncluded, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.List(uri, timeout, listingContext, detailsIncluded, true, operationContext);
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x00026790 File Offset: 0x00024990
		public static HttpWebRequest List(Uri uri, int? timeout, ListingContext listingContext, QueueListingDetails detailsIncluded, bool useVersionHeader, OperationContext operationContext)
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
			if ((detailsIncluded & QueueListingDetails.Metadata) != QueueListingDetails.None)
			{
				uriQueryBuilder.Add("include", "metadata");
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0002683F File Offset: 0x00024A3F
		public static HttpWebRequest GetAcl(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.GetAcl(uri, timeout, true, operationContext);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0002684C File Offset: 0x00024A4C
		public static HttpWebRequest GetAcl(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetAcl(uri, null, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00026865 File Offset: 0x00024A65
		public static HttpWebRequest SetAcl(Uri uri, int? timeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.SetAcl(uri, timeout, true, operationContext);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x00026870 File Offset: 0x00024A70
		public static HttpWebRequest SetAcl(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetAcl(uri, null, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x00026889 File Offset: 0x00024A89
		public static HttpWebRequest AddMessage(Uri uri, int? timeout, int? timeToLiveInSeconds, int? visibilityTimeoutInSeconds, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.AddMessage(uri, timeout, timeToLiveInSeconds, visibilityTimeoutInSeconds, true, operationContext);
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00026898 File Offset: 0x00024A98
		public static HttpWebRequest AddMessage(Uri uri, int? timeout, int? timeToLiveInSeconds, int? visibilityTimeoutInSeconds, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			if (timeToLiveInSeconds != null)
			{
				uriQueryBuilder.Add("messagettl", timeToLiveInSeconds.Value.ToString(CultureInfo.InvariantCulture));
			}
			if (visibilityTimeoutInSeconds != null)
			{
				uriQueryBuilder.Add("visibilitytimeout", visibilityTimeoutInSeconds.Value.ToString(CultureInfo.InvariantCulture));
			}
			return HttpWebRequestFactory.CreateWebRequest("POST", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002690E File Offset: 0x00024B0E
		public static HttpWebRequest UpdateMessage(Uri uri, int? timeout, string popReceipt, int visibilityTimeoutInSeconds, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.UpdateMessage(uri, timeout, popReceipt, visibilityTimeoutInSeconds, true, operationContext);
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0002691C File Offset: 0x00024B1C
		public static HttpWebRequest UpdateMessage(Uri uri, int? timeout, string popReceipt, int visibilityTimeoutInSeconds, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("popreceipt", popReceipt);
			uriQueryBuilder.Add("visibilitytimeout", visibilityTimeoutInSeconds.ToString(CultureInfo.InvariantCulture));
			return HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00026965 File Offset: 0x00024B65
		public static HttpWebRequest DeleteMessage(Uri uri, int? timeout, string popReceipt, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.DeleteMessage(uri, timeout, popReceipt, true, operationContext);
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00026974 File Offset: 0x00024B74
		public static HttpWebRequest DeleteMessage(Uri uri, int? timeout, string popReceipt, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("popreceipt", popReceipt);
			return HttpWebRequestFactory.Delete(uri, uriQueryBuilder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000269A0 File Offset: 0x00024BA0
		public static HttpWebRequest GetMessages(Uri uri, int? timeout, int numberOfMessages, TimeSpan? visibilityTimeout, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.GetMessages(uri, timeout, numberOfMessages, visibilityTimeout, true, operationContext);
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000269B0 File Offset: 0x00024BB0
		public static HttpWebRequest GetMessages(Uri uri, int? timeout, int numberOfMessages, TimeSpan? visibilityTimeout, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("numofmessages", numberOfMessages.ToString(CultureInfo.InvariantCulture));
			if (visibilityTimeout != null)
			{
				uriQueryBuilder.Add("visibilitytimeout", visibilityTimeout.Value.RoundUpToSeconds().ToString(CultureInfo.InvariantCulture));
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00026A1A File Offset: 0x00024C1A
		public static HttpWebRequest PeekMessages(Uri uri, int? timeout, int numberOfMessages, OperationContext operationContext)
		{
			return QueueHttpWebRequestFactory.PeekMessages(uri, timeout, numberOfMessages, true, operationContext);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00026A28 File Offset: 0x00024C28
		public static HttpWebRequest PeekMessages(Uri uri, int? timeout, int numberOfMessages, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("peekonly", "true");
			uriQueryBuilder.Add("numofmessages", numberOfMessages.ToString(CultureInfo.InvariantCulture));
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}
	}
}
