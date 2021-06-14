using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x02000022 RID: 34
	public static class ContainerHttpWebRequestFactory
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x00018829 File Offset: 0x00016A29
		public static HttpWebRequest Create(Uri uri, int? timeout, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.Create(uri, timeout, true, operationContext, BlobContainerPublicAccessType.Off);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00018835 File Offset: 0x00016A35
		public static HttpWebRequest Create(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.Create(uri, timeout, useVersionHeader, operationContext, BlobContainerPublicAccessType.Off);
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00018841 File Offset: 0x00016A41
		public static HttpWebRequest Create(Uri uri, int? timeout, OperationContext operationContext, BlobContainerPublicAccessType accessType)
		{
			return ContainerHttpWebRequestFactory.Create(uri, timeout, true, operationContext, accessType);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00018850 File Offset: 0x00016A50
		public static HttpWebRequest Create(Uri uri, int? timeout, bool useVersionHeader, OperationContext operationContext, BlobContainerPublicAccessType accessType)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Create(uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
			if (accessType != BlobContainerPublicAccessType.Off)
			{
				httpWebRequest.Headers.Add("x-ms-blob-public-access", accessType.ToString().ToLower(CultureInfo.InvariantCulture));
			}
			return httpWebRequest;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00018899 File Offset: 0x00016A99
		public static HttpWebRequest Delete(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.Delete(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000188A8 File Offset: 0x00016AA8
		public static HttpWebRequest Delete(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.Delete(uri, containerUriQueryBuilder, timeout, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x000188CF File Offset: 0x00016ACF
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.GetMetadata(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x000188DC File Offset: 0x00016ADC
		public static HttpWebRequest GetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			HttpWebRequest metadata = HttpWebRequestFactory.GetMetadata(uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
			metadata.ApplyAccessCondition(accessCondition);
			return metadata;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00018903 File Offset: 0x00016B03
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.GetProperties(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00018910 File Offset: 0x00016B10
		public static HttpWebRequest GetProperties(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			HttpWebRequest properties = HttpWebRequestFactory.GetProperties(uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
			properties.ApplyAccessCondition(accessCondition);
			return properties;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00018937 File Offset: 0x00016B37
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.SetMetadata(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00018944 File Offset: 0x00016B44
		public static HttpWebRequest SetMetadata(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetMetadata(uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001896C File Offset: 0x00016B6C
		public static HttpWebRequest Lease(Uri uri, int? timeout, LeaseAction action, string proposedLeaseId, int? leaseDuration, int? leaseBreakPeriod, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.Lease(uri, timeout, action, proposedLeaseId, leaseDuration, leaseBreakPeriod, accessCondition, true, operationContext);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001898C File Offset: 0x00016B8C
		public static HttpWebRequest Lease(Uri uri, int? timeout, LeaseAction action, string proposedLeaseId, int? leaseDuration, int? leaseBreakPeriod, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			containerUriQueryBuilder.Add("comp", "lease");
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.CreateWebRequest("PUT", uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
			BlobHttpWebRequestFactory.AddLeaseAction(httpWebRequest, action);
			BlobHttpWebRequestFactory.AddLeaseDuration(httpWebRequest, leaseDuration);
			BlobHttpWebRequestFactory.AddProposedLeaseId(httpWebRequest, proposedLeaseId);
			BlobHttpWebRequestFactory.AddLeaseBreakPeriod(httpWebRequest, leaseBreakPeriod);
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000189E8 File Offset: 0x00016BE8
		public static void AddMetadata(HttpWebRequest request, IDictionary<string, string> metadata)
		{
			HttpWebRequestFactory.AddMetadata(request, metadata);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000189F1 File Offset: 0x00016BF1
		public static void AddMetadata(HttpWebRequest request, string name, string value)
		{
			HttpWebRequestFactory.AddMetadata(request, name, value);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000189FB File Offset: 0x00016BFB
		public static HttpWebRequest List(Uri uri, int? timeout, ListingContext listingContext, ContainerListingDetails detailsIncluded, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.List(uri, timeout, listingContext, detailsIncluded, true, operationContext);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00018A0C File Offset: 0x00016C0C
		public static HttpWebRequest List(Uri uri, int? timeout, ListingContext listingContext, ContainerListingDetails detailsIncluded, bool useVersionHeader, OperationContext operationContext)
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
			if ((detailsIncluded & ContainerListingDetails.Metadata) != ContainerListingDetails.None)
			{
				uriQueryBuilder.Add("include", "metadata");
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, uriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00018ABB File Offset: 0x00016CBB
		public static HttpWebRequest GetAcl(Uri uri, int? timeout, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.GetAcl(uri, timeout, accessCondition, true, operationContext);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00018AC8 File Offset: 0x00016CC8
		public static HttpWebRequest GetAcl(Uri uri, int? timeout, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest acl = HttpWebRequestFactory.GetAcl(uri, ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder(), timeout, useVersionHeader, operationContext);
			acl.ApplyAccessCondition(accessCondition);
			return acl;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00018AED File Offset: 0x00016CED
		public static HttpWebRequest SetAcl(Uri uri, int? timeout, BlobContainerPublicAccessType publicAccess, AccessCondition accessCondition, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.SetAcl(uri, timeout, publicAccess, accessCondition, true, operationContext);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00018AFC File Offset: 0x00016CFC
		public static HttpWebRequest SetAcl(Uri uri, int? timeout, BlobContainerPublicAccessType publicAccess, AccessCondition accessCondition, bool useVersionHeader, OperationContext operationContext)
		{
			HttpWebRequest httpWebRequest = HttpWebRequestFactory.SetAcl(uri, ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder(), timeout, useVersionHeader, operationContext);
			if (publicAccess != BlobContainerPublicAccessType.Off)
			{
				httpWebRequest.Headers.Add("x-ms-blob-public-access", publicAccess.ToString().ToLower());
			}
			httpWebRequest.ApplyAccessCondition(accessCondition);
			return httpWebRequest;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00018B45 File Offset: 0x00016D45
		public static HttpWebRequest ListBlobs(Uri uri, int? timeout, BlobListingContext listingContext, OperationContext operationContext)
		{
			return ContainerHttpWebRequestFactory.ListBlobs(uri, timeout, listingContext, true, operationContext);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00018B54 File Offset: 0x00016D54
		public static HttpWebRequest ListBlobs(Uri uri, int? timeout, BlobListingContext listingContext, bool useVersionHeader, OperationContext operationContext)
		{
			UriQueryBuilder containerUriQueryBuilder = ContainerHttpWebRequestFactory.GetContainerUriQueryBuilder();
			containerUriQueryBuilder.Add("comp", "list");
			if (listingContext != null)
			{
				if (listingContext.Prefix != null)
				{
					containerUriQueryBuilder.Add("prefix", listingContext.Prefix);
				}
				if (listingContext.Delimiter != null)
				{
					containerUriQueryBuilder.Add("delimiter", listingContext.Delimiter);
				}
				if (listingContext.Marker != null)
				{
					containerUriQueryBuilder.Add("marker", listingContext.Marker);
				}
				if (listingContext.MaxResults != null)
				{
					containerUriQueryBuilder.Add("maxresults", listingContext.MaxResults.ToString());
				}
				if (listingContext.Details != BlobListingDetails.None)
				{
					StringBuilder stringBuilder = new StringBuilder();
					bool flag = false;
					if ((listingContext.Details & BlobListingDetails.Snapshots) == BlobListingDetails.Snapshots)
					{
						if (!flag)
						{
							flag = true;
						}
						else
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append("snapshots");
					}
					if ((listingContext.Details & BlobListingDetails.UncommittedBlobs) == BlobListingDetails.UncommittedBlobs)
					{
						if (!flag)
						{
							flag = true;
						}
						else
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append("uncommittedblobs");
					}
					if ((listingContext.Details & BlobListingDetails.Metadata) == BlobListingDetails.Metadata)
					{
						if (!flag)
						{
							flag = true;
						}
						else
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append("metadata");
					}
					if ((listingContext.Details & BlobListingDetails.Copy) == BlobListingDetails.Copy)
					{
						if (flag)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append("copy");
					}
					containerUriQueryBuilder.Add("include", stringBuilder.ToString());
				}
			}
			return HttpWebRequestFactory.CreateWebRequest("GET", uri, timeout, containerUriQueryBuilder, useVersionHeader, operationContext);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018CD8 File Offset: 0x00016ED8
		internal static UriQueryBuilder GetContainerUriQueryBuilder()
		{
			UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
			uriQueryBuilder.Add("restype", "container");
			return uriQueryBuilder;
		}
	}
}
