using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x02000020 RID: 32
	public static class BlobHttpWebRequestFactory
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x00017A96 File Offset: 0x00015C96
		public static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00017AA2 File Offset: 0x00015CA2
		internal static HttpWebRequest GetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00017AAF File Offset: 0x00015CAF
		public static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00017ABB File Offset: 0x00015CBB
		internal static HttpWebRequest SetServiceProperties(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.SetServiceProperties(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00017AC8 File Offset: 0x00015CC8
		public static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetServiceStats(uri, builder, timeout, true, operationContext);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00017AD4 File Offset: 0x00015CD4
		internal static HttpWebRequest GetServiceStats(Uri uri, UriQueryBuilder builder, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return HttpWebRequestFactory.GetServiceStats(uri, builder, timeout, useVersionHeader, operationContext);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00017AE1 File Offset: 0x00015CE1
		public static void WriteServiceProperties(ServiceProperties properties, Stream outputStream)
		{
			CommonUtility.AssertNotNull("properties", properties);
			properties.WriteServiceProperties(outputStream);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00017AF5 File Offset: 0x00015CF5
		public static HttpWebRequest Put(Uri uri, int? timeout, BlobProperties properties, BlobType blobType, long pageBlobSize, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Put(uri, timeout, properties, blobType, pageBlobSize, accessCondition, true, operationContext);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00017B08 File Offset: 0x00015D08
		public static HttpWebRequest Put(Uri uri, int? timeout, BlobProperties properties, BlobType blobType, long pageBlobSize, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			if (blobType == BlobType.Unspecified)
			{
				throw new InvalidOperationException("The blob type cannot be undefined.");
			}
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
				httpWebRequest.Headers["x-ms-blob-content-disposition"] = properties.ContentDisposition;
			}
			if (blobType == BlobType.PageBlob)
			{
				httpWebRequest.Headers["x-ms-blob-type"] = "PageBlob";
				httpWebRequest.Headers["x-ms-blob-content-length"] = pageBlobSize.ToString(NumberFormatInfo.InvariantInfo);
				properties.Length = pageBlobSize;
			}
			else if (blobType == BlobType.BlockBlob)
			{
				httpWebRequest.Headers["x-ms-blob-type"] = "BlockBlob";
			}
			else
			{
				httpWebRequest.Headers["x-ms-blob-type"] = "AppendBlob";
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00017C55 File Offset: 0x00015E55
		private static void AddSnapshot(UriQueryBuilder builder, DateTimeOffset? snapshot)
		{
			if (snapshot != null)
			{
				builder.Add("snapshot", Request.ConvertDateTimeToSnapshotString(snapshot.Value));
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00017C77 File Offset: 0x00015E77
		public static HttpWebRequest AppendBlock(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.AppendBlock(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00017C84 File Offset: 0x00015E84
		public static HttpWebRequest AppendBlock(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "appendblock");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			httpWebRequest.ApplyAppendCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00017CC7 File Offset: 0x00015EC7
		public static HttpWebRequest GetPageRanges(Uri uri, int? timeout, DateTimeOffset? snapshot, long? offset, long? count, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetPageRanges(uri, timeout, snapshot, offset, count, accessCondition, true, operationContext);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00017CDC File Offset: 0x00015EDC
		public static HttpWebRequest GetPageRanges(Uri uri, int? timeout, DateTimeOffset? snapshot, long? offset, long? count, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			if (offset != null)
			{
				CommonUtility.AssertNotNull("count", count);
			}
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "pagelist");
			BlobHttpWebRequestFactory.AddSnapshot(uriQueryBuilder, snapshot);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			BlobHttpWebRequestFactory.AddRange(httpWebRequest, offset, count);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00017D44 File Offset: 0x00015F44
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

		// Token: 0x06000660 RID: 1632 RVA: 0x00017E49 File Offset: 0x00016049
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetProperties(uri, timeout, snapshot, accessCondition, true, operationContext);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00017E58 File Offset: 0x00016058
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder builder = new UriQueryBuilder();
			BlobHttpWebRequestFactory.AddSnapshot(builder, snapshot);
			HttpWebRequest properties = HttpWebRequestFactory.GetProperties(uri, timeout, builder, useVersionHeader, operationContext);
			properties.ApplyAccessCondition(accessCondition);
			return properties;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00017E87 File Offset: 0x00016087
		public static HttpWebRequest SetProperties(Uri uri, int? timeout, BlobProperties properties, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.SetProperties(uri, timeout, properties, accessCondition, true, operationContext);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00017E98 File Offset: 0x00016098
		public static HttpWebRequest SetProperties(Uri uri, int? timeout, BlobProperties properties, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			if (properties != null)
			{
				httpWebRequest.AddOptionalHeader("x-ms-blob-cache-control", properties.CacheControl);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-disposition", properties.ContentDisposition);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-encoding", properties.ContentEncoding);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-language", properties.ContentLanguage);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-md5", properties.ContentMD5);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-type", properties.ContentType);
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00017F49 File Offset: 0x00016149
		public static HttpWebRequest Resize(Uri uri, int? timeout, long newBlobSize, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Resize(uri, timeout, newBlobSize, accessCondition, true, operationContext);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00017F58 File Offset: 0x00016158
		public static HttpWebRequest Resize(Uri uri, int? timeout, long newBlobSize, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-blob-content-length", newBlobSize.ToString(NumberFormatInfo.InvariantInfo));
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00017FB1 File Offset: 0x000161B1
		public static HttpWebRequest SetSequenceNumber(Uri uri, int? timeout, SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.SetSequenceNumber(uri, timeout, sequenceNumberAction, sequenceNumber, accessCondition, true, operationContext);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00017FC4 File Offset: 0x000161C4
		public static HttpWebRequest SetSequenceNumber(Uri uri, int? timeout, SequenceNumberAction sequenceNumberAction, long? sequenceNumber, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertInBounds<SequenceNumberAction>("sequenceNumberAction", sequenceNumberAction, SequenceNumberAction.Max, SequenceNumberAction.Increment);
			if (sequenceNumberAction == SequenceNumberAction.Increment)
			{
				if (sequenceNumber != null)
				{
					throw new ArgumentException("The sequence number may not be specified for an increment operation.", "sequenceNumber");
				}
			}
			else
			{
				CommonUtility.AssertNotNull("sequenceNumber", sequenceNumber);
				CommonUtility.AssertInBounds<long>("sequenceNumber", sequenceNumber.Value, 0L);
			}
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "properties");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ContentLength = 0L;
			httpWebRequest.Headers.Add("x-ms-sequence-number-action", sequenceNumberAction.ToString());
			if (sequenceNumberAction != SequenceNumberAction.Increment)
			{
				httpWebRequest.Headers.Add("x-ms-blob-sequence-number", sequenceNumber.Value.ToString(CultureInfo.InvariantCulture));
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001809A File Offset: 0x0001629A
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetMetadata(uri, timeout, snapshot, accessCondition, true, operationContext);
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x000180A8 File Offset: 0x000162A8
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder builder = new UriQueryBuilder();
			BlobHttpWebRequestFactory.AddSnapshot(builder, snapshot);
			HttpWebRequest metadata = HttpWebRequestFactory.GetMetadata(uri, timeout, builder, useVersionHeader, operationContext);
			metadata.ApplyAccessCondition(accessCondition);
			return metadata;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000180D7 File Offset: 0x000162D7
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.SetMetadata(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000180E4 File Offset: 0x000162E4
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetMetadata(uri, timeout, null, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00018105 File Offset: 0x00016305
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001810E File Offset: 0x0001630E
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00018118 File Offset: 0x00016318
		public static HttpWebRequest Delete(Uri uri, int? timeout, DateTimeOffset? snapshot, DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Delete(uri, timeout, snapshot, deleteSnapshotsOption, accessCondition, true, operationContext);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00018128 File Offset: 0x00016328
		public static HttpWebRequest Delete(Uri uri, int? timeout, DateTimeOffset? snapshot, DeleteSnapshotsOption deleteSnapshotsOption, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			if (snapshot != null && deleteSnapshotsOption != DeleteSnapshotsOption.None)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The option '{0}' must be 'None' to delete a specific snapshot specified by '{1}'", new object[]
				{
					"deleteSnapshotsOption",
					"snapshot"
				}));
			}
			UriQueryBuilder builder = new UriQueryBuilder();
			BlobHttpWebRequestFactory.AddSnapshot(builder, snapshot);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Delete(uri, builder, timeout, useVersionHeader, operationContext);
			switch (deleteSnapshotsOption)
			{
			case DeleteSnapshotsOption.IncludeSnapshots:
				httpWebRequest.Headers.Add("x-ms-delete-snapshots", "include");
				break;
			case DeleteSnapshotsOption.DeleteSnapshotsOnly:
				httpWebRequest.Headers.Add("x-ms-delete-snapshots", "only");
				break;
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000181D3 File Offset: 0x000163D3
		public static HttpWebRequest Snapshot(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Snapshot(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000181E0 File Offset: 0x000163E0
		public static HttpWebRequest Snapshot(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "snapshot");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001821C File Offset: 0x0001641C
		public static HttpWebRequest Lease(Uri uri, int? timeout, LeaseAction action, string proposedLeaseId, int? leaseDuration, int? leaseBreakPeriod, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Lease(uri, timeout, action, proposedLeaseId, leaseDuration, leaseBreakPeriod, accessCondition, true, operationContext);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001823C File Offset: 0x0001643C
		public static HttpWebRequest Lease(Uri uri, int? timeout, LeaseAction action, string proposedLeaseId, int? leaseDuration, int? leaseBreakPeriod, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "lease");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			BlobHttpWebRequestFactory.AddLeaseAction(httpWebRequest, action);
			BlobHttpWebRequestFactory.AddLeaseDuration(httpWebRequest, leaseDuration);
			BlobHttpWebRequestFactory.AddProposedLeaseId(httpWebRequest, proposedLeaseId);
			BlobHttpWebRequestFactory.AddLeaseBreakPeriod(httpWebRequest, leaseBreakPeriod);
			return httpWebRequest;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00018298 File Offset: 0x00016498
		internal static void AddProposedLeaseId(HttpWebRequest request, string proposedLeaseId)
		{
			request.AddOptionalHeader("x-ms-proposed-lease-id", proposedLeaseId);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000182A6 File Offset: 0x000164A6
		internal static void AddLeaseDuration(HttpWebRequest request, int? leaseDuration)
		{
			request.AddOptionalHeader("x-ms-lease-duration", leaseDuration);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000182B4 File Offset: 0x000164B4
		internal static void AddLeaseBreakPeriod(HttpWebRequest request, int? leaseBreakPeriod)
		{
			request.AddOptionalHeader("x-ms-lease-break-period", leaseBreakPeriod);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000182C2 File Offset: 0x000164C2
		internal static void AddLeaseAction(HttpWebRequest request, LeaseAction leaseAction)
		{
			request.Headers.Add("x-ms-lease-action", leaseAction.ToString().ToLower());
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000182E4 File Offset: 0x000164E4
		public static HttpWebRequest PutBlock(Uri uri, int? timeout, string blockId, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.PutBlock(uri, timeout, blockId, accessCondition, true, operationContext);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000182F4 File Offset: 0x000164F4
		public static HttpWebRequest PutBlock(Uri uri, int? timeout, string blockId, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "block");
			uriQueryBuilder.Add("blockid", blockId);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyLeaseId(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001833D File Offset: 0x0001653D
		public static HttpWebRequest PutBlockList(Uri uri, int? timeout, BlobProperties properties, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.PutBlockList(uri, timeout, properties, accessCondition, true, operationContext);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001834C File Offset: 0x0001654C
		public static HttpWebRequest PutBlockList(Uri uri, int? timeout, BlobProperties properties, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("properties", properties);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "blocklist");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			if (properties != null)
			{
				httpWebRequest.AddOptionalHeader("x-ms-blob-cache-control", properties.CacheControl);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-type", properties.ContentType);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-md5", properties.ContentMD5);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-language", properties.ContentLanguage);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-encoding", properties.ContentEncoding);
				httpWebRequest.AddOptionalHeader("x-ms-blob-content-disposition", properties.ContentDisposition);
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x000183FD File Offset: 0x000165FD
		public static HttpWebRequest GetBlockList(Uri uri, int? timeout, DateTimeOffset? snapshot, BlockListingFilter typesOfBlocks, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.GetBlockList(uri, timeout, snapshot, typesOfBlocks, accessCondition, true, operationContext);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00018410 File Offset: 0x00016610
		public static HttpWebRequest GetBlockList(Uri uri, int? timeout, DateTimeOffset? snapshot, BlockListingFilter typesOfBlocks, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "blocklist");
			uriQueryBuilder.Add("blocklisttype", typesOfBlocks.ToString());
			BlobHttpWebRequestFactory.AddSnapshot(uriQueryBuilder, snapshot);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001846B File Offset: 0x0001666B
		public static HttpWebRequest PutPage(Uri uri, int? timeout, PageRange pageRange, PageWrite pageWrite, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.PutPage(uri, timeout, pageRange, pageWrite, accessCondition, true, operationContext);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001847C File Offset: 0x0001667C
		public static HttpWebRequest PutPage(Uri uri, int? timeout, PageRange pageRange, PageWrite pageWrite, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("pageRange", pageRange);
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("comp", "page");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-range", pageRange.ToString());
			httpWebRequest.Headers.Add("x-ms-page-write", pageWrite.ToString());
			httpWebRequest.ApplyAccessCondition(accessCondition);
			httpWebRequest.ApplySequenceNumberCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000184FE File Offset: 0x000166FE
		public static HttpWebRequest CopyFrom(Uri uri, int? timeout, Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.CopyFrom(uri, timeout, source, sourceAccessCondition, destAccessCondition, true, operationContext);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00018510 File Offset: 0x00016710
		public static HttpWebRequest CopyFrom(Uri uri, int? timeout, Uri source, AccessCondition sourceAccessCondition, AccessCondition destAccessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			CommonUtility.AssertNotNull("source", source);
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, null, useVersionHeader, operationContext);
			httpWebRequest.Headers.Add("x-ms-copy-source", source.AbsoluteUri);
			httpWebRequest.ApplyAccessCondition(destAccessCondition);
			httpWebRequest.ApplyAccessConditionToSource(sourceAccessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00018560 File Offset: 0x00016760
		public static HttpWebRequest AbortCopy(Uri uri, int? timeout, string copyId, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.AbortCopy(uri, timeout, copyId, accessCondition, true, operationContext);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00018570 File Offset: 0x00016770
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

		// Token: 0x06000684 RID: 1668 RVA: 0x000185CE File Offset: 0x000167CE
		public static HttpWebRequest Get(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Get(uri, timeout, snapshot, accessCondition, true, operationContext);
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000185DC File Offset: 0x000167DC
		public static HttpWebRequest Get(Uri uri, int? timeout, DateTimeOffset? snapshot, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			if (snapshot != null)
			{
				uriQueryBuilder.Add("snapshot", Request.ConvertDateTimeToSnapshotString(snapshot.Value));
			}
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001862C File Offset: 0x0001682C
		public static HttpWebRequest Get(Uri uri, int? timeout, DateTimeOffset? snapshot, long? offset, long? count, bool rangeContentMD5, AccessCondition accessCondition, OperationContext operationContext)
		{
			return BlobHttpWebRequestFactory.Get(uri, timeout, snapshot, offset, count, rangeContentMD5, accessCondition, true, operationContext);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001864C File Offset: 0x0001684C
		public static HttpWebRequest Get(Uri uri, int? timeout, DateTimeOffset? snapshot, long? offset, long? count, bool rangeContentMD5, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
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
			HttpWebRequest httpWebRequest = BlobHttpWebRequestFactory.Get(uri, timeout, snapshot, accessCondition, useVersionHeader, operationContext);
			BlobHttpWebRequestFactory.AddRange(httpWebRequest, offset, count);
			if (offset != null && rangeContentMD5)
			{
				httpWebRequest.Headers.Add("x-ms-range-get-content-md5", "true");
			}
			return httpWebRequest;
		}
	}
}
