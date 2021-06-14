using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x02000021 RID: 33
	public static class ContainerHttpResponseParsers
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x000186EF File Offset: 0x000168EF
		public static string GetRequestId(HttpWebResponse response)
		{
			return Response.GetRequestId(response);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000186F8 File Offset: 0x000168F8
		public static BlobContainerProperties GetProperties(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			return new BlobContainerProperties
			{
				ETag = HttpResponseParsers.GetETag(response),
				LastModified = new DateTimeOffset?(response.LastModified.ToUniversalTime()),
				LeaseStatus = BlobHttpResponseParsers.GetLeaseStatus(response),
				LeaseState = BlobHttpResponseParsers.GetLeaseState(response),
				LeaseDuration = BlobHttpResponseParsers.GetLeaseDuration(response)
			};
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00018765 File Offset: 0x00016965
		public static IDictionary<string, string> GetMetadata(HttpWebResponse response)
		{
			return HttpResponseParsers.GetMetadata(response);
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00018770 File Offset: 0x00016970
		public static BlobContainerPublicAccessType GetAcl(HttpWebResponse response)
		{
			CommonUtility.AssertNotNull("response", response);
			string acl = response.Headers["x-ms-blob-public-access"];
			return ContainerHttpResponseParsers.GetContainerAcl(acl);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001879F File Offset: 0x0001699F
		public static void ReadSharedAccessIdentifiers(Stream inputStream, BlobContainerPermissions permissions)
		{
			CommonUtility.AssertNotNull("permissions", permissions);
			Response.ReadSharedAccessIdentifiers<SharedAccessBlobPolicy>(permissions.SharedAccessPolicies, new BlobAccessPolicyResponse(inputStream));
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x000187C0 File Offset: 0x000169C0
		private static BlobContainerPublicAccessType GetContainerAcl(string acl)
		{
			BlobContainerPublicAccessType result = BlobContainerPublicAccessType.Off;
			if (!string.IsNullOrEmpty(acl))
			{
				string a;
				if ((a = acl.ToLower()) != null)
				{
					if (a == "container")
					{
						return BlobContainerPublicAccessType.Container;
					}
					if (a == "blob")
					{
						return BlobContainerPublicAccessType.Blob;
					}
				}
				string message = string.Format(CultureInfo.CurrentCulture, "Invalid acl public access type returned '{0}'. Expected blob or container.", new object[]
				{
					acl
				});
				throw new InvalidOperationException(message);
			}
			return result;
		}
	}
}
