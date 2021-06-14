using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x02000104 RID: 260
	public static class QueueRequest
	{
		// Token: 0x0600129B RID: 4763 RVA: 0x00044FFB File Offset: 0x000431FB
		public static void WriteSharedAccessIdentifiers(SharedAccessQueuePolicies sharedAccessPolicies, Stream outputStream)
		{
			Request.WriteSharedAccessIdentifiers<SharedAccessQueuePolicy>(sharedAccessPolicies, outputStream, delegate(SharedAccessQueuePolicy policy, XmlWriter writer)
			{
				writer.WriteElementString("Start", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessStartTime));
				writer.WriteElementString("Expiry", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessExpiryTime));
				writer.WriteElementString("Permission", SharedAccessQueuePolicy.PermissionsToString(policy.Permissions));
			});
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00045024 File Offset: 0x00043224
		public static void WriteMessageContent(string messageContent, Stream outputStream)
		{
			CommonUtility.AssertNotNull("outputStream", outputStream);
			using (XmlWriter xmlWriter = XmlWriter.Create(outputStream, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				NewLineHandling = NewLineHandling.Entitize
			}))
			{
				xmlWriter.WriteStartElement("QueueMessage");
				xmlWriter.WriteElementString("MessageText", messageContent);
				xmlWriter.WriteEndDocument();
			}
		}
	}
}
