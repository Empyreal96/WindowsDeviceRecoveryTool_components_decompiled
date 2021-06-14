using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000165 RID: 357
	internal static class Response
	{
		// Token: 0x0600152E RID: 5422 RVA: 0x00050500 File Offset: 0x0004E700
		internal static string GetRequestId(HttpWebResponse response)
		{
			return response.Headers["x-ms-request-id"];
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x00050514 File Offset: 0x0004E714
		internal static void ReadSharedAccessIdentifiers<T>(IDictionary<string, T> sharedAccessPolicies, AccessPolicyResponseBase<T> policyResponse) where T : new()
		{
			foreach (KeyValuePair<string, T> keyValuePair in policyResponse.AccessIdentifiers)
			{
				sharedAccessPolicies.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00050570 File Offset: 0x0004E770
		internal static IDictionary<string, string> ParseMetadata(XmlReader reader)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = true;
			while (!flag || reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					flag = false;
					string name = reader.Name;
					string value = reader.ReadElementContentAsString();
					if (name != "x-ms-invalid-name")
					{
						dictionary.Add(name, value);
					}
				}
				else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Metadata")
				{
					reader.Read();
					return dictionary;
				}
			}
			return dictionary;
		}
	}
}
