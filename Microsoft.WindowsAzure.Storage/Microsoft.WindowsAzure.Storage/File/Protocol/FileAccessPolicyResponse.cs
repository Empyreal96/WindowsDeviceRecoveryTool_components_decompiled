using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000E5 RID: 229
	internal class FileAccessPolicyResponse : AccessPolicyResponseBase<SharedAccessFilePolicy>
	{
		// Token: 0x060011F0 RID: 4592 RVA: 0x000427AB File Offset: 0x000409AB
		internal FileAccessPolicyResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x000427B4 File Offset: 0x000409B4
		protected override SharedAccessFilePolicy ParseElement(XElement accessPolicyElement)
		{
			CommonUtility.AssertNotNull("accessPolicyElement", accessPolicyElement);
			SharedAccessFilePolicy sharedAccessFilePolicy = new SharedAccessFilePolicy();
			string text = (string)accessPolicyElement.Element("Start");
			if (!string.IsNullOrEmpty(text))
			{
				sharedAccessFilePolicy.SharedAccessStartTime = new DateTimeOffset?(Uri.UnescapeDataString(text).ToUTCTime());
			}
			string text2 = (string)accessPolicyElement.Element("Expiry");
			if (!string.IsNullOrEmpty(text2))
			{
				sharedAccessFilePolicy.SharedAccessExpiryTime = new DateTimeOffset?(Uri.UnescapeDataString(text2).ToUTCTime());
			}
			string text3 = (string)accessPolicyElement.Element("Permission");
			if (!string.IsNullOrEmpty(text3))
			{
				sharedAccessFilePolicy.Permissions = SharedAccessFilePolicy.PermissionsFromString(text3);
			}
			return sharedAccessFilePolicy;
		}
	}
}
