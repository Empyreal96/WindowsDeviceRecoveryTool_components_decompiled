using System;
using System.IO;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Auth;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Table.Protocol
{
	// Token: 0x02000153 RID: 339
	public static class TableRequest
	{
		// Token: 0x06001504 RID: 5380 RVA: 0x0004FFFB File Offset: 0x0004E1FB
		public static void WriteSharedAccessIdentifiers(SharedAccessTablePolicies sharedAccessPolicies, Stream outputStream)
		{
			Request.WriteSharedAccessIdentifiers<SharedAccessTablePolicy>(sharedAccessPolicies, outputStream, delegate(SharedAccessTablePolicy policy, XmlWriter writer)
			{
				writer.WriteElementString("Start", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessStartTime));
				writer.WriteElementString("Expiry", SharedAccessSignatureHelper.GetDateTimeOrEmpty(policy.SharedAccessExpiryTime));
				writer.WriteElementString("Permission", SharedAccessTablePolicy.PermissionsToString(policy.Permissions));
			});
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00050024 File Offset: 0x0004E224
		internal static string ExtractEntityIndexFromExtendedErrorInformation(RequestResult result)
		{
			if (result != null && result.ExtendedErrorInformation != null && !string.IsNullOrEmpty(result.ExtendedErrorInformation.ErrorMessage))
			{
				int num = result.ExtendedErrorInformation.ErrorMessage.IndexOf(":");
				if (num > 0 && num < 3)
				{
					return result.ExtendedErrorInformation.ErrorMessage.Substring(0, num);
				}
			}
			return null;
		}
	}
}
