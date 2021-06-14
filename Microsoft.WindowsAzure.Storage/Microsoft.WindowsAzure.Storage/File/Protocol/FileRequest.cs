using System;
using System.IO;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000E9 RID: 233
	public static class FileRequest
	{
		// Token: 0x060011F4 RID: 4596 RVA: 0x00042977 File Offset: 0x00040B77
		public static void WriteSharedAccessIdentifiers(SharedAccessFilePolicies sharedAccessPolicies, Stream outputStream)
		{
			Request.WriteSharedAccessIdentifiers<SharedAccessFilePolicy>(sharedAccessPolicies, outputStream, delegate(SharedAccessFilePolicy policy, XmlWriter writer)
			{
				writer.WriteElementString("Start", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessStartTime));
				writer.WriteElementString("Expiry", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessExpiryTime));
				writer.WriteElementString("Permission", SharedAccessFilePolicy.PermissionsToString(policy.Permissions));
			});
		}
	}
}
