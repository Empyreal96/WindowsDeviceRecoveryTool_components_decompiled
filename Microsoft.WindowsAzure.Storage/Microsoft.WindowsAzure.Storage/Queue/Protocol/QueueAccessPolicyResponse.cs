using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x020000FD RID: 253
	internal class QueueAccessPolicyResponse : AccessPolicyResponseBase<SharedAccessQueuePolicy>
	{
		// Token: 0x0600127B RID: 4731 RVA: 0x00044D55 File Offset: 0x00042F55
		internal QueueAccessPolicyResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00044D60 File Offset: 0x00042F60
		protected override SharedAccessQueuePolicy ParseElement(XElement accessPolicyElement)
		{
			CommonUtility.AssertNotNull("accessPolicyElement", accessPolicyElement);
			SharedAccessQueuePolicy sharedAccessQueuePolicy = new SharedAccessQueuePolicy();
			string text = (string)accessPolicyElement.Element("Start");
			if (!string.IsNullOrEmpty(text))
			{
				sharedAccessQueuePolicy.SharedAccessStartTime = new DateTimeOffset?(Uri.UnescapeDataString(text).ToUTCTime());
			}
			string text2 = (string)accessPolicyElement.Element("Expiry");
			if (!string.IsNullOrEmpty(text2))
			{
				sharedAccessQueuePolicy.SharedAccessExpiryTime = new DateTimeOffset?(Uri.UnescapeDataString(text2).ToUTCTime());
			}
			string text3 = (string)accessPolicyElement.Element("Permission");
			if (!string.IsNullOrEmpty(text3))
			{
				sharedAccessQueuePolicy.Permissions = SharedAccessQueuePolicy.PermissionsFromString(text3);
			}
			return sharedAccessQueuePolicy;
		}
	}
}
