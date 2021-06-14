using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x02000150 RID: 336
	internal class TableAccessPolicyResponse : AccessPolicyResponseBase<SharedAccessTablePolicy>
	{
		// Token: 0x06001500 RID: 5376 RVA: 0x0004FD89 File Offset: 0x0004DF89
		internal TableAccessPolicyResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0004FD94 File Offset: 0x0004DF94
		protected override SharedAccessTablePolicy ParseElement(XElement accessPolicyElement)
		{
			CommonUtility.AssertNotNull("accessPolicyElement", accessPolicyElement);
			SharedAccessTablePolicy sharedAccessTablePolicy = new SharedAccessTablePolicy();
			string text = (string)accessPolicyElement.Element("Start");
			if (!string.IsNullOrEmpty(text))
			{
				sharedAccessTablePolicy.SharedAccessStartTime = new DateTimeOffset?(Uri.UnescapeDataString(text).ToUTCTime());
			}
			string text2 = (string)accessPolicyElement.Element("Expiry");
			if (!string.IsNullOrEmpty(text2))
			{
				sharedAccessTablePolicy.SharedAccessExpiryTime = new DateTimeOffset?(Uri.UnescapeDataString(text2).ToUTCTime());
			}
			string text3 = (string)accessPolicyElement.Element("Permission");
			if (!string.IsNullOrEmpty(text3))
			{
				sharedAccessTablePolicy.Permissions = SharedAccessTablePolicy.PermissionsFromString(text3);
			}
			return sharedAccessTablePolicy;
		}
	}
}
