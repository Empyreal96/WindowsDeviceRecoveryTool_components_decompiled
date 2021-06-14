using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000C6 RID: 198
	internal class BlobAccessPolicyResponse : AccessPolicyResponseBase<SharedAccessBlobPolicy>
	{
		// Token: 0x0600111F RID: 4383 RVA: 0x0003FC69 File Offset: 0x0003DE69
		internal BlobAccessPolicyResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0003FC74 File Offset: 0x0003DE74
		protected override SharedAccessBlobPolicy ParseElement(XElement accessPolicyElement)
		{
			CommonUtility.AssertNotNull("accessPolicyElement", accessPolicyElement);
			SharedAccessBlobPolicy sharedAccessBlobPolicy = new SharedAccessBlobPolicy();
			string text = (string)accessPolicyElement.Element("Start");
			if (!string.IsNullOrEmpty(text))
			{
				sharedAccessBlobPolicy.SharedAccessStartTime = new DateTimeOffset?(Uri.UnescapeDataString(text).ToUTCTime());
			}
			string text2 = (string)accessPolicyElement.Element("Expiry");
			if (!string.IsNullOrEmpty(text2))
			{
				sharedAccessBlobPolicy.SharedAccessExpiryTime = new DateTimeOffset?(Uri.UnescapeDataString(text2).ToUTCTime());
			}
			string text3 = (string)accessPolicyElement.Element("Permission");
			if (!string.IsNullOrEmpty(text3))
			{
				sharedAccessBlobPolicy.Permissions = SharedAccessBlobPolicy.PermissionsFromString(text3);
			}
			return sharedAccessBlobPolicy;
		}
	}
}
