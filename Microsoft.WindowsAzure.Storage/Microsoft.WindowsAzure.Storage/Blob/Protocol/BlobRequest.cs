using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000CB RID: 203
	public static class BlobRequest
	{
		// Token: 0x06001137 RID: 4407 RVA: 0x0003FFFB File Offset: 0x0003E1FB
		public static void WriteSharedAccessIdentifiers(SharedAccessBlobPolicies sharedAccessPolicies, Stream outputStream)
		{
			Request.WriteSharedAccessIdentifiers<SharedAccessBlobPolicy>(sharedAccessPolicies, outputStream, delegate(SharedAccessBlobPolicy policy, XmlWriter writer)
			{
				writer.WriteElementString("Start", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessStartTime));
				writer.WriteElementString("Expiry", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessExpiryTime));
				writer.WriteElementString("Permission", SharedAccessBlobPolicy.PermissionsToString(policy.Permissions));
			});
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00040024 File Offset: 0x0003E224
		public static void WriteBlockListBody(IEnumerable<PutBlockListItem> blocks, Stream outputStream)
		{
			CommonUtility.AssertNotNull("blocks", blocks);
			using (XmlWriter xmlWriter = XmlWriter.Create(outputStream, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8
			}))
			{
				xmlWriter.WriteStartElement("BlockList");
				foreach (PutBlockListItem putBlockListItem in blocks)
				{
					if (putBlockListItem.SearchMode == BlockSearchMode.Committed)
					{
						xmlWriter.WriteElementString("Committed", putBlockListItem.Id);
					}
					else if (putBlockListItem.SearchMode == BlockSearchMode.Uncommitted)
					{
						xmlWriter.WriteElementString("Uncommitted", putBlockListItem.Id);
					}
					else if (putBlockListItem.SearchMode == BlockSearchMode.Latest)
					{
						xmlWriter.WriteElementString("Latest", putBlockListItem.Id);
					}
				}
				xmlWriter.WriteEndDocument();
			}
		}
	}
}
